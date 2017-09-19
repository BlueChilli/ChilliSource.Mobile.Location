#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

/* 
 based on
Source: 	serilog-sinks-raygun (https://github.com/serilog/serilog-sinks-raygun)
Author: 	Serilog (https://github.com/serilog)
License:	Apache License Version 2.0 (https://github.com/serilog/serilog/blob/dev/LICENSE)
*/

// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChilliSource.Mobile.Core;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Builders;
using Mindscape.Raygun4Net.Messages;
using Serilog.Core;
using Serilog.Events;

namespace ChilliSource.Mobile.Logging.Sinks.Raygun
{
    public class UnknownException : Exception
    {
        public UnknownException() : base("There is no exception information") { }
    }

    internal class Info : Exception
    {
        public Info(string message) : base(message) { }
    }

    internal class Warning : Exception
    {
        public Warning(string message) : base(message) { }
    }

    internal class Debug : Exception
    {
        public Debug(string message) : base(message) { }
    }

     /// <summary>
    /// Writes log events to the Raygun.com service.
    /// </summary>
    public class RaygunSink : ILogEventSink
    {
        readonly IFormatProvider _formatProvider;
        readonly string _userNameProperty;
        readonly string _applicationVersionProperty;
        readonly IEnumerable<string> _tags;
        readonly string _tagsProperty;
        
        /// <summary>
        /// Construct a sink that saves errors to the Raygun.io service. Properties are being send as userdata and the level is included as tag. The message is included inside the userdata.
        /// </summary>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="applicationKey">The application key as found on the Raygun website.</param>
        /// <param name="wrapperExceptions">If you have common outer exceptions that wrap a valuable inner exception which you'd prefer to group by, you can specify these by providing a list.</param>
        /// <param name="userNameProperty">Specifies the property name to read the username from. By default it is UserName. Set to null if you do not want to use this feature.</param>
        /// <param name="applicationVersionProperty">Specifies the property to use to retrieve the application version from. You can use an enricher to add the application version to all the log events. When you specify null, Raygun will use the assembly version.</param>
        /// <param name="tags">Specifies the tags to include with every log message. The log level will always be included as a tag.</param>
        /// <param name="tagsProperty">The property where additional tags are stored when emitting log events</param>
        public RaygunSink(IFormatProvider formatProvider, string applicationKey,
            IEnumerable<Type> wrapperExceptions = null,
            string userNameProperty = "UserName",
            string applicationVersionProperty = "ApplicationVersion",
            IEnumerable<string> tags = null,
            string tagsProperty = "Tags")
        {
            if (String.IsNullOrWhiteSpace(applicationKey))
            {
                throw new ArgumentNullException(nameof(applicationKey));
            }

            _formatProvider = formatProvider;
            _userNameProperty = userNameProperty;
            _applicationVersionProperty = applicationVersionProperty;
            _tags = tags ?? new string[0];
            _tagsProperty = tagsProperty;

            RaygunClient.Attach(applicationKey);

            if (wrapperExceptions != null)
            {
                RaygunClient.Current?.AddWrapperExceptions(wrapperExceptions.ToArray());
            }
        }

        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
             var builder = RaygunMessageBuilder.New;

            //Include the log level as a tag.
            var tags = _tags.Concat(new[] { logEvent.Level.ToString() }).ToList();

            var properties = logEvent.Properties
                         .Select(pv => new { Name = pv.Key, Value = RaygunPropertyFormatter.Simplify(pv.Value) })
                         .ToDictionary(a => a.Name, b => b.Value);

            // Add the message 
            var message = logEvent.RenderMessage(_formatProvider);
            properties.Add("RenderedLogMessage", message);
            properties.Add("LogMessageTemplate", logEvent.MessageTemplate.Text);
            properties.Add("LogLevel", logEvent.Level.ToString());
            properties.Add("Timestamp", logEvent.Timestamp.ToString("o"));

            builder.SetTimeStamp(logEvent.Timestamp.UtcDateTime);

            // Add exception when available
            if (logEvent.Exception != null)
            {
                builder.SetExceptionDetails(logEvent.Exception);
            }
            else 
            {
                Exception ex;
                switch (logEvent.Level)
                {
                    case Serilog.Events.LogEventLevel.Debug:
                        ex = new Debug(message);
                        break;
                    case Serilog.Events.LogEventLevel.Information:
                        ex = new Info(message);
                        break;
                    case Serilog.Events.LogEventLevel.Warning:
                        ex = new Warning(message);
                        break;
                    default:
                        ex = new UnknownException();
                        break;    
                }

                builder.SetExceptionDetails(ex);
            }

            // Add user when requested
            if (!String.IsNullOrWhiteSpace(_userNameProperty) &&
                logEvent.Properties.ContainsKey(_userNameProperty) &&
                logEvent.Properties[_userNameProperty] != null)
            {
                 builder.SetUser(new RaygunIdentifierMessage(logEvent.Properties[_userNameProperty].ToString()));
            }

            // Add version when requested
            if (!String.IsNullOrWhiteSpace(_applicationVersionProperty) &&
                logEvent.Properties.ContainsKey(_applicationVersionProperty) &&
                logEvent.Properties[_applicationVersionProperty] != null)
            {
                var version = logEvent.Properties[_applicationVersionProperty].ToString();
                builder.SetVersion(!String.IsNullOrWhiteSpace(version) ? version : logEvent.Properties[nameof(IEnvironmentInformation.AppVersion)].ToString());
            }

            builder.SetEnvironmentDetails();
            builder.SetMachineName(logEvent.Properties[nameof(IEnvironmentInformation.DeviceName)].ToString());
            builder.SetUserCustomData(properties);
            builder.SetClientDetails();

            // Add additional custom tags
            object eventTags;
            if (properties.TryGetValue(_tagsProperty, out eventTags) && eventTags is object[])
            {
                foreach (var tag in (object[])eventTags)
                {
                    tags.Add(tag.ToString());
                }
            }
            
            builder.SetTags(tags);
         
            // Submit
           RaygunClient.Current?.SendInBackground(builder.Build());
        }
    }

}

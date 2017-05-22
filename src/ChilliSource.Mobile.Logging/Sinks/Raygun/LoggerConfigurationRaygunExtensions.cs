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

/* 
Source:     serilog-sinks-raygun (https://github.com/serilog/serilog-sinks-raygun)
Author:     Serilog (https://github.com/serilog)
License:    Apache License Version 2.0 (https://github.com/serilog/serilog/blob/dev/LICENSE)
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
using System.Text;
using ChilliSource.Mobile.Core;
using Mindscape.Raygun4Net;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace ChilliSource.Mobile.Logging.Sinks.Raygun
{


    /// <summary>
    /// Adds the WriteTo.Raygun() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationRaygunExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events (defaults to error and up) to the Raygun.io webservice. Properties are being send as data and the level is used as a tag.
        /// Your message is part of the custom data.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="applicationKey">The application key as found on the Raygun.io website.</param>
        /// <param name="wrapperExceptions">If you have common outer exceptions that wrap a valuable inner exception which you'd prefer to group by, you can specify these by providing a list.</param>
        /// <param name="userNameProperty">Specifies the property name to read the username from. By default it is UserName. Set to null if you do not want to use this feature.</param>
        /// <param name="applicationVersionProperty">Specifies the property to use to retrieve the application version from. You can use an enricher to add the application version to all the log events. When you specify null, Raygun will use the assembly version.</param> 
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink. By default set to Error as Raygun is mostly used for error reporting.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="tags">Specifies the tags to include with every log message. The log level will always be included as a tag.</param>
        /// <param name="tagsProperty">The property where additional tags are stored when emitting log events</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration Raygun(
            this LoggerSinkConfiguration loggerConfiguration,
            string applicationKey,
            IEnumerable<Type> wrapperExceptions = null, 
            string userNameProperty = "UserName",
            string applicationVersionProperty = "ApplicationVersion",
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Error,
            IFormatProvider formatProvider = null,
            IEnumerable<string> tags = null,
            string tagsProperty = "Tags")
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (String.IsNullOrWhiteSpace(applicationKey))
            {
                throw new ArgumentNullException(nameof(applicationKey));
            }
           
            return loggerConfiguration.Sink(
                new RaygunSink(formatProvider, applicationKey, wrapperExceptions, userNameProperty, applicationVersionProperty, tags, tagsProperty),
                restrictedToMinimumLevel);
        }
    }

}

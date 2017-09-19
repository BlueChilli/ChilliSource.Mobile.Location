#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ChilliSource.Mobile.Core;
using Serilog.Core;
using Serilog.Events;

namespace ChilliSource.Mobile.Logging
{
    /// <summary>
    /// Adds app-specific information to a <see cref="LogEvent"/> instance
    /// </summary>
    public class ApplicationInformationEnricher : ILogEventEnricher
    {
        private readonly IEnvironmentInformation _information;
        private readonly Func<string> _userKeyRetriever;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ChilliSource.Mobile.Logging.ApplicationInformationEnricher"/> class.
		/// </summary>
		/// <param name="information"><see cref="IEnvironmentInformation"/> implementation holding app information to be logged.</param>
		/// <param name="userKeyRetriever">Function to retrieve the user key for API authentication.</param>
		public ApplicationInformationEnricher(IEnvironmentInformation information, Func<string> userKeyRetriever = null)
        {
            _information = information;
            _userKeyRetriever = userKeyRetriever;
        }

        /// <summary>
        /// Adds application specific information based on <see cref="IEnvironmentInformation"/>
        /// to the specified <paramref name="logEvent"/>
        /// </summary>
        /// <param name="logEvent">Log event to enrich.</param>
        /// <param name="propertyFactory">Property factory to create new LogEvent properties with the information to be added.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.ApplicationName), _information.ApplicationName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.AppId), _information.AppId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.AppVersion), _information.AppVersion));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.ExecutionEnvironment), _information.ExecutionEnvironment));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.Platform), _information.Platform));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.Timezone), _information.Timezone));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(nameof(_information.DeviceName), _information.DeviceName));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserKey", _userKeyRetriever?.Invoke()));
        }
    }
}


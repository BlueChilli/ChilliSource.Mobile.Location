#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	/// <summary>
	/// Contains the deserialized response data returned from the Directions service
	/// </summary>
	public class DirectionsResponse
	{
        /// <summary>
        /// Set of possible routes from the origin to the destination location. 
        /// See: https://developers.google.com/maps/documentation/directions/intro#Routes
        /// </summary>
		public List<Route> Routes { get; set; }

        /// <summary>
        /// Status of request. See: https://developers.google.com/maps/documentation/directions/intro#StatusCodes
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("status")]
		public GoogleApiResponseStatus Status { get; set; }

        /// <summary>
        /// Stores the complete response data
        /// </summary>
        [JsonProperty("json")]
        public string JsonData { get; set; }

		#region Helper Methods

        /// <summary>
        /// Returns the distance of the first leg of the first route
        /// </summary>
        /// <returns></returns>
		public double GetJourneyDistanceInMeters()
		{
			if (Status == GoogleApiResponseStatus.Ok)
			{
				return Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.Distance.Value ?? 0;
			}

			return 0.0;
		}

        /// <summary>
        /// Returns the duration in seconds of the first leg of the first route
        /// </summary>
        /// <returns></returns>
		public TimeSpan GetJourneyDurationInSeconds()
		{
			if (Status == GoogleApiResponseStatus.Ok)
			{
				return TimeSpan.FromSeconds(Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.Duration.Value ?? 0);
			}

			return TimeSpan.MinValue;
		}

        /// <summary>
        /// Returns the duration in seconds of the first leg of the first 
        /// route when travelling in traffic, based on current and historical traffic conditions.
        /// </summary>
        /// <returns></returns>
		public TimeSpan GetJourneyDurationInSecondsInTraffic()
		{
			if (Status == GoogleApiResponseStatus.Ok)
			{
				return TimeSpan.FromSeconds(Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.DurationInTraffic.Value ?? 0);
			}

			return TimeSpan.MinValue;
		}

        /// <summary>
        /// Returns the starting address of the first leg of the first route
        /// </summary>
        /// <returns></returns>
		public string GetJourneyStartAddress()
		{
			if (Status == GoogleApiResponseStatus.Ok)
			{
				return Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.StartAddress;
			}

			return "";
		}

		#endregion
	}
}

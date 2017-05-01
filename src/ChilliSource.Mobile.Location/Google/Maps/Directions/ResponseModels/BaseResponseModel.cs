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
	/// Contains the deserialized response data returned from the service
	/// </summary>
	public class BaseResponseModel
	{
		public List<Route> Routes { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("status")]
		public ResponseStatus ResponseStatus { get; set; }

		public string Json { get; set; }

		#region Helper Methods

		public double GetJourneyDistance()
		{
			if (ResponseStatus == ResponseStatus.Ok)
			{
				return Routes.FirstOrDefault().Legs.FirstOrDefault().Distance.Value;
			}

			return 0.0;
		}

		public TimeSpan GetJourneyDuration()
		{
			if (ResponseStatus == ResponseStatus.Ok)
			{
				return TimeSpan.FromSeconds(Routes.FirstOrDefault().Legs.FirstOrDefault().Duration.Value);
			}

			return TimeSpan.MinValue;
		}

		public TimeSpan GetJourneyDurationWithTraffic()
		{
			if (ResponseStatus == ResponseStatus.Ok)
			{
				return TimeSpan.FromSeconds(Routes.FirstOrDefault().Legs.FirstOrDefault().DurationInTraffic.Value);
			}

			return TimeSpan.MinValue;
		}

		public string GetJourneyStartAddress()
		{
			if (ResponseStatus == ResponseStatus.Ok)
			{
				return Routes.FirstOrDefault().Legs.FirstOrDefault().StartAddress;
			}

			return "";
		}

		#endregion
	}
}

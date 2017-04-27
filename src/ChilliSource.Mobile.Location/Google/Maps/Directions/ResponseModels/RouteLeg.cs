#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	public class RouteLeg
	{
		public TextValue Distance { get; set; }

		public TextValue Duration { get; set; }

		[JsonProperty("duration_in_traffic")]
		public TextValue DurationInTraffic { get; set; }

		[JsonProperty("end_address")]
		public string EndAddress { get; set; }

		[JsonProperty("end_location")]
		public Coordinate EndAddressLocation { get; set; }

		[JsonProperty("start_address")]
		public string StartAddress { get; set; }

		[JsonProperty("start_location")]
		public Coordinate StartAddressLocation { get; set; }

		public List<Step> Steps { get; set; }
	}
}

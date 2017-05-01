#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Location.Google.Places
{
	public class DetailsResult
	{
		[JsonProperty("html_attributions")]
		public object[] HtmlAttributions { get; set; }

		public ResultModel Result { get; set; }

		public string Status { get; set; }

		public class ResultModel
		{
			[JsonProperty("address_components")]
			public IEnumerable<AddressComponent> AddressComponents { get; set; }

			[JsonProperty("adr_address")]
			public string AdrAddress { get; set; }

			[JsonProperty("formatted_address")]
			public string FormattedAddress { get; set; }

			public string PredictedAddress { get; set; }

			public Geometry Geometry { get; set; }

			public string Icon { get; set; }

			public string Id { get; set; }

			public string Name { get; set; }

			[JsonProperty("place_id")]
			public string PlaceId { get; set; }

			public string Reference { get; set; }

			public string Scope { get; set; }

			public IEnumerable<string> Types { get; set; }

			public string Url { get; set; }

			public string Vicinity { get; set; }

		}

		public class Geometry
		{
			public Location Location { get; set; }
		}

		public class Location
		{
			public float Lat { get; set; }

			public float Lng { get; set; }
		}

		public class AddressComponent
		{
			[JsonProperty("long_name")]
			public string LongName { get; set; }

			[JsonProperty("short_name")]
			public string ShortName { get; set; }

			public IEnumerable<string> Types { get; set; }
		}
	}
}

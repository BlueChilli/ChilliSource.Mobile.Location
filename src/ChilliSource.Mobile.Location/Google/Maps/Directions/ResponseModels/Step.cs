#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Location.Google.Maps.Directions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ChilliSource.Mobile.Location
{
	public class Step
	{
		string _instructionsProperty;

		public TextValue Distance { get; set; }

		public TextValue Duration { get; set; }

		[JsonProperty("end_location")]
		public Coordinate EndAddressLocation { get; set; }

		[JsonProperty("start_location")]
		public Coordinate StartAddressLocation { get; set; }

		[JsonProperty("travel_mode")]
		public string TravelMode { get; set; }

		[JsonProperty("html_instructions")]
		public string Instructions
		{
			get
			{
				return _instructionsProperty;
			}
			set
			{
				//Decode HTML String
				_instructionsProperty = Regex.Replace(value, "<[^>]+>", String.Empty);
			}
		}
	}
}

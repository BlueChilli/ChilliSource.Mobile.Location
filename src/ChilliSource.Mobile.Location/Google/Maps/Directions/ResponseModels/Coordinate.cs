#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	public class Coordinate
	{
		[JsonProperty("lat")]
		public double Latitude { get; set; }

		[JsonProperty("lng")]
		public double Longitude { get; set; }
	}
}

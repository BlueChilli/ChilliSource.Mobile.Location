#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Text;
using System.Collections.Generic;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	/// <summary>
	/// Request model to pass to the directions manager
	/// </summary>
	public class Request
	{
		public Tuple<string, string> OriginCoordinates { get; set; }

		public Tuple<string, string> DestinationCoordinates { get; set; }

		public string DestinationPlaceId { get; set; }

		public string DestinationAddress { get; set; }

		public TravelMode Mode { get; set; }

		public bool AvoidTolls { get; set; }

		public UnitType UnitType { get; set; }

		public string Region { get; set; } = "AU";

		public TrafficModel TrafficModel { get; set; }

		public List<Waypoint> Waypoints { get; set; }

		public string GetRequestURL(string apiKey)
		{
			return RequestURLBuilder.BuildDirectionsURL(this, apiKey);
		}
	}
}

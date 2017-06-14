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
	/// Contains parameters to control the direction results
	/// </summary>
	public class DirectionsRequest
	{
        /// <summary>
        /// Origin address
        /// </summary>
        public string OriginAddress { get; set; }

        /// <summary>
        /// Origin place id to be used if the <see cref="OriginAddress"/> was not provided
        /// </summary>
		public string OriginPlaceId { get; set; }

        /// <summary>
        /// Start location expressed a textual latitude/longitude value pair,
        /// to be used if <see cref="OriginPlaceId"/> or <see cref="OriginAddress"/> was not provided
        /// </summary>
		public Tuple<string, string> OriginCoordinates { get; set; }
        
        /// <summary>
        /// Destination address
        /// </summary>
		public string DestinationAddress { get; set; }

        /// <summary>
        /// Destination place id to be used if the <see cref="DestinationAddress"/> was not provided
        /// </summary>
		public string DestinationPlaceId { get; set; }

        /// <summary>
        /// Destination location expressed a textual latitude/longitude value pair, 
        /// to be used if <see cref="DestinationPlaceId"/> or <see cref="DestinationAddress"/> was not provided
        /// </summary>
		public Tuple<string, string> DestinationCoordinates { get; set; }

        /// <summary>
        /// Specifies the mode of transport to use when calculating directions. 
        /// See: https://developers.google.com/maps/documentation/directions/intro#TravelModes
        /// </summary>
        public TravelMode Mode { get; set; }

        /// <summary>
        /// Indicates that the calculated route(s) should avoid tolls
        /// </summary>
		public bool AvoidTolls { get; set; }

        /// <summary>
        /// Specifies the unit system to use when displaying results
        /// </summary>
		public UnitType UnitType { get; set; }

        /// <summary>
        /// Specifies the region code, specified as a ccTLD ("top-level domain") two-character value
        /// </summary>
		public string Region { get; set; } = "AU";

        /// <summary>
        /// Specifies the assumptions to use when calculating time in traffic
        /// </summary>
		public TrafficModel TrafficModel { get; set; }

        /// <summary>
        /// Waypoints alter a route by routing it through the specified location(s). A waypoint is specified as a latitude/longitude coordinate, an encoded polyline, a place ID, or an address which will be geocoded
        /// </summary>
		public List<Waypoint> Waypoints { get; set; }		
	}
}

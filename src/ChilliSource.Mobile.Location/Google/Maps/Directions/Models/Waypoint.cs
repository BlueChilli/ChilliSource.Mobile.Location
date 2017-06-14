#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
    /// <summary>
    ///  Waypoints can be used to calculate routes through additional locations, in which case the 
    ///  returned route includes stopovers at each of the given waypoints.
    ///  See: https://developers.google.com/maps/documentation/directions/intro#Waypoints
    /// </summary>
	public class Waypoint
	{
		public string Latitude { get; set; }

		public string Longitude { get; set; }

		public bool IsStopover { get; set; }
	}
}

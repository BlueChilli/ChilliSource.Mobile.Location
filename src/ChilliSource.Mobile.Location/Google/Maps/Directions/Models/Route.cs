#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Collections.Generic;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
    /// <summary>
    /// Represents one of potentially multiple paths of navigation from the origin to the destination location
    /// </summary>
	public class Route
	{
        /// <summary>
        /// Set of legs specifying the different segments of a route. For routes that contain no waypoints, 
        /// the route will consist of a single "leg," but for routes that define one or more waypoints, 
        /// the route will consist of one or more legs, corresponding to the specific legs of the journey.
        /// </summary>
		public List<RouteLeg> Legs { get; set; }
	}
}

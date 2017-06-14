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
    /// <summary>
    /// Represents a segment of the route if the route is split by waypoints, 
    /// or the complete route if the route does not have any waypoints
    /// </summary>
	public class RouteLeg
	{
        /// <summary>
        /// Distance covered by the leg as a value in meteres and a human-readable text representation
        /// </summary>
		public TextValue Distance { get; set; }

        /// <summary>
        /// Time needed to travel the leg as a value in seconds and a human-readable text representation
        /// </summary>
		public TextValue Duration { get; set; }

        /// <summary>
        /// Time needed to travel the leg in traffic conditions as a value in seconds and a human-readable text representation
        /// </summary>
		[JsonProperty("duration_in_traffic")]
		public TextValue DurationInTraffic { get; set; }

        /// <summary>
        /// Destination address
        /// </summary>
		[JsonProperty("end_address")]
		public string EndAddress { get; set; }

        /// <summary>
        /// Coordinates of destination address
        /// </summary>
		[JsonProperty("end_location")]
		public GoogleCoordinate EndAddressLocation { get; set; }

        /// <summary>
        /// Origin address
        /// </summary>
		[JsonProperty("start_address")]
		public string StartAddress { get; set; }

        /// <summary>
        /// Coordinates of origin address
        /// </summary>
		[JsonProperty("start_location")]
		public GoogleCoordinate StartAddressLocation { get; set; }

        /// <summary>
        /// Set of granular direction instructions for the navigation of the route leg
        /// </summary>
		public List<Step> Steps { get; set; }
	}
}

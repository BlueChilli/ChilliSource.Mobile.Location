#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
    /// <summary>
    /// Contains the deserialized response data returned from the Roads service
    /// </summary>
	public class RoadsResponse
	{
        /// <summary>
        /// Set of points that have been snapped to the roads found by Google Maps
        /// </summary>
		public List<SnappedPoint> SnappedPoints { get; set; }

        /// <summary>
        /// Status of request
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public GoogleApiResponseStatus Status { get; set; }
    }
}

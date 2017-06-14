#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
    /// <summary>
    /// Represents a point that was snapped to a road. See: https://developers.google.com/maps/documentation/roads/snap
    /// </summary>
	public class SnappedPoint
	{
        /// <summary>
        /// Latitude and longitude of point
        /// </summary>
		public GoogleCoordinate Location { get; set; }

        /// <summary>
        /// An integer that indicates the corresponding value in the original request. 
        /// Each value in the request should map to a snapped value in the response. 
        /// However, if you've set interpolate=true, then it's possible that the response
        /// will contain more coordinates than the request. Interpolated values will not 
        /// have an originalIndex
        /// </summary>
		public int OriginalIndex { get; set; }

        /// <summary>
        /// A unique identifier for a place.All place IDs returned by the Google Maps Roads API 
        /// correspond to road segments.Place IDs can be used with other Google APIs, including 
        /// the Google Places API and the Google Maps JavaScript API.For example, if you need to 
        /// get road names for the snapped points returned by the Google Maps Roads API, you can 
        /// pass the placeId to the Google Places API or the Google Maps Geocoding API 
        /// </summary>
        public string PlaceId { get; set; }
	}
}

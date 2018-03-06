#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChilliSource.Mobile.Location.Google.Places
{	
    /// <summary>
    /// Contains the deserialized response data from the place autocomplete service
    /// </summary>
	public class PlaceResponse
	{
        /// <summary>
        /// All the predictions that match the search text. Each prediction stores information about one place.
        /// </summary>
		public IEnumerable<Prediction> Predictions { get; set; }

        /// <summary>
        /// Status of the request. See: https://developers.google.com/places/web-service/autocomplete#place_autocomplete_status_codes
        /// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public GoogleApiResponseStatus Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }			
}

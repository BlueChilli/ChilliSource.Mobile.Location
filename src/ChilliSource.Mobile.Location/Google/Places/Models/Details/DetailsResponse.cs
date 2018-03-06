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
    /// Contains the place details service call result
    /// </summary>
	public class DetailsResponse
	{
        /// <summary>
        /// Contains a set of attributions about this place which must be displayed to the user
        /// </summary>
		[JsonProperty("html_attributions")]
		public object[] HtmlAttributions { get; set; }

        /// <summary>
        /// Contains the detailed information about the place requested. 
        /// See: https://developers.google.com/places/web-service/details#PlaceDetailsResults
        /// </summary>
		public PlaceDetail Result { get; set; }

        /// <summary>
        /// Status of the request. See: https://developers.google.com/places/web-service/details#PlaceDetailsStatusCodes
        /// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public GoogleApiResponseStatus Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

    }
}

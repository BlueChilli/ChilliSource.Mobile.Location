using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChilliSource.Mobile.Location.Google.Places
{
    public class ReverseGeocodingResponse
    {
        /// <summary>
        /// The addresses nearby the specified coordinates
        /// </summary>
        [JsonProperty("results")]
        public IEnumerable<Address> Addresses { get; set; }


        /// <summary>
        /// Status of the request. See: https://developers.google.com/maps/documentation/geocoding/intro#GeocodingResponses
        /// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("status")]
        public GoogleApiResponseStatus Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}

using System;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Location.Google.Places
{
    public class Address
    {
        /// <summary>
        /// The full address as a text string
        /// </summary>
        /// <value>The formatted address.</value>
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Location.Google.Places
{
    public class ReverseGeocodingResponse
    {
        /// <summary>
        /// The addresses nearby the specified coordinates
        /// </summary>
        [JsonProperty("results")]
        public IEnumerable<Address> Addresses { get; set; }
    }
}

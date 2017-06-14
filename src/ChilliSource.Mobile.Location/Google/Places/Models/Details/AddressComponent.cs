using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// A component of a given address, e.g. street number, street, city
    /// </summary>
    public class AddressComponent
    {
        /// <summary>
        /// Full text description of this component
        /// </summary>
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        /// <summary>
        /// Abberviated textual name of the component, e.g. "AK" for Alaska
        /// </summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Set of types representing this component, e.g. "street_number"
        /// </summary>
        public IEnumerable<string> Types { get; set; }
    }
}

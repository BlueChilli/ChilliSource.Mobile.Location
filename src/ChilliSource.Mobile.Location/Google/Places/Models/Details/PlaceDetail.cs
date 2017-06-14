using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Contains the detailed information about the place requested. 
    /// See: https://developers.google.com/places/web-service/details#PlaceDetailsResults
    /// </summary>
    public class PlaceDetail
    {
        /// <summary>
        /// Original prediction info of an autocomplete result for this place.
        /// Will only have a value if this result was generated from an autocomplete prediction
        /// </summary>
        public string Prediction { get; set; }

        /// <summary>
        /// Unique stable identifier denoting this place. 
        /// This identifier may not be used to retrieve information about this place, 
        /// but can be used to consolidate data about this place, 
        /// and to verify the identity of a place across separate searches
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Human-readable name for the returned result
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Textual identifier that uniquely identifies a place
        /// </summary>
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// Contains a token that can be used to query the Details service in future
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Scope of the <see cref="PlaceId"/>. 
        /// Either "GOOGLE" or "APP", determining whether the PlaceId is only recognized by your app or available to other applications on Google Maps.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Set of address components used to compose a given address, e.g. street number, street, city
        /// </summary>
        [JsonProperty("address_components")]
        public IEnumerable<AddressComponent> AddressComponents { get; set; }

        /// <summary>
        /// Adr microformat representation of place's address
        /// </summary>
        [JsonProperty("adr_address")]
        public string AdrAddress { get; set; }

        /// <summary>
        /// Human-readable string representation of this place
        /// </summary>
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Location info about this place
        /// </summary>
        public Geometry Geometry { get; set; }

        /// <summary>
        /// URL of a suggested icon which may be displayed to the user when indicating this result on a map
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Set of feature types describing this result. See: https://developers.google.com/places/web-service/supported_types#table2
        /// </summary>
        public IEnumerable<string> Types { get; set; }

        /// <summary>
        /// URL of the official Google page for this place
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A simplified address for the place, including the street name, street number, and locality, but not the province/state, postal code, or country
        /// </summary>
        public string Vicinity { get; set; }

    }
}

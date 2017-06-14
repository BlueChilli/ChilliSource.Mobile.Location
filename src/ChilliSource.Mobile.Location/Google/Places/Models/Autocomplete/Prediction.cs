using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Stores information about a place that has been returned as part of the autocomplete results. 
    /// For more info see here: https://developers.google.com/places/web-service/autocomplete#place_autocomplete_results
    /// </summary>
    public class Prediction
    {
        /// <summary>
        /// Human readable name of the place
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique id that can be used to consolidate data about the place and verify the identity of the place across separate searches
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Offset and length information representing the locations within the <see cref="Description"/> where the original search term was found.
        /// Useful for highlighting the original search term in the results.
        /// </summary>
        [JsonProperty("matched_substrings")]
        public IEnumerable<MatchedSubstring> MatchedSubstrings { get; set; }

        /// <summary>
        /// Unique text id that can be used in a separate query to request more specific details about the place
        /// </summary>
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// Unique token that can be used to retrieve additional information about the place
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The terms that make up the <see cref="Description"/>, usually separated by commas
        /// </summary>
        public IEnumerable<Term> Terms { get; set; }

        /// <summary>
        /// The types that apply to this place. 
        /// See https://developers.google.com/places/web-service/autocomplete#place_types
        /// </summary>
        public IEnumerable<string> Types { get; set; }
    }
}

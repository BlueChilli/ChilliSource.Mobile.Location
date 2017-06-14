#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Stores parameters that control how the search results should be restricted
    /// </summary>
	public class AutocompleteRequest
	{
        /// <summary>
        /// The types of place results to return. 
        /// See https://developers.google.com/places/web-service/autocomplete#place_types
        /// </summary>
		public string Types { get; set; }

        /// <summary>
        /// A grouping of places to which you would like to restrict your results. 
        /// Currently, you can use components to filter by up to 5 countries. 
        /// Countries must be passed as a two character, ISO 3166-1 Alpha-2 compatible country code. 
        /// For example: components=country:fr would restrict your results to places within France
        /// </summary>
		public string Components { get; set; }

        /// <summary>
        /// Specifies the country code to restrict the search to. E.g. "au".
        /// </summary>
		public string Region { get; set; }
	}
}

#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
	/// <summary>
	/// Provides methods for building Google Places request Urls
	/// </summary>
	internal class PlacesUrlFactory
	{
		private static string _baseURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?key=";
	    private static string _baseDetailUrl = "https://maps.googleapis.com/maps/api/place/details/json?key=";

        private readonly string _apiKey;
		private readonly string _language;

		internal PlacesUrlFactory(string apiKey, string language)
		{
			_apiKey = apiKey;
			_language = language;
		}

        /// <summary>
        /// Constructs a <see cref="Uri"/> for a search request based on the provided <paramref name="searchString"/>
        /// <param name="searchString">Search query</param>
        /// <param name="request">Autocomplete request specifying additional components in the search Url to be generated</param>
        /// <returns>URI</returns>
        /// </summary>
        public Uri BuildSearchUrl(string searchString, AutocompleteRequest request)
		{
			var url = new StringBuilder();
			url.Append($"{_baseURL}{_apiKey}");
			url.Append($"&input={searchString}");

			if (!string.IsNullOrWhiteSpace(_language))
			{
				url.Append($"&language={_language}");
			}

			if (request != null)
			{
				if (!string.IsNullOrWhiteSpace(request.Components))
				{
					url.Append($"&components={request.Components}");
				}

				if (!string.IsNullOrWhiteSpace(request.Types))
				{
					url.Append($"&types={request.Types}");
				}

				if (!string.IsNullOrWhiteSpace(request.Region))
				{
					url.Append("&region=au");
				}
			}

			return new Uri(url.ToString());
		}

        /// <summary>
        /// Constructs a <see cref="Uri"/> to request additional details about a particular place
        /// represented by the <paramref name="placeId"/>
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
		public Uri BuildDetailsUrl(string placeId)
		{
			var url = new StringBuilder();
			url.Append($"{_baseDetailUrl}{_apiKey}");
			url.Append($"&placeid={placeId}");

			if (!string.IsNullOrWhiteSpace(_language))
			{
				url.Append($"&language={_language}");
			}

			return new Uri(url.ToString());
		}
	}
}

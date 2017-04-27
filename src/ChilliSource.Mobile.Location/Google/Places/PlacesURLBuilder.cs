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
	/// Constructs the Places request URL.
	/// </summary>
	internal class PlacesURLBuilder
	{
		private static string _baseURL = "https://maps.googleapis.com/maps/api/place/autocomplete/json?key=";

		private readonly string _apiKey;
		private readonly string _language;

		internal PlacesURLBuilder(string apiKey, string language)
		{
			_apiKey = apiKey;
			_language = language;
		}

		/// <summary>
		/// Generates the URL.
		/// <param name="input">Search input</param>
		/// <param name="request">Autocomplete request</param>
		/// <returns>URI</returns>
		/// </summary>
		public Uri GenerateSearchUrl(string input, AutocompleteRequest request)
		{
			var url = new StringBuilder();
			url.Append($"{_baseURL}{_apiKey}");
			url.Append($"&input={input}");

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

		public Uri GenerateDetailsUrl(string placeId)
		{
			var url = new StringBuilder();
			url.Append($"{_baseURL}{_apiKey}");
			url.Append($"&placeid={placeId}");

			if (!string.IsNullOrWhiteSpace(_language))
			{
				url.Append($"&language={_language}");
			}

			return new Uri(url.ToString());
		}
	}
}

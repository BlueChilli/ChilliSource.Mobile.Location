#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChilliSource.Mobile.Location.Google.Places
{
	/// <summary>
	/// Provides a list of addresses from a search query
	/// </summary>
	public class PlacesService
	{
		private PlacesURLBuilder _urlFactory;

		private string _apiKey;
		private readonly string _language;
		private readonly ICaching _cachingProvider;

		public PlacesService(string apiKey) : this(apiKey, null) { }

		/// <summary>
		/// Constructor
		/// <param name="apiKey">API Key</param>
		/// <param name="language">Language</param>
		/// </summary>
		public PlacesService(string apiKey, string language)
		{
			if (apiKey == null) throw new NullReferenceException("apiKey");

			_apiKey = apiKey;
			_language = language;
			_urlFactory = new PlacesURLBuilder(_apiKey, _language);
			_cachingProvider = new CachingProvider();
		}

		public string ApiKey
		{
			set
			{
				_apiKey = value;
				_urlFactory = new PlacesURLBuilder(value, _language);
			}
		}

		/// <summary>
		/// Simple search
		/// <param name="input">Search string</param>
		/// </summary>
		public Task<PlaceResult> SearchAsync(string input)
		{
			return SearchAsync(input, null);
		}

		/// <summary>
		/// Autocomplete search
		/// <param name="input">Search string</param>
		/// <param name="request">Autocomplete request</param>
		/// </summary>
		public async Task<PlaceResult> AutocompleteAsync(string input, AutocompleteRequest request)
		{
			//Get cached results
			var results = _cachingProvider.GetAutocompleteResult(input, request);

			if (results != null)
				return results;

			var items = await SearchAsync(input, new AutocompleteRequest
			{
				Region = "au"
			}).ContinueWith(x =>
				{
					if (x.Result == null)
						throw new NullReferenceException("result");

					return x.Result;

				}).ConfigureAwait(false);

			results = items;

			//Store results
			_cachingProvider.StoreAutocompleteResult(input, request, results);

			return results;
		}

		/// <summary>
		/// Simple search
		/// <param name="input">Search string</param>
		/// <param name="request">Autocomplete request</param>
		/// <returns>Place result with predictions</returns>
		/// </summary>
		public Task<PlaceResult> SearchAsync(string input, AutocompleteRequest request)
		{
			if (string.IsNullOrWhiteSpace(input)) throw new NullReferenceException("input");

			var url = _urlFactory.GenerateSearchUrl(input, request);

			using (var webClient = new WebClient())
			{
				webClient.Encoding = Encoding.UTF8;

				return webClient.DownloadStringTaskAsync(url)
						.ContinueWith(x => JsonConvert.DeserializeObject<PlaceResult>(x.Result));
			}
		}

		/// <summary>
		/// Get detailed results of a place
		/// <param name="prediction">Search prediction</param>
		/// <returns>Place a detailed result object</returns>
		/// </summary>
		public Task<DetailsResult> GetDetailsByPlaceId(Prediction prediction)
		{
			if (string.IsNullOrWhiteSpace(prediction.PlaceId)) throw new NullReferenceException("placeId");

			var url = _urlFactory.GenerateDetailsUrl(prediction.PlaceId);

			using (var webClient = new WebClient())
			{
				webClient.Encoding = Encoding.UTF8;

				return webClient.DownloadStringTaskAsync(url)
						.ContinueWith(x =>
				{
					var detailsResult = JsonConvert.DeserializeObject<DetailsResult>(x.Result);

					if (detailsResult.Result.PredictedAddress != null)
					{
						detailsResult.Result.PredictedAddress = prediction.Description;
					}
					return detailsResult;
				});
			}
		}
	}
}

#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Location.Google.Places
{
	/// <summary>
	/// Provides functionality to query Google's Places Api and retrieve autocomplate places lists
	/// </summary>
	public class PlacesService
	{
		private readonly PlacesUrlFactory _urlFactory;		    
		private readonly CachingProvider _cachingProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey">Google Places Api key</param>
		public PlacesService(string apiKey) : this(apiKey, null) { }

		/// <summary>
		/// Constructor
		/// <param name="apiKey">Google Places Api key</param>
		/// <param name="language">Language of the search request strings</param>
		/// </summary>
		public PlacesService(string apiKey, string language)
		{
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("apiKey");
            }
            			
			_urlFactory = new PlacesUrlFactory(apiKey, language);
			_cachingProvider = new CachingProvider();
		}
		
		/// <summary>
		/// Performs a simple search without any custom parameters
		/// <param name="searchString">Search string</param>
		/// </summary>
		public Task<OperationResult<PlaceResponse>> SearchAsync(string searchString)
		{
			return SearchAsync(searchString, null);
		}

        /// <summary>
        /// Invokes <see cref="SearchAsync(string, AutocompleteRequest)"/> with the given <paramref name="autocomleteRequest"/> and caches the results  
        /// <param name="input">Search string</param>
        /// <param name="request">Autocomplete request specifying additional components in the search Url to be generated</param>
        /// </summary>
        public async Task<OperationResult<PlaceResponse>> AutocompleteAsync(string input, AutocompleteRequest autocomleteRequest)
		{
			//Get cached results
			var result = _cachingProvider.GetAutocompleteResult(input);

            if (result != null)
            {
                return OperationResult<PlaceResponse>.AsSuccess(result);
            }

			var searchResult = await SearchAsync(input, new AutocompleteRequest
			{
				Region = "au"
			})
            .ContinueWith(response =>
			    {
                    if (response.Result == null)
                    {
                        return OperationResult<PlaceResponse>.AsFailure("Search response result is null");                       
                    }
				    return response.Result;
			    })
            .ConfigureAwait(false);

            if (searchResult.IsSuccessful)
            {
                _cachingProvider.StoreAutocompleteResult(input, searchResult.Result);
            }

			return searchResult;
		}

		/// <summary>
		/// Invokes places search webservice
		/// <param name="searchString">Search string</param>
		/// <param name="request">Autocomplete request specifying additional components in the search Url to be generated</param>
		/// <returns>Place result with predictions</returns>
		/// </summary>
		public Task<OperationResult<PlaceResponse>> SearchAsync(string searchString, AutocompleteRequest request)
		{
            if (string.IsNullOrWhiteSpace(searchString))
            {
                Task.Run(() =>
                {
                    return OperationResult<DetailsResponse>.AsFailure("PlaceId cannot be empty");
                });
            }

			var url = _urlFactory.BuildSearchUrl(searchString, request);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=utf-8");

				return client.GetStringAsync(url)
						.ContinueWith(response =>
                        {
                            var result = JsonConvert.DeserializeObject<PlaceResponse>(response.Result);
                            return OperationResult<PlaceResponse>.AsSuccess(result);
                           });
			}
		}

        /// <summary>
        /// Invokes places detail webservice to download detail info about a place identified by <paramref name="prediction"/>.PlaceId
        /// <param name="prediction">Search prediction</param>
        /// <returns>The place details</returns>
        /// </summary>
        public Task<OperationResult<DetailsResponse>> GetPlaceDetails(Prediction prediction)
		{
            if (string.IsNullOrWhiteSpace(prediction.PlaceId))
            {                
                Task.Run(() =>
                {
                    return OperationResult<DetailsResponse>.AsFailure("PlaceId cannot be empty");
                });                
            }

			var url = _urlFactory.BuildDetailsUrl(prediction.PlaceId);

			using (var client = new HttpClient())
			{
			    client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=utf-8");

                return client.GetStringAsync(url)
                        .ContinueWith(response =>
				            {
					            var detailsResult = JsonConvert.DeserializeObject<DetailsResponse>(response.Result);

					            if (detailsResult.Result.Prediction != null)
					            {
						            detailsResult.Result.Prediction = prediction.Description;
					            }
					            return OperationResult<DetailsResponse>.AsSuccess(detailsResult);
				            });
			}
		}
	}
}

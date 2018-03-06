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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
	/// Provides functionality to query Google's Places Api and retrieve autocomplete places lists
	/// </summary>
	public class PlacesService : Google.BaseService
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
        public async Task<OperationResult<PlaceResponse>> AutocompleteAsync(string input, AutocompleteRequest autocomleteRequest = null)
		{
			//Get cached results
			var result = _cachingProvider.GetAutocompleteResult(input);

            if (result != null)
            {
                return OperationResult<PlaceResponse>.AsSuccess(result);
            }

		    OperationResult<PlaceResponse> searchResult;

		    if (autocomleteRequest != null)
		    {
		        searchResult = await SearchAsync(input, autocomleteRequest);
		    }
		    else
		    {
		        searchResult = await SearchAsync(input, new AutocompleteRequest()
		        {
		            Region = "au"
		        });
		    }


		    if (!searchResult.IsSuccessful)
		    {
		        return OperationResult<PlaceResponse>.AsFailure("Search response result is null");
            }

		    _cachingProvider.StoreAutocompleteResult(input, searchResult.Result);

            return searchResult;
		}

		/// <summary>
		/// Invokes places search webservice
		/// <param name="searchString">Search string</param>
		/// <param name="request">Autocomplete request specifying additional components in the search Url to be generated</param>
		/// <returns>Place result with predictions</returns>
		/// </summary>
		public async Task<OperationResult<PlaceResponse>> SearchAsync(string searchString, AutocompleteRequest request)
		{
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return OperationResult<PlaceResponse>.AsFailure("Search string cannot be empty");
            }

			var url = _urlFactory.BuildSearchUrl(searchString, request);

            using (var client = new HttpClient())
            {
                AcceptJsonResponse(client);

                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return OperationResult<PlaceResponse>.AsFailure(response.ReasonPhrase);
                }

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<PlaceResponse>(content);

                if (result.Status != GoogleApiResponseStatus.Ok)
                {
                    return OperationResult<PlaceResponse>.AsFailure(result.ErrorMessage);
                }

                return OperationResult<PlaceResponse>.AsSuccess(result);
            }
		}

        /// <summary>
        /// Invokes places detail webservice to download detail info about a place identified by <paramref name="prediction"/>.PlaceId
        /// <param name="prediction">Search prediction</param>
        /// <returns>The place details</returns>
        /// </summary>
        public async Task<OperationResult<DetailsResponse>> GetPlaceDetails(Prediction prediction)
		{
            if (string.IsNullOrWhiteSpace(prediction.PlaceId))
            {
                return OperationResult<DetailsResponse>.AsFailure("PlaceId cannot be empty");
            }

			var url = _urlFactory.BuildDetailsUrl(prediction.PlaceId);

			using (var client = new HttpClient())
			{
			    AcceptJsonResponse(client);

			    var response = await client.GetAsync(url).ConfigureAwait(false);

			    if (!response.IsSuccessStatusCode)
			    {
			        return OperationResult<DetailsResponse>.AsFailure(response.ReasonPhrase);
			    }

			    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			    var detailsResult = JsonConvert.DeserializeObject<DetailsResponse>(content);

                if (detailsResult.Status != GoogleApiResponseStatus.Ok)
                {
                    return OperationResult<DetailsResponse>.AsFailure(detailsResult.ErrorMessage);
                }

                if (detailsResult.Result.Prediction != null)
			    {
			        detailsResult.Result.Prediction = prediction.Description;
                }

                return OperationResult<DetailsResponse>.AsSuccess(detailsResult);
            }
		}

        /// <summary>
        /// Queries Google's reverse geocoding service to return a list of addresses that are closest to the specified 
        /// <paramref name="latitude"/> and <paramref name="longitude"/>
        /// </summary>
        /// <returns>The addresses.</returns>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        public async Task<OperationResult<ReverseGeocodingResponse>> GetAddresses(double latitude, double longitude)
        {            
            var url = _urlFactory.BuildReverseGeocodingUrl(latitude, longitude);

            using (var client = new HttpClient())
            {
                AcceptJsonResponse(client);

                var response = await client.GetAsync(url).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return OperationResult<ReverseGeocodingResponse>.AsFailure(response.ReasonPhrase);
                }

                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<ReverseGeocodingResponse>(content);

                if (result.Status != GoogleApiResponseStatus.Ok)
                {
                    return OperationResult<ReverseGeocodingResponse>.AsFailure(result.ErrorMessage);
                }

                return OperationResult<ReverseGeocodingResponse>.AsSuccess(result);
            }
        }
	}
}

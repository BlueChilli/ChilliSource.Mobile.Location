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

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
    /// <summary>
    ///  Provides functionality to query Google's Directions Api and retrieve directions 
    /// </summary>
    public class DirectionsService
	{
		private string _apiKey;

		/// <summary>
		/// Initializes the service using a Google Directions <paramref name="apiKey"/>
		/// <param name="apiKey">Google Directions API key</param>
		/// </summary>
		public DirectionsService(string apiKey)
		{
			_apiKey = apiKey;
		}

		/// <summary>
		/// Request Google Directions
		/// <param name="request">Request object</param>
		/// <returns>The response model</returns>
		/// </summary>
		public async Task<OperationResult<DirectionsResponse>> RequestDirections(DirectionsRequest request)
		{
			using (var client = new HttpClient())
			{
			    client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=utf-8");

                var result = DirectionsUrlFactory.BuildDirectionsUrl(request, _apiKey);
                if (result.IsSuccessful)
                {
                    var fullUrlString = result.Result;
                    var responseString = await client.GetStringAsync(fullUrlString);

                    return await ProcessResponse(responseString);
                }
                else
                {
                    return OperationResult<DirectionsResponse>.AsFailure(result.Exception);
                }
			}
		}

		/// <summary>
		/// Deserilizes the response from Google
		/// <param name="response">Response string</param>
		/// <returns>The response model</returns>
		/// </summary>
		private async Task<OperationResult<DirectionsResponse>> ProcessResponse(string response)
		{
			try
			{
				return await Task.Run(() =>
				{
					var model = JsonConvert.DeserializeObject<DirectionsResponse>(response);
					model.JsonData = response;
					return OperationResult<DirectionsResponse>.AsSuccess(model);
				});
			}
			catch (JsonSerializationException ex)
			{
                return OperationResult<DirectionsResponse>.AsFailure("Error communicating with Google Directions API: " + ex.Message);				
			}
		}
	}
}

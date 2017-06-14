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
using System.Collections.Generic;
using System.Net.Http;
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
    /// <summary>
    ///  Provides functionality to query Google's Roads Api and retrieve geo points snapped to a road
    /// </summary>
    public class RoadsService : BaseService
	{
		private string _apiKey;		

        /// <summary>
        /// Initializes the service using a Google Roads <paramref name="apiKey"/>
        /// </summary>
        /// <param name="apiKey"></param>
		public RoadsService(string apiKey)
		{
			_apiKey = apiKey;
		}

        /// <summary>
        /// Sends the API request to generate a list of geo points snapped to a road from the specified <paramref name="positions"/> list of unsnapped points.
        /// <param name="positions">A list of unsnapped geo points</param>
        /// <param name="shouldInterpolate">Determines whether to return additional points to smoothly follow the geometry of the road</param>
        /// <returns>Response model with the snapped positions</returns>
        /// </summary>
        public async Task<OperationResult<RoadsResponse>> RequestSnappedLocations(IList<Position> positions, bool shouldInterpolate = false)
		{
			if (positions == null || positions.Count == 0)
			{               
               return OperationResult<RoadsResponse>.AsFailure("Please provide at least one position");               
            }
			
			using (var client = new HttpClient())
			{
                AcceptJsonResponse(client);
                var fullURLString = RoadsUrlFactory.BuildSnapToRoadsUrl(_apiKey, positions, shouldInterpolate);

				try
				{
					var response = await client.GetAsync(fullURLString);

				    if (!response.IsSuccessStatusCode)
				    {
				        return OperationResult<RoadsResponse>.AsFailure(response.ReasonPhrase);
				    }

				    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

				    return await ProcessResponse(content);
                }
				catch (Exception ex)
				{
                   return OperationResult<RoadsResponse>.AsFailure("Error communicating with Google Snap To Roads API: " + ex.Message);					
				}
			}
		}
				
		private async Task<OperationResult<RoadsResponse>> ProcessResponse(string response)
		{
			try
			{
				return await Task.Run(() =>
				{
					var model = JsonConvert.DeserializeObject<RoadsResponse>(response);
					return OperationResult<RoadsResponse>.AsSuccess(model);
				});
			}
			catch (JsonSerializationException ex)
			{
                return OperationResult<RoadsResponse>.AsFailure("Error communicating with Google Snap To Roads API: " + ex.Message);				
			}
		}
	}
}

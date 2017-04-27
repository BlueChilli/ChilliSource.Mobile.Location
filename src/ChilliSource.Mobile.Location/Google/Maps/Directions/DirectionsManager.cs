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

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	/// <summary>
	/// Google Directions Manager
	/// </summary>
	public class DirectionsManager
	{
		private string _apiKey;

		/// <summary>
		/// Initialize manager
		/// <param name="APIKey">Google Directions API key</param>
		/// </summary>
		public DirectionsManager(string APIKey)
		{
			_apiKey = APIKey;
		}

		/// <summary>
		/// Request Google Directions
		/// <param name="request">Request object</param>
		/// <returns>The response model</returns>
		/// </summary>
		public async Task<BaseResponseModel> RequestDirections(Request request)
		{
			using (var webClient = new WebClient())
			{
				webClient.Encoding = Encoding.UTF8;

				var fullURLString = request.GetRequestURL(_apiKey);
				var responseString = await webClient.DownloadStringTaskAsync((fullURLString));

				return await ProcessResponse(responseString);
			}
		}

		/// <summary>
		/// Deserilizes the response from Google
		/// <param name="response">Response string</param>
		/// <returns>The response model</returns>
		/// </summary>
		async Task<BaseResponseModel> ProcessResponse(string response)
		{
			try
			{
				return await Task.Run(() =>
				{
					var model = JsonConvert.DeserializeObject<BaseResponseModel>(response);
					model.Json = response;
					return model;
				});
			}
			catch (JsonSerializationException ex)
			{
				Console.WriteLine("Error communicating with Google Directions API: " + ex.Message);
				return null;
			}
		}
	}
}

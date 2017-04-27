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

namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
	/// <summary>
	/// The manager for requesting a list of points snapped to a road from a provided list of unsnapped points.
	/// </summary>
	public class SnapToRoadsManager
	{
		private string _apiKey;

		private bool _shouldInterpolate;

		public SnapToRoadsManager(string APIKey)
		{
			_apiKey = APIKey;
		}

		/// <summary>
		/// The manager for requesting a list of points snapped to a road from a provided list of unsnapped points.
		/// <param name="positions">A list of unsnapped points</param>
		/// <param name="shouldInterpolate">If the points should follow a smooth line</param>
		/// <returns>Response model</returns>
		/// </summary>
		public async Task<BaseResponseModel> RequestSnappedLocations(IList<Position> positions, bool shouldInterpolate)
		{
			if (positions == null || positions.Count == 0)
			{
				return null;
			}

			_shouldInterpolate = shouldInterpolate;

			using (var webClient = new WebClient())
			{
				webClient.Encoding = Encoding.UTF8;

				var fullURLString = CreateRequestString(positions);

				try
				{
					var responseString = await webClient.DownloadStringTaskAsync((fullURLString));
					return await ProcessResponse(responseString);

				}
				catch (WebException x)
				{
					Console.WriteLine("Error communicating with Google Snap To Roads API: " + x.Message);
					return null;
				}
			}
		}

		/// <summary>
		/// Creates the request.
		/// <param name="positions">A list of unsnapped points</param>
		/// <returns>Request string</returns>
		/// </summary>
		public string CreateRequestString(IList<Position> positions)
		{
			var positionsString = new StringBuilder();

			foreach (var position in positions)
			{
				if (position != null)
				{
					positionsString.Append($"{position.Latitude.ToString()} , {position.Longitude.ToString()}| ");
				}
			}

			if (positionsString.Length <= 2)
			{
				return "";
			}

			positionsString.Remove(positionsString.Length - 2, 2);

			return $"https://roads.googleapis.com/v1/snapToRoads?path={positionsString}&interpolate={_shouldInterpolate.ToString()}&key={_apiKey}";
		}

		/// <summary>
		/// Deserialize the JSON response
		/// <param name="response">Response string</param>
		/// <returns>Response model</returns>
		/// </summary>
		async Task<BaseResponseModel> ProcessResponse(string response)
		{
			try
			{
				return await Task.Run(() =>
				{
					var model = JsonConvert.DeserializeObject<BaseResponseModel>(response);
					return model;
				});
			}
			catch (JsonSerializationException ex)
			{
				Console.WriteLine("Error communicating with Google Snap To Roads API: " + ex.Message);
				return null;
			}
		}
	}
}

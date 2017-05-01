#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Text;
using Humanizer;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	/// <summary>
	/// Builds the request URL
	/// </summary>
	public static class RequestURLBuilder
	{
		private const string GoogleDirectionsApiUrl = "https://maps.googleapis.com/maps/api/directions/json?";

		public static string BuildDirectionsURL(Request request, string apiKey)
		{
			if (request == null || request.OriginCoordinates == null || (apiKey?.Length == 0)) return null;

			var stringBuilder = new StringBuilder();

			//URL
			stringBuilder.Append(GoogleDirectionsApiUrl);

			//Origin
			stringBuilder.Append($"&origin={request.OriginCoordinates.Item1},{request.OriginCoordinates.Item2}");

			//Destination
			var destinationString = request.DestinationAddress;

			if (request.DestinationPlaceId != null)
			{
				destinationString = $"place_id:{request.DestinationPlaceId}";
			}
			else if (request.DestinationCoordinates != null)
			{
				destinationString = $"{request.DestinationCoordinates.Item1},{request.DestinationCoordinates.Item2}";
			}

			stringBuilder.Append($"&destination={destinationString}");

			//Departure Time
			stringBuilder.Append("&departure_time=now");

			//Travel Mode
			stringBuilder.Append($"&mode={request.Mode.ToString().ToLower()}");

			//Tolls
			if (request.AvoidTolls)
			{
				stringBuilder.Append("&avoid=tolls");
			}

			//Unit Type
			stringBuilder.Append($"&units={request.UnitType.ToString().ToLower()}");

			//Region
			stringBuilder.Append($"&region={request.Region.ToLower()}");

			//Traffic Model
			stringBuilder.Append($"&traffic_model={request.TrafficModel.Humanize().ToLower()}");

			//API Key
			stringBuilder.Append($"&key={apiKey}");

			//Sensor
			stringBuilder.Append("&sensor=false");

			//Add Waypoints
			if (request.Waypoints?.Count > 0)
			{
				stringBuilder.Append("&waypoints=");

				request.Waypoints.ForEach((waypoint) =>
				{
					if (!waypoint.IsStopover)
					{
						stringBuilder.Append("via:");
					}

					stringBuilder.Append($"{waypoint.Latitude},{waypoint.Longitude}");
				});
			}

			Console.WriteLine("GOOGLE DIRECTIONS URL - {0}", stringBuilder.ToString());

			return stringBuilder.ToString();
		}
	}
}

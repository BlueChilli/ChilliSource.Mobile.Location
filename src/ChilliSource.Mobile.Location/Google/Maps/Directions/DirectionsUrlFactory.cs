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
using ChilliSource.Mobile.Core;

namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
	/// <summary>
	/// Provides methods to build Google Directions request URLs
	/// </summary>
	public static class DirectionsUrlFactory
	{
		private const string _googleDirectionsApiBaseUrl = "https://maps.googleapis.com/maps/api/directions/json?";

        /// <summary>
        /// Constructs a Google Directions URL string based on the <paramref name="request"/> and <paramref name="apiKey"/> parameters.
        /// Note that the <paramref name="request"/> parameter must contain a value for OriginCoordinates
        /// </summary>
        /// <param name="request"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
		public static OperationResult<string> BuildDirectionsUrl(DirectionsRequest request, string apiKey)
		{
            if (request == null || request.OriginCoordinates == null)
            {
                return OperationResult<string>.AsFailure("Invalid request parameter specified. Please make sure to specify the OriginCoordinates");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                return OperationResult<string>.AsFailure("Invalid Api key specified");
            }

			var result = new StringBuilder();

			//URL
			result.Append(_googleDirectionsApiBaseUrl);

            //Origin

            var originString = request.OriginAddress;

            if (request.OriginPlaceId != null)
            {
                originString = $"place_id:{request.OriginPlaceId}";
            }
            else if (request.OriginCoordinates != null)
            {
                originString = $"{request.OriginCoordinates.Item1},{request.OriginCoordinates.Item2}";
            }

            result.Append($"&origin={originString}");

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

			result.Append($"&destination={destinationString}");

			//Departure Time
			result.Append("&departure_time=now");

			//Travel Mode
			result.Append($"&mode={request.Mode.ToString().ToLower()}");

			//Tolls
			if (request.AvoidTolls)
			{
				result.Append("&avoid=tolls");
			}

			//Unit Type
			result.Append($"&units={request.UnitType.ToString().ToLower()}");

			//Region
			result.Append($"&region={request.Region.ToLower()}");

			//Traffic Model
			result.Append($"&traffic_model={request.TrafficModel.Humanize().ToLower()}");

			//API Key
			result.Append($"&key={apiKey}");

			//Sensor
			result.Append("&sensor=false");

			//Add Waypoints
			if (request.Waypoints?.Count > 0)
			{
				result.Append("&waypoints=");

				request.Waypoints.ForEach((waypoint) =>
				{
					if (!waypoint.IsStopover)
					{
						result.Append("via:");
					}

					result.Append($"{waypoint.Latitude},{waypoint.Longitude}");
				});
			}
			
			return OperationResult<string>.AsSuccess(result.ToString());
		}        
    }
}

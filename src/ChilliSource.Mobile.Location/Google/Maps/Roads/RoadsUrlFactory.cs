using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
    public static class RoadsUrlFactory
    {
        private const string _googleRoadsApiBaseUrl = "https://roads.googleapis.com/v1/snapToRoads?";

        /// <summary>
		/// Creates the request.
		/// <param name="positions">A list of unsnapped points</param>
        /// <param name="shouldInterpolate">Determines whether to return additional points to smoothly follow the geometry of the road</param>
		/// <returns>Request string</returns>
		/// </summary>
		public static string BuildSnapToRoadsUrl(string apiKey, IList<Position> positions, bool shouldInterpolate)
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

            return _googleRoadsApiBaseUrl + $"path={positionsString}&interpolate={shouldInterpolate.ToString()}&key={apiKey}";
        }
    }
}

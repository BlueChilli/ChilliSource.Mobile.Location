#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core.Extensions;

namespace ChilliSource.Mobile.Location
{
	/// <summary>
	/// Provides methods for working with geographical locations
	/// </summary>
	public static class LocationHelper
	{
		/// <summary>
		/// Calculates the endpoint of a trajectory defined by the <paramref name="startingPoint"/>, 
		/// <paramref name="initialBearing"/>, and <paramref name="distanceInKilometres"/>
		/// </summary>
		/// <returns>A <see cref="T:ChilliSource.Mobile.Location.Position"/> instance representing the endpoint of the calculation</returns>
		/// <param name="startingPoint">Starting point location.</param>
		/// <param name="initialBearing">Initial bearing.</param>
		/// <param name="distanceInKilometres">Distance kilometres.</param>
		/// Source: StackOverflow (http://stackoverflow.com/questions/3225803/calculate-endpoint-given-distance-bearing-starting-point)
		/// Author: Drew Noakes (http://stackoverflow.com/users/24874/drew-noakes)
		public static Position GetPointAtDistanceFrom(Position startingPoint, double initialBearing, double distanceInKilometres)
		{
			const double earthRadiusInKilometres = 6378.14;

			initialBearing = initialBearing.ToRadians();

			var distanceRatio = distanceInKilometres / earthRadiusInKilometres;
			var distanceRatioSine = Math.Sin(distanceRatio);
			var distanceRatioCosine = Math.Cos(distanceRatio);

			var startLatitudeRadiants = startingPoint.Latitude.ToRadians();
			var startLongitudeRadiants = startingPoint.Longitude.ToRadians();

			var startLatitudeCosine = Math.Cos(startLatitudeRadiants);
			var startLatitudeSine = Math.Sin(startLatitudeRadiants);

			var endLatitudeRadiants = Math.Asin((startLatitudeSine * distanceRatioCosine) + (startLatitudeCosine * distanceRatioSine * Math.Cos(initialBearing)));

			var endLongitudeRadiants = startLongitudeRadiants
				+ Math.Atan2(
					Math.Sin(initialBearing) * distanceRatioSine * startLatitudeCosine,
					distanceRatioCosine - startLatitudeSine * Math.Sin(endLatitudeRadiants));

			return new Position
			{
				Heading = initialBearing,
				Latitude = endLatitudeRadiants.ToDegrees(),
				Longitude = endLongitudeRadiants.ToDegrees(),
				Timestamp = DateTime.Now
			};
		}

	}
}

#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Mobile.Location
{
	/// <summary>
	/// Represents a geographical location
	/// </summary>
	public class Position
	{
		public Position()
		{
		}

		public Position(Position position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}

			Timestamp = position.Timestamp;
			Latitude = position.Latitude;
			Longitude = position.Longitude;
			Altitude = position.Altitude;
			AltitudeAccuracy = position.AltitudeAccuracy;
			Accuracy = position.Accuracy;
			Heading = position.Heading;
			Speed = position.Speed;
		}

		public DateTimeOffset Timestamp
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		public double Longitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the altitude in meters relative to sea level.
		/// </summary>
		public double Altitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential position error radius in meters.
		/// </summary>
		public double Accuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential altitude error range in meters.
		/// </summary>
		/// <remarks>
		/// Not supported on Android, will always read 0.
		/// </remarks>
		public double AltitudeAccuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		public double Heading
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential heading error range in raidians.
		/// </summary>
		/// <remarks>
		/// Not supported on Android, will always read 0.
		/// </remarks>
		public double HeadingAccuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		public double MagneticHeading
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the speed in meters per second.
		/// </summary>
		public double Speed
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Course.
		/// </summary>
		public double Course
		{
			get;
			set;
		}

	}
}

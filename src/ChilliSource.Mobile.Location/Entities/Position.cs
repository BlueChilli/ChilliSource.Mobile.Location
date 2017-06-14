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

        /// <summary>
        /// Timestamp of when the location data was recorded
        /// </summary>
		public DateTimeOffset Timestamp
		{
			get;
			set;
		}

		/// <summary>
		/// Location latitude
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// Location longitude
		/// </summary>
		public double Longitude
		{
			get;
			set;
		}

		/// <summary>
		/// Altitude in meters relative to sea level
		/// </summary>
		public double Altitude
		{
			get;
			set;
		}

		/// <summary>
		/// Potential position error radius in meters
		/// </summary>
		public double Accuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Potential altitude error range in meters
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
		/// Heading in degrees relative to true North
		/// </summary>
		public double Heading
		{
			get;
			set;
		}

		/// <summary>
		/// Potential heading error range in raidians
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
		/// Heading in degrees relative to true North
		/// </summary>
		public double MagneticHeading
		{
			get;
			set;
		}

		/// <summary>
		/// The speed in meters per second
		/// </summary>
		public double Speed
		{
			get;
			set;
		}

		/// <summary>
		/// The direction of movement as determined by the GPS reading
		/// </summary>
		public double Course
		{
			get;
			set;
		}

	}
}

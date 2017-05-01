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
	public class GeolocationException : Exception
	{
		public GeolocationException(GeolocationError error) : base("A geolocation error occured: " + error)
		{
			if (!Enum.IsDefined(typeof(GeolocationError), error))
			{
				throw new ArgumentException("error is not a valid GelocationError member", "error");
			}
			Error = error;
		}

		public GeolocationException(GeolocationError error, Exception innerException) : base("A geolocation error occured: " + error, innerException)
		{
			if (!Enum.IsDefined(typeof(GeolocationError), error))
			{
				throw new ArgumentException("error is not a valid GelocationError member", "error");
			}

			Error = error;
		}

		public GeolocationError Error
		{
			get;
			private set;
		}
	}

}

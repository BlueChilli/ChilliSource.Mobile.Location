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
    /// <see cref="Exception"/> representing a <see cref="LocationErrorType"/>
    /// </summary>
	public class LocationException : Exception
    {
        public LocationException(LocationErrorType error) : base("A geolocation error occured: " + error)
        {
            if (!Enum.IsDefined(typeof(LocationErrorType), error))
            {
                throw new ArgumentException("error is not a valid GelocationError member", nameof(error));
            }
            Error = error;
        }

        public LocationException(LocationErrorType error, Exception innerException) : base("A geolocation error occured: " + error, innerException)
        {
            if (!Enum.IsDefined(typeof(LocationErrorType), error))
            {
                throw new ArgumentException("error is not a valid GelocationError member", nameof(error));
            }

            Error = error;
        }

        /// <summary>
        /// Position retrieval error
        /// </summary>
        public LocationErrorType Error
        {
            get;
            private set;
        }
    }

}

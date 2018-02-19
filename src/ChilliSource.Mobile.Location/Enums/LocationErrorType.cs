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
    /// Represents common error scenarios when retrieving location data
    /// </summary>
	public enum LocationErrorType
    {
        /// <summary>
        /// The provider was unable to retrieve any position data.
        /// </summary>
        PositionUnavailable,

        /// <summary>
        /// The app is not, or no longer, authorized to receive location data.
        /// </summary>
        Unauthorized
    }
}

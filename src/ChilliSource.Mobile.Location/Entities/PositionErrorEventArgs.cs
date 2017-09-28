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
    /// <see cref="EventArgs"/> for storing error information when a position error event has been raised
    /// </summary>
	public class PositionErrorEventArgs : EventArgs
	{
		public PositionErrorEventArgs(LocationErrorType error)
		{
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

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
    /// <see cref="EventArgs"/> for storing the new position when the position changed event is raised
    /// </summary>
	public class PositionEventArgs : EventArgs
	{
		public PositionEventArgs(Position position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}

			Position = position;
		}

        /// <summary>
        /// The latest position data
        /// </summary>
		public Position Position
		{
			get;
			private set;
		}
	}
}

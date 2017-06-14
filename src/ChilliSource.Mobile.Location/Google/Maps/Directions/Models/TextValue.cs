#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Mobile.Location.Google.Maps.Directions
{
    /// <summary>
    /// Represents data that has both a numeric value and a human-readable text representation, 
    /// e.g. time or duration of a route
    /// </summary>
	public class TextValue
	{
        /// <summary>
        /// Human-readable representation of <see cref="Value"/>
        /// </summary>
		public string Text { get; set; }

        /// <summary>
        /// Numeric value representing different units depending on usage, eg. time, distance etc.
        /// </summary>
		public double Value { get; set; }
	}
}

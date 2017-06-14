#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.ComponentModel;

namespace ChilliSource.Mobile.Location
{
    /// <summary>
    /// Specifies the assumptions to use when calculating time in traffic
    /// </summary>
	public enum TrafficModel
	{
        /// <summary>
        /// Indicates that the returned duration_in_traffic should be the best estimate 
        /// of travel time given what is known about both historical traffic conditions 
        /// and live traffic. Live traffic becomes more important the closer the 
        /// departure_time is to now
        /// </summary>
		[Description("best_guess")]
		BestGuess,

        /// <summary>
        /// Indicates that the returned duration_in_traffic should be longer than the 
        /// actual travel time on most days, though occasional days with particularly 
        /// bad traffic conditions may exceed this value.
        /// </summary>
        [Description("pessimistic")]
		Pessimistic,

        /// <summary>
        /// Indicates that the returned duration_in_traffic should be shorter than the 
        /// actual travel time on most days, though occasional days with particularly 
        /// good traffic conditions may be faster than this value.
        /// </summary>
        [Description("optimistic")]
		Optimistic
	}
}

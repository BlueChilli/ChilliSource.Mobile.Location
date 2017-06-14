using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Defines the range within the description of the returned <see cref="Prediction"/> that matches the search request string
    /// </summary>
    public class MatchedSubstring
    {
        /// <summary>
        /// Number of characters matched from the <see cref="Offset"/>
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Starting position within the description
        /// </summary>
        public int Offset { get; set; }
    }
}

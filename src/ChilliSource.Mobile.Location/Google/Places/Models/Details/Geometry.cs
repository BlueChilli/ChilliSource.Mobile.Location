using System;
using System.Collections.Generic;
using System.Text;

namespace ChilliSource.Mobile.Location.Google.Places
{
    /// <summary>
    /// Contains <see cref="Location"/> info about the place
    /// </summary>
    public class Geometry
    {
        /// <summary>
        /// Latitude and longitude values for the place
        /// </summary>
        public GoogleCoordinate Location { get; set; }
    }
}

#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xamarin.Forms;

namespace ChilliSource.Mobile.Location
{
    /// <summary>
    /// Effect used to disable user interaction for MapView
    /// </summary>
	public class NonInteractiveMapEffect : RoutingEffect
	{        
        /// <summary>
        /// Hides the Google/Apple logo at the bottom of the map
        /// </summary>
		public bool HideCompanyIcons { get; set; }

		public NonInteractiveMapEffect() : base("ChilliSource.Mobile.Location.NonInteractiveMapEffect")
		{

		}
	}
}


#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Mobile.Location.Google.Maps.Roads
{
	public class SnappedPoint
	{
		public Location Location { get; set; }

		public int OriginalIndex { get; set; }

		public string PlaceId { get; set; }
	}
}

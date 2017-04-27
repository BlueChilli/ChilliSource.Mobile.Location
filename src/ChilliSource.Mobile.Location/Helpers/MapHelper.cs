#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Net;
using Xamarin.Forms;

namespace ChilliSource.Mobile.Location
{
	/// <summary>
	/// Provides helper methods for working with maps
	/// </summary>
	public static class MapHelper
	{
		public static void OpenNativeMapApp(string address)
		{
			var request = Device.OnPlatform(
				// iOS doesn't like %s or spaces in their URLs, so manually replace spaces with +s
				string.Format("http://maps.apple.com/?address={0}", address.Replace(' ', '+')),

				// pass the address to Android if we have it
				string.Format("geo:0,0?q={0})", address),

				// WinPhone
				string.Format("bingmaps:?cp={0}", address)
			);

			Device.OpenUri(new Uri(request));
		}

		public static string BuildGoogleMapsUrlString(string startAddress, string endAddress)
		{
			if (string.IsNullOrEmpty(startAddress) || string.IsNullOrEmpty(endAddress))
			{
				return "";
			}

			return string.Format("comgooglemaps://?saddr={0}&daddr={1}&directionsmode=driving", WebUtility.UrlEncode(startAddress), WebUtility.UrlEncode(endAddress));
		}
	}
}


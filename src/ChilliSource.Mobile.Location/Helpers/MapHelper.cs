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
        /// <summary>
        /// Opens the native maps app at the specified <paramref name="address"/> on each platform
        /// </summary>
        /// <param name="address">Street address to display on map</param>
		public static void OpenNativeMapApp(string address)
		{
            string request = String.Empty;

            switch(Device.RuntimePlatform) 
            {
                case Device.iOS:
                    request = string.Format("http://maps.apple.com/?address={0}", address.Replace(' ', '+'));
                    break;
                case Device.Android:
					request = string.Format("geo:0,0?q={0})", address);
					break;
                case Device.WinPhone:
                    request = string.Format("bingmaps:?cp={0}", address);
                    break;
                default:
                    throw new NotSupportedException();
			}

			Device.OpenUri(new Uri(request));
		}

        /// <summary>
        /// Creates a Google Maps Url string requesting driving navigation directions from the <paramref name="startAddress"/> to the <paramref name="endAddress"/>
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
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


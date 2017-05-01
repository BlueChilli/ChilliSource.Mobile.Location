#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Maps;
using System.Linq;
using ChilliSource.Mobile.Location;

[assembly: ExportEffect(typeof(NonInteractiveMapViewEffect), "NonInteractiveMapEffect")]
namespace ChilliSource.Mobile.Location
{
	public class NonInteractiveMapViewEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var mapView = Control as MapView;

			mapView.GetMapAsync(new MapReadyHandler());

			var effect = (NonInteractiveMapEffect)Element.Effects.FirstOrDefault(e => e is NonInteractiveMapEffect);

			if (effect.HideCompanyIcons)
			{
				mapView.SetPadding(0, 0, 0, -75);
			}
		}

		protected override void OnDetached()
		{
		}

		class MapReadyHandler : Java.Lang.Object, IOnMapReadyCallback
		{
			public void OnMapReady(GoogleMap googleMap)
			{
				googleMap.UiSettings.ZoomControlsEnabled = false;
				googleMap.UiSettings.ZoomGesturesEnabled = false;
				googleMap.UiSettings.ScrollGesturesEnabled = false;
			}
		}
	}
}


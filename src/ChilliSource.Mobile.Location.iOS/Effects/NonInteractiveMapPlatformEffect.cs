#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MapKit;
using UIKit;
using System.Linq;
using ChilliSource.Mobile.Location;

[assembly: ExportEffect(typeof(NonInteractiveMapPlatformEffect), "NonInteractiveMapEffect")]
namespace ChilliSource.Mobile.Location
{
	public class NonInteractiveMapPlatformEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var mapKitView = Control as MKMapView;
			mapKitView.ShowsPointsOfInterest = false;

			var effect = (NonInteractiveMapEffect)Element.Effects.FirstOrDefault(e => e is NonInteractiveMapEffect);

			if (effect.HideCompanyIcons)
			{
				mapKitView.LayoutMargins = new UIEdgeInsets(0, 0, -20, 0);
			}
		}

		protected override void OnDetached()
		{

		}
	}
}


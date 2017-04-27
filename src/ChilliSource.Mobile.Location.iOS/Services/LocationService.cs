#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Geolocator.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Threading;
using System.Threading.Tasks;
using ChilliSource.Mobile.Location;
using CoreLocation;
using Foundation;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]

namespace ChilliSource.Mobile.Location
{
	public class LocationService : ILocationService
	{
		Position _position;
		CLLocationManager _manager;

		#region Properties

		public double DesiredAccuracy { get; set; }

		public bool IsListening { get; private set; }

		public bool SupportsHeading
		{
			get
			{
				return CLLocationManager.HeadingAvailable;
			}
		}

		public bool IsGeolocationAvailable
		{
			get
			{
				return true;
			} // all iOS devices support at least wifi geolocation
		}

		public bool IsGeolocationEnabled
		{
			get
			{
				return CLLocationManager.Status >= CLAuthorizationStatus.Authorized;
			}
		}

		#endregion

		#region Events

		public event EventHandler<PositionErrorEventArgs> ErrorOccured;

		public event EventHandler<PositionEventArgs> PositionChanged;

		#endregion

		#region Public Methods

		public void Initialize(LocationAuthorizationType authorizationType, bool allowBackgroundLocationUpdates)
		{
			_manager = GetManager();
			_manager.AuthorizationChanged += OnAuthorizationChanged;
			_manager.Failed += OnFailed;

			_manager.PausesLocationUpdatesAutomatically = false;

			if (authorizationType == LocationAuthorizationType.Always)
			{
				_manager.RequestAlwaysAuthorization();
			}
			else if (authorizationType == LocationAuthorizationType.WhenInUse)
			{
				_manager.RequestWhenInUseAuthorization();
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0) && allowBackgroundLocationUpdates)
			{
				_manager.AllowsBackgroundLocationUpdates = true;
			}
		}

		public void Dispose()
		{
			if (_manager != null)
			{
				StopListening();

				_manager.AuthorizationChanged -= OnAuthorizationChanged;
				_manager.Failed -= OnFailed;

				_manager.Dispose();
				_manager = null;
			}
		}

		/// <summary>
		/// Start listening for location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// minTime
		/// or
		/// minDistance
		/// </exception>
		/// <exception cref="InvalidOperationException">Already listening</exception>
		public void StartListening(uint minTime, double minDistance, bool includeHeading = false)
		{
			_manager.LocationsUpdated -= OnLocationsUpdated;
			_manager.UpdatedHeading -= OnHeadingUpdated;

			_manager.LocationsUpdated += OnLocationsUpdated;
			_manager.UpdatedHeading += OnHeadingUpdated;

			if (minTime < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(minTime));
			}
			if (minDistance < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(minDistance));
			}
			if (IsListening)
			{
				throw new InvalidOperationException("Already listening");
			}

			IsListening = true;
			_manager.DesiredAccuracy = DesiredAccuracy;
			_manager.DistanceFilter = minDistance;
			_manager.StartUpdatingLocation();

			if (includeHeading && CLLocationManager.HeadingAvailable)
			{
				_manager.StartUpdatingHeading();
			}
		}

		public void StopListening()
		{
			if (!IsListening)
			{
				return;
			}

			_manager.LocationsUpdated -= OnLocationsUpdated;
			_manager.UpdatedHeading -= OnHeadingUpdated;


			IsListening = false;
			if (CLLocationManager.HeadingAvailable)
			{
				_manager.StopUpdatingHeading();
			}

			_manager.StopUpdatingLocation();
			_position = null;
		}

		public Task<Position> GetPositionAsync(int timeout, bool includeHeading = false)
		{
			return GetPositionAsync(timeout, CancellationToken.None, includeHeading);
		}

		public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading = false)
		{
			return GetPositionAsync(Timeout.Infinite, cancelToken, includeHeading);
		}

		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading = false)
		{
			if (timeout <= 0 && timeout != Timeout.Infinite)
			{
				throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be positive or Timeout.Infinite");
			}

			TaskCompletionSource<Position> tcs;
			if (!IsListening)
			{
				var manager = GetManager();

				tcs = new TaskCompletionSource<Position>(manager);
				var singleListener = new GeolocationSingleUpdateDelegate(manager, DesiredAccuracy, includeHeading, timeout, cancelToken);
				manager.Delegate = singleListener;

				manager.StartUpdatingLocation();
				if (includeHeading && SupportsHeading)
				{
					manager.StartUpdatingHeading();
				}

				return singleListener.Task;
			}
			else
			{
				tcs = new TaskCompletionSource<Position>();
			}

			if (_position == null)
			{
				EventHandler<PositionErrorEventArgs> gotError = null;
				gotError = (s, e) =>
					{
						tcs.TrySetException(new GeolocationException(e.Error));
						ErrorOccured -= gotError;
					};

				ErrorOccured += gotError;

				EventHandler<PositionEventArgs> gotPosition = null;
				gotPosition = (s, e) =>
					{
						tcs.TrySetResult(e.Position);
						PositionChanged -= gotPosition;
					};

				PositionChanged += gotPosition;
			}
			else
			{
				tcs.SetResult(_position);
			}

			return tcs.Task;
		}


		#endregion

		#region Event Handlers

		private void OnHeadingUpdated(object sender, CLHeadingUpdatedEventArgs e)
		{
			if (e.NewHeading.TrueHeading == -1)
			{
				return;
			}

			var p = (_position == null) ? new Position() : new Position(_position);

			p.Heading = e.NewHeading.TrueHeading;
			p.MagneticHeading = e.NewHeading.MagneticHeading;
			p.HeadingAccuracy = e.NewHeading.HeadingAccuracy;

			_position = p;

			OnPositionChanged(new PositionEventArgs(p));
		}

		private void OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			foreach (var location in e.Locations)
			{
				UpdatePosition(location);
			}
		}

		private void UpdatePosition(CLLocation location)
		{
			var p = (_position == null) ? new Position() : new Position(_position);

			if (location.HorizontalAccuracy > -1)
			{
				p.Accuracy = location.HorizontalAccuracy;
				p.Latitude = location.Coordinate.Latitude;
				p.Longitude = location.Coordinate.Longitude;
				p.Course = location.Course;
			}

			if (location.VerticalAccuracy > -1)
			{
				p.Altitude = location.Altitude;
				p.AltitudeAccuracy = location.VerticalAccuracy;
			}

			if (location.Speed > -1)
			{
				p.Speed = location.Speed;
			}

			p.Timestamp = new DateTimeOffset((DateTime)location.Timestamp);

			_position = p;

			OnPositionChanged(new PositionEventArgs(p));

			location.Dispose();
		}

		private void OnFailed(object sender, NSErrorEventArgs e)
		{
			if ((int)e.Error.Code == (int)CLError.Network)
			{
				OnPositionError(new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
			}
		}

		private void OnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
		{
			if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
			{
				OnPositionError(new PositionErrorEventArgs(GeolocationError.Unauthorized));
			}
		}

		private void OnPositionChanged(PositionEventArgs e)
		{
			var changed = PositionChanged;
			if (changed != null)
			{
				changed(this, e);
			}
		}

		private void OnPositionError(PositionErrorEventArgs e)
		{
			StopListening();

			var error = ErrorOccured;
			if (error != null)
			{
				error(this, e);
			}
		}

		#endregion

		#region Helper Methods

		private CLLocationManager GetManager()
		{
			CLLocationManager manager = null;
			new NSObject().InvokeOnMainThread(() => manager = new CLLocationManager());
			return manager;
		}

		#endregion
	}
}

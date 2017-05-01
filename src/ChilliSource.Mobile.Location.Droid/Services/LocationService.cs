#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using ChilliSource.Mobile.Location;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]

namespace ChilliSource.Mobile.Location
{
	public class LocationService : ILocationService
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		private string _headingProvider;
		private Position _lastPosition;
		private GeolocationContinuousListener _listener;
		private LocationManager _manager;
		private readonly object _positionSync = new object();
		private string[] _providers;

		#region Properties

		public bool IsListening
		{
			get
			{
				return _listener != null;
			}
		}

		public double DesiredAccuracy { get; set; }

		public bool SupportsHeading
		{
			get
			{

				if (string.IsNullOrEmpty(_headingProvider) || _manager.IsProviderEnabled(_headingProvider))
				{
					Criteria c = new Criteria { BearingRequired = true };
					string providerName = _manager.GetBestProvider(c, enabledOnly: false);

					LocationProvider provider = _manager.GetProvider(providerName);

					if (provider.SupportsBearing())
					{
						_headingProvider = providerName;
						return true;
					}
					else
					{
						_headingProvider = null;
						return false;
					}
				}
				else
				{
					return true;
				}
			}
		}

		public bool IsGeolocationAvailable
		{
			get
			{
				return _providers.Length > 0;
			}
		}

		public bool IsGeolocationEnabled
		{
			get
			{
				return _providers.Any(_manager.IsProviderEnabled);
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
			_manager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
			_providers = _manager.GetProviders(false).Where(s => s != LocationManager.PassiveProvider).ToArray();
		}

		public void Dispose()
		{
			_manager?.Dispose();
			_manager = null;
		}

		public void StopListening()
		{
			if (_listener == null)
			{
				return;
			}

			_listener.PositionChanged -= OnListenerPositionChanged;
			_listener.PositionError -= OnListenerPositionError;

			for (var i = 0; i < _providers.Length; ++i)
			{
				_manager.RemoveUpdates(_listener);
			}

			_listener = null;
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///     minTime
		///     or
		///     minDistance
		/// </exception>
		/// <exception cref="System.InvalidOperationException">This Geolocator is already listening</exception>
		public void StartListening(uint minTime, double minDistance, bool includeHeading)
		{
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
				throw new InvalidOperationException("This Geolocator is already listening");
			}

			_listener = new GeolocationContinuousListener(_manager, TimeSpan.FromMilliseconds(minTime), _providers);
			_listener.PositionChanged += OnListenerPositionChanged;
			_listener.PositionError += OnListenerPositionError;

			var looper = Looper.MyLooper() ?? Looper.MainLooper;
			for (var i = 0; i < _providers.Length; ++i)
			{
				_manager.RequestLocationUpdates(_providers[i], minTime, (float)minDistance, _listener, looper);
			}
		}

		public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading = false)
		{
			return GetPositionAsync(Timeout.Infinite, cancelToken);
		}

		public Task<Position> GetPositionAsync(int timeout, bool includeHeading = false)
		{
			return GetPositionAsync(timeout, CancellationToken.None);
		}

		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading = false)
		{
			if (timeout <= 0 && timeout != Timeout.Infinite)
			{
				throw new ArgumentOutOfRangeException("timeout", "timeout must be greater than or equal to 0");
			}

			var tcs = new TaskCompletionSource<Position>();

			if (!IsListening)
			{
				GeolocationSingleListener singleListener = null;
				singleListener = new GeolocationSingleListener(
					(float)DesiredAccuracy,
					timeout,
					_providers.Where(_manager.IsProviderEnabled),
					() =>
						{
							for (var i = 0; i < _providers.Length; ++i)
							{
								_manager.RemoveUpdates(singleListener);
							}
						});

				if (cancelToken != CancellationToken.None)
				{
					cancelToken.Register(
						() =>
							{
								singleListener.Cancel();

								for (var i = 0; i < _providers.Length; ++i)
								{
									_manager.RemoveUpdates(singleListener);
								}
							},
						true);
				}

				try
				{
					var looper = Looper.MyLooper() ?? Looper.MainLooper;

					var enabled = 0;
					for (var i = 0; i < _providers.Length; ++i)
					{
						if (_manager.IsProviderEnabled(_providers[i]))
						{
							enabled++;
						}

						_manager.RequestLocationUpdates(_providers[i], 0, 0, singleListener, looper);
					}

					if (enabled == 0)
					{
						for (var i = 0; i < _providers.Length; ++i)
						{
							_manager.RemoveUpdates(singleListener);
						}

						tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
						return tcs.Task;
					}
				}
				catch (SecurityException ex)
				{
					tcs.SetException(new GeolocationException(GeolocationError.Unauthorized, ex));
					return tcs.Task;
				}

				return singleListener.Task;
			}

			// If we're already listening, just use the current listener
			lock (_positionSync)
			{
				if (_lastPosition == null)
				{
					if (cancelToken != CancellationToken.None)
					{
						cancelToken.Register(() => tcs.TrySetCanceled());
					}

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
					tcs.SetResult(_lastPosition);
				}
			}

			return tcs.Task;
		}

		#endregion

		#region Event Handlers

		private void OnListenerPositionChanged(object sender, PositionEventArgs e)
		{
			if (!IsListening) // ignore anything that might come in afterwards
			{
				return;
			}

			lock (_positionSync)
			{
				_lastPosition = e.Position;

				var changed = PositionChanged;
				if (changed != null)
				{
					changed(this, e);
				}
			}
		}

		private void OnListenerPositionError(object sender, PositionErrorEventArgs e)
		{
			StopListening();

			var error = ErrorOccured;
			if (error != null)
			{
				error(this, e);
			}
		}

		#endregion

		#region Helper methods

		internal static DateTimeOffset GetTimestamp(Android.Locations.Location location)
		{
			return new DateTimeOffset(Epoch.AddMilliseconds(location.Time));
		}

		#endregion
	}
}

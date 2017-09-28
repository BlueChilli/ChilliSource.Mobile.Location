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
// <copyright file="GeolocationSingleListener.cs" company="XLabs Team">
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.Locations;
using Android.OS;
using ChilliSource.Mobile.Core;
using Object = Java.Lang.Object;

namespace ChilliSource.Mobile.Location
{

    internal class GeolocationSingleListener : Object, ILocationListener
    {
        private Android.Locations.Location _bestLocation;

        private readonly HashSet<string> _activeProviders;

        private readonly TaskCompletionSource<OperationResult<Position>> _tcs = new TaskCompletionSource<OperationResult<Position>>();

        private readonly double _desiredAccuracy;

        private readonly Action _finishedCallback;

        private readonly object _locationSync = new object();

        private readonly Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeolocationSingleListener" /> class.
        /// </summary>
        /// <param name="desiredAccuracy">The desired accuracy.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="activeProviders">The active providers.</param>
        /// <param name="finishedCallback">The finished callback.</param>
        public GeolocationSingleListener(double desiredAccuracy, int timeout,
            IEnumerable<string> activeProviders, Action finishedCallback)
        {
            _desiredAccuracy = desiredAccuracy;
            _finishedCallback = finishedCallback;

            _activeProviders = new HashSet<string>(activeProviders);

            if (timeout != Timeout.Infinite)
            {
                _timer = new Timer(TimesUp, null, timeout, 0);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            base.Dispose(disposing);
        }

        public Task<OperationResult<Position>> Task
        {
            get
            {
                return _tcs.Task;
            }
        }

        /// <summary>
        ///     Called when the location has changed.
        /// </summary>
        /// <param name="location">The new location, as a Location object.</param>
        /// <since version="Added in API level 1" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">
        ///         Called when the location has changed.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc"> There are no restrictions on the use of the supplied Location object.</para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onLocationChanged(android.location.Location)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        public void OnLocationChanged(Android.Locations.Location location)
        {
            if (location.Accuracy <= _desiredAccuracy)
            {
                Finish(location);
                return;
            }

            lock (_locationSync)
            {
                if (_bestLocation == null || location.Accuracy <= _bestLocation.Accuracy)
                {
                    _bestLocation = location;
                }
            }
        }

        /// <summary>
        ///     Called when the provider is disabled by the user.
        /// </summary>
        /// <param name="provider">
        ///     the name of the location provider associated with this
        ///     update.
        /// </param>
        /// <since version="Added in API level 1" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">
        ///         Called when the provider is disabled by the user. If requestLocationUpdates
        ///         is called on an already disabled provider, this method is called
        ///         immediately.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderDisabled(java.lang.String)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        public void OnProviderDisabled(string provider)
        {
            lock (_activeProviders)
            {
                if (_activeProviders.Remove(provider) && _activeProviders.Count == 0)
                {
                    _tcs.TrySetException(new LocationException(LocationErrorType.PositionUnavailable));
                }
            }
        }

        /// <summary>
        ///     Called when the provider is enabled by the user.
        /// </summary>
        /// <param name="provider">
        ///     the name of the location provider associated with this
        ///     update.
        /// </param>
        /// <since version="Added in API level 1" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">Called when the provider is enabled by the user.</para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderEnabled(java.lang.String)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        public void OnProviderEnabled(string provider)
        {
            lock (_activeProviders) _activeProviders.Add(provider);
        }

        /// <summary>
        ///     Called when the provider status changes.
        /// </summary>
        /// <param name="provider">
        ///     the name of the location provider associated with this
        ///     update.
        /// </param>
        /// <param name="status">
        ///     <c>
        ///         <see cref="F:Android.Locations.Availability.OutOfService" />
        ///     </c>
        ///     if the
        ///     provider is out of service, and this is not expected to change in the
        ///     near future;
        ///     <c>
        ///         <see cref="F:Android.Locations.Availability.TemporarilyUnavailable" />
        ///     </c>
        ///     if
        ///     the provider is temporarily unavailable but is expected to be available
        ///     shortly; and
        ///     <c>
        ///         <see cref="F:Android.Locations.Availability.Available" />
        ///     </c>
        ///     if the
        ///     provider is currently available.
        /// </param>
        /// <param name="extras">
        ///     an optional Bundle which will contain provider specific
        ///     status variables.
        ///     <para tool="javadoc-to-mdoc" />
        ///     A number of common key/value pairs for the extras Bundle are listed
        ///     below. Providers that use any of the keys on this list must
        ///     provide the corresponding value as described below.
        ///     <list type="bullet">
        ///         <item>
        ///             <term>
        ///                 satellites - the number of satellites used to derive the fix
        ///             </term>
        ///         </item>
        ///     </list>
        /// </param>
        /// <since version="Added in API level 1" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">
        ///         Called when the provider status changes. This method is called when
        ///         a provider is unable to fetch a location or if the provider has recently
        ///         become available after a period of unavailability.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onStatusChanged(java.lang.String, int, android.os.Bundle)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            switch (status)
            {
                case Availability.Available:
                    {
                        OnProviderEnabled(provider);
                        break;
                    }
                case Availability.OutOfService:
                    {
                        OnProviderDisabled(provider);
                        break;
                    }
            }
        }

        public void Cancel()
        {
            _tcs.TrySetCanceled();
        }

        private void TimesUp(object state)
        {
            lock (_locationSync)
            {
                if (_bestLocation == null)
                {
                    if (_tcs.TrySetCanceled() && _finishedCallback != null)
                    {
                        _finishedCallback();
                    }
                }
                else
                {
                    Finish(_bestLocation);
                }
            }
        }

        private void Finish(Android.Locations.Location location)
        {
            var position = new Position();
            if (location.HasAccuracy)
            {
                position.Accuracy = location.Accuracy;
            }
            if (location.HasAltitude)
            {
                position.Altitude = location.Altitude;
            }
            if (location.HasBearing)
            {
                position.Heading = location.Bearing;
            }
            if (location.HasSpeed)
            {
                position.Speed = location.Speed;
            }

            position.Longitude = location.Longitude;
            position.Latitude = location.Latitude;
            position.Timestamp = LocationService.GetTimestamp(location);

            _finishedCallback?.Invoke();

            _tcs.TrySetResult(OperationResult<Position>.AsSuccess(position));
        }
    }
}

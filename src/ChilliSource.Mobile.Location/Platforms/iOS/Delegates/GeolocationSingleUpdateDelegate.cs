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
// <copyright file="GeolocationSingleUpdateDelegate.cs" company="XLabs Team">
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
using ChilliSource.Mobile.Core;
using CoreLocation;
using Foundation;
using Xamarin.Forms.Maps;

namespace ChilliSource.Mobile.Location
{
    internal class GeolocationSingleUpdateDelegate : CLLocationManagerDelegate
    {
        private Timer _timer;
        private CLHeading _bestHeading;
        private bool _hasHeading;
        private bool _hasLocation;
        private readonly double _desiredAccuracy;
        private readonly bool _includeHeading;
        private readonly CLLocationManager _manager;
        private readonly Position _position = new Position();
        private readonly TaskCompletionSource<OperationResult<Position>> _tcs;

        public GeolocationSingleUpdateDelegate(
            CLLocationManager manager,
            double desiredAccuracy,
            bool includeHeading,
            int timeout,
            CancellationToken cancelToken)
        {
            _manager = manager;
            _tcs = new TaskCompletionSource<OperationResult<Position>>(manager);
            _desiredAccuracy = desiredAccuracy;
            _includeHeading = includeHeading;

            if (timeout != Timeout.Infinite)
            {
                _timer = new Timer(HandleTimerCallback, null, timeout, 0);
            }

            cancelToken.Register(() =>
                {
                    StopListening();
                    _tcs.TrySetCanceled();
                });
        }

        public Task<OperationResult<Position>> Task
        {
            get
            {
                return _tcs.Task;
            }
        }

        public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
        {
            // If user has services disabled, we're just going to throw an exception for consistency.
            if (status == CLAuthorizationStatus.Denied || status == CLAuthorizationStatus.Restricted)
            {
                StopListening();
                _tcs.SetResult(OperationResult<Position>.AsFailure(new LocationException(LocationErrorType.Unauthorized)));
            }
        }

        public override void Failed(CLLocationManager manager, NSError error)
        {
            switch ((CLError)(int)error.Code)
            {
                case CLError.Network:
                    {
                        StopListening();
                        _tcs.SetResult(OperationResult<Position>.AsFailure((new LocationException(LocationErrorType.PositionUnavailable))));
                        break;
                    }
            }
        }

        public override bool ShouldDisplayHeadingCalibration(CLLocationManager manager)
        {
            return true;
        }

        public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
        {
            if (newLocation.HorizontalAccuracy < 0)
            {
                return;
            }

            if (_hasLocation && newLocation.HorizontalAccuracy > _position.Accuracy)
            {
                return;
            }

            _position.Accuracy = newLocation.HorizontalAccuracy;
            _position.Altitude = newLocation.Altitude;
            _position.AltitudeAccuracy = newLocation.VerticalAccuracy;
            _position.Latitude = newLocation.Coordinate.Latitude;
            _position.Longitude = newLocation.Coordinate.Longitude;
            _position.Speed = newLocation.Speed;
            _position.Timestamp = new DateTimeOffset((DateTime)newLocation.Timestamp);

            _hasLocation = true;

            if ((!_includeHeading || _hasHeading) && _position.Accuracy <= _desiredAccuracy)
            {
                _tcs.TrySetResult(OperationResult<Position>.AsSuccess(new Position(_position)));
                StopListening();
            }
        }

        public override void UpdatedHeading(CLLocationManager manager, CLHeading newHeading)
        {
            if (newHeading.HeadingAccuracy < 0)
            {
                return;
            }
            if (_bestHeading != null && newHeading.HeadingAccuracy >= _bestHeading.HeadingAccuracy)
            {
                return;
            }

            _bestHeading = newHeading;
            _position.Heading = newHeading.TrueHeading;
            _hasHeading = true;

            if (_hasLocation && _position.Accuracy <= _desiredAccuracy)
            {
                _tcs.TrySetResult(OperationResult<Position>.AsSuccess(new Position(_position)));
                StopListening();
            }
        }

        void StopListening()
        {
            if (CLLocationManager.HeadingAvailable)
            {
                _manager.StopUpdatingHeading();
            }

            _manager.StopUpdatingLocation();
        }

        void HandleTimerCallback(object state)
        {
            if (_hasLocation)
            {
                _tcs.TrySetResult(OperationResult<Position>.AsSuccess(new Position(_position)));
            }
            else
            {
                _tcs.TrySetCanceled();
            }

            StopListening();
            _timer.Dispose();
        }
    }
}

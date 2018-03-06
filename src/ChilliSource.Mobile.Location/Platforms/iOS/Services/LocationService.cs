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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChilliSource.Mobile.Core;
using ChilliSource.Mobile.Location;
using CoreLocation;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]

namespace ChilliSource.Mobile.Location
{
    public class LocationService : ILocationService
    {
        Position _position;
        CLLocationManager _manager;
        ILogger _logger;
        double _desiredAccuracy;

        #region Properties



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

        public event EventHandler<RegionEventArgs> RegionEntered;

        public event EventHandler<RegionEventArgs> RegionLeft;

        public event EventHandler<AuthorizationEventArgs> LocationAuthorizationChanged;

        #endregion

        #region Lifecycle

        public void Initialize(LocationAuthorizationType authorizationType, bool allowBackgroundLocationUpdates, bool monitorRegions = false, ILogger logger = null)
        {
            _logger = logger;

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

            if (monitorRegions)
            {
                _manager.DidStartMonitoringForRegion += LocationManager_DidStartMonitoringForRegion;

                _manager.RegionEntered += LocationManager_RegionEntered;

                _manager.RegionLeft += LocationManager_RegionLeft;

                _manager.DidDetermineState += LocationManager_DidDetermineState;
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

        #endregion

        #region Location Monitoring

        public void RequestAlwaysAuthorization()
        {
            if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    _manager.RequestAlwaysAuthorization();
                }
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
        public OperationResult StartListening(uint minTime, double minDistance, double desiredAccurancy = 0, bool includeHeading = false)
        {
            if (IsListening)
            {
                return OperationResult.AsFailure("Already listening");
            }

            if (minTime < 0)
            {
                return OperationResult.AsFailure(new ArgumentOutOfRangeException(nameof(minTime)));
            }
            if (minDistance < 0)
            {
                return OperationResult.AsFailure(new ArgumentOutOfRangeException(nameof(minDistance)));
            }

            _desiredAccuracy = desiredAccurancy;

            _manager.LocationsUpdated -= OnLocationsUpdated;
            _manager.UpdatedHeading -= OnHeadingUpdated;

            _manager.LocationsUpdated += OnLocationsUpdated;
            _manager.UpdatedHeading += OnHeadingUpdated;

            IsListening = true;

            _manager.DesiredAccuracy = _desiredAccuracy;
            _manager.DistanceFilter = minDistance;
            _manager.StartUpdatingLocation();

            if (includeHeading && CLLocationManager.HeadingAvailable)
            {
                _manager.StartUpdatingHeading();
            }

            return OperationResult.AsSuccess();
        }


        public OperationResult StopListening()
        {
            if (!IsListening)
            {
                return OperationResult.AsFailure("Location updates already stopped");
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

            return OperationResult.AsSuccess();
        }

        public void StartListeningForSignificantLocationChanges()
        {
            _manager.StartMonitoringSignificantLocationChanges();
        }

        public void StopListeningForSignificantLocationChanges()
        {
            _manager.StopMonitoringSignificantLocationChanges();
        }


        public void StartMonitoringBeaconRegion(string uuid, ushort major, ushort minor, string identifier)
        {
            var beaconUUID = new NSUuid(uuid);

            CLBeaconRegion beaconRegion = new CLBeaconRegion(beaconUUID, major, minor, identifier)
            {
                NotifyEntryStateOnDisplay = true,
                NotifyOnEntry = true,
                NotifyOnExit = true
            };

            if (_manager != null)
            {
                _manager.StartMonitoring(beaconRegion);
                _logger?.Information($"iOS: Started monitoring: {identifier}");
            }
        }

        public void StartMonitoringCircularRegion(Position centerPosition, double radius, string identifier)
        {
            var coordinate = new CLLocationCoordinate2D(centerPosition.Latitude, centerPosition.Longitude);
            var region = new CLCircularRegion(coordinate, radius, identifier);

            if (_manager != null)
            {
                _manager.StartMonitoring(region);
                _logger?.Information($"iOS: Started monitoring: {identifier}");
            }
        }

        public void StopMonitoringRegion(string identifier)
        {
            var region = _manager.MonitoredRegions.ToArray<CLRegion>().FirstOrDefault(r => r.Identifier.Equals(identifier));

            if (_manager != null && region != null)
            {
                _manager.StopMonitoring(region);
                _logger?.Information($"iOS: Stopped monitoring: {identifier}");
            }
        }

        #endregion

        #region Position Management

        public Task<OperationResult<Position>> GetPositionAsync(int timeout, bool includeHeading = false)
        {
            return GetPositionAsync(timeout, CancellationToken.None, includeHeading);
        }

        public Task<OperationResult<Position>> GetPositionAsync(CancellationToken cancelToken, bool includeHeading = false)
        {
            return GetPositionAsync(Timeout.Infinite, cancelToken, includeHeading);
        }

        public Task<OperationResult<Position>> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading = false)
        {
            TaskCompletionSource<OperationResult<Position>> tcs;

            if (timeout <= 0 && timeout != Timeout.Infinite)
            {
                tcs = new TaskCompletionSource<OperationResult<Position>>();
                var exception = new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be positive or Timeout.Infinite");
                tcs.SetResult(OperationResult<Position>.AsFailure(exception));
                return tcs.Task;
            }

            if (!IsListening)
            {
                var manager = GetManager();

                tcs = new TaskCompletionSource<OperationResult<Position>>(manager);
                var singleListener = new GeolocationSingleUpdateDelegate(manager, _desiredAccuracy, includeHeading, timeout, cancelToken);
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
                tcs = new TaskCompletionSource<OperationResult<Position>>();
            }

            if (_position == null)
            {
                EventHandler<PositionErrorEventArgs> gotError = null;
                gotError = (s, e) =>
                {
                    tcs.SetResult(OperationResult<Position>.AsFailure(new LocationException(e.Error)));

                    ErrorOccured -= gotError;
                };

                ErrorOccured += gotError;

                EventHandler<PositionEventArgs> gotPosition = null;
                gotPosition = (s, e) =>
                {
                    tcs.TrySetResult(OperationResult<Position>.AsSuccess(e.Position));
                    PositionChanged -= gotPosition;
                };

                PositionChanged += gotPosition;
            }
            else
            {
                tcs.SetResult(OperationResult<Position>.AsSuccess(_position));
            }

            return tcs.Task;
        }

        public OperationResult<double> GetDistanceFrom(Position referencePosition)
        {
            if (referencePosition == null)
            {
                return OperationResult<double>.AsFailure("Invalid reference position specified");
            }

            if (_position == null)
            {
                return OperationResult<double>.AsFailure("Current position not available");
            }

            var currentLocation = new CLLocation(_position.Latitude, _position.Longitude);
            var referenceLocation = new CLLocation(referencePosition.Latitude, referencePosition.Longitude);
            return OperationResult<double>.AsSuccess(referenceLocation.DistanceFrom(currentLocation));
        }

        public OperationResult<double> GetDistanceBetween(Position firstPosition, Position secondPosition)
        {
            if (firstPosition == null || secondPosition == null)
            {
                return OperationResult<double>.AsFailure("Invalid positions specified");
            }

            var firstLocation = new CLLocation(firstPosition.Latitude, firstPosition.Longitude);
            var secondLocation = new CLLocation(secondPosition.Latitude, secondPosition.Longitude);

            return OperationResult<double>.AsSuccess(firstLocation.DistanceFrom(secondLocation));
        }

        #endregion

        #region Event Handlers

        void OnHeadingUpdated(object sender, CLHeadingUpdatedEventArgs e)
        {
            if (e.NewHeading.TrueHeading.Equals(-1))
            {
                return;
            }

            var newPosition = (_position == null) ? new Position() : new Position(_position);

            newPosition.Heading = e.NewHeading.TrueHeading;
            newPosition.MagneticHeading = e.NewHeading.MagneticHeading;
            newPosition.HeadingAccuracy = e.NewHeading.HeadingAccuracy;

            _position = newPosition;

            OnPositionChanged(new PositionEventArgs(newPosition));
        }

        void OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            foreach (var location in e.Locations)
            {
                UpdatePosition(location);
            }
        }

        void OnFailed(object sender, NSErrorEventArgs e)
        {
            if ((int)e.Error.Code == (int)CLError.Network)
            {
                OnPositionError(new PositionErrorEventArgs(LocationErrorType.PositionUnavailable));
            }
        }

        void OnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
            {
                LocationAuthorizationChanged?.Invoke(this, new AuthorizationEventArgs(LocationAuthorizationType.None));

                OnPositionError(new PositionErrorEventArgs(LocationErrorType.Unauthorized));
            }
            else if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
            {
                LocationAuthorizationChanged?.Invoke(this, new AuthorizationEventArgs(LocationAuthorizationType.Always));
            }
            else if (e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                LocationAuthorizationChanged?.Invoke(this, new AuthorizationEventArgs(LocationAuthorizationType.WhenInUse));
            }
        }

        void OnPositionChanged(PositionEventArgs e)
        {
            PositionChanged?.Invoke(this, e);
        }

        void OnPositionError(PositionErrorEventArgs e)
        {
            StopListening();

            ErrorOccured?.Invoke(this, e);
        }

        #endregion

        #region Region Event Handlers

        void LocationManager_DidDetermineState(object sender, CLRegionStateDeterminedEventArgs e)
        {
            _logger?.Information($"iOS: State for {e.Region.Identifier} is {e.State.ToString()}");
        }

        void LocationManager_RegionEntered(object sender, CLRegionEventArgs e)
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });

            _logger?.Information($"iOS: Region entered {e.Region.Identifier}");
            RegionEntered?.Invoke(this, new RegionEventArgs(e.Region.Identifier));

            UIApplication.SharedApplication.EndBackgroundTask(taskID);
        }

        void LocationManager_RegionLeft(object sender, CLRegionEventArgs e)
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });

            _logger?.Information($"iOS: Region left {e.Region.Identifier}");
            RegionLeft?.Invoke(this, new RegionEventArgs(e.Region.Identifier));

            UIApplication.SharedApplication.EndBackgroundTask(taskID);

        }

        void LocationManager_DidStartMonitoringForRegion(object sender, CLRegionEventArgs e)
        {
            _manager.RequestState(e.Region);
        }

        #endregion

        #region Helper Methods

        private CLLocationManager GetManager()
        {
            CLLocationManager manager = null;
            new NSObject().InvokeOnMainThread(() => manager = new CLLocationManager());
            return manager;
        }

        private void UpdatePosition(CLLocation location)
        {
            var newPosition = (_position == null) ? new Position() : new Position(_position);

            if (location.HorizontalAccuracy > -1)
            {
                newPosition.Accuracy = location.HorizontalAccuracy;
                newPosition.Latitude = location.Coordinate.Latitude;
                newPosition.Longitude = location.Coordinate.Longitude;
                newPosition.Course = location.Course;
            }

            if (location.VerticalAccuracy > -1)
            {
                newPosition.Altitude = location.Altitude;
                newPosition.AltitudeAccuracy = location.VerticalAccuracy;
            }

            if (location.Speed > -1)
            {
                newPosition.Speed = location.Speed;
            }

            newPosition.Timestamp = new DateTimeOffset((DateTime)location.Timestamp);

            _position = newPosition;

            OnPositionChanged(new PositionEventArgs(newPosition));

            location.Dispose();
        }

        #endregion
    }
}

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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChilliSource.Mobile.Core;
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
        ILogger _logger;
        //List<CLRegion> _monitoredRegions;

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

        public event EventHandler<RegionEventArgs> OnRegionEntered;

        public event EventHandler<RegionEventArgs> OnRegionLeft;

        #endregion

        #region Lifecycle

        public LocationService()
        {
            //_monitoredRegions = new List<CLRegion>();
        }

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
            //_monitoredRegions.Add(beaconRegion);

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

            //_monitoredRegions.Add(region);

            if (_manager != null)
            {
                _manager.StartMonitoring(region);
                _logger?.Information($"iOS: Started monitoring: {identifier}");
            }
        }

        public void StopMonitoringRegion(string identifier)
        {
            var region = _manager.MonitoredRegions.ToArray<CLRegion>().FirstOrDefault(r => r.Identifier.Equals(identifier));

            //var region = _monitoredRegions.FirstOrDefault(b => b.Identifier.Equals(identifier));
            if (_manager != null && region != null)
            {
                _manager.StopMonitoring(region);
                _logger?.Information($"iOS: Stopped monitoring: {identifier}");
                //_monitoredRegions.Remove(region);
            }
        }

        #endregion

        #region Position Management

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
                OnPositionError(new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
            }
        }

        void OnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
            {
                OnPositionError(new PositionErrorEventArgs(GeolocationError.Unauthorized));
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
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });

            _logger?.Information($"iOS: State for {e.Region.Identifier} is {e.State.ToString()}");

            if (e.State == CLRegionState.Inside)
            {
                OnRegionEntered?.Invoke(this, new RegionEventArgs(e.Region.Identifier));
            }

            UIApplication.SharedApplication.EndBackgroundTask(taskID);
        }

        void LocationManager_RegionEntered(object sender, CLRegionEventArgs e)
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });

            _logger?.Information($"iOS: Region entered {e.Region.Identifier}");
            OnRegionEntered?.Invoke(this, new RegionEventArgs(e.Region.Identifier));

            UIApplication.SharedApplication.EndBackgroundTask(taskID);
        }

        void LocationManager_RegionLeft(object sender, CLRegionEventArgs e)
        {
            nint taskID = UIApplication.SharedApplication.BeginBackgroundTask(() => { });

            _logger?.Information($"iOS: Region left {e.Region.Identifier}");
            OnRegionLeft?.Invoke(this, new RegionEventArgs(e.Region.Identifier));

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

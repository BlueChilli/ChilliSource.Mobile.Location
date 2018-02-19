#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IGeolocator.cs" company="XLabs Team">
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

namespace ChilliSource.Mobile.Location
{
    /// <summary>
    /// Provides functionality for monitoring device location events
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Gets a value indicating whether the app is listening for location events.
        /// </summary>
        /// <value><c>true</c> if this instance is listening; otherwise, <c>false</c>.</value>
        bool IsListening { get; }

        /// <summary>
        /// Gets a value indicating whether the device can provide heading information
        /// </summary>
        /// <value><c>true</c> if [supports heading]; otherwise, <c>false</c>.</value>
        bool SupportsHeading { get; }

        /// <summary>
        /// Gets a value indicating whether the device can provide location info.
        /// </summary>
        /// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
        bool IsGeolocationAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether the app has permission to receive location info
        /// </summary>
        /// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
        bool IsGeolocationEnabled { get; }

        /// <summary>
        /// Triggered when an error occurs while monitoring location changes
        /// </summary>
        event EventHandler<PositionErrorEventArgs> ErrorOccured;

        /// <summary>
        /// Triggered when a location change is detected
        /// </summary>
        event EventHandler<PositionEventArgs> PositionChanged;

        /// <summary>
        /// Triggered when the device has entered one of the regions that it was assigned to monitor
        /// </summary>
        event EventHandler<RegionEventArgs> OnRegionEntered;

        /// <summary>
        /// Triggered when the device has left one of the regions that it was assigned to monitor
        /// </summary>
        event EventHandler<RegionEventArgs> OnRegionLeft;

        /// <summary>
        /// Triggered when the user changes the location authorization settings for the app
        /// </summary>
        event EventHandler<AuthorizationEventArgs> OnLocationAuthorizationChanged;

        /// <summary>
        /// Initialize the specified authorizationType, allowBackgroundLocationUpdates, monitorRegions and logger.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="authorizationType">Authorization type. See <see cref="LocationAuthorizationType"/></param>
        /// <param name="allowBackgroundLocationUpdates">If set to <c>true</c> enables high-accuracy background location updates. 
        /// Should only be used for navigation apps or apps that require high-accuracy location updates e.g. for safety and security, otherwise the app
        /// will use up a lot of battery power unnecessarily and also risk getting rejected by Apple.</param>
        /// <param name="monitorRegions">If set to <c>true</c> triggers region monitoring events.</param>
        /// <param name="logger">Logger.</param>
        void Initialize(LocationAuthorizationType authorizationType, bool allowBackgroundLocationUpdates, bool monitorRegions = false, ILogger logger = null);

        void Dispose();

        /// <summary>
        /// Escalate location authorization to Always (iOS 11 only)
        /// </summary>
        void RequestAlwaysAuthorization();

        /// <summary>
        /// Start listening for location changes
        /// </summary>
        /// <param name="minTime">Minimum interval in milliseconds</param>
        /// <param name="minDistance">Minimum distance in meters</param>
        /// <param name="desiredAccurancy">Specifies the accuracy tolerance in meters. 
        /// The higher the number the less accurate the location data will be but the battery performance will be better.</param>
        /// <param name="includeHeading">Include heading information</param>
        OperationResult StartListening(uint minTime, double minDistance, double desiredAccurancy = 0, bool includeHeading = false);

        /// <summary>
        /// Stop listening for location changes
        /// </summary>
        OperationResult StopListening();

        /// <summary>
        /// Starts listening for coarse location changes while the app is in the background. 
        /// Requiers "Always" location permissions to be approved by the user.
        /// </summary>
        void StartListeningForSignificantLocationChanges();

        /// <summary>
        /// Stops listening for coarse background location changes
        /// </summary>
        void StopListeningForSignificantLocationChanges();

        /// <summary>
        /// Creates a region for the beacon matching <paramref name="uuid"/>, <paramref name="major"/>,
        /// and <paramref name="minor"/> and starts listening for events indicating whether the device
        /// has entered or exited the region of the beacon
        /// </summary>
        /// <param name="uuid">Proximmity UUID of the beacons to be monitored.</param>
        /// <param name="major">Value identifying a group of beacons.</param>
        /// <param name="minor">Value identifying a specific beacon within a group.</param>
        /// <param name="identifier">Unique identifier for the region created.</param>
        void StartMonitoringBeaconRegion(string uuid, ushort major, ushort minor, string identifier);

        /// <summary>
        /// Creates a circular region with a <paramref name="centerPosition"/> and <paramref name="radius"/>
        /// and starts listening for events indicating whether the device has 
        /// entered or exited the region of the beacon
        /// </summary>
        /// <param name="centerPosition">Center of circular region to monitor.</param>
        /// <param name="radius">Radius of circular region.</param>
        /// <param name="identifier">Unique identifier for the region created.</param>
        void StartMonitoringCircularRegion(Position centerPosition, double radius, string identifier);

        /// <summary>
        /// Stops the monitoring of the region identified by <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">Region identifier</param>
        void StopMonitoringRegion(string identifier);


        /// <summary>
        /// Returns the most recently captured position info of the device
        /// </summary>
        /// <returns>The position async.</returns>
        /// <param name="timeout">Timeout.</param>
        /// <param name="includeHeading">If set to <c>true</c> include heading.</param>
        Task<OperationResult<Position>> GetPositionAsync(int timeout, bool includeHeading = false);

        /// <summary>
        /// Returns the most recently captured position info of the device
        /// </summary>
        /// <returns>The position async.</returns>
        /// <param name="cancelToken">Cancel token.</param>
        /// <param name="includeHeading">If set to <c>true</c> include heading.</param>
        Task<OperationResult<Position>> GetPositionAsync(CancellationToken cancelToken, bool includeHeading = false);

        /// <summary>
        /// Returns the most recently captured position info of the device
        /// </summary>
        /// <returns>The position async.</returns>
        /// <param name="timeout">Timeout.</param>
        /// <param name="cancelToken">Cancel token.</param>
        /// <param name="includeHeading">If set to <c>true</c> include heading.</param>
        Task<OperationResult<Position>> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading = false);

        /// <summary>
        /// Calculates the distance between the current location and the specified <paramref name="referencePosition"/>
        /// </summary>
        /// <returns>The distance from.</returns>
        /// <param name="referencePosition">Reference position.</param>
        OperationResult<double> GetDistanceFrom(Position referencePosition);


        /// <summary>
        /// Calculates the distance between the two specified positions.
        /// </summary>
        /// <param name="firstPosition">First position.</param>
        /// <param name="secondPosition">Second position.</param>
        OperationResult<double> GetDistanceBetween(Position firstPosition, Position secondPosition);
    }
}

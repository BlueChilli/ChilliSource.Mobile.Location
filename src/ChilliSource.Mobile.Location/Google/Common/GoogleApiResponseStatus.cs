#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System.Runtime.Serialization;

namespace ChilliSource.Mobile.Location.Google
{
    /// <summary>
    /// Common Google Api status codes
    /// </summary>
	public enum GoogleApiResponseStatus
	{
		[EnumMember(Value = "OK")]
		Ok,

        [EnumMember(Value = "ZERO_RESULTS")]
		NoResults,

        [EnumMember(Value = "NOT_FOUND")]
		NotFound,

        [EnumMember(Value = "OVER_QUERY_LIMIT")]
		QueryLimitReached,

        [EnumMember(Value = "INVALID_REQUEST")]
		InvalidRequest,

        [EnumMember(Value = "REQUEST_DENIED")]
        RequestDenied,

        [EnumMember(Value = "UNKNOWN_ERROR")]
		UnknownError,

        [EnumMember(Value = "MAX_ROUTE_LENGTH_EXCEEDED")]
        MaxRouteLengthExceeded,

        [EnumMember(Value = "MAX_WAYPOINTS_EXCEEDED")]
        MaxWaypointsExceeded
    }
}

#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

namespace ChilliSource.Mobile.Location.Google.Places
{
	public abstract class ResponseStatus
	{
		private struct Shared
		{
			public const string InvalidRequest = "INVALID_REQUEST";

			public const string Ok = "OK";

			public const string RequestDenied = "REQUEST_DENIED";

			public const string OverQueryLimit = "OVER_QUERY_LIMIT";

			public const string ZeroResults = "ZERO_RESULTS";

			public const string NotFound = "NOT_FOUND";

			public const string UnknownError = "UNKNOWN_ERROR";
		}

		public struct Places
		{
			public const string UnknownError = Shared.UnknownError;

			public const string InvalidRequest = Shared.InvalidRequest;

			public const string Ok = Shared.Ok;

			public const string ZeroResults = Shared.ZeroResults;

			public const string OverQueryLimit = Shared.OverQueryLimit;

			public const string NotFound = Shared.NotFound;

			public const string RequestDenied = Shared.RequestDenied;
		}

		public struct Autocomplete
		{
			public const string Ok = Shared.Ok;

			public const string ZeroResults = Shared.ZeroResults;

			public const string OverQueryLimit = Shared.OverQueryLimit;

			public const string RequestDenied = Shared.RequestDenied;

			public const string InvalidRequest = Shared.InvalidRequest;
		}
	}
}

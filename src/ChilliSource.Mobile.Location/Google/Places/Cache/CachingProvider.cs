#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System.Collections.Concurrent;

namespace ChilliSource.Mobile.Location.Google.Places
{
	/// <summary>
	/// Caches the places results
	/// </summary>
	internal class CachingProvider : ICaching
	{
		private static readonly ConcurrentDictionary<string, PlaceResult> CacheStorage = new ConcurrentDictionary<string, PlaceResult>();

		public void StoreAutocompleteResult(string input, AutocompleteRequest request, PlaceResult result)
		{
			CacheStorage.TryAdd(input, result);
		}

		public PlaceResult GetAutocompleteResult(string input, AutocompleteRequest request)
		{
			PlaceResult results;
			CacheStorage.TryGetValue(input, out results);

			return results;
		}
	}
}

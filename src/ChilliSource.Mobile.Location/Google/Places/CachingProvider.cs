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
	/// Provides caching functionality for the places autocomplete results
	/// </summary>
	internal class CachingProvider
	{
		private static readonly ConcurrentDictionary<string, PlaceResponse> CacheStorage = new ConcurrentDictionary<string, PlaceResponse>();

        /// <summary>
        /// Stores the provided <paramref name="result"/> using the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
		public void StoreAutocompleteResult(string key, PlaceResponse result)
		{
			CacheStorage.TryAdd(key, result);
		}

        /// <summary>
        /// Retrieves the <see cref="PlaceResponse"/> associated with the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public PlaceResponse GetAutocompleteResult(string key)
		{
			PlaceResponse results;
			CacheStorage.TryGetValue(key, out results);

			return results;
		}
	}
}

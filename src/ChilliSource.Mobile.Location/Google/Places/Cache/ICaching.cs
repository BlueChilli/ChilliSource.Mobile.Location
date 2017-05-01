#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

namespace ChilliSource.Mobile.Location.Google.Places
{
	public interface ICaching
	{
		void StoreAutocompleteResult(string input, AutocompleteRequest request, PlaceResult result);

		PlaceResult GetAutocompleteResult(string input, AutocompleteRequest request);
	}
}

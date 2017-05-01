#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Location.Google.Maps.Directions;
using Xunit;
using System.Text;

namespace Location.Tests
{
	public class GoogleDirectionsTests
	{
		static readonly Tuple<string, string> _originCoordintates = new Tuple<string, string>("-33.865143", "151.209900");

		static readonly string _destinationAddress = "Bennelong Point, Sydney NSW 2000";

		[Fact]
		public void BuildString_ShouldReturnURLString()
		{
			var request = new Request()
			{
				OriginCoordinates = _originCoordintates,
				DestinationAddress = _destinationAddress
			};

			var urlString = request.GetRequestURL("APIKEY");
			Assert.NotNull(urlString);
		}

		[Fact]
		public void BuildString_ShouldReturnNull()
		{
			var request = new Request();
			var urlString = request.GetRequestURL("APIKEY");
			Assert.Null(urlString);
		}

		[Fact]
		public void BuildString_ShouldContainDestinationAddress()
		{
			var request = new Request()
			{
				OriginCoordinates = _originCoordintates,
				DestinationAddress = _destinationAddress
			};

			var urlString = request.GetRequestURL("APIKEY");
			Assert.Contains(_destinationAddress, urlString);
		}
	}
}

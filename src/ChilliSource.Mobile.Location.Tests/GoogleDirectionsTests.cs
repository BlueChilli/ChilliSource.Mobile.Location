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
		public void BuildDirectionsUrl_ShouldReturnURLString_ForValidRequest()
		{
			var request = new DirectionsRequest()
			{
				OriginCoordinates = _originCoordintates,
				DestinationAddress = _destinationAddress
			};

            var result = DirectionsUrlFactory.BuildDirectionsUrl(request, "APIKEY");
            Assert.True(result.IsSuccessful);			
			Assert.NotNull(result.Result);
		}

		[Fact]
		public void BuildDirectionsUrl_ShouldFail_ForInvalidRequest()
		{
			var request = new DirectionsRequest();

            var result = DirectionsUrlFactory.BuildDirectionsUrl(request, "APIKEY");
            Assert.True(result.IsFailure);            
		}

		[Fact]
		public void BuildDirectionsUrl_ShouldAddDestinationAddressToUrl()
		{
			var request = new DirectionsRequest()
			{
				OriginCoordinates = _originCoordintates,
				DestinationAddress = _destinationAddress
			};

            var result = DirectionsUrlFactory.BuildDirectionsUrl(request, "APIKEY");
            Assert.True(result.IsSuccessful);
            Assert.Contains(_destinationAddress, result.Result);
		}
	}
}

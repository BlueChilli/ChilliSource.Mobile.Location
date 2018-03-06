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
using System.Threading.Tasks;
using ChilliSource.Mobile.Location.Google;
using ChilliSource.Mobile.Location.Google.Places;

namespace Location.Tests
{
	public class GoogleDirectionsTests
	{
		static readonly Tuple<string, string> _originCoordintates = new Tuple<string, string>("-33.865143", "151.209900");

		static readonly string _destinationAddress = "Bennelong Point, Sydney NSW 2000";


	    private DirectionsService fixture;
	    private const string GOOGLE_PLACES_API_KEY ="<Put Your Google Places Api Key here>";
	    public GoogleDirectionsTests()
	    {
	        fixture = new DirectionsService(GOOGLE_PLACES_API_KEY);
	    }


	    [Theory(Skip = "Populate the api key above from google and enable this test")]
	    //[Theory]
	    [InlineData("ChIJ13R-ET-uEmsRmwozvQ1oFiY", "ChIJCcEZEl2uEmsR0lmLFhmrdnI")]
        public async Task RequestDirections_ShouldGetDirections(string originPlaceId, string destinationPlaceId)
	    {
	        var r = await fixture.RequestDirections(new DirectionsRequest()
	        {
	            OriginPlaceId = originPlaceId,
                DestinationPlaceId = destinationPlaceId
	        });

	        Assert.NotNull(r);
	        Assert.True(r.IsSuccessful);
	        Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
	    }

        [Fact]
		public void BuildDirectionsUrl_ShouldReturnUrlString_ForValidRequest()
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

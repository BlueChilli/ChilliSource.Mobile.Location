using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChilliSource.Mobile.Location.Google;
using ChilliSource.Mobile.Location.Google.Places;
using Xunit;

namespace ChilliSource.Mobile.Location.Tests
{
    public class GooglePlacesTests
    {
        private PlacesService fixture;
        private const string GOOGLE_PLACES_API_KEY = "<Put Your Google Places Api Key here>";
                
        public GooglePlacesTests()
        {
            fixture = new PlacesService(GOOGLE_PLACES_API_KEY);
        }


        [Theory(Skip = "Populate the api key above from google and enable this test")]
        //[Theory]
        [InlineData("Apple")]
        [InlineData("David Jones")]
        public async Task SearchAsync_ShouldBeAbleToSearchPlaces(string searchData)
        {
            var r = await fixture.SearchAsync(searchData);

            Assert.NotNull(r);
            Assert.True(r.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
        }

        [Theory(Skip = "Populate the api key above from google and enable this test")]
        //[Theory]
        [InlineData("40 George st Sydney NSW")]
        [InlineData("52 York St Sydney NSW")]
        public async Task AutocompleteAsync_ShouldGetAutocompleteThePlaces(string searchData)
        {
            var r = await fixture.AutocompleteAsync(searchData);

            Assert.NotNull(r);
            Assert.True(r.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
        }

        [Theory(Skip = "Populate the api key above from google and enable this test")]
        //[Theory]
        [InlineData("ChIJyfYDuYK2A4gRW5N1BqXWoZw")]
        [InlineData("ChIJj61dQgK6j4AR4GeTYWZsKWw")]
        public async Task GetPlaceDetails_ShouldGetPlaceDetails(string placeId)
        {
            var r = await fixture.GetPlaceDetails(new Prediction()
            {
                PlaceId = placeId
            });

            Assert.NotNull(r);
            Assert.True(r.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
        }

        [Theory(Skip = "Populate the api key above from google and enable this test")]
        //[Theory]
        [InlineData(-27.4565698, 153.0360851)]        
        public async Task GetAddresses_ShouldGetAddressesForCoordinates(double latitude, double longitude)
        {
            var result = await fixture.GetAddresses(latitude, longitude);

            Assert.NotNull(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, result.Result.Status);
            Assert.True(result.Result.Addresses.Count() > 0);            
        }
    }
}

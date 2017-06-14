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
        public async Task ShouldBeAbleToSearchPlaces(string searchData)
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
        public async Task ShouldGetAutocompleteThePlaces(string searchData)
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
        public async Task ShouldGetPlaceDetails(string placeId)
        {
            var r = await fixture.GetPlaceDetails(new Prediction()
            {
                PlaceId = placeId
            });

            Assert.NotNull(r);
            Assert.True(r.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
        }
    }
}

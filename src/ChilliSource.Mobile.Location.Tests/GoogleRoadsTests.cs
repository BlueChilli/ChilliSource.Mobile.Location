using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChilliSource.Mobile.Location.Google;
using ChilliSource.Mobile.Location.Google.Maps.Roads;
using ChilliSource.Mobile.Location.Google.Places;
using Xunit;

namespace ChilliSource.Mobile.Location.Tests
{
    public class GoogleRoadsTests
    {
        private RoadsService fixture;
        private const string GOOGLE_PLACES_API_KEY = "<Put Your Google Places Api Key here>";

        public GoogleRoadsTests()
        {
            fixture = new RoadsService(GOOGLE_PLACES_API_KEY);
        }

        [Fact(Skip = "Populate the api key above from google and enable this test")]
//        [Fact]
        public async Task ShouldBeAbleToRequestASnappedLocation()
        {
            var r = await fixture.RequestSnappedLocations(new List<Position>()
            {
                new Position {Latitude = 60.170880, Longitude=24.942795 },
                new Position {Latitude = 60.170879, Longitude=24.942796},
                new Position {Latitude = 60.170877, Longitude =24.942796}

            });

            Assert.NotNull(r);
            Assert.True(r.IsSuccessful);
            Assert.Equal(GoogleApiResponseStatus.Ok, r.Result.Status);
        }

    }
}

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) ![Built With C#](https://img.shields.io/badge/Built_with-C%23-green.svg)

# ChilliSource.Mobile.Location #

This project is part of the ChilliSource framework developed by [BlueChilli](https://github.com/BlueChilli).

## Summary ##

```ChilliSource.Mobile.Location``` provides location monitoring functionality and wrappers for the Google Roads, Directions, and Places APIs.

## Usage ##

### Location Monitoring ###

**Initialization**

To monitor the device's location first initialize the dependency service:

```csharp
var locationService = DependencyService.Get<ILocationService>();
locationService.Initialize(LocationAuthorizationType.Always, allowBackgroundLocationUpdates:false);
```

**Continuous Monitoring**

For continuous location monitoring subscribe to the ```PositionChanged``` event:

```csharp
locationService.PositionChanged += (sender, args) =>
{
    Console.WriteLine("Position changed to: " + args.Position.Latitude + " " + args.Position.Longitude);   
};
```

Then tell the service to start listening for location changes by specifying the minimum
distance and time interval that should trigger the ```PositionChanged``` event to fire:
```csharp
locationService.StartListening(minTime: 1000, minDistance:10, includeHeading:false);
```

You can stop listening for location changes by calling ```StopListening```:
```csharp
locationService.StopListening();
```

**Once-Off Monitoring**

Alternatively if you would only like to retrieve the device's location once or at specific times, you can do so by calling ```GetPositionAsync``` instead of using the ```PositionChanged``` event:

```csharp
var position = await locationService.GetPositionAsync(timeout: 10, includeHeading: false);
Console.WriteLine("Current position: " + e.Position.Latitude + " " + e.Position.Longitude);   
```

**Position Information**

You can retrieve additional information about the position from the returned ```Position``` object, including Accuracy, Altitude, Heading, Speed, Course, and Timestamp. Please note that some of these properties may not be available or the values may not be accurate depending on the platform and type of device used, GPS availability, etc.

### Google Places ###

With Google Places you can retrieve a list of addresses that match a search text that you specify:

```csharp
var placesService = new PlacesService(googlePlacesApiKey);

var result = await placesService.AutocompleteAsync(searchString, new AutocompleteRequest());
if (result.IsSuccessful)
{
    var predictions = result.Result.Predictions.ToList();
    if (predictions.Count > 0)
    {
        foreach (var prediction in predictions)
        {
            Console.WriteLine(prediction.Description + " - " + prediction.PlaceId);            
        }
    }
}
```

### Google Directions ###

To request directions, first construct a ```DirectionsRequest``` object with the start and end destination info. You can specify coordinates, addresses, or placeIds for the origin and destination:

```csharp
var request = new DirectionsRequest()
    {
        OriginCoordinates = new Tuple<string, string>("-33.865143", "151.209900"),
        DestinationCoordinates = new Tuple<string, string>("-34.123143", "147.998900")
    };
```

Then invoke the service and retrieve the directions:
```csharp
var directionsService = new DirectionsService(GoogleDirectionsApiKey);

var result = await directionsService.RequestDirections(request);
if (result.IsSuccessful)
{
    var response = result.Result;        
    Console.WriteLine("Journey distance: " + response.GetJourneyDistanceInMeters() + "m");
    Console.WriteLine("Journey duration: " + response.GetJourneyDurationInSeconds() + "s");
    Console.WriteLine("Journey duration in traffic: " + response.GetJourneyDurationInSecondsInTraffic() + "s");

    var route = response.Routes.FirstOrDefault();
    var leg = route.Legs.FirstOrDefault();
    foreach (var step in legs.Steps)
    {
        Console.WriteLine(step.Instructions);
    }
}
```

### Google Roads ###

The roads service provides functionality to snap a set of points to fit the roads that
are the closest match to the points in order to create a navigation journey.

Simply provide the coordiantes of the original points and receive the snapped version of the points:

```csharp
var roadsService = new RoadsService(GoogleRoadsApiKey)

var positions = new List<Position>();
positions.Add(new Position(){Latitude=-33.86, Longitude=151.20});
positions.Add(new Position(){Latitude=-34.79, Longitude=151.23});
positions.Add(new Position(){Latitude=-34.80, Longitude=151.45});

var result = await roadsService.RequestSnappedLocations(positions, shouldInterpolate:false);

if (result.IsSuccessful)
{
    foreach (var point in result.Result.SnappedPoints)
    {
        Console.WriteLine("Lat: " + point.Location.Latitude + " Lng: " + point.Location.Longitude);
    }
}
```

## Installation ##

The library is available via NuGet [here](https://www.nuget.org/packages/ChilliSource.Mobile.Location).

## Releases ##

See the [releases](https://github.com/BlueChilli/ChilliSource.Mobile.Location/releases).

## Contribution ##

Please see the [Contribution Guide](.github/CONTRIBUTING.md).

## License ##

ChilliSource.Mobile is licensed under the [MIT license](LICENSE).

## Feedback and Contact ##

For questions or feedback, please contact [chillisource@bluechilli.com](mailto:chillisource@bluechilli.com).



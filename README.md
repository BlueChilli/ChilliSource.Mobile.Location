[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT) ![Built With C#](https://img.shields.io/badge/Built_with-C%23-green.svg)

# ChilliSource.Mobile.Logging #

This project is part of the ChilliSource framework developed by [BlueChilli](https://github.com/BlueChilli).

## Summary ##

This library provides a [Raygun](https://raygun.com/platform/crash-reporting) logging sink and [Serilog](https://serilog.net/) based logging extensions and helper classes to simplify the instantiation and usage of [Serilog](https://serilog.net/) logging functionality.

## Usage ##

**Initialization**

You need to first create a ```LoggerConfiguration``` instance and specify the type of logging sink that receives the logged data. Currently ```ChilliSource.Mobile.Logging``` only provides a [Raygun](https://raygun.com/platform/crash-reporting) sink, however you can add additional sinks if you wish.

```csharp
var config = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Raygun("RaygunAPIKey");
```

Then create the logger from the configuration:

```csharp
ILogger logger = config.BuildLogger();
```

**Logging**

You can now use the logger to capture errors, warnings, information, and debugging data:

```csharp
logger?.Error(ex, message);
logger?.Warning(message);
logger?.Information(message);
logger?.Debug(message);
```

**Logging within ChilliSource.Mobile**

The ```ChilliSource.Mobile``` frameworks perform their own logging using the ```ILogger``` interface (available in [ChilliSource.Mobile.Core](https://github.com/BlueChilli/ChilliSource.Mobile.Core)). 

In order to let the frameworks log their data, simply create a new logger as outlined above and pass it as a parameter to one of the relevant methods or constructors.

For example, to use Raygun logging for the ```BeaconService``` in ```ChilliSource.Mobile.IoT.Beacons``` framework, write the following code:

```csharp
var logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Raygun("RaygunAPIKey")
    .BuildLogger();

var beaconService = DependencyService.Get<IBeaconService>();    
beaconService.InitializeService(BeaconMonitoringType.RegionMonitoring, logger);
```

## Installation ##

The library is available via NuGet [here](https://www.nuget.org/packages/ChilliSource.Mobile.Logging).
 
## Releases ##

See the [releases](https://github.com/BlueChilli/ChilliSource.Mobile.Logging/releases).

## Contribution ##

Please see the [Contribution Guide](.github/CONTRIBUTING.md).

## License ##

ChilliSource.Mobile is licensed under the [MIT license](LICENSE).

## Feedback and Contact ##

For questions or feedback, please contact [chillisource@bluechilli.com](mailto:chillisource@bluechilli.com).


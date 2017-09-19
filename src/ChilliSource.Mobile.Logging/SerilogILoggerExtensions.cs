using System;
using Serilog;

namespace ChilliSource.Mobile.Logging
{
    /// <summary>
    /// Serilog ILogger extensions.
    /// </summary>
    public static class SerilogILoggerExtensions
    {
        /// <summary>
        /// Assigns the specified <paramref name="logger"/> instance as a main Serilog Logger
        /// </summary>
        /// <param name="logger">The <see cref="Serilog.ILogger"/> instance to register</param>
		public static void Register(this Serilog.ILogger logger)
		{
			Log.Logger = logger;
		}
	}
}

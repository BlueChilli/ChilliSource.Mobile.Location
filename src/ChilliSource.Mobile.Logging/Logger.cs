#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using Serilog;

namespace ChilliSource.Mobile.Logging
{    
    public static class Logger
    {
		/// <summary>
		/// Builds a new <see cref="LoggerConfiguration"/> instance
		/// </summary>
		/// <returns>The new <see cref="LoggerConfiguration"/> instance</returns>
		public static LoggerConfiguration Configure() =>
            new LoggerConfiguration()
                .MinimumLevel.Verbose();
    }
}

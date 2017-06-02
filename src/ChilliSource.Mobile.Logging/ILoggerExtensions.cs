#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Mobile.Core;
using Serilog;

namespace ChilliSource.Mobile.Logging
{

	/// <summary>
	/// Provides <see cref="Core.ILogger"/> extensions for handling error, warnining, information, and debugging data logging.
	/// </summary>
	public static class ILoggerExtensions
    {
#pragma warning disable 1591
		public static void Dispose(this Core.ILogger logger)
        {
            (logger as LoggerProxy)?.Dispose();
        }

        public static void Verbose(this Core.ILogger logger, string messageTemplate)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate);
        }

        public static void Verbose(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate);
        }

        public static void Verbose(this Core.ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Verbose(exception, messageTemplate, propertyValues);
        }

        public static void Debug(this Core.ILogger logger, string messageTemplate)
        {
            (logger as LoggerProxy)?.Debug(messageTemplate);
        }

        public static void Debug(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Debug(messageTemplate, propertyValues);
        }

        public static void Debug(this Core.ILogger logger, Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Debug(exception, messageTemplate);
        }

        public static void Debug(this Core.ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Debug(exception, messageTemplate, propertyValues);
        }

        public static void Information(this Core.ILogger logger, string messageTemplate)
        {
            (logger as LoggerProxy)?.Information(messageTemplate);
        }

        public static void Information(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Information(messageTemplate, propertyValues);
        }

        public static void Information(this Core.ILogger logger, Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Information(exception, messageTemplate);
        }

        public static void Information(this Core.ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Information(exception, messageTemplate, propertyValues);
        }

        public static void Warning(this Core.ILogger logger, string messageTemplate)
        {
            (logger as LoggerProxy)?.Warning(messageTemplate);
        }

        public static void Warning(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Warning(messageTemplate, propertyValues);
        }

        public static void Warning(this Core.ILogger logger,Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Warning(exception, messageTemplate);
        }

        public static void Warning(this Core.ILogger logger,Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Warning(exception, messageTemplate, propertyValues);
        }

        public static void Error(this Core.ILogger logger,string messageTemplate)
        {
            (logger as LoggerProxy)?.Error(messageTemplate);
        }

        public static void Error(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Error(messageTemplate, propertyValues);
        }

        public static void Error(this Core.ILogger logger, Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Error(exception, messageTemplate);
        }

        public static void Error(this Core.ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Error(exception, messageTemplate, propertyValues);
        }

        public static void Fatal(this Core.ILogger logger, string messageTemplate)
        {
            (logger as LoggerProxy)?.Fatal(messageTemplate);
        }

        public static void Fatal(this Core.ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Fatal(messageTemplate, propertyValues);
        }

        public static void Fatal(this Core.ILogger logger, Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Fatal(exception, messageTemplate);
        }

        public static void Fatal(this Core.ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            (logger as LoggerProxy)?.Fatal(exception, messageTemplate, propertyValues);
        }

        public static Core.ILogger ForContext<TSource>(this Core.ILogger logger)
        {
            return (logger as LoggerProxy)?.ForContext<TSource>();
        }

        public static Core.ILogger ForContext(this Core.ILogger logger, Type source)
        {
            return (logger as LoggerProxy)?.ForContext(source);
        }

        public static bool IsEnabled(this Core.ILogger logger, Serilog.Events.LogEventLevel level)
        {
            var r = (logger as LoggerProxy)?.IsEnabled(level);
            return r ?? false;
        }

        public static void Verbose<T>(this Core.ILogger logger, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate, propertyValue);
        }

        public static void Verbose<T0, T1>(this Core.ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Verbose<T0, T1, T2>(this Core.ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue1, propertyValue2);
        }

        public static void Verbose(this Core.ILogger logger,Exception exception, string messageTemplate)
        {
            (logger as LoggerProxy)?.Verbose(messageTemplate, messageTemplate);
        }

        public static void Verbose<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Verbose(exception, messageTemplate, propertyValue);
        }

        public static  void Verbose<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Verbose<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Debug<T>(this Core.ILogger logger,string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Debug(messageTemplate, propertyValue);
        }

        public static void Debug<T0, T1>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Debug<T0, T1, T2>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Debug<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Debug(exception, messageTemplate, propertyValue);
        }

        public static void Debug<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Debug<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Information<T>(this Core.ILogger logger,string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Information(messageTemplate, propertyValue);
        }

        public static void Information<T0, T1>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Information<T0, T1, T2>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Information<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Information(exception, messageTemplate, propertyValue);
        }

        public static void Information<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Information<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Warning<T>(this Core.ILogger logger,string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Warning(messageTemplate, propertyValue);
        }

        public static void Warning<T0, T1>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Warning(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Warning<T0, T1, T2>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Warning<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Warning(exception, messageTemplate, propertyValue);
        }

        public static void Warning<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Warning(exception, messageTemplate, propertyValue0, propertyValue1);

        }

        public static void Warning<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Error<T>(this Core.ILogger logger,string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Error(messageTemplate, propertyValue);
        }

        public static void Error<T0, T1>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Error<T0, T1, T2>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Error<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Error(exception, messageTemplate, propertyValue);

        }

        public static void Error<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Error<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Fatal<T>(this Core.ILogger logger,string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Fatal(messageTemplate, propertyValue);
        }

        public static void Fatal<T0, T1>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Fatal<T0, T1, T2>(this Core.ILogger logger,string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Fatal<T>(this Core.ILogger logger,Exception exception, string messageTemplate, T propertyValue)
        {
            (logger as LoggerProxy)?.Fatal(exception, messageTemplate, propertyValue);
        }

        public static void Fatal<T0, T1>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            (logger as LoggerProxy)?.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Fatal<T0, T1, T2>(this Core.ILogger logger,Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
            (logger as LoggerProxy)?.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Error(this Core.ILogger logger,Exception exception)
        {
            Error(logger,exception, String.Empty);
        }
#pragma warning restore 1591
	}
}

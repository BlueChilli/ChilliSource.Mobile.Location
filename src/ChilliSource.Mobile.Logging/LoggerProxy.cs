#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

/* Based on
Source:     Serilog (https://github.com/serilog/serilog)
Author:     Serilog (https://github.com/serilog)
License:    Apache License Version 2.0 (https://github.com/serilog/serilog/blob/dev/LICENSE)
*/

#pragma warning disable 1591

using System;
namespace ChilliSource.Mobile.Logging
{	

	internal class LoggerProxy : Core.ILogger, IDisposable
    {
        readonly Serilog.ILogger _logger;

        public LoggerProxy()
        {
             _logger = Serilog.Log.Logger;
        }

        public LoggerProxy(Serilog.ILogger logger)
        {
             _logger = logger;
        }

		public Core.ILogger ForContext<TSource>()
		{
			return new LoggerProxy(_logger?.ForContext<TSource>());
		}

		public Core.ILogger ForContext(Type source)
		{
			return new LoggerProxy(_logger?.ForContext(source));
		}


		public void Verbose(string messageTemplate)
        {
             _logger?.Verbose(messageTemplate);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate)
        {
             _logger?.Debug(messageTemplate);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
             _logger?.Debug(exception, messageTemplate);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Debug(exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate)
        {
             _logger?.Information(messageTemplate);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate)
        {
             _logger?.Information(exception, messageTemplate);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Information(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate)
        {
             _logger?.Warning(messageTemplate);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate)
        {
             _logger?.Warning(exception, messageTemplate);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Warning(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate)
        {
             _logger?.Error(messageTemplate);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate)
        {
             _logger?.Error(exception, messageTemplate);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate)
        {
             _logger?.Fatal(messageTemplate);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
             _logger?.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
             _logger?.Fatal(exception, messageTemplate);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
             _logger?.Fatal(exception, messageTemplate, propertyValues);
        }
               
        public bool IsEnabled(Serilog.Events.LogEventLevel level)
        {
            var r = _logger?.IsEnabled(level);
            return r ?? false;
        }

        public void Verbose<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Verbose(messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue1, propertyValue2);
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
             _logger?.Verbose(messageTemplate, messageTemplate);
        }

        public void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Verbose(exception, messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Information(messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Warning(messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Warning(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Warning(exception, messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Warning(exception, messageTemplate, propertyValue0, propertyValue1);

        }

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Error(messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Error(exception, messageTemplate, propertyValue);

        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal<T>(string messageTemplate, T propertyValue)
        {
             _logger?.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
             _logger?.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
        {
             _logger?.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
             _logger?.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2)
        {
             _logger?.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Dispose()
        {
            ( _logger as IDisposable)?.Dispose();
        }

        public void Error(Exception exception)
        {
            this.Error(exception, String.Empty);
        }
	}
}

#pragma warning restore 1591

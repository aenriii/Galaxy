using System;
using Microsoft.Extensions.Logging;

namespace Galaxy.Tcp.Util
{
    public class Logger
    {
        LogLevel minimumLogLevel;
        public Logger(LogLevel minimumLogLevel)
        {
            this.minimumLogLevel = minimumLogLevel;
        }

        public Logger()
        {
            this.minimumLogLevel = LogLevel.Information;
        }
        public void Info(string message)
        {
            Log(LogLevel.Information, message);
        }
        
        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }
        public void Warn(string message)
        {
            Log(LogLevel.Warning, message);
        }
        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }
        public void Log(LogLevel logLevel, string message)
        {
            // if (logLevel >= minimumLogLevel)
            // {
                Console.WriteLine(message);
            // }
        }
    }
}
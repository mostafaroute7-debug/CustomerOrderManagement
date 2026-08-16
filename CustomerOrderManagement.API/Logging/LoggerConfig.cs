using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CustomerOrderManagement.API.Logging
{
    public static class LoggerConfig
    {
        public static void Configure()
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Logs");

            Directory.CreateDirectory(logDirectory);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()

                .WriteTo.File(
                    Path.Combine(logDirectory, "app-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true,
                    outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                        "[{Level:u3}] " +
                        "{Message:lj}{NewLine}" +
                        "{Exception}{NewLine}")

                .CreateLogger();
        }
    }
}
﻿using Serilog;

namespace Soloco.RealTimeWeb.Common.Infrastructure
{
    public static class LoggingInitializer
    {
        public static void Initialize()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }
    }
}
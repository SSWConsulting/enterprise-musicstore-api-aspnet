using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace SSW.MusicStore.Data.Test.Int.Setup
{
    public class Logging
    {

        public static void SerilogConfiguration()
        {
            var config =
                new LoggerConfiguration()
                    .WriteTo.ColoredConsole();
                   
            Log.Logger = config.CreateLogger();
        }

    }
}

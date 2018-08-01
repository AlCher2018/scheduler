using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using AppsShdl.Configuration;
using AppsShdl.Servers;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

namespace AppsShdl
{
    class Program
    {

        static void Main(string[] args)
        {
            Startup.Init(args);

            ILogger logger = Startup.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<NLogLoggerProvider>();
            logger.LogTrace(" *** Starting sheduler (my first .Net Core project) ***");

            var cmd = Startup.ServiceProvider.GetService<IOptions<CmdConfig>>().Value;
            if (cmd.Help)
            {
                usage(); return;
            }

            /*
             shutdown handlers with thread events
             */

            IRunSheduler shedulersServer = null;
            try
            {
                shedulersServer = Startup.ServiceProvider.GetService<IRunSheduler>();
                shedulersServer.Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Something failed");
            }

            Console.WriteLine("Press a key...");
            Console.ReadKey();
            logger.LogTrace(" *** FINISH ***");
        }

        private static void cb(object state)
        {
            Console.WriteLine("timer - " + DateTime.Now.ToString());
        }

        private static void testJSONConf()
        {
            IConfigurationRoot confRoot = new ConfigurationBuilder()
                .AddJsonFile("ycSheduler.conf")
                .Build();

            var ttt = confRoot.AsEnumerable();
            WorkersConf wc = confRoot.Get<WorkersConf>();
        }


        public static Dictionary<string, string> GetSwitchMappings(
        IReadOnlyDictionary<string, string> configurationStrings)
        {
            return configurationStrings.Select(item =>
                new KeyValuePair<string, string>(
                    "-" + item.Key.Substring(item.Key.LastIndexOf(':') + 1),
                    item.Key))
                    .ToDictionary(
                        item => item.Key, item => item.Value);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Unknown exception: " + e.ToString());
            if (e.IsTerminating) Environment.Exit(-1);
        }

        private static void usage()
        {
            string usageText = @"
Шедулер для запуска приложений (Application Sheduler).
Usage:
    appsShdl...

Examples:

";
            Console.WriteLine(usageText);
           
        }

    }

}

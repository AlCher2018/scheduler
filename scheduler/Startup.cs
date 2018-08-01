using AppsShdl.Configuration;
using AppsShdl.Servers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsShdl
{
    internal static class Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfigurationRoot AppConfiguration { get; private set; }

        public static void Init(string[] args)
        {
            AppConfiguration = InitConfig(args);

            // app services
            // add to services logger (NLog) and Conf_to_Class feature (by AddOptions)
            var services = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(new NLogLoggerFactory())
                .AddOptions()
                .Configure<CmdConfig>(AppConfiguration.GetSection("Cmd"))
                .Configure<WorkersConf>(AppConfiguration)
                ;
            // add app services
            services.AddSingleton<IRunSheduler, RunSheduler>()
                    .AddSingleton<IShedulerFactory, ShedulerFactory>();
            ServiceProvider = services.BuildServiceProvider();

            // create and configure logger
            InitLog();
        }

        private static IConfigurationRoot InitConfig(string[] args)
        {
            args = args.Select(arg => arg.IndexOf('=') < 0 ? string.Concat(arg, "=true") : arg).ToArray();

            string systemConfigDir = "/etc";
            string configFileName = "ycSheduler.conf";
            string configDir = File.Exists($"{systemConfigDir}/{configFileName}") ? systemConfigDir : Directory.GetCurrentDirectory();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(configDir)
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
                .AddCommandLine(args, new Dictionary<string, string>{
                     {"--help", "Cmd:Help"}
                    ,{"-h", "Cmd:Help"}
                    ,{"--nodaemon", "Cmd:Nodaemon"}
                    ,{"-d", "Cmd:Nodaemon"}
                });
            return builder.Build();
        }

        #region Logger funcs

        public static void InitLog()
        {
            LogManager.LoadConfiguration("NLog.config");
        }

        public static void ReinitLogger()
        {
            ShutdownLogger();
            InitLog();
        }

        public static void ShutdownLogger()
        {
            LogManager.Flush();
            LogManager.Shutdown();
        }

        #endregion
    }


}

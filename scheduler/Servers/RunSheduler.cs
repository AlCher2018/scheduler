using AppsShdl.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsShdl.Servers
{
    internal class RunSheduler : IRunSheduler
    {
        private ILogger _logger;
        private CmdConfig _cmdConfig;
        private List<WorkerConf> _workers;
        private IShedulerFactory _shedulerFactory;

        private List<ISheduler> _shedulers;


        // CTOR
        public RunSheduler(ILoggerFactory loggerFactory 
            ,IOptions<CmdConfig> confCmd
            ,IOptions<WorkersConf> confWorkers
            ,IShedulerFactory shedulerFactory)
        {
            _logger = loggerFactory.CreateLogger<NLogLoggerProvider>();
            _cmdConfig = confCmd.Value;
            _workers = confWorkers.Value.Workers;
            _shedulerFactory = shedulerFactory;
        }

        public void Run()
        {
            // create sheduler for each worker, i.e. convert _workers to _shedulers
            _shedulers = _workers.Select(w => _shedulerFactory.CreateSheduler(_logger, w)).ToList();
        }

        public void Stop()
        {

        }

    }
}

using AppsShdl.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Servers
{
    internal interface IShedulerFactory
    {
        ISheduler CreateSheduler(ILogger logger, WorkerConf workerConf);
    }
}

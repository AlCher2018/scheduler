using AppsShdl.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Servers
{
    internal class ShedulerFactory : IShedulerFactory
    {
        public ISheduler CreateSheduler(ILogger logger, WorkerConf workerConf)
        {
            if (workerConf.Shedule.StartTimes.Count > 0)
            {
                return new ShedulerOnTime(logger, workerConf);
            }
            else if (workerConf.Shedule.RepeatTimeSpanMinutes > 0)
            {
                return new ShedulerSequential(logger, workerConf);
            }
            else
                return new ShedulerBase(logger, workerConf);
        }
    }
}

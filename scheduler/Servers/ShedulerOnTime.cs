using AppsShdl.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppsShdl.Servers
{
    internal class ShedulerOnTime : ShedulerBase
    {
        public ShedulerOnTime(ILogger logger, WorkerConf workerConf): base(logger, workerConf)
        {
            // each second check start time
            createTimer(timerCallback, 1000);
        }

        private void timerCallback(object state)
        {
            if (_workerConf.Shedule.StartTimes.Any(i =>
            {
                double sec = (DateTime.Now - (DateTime.Today + i.StartTime)).TotalSeconds;
                return (sec >= 0d) && (sec <= 5d);
            }))
            {
                stopTimer();

                RunTask();

                startTimer();
            }
        }

    }
}

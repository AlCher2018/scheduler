using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AppsShdl.Configuration;
using Microsoft.Extensions.Logging;

namespace AppsShdl.Servers
{
    internal class ShedulerSequential : ShedulerBase
    {
        public ShedulerSequential(ILogger logger, WorkerConf workerConf) : base(logger, workerConf)
        {
            createTimer(timerCallback, _workerConf.Shedule.RepeatTimeSpanMinutes * 60 * 1000);
        }

        private void timerCallback(object state)
        {
            stopTimer();

            RunTask();

            startTimer();
        }
    }
}

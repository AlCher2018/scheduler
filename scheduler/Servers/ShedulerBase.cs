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
    internal class ShedulerBase : ISheduler
    {
        protected CancellationTokenSource _cancelToken;
        protected ILogger _logger;
        protected WorkerConf _workerConf;

        protected Timer _timer;


        public ShedulerBase(ILogger logger, WorkerConf workerConf)
        {
            _logger = logger;
            _workerConf = workerConf;
            _cancelToken = new CancellationTokenSource();

            // some checks
            if (_logger == null) throw new Exception("Module 'ShedulerOnTime': constructor not get logger object.");
            if (_workerConf == null)
            {
                _logger.LogWarning($"Not pass '{nameof(WorkerConf)}' object to module 'ShedulerOnTime'");
                return;
            }

            if (_workerConf.Shedule.IsStartAfterAppRun) { RunTask(); }
        }

        #region timer
        private int _period;
        protected void createTimer(TimerCallback timerCallback, int period)
        {
            _period = period;
            _timer = new Timer(timerCallback, null, _period, _period);
        }
        protected void stopTimer()
        {
            if (_timer != null) _timer.Change(Timeout.Infinite, 0);
        }
        protected void startTimer()
        {
            if (_timer != null) _timer.Change(_period, _period);
        }
        #endregion

        public virtual void Dispose()
        {
            _timer.Dispose();
        }

        public async void RunTask()
        {
            bool isSuccess = await RunTaskAsync();

            // auto restart task after error
            if (!isSuccess
                && (_workerConf.Shedule.ErrorAutoStartDelaySec > 0)
                && (_workerConf.Shedule.ErrorAutoStartCount > 0))
            {
                stopTimer();

                int counter = 1;
                while (!isSuccess && (counter <= _workerConf.Shedule.ErrorAutoStartCount))
                {
                    _logger.LogInformation($"Restart worker '{_workerConf.Name}' after {_workerConf.Shedule.ErrorAutoStartDelaySec} seconds. Try {counter}");
                    Thread.Sleep(_workerConf.Shedule.ErrorAutoStartDelaySec * 1000);
                    isSuccess = await RunTaskAsync();
                    counter++;
                }

                startTimer();
            }
        }

        private async Task<bool> RunTaskAsync()
        {
            bool isSuccess = false;
            try
            {
                _logger.LogInformation($"Start worker '{_workerConf.Name}'...");

                // check file exists
                bool isExists = await Task.Run(() => System.IO.File.Exists(_workerConf.FilePath));
                if (!isExists)
                {
                    _logger.LogWarning($"Worker '{_workerConf.Name}' hasn't it's file '{_workerConf.FilePath}'");
                    return false;
                }

                // load assembly
                Assembly workerAssembly = null;
                // TODO load worker assembly
                _logger.LogInformation($"Load worker '{_workerConf.Name}' assembly '{_workerConf.FilePath}'...");
                // not working yet
                //                    workerAssembly = Assembly.LoadFile(_workerConf.FilePath);
                // exception here
                //                    Type[] types = workerAssembly.GetTypes();
                Thread.Sleep(500);
                _logger.LogInformation($" - Load worker '{_workerConf.Name}' - Success");

                // TODO start Run method
                Thread.Sleep(6000);

                _logger.LogInformation($"Finish worker '{_workerConf.Name}' successful.");
                isSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Finish worker '{_workerConf.Name}' with ERROR: {ex.Message}");
            }

            return isSuccess;
        }



    }
}

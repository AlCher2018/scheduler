using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Configuration
{
    internal class SheduleConf
    {
        public bool IsStartAfterAppRun { get; set; }

        public int ErrorAutoStartCount { get; set; }

        public int ErrorAutoStartDelaySec { get; set; }

        public int RepeatTimeSpanMinutes { get; set; }

        public List<SheduleStartTime> StartTimes { get; private set; }

        public SheduleConf()
        {
            this.StartTimes = new List<SheduleStartTime>();
        }
    }

    internal class SheduleStartTime
    {
        public TimeSpan StartTime { get; set; }
    }

}

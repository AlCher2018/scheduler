using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Configuration
{
    internal class WorkerConf
    {
        public string Name { get; set; }

        public string FilePath { get; set; }

        public SheduleConf Shedule { get; set; }
    }
}

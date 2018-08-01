using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Configuration
{
    internal sealed class CmdConfig
    {
        public bool Help { get; set; }

        public bool Nodaemon { get; set; }

        public string InitDir { get; set; }
    }
}

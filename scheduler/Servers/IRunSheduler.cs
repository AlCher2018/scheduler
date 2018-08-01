using System;
using System.Collections.Generic;
using System.Text;

namespace AppsShdl.Servers
{
    interface IRunSheduler
    {
        void Run();

        void Stop();
    }
}

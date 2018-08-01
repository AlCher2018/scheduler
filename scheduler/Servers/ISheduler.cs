using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppsShdl.Servers
{
    internal interface ISheduler: IDisposable
    {
        void RunTask();
    }
}

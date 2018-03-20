using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    interface IHardwareMonitorService
    {

        void UpdateCPUUsage(double nextValue);

        void UpdateRAMUsage(double nextValue);

        void UpdateCPUTemp(double nextValue);

    }
}

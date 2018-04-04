using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    interface IHardwareMonitorService
    {

        void SaveHardwareData(double nextCpuValue, double nextRamValue, double nextTempValue);

    }
}

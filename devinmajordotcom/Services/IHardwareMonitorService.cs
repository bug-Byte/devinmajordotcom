using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    interface IHardwareMonitorService
    {

        void SaveHardwareData(List<string> cpuList, string ramString, List<double> nextCpuValues, double nextRamValue, List<double> nextTempValues);

    }
}

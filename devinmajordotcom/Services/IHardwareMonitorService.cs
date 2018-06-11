using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface IHardwareMonitorService
    {

        void SaveHardwareData(List<string> cpuList, string ramString, List<double> nextCpuValues, double nextRamValue, List<double> nextTempValues);

        List<double> GetRAMHistory(string type = "RAM Usage");

        List<object> GetCPULoadHistory(string type = "CPU 1 Usage");

        List<object> GetCPUTempHistory(string type = "CPU 1 Temp");

        GraphDataViewModel GetServerData(int typeID, int dateRangeID);

    }
}

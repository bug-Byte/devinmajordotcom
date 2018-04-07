using System.Collections.Generic;
using System.Linq;
using devinmajordotcom.Models;

namespace devinmajordotcom.Services
{
    public class HardwareMonitorService : IHardwareMonitorService
    {

        protected dbContext db;
        public HardwareMonitorService()
        {
            db = new dbContext();
        }

        public void SaveHardwareData(List<string> cpuList, string ramString, List<double> nextCpuValues, double nextRamValue, List<double> nextTempValues)
        {
            var itemsToAdd = new List<Security_HardwarePerformance>()
            {
                new Security_HardwarePerformance()
                {
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.RAMUsage,
                    HardwareName = ramString,
                    PercentageValue = nextRamValue
                }
            };
            for(var x = 0; x < cpuList.Count; x++)
            {
                var newUsageItem = new Security_HardwarePerformance()
                {
                    HardwareName = cpuList[x],
                    HardwareNumber = x,
                    PercentageValue = nextCpuValues[x],
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage
                };
                var newTempItem = new Security_HardwarePerformance()
                {
                    HardwareName = cpuList[x],
                    HardwareNumber = x,
                    PercentageValue = nextTempValues[x],
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUTemp
                };
                itemsToAdd.Add(newUsageItem);
                itemsToAdd.Add(newTempItem);
            }
            db.Security_HardwarePerformances.AddRange(itemsToAdd);
            db.SaveChanges();
        }
        
    }
}
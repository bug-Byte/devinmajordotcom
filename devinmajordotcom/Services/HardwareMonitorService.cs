using System.Collections.Generic;
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

        public void SaveHardwareData(double nextCpuValue, double nextRamValue, double nextTempValue)
        {
            var itemsToAdd = new List<Security_HardwarePerformance>()
            {
                new Security_HardwarePerformance()
                {
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage,
                    PercentageValue = nextCpuValue
                },
                new Security_HardwarePerformance()
                {
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.RAMUsage,
                    PercentageValue = nextRamValue
                },
                new Security_HardwarePerformance()
                {
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUTemp,
                    PercentageValue = nextTempValue
                }
            };
            db.Security_HardwarePerformances.AddRange(itemsToAdd);
            db.SaveChanges();
        }
        
    }
}
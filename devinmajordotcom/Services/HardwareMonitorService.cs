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

        public void SaveHardwareData(List<double> nextCpuValues, double nextRamValue, List<double> nextTempValues)
        {
            var itemsToAdd = new List<Security_HardwarePerformance>()
            {
                new Security_HardwarePerformance()
                {
                    HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.RAMUsage,
                    PercentageValue = nextRamValue
                }
            };
            itemsToAdd.AddRange(nextCpuValues.Select(nextValue => new Security_HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage,
                PercentageValue = nextValue
            }));
            itemsToAdd.AddRange(nextTempValues.Select(nextValue => new Security_HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUTemp,
                PercentageValue = nextValue
            }));
            db.Security_HardwarePerformances.AddRange(itemsToAdd);
            db.SaveChanges();
        }
        
    }
}
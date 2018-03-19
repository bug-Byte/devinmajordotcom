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

        public void UpdateCPUUsage(float nextValue)
        {
            var newHardwareDataRecord = new Security_HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage,
                PercentageValue = nextValue
            };
            db.Security_HardwarePerformances.Add(newHardwareDataRecord);
            db.SaveChanges();
        }

        public void UpdateRAMUsage(double nextValue)
        {
            var newHardwareDataRecord = new Security_HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.RAMUsage,
                PercentageValue = nextValue
            };
            db.Security_HardwarePerformances.Add(newHardwareDataRecord);
            db.SaveChanges();
        }

    }
}
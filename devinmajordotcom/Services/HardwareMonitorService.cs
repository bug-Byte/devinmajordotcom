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
            var newHardwareDataRecord = new HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage,
                PercentageValue = nextValue
            };
            db.HardwarePerformances.Add(newHardwareDataRecord);
            db.SaveChanges();
        }

        public void UpdateRAMUsage(double nextValue)
        {
            var newHardwareDataRecord = new HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.RAMUsage,
                PercentageValue = nextValue
            };
            db.HardwarePerformances.Add(newHardwareDataRecord);
            db.SaveChanges();
        }

    }
}
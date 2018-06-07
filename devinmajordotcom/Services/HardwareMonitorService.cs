using System.Collections.Generic;
using System.Linq;
using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;

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
            for (var x = 0; x < cpuList.Count; x++)
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

        public ServerDataViewModel GetServerData(string type = null, string range = null)
        {
            var types = db.Security_HardwareTypes.ToList();
            var viewModel = new ServerDataViewModel()
            {
                GraphList = new List<GraphViewModel>()
            };
            if (type == null && range == null)
            {
                foreach (var hardwareType in types)
                {
                    var newGraph = new GraphViewModel()
                    {
                        Labels = new List<string>(),
                        Values = new List<double>()
                    };
                    for (var i = 60; i > 0; i--)
                    {
                        newGraph.Labels.Add(i.ToString());
                    }
                    var timeSpan = DateTime.Now.AddMinutes(-1);
                    var values = db.Security_HardwarePerformances.Where(x => x.HardwareTypeId == hardwareType.Id).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => x.PercentageValue).ToList();
                    newGraph.Values.AddRange(values);
                    viewModel.GraphList.Add(newGraph);
                }
            }
            return viewModel;
        }

        public List<double> GetRAMHistory(string type = "RAM Usage")
        {
            var values = new List<double>();
            var types = db.Security_HardwareTypes.ToList();
            var typeToReturn = types.FirstOrDefault(x => x.Name.Contains(type));
            if (typeToReturn != null)
            {
                values = db.Security_HardwarePerformances.Where(x => x.HardwareTypeId == typeToReturn.Id).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => Math.Round(x.PercentageValue, 2)).ToList();
            }
            return values;
        }

        public List<object> GetCPULoadHistory(string type = "CPU Usage")
        {
            var cpuLoads = new List<object>();
            var types = db.Security_HardwareTypes.ToList();
            var typeToReturn = types.FirstOrDefault(x => x.Name.Contains(type));
            if (typeToReturn != null)
            {
                var cpus = db.Security_HardwarePerformances.Where(x => x.HardwareNumber != null).Select(x => x.HardwareNumber).Distinct().ToList();
                cpuLoads.AddRange(cpus.Select(cpu => db.Security_HardwarePerformances.Where(x => x.HardwareTypeId == typeToReturn.Id && x.HardwareNumber == cpu).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => Math.Round(x.PercentageValue, 2)).ToList()));
            }
            return cpuLoads;
        }

        public List<object> GetCPUTempHistory(string type = "CPU Temp")
        {
            var cpuTemps = new List<object>();
            var types = db.Security_HardwareTypes.ToList();
            var typeToReturn = types.FirstOrDefault(x => x.Name.Contains(type));
            if (typeToReturn != null)
            {
                var cpus = db.Security_HardwarePerformances.Where(x => x.HardwareNumber != null).Select(x => x.HardwareNumber).Distinct().ToList();
                cpuTemps.AddRange(cpus.Select(cpu => db.Security_HardwarePerformances.Where(x => x.HardwareTypeId == typeToReturn.Id && x.HardwareNumber == cpu).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => Math.Round(x.PercentageValue, 2)).ToList()));
            }
            return cpuTemps;
        }

    }
}
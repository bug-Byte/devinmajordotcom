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
                var xPlusOne = (x + 1);
                var tempTypeId = db.Security_HardwareTypes.Where(o => o.Name.Contains(xPlusOne.ToString() + " Temp")).Select(o => o.Id).FirstOrDefault();
                var loadTypeId = db.Security_HardwareTypes.Where(o => o.Name.Contains(xPlusOne.ToString() + " Usage")).Select(o => o.Id).FirstOrDefault();

                if (tempTypeId == 0)
                {
                    var item = new Security_HardwareType()
                    {
                        Name = "CPU " + xPlusOne + " Temp"
                    };
                    db.Security_HardwareTypes.Add(item);
                    db.SaveChanges();
                }
                if (tempTypeId == 0)
                {
                    var item = new Security_HardwareType()
                    {
                        Name = "CPU " + xPlusOne + " Usage"
                    };
                    db.Security_HardwareTypes.Add(item);
                    db.SaveChanges();
                }

                var newUsageItem = new Security_HardwarePerformance()
                {
                    HardwareName = cpuList[x],
                    HardwareNumber = x,
                    PercentageValue = nextCpuValues[x],
                    HardwareTypeId = loadTypeId
                };
                var newTempItem = new Security_HardwarePerformance()
                {
                    HardwareName = cpuList[x],
                    HardwareNumber = x,
                    PercentageValue = nextTempValues[x],
                    HardwareTypeId = tempTypeId
                };
                itemsToAdd.Add(newUsageItem);
                itemsToAdd.Add(newTempItem);
            }
            db.Security_HardwarePerformances.AddRange(itemsToAdd);
            db.SaveChanges();
        }

        public GraphDataViewModel GetServerData(int typeID, int dateRangeID)
        {
            var dateRange = db.Security_DateRanges.FirstOrDefault(x => x.Id == dateRangeID);
            var type = db.Security_HardwareTypes.FirstOrDefault(x => x.Id == typeID);
            var results = new GraphDataViewModel()
            {
                Labels = new List<string>(),
                Values = new List<double>()
            };
            var today = DateTime.Now;

            if (dateRange == null)
            {
                return results;
            }

            if (dateRange.Name.Contains("Seconds"))
            {
                for (var i = 1; i <= 60; i++)
                {
                    results.Labels.Add(i.ToString());
                }
                results.Values.AddRange(db.Security_HardwarePerformances.Where(x => x.HardwareTypeId == type.Id).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => Math.Round(x.PercentageValue, 2)).ToList());
            }
            else if (dateRange.Name.Contains("Minutes"))
            {
                for (var i = 0; i > -60; i--)
                {
                    var max = today.AddMinutes(i);
                    var iMinus1 = i - 1;
                    var min = today.AddMinutes(iMinus1);
                    results.Labels.Add(iMinus1.ToString().Replace("-",""));
                    var thisMinutesData = db.Security_HardwarePerformances.Where(x => 
                        x.CreatedOn <= max && x.CreatedOn >= min && x.HardwareTypeId == type.Id
                    ).OrderByDescending(x => x.CreatedOn).Take(60).Select(x => Math.Round(x.PercentageValue, 2)).ToList();
                    if (thisMinutesData.Count > 0)
                    {
                        var thisMinutesAverage = thisMinutesData.Average();
                        results.Values.Add(Math.Round(thisMinutesAverage,2));
                    }
                }
            }
            else if (dateRange.Name.Contains("Hours"))
            {
                for (var i = 0; i > -24; i--)
                {
                    var max = today.AddHours(i);
                    var min = today.AddHours(i - 1);
                    results.Labels.Add(max.ToString("H:mm tt"));
                    var thisHoursData = db.Security_HardwarePerformances.Where(x =>
                        x.CreatedOn <= max && x.CreatedOn >= min && x.HardwareTypeId == type.Id
                    ).OrderByDescending(x => x.CreatedOn).Take(3600).Select(x => Math.Round(x.PercentageValue, 2)).ToList();
                    if (thisHoursData.Count > 0)
                    {
                        var thisHoursAverage = thisHoursData.Average();
                        results.Values.Add(Math.Round(thisHoursAverage, 2));
                    }
                }
            }
            else if (dateRange.Name.Contains("Days"))
            {
                if (dateRange.Name.Contains("7"))
                {
                    for (var i = 0; i > -7; i--)
                    {
                        var max = today.AddDays(i);
                        var min = today.AddDays(i - 1);
                        results.Labels.Add(max.ToString("dddd"));
                        var thisDaysData = db.Security_HardwarePerformances.Where(x =>
                            x.CreatedOn <= max && x.CreatedOn >= min && x.HardwareTypeId == type.Id
                        ).OrderByDescending(x => x.CreatedOn).Take(86400).Select(x => Math.Round(x.PercentageValue, 2)).ToList();
                        if (thisDaysData.Count > 0)
                        {
                            var thisDaysAverage = thisDaysData.Average();
                            results.Values.Add(Math.Round(thisDaysAverage, 2));
                        }
                    }
                }
                else if (dateRange.Name.Contains("30"))
                {
                    for (var i = 0; i > -30; i--)
                    {
                        var max = today.AddDays(i);
                        var min = today.AddDays(i - 1);
                        results.Labels.Add(max.ToString("MMMM dd"));
                        var thisDaysData = db.Security_HardwarePerformances.Where(x =>
                            x.CreatedOn <= max && x.CreatedOn >= min && x.HardwareTypeId == type.Id
                        ).OrderByDescending(x => x.CreatedOn).Take(86400).Select(x => Math.Round(x.PercentageValue, 2)).ToList();
                        if (thisDaysData.Count > 0)
                        {
                            var thisDaysAverage = thisDaysData.Average();
                            results.Values.Add(Math.Round(thisDaysAverage, 2));
                        }
                    }
                }
            }

            return results;

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

        public List<object> GetCPULoadHistory(string type = "CPU 1 Usage")
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

        public List<object> GetCPUTempHistory(string type = "CPU 1 Temp")
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
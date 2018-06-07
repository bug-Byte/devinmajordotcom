using devinmajordotcom.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Web;

namespace devinmajordotcom
{

    public class PerformanceHistoryHub : Hub
    {

        [HubMethodName("SendPerformanceHistory")]
        public void SendPerformanceHistory()
        {

            HardwareMonitorService service = new HardwareMonitorService();
            var drives = DriveInfo.GetDrives();
            int i = 0;

            Computer computer = new Computer()
            {
                CPUEnabled = true,
                FanControllerEnabled = true,
                GPUEnabled = true,
                HDDEnabled = true,
                MainboardEnabled = true,
                RAMEnabled = true
            };

            bool hasPackageTemp = false;
            var ramLoad = new List<double>();
            var ramString = "";
            var cpuTemp = new List<double>();
            var cpuLoad = new List<double>();
            var cpuTempHistory = new List<object>();
            var cpuLoadHistory = new List<object>();
            var diskList = new List<string>();
            var cpuList = new List<string>();

            while (i != -1)
            {

                computer.Open();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var diskInfo = drive.Name + "," + drive.VolumeLabel + "," + drive.DriveType + "," + drive.DriveFormat + "," + drive.TotalFreeSpace + "," + drive.TotalSize;
                        diskList.Add(diskInfo);
                    }
                }

                foreach (var t2 in computer.Hardware)
                {
                    if (t2.HardwareType == HardwareType.CPU)
                    {
                        cpuList.Add(t2.Name);
                        hasPackageTemp = t2.Sensors.Any(x => x.SensorType == SensorType.Temperature && x.Name == "CPU Package");
                        foreach (var t1 in t2.Sensors)
                        {
                            if (t1.SensorType == SensorType.Temperature && ((hasPackageTemp && t1.Name.Contains("Package")) || t1.Name == "CPU Core #1"))
                            {
                                cpuTemp.Add(t1.Value.GetValueOrDefault());
                            }
                            if (t1.SensorType == SensorType.Load && t1.Name == "CPU Total")
                            {
                                cpuLoad.Add(t1.Value.GetValueOrDefault());
                            }
                        }
                    }

                    if (t2.HardwareType != HardwareType.RAM) continue;

                    foreach (var t1 in t2.Sensors)
                    {
                        if (t1.SensorType == SensorType.Load)
                        {
                            ramLoad.Add(t1.Value.GetValueOrDefault());
                        }
                        if (t1.SensorType != SensorType.Data || t1.Name != "Used Memory") continue;
                        var available = t2.Sensors.FirstOrDefault(x => x.SensorType == SensorType.Data && x.Name == "Available Memory");
                        if (available == null) continue;
                        var used = t1.Value.GetValueOrDefault();
                        ramString = Math.Round(used, 0) + "/" + Math.Round(used + available.Value.GetValueOrDefault(), 0) + " GB In Use";
                    }
                }

                service.SaveHardwareData(cpuList, ramString, cpuLoad, ramLoad.FirstOrDefault(), cpuTemp);
                computer.Close();
                i++;

                cpuLoadHistory = service.GetCPULoadHistory();
                ramLoad = service.GetRAMHistory();
                cpuTempHistory = service.GetCPUTempHistory();

                Clients.All.updatePerformanceHistory(cpuList, cpuLoadHistory, ramLoad, cpuTempHistory);

                cpuLoadHistory.Clear();
                cpuTempHistory.Clear();
                cpuTemp.Clear();
                cpuLoad.Clear();
                diskList.Clear();
                cpuList.Clear();
                ramLoad.Clear();

                Thread.Sleep(1000);

            }

        }

    }

}
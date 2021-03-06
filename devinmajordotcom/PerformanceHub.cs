﻿using devinmajordotcom.Services;
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

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }

    public class PerformanceHub : Hub
    {

        [HubMethodName("SendPerformanceHistory")]
        public void SendPerformanceHistory()
        {

            HardwareMonitorService service = new HardwareMonitorService();
            UpdateVisitor updateVisitor1 = new UpdateVisitor();
            var drives = DriveInfo.GetDrives();
            int i = 0;

            Computer computer1 = new Computer()
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

                computer1.Open();
                computer1.Accept(updateVisitor1);

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var diskInfo = drive.Name + "," + drive.VolumeLabel + "," + drive.DriveType + "," + drive.DriveFormat + "," + drive.TotalFreeSpace + "," + drive.TotalSize;
                        diskList.Add(diskInfo);
                    }
                }

                foreach (var t2 in computer1.Hardware)
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
                computer1.Close();
                i++;

                cpuLoadHistory = service.GetCPULoadHistory();
                ramLoad = service.GetRAMHistory();
                cpuTempHistory = service.GetCPUTempHistory();

                Clients.All.updatePerformanceHistory(cpuList, cpuLoadHistory, ramLoad, cpuTempHistory, ramString);

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

        [HubMethodName("SendPerformanceMonitoring")]
        public void SendPerformanceMonitoring()
        {

            HardwareMonitorService service = new HardwareMonitorService();
            UpdateVisitor updateVisitor2 = new UpdateVisitor();
            var drives = DriveInfo.GetDrives();
            int i = 0;

            Computer computer2 = new Computer()
            {
                CPUEnabled = true,
                FanControllerEnabled = true,
                GPUEnabled = true,
                HDDEnabled = true,
                MainboardEnabled = true,
                RAMEnabled = true
            };

            bool hasPackageTemp = false;
            var ramLoad = (float)0.0;
            var ramString = "";
            var cpuTemp = new List<double>();
            var cpuLoad = new List<double>();
            var diskList = new List<string>();
            var cpuList = new List<string>();

            while (i != -1)
            {

                computer2.Open();
                computer2.Accept(updateVisitor2);

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var diskInfo = drive.Name + "," + drive.VolumeLabel + "," + drive.DriveType + "," + drive.DriveFormat + "," + drive.TotalFreeSpace + "," + drive.TotalSize;
                        diskList.Add(diskInfo);
                    }
                }           
                
                foreach (var t2 in computer2.Hardware)
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
                            ramLoad = t1.Value.GetValueOrDefault();
                        }
                        if (t1.SensorType != SensorType.Data || t1.Name != "Used Memory") continue;
                        var available = t2.Sensors.FirstOrDefault(x => x.SensorType == SensorType.Data && x.Name == "Available Memory");
                        if (available == null) continue;
                        var used = t1.Value.GetValueOrDefault();
                        ramString = Math.Round(used ,0) + "/" + Math.Round(used + available.Value.GetValueOrDefault(), 0) + " GB In Use";
                    }
                }
                
                Clients.All.updatePerformanceCounters(cpuList, ramString, cpuLoad, ramLoad, cpuTemp, diskList);
                service.SaveHardwareData(cpuList, ramString, cpuLoad, ramLoad, cpuTemp);
                computer2.Close();
                i++;

                cpuTemp.Clear();
                cpuLoad.Clear();
                diskList.Clear();
                cpuList.Clear();

                Thread.Sleep(1000);

            }

        }

    }

}
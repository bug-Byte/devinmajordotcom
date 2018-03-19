using devinmajordotcom.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Web;
using OpenHardwareMonitor.Hardware;

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

        [HubMethodName("SendPerformanceMonitoring")]
        public void SendPerformanceMonitoring()
        {

            HardwareMonitorService service = new HardwareMonitorService();

            var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            //var networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", "Realtek RTL8174C[P]_8111C Family PCI-E Gigabit Ethernet NIC");
            //var cpuTempCounter = new PerformanceCounter("Thermal Zone Information", "Temperature", @"\_TZ.TZ01");
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var totalRam = ((computerInfo.TotalPhysicalMemory / 1024) / 1024);
            var availableRamCounter = new PerformanceCounter("Memory", "Available MBytes");
            //var cpuTempCounter = new PerformanceCounter("Thermal Zone Information", "Temperature", @"\_TZ.TZ01");

            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);

            var drives = DriveInfo.GetDrives();
            int i = 0;

            while (i != -1)
            {
                var ramCounter = ((totalRam - availableRamCounter.NextValue()) / 1024) + " / " + Math.Round((decimal)totalRam / 1024);
                var cpuTemp = /*(cpuTempCounter.NextValue() - 273.15) + */"°C";
                var diskList = new List<string>();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var diskInfo = drive.Name + "," + drive.VolumeLabel + "," + drive.DriveType + "," + drive.DriveFormat + "," + drive.TotalFreeSpace + "," + drive.TotalSize;
                        diskList.Add(diskInfo);
                    }
                }

                var ramValue = (((totalRam - availableRamCounter.NextValue()) / 1024) / Math.Round((float)totalRam / 1024)) * 100;
            
                foreach (IHardware t1 in computer.Hardware)
                {
                    if (t1.HardwareType == HardwareType.CPU)
                    {
                        foreach (ISensor t in t1.Sensors)
                        {
                            if (t.SensorType == SensorType.Temperature)
                                cpuTemp = t.Value + "°C";
                                break;
                        }
                    }
                }

                //string[] driveList = diskList.ToArray();
                Clients.All.updatePerformanceCounters(cpuCounter.NextValue(), ramCounter, cpuTemp, diskList);
                service.UpdateCPUUsage(cpuCounter.NextValue());
                service.UpdateRAMUsage(ramValue);
                i++;
                Thread.Sleep(1000);
            }
        }

    }
}
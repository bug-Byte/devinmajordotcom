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
            UpdateVisitor updateVisitor = new UpdateVisitor();
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
            

            while (i != -1)
            {

                computer.Open();
                computer.Accept(updateVisitor);

                var ramLoad = (float)0.0;
                var cpuTemp = new List<double>();
                var cpuLoad = new List<double>();
                var diskList = new List<string>();

                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        var diskInfo = drive.Name + "," + drive.VolumeLabel + "," + drive.DriveType + "," + drive.DriveFormat + "," + drive.TotalFreeSpace + "," + drive.TotalSize;
                        diskList.Add(diskInfo);
                    }
                }           
                
                foreach (IHardware t2 in computer.Hardware)
                {
                    if (t2.HardwareType == HardwareType.CPU)
                    {
                        foreach (ISensor t1 in t2.Sensors)
                        {
                            if (t1.SensorType == SensorType.Temperature && t1.Name == "CPU Package")
                            {
                                cpuTemp.Add(t1.Value.GetValueOrDefault());
                            }
                            if (t1.SensorType == SensorType.Load && t1.Name == "CPU Total")
                            {
                                cpuLoad.Add(t1.Value.GetValueOrDefault());
                            }
                        }
                    }
                    if (t2.HardwareType == HardwareType.RAM)
                    {
                        foreach (ISensor t1 in t2.Sensors)
                        {
                            if (t1.SensorType == SensorType.Load)
                            {
                                ramLoad = t1.Value.GetValueOrDefault();
                            }
                        }
                    }
                }
                
                Clients.All.updatePerformanceCounters(cpuLoad, ramLoad, cpuTemp, diskList);
                service.SaveHardwareData(cpuLoad, ramLoad, cpuTemp);
                computer.Close();

                i++;
                Thread.Sleep(1000);

            }

        }

    }

}
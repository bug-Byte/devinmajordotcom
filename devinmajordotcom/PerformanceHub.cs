using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace devinmajordotcom
{
    public class PerformanceHub : Hub
    {
        //private static readonly IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<PerformanceHub>();
        [HubMethodName("SendPerformanceMonitoring")]
        public void SendPerformanceMonitoring()
        {
            var categories = PerformanceCounterCategory.GetCategories();
            var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
            //var networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", "Realtek RTL8174C[P]_8111C Family PCI-E Gigabit Ethernet NIC");
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var totalRam = ((computerInfo.TotalPhysicalMemory / 1024) / 1024);
            var availableRamCounter = new PerformanceCounter("Memory", "Available MBytes");
            int i = 0;
            while (i != -1)
            {
                var ramCounter = ((totalRam - availableRamCounter.NextValue()) / 1024) + " / " + Math.Round((decimal)totalRam / 1024);
                Clients.All.updatePerformanceCounters(cpuCounter.NextValue(), ramCounter);
                i++;
                Thread.Sleep(1000);
            }
        }

    }
}
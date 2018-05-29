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
            int i = 0;

            var cpuList = new List<string>();
            var cpuTemp = new List<double>();
            var cpuLoad = new List<double>();
            var ramLoad = new List<double>();
            
            while (i != -1)
            {

                cpuLoad = service.GetHardwareHistory("CPU Usage");
                ramLoad = service.GetHardwareHistory("RAM Usage");
                cpuTemp = service.GetHardwareHistory("CPU Temp");

                Clients.All.updatePerformanceHistory(cpuList, cpuLoad, ramLoad, cpuTemp);
                i++;

                cpuTemp.Clear();
                cpuLoad.Clear();
                cpuList.Clear();
                ramLoad.Clear();

                Thread.Sleep(1000);

            }

        }

    }

}
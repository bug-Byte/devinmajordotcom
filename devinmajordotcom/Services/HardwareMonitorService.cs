using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Management;
using System.Management.Instrumentation;
using devinmajordotcom.ViewModels;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Hardware;

// REQUIRES OpenHardwareMonitorLib.dll added to project

namespace devinmajordotcom.Services
{

    public class HardwareMonitorService
    {
       
        public void Update()
        {
            var viewModel = new OHData();
            string s = String.Empty;
            string name = string.Empty;
            string type = string.Empty;
            string value = string.Empty;
            int x, y, z, n;
            int hardwareCount;
            int subcount;
            int sensorcount;
            viewModel.ReportItems.Clear();
            viewModel.computerHardware.MainboardEnabled = true;
            viewModel.computerHardware.FanControllerEnabled = true;
            viewModel.computerHardware.CPUEnabled = true;
            viewModel.computerHardware.GPUEnabled = true;
            viewModel.computerHardware.RAMEnabled = true;
            viewModel.computerHardware.HDDEnabled = true;
            viewModel.computerHardware.Open();
            hardwareCount = viewModel.computerHardware.Hardware.Count();
            for (x = 0; x < hardwareCount; x++)
            {
                name = viewModel.computerHardware.Hardware[x].Name;
                type = viewModel.computerHardware.Hardware[x].HardwareType.ToString();
                value = ""; // no value for non-sensors;
                AddReportItem(name, type, value);

                subcount = viewModel.computerHardware.Hardware[x].SubHardware.Count();

                // ADDED 07-28-2016
                // NEED Update to view Subhardware
                for (y = 0; y < subcount; y++)
                {
                    viewModel.computerHardware.Hardware[x].SubHardware[y].Update();
                }
                //

                if (subcount > 0)
                {
                    for (y = 0; y < subcount; y++)
                    {
                        sensorcount = viewModel.computerHardware.Hardware[x].
                            SubHardware[y].Sensors.Count();
                        name = viewModel.computerHardware.Hardware[x].SubHardware[y].Name;
                        type = viewModel.computerHardware.Hardware[x].SubHardware[y].
                            HardwareType.ToString();
                        value = "";
                        AddReportItem(name, type, value);

                        if (sensorcount > 0)
                        {

                            for (z = 0; z < sensorcount; z++)
                            {

                                name = viewModel.computerHardware.Hardware[x].
                                    SubHardware[y].Sensors[z].Name;
                                type = viewModel.computerHardware.Hardware[x].
                                    SubHardware[y].Sensors[z].SensorType.ToString();
                                value = viewModel.computerHardware.Hardware[x].
                                    SubHardware[y].Sensors[z].Value.ToString();
                                AddReportItem(name, type, value);

                            }
                        }
                    }
                }
                sensorcount = viewModel.computerHardware.Hardware[x].Sensors.Count();

                if (sensorcount > 0)
                {
                    for (z = 0; z < sensorcount; z++)
                    {
                        name = viewModel.computerHardware.Hardware[x].Sensors[z].Name;
                        type = viewModel.computerHardware.Hardware[x].Sensors[z].SensorType.ToString();
                        value = viewModel.computerHardware.Hardware[x].Sensors[z].Value.ToString();
                        AddReportItem(name, type, value);

                    }
                }
            }
            viewModel.computerHardware.Close();
        }

        private void AddReportItem(string ARIName, string ARIType, string ARIValue)
        {
            // int readingwidth = 26;
            if (ARIType == "Data")
            {
                return; // ignore data items
            }
            OHMitem ARItem = new OHMitem();
            OHData viewModel = new OHData();
            ARItem.name = ARIName;
            ARItem.type = ARIType + ": ";
            ARItem.reading = ARIValue;
            if (ARIType == "GpuAti")
            {
                ARItem.type = "Graphics Card";
            }

            if (ARIType == "Temperature")
            {
                try
                {
                    double temp = Convert.ToDouble(ARIValue);
                    ARItem.reading = ((((9.0 / 5.0) * temp) + 32).ToString("000.0") + " F");
                }
                catch
                {
                    
                    return;
                }
            }
            if (ARIType == "Clock")
            {
                try
                {
                    double temp = Convert.ToDouble(ARIValue);
                    if (temp < 1000)
                    {
                        ARItem.reading = (temp.ToString("F1") + " MHZ");
                    }
                    else
                    {
                        temp = temp / 1000;
                        ARItem.reading = (temp.ToString("F1") + " GHZ");
                    }
                }
                catch
                {
                    return;
                }

            }
            if (ARIType == "Control" || ARIType == "Load")
            {
                try
                {
                    double temp = Convert.ToDouble(ARIValue);
                    ARItem.name = ARIName;
                    ARItem.reading = (temp.ToString("F0") + " %");
                }
                catch
                {
                    return;
                }
            }
            if (ARIType == "Voltage")
            {
                try
                {
                    double temp = Convert.ToDouble(ARIValue);
                    ARItem.name = ARIName;
                    ARItem.reading = (temp.ToString("F1") + " V");
                }
                catch
                {
                    return;
                }
            }
            // 07-28-2016 Added This item
            if (ARIType == "Fan")
            {
                try
                {
                    double rpm = Convert.ToDouble(ARIValue);
                    ARItem.name = ARIName;
                    ARItem.reading = (rpm.ToString("F0") + " RPM");
                }
                catch
                {
                    return;
                }
            }

            viewModel.ReportItems.Add(ARItem);
        }

    }
    
}
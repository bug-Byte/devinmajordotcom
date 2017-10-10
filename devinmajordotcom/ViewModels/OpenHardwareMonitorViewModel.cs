using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    //public class OpenHardwareMonitorViewModel
    //{
    //}

    // for report compilation
    public class OHMitem
    {

        public OHMitem()
        {
        }

        public string name
        {
            get { return Name; }
            set { Name = value; }
        }

        public string type
        {
            get { return OHMType; }
            set { OHMType = value; }
        }

        public string reading
        {
            get { return OHMValue; }
            set { OHMValue = value; }
        }

        private string Name = String.Empty;
        private string OHMType = String.Empty;
        private string OHMValue = String.Empty;

    }


    /// <summary>
    /// Wrapper for BIOS Information from Win_32BIOS WMI
    /// </summary>
    public class WMIBIOS
    {

        private string name = String.Empty;
        private string manufacturer = String.Empty;
        private string date = String.Empty;
        private string version = String.Empty;

        public string Name
        {
            get { return name; }
        }

        public string Manufacturer
        {
            get { return manufacturer; }
        }

        public string Date
        {
            get { return FormatDate(date); }
        }

        public string Version
        {
            get { return version; }
        }

        public void Update()
        {
            update();
        }


        // Get BIOS Data using WMI
        private void update()
        {

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(@"\\.\root\cimv2", "SELECT * FROM Win32_BIOS");
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject o in collection)
            {
                try
                {
                    name = Convert.ToString(o.GetPropertyValue("Name"));
                }
                catch
                {
                    name = String.Empty;
                }
                try
                {
                    date = Convert.ToString(o.GetPropertyValue("ReleaseDate"));
                }
                catch
                {
                    date = String.Empty;
                }
                try
                {
                    manufacturer = Convert.ToString(o.GetPropertyValue("Manufacturer"));
                }
                catch
                {
                    manufacturer = String.Empty;
                }
                try
                {
                    version = Convert.ToString(o.GetPropertyValue("SMBIOSBIOSVersion"));
                }
                catch
                {
                    version = String.Empty;
                }
            }
            searcher.Dispose();
            return;
        }

        // FORMAT DATE FROM WIN_32 BIOS INTO USABLE FORM
        private string FormatDate(string rawdata)
        {
            string result = String.Empty;
            string year = String.Empty;
            string month = String.Empty;
            string day = String.Empty;
            try
            {
                year = rawdata.Substring(0, 4);
                month = rawdata.Substring(4, 2);
                day = rawdata.Substring(6, 2);
            }
            catch
            {
                return result;
            }
            result = month + "-" + day + "-" + year;
            return result;
        }

    }


    /// Wrapper For OpenHarwareMonitor.dll
    /// 
    public class OHData
    {
        public List<OHMitem> DataList
        {
            get { return ReportItems; }
            set { }

        }
        public List<OHMitem> ReportItems = new List<OHMitem>();
        public OpenHardwareMonitor.Hardware.Computer computerHardware = new OpenHardwareMonitor.Hardware.Computer();
    }

}
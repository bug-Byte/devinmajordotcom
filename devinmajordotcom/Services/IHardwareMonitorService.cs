﻿using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface IHardwareMonitorService
    {

        void UpdateCPUUsage(float nextValue);

        void UpdateRAMUsage(double nextValue);

    }
}
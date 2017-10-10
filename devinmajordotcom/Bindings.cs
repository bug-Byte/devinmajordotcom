using devinmajordotcom.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMediaDashboardService>().To<MediaDashboardService>();
        }
    }
}
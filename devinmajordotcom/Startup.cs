﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(devinmajordotcom.Startup))]
namespace devinmajordotcom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

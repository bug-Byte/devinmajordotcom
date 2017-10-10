using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class SiteLinkViewModel
    {

        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string URL { get; set; }

        public string DisplayIcon { get; set; }

        public bool IsDefault { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsAdminLink { get; set; }

        public int? Order { get; set; }

    }
}
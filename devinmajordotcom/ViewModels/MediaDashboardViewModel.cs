using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class MediaDashboardViewModel
    {

        public UserViewModel CurrentUserViewModel { get; set; }

        public List<SiteLinkViewModel> SidebarLinks { get; set; }

    }
}
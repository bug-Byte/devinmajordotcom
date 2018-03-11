using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class MediaDashboardViewModel
    {

        public UserViewModel CurrentUserViewModel { get; set; }

        public MediaDashboardUserConfigViewModel UserConfig { get; set; }

        public List<SiteLinkViewModel> SidebarLinks { get; set; }

    }

    public class MediaDashboardUserConfigViewModel
    {

        public int UserID { get; set; }

        public string SidebarFullTitle { get; set; }

        public string SidebarColor { get; set; }

        public string SidebarAccentColor { get; set; }

        public string SidebarCollapsedTitle { get; set; }

        public byte[] BackgroundImage { get; set; }

    }

}
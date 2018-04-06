using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Expanded Sidebar Title: ")]
        public string SidebarFullTitle { get; set; }

        [DisplayName("Sidebar Color: ")]
        public string SidebarColor { get; set; }

        [DisplayName("Sidebar Accent Color: ")]
        public string SidebarAccentColor { get; set; }

        [DisplayName("Collapsed Sidebar Title: ")]
        public string SidebarCollapsedTitle { get; set; }

        [DisplayName("Background Image: ")]
        public byte[] BackgroundImage { get; set; }

        [DisplayName("Website Title: ")]
        public string WebsiteTitle { get; set; }

    }

}
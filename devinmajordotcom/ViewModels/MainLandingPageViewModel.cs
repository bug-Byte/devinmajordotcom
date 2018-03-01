using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{

    public class MainLandingPageViewModel
    {

        public List<SiteLinkViewModel> LandingPageApplicationLinks { get; set; }

        public ApplicationManagementViewModel CurrentApplicationData { get; set; }

        public UserViewModel CurrentUserViewModel { get; set; }

        public ContactEmailViewModel ContactEmailData { get; set; }

        public MainLandingPageViewModel()
        {
            
        }

    }

    public class ApplicationManagementViewModel
    {

        public List<SiteLinkViewModel> LandingPageApplicationLinks { get; set; }

        public PortfolioViewModel CurrentPortfolioData { get; set; }

        public MediaDashboardViewModel CurrentMediaDashboardData { get; set; }

    }

}
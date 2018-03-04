using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{

    public class MainLandingPageViewModel
    {

        public List<SiteLinkViewModel> LandingPageApplicationLinks { get; set; }

        public List<SiteLinkViewModel> LandingPageBannerLinks { get; set; }

        public LandingPageConfigViewModel Config { get; set; }

        public UserViewModel CurrentUserViewModel { get; set; }

        public ContactEmailViewModel ContactEmailData { get; set; }

        public PortfolioViewModel CurrentPortfolioData { get; set; }

        public MediaDashboardViewModel CurrentMediaDashboardData { get; set; }

        public ApplicationConfigViewModel CurrentApplicationConfig { get; set; }

        public MainLandingPageViewModel()
        {

        }

    }

    public class ApplicationConfigViewModel : MainLandingPageViewModel
    {

    }

    public class LandingPageConfigViewModel
    {

        public int ID { get; set; }

        public string AppsTitle { get; set; }

        public string AppsIntro { get; set; }

        public string AppsDescription { get; set; }

        public string ContactTitle { get; set; }

        public string ContactInstructions { get; set; }

        public string ServerStatusTitle { get; set; }

        public string ServerStatusDescription { get; set; }

    }

}
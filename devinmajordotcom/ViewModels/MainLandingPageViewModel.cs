using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Link Title : ")]
        public string AppsTitle { get; set; }

        [DisplayName("Link Introduction : ")]
        public string AppsIntro { get; set; }

        [DisplayName("Link Description : ")]
        public string AppsDescription { get; set; }

        [DisplayName("Link Title : ")]
        public string ContactTitle { get; set; }

        [DisplayName("Link Instructions : ")]
        public string ContactInstructions { get; set; }

        [DisplayName("Link Title : ")]
        public string ServerStatusTitle { get; set; }

        [DisplayName("Link Description : ")]
        public string ServerStatusDescription { get; set; }

    }

}
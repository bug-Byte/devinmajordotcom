using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{

    public class MainLandingPageViewModel
    {

        public List<SiteLinkViewModel> LandingPageApplicationLinks { get; set; }

        public List<SiteLinkViewModel> LandingPageBannerLinks { get; set; }

        public LandingPageConfigViewModel Config { get; set; }

        public MyHomeUserConfigViewModel AdminMyHomeConfig { get; set; }

        public MyHomeUserConfigViewModel GuestMyHomeConfig { get; set; }

        public UserViewModel CurrentUserViewModel { get; set; }

        public List<DropDownViewModel> AvailableHardwareTypes { get; set; }

        [Required]
        [DisplayName("Select a Metric * :")]
        public int SelectedHardwareTypeID { get; set; }

        public List<DropDownViewModel> AvailableDateRanges { get; set; }

        [Required]
        [DisplayName("Select a Date/Time Range * :")]
        public int SelectedDateRangeID { get; set; }

        public ContactEmailViewModel ContactEmailData { get; set; }

        public PortfolioViewModel CurrentPortfolioData { get; set; }

        public MediaDashboardViewModel CurrentMediaDashboardData { get; set; }

        public MyHomeMasterSettingsViewModel CurrentMyHomeData { get; set; }

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

        [DisplayName("Main Title")]
        [Required]
        public string AppsTitle { get; set; }

        [DisplayName("Website Name")]
        public string WebsiteName { get; set; }

        public bool IsParticleCanvasOn { get; set; }

        [DisplayName("Background Image")]
        public string BackgroundImage { get; set; }

    }

    public class ServerDataViewModel
    {

        public List<SiteLinkViewModel> LandingPageBannerLinks { get; set; }

        public List<GraphViewModel> GraphList { get; set; }

    }

    public class GraphViewModel
    {

        public List<string> Labels { get; set; }

        public List<double> Values { get; set; }

    }

}
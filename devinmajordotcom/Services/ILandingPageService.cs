using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface ILandingPageService : IBaseDataService
    {

        MainLandingPageViewModel GetLandingPageViewModel();

        ApplicationConfigViewModel GetAppConfigData(Security_User admin = null);

        void ManageLandingPage(MainLandingPageViewModel viewModel);

        List<SiteLinkViewModel> GetMainSiteLinks(bool isRetrievingSettings = false);

        List<SiteLinkViewModel> GetMediaSiteLinks(bool isRetrievingSettings = false);

        List<SiteLinkViewModel> GetMainBannerLinks(bool isRetrievingSettings = false);

        void RemoveSiteLinkById(int ID);

        void RemoveBannerLinkById(int ID);

        //string SendContactEmailToSiteAdmin(ContactEmailViewModel viewModel);

    }
}

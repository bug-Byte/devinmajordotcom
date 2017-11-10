using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface ILandingPageService
    {

        MainLandingPageViewModel GetLandingPageViewModel();

        List<SiteLinkViewModel> GetMainSiteLinks();

        List<SiteLinkViewModel> GetMediaSiteLinks();

    }
}

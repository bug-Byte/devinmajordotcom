using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface IMyHomeService
    {

        MyHomeViewModel GetMyHomeViewModel();

        UserConfigViewModel GetUserConfigViewModelByUserId(int userId);

        List<SiteLinkViewModel> GetFavoritesAndBookmarksByUserId(int userId);

    }
}

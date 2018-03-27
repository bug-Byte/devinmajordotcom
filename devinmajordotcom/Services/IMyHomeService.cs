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

        EditFavoritesViewModel GetEditFavoritesViewModel(int userID);

        SiteLinkViewModel GetFavoriteByID(int ID);

        SiteLinkViewModel GetNewFavoriteViewModel(int userID);

        MyHomeUserConfigViewModel GetUserConfigViewModelByUserId(int userId);

        List<SiteLinkViewModel> GetFavoritesAndBookmarksByUserId(int userId);

        int RemoveFavoriteByID(int ID);

        void AddEditFavorite(SiteLinkViewModel viewModel);

        List<BlogPostViewModel> GetBlogPostsByUserId(int userId);

        BlogPostViewModel GetBlogPostById(int ID);

        void SetUserConfigViewModel(MyHomeUserConfigViewModel viewModel);

    }
}

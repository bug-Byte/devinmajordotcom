using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using devinmajordotcom.Models;

namespace devinmajordotcom.Services
{
    public interface IMyHomeService
    {

        MyHomeViewModel GetMyHomeViewModel();

        EditFavoritesViewModel GetEditFavoritesViewModel(int userID);

        SiteLinkViewModel GetFavoriteByID(int ID);

        SiteLinkViewModel GetNewFavoriteViewModel(int userID);

        BlogPostViewModel GetNewBlogPostViewModel(int userID);

        EditBlogPostsViewModel GetEditBlogPostsViewModel(int userID);

        MyHomeUserConfigViewModel GetUserConfigViewModelByUserId(int userId);

        List<SiteLinkViewModel> GetFavoritesAndBookmarksByUserId(int userId);

        int RemoveFavoriteByID(int ID);

        int RemoveBlogPostByID(int ID);

        void AddEditFavorite(SiteLinkViewModel viewModel);

        void AddEditBlogPost(BlogPostViewModel viewModel);

        List<BlogPostViewModel> GetBlogPostsByUserId(int userId);

        BlogPostViewModel GetBlogPostById(int ID);

        void AddComment(CommentViewModel viewModel);

        List<CommentViewModel> GetCommentsByBlogPostID(int ID);

        void SetUserConfigViewModel(MyHomeUserConfigViewModel viewModel);

    }
}

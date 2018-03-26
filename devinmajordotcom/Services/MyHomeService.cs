using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using devinmajordotcom.ViewModels;
using devinmajordotcom.Models;

namespace devinmajordotcom.Services
{
    public class MyHomeService : BaseDataService, IMyHomeService
    {

        public MyHomeViewModel GetMyHomeViewModel()
        {
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            var adminUserConfig = GetUserConfigViewModelByUserId(siteAdminUser.Id);
            var siteGuestUser = db.Security_Users.FirstOrDefault(x => !x.IsActive && x.ClientName == "::1" && x.UserName == "Guest");
            var GuestUserConfig = GetUserConfigViewModelByUserId(siteGuestUser.Id);

            var guid = HttpContext.Current.Session["MainPageUserAuthID"];
            if (guid == null)
            {
                guid = AddNewUser().Guid;
                if (adminUserConfig.ShowVisitorsAdminHome.GetValueOrDefault())
                {
                    return new MyHomeViewModel()
                    {
                        CurrentUserViewModel = GetCurrentUser((Guid)guid),
                        UserConfig = adminUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteAdminUser.Id),
                        BlogPosts = GetBlogPostsByUserId(siteAdminUser.Id),
                        CanEdit = false,
                    };
                }
                else
                {
                    return new MyHomeViewModel()
                    {
                        CurrentUserViewModel = GetCurrentUser((Guid)guid),
                        UserConfig = GuestUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteGuestUser.Id),
                        BlogPosts = GetBlogPostsByUserId(siteGuestUser.Id),
                        CanEdit = false,
                    };
                }
            }

            var user = GetCurrentUser((Guid)guid);
            return new MyHomeViewModel()
            {
                CurrentUserViewModel = user,
                UserConfig = GetUserConfigViewModelByUserId(user.UserID),
                FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(user.UserID),
                BlogPosts = GetBlogPostsByUserId(user.UserID),
                CanEdit = true
            };
        }

        public void SetUserConfigViewModel(MyHomeUserConfigViewModel viewModel)
        {
            var config = db.MyHome_UserConfigs.FirstOrDefault(x => x.UserId == viewModel.UserID);

            if (config == null) return;
            if (viewModel.BackgroundImage.Length != config.BackgroundImage.Length)
            {
                var newString = System.Text.Encoding.Default.GetString(viewModel.BackgroundImage);
                config.BackgroundImage = Convert.FromBase64String(newString);
            }

            config.ShowWeather = viewModel.ShowWeather;
            config.ShowVisitorsAdminHome = viewModel.ShowVisitorsAdminHome;
            config.ShowDateAndTime = viewModel.ShowDateAndTime;
            config.ShowBookmarks = viewModel.ShowBookmarks;
            config.ShowBanner = viewModel.ShowBanner;
            config.IsEditable = viewModel.IsEditable;
            config.ShowBlog = viewModel.ShowBlog;
            config.BlogTitle = viewModel.BlogTitle;
            config.BookmarksTitle = viewModel.BookmarksTitle;
            config.Greeting = viewModel.Greeting;
            db.SaveChanges();
        }

        public MyHomeUserConfigViewModel GetUserConfigViewModelByUserId(int userId)
        {
            return db.MyHome_UserConfigs.Where(x => x.UserId == userId).Select(x => new MyHomeUserConfigViewModel()
            {
                BackgroundImage = x.BackgroundImage,
                BlogTitle = x.BlogTitle,
                BookmarksTitle = x.BookmarksTitle,
                Greeting = x.Greeting,
                ShowBanner = x.ShowBanner,
                ShowBlog = x.ShowBlog,
                ShowBookmarks = x.ShowBookmarks,
                ShowDateAndTime = x.ShowDateAndTime,
                ShowWeather = x.ShowWeather,
                UserID = x.UserId,
                IsEditable = x.IsEditable,
                ShowVisitorsAdminHome = x.ShowVisitorsAdminHome
            }).FirstOrDefault();
        }

        public EditFavoritesViewModel GetEditFavoritesViewModel(int userID)
        {

            var favorites = db.MyHome_SiteLinks.Where(x => x.UserId == userID).Select(x => new DropDownViewModel()
            {
                ID = x.Id,
                Name = x.DisplayName
            }).ToList();

            return new EditFavoritesViewModel()
            {
                UserID = userID,
                AvailableFavorites = favorites,
                SelectedFavoriteID = favorites.Select(x => x.ID).FirstOrDefault()
            };

        }

        public List<SiteLinkViewModel> GetFavoritesAndBookmarksByUserId(int userId)
        {
            return db.MyHome_SiteLinks.Where(x => x.UserId == userId).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Controller = x.Controller,
                Action = x.Action,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                DisplayName = x.DisplayName,
                UserID = x.UserId,
                Order = x.Order,
                URL = x.Url,
                BackgroundImage = x.Image
            }).ToList();
        }

        public SiteLinkViewModel GetFavoriteByID(int ID)
        {
            return db.MyHome_SiteLinks.Where(x => x.Id == ID).Select(x => new SiteLinkViewModel()
            {
                Controller = x.Controller,
                ID = x.Id,
                IsDefault = x.IsDefault,
                DisplayName = x.DisplayName,
                Action = x.Action,
                IsEnabled = x.IsEnabled,
                Order = x.Order,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                UserID = x.UserId,
                BackgroundImage = x.Image
            }).FirstOrDefault();
        } 

        public int RemoveFavoriteByID(int ID)
        {
            var item = db.MyHome_SiteLinks.FirstOrDefault(x => x.Id == ID);
            var userId = item.UserId;
            if(item != null)
            {
                db.MyHome_SiteLinks.Remove(item);
            }
            db.SaveChanges();
            return userId;
        }

        public void AddEditFavorite(SiteLinkViewModel viewModel)
        {
            var record = db.MyHome_SiteLinks.FirstOrDefault(x => x.Id == viewModel.ID);
            
            if (record != null)
            {
                if (viewModel.BackgroundImage.Length != record.Image.Length)
                {
                    var newString = System.Text.Encoding.Default.GetString(viewModel.BackgroundImage);
                    record.Image = Convert.FromBase64String(newString);
                }
                record.UserId = viewModel.UserID;
                record.Controller = viewModel.Controller;
                record.Id = viewModel.ID;
                record.IsDefault = viewModel.IsDefault;
                record.DisplayName = viewModel.DisplayName;
                record.Action = viewModel.Action;
                record.IsEnabled = viewModel.IsEnabled;
                record.Order = viewModel.Order;
                record.Description = viewModel.Description;
                record.Directive = viewModel.Directive;
                record.DisplayIcon = viewModel.DisplayIcon;
                record.Url = viewModel.URL;
            }
            else
            {
                var newRecord = new MyHome_SiteLink()
                {
                    UserId = viewModel.UserID,
                    Controller = viewModel.Controller,
                    Id = viewModel.ID,
                    IsDefault = viewModel.IsDefault,
                    DisplayName = viewModel.DisplayName,
                    Action = viewModel.Action,
                    IsEnabled = viewModel.IsEnabled,
                    Order = viewModel.Order,
                    Description = viewModel.Description,
                    Directive = viewModel.Directive,
                    DisplayIcon = viewModel.DisplayIcon,
                    Url = viewModel.URL
                };
                if (viewModel.BackgroundImage.Length > 0)
                {
                    var newString = System.Text.Encoding.Default.GetString(viewModel.BackgroundImage);
                    newRecord.Image = Convert.FromBase64String(newString);
                }
                db.MyHome_SiteLinks.Add(newRecord);
            }
            db.SaveChanges();
        }

        public BlogPostViewModel GetBlogPostById(int ID)
        {
            return db.MyHome_BlogPosts.Where(x => x.Id == ID).Select(x => new BlogPostViewModel()
            {
                BlogPostID = x.Id,
                AuthorUserID = x.UserId,
                PostTitle = x.Title,
                Image = x.Image,
                Body = x.Body,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                PostComments = x.MyHome_BlogPostComments.Select(y => new CommentViewModel()
                {
                    AuthorUserID = y.UserId,
                    Body = y.CommentBody,
                    Image = y.Image,
                    CreatedOn = y.CreatedOn,
                    CreatedBy = y.CreatedBy,
                    ModifiedOn = y.ModifiedOn,
                    ModifiedBy = y.ModifiedBy
                }).OrderBy(y => y.CreatedOn).ToList()
            }).FirstOrDefault();
        }

        public List<BlogPostViewModel> GetBlogPostsByUserId(int userId)
        {
            return db.MyHome_BlogPosts.Where(x => x.UserId == userId).Select(x => new BlogPostViewModel()
            {
                BlogPostID = x.Id,
                AuthorUserID = x.UserId,
                PostTitle = x.Title,
                Image = x.Image,
                Body = x.Body,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn
            }).OrderBy(y => y.CreatedOn).ToList();
        }

    }
}
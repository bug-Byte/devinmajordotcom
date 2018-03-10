using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using devinmajordotcom.ViewModels;

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
                if(adminUserConfig.ShowVisitorsAdminHome.GetValueOrDefault())
                {
                    return new MyHomeViewModel()
                    {
                        UserConfig = adminUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteAdminUser.Id),
                        CanEdit = false,
                    };
                }
                else
                {
                    return new MyHomeViewModel()
                    {
                        UserConfig = GuestUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteGuestUser.Id),
                        CanEdit = false,
                    };
                }
            }

            var user = GetCurrentUser((Guid)guid);
            return new MyHomeViewModel()
            {
                UserConfig = GetUserConfigViewModelByUserId(user.UserID),
                FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(user.UserID),
                CanEdit = true
            };
        }

        public UserConfigViewModel GetUserConfigViewModelByUserId(int userId)
        {
            return db.MyHome_UserConfigs.Where(x => x.UserId == userId).Select(x => new UserConfigViewModel()
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
                Order = x.Order,
                URL = x.Url,
                EncodedImage = x.Image
            }).ToList();
        }

    }
}
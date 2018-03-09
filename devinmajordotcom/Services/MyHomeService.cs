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
            var guid = HttpContext.Current.Session["MainPageUserAuthID"];
            if (siteAdminUser == null)
            {
                guid = AddNewUser(true).Guid;
            }
            if (guid == null)
            {
                guid = AddNewUser().Guid;
            }
            var user = GetCurrentUser((Guid)guid);

            return new MyHomeViewModel()
            {
                FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(user.UserID)
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
                Order = x.Order,
                URL = x.Url,
                EncodedImage = x.Image
            }).ToList();
        }

    }
}
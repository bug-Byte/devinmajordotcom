using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Devinmajordotcom;
using System.Security.Principal;

namespace devinmajordotcom.Services
{
    public class LandingPageService : ILandingPageService
    {

        protected dbContext db;

        public LandingPageService()
        {
            this.db = new dbContext();
        }

        public MainLandingPageViewModel GetLandingPageViewModel()
        {
            return new MainLandingPageViewModel()
            {
                CurrentUserViewModel = GetCurrentUserStatus(),
                LandingPageApplicationLinks = GetMainSiteLinks()
            };
        }

        public UserStatusViewModel GetCurrentUserStatus()
        {
            var isUserAdmin = false;
            var isUserActive = false;
            var currentContextUser = WindowsIdentity.GetCurrent().Name.ToString().Split('\\')[0];
            var currentDbUser = db.Users.FirstOrDefault(x => x.ClientName == currentContextUser);
            if(currentDbUser != null)
            {
                isUserAdmin = currentDbUser.IsAdmin;
                isUserActive = currentDbUser.IsActive;
            }
            else
            {
                var user = new User()
                {
                    ClientName = currentContextUser,
                    IsActive = true,
                    IsAdmin = false
                };
            }
            return new UserStatusViewModel()
            {
                UserIsAdmin = isUserAdmin,
                UserIsActive = isUserActive
            };
        }

        public List<SiteLinkViewModel> GetMainSiteLinks()
        {
            return db.SiteLinks.Where(y => y.IsEnabled && y.ApplicationId == (int)Devinmajordotcom.ApplicationMaster.ApplicationMasters.MainLandingPage).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                ParentApplicationId = x.ApplicationId,
                ParentApplicationName = x.ApplicationMaster.Name,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Order = x.Order
            }).ToList();
        }

        public List<SiteLinkViewModel> GetMediaSiteLinks()
        {
            return db.SiteLinks.Where(y => y.IsEnabled && y.ApplicationId == (int)Devinmajordotcom.ApplicationMaster.ApplicationMasters.PlexMediaDashboard).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                ParentApplicationId = x.ApplicationId,
                ParentApplicationName = x.ApplicationMaster.Name,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Order = x.Order
            }).ToList();
        }

    }
}
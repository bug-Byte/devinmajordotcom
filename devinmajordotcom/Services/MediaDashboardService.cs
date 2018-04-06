using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.Services
{
    public class MediaDashboardService : BaseDataService, IMediaDashboardService
    {

        protected dbContext db;

        public MediaDashboardService()
        {
            db = new dbContext();
        }

        public MediaDashboardViewModel GetMediaDashboardViewModel()
        {
            var guid = HttpContext.Current.Session["MainPageUserAuthID"];

            if (guid == null)
            {
                guid = AddNewUser().Guid;
            }

            var user = GetCurrentUser((Guid)guid);
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);

            return new MediaDashboardViewModel()
            {
                CurrentUserViewModel = user,
                UserConfig = GetUserConfigByUserId(siteAdminUser.Id),
                SidebarLinks = db.MediaDashboard_SiteLinks.Select( x => new SiteLinkViewModel()
                {
                    DisplayName = x.DisplayName,
                    DisplayIcon = x.DisplayIcon,
                    Action = x.Action,
                    Controller = x.Controller,
                    Description = x.Description,
                    IsPublic = x.IsPublic,
                    IsDefault = x.IsDefault,
                    IsEnabled = x.IsEnabled,
                    ID = x.Id,
                    URL = x.Url,
                    Order = x.Order
                }).OrderBy(x => x.Order).ToList()
            };
        }

        public MediaDashboardUserConfigViewModel GetUserConfigByUserId(int userID)
        {
            return db.MediaDashboard_UserConfigs.Where(x => x.UserId == userID).Select(x => new MediaDashboardUserConfigViewModel()
            {
                UserID = x.UserId,
                BackgroundImage = x.BackgroundImage,
                SidebarCollapsedTitle = x.SidebarCollapsedTitle,
                SidebarColor = x.SidebarColor,
                SidebarFullTitle = x.SidebarFullTitle,
                WebsiteTitle = x.WebsiteTitle,
                SidebarAccentColor = x.SidebarAccentColor
            }).FirstOrDefault();
        }

        public string ManageMediaDashboard(MediaDashboardViewModel viewModel)
        {
            try
            {

                var adminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);

                if(adminUser != null)
                {
                    var adminUserConfig = db.MediaDashboard_UserConfigs.FirstOrDefault(x => x.UserId == adminUser.Id);
                    if (adminUserConfig != null && viewModel.UserConfig != null)
                    {
                        adminUserConfig.BackgroundImage = viewModel.UserConfig.BackgroundImage;
                        adminUserConfig.WebsiteTitle = viewModel.UserConfig.WebsiteTitle;
                        adminUserConfig.SidebarFullTitle = viewModel.UserConfig.SidebarFullTitle;
                        adminUserConfig.SidebarColor = viewModel.UserConfig.SidebarColor;
                        adminUserConfig.SidebarCollapsedTitle = viewModel.UserConfig.SidebarCollapsedTitle;
                        adminUserConfig.SidebarAccentColor = viewModel.UserConfig.SidebarAccentColor;
                        db.SaveChanges();
                    }
                }

                foreach (var link in viewModel.SidebarLinks.Where(x => x.DisplayName != null && x.URL != null))
                {
                    var linkRecord = db.MediaDashboard_SiteLinks.FirstOrDefault(x => x.Id == link.ID);
                    if (linkRecord != null)
                    {
                        linkRecord.DisplayName = link.DisplayName;
                        linkRecord.Description = link.Description;
                        linkRecord.Directive = link.Directive;
                        linkRecord.Url = link.URL;
                        linkRecord.Action = link.Action;
                        linkRecord.Controller = link.Controller;
                        linkRecord.DisplayIcon = link.DisplayIcon;
                        linkRecord.Order = link.Order;
                        linkRecord.IsPublic = link.IsPublic;
                        linkRecord.IsDefault = link.IsDefault;
                        linkRecord.IsEnabled = link.IsEnabled;
                    }
                    else
                    {
                        var newLinkRecord = new MediaDashboard_SiteLink()
                        {
                            DisplayName = link.DisplayName,
                            Description = link.Description,
                            Directive = link.Directive,
                            Url = link.URL,
                            Action = link.Action,
                            Controller = link.Controller,
                            DisplayIcon = link.DisplayIcon,
                            Order = link.Order,
                            IsDefault = link.IsDefault,
                            IsPublic = link.IsPublic,
                            IsEnabled = link.IsEnabled
                        };
                        db.MediaDashboard_SiteLinks.Add(newLinkRecord);
                    }
                }

                db.SaveChanges();
                return "success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void RemoveLink(int ID)
        {
            var link = db.MediaDashboard_SiteLinks.FirstOrDefault(x => x.Id == ID);
            if (link != null)
            {
                db.MediaDashboard_SiteLinks.Remove(link);
            }
            db.SaveChanges();
        }

    }
}
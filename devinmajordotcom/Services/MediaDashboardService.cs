using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.Services
{
    public class MediaDashboardService : IMediaDashboardService
    {

        protected dbContext db;

        public MediaDashboardService()
        {
            db = new dbContext();
        }

        public MediaDashboardViewModel GetMediaDashboardViewModel()
        {
            return new MediaDashboardViewModel()
            {
                SidebarLinks = db.SiteLinks.Where(x => x.ApplicationId == (int)Devinmajordotcom.ApplicationMaster.ApplicationMasters.PlexMediaDashboard).Select( x => new SiteLinkViewModel()
                {
                    DisplayName = x.DisplayName,
                    DisplayIcon = x.DisplayIcon,
                    Action = x.Action,
                    Controller = x.Controller,
                    Description = x.Description,
                    IsDefault = x.IsDefault,
                    IsEnabled = x.IsEnabled,
                    ID = x.Id,
                    URL = x.Url,
                    Order = x.Order,
                    ParentApplicationId = x.ApplicationId,
                    ParentApplicationName = x.ApplicationMaster.Name
                }).ToList()
            };
        }

        public string ManageMediaDashboard(MediaDashboardViewModel viewModel)
        {
            return "";
        }

    }
}
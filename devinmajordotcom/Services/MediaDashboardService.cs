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
            this.db = new dbContext();
        }

        public List<SiteLinkViewModel> GetMediaSiteLinks()
        {
            return db.SiteLinks.Select(x => new SiteLinkViewModel() {
                ID = x.Id,
                DisplayName = x.DisplayName,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url, 
                IsAdminLink = x.IsAdminLink,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Order = x.Order
            }).ToList();
        }

    }
}
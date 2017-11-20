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
            try
            {
                foreach (var link in viewModel.SidebarLinks)
                {
                    var linkRecord = db.SiteLinks.FirstOrDefault(x => x.Id == link.ID);
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
                        linkRecord.IsDefault = link.IsDefault;
                        linkRecord.IsEnabled = link.IsEnabled;
                        linkRecord.ApplicationId = link.ParentApplicationId;
                    }
                    else
                    {
                        var newLinkRecord = new SiteLink()
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
                            IsEnabled = link.IsEnabled,
                            ApplicationId = link.ParentApplicationId
                        };
                        db.SiteLinks.Add(newLinkRecord);
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

    }
}
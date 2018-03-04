﻿using devinmajordotcom.Models;
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

            return new MediaDashboardViewModel()
            {
                CurrentUserViewModel = GetCurrentUser((Guid)guid),
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

        public string ManageMediaDashboard(MediaDashboardViewModel viewModel)
        {
            try
            {
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
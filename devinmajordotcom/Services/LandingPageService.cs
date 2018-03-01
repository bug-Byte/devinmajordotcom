﻿using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Devinmajordotcom;
using System.Security.Principal;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using devinmajordotcom.Helpers;

namespace devinmajordotcom.Services
{
    public class LandingPageService : BaseDataService, ILandingPageService
    {
        
        protected IPortfolioService portfolioService;
        protected IMediaDashboardService mediaDashboardService;
        public LandingPageService(IPortfolioService PortfolioService, IMediaDashboardService MediaDashboardService)
        {
            portfolioService = PortfolioService;
            mediaDashboardService = MediaDashboardService;
        }

        public MainLandingPageViewModel GetLandingPageViewModel()
        {
            var siteAdminUser = db.Users.FirstOrDefault(x => x.IsActive && x.IsAdmin) ?? AddNewUser(true);

            return new MainLandingPageViewModel()
            {
                CurrentUserViewModel = GetCurrentUser(),
                LandingPageApplicationLinks = GetMainSiteLinks(),
                CurrentApplicationData = new ApplicationManagementViewModel()
                {
                    CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                    CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel()
                },
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = siteAdminUser == null ? "" : siteAdminUser.EmailAddress
                }
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

        public string SendContactEmailToSiteAdmin(ContactEmailViewModel viewModel)
        {
            var message = new MailMessage();
            try
            {
                var body = "<p>From: </p><p>{0}</p><p>({1})</p><p>Message:</p><p>{2}</p>";
                message.To.Add(new MailAddress(viewModel.RecipientEmail));
                message.Subject = "Attn Site Admin: " + viewModel.Subject;
                message.Body = string.Format(body, viewModel.SenderName, viewModel.SenderEmailAddress, viewModel.Content);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                    return "Success";
                }
            }
            catch(Exception e)
            {
                message.Dispose();
                return e.Message;
            }
        }
    }
}
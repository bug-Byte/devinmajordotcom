using devinmajordotcom.Models;
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
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            var guid = HttpContext.Current.Session["MainPageUserAuthID"];
            if(siteAdminUser == null)
            {
                guid = AddNewUser(true).Guid;
            }
            if(guid == null)
            {
                guid = AddNewUser().Guid;
            }
            var user = GetCurrentUser((Guid)guid);
            return new MainLandingPageViewModel()
            {
                Config = GetLandingPageConfig(),
                CurrentUserViewModel = user,
                LandingPageApplicationLinks = GetMainSiteLinks(),
                LandingPageBannerLinks = GetMainBannerLinks(),
                CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel(),
                CurrentApplicationConfig = GetAppConfigData(siteAdminUser),
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = siteAdminUser == null ? "" : siteAdminUser.EmailAddress
                }
            };
        }

        public ApplicationConfigViewModel GetAppConfigData(Security_User admin = null)
        {
            var results = new ApplicationConfigViewModel()
            {
                Config = GetLandingPageConfig(),
                LandingPageApplicationLinks = GetMainSiteLinks(true),
                LandingPageBannerLinks = GetMainBannerLinks(true),
                CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel(),
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = admin == null ? "" : admin.EmailAddress
                }
            };
            results.CurrentMediaDashboardData.SidebarLinks = GetMediaSiteLinks(true);
            return results;
        }

        public void ManageLandingPage(MainLandingPageViewModel viewModel)
        {
            var configRecord = db.LandingPage_Configs.FirstOrDefault();
            if(configRecord != null)
            {
                configRecord.AppsTitle = viewModel.Config.AppsTitle;
                configRecord.BackgroundImage = viewModel.Config.BackgroundImage;
                configRecord.IsParticleCanvasOn = viewModel.Config.IsParticleCanvasOn;
            }
            else
            {
                var newRecord = new LandingPage_Config()
                {
                    AppsTitle = viewModel.Config.AppsTitle,
                    IsParticleCanvasOn = viewModel.Config.IsParticleCanvasOn,
                    BackgroundImage = viewModel.Config.BackgroundImage
                };
                db.LandingPage_Configs.Add(newRecord);
                db.SaveChanges();
            }
            foreach (var link in viewModel.LandingPageBannerLinks)
            {
                var linkRecord = db.LandingPage_BannerLinks.First();
                linkRecord.IsDefault = link.IsDefault;
                linkRecord.IsEnabled = link.IsEnabled;
                linkRecord.Url = link.URL;
            }
            foreach (var link in viewModel.LandingPageApplicationLinks)
            {
                var linkRecord = db.LandingPage_SiteLinks.First();
                linkRecord.IsDefault = link.IsDefault;
                linkRecord.IsEnabled = link.IsEnabled;
                linkRecord.Action = link.Action;
                linkRecord.Controller = link.Controller;
                linkRecord.Url = link.URL;
                linkRecord.Description = link.Description;
                linkRecord.Directive = link.Directive;
                linkRecord.DisplayIcon = link.DisplayIcon;
                linkRecord.Order = link.Order;
                linkRecord.DisplayName = link.DisplayName;
            }
            db.SaveChanges();
        }

        public LandingPageConfigViewModel GetLandingPageConfig()
        {
            return db.LandingPage_Configs.Select(x => new LandingPageConfigViewModel() {
                AppsTitle = x.AppsTitle,
                BackgroundImage = x.BackgroundImage,
                IsParticleCanvasOn = x.IsParticleCanvasOn,
                ID = x.Id
            }).FirstOrDefault();
        }

        public List<SiteLinkViewModel> GetMainSiteLinks(bool isRetrievingSettings = false)
        {
            return db.LandingPage_SiteLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        public List<SiteLinkViewModel> GetMainBannerLinks(bool isRetrievingSettings = false)
        {
            return db.LandingPage_BannerLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        public List<SiteLinkViewModel> GetMediaSiteLinks(bool isRetrievingSettings = false)
        {
            return db.MediaDashboard_SiteLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                IsPublic = x.IsPublic,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        //public string SendContactEmailToSiteAdmin(ContactEmailViewModel viewModel)
        //{
        //    var message = new MailMessage();
        //    try
        //    {
        //        var body = "<p>From: </p><p>{0}</p><p>({1})</p><p>Message:</p><p>{2}</p>";
        //        message.To.Add(new MailAddress(viewModel.RecipientEmail));
        //        message.Subject = "Attn Site Admin: " + viewModel.Subject;
        //        message.Body = string.Format(body, viewModel.SenderName, viewModel.SenderEmailAddress, viewModel.Content);
        //        message.IsBodyHtml = true;
        //        using (var smtp = new SmtpClient())
        //        {
        //            smtp.Send(message);
        //            return "Success";
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        message.Dispose();
        //        return e.Message;
        //    }
        //}
    }
}
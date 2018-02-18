using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Net.Mail;
using System.Net;

namespace devinmajordotcom.Services
{
    public class LandingPageService : ILandingPageService
    {

        protected dbContext db;
        protected IPortfolioService portfolioService;
        protected IMediaDashboardService mediaDashboardService;
        public LandingPageService(IPortfolioService PortfolioService, IMediaDashboardService MediaDashboardService)
        {
            db = new dbContext();
            portfolioService = PortfolioService;
            mediaDashboardService = MediaDashboardService;
        }

        public MainLandingPageViewModel GetLandingPageViewModel()
        {
            var siteAdminUser = db.Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            if(siteAdminUser == null)
            {
                siteAdminUser = AddNewUser(true);
            }
            return new MainLandingPageViewModel()
            {
                CurrentUserViewModel = GetCurrentUserStatus(),
                LandingPageApplicationLinks = GetMainSiteLinks(),
                CurrentApplicationData = new ApplicationManagementViewModel()
                {
                    CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                    CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel()
                },
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = siteAdminUser.EmailAddress
                }
            };
        }

        public User AddNewUser(bool IsUserToAddAnAdmin = false)
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            var newUser = new User()
            {
                ClientName = ip,
                EmailAddress = "",
                IsActive = true,
                IsAdmin = IsUserToAddAnAdmin
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return newUser;

        }

        public UserStatusViewModel GetCurrentUserStatus()
        {

            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            var currentDbUser = db.Users.FirstOrDefault(x => x.ClientName == ip);

            if (currentDbUser == null)
            {
                var newUser = AddNewUser();
                return new UserStatusViewModel()
                {
                    UserIsAdmin = newUser.IsAdmin,
                    UserIsActive = newUser.IsActive
                };
            }
            return new UserStatusViewModel()
            {
                UserIsAdmin = currentDbUser.IsAdmin,
                UserIsActive = currentDbUser.IsActive
            };

        }

        public List<SiteLinkViewModel> GetMainSiteLinks()
        {
            return db.SiteLinks.Where(y => y.IsEnabled && y.ApplicationId == (int)ApplicationMasterEnum.ApplicationMasters.MainLandingPage).Select(x => new SiteLinkViewModel()
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
            return db.SiteLinks.Where(y => y.IsEnabled && y.ApplicationId == (int)ApplicationMasterEnum.ApplicationMasters.PlexMediaDashboard).Select(x => new SiteLinkViewModel()
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
            MailMessage message = new MailMessage();
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

        public void UpdateCPUUsage(int nextValue)
        {
            var newHardwareDataRecord = new HardwarePerformance()
            {
                HardwareTypeId = (int)HardwareTypeEnum.HardwareTypes.CPUUsage,
                PercentageValue = nextValue
            };
            db.HardwarePerformances.Add(newHardwareDataRecord);
        }

    }
}
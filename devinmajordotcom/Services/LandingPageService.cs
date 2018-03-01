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
using devinmajordotcom.Helpers;

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
        
        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            var user = db.Users.FirstOrDefault(x => x.Guid == viewModel.GUID);
            if(user != null)
            {
                user.EmailAddress = viewModel.EmailAddress;
                user.IsActive = viewModel.UserIsActive;
                user.IsAdmin = viewModel.UserIsAdmin;
                user.UserName = viewModel.UserName;
                user.Password = SecurityHelper.HashSHA1(viewModel.Password);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("NULL User ID", new NullReferenceException());
            }
        }

        public User AddNewUser(bool IsUserToAddAnAdmin = false)
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var userGuid = Guid.NewGuid();

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            var newUser = new User()
            {
                ClientName = ip,
                EmailAddress = "",
                Guid = userGuid,
                IsActive = true,
                IsAdmin = IsUserToAddAnAdmin
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            return newUser;

        }

        public UserViewModel GetCurrentUser()
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
                var newUser = AddNewUser();
                return new UserViewModel()
                {
                    UserID = newUser.Id,
                    EmailAddress = newUser.EmailAddress,
                    GUID = newUser.Guid,
                    UserName = newUser.UserName,
                    Password = newUser.Password,
                    UserIsAdmin = newUser.IsAdmin,
                    UserIsActive = newUser.IsActive
                };
            }
            return new UserViewModel()
            {
                EmailAddress = currentDbUser.EmailAddress,
                GUID = currentDbUser.Guid,
                UserID = currentDbUser.Id,
                Password = currentDbUser.Password,
                UserName = currentDbUser.UserName,
                UserIsAdmin = currentDbUser.IsAdmin,
                UserIsActive = currentDbUser.IsActive
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
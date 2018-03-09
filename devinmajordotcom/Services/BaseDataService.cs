using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using devinmajordotcom.Helpers;
using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace devinmajordotcom.Services
{
    public class BaseDataService : IBaseDataService, IRequiresSessionState
    {

        protected dbContext db;

        public BaseDataService()
        {
            db = new dbContext();
        }

        public UserViewModel Login(UserViewModel viewModel, bool isAdmin = false)
        {
            var results = ValidateCredentials(viewModel.EmailAddress, viewModel.Password, viewModel.UserName);

            if (results.LoginAttemptStatus != "Success" && results.User.UserID != 0 && results.User.UserIsActive && (!isAdmin || results.User.UserIsAdmin))
            {
                throw new Exception("Unauthorized User", new UnauthorizedAccessException());
            }

            if (HttpContext.Current.Session["MainPageUserAuthID"] == null || (Guid)HttpContext.Current.Session["MainPageUserAuthID"] != results.User.GUID)
            {
                HttpContext.Current.Session.Timeout = 86400;
                HttpContext.Current.Session["MainPageUserAuthID"] = results.User.GUID;
                HttpContext.Current.Session["MainPageUserName"] = results.User.UserName;
            }
            return results.User;
        }

        public void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public UserValidationViewModel ValidateCredentials(string emailAddress, string password , string userName = null)
        {
            var results = new UserValidationViewModel()
            {
                LoginAttemptStatus = "Failure",
                User = new UserViewModel()
            };

            var user = db.Security_Users.Where(x => string.IsNullOrEmpty(userName)
                ? x.EmailAddress == emailAddress
                : x.UserName == userName).Select(x => new UserViewModel()
            {
                EmailAddress = x.EmailAddress,
                GUID = x.Guid,
                UserID = x.Id,
                Password = x.Password,
                UserName = x.UserName,
                UserIsAdmin = x.IsAdmin,
                UserIsActive = x.IsActive
            }).FirstOrDefault();

            if (user == null) return results;
            var reHashedPassword = SecurityHelper.HashSHA1(password + user.GUID.ToString());
            if (reHashedPassword != user.Password) return results;
            results.LoginAttemptStatus = "Success";
            results.User = user;
            return results;
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            var user = db.Security_Users.FirstOrDefault(x => x.Guid == viewModel.GUID);
            if (user != null)
            {
                user.EmailAddress = viewModel.EmailAddress;
                user.IsActive = viewModel.UserIsActive;
                user.IsAdmin = viewModel.UserIsAdmin;
                user.UserName = viewModel.UserName;
                user.Password = SecurityHelper.HashSHA1(viewModel.Password + user.Guid.ToString());
                db.SaveChanges();
            }
            else
            {
                throw new Exception("NULL User ID", new NullReferenceException());
            }
        }

        public Security_User AddNewUser(bool IsUserToAddAnAdmin = false)
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var userGuid = HttpContext.Current.Session["MainPageUserAuthID"];
            if(userGuid == null)
            {
                userGuid = Guid.NewGuid();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            var newUser = new Security_User()
            {
                ClientName = ip,
                EmailAddress = "",
                Guid = (Guid)userGuid,
                IsActive = true,
                IsAdmin = IsUserToAddAnAdmin
            };

            db.Security_Users.Add(newUser);
            db.SaveChanges();

            if(IsUserToAddAnAdmin)
            {
                var guestMyHomeLinks = db.MyHome_SiteLinks.Where(x => x.UserId == db.Security_Users.Where(y => y.ClientName == "::1" && y.UserName == "Guest").Select(y => y.Id).FirstOrDefault()).ToList();
                if(guestMyHomeLinks != null && guestMyHomeLinks.Count > 0)
                {
                    foreach(var link in guestMyHomeLinks)
                    {
                        var newLinkRecord = new MyHome_SiteLink()
                        {
                            Action = null,
                            Controller = null,
                            DisplayIcon = null,
                            Description = null,
                            Directive = null,
                            UserId = newUser.Id,
                            DisplayName = link.DisplayName,
                            Url = link.Url,
                            IsDefault = link.IsDefault,
                            IsEnabled = link.IsEnabled,
                            Image = link.Image,
                            Order = link.Order
                        };
                        db.MyHome_SiteLinks.Add(newLinkRecord);
                    }
                    db.SaveChanges();
                }
            }

            return newUser;
        }

        public UserViewModel GetCurrentUser(Guid? GUID = null)
        {
            return db.Security_Users.Where(x => x.Guid == GUID).Select(x => new UserViewModel()
            {
                EmailAddress = x.EmailAddress,
                GUID = x.Guid,
                UserID = x.Id,
                Password = x.Password,
                UserName = x.UserName,
                UserIsAdmin = x.IsAdmin,
                UserIsActive = x.IsActive
            }).FirstOrDefault();
        }

    }
}
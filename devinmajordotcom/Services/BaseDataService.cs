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
using Newtonsoft.Json;

namespace devinmajordotcom.Services
{
    public class BaseDataService : IBaseDataService
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

            return results.User;
        }

        public UserValidationViewModel ValidateCredentials(string emailAddress, string password , string userName = null)
        {
            var results = new UserValidationViewModel()
            {
                LoginAttemptStatus = "Failure",
                User = new UserViewModel()
            };

            var user = db.Users.Where(x => string.IsNullOrEmpty(userName)
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
            var reHashedPassword = SecurityHelper.HashSHA1(password + user.GUID);
            if (reHashedPassword != user.Password) return results;
            results.LoginAttemptStatus = "Success";
            results.User = user;
            return results;
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            var user = db.Users.FirstOrDefault(x => x.Guid == viewModel.GUID);
            if (user != null)
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

        public UserViewModel GetCurrentUser(Guid? GUID = null)
        {
            if (GUID != null)
            {
                return db.Users.Where(x => x.Guid == GUID).Select(x => new UserViewModel()
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

    }
}
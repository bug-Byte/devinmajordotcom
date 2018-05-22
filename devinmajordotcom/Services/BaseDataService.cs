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
            var results = ValidateCredentials(viewModel.UserName, viewModel.Password, viewModel.EmailAddress);

            if (results.User.UserID != 0 && results.User.IsEmailConfirmed && ((isAdmin && results.User.UserIsAdmin) || (isAdmin == results.User.UserIsAdmin)) || HttpContext.Current.Session["MainPageUserAuthID"] == null || (Guid)HttpContext.Current.Session["MainPageUserAuthID"] != results.User.GUID)
            {
                HttpContext.Current.Session.Timeout = 86400;
                HttpContext.Current.Session["MainPageUserAuthID"] = results.User.GUID;
                HttpContext.Current.Session["MainPageUserName"] = results.User.UserName;
                HttpContext.Current.Session["UserId"] = results.User.UserID;
                return results.User;
            }

            return new UserViewModel();
        }

        public void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public void SetConfirmationEmailSent(UserViewModel viewModel)
        {
            var user = db.Security_Users.FirstOrDefault(x => x.Guid == viewModel.GUID);
            if(user != null)
            {
                user.IsEmailConfirmationSent = true;
                db.SaveChanges();
            }
        }

        public void EmailSent(ContactEmailViewModel viewModel)
        {
            var record = new Security_Email()
            {
                SenderName = viewModel.SenderName,
                SenderEmailAddress = viewModel.SenderEmailAddress,
                RecipientEmail = viewModel.RecipientEmail,
                RecipientName = viewModel.RecipientName,
                Subject = viewModel.Subject,
                SenderUserGuid = viewModel.UserGUID,
                Content = viewModel.Content,
                EmailTypeId = viewModel.EmailTypeID
            };
            db.Security_Emails.Add(record);
            db.SaveChanges();
        }

        public bool CheckIfEmailExpired(Guid UserGUID, int EmailTypeID)
        {
            var record = db.Security_Emails.Where(x => x.SenderUserGuid == UserGUID && x.EmailTypeId == EmailTypeID).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            if (record == null) return true;
            var difference = DateTime.Now - record.CreatedOn;
            if (difference != null)
            {
                return difference.Value.Days >= 1;
            }
            return true;
        }

        public UserViewModel LookupUser(string userString)
        {
            return db.Security_Users.Select(x => new UserViewModel() {
                EmailAddress = x.EmailAddress,
                GUID = x.Guid,
                UserID = x.Id,
                UserIsAdmin = x.IsAdmin,
                IsEmailConfirmed = x.IsEmailConfirmed,
                UserName = x.UserName,
                UserIsActive = x.IsActive
            }).FirstOrDefault(x => x.UserName == userString || x.EmailAddress == userString);
        }
        public UserValidationViewModel ValidateCredentials(string userString, string password , string email = null)
        {
            var results = new UserValidationViewModel()
            {
                LoginAttemptStatus = "Failure",
                User = new UserViewModel()
            };

            var user = db.Security_Users.Where(x => x.UserName != "Guest" && x.EmailAddress == (email ?? userString) || x.UserName == userString).Select(x => new UserViewModel()
            {
                EmailAddress = x.EmailAddress,
                GUID = x.Guid,
                UserID = x.Id,
                Password = x.Password,
                UserName = x.UserName,
                UserIsAdmin = x.IsAdmin,
                UserIsActive = x.IsActive,
                IsEmailConfirmed = x.IsEmailConfirmed
            }).FirstOrDefault();

            if (user == null) return results;
            var reHashedPassword = SecurityHelper.HashSHA1(password + user.GUID.ToString());
            if (reHashedPassword != user.Password) return results;
            results.LoginAttemptStatus = "Success";
            results.User = user;
            return results;
        }

        public bool DoesUserExist(string userString)
        {
            return !string.IsNullOrEmpty(userString) && db.Security_Users.Any(x => x.UserName == userString || x.EmailAddress == userString);
        }

        public bool IsEmailConfirmed(string emailString, bool IsSigningUp)
        {
            if (string.IsNullOrEmpty(emailString))
            {
                return false;
            }
            return IsSigningUp || db.Security_Users.Any(x => (x.EmailAddress == emailString || x.UserName == emailString) && (x.IsEmailConfirmed || x.IsAdmin));
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            var user = db.Security_Users.FirstOrDefault(x => x.Guid == viewModel.GUID);
            if (user != null)
            {
                user.EmailAddress = viewModel.EmailAddress;
                user.IsActive = viewModel.UserIsActive;
                user.IsAdmin = viewModel.UserIsAdmin;
                user.UserName = string.IsNullOrEmpty(viewModel.UserName) ? viewModel.EmailAddress : viewModel.UserName;
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
            var userGuid = HttpContext.Current.Session["MainPageUserAuthID"] ?? Guid.NewGuid();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            var newUser = new Security_User()
            {
                ClientName = ip,
                EmailAddress = null,
                UserName = "Anon Visitor",
                Guid = (Guid)userGuid,
                IsActive = true,
                IsAdmin = IsUserToAddAnAdmin,
                IsEmailConfirmed = IsUserToAddAnAdmin
            };

            db.Security_Users.Add(newUser);
            db.SaveChanges();

            if (IsUserToAddAnAdmin)
            {
                GiveAdminTestData(newUser);
            }
            else
            {
                GiveUserTestData(newUser);
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
                UserIsActive = x.IsActive,
                IsConfirmationEmailSent = x.IsEmailConfirmationSent,
                UserIsLoggedIn = !x.IsEmailConfirmationSent && x.EmailAddress != null && x.EmailAddress != "",
                
            }).FirstOrDefault();
        }

        public void GiveUserTestData(Security_User newUser)
        {
            var guestUserId = db.Security_Users.Where(y => y.ClientName == "::1" && y.UserName == "Guest").Select(y => y.Id).FirstOrDefault();
            if (guestUserId == 0) return;
            var guestMyHomeConfig = db.MyHome_UserConfigs.FirstOrDefault(x => x.UserId == guestUserId);
            var guestMyHomeLinks = db.MyHome_SiteLinks.Where(x => x.UserId == guestUserId).ToList();
            var guestMyHomeBlogPosts = db.MyHome_BlogPosts.Where(x => x.UserId == guestUserId).ToList();

            if (guestMyHomeConfig != null)
            {
                var newHomeConfigRecord = new MyHome_UserConfig()
                {
                    BackgroundImage = guestMyHomeConfig.BackgroundImage,
                    BlogTitle = guestMyHomeConfig.BlogTitle,
                    BookmarksTitle = guestMyHomeConfig.BookmarksTitle,
                    ShowBanner = guestMyHomeConfig.ShowBanner,
                    ShowBlog = guestMyHomeConfig.ShowBlog,
                    ShowDateAndTime = guestMyHomeConfig.ShowDateAndTime,
                    ShowVisitorsAdminHome = guestMyHomeConfig.ShowVisitorsAdminHome,
                    ShowBookmarks = guestMyHomeConfig.ShowBookmarks,
                    ShowWeather = guestMyHomeConfig.ShowWeather,
                    Greeting = guestMyHomeConfig.Greeting,
                    DefaultBlogPostImage = guestMyHomeConfig.DefaultBlogPostImage,
                    DefaultFavoriteImage = guestMyHomeConfig.DefaultFavoriteImage,
                    AddNewFavoriteImage = guestMyHomeConfig.AddNewFavoriteImage,
                    WebsiteName = guestMyHomeConfig.WebsiteName,
                    UserId = newUser.Id,
                    IsEditable = true
                };
                db.MyHome_UserConfigs.Add(newHomeConfigRecord);
                db.SaveChanges();
            }
            if (guestMyHomeBlogPosts.Count > 0)
            {
                foreach (var post in guestMyHomeBlogPosts)
                {
                    var newPostRecord = new MyHome_BlogPost()
                    {
                        UserId = newUser.Id,
                        Image = post.Image,
                        Body = post.Body,
                        Title = post.Title
                    };
                    db.MyHome_BlogPosts.Add(newPostRecord);
                }
                db.SaveChanges();
            }
            if (guestMyHomeLinks.Count <= 0) return;
            foreach (var link in guestMyHomeLinks)
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

        public void GiveAdminTestData(Security_User newUser)
        {
            var guestUserId = db.Security_Users.Where(y => y.ClientName == "::1" && y.UserName == "Guest").Select(y => y.Id).FirstOrDefault();
            if (guestUserId != 0)
            {
                var guestMyHomeConfig = db.MyHome_UserConfigs.Where(x => x.UserId == guestUserId).FirstOrDefault();
                var guestMediaDashboardConfig = db.MediaDashboard_UserConfigs.Where(x => x.UserId == guestUserId).FirstOrDefault();
                var guestMyHomeLinks = db.MyHome_SiteLinks.Where(x => x.UserId == guestUserId).ToList();
                var guestMyHomeBlogPosts = db.MyHome_BlogPosts.Where(x => x.UserId == guestUserId).ToList();

                if(guestMediaDashboardConfig != null)
                {
                    var newMediaDashboardConfigRecord = new MediaDashboard_UserConfig()
                    {
                        SidebarCollapsedTitle = guestMediaDashboardConfig.SidebarCollapsedTitle,
                        SidebarColor = guestMediaDashboardConfig.SidebarColor,
                        UserId = newUser.Id,
                        SidebarFullTitle = guestMediaDashboardConfig.SidebarFullTitle,
                        BackgroundImage = guestMediaDashboardConfig.BackgroundImage,
                        SidebarAccentColor = guestMediaDashboardConfig.SidebarAccentColor,
                        WebsiteTitle = guestMediaDashboardConfig.WebsiteTitle
                    };
                    db.MediaDashboard_UserConfigs.Add(newMediaDashboardConfigRecord);
                    db.SaveChanges();
                }

                if (guestMyHomeConfig != null)
                {
                    var newHomeConfigRecord = new MyHome_UserConfig()
                    {
                        BackgroundImage = guestMyHomeConfig.BackgroundImage,
                        BlogTitle = guestMyHomeConfig.BlogTitle,
                        BookmarksTitle = guestMyHomeConfig.BookmarksTitle,
                        ShowBanner = guestMyHomeConfig.ShowBanner,
                        ShowBlog = guestMyHomeConfig.ShowBlog,
                        ShowDateAndTime = guestMyHomeConfig.ShowDateAndTime,
                        ShowVisitorsAdminHome = guestMyHomeConfig.ShowVisitorsAdminHome,
                        ShowBookmarks = guestMyHomeConfig.ShowBookmarks,
                        ShowWeather = guestMyHomeConfig.ShowWeather,
                        Greeting = guestMyHomeConfig.Greeting,
                        DefaultBlogPostImage = guestMyHomeConfig.DefaultBlogPostImage,
                        DefaultFavoriteImage = guestMyHomeConfig.DefaultFavoriteImage,
                        AddNewFavoriteImage = guestMyHomeConfig.AddNewFavoriteImage,
                        WebsiteName = guestMyHomeConfig.WebsiteName,
                        UserId = newUser.Id,
                        IsEditable = true
                    };
                    db.MyHome_UserConfigs.Add(newHomeConfigRecord);
                    db.SaveChanges();
                }
                if(guestMyHomeBlogPosts != null && guestMyHomeBlogPosts.Count > 0)
                {
                    foreach(var post in guestMyHomeBlogPosts)
                    {
                        var newPostRecord = new MyHome_BlogPost()
                        {
                            UserId = newUser.Id,
                            Image = post.Image,
                            Body = post.Body,
                            Title = post.Title
                        };
                        db.MyHome_BlogPosts.Add(newPostRecord);
                    }
                    db.SaveChanges();
                }
                if (guestMyHomeLinks != null && guestMyHomeLinks.Count > 0)
                {
                    foreach (var link in guestMyHomeLinks)
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
        }

        public MyHomeUserConfigViewModel GetUserConfigViewModelByUserId(int userId)
        {
            return db.MyHome_UserConfigs.Where(x => x.UserId == userId).Select(x => new MyHomeUserConfigViewModel()
            {
                BackgroundImage = x.BackgroundImage,
                BlogTitle = x.BlogTitle,
                BookmarksTitle = x.BookmarksTitle,
                Greeting = x.Greeting,
                ShowBanner = x.ShowBanner,
                ShowBlog = x.ShowBlog,
                ShowBookmarks = x.ShowBookmarks,
                ShowDateAndTime = x.ShowDateAndTime,
                ShowWeather = x.ShowWeather,
                UserID = x.UserId,
                IsEditable = x.IsEditable,
                ShowVisitorsAdminHome = x.ShowVisitorsAdminHome,
                DefaultFavoriteImage = x.DefaultFavoriteImage,
                DefaultBlogPostImage = x.DefaultBlogPostImage,
                AddNewFavoriteImage = x.AddNewFavoriteImage,
                WebsiteName = x.WebsiteName
            }).FirstOrDefault();
        }

        public void ConfirmAccount(Guid GUID)
        {
            var user = db.Security_Users.FirstOrDefault(x => x.Guid == GUID);
            if (user == null) return;
            user.IsEmailConfirmed = true;
            user.IsEmailConfirmationSent = false;
            db.SaveChanges();
        }

        public bool IsPasswordConfirmed(string pass1, string pass2)
        {
            return pass1 == pass2;
        }

    }
}
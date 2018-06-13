using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using devinmajordotcom.ViewModels;
using devinmajordotcom.Models;

namespace devinmajordotcom.Services
{
    public class MyHomeService : BaseDataService, IMyHomeService
    {

        public MyHomeViewModel GetMyHomeViewModel(int? userID = null)
        {
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            var adminUserConfig = GetUserConfigViewModelByUserId(siteAdminUser.Id);
            var siteGuestUser = db.Security_Users.FirstOrDefault(x => !x.IsActive && x.ClientName == "::1" && x.UserName == "Guest");
            var GuestUserConfig = GetUserConfigViewModelByUserId(siteGuestUser.Id);

            var guid = HttpContext.Current.Session["MainPageUserAuthID"];
            if (guid == null)
            {
                guid = AddNewUser().Guid;
                if (adminUserConfig.ShowVisitorsAdminHome)
                {
                    return new MyHomeViewModel()
                    {
                        CurrentUserViewModel = GetCurrentUser((Guid)guid),
                        UserConfig = adminUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteGuestUser.Id),
                        BlogPosts = GetBlogPostsByUserId(siteGuestUser.Id),
                        CanEdit = false,
                    };
                }
                else
                {
                    return new MyHomeViewModel()
                    {
                        CurrentUserViewModel = GetCurrentUser((Guid)guid),
                        UserConfig = GuestUserConfig,
                        FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(siteGuestUser.Id),
                        BlogPosts = GetBlogPostsByUserId(siteGuestUser.Id),
                        CanEdit = false,
                    };
                }
            }

            if(userID != null)
            {
                var dbUser = db.Security_Users.FirstOrDefault(x => x.Id == userID);
                var userViewModel = GetCurrentUser(dbUser.Guid);
                var newViewModel = new MyHomeViewModel()
                {
                    CurrentUserViewModel = userViewModel,
                    UserConfig = GetUserConfigViewModelByUserId(dbUser.Id),
                    FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(dbUser.Id),
                    BlogPosts = GetBlogPostsByUserId(dbUser.Id),
                    CanEdit = true
                };
                if(dbUser.UserName == "Guest" && dbUser.ClientName == "::1")
                {
                    newViewModel.UserConfig.IsEditingMasterSettings = true;
                }
                return newViewModel;
            }

            var user = GetCurrentUser((Guid)guid);
            return new MyHomeViewModel()
            {
                CurrentUserViewModel = user,
                UserConfig = GetUserConfigViewModelByUserId(user.UserID),
                FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(user.UserID),
                BlogPosts = GetBlogPostsByUserId(user.UserID),
                CanEdit = true
            };

        }

        public MyHomeMasterSettingsViewModel GetMasterSettingsViewModel()
        {

            var admin = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            var guest = db.Security_Users.FirstOrDefault(x => x.ClientName == "::1" && x.UserName == "Guest");

            var viewModel = new MyHomeMasterSettingsViewModel();

            if(admin != null)
            {
                var user = GetCurrentUser(admin.Guid);
                viewModel.AdminData = new MyHomeViewModel()
                {
                    CurrentUserViewModel = user,
                    UserConfig = GetUserConfigViewModelByUserId(admin.Id),
                    FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(admin.Id),
                    BlogPosts = GetBlogPostsByUserId(admin.Id)
                };
                viewModel.AdminData.UserConfig.IsAdmin = true;
            }
            if (guest != null)
            {
                var user = GetCurrentUser(guest.Guid);
                viewModel.GuestData = new MyHomeViewModel()
                {
                    CurrentUserViewModel = user,
                    UserConfig = GetUserConfigViewModelByUserId(guest.Id),
                    FavoritesAndBookmarks = GetFavoritesAndBookmarksByUserId(guest.Id),
                    BlogPosts = GetBlogPostsByUserId(guest.Id)
                };
                viewModel.GuestData.UserConfig.IsEditingMasterSettings = true;
            }

            return viewModel;
        }

        public void SetUserConfigViewModel(MyHomeUserConfigViewModel viewModel)
        {
            var config = db.MyHome_UserConfigs.FirstOrDefault(x => x.UserId == viewModel.UserID);

            if (config == null) return;
            config.BackgroundImage = viewModel.BackgroundImage;
            config.ShowWeather = viewModel.ShowWeather;
            config.ShowVisitorsAdminHome = viewModel.ShowVisitorsAdminHome;
            config.ShowDateAndTime = viewModel.ShowDateAndTime;
            config.ShowBookmarks = viewModel.ShowBookmarks;
            config.ShowBanner = viewModel.ShowBanner;
            config.IsEditable = viewModel.IsEditable;
            config.ShowBlog = viewModel.ShowBlog;
            config.BlogTitle = viewModel.BlogTitle;
            config.BookmarksTitle = viewModel.BookmarksTitle;
            config.Greeting = viewModel.Greeting;
            config.WebsiteName = viewModel.WebsiteName;
            //config.DefaultFavoriteImage = viewModel.BackgroundImage;
            //config.DefaultBlogPostImage = viewModel.DefaultBlogPostImage;
            //config.AddNewFavoriteImage = viewModel.AddNewFavoriteImage;
            db.SaveChanges();
        }
        
        public EditFavoritesViewModel GetEditFavoritesViewModel(int userID)
        {

            var favorites = db.MyHome_SiteLinks.Where(x => x.UserId == userID).Select(x => new DropDownViewModel()
            {
                ID = x.Id,
                Name = x.DisplayName
            }).ToList();

            return new EditFavoritesViewModel()
            {
                UserID = userID,
                AvailableFavorites = favorites,
                SelectedFavoriteID = favorites.Select(x => x.ID).FirstOrDefault()
            };

        }

        public EditBlogPostsViewModel GetEditBlogPostsViewModel(int userID)
        {

            var blogPosts = db.MyHome_BlogPosts.Where(x => x.UserId == userID).Select(x => new DropDownViewModel()
            {
                ID = x.Id,
                Name = x.Title
            }).ToList();

            return new EditBlogPostsViewModel()
            {
                UserID = userID,
                AvailableBlogPosts = blogPosts,
                SelectedBlogPostID = blogPosts.Select(x => x.ID).FirstOrDefault()
            };

        }

        public List<SiteLinkViewModel> GetFavoritesAndBookmarksByUserId(int userId)
        {
            return db.MyHome_SiteLinks.Where(x => x.UserId == userId).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                Controller = x.Controller,
                Action = x.Action,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                DisplayName = x.DisplayName,
                UserID = x.UserId,
                Order = x.Order,
                URL = x.Url,
                BackgroundImage = x.Image
            }).ToList();
        }

        public SiteLinkViewModel GetFavoriteByID(int ID)
        {
            return db.MyHome_SiteLinks.Where(x => x.Id == ID).Select(x => new SiteLinkViewModel()
            {
                Controller = x.Controller,
                ID = x.Id,
                IsDefault = x.IsDefault,
                DisplayName = x.DisplayName,
                Action = x.Action,
                IsEnabled = x.IsEnabled,
                Order = x.Order,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                UserID = x.UserId,
                BackgroundImage = x.Image
            }).FirstOrDefault();
        }

        public SiteLinkViewModel GetNewFavoriteViewModel(int userID)
        {
            var newFavorite = new SiteLinkViewModel()
            {
                UserID = userID,
                IsEnabled = true
            };
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            if (siteAdminUser == null) return newFavorite;
            var adminUserConfig = GetUserConfigViewModelByUserId(siteAdminUser.Id);
            if (adminUserConfig != null)
            {
                newFavorite.BackgroundImage = adminUserConfig.DefaultFavoriteImage;
            }
            return newFavorite;
        }

        public BlogPostViewModel GetNewBlogPostViewModel(int userID)
        {
            var newBlogPost = new BlogPostViewModel()
            {
                AuthorUserID = userID
            };
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            if (siteAdminUser == null) return newBlogPost;
            var adminUserConfig = GetUserConfigViewModelByUserId(siteAdminUser.Id);
            if (adminUserConfig != null)
            {
                newBlogPost.BackgroundImage = adminUserConfig.DefaultBlogPostImage;
            }
            return newBlogPost;
        }

        public int RemoveFavoriteByID(int ID)
        {
            var item = db.MyHome_SiteLinks.FirstOrDefault(x => x.Id == ID);
            var userId = item.UserId;
            if (item != null)
            {
                db.MyHome_SiteLinks.Remove(item);
            }
            db.SaveChanges();
            return userId;
        }

        public int RemoveBlogPostByID(int ID)
        {
            var item = db.MyHome_BlogPosts.FirstOrDefault(x => x.Id == ID);
            var userId = item.UserId;
            if (item != null)
            {
                var comments = db.MyHome_BlogPostComments.Where(x => x.BlogPostId == item.Id);
                foreach (var comment in comments)
                {
                    db.MyHome_BlogPostComments.Remove(comment);
                }
                db.SaveChanges();
                db.MyHome_BlogPosts.Remove(item);
            }
            db.SaveChanges();
            return userId;
        }

        public void AddEditBlogPost(BlogPostViewModel viewModel)
        {
            var record = db.MyHome_BlogPosts.FirstOrDefault(x => x.Id == viewModel.BlogPostID);
            if (record != null)
            {
                record.Title = viewModel.PostTitle;
                record.Body = viewModel.Body;
                record.Image = viewModel.BackgroundImage;
            }
            else
            {
                var newRecord = new MyHome_BlogPost()
                {
                    UserId = viewModel.AuthorUserID,
                    Body = viewModel.Body,
                    Title = viewModel.PostTitle,
                    Image = viewModel.BackgroundImage
                };
                db.MyHome_BlogPosts.Add(newRecord);
            }
            db.SaveChanges();
        }

        public void AddEditFavorite(SiteLinkViewModel viewModel)
        {
            var record = db.MyHome_SiteLinks.FirstOrDefault(x => x.Id == viewModel.ID);
            
            if (record != null)
            {
                record.Image = viewModel.BackgroundImage;
                record.UserId = viewModel.UserID;
                record.Controller = viewModel.Controller;
                record.Id = viewModel.ID;
                record.IsDefault = viewModel.IsDefault;
                record.DisplayName = viewModel.DisplayName;
                record.Action = viewModel.Action;
                record.IsEnabled = viewModel.IsEnabled;
                record.Order = viewModel.Order;
                record.Description = viewModel.Description;
                record.Directive = viewModel.Directive;
                record.DisplayIcon = viewModel.DisplayIcon;
                record.Url = viewModel.URL;
            }
            else
            {
                var newRecord = new MyHome_SiteLink()
                {
                    UserId = viewModel.UserID,
                    Controller = viewModel.Controller,
                    Id = viewModel.ID,
                    IsDefault = viewModel.IsDefault,
                    DisplayName = viewModel.DisplayName,
                    Action = viewModel.Action,
                    IsEnabled = viewModel.IsEnabled,
                    Order = viewModel.Order,
                    Description = viewModel.Description,
                    Directive = viewModel.Directive,
                    DisplayIcon = viewModel.DisplayIcon,
                    Url = viewModel.URL,
                    Image = viewModel.BackgroundImage
                };
                db.MyHome_SiteLinks.Add(newRecord);
            }
            db.SaveChanges();
        }

        public void AddComment(CommentViewModel viewModel)
        {

            var newRecord = new MyHome_BlogPostComment()
            {
                BlogPostId = viewModel.BlogPostID,
                UserId = viewModel.AuthorUserID,
                CommentBody = viewModel.Body
            };
            db.MyHome_BlogPostComments.Add(newRecord);
            db.SaveChanges();

        }

        public List<CommentViewModel> GetCommentsByBlogPostID(int ID)
        {

            var guid = HttpContext.Current.Session["MainPageUserAuthID"] ?? AddNewUser().Guid;
            var user = GetCurrentUser((Guid)guid);

            return db.MyHome_BlogPostComments.Where(x => x.BlogPostId == ID).Select(x => new CommentViewModel() {
                AuthorUserID = x.UserId,
                AuthorUserName = db.Security_Users.Where(y => y.Id == x.UserId).Select(y => y.UserName).FirstOrDefault(),
                Body = x.CommentBody,
                BlogPostID = x.BlogPostId,
                IsCurrentUserCommenterOrAdmin = user.UserIsAdmin || user.UserID == x.UserId,
                CommentID = x.Id,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn
            }).ToList();
        }

        public BlogPostViewModel GetBlogPostById(int ID)
        {
            var viewModel = new BlogPostViewModel();

            var guid = HttpContext.Current.Session["MainPageUserAuthID"] ?? AddNewUser().Guid;
            var user = GetCurrentUser((Guid) guid);

            if (user != null && viewModel != null)
            {
                var blogPost = db.MyHome_BlogPosts.Where(x => x.Id == ID).FirstOrDefault();

                if(blogPost != null)
                {
                    var poster = db.Security_Users.FirstOrDefault(x => x.Id == blogPost.UserId);
                    viewModel = new BlogPostViewModel()
                    {
                        BlogPostID = blogPost.Id,
                        AuthorUserID = blogPost.UserId,
                        AuthorUserName = db.Security_Users.Where(t => t.Id == blogPost.UserId).Select(t => t.UserName).FirstOrDefault(),
                        PostTitle = blogPost.Title,
                        BackgroundImage = blogPost.Image,
                        Body = blogPost.Body,
                        CreatedBy = blogPost.CreatedBy,
                        CreatedOn = blogPost.CreatedOn,
                        ModifiedBy = blogPost.ModifiedBy,
                        ModifiedOn = blogPost.ModifiedOn,
                        PostComments = blogPost.MyHome_BlogPostComments.Select(y => new CommentViewModel()
                        {
                            CommentID = y.Id,
                            AuthorUserID = y.UserId,
                            AuthorUserName = db.Security_Users.Where(z => z.Id == y.UserId).Select(z => z.UserName).FirstOrDefault(),
                            Body = y.CommentBody,
                            BackgroundImage = y.Image,
                            IsCurrentUserCommenterOrAdmin = user.UserIsAdmin || user.UserID == poster.Id,
                            CreatedOn = y.CreatedOn,
                            CreatedBy = y.CreatedBy,
                            ModifiedOn = y.ModifiedOn,
                            ModifiedBy = y.ModifiedBy
                        }).OrderBy(y => y.CreatedOn).ToList(),
                    };
                    viewModel.NewComment = new CommentViewModel()
                    {
                        AuthorUserID = user.UserID,
                        BlogPostID = viewModel.BlogPostID
                    };
                }
            }

            return viewModel;
        }

        public List<BlogPostViewModel> GetBlogPostsByUserId(int userId)
        {
            return db.MyHome_BlogPosts.Where(x => x.UserId == userId).Select(x => new BlogPostViewModel()
            {
                BlogPostID = x.Id,
                AuthorUserName = db.Security_Users.Where(y => y.Id == x.UserId).Select(y => y.UserName).FirstOrDefault(),
                AuthorUserID = x.UserId,
                PostTitle = x.Title,
                BackgroundImage = x.Image,
                Body = x.Body,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn
            }).OrderBy(y => y.CreatedOn).ToList();
        }

        public void RemoveCommentById(int ID)
        {
            var recordToRemove = db.MyHome_BlogPostComments.FirstOrDefault(x => x.Id == ID);
            if (recordToRemove != null)
            {
                db.MyHome_BlogPostComments.Remove(recordToRemove);
            }
            db.SaveChanges();
        }

    }
}
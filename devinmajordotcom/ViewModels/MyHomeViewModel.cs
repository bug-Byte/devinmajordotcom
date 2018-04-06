using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.ViewModels
{

    public class MyHomeViewModel
    {

        public UserViewModel CurrentUserViewModel { get; set; }

        public MyHomeUserConfigViewModel UserConfig { get; set; }

        public List<SiteLinkViewModel> FavoritesAndBookmarks { get; set; }

        public List<BlogPostViewModel> BlogPosts { get; set; }

        public bool CanEdit { get; set; }

    }

    public class MyHomeMasterSettingsViewModel
    {

        public MyHomeViewModel GuestData { get; set; }

        public MyHomeViewModel AdminData { get; set; }

    }

    public class MyHomeUserConfigViewModel
    {
        public int UserID { get; set; }

        [DisplayName("Show Date & Time: ")]
        public bool ShowDateAndTime { get; set; }

        [DisplayName("Show Weather: ")]
        public bool ShowWeather { get; set; }

        [DisplayName("Show Banner: ")]
        public bool ShowBanner { get; set; }

        [DisplayName("Show Bookmarks: ")]
        public bool ShowBookmarks { get; set; }

        [DisplayName("Show Blog: ")]
        public bool ShowBlog { get; set; }

        [DisplayName("Website Title: ")]
        public string WebsiteName { get; set; }

        [DisplayName("Bookmarks Title: ")]
        public string BookmarksTitle { get; set; }

        [DisplayName("Greeting: ")]
        public string Greeting { get; set; }

        [DisplayName("Blog Title: ")]
        public string BlogTitle { get; set; }

        [DisplayName("Background Image: ")]
        public byte[] BackgroundImage { get; set; }

        [DisplayName("Use Admin settings for visitors?")]
        public bool ShowVisitorsAdminHome { get; set; }

        public bool IsEditingMasterSettings { get; set; }

        [DisplayName("Show Editors:")]
        public bool IsEditable { get; set; }

        public bool IsAdmin { get; set; }

        public byte[] DefaultFavoriteImage { get; set; }

        public byte[] DefaultBlogPostImage { get; set; }

        public byte[] AddNewFavoriteImage { get; set; }

    }

    public class EditFavoritesViewModel
    {

        public int UserID { get; set; }

        public int SelectedFavoriteID { get; set; }

        public List<DropDownViewModel> AvailableFavorites { get; set; }

    }

    public class EditBlogPostsViewModel
    {

        public int UserID { get; set; }

        public int SelectedBlogPostID { get; set; }

        public List<DropDownViewModel> AvailableBlogPosts { get; set; }

    }

    public class BlogPostViewModel : CommentViewModel
    {
        [Required]
        [DisplayName("Title *")]
        public string PostTitle { get; set; }

        public List<CommentViewModel> PostComments { get; set; }

        public CommentViewModel NewComment { get; set; }

    }

    public class CommentViewModel
    {

        public int BlogPostID { get; set; }

        public int AuthorUserID { get; set; }

        public string AuthorUserName { get; set; }

        [AllowHtml]
        [DisplayName("Body Content")]
        public string Body { get; set; }

        [DisplayName("Image")]
        public byte[] BackgroundImage { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }

}
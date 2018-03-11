using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    public class MyHomeUserConfigViewModel
    {
        public int UserID { get; set; }

        public bool ShowDateAndTime { get; set; }

        public bool ShowWeather { get; set; }

        public bool ShowBanner { get; set; }

        public bool ShowBookmarks { get; set; }

        public bool ShowBlog { get; set; }

        public string BookmarksTitle { get; set; }

        public string Greeting { get; set; }

        public string BlogTitle { get; set; }

        public byte[] BackgroundImage { get; set; }

        public bool? ShowVisitorsAdminHome { get; set; }

        public bool IsEditable { get; set; }

    }

    public class BlogPostViewModel : CommentViewModel
    {

        public int BlogPostID { get; set; }

        public string PostTitle { get; set; }

        public List<CommentViewModel> PostComments { get; set; }

    }

    public class CommentViewModel
    {

        public int AuthorUserID { get; set; }

        public string Body { get; set; }

        public byte[] Image { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

    }

}
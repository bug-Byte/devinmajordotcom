using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.ViewModels
{
    public class ContactEmailViewModel
    {

        public string RecipientEmail { get; set; }

        public string RecipientName { get; set; }

        [Required(ErrorMessage = "Your Name is required!")]
        [DisplayName("Your Name * : ")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Your Email Address is required!")]
        [DisplayName("Your Email Address * : ")]
        public string SenderEmailAddress { get; set; }

        [DisplayName("Your Subject : ")]
        public string Subject { get; set; }

        public Guid UserGUID { get; set; }

        public int EmailTypeID { get; set; }

        [AllowHtml]
        [DisplayName("Your Email Body * : ")]
        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.ViewModels
{
    public class ContactEmailViewModel
    {

        public string RecipientEmail { get; set; }

        public string RecipientName { get; set; }

        public string SenderName { get; set; }

        public string SenderEmailAddress { get; set; }

        public string Subject { get; set; }

        public Guid UserGUID { get; set; }

        public int EmailTypeID { get; set; }

        [AllowHtml]
        public string Content { get; set; }

    }
}
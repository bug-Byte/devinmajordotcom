using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class SiteLinkViewModel
    {

        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        [DisplayName("Display Name *")]
        public string DisplayName { get; set; }

        public string Color { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public string Directive { get; set; }

        [Required]
        [DisplayName("URL")]
        public string URL { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public string DisplayIcon { get; set; }

        public bool IsDefault { get; set; }

        public bool IsPublic { get; set; }

        [DisplayName("Is Link Enabled?")]
        public bool IsEnabled { get; set; }

        public int ParentApplicationId { get; set; }

        public string ParentApplicationName { get; set; }

        public int? Order { get; set; }

        [DisplayName("Background Image *")]
        public string BackgroundImage { get; set; }

        public SiteLinkViewModel()
        {
            
        }

    }
}
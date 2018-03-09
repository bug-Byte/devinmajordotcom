using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class SiteLinkViewModel
    {

        public int ID { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Directive { get; set; }

        [Required]
        public string URL { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public string DisplayIcon { get; set; }

        public bool IsDefault { get; set; }

        public bool IsPublic { get; set; }

        public bool IsEnabled { get; set; }

        public int ParentApplicationId { get; set; }

        public string ParentApplicationName { get; set; }

        public int? Order { get; set; }

        public byte[] EncodedImage { get; set; }

        public SiteLinkViewModel()
        {
            
        }

    }
}
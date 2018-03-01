using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public int UserID { get; set; }

        [DisplayName("User Name (optional): ")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email Address * : ")]
        public string EmailAddress { get; set; }

        [Required]
        public Guid GUID { get; set; }

        [Required]
        [DisplayName("Password * : ")]
        public string Password { get; set; }

        [Required]
        public bool UserIsAdmin { get; set; }

        [Required]
        public bool UserIsActive { get; set; }

        public UserViewModel()
        {

        }

    }
}
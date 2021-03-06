﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public int UserID { get; set; }

        [DisplayName("User Name: ")]
        [Remote("VerifyUserExists", "Validation", AdditionalFields = "IsSigningUp,IsUpdatingCredentials,EmailAddress")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email Address * : ")]
        [Remote("VerifyEmail", "Validation", AdditionalFields = "IsSigningUp,IsUpdatingCredentials")]
        public string EmailAddress { get; set; }

        [Required]
        public Guid GUID { get; set; }

        [Required]
        [DisplayName("Password * : ")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password * : ")]
        [Remote("VerifyPassword", "Validation", AdditionalFields = "Password")]
        public string ConfirmedPassword { get; set; }

        [Required]
        public bool UserIsAdmin { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        public bool UserIsActive { get; set; }

        public bool UserIsLoggedIn { get; set; }

        [Required]
        public bool IsSigningUp { get; set; }

        [Required]
        public bool IsUpdatingCredentials { get; set; }

        public bool IsConfirmationEmailSent { get; set; }

        public UserViewModel()
        {

        }

    }

    public class UserValidationViewModel
    {
        
        public UserViewModel User { get; set; }

        public string LoginAttemptStatus { get; set; }

    }

}
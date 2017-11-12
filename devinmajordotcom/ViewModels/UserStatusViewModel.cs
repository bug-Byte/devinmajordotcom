using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{
    public class UserStatusViewModel
    {

        public bool UserIsAdmin { get; set; }

        public bool UserIsActive { get; set; }

        public UserStatusViewModel()
        {

        }

    }
}
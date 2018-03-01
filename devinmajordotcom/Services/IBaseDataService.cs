using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;

namespace devinmajordotcom.Services
{
    public interface IBaseDataService
    {

        UserViewModel Login(UserViewModel viewModel, bool IsAdmin = false);

        UserValidationViewModel ValidateCredentials(string emailAddress, string password, string userName = null);

        void UpdateCurrentUser(UserViewModel viewModel);

        User AddNewUser(bool IsUserToAddAnAdmin = false);

        UserViewModel GetCurrentUser(Guid? GUID = null);

    }
}

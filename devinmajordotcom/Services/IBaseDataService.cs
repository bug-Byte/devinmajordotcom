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

        void Logout();

        bool DoesUserExist(string userString);

        bool IsEmailConfirmed(string emailString, bool IsSigningUp);

        UserViewModel LookupUser(string userString);

        void SetConfirmationEmailSent(UserViewModel viewModel);

        UserValidationViewModel ValidateCredentials(string userString, string password, string emailAddress = null);

        void UpdateCurrentUser(UserViewModel viewModel);

        Security_User AddNewUser(bool IsUserToAddAnAdmin = false);

        UserViewModel GetCurrentUser(Guid? GUID = null);

        void GiveAdminTestData(Security_User newUser);

        void ConfirmAccount(Guid GUID);

        bool IsPasswordConfirmed(string pass1, string pass2);

    }
}

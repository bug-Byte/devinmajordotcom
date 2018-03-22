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

        UserViewModel FindUser();

        bool DoesUserExist(string userString);

        UserValidationViewModel ValidateCredentials(string userString, string password, string emailAddress = null);

        void UpdateCurrentUser(UserViewModel viewModel);

        Security_User AddNewUser(bool IsUserToAddAnAdmin = false);

        UserViewModel GetCurrentUser(Guid? GUID = null);

        void GiveAdminTestData(Security_User newUser);

    }
}

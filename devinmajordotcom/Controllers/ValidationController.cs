using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using devinmajordotcom.Services;

namespace devinmajordotcom.Controllers
{

    public class ValidationController : BaseController
    {

        [HttpGet]
        public ActionResult VerifyUserExists(string UserName, bool IsSigningUp)
        {
            var result1 = landingPageService.DoesUserExist(UserName);
            var result2 = landingPageService.IsEmailConfirmed(UserName, IsSigningUp);
            if (IsSigningUp)
            {
                return result1 ? Json($"The user \"{UserName}\" already exists in the system.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
            }
            if (!result2 && result1)
            {
                return Json($"The email address for this account has not been confirmed yet. Check your inbox!", JsonRequestBehavior.AllowGet);
            }
            return result1 ? Json(true, JsonRequestBehavior.AllowGet) : Json($"The user \"{UserName}\" does not exist in the system.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerifyEmail(string EmailAddress, bool IsSigningUp)
        {
            var result1 = landingPageService.DoesUserExist(EmailAddress);
            var result2 = landingPageService.IsEmailConfirmed(EmailAddress, IsSigningUp);
            if (result2)
            {
                return result1 ? Json($"The email address \"{EmailAddress}\" is already in use!", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json("Your account has not been activated! Please confirm your email address before signing in.", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult VerifyPassword(string ConfirmedPassword, string Password)
        {
/*            var result1 = landingPageService.DoesPasswordMatchOldPassword(ConfirmedPassword)*/;
            var result2 = landingPageService.IsPasswordConfirmed(ConfirmedPassword, Password);
            //if(result1)
            //{
            //    return Json("The password you have typed matches an older password. Please try again!", JsonRequestBehavior.AllowGet);
            //}
            return result2 ? Json(true, JsonRequestBehavior.AllowGet) : Json("The passwords you have typed do not match. Please type carefully!", JsonRequestBehavior.AllowGet);

        }

    }

}
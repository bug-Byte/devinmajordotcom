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
        public ActionResult VerifyUserExists(string UserName, bool IsSigningUp, bool IsUpdatingCredentials, string EmailAddress)
        {
            var result1 = landingPageService.DoesUserExist(UserName);
            var result2 = landingPageService.IsEmailConfirmed(UserName);
            if (IsSigningUp || IsUpdatingCredentials)
            {
                if (result1)
                {
                    if (IsUpdatingCredentials)
                    {
                        var result3 = landingPageService.DoesEmailAccountMatchUserName(EmailAddress, UserName);
                        if(!result3)
                        {
                            return Json($"The user \"{UserName}\" already exists in the system.", JsonRequestBehavior.AllowGet);
                        }
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    return Json($"The user \"{UserName}\" already exists in the system.", JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (result1)
            {
                if(result1 && !result2)
                {
                    return Json($"The email address for this account has not been confirmed yet. Check your inbox!", JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json($"The user \"{UserName}\" does not exist in the system.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerifyEmail(string EmailAddress, bool IsSigningUp, bool IsUpdatingCredentials)
        {
            var result1 = landingPageService.DoesUserExist(EmailAddress);
            var result2 = landingPageService.IsEmailConfirmed(EmailAddress);
            if(result1 && IsSigningUp)
            {
                return Json($"The email address \"{EmailAddress}\" is already in use!", JsonRequestBehavior.AllowGet);
            }
            if (!result2 && !IsUpdatingCredentials && !IsSigningUp)
            {
                return Json("Your account has not been activated! Please confirm your email address before signing in.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerifyPassword(string ConfirmedPassword, string Password)
        {
            var result = ConfirmedPassword == Password;
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json("The passwords you have typed do not match. Please type carefully!", JsonRequestBehavior.AllowGet);
        }

    }

}
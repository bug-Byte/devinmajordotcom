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
            var result = landingPageService.DoesUserExist(UserName);
            if (IsSigningUp)
            {
                return result ? Json($"The user \"{UserName}\" already exists in the system.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
            }
            return result ? Json(true, JsonRequestBehavior.AllowGet) : Json($"The user \"{UserName}\" does not exist in the system.", JsonRequestBehavior.AllowGet);
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

    }

}
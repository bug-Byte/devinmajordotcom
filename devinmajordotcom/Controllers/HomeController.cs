using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System.Text;
using System.Net.Mail;
using devinmajordotcom.Helpers;

namespace devinmajordotcom.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            var viewModel = new MainLandingPageViewModel();
            viewModel = landingPageService.GetLandingPageViewModel();

            this.ViewData["CurrentUserViewModel"] = viewModel.CurrentUserViewModel;
            this.ViewData["BannerLinks"] = viewModel.LandingPageBannerLinks;
            this.ViewData["Config"] = viewModel.Config;
            ViewBag.Title = "D3V!N M@J0R";
            ViewBag.ControllerName = "Home";

            if (viewModel.CurrentUserViewModel != null && viewModel.CurrentUserViewModel.UserIsActive)
            {
                return View(viewModel);
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                return new HttpStatusCodeResult(401);
            }
        }

        [HttpPost]
        public void _HomeSettingsForm(MyHomeUserConfigViewModel viewModel)
        {
            myHomeService.SetUserConfigViewModel(viewModel);
        }

        public ActionResult _ApplicationManager()
        {
            ViewBag.Title = "D3V!N M@J0R";
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetServerData(MainLandingPageViewModel viewModel)
        {
            var result = hardwareService.GetServerData(viewModel.SelectedHardwareTypeID, viewModel.SelectedDateRangeID);
            return new JsonResult { Data = result };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void ManageLandingPage(MainLandingPageViewModel viewModel)
        {
            landingPageService.ManageLandingPage(viewModel);
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            var doesEmailMatchAccount = landingPageService.DoesFormEmailMatchRecordEmail(viewModel);
            landingPageService.UpdateCurrentUser(viewModel);
            
            if (viewModel.IsSigningUp)
            {
                if(viewModel.UserIsAdmin)
                {
                    SendUpdateCredentialsEmail(viewModel);
                }
                else
                {
                    SendConfirmationEmail(viewModel);
                }
            }
            else if (viewModel.IsUpdatingCredentials)
            {
                if (!doesEmailMatchAccount)
                {
                    SendConfirmationEmail(viewModel);
                    Logout();
                }
                else
                {
                    SendUpdateCredentialsEmail(viewModel);
                }
            }
        }

        [HttpGet]
        public void RemoveBannerLink(int ID)
        {
            var userguid = Session["MainPageUserAuthID"];
            var admin = landingPageService.GetAdmin();
            if(userguid != null && (Guid)userguid == admin.GUID)
            {
                landingPageService.RemoveBannerLinkById(ID);
            }
        }

        [HttpGet]
        public void RemoveSiteLink(int ID)
        {
            var userguid = Session["MainPageUserAuthID"];
            var admin = landingPageService.GetAdmin();
            if (userguid != null && (Guid)userguid == admin.GUID)
            {
                landingPageService.RemoveSiteLinkById(ID);
            }
        }

    }
}
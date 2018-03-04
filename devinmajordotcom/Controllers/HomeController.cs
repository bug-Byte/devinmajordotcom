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

namespace devinmajordotcom.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            var viewModel = new MainLandingPageViewModel();
            viewModel = landingPageService.GetLandingPageViewModel();

            this.ViewData["CurrentUserViewModel"] = viewModel.CurrentUserViewModel;
            this.ViewData["BannerLinks"] = viewModel.LandingPageBannerLinks;
            ViewBag.Title = "D3V!N M@J0R";
            ViewBag.ControllerName = "Home";

            if (viewModel.CurrentUserViewModel.UserIsActive)
            {
                return View(viewModel);
            }
            else
            {
                throw new Exception("Unauthorized User", new UnauthorizedAccessException());
            }
        }
       
        public void ManageLandingPage(MainLandingPageViewModel viewModel)
        {
            landingPageService.ManageLandingPage(viewModel);
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            landingPageService.UpdateCurrentUser(viewModel);
        }

        public ActionResult _ApplicationManager()
        {
            ViewBag.Title = "D3V!N M@J0R";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DropMeALine(ContactEmailViewModel viewModel)
        {
            var emailSuccessful = "";
            if (ModelState.IsValid)
            {
                emailSuccessful = landingPageService.SendContactEmailToSiteAdmin(viewModel);
            }
            return new JsonResult { Data = emailSuccessful };
        }

    }
}
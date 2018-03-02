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
    public class HomeController : Controller
    {

        public ILandingPageService service;

        public HomeController(ILandingPageService landingPageService)
        {
            service = landingPageService;
        }

        public ActionResult Index()
        {
            var viewModel = new MainLandingPageViewModel();
            viewModel = service.GetLandingPageViewModel();

            this.ViewData["CurrentUserViewModel"] = viewModel.CurrentUserViewModel;
            ViewBag.Title = "D3V!N M@J0R";
            ViewBag.Layout = "../Shared/_Layout.cshtml";

            if (viewModel.CurrentUserViewModel.UserIsActive)
            {

                if (Session["MainPageUserAuthID"] == null || (Guid)Session["MainPageUserAuthID"] != viewModel.CurrentUserViewModel.GUID)
                {
                    Session["MainPageUserAuthID"] = viewModel.CurrentUserViewModel.GUID;
                    Session["MainPageUserName"] = viewModel.CurrentUserViewModel.UserName;
                    //var cookie = new HttpCookie("UserData");
                    //cookie.Values["UserName"] = viewModel.CurrentUserViewModel.UserName;
                    //cookie.Values["UserEmail"] = viewModel.CurrentUserViewModel.EmailAddress;
                    //cookie.Expires = DateTime.Now.AddMinutes(2);
                    //Response.Cookies.Add(cookie);
                }

                viewModel.CurrentApplicationData.LandingPageApplicationLinks = viewModel.LandingPageApplicationLinks;      
                return View(viewModel);

            }
            else
            {
                throw new Exception("Unauthorized User", new UnauthorizedAccessException());
            }
        }

        [HttpPost]
        public ActionResult AdminLogin(UserViewModel viewModel)
        {
            var validatedUser = service.Login(viewModel, true);
            return RedirectToAction("Index");
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            service.UpdateCurrentUser(viewModel);
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
                emailSuccessful = service.SendContactEmailToSiteAdmin(viewModel);
            }
            return new JsonResult { Data = emailSuccessful };
        }

    }
}
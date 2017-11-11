using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;

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
                return View(viewModel);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult _ApplicationManager()
        {
            ViewBag.Title = "Application Manager";
            return PartialView();
        }

    }
}
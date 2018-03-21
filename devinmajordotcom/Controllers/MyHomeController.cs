using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class MyHomeController : BaseController
    {
        // GET: MyHome
        public ActionResult Index()
        {
            ViewBag.Title = "D3V!N M@J0R";
            ViewBag.ControllerName = "MyHome";
            var viewModel = myHomeService.GetMyHomeViewModel();
            return View(viewModel);
        }

        public ActionResult BlogPost(int ID)
        {
            var viewModel = myHomeService.GetBlogPostById(ID);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult _HomeSettingsForm(MyHomeUserConfigViewModel viewModel)
        {
            myHomeService.SetUserConfigViewModel(viewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCurrentUser(UserViewModel viewModel)
        {
            landingPageService.UpdateCurrentUser(viewModel);
            Login(viewModel);
            return RedirectToAction("Index");
        }



    }
}
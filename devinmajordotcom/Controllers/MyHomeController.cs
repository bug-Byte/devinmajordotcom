using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using devinmajordotcom.Models;

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

        [HttpGet]
        public ActionResult _EditFavorites(int userID)
        {
            var viewModel = myHomeService.GetEditFavoritesViewModel(userID);
            return PartialView(viewModel);
        }

        [HttpGet]
        public ActionResult _EditBlogPosts(int userID)
        {
            var viewModel = myHomeService.GetEditFavoritesViewModel(userID);
            return PartialView(viewModel);
        }

        [HttpGet]
        public ActionResult _AddFavoriteForm(int userID)
        {
            var newViewModel = myHomeService.GetNewFavoriteViewModel(userID);
            return PartialView(newViewModel);
        }

        [HttpPost]
        public ActionResult _EditFavoriteForm(EditFavoritesViewModel viewModel)
        {
            var newViewModel = myHomeService.GetFavoriteByID(viewModel.SelectedFavoriteID);
            return PartialView(newViewModel);
        }

        [HttpPost]
        public ActionResult AddEditFavorite(SiteLinkViewModel viewModel)
        {
            myHomeService.AddEditFavorite(viewModel);
            var newViewModel = myHomeService.GetMyHomeViewModel();
            return PartialView("_Favorites", newViewModel);
        }

        [HttpGet]
        public ActionResult RemoveFavoriteByID(int ID)
        {
            int userID = myHomeService.RemoveFavoriteByID(ID);
            var newViewModel = myHomeService.GetMyHomeViewModel();
            return PartialView("_Favorites", newViewModel);
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
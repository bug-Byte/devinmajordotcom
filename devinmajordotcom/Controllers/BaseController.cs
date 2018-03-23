using devinmajordotcom.Helpers;
using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace devinmajordotcom.Controllers
{
    public class BaseController : Controller
    {

        public ILandingPageService landingPageService;
        public IPortfolioService portfolioService;
        public IMediaDashboardService mediaDashboardService;
        public IMyHomeService myHomeService;

        protected RedirectResult RedirectToAction<T>(Expression<Action<T>> action, RouteValueDictionary values = null) where T : Controller
        {
            return new RedirectResult(RedirectionHelper.GetUrl(action, Request.RequestContext, values));
        }

        public BaseController()
        {
            landingPageService = DependencyResolver.Current.GetService<ILandingPageService>();
            portfolioService = DependencyResolver.Current.GetService<IPortfolioService>();
            mediaDashboardService = DependencyResolver.Current.GetService<IMediaDashboardService>();
            myHomeService = DependencyResolver.Current.GetService<IMyHomeService>();
        }

        [HttpGet]
        public ActionResult UploadTemplate()
        {
            ViewBag.ControllerName = ControllerContext.RouteData.Values["controller"].ToString();
            return PartialView("_ImageUploader");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileWrapper qqfile)
        {
            if (qqfile == null || qqfile.ContentLength <= 0)
            {
                return Json(new { success = false, message = "Could not upload file: File was empty!" });
            }

            try
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(qqfile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(qqfile.ContentLength);
                }
                var base64ConvertedFile = Convert.ToBase64String(fileData);
                return Json(new { success = true, message = "File successfully uploaded. Dont forget to save your changes in the settings menu!", file = base64ConvertedFile });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Could not upload file: There was a problem with the file!" });
            }
        }

        [HttpPost]
        public ActionResult AdminLogin(UserViewModel viewModel)
        {
            var validatedUser = landingPageService.Login(viewModel, true);
            if (validatedUser.UserID == 0 || !validatedUser.UserIsAdmin)
            {
                return new HttpStatusCodeResult(500);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(UserViewModel viewModel, bool IsAdmin = false)
        {
            var validatedUser = landingPageService.Login(viewModel, IsAdmin);
            if(validatedUser.UserID == 0)
            {
                return new HttpStatusCodeResult(500);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            landingPageService.Logout();
            return RedirectToAction("Index");
        }

    }
}
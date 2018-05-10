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
using devinmajordotcom.Models;

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
            var controller = ControllerContext.RouteData.Values["controller"].ToString();
            ViewBag.ControllerName = controller;
            HttpContext.Session["ImageSubfolder"] = controller;
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
                var uploadDir = "~/Content/";
                if (HttpContext.Session["ImageSubfolder"] != null)
                {
                    switch ((string)HttpContext.Session["ImageSubfolder"])
                    {
                        case "Home":
                            uploadDir = "~/Content/Images/";
                            break;
                        case "MyHome":
                            uploadDir = "~/Content/HomeImages/";
                            break;
                        case "Portfolio":
                            uploadDir = "~/Content/MediaImages/";
                            break;
                        case "MediaDashboard":
                            uploadDir = "~/Content/PortfolioImages/";
                            break;
                    }
                }
                var imagePath = Path.Combine(Server.MapPath(uploadDir), qqfile.FileName);
                var imageUrl = Path.Combine(uploadDir, qqfile.FileName);
                qqfile.SaveAs(imagePath);
                var jsonResult = Json(new { success = true, message = "File successfully uploaded. Dont forget to save your changes in the settings menu!", file = imageUrl });
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
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
            var controller = ControllerContext.RouteData.Values["controller"].ToString();
            if (validatedUser.UserID == 0 || !validatedUser.UserIsAdmin)
            {
                return new HttpStatusCodeResult(500);
            }
            if (controller == "Home")
            {
                var user = new Security_User()
                {
                    Id = validatedUser.UserID,
                    EmailAddress = validatedUser.EmailAddress,
                    Guid = validatedUser.GUID,
                    IsActive = validatedUser.UserIsActive,
                    IsAdmin = validatedUser.UserIsAdmin,
                    UserName = validatedUser.UserName
                };
                var newViewModel = landingPageService.GetAppConfigData(user);
                return PartialView("_ApplicationManager", newViewModel);
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

        [HttpGet]
        public ActionResult Error()
        {
            ViewBag.Title = "3RR0R";
            ViewBag.ErrorStuff = new HandleErrorInfo(new Exception(), "MediaDashboard", "Index");
            return PartialView("Error");
        }

    }
}
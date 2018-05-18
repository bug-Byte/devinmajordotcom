using devinmajordotcom.Helpers;
using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using devinmajordotcom.Models;
using Newtonsoft.Json;

namespace devinmajordotcom.Controllers
{
    public class BaseController : Controller
    {

        protected log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Controller));
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

        [ValidateInput(false)]
        protected override void OnException(ExceptionContext filterContext)
        {
            var userId = HttpContext.Session["UserId"];
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogicalThreadContext.Properties["UserId"] = userId ?? "Anon Visitor";
            log4net.LogicalThreadContext.Properties["Action"] = filterContext.RouteData.Values["Action"];
            log4net.LogicalThreadContext.Properties["Controller"] = filterContext.RouteData.Values["Controller"];
            Log.Error("Exception Has Occurred", filterContext.Exception);
        }

        [ValidateInput(false)]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = HttpContext.Session["UserId"];
            base.OnActionExecuting(filterContext);
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogicalThreadContext.Properties["UserId"] = userId ?? "Anon Visitor";
            log4net.LogicalThreadContext.Properties["Action"] = filterContext.RouteData.Values["Action"];
            log4net.LogicalThreadContext.Properties["Controller"] = filterContext.RouteData.Values["Controller"];

            try
            {
                var allparams = filterContext.RequestContext.HttpContext.Request.Params;
                log4net.LogicalThreadContext.Properties["Params"] =
                    JsonConvert.SerializeObject(new { action = filterContext.ActionParameters, full = allparams });
                Log.Info("User Action");
            }
            catch (Exception e)
            {
                Log.Info("User Action");
            }
            

            

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
                var jsonResult = Json(new { success = true, message = "File successfully uploaded. Dont forget to save your changes in the settings menu!", file = imageUrl.Replace("~/","") });
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
                    IsEmailConfirmed = validatedUser.IsEmailConfirmed,
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

        

        [HttpPost]
        public JsonResult SendConfirmationEmail(UserViewModel viewModel)
        {
            var emailSuccessful = "";
            if (!ModelState.IsValid) return new JsonResult {Data = emailSuccessful};
            var message = new MailMessage();
            var body = PartialHelper.RenderViewToString(ControllerContext, "../Shared/ConfirmationEmail", viewModel);
            try
            {

                message.To.Add(new MailAddress(viewModel.EmailAddress));
                message.Subject = "Confirm your Email for devinmajor.com";
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                    new JsonResult { Data = "Success" };
                }
            }
            catch (Exception e)
            {
                message.Dispose();
            }
            return new JsonResult { Data = emailSuccessful };
        }

    }
}
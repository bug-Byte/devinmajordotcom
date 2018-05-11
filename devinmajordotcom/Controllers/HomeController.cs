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
       
        public void ManageLandingPage(MainLandingPageViewModel viewModel)
        {
            landingPageService.ManageLandingPage(viewModel);
        }

        public void UpdateCurrentUser(UserViewModel viewModel)
        {
            landingPageService.UpdateCurrentUser(viewModel);
            Login(viewModel, true);
        }

        public ActionResult _ApplicationManager()
        {
            ViewBag.Title = "D3V!N M@J0R";
            return PartialView();
        }

        [HttpGet]
        public void RemoveBannerLink(int ID)
        {
            landingPageService.RemoveBannerLinkById(ID);
        }

        [HttpGet]
        public void RemoveSiteLink(int ID)
        {
            landingPageService.RemoveSiteLinkById(ID);
        }

        [HttpPost]
        public void _HomeSettingsForm(MyHomeUserConfigViewModel viewModel)
        {
            myHomeService.SetUserConfigViewModel(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DropMeALine(ContactEmailViewModel viewModel)
        {
            var emailSuccessful = "";
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                var body = PartialHelper.RenderViewToString(ControllerContext, "../Shared/MainContactEmail", viewModel);
                try
                {
                    
                    message.To.Add(new MailAddress(viewModel.RecipientEmail));
                    message.Subject = "Attn Site Admin: " + viewModel.Subject;
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
            }
            return new JsonResult { Data = emailSuccessful };
        }

    }
}
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet]
        public ActionResult UploadTemplate()
        {
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
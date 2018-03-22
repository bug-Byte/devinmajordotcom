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

        [HttpGet]
        public ActionResult UploadTemplate()
        {
            return PartialView("_ImageUploader");
        }

        [HttpPost]
        public void UploadImage(HttpPostedFileWrapper qqfile)
        {
            var a = 0;
            //if (qqfile == null || qqfile.ContentLength <= 0)
            //{
            //    return Json(new { success = false, error = ScorecardResources.Error_File_Empty });
            //}

            //var responseData = new ResponseViewModel();

            //try
            //{
            //    responseData = _service.SalesDataExcelDocumentInsertion(new XLWorkbook(qqfile.InputStream));
            //}
            //catch (Exception ex)
            //{
            //    responseData.OperationStatus = false;
            //    responseData.Error = "Could not upload this Image";
            //}

            //return Json(new { success = responseData.OperationStatus, error = responseData.Error, successMessage = "Image Successfully Uploaded" });
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
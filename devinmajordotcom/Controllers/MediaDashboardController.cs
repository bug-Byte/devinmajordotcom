using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class MediaDashboardController : BaseController
    {

        public ActionResult Index()
        {
            
            ViewBag.ControllerName = "MediaDashboard";
            ViewBag.Layout = "../Shared/_MediaLayout.cshtml";
            var viewModel = new MediaDashboardViewModel();
            viewModel = mediaDashboardService.GetMediaDashboardViewModel();
            ViewBag.Title = viewModel.UserConfig.WebsiteTitle;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult _ManageMediaDashboard(MediaDashboardViewModel viewModel)
        {
            var data = mediaDashboardService.ManageMediaDashboard(viewModel);
            return new JsonResult { Data = data };
        }

        public void RemoveMediaDashboardLink(int ID)
        {
            mediaDashboardService.RemoveLink(ID);
        }

    }
}
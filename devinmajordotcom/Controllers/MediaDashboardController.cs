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
            ViewBag.Title = "D3V!N M@J0R";
            ViewBag.ControllerName = "MediaDashboard";
            ViewBag.Layout = "../Shared/_MediaLayout.cshtml";
            var viewModel = new MediaDashboardViewModel();
            viewModel = mediaDashboardService.GetMediaDashboardViewModel();
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
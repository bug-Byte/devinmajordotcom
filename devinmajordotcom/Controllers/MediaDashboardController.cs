using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class MediaDashboardController : Controller
    {

        public IMediaDashboardService service;

        public MediaDashboardController(IMediaDashboardService mediaDashboardService)
        {
            service = mediaDashboardService;
        }

        public ActionResult Index()
        {
            ViewBag.Title="D3V!N M@J0R";
            var viewModel = new MediaDashboardViewModel();
            viewModel = service.GetMediaDashboardViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult _ManageMediaDashboard(MediaDashboardViewModel viewModel)
        {
            var data = service.ManageMediaDashboard(viewModel);
            return new JsonResult { Data = data };
        }

        public void RemoveMediaDashboardLink(int ID)
        {
            service.RemoveLink(ID);
        }

    }
}
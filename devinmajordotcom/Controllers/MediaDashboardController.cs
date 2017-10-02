using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class MediaDashboardController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title="D3V!N M@J0R";
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class MyHomeController : Controller
    {
        // GET: MyHome
        public ActionResult Index()
        {
            return View();
        }
    }
}
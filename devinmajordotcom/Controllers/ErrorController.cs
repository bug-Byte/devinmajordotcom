using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class ErrorController : Controller
    {
        [HandleError]
        public ActionResult Error(HandleErrorInfo error)
        {
            return View(error);
        }

        [HandleError]
        public ActionResult Index()
        {
            return View();
        }

    }
}
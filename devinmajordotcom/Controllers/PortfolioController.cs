﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class PortfolioController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title="Media Dashboard";
            return View();
        }
    }
}
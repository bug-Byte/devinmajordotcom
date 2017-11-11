using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class PortfolioController : Controller
    {

        public IPortfolioService portfolioService;

        public PortfolioController(IPortfolioService PortfolioService)
        {
            portfolioService = PortfolioService;
        }

        public ActionResult Index()
        {
            ViewBag.Title="Professional Portfolio";

            var viewModel = new PortfolioViewModel();
            viewModel = portfolioService.GetPortfolioViewModel();

            return View(viewModel);
        }
    }
}
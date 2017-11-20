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
            ViewBag.Title = "D3V!N M@J0R";
            var viewModel = new PortfolioViewModel();
            viewModel = portfolioService.GetPortfolioViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult _ManagePortfolio(PortfolioViewModel viewModel)
        {
            var data = portfolioService.ManagePortfolio(viewModel);
            return new JsonResult { Data = data };
        }

    }
}
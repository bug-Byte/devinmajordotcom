using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class PortfolioController : BaseController
    {

        public ActionResult Index()
        {
            ViewBag.Title = "D3V!N M@J0R";
            var viewModel = new PortfolioViewModel();
            viewModel = portfolioService.GetPortfolioViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _ManagePortfolio(PortfolioViewModel viewModel)
        {
            var data = portfolioService.ManagePortfolio(viewModel);
            return new JsonResult { Data = data };
        }

        public void RemoveHighlightedSkill(int ID)
        {
            portfolioService.RemoveHighlightedSkill(ID);
        }

        public void RemoveTechSkill(int ID)
        {
            portfolioService.RemoveTechSkill(ID);
        }

        public void RemoveLanguageSkill(int ID)
        {
            portfolioService.RemoveLanguageSkill(ID);
        }

        public void RemoveContactLink(int ID)
        {
            portfolioService.RemoveContactLink(ID);
        }

    }
}
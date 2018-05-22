using devinmajordotcom.Services;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.Controllers
{
    public class PortfolioController : BaseController
    {

        public ActionResult Index()
        {
            var viewModel = new PortfolioViewModel();
            viewModel = portfolioService.GetPortfolioViewModel();
            ViewBag.Title = viewModel.PortfolioConfig.WebsiteTitle;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult _ManagePortfolio(PortfolioViewModel viewModel)
        {
            var data = portfolioService.ManagePortfolio(viewModel);
            return new JsonResult { Data = data };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ShootMeAnEmail(PortfolioViewModel viewModel)
        {
            return await DropMeALine(viewModel.ContactEmail);
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

        public void RemoveProject(int ID)
        {
            portfolioService.RemoveProject(ID);
        }

    }
}
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devinmajordotcom.Services
{
    public interface IPortfolioService
    {

        PortfolioViewModel GetPortfolioViewModel();

        string ManagePortfolio(PortfolioViewModel viewModel);

        void UpdateProfileAndPersonalDescription(PortfolioViewModel viewModel);

        void UpdateSkills(PortfolioViewModel viewModel);

        void UpdateProjectsAndFilters(PortfolioViewModel viewModel);

        void UpdateContactLinks(PortfolioViewModel viewModel);

        void RemoveContactLink(int IdToRemove);

        void RemoveHighlightedSkill(int IdToRemove);

        void RemoveTechSkill(int IdToRemove);

        void RemoveLanguageSkill(int IdToRemove);

    }
}

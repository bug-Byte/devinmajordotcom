using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Devinmajordotcom;
using System.Security.Principal;

namespace devinmajordotcom.Services
{
    public class PortfolioService : IPortfolioService
    {

        protected dbContext db;

        public PortfolioService()
        {
            this.db = new dbContext();
        }

        public PortfolioViewModel GetPortfolioViewModel()
        {

            return new PortfolioViewModel()
            {

                AvailableProjectFilters = db.Portfolio_ProjectTypes.Select(x => new DropDownViewModel() {
                    ID = x.Id,
                    Name = x.Type
                }).ToList(),

                SplashScreenDetails = db.Portfolio_Profiles.Select(x => new SplashScreenViewModel()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PersonalDescription = x.PersonalDescription,
                    PositionTitle = x.PositionTitle
                }).FirstOrDefault(),

                ProfileDetails = db.Portfolio_Profiles.Select(x => new ProfileViewModel()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth,
                    Address = x.Address,
                    Email = x.EmailAddress,
                    PhoneNumber = x.PhoneNumber,
                    ProfileID = x.Id,
                    WebsiteText = x.WebsiteText,
                    WebsiteURL = x.WebsiteUrl
                }).FirstOrDefault(),

                PersonalDescription = db.Portfolio_PersonalDescriptions.Select(x => new PersonalDescriptionViewModel()
                {
                    Adjective1 = x.Adjective1,
                    Adjective2 = x.Adjective2,
                    Adjective3 = x.Adjective3,
                    DescriptionID = x.Id,
                    Blurb = x.Blurb
                }).FirstOrDefault(),

                ContactSiteLinks = db.SiteLinks.Where(x => x.IsEnabled && x.ApplicationId == (int)Devinmajordotcom.ApplicationMaster.ApplicationMasters.ProfessionalPortfolio).Select(x => new SiteLinkViewModel()
                {
                    ID = x.Id,
                    Directive = x.Directive,
                    DisplayName = x.DisplayName,
                    DisplayIcon = x.DisplayIcon,
                    URL = x.Url
                }).ToList(),

                LanguageSkills = db.Portfolio_Skills.Where(x => x.SkillTypeId == (int)Devinmajordotcom.SkillTypeMaster.SkillTypeMasters.Language).Select(x => new LanguageSkillViewModel()
                {
                    LanguageSkillID = x.Id,
                    LanguageName = x.DisplayName,
                    LanguageSpecifics = x.Description,
                    LanguageCapabilityPercentage = x.ProficiencyPercentage.Value
                }).ToList(),

                TechSkills = db.Portfolio_Skills.Where(x => x.SkillTypeId == (int)Devinmajordotcom.SkillTypeMaster.SkillTypeMasters.Technical).Select(x => new TechSkillViewModel()
                {
                    TechSkillID = x.Id,
                    SkillDescription = x.Description
                }).ToList(),

                HighlightedWorkSkills = db.Portfolio_Skills.Where(x => x.SkillTypeId == (int)Devinmajordotcom.SkillTypeMaster.SkillTypeMasters.WorkHighlighted).Select(x => new WorkSkillViewModel()
                {
                    WorkSkillID = x.Id,
                    SkillIcon = x.DisplayIcon,
                    SkillTitle = x.DisplayName,
                    SkillDetails = x.Description
                }).ToList(),

                PortfolioProjects = db.Portfolio_Projects.Select(x => new ProjectViewModel() {
                    ProjectID = x.Id,
                    ProjectName = x.Name,
                    ProjectDescription = x.Description,
                    EncodedImage = x.Image,
                    ProjectFilters = x.Portfolio_ProjectTypes.Select(y => new DropDownViewModel() {
                        ID = y.Id,
                        Name = y.Type
                    }).ToList()
                }).ToList()

            };
        }

    }
}
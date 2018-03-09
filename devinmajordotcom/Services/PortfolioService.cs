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
    public class PortfolioService : BaseDataService, IPortfolioService
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

                ContactSiteLinks = db.Portfolio_ContactLinks.Where(x => x.IsEnabled).Select(x => new SiteLinkViewModel()
                {
                    DisplayName = x.DisplayName,
                    DisplayIcon = x.DisplayIcon,
                    Directive = x.Directive,
                    Action = x.Action,
                    Controller = x.Controller,
                    Description = x.Description,
                    IsDefault = x.IsDefault,
                    IsEnabled = x.IsEnabled,
                    ID = x.Id,
                    URL = x.Url,
                    Order = x.Order
                }).ToList(),

                LanguageSkills = db.Portfolio_LanguageSkills.Select(x => new LanguageSkillViewModel()
                {
                    LanguageSkillID = x.Id,
                    LanguageName = x.DisplayName,
                    LanguageSpecifics = x.Description,
                    LanguageCapabilityPercentage = x.ProficiencyPercentage.Value
                }).ToList(),

                TechSkills = db.Portfolio_TechSkills.Select(x => new TechSkillViewModel()
                {
                    TechSkillID = x.Id,
                    SkillDescription = x.Description
                }).ToList(),

                HighlightedWorkSkills = db.Portfolio_HighlightedSkills.Select(x => new WorkSkillViewModel()
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
                    ProjectFilters = x.Portfolio_ProjectTypeMappings.Select(y => new DropDownViewModel() {
                        ID = y.Id,
                        Name = y.Portfolio_ProjectType.Type
                    }).ToList()
                }).ToList()

            };
        }

        public string ManagePortfolio(PortfolioViewModel viewModel)
        {
            try
            {
                UpdateProfileAndPersonalDescription(viewModel);
                UpdateSkills(viewModel);
                UpdateProjectsAndFilters(viewModel);
                UpdateContactLinks(viewModel);
                db.SaveChanges();
                return "success";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public void UpdateProfileAndPersonalDescription(PortfolioViewModel viewModel)
        {
            Portfolio_PersonalDescription currentPortfolioPersonalDescription = db.Portfolio_PersonalDescriptions.FirstOrDefault(x => x.Id == viewModel.PersonalDescription.DescriptionID);
            Portfolio_Profile currentPortfolioProfile = db.Portfolio_Profiles.FirstOrDefault(x => x.Id == viewModel.ProfileDetails.ProfileID);

            if (currentPortfolioPersonalDescription != null)
            {
                currentPortfolioPersonalDescription.Blurb = viewModel.PersonalDescription.Blurb;
                currentPortfolioPersonalDescription.Adjective1 = viewModel.PersonalDescription.Adjective1;
                currentPortfolioPersonalDescription.Adjective2 = viewModel.PersonalDescription.Adjective2;
                currentPortfolioPersonalDescription.Adjective3 = viewModel.PersonalDescription.Adjective3;
            }
            else
            {
                var newPortfolioPersonalDescription = new Portfolio_PersonalDescription()
                {
                    Blurb = viewModel.PersonalDescription.Blurb,
                    Adjective1 = viewModel.PersonalDescription.Adjective1,
                    Adjective2 = viewModel.PersonalDescription.Adjective2,
                    Adjective3 = viewModel.PersonalDescription.Adjective3
                };
                db.Portfolio_PersonalDescriptions.Add(newPortfolioPersonalDescription);
            }

            if (currentPortfolioProfile != null)
            {
                currentPortfolioProfile.FirstName = viewModel.SplashScreenDetails.FirstName;
                currentPortfolioProfile.LastName = viewModel.SplashScreenDetails.LastName;
                currentPortfolioProfile.DateOfBirth = viewModel.ProfileDetails.DateOfBirth;
                currentPortfolioProfile.Address = viewModel.ProfileDetails.Address;
                currentPortfolioProfile.PhoneNumber = viewModel.ProfileDetails.PhoneNumber;
                currentPortfolioProfile.EmailAddress = viewModel.ProfileDetails.Email;
                currentPortfolioProfile.PositionTitle = viewModel.SplashScreenDetails.PositionTitle;
                currentPortfolioProfile.PersonalDescription = viewModel.SplashScreenDetails.PersonalDescription;
                currentPortfolioProfile.WebsiteText = viewModel.ProfileDetails.WebsiteText;
                currentPortfolioProfile.WebsiteUrl = viewModel.ProfileDetails.WebsiteURL;
            }
            else
            {
                var newPortfolioProfile = new Portfolio_Profile()
                {
                    FirstName = viewModel.SplashScreenDetails.FirstName,
                    LastName = viewModel.SplashScreenDetails.LastName,
                    DateOfBirth = viewModel.ProfileDetails.DateOfBirth,
                    Address = viewModel.ProfileDetails.Address,
                    PhoneNumber = viewModel.ProfileDetails.PhoneNumber,
                    EmailAddress = viewModel.ProfileDetails.Email,
                    PositionTitle = viewModel.SplashScreenDetails.PositionTitle,
                    PersonalDescription = viewModel.SplashScreenDetails.PersonalDescription,
                    WebsiteText = viewModel.ProfileDetails.WebsiteText,
                    WebsiteUrl = viewModel.ProfileDetails.WebsiteURL,
                };
                db.Portfolio_Profiles.Add(newPortfolioProfile);
            }
        }

        public void UpdateSkills(PortfolioViewModel viewModel)
        {
            foreach (var techSkill in viewModel.TechSkills.Where(x => x.SkillDescription != null))
            {
                var skillRecord = db.Portfolio_TechSkills.FirstOrDefault(x => x.Id == techSkill.TechSkillID);
                if (skillRecord != null)
                {
                    skillRecord.Description = techSkill.SkillDescription;
                }
                else
                {
                    var newTechSkill = new Portfolio_TechSkill()
                    {
                        Description = techSkill.SkillDescription,
                    };
                    db.Portfolio_TechSkills.Add(newTechSkill);
                    db.SaveChanges();
                }
            }
            foreach (var workSkill in viewModel.HighlightedWorkSkills.Where(x => x.SkillTitle != null))
            {
                var skillRecord = db.Portfolio_HighlightedSkills.FirstOrDefault(x => x.Id == workSkill.WorkSkillID);
                if (skillRecord != null)
                {
                    skillRecord.DisplayIcon = workSkill.SkillIcon;
                    skillRecord.DisplayName = workSkill.SkillTitle;
                    skillRecord.Description = workSkill.SkillDetails;
                }
                else
                {
                    var newWorkSkill = new Portfolio_HighlightedSkill()
                    {
                        DisplayName = workSkill.SkillTitle,
                        DisplayIcon = workSkill.SkillIcon,
                        Description = workSkill.SkillDetails
                    };
                    db.Portfolio_HighlightedSkills.Add(newWorkSkill);
                    db.SaveChanges();
                }
            }
            foreach (var languageSkill in viewModel.LanguageSkills.Where(x => x.LanguageName != null))
            {
                var skillRecord = db.Portfolio_LanguageSkills.FirstOrDefault(x => x.Id == languageSkill.LanguageSkillID);
                if (skillRecord != null)
                {
                    skillRecord.Description = languageSkill.LanguageSpecifics;
                    skillRecord.DisplayName = languageSkill.LanguageName;
                    skillRecord.ProficiencyPercentage = languageSkill.LanguageCapabilityPercentage;
                }
                else
                {
                    var newLanguageSkill = new Portfolio_LanguageSkill()
                    {
                        DisplayName = languageSkill.LanguageName,
                        Description = languageSkill.LanguageSpecifics,
                        ProficiencyPercentage = languageSkill.LanguageCapabilityPercentage
                    };
                    db.Portfolio_LanguageSkills.Add(newLanguageSkill);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
        }

        public void UpdateProjectsAndFilters(PortfolioViewModel viewModel)
        {
            foreach(var project in viewModel.PortfolioProjects)
            {
                var projectRecord = db.Portfolio_Projects.FirstOrDefault(x => x.Id == project.ProjectID);
                int projectID = 0;
                if(projectRecord != null)
                {
                    projectID = projectRecord.Id;
                    projectRecord.Name = project.ProjectName;
                    projectRecord.Description = project.ProjectDescription;
                    projectRecord.Image = project.EncodedImage;
                }
                else
                {
                    var newProjectRecord = new Portfolio_Project()
                    {
                        Name = project.ProjectName,
                        Description = project.ProjectDescription,
                        Image = project.EncodedImage,
                    };
                    db.Portfolio_Projects.Add(newProjectRecord);
                    projectID = newProjectRecord.Id;
                }
                foreach(var filter in project.ProjectFilters)
                {
                    var filterRecord = db.Portfolio_ProjectTypes.FirstOrDefault(x => x.Id == filter.ID);
                    int filterID = 0;
                    if(filterRecord != null)
                    {
                        filterID = filterRecord.Id;
                    }
                    else
                    {
                        var newFilter = new Portfolio_ProjectType()
                        {
                            Type = filter.Name
                        };
                        db.Portfolio_ProjectTypes.Add(newFilter);
                        filterID = newFilter.Id;
                    }
                    var newProjectTypeMapping = new Portfolio_ProjectTypeMapping()
                    {
                        ProjectId = projectID,
                        ProjectTypeId = filterID
                    };
                    db.Portfolio_ProjectTypeMappings.Add(newProjectTypeMapping);
                }
            }
        }

        public void UpdateContactLinks(PortfolioViewModel viewModel)
        {
            foreach(var contactLink in viewModel.ContactSiteLinks)
            {
                var linkRecord = db.Portfolio_ContactLinks.FirstOrDefault(x => x.Id == contactLink.ID);
                if(linkRecord != null)
                {
                    linkRecord.DisplayName = contactLink.DisplayName;
                    linkRecord.DisplayIcon = contactLink.DisplayIcon;
                    linkRecord.Action = contactLink.Action;
                    linkRecord.Controller = contactLink.Controller;
                    linkRecord.Description = contactLink.Description;
                    linkRecord.IsDefault = contactLink.IsDefault;
                    linkRecord.IsEnabled = contactLink.IsEnabled;
                    linkRecord.Url = contactLink.URL;
                    linkRecord.Order = contactLink.Order;
                    linkRecord.Directive = contactLink.Directive;
                }
                else
                {
                    var newLinkRecord = new Portfolio_ContactLink()
                    {
                        DisplayName = contactLink.DisplayName,
                        DisplayIcon = contactLink.DisplayIcon,
                        Action = contactLink.Action,
                        Controller = contactLink.Controller,
                        Description = contactLink.Description,
                        IsDefault = contactLink.IsDefault,
                        IsEnabled = contactLink.IsEnabled,
                        Url = contactLink.URL,
                        Order = contactLink.Order,
                        Directive = contactLink.Directive
                    };
                    db.Portfolio_ContactLinks.Add(newLinkRecord);
                }
            }
        }

        public void RemoveHighlightedSkill(int IdToRemove)
        {
            var recordToRemove = db.Portfolio_HighlightedSkills.FirstOrDefault(x => x.Id == IdToRemove);
            if(recordToRemove != null)
            {
                db.Portfolio_HighlightedSkills.Remove(recordToRemove);
                db.SaveChanges();
            }
        }

        public void RemoveTechSkill(int IdToRemove)
        {
            var recordToRemove = db.Portfolio_TechSkills.FirstOrDefault(x => x.Id == IdToRemove);
            if (recordToRemove != null)
            {
                db.Portfolio_TechSkills.Remove(recordToRemove);
                db.SaveChanges();
            }
        }

        public void RemoveLanguageSkill(int IdToRemove)
        {
            var recordToRemove = db.Portfolio_LanguageSkills.FirstOrDefault(x => x.Id == IdToRemove);
            if (recordToRemove != null)
            {
                db.Portfolio_LanguageSkills.Remove(recordToRemove);
                db.SaveChanges();
            }
        }

    }
}
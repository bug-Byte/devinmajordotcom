using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace devinmajordotcom.ViewModels
{

    public class PortfolioViewModel
    {

        public SplashScreenViewModel SplashScreenDetails { get; set; }

        public List<WorkSkillViewModel> HighlightedWorkSkills { get; set; }

        public ProfileViewModel ProfileDetails { get; set; }

        public List<LanguageSkillViewModel> LanguageSkills { get; set; }

        public List<TechSkillViewModel> TechSkills { get; set; }

        public PersonalDescriptionViewModel PersonalDescription { get; set; }

        public List<SiteLinkViewModel> ContactSiteLinks { get; set; }

        public List<ProjectViewModel> PortfolioProjects { get; set; }

        public List<DropDownViewModel> AvailableProjectFilters { get; set; }

    }

    public class ProfileViewModel
    {

        public int ProfileID { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string WebsiteText { get; set; }

        public string WebsiteURL { get; set; }

    }

    public class SplashScreenViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PositionTitle { get; set; }

        public string PersonalDescription { get; set; }

    }

    public class LanguageSkillViewModel
    {

        public int LanguageSkillID { get; set; }

        public string LanguageName { get; set; }

        public string LanguageSpecifics { get; set; }

        public int LanguageCapabilityPercentage { get; set; }

    }

    public class TechSkillViewModel
    {

        public int TechSkillID { get; set; }
        
        public string SkillDescription { get; set; } 

    }

    public class WorkSkillViewModel
    {

        public int WorkSkillID { get; set; }

        public string SkillTitle { get; set; }

        public string SkillIcon { get; set; }

        public string SkillDetails { get; set; }

    }

    public class PersonalDescriptionViewModel
    {

        public int DescriptionID { get; set; }

        public string Adjective1 { get; set; }

        public string Adjective2 { get; set; }

        public string Adjective3 { get; set; }

        public string Blurb { get; set; }

    }

    public class ProjectViewModel
    {

        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public byte[] EncodedImage { get; set; }

        public string ProjectDescription { get; set; }

        public List<DropDownViewModel> ProjectFilters { get; set; }

    }

}
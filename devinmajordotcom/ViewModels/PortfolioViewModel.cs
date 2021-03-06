﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace devinmajordotcom.ViewModels
{

    public class PortfolioViewModel
    {

        public UserViewModel CurrentUserViewModel { get; set; }

        public PortfolioConfigViewModel PortfolioConfig { get; set; }

        public SplashScreenViewModel SplashScreenDetails { get; set; }

        public List<WorkSkillViewModel> HighlightedWorkSkills { get; set; }

        public ProfileViewModel ProfileDetails { get; set; }

        public ContactEmailViewModel ContactEmail { get; set; }

        public List<LanguageSkillViewModel> LanguageSkills { get; set; }

        public List<TechSkillViewModel> TechSkills { get; set; }

        public List<AcademicViewModel> Academics { get; set; }

        public List<JobViewModel> Jobs { get; set; }

        public PersonalDescriptionViewModel PersonalDescription { get; set; }

        public List<SiteLinkViewModel> ContactSiteLinks { get; set; }

        public List<ProjectViewModel> PortfolioProjects { get; set; }

        public List<DropDownViewModel> AvailableProjectFilters { get; set; }

    }

    public class JobViewModel
    {
        
        public int ID { get; set; }

        [Required]
        [DisplayName("Job/Position Title * : ")]
        public string JobTitle { get; set; }

        [DisplayName("Company Name : ")]
        public string CompanyName { get; set; }

        [DisplayName("Job Description : ")]
        public string Description { get; set; }

        [DisplayName("Start Date : ")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date : ")]
        public DateTime? EndDate { get; set; }

    }

    public class AcademicViewModel
    {

        public int ID { get; set; }

        [DisplayName("Certificate Name * : ")]
        public string CertificateName { get; set; }

        [DisplayName("School/Institution Name : ")]
        public string SchoolName { get; set; }

        [DisplayName("Program Name : ")]
        public string ProgramName { get; set; }

        [DisplayName("Start Date : ")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date : ")]
        public DateTime? EndDate { get; set; }

    }

    public class PortfolioConfigViewModel
    {
        
        public int ID { get; set; }

        [DisplayName("Website Title: ")]
        public string WebsiteTitle { get; set; }

        [DisplayName("Background Image: ")]
        public string BackgroundImage { get; set; }

    }


    public class ProfileViewModel
    {

        public int ProfileID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Birth Date * : ")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
        [DisplayName("Address * : ")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Phone # * : ")]
        [StringLength(25, ErrorMessage = "Phone # cannot be longer than 25 characters.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Email Address cannot be longer than 100 characters.")]
        [DisplayName("Email Address * : ")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Website Name cannot be longer than 100 characters.")]
        [DisplayName("Website Name * : ")]
        public string WebsiteText { get; set; }

        [StringLength(200, ErrorMessage = "Website URL cannot be longer than 200 characters.")]
        [DisplayName("Website URL * : ")]
        public string WebsiteURL { get; set; }

    }

    public class SplashScreenViewModel
    {

        [DisplayName("First Name * : ")]
        [Required]
        [StringLength(100, ErrorMessage = "First Name cannot be longer than 100 characters.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name * : ")]
        [Required]
        [StringLength(100, ErrorMessage = "Last Name cannot be longer than 100 characters.")]
        public string LastName { get; set; }

        [DisplayName("Job / Position Title: ")]
        [StringLength(100, ErrorMessage = "First Name cannot be longer than 100 characters.")]
        public string PositionTitle { get; set; }

        [DisplayName("Personal Description: ")]
        [StringLength(500, ErrorMessage = "Personal Description cannot be longer than 500 characters.")]
        [AllowHtml]
        public string PersonalDescription { get; set; }

    }

    public class LanguageSkillViewModel
    {

        public int LanguageSkillID { get; set; }

        [StringLength(100, ErrorMessage = "Language Name cannot be longer than 100 characters.")]
        [DisplayName("Language Name * : ")]
        [Required]
        public string LanguageName { get; set; }

        [StringLength(500, ErrorMessage = "Language Specifics cannot be longer than 500 characters.")]
        [DisplayName("Language Specifics * : ")]
        public string LanguageSpecifics { get; set; }

        public int? LanguageCapabilityPercentage { get; set; }

    }

    public class TechSkillViewModel
    {

        public int TechSkillID { get; set; }

        [StringLength(500, ErrorMessage = "Skill Description cannot be longer than 500 characters.")]
        [DisplayName("Skill Description * : ")]
        [Required]
        public string SkillDescription { get; set; } 

    }

    public class WorkSkillViewModel
    {

        public int WorkSkillID { get; set; }

        [StringLength(100, ErrorMessage = "Skill Name cannot be longer than 100 characters.")]
        [DisplayName("Skill Name * : ")]
        [Required]
        public string SkillTitle { get; set; }

        [StringLength(100, ErrorMessage = "Skill Icon is invalid, cannot be longer than 100 characters.")]
        [DisplayName("Skill Icon: ")]
        public string SkillIcon { get; set; }

        [StringLength(500, ErrorMessage = "Skill Description cannot be longer than 500 characters.")]
        [DisplayName("Skill Description * : ")]
        [AllowHtml]
        public string SkillDetails { get; set; }

    }

    public class PersonalDescriptionViewModel
    {

        public int DescriptionID { get; set; }

        [DisplayName("Personality Traits :")]
        public string Adjective1 { get; set; }

        public string Adjective2 { get; set; }

        public string Adjective3 { get; set; }

        [DisplayName("Portfolio Description :")]
        public string Blurb { get; set; }

    }

    public class ProjectViewModel
    {

        public int ProjectID { get; set; }

        [StringLength(100, ErrorMessage = "Project Name cannot be longer than 100 characters.")]
        [DisplayName("Project Name * : ")]
        [Required]
        public string ProjectName { get; set; }

        [DisplayName("Project Image : ")]
        public string EncodedImage { get; set; }

        [DisplayName("Project Description : ")]
        public string ProjectDescription { get; set; }

        public List<DropDownViewModel> ProjectFilters { get; set; }

        public string _CommaDelimitedProjectFilters;

        [DisplayName("Project Tags : ")]
        public string CommaDelimitedProjectFilters
        {
            get
            {
                var filters = ProjectFilters?.Select(x => x.Name + ",").ToList();
                return filters?.Count > 0 ? filters.Aggregate("", (current, filter) => current + (filter + ",")) : "";
            }
            set
            {
                _CommaDelimitedProjectFilters = value;
            }
        }

    }

}
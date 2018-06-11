using devinmajordotcom.Models;
using devinmajordotcom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using devinmajordotcom.Helpers;

namespace devinmajordotcom.Services
{
    public class LandingPageService : BaseDataService, ILandingPageService
    {
        
        protected IPortfolioService portfolioService;
        protected IMediaDashboardService mediaDashboardService;
        protected IMyHomeService myHomeService;
        protected IHardwareMonitorService hardwareService;

        public LandingPageService(IPortfolioService PortfolioService, IMediaDashboardService MediaDashboardService, IMyHomeService MyHomeService, IHardwareMonitorService HardwareService)
        {
            portfolioService = PortfolioService;
            mediaDashboardService = MediaDashboardService;
            myHomeService = MyHomeService;
            hardwareService = HardwareService;
        }

        public MainLandingPageViewModel GetLandingPageViewModel()
        {
            var siteAdminUser = db.Security_Users.FirstOrDefault(x => x.IsActive && x.IsAdmin);
            var guid = HttpContext.Current.Session["MainPageUserAuthID"];
            if(siteAdminUser == null)
            {
                guid = AddNewUser(true).Guid;
            }
            if(guid == null)
            {
                guid = AddNewUser().Guid;
            }
            var user = GetCurrentUser((Guid)guid);
            var dateRanges = GetAvailableDateRanges();
            var hardwareTypes = GetAvailableHardwareTypes();
            return new MainLandingPageViewModel()
            {
                Config = GetLandingPageConfig(),
                CurrentUserViewModel = user,
                AvailableHardwareTypes = hardwareTypes,
                SelectedHardwareTypeID = hardwareTypes.Select(x => x.ID).FirstOrDefault(),
                AvailableDateRanges = dateRanges,
                SelectedDateRangeID = dateRanges.Select(x => x.ID).FirstOrDefault(),
                LandingPageApplicationLinks = GetMainSiteLinks(),
                LandingPageBannerLinks = GetMainBannerLinks(),
                CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel(),
                CurrentApplicationConfig = GetAppConfigData(siteAdminUser),
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = siteAdminUser == null ? "" : siteAdminUser.EmailAddress,
                    UserGUID = user.GUID
                }
            };
        }

        public List<DropDownViewModel> GetAvailableHardwareTypes()
        {
            var results = new List<DropDownViewModel>();
            var cpus = db.Security_HardwarePerformances.Where(x => x.HardwareNumber != null).Select(x => x.HardwareNumber).Distinct().ToList();
            var currentCpuTypes = db.Security_HardwareTypes.Where(x => x.Name.Contains("CPU")).ToList();
            if (cpus.Count == 0 || currentCpuTypes.Count != (cpus.Count * 2))
            {
                for (var i = 0; i < cpus.Count; i++)
                {
                    var iPlusOne = (i + 1);
                    var metrics = db.Security_HardwareTypes.Where(x => x.Name.Contains("CPU " + iPlusOne.ToString())).ToList();
                    if (metrics.Count == 0)
                    {
                        var items = new List<Security_HardwareType>()
                        {
                            new Security_HardwareType()
                            {
                                Name = "CPU " + iPlusOne + " Temp"
                            },
                            new Security_HardwareType()
                            {
                                Name = "CPU " + iPlusOne + " Usage"
                            },
                        };
                        db.Security_HardwareTypes.AddRange(items);
                        db.SaveChanges();
                    }
                }
            }
            var cpuTypes = db.Security_HardwareTypes.Where(x => x.Name.Contains("CPU")).Select(y => new DropDownViewModel()
            {
                ID = y.Id,
                Name = y.Name
            }).ToList();
            results.AddRange(cpuTypes);
            var ramType = db.Security_HardwareTypes.Where(x => x.Name.Contains("RAM")).Select(x => new DropDownViewModel() { ID = x.Id, Name = x.Name }).FirstOrDefault();
            if (ramType != null)
            {
                results.Add(ramType);
            }
            return results;
        }

        public List<DropDownViewModel> GetAvailableDateRanges()
        {
            return db.Security_DateRanges.Select(x => new DropDownViewModel() {
                ID = x.Id,
                Name = x.Name
            }).ToList();
        }

        public ApplicationConfigViewModel GetAppConfigData(Security_User admin = null)
        {
            var results = new ApplicationConfigViewModel()
            {
                Config = GetLandingPageConfig(),
                LandingPageApplicationLinks = GetMainSiteLinks(true),
                LandingPageBannerLinks = GetMainBannerLinks(true),
                CurrentPortfolioData = portfolioService.GetPortfolioViewModel(),
                CurrentMediaDashboardData = mediaDashboardService.GetMediaDashboardViewModel(),
                CurrentMyHomeData = myHomeService.GetMasterSettingsViewModel(),
                ContactEmailData = new ContactEmailViewModel()
                {
                    RecipientEmail = admin == null ? "" : admin.EmailAddress
                }
            };
            results.CurrentMediaDashboardData.SidebarLinks = GetMediaSiteLinks(true);
            return results;
        }

        public void ManageLandingPage(MainLandingPageViewModel viewModel)
        {
            var configRecord = db.LandingPage_Configs.FirstOrDefault();
            if(configRecord != null)
            {
                configRecord.BackgroundImage = viewModel.Config.BackgroundImage;
                configRecord.AppsTitle = viewModel.Config.AppsTitle;
                configRecord.IsParticleCanvasOn = viewModel.Config.IsParticleCanvasOn;
                configRecord.WebsiteName = viewModel.Config.WebsiteName;
            }
            else
            {
                var newRecord = new LandingPage_Config()
                {
                    AppsTitle = viewModel.Config.AppsTitle,
                    IsParticleCanvasOn = viewModel.Config.IsParticleCanvasOn,
                    WebsiteName = viewModel.Config.WebsiteName,
                    BackgroundImage = viewModel.Config.BackgroundImage
                };
                db.LandingPage_Configs.Add(newRecord);
            }
            db.SaveChanges();
            foreach (var link in viewModel.LandingPageBannerLinks)
            {
                if(link.DisplayName != null)
                {
                    var linkRecord = db.LandingPage_BannerLinks.FirstOrDefault(x => x.Id == link.ID);
                    if (linkRecord != null)
                    {
                        linkRecord.IsDefault = link.IsDefault;
                        linkRecord.IsEnabled = link.IsEnabled;
                        linkRecord.IsPublic = link.IsPublic;
                        linkRecord.Action = link.Action;
                        linkRecord.Controller = link.Controller;
                        linkRecord.Url = link.URL;
                        linkRecord.Description = link.Description;
                        linkRecord.Directive = link.Directive;
                        linkRecord.DisplayIcon = link.DisplayIcon;
                        linkRecord.Order = link.Order;
                        linkRecord.DisplayName = link.DisplayName;
                    }
                    else
                    {
                        var newRecord = new LandingPage_BannerLink()
                        {
                            IsDefault = link.IsDefault,
                            IsEnabled = link.IsEnabled,
                            Action = link.Action,
                            Controller = link.Controller,
                            Url = link.URL,
                            Description = link.Description,
                            Directive = link.Directive,
                            DisplayIcon = link.DisplayIcon,
                            Order = link.Order,
                            DisplayName = link.DisplayName,
                            IsPublic = link.IsPublic
                        };
                        db.LandingPage_BannerLinks.Add(newRecord);
                    }
                }
                db.SaveChanges();
            }
            foreach (var link in viewModel.LandingPageApplicationLinks)
            {
                if (link.DisplayName != null)
                {
                    var linkRecord = db.LandingPage_SiteLinks.FirstOrDefault(x => x.Id == link.ID);
                    if (linkRecord != null)
                    {
                        linkRecord.IsDefault = link.IsDefault;
                        linkRecord.IsEnabled = link.IsEnabled;
                        linkRecord.IsPublic = link.IsPublic;
                        linkRecord.Action = link.Action;
                        linkRecord.Controller = link.Controller;
                        linkRecord.Url = link.URL;
                        linkRecord.Description = link.Description;
                        linkRecord.Directive = link.Directive;
                        linkRecord.DisplayIcon = link.DisplayIcon;
                        linkRecord.Order = link.Order;
                        linkRecord.DisplayName = link.DisplayName;
                    }
                    else
                    {
                   
                        var newRecord = new LandingPage_SiteLink()
                        {
                            IsDefault = link.IsDefault,
                            IsEnabled = link.IsEnabled,
                            Action = link.Action,
                            Controller = link.Controller,
                            Url = link.URL,
                            Description = link.Description,
                            Directive = link.Directive,
                            DisplayIcon = link.DisplayIcon,
                            Order = link.Order,
                            DisplayName = link.DisplayName,
                            IsPublic = link.IsPublic
                        };
                        db.LandingPage_SiteLinks.Add(newRecord);                    
                    }
                }
                db.SaveChanges();
                
            }
        }

        public LandingPageConfigViewModel GetLandingPageConfig()
        {
            return db.LandingPage_Configs.Select(x => new LandingPageConfigViewModel() {
                WebsiteName = x.WebsiteName,
                AppsTitle = x.AppsTitle,
                BackgroundImage = x.BackgroundImage,
                IsParticleCanvasOn = x.IsParticleCanvasOn,
                ID = x.Id
            }).FirstOrDefault();
        }

        public List<SiteLinkViewModel> GetMainSiteLinks(bool isRetrievingSettings = false)
        {
            return db.LandingPage_SiteLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                IsPublic = x.IsPublic,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        public List<SiteLinkViewModel> GetMainBannerLinks(bool isRetrievingSettings = false)
        {
            return db.LandingPage_BannerLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Directive = x.Directive,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                IsPublic = x.IsPublic,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        public List<SiteLinkViewModel> GetMediaSiteLinks(bool isRetrievingSettings = false)
        {
            return db.MediaDashboard_SiteLinks.Where(y => isRetrievingSettings == true ? (y.IsEnabled == false || y.IsEnabled == true) : y.IsEnabled).Select(x => new SiteLinkViewModel()
            {
                ID = x.Id,
                DisplayName = x.DisplayName,
                Description = x.Description,
                DisplayIcon = x.DisplayIcon,
                URL = x.Url,
                Action = x.Action,
                Controller = x.Controller,
                IsDefault = x.IsDefault,
                IsEnabled = x.IsEnabled,
                IsPublic = x.IsPublic,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        public void RemoveBannerLinkById(int ID)
        {
            var link = db.LandingPage_BannerLinks.FirstOrDefault(x => x.Id == ID);
            if (link != null)
            {
                db.LandingPage_BannerLinks.Remove(link);
                db.SaveChanges();
            }
        }

        public void RemoveSiteLinkById(int ID)
        {
            var link = db.LandingPage_SiteLinks.FirstOrDefault(x => x.Id == ID);
            if (link != null)
            {
                db.LandingPage_SiteLinks.Remove(link);
                db.SaveChanges();
            }
        }

        //public string SendContactEmailToSiteAdmin(ContactEmailViewModel viewModel)
        //{
        //    var message = new MailMessage();
        //    try
        //    {
        //        var body = "<p>From: </p><p>{0}</p><p>({1})</p><p>Message:</p><p>{2}</p>";
        //        message.To.Add(new MailAddress(viewModel.RecipientEmail));
        //        message.Subject = "Attn Site Admin: " + viewModel.Subject;
        //        message.Body = string.Format(body, viewModel.SenderName, viewModel.SenderEmailAddress, viewModel.Content);
        //        message.IsBodyHtml = true;
        //        using (var smtp = new SmtpClient())
        //        {
        //            smtp.Send(message);
        //            return "Success";
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        message.Dispose();
        //        return e.Message;
        //    }
        //}
    }
}
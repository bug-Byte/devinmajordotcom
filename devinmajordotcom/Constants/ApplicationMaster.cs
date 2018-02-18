﻿
/*
	This File is generated by the CategoryMaster.tt file written by mdoherty
	It is used to keep our Categories synchronized with the database
	If you add another Category save this template so we have up to date values
	
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace devinmajordotcom
{
    /// <summary>
    /// We need the ability to check for new Applications
    /// </summary>
	public static class ApplicationMasterEnum
	{

		public enum ApplicationMasters
		{
			[Display(Name="Main Landing Page")]
			MainLandingPage = 1,
			[Display(Name="My Custom Homepage")]
			MyCustomHomepage = 2,
			[Display(Name="Plex Media Dashboard")]
			PlexMediaDashboard = 3,
			[Display(Name="Professional Portfolio")]
			ProfessionalPortfolio = 4,
		}
	}
}
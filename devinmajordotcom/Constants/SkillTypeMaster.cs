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

namespace Devinmajordotcom
{
    /// <summary>
    /// We need the ability to check for new SkillTypes
    /// </summary>
	public static class SkillTypeMaster
	{

		public enum SkillTypeMasters
		{
			[Display(Name="Work/Highlighted")]
			WorkHighlighted = 1,
			[Display(Name="Technical")]
			Technical = 2,
			[Display(Name="Language")]
			Language = 3,
		}
	}
}
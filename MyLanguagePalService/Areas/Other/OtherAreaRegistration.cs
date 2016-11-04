﻿using System.Web.Mvc;

namespace MyLanguagePalService.Areas.Other
{
    public class OtherAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Other";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Other_default",
                "Other/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
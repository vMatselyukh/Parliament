﻿using System.Web.Mvc;

namespace WebApi.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "home", action = "index", id = UrlParameter.Optional },
                new string[] { "WebApi.Areas.Admin.Controllers" }
            );
        }
    }
}
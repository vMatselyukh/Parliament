using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { area="admin", controller="parliament", action = "politicians", id = UrlParameter.Optional },
                null,
                new[] { "WebApi.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "admin");

            routes.MapRoute(
                "Track",
                "{area}/parliament/politician/{politicianId}/track/{trackId}",
                new { area = "admin", controller = "parliament", action = "track", trackId = UrlParameter.Optional },
                null,
                new[] { "WebApi.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "admin");

            routes.MapRoute(
                "Error",
                "Error",
                new { area = "admin", controller = "Error", action = "Index" },
                null,
                new[] { "WebApi.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "admin");
        }
    }
}


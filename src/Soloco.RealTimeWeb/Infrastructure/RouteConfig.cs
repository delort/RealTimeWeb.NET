﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;

namespace Soloco.RealTimeWeb
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(this IRouteBuilder routeBuilder)
        {
            //routeBuilder.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routeBuilder.MapRoute(name: "Default", template: "", defaults: new { controller = "Home", action = "Index" });
            routeBuilder. MapRoute(name: "Account", template: "Account/{action}", defaults: new { controller = "Account" });
        }
    }
}

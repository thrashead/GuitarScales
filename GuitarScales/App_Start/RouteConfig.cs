using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GuitarScales
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Giris",
                url: "giris",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "GuitarScales.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "GuitarScales.Controllers" }
            );

            routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new[] { "GuitarScales.Controllers" }
			);
		}
	}
}

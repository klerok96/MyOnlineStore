using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "Goods/{action}/{genre}/Page{page}",
                new { controller = "Goods", action = "Index" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Goods/{action}/Page{page}",
                new { controller = "Goods", action = "Index", genre = (string)null },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "Goods/{action}/{genre}",
                new { controller = "Goods", action = "Index", page = 1 }
            );

            routes.MapRoute(
                null,
                "Goods/{action}/",
                new { controller = "Goods", action = "Index" }
            );


            routes.MapRoute(
                null,
                "GoodsAdmin/{action}/{genre}/Page{page}",
                new { controller = "GoodsAdmin", action = "Index" },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "GoodsAdmin/{action}/Page{page}",
                new { controller = "GoodsAdmin", action = "Index", genre = (string)null },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                null,
                "GoodsAdmin/{action}/{genre}",
                new { controller = "GoodsAdmin", action = "Index", page = 1 }
            );

            routes.MapRoute(
                null,
                "GoodsAdmin/{action}/",
                new { controller = "GoodsAdmin", action = "Index" }
            );


            routes.MapRoute(
                null,
                "{controller}/{action}/{id}",
                new { controller = "categories", action = "index", id = UrlParameter.Optional }
            );

        }
    }
}

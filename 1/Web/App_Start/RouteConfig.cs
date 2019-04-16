using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class GetSEOFriendlyRoute : Route
    {
        public GetSEOFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("id"))
                    routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
            }

            return routeData;
        }

        private object GetIdValue(object id)
        {
            if (id != null)
            {
                string idValue = id.ToString();

                var regex = new Regex(@"^(?<id>\d+).*$");
                var match = regex.Match(idValue);

                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }

            return id;
        }
    }  
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //  route mở rộng
            routes.MapMvcAttributeRoutes();  
            // custome
            routes.MapRoute(
                name:"About",
                url:"Gioi-Thieu-Inox-Thaibinh",
                defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Index2",
                url: "Inox-ThaiBinh",
                defaults: new { controller = "Home", action = "Index2", id = UrlParameter.Optional }
            );
            
            routes.Add("Category", new GetSEOFriendlyRoute("San-Pham-Inox/{id}",
            new RouteValueDictionary(new { controller = "Home", action = "Category" }),
            new MvcRouteHandler()));

            routes.MapRoute(
                name: "Category1",
                url: "San-Pham-Inox",
                defaults: new { controller = "Home", action = "Category" }
            );  
            routes.MapRoute(
                name: "Customer1",
                url: "Du-An-Inox/{id}",
                defaults: new { controller = "Home", action = "Customer", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Contact",
                url: "Lien-He-Inox-ThaiBinh",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );
            
            routes.Add("ProductDetails", new GetSEOFriendlyRoute("Chi-tiet-Inox/{id}",
            new RouteValueDictionary(new { controller = "Home", action = "Product" }),
            new MvcRouteHandler()));
            // route mặc định admin
            //routes.MapRoute(
            //    name: "DefaultAdmin",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Admin", id = "Product" }
            //    );
            // route mặc định
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index2", id = UrlParameter.Optional }
            );
           
        }
    }
}

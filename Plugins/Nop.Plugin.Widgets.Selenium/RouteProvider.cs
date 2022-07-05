using System.Web;
using System.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using Nop.Plugin.Widgets.Selenium.Infrastructure;


namespace Nop.Plugin.Widgets.Selenium
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Widgets.Selenium.ManageSelenium",
                 "Selenium/Manage",
                 new { controller = "Selenium", action = "Manage" },
                 new[] { "Nop.Plugin.Widgets.Selenium.Controllers" }
            );

            ViewEngines.Engines.Insert(0, new CustomViewEngine());
            ViewEngines.Engines.Add(new CustomViewEngine());
        }
    }
}

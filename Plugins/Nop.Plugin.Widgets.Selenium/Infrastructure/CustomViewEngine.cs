using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Widgets.Selenium.Infrastructure
{
    public class CustomViewEngine : ThemeableRazorViewEngine
    {
        public CustomViewEngine()
        {
            ViewLocationFormats = new[] { "~/Plugins/Widgets.Selenium/Views/{0}.cshtml" };
            PartialViewLocationFormats = new[] { "~/Plugins/Widgets.Selenium/Views/{0}.cshtml" };
        }
    }
}

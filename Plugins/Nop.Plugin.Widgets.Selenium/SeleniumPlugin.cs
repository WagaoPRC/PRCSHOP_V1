using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;
using Nop.Plugin.Widgets.Selenium.Data;
using Nop.Plugin.Widgets.Selenium.Domain;
using Nop.Core.Data;
using System.Web.Routing;

namespace Nop.Plugin.Widgets.Selenium
{
    public class SeleniumPlugin : BasePlugin, IAdminMenuPlugin
    {
        private SeleniumRecordObjectContext _context;
        private IRepository<SeleniumRecord> _seleniumRepository;

        public SeleniumPlugin(SeleniumRecordObjectContext context, IRepository<SeleniumRecord> seleniumRepository)
        {
            _context = context;
            _seleniumRepository = seleniumRepository;
        }
        public bool Authenticate()
        {
            return true;
        }
        public override void Install()
        {
            _context.Install();
            base.Install();
        }
        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            SiteMapNode menuItem = new SiteMapNode
            {
                Title = "Selenium Price",
                Visible = true,
                ControllerName = "Selenium"
                ,Url = "/Selenium/Manage"
                ,RouteValues = new RouteValueDictionary()
                {
                    { "Namespaces", "Nop.Plugin.Widgets.Selenium.Controller" },
                    { "area", null}
                }
            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Infrastructure.DependencyManagement;
using Autofac;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.Selenium.Data;
using Nop.Web.Framework.Mvc;
using Nop.Data;
using Nop.Plugin.Widgets.Selenium.Domain;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Plugin.Widgets.Selenium.Service;
using Nop.Services.Catalog;

namespace Nop.Plugin.Widgets.Selenium.Infrastructure
{
    public class DependencyRegistrar:IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_widgets_selenium";
 
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //builder.RegisterType<Service>().As<IViewTrackingService>().InstancePerLifetimeScope();
            //data context
            this.RegisterPluginDataContext<SeleniumRecordObjectContext>(builder, CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<SeleniumRecord>>()
                .As<IRepository<SeleniumRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            
            //service
            builder.RegisterType<SeleniumService>().As<ISeleniumService>().InstancePerLifetimeScope();
        }
        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 1; }
        }
    }
}

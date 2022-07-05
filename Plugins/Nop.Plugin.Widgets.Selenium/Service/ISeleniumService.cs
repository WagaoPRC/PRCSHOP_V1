using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.Selenium.Domain;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Catalog;

namespace Nop.Plugin.Widgets.Selenium.Service
{
    public partial interface ISeleniumService
    {
        Ncards UpdatePrice(Ncards oProduct);
        void UpdatePrice(Product oProduct);
        bool VerifyChangePrice(Product oProduct);
        List<Ligamagic> ReadyTable(IReadOnlyCollection<IWebElement> TableElement);
        void GetLigaMagicData(IWebDriver oEdgeDriver, IList<Ncards> lstNcards);

    }

}

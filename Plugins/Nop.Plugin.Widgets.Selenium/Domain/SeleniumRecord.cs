using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using OpenQA.Selenium;

namespace Nop.Plugin.Widgets.Selenium.Domain
{
    public class SeleniumRecord : BaseEntity
    {
        //BaseSelenium
        public static IWebDriver Driver;

        public string ModificationType { get; set; }//UpPrice, DownPrice, ConsultPrice, WarningPrice
        public int StoreID{ get; set; }
        public string StoreName { get; set; }
        public int NcardID { get; set; }
        public string ProductSKU { get; set; }
        public int ProductWarehouseInventoryDomainID { get; set; }
        public string ShortDescription { get; set; }

        #region Table
        public int rowNumber { get; set; }
        public string columnName { get; set; }
        public string columValue { get; set; }
        #endregion
    }


}

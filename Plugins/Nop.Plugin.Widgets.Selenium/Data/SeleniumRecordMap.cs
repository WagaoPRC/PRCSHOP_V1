using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Nop.Plugin.Widgets.Selenium.Domain;

namespace Nop.Plugin.Widgets.Selenium.Data
{
   public class SeleniumRecordMap: EntityTypeConfiguration<SeleniumRecord>
   {
        public SeleniumRecordMap()
        {
            ToTable("Selenium");
            HasKey(selenium => selenium.Id);

            Property(selenium => selenium.ModificationType);
            Property(selenium => selenium.NcardID);
            Property(selenium => selenium.StoreID);
            Property(selenium => selenium.ProductSKU);
            Property(selenium => selenium.ProductWarehouseInventoryDomainID);
            Property(selenium => selenium.ShortDescription);
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class NsetsMap : NopEntityTypeConfiguration<Nsets>
    {
        public NsetsMap()
        {
            ToTable("Nsets");
            HasKey(nsets => nsets.Id);
            Property(nsets => nsets.Nname).IsRequired();
            Property(nsets => nsets.Ncode).IsRequired();
            Property(nsets => nsets.Ncode_magiccards);
            Property(nsets => nsets.Ndate);
            Property(nsets => nsets.Nis_promo);
            Property(nsets => nsets.Nboosterpack_nM);
            Property(nsets => nsets.Nboosterpack_nR);
            Property(nsets => nsets.Nboosterpack_nU);
            Property(nsets => nsets.Nboosterpack_nC);
            Property(nsets => nsets.Nboosterpack_nE);
            Property(nsets => nsets.Nboosterpack_pM);
            Property(nsets => nsets.Nboosterpack_pR);
            Property(nsets => nsets.Nboosterpack_typeExtra1);
            Property(nsets => nsets.Nboosterpack_typeExtra2);
            Property(nsets => nsets.Nboosterpack_listExtra1);
            Property(nsets => nsets.Nboosterpack_listExtra2);
            Property(nsets => nsets.Nboosterpack_has_foil);
            Property(nsets => nsets.Nboosterpack_pF);
            Property(nsets => nsets.Nboosterpack_hasmasterpiece);
            Property(nsets => nsets.Nboosterpack_pPM);
            Property(nsets => nsets.Nboosterpack_listPMid);
        }
    }
}

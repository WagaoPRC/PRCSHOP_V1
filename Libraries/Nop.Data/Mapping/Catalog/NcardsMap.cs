using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class NcardsMap : NopEntityTypeConfiguration<Ncards>
    {
        public NcardsMap()
        {
            ToTable("Ncards");
            HasKey(ncards => ncards.Id);
            Property(ncards => ncards.Nname);
            Property(ncards => ncards.Nset);
            Property(ncards => ncards.Ntype);
            Property(ncards => ncards.Nrarity);
            Property(ncards => ncards.Nmanacost);
            Property(ncards => ncards.Nconverted_manacost);
            Property(ncards => ncards.Npower);
            Property(ncards => ncards.Ntoughness);
            Property(ncards => ncards.Nloyalty);
            Property(ncards => ncards.Nability);
            Property(ncards => ncards.Nflavor);
            Property(ncards => ncards.Nvariation);
            Property(ncards => ncards.Nartist);
            Property(ncards => ncards.Nnumber);
            Property(ncards => ncards.Nrating);
            Property(ncards => ncards.Nruling);
            Property(ncards => ncards.Ncolor);
            Property(ncards => ncards.Ngenerated_mana);
            Property(ncards => ncards.Npricing_low);
            Property(ncards => ncards.Npricing_mid);
            Property(ncards => ncards.Npricing_high);
            Property(ncards => ncards.Nback_id);
            Property(ncards => ncards.Nwatermark);
            Property(ncards => ncards.Nprint_number);
            Property(ncards => ncards.Nis_original);
            Property(ncards => ncards.Ncolor_identity);            
            Property(ncards => ncards.Nname_CN);
            Property(ncards => ncards.Nname_TW);
            Property(ncards => ncards.Nname_FR);
            Property(ncards => ncards.Nname_DE);
            Property(ncards => ncards.Nname_IT);
            Property(ncards => ncards.Nname_JP);
            Property(ncards => ncards.Nname_PT);
            Property(ncards => ncards.Nname_RU);
            Property(ncards => ncards.Nname_ES);
            Property(ncards => ncards.Nname_KO);
            Property(ncards => ncards.Ntype_CN);
            Property(ncards => ncards.Ntype_TW);
            Property(ncards => ncards.Ntype_FR);
            Property(ncards => ncards.Ntype_DE);
            Property(ncards => ncards.Ntype_IT);
            Property(ncards => ncards.Ntype_JP);
            Property(ncards => ncards.Ntype_PT);
            Property(ncards => ncards.Ntype_RU);
            Property(ncards => ncards.Ntype_ES);
            Property(ncards => ncards.Ntype_KO);
            Property(ncards => ncards.Nability_CN);
            Property(ncards => ncards.Nability_TW);
            Property(ncards => ncards.Nability_FR);
            Property(ncards => ncards.Nability_DE);
            Property(ncards => ncards.Nability_IT);
            Property(ncards => ncards.Nability_JP);
            Property(ncards => ncards.Nability_PT);
            Property(ncards => ncards.Nability_RU);
            Property(ncards => ncards.Nability_ES);
            Property(ncards => ncards.Nability_KO);
            Property(ncards => ncards.Nflavor_CN);
            Property(ncards => ncards.Nflavor_TW);
            Property(ncards => ncards.Nflavor_FR);
            Property(ncards => ncards.Nflavor_DE);
            Property(ncards => ncards.Nflavor_IT);
            Property(ncards => ncards.Nflavor_JP);
            Property(ncards => ncards.Nflavor_PT);
            Property(ncards => ncards.Nflavor_RU);
            Property(ncards => ncards.Nflavor_ES);
            Property(ncards => ncards.Nflavor_KO);
            Property(ncards => ncards.Nlegality_Block);
            Property(ncards => ncards.Nlegality_Standard);
            Property(ncards => ncards.Nlegality_Modern);
            Property(ncards => ncards.Nlegality_Legacy);
            Property(ncards => ncards.Nlegality_Vintage);
            Property(ncards => ncards.Nlegality_Highlander);
            Property(ncards => ncards.Nlegality_French_Commander);
            Property(ncards => ncards.Nlegality_Tiny_Leaders_Commander);
            Property(ncards => ncards.Nlegality_Leviathan_Commander);
            Property(ncards => ncards.Nlegality_Commander);
            Property(ncards => ncards.Nlegality_Peasant);
            Property(ncards => ncards.Nlegality_Pauper);           

        }
    }
}

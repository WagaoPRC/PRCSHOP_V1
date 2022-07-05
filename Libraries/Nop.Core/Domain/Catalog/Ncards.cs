

namespace Nop.Core.Domain.Catalog
{
    using Nop.Core.Domain.Localization;
    using Nop.Core.Domain.Seo;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Ncards : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        [Column(TypeName = "nvarchar(MAX)")]
        public string Nid { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nset { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nrarity { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nmanacost { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nconverted_manacost { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Npower { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntoughness { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nloyalty { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nvariation { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nartist { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nnumber { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nrating { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nruling { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ncolor { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ngenerated_mana { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Npricing_low { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Npricing_mid { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Npricing_high { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nback_id { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nwatermark { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nprint_number { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nis_original { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ncolor_identity { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_CN { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_TW { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_FR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_DE { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_IT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_JP { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_PT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_RU { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_ES { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nname_KO { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_CN { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_TW { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_FR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_DE { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_IT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_JP { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_PT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_RU { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_ES { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ntype_KO { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_CN { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_TW { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_FR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_DE { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_IT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_JP { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_PT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_RU { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_ES { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nability_KO { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_CN { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_TW { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_FR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_DE { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_IT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_JP { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_PT { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_RU { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_ES { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nflavor_KO { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Block { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Standard { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Modern { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Legacy { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Vintage { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Highlander { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_French_Commander { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Tiny_Leaders_Commander { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Leviathan_Commander { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Commander { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Peasant { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nlegality_Pauper { get; set; }

    }
}

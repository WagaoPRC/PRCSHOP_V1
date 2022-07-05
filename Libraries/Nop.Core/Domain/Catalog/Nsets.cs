namespace Nop.Core.Domain.Catalog
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Localization;
    using Seo;
    using System;

    public partial class Nsets: BaseEntity, ILocalizedEntity, ISlugSupported
    {
        
        [Column(TypeName = "nvarchar(MAX)")]
        [Required]
        public string Nname { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ncode { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Ncode_magiccards { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Ndate { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nis_promo { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_nM { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_nR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_nU { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_nC { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_nE { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_pM { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_pR { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_typeExtra1 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_typeExtra2 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_listExtra1 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_listExtra2 { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_has_foil { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_pF { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_hasmasterpiece { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_pPM { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nboosterpack_listPMid { get; set; }
    }
}

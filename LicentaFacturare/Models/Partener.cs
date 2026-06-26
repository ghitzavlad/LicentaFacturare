using System.ComponentModel.DataAnnotations;

namespace LicentaFacturare.Models
{
    public class Partener
        {
        public int Id { get; set; }

        [Required]
        public string Cod { get; set; } = string.Empty;

        [Required]
        public string Denumire { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CNP sau Cod Fiscal")]
        public string CnpCodFiscal { get; set; } = string.Empty;

        [Display(Name = "Numar Reg. Comertului")]
        public string? NrRegComertului { get; set; } = string.Empty;

        public string? Tara { get; set; }

        public string? Judet { get; set; }

        public string? Localitate { get; set; }

        public string? Adresa { get; set; }

        [Display(Name = "Cont Bancar")]
        public string? ContBancar { get; set; }

        [Required]
        [Display(Name = "Tip Partener")]
        public TipPartener TipPartener { get; set; }

        public List<Factura> Facturi { get; set; } = new();

    }

    public enum TipPartener
    {
        Client = 1,
        Furnizor = 2
    }
}

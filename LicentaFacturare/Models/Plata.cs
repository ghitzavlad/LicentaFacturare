using System.ComponentModel.DataAnnotations;

namespace LicentaFacturare.Models
{
    public class Plata
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ID Factura")]

        public int FacturaId { get; set; }

        public Factura? Factura { get; set; }

        [Required]
        [Display(Name = "Data plata")]
        public DateTime DataPlata { get; set; } = DateTime.Today;

        [Required]
        public decimal Suma { get; set; }

        [Required]
        [Display(Name = "Metoda de plata")]
        public MetodaPlata MetodaPlata { get; set; }

        public string? Observatii { get; set; }

        [Display(Name = "IBAN")]
        public string? IBAN { get; set; }
    }

    public enum MetodaPlata
    {
        Transfer = 1,
        Card = 2,
        Numerar = 3
    }
}
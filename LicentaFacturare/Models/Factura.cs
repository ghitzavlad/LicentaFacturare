using System.ComponentModel.DataAnnotations;

namespace LicentaFacturare.Models
{
    public class Factura
        {
        public int Id { get; set; } 

        [Required]
        [Display(Name = "Partener")]
        public int PartenerId { get; set; }

        public Partener? Partener { get; set; } = default!;

        [Required]
        public string Serie { get; set; } = string.Empty;

        [Required]
        public int Numar { get; set; }

        [Required]
        [Display(Name = "Data Emitere")]
        public DateTime DataEmitere { get; set; } = DateTime.Today;
        /* test comment dupa push pe git*/

        [Required]
        [Display(Name = "Data Scadenta")]
        public DateTime DataScadenta { get; set; } = DateTime.Today.AddDays(30);

        [Required]
        [Display(Name = "Tip Factura")]
        public TipFactura TipFactura { get; set; }

        [Required]
        public StareFactura Stare { get; set; } = StareFactura.Ciorna;

        public List<LinieFactura> LinieFactura { get; set; } = new();

        public List<Plata> Plati { get; set; } = new();

    }

    public enum TipFactura
    {
        Vanzare = 1,
        Achizitie = 2
    }

    public enum StareFactura
    {
        Ciorna = 1,
        Emisa = 2,
        Respinsa = 3,
        Platita = 4
    }
}

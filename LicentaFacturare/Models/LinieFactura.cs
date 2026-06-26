using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LicentaFacturare.Models
{
    public class LinieFactura
        {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ID Factura")]
        public int FacturaId { get; set; }

        public Factura? Factura { get; set; }

        [Required]
        [Display(Name = "Produs")]
        public int ProdusId { get; set; }

        public Produs? Produs { get; set; }

        public decimal Cantitate { get; set; }

        [Display(Name = "Pret fara TVA")]
        public decimal? PretUnitarFaraTVA { get; set; }

        [Display(Name = "Pret cu TVA")]
        public decimal? PretUnitarCuTVA { get; set; }

        [Display(Name = "Valoare (Cantitate * Pret)")]
        public decimal? Valoare { get; set; }

        [Display(Name = "Valoare TVA")]
        public decimal? ValoareTVA { get; set; }

        [Display(Name = "Total")]

        public decimal? TotalLinie { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LicentaFacturare.Models
{
    public class Produs
        {
        public int Id { get; set; }

        [Required]
        public string Cod { get; set; } = string.Empty;

        [Required]
        public string Denumire { get; set; } = string.Empty;

        [Required]
        public decimal Pret { get; set; }

        public bool AreTVA { get; set; }
    }
}

using LicentaFacturare.Data;
using LicentaFacturare.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LicentaFacturare.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public decimal TotalFacturi { get; set; }
        public decimal TotalIncasat { get; set; }
        public decimal RestDeIncasat { get; set; }

        public int FacturiPlatite { get; set; }
        public int FacturiNeplatite { get; set; }
        public int TotalParteneri { get; set; }
        public int TotalProduse { get; set; }

        public int FacturiCiorna { get; set; }
        public int FacturiEmise { get; set; }
        public int FacturiRespinse { get; set; }

        public List<string> LuniIncasari { get; set; } = new();
        public List<decimal> ValoriIncasari { get; set; } = new();
        public async Task OnGetAsync()
        {
            var facturi = await _context.Facturi
                .Include(f => f.LinieFactura)
                .Include(f => f.Plati)
                .ToListAsync();

            TotalFacturi = facturi.Sum(f => f.LinieFactura.Sum(l => l.TotalLinie ?? 0));

            var facturiVanzare = facturi
                .Where(f => f.TipFactura == TipFactura.Vanzare)
                .ToList();

            var totalFacturiVanzare = facturiVanzare
                .Sum(f => f.LinieFactura.Sum(l => l.TotalLinie ?? 0));

            TotalIncasat = facturiVanzare
                .Sum(f => f.Plati.Sum(p => p.Suma));

            RestDeIncasat = totalFacturiVanzare - TotalIncasat;

            FacturiPlatite = facturi.Count(f => f.Stare == StareFactura.Platita);
            FacturiNeplatite = facturi.Count(f => f.Stare != StareFactura.Platita);

            TotalParteneri = await _context.Parteneri.CountAsync();
            TotalProduse = await _context.Produse.CountAsync();

            FacturiCiorna = facturi.Count(f => f.Stare == StareFactura.Ciorna);
            FacturiEmise = facturi.Count(f => f.Stare == StareFactura.Emisa);
            FacturiRespinse = facturi.Count(f => f.Stare == StareFactura.Respinsa);

            var incasariPeLuni = await _context.Plati
                .Include(p => p.Factura)
                .Where(p => p.Factura != null && p.Factura.TipFactura == TipFactura.Vanzare)
                .GroupBy(p => new { p.DataPlata.Year, p.DataPlata.Month })
                .Select(g => new
                 {
                    An = g.Key.Year,
                    Luna = g.Key.Month,
                    Total = g.Sum(p => p.Suma)
                 })
                .OrderBy(x => x.An)
                .ThenBy(x => x.Luna)
                .ToListAsync();

            LuniIncasari = incasariPeLuni
                .Select(x => $"{x.Luna}/{x.An}")
                .ToList();

            ValoriIncasari = incasariPeLuni
                .Select(x => x.Total)
                .ToList();
        }
    }
}
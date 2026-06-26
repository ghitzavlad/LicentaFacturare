using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.Plati
{
    public class DeleteModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DeleteModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Plata Plata { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plata = await _context.Plati.FirstOrDefaultAsync(m => m.Id == id);

            if (plata == null)
            {
                return NotFound();
            }
            else
            {
                Plata = plata;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plata = await _context.Plati.FindAsync(id);
            if (plata != null)
            {
                Plata = plata;
                _context.Plati.Remove(Plata);
                await _context.SaveChangesAsync();
            }

            var facturaId = Plata.FacturaId;

            var totalFactura = _context.LiniiFactura
                .Where(l => l.FacturaId == facturaId)
                .Sum(l => l.TotalLinie ?? 0);

            var totalPlatit = _context.Plati
                .Where(p => p.FacturaId == facturaId)
                .Sum(p => p.Suma);

            var factura = await _context.Facturi.FindAsync(facturaId);

            if (factura != null)
            {
                if (totalPlatit >= totalFactura && totalFactura > 0)
                {
                    factura.Stare = StareFactura.Platita;
                }
                else
                {
                    factura.Stare = StareFactura.Emisa;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Facturi/Edit", new { id = facturaId });
        }
    }
}

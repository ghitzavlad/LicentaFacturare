using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.LiniiFactura
{
    public class DeleteModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DeleteModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LinieFactura LinieFactura { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liniefactura = await _context.LiniiFactura.FirstOrDefaultAsync(m => m.Id == id);

            if (liniefactura == null)
            {
                return NotFound();
            }
            else
            {
                LinieFactura = liniefactura;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var linie = await _context.LiniiFactura.FindAsync(id);

            if (linie == null)
            {
                return NotFound();
            }

            var facturaId = linie.FacturaId;

            var totalFacturaCurent = _context.LiniiFactura
                .Where(l => l.FacturaId == facturaId)
                .Sum(l => l.TotalLinie ?? 0);

            var totalFacturaDupaStergere = totalFacturaCurent - (linie.TotalLinie ?? 0);

            var totalPlatit = _context.Plati
                .Where(p => p.FacturaId == facturaId)
                .Sum(p => p.Suma);

            if (totalPlatit > totalFacturaDupaStergere)
            {
                ModelState.AddModelError("", "Linia nu poate fi stearsa. Totalul platit devine mai mare decat totalul facturii.");
                LinieFactura = linie;
                return Page();
            }

            _context.LiniiFactura.Remove(linie);
            await _context.SaveChangesAsync();

            var factura = await _context.Facturi.FindAsync(facturaId);

            if (factura != null)
            {
                if (totalPlatit >= totalFacturaDupaStergere && totalFacturaDupaStergere > 0)
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

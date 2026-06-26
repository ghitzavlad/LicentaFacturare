using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicentaFacturare.Data;
using LicentaFacturare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LicentaFacturare.Pages.Plati
{
    public class CreateModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public CreateModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? facturaId)
        {
            if (facturaId.HasValue)
            {
                var factura = _context.Facturi
                    .FirstOrDefault(f => f.Id == facturaId.Value);

                ViewData["FacturaAfisare"] = factura != null
                    ? factura.Serie + " " + factura.Numar
                    : facturaId.Value.ToString();

                Plata = new Plata
                {
                    FacturaId = facturaId.Value,
                    DataPlata = DateTime.Today,
                    MetodaPlata = MetodaPlata.Transfer
                };
            }

            return Page();
        }

        [BindProperty]
        public Plata Plata { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["FacturaId"] = new SelectList(_context.Facturi, "Id", "Serie", Plata.FacturaId);
                return Page();
            }

            var totalFactura = _context.LiniiFactura
                .Where(l => l.FacturaId == Plata.FacturaId)
                .Sum(l => l.TotalLinie ?? 0);

            var totalPlatit = _context.Plati
                .Where(p => p.FacturaId == Plata.FacturaId)
                .Sum(p => p.Suma);

            var restDePlata = totalFactura - totalPlatit;

            if (Plata.Suma > restDePlata)
            {
                ModelState.AddModelError("Plata.Suma", $"Suma introdusa este prea mare. Rest de plata: {restDePlata}");
                return Page();
            }

            if (Plata.Suma <= 0)
            {
                ModelState.AddModelError("Plata.Suma", "Suma trebuie sa fie mai mare decat 0.");
                return Page();
            }

            _context.Plati.Add(Plata);
            await _context.SaveChangesAsync();

            if (Plata.Suma == restDePlata)
            {
                var factura = await _context.Facturi.FindAsync(Plata.FacturaId);

                if (factura != null)
                {
                    factura.Stare = StareFactura.Platita;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("/Facturi/Edit", new { id = Plata.FacturaId });
        }
    }
}

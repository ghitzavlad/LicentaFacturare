using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.LiniiFactura
{
    public class CreateModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public CreateModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? FacturaID)
        {
        ViewData["FacturaId"] = new SelectList(_context.Facturi, "Id", "Serie");
        ViewData["ProdusId"] = new SelectList(_context.Produse, "Id", "Denumire");
            if (FacturaID.HasValue)
            {
                LinieFactura = new LinieFactura
                {
                    FacturaId = FacturaID.Value
                };
            }

            return Page();
        }

        [BindProperty]
        public LinieFactura LinieFactura { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var produs = await _context.Produse.FindAsync(LinieFactura.ProdusId);

            if (produs == null)
            {
                return NotFound();
            }

            LinieFactura.PretUnitarFaraTVA = produs.Pret;
            const decimal TVA = 0.21m;


            if (produs.AreTVA)
            {
                LinieFactura.PretUnitarCuTVA = produs.Pret * (1 + TVA);
                LinieFactura.Valoare = LinieFactura.Cantitate * produs.Pret;
                LinieFactura.ValoareTVA = LinieFactura.Valoare * TVA;
                LinieFactura.TotalLinie = LinieFactura.Valoare + LinieFactura.ValoareTVA;
            }
            else
            {
                LinieFactura.PretUnitarCuTVA = produs.Pret;
                LinieFactura.Valoare = LinieFactura.Cantitate * produs.Pret;
                LinieFactura.ValoareTVA = 0;
                LinieFactura.TotalLinie = LinieFactura.Valoare;
            }

            _context.LiniiFactura.Add(LinieFactura);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Facturi/Details", new { id = LinieFactura.FacturaId });
        }
    }
}

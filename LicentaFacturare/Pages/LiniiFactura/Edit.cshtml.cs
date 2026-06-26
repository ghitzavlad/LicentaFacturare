using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.LiniiFactura
{
    public class EditModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public EditModel(LicentaFacturare.Data.ApplicationDbContext context)
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

            var liniefactura =  await _context.LiniiFactura.FirstOrDefaultAsync(m => m.Id == id);
            if (liniefactura == null)
            {
                return NotFound();
            }
            LinieFactura = liniefactura;
           ViewData["FacturaId"] = new SelectList(_context.Facturi, "Id", "Serie");
           ViewData["ProdusId"] = new SelectList(_context.Produse, "Id", "Cod");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LinieFactura).State = EntityState.Modified;


            var produs = await _context.Produse.FindAsync(LinieFactura.ProdusId);

            if (produs == null)
            {
                return NotFound();
            }

            var pret = LinieFactura.PretUnitarFaraTVA;
            const decimal TVA = 0.21m;

            if (produs.AreTVA)
            {
                LinieFactura.PretUnitarCuTVA = pret * (1 + TVA);
                LinieFactura.Valoare = LinieFactura.Cantitate * pret;
                LinieFactura.ValoareTVA = LinieFactura.Valoare * TVA;
                LinieFactura.TotalLinie = LinieFactura.Valoare + LinieFactura.ValoareTVA;
            }
            else
            {
                LinieFactura.PretUnitarCuTVA = pret;
                LinieFactura.Valoare = LinieFactura.Cantitate * pret;
                LinieFactura.ValoareTVA = 0;
                LinieFactura.TotalLinie = LinieFactura.Valoare;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinieFacturaExists(LinieFactura.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Facturi/Edit", new { id = LinieFactura.FacturaId });
        }

        private bool LinieFacturaExists(int id)
        {
            return _context.LiniiFactura.Any(e => e.Id == id);
        }
    }
}

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

namespace LicentaFacturare.Pages.Facturi
{
    public class EditModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public EditModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Factura Factura { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura =  await _context.Facturi.FirstOrDefaultAsync(m => m.Id == id);
            if (factura == null)
            {
                return NotFound();
            }
            Factura = await _context.Facturi
            .Include(f => f.Partener)
            .Include(f => f.LinieFactura)
            .ThenInclude(l => l.Produs)
            .Include(f => f.Plati)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (Factura == null)
            {
                return NotFound();
            }

            ViewData["PartenerId"] = new SelectList(
                _context.Parteneri,
                "Id",
                "Denumire",
                Factura.PartenerId
            );

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["PartenerId"] = new SelectList(
                    _context.Parteneri,
                    "Id",
                    "Denumire",
                    Factura.PartenerId
                );
            }

            var existaFactura = await _context.Facturi
                .AnyAsync(f =>
            f.Id != Factura.Id &&
            f.Serie == Factura.Serie &&
            f.Numar == Factura.Numar);


            if (existaFactura)
            {
                ModelState.AddModelError("", "Exista deja o factura cu aceeasi serie si numar.");
                ViewData["PartenerId"] = new SelectList(_context.Parteneri, "Id", "Denumire", Factura.PartenerId);
                return Page();
            }

            _context.Attach(Factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(Factura.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FacturaExists(int id)
        {
            return _context.Facturi.Any(e => e.Id == id);
        }
    }
}

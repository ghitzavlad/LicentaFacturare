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

namespace LicentaFacturare.Pages.Plati
{
    public class EditModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public EditModel(LicentaFacturare.Data.ApplicationDbContext context)
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

            var plata =  await _context.Plati.FirstOrDefaultAsync(m => m.Id == id);
            if (plata == null)
            {
                return NotFound();
            }
            Plata = await _context.Plati.Include(p => p.Factura).FirstOrDefaultAsync(m => m.Id == id);
            ViewData["FacturaId"] = new SelectList(_context.Facturi, "Id", "Serie");
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

            if (Plata.Suma <= 0)
            {
                ModelState.AddModelError("Plata.Suma", "Suma trebuie sa fie mai mare decat 0.");
                return Page();
            }

            _context.Attach(Plata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlataExists(Plata.Id))
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

        private bool PlataExists(int id)
        {
            return _context.Plati.Any(e => e.Id == id);
        }
    }
}

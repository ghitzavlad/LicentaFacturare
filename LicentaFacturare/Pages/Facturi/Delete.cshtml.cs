using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.Facturi
{
    public class DeleteModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DeleteModel(LicentaFacturare.Data.ApplicationDbContext context)
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

            var factura = await _context.Facturi.FirstOrDefaultAsync(m => m.Id == id);

            if (factura == null)
            {
                return NotFound();
            }
            else
            {
                Factura = factura;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Factura.Stare == StareFactura.Platita)
            {
                ModelState.AddModelError("", "Factura platita integral nu poate fi stearsa.");
                return Page();
            }

            var factura = await _context.Facturi.FindAsync(id);
            if (factura != null)
            {
                Factura = factura;
                _context.Facturi.Remove(Factura);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

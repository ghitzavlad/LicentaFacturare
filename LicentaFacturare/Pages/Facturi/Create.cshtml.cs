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

namespace LicentaFacturare.Pages.Facturi
{
    public class CreateModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public CreateModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Factura = new Factura
            {
                DataEmitere = DateTime.Today,
                DataScadenta = DateTime.Today.AddDays(30),
                Stare = StareFactura.Ciorna
            };

            ViewData["PartenerId"] = new SelectList(_context.Parteneri, "Id", "Denumire");
            return Page();
        }

        [BindProperty]
        public Factura Factura { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["PartenerId"] = new SelectList(_context.Parteneri, "Id", "Denumire", Factura.PartenerId);
                return Page();
            }

            var existaFactura = await _context.Facturi
                .AnyAsync(f => f.Serie == Factura.Serie && f.Numar == Factura.Numar);

            if (existaFactura)
            {
                ModelState.AddModelError("", "Exista deja o factura cu aceeasi serie si numar.");
                ViewData["PartenerId"] = new SelectList(_context.Parteneri, "Id", "Denumire", Factura.PartenerId);
                return Page();
            }

            _context.Facturi.Add(Factura);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

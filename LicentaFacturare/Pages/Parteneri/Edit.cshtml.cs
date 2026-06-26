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

namespace LicentaFacturare.Pages.Parteneri
{
    public class EditModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public EditModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Partener Partener { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partener =  await _context.Parteneri.FirstOrDefaultAsync(m => m.Id == id);
            if (partener == null)
            {
                return NotFound();
            }
            Partener = partener;
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

            _context.Attach(Partener).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartenerExists(Partener.Id))
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

        private bool PartenerExists(int id)
        {
            return _context.Parteneri.Any(e => e.Id == id);
        }
    }
}

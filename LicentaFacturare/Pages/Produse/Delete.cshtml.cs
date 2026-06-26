using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.Produse
{
    public class DeleteModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DeleteModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Produs Produs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produs = await _context.Produse.FirstOrDefaultAsync(m => m.Id == id);

            if (produs == null)
            {
                return NotFound();
            }
            else
            {
                Produs = produs;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produs = await _context.Produse.FindAsync(id);
            if (produs != null)
            {
                Produs = produs;
                _context.Produse.Remove(Produs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

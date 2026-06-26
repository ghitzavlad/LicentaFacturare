using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.Parteneri
{
    public class DeleteModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DeleteModel(LicentaFacturare.Data.ApplicationDbContext context)
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

            var partener = await _context.Parteneri.FirstOrDefaultAsync(m => m.Id == id);

            if (partener == null)
            {
                return NotFound();
            }
            else
            {
                Partener = partener;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partener = await _context.Parteneri.FindAsync(id);
            if (partener != null)
            {
                Partener = partener;
                _context.Parteneri.Remove(Partener);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

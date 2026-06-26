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
    public class DetailsModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DetailsModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}

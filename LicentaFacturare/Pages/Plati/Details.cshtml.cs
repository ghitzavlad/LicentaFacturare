using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.Plati
{
    public class DetailsModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DetailsModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Plata Plata { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plata = await _context.Plati.FirstOrDefaultAsync(m => m.Id == id);
            if (plata == null)
            {
                return NotFound();
            }
            else
            {
                Plata = await _context.Plati
                    .Include(p => p.Factura)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            return Page();
        }
    }
}

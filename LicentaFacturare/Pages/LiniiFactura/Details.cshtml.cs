using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LicentaFacturare.Data;
using LicentaFacturare.Models;

namespace LicentaFacturare.Pages.LiniiFactura
{
    public class DetailsModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public DetailsModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LinieFactura LinieFactura { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liniefactura = await _context.LiniiFactura.FirstOrDefaultAsync(m => m.Id == id);
            if (liniefactura == null)
            {
                return NotFound();
            }
            else
            {
                LinieFactura = liniefactura;
            }
            return Page();
        }
    }
}

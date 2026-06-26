using LicentaFacturare.Data;
using LicentaFacturare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LicentaFacturare.Pages.Facturi
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Factura Factura { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
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

            return Page();
        }
    }
}
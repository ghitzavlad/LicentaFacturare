using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicentaFacturare.Data;
using LicentaFacturare.Models;
using LicentaFacturare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LicentaFacturare.Pages.Facturi
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly FacturaPdf _facturaPdf;

        public IndexModel(ApplicationDbContext context, FacturaPdf facturaPdf)
        {
            _context = context;
            _facturaPdf = facturaPdf;
        }

        public IList<Factura> Factura { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Factura = await _context.Facturi
                .Include(f => f.Partener)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetPdfAsync(int id)
        {
            var factura = await _context.Facturi
                .Include(f => f.Partener)
                .Include(f => f.LinieFactura)
                    .ThenInclude(l => l.Produs)
                .Include(f => f.Plati)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
            {
                return NotFound();
            }

            var pdfBytes = _facturaPdf.Genereaza(factura);

            var fileName = $"Factura_{factura.Serie}_{factura.Numar}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
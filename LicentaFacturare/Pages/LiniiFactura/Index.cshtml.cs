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
    public class IndexModel : PageModel
    {
        private readonly LicentaFacturare.Data.ApplicationDbContext _context;

        public IndexModel(LicentaFacturare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LinieFactura> LinieFactura { get;set; } = default!;

        public async Task OnGetAsync()
        {
            LinieFactura = await _context.LiniiFactura
                .Include(l => l.Factura)
                .Include(l => l.Produs).ToListAsync();
        }
    }
}

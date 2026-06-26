using System.Xml.Linq;
using LicentaFacturare.Data;
using LicentaFacturare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LicentaFacturare.Pages.Import
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile? FisierXml { get; set; }
        [BindProperty]
        public List<PlataPreview> PlatiPreview { get; set; } = new();

        public SelectList FacturiList { get; set; } = default!;

        public void OnGet()
        {
            IncarcaFacturiList();
        }

        private void IncarcaFacturiList()
        {
            FacturiList = new SelectList(
                _context.Facturi
                    .Select(f => new
                    {
                        f.Id,
                        Text = f.Serie + " " + f.Numar
                    })
                    .ToList(),
                "Id",
                "Text"
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FisierXml == null || FisierXml.Length == 0)
            {
                ModelState.AddModelError("FisierXml", "Selecteaza un fisier XML.");
                return Page();
            }

            using var stream = FisierXml.OpenReadStream();
            var document = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

            XNamespace ns = document.Root!.Name.Namespace;

            var entries = document.Descendants(ns + "Ntry");

            foreach (var entry in entries)
            {
                var creditDebit = entry.Element(ns + "CdtDbtInd")?.Value;

                if (creditDebit != "CRDT")
                {
                    continue;
                }

                var sumaText = entry.Element(ns + "Amt")?.Value;
                var dataText = entry.Element(ns + "BookgDt")?.Element(ns + "Dt")?.Value;

                var txDetails = entry
                    .Element(ns + "NtryDtls")?
                    .Element(ns + "TxDtls");

                var referinta = txDetails?
                    .Element(ns + "RmtInf")?
                    .Element(ns + "Ustrd")?
                    .Value;

                var iban = txDetails?
                    .Element(ns + "RltdPties")?
                    .Element(ns + "DbtrAcct")?
                    .Element(ns + "Id")?
                    .Element(ns + "IBAN")?
                    .Value;

                if (!decimal.TryParse(sumaText, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var suma))
                {
                    continue;
                }

                if (!DateTime.TryParse(dataText, out var dataPlata))
                {
                    dataPlata = DateTime.Today;
                }

                var factura = GasesteFacturaDupaReferinta(referinta);

                PlatiPreview.Add(new PlataPreview
                {
                    Selectata = factura != null,
                    DataPlata = dataPlata,
                    Suma = suma,
                    IBAN = iban,
                    Referinta = referinta,
                    Observatii = referinta,
                    FacturaId = factura?.Id,
                    FacturaText = factura != null
                        ? $"{factura.Serie} {factura.Numar}"
                        : "Negasita",
                    MetodaPlata = MetodaPlata.Transfer
                });
            }

            IncarcaFacturiList();

            return Page();
        }

        public async Task<IActionResult> OnPostImportAsync()
        {
            IncarcaFacturiList();

            if (PlatiPreview == null || !PlatiPreview.Any(p => p.Selectata))
            {
                ModelState.AddModelError("", "Selecteaza cel putin o plata pentru import.");
                return Page();
            }

            foreach (var item in PlatiPreview.Where(p => p.Selectata))
            {
                if (item.FacturaId == null)
                {
                    ModelState.AddModelError("", "Toate platile selectate trebuie sa aiba factura selectata.");
                    return Page();
                }

                if (item.Suma <= 0)
                {
                    ModelState.AddModelError("", "Suma trebuie sa fie mai mare decat 0");
                    return Page();
                }

                var totalFactura = _context.LiniiFactura
                    .Where(l => l.FacturaId == item.FacturaId.Value)
                    .Sum(l => l.TotalLinie ?? 0);

                var totalPlatit = _context.Plati
                    .Where(p => p.FacturaId == item.FacturaId.Value)
                    .Sum(p => p.Suma);

                var restDePlata = totalFactura - totalPlatit;

                if (item.Suma > restDePlata)
                {
                    ModelState.AddModelError("", $"Plata de {item.Suma} depaseste totalul facturii. Rest de plata: {restDePlata}");
                    return Page();
                }

                var plata = new Plata
                {
                    FacturaId = item.FacturaId.Value,
                    DataPlata = item.DataPlata,
                    Suma = item.Suma,
                    IBAN = item.IBAN,
                    Observatii = item.Observatii,
                    MetodaPlata = item.MetodaPlata
                };

                _context.Plati.Add(plata);

                if (item.Suma >= restDePlata)
                {
                    var factura = await _context.Facturi.FindAsync(item.FacturaId.Value);

                    if (factura != null)
                    {
                        factura.Stare = StareFactura.Platita;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Plati/Index");
        }

        private Models.Factura? GasesteFacturaDupaReferinta(string? referinta)
        {
            if (string.IsNullOrWhiteSpace(referinta))
            {
                return null;
            }

            var referintaCurata = CurataText(referinta);

            var facturi = _context.Facturi.ToList();

            foreach (var factura in facturi)
            {
                var textCautat = CurataText($"{factura.Serie}{factura.Numar}");

                if (referintaCurata.Contains(textCautat, StringComparison.OrdinalIgnoreCase))
                {
                    return factura;
                }
            }

            return null;
        }
         private string CurataText(string text)
        {
            return text
                .Replace(" ", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace("/", "")
                .Replace("_", "");
        }
    }


    public class PlataPreview
    {
        public bool Selectata { get; set; }

        public DateTime DataPlata { get; set; }

        public decimal Suma { get; set; }

        public string? IBAN { get; set; }

        public string? Referinta { get; set; }

        public int? FacturaId { get; set; }

        public string? FacturaText { get; set; }

        public string? Observatii { get; set; }

        public MetodaPlata MetodaPlata { get; set; } = MetodaPlata.Transfer;
    }
}
using LicentaFacturare.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LicentaFacturare.Services
{
    public class FacturaPdf
    {
        public byte[] Genereaza(Factura factura)
        {
            var totalFactura = factura.LinieFactura.Sum(l => l.TotalLinie ?? 0);
            var totalPlatit = factura.Plati.Sum(p => p.Suma);
            var restDePlata = totalFactura - totalPlatit;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(35);
                    page.Size(PageSizes.A4);

                    page.Header().Column(header =>
                    {
                        header.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("FACTURA")
                                    .FontSize(24)
                                    .Bold();

                                col.Item().Text($"Serie: {factura.Serie}  Numar: {factura.Numar}")
                                    .FontSize(11);

                                col.Item().Text($"Data emitere: {factura.DataEmitere:dd.MM.yyyy}")
                                    .FontSize(11);

                                col.Item().Text($"Data scadenta: {factura.DataScadenta:dd.MM.yyyy}")
                                    .FontSize(11);
                            });

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text("LicentaFacturare")
                                    .FontSize(16)
                                    .Bold();

                                col.Item().Text("Aplicatie pentru gestionarea facturilor si platilor")
                                    .FontSize(10);

                                col.Item().Text("Document generat automat")
                                    .FontSize(10);
                            });
                        });

                        header.Item().PaddingTop(15).LineHorizontal(1);
                    });

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Spacing(16);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Partener")
                                    .FontSize(13)
                                    .Bold();

                                col.Item().PaddingTop(5).Text($"Denumire: {factura.Partener?.Denumire}");
                                col.Item().Text($"CNP/CIF: {factura.Partener?.CnpCodFiscal}");
                                col.Item().Text($"Adresa: {factura.Partener?.Adresa}");
                                col.Item().Text($"Cont bancar: {factura.Partener?.ContBancar}");
                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Detalii factura")
                                    .FontSize(13)
                                    .Bold();

                                col.Item().PaddingTop(5).Text($"Tip factura: {factura.TipFactura}");
                                col.Item().Text($"Stare: {factura.Stare}");
                                col.Item().Text($"Data emitere: {factura.DataEmitere:dd.MM.yyyy}");
                                col.Item().Text($"Data scadenta: {factura.DataScadenta:dd.MM.yyyy}");
                            });
                        });

                        column.Item().Text("Linii factura")
                            .FontSize(13)
                            .Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#eef3f6").Padding(5).Text("Produs").Bold();
                                header.Cell().Background("#eef3f6").Padding(5).AlignRight().Text("Cantitate").Bold();
                                header.Cell().Background("#eef3f6").Padding(5).AlignRight().Text("Pret").Bold();
                                header.Cell().Background("#eef3f6").Padding(5).AlignRight().Text("TVA").Bold();
                                header.Cell().Background("#eef3f6").Padding(5).AlignRight().Text("Total").Bold();
                            });

                            foreach (var linie in factura.LinieFactura)
                            {
                                table.Cell().BorderBottom(1).BorderColor("#dddddd").Padding(5)
                                    .Text(linie.Produs?.Denumire ?? "");

                                table.Cell().BorderBottom(1).BorderColor("#dddddd").Padding(5).AlignRight()
                                    .Text(linie.Cantitate.ToString("0.##"));

                                table.Cell().BorderBottom(1).BorderColor("#dddddd").Padding(5).AlignRight()
                                    .Text($"{linie.PretUnitarFaraTVA ?? 0:0.00}");

                                table.Cell().BorderBottom(1).BorderColor("#dddddd").Padding(5).AlignRight()
                                    .Text($"{linie.ValoareTVA ?? 0:0.00}");

                                table.Cell().BorderBottom(1).BorderColor("#dddddd").Padding(5).AlignRight()
                                    .Text($"{linie.TotalLinie ?? 0:0.00}");
                            }
                        });

                        column.Item().AlignRight().Width(220).Column(total =>
                        {
                            total.Item().PaddingTop(8).Row(row =>
                            {
                                row.RelativeItem().Text("Total factura:");
                                row.ConstantItem(90).AlignRight().Text($"{totalFactura:0.00} lei").Bold();
                            });

                            total.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Total platit:");
                                row.ConstantItem(90).AlignRight().Text($"{totalPlatit:0.00} lei");
                            });

                            total.Item().LineHorizontal(1);

                            total.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Rest de plata:").Bold();
                                row.ConstantItem(90).AlignRight().Text($"{restDePlata:0.00} lei").Bold();
                            });
                        });
                    });

                    page.Footer().Column(footer =>
                    {
                        footer.Item().LineHorizontal(1);

                        footer.Item().PaddingTop(8).AlignCenter()
                            .Text("Document generat automat de aplicatia LicentaFacturare")
                            .FontSize(9);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
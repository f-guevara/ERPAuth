using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using ERPAuth.Client.Models;
using QuestPDF.Helpers;

public class PdfService
{
    private const decimal VAT_RATE = 0.19m;
    private const int PAGE_MARGIN = 20;

    public byte[] GenerateOrderPdf(Order order)
    {
        if (order?.Items == null || !order.Items.Any())
            throw new InvalidOperationException("Order or Order items are not properly populated.");

        var orderItems = order.Items.ToList();
        var subtotal = orderItems.Sum(item => item.Quantity * item.Price);
        var vat = subtotal * VAT_RATE;
        var total = subtotal + vat;

        try
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(PAGE_MARGIN);
                    page.Content().Column(column =>
                    {
                        // Header
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("M.C. Medizintechnik-Export GmbH & Co.KG").FontSize(16).Bold();
                                col.Item().Text("Eltastr. 2, D-78573 Wurmlingen");
                                col.Item().Text("Tel: 07461/1717971");
                                col.Item().Text("Email: macontardi@mcm-export.de");
                            });
                        });

                        // Customer and Order Details
                        column.Item().Text($"Kunde: {order.Customer?.FirstName ?? "Unknown"} {order.Customer?.LastName ?? "Unknown"}").FontSize(14).Bold();
                        column.Item().Text($"Adresse: {order.Customer?.Address ?? "No Address"}, {order.Customer?.City ?? "No City"}");
                        column.Item().Text($"Datum: {order.OrderDate.ToShortDateString()}");
                        column.Item().Text($"Bestellnummer: {order.Id}").FontSize(14).Bold();

                        // Table Header
                        column.Item().LineHorizontal(1);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Pos.").Bold();
                                header.Cell().Text("Artikel").Bold();
                                header.Cell().Text("Losnummer").Bold();
                                header.Cell().Text("Menge").Bold();
                                header.Cell().Text("Stückpreis €").Bold();
                                header.Cell().Text("Gesamt €").Bold();
                                header.Cell().Text("Lieferdatum").Bold();
                            });

                            for (int i = 0; i < orderItems.Count; i++)
                            {
                                var item = orderItems[i];
                                table.Cell().Text((i + 1).ToString());
                                table.Cell().Text(item.Article?.Name ?? "Unknown Article");
                                table.Cell().Text(item.Inventory?.LotNumber ?? "Not Assigned");
                                table.Cell().AlignRight().Text(item.Quantity.ToString());
                                table.Cell().AlignRight().Text(item.Price.ToString("F2"));
                                table.Cell().AlignRight().Text((item.Quantity * item.Price).ToString("F2"));
                                table.Cell().Text(item.DeliveryDate.ToShortDateString());
                            }
                        });

                        // Footer
                        column.Item().LineHorizontal(1);
                        column.Item().AlignRight().Column(footer =>
                        {
                            footer.Item().Text($"Zwischensumme €: {subtotal:F2}");
                            footer.Item().Text($"Mehrwertsteuer (19%) €: {vat:F2}");
                            footer.Item().Text($"Gesamtsumme €: {total:F2}").FontSize(14).Bold();
                        });

                        // Banking
                        column.Item().Text("Banking Arrangements:").Bold();
                        column.Item().Text("Volksbank Albstadt eG - IBAN: DE70 6539 0120 0396 1390 00");
                        column.Item().Text("Kreissparkasse Tuttlingen - IBAN: DE05 6435 0070 0008 5769 69");
                    });
                });
            }).GeneratePdf();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate PDF", ex);
        }
    }
}

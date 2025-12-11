using Cspdf;
using System.Drawing;

namespace Examples;

/// <summary>
/// Example demonstrating table creation
/// </summary>
public class TableExample
{
    public static void CreateTablePdf()
    {
        using var document = new PdfDocument();
        var page = document.AddPage(PageSize.A4, PageOrientation.Portrait);

        // Create a table
        var table = new PdfTable();
        table.ColumnWidths = new float[] { 150, 200, 150 };
        table.BorderColor = Color.Black;
        table.BorderWidth = 1f;
        table.HeaderBackgroundColor = Color.LightGray;
        table.AlternateRowColor = Color.WhiteSmoke;

        // Add header row
        var headerRow = table.AddHeaderRow();
        headerRow.AddCell("Product Name").Font = new Font("Arial", 12, FontStyle.Bold);
        headerRow.AddCell("Description").Font = new Font("Arial", 12, FontStyle.Bold);
        headerRow.AddCell("Price").Font = new Font("Arial", 12, FontStyle.Bold);

        // Add data rows
        table.AddRow("Laptop", "High-performance laptop", "$999.99");
        table.AddRow("Mouse", "Wireless optical mouse", "$29.99");
        table.AddRow("Keyboard", "Mechanical keyboard", "$79.99");
        table.AddRow("Monitor", "27-inch 4K monitor", "$399.99");
        table.AddRow("Headphones", "Noise-cancelling headphones", "$199.99");

        // Draw the table
        table.Draw(page.Graphics, 50, 50, 500);

        document.Save("table-example.pdf");
        Console.WriteLine("PDF with table created: table-example.pdf");
    }
}


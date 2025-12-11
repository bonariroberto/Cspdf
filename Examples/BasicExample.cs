using Cspdf;
using System.Drawing;

namespace Examples;

/// <summary>
/// Basic example demonstrating PDF creation
/// </summary>
public class BasicExample
{
    public static void CreateBasicPdf()
    {
        // Create a new PDF document
        using var document = new PdfDocument();

        // Set metadata
        document.Metadata.Title = "My First PDF";
        document.Metadata.Author = "John Doe";
        document.Metadata.Subject = "Example PDF";
        document.Metadata.Keywords = "example, pdf, cspdf";

        // Add a page
        var page = document.AddPage(PageSize.A4, PageOrientation.Portrait);
        var graphics = page.Graphics;

        // Draw a title
        using var titleFont = new Font("Arial", 24, FontStyle.Bold);
        using var titleBrush = new SolidBrush(Color.DarkBlue);
        graphics.DrawString("Welcome to Cspdf", titleFont, titleBrush, 50, 50);

        // Draw some text
        using var textFont = new Font("Arial", 12);
        using var textBrush = new SolidBrush(Color.Black);
        graphics.DrawString("This is a simple PDF document created with Cspdf library.", 
            textFont, textBrush, 50, 100);

        // Draw a rectangle
        using var pen = new Pen(Color.Blue, 2);
        graphics.DrawRectangle(pen, 50, 150, 500, 100);

        // Fill a rectangle
        using var brush = new SolidBrush(Color.LightBlue);
        graphics.FillRectangle(brush, 60, 160, 480, 80);

        // Draw a circle
        graphics.DrawEllipse(pen, 50, 300, 100, 100);
        graphics.FillEllipse(brush, 60, 310, 80, 80);

        // Save the document
        document.Save("basic-example.pdf");
        Console.WriteLine("PDF created: basic-example.pdf");
    }
}



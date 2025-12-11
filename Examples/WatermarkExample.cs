using Cspdf;
using System.Drawing;

namespace Examples;

/// <summary>
/// Example demonstrating watermark functionality
/// </summary>
public class WatermarkExample
{
    public static void CreateWatermarkedPdf()
    {
        using var document = new PdfDocument();

        // Add multiple pages
        for (int i = 0; i < 3; i++)
        {
            var page = document.AddPage();
            var graphics = page.Graphics;

            using var font = new Font("Arial", 14);
            using var brush = new SolidBrush(Color.Black);
            graphics.DrawString($"This is page {i + 1}", font, brush, 50, 50);
        }

        // Create text watermark
        var textWatermark = new Watermark
        {
            Text = "CONFIDENTIAL",
            Font = new Font("Arial", 48, FontStyle.Bold),
            Color = Color.FromArgb(128, 255, 0, 0),
            Rotation = -45f,
            Opacity = 0.3f,
            HorizontalPosition = 0.5f,
            VerticalPosition = 0.5f
        };

        // Apply watermark to all pages
        document.ApplyWatermark(textWatermark);

        document.Save("watermark-example.pdf");
        Console.WriteLine("Watermarked PDF created: watermark-example.pdf");
    }
}



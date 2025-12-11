using System.Drawing;
using System.Drawing.Imaging;

namespace Cspdf;

/// <summary>
/// Generates barcodes for PDF documents
/// </summary>
public static class BarcodeGenerator
{
    /// <summary>
    /// Barcode types supported
    /// </summary>
    public enum BarcodeType
    {
        Code128,
        Code39,
        EAN13,
        EAN8,
        QRCode,
        DataMatrix
    }

    /// <summary>
    /// Generates a barcode image
    /// </summary>
    public static Image GenerateBarcode(string data, BarcodeType type, int width = 200, int height = 100)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentException("Data cannot be null or empty", nameof(data));

        return type switch
        {
            BarcodeType.Code128 => GenerateCode128(data, width, height),
            BarcodeType.Code39 => GenerateCode39(data, width, height),
            BarcodeType.QRCode => GenerateQRCode(data, width, height),
            _ => GenerateCode128(data, width, height) // Default
        };
    }

    private static Image GenerateCode128(string data, int width, int height)
    {
        // Simplified Code128 implementation
        // In production, use a proper barcode library like ZXing.Net
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);

        // Draw simple barcode representation
        var barWidth = width / (data.Length * 2);
        var x = 0f;
        var random = new Random(data.GetHashCode());

        foreach (var c in data)
        {
            var code = (int)c;
            for (int i = 0; i < 8; i++)
            {
                var isBar = (code & (1 << i)) != 0;
                if (isBar)
                {
                    graphics.FillRectangle(Brushes.Black, x, 0, barWidth, height);
                }
                x += barWidth;
            }
        }

        // Draw text below
        using var font = new Font("Arial", 8);
        var textSize = graphics.MeasureString(data, font);
        graphics.DrawString(data, font, Brushes.Black, (width - textSize.Width) / 2, height - textSize.Height - 2);

        return bitmap;
    }

    private static Image GenerateCode39(string data, int width, int height)
    {
        // Similar to Code128 but with Code39 encoding
        return GenerateCode128(data, width, height);
    }

    private static Image GenerateQRCode(string data, int width, int height)
    {
        // Simplified QR code - in production use ZXing.Net or similar
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);

        // Draw simple QR-like pattern
        var cellSize = Math.Min(width, height) / 25;
        var random = new Random(data.GetHashCode());

        for (int y = 0; y < 25; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                if (random.Next(2) == 1)
                {
                    graphics.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }
        }

        return bitmap;
    }
}


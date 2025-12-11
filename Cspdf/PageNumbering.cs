using System.Drawing;

namespace Cspdf;

/// <summary>
/// Provides page numbering functionality
/// </summary>
public static class PageNumbering
{
    /// <summary>
    /// Adds page numbers to all pages in a document
    /// </summary>
    public static void AddPageNumbers(PdfDocument document, PageNumberOptions? options = null)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));

        options ??= new PageNumberOptions();

        for (int i = 0; i < document.Pages.Count; i++)
        {
            var page = document.Pages[i];
            var pageNumber = options.StartNumber + i;
            var text = options.Format.Replace("{page}", pageNumber.ToString())
                                      .Replace("{total}", document.Pages.Count.ToString());

            var graphics = page.Graphics;
            var x = CalculateXPosition(page, options.Position);
            var y = CalculateYPosition(page, options.Position);

            using var font = options.Font ?? new Font("Arial", 10);
            using var brush = new SolidBrush(options.Color);
            graphics.DrawString(text, font, brush, x, y);
        }
    }

    private static float CalculateXPosition(IPage page, PageNumberPosition position)
    {
        return position switch
        {
            PageNumberPosition.TopLeft or PageNumberPosition.BottomLeft => 50f,
            PageNumberPosition.TopCenter or PageNumberPosition.BottomCenter => (float)(page.Width / 2),
            PageNumberPosition.TopRight or PageNumberPosition.BottomRight => (float)(page.Width - 50),
            _ => 50f
        };
    }

    private static float CalculateYPosition(IPage page, PageNumberPosition position)
    {
        return position switch
        {
            PageNumberPosition.TopLeft or PageNumberPosition.TopCenter or PageNumberPosition.TopRight => 30f,
            PageNumberPosition.BottomLeft or PageNumberPosition.BottomCenter or PageNumberPosition.BottomRight => (float)(page.Height - 30),
            _ => 30f
        };
    }
}

/// <summary>
/// Page numbering options
/// </summary>
public class PageNumberOptions
{
    /// <summary>
    /// Gets or sets the position of page numbers
    /// </summary>
    public PageNumberPosition Position { get; set; } = PageNumberPosition.BottomCenter;

    /// <summary>
    /// Gets or sets the format string (use {page} for page number, {total} for total pages)
    /// </summary>
    public string Format { get; set; } = "{page} / {total}";

    /// <summary>
    /// Gets or sets the font for page numbers
    /// </summary>
    public Font? Font { get; set; }

    /// <summary>
    /// Gets or sets the color for page numbers
    /// </summary>
    public Color Color { get; set; } = Color.Black;

    /// <summary>
    /// Gets or sets the starting page number
    /// </summary>
    public int StartNumber { get; set; } = 1;
}

/// <summary>
/// Page number position
/// </summary>
public enum PageNumberPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}


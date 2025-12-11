using System.Drawing;

namespace Cspdf;

/// <summary>
/// Provides functionality to stamp (overlay) content on existing PDF documents
/// </summary>
public class PdfStamper
{
    private readonly PdfDocument _document;

    /// <summary>
    /// Initializes a new instance of the PdfStamper class
    /// </summary>
    public PdfStamper(PdfDocument document)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
    }

    /// <summary>
    /// Stamps text on a specific page
    /// </summary>
    public void StampText(int pageIndex, string text, float x, float y, Font? font = null, Brush? brush = null)
    {
        if (pageIndex < 0 || pageIndex >= _document.Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var page = _document.Pages[pageIndex];
        var graphics = page.Graphics;

        font ??= new Font("Arial", 12);
        brush ??= new SolidBrush(Color.Black);

        graphics.DrawString(text, font, brush, x, y);
    }

    /// <summary>
    /// Stamps an image on a specific page
    /// </summary>
    public void StampImage(int pageIndex, Image image, float x, float y)
    {
        if (pageIndex < 0 || pageIndex >= _document.Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var page = _document.Pages[pageIndex];
        page.Graphics.DrawImage(image, x, y);
    }

    /// <summary>
    /// Stamps an image on a specific page with scaling
    /// </summary>
    public void StampImage(int pageIndex, Image image, RectangleF bounds)
    {
        if (pageIndex < 0 || pageIndex >= _document.Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var page = _document.Pages[pageIndex];
        page.Graphics.DrawImage(image, bounds);
    }

    /// <summary>
    /// Stamps text on all pages
    /// </summary>
    public void StampTextOnAllPages(string text, float x, float y, Font? font = null, Brush? brush = null)
    {
        for (int i = 0; i < _document.Pages.Count; i++)
        {
            StampText(i, text, x, y, font, brush);
        }
    }

    /// <summary>
    /// Stamps a watermark on all pages
    /// </summary>
    public void StampWatermark(Watermark watermark)
    {
        if (watermark == null)
            throw new ArgumentNullException(nameof(watermark));

        foreach (var page in _document.Pages)
        {
            watermark.Apply(page);
        }
    }

    /// <summary>
    /// Stamps a table on a specific page
    /// </summary>
    public void StampTable(int pageIndex, PdfTable table, float x, float y, float width)
    {
        if (pageIndex < 0 || pageIndex >= _document.Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var page = _document.Pages[pageIndex];
        table.Draw(page.Graphics, x, y, width);
    }

    /// <summary>
    /// Stamps a form field on a specific page
    /// </summary>
    public void StampFormField(int pageIndex, PdfFormField field)
    {
        if (pageIndex < 0 || pageIndex >= _document.Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var page = _document.Pages[pageIndex];
        field.Draw(page.Graphics);
        _document.AddFormField(field);
    }
}



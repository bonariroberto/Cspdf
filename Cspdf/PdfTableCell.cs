using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents a cell in a PDF table
/// </summary>
public class PdfTableCell
{
    /// <summary>
    /// Gets or sets the cell text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the cell font
    /// </summary>
    public Font? Font { get; set; }

    /// <summary>
    /// Gets or sets the cell brush (for text color)
    /// </summary>
    public Brush? Brush { get; set; }

    /// <summary>
    /// Gets or sets the cell background color
    /// </summary>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment
    /// </summary>
    public StringAlignment HorizontalAlignment { get; set; } = StringAlignment.Near;

    /// <summary>
    /// Gets or sets the vertical alignment
    /// </summary>
    public StringAlignment VerticalAlignment { get; set; } = StringAlignment.Near;

    /// <summary>
    /// Initializes a new instance of the PdfTableCell class
    /// </summary>
    public PdfTableCell(string? text = null)
    {
        Text = text;
    }
}



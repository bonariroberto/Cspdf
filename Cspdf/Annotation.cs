using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents an annotation on a PDF page
/// </summary>
public abstract class Annotation
{
    /// <summary>
    /// Gets or sets the annotation bounds
    /// </summary>
    public RectangleF Bounds { get; set; }

    /// <summary>
    /// Gets or sets the annotation title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the annotation contents
    /// </summary>
    public string? Contents { get; set; }

    /// <summary>
    /// Gets or sets whether the annotation is visible
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets the annotation type
    /// </summary>
    public abstract string AnnotationType { get; }

    /// <summary>
    /// Draws the annotation on the page
    /// </summary>
    public abstract void Draw(IGraphics graphics);
}

/// <summary>
/// Represents a text annotation (sticky note)
/// </summary>
public class TextAnnotation : Annotation
{
    /// <summary>
    /// Gets or sets the icon type
    /// </summary>
    public string Icon { get; set; } = "Note";

    public override string AnnotationType => "Text";

    public override void Draw(IGraphics graphics)
    {
        if (!Visible) return;

        // Draw note icon
        using var brush = new SolidBrush(Color.Yellow);
        graphics.FillRectangle(brush, Bounds.X, Bounds.Y, 20, 20);

        using var pen = new Pen(Color.Black, 1);
        graphics.DrawRectangle(pen, Bounds.X, Bounds.Y, 20, 20);
    }
}

/// <summary>
/// Represents a highlight annotation
/// </summary>
public class HighlightAnnotation : Annotation
{
    /// <summary>
    /// Gets or sets the highlight color
    /// </summary>
    public Color HighlightColor { get; set; } = Color.Yellow;

    public override string AnnotationType => "Highlight";

    public override void Draw(IGraphics graphics)
    {
        if (!Visible) return;

        using var brush = new SolidBrush(Color.FromArgb(128, HighlightColor));
        graphics.FillRectangle(brush, Bounds);
    }
}

/// <summary>
/// Represents a link annotation
/// </summary>
public class LinkAnnotation : Annotation
{
    /// <summary>
    /// Gets or sets the destination page index
    /// </summary>
    public int? DestinationPage { get; set; }

    /// <summary>
    /// Gets or sets the destination URL
    /// </summary>
    public string? Url { get; set; }

    public override string AnnotationType => "Link";

    public override void Draw(IGraphics graphics)
    {
        if (!Visible) return;

        // Draw link border
        using var pen = new Pen(Color.Blue, 1);
        graphics.DrawRectangle(pen, Bounds);
    }
}

/// <summary>
/// Represents a free text annotation
/// </summary>
public class FreeTextAnnotation : Annotation
{
    /// <summary>
    /// Gets or sets the font
    /// </summary>
    public Font Font { get; set; } = new Font("Arial", 12);

    /// <summary>
    /// Gets or sets the text color
    /// </summary>
    public Color TextColor { get; set; } = Color.Black;

    public override string AnnotationType => "FreeText";

    public override void Draw(IGraphics graphics)
    {
        if (!Visible || string.IsNullOrEmpty(Contents)) return;

        using var brush = new SolidBrush(TextColor);
        graphics.DrawString(Contents, Font, brush, Bounds);
    }
}


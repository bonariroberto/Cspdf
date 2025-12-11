using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents a watermark that can be applied to PDF pages
/// </summary>
public class Watermark
{
    /// <summary>
    /// Gets or sets the watermark text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the watermark image
    /// </summary>
    public Image? Image { get; set; }

    /// <summary>
    /// Gets or sets the font for text watermark
    /// </summary>
    public Font Font { get; set; } = new Font("Arial", 48, FontStyle.Bold);

    /// <summary>
    /// Gets or sets the color for text watermark
    /// </summary>
    public Color Color { get; set; } = Color.FromArgb(128, 128, 128, 128);

    /// <summary>
    /// Gets or sets the rotation angle in degrees
    /// </summary>
    public float Rotation { get; set; } = -45f;

    /// <summary>
    /// Gets or sets the horizontal position (0.0 = left, 0.5 = center, 1.0 = right)
    /// </summary>
    public float HorizontalPosition { get; set; } = 0.5f;

    /// <summary>
    /// Gets or sets the vertical position (0.0 = top, 0.5 = center, 1.0 = bottom)
    /// </summary>
    public float VerticalPosition { get; set; } = 0.5f;

    /// <summary>
    /// Gets or sets the opacity (0.0 to 1.0)
    /// </summary>
    public float Opacity { get; set; } = 0.3f;

    /// <summary>
    /// Applies the watermark to a page
    /// </summary>
    public void Apply(IPage page)
    {
        if (page == null)
            throw new ArgumentNullException(nameof(page));

        var graphics = page.Graphics;
        var centerX = (float)(page.Width * HorizontalPosition);
        var centerY = (float)(page.Height * VerticalPosition);

        graphics.TranslateTransform(centerX, centerY);
        graphics.RotateTransform(Rotation);

        if (!string.IsNullOrEmpty(Text))
        {
            var brush = new SolidBrush(Color.FromArgb((int)(Opacity * 255), Color));
            var textSize = graphics.MeasureString(Text, Font);
            graphics.DrawString(Text, Font, brush, -textSize.Width / 2, -textSize.Height / 2);
        }
        else if (Image != null)
        {
            var imageWidth = Image.Width * Opacity;
            var imageHeight = Image.Height * Opacity;
            var rect = new RectangleF(-(float)imageWidth / 2, -(float)imageHeight / 2, (float)imageWidth, (float)imageHeight);
            graphics.DrawImage(Image, rect);
        }

        graphics.ResetTransform();
    }
}



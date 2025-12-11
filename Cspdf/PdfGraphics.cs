using System.Drawing;
using System.Drawing.Drawing2D;

namespace Cspdf;

/// <summary>
/// Provides methods for drawing on a PDF page
/// </summary>
public class PdfGraphics : IGraphics, IDisposable
{
    private readonly IPage _page;
    private readonly System.Drawing.Graphics _graphics;
    private readonly Bitmap _bitmap;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the PdfGraphics class
    /// </summary>
    public PdfGraphics(IPage page)
    {
        _page = page ?? throw new ArgumentNullException(nameof(page));
        _bitmap = new Bitmap((int)page.Width, (int)page.Height);
        _graphics = System.Drawing.Graphics.FromImage(_bitmap);
        _graphics.SmoothingMode = SmoothingMode.AntiAlias;
        _graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
    }

    /// <summary>
    /// Draws text on the page
    /// </summary>
    public void DrawString(string text, Font font, Brush brush, float x, float y)
    {
        _graphics.DrawString(text, font, brush, x, y);
    }

    /// <summary>
    /// Draws text within a rectangle
    /// </summary>
    public void DrawString(string text, Font font, Brush brush, RectangleF layoutRectangle)
    {
        _graphics.DrawString(text, font, brush, layoutRectangle);
    }

    /// <summary>
    /// Draws an image on the page
    /// </summary>
    public void DrawImage(Image image, float x, float y)
    {
        _graphics.DrawImage(image, x, y);
    }

    /// <summary>
    /// Draws an image scaled to fit a rectangle
    /// </summary>
    public void DrawImage(Image image, RectangleF destRect)
    {
        _graphics.DrawImage(image, destRect);
    }

    /// <summary>
    /// Draws a line
    /// </summary>
    public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
    {
        _graphics.DrawLine(pen, x1, y1, x2, y2);
    }

    /// <summary>
    /// Draws a rectangle
    /// </summary>
    public void DrawRectangle(Pen pen, float x, float y, float width, float height)
    {
        _graphics.DrawRectangle(pen, x, y, width, height);
    }

    /// <summary>
    /// Fills a rectangle
    /// </summary>
    public void FillRectangle(Brush brush, float x, float y, float width, float height)
    {
        _graphics.FillRectangle(brush, x, y, width, height);
    }

    /// <summary>
    /// Draws an ellipse
    /// </summary>
    public void DrawEllipse(Pen pen, float x, float y, float width, float height)
    {
        _graphics.DrawEllipse(pen, x, y, width, height);
    }

    /// <summary>
    /// Fills an ellipse
    /// </summary>
    public void FillEllipse(Brush brush, float x, float y, float width, float height)
    {
        _graphics.FillEllipse(brush, x, y, width, height);
    }

    /// <summary>
    /// Draws a polygon
    /// </summary>
    public void DrawPolygon(Pen pen, PointF[] points)
    {
        _graphics.DrawPolygon(pen, points);
    }

    /// <summary>
    /// Fills a polygon
    /// </summary>
    public void FillPolygon(Brush brush, PointF[] points)
    {
        _graphics.FillPolygon(brush, points);
    }

    /// <summary>
    /// Draws a path
    /// </summary>
    public void DrawPath(Pen pen, GraphicsPath path)
    {
        _graphics.DrawPath(pen, path);
    }

    /// <summary>
    /// Fills a path
    /// </summary>
    public void FillPath(Brush brush, GraphicsPath path)
    {
        _graphics.FillPath(brush, path);
    }

    /// <summary>
    /// Translates the coordinate system
    /// </summary>
    public void TranslateTransform(float dx, float dy)
    {
        _graphics.TranslateTransform(dx, dy);
    }

    /// <summary>
    /// Rotates the coordinate system
    /// </summary>
    public void RotateTransform(float angle)
    {
        _graphics.RotateTransform(angle);
    }

    /// <summary>
    /// Scales the coordinate system
    /// </summary>
    public void ScaleTransform(float sx, float sy)
    {
        _graphics.ScaleTransform(sx, sy);
    }

    /// <summary>
    /// Resets the transformation matrix
    /// </summary>
    public void ResetTransform()
    {
        _graphics.ResetTransform();
    }

    /// <summary>
    /// Measures the size of the specified text when drawn with the specified font
    /// </summary>
    public SizeF MeasureString(string text, Font font)
    {
        return _graphics.MeasureString(text, font);
    }

    /// <summary>
    /// Gets the internal bitmap for PDF generation
    /// </summary>
    internal Bitmap GetBitmap() => _bitmap;

    /// <summary>
    /// Disposes the graphics and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _graphics?.Dispose();
            _bitmap?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}


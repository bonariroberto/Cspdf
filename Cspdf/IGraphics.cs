using System.Drawing;

namespace Cspdf;

/// <summary>
/// Provides methods for drawing on a PDF page
/// </summary>
public interface IGraphics
{
    /// <summary>
    /// Draws text on the page
    /// </summary>
    void DrawString(string text, Font font, Brush brush, float x, float y);

    /// <summary>
    /// Draws text within a rectangle
    /// </summary>
    void DrawString(string text, Font font, Brush brush, RectangleF layoutRectangle);

    /// <summary>
    /// Draws an image on the page
    /// </summary>
    void DrawImage(Image image, float x, float y);

    /// <summary>
    /// Draws an image scaled to fit a rectangle
    /// </summary>
    void DrawImage(Image image, RectangleF destRect);

    /// <summary>
    /// Draws a line
    /// </summary>
    void DrawLine(Pen pen, float x1, float y1, float x2, float y2);

    /// <summary>
    /// Draws a rectangle
    /// </summary>
    void DrawRectangle(Pen pen, float x, float y, float width, float height);

    /// <summary>
    /// Fills a rectangle
    /// </summary>
    void FillRectangle(Brush brush, float x, float y, float width, float height);

    /// <summary>
    /// Draws an ellipse
    /// </summary>
    void DrawEllipse(Pen pen, float x, float y, float width, float height);

    /// <summary>
    /// Fills an ellipse
    /// </summary>
    void FillEllipse(Brush brush, float x, float y, float width, float height);

    /// <summary>
    /// Draws a polygon
    /// </summary>
    void DrawPolygon(Pen pen, PointF[] points);

    /// <summary>
    /// Fills a polygon
    /// </summary>
    void FillPolygon(Brush brush, PointF[] points);

    /// <summary>
    /// Draws a path
    /// </summary>
    void DrawPath(Pen pen, GraphicsPath path);

    /// <summary>
    /// Fills a path
    /// </summary>
    void FillPath(Brush brush, GraphicsPath path);

    /// <summary>
    /// Translates the coordinate system
    /// </summary>
    void TranslateTransform(float dx, float dy);

    /// <summary>
    /// Rotates the coordinate system
    /// </summary>
    void RotateTransform(float angle);

    /// <summary>
    /// Scales the coordinate system
    /// </summary>
    void ScaleTransform(float sx, float sy);

    /// <summary>
    /// Resets the transformation matrix
    /// </summary>
    void ResetTransform();

    /// <summary>
    /// Measures the size of the specified text when drawn with the specified font
    /// </summary>
    SizeF MeasureString(string text, Font font);
}


namespace Cspdf;

/// <summary>
/// Represents a single page in a PDF document
/// </summary>
public interface IPage
{
    /// <summary>
    /// Gets the page size
    /// </summary>
    PageSize Size { get; }

    /// <summary>
    /// Gets the page orientation
    /// </summary>
    PageOrientation Orientation { get; }

    /// <summary>
    /// Gets the page width in points
    /// </summary>
    double Width { get; }

    /// <summary>
    /// Gets the page height in points
    /// </summary>
    double Height { get; }

    /// <summary>
    /// Gets the graphics context for drawing on this page
    /// </summary>
    IGraphics Graphics { get; }

    /// <summary>
    /// Gets or sets the rotation angle (0, 90, 180, 270)
    /// </summary>
    int Rotation { get; set; }
}



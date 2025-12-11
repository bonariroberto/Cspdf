namespace Cspdf;

/// <summary>
/// Represents a single page in a PDF document
/// </summary>
public class PdfPage : IPage, IDisposable
{
    private readonly IGraphics _graphics;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the PdfPage class
    /// </summary>
    public PdfPage(PageSize size, PageOrientation orientation)
    {
        Size = size;
        Orientation = orientation;
        var (width, height) = PageSizeHelper.GetDimensions(size, orientation);
        Width = width;
        Height = height;
        _graphics = new PdfGraphics(this);
        Rotation = 0;
    }

    /// <summary>
    /// Gets the page size
    /// </summary>
    public PageSize Size { get; }

    /// <summary>
    /// Gets the page orientation
    /// </summary>
    public PageOrientation Orientation { get; }

    /// <summary>
    /// Gets the page width in points
    /// </summary>
    public double Width { get; }

    /// <summary>
    /// Gets the page height in points
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Gets the graphics context for drawing on this page
    /// </summary>
    public IGraphics Graphics => _graphics;

    /// <summary>
    /// Gets or sets the rotation angle (0, 90, 180, 270)
    /// </summary>
    public int Rotation { get; set; }

    /// <summary>
    /// Gets the annotations on this page
    /// </summary>
    public List<Annotation> Annotations { get; } = new();

    /// <summary>
    /// Disposes the page and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            if (_graphics is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}


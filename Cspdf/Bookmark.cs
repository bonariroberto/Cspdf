namespace Cspdf;

/// <summary>
/// Represents a bookmark (outline entry) in a PDF document
/// </summary>
public class Bookmark
{
    private readonly List<Bookmark> _children = new();

    /// <summary>
    /// Gets or sets the bookmark title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page index this bookmark points to
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Gets or sets the vertical position on the page (0 = top, 1 = bottom)
    /// </summary>
    public float VerticalPosition { get; set; } = 0f;

    /// <summary>
    /// Gets or sets the color of the bookmark text
    /// </summary>
    public System.Drawing.Color? Color { get; set; }

    /// <summary>
    /// Gets or sets whether the bookmark is bold
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Gets or sets whether the bookmark is italic
    /// </summary>
    public bool Italic { get; set; }

    /// <summary>
    /// Gets the child bookmarks
    /// </summary>
    public IList<Bookmark> Children => _children;

    /// <summary>
    /// Adds a child bookmark
    /// </summary>
    public Bookmark AddChild(string title, int pageIndex, float verticalPosition = 0f)
    {
        var bookmark = new Bookmark
        {
            Title = title,
            PageIndex = pageIndex,
            VerticalPosition = verticalPosition
        };
        _children.Add(bookmark);
        return bookmark;
    }

    /// <summary>
    /// Removes a child bookmark
    /// </summary>
    public void RemoveChild(Bookmark bookmark)
    {
        _children.Remove(bookmark);
    }
}


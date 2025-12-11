using System.Collections.ObjectModel;

namespace Cspdf;

/// <summary>
/// Main class for creating and manipulating PDF documents
/// </summary>
public class PdfDocument : IDocument
{
    private readonly List<IPage> _pages = new();
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the PdfDocument class
    /// </summary>
    public PdfDocument()
    {
        Metadata = new DocumentMetadata
        {
            Creator = "Cspdf",
            Producer = "Cspdf Library",
            CreationDate = DateTime.Now
        };
        Security = new DocumentSecurity();
    }

    /// <summary>
    /// Gets the collection of pages in this document
    /// </summary>
    public IList<IPage> Pages => new ReadOnlyCollection<IPage>(_pages);

    /// <summary>
    /// Gets or sets the document metadata
    /// </summary>
    public DocumentMetadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the document security settings
    /// </summary>
    public DocumentSecurity Security { get; set; }

    /// <summary>
    /// Gets the bookmarks (outline) of the document
    /// </summary>
    public List<Bookmark> Bookmarks { get; } = new();

    /// <summary>
    /// Gets the form fields in the document
    /// </summary>
    public List<PdfFormField> FormFields { get; } = new();

    /// <summary>
    /// Adds a new page to the document
    /// </summary>
    public IPage AddPage(PageSize size = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait)
    {
        var page = new PdfPage(size, orientation);
        _pages.Add(page);
        return page;
    }

    /// <summary>
    /// Removes a page from the document
    /// </summary>
    public void RemovePage(int index)
    {
        if (index < 0 || index >= _pages.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        _pages.RemoveAt(index);
    }

    /// <summary>
    /// Saves the document to a file
    /// </summary>
    public void Save(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        using var stream = new FileStream(filePath, FileMode.Create);
        Save(stream);
    }

    /// <summary>
    /// Saves the document to a stream
    /// </summary>
    public void Save(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        var writer = new Internal.PdfWriter(this);
        writer.Write(stream);
    }

    /// <summary>
    /// Gets the document as a byte array
    /// </summary>
    public byte[] ToByteArray()
    {
        using var stream = new MemoryStream();
        Save(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// Opens an existing PDF document from a file
    /// </summary>
    public static PdfDocument Open(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("PDF file not found", filePath);

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return Open(stream);
    }

    /// <summary>
    /// Opens an existing PDF document from a stream
    /// </summary>
    public static PdfDocument Open(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        var reader = new Internal.PdfReader(stream);
        return reader.Read();
    }

    /// <summary>
    /// Opens an existing PDF document from a byte array
    /// </summary>
    public static PdfDocument Open(byte[] data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        using var stream = new MemoryStream(data);
        return Open(stream);
    }

    /// <summary>
    /// Merges multiple PDF documents into one
    /// </summary>
    public static PdfDocument Merge(params PdfDocument[] documents)
    {
        if (documents == null || documents.Length == 0)
            throw new ArgumentException("At least one document is required", nameof(documents));

        var merged = new PdfDocument();
        
        foreach (var doc in documents)
        {
            foreach (var page in doc.Pages)
            {
                merged._pages.Add(page);
            }
        }

        return merged;
    }

    /// <summary>
    /// Splits the document into multiple documents, one per page
    /// </summary>
    public PdfDocument[] Split()
    {
        var documents = new PdfDocument[Pages.Count];
        
        for (int i = 0; i < Pages.Count; i++)
        {
            var doc = new PdfDocument();
            doc._pages.Add(Pages[i]);
            documents[i] = doc;
        }

        return documents;
    }

    /// <summary>
    /// Applies a watermark to all pages
    /// </summary>
    public void ApplyWatermark(Watermark watermark)
    {
        if (watermark == null)
            throw new ArgumentNullException(nameof(watermark));

        foreach (var page in Pages)
        {
            watermark.Apply(page);
        }
    }

    /// <summary>
    /// Applies a watermark to a specific page
    /// </summary>
    public void ApplyWatermark(Watermark watermark, int pageIndex)
    {
        if (watermark == null)
            throw new ArgumentNullException(nameof(watermark));
        if (pageIndex < 0 || pageIndex >= Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        watermark.Apply(Pages[pageIndex]);
    }

    /// <summary>
    /// Adds a bookmark to the document
    /// </summary>
    public Bookmark AddBookmark(string title, int pageIndex, float verticalPosition = 0f)
    {
        if (pageIndex < 0 || pageIndex >= Pages.Count)
            throw new ArgumentOutOfRangeException(nameof(pageIndex));

        var bookmark = new Bookmark
        {
            Title = title,
            PageIndex = pageIndex,
            VerticalPosition = verticalPosition
        };
        Bookmarks.Add(bookmark);
        return bookmark;
    }

    /// <summary>
    /// Adds a form field to the document
    /// </summary>
    public void AddFormField(PdfFormField field)
    {
        if (field == null)
            throw new ArgumentNullException(nameof(field));

        FormFields.Add(field);
    }

    /// <summary>
    /// Adds page numbers to all pages
    /// </summary>
    public void AddPageNumbers(PageNumberOptions? options = null)
    {
        PageNumbering.AddPageNumbers(this, options);
    }

    /// <summary>
    /// Creates a stamper for this document
    /// </summary>
    public PdfStamper CreateStamper()
    {
        return new PdfStamper(this);
    }

    /// <summary>
    /// Creates a redactor for this document
    /// </summary>
    public PdfRedactor CreateRedactor()
    {
        return new PdfRedactor(this);
    }

    /// <summary>
    /// Extracts text from this document
    /// </summary>
    public string ExtractText()
    {
        return TextExtractor.ExtractText(this);
    }

    /// <summary>
    /// Disposes the document and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            foreach (var page in _pages.OfType<IDisposable>())
            {
                page.Dispose();
            }
            _pages.Clear();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}


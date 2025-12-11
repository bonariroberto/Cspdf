namespace Cspdf;

/// <summary>
/// Represents a PDF document with pages and metadata
/// </summary>
public interface IDocument : IDisposable
{
    /// <summary>
    /// Gets the collection of pages in this document
    /// </summary>
    IList<IPage> Pages { get; }

    /// <summary>
    /// Gets or sets the document metadata
    /// </summary>
    DocumentMetadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the document security settings
    /// </summary>
    DocumentSecurity Security { get; set; }

    /// <summary>
    /// Adds a new page to the document
    /// </summary>
    IPage AddPage(PageSize size = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait);

    /// <summary>
    /// Removes a page from the document
    /// </summary>
    void RemovePage(int index);

    /// <summary>
    /// Saves the document to a file
    /// </summary>
    void Save(string filePath);

    /// <summary>
    /// Saves the document to a stream
    /// </summary>
    void Save(Stream stream);

    /// <summary>
    /// Gets the document as a byte array
    /// </summary>
    byte[] ToByteArray();
}


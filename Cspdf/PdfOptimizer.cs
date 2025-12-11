namespace Cspdf;

/// <summary>
/// Provides PDF optimization functionality
/// </summary>
public class PdfOptimizer
{
    /// <summary>
    /// Optimization options
    /// </summary>
    public class OptimizationOptions
    {
        /// <summary>
        /// Gets or sets whether to compress images
        /// </summary>
        public bool CompressImages { get; set; } = true;

        /// <summary>
        /// Gets or sets the image compression quality (0-100)
        /// </summary>
        public int ImageQuality { get; set; } = 85;

        /// <summary>
        /// Gets or sets whether to remove unused objects
        /// </summary>
        public bool RemoveUnusedObjects { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to linearize the PDF (fast web view)
        /// </summary>
        public bool Linearize { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to remove metadata
        /// </summary>
        public bool RemoveMetadata { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to remove annotations
        /// </summary>
        public bool RemoveAnnotations { get; set; } = false;

        /// <summary>
        /// Gets or sets whether to remove bookmarks
        /// </summary>
        public bool RemoveBookmarks { get; set; } = false;
    }

    /// <summary>
    /// Optimizes a PDF document
    /// </summary>
    public static PdfDocument Optimize(PdfDocument document, OptimizationOptions? options = null)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));

        options ??= new OptimizationOptions();

        var optimized = new PdfDocument();

        // Copy metadata if not removing
        if (!options.RemoveMetadata)
        {
            optimized.Metadata = document.Metadata;
        }

        // Copy pages
        foreach (var page in document.Pages)
        {
            var newPage = optimized.AddPage(page.Size, page.Orientation);
            
            // Copy page content
            // In full implementation, would optimize images, remove unused resources, etc.
        }

        // Copy bookmarks if not removing
        if (!options.RemoveBookmarks)
        {
            foreach (var bookmark in document.Bookmarks)
            {
                optimized.Bookmarks.Add(bookmark);
            }
        }

        // Copy form fields
        foreach (var field in document.FormFields)
        {
            optimized.AddFormField(field);
        }

        return optimized;
    }

    /// <summary>
    /// Gets optimization statistics
    /// </summary>
    public static OptimizationStatistics GetStatistics(PdfDocument document)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));

        return new OptimizationStatistics
        {
            PageCount = document.Pages.Count,
            HasBookmarks = document.Bookmarks.Count > 0,
            HasForms = document.FormFields.Count > 0,
            HasAnnotations = document.Pages.Any(p => p.Annotations.Count > 0)
        };
    }
}

/// <summary>
/// PDF optimization statistics
/// </summary>
public class OptimizationStatistics
{
    /// <summary>
    /// Gets or sets the page count
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Gets or sets whether the document has bookmarks
    /// </summary>
    public bool HasBookmarks { get; set; }

    /// <summary>
    /// Gets or sets whether the document has forms
    /// </summary>
    public bool HasForms { get; set; }

    /// <summary>
    /// Gets or sets whether the document has annotations
    /// </summary>
    public bool HasAnnotations { get; set; }
}



namespace Cspdf;

/// <summary>
/// Represents security settings for a PDF document
/// </summary>
public class DocumentSecurity
{
    /// <summary>
    /// Gets or sets the user password (for opening the document)
    /// </summary>
    public string? UserPassword { get; set; }

    /// <summary>
    /// Gets or sets the owner password (for permissions)
    /// </summary>
    public string? OwnerPassword { get; set; }

    /// <summary>
    /// Gets or sets whether printing is allowed
    /// </summary>
    public bool AllowPrinting { get; set; } = true;

    /// <summary>
    /// Gets or sets whether modifying the document is allowed
    /// </summary>
    public bool AllowModifyContents { get; set; } = true;

    /// <summary>
    /// Gets or sets whether copying text and graphics is allowed
    /// </summary>
    public bool AllowCopy { get; set; } = true;

    /// <summary>
    /// Gets or sets whether adding or modifying annotations is allowed
    /// </summary>
    public bool AllowModifyAnnotations { get; set; } = true;

    /// <summary>
    /// Gets or sets whether filling form fields is allowed
    /// </summary>
    public bool AllowFillForms { get; set; } = true;

    /// <summary>
    /// Gets or sets whether extracting text and graphics is allowed
    /// </summary>
    public bool AllowExtractContent { get; set; } = true;

    /// <summary>
    /// Gets or sets whether assembling the document is allowed
    /// </summary>
    public bool AllowAssemble { get; set; } = true;

    /// <summary>
    /// Gets or sets whether printing in high quality is allowed
    /// </summary>
    public bool AllowPrintHighQuality { get; set; } = true;
}



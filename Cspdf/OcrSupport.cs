namespace Cspdf;

/// <summary>
/// Provides OCR (Optical Character Recognition) support for PDF documents
/// </summary>
public interface IOcrEngine
{
    /// <summary>
    /// Performs OCR on an image and returns the extracted text
    /// </summary>
    string RecognizeText(byte[] imageData);

    /// <summary>
    /// Performs OCR on a PDF page and returns the extracted text
    /// </summary>
    string RecognizeText(IPage page);

    /// <summary>
    /// Performs OCR on an entire PDF document
    /// </summary>
    string RecognizeText(PdfDocument document);
}

/// <summary>
/// OCR configuration
/// </summary>
public class OcrConfig
{
    /// <summary>
    /// Gets or sets the language for OCR (e.g., "eng", "tur", "deu")
    /// </summary>
    public string Language { get; set; } = "eng";

    /// <summary>
    /// Gets or sets the DPI (dots per inch) for image processing
    /// </summary>
    public int Dpi { get; set; } = 300;

    /// <summary>
    /// Gets or sets whether to preserve formatting
    /// </summary>
    public bool PreserveFormatting { get; set; } = false;

    /// <summary>
    /// Gets or sets the confidence threshold (0.0 to 1.0)
    /// </summary>
    public double ConfidenceThreshold { get; set; } = 0.7;
}

/// <summary>
/// Base implementation for OCR engines
/// </summary>
public abstract class OcrEngineBase : IOcrEngine
{
    /// <summary>
    /// Gets or sets the OCR configuration
    /// </summary>
    public OcrConfig Config { get; set; } = new();

    public abstract string RecognizeText(byte[] imageData);

    public virtual string RecognizeText(IPage page)
    {
        // Convert page to image and perform OCR
        // This is a placeholder - actual implementation would convert page to image
        return string.Empty;
    }

    public virtual string RecognizeText(PdfDocument document)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var page in document.Pages)
        {
            var text = RecognizeText(page);
            sb.AppendLine(text);
        }
        return sb.ToString();
    }
}



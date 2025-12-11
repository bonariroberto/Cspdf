using System.Text;
using System.Text.RegularExpressions;

namespace Cspdf.Internal;

/// <summary>
/// Internal class for reading PDF documents
/// </summary>
internal class PdfReader
{
    private readonly Stream _stream;
    private byte[] _data = Array.Empty<byte>();

    public PdfReader(Stream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public PdfDocument Read()
    {
        using var ms = new MemoryStream();
        _stream.CopyTo(ms);
        _data = ms.ToArray();

        var document = new PdfDocument();
        
        // Parse PDF header
        var text = Encoding.UTF8.GetString(_data);
        
        // Extract pages (simplified parsing)
        var pageMatches = Regex.Matches(text, @"/Type\s*/Page", RegexOptions.IgnoreCase);
        
        // For each page found, create a page object
        foreach (Match match in pageMatches)
        {
            // Extract page dimensions
            var mediaBoxMatch = Regex.Match(text.Substring(match.Index), @"/MediaBox\s*\[\s*(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s*\]");
            if (mediaBoxMatch.Success)
            {
                var width = double.Parse(mediaBoxMatch.Groups[3].Value);
                var height = double.Parse(mediaBoxMatch.Groups[4].Value);
                
                // Determine page size and orientation
                var (size, orientation) = DeterminePageSize(width, height);
                var page = document.AddPage(size, orientation);
            }
        }

        // Extract metadata
        ExtractMetadata(document, text);

        return document;
    }

    private void ExtractMetadata(PdfDocument document, string text)
    {
        var titleMatch = Regex.Match(text, @"/Title\s*\(([^)]*)\)");
        if (titleMatch.Success)
            document.Metadata.Title = UnescapeString(titleMatch.Groups[1].Value);

        var authorMatch = Regex.Match(text, @"/Author\s*\(([^)]*)\)");
        if (authorMatch.Success)
            document.Metadata.Author = UnescapeString(authorMatch.Groups[1].Value);

        var subjectMatch = Regex.Match(text, @"/Subject\s*\(([^)]*)\)");
        if (subjectMatch.Success)
            document.Metadata.Subject = UnescapeString(subjectMatch.Groups[1].Value);

        var keywordsMatch = Regex.Match(text, @"/Keywords\s*\(([^)]*)\)");
        if (keywordsMatch.Success)
            document.Metadata.Keywords = UnescapeString(keywordsMatch.Groups[1].Value);
    }

    private (PageSize, PageOrientation) DeterminePageSize(double width, double height)
    {
        // Standard A4 dimensions in points
        const double a4Width = 595.28;
        const double a4Height = 841.89;

        var isLandscape = width > height;
        var orientation = isLandscape ? PageOrientation.Landscape : PageOrientation.Portrait;

        // Simple size detection (can be enhanced)
        if (Math.Abs(width - a4Width) < 10 && Math.Abs(height - a4Height) < 10)
            return (PageSize.A4, orientation);
        if (Math.Abs(width - a4Height) < 10 && Math.Abs(height - a4Width) < 10)
            return (PageSize.A4, orientation);

        return (PageSize.A4, orientation); // Default
    }

    private string UnescapeString(string str)
    {
        return str.Replace("\\r", "\r")
                  .Replace("\\n", "\n")
                  .Replace("\\(", "(")
                  .Replace("\\)", ")")
                  .Replace("\\\\", "\\");
    }
}


namespace Cspdf;

/// <summary>
/// Converts HTML content to PDF documents
/// </summary>
public static class HtmlToPdf
{
    /// <summary>
    /// Converts HTML string to PDF document
    /// </summary>
    public static PdfDocument Convert(string html, PageSize pageSize = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait)
    {
        if (string.IsNullOrEmpty(html))
            throw new ArgumentException("HTML content cannot be null or empty", nameof(html));

        var document = new PdfDocument();
        var page = document.AddPage(pageSize, orientation);
        var graphics = page.Graphics;

        // Simple HTML to PDF conversion
        // In production, use a library like PuppeteerSharp, HtmlRenderer, or similar
        // This is a simplified implementation

        // Remove HTML tags and extract text
        var text = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();

        // Draw text on page
        using var font = new System.Drawing.Font("Arial", 12);
        using var brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        var margin = 50f;
        var maxWidth = (float)(page.Width - 2 * margin);
        var y = margin;

        var words = text.Split(' ');
        var currentLine = new System.Text.StringBuilder();

        foreach (var word in words)
        {
            var testLine = currentLine.Length > 0 ? currentLine + " " + word : word;
            var testSize = graphics.MeasureString(testLine, font);

            if (testSize.Width > maxWidth && currentLine.Length > 0)
            {
                graphics.DrawString(currentLine.ToString(), font, brush, margin, y);
                y += testSize.Height + 5;
                currentLine.Clear();

                // Check if we need a new page
                if (y > page.Height - margin)
                {
                    page = document.AddPage(pageSize, orientation);
                    graphics = page.Graphics;
                    y = margin;
                }
            }

            if (currentLine.Length > 0)
                currentLine.Append(" ");
            currentLine.Append(word);
        }

        if (currentLine.Length > 0)
        {
            graphics.DrawString(currentLine.ToString(), font, brush, margin, y);
        }

        return document;
    }

    /// <summary>
    /// Converts HTML file to PDF document
    /// </summary>
    public static PdfDocument ConvertFromFile(string htmlFilePath, PageSize pageSize = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait)
    {
        if (string.IsNullOrWhiteSpace(htmlFilePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(htmlFilePath));
        if (!File.Exists(htmlFilePath))
            throw new FileNotFoundException("HTML file not found", htmlFilePath);

        var html = File.ReadAllText(htmlFilePath);
        return Convert(html, pageSize, orientation);
    }

    /// <summary>
    /// Converts URL content to PDF document
    /// </summary>
    public static async Task<PdfDocument> ConvertFromUrlAsync(string url, PageSize pageSize = PageSize.A4, PageOrientation orientation = PageOrientation.Portrait)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty", nameof(url));

        using var httpClient = new System.Net.Http.HttpClient();
        var html = await httpClient.GetStringAsync(url);
        return Convert(html, pageSize, orientation);
    }
}


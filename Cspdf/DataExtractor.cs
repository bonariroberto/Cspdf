using System.Text.Json;

namespace Cspdf;

/// <summary>
/// Provides data extraction functionality from PDF documents (similar to pdf2Data)
/// </summary>
public static class DataExtractor
{
    /// <summary>
    /// Extracts structured data from a PDF document based on a template
    /// </summary>
    public static Dictionary<string, object> ExtractData(PdfDocument document, ExtractionTemplate template)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));
        if (template == null)
            throw new ArgumentNullException(nameof(template));

        var data = new Dictionary<string, object>();

        foreach (var field in template.Fields)
        {
            var value = ExtractFieldValue(document, field);
            if (value != null)
            {
                data[field.Name] = value;
            }
        }

        return data;
    }

    /// <summary>
    /// Extracts data from a PDF and returns as JSON
    /// </summary>
    public static string ExtractDataAsJson(PdfDocument document, ExtractionTemplate template)
    {
        var data = ExtractData(document, template);
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    private static object? ExtractFieldValue(PdfDocument document, ExtractionField field)
    {
        // In full implementation, would:
        // 1. Locate the field on the page based on coordinates or pattern matching
        // 2. Extract text from that region
        // 3. Apply transformations/formatting
        // 4. Return typed value

        return null;
    }
}

/// <summary>
/// Represents a data extraction template
/// </summary>
public class ExtractionTemplate
{
    /// <summary>
    /// Gets the list of fields to extract
    /// </summary>
    public List<ExtractionField> Fields { get; } = new();

    /// <summary>
    /// Adds a field to extract
    /// </summary>
    public ExtractionField AddField(string name, FieldType type)
    {
        var field = new ExtractionField
        {
            Name = name,
            Type = type
        };
        Fields.Add(field);
        return field;
    }
}

/// <summary>
/// Represents a field to extract
/// </summary>
public class ExtractionField
{
    /// <summary>
    /// Gets or sets the field name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field type
    /// </summary>
    public FieldType Type { get; set; }

    /// <summary>
    /// Gets or sets the page index (null for all pages)
    /// </summary>
    public int? PageIndex { get; set; }

    /// <summary>
    /// Gets or sets the region to search (null for automatic detection)
    /// </summary>
    public System.Drawing.RectangleF? Region { get; set; }

    /// <summary>
    /// Gets or sets a pattern to match (regex)
    /// </summary>
    public string? Pattern { get; set; }

    /// <summary>
    /// Gets or sets a label to search for (e.g., "Invoice Number:")
    /// </summary>
    public string? Label { get; set; }
}

/// <summary>
/// Field data types
/// </summary>
public enum FieldType
{
    Text,
    Number,
    Date,
    Boolean,
    Currency,
    Email,
    Phone
}


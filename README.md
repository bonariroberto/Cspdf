# Cspdf - Comprehensive PDF Library for .NET

Cspdf is a powerful, feature-rich PDF library for .NET that provides comprehensive PDF creation, manipulation, and processing capabilities. It aims to be a complete alternative to commercial PDF libraries like iText 7.

## Features

### Core Functionality
- **PDF Creation**: Create PDF documents from scratch
- **PDF Reading**: Open and parse existing PDF documents
- **PDF Manipulation**: Merge, split, rotate, and modify PDFs
- **Text Rendering**: Draw text with custom fonts, colors, and styles
- **Image Support**: Add images to PDF documents
- **Graphics Drawing**: Draw shapes, lines, polygons, and paths
- **Tables**: Create and render tables with customizable styling
- **Forms**: Create interactive PDF forms (text fields, checkboxes, radio buttons, comboboxes)
- **Watermarks**: Add text or image watermarks to pages
- **Digital Signatures**: Sign PDF documents with certificates
- **Security**: Password protection and permission settings
- **Bookmarks**: Create document outlines and navigation
- **Annotations**: Add text annotations, highlights, links, and free text
- **HTML to PDF**: Convert HTML content to PDF
- **Barcode Generation**: Generate various barcode types (Code128, Code39, QR Code, etc.)
- **Metadata**: Set document metadata (title, author, subject, keywords, etc.)

### Advanced Features
- **Text Extraction**: Extract text content from PDF documents
- **OCR Support**: Interface for OCR (Optical Character Recognition) integration
- **PDF/A Compliance**: Create and validate PDF/A compliant documents
- **Tagged PDF (PDF/UA)**: Create accessible PDFs with structure tags
- **Stamping**: Overlay content on existing PDF documents
- **XFA Forms**: Support for XFA (XML Forms Architecture) forms
- **Redaction**: Remove sensitive information from PDFs
- **PDF Optimization**: Optimize PDF file size and performance
- **Page Numbering**: Add page numbers with customizable formatting
- **Data Extraction**: Extract structured data from PDFs (pdf2Data equivalent)

## Installation

```bash
dotnet add package Cspdf
```

Or via NuGet Package Manager:
```
Install-Package Cspdf
```

## Quick Start

### Creating a Simple PDF

```csharp
using Cspdf;
using System.Drawing;

// Create a new PDF document
using var document = new PdfDocument();

// Add a page
var page = document.AddPage(PageSize.A4, PageOrientation.Portrait);
var graphics = page.Graphics;

// Draw text
using var font = new Font("Arial", 16, FontStyle.Bold);
using var brush = new SolidBrush(Color.Black);
graphics.DrawString("Hello, Cspdf!", font, brush, 50, 50);

// Save the document
document.Save("output.pdf");
```

### Creating a Table

```csharp
using var document = new PdfDocument();
var page = document.AddPage();

// Create a table
var table = new PdfTable();
table.ColumnWidths = new float[] { 100, 200, 150 };

// Add header
var headerRow = table.AddHeaderRow();
headerRow.AddCell("Name");
headerRow.AddCell("Email");
headerRow.AddCell("Phone");

// Add data rows
table.AddRow("John Doe", "john@example.com", "123-456-7890");
table.AddRow("Jane Smith", "jane@example.com", "098-765-4321");

// Draw the table
table.Draw(page.Graphics, 50, 50, 450);

document.Save("table.pdf");
```

### Adding Watermarks

```csharp
using var document = new PdfDocument();
var page = document.AddPage();

// Create watermark
var watermark = new Watermark
{
    Text = "CONFIDENTIAL",
    Font = new Font("Arial", 48, FontStyle.Bold),
    Color = Color.FromArgb(128, 128, 128, 128),
    Rotation = -45f,
    Opacity = 0.3f
};

// Apply to all pages
document.ApplyWatermark(watermark);

document.Save("watermarked.pdf");
```

### HTML to PDF Conversion

```csharp
var html = @"
<html>
<body>
    <h1>Hello from HTML!</h1>
    <p>This is converted from HTML to PDF.</p>
</body>
</html>";

var document = HtmlToPdf.Convert(html);
document.Save("html-output.pdf");
```

### Merging PDFs

```csharp
var doc1 = PdfDocument.Open("file1.pdf");
var doc2 = PdfDocument.Open("file2.pdf");
var doc3 = PdfDocument.Open("file3.pdf");

var merged = PdfDocument.Merge(doc1, doc2, doc3);
merged.Save("merged.pdf");
```

### Creating Forms

```csharp
using var document = new PdfDocument();
var page = document.AddPage();

// Create text field
var textField = new PdfTextField
{
    Name = "name",
    Bounds = new RectangleF(50, 50, 200, 30),
    Value = "Enter your name"
};

// Create checkbox
var checkbox = new PdfCheckBox
{
    Name = "agree",
    Bounds = new RectangleF(50, 100, 20, 20),
    Checked = true
};

// Add to document
document.AddFormField(textField);
document.AddFormField(checkbox);

// Draw form fields
foreach (var field in document.FormFields)
{
    field.Draw(page.Graphics);
}

document.Save("form.pdf");
```

### Adding Barcodes

```csharp
using var document = new PdfDocument();
var page = document.AddPage();

// Generate barcode
var barcode = BarcodeGenerator.GenerateBarcode(
    "1234567890",
    BarcodeGenerator.BarcodeType.Code128,
    width: 200,
    height: 100
);

// Draw barcode
page.Graphics.DrawImage(barcode, 50, 50);

document.Save("barcode.pdf");
```

### Digital Signatures

```csharp
using var document = new PdfDocument();
// ... add content ...

var signature = new DigitalSignature
{
    Certificate = new X509Certificate2("certificate.pfx", "password"),
    Reason = "Document approval",
    Location = "Office",
    ContactInfo = "contact@example.com"
};

using var outputStream = new FileStream("signed.pdf", FileMode.Create);
signature.Sign(document, outputStream);
```

### Text Extraction

```csharp
using var document = PdfDocument.Open("document.pdf");

// Extract all text
var text = document.ExtractText();
Console.WriteLine(text);

// Extract with positions
var chunks = TextExtractor.ExtractTextWithPositions(document);
foreach (var chunk in chunks)
{
    Console.WriteLine($"Page {chunk.PageIndex}: {chunk.Text} at ({chunk.X}, {chunk.Y})");
}
```

### PDF/A Compliance

```csharp
using var document = new PdfDocument();
// ... add content ...

// Convert to PDF/A-2b
var pdfA = PdfACompliance.ConvertToPdfA(document, PdfAConformanceLevel.A2b);
pdfA.Save("pdfa-document.pdf");

// Validate PDF/A compliance
var result = PdfACompliance.Validate(document, PdfAConformanceLevel.A2b);
if (result.IsCompliant)
{
    Console.WriteLine("Document is PDF/A compliant!");
}
else
{
    Console.WriteLine($"Errors: {string.Join(", ", result.Errors)}");
}
```

### Stamping (Overlaying Content)

```csharp
using var document = PdfDocument.Open("existing.pdf");
var stamper = document.CreateStamper();

// Stamp text on first page
stamper.StampText(0, "APPROVED", 50, 50, 
    new Font("Arial", 24, FontStyle.Bold), 
    new SolidBrush(Color.Green));

// Stamp image on all pages
var logo = Image.FromFile("logo.png");
for (int i = 0; i < document.Pages.Count; i++)
{
    stamper.StampImage(i, logo, 500, 50);
}

document.Save("stamped.pdf");
```

### Redaction (Removing Sensitive Information)

```csharp
using var document = PdfDocument.Open("document.pdf");
var redactor = document.CreateRedactor();

// Redact a region on page 0
redactor.AddRedaction(0, new RectangleF(100, 200, 300, 50), Color.Black);

// Apply redactions
var redacted = redactor.Apply();
redacted.Save("redacted.pdf");
```

### PDF Optimization

```csharp
using var document = PdfDocument.Open("large.pdf");

var options = new PdfOptimizer.OptimizationOptions
{
    CompressImages = true,
    ImageQuality = 85,
    RemoveUnusedObjects = true,
    Linearize = true
};

var optimized = PdfOptimizer.Optimize(document, options);
optimized.Save("optimized.pdf");

// Get statistics
var stats = PdfOptimizer.GetStatistics(document);
Console.WriteLine($"Pages: {stats.PageCount}, Has Forms: {stats.HasForms}");
```

### Page Numbering

```csharp
using var document = new PdfDocument();
// ... add pages ...

var options = new PageNumberOptions
{
    Position = PageNumberPosition.BottomCenter,
    Format = "Page {page} of {total}",
    Font = new Font("Arial", 10),
    Color = Color.Gray
};

document.AddPageNumbers(options);
document.Save("numbered.pdf");
```

### Data Extraction

```csharp
using var document = PdfDocument.Open("invoice.pdf");

var template = new ExtractionTemplate();
template.AddField("InvoiceNumber", FieldType.Text)
    .Label = "Invoice #:";
template.AddField("Amount", FieldType.Currency)
    .Pattern = @"\$[\d,]+\.\d{2}";
template.AddField("Date", FieldType.Date)
    .Label = "Date:";

var data = DataExtractor.ExtractData(document, template);
var json = DataExtractor.ExtractDataAsJson(document, template);
Console.WriteLine(json);
```

## Advanced Features

### Bookmarks

```csharp
var document = new PdfDocument();
// ... add pages ...

var bookmark1 = document.AddBookmark("Introduction", 0);
var bookmark2 = document.AddBookmark("Chapter 1", 1);
bookmark1.AddChild("Section 1.1", 2);
```

### Annotations

```csharp
var page = document.AddPage();
var annotation = new TextAnnotation
{
    Bounds = new RectangleF(100, 100, 200, 50),
    Title = "Note",
    Contents = "This is an important note",
    Icon = "Note"
};
page.Annotations.Add(annotation);
```

### Security Settings

```csharp
document.Security = new DocumentSecurity
{
    UserPassword = "user123",
    OwnerPassword = "owner123",
    AllowPrinting = true,
    AllowCopy = false,
    AllowModifyContents = false
};
```

## API Reference

### Main Classes

- `PdfDocument`: Main document class
- `PdfPage`: Represents a page in the document
- `IGraphics`: Interface for drawing operations
- `PdfTable`: Table creation and rendering
- `Watermark`: Watermark functionality
- `DigitalSignature`: Digital signature support
- `HtmlToPdf`: HTML to PDF conversion
- `BarcodeGenerator`: Barcode generation

### Enums

- `PageSize`: A0, A1, A2, A3, A4, A5, A6, Letter, Legal, etc.
- `PageOrientation`: Portrait, Landscape

## Requirements

- .NET 8.0 or later
- System.Drawing.Common

## License

MIT License

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Additional Features
- **Stamping**: Overlay content on existing PDFs
- **Redaction**: Remove sensitive information
- **PDF Optimization**: Compress and optimize PDFs
- **Page Numbering**: Automatic page numbering
- **Data Extraction**: Extract structured data from PDFs

## Roadmap

- [ ] Enhanced PDF parsing with full content stream support
- [ ] Complete XFA form flattening
- [ ] Full PDF/A validation and compliance
- [ ] OCR engine integration (Tesseract, etc.)
- [ ] Advanced typography (pdfCalligraph equivalent)
- [ ] Better HTML/CSS rendering with full CSS support
- [ ] Font embedding and subsetting
- [ ] Advanced encryption algorithms

## Support

For issues, questions, or contributions, please open an issue on GitHub.


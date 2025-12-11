using System.Drawing.Imaging;
using System.Text;

namespace Cspdf.Internal;

/// <summary>
/// Internal class for writing PDF documents
/// </summary>
internal class PdfWriter
{
    private readonly PdfDocument _document;
    private readonly StringBuilder _content = new();
    private int _objectNumber = 1;
    private readonly Dictionary<string, int> _objectMap = new();

    public PdfWriter(PdfDocument document)
    {
        _document = document ?? throw new ArgumentNullException(nameof(document));
    }

    public void Write(Stream stream)
    {
        _content.Clear();
        _objectMap.Clear();
        _objectNumber = 1;

        // PDF Header
        WriteLine("%PDF-1.7");
        WriteLine("%\xE2\xE3\xCF\xD3");

        // Write pages
        var pageRefs = new List<int>();
        foreach (var page in _document.Pages)
        {
            var pageObjNum = WritePage(page);
            pageRefs.Add(pageObjNum);
        }

        // Write catalog
        var catalogObjNum = WriteCatalog(pageRefs);

        // Write document info
        var infoObjNum = WriteDocumentInfo();

        // Write xref table
        var xrefOffset = WriteXRefTable();

        // Write trailer
        WriteTrailer(catalogObjNum, infoObjNum, xrefOffset);

        // Write to stream
        var bytes = Encoding.UTF8.GetBytes(_content.ToString());
        stream.Write(bytes, 0, bytes.Length);
    }

    private int WritePage(IPage page)
    {
        var objNum = GetNextObjectNumber();
        var contentObjNum = GetNextObjectNumber();

        // Write page content
        var content = WritePageContent(page);
        WriteObject(contentObjNum, content);

        // Write page object
        var pageContent = $@"<<
/Type /Page
/Parent {GetPageTreeRef()}
/MediaBox [0 0 {page.Width} {page.Height}]
/Contents {contentObjNum} 0 R
/Resources <<
  /XObject <<
  >>
  /Font <<
  >>
>>
/Rotate {page.Rotation}
>>";
        WriteObject(objNum, pageContent);

        return objNum;
    }

    private string WritePageContent(IPage page)
    {
        var sb = new StringBuilder();
        sb.AppendLine("q");

        if (page is PdfPage pdfPage && pdfPage.Graphics is PdfGraphics graphics)
        {
            var bitmap = graphics.GetBitmap();
            if (bitmap != null)
            {
                // Convert bitmap to PDF image
                using var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                var imageData = ms.ToArray();
                var imageObjNum = WriteImage(imageData, bitmap.Width, bitmap.Height);
                sb.AppendLine($"{bitmap.Width} 0 0 {bitmap.Height} 0 0 cm");
                sb.AppendLine($"/Im{imageObjNum} Do");
            }
        }

        sb.AppendLine("Q");
        return sb.ToString();
    }

    private int WriteImage(byte[] imageData, int width, int height)
    {
        var objNum = GetNextObjectNumber();
        var imageContent = $@"<<
/Type /XObject
/Subtype /Image
/Width {width}
/Height {height}
/ColorSpace /DeviceRGB
/BitsPerComponent 8
/Filter /DCTDecode
/Length {imageData.Length}
>>";
        WriteObject(objNum, imageContent);
        WriteLine($"stream");
        WriteLine(Convert.ToBase64String(imageData));
        WriteLine("endstream");
        return objNum;
    }

    private int WriteCatalog(List<int> pageRefs)
    {
        var objNum = GetNextObjectNumber();
        var pagesObjNum = WritePages(pageRefs);
        var catalogContent = $@"<<
/Type /Catalog
/Pages {pagesObjNum} 0 R
>>";
        WriteObject(objNum, catalogContent);
        return objNum;
    }

    private int WritePages(List<int> pageRefs)
    {
        var objNum = GetNextObjectNumber();
        var kids = string.Join(" ", pageRefs.Select(p => $"{p} 0 R"));
        var pagesContent = $@"<<
/Type /Pages
/Kids [{kids}]
/Count {pageRefs.Count}
>>";
        WriteObject(objNum, pagesContent);
        return objNum;
    }

    private int GetPageTreeRef()
    {
        // This would reference the Pages object
        return 2; // Simplified
    }

    private int WriteDocumentInfo()
    {
        var objNum = GetNextObjectNumber();
        var metadata = _document.Metadata;
        var infoContent = $@"<<
/Title ({EscapeString(metadata.Title ?? "")})
/Author ({EscapeString(metadata.Author ?? "")})
/Subject ({EscapeString(metadata.Subject ?? "")})
/Keywords ({EscapeString(metadata.Keywords ?? "")})
/Creator ({EscapeString(metadata.Creator ?? "Cspdf")})
/Producer ({EscapeString(metadata.Producer ?? "Cspdf Library")})
/CreationDate (D:{DateTime.Now:yyyyMMddHHmmss})
/ModDate (D:{(metadata.ModificationDate ?? DateTime.Now):yyyyMMddHHmmss})
>>";
        WriteObject(objNum, infoContent);
        return objNum;
    }

    private int WriteXRefTable()
    {
        // Simplified xref table
        return _content.Length;
    }

    private void WriteTrailer(int catalogObjNum, int infoObjNum, int xrefOffset)
    {
        WriteLine("trailer");
        WriteLine($@"<<
/Size {_objectNumber}
/Root {catalogObjNum} 0 R
/Info {infoObjNum} 0 R
>>");
        WriteLine("startxref");
        WriteLine(xrefOffset.ToString());
        WriteLine("%%EOF");
    }

    private void WriteObject(int objNum, string content)
    {
        WriteLine($"{objNum} 0 obj");
        WriteLine(content);
        WriteLine("endobj");
    }

    private int GetNextObjectNumber()
    {
        return _objectNumber++;
    }

    private void WriteLine(string line)
    {
        _content.AppendLine(line);
    }

    private string EscapeString(string str)
    {
        return str.Replace("\\", "\\\\")
                  .Replace("(", "\\(")
                  .Replace(")", "\\)")
                  .Replace("\r", "\\r")
                  .Replace("\n", "\\n");
    }
}


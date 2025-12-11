using Cspdf;

namespace Examples;

/// <summary>
/// Example demonstrating PDF merging
/// </summary>
public class MergeExample
{
    public static void MergePdfs()
    {
        // Create first document
        var doc1 = new PdfDocument();
        var page1 = doc1.AddPage();
        page1.Graphics.DrawString("Document 1 - Page 1", 
            new System.Drawing.Font("Arial", 14), 
            new System.Drawing.SolidBrush(System.Drawing.Color.Black), 
            50, 50);
        doc1.Save("doc1.pdf");

        // Create second document
        var doc2 = new PdfDocument();
        var page2 = doc2.AddPage();
        page2.Graphics.DrawString("Document 2 - Page 1", 
            new System.Drawing.Font("Arial", 14), 
            new System.Drawing.SolidBrush(System.Drawing.Color.Black), 
            50, 50);
        doc2.Save("doc2.pdf");

        // Merge documents
        var merged = PdfDocument.Merge(doc1, doc2);
        merged.Save("merged.pdf");

        Console.WriteLine("PDFs merged: merged.pdf");

        // Cleanup
        doc1.Dispose();
        doc2.Dispose();
        merged.Dispose();
    }
}


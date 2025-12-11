using Xunit;
using System.Drawing;

namespace Cspdf.Tests;

public class PdfDocumentTests
{
    [Fact]
    public void CreateDocument_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        using var document = new PdfDocument();

        // Assert
        Assert.NotNull(document);
        Assert.NotNull(document.Metadata);
        Assert.NotNull(document.Security);
        Assert.NotNull(document.Pages);
        Assert.Empty(document.Pages);
    }

    [Fact]
    public void AddPage_ShouldAddPageToDocument()
    {
        // Arrange
        using var document = new PdfDocument();

        // Act
        var page = document.AddPage(PageSize.A4, PageOrientation.Portrait);

        // Assert
        Assert.Single(document.Pages);
        Assert.NotNull(page);
        Assert.Equal(PageSize.A4, page.Size);
        Assert.Equal(PageOrientation.Portrait, page.Orientation);
    }

    [Fact]
    public void AddPage_WithLandscape_ShouldSetCorrectOrientation()
    {
        // Arrange
        using var document = new PdfDocument();

        // Act
        var page = document.AddPage(PageSize.A4, PageOrientation.Landscape);

        // Assert
        Assert.Equal(PageOrientation.Landscape, page.Orientation);
        Assert.True(page.Width > page.Height);
    }

    [Fact]
    public void RemovePage_WithValidIndex_ShouldRemovePage()
    {
        // Arrange
        using var document = new PdfDocument();
        document.AddPage();
        document.AddPage();
        document.AddPage();

        // Act
        document.RemovePage(1);

        // Assert
        Assert.Equal(2, document.Pages.Count);
    }

    [Fact]
    public void RemovePage_WithInvalidIndex_ShouldThrowException()
    {
        // Arrange
        using var document = new PdfDocument();
        document.AddPage();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => document.RemovePage(5));
        Assert.Throws<ArgumentOutOfRangeException>(() => document.RemovePage(-1));
    }

    [Fact]
    public void Save_ShouldCreateFile()
    {
        // Arrange
        using var document = new PdfDocument();
        var page = document.AddPage();
        var graphics = page.Graphics;
        
        using var font = new Font("Arial", 12);
        using var brush = new SolidBrush(Color.Black);
        graphics.DrawString("Test", font, brush, 50, 50);

        var filePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");

        try
        {
            // Act
            document.Save(filePath);

            // Assert
            Assert.True(File.Exists(filePath));
            Assert.True(new FileInfo(filePath).Length > 0);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    [Fact]
    public void ToByteArray_ShouldReturnNonEmptyArray()
    {
        // Arrange
        using var document = new PdfDocument();
        document.AddPage();

        // Act
        var bytes = document.ToByteArray();

        // Assert
        Assert.NotNull(bytes);
        Assert.NotEmpty(bytes);
    }

    [Fact]
    public void Merge_WithMultipleDocuments_ShouldCombinePages()
    {
        // Arrange
        var doc1 = new PdfDocument();
        doc1.AddPage();
        
        var doc2 = new PdfDocument();
        doc2.AddPage();
        doc2.AddPage();

        // Act
        var merged = PdfDocument.Merge(doc1, doc2);

        // Assert
        Assert.Equal(3, merged.Pages.Count);

        // Cleanup
        doc1.Dispose();
        doc2.Dispose();
        merged.Dispose();
    }

    [Fact]
    public void Split_ShouldCreateSeparateDocuments()
    {
        // Arrange
        using var document = new PdfDocument();
        document.AddPage();
        document.AddPage();
        document.AddPage();

        // Act
        var split = document.Split();

        // Assert
        Assert.Equal(3, split.Length);
        Assert.Single(split[0].Pages);
        Assert.Single(split[1].Pages);
        Assert.Single(split[2].Pages);

        // Cleanup
        foreach (var doc in split)
        {
            doc.Dispose();
        }
    }

    [Fact]
    public void AddBookmark_ShouldAddBookmark()
    {
        // Arrange
        using var document = new PdfDocument();
        document.AddPage();

        // Act
        var bookmark = document.AddBookmark("Test Bookmark", 0);

        // Assert
        Assert.Single(document.Bookmarks);
        Assert.Equal("Test Bookmark", bookmark.Title);
        Assert.Equal(0, bookmark.PageIndex);
    }

    [Fact]
    public void AddFormField_ShouldAddFormField()
    {
        // Arrange
        using var document = new PdfDocument();
        var field = new PdfTextField
        {
            Name = "testField",
            Bounds = new RectangleF(10, 10, 100, 20)
        };

        // Act
        document.AddFormField(field);

        // Assert
        Assert.Single(document.FormFields);
        Assert.Equal("testField", document.FormFields[0].Name);
    }
}



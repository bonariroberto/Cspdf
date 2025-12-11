using Xunit;

namespace Cspdf.Tests;

public class PageSizeHelperTests
{
    [Fact]
    public void GetDimensions_A4Portrait_ShouldReturnCorrectDimensions()
    {
        // Act
        var (width, height) = PageSizeHelper.GetDimensions(PageSize.A4, PageOrientation.Portrait);

        // Assert
        Assert.Equal(595.28, width, 2);
        Assert.Equal(841.89, height, 2);
    }

    [Fact]
    public void GetDimensions_A4Landscape_ShouldSwapDimensions()
    {
        // Act
        var (width, height) = PageSizeHelper.GetDimensions(PageSize.A4, PageOrientation.Landscape);

        // Assert
        Assert.Equal(841.89, width, 2);
        Assert.Equal(595.28, height, 2);
    }

    [Fact]
    public void GetDimensions_LetterPortrait_ShouldReturnCorrectDimensions()
    {
        // Act
        var (width, height) = PageSizeHelper.GetDimensions(PageSize.Letter, PageOrientation.Portrait);

        // Assert
        Assert.Equal(612, width);
        Assert.Equal(792, height);
    }

    [Fact]
    public void GetDimensions_Custom_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            PageSizeHelper.GetDimensions(PageSize.Custom, PageOrientation.Portrait));
    }
}


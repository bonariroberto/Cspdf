namespace Cspdf;

/// <summary>
/// Helper class for page size calculations
/// </summary>
public static class PageSizeHelper
{
    private static readonly Dictionary<PageSize, (double Width, double Height)> _sizes = new()
    {
        { PageSize.A0, (2383.94, 3370.39) },
        { PageSize.A1, (1683.78, 2383.94) },
        { PageSize.A2, (1190.55, 1683.78) },
        { PageSize.A3, (841.89, 1190.55) },
        { PageSize.A4, (595.28, 841.89) },
        { PageSize.A5, (419.53, 595.28) },
        { PageSize.A6, (297.64, 419.53) },
        { PageSize.Letter, (612, 792) },
        { PageSize.Legal, (612, 1008) },
        { PageSize.Tabloid, (792, 1224) },
        { PageSize.Ledger, (1224, 792) }
    };

    /// <summary>
    /// Gets the dimensions for a page size in points
    /// </summary>
    public static (double Width, double Height) GetDimensions(PageSize size, PageOrientation orientation)
    {
        if (size == PageSize.Custom)
            throw new ArgumentException("Custom page size requires explicit dimensions");

        var (width, height) = _sizes[size];

        return orientation == PageOrientation.Landscape
            ? (height, width)
            : (width, height);
    }
}



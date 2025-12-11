using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents a row in a PDF table
/// </summary>
public class PdfTableRow
{
    private readonly PdfTable _table;
    private readonly List<PdfTableCell> _cells = new();
    private readonly bool _isHeader;

    internal PdfTableRow(PdfTable table, bool isHeader = false)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        _isHeader = isHeader;
    }

    /// <summary>
    /// Gets the number of cells in this row
    /// </summary>
    public int CellCount => _cells.Count;

    /// <summary>
    /// Gets whether this is a header row
    /// </summary>
    public bool IsHeader => _isHeader;

    /// <summary>
    /// Adds a cell to this row
    /// </summary>
    public PdfTableCell AddCell(string? text = null)
    {
        var cell = new PdfTableCell(text);
        _cells.Add(cell);
        return cell;
    }

    /// <summary>
    /// Gets a cell at the specified index
    /// </summary>
    public PdfTableCell? GetCell(int index)
    {
        if (index < 0 || index >= _cells.Count)
            return null;
        return _cells[index];
    }

    /// <summary>
    /// Removes a cell at the specified index
    /// </summary>
    public void RemoveCell(int index)
    {
        if (index < 0 || index >= _cells.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _cells.RemoveAt(index);
    }
}



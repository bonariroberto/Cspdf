using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents a table that can be drawn on a PDF page
/// </summary>
public class PdfTable
{
    private readonly List<PdfTableRow> _rows = new();
    private float[]? _columnWidths;
    private Color _borderColor = Color.Black;
    private float _borderWidth = 1f;
    private Color _headerBackgroundColor = Color.LightGray;
    private Color _alternateRowColor = Color.White;
    private bool _hasHeader = false;

    /// <summary>
    /// Gets or sets the column widths (null for auto-sizing)
    /// </summary>
    public float[]? ColumnWidths
    {
        get => _columnWidths;
        set
        {
            if (value != null && value.Any(w => w <= 0))
                throw new ArgumentException("Column widths must be positive", nameof(value));
            _columnWidths = value;
        }
    }

    /// <summary>
    /// Gets or sets the border color
    /// </summary>
    public Color BorderColor
    {
        get => _borderColor;
        set => _borderColor = value;
    }

    /// <summary>
    /// Gets or sets the border width
    /// </summary>
    public float BorderWidth
    {
        get => _borderWidth;
        set
        {
            if (value < 0)
                throw new ArgumentException("Border width must be non-negative", nameof(value));
            _borderWidth = value;
        }
    }

    /// <summary>
    /// Gets or sets the header background color
    /// </summary>
    public Color HeaderBackgroundColor
    {
        get => _headerBackgroundColor;
        set => _headerBackgroundColor = value;
    }

    /// <summary>
    /// Gets or sets the alternate row background color
    /// </summary>
    public Color AlternateRowColor
    {
        get => _alternateRowColor;
        set => _alternateRowColor = value;
    }

    /// <summary>
    /// Gets the number of columns
    /// </summary>
    public int ColumnCount => _rows.Count > 0 ? _rows[0].CellCount : 0;

    /// <summary>
    /// Gets the number of rows
    /// </summary>
    public int RowCount => _rows.Count;

    /// <summary>
    /// Adds a header row
    /// </summary>
    public PdfTableRow AddHeaderRow()
    {
        var row = new PdfTableRow(this, isHeader: true);
        _rows.Insert(0, row);
        _hasHeader = true;
        return row;
    }

    /// <summary>
    /// Adds a data row
    /// </summary>
    public PdfTableRow AddRow()
    {
        var row = new PdfTableRow(this, isHeader: false);
        _rows.Add(row);
        return row;
    }

    /// <summary>
    /// Adds a data row with cells
    /// </summary>
    public PdfTableRow AddRow(params string[] cells)
    {
        var row = AddRow();
        foreach (var cell in cells)
        {
            row.AddCell(cell);
        }
        return row;
    }

    /// <summary>
    /// Removes a row at the specified index
    /// </summary>
    public void RemoveRow(int index)
    {
        if (index < 0 || index >= _rows.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _rows.RemoveAt(index);
    }

    /// <summary>
    /// Gets a row at the specified index
    /// </summary>
    public PdfTableRow GetRow(int index)
    {
        if (index < 0 || index >= _rows.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        return _rows[index];
    }

    /// <summary>
    /// Draws the table on the specified graphics at the given position
    /// </summary>
    public void Draw(IGraphics graphics, float x, float y, float width)
    {
        if (graphics == null)
            throw new ArgumentNullException(nameof(graphics));
        if (width <= 0)
            throw new ArgumentException("Width must be positive", nameof(width));

        if (_rows.Count == 0)
            return;

        var columnCount = ColumnCount;
        if (columnCount == 0)
            return;

        // Calculate column widths
        var columnWidths = _columnWidths ?? Enumerable.Repeat(width / columnCount, columnCount).ToArray();
        if (columnWidths.Sum() != width)
        {
            var scale = width / columnWidths.Sum();
            columnWidths = columnWidths.Select(w => w * scale).ToArray();
        }

        var currentY = y;
        var rowHeight = CalculateRowHeight(graphics, columnWidths);

        for (int i = 0; i < _rows.Count; i++)
        {
            var row = _rows[i];
            var isHeader = _hasHeader && i == 0;
            var rowBgColor = isHeader ? _headerBackgroundColor : (i % 2 == 0 ? Color.White : _alternateRowColor);

            // Draw row background
            graphics.FillRectangle(new SolidBrush(rowBgColor), x, currentY, width, rowHeight);

            // Draw cells
            var currentX = x;
            for (int j = 0; j < columnCount && j < row.CellCount; j++)
            {
                var cell = row.GetCell(j);
                var cellWidth = columnWidths[j];

                // Draw cell content
                if (cell != null)
                {
                    var font = cell.Font ?? new Font("Arial", 10);
                    var brush = cell.Brush ?? new SolidBrush(Color.Black);
                    var padding = 4f;
                    var cellRect = new RectangleF(currentX + padding, currentY + padding, cellWidth - 2 * padding, rowHeight - 2 * padding);
                    graphics.DrawString(cell.Text ?? "", font, brush, cellRect);
                }

                // Draw cell border
                if (_borderWidth > 0)
                {
                    var pen = new Pen(_borderColor, _borderWidth);
                    graphics.DrawRectangle(pen, currentX, currentY, cellWidth, rowHeight);
                }

                currentX += cellWidth;
            }

            currentY += rowHeight;
        }
    }

    private float CalculateRowHeight(IGraphics graphics, float[] columnWidths)
    {
        // Simple calculation - can be enhanced
        return 30f; // Default row height
    }

    internal bool HasHeader => _hasHeader;
}


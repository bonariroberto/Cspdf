using System.Drawing;

namespace Cspdf;

/// <summary>
/// Represents a form field in a PDF document
/// </summary>
public abstract class PdfFormField
{
    /// <summary>
    /// Gets or sets the field name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field value
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets whether the field is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets whether the field is read-only
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets the field position and size
    /// </summary>
    public RectangleF Bounds { get; set; }

    /// <summary>
    /// Gets the field type
    /// </summary>
    public abstract string FieldType { get; }

    /// <summary>
    /// Draws the field on the page
    /// </summary>
    public abstract void Draw(IGraphics graphics);
}

/// <summary>
/// Represents a text field in a PDF form
/// </summary>
public class PdfTextField : PdfFormField
{
    /// <summary>
    /// Gets or sets the font
    /// </summary>
    public Font Font { get; set; } = new Font("Arial", 12);

    /// <summary>
    /// Gets or sets the maximum length
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Gets or sets whether the field is multiline
    /// </summary>
    public bool Multiline { get; set; }

    /// <summary>
    /// Gets or sets whether the field is password (masked)
    /// </summary>
    public bool IsPassword { get; set; }

    public override string FieldType => "Text";

    public override void Draw(IGraphics graphics)
    {
        // Draw field border
        using var pen = new Pen(Color.Black, 1);
        graphics.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

        // Draw field value
        if (!string.IsNullOrEmpty(Value))
        {
            var displayValue = IsPassword ? new string('*', Value.Length) : Value;
            using var brush = new SolidBrush(Color.Black);
            var textRect = new RectangleF(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4);
            graphics.DrawString(displayValue, Font, brush, textRect);
        }
    }
}

/// <summary>
/// Represents a checkbox in a PDF form
/// </summary>
public class PdfCheckBox : PdfFormField
{
    /// <summary>
    /// Gets or sets whether the checkbox is checked
    /// </summary>
    public bool Checked { get; set; }

    public override string FieldType => "CheckBox";

    public override void Draw(IGraphics graphics)
    {
        // Draw checkbox border
        using var pen = new Pen(Color.Black, 1);
        graphics.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

        // Draw checkmark if checked
        if (Checked)
        {
            using var brush = new SolidBrush(Color.Black);
            using var font = new Font("Arial", Bounds.Height * 0.8f, FontStyle.Bold);
            graphics.DrawString("✓", font, brush, Bounds.X, Bounds.Y);
        }
    }
}

/// <summary>
/// Represents a radio button in a PDF form
/// </summary>
public class PdfRadioButton : PdfFormField
{
    /// <summary>
    /// Gets or sets whether the radio button is selected
    /// </summary>
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the radio button group name
    /// </summary>
    public string GroupName { get; set; } = string.Empty;

    public override string FieldType => "RadioButton";

    public override void Draw(IGraphics graphics)
    {
        var centerX = Bounds.X + Bounds.Width / 2;
        var centerY = Bounds.Y + Bounds.Height / 2;
        var radius = Math.Min(Bounds.Width, Bounds.Height) / 2;

        // Draw circle border
        using var pen = new Pen(Color.Black, 1);
        graphics.DrawEllipse(pen, centerX - radius, centerY - radius, radius * 2, radius * 2);

        // Fill if selected
        if (Selected)
        {
            using var brush = new SolidBrush(Color.Black);
            graphics.FillEllipse(brush, centerX - radius * 0.5f, centerY - radius * 0.5f, radius, radius);
        }
    }
}

/// <summary>
/// Represents a combo box (dropdown) in a PDF form
/// </summary>
public class PdfComboBox : PdfFormField
{
    /// <summary>
    /// Gets or sets the available options
    /// </summary>
    public List<string> Options { get; set; } = new();

    /// <summary>
    /// Gets or sets the selected index
    /// </summary>
    public int SelectedIndex { get; set; } = -1;

    public override string FieldType => "ComboBox";

    public override void Draw(IGraphics graphics)
    {
        // Draw field border
        using var pen = new Pen(Color.Black, 1);
        graphics.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

        // Draw selected value
        if (SelectedIndex >= 0 && SelectedIndex < Options.Count)
        {
            using var font = new Font("Arial", 12);
            using var brush = new SolidBrush(Color.Black);
            var textRect = new RectangleF(Bounds.X + 2, Bounds.Y + 2, Bounds.Width - 4, Bounds.Height - 4);
            graphics.DrawString(Options[SelectedIndex], font, brush, textRect);
        }

        // Draw dropdown arrow
        var arrowSize = 8f;
        var arrowX = Bounds.Right - arrowSize - 2;
        var arrowY = Bounds.Y + (Bounds.Height - arrowSize) / 2;
        var points = new PointF[]
        {
            new(arrowX, arrowY),
            new(arrowX + arrowSize, arrowY),
            new(arrowX + arrowSize / 2, arrowY + arrowSize)
        };
        using var arrowBrush = new SolidBrush(Color.Black);
        graphics.FillPolygon(arrowBrush, points);
    }
}


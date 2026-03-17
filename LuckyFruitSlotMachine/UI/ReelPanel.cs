using System.Drawing;
using System.Drawing.Drawing2D;
using FruitSlot.Models;

namespace FruitSlot.UI;

public class ReelPanel : Panel
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Symbol DisplaySymbol { get; set; } = Symbol.Cherry;

    public ReelPanel()
    {
        DoubleBuffered = true;
        BackColor      = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var bgBrush = new LinearGradientBrush(
            ClientRectangle,
            Color.FromArgb(255, 255, 240),
            Color.FromArgb(230, 225, 200),
            LinearGradientMode.Vertical);
        g.FillRectangle(bgBrush, ClientRectangle);

        float fontSize = Height * 0.48f;
        Font  font;
        Color color;

        if (DisplaySymbol.IsTextSymbol())
        {
            font  = new Font("Arial Black", fontSize * 0.52f, FontStyle.Bold, GraphicsUnit.Pixel);
            color = DisplaySymbol == Symbol.Bar ? Color.FromArgb(180, 130, 0) : Color.Crimson;
        }
        else
        {
            font  = new Font("Segoe UI Emoji", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            color = Color.Black;
        }

        var sf = new StringFormat
        {
            Alignment     = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        using (font)
        using (var brush = new SolidBrush(color))
            g.DrawString(DisplaySymbol.ToEmoji(), font, brush, ClientRectangle, sf);

        using var borderPen = new Pen(Color.FromArgb(100, 80, 30), 2f);
        g.DrawRectangle(borderPen, 1, 1, Width - 3, Height - 3);
    }
}

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AmirCalendar
{
    [ToolboxItem(false), DesignTimeVisible(false)]
    internal class MyGrid : DataGridView
    {
        private readonly StringFormat _sf;

        public MyGrid()
        {
            _sf = new StringFormat
                      {
                          Alignment = StringAlignment.Center,
                          FormatFlags = StringFormatFlags.DirectionRightToLeft,
                          LineAlignment = StringAlignment.Center
                      };
            ColumnHeadersHeight = 23;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ColorOne = Color.Lavender;
            ColorTwo = Color.FromArgb(169, 197, 232);
        }

        public Color ColorOne { get; set; }
        public Color ColorTwo { get; set; }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.RowIndex >= 0 || e.ColumnIndex == -1) return;
            var g = e.Graphics;
            using (var lgb = new LinearGradientBrush(e.CellBounds, ColorOne, ColorTwo, LinearGradientMode.Vertical))
            {
                g.FillRectangle(lgb, e.CellBounds);
                g.DrawRectangle(new Pen(Color.SlateGray), new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height - 1));
                g.DrawString(e.Value.ToString(), e.CellStyle.Font, new SolidBrush(Color.Black), e.CellBounds, _sf);
            }
            e.Handled = true;
        }
    }
}

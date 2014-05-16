using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPA
{
    public  class KeyButton : Control
    {
        GraphicsPath border;
        int penWidth = 1;
        int c = 5;
        
        public KeyButton()
        {
            base.Font = Properties.Settings.Default.buttonFont;
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return new System.Drawing.Size(Properties.Settings.Default.buttonWidth, Properties.Settings.Default.buttonHeight);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Rectangle borderRect = new Rectangle(penWidth / 2, penWidth / 2, Width - penWidth, Height - penWidth);
            
            border = new GraphicsPath();
            Point p1 = new Point(borderRect.X, borderRect.Y + c);
            Point p2 = new Point(borderRect.X + c, borderRect.Y);
            Point p3 = new Point(borderRect.Right - c, borderRect.Y);
            Point p4 = new Point(borderRect.Right, borderRect.Y  + c);
            Point p5 = new Point(borderRect.Right, borderRect.Bottom - c);
            Point p6 = new Point(borderRect.Right - c, borderRect.Bottom);
            Point p7 = new Point(borderRect.X + c, borderRect.Bottom);
            Point p8 = new Point(borderRect.X, borderRect.Bottom - c);
            border.AddArc(p1.X, p2.Y, 2 * c, 2 * c, 180, 90);
            border.AddLine(p2, p3);
            border.AddArc(p3.X - c, p4.Y - c, 2 * c, 2 * c, -90, 90);
            border.AddLine(p4, p5);
            border.AddArc(p5.X - 2*c, p6.Y - 2*c, 2 * c, 2 * c, 0, 90);
            border.AddLine(p6, p7);
            border.AddArc(p7.X -c, p8.Y - c, 2 * c, 2 * c, 90, 90);
            border.AddLine(p8, p1); 
        }

        bool mouseEntered = false;
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseEntered = false;
            Invalidate();
        }
       
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Properties.Settings.Default.buttonBorderColor))
            {
                p.Width = penWidth;
                e.Graphics.DrawPath(p, border);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            if(mouseEntered)
                e.Graphics.DrawString(Text,Font, Brushes.Gray,ClientRectangle ,sf);
            else
                e.Graphics.DrawString(Text, Font, Brushes.Black, ClientRectangle, sf);
            
            
        }

    }
}

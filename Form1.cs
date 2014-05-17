using CSSoftKeyboard.NoActivate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPA
{
    public partial class Form1 : NoActivateWindow
    {
        private GraphicsPath pathOutline;
        private Rectangle borderRect;
        
        private int tabWidth = 200;
        private int tabHeight = 30;
        private int borderWidth = 1;

        private HotKey hotKey_F1;
        public Form1()
        {
            InitializeComponent();
        }

        IEnumerable<KeyButton> keyButtonList = null;
        IEnumerable<KeyButton> KeyButtonList
        {
            get
            {
                if (keyButtonList == null)
                {
                    keyButtonList = this.Controls.OfType<KeyButton>();
                }
                return keyButtonList;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            label1.MouseDown += Form1_MouseDown;
            label1.MouseMove += Form1_MouseMove;
            label1.MouseUp += Form1_MouseUp;

            foreach (KeyButton btn in this.KeyButtonList)
            {
                btn.Click += new EventHandler(KeyButton_Click);
            }
            keyButton_Clear.Click -= new EventHandler(KeyButton_Click);
            keyButton_Clear.Click += keyButton_Clear_Click;
            keyButton_Copy.Click -= new EventHandler(KeyButton_Click);
            keyButton_Copy.Click += keyButton_Copy_Click;
            keyButton_Delete.Click -= new EventHandler(KeyButton_Click);
            keyButton_Delete.Click += keyButton_Delete_Click;
            keyButton_Enter.Click -= new EventHandler(KeyButton_Click);
            keyButton_Enter.Click += keyButton_Enter_Click;


            int arcW = 10;
             pathOutline = new GraphicsPath();
             Point p1 = new Point(0, 0);
             Point p2 = new Point(tabWidth - arcW , 0);
             Point p3 = new Point(tabWidth, arcW);
             Point p4 = new Point(tabWidth, tabHeight);
             Point p5 = new Point(Width, tabHeight);
             Point p6 = new Point(Width, Height);
             Point p7 = new Point(0, Height);
             pathOutline.AddLine(p1, p2);

             pathOutline.AddArc(p2.X - arcW, p2.Y, arcW * 2, arcW * 2, -90, 90);
             pathOutline.AddLine(p3, p4);
             pathOutline.AddLine(p4, p5);
             pathOutline.AddLine(p5, p6);
             pathOutline.AddLine(p6, p7);
             pathOutline.AddLine(p7, p1);
             Region region = new Region(pathOutline);  
             this.Region = region;

             borderRect = new Rectangle(p1.X + borderWidth /2, p1.Y + tabHeight + borderWidth /2, Width - borderWidth, Height - tabHeight - borderWidth);
        }

        private void keyButton_Enter_Click(object sender, EventArgs e)
        {
            if(label1.Text.Length > 0)
            { 
                SendKeys.Send(label1.Text);
                label1.Text = "";
            }
        }

        private void keyButton_Delete_Click(object sender, EventArgs e)
        {
            string s = label1.Text;
            if (s.Length > 0)
            {
                label1.Text = s.Substring(0, s.Length - 1);
            }
        }

        private void keyButton_Copy_Click(object sender, EventArgs e)
        {
            if (label1.Text.Length > 0)
            {
                Clipboard.SetText(label1.Text);
            }
        }

        private void keyButton_Clear_Click(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void KeyButton_Click(object sender, EventArgs e)
        {
            if(sender is KeyButton)
            {
                KeyButton keyButton = sender as KeyButton;
                label1.Text = label1.Text + keyButton.Text;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point cp = e.Location;
                Point loc = this.Location;
                loc.Offset(cp.X - dragStartPoint.X, cp.Y - dragStartPoint.Y);
                this.Location = loc;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            dragStartPoint = Point.Empty;
        }

        Point dragStartPoint;
        bool drag = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragStartPoint = e.Location;
            drag = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.FromArgb(3, 189, 254)))
            {
                p.Width = borderWidth;
                e.Graphics.DrawRectangle(p, borderRect);
            }

          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.winLocation;

            hotKey_F1 = new HotKey();
            hotKey_F1.Key = Keys.F1;
            hotKey_F1.HotKeyPressed += hotKey_F1_HotKeyPressed;
        }

        private void hotKey_F1_HotKeyPressed(object sender, KeyEventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void toolStripMenuItem_Help_Click(object sender, EventArgs e)
        {
            Process.Start("http://bulo.hujiang.com/u/11047386/diary/976044/");
        }

        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.winLocation = this.Location;
            }
            else
            {
                Properties.Settings.Default.winLocation = this.RestoreBounds.Location;
            }
            Properties.Settings.Default.Save();

            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            notifyIcon1.Icon = null;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }
    }
}

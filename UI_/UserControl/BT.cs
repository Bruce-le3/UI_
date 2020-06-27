using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
//using Justech.Properties;


namespace Justech
{
    public partial class BT : Component
    {
        public BT()
        {
            InitializeComponent();
        }

        public BT(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
    public partial class TXbutton : Button
    {
      public    bool status = false;

        private void Draw(Rectangle rectangle, Graphics g, int _radius, bool cusp, Color begin_color, Color end_color)
        {
            try
            {
                int span = 2;
                //抗锯齿
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //渐变填充
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
                //画尖角
                if (cusp)
                {
                    span = 10;
                    PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                    PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                    PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                    PointF[] ptsArray = { p1, p2, p3 };
                    g.FillPolygon(myLinearGradientBrush, ptsArray);
                }
                //填充
                g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
            }
            catch 
            {


            }
        }
        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {

            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }
        public string name = null;  //名称
        public string name_move = null;
        public int font_size =12;  //字体的大小
        public Color TXcolor_start;  //渐变色的颜色，start开始，end结束
        public Color TXcolor_end;
        public Color TXcolor_start_move;
        public Color TXcolor_end_move;
        public Image TX_image = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            //path.AddEllipse(0, 0, this.Width, this.Height);
            //     this.Region = new Region(path);
          //  this.FlatStyle = FlatStyle.Flat;
          // this.FlatAppearance.BorderSize = 0;
            this.Text = null;
            font_size =10;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            if (status == false)
            {
                if (TXcolor_start.IsEmpty)
                {

                    Draw(e.ClipRectangle, e.Graphics, 12, false, Color.FromArgb(0xEA, 0xEA, 0xEB), Color.FromArgb(0xEA, 0xEA, 0xEB));
                }
                else
                {
                    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start, TXcolor_start);
                }

                // Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start, TXcolor_end);
                if (name != null)
                {
                    e.Graphics.DrawString(name, new Font("微软雅黑", font_size, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(this.Width / 2-name.Length*(font_size/2), this.Height / 2 - FontHeight ));
                }
                if (TX_image != null)
                {

                    e.Graphics.DrawImage(TX_image, 0, 0, this.Width, this.Height);
                }
           
                this.Cursor = Cursors.Arrow;

            }
            else
            {
                if (TXcolor_start_move.IsEmpty)
                {
                Draw(e.ClipRectangle, e.Graphics, 12, false, Color.FromArgb(0xAE, 0xDA, 0x97), Color.FromArgb(0xAE, 0xDA, 0x97));

                    }
                else
                {
                    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start_move, TXcolor_start_move);
                }

                //    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start_move, TXcolor_end_move);
                if (name != null)
                {
                    e.Graphics.DrawString(name, new Font("微软雅黑", font_size, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(this.Width / 2 - name.Length * (font_size / 2), this.Height / 2 - FontHeight));
                }
                if (TX_image != null)
                {

                    e.Graphics.DrawImage(TX_image, 0, 0, this.Width, this.Height);
                }
             
                this.Cursor = Cursors.Arrow;

            }



        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //  base.OnPaintBackground(e);
            //  Draw(e.ClipRectangle, e.Graphics, 18, false, Color.Gray, Color.Green);

        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);


         //   status = false;

        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
     
    
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            status = true;




        }

    }
    public partial class TXpanel : Panel
    {
        bool status = false;

        private void Draw(Rectangle rectangle, Graphics g, int _radius, bool cusp, Color begin_color, Color end_color)
        {
            try
            {
                int span = 2;
                //抗锯齿
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //渐变填充
                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
                //画尖角
                if (cusp)
                {
                    span = 10;
                    PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                    PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                    PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                    PointF[] ptsArray = { p1, p2, p3 };
                    g.FillPolygon(myLinearGradientBrush, ptsArray);
                }
                //填充
                g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
            }
            catch 
            {
            }
        }
        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }
        public string name = null;  //名称
        public string name_move = null;
        public int font_size = 12;  //字体的大小
        public Color TXcolor_start;  //渐变色的颜色，start开始，end结束
        public Color TXcolor_end;
        public Color TXcolor_start_move;
        public Color TXcolor_end_move;
        public Image TX_image = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            //path.AddEllipse(0, 0, this.Width, this.Height);
            //     this.Region = new Region(path);
            //     this.FlatStyle = FlatStyle.Flat;
            // this.FlatAppearance.BorderSize = 0;
          //  this.Text = null;
         
            if (status == false)
            {
                if (TXcolor_start.IsEmpty)
                {

                 //   Draw(e.ClipRectangle, e.Graphics, 12, false, Color.FromArgb(0xEA, 0xEA, 0xEB), Color.FromArgb(0xEA, 0xEA, 0xEB));

                    Draw(e.ClipRectangle, e.Graphics, 12, false, SystemColors.Control, SystemColors.Control);
                 
                }
                else
                {
                    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start, TXcolor_start);
                }

                // Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start, TXcolor_end);

                e.Graphics.DrawString(name, new Font("微软雅黑", font_size, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(font_size + font_size / 2, this.Height / 2 - FontHeight));
                if (TX_image != null)
                {

                    e.Graphics.DrawImage(TX_image, 0, 0, this.Width, this.Height);
                }
                this.Cursor = Cursors.Arrow;

            }
            else
            {
                if (TXcolor_start_move.IsEmpty)
                {

                    Draw(e.ClipRectangle, e.Graphics, 12, false, SystemColors.Control, SystemColors.Control);
                   // Draw(e.ClipRectangle, e.Graphics, 12, false, Color.FromArgb(0xEA, 0xEA, 0xEB), Color.FromArgb(0xEA, 0xEA, 0xEB));

                }
                else
                {
                    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start_move, TXcolor_start_move);
                }

                //    Draw(e.ClipRectangle, e.Graphics, 12, false, TXcolor_start_move, TXcolor_end_move);

                e.Graphics.DrawString(name, new Font("微软雅黑", font_size, FontStyle.Regular), new SolidBrush(Color.Black), new PointF(font_size + font_size / 2, this.Height / 2 - FontHeight));
                if (TX_image != null)
                {

                    e.Graphics.DrawImage(TX_image, 0, 0, this.Width, this.Height);
                }
                this.Cursor = Cursors.Arrow;

            }



        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //  base.OnPaintBackground(e);
            //  Draw(e.ClipRectangle, e.Graphics, 18, false, Color.Gray, Color.Green);

        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);


            status = true;



        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            status = false;
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);




        }

    }
    public partial class TXbutton_circular : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);

        }

    }
    public class TXDraw : Panel
    {
        Color Draw_Color = Color.FromArgb(30, 255, 50);
        public  int Percentage = 0;
        public  int Percentage_current = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
        
            base.OnPaint(e);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            //   path.AddEllipse(0, 0, this.Width, this.Height);
            Rectangle ret = new Rectangle(0, 0, this.Width, this.Height);

            path.AddRectangle(ret);
            this.Region = new Region(path);


            Graphics ex = this.CreateGraphics();
      


            Rectangle retc = new Rectangle(10, 10, 180, 180);
            Rectangle retd = new Rectangle(35, 35, 130, 130);

            Rectangle rete = new Rectangle(40, 40, 120, 120);
            Rectangle retf = new Rectangle(65, 65, 70, 70);
            Rectangle retg = new Rectangle(70, 70, 60, 60);

            System.Drawing.Font strname_ok_ng = new System.Drawing.Font("黑体", 15, FontStyle.Bold);
            System.Drawing.Font strname = new System.Drawing.Font("黑体", 10, FontStyle.Bold);
            StringFormat ss = new StringFormat(StringFormatFlags.DirectionVertical);




            ex.DrawArc(new Pen(Draw_Color), retc, 180, 180);
            ex.DrawArc(new Pen(Draw_Color), retd, 180, 180);

            ex.FillPie(new SolidBrush(Draw_Color), retc, 180, 180);
            ex.FillPie(new SolidBrush(Color.Red), retc, 180, Percentage);  //百分比100%
            ex.FillPie(Brushes.White, retd, 180, 180);



            ex.DrawArc(new Pen(Draw_Color), rete, 180, 180);
            ex.DrawArc(new Pen(Draw_Color), retf, 180, 180);
            ex.FillPie(new SolidBrush(Draw_Color), rete, 180, 180);
            ex.FillPie(new SolidBrush(Color.Red), rete, 180, Percentage_current);  //百分比100%
            ex.FillPie(Brushes.White, retf, 180, 180);

            ex.DrawArc(new Pen(Color.Red), retg, 180, 180);

            ex.FillPie(Brushes.Red, retg, 360, 360);
            ex.DrawString("OK", strname_ok_ng, Brushes.White, 90, 90, StringFormat.GenericTypographic);

           ex.DrawString(System.DateTime.Now.ToString("d"), strname_ok_ng, Brushes.Black, 20, 140, StringFormat.GenericTypographic);













        }

    }
}

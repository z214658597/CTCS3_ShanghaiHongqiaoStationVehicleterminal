using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ControlLib
{
    public partial class huoche : UserControl
    {
        public string str="G11";                                                          //word
        public static Color s颜色 = Color.FromArgb(0, 0, 0);                     //back颜色
        public Color c_oColor字体 = Color.White;                              //字体的颜色
        public int fangxiang = 15;

        public string Str
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
            }
        }
        //public int Fangxiang
        //{
        //    get
        //    {
        //        return fangxiang;
        //    }
        //    set
        //    {
        //        fangxiang = value;
        //    }
        //}
        public Color back颜色
        {
            get
            {
                return s颜色;
            }
            set
            {
                s颜色 = value;
            }
        }
        public Color Color字体
        {
            get
            {
                return c_oColor字体;
            }
            set
            {
                c_oColor字体 = value;
            }
        }

        public huoche()
        {
            InitializeComponent();
        }

        private void huoche_Load(object sender, EventArgs e)
        {

        }

        private void huoche_Paint(object sender, PaintEventArgs e)
        {
            Graphics l_o图形 = e.Graphics;
            SolidBrush l_aBrush = new SolidBrush(s颜色);

            Pen l_pen1指针1 = new Pen(s颜色, 60);
            l_o图形.DrawRectangle(l_pen1指针1, 10, 10, 120, 40);

            Point point1 = new Point(fangxiang, 20);
            Point point3 = new Point(120-fangxiang, 5);
            Point point4 = new Point(120 - fangxiang, 35);
            Point point2;
            Point point5;
            
            point2 = new Point(fangxiang + 10, 5);
            point5 = new Point(fangxiang + 10, 35);
           
            Point[] mypoints = { point1, point2, point3, point4, point5 };
            RegionControl(mypoints);

            StringFormat sf = new StringFormat();
            SolidBrush l_zBrush = new SolidBrush(c_oColor字体);                     //使用solidbrush类创建一个solidbrush对象  字体颜色
            sf.Alignment = StringAlignment.Center;
            Font font1 = new Font("Arial",18, FontStyle.Bold);
            l_o图形.DrawString(str, font1, l_zBrush, 55, 5, sf);
            l_o图形.Dispose();
        }

        private void RegionControl(Point[] points)
        {
            GraphicsPath mygraphicsPath = new GraphicsPath();
            mygraphicsPath.AddPolygon(points);
            Region myregion = new Region(mygraphicsPath);
            this.Region = myregion;
        }
    }
}

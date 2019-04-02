using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/************************************************************************/
/* 右开道岔-类型2                                                                     */
/************************************************************************/
namespace ControlLib
{
   [ToolboxBitmap(typeof(Daocha1), "Icon.Daocha1.bmp")]
    public partial class Daocha1 : UserControl
    {
        public enum lable
        {
            左开正向,
            右开正向,
            左开反向,
            右开反向
        }
        #region 道岔属性
        public string name_quduan;
        public bool state_dingwei;
        public bool state_Suobi;
        public int width;
        public int height;
        public int mode;
        public string Rlocation = "0,0";
        private lable mylable = lable.左开反向;
        #endregion
        #region 公共属性区
        [Browsable(true), Category("Appearance")]
        public lable Daocha_lable
        {
            get { return mylable; }
            set { mylable = value; drawpic(); }
        }
        #endregion
        #region 辅助变量
        Bitmap bmp;
        Pen p_white, p_blue;
        #endregion
        public int cuxi;
        [Browsable(true), Category("Appearance")]
        public int 粗细
        {
            get { return cuxi; }
            set
            {
                cuxi = value;
                p_white = new Pen(new SolidBrush(Color.White), cuxi);
                p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), cuxi);
                drawpic();
            }
        }
        [Browsable(true), Category("Appearance")]
        public string 实际位置
        {
            get { return Rlocation; }
            set
            {
                Rlocation = value;
            }
        } 
       public Daocha1()
        {
            InitializeComponent();
            initial();
            p_white = new Pen(new SolidBrush(Color.White), 3);
            p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), 3);
            drawpic();
        }
       private void Daocha1_Load(object sender, EventArgs e)
       {
           drawpic();
       }
       private void initial()
       {
           pictureBox1.Width = this.Width;
           pictureBox1.Height = this.Height;
           height = this.Height;
           width = this.Width;
           pictureBox1.Location = new Point(0, 0);
       }
       private void drawpic()
       {
           initial();
           bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
           pictureBox1.Image = bmp;
           Graphics g = Graphics.FromImage(pictureBox1.Image);
           Point p1, p2, temp1, temp2;
           //斜线
           if (mylable == lable.左开反向 || mylable == lable.左开正向)
           {
               p1 = new Point(Width / 6 * 1, height / 4 * 1);//左上
               p2 = new Point(Width / 6 * 5, height / 4 * 3);//右下
               g.DrawLine(p_blue, p1, p2);
               //延长线
               if (mylable == lable.左开反向)
               {
                   temp1 = new Point(p2.X, p2.Y);
                   temp2 = new Point(0, p2.Y);
                   g.DrawLine(p_blue, temp1, temp2);
               }
               else
               {
                   temp1 = new Point(p1.X, p1.Y);
                   temp2 = new Point(width, p1.Y);
                   g.DrawLine(p_blue, temp1, temp2);
               }
           }
           else
           {
               p1 = new Point(Width / 6 * 1, height / 4 * 3);//左下
               p2 = new Point(Width / 6 * 5, height / 4 * 1);//右上
               g.DrawLine(p_blue, p1, p2);
               //延长线
               if (mylable == lable.右开反向)
               {
                   temp1 = new Point(p2.X, p2.Y);
                   temp2 = new Point(0, p2.Y);
                   g.DrawLine(p_blue, temp1, temp2);
               }
               else
               {
                   temp1 = new Point(p1.X, p1.Y);
                   temp2 = new Point(width, p1.Y);
                   g.DrawLine(p_blue, temp1, temp2);
               }
           }
           //绘制道岔延长部分
           temp1 = new Point(p1.X, p1.Y);
           temp2 = new Point(0, p1.Y);
           g.DrawLine(p_blue, temp1, temp2);
           temp1 = new Point(p2.X, p2.Y);
           temp2 = new Point(width, p2.Y);
           g.DrawLine(p_blue, temp1, temp2);
           g.Save();
           pictureBox1.Refresh();
       }
       public void draw_df(bool df)
       {
           initial();
           bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
           pictureBox1.Image = bmp;
           Graphics g = Graphics.FromImage(pictureBox1.Image);
           Point p1, p2, temp1, temp2;
           //斜线
           if (mylable == lable.左开反向 || mylable == lable.左开正向)
           {
               p1 = new Point(Width / 6 * 1, height / 4 * 1);//左上
               p2 = new Point(Width / 6 * 5, height / 4 * 3);//右下
               if (df)
                   g.DrawLine(p_blue, p1, p2);
               else
                   g.DrawLine(p_white, p1, p2);
               //延长线
               if (mylable == lable.左开反向)
               {
                   temp1 = new Point(p2.X, p2.Y);
                   temp2 = new Point(0, p2.Y);
                   if (df)
                       g.DrawLine(p_white, temp1, temp2);
                   else
                       g.DrawLine(p_blue, temp1, temp2);
                   //绘制两端道岔延长部分
                   if (df)
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_blue, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
                   else
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
               }
               else
               {
                   temp1 = new Point(p1.X, p1.Y);
                   temp2 = new Point(width, p1.Y);
                   if (df)
                       g.DrawLine(p_white, temp1, temp2);
                   else
                       g.DrawLine(p_blue, temp1, temp2);
                   //绘制两端道岔延长部分
                   if (df)
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_blue, temp1, temp2);
                   }
                   else
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
               }

           }
           else
           {
               p1 = new Point(Width / 6 * 1, height / 4 * 3);//左下
               p2 = new Point(Width / 6 * 5, height / 4 * 1);//右上
               if (df)
                   g.DrawLine(p_blue, p1, p2);
               else
                   g.DrawLine(p_white, p1, p2);
               //延长线
               if (mylable == lable.右开反向)
               {
                   temp1 = new Point(p2.X, p2.Y);
                   temp2 = new Point(0, p2.Y);
                   if (df)
                       g.DrawLine(p_white, temp1, temp2);
                   else
                       g.DrawLine(p_blue, temp1, temp2);
                   //绘制两端道岔延长部分
                   if (df)
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_blue, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
                   else
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
               }
               else
               {
                   temp1 = new Point(p1.X, p1.Y);
                   temp2 = new Point(width, p1.Y);
                   if (df)
                       g.DrawLine(p_white, temp1, temp2);
                   else
                       g.DrawLine(p_blue, temp1, temp2);
                   //绘制两端道岔延长部分
                   if (df)
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_blue, temp1, temp2);
                   }
                   else
                   {
                       temp1 = new Point(p1.X, p1.Y);
                       temp2 = new Point(0, p1.Y);
                       g.DrawLine(p_white, temp1, temp2);
                       temp1 = new Point(p2.X, p2.Y);
                       temp2 = new Point(width, p2.Y);
                       g.DrawLine(p_white, temp1, temp2);
                   }
               }
           }
           g.Save();
           pictureBox1.Refresh();
       }
       public void reset()
       {
           drawpic();
       }
       private void pictureBox1_Click(object sender, EventArgs e)
       {

       }
    }
}

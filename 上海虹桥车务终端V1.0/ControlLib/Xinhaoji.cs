using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlLib
{
    [ToolboxBitmap(typeof(Xinhaoji), "Icon.Xinhaoji.bmp")]
    public partial class Xinhaoji : UserControl
    {
        public enum lable
        {
            左边,
            右边
        }
        #region 信号机属性
        Graphics g;
        Pen p_white;
        lable mylable;
        int flag;
        int width, height;
        #endregion
        #region 公共属性
        int Fenshu = 4;
        [Browsable(true), Category("Appearance")]
        public lable Daocha_lable
        {
            get { return mylable; }
            set { mylable = value; drawpic(flag); }
        }
        #endregion
        public Xinhaoji()
        {
            InitializeComponent();
            Initial();
            flag = 2;
            mylable = lable.左边;
            p_white = new Pen(Color.White);
        }
        public void Initial()
        {
            width = this.Width;
            height = this.Height;
        }
        public void drawpic(int flag)//1-绿 2-红 3-黄
        {
            Initial();
            g = this.CreateGraphics();
            g.Clear(Color.Black);
            if (mylable == lable.左边)
            {
                Rectangle rt;
                for (int i = 3; i > 0; i--)
                {
                    rt = new Rectangle(new Point(width / Fenshu * i, 0), new Size(width / Fenshu, height));
                    g.FillEllipse(new SolidBrush(Color.White), rt);
                    rt = new Rectangle(new Point(width / Fenshu * i + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Black), rt);
                }
                if (flag == 1)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 3 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Green), rt);
                }
                else if (flag == 2)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 2 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Red), rt);
                }
                else if (flag == 3)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 1 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                g.DrawLine(p_white, new Point(width / 10, 0), new Point(width / 10, height));
                g.DrawLine(p_white, new Point(width / 10, height / 2), new Point(width / Fenshu, height / 2));
            }
            else
            {
                Rectangle rt;
                for (int i = 0; i < 3; i++)
                {
                    rt = new Rectangle(new Point(width / Fenshu * i, 0), new Size(width / Fenshu, height));
                    g.FillEllipse(new SolidBrush(Color.White), rt);
                    rt = new Rectangle(new Point(width / Fenshu * i + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Black), rt);
                }
                if (flag == 1)//绿灯
                {
                    rt = new Rectangle(new Point(0 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Green), rt);
                }
                else if (flag == 2)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 1 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Red), rt);
                }
                else if (flag == 3)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 2 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                g.DrawLine(p_white, new Point(width * 9 / 10, 0), new Point(width * 9 / 10, height));
                g.DrawLine(p_white, new Point(width * 9 / 10, height / 2), new Point(width / Fenshu * 3, height / 2));
            }
            g.Save();
        }
        private void Xinhaoji_Load(object sender, EventArgs e)
        {
            drawpic(flag);
        }
        private void Onpaint(object sender, PaintEventArgs e)
        {
            drawpic(flag);
        }
    }
}

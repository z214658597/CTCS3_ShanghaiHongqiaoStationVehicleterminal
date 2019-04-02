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
    [ToolboxBitmap(typeof(ChuzhanXHJ), "Icon.ChuzhanXHJ.bmp")]
    
    public partial class ChuzhanXHJ : UserControl
    {
        public enum lable
        {
            左边,
            右边
        }
        public enum XinhaojiLable
        {
            高柱,
            矮柱
        }
        public enum Jinlustate
        {
            绿灯,
            黄灯,
            红灯,
        }
        #region 信号机属性
        Graphics g;
        Pen p_white;
        lable mylable;
        Jinlustate jinlustate;
        XinhaojiLable xinhaojilable;
        int flag;
        int width, height;
        #endregion
        #region 公共属性
        [Browsable(true), Category("Appearance")]
        public lable Daocha_lable
        {
            get { return mylable; }
            set { mylable = value; drawpic(flag); }
        }
        [Browsable(true), Category("Appearance")]
        public XinhaojiLable Xinhaoji_lable
        {
            get { return xinhaojilable; }
            set { xinhaojilable = value;
            if (xinhaojilable == XinhaojiLable.高柱)
            {
                this.Size = new Size(height/2*5, height/2);
            }
            else
            {
                this.Size = new Size(height * 2, height * 2);
            }
                drawpic(flag);
            }
        }
        [Browsable(true), Category("Appearance")]
        public Jinlustate Jinlu
        {
            get { return jinlustate; }
            set
            {
                jinlustate = value;
                switch (jinlustate)
                {
                    case Jinlustate.绿灯:
                        flag = 1;
                        break;
                    case Jinlustate.黄灯:
                        flag = 2;
                        break;
                    case Jinlustate.红灯:
                        flag = 3;
                        break;
                }
                drawpic(flag);
            }
        }
        #endregion
        public ChuzhanXHJ()
        {
            InitializeComponent();
            Initial();
            flag = 3;
            mylable = lable.左边;
            jinlustate = Jinlustate.红灯;
            xinhaojilable = XinhaojiLable.高柱;
            p_white = new Pen(Color.White);
        }
        public void Initial()
        {
            width = this.Width;
            height = this.Height;
        }
        /// <summary>
        /// flag 1—绿
        ///      2—黄
        ///      3—红
        /// </summary>
        /// <param name="flag"></param>
        public void drawpic(int flag)
        {
            Initial();
            g = this.CreateGraphics();
            g.Clear(Color.Black);
            if (mylable == lable.左边)
            {
                if (xinhaojilable == XinhaojiLable.高柱)
                {
                    Rectangle rt;
                    for (int i = 4; i > 0; i--)
                    {
                        rt = new Rectangle(new Point(width / 5 * i, 0), new Size(width / 5, height));
                        g.FillEllipse(new SolidBrush(Color.White), rt);
                        rt = new Rectangle(new Point(width / 5 * i + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Black), rt);
                    }
                    if (flag == 1)
                    {
                        rt = new Rectangle(new Point(width / 5 * 4 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Green), rt);
                    }
                    else if (flag == 2)
                    {
                        rt = new Rectangle(new Point(width / 5 * 3 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    }
                    else if (flag == 3)
                    {
                        rt = new Rectangle(new Point(width / 5 * 2 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Red), rt);
                    }
                    g.DrawLine(p_white, new Point(width / 12, 0), new Point(width / 12, height));
                    g.DrawLine(p_white, new Point(width / 12, height / 2), new Point(width / 5, height / 2));
                }
                else
                {
                    Rectangle rt;
                    for (int i = 2; i > 0; i--)
                    {
                        for (int j = 2; j > 0; j--)
                        {
                            rt = new Rectangle(new Point(width / 2 * (i - 1), height / 2 * (j - 1)), new Size(width / 2, height / 2));
                            g.FillEllipse(new SolidBrush(Color.White), rt);
                            rt = new Rectangle(new Point(width / 2 * (i - 1) + 1, height / 2 * (j - 1) + 1), new Size(width / 2 - 2, height / 2 - 2));
                            g.FillEllipse(new SolidBrush(Color.Black), rt);
                        }
                    }
                    if (flag == 1)
                    {
                        rt = new Rectangle(new Point(width / 2 * 0 + 1, height / 2 * 0 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Green), rt);
                    }
                    else if (flag == 2)
                    {
                        rt = new Rectangle(new Point(width / 2 * 1 + 1, height / 2 * 0 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    }
                    else if (flag == 3)
                    {
                        rt = new Rectangle(new Point(width / 2 * 0 + 1, height / 2 * 1 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Red), rt);
                    }
                    g.DrawLine(p_white, new Point(2, 0), new Point(2, height));
                }

            }
            else
            {
                if (xinhaojilable == XinhaojiLable.高柱)
                {
                    Rectangle rt;
                    for (int i = 0; i < 4; i++)
                    {
                        rt = new Rectangle(new Point(width / 5 * i, 0), new Size(width / 5, height));
                        g.FillEllipse(new SolidBrush(Color.White), rt);
                        rt = new Rectangle(new Point(width / 5 * i + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Black), rt);
                    }
                    if (flag == 1)
                    {
                        rt = new Rectangle(new Point(width / 5 * 1 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Green), rt);
                    }
                    else if (flag == 2)
                    {
                        rt = new Rectangle(new Point(width / 5 * 0 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    }
                    else if (flag == 3)
                    {
                        rt = new Rectangle(new Point(width / 5 * 2 + 1, 1), new Size(width / 5 - 2, height - 2));
                        g.FillEllipse(new SolidBrush(Color.Red), rt);
                    }
                    g.DrawLine(p_white, new Point(width * 11 / 12, 0), new Point(width * 11 / 12, height));
                    g.DrawLine(p_white, new Point(width * 11 / 12, height / 2), new Point(width / 5 * 4, height / 2));
                }
                else
                {
                    Rectangle rt;
                    for (int i = 2; i > 0; i--)
                    {
                        for (int j = 2; j > 0; j--)
                        {
                            rt = new Rectangle(new Point(width / 2 * (i - 1), height / 2 * (j - 1)), new Size(width / 2, height / 2));
                            g.FillEllipse(new SolidBrush(Color.White), rt);
                            rt = new Rectangle(new Point(width / 2 * (i - 1) + 1, height / 2 * (j - 1) + 1), new Size(width / 2 - 2, height / 2 - 2));
                            g.FillEllipse(new SolidBrush(Color.Black), rt);
                        }
                    }
                    if (flag == 1)
                    {
                        rt = new Rectangle(new Point(width / 2 * 1 + 1, height / 2 * 1 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Green), rt);
                    }
                    else if (flag == 2)
                    {
                        rt = new Rectangle(new Point(width / 2 * 0 + 1, height / 2 * 1 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    }
                    else if (flag == 3)
                    {
                        rt = new Rectangle(new Point(width / 2 * 1 + 1, height / 2 * 0 + 1), new Size(width / 2 - 2, height / 2 - 2));
                        g.FillEllipse(new SolidBrush(Color.Red), rt);
                    }
                    g.DrawLine(p_white, new Point(width - 2, 0), new Point(width - 2, height));
                }

            }
            g.Save();
        }

        private void ChuzhanX_Load(object sender, EventArgs e)
        {
            drawpic(flag);
        }

        private void Onpaint(object sender, PaintEventArgs e)
        {
            drawpic(flag);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        
        
        
    }
}

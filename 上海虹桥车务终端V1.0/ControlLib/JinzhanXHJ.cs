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
    [ToolboxBitmap(typeof(JinzhanXHJ), "Icon.JinzhanX.bmp")]
    public partial class JinzhanXHJ : UserControl
    {
        public enum lable
        {
            左边,
            右边
        }
        public enum Jinlustate
        {
            正线接车,
            侧线接车,
            正线通过,
            禁止通过,
        }
        int Fenshu = 6;
        #region 信号机属性
        Graphics g;
        Pen p_white;
        lable mylable;
        Jinlustate jinlustate;
        int flag;
        int width, height;
        string ID = "1";
        #endregion
        #region 公共属性
        public string ID号
        {
            get { return ID; }
            set
            {
                ID = value;
                drawpic(flag);
            }
        }
        public enum IDweizhi
        {
            上,
            下
        }
        IDweizhi idweizhi = IDweizhi.上;
        [Browsable(true), Category("Appearance")]
        public IDweizhi ID位置
        {
            get { return idweizhi; }
            set
            {
                idweizhi = value;
                drawpic(flag);
            }
        }
        [Browsable(true), Category("Appearance")]
        public lable Daocha_lable
        {
            get { return mylable; }
            set { mylable = value; drawpic(flag); }
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
                    case Jinlustate.正线接车:
                        flag = 1;
                        break;
                    case Jinlustate.侧线接车:
                        flag = 2;
                        break;
                    case Jinlustate.正线通过:
                        flag = 3;
                        break;
                    case Jinlustate.禁止通过:
                        flag = 4;
                        break;
                }
                drawpic(flag);
            }
        }
        #endregion
        public JinzhanXHJ()
        {
            InitializeComponent();
            Initial();
            flag = 4;
            mylable = lable.左边;
            jinlustate = Jinlustate.禁止通过;
            p_white = new Pen(Color.White);
        }
        public void Initial()
        {
            width = this.Width;
            height = this.Height;
        }
        /// <summary>
        /// flag 1—正线停车
        ///      2—侧线停车
        ///      3—正线通过
        ///      4—信号机前停车
        /// </summary>
        /// <param name="flag"></param>
        public void drawpic(int flag)
        {
            Initial();
            g = this.CreateGraphics();
            g.Clear(Color.Black);
            if (mylable == lable.左边)
            {
                Rectangle rt;
                for (int i = 5; i > 0; i--)
                {
                    rt = new Rectangle(new Point(width / Fenshu * i, 0), new Size(width / Fenshu, height));
                    g.FillEllipse(new SolidBrush(Color.White), rt);
                    rt = new Rectangle(new Point(width / Fenshu * i + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Black), rt);
                }
                if (flag == 1)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 5 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                else if (flag == 2)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 5 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    rt = new Rectangle(new Point(width / Fenshu * 2 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                else if (flag == 3)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 4 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Green), rt);
                }
                else if (flag == 4)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 3 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Red), rt);
                }
                g.DrawLine(p_white, new Point(width / 14, 0), new Point(width / 14, height));
                g.DrawLine(p_white, new Point(width / 14, height / 2), new Point(width / 6, height / 2));
                if (idweizhi == IDweizhi.上)
                {
                    g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(width / 14, 0));
                }
                else
                {
                    g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(width / 14, height - 10));
                }
            }
            else
            {
                Rectangle rt;
                for (int i = 0; i < 5; i++)
                {
                    rt = new Rectangle(new Point(width / Fenshu * i, 0), new Size(width / Fenshu, height));
                    g.FillEllipse(new SolidBrush(Color.White), rt);
                    rt = new Rectangle(new Point(width / Fenshu * i + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Black), rt);
                }
                if (flag == 1)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 0 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                else if (flag == 2)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 0 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    rt = new Rectangle(new Point(width / Fenshu * 3 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                }
                else if (flag == 3)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 1 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Green), rt);
                }
                else if (flag == 4)
                {
                    rt = new Rectangle(new Point(width / Fenshu * 2 + 1, 1), new Size(width / Fenshu - 2, height - 2));
                    g.FillEllipse(new SolidBrush(Color.Red), rt);
                }
                g.DrawLine(p_white, new Point(width * 13 / 14, 0), new Point(width * 13 / 14, height));
                g.DrawLine(p_white, new Point(width * 13 / 14, height / 2), new Point(width / Fenshu * 5, height / 2));
                if (idweizhi == IDweizhi.上)
                {
                    g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(width * 13 / 14, 0));
                }
                else
                {
                    g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(width * 13 / 14, height - 10));
                }
            }
            g.Save();
        }

        private void JinzhanX_Load(object sender, EventArgs e)
        {
            drawpic(flag);
        }

        private void Onpaint(object sender, PaintEventArgs e)
        {
            drawpic(flag);
        }
    }
}

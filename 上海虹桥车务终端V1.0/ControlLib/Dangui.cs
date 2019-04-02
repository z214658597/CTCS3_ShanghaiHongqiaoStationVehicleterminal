using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ControlLib
{
    [ToolboxBitmap(typeof(Dangui), "Icon.Dangui.bmp")]
    public partial class Dangui : UserControl
    {
        #region 属性
        public int flag_zt;
        public string Rlocation;
        public string ID;
        public string p_form = "";
        public IntPtr wparam = new IntPtr(0);
        public IntPtr pwaram = new IntPtr(0);
        public IntPtr handle = new IntPtr(0);
        #endregion
        #region z位置属性
        public enum Weizhi
        {
            置顶,
            置底
        }
        public Weizhi weizhi;
        #endregion
        #region 辅助变量
        Bitmap bmp;
        Pen p_white, p_blue, p_red, p_purple, jyj_white;
        //public const int CUSTOM_MESSAGE = 0X400 + 2;
        #endregion
        public enum IDweizhi
        {
            上,
            下
        }
        IDweizhi idweizhi = IDweizhi.上;
        [Browsable(true), Category("Appearance")]
        public string 实际位置
        {
            get { return Rlocation; }
            set
            {
                Rlocation = value;
            }
        }
        [Browsable(true), Category("Appearance")]
        public IDweizhi ID位置
        {
            get { return idweizhi; }
            set
            {
                idweizhi = value;
                flag_zt = 3;
                Drawpic();
            }
        }
        [Browsable(true), Category("Appearance")]
        public string ID_Dangui
        {
            get { return ID; }
            set
            {
                ID = value;
            }
        }
        public enum Jueyuanjie
        {
            左,
            右,
            双边,
            无
        }
        public Jueyuanjie jyj;
        [Browsable(true), Category("Appearance")]
        public Jueyuanjie 绝缘节
        {
            get { return jyj; }
            set
            {
                jyj = value;
                flag_zt = 3;
                Drawpic();
            }
        }
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
                p_red = new Pen(new SolidBrush(Color.Red), cuxi);
                jyj_white = new Pen(new SolidBrush(Color.White), cuxi / 2);
                flag_zt = 3;
                Drawpic();
            }
        }
        [Browsable(true), Category("Appearance")]
        public Weizhi Zlocation
        {
            get { return weizhi; }
            set
            {
                weizhi = value;
                switch (weizhi)
                {
                    case Weizhi.置底:
                        this.SendToBack();
                        break;
                    case Weizhi.置顶:
                        this.BringToFront();
                        break;
                }
            }
        }
        public string ch365_position;//?????
        public string 板卡位置//????
        {
            get
            { return ch365_position; ; }
            set
            {
                ch365_position = value;
            }
        }
        
        
        
        public Dangui()
        {
            InitializeComponent();
            initial();
            cuxi = 2;
            weizhi = Weizhi.置顶;
            p_white = new Pen(new SolidBrush(Color.White), cuxi);
            p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), cuxi);
            p_red = new Pen(new SolidBrush(Color.Red), cuxi);
            p_purple = new Pen(new SolidBrush(Color.Purple), cuxi);
            jyj_white = new Pen(new SolidBrush(Color.White), cuxi);
            p_white.Width = cuxi;
            p_blue.Width = cuxi;
            p_red.Width = cuxi;
            p_purple.Width = cuxi;
            jyj_white.Width = cuxi / 2;
        }
        private void initial()
        {
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.Location = new Point(0, 0);
        }
        private void Dangui_Load(object sender, EventArgs e)
        {
            flag_zt = 3;
            Drawpic();
        }
        public void reset()
        {
            flag_zt = 3;
            Drawpic();
        }
        /// <summary>
        /// 1是占用绘制信息，2是锁闭绘制信息,3是未锁闭未占用
        /// </summary>
        /// <param name="choose"></param>
        public void Drawpic()
        {
            pwaram = new IntPtr(flag_zt);
            initial();
            Point p1, p2;
            int m = 0;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            p1 = new Point(cuxi, m / 2 + (pictureBox1.Height - m) / 2);
            p2 = new Point(pictureBox1.Width - 1, m / 2 + (pictureBox1.Height - m) / 2);
            switch (flag_zt)
            {
                case 1:
                    g.DrawLine(p_red, p1, p2);
                    break;
                case 2:
                    g.DrawLine(p_white, p1, p2);
                    break;
                case 3:
                    g.DrawLine(p_blue, p1, p2);
                    break;
                case 4:
                    g.DrawLine(p_purple, p1, p2);
                    break;
                default:
                    throw new Exception("单轨输入信息不在范围内");
            }
            switch (jyj)
            {
                case Jueyuanjie.左:
                    p1 = new Point(cuxi / 2, 0);
                    p2 = new Point(cuxi / 2, pictureBox1.Height);
                    g.DrawLine(jyj_white, p1, p2);
                    break;
                case Jueyuanjie.右:
                    p1 = new Point(pictureBox1.Width - cuxi / 2, 0);
                    p2 = new Point(pictureBox1.Width - cuxi / 2, pictureBox1.Height);
                    g.DrawLine(jyj_white, p1, p2);
                    break;
                case Jueyuanjie.双边:
                    p1 = new Point(cuxi / 2, 0);
                    p2 = new Point(cuxi / 2, pictureBox1.Height);
                    g.DrawLine(jyj_white, p1, p2);
                    p1 = new Point(pictureBox1.Width - cuxi / 2, 0);
                    p2 = new Point(pictureBox1.Width - cuxi / 2, pictureBox1.Height);
                    g.DrawLine(jyj_white, p1, p2);
                    break;
            }
        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void Onpaint(object sender, PaintEventArgs e)
        {
            initial();
        }

        private void MounseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void MounseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
               contextMenuStrip1.Location = this.Location;
                contextMenuStrip1.Show(contextMenuStrip1.Location);
            }
        }

        private void 置为白光带区段加锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_zt = 2;
            Drawpic();
        }

        private void 去除白光带区段解锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_zt = 3;
            Drawpic();
        }

        private void 去除红光带区段占用解除ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            flag_zt = 3;
            Drawpic();
            pwaram = new IntPtr(flag_zt);
            //Send_Message.sendmessage(handle, CUSTOM_MESSAGE, wparam, pwaram);
        }

        private void 置为红光带区段占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_zt = 1;
            Drawpic();
            pwaram = new IntPtr(flag_zt);//Send_Message.Message_DG
            //Send_Message.sendmessage(handle, CUSTOM_MESSAGE, wparam, pwaram);
        }

        private void 股道封锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_zt = 4;
            Drawpic();
        }

        private void 股道解封ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag_zt = 3;
            Drawpic();
            pwaram = new IntPtr(flag_zt);
        }

    }
}

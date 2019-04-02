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
    public partial class Daocha_1_1 : UserControl
    {
        #region 定义事件
        public event CustomEventHandler Custom;
        public delegate void CustomEventHandler(object sender, CustomEventArgs e);
        public sealed class CustomEventArgs : EventArgs
        {
            private bool testFlag;
            public CustomEventArgs(bool testFlag)
            {
                this.testFlag = testFlag;
            }
            public bool Flag
            {
                get { return testFlag; }
            }
        }
        protected virtual void OnCustom(CustomEventArgs e)
        {
            if (Custom != null)
            {
                Custom(this, e);
            }
        }
        bool testFlag1;
        //private void MyControl(object sender, EventArgs e)
        //{
        //    Console.WriteLine("111");
        //    bool testFlag1 = true;

        //    if (DF_flag == Daocha_1.DingFan.定位)
        //    {
        //        testFlag1 = true;
        //    }
        //    else
        //    {
        //        testFlag1 = false;
        //    }
        //    CustomEventArgs eArgs = new CustomEventArgs(testFlag1);
        //    OnCustom(eArgs);
        //}
        #endregion

        public enum STATE
        {
            空闲,
            占用,
            锁闭
        }
        public enum DingFan
        {
            定位,
            反位
        }
        public enum Fangwei
        {
            左上,
            右上,
            左下,
            右下
        }
        public enum Weizhi
        {
            置顶,
            置底
        }
        public enum DanCao
        {
            进路,
            单操
        }
        #region 变量区
        Point[] a = new Point[8];
        public IntPtr handle = new IntPtr(0);
        public IntPtr wparam = new IntPtr(0);
        public IntPtr pwaram = new IntPtr(0);
        public STATE state = STATE.空闲;
        string ID = "1";
        public Weizhi weizhi;
        public string Rlocation = "0,0";
        public Fangwei fangwei = Fangwei.左上;
        public DanCao dancao = DanCao.进路;
        Bitmap bmp;
        public DingFan DF_flag = DingFan.定位;
        Pen p_white, p_blue, p_red;
        List<myLine> line = new List<myLine>();
        public int cuxi = 3;
        [Browsable(true), Category("Appearance")]
        public string ID号
        {
            get { return ID; }
            set
            {
                ID = value;
                Drawpic(DF_flag, state);
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
        [Browsable(true), Category("Appearance")]
        public Fangwei 方位
        {
            get { return fangwei; }
            set
            {
                fangwei = value;
                Drawpic(DF_flag, state);
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
                Drawpic(DF_flag, state);
            }
        }
        [Browsable(true), Category("Appearance")]
        public DingFan 定反位
        {
            get { return DF_flag; }
            set
            {
                DF_flag = value;
                if (ID != "")
                {
                    wparam = new IntPtr(Convert.ToInt32(ID));
                    if (DF_flag == DingFan.反位)
                        pwaram = new IntPtr(2);
                    else if (DF_flag == DingFan.定位)
                        pwaram = new IntPtr(1);
                    //Send_Message.sendmessage(handle, Send_Message.Message_DC1, wparam, pwaram);
                }
                Drawpic(DF_flag, state);
            }
        }
        [Browsable(true), Category("Appearance")]
        public STATE 锁闭状态
        {
            get { return state; }
            set
            {
                state = value;
                Drawpic(DF_flag, state);
            }
        }

        public string ch365_position;
        public string 道岔板卡位置
        {
            get
            { return ch365_position; ; }
            set
            {
                ch365_position = value;
            }
        }
        public DanCao 道岔单操
        {
            get
            { return dancao; }
            set
            {
                dancao = value;
            }
        }
        #endregion   
        public Daocha_1_1()
        {
            InitializeComponent();
            p_white = new Pen(new SolidBrush(Color.White), cuxi);
            p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), cuxi);
            p_red = new Pen(new SolidBrush(Color.Red), cuxi);
            //p_jyj_white = new Pen(new SolidBrush(Color.White), cuxi / 2);
        }
        public void initial()
        {
            int m = 0;
            a[1] = new Point(0, m / 2 + (this.Height - m) * 15 / 16);
            a[2] = new Point(this.Width * 3 / 16, m / 2 + (this.Height - m) * 15 / 16);
            //a[3] = new Point(this.Width * 13 / 32, m / 2 + (this.Height - m) * 15 / 16);
            a[3] = new Point(this.Width * 16 / 16, m / 2 + (this.Height - m) * 15 / 16);
            a[4] = new Point(this.Width * 1 / 16, m / 2 + (this.Height - m) * 13 / 16);
            a[5] = new Point(this.Width * 7 / 16, m / 2 + (this.Height - m) * 1 / 16);
            a[6] = new Point(this.Width * 16 / 16, m / 2 + (this.Height - m) * 1 / 16);
            line.Clear();
            myLine myline = new myLine(a[1], a[2], 1);
            line.Add(myline);
            myline = new myLine(a[2], a[3], 2);
            line.Add(myline);
            myline = new myLine(a[1], a[4], 3);
            line.Add(myline);
            myline = new myLine(a[4], a[5], 4);
            line.Add(myline);
            myline = new myLine(a[5], a[6], 5);
            line.Add(myline);
            //myline = new myLine(a[6], a[7], 6);
            //line.Add(myline);
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
        }
        private void Daocha_1_1_Load(object sender, EventArgs e)
        {
            Drawpic(DF_flag, state);
        }
        public void draw()
        {
            Drawpic(DF_flag, state);
        }
        private void Drawpic(DingFan flag, STATE st)
        {
            Point p1, p2;
            initial();
            if (bmp != null)
            {
                bmp.Dispose();
            }
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            foreach (myLine m in line)
            {
                if (m.key == 1)
                {
                    if (DF_flag == DingFan.定位)
                    {
                        m.enable = true;
                    }
                    else if (DF_flag == DingFan.反位)
                    {
                        m.enable = false;
                    }
                }
                else if (m.key == 3)
                {
                    if (DF_flag == DingFan.定位)
                    {
                        m.enable = false;
                    }
                    else if (DF_flag == DingFan.反位)
                    {
                        m.enable = true;
                    }
                }
                m.draw(g, p_blue);
            }
            if (DF_flag == DingFan.定位)
            {
                foreach (myLine m in line)
                {
                    if (m.key == 1 || m.key == 2 )
                    {
                        switch (state)
                        {
                            case STATE.锁闭:
                                m.draw(g, p_white);
                                break;
                            case STATE.占用:
                                m.draw(g, p_red);
                                break;
                        }
                    }
                }
            }
            else
            {
                foreach (myLine m in line)
                {
                    if (m.key == 3 || m.key == 4 || m.key == 5)
                    {
                        switch (state)
                        {
                            case STATE.锁闭:
                                m.draw(g, p_white);
                                break;
                            case STATE.占用:
                                m.draw(g, p_red);
                                break;
                        }
                    }
                }
            }
            switch (fangwei)
            {
                case Fangwei.左上:
                    //g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(a[2].X-5, a[2].Y - 12));
                    p1 = new Point(0, pictureBox1.Height - 9);
                    p2 = new Point(0, pictureBox1.Height);
                   // g.DrawLine(p_jyj_white, p1, p2);
                    break;
                case Fangwei.左下:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    //g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(a[2].X-5, this.Height-a[2].Y+6));
                    p1 = new Point(0, 0);
                    p2 = new Point(0, 9);
                   // g.DrawLine(p_jyj_white, p1, p2);
                    break;
                case Fangwei.右上:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    //g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(this.Width-a[2].X-4, a[2].Y - 12));
                    p1 = new Point(pictureBox1.Width - 1, pictureBox1.Height - 9);
                    p2 = new Point(pictureBox1.Width - 1, pictureBox1.Height);
                   // g.DrawLine(p_jyj_white, p1, p2);
                    break;
                case Fangwei.右下:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                    //g.DrawString(ID, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(this.Width - a[2].X-4, this.Height - a[2].Y + 6));
                    p1 = new Point(pictureBox1.Width - 1, 0);
                    p2 = new Point(pictureBox1.Width - 1, 9);
                  //  g.DrawLine(p_jyj_white, p1, p2);
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
        }
        public void Draw(object sender, PaintEventArgs e)
        {
            Drawpic(DF_flag, state);
        }
        private void Onpaint(object sender, PaintEventArgs e)
        {
            Drawpic(DF_flag, state);
        }
        private void MounseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void MounseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void 道岔定位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DF_flag = DingFan.定位;
            wparam = new IntPtr(Convert.ToInt32(ID));
            pwaram = new IntPtr(1);
            //Send_Message.sendmessage(handle, Send_Message.Message_DC1, wparam, pwaram);
            Drawpic(DF_flag, state);

            //事件部分
            testFlag1 = true;
            CustomEventArgs eArgs = new CustomEventArgs(testFlag1);
            OnCustom(eArgs);
        }

        private void 道岔反位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DF_flag = DingFan.反位;
            wparam = new IntPtr(Convert.ToInt32(ID));
            pwaram = new IntPtr(2);
            //Send_Message.sendmessage(handle, Send_Message.Message_DC1, wparam, pwaram);
            Drawpic(DF_flag, state);

            //事件部分
            testFlag1 = false;
            CustomEventArgs eArgs = new CustomEventArgs(testFlag1);
            OnCustom(eArgs);

        }

        private void 道岔空闲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = STATE.空闲;
            Drawpic(DF_flag, state);
        }

        private void 道岔锁闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = STATE.锁闭;
            Drawpic(DF_flag, state);
        }

        private void 道岔占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = STATE.占用;
            Drawpic(DF_flag, state);
        }

        private void 道岔单锁ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = STATE.锁闭;
            Drawpic(DF_flag, state);
        }

        private void 道岔单解ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = STATE.空闲;
            Drawpic(DF_flag, state);
        }
        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this.Location);
            }
        }
        public class myLine
        {
            Point p1, p2;
            public int key;
            public bool enable;
            public myLine(Point t1, Point t2, int i)
            {
                p1 = t1;
                p2 = t2;
                key = i;
                enable = true;
            }
            public void draw(Graphics g, Pen p)
            {
                if (enable)
                {
                    g.DrawLine(p, p1, p2);
                }
            }
        }

    }
}

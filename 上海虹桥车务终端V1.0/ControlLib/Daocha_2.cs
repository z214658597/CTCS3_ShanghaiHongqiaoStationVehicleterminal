using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace ControlLib
{
    public partial class Daocha_2 : UserControl
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
            左,
            右
        }
        public enum Weizhi
        {
            置顶,
            置底
        }
        public enum DanCao
        {
            单操,
            进路
        }

        #region 变量区
        Point[] a = new Point[7];
        Point[] b = new Point[7];
        public IntPtr handle, wparam, pwaram;
        public STATE state_up = STATE.空闲;
        public STATE state_down = STATE.空闲;
        public string ID_up = "0";
        public string ID_down = "0";
        public string RlocationS = "0,0";
        public string RlocationX = "0,0";
        public Fangwei fangwei = Fangwei.左;
        Bitmap bmp;
        public DingFan DF_flag_up = DingFan.定位;
        public DingFan DF_flag_down = DingFan.定位;
        public DanCao dancao = DanCao.进路;
        Pen p_white, p_blue, p_red;
        List<myLine> linea = new List<myLine>();
        List<myLine> lineb = new List<myLine>();
        public int cunxi = 3;
        public Weizhi weizhi;
        [Browsable(true), Category("Appearance")]
        public string ID号上
        {
            get { return ID_up; }
            set
            {
                ID_up = value;
            }
        }
        [Browsable(true), Category("Appearance")]
        public string 实际位置下行
        {
            get { return RlocationX; }
            set
            {
                RlocationX = value;
            }
        }
        [Browsable(true), Category("Appearance")]
        public string 实际位置上行
        {
            get { return RlocationS; }
            set
            {
                RlocationS = value;
            }
        }
        [Browsable(true), Category("Appearance")]
        public string ID号下
        {
            get { return ID_down; }
            set
            {
                ID_down = value;
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
        public Fangwei 方位
        {
            get { return fangwei; }
            set
            {
                fangwei = value;
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
        [Browsable(true), Category("Appearance")]
        public int 粗细
        {
            get { return cunxi; }
            set
            {
                cunxi = value;
                p_white = new Pen(new SolidBrush(Color.White), cunxi);
                p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), cunxi);
                p_red = new Pen(new SolidBrush(Color.Red), cunxi);
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
        [Browsable(true), Category("Appearance")]
        public DingFan 定反位上
        {
            get { return DF_flag_up; }
            set
            {
                DF_flag_up = value;
                if ((ID_down != "") && (ID_up != ""))
                {
                    if ((DF_flag_up == DingFan.定位) && (DF_flag_down == DingFan.定位))
                    {
                        wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
                        pwaram = new IntPtr(1);
                        //Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
                    }
                    else if ((DF_flag_up == DingFan.反位) && (DF_flag_down == DingFan.反位))
                    {
                        wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
                        pwaram = new IntPtr(2);
                       // Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
                    }
                }
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
        [Browsable(true), Category("Appearance")]
        public DingFan 定反位下
        {
            get { return DF_flag_down; }
            set
            {
                DF_flag_down = value;
                if ((ID_down != "") && (ID_up != ""))
                {
                    if ((DF_flag_up == DingFan.定位) && (DF_flag_down == DingFan.定位))
                    {
                        wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
                        pwaram = new IntPtr(1);
                        //Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
                    }
                    else if ((DF_flag_up == DingFan.反位) && (DF_flag_down == DingFan.反位))
                    {
                        wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
                        pwaram = new IntPtr(2);
                        //Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
                    }
                }
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
        [Browsable(true), Category("Appearance")]
        public STATE 锁闭状态上
        {
            get { return state_up; }
            set
            {
                state_up = value;
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
        [Browsable(true), Category("Appearance")]
        public STATE 锁闭状态下
        {
            get { return state_down; }
            set
            {
                state_down = value;
                Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
            }
        }
 
        public string ch365_s_position;
        public string 上行轨道板卡位置
        {
            get
            { return ch365_s_position; ; }
            set
            {
                ch365_s_position = value;
            }
        }
        public string ch365_x_position;
        public string 下行轨道板卡位置
        {
            get
            { return ch365_x_position; ; }
            set
            {
                ch365_x_position = value;
            }
        }
        public DanCao 道岔单操//新加
        {
            get { return dancao; }
            set
            {
                dancao = value;

            }
        }
        #endregion
        public Daocha_2()
        {
            InitializeComponent();
            p_white = new Pen(new SolidBrush(Color.White), cunxi);
            p_blue = new Pen(new SolidBrush(Color.MediumTurquoise), cunxi);
            p_red = new Pen(new SolidBrush(Color.Red), cunxi);
        }
        public void initial()
        {
            int m = 0;
            a[1] = new Point(0, m / 2 + (this.Height - m) * 15 / 16);
            a[2] = new Point(this.Width * 4 / 16, m / 2 + (this.Height - m) * 15 / 16);
            a[3] = new Point(this.Width * 5 / 16, m / 2 + (this.Height - m) * 15 / 16);
            a[4] = new Point(this.Width * 16 / 16, m / 2 + (this.Height - m) * 15 / 16);
            a[5] = new Point(this.Width * 5 / 16, m / 2 + (this.Height - m) * 13 / 16);
            a[6] = new Point(this.Width * 8 / 16, m / 2 + (this.Height - m) * 8 / 16);
            b[1] = new Point(this.Width * 16 / 16, m / 2 + (this.Height - m) * 1 / 16);
            b[2] = new Point(this.Width * 12 / 16, m / 2 + (this.Height - m) * 1 / 16);
            b[3] = new Point(this.Width * 11 / 16, m / 2 + (this.Height - m) * 1 / 16);
            b[4] = new Point(this.Width * 0 / 16, m / 2 + (this.Height - m) * 1 / 16);
            b[5] = new Point(this.Width * 11 / 16, m / 2 + (this.Height - m) * 3 / 16);
            b[6] = new Point(this.Width * 8 / 16, m / 2 + (this.Height - m) * 8 / 16);
            linea.Clear();
            lineb.Clear();
            myLine myline = new myLine(a[1], a[2], 1);
            linea.Add(myline);
            myline = new myLine(a[2], a[3], 2);
            linea.Add(myline);
            myline = new myLine(a[3], a[4], 3);
            linea.Add(myline);
            myline = new myLine(a[2], a[5], 4);
            linea.Add(myline);
            myline = new myLine(a[5], a[6], 5);
            linea.Add(myline);

            myline = new myLine(b[1], b[2], 1);
            lineb.Add(myline);
            myline = new myLine(b[2], b[3], 2);
            lineb.Add(myline);
            myline = new myLine(b[3], b[4], 3);
            lineb.Add(myline);
            myline = new myLine(b[2], b[5], 4);
            lineb.Add(myline);
            myline = new myLine(b[5], b[6], 5);
            lineb.Add(myline);

            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
        }
        private void Daocha_2_Load(object sender, EventArgs e)
        {
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }
        private void Drawpic_sub(Graphics g, DingFan DF_flag, STATE state, List<myLine> line)
        {
            foreach (myLine m in line)
            {
                if (m.key == 2)
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
                else if (m.key == 4)
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
                    if (m.key == 1 || m.key == 2 || m.key == 3)
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
                    if (m.key == 1 || m.key == 4 || m.key == 5)
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
        }
        public void Draw()
        {
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }
        private void Drawpic(DingFan DF_flag, DingFan DF_flag2, STATE state, STATE state2)
        {
            initial();
            if (bmp != null)
            {
                bmp.Dispose();
            }
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            Drawpic_sub(g, DF_flag, state, linea);
            Drawpic_sub(g, DF_flag2, state2, lineb);
            g.DrawLine(p_blue, new Point(a[6].X - 4, a[6].Y - 4), new Point(a[6].X + 4, a[6].Y + 2));
            switch (fangwei)
            {
                case Fangwei.左:
                    //g.DrawString(ID_down, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(a[2].X -6, a[2].Y));
                    //g.DrawString(ID_up, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(b[2].X-6 , b[2].Y-10));
                    break;
                case Fangwei.右:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    //g.DrawString(ID_down, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(this.Width - a[2].X , a[2].Y));
                    //g.DrawString(ID_up, new Font("宋体", 8, FontStyle.Bold), new SolidBrush(Color.White), new Point(this.Width - b[2].X , b[2].Y-10));
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
        }
        private void Onpaint(object sender, PaintEventArgs e)
        {
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void MounseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void MounseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// pwaram 1—定位 2—反位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void 道岔定位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DF_flag_up = DingFan.定位;
            DF_flag_down = DingFan.定位;
            wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
            pwaram = new IntPtr(1);
           // Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);

            //与button关联函数 事件部分
            testFlag1 = true;
            CustomEventArgs eArgs = new CustomEventArgs(testFlag1);
            OnCustom(eArgs);
        }

        private void 道岔反位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DF_flag_up = DingFan.反位;
            DF_flag_down = DingFan.反位;
            wparam = new IntPtr(Convert.ToInt32(ID_up + "0" + ID_down));
            pwaram = new IntPtr(2);
            //Send_Message.sendmessage(handle, Send_Message.Message_DC2, wparam, pwaram);
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);

            //与button关联函数 事件部分
            testFlag1 = false;
            CustomEventArgs eArgs = new CustomEventArgs(testFlag1);
            OnCustom(eArgs);
        }

        private void 下行道岔空闲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_down = STATE.空闲;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 下行道岔锁闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_down = STATE.锁闭;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 下行道岔占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_down = STATE.占用;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 上行道岔空闲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_up = STATE.空闲;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 上行道岔锁闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_up = STATE.锁闭;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 道岔锁闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_down = STATE.锁闭;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 道岔占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_down = STATE.占用;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 道岔空闲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state_up = STATE.空闲;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
        }

        private void 上行道岔占用_Click(object sender, EventArgs e)
        {
            state_up = STATE.占用;
            Drawpic(DF_flag_up, DF_flag_down, state_up, state_down);
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

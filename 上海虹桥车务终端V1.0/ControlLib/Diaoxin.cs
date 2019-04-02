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
    public partial class Diaoxin : UserControl
    {
        #region 枚举区
        public enum Dengwei
        {
            灯1,
            灯2
        }
        public enum Xianshi
        {
            红,
            黄,
            绿,
            蓝
        }
        public enum Fangwei
        {
            左边,
            右边
        }
        public enum Weizhi
        {
            置顶,
            置底
        }
        #endregion
        #region 变量区
        public int X_flag = 1;
        string ID = "XN";
        Bitmap bmp;
        int cuxi = 2;
        int width, height;
        Pen p_white;
        Fangwei fangwei = Fangwei.左边;
        Weizhi weizhi = Weizhi.置顶;
        [Browsable(true), Category("Appearance")]
        public Fangwei 方位
        {
            get { return fangwei; }
            set
            {
                fangwei = value;
                drawpic(X_flag);
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
                drawpic(X_flag);
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
        public int 信号灯状态
        {
            get { return X_flag; }
            set
            {
                X_flag = value;
                drawpic(X_flag);
            }
        }
        [Browsable(true), Category("Appearance")]
        public string ID号
        {
            get { return ID; }
            set
            {
                ID = value;
                drawpic(X_flag);
            }
        }
        #endregion
        public Diaoxin()
        {
            InitializeComponent();
            initial();
            p_white = new Pen(new SolidBrush(Color.White));
        }
        private void DiaoXin_Load(object sender, EventArgs e)
        {
            initial();
            drawpic(X_flag);
        }
        public void initial()
        {
            width = this.Width;
            height = this.Height;
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.Location = new Point(0, 0);
        }
        private void Onpaint(object sender, PaintEventArgs e)
        {
            initial();
            drawpic(X_flag);
        }
        public void drawpic()
        {
            drawpic(X_flag);
        }
        /// <summary>
        /// flag——1 蓝光 2 白光调车 
        /// </summary>
        /// <param name="flag"></param>
        public void drawpic(int flag)
        {
            initial();
            if (bmp != null)
            {
                bmp.Dispose();
            }
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            //g.Clear(Color.Red);
            Rectangle rt;
            int Fenshu = 1;
            int m = 0;
            if (flag == 1)
            {
                rt = new Rectangle(new Point(width * 0 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Blue), rt);
            }
            else if (flag == 2)
            {
                rt = new Rectangle(new Point(width * 0 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.White), rt);
            }
            else
            {
                throw new Exception("输入超出范围");
            }
            g.DrawLine(p_white, new Point(width / Fenshu * 0, m / 2), new Point(width / Fenshu * 0, height - m / 2));
            switch (fangwei)
            {
                case Fangwei.左边:
                    //g.DrawString(ID, new Font("宋体", 8), new SolidBrush(Color.White), new Point(width / Fenshu * 0+1, 0));
                    break;
                case Fangwei.右边:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    //g.DrawString(ID, new Font("宋体", 8), new SolidBrush(Color.White), new Point(width / Fenshu * 1 - 16, height - m / 2));
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
            g.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //private void 信号置为蓝光ToolStripMenuItem2_Click(object sender, EventArgs e)
        //{
        //    X_flag = 1;
        //    drawpic(X_flag);
        //}

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this.Location);
            }
        }

        private void MounseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void MounseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        //private void 信号置为白光ToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    X_flag = 2;
        //    drawpic(X_flag);
        //}

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            X_flag = 2;
            drawpic(X_flag);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            X_flag = 1;
            drawpic(X_flag);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}

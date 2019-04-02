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
    public partial class Xinhaoji2 : UserControl
    {
        #region 枚举区
        public enum Dengwei
        {
            灯1,
            灯2
        }
        public enum IDWeizhi
        {
            上,
            下
        }
        public enum Xianshi
        {
            红,
            黄,
            绿,
            白
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
        public enum Lable
        {
            高柱,
            矮柱
        }
        #endregion
        #region 变量区
        public int X_flag = 5;
        string ID = "XN";
        Bitmap bmp;
        int cunxi = 2;
        public IDWeizhi idweizhi;
        int width, height;
        Pen p_white;
        public string p_form = "";
        public string s_form = "";
        public IntPtr handle = new IntPtr(0);
        Fangwei fangwei = Fangwei.左边;
        Weizhi weizhi = Weizhi.置顶;
        Lable lable = Lable.矮柱;
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
        public IDWeizhi ID位置
        {
            get { return idweizhi; }
            set
            {
                idweizhi = value;
                drawpic(X_flag);
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
        [Browsable(true), Category("Appearance")]
        public Lable 类型
        {
            get { return lable; }
            set
            {
                lable = value;
                drawpic(X_flag);
            }
        }
        #endregion
        public Xinhaoji2()
        {
            InitializeComponent();
            initial();
            p_white = new Pen(new SolidBrush(Color.White));
        }
        private void Xinhaoji_2_Load(object sender, EventArgs e)
        {
            initial();
            drawpic();
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
            drawpic();
        }
        /// <summary>
        /// flag——1 正线停车 2 侧线停车 3 正线通过 4 调车 5 禁止 6黄绿
        /// flag——1 黄       2 双黄     3 绿       4 白   5 红   6黄绿
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
            Rectangle rt;
            int Fenshu = 6;
            int m = 0;
            for (int i = 0; i < 3; i = i + 2)
            {
                rt = new Rectangle(new Point(width * (i + 1) / Fenshu, 0 + m / 2), new Size(width / Fenshu * 2, (height - m)));
                g.FillEllipse(new SolidBrush(Color.White), rt);
                rt = new Rectangle(new Point(width * (i + 1) / Fenshu + 1, 1 + m / 2), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Black), rt);
            }
            if (flag == 1)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
            }
            else if (flag == 2)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
            }
            else if (flag == 3)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Green), rt);
            }
            else if (flag == 4)
            {
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.White), rt);
            }
            else if (flag == 5)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Red), rt);
            }
            else if (flag == 6)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Green), rt);
            }
            else
            {
                throw new Exception("输入超出范围");
            }
            if (lable == Lable.矮柱)
            {
                g.DrawLine(p_white, new Point(width / Fenshu * 1, m / 2), new Point(width / Fenshu * 1, height - m / 2));
            }
            else
            {
                g.DrawLine(p_white, new Point(width / Fenshu * 1 / 2, m / 2), new Point(width / Fenshu * 1 / 2, height - m / 2));
                g.DrawLine(p_white, new Point(width / Fenshu * 1 / 2, m / 2 + (height - m) / 2), new Point(width / Fenshu * 1, m / 2 + (height - m) / 2));
            }
            switch (fangwei)
            {
                case Fangwei.左边:
                    break;
                case Fangwei.右边:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
            g.Dispose();
        }
        public void drawpic()
        {
            initial();
            if (bmp != null)
            {
                bmp.Dispose();
            }
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            Rectangle rt;
            int Fenshu = 6;
            int m = 0;
            for (int i = 0; i < 3; i = i + 2)
            {
                rt = new Rectangle(new Point(width * (i + 1) / Fenshu, 0 + m / 2), new Size(width / Fenshu * 2, (height - m)));
                g.FillEllipse(new SolidBrush(Color.White), rt);
                rt = new Rectangle(new Point(width * (i + 1) / Fenshu + 1, 1 + m / 2), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Black), rt);
            }
            if (X_flag == 1)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
            }
            else if (X_flag == 2)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
            }
            else if (X_flag == 3)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Green), rt);
            }
            else if (X_flag == 4)
            {
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.White), rt);
            }
            else if (X_flag == 5)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Red), rt);
            }
            else if (X_flag == 6)
            {
                rt = new Rectangle(new Point(width * 1 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                rt = new Rectangle(new Point(width * 3 / Fenshu + 1, 0 + m / 2 + 1), new Size(width / Fenshu * 2 - 2, (height - m) - 2));
                g.FillEllipse(new SolidBrush(Color.Green), rt);
            }
            else
            {
                throw new Exception("输入超出范围");
            }
            if (lable == Lable.矮柱)
            {
                g.DrawLine(p_white, new Point(width / Fenshu * 1, m / 2), new Point(width / Fenshu * 1, height - m / 2));
            }
            else
            {
                g.DrawLine(p_white, new Point(width / Fenshu * 1 / 2, m / 2), new Point(width / Fenshu * 1 / 2, height - m / 2));
                g.DrawLine(p_white, new Point(width / Fenshu * 1 / 2, m / 2 + (height - m) / 2), new Point(width / Fenshu * 1, m / 2 + (height - m) / 2));
            }
            switch (fangwei)
            {
                case Fangwei.左边:
                    break;
                case Fangwei.右边:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
            g.Dispose();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        /// flag——1 正线停车 2 侧线停车 3 正线通过 4 调车 5 禁止 6黄绿
        /// flag——1 黄       2 双黄     3 绿       4 白   5 红   6黄绿
        private void 信号机置为绿光ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 3;
            drawpic();
        }

        private void 信号机置为黄光ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 1;
            drawpic();
        }

        private void 信号机置为双黄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 2;
            drawpic();
        }

        private void 信号机置为红光ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 5;
            drawpic();
        }

        private void 信号机置为白光ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 4;
            drawpic();
        }

        private void 信号机置为黄绿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            X_flag = 6;
            drawpic();
        }
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


    }
}

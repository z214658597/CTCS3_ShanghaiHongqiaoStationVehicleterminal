using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ctc
{
    public partial class Jihuatu : Form
    {
        public Jihuatu()
        {
            InitializeComponent();
        }
        public string hh = DateTime.Now.Hour.ToString();
        public string mm = DateTime.Now.Minute.ToString();
        #region 画运营图与时间线
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int line1 = 60, line3 = 560;
            Graphics gr = e.Graphics;
            Pen PenGreen = new Pen(Color.Green, 1);//绿 虚线 细
            PenGreen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;//选择线段样式
            Pen PenGreen1 = new Pen(Color.Green, 1);//绿 实线 细
            Pen PenGreen2 = new Pen(Color.Green, 2);//绿 实线 粗
            Pen PenBlack = new Pen(Color.Black, 2);//黑 实线 粗  当前时间线
            Pen PenRed1 = new Pen(Color.Red, 3);
            Pen PenRed2 = new Pen(Color.Red, 2);
            for (int i = 0; i < 145; i++)//画时分图
            {
                if (i < 3&&i!=1) gr.DrawLine(PenGreen2, 0, line1 + i * 250, 2880, line1 + i * 250);//画运行图 3条横线 75、325、575
                if (i ==1)
                {
                    gr.DrawLine(PenGreen2, 0, 265, 2880, 265);
                }
                if (i % 6 == 0)
                {
                    gr.DrawLine(PenGreen2, 1 + i * 20, line1, 1 + i * 20, line3);//画运行图实粗 竖线
                    gr.DrawLine(PenGreen2, 1 + i * 20, 10, 1 + i * 20, 15);
                    gr.DrawLine(PenGreen2, 1 + i * 20, 585, 1 + i * 20, 590);
                }
                else if (i % 3 == 0 && i % 6 != 0)
                {
                    gr.DrawLine(PenGreen1, 1 + i * 20, line1, 1 + i * 20, line3);//画运行图实细 竖线
                    gr.DrawLine(PenGreen1, 1 + i * 20, 12, 1 + i * 20, 15);
                    gr.DrawLine(PenGreen1, 1 + i * 20, 585, 1 + i * 20, 588);
                }
                else
                {
                    gr.DrawLine(PenGreen, 1 + i * 20, line1, 1 + i * 20, line3);//画运行图虚 竖线
                    gr.DrawLine(PenGreen1, 1 + i * 20, 12, 1 + i * 20, 15);
                    gr.DrawLine(PenGreen1, 1 + i * 20, 585, 1 + i * 20, 588);
                }
            }
            gr.DrawLine(PenGreen2, 0, 15, 2880, 15);//画最上方 横线
            gr.DrawLine(PenGreen2, 0, 585, 2880, 585);//画最下方 横线
            gr.DrawLine(PenBlack, int.Parse(hh) * 120 + int.Parse(mm) * 2 + 3, 20, int.Parse(hh) * 120 + int.Parse(mm) * 2 + 3, 580);//画当前时间线
            PenGreen.Dispose();
            PenGreen1.Dispose();
            PenGreen2.Dispose();
            PenBlack.Dispose();
            PenRed2.Dispose();
        }
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToLongDateString();
            string time = dt.ToLongTimeString();
            nowTime.Text = time;
        }

        private void Jihuatu_Load(object sender, EventArgs e)
        {

        }
        private void 北京南站场图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chewu C1 = new chewu();
            C1.Show();
        }
        private void 南京南站场图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chewunanjing C2 = new chewunanjing();
            C2.Show();
        }
        private void 上海虹桥站场图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chewushanghai C3 = new chewushanghai();
            C3.Show();
        }
        private void 北京南站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xingcherizhi1 X1 = new xingcherizhi1();
            X1.Show();
        }
        private void 南京南站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xingcherizhi2 X2 = new xingcherizhi2();
            X2.Show();
        }
        private void 上海虹桥站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xingcherizhi3 X3 = new xingcherizhi3();
            X3.Show();
        }
    }
}

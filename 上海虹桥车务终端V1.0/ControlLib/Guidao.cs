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
    [ToolboxBitmap(typeof(Guidao), "Icon.Guidao.bmp")]
    public partial class Guidao : UserControl
    {
        #region 属性
        public string gdname_S, gdname_X;
        public bool flag_zhanyong_S, flag_zhanyong_X;
        public bool flag_suobi_S, flag_suobi_X;
        #endregion
        #region 辅助变量
        Bitmap bmp;
        Pen p_white, p_blue;
        #endregion
        public Guidao()
        {
            InitializeComponent();
            initial();
            flag_zhanyong_S = false;
            flag_zhanyong_S = false;
            flag_zhanyong_X = false;
            flag_zhanyong_X = false;
            p_white = new Pen(new SolidBrush(Color.White), 3);
            p_blue = new Pen(new SolidBrush(Color.Blue), 3);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void initial()
        {
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.Location = new Point(0, 0);
        }

        private void Guidao_Load(object sender, EventArgs e)
        {
            Drawpic();
        }
        public void reset()
        {
            flag_suobi_S = false;
            flag_suobi_X = false;
            flag_zhanyong_S = false;
            flag_zhanyong_X = false;
            Drawpic();
        }
        public void Drawpic()
        {
            initial();
            Point p1, p2;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            p1 = new Point(0 + 1, pictureBox1.Height / 4 * 1);
            p2 = new Point(pictureBox1.Width - 1, pictureBox1.Height / 4 * 1);
            if (flag_suobi_S)
                g.DrawLine(p_white, p1, p2);
            else
                g.DrawLine(p_blue, p1, p2);
            p1 = new Point(0 + 1, pictureBox1.Height / 4 * 3);
            p2 = new Point(pictureBox1.Width - 1, pictureBox1.Height / 4 * 3);
            if (flag_suobi_X)
                g.DrawLine(p_white, p1, p2);
            else
                g.DrawLine(p_blue, p1, p2);
        }
    }
}

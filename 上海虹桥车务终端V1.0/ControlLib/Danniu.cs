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
    public partial class Danniu : UserControl
    {
        #region 枚举区
        public enum Xianshi
        {
            绿,
            红,
            黄,
            默认
        }
        #endregion
        #region 变量区
        public Xianshi xianshi = Xianshi.默认;
        [Browsable(true), Category("Appearance")]
        public Xianshi 显示状态
        {
            get { return xianshi; }
            set
            {
                xianshi = value;
                Drawpic();
            }
        }
        Bitmap bmp;
        #endregion
        public Danniu()
        {
            InitializeComponent();
            initial();
        }
        public void initial()
        {
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;
            pictureBox1.Location = new Point(0, 0);
        }

        private void Danniu_Load(object sender, EventArgs e)
        {
            initial();
            Drawpic();
        }
        public void Drawpic()
        {
            if (bmp != null)
            {
                bmp.Dispose();
            }
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            Rectangle rt = new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height));
            g.FillEllipse(new SolidBrush(Color.White), rt);
            rt = new Rectangle(new Point(1, 1), new Size(pictureBox1.Width - 2, pictureBox1.Height - 2));
            g.FillEllipse(new SolidBrush(Color.Black), rt);
            switch (xianshi)
            {
                case Xianshi.默认:
                    break;
                case Xianshi.红:
                    rt = new Rectangle(new Point(1, 1), new Size(pictureBox1.Width - 2, pictureBox1.Height - 2));
                    g.FillEllipse(new SolidBrush(Color.Red), rt);
                    break;
                case Xianshi.黄:
                    rt = new Rectangle(new Point(1, 1), new Size(pictureBox1.Width - 2, pictureBox1.Height - 2));
                    g.FillEllipse(new SolidBrush(Color.Yellow), rt);
                    break;
                case Xianshi.绿:
                    rt = new Rectangle(new Point(1, 1), new Size(pictureBox1.Width - 2, pictureBox1.Height - 2));
                    g.FillEllipse(new SolidBrush(Color.Green), rt);
                    break;
            }
            g.Save();
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Onpaint(object sender, PaintEventArgs e)
        {
            initial();
            Drawpic();
        }
    }
}

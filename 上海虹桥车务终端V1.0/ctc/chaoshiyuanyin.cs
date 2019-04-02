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
    public partial class chaoshiyuanyin : Form
    {
        public chaoshiyuanyin()
        {
            InitializeComponent();
        }
        int SelRow;
        public static string reason = "";
        private void button1_Click(object sender, EventArgs e)
        {
            reason = textBox1.Text;
            SelRow = xingcherizhi3.SelRow;
            if (chaoshiyuanyin.reason.Length != 0)
            {
                xingcherizhi3.current.dataGridView4.Rows[SelRow].Cells[0].Value = chaoshiyuanyin.reason;
            }
            this.Hide();
        }
    }
}

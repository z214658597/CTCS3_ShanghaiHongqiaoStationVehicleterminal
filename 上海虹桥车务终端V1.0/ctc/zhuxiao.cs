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
    public partial class zhuxiao : Form
    {
        public zhuxiao()
        {
            InitializeComponent();
        }
        private void Loaded_Load(object sender, EventArgs e)
        {
            label_username.Text = chewushanghai.user;
            label_number.Text = chewushanghai.number;
            label_time.Text = chewushanghai.loadtime;
        }
        private void button_注销_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("是否注销登陆？", "注销确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)//确定注销
            {
                chewushanghai.LoadStatus = false;
                this.Close();
            }
            else//放弃注销
            {
                this.Close();
            }
        }
    }
}

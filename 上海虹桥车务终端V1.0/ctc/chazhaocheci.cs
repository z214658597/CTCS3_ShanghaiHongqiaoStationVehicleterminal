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
    public partial class chazhaocheci : Form
    {
        public chazhaocheci()
        {
            InitializeComponent();
        }
        public static string cch = "";//车次号
        public static string cz = "";//车站
        public bool chazhao1 = false;//查找结果
        public bool chazhao2 = false;//查找结果
        private void 确定_Click(object sender, EventArgs e)
        {
            cch = textBox1.Text;
            if (cch.Length != 0 && cz == "北京南站")
            {
                for (int i = 0; i < xingcherizhi1.current.dataGridView2.Rows.Count - 1; i++)
                {
                    if (xingcherizhi1.current.dataGridView2.Rows[i].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi1.current.dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        chazhao1 = true;
                    }
                }
                for (int j = 0; j < xingcherizhi1.current.dataGridView3.Rows.Count - 1; j++)
                {
                    if (xingcherizhi1.current.dataGridView3.Rows[j].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi1.current.dataGridView3.Rows[j].DefaultCellStyle.BackColor = Color.Red;
                        chazhao2 = true;
                    }
                }
                if (chazhao1 == false && chazhao2 == false)
                    MessageBox.Show("车次未找到！");
                this.Hide();
            }
            else if (cch.Length != 0 && cz == "南京南站")
            {
                for (int i = 0; i < xingcherizhi2.current.dataGridView2.Rows.Count - 1; i++)
                {
                    if (xingcherizhi2.current.dataGridView2.Rows[i].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi2.current.dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        chazhao1 = true;
                    }
                }
                for (int j = 0; j < xingcherizhi2.current.dataGridView3.Rows.Count - 1; j++)
                {
                    if (xingcherizhi2.current.dataGridView3.Rows[j].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi2.current.dataGridView3.Rows[j].DefaultCellStyle.BackColor = Color.Red;
                        chazhao2 = true;
                    }
                }
                if (chazhao1 == false && chazhao2 == false)
                    MessageBox.Show("车次未找到！");
                this.Hide();
            }
            else if (cch.Length != 0 && cz == "上海虹桥站")
            {
                for (int i = 0; i < xingcherizhi3.current.dataGridView2.Rows.Count - 1; i++)
                {
                    if (xingcherizhi3.current.dataGridView2.Rows[i].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi3.current.dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        chazhao1 = true;
                    }
                }
                for (int j = 0; j < xingcherizhi3.current.dataGridView3.Rows.Count - 1; j++)
                {
                    if (xingcherizhi3.current.dataGridView3.Rows[j].Cells[0].Value.ToString() == cch)
                    {
                        xingcherizhi3.current.dataGridView3.Rows[j].DefaultCellStyle.BackColor = Color.Red;
                        chazhao2 = true;
                    }
                }
                if (chazhao1 == false && chazhao2 == false)
                    MessageBox.Show("车次未找到！");
                this.Hide();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cz = comboBox1.SelectedItem.ToString();
        }
        private void 取消_Click(object sender, EventArgs e)
        {
            cch = "";
            cz = "";
            this.Hide();
        }
    }
}
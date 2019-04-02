using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ctc
{
    public partial class xingcherizhi1 : Form
    {
        public static xingcherizhi1 current = null;
        public xingcherizhi1()
        {
            InitializeComponent();
            current = this;
        }
        #region 变量设置
        //创建MySqlConnection对象，连接MySql数据库
        MySqlCommand com = new MySqlCommand();
        MySqlConnection conn = null;
        //列表信息
        string jiefa = "";//接发车
        string fsf = "";//发送方
        string cch = "";//车次号
        string dfsj = "";//到发时间
        string fssj = "";//发送时间
        string gd = "";//股道
        //列车报点信息
        string xh = "";//序号
        string zwd = "";//早晚点类型
        string cha = "";//时间差
        string sjdfsj = "";//实际到发时间
        string senddata1 = "";//列车报点信息
        List<string> list_1 = new List<string>();//列车报点接车信息
        List<string> list_2 = new List<string>();//列车报点发车信息
        #endregion
        private void xingcherizhi_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection("Data Source=127.0.0.1;Initial Catalog=sql1;UserID=root;Password=tianba");//数据库名字，用户名，密码
            conn.Open();//调用open对象，打开对象
            if (conn.State.ToString() == "Open")//判断conn的状态是否打开
            {
                Console.WriteLine("成功");
            }
            else
            {
                Console.WriteLine("失败");
            }

            com.Connection = conn;
            com.CommandText = "SQL语句";

            this.dataGridView1.Rows[0].Cells[0].Value = chewushanghai.user;
            this.dataGridView1.Rows[0].Cells[1].Value = chewushanghai.loadtime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToLongDateString();
            string time = dt.ToLongTimeString();
            Nowtime.Text = date + time;
        }
        private void 更新列表_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Rows.Clear();
            this.dataGridView3.Rows.Clear();
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                if (dr.GetString(dr.GetOrdinal("接收方")) == "北京南")
                {
                    cch = dr.GetString(dr.GetOrdinal("车次号"));
                    车次号();
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "接车")
                    {
                        int index2 = this.dataGridView2.Rows.Add();
                        this.dataGridView2.Rows[index2].Cells[0].Value = cch;
                        this.dataGridView2.Rows[index2].Cells[1].Value = dr["接车股道"];
                        this.dataGridView2.Rows[index2].Cells[2].Value = dr["发车车站"];
                        this.dataGridView2.Rows[index2].Cells[3].Value = dr["计划发车时间"];
                        this.dataGridView2.Rows[index2].Cells[4].Value = dr["计划到达时间"];
                        this.dataGridView2.Rows[index2].Cells[5].Value = dr["实际到达时间"];
                        for (int i = 0; i < list_1.Count; i++)
                        {
                            string time = list_1[i].Substring(32, 2) + ":" + list_1[i].Substring(34, 2);
                            if (list_1[i].Substring(26, 4) == dr.GetString(dr.GetOrdinal("车次号")) && time == dr.GetString(dr.GetOrdinal("计划到达时间")))
                                this.dataGridView2.Rows[index2].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "发车")
                    {
                        int index3 = this.dataGridView3.Rows.Add();
                        this.dataGridView3.Rows[index3].Cells[0].Value = cch;
                        this.dataGridView3.Rows[index3].Cells[1].Value = dr["发车股道"];
                        this.dataGridView3.Rows[index3].Cells[2].Value = dr["到达车站"];
                        this.dataGridView3.Rows[index3].Cells[3].Value = dr["计划发车时间"];
                        this.dataGridView3.Rows[index3].Cells[4].Value = dr["实际发车时间"];
                        this.dataGridView3.Rows[index3].Cells[5].Value = dr["计划到达时间"];
                        for (int i = 0; i < list_2.Count; i++)
                        {
                            string time = list_2[i].Substring(32, 2) + ":" + list_2[i].Substring(34, 2);
                            if (list_2[i].Substring(26, 4) == dr.GetString(dr.GetOrdinal("车次号")) && time == dr.GetString(dr.GetOrdinal("计划发车时间")))
                                this.dataGridView3.Rows[index3].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
            }
            dr.Close();
        }
        private void 车次号()
        {
            for (int l = 0; l < 4; l++)
            {
                if (cch.Substring(l, 1) != "0")
                {
                    cch = "G" + cch.Substring(l, 4 - l);
                    break;
                }
            }
        }
        int m = 1;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedCells.Count != 0)
            {
                MessageBox.Show("列车报点！");
                fsf = "07";
                jiefa = "接车";
                int selRow = e.RowIndex;
                this.dataGridView2.Rows[selRow].DefaultCellStyle.BackColor = Color.Green;
                cch = dataGridView2.Rows[selRow].Cells[0].Value.ToString();
                车次号1();
                gd = "0" + dataGridView2.Rows[selRow].Cells[1].Value.ToString().Substring(0, 1);
                dfsj = dataGridView2.Rows[selRow].Cells[4].Value.ToString();
                sjdfsj = dataGridView2.Rows[selRow].Cells[5].Value.ToString();
                早晚点();
                DateTime dt = DateTime.Now;
                string date = dt.ToLongDateString();
                string time = dt.ToLongTimeString();
                fssj = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (m < 10)
                    xh = "0" + Convert.ToString(m);
                else
                    xh = Convert.ToString(m);
                if (sjdfsj.Length != 0)
                {
                    senddata1 = "AB9101" + fsf + "01" + xh + fssj + cch + gd + dfsj.Substring(0, 2) + dfsj.Substring(3, 2) + zwd + sjdfsj.Substring(0, 2) + sjdfsj.Substring(3, 2) + cha + "AC";
                    list_1.Add(senddata1);
                    ClientSendMsg(senddata1);
                    m++;
                    dataGridView2.Rows[selRow].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }
        int j = 1;
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.SelectedCells.Count != 0)
            {
                MessageBox.Show("列车报点！");
                fsf = "07";
                jiefa = "发车";
                int selRow = e.RowIndex;
                this.dataGridView3.Rows[selRow].DefaultCellStyle.BackColor = Color.Green;
                cch = dataGridView3.Rows[selRow].Cells[0].Value.ToString();
                车次号1();
                gd = "0" + dataGridView3.Rows[selRow].Cells[1].Value.ToString().Substring(0, 1);
                dfsj = dataGridView3.Rows[selRow].Cells[3].Value.ToString();
                sjdfsj = dataGridView3.Rows[selRow].Cells[4].Value.ToString();
                早晚点();
                DateTime dt = DateTime.Now;
                string date = dt.ToLongDateString();
                string time = dt.ToLongTimeString();
                fssj = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (j < 10)
                    xh = "0" + Convert.ToString(j);
                else
                    xh = Convert.ToString(j);
                if (sjdfsj.Length != 0)
                {
                    senddata1 = "AB9101" + fsf + "01" + xh + fssj + cch + gd + dfsj.Substring(0, 2) + dfsj.Substring(3, 2) + zwd + sjdfsj.Substring(0, 2) + sjdfsj.Substring(3, 2) + cha + "AC";
                    list_2.Add(senddata1);
                    ClientSendMsg(senddata1);
                    j++;
                    dataGridView3.Rows[selRow].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }
        string cch1 = "";
        private void 车次号1()
        {
            if (cch.Substring(1, cch.Length - 1).Length < 4)
            {
                for (int n = 1; n <= 4 - cch.Substring(1, cch.Length - 1).Length; n++)
                {
                    cch1 = cch1 + "0";
                }
                cch = cch1 + cch.Substring(1, cch.Length - 1);
            }
            else
                cch = cch.Substring(1, cch.Length - 1);
        }
        #region 早晚点
        private void 早晚点()
        {
            DateTime hm1 = DateTime.Parse(dfsj);
            DateTime hm2 = DateTime.Parse(sjdfsj);
            TimeSpan hm = new TimeSpan(0);
            if (hm1 > hm2)
            {
                hm = hm1 - hm2;
                if (jiefa == "接车")
                    zwd = "03";
                if (jiefa == "发车")
                    zwd = "04";
            }
            if (hm1 < hm2)
            {
                hm = hm2 - hm1;
                if (jiefa == "接车")
                    zwd = "05";
                if (jiefa == "发车")
                    zwd = "06";
            }
            if (hm1 == hm2)
            {
                if (jiefa == "接车")
                    zwd = "01";
                if (jiefa == "发车")
                    zwd = "02";
                hm = hm1 - hm2;
            }
            cha = hm.ToString().Substring(0, 2) + hm.ToString().Substring(3, 2);
        }
        #endregion
        //发送信息
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            chewushanghai.socketClient.Send(arrClientSendMsg);
        }
        #region 切换窗口
        private void 值班员登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!chewushanghai.LoadStatus)//系统未登陆
            {
                denglu D1 = new denglu();
                D1.Show();
            }
        }
        private void 值班员注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chewushanghai.LoadStatus)//系统已登陆
            {
                zhuxiao Z1 = new zhuxiao();
                Z1.Show();
            }
        }
        private void 运行图_Click(object sender, EventArgs e)
        {
            Jihuatu J1 = new Jihuatu();
            J1.Show();
        }
        private void 上行_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < dataGridView3.Rows.Count - 1; j++)
            {
                dataGridView3.Rows[j].Visible = false;
            }
        }
        private void 下行_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                dataGridView2.Rows[i].Visible = false;
            }
        }
        private void 上下行_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                dataGridView2.Rows[i].Visible = true;
            }
            for (int j = 0; j < dataGridView3.Rows.Count - 1; j++)
            {
                dataGridView3.Rows[j].Visible = true;
            }
        }
        private void 查找车次_Click(object sender, EventArgs e)
        {
            chazhaocheci C4 = new chazhaocheci();
            C4.Show();
        }
        #endregion
    }
}

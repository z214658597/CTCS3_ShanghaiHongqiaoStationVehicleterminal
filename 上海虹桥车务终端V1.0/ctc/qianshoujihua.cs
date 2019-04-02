using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ctc
{
    public partial class qianshoujihua : Form
    {
        public qianshoujihua()
        {
            InitializeComponent();
        }
        string xh = "";//序号
        string fsf = "";//发送方
        string hzsj = "";//回执时间
        string xdsj = "";//下达时间
        public static string senddata = "";//阶段计划回执
        public static MySqlCommand com = new MySqlCommand();
        public static MySqlConnection conn = null;//创建MySqlConnection对象，连接MySql数据库

        private void qianshoujihua_Load(object sender, EventArgs e)
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
            this.dataGridView1.Rows.Clear();
            MySqlDataReader dr = null; DateTime dt1 = DateTime.Parse(chewushanghai.xdsj);
            DateTime dt2 = dt1.AddSeconds(-1);
            string xdsj1 = chewushanghai.xdsj;
            string xdsj2 = dt2.ToString("yyyyMMddHHmmss");
            string xdsj3 = xdsj2.Substring(0, 4) + "." + xdsj2.Substring(4, 2) + "." + xdsj2.Substring(6, 2) + " " + xdsj2.Substring(8, 2) + ":" + xdsj2.Substring(10, 2) + ":" + xdsj2.Substring(12, 2);
            com = new MySqlCommand("SELECT*FROM jiefachejihua where 下达时间='" + xdsj1 + "' or 下达时间='" + xdsj3 + "'", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = dr["序号"];
                this.dataGridView1.Rows[index].Cells[1].Value = dr["车次号"];
                this.dataGridView1.Rows[index].Cells[2].Value = dr["接收方"];
                this.dataGridView1.Rows[index].Cells[3].Value = dr["下达时间"];
                this.dataGridView1.Rows[index].Cells[4].Value = "未签收";
            }
            dr.Close();
        }
        private void 阶段计划回执_Click(object sender, EventArgs e)
        {
            //   xingcherizhi3.current.textBox5.Text = "车务终端发送阶段计划人工回执" + "\r\n";
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                xh = this.dataGridView1.Rows[i].Cells[0].Value.ToString();
                xdsj = this.dataGridView1.Rows[i].Cells[3].Value.ToString();
                if (this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "北京南")
                {
                    fsf = "07";
                }
                if (this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "南京南")
                {
                    fsf = "08";
                }
                if (this.dataGridView1.Rows[i].Cells[2].Value.ToString() == "上海虹桥")
                {
                    fsf = "09";
                }

                DateTime dt = DateTime.Now;
                string date = dt.ToLongDateString();
                string time = dt.ToLongTimeString();
                hzsj = DateTime.Now.ToString("yyyyMMddHHmmss");
                senddata = "AB8105" + fsf + "01" + xh + xdsj.Substring(0, 4) + xdsj.Substring(5, 2) + xdsj.Substring(8, 2) + xdsj.Substring(11, 2) + xdsj.Substring(14, 2) + xdsj.Substring(17, 2) + hzsj + "01AC";
                //调用ClientSendMsg方法

                ClientSendMsg(senddata);
                this.dataGridView1.Rows[i].Cells[4].Value = "已签收";
                MessageBox.Show("已签收");
                this.Close();
            }
        }
        //发送信息
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            chewushanghai.socketClient.Send(arrClientSendMsg);
        }
    }
}

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
    public partial class qianshoumingling : Form
    {
        public qianshoumingling()
        {
            InitializeComponent();
        }
        string sldw = "";//受令单位
        string mlxh = "";//命令序号
        string mlbh = "";//命令编号
        string flsj = "";//发令时间
        public static string senddata2 = "";//调度命令回执
        public static MySqlCommand com = new MySqlCommand();
        public static MySqlConnection conn = null;//创建MySqlConnection对象，连接MySql数据库
        
        private void qianshoumingling_Load(object sender, EventArgs e)
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
            this.dataGridView2.Rows.Clear();
            textBox1.Text = "";
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM diaodumingling where 发令时间='" + chewushanghai.flsj + "'", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                int index1 = this.dataGridView2.Rows.Add();
                this.dataGridView2.Rows[index1].Cells[0].Value = dr["命令序号"];
                this.dataGridView2.Rows[index1].Cells[1].Value = dr["命令编号"];
                this.dataGridView2.Rows[index1].Cells[2].Value = dr["受令单位"];
                this.dataGridView2.Rows[index1].Cells[3].Value = "未签收";
                textBox1.Text = dr.GetString(dr.GetOrdinal("命令内容"));
            }
            dr.Close();
        }
        private void 调度命令回执_Click(object sender, EventArgs e)
        {
            xingcherizhi3.current.textBox5.Text = "车务终端发送调度命令回执" + "\r\n";
            for (int j = 0; j < this.dataGridView2.Rows.Count; j++)
            {
                mlxh = this.dataGridView2.Rows[j].Cells[0].Value.ToString();
                mlbh = this.dataGridView2.Rows[j].Cells[1].Value.ToString();
                if (this.dataGridView2.Rows[j].Cells[2].Value.ToString() == "北京南")
                {
                    sldw = "07";
                }
                if (this.dataGridView2.Rows[j].Cells[2].Value.ToString() == "南京南")
                {
                    sldw = "08";
                }
                if (this.dataGridView2.Rows[j].Cells[2].Value.ToString() == "上海虹桥")
                {
                    sldw = "09";
                }
                DateTime dt = DateTime.Now;
                string date = dt.ToLongDateString();
                string time = dt.ToLongTimeString();
                flsj = DateTime.Now.ToString("yyyyMMddHHmmss");
                senddata2 = "AB8A0301" + sldw + mlxh + mlbh + flsj + "0101AC";
                //调用ClientSendMsg方法
                ClientSendMsg(senddata2);
                this.dataGridView2.Rows[j].Cells[3].Value = "已签收";
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

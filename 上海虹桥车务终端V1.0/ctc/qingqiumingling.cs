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
    public partial class qingqiumingling : Form
    {
        #region 变量设置
        //创建MySqlConnection对象，连接MySql数据库
        public static MySqlCommand com = new MySqlCommand();
        public static MySqlConnection conn = null;
        string mllx = "";//命令类型
        string mlxh = "";//命令序号
        string senddata = "";//请求调度命令
        #endregion
        public qingqiumingling()
        {
            InitializeComponent();
        }
        private void qingqiumingling_Load(object sender, EventArgs e)
        {
            //连接数据库
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
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM qingqiumingling", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[0].Value = dr["命令序号"];
                this.dataGridView1.Rows[index].Cells[1].Value = dr["命令类型"];
                this.dataGridView1.Rows[index].Cells[2].Value = dr["命令编号"];
                this.dataGridView1.Rows[index].Cells[3].Value = dr["请求结果"];
            }
            dr.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mllx = comboBox1.SelectedItem.ToString();
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM cunchumingling where 类型='" + mllx + "'", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                textBox1.Text = dr["名称"].ToString();
                textBox2.Text = dr["命令正文"].ToString();
            }
            dr.Close();
        }
        private void 发送请求_Click(object sender, EventArgs e)
        {
            chewushanghai.mlxh1 = chewushanghai.mlxh1 + 1;
            if (chewushanghai.mlxh1 < 10)
                mlxh = "0" + Convert.ToString(chewushanghai.mlxh1);
            else
                mlxh = Convert.ToString(chewushanghai.mlxh1);
            senddata = "AB8A040901" + mlxh + "0000" + mllx + textBox2.Text + "00AC";
            ClientSendMsg(senddata);
            string InsertSql = "insert into qingqiumingling(命令序号,命令类型)values('" + mlxh + "','" + mllx + "')";
            com = new MySqlCommand(InsertSql, conn);
            com.ExecuteNonQuery();
            this.Hide();
        }
        //发送信息
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            chewushanghai.socketClient.Send(arrClientSendMsg);
        }
        private void 取消_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

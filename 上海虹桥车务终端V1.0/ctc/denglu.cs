using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ctc
{
    public partial class denglu : Form
    {
        public denglu()
        {
            InitializeComponent();
        }
        //创建MySqlConnection对象，连接MySql数据库
        MySqlConnection conn = null;
        MySqlCommand com = new MySqlCommand();
        private void denglu_Load(object sender, EventArgs e)
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
        }
        private void button_登录_Click(object sender, EventArgs e)
        {
            com = new MySqlCommand("select * from login where number='" + textBox_number.Text + "'and username='" + textBox_name.Text + "'and password='" + textBox_password.Text + "'",conn);
            MySqlDataReader dr = null;
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                chewushanghai.user = textBox_name.Text;
                chewushanghai.number = textBox_number.Text;
                chewushanghai.loadtime = DateTime.Now.ToString("HH:mm:ss");
                chewushanghai.LoadStatus = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("员工信息错误！");
            }
        }
    }
}

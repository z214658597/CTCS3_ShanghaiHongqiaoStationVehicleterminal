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
    public partial class xingcherizhi : Form
    {
        public xingcherizhi()
        {
            InitializeComponent();
        }

        private void xingcherizhi_Load(object sender, EventArgs e)
        {
        }
        //用以提取数据库数据
        public string shangxiaxing="";
        public string GD = "";
        public string jiefa = "";
        public string JLAN = "";
        private void 检查网络状态_Tick(object sender, EventArgs e)
        {
            #region 与数据库相连并提取数据
            //连接数据库
            MySqlConnection conn = null;//创建MySqlConnection对象，连接MySql数据库
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
            MySqlCommand com = new MySqlCommand();
            com.Connection = conn;
            com.CommandText = "SQL语句";
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                //string[] id = new string[]{};
                //dr.GetOrdinal("id")是得到ID所在列的index,  
                //dr.GetInt32(int n)这是将第n列的数据以Int32的格式返回  
                //dr.GetString(int n)这是将第n列的数据以string 格式返回  
                //string id = dr.GetString(dr.GetOrdinal("车次号"));
                //string TIME = dr.GetString(dr.GetOrdinal("计划到开时间"));
                //int age = reader.GetInt32(reader.GetOrdinal("age"));
                if (dr.GetString(dr.GetOrdinal("发车车站")) == "北京南")
                {
                    jiefa = "发车";
                    shangxiaxing = "下行";
                    GD = dr.GetString(dr.GetOrdinal("发车股道"));
                }
                if (dr.GetString(dr.GetOrdinal("到达车站")) == "北京南")
                {
                    jiefa = "接车";
                    shangxiaxing = "上行";
                    GD = dr.GetString(dr.GetOrdinal("接车股道"));
                }
                //Console.WriteLine(jiefa);
                //string address = reader.GetString(reader.GetOrdinal("Address"));  
                // dr["车次号"]
                
                //Console.WriteLine(dr["车次号"]);
                //DataGridViewRow Row = new DataGridViewRow();

                if (jiefa.Contains("接车"))
                    {
                        int index2 = this.dataGridView2.Rows.Add();
                        this.dataGridView2.Rows[index2].Cells[0].Value = dr["车次号"];
                        this.dataGridView2.Rows[index2].Cells[1].Value = dr["接车股道"];
                        this.dataGridView2.Rows[index2].Cells[2].Value = dr["发车车站"];
                        this.dataGridView2.Rows[index2].Cells[3].Value = dr["计划发车时间"];
                        this.dataGridView2.Rows[index2].Cells[4].Value = dr["计划到达时间"];
                        this.dataGridView2.Rows[index2].Cells[5].Value = dr["实际到达时间"];
                        for (int i = 1; i < dataGridView2.Rows.Count; i++)
                        {
                            DataGridViewRow row2 = this.dataGridView2.Rows[i];
                            if (dr["车次号"] == this.dataGridView2.Rows[i].Cells[0].Value)
                                this.dataGridView2.Rows.Remove(row2);
                        }
                    }
                if (jiefa.Contains("发车"))
                    {
                        int index3 = this.dataGridView3.Rows.Add();
                        this.dataGridView3.Rows[index3].Cells[0].Value = dr["车次号"];
                        this.dataGridView3.Rows[index3].Cells[1].Value = dr["到达车站"];
                        this.dataGridView3.Rows[index3].Cells[2].Value = dr["计划发车时间"];
                        this.dataGridView3.Rows[index3].Cells[3].Value = dr["实际发车时间"];
                        this.dataGridView3.Rows[index3].Cells[4].Value = dr["计划到达时间"];
                        for (int i = 1; i < dataGridView3.Rows.Count; i++)
                        {
                            DataGridViewRow row3 = this.dataGridView3.Rows[i];
                            if (dr["车次号"] == this.dataGridView3.Rows[i].Cells[0].Value)
                                this.dataGridView3.Rows.Remove(row3);
                        }
                    }
            }
            #endregion
            检查网络状态.Enabled = false;
          
            this.dataGridView1.Rows[0].Cells[0].Value = "值班人";
            this.dataGridView1.Rows[0].Cells[1].Value = chewu.user;
            //this.dataGridView2.Rows[index].Cells[0].Value = "G232";
            //this.dataGridView2.Rows[index].Cells[1].Value = "IG";
            //this.dataGridView2.Rows[index].Cells[2].Value = "上海虹桥站";
            //this.dataGridView2.Rows[index].Cells[3].Value = "08：36";
            //this.dataGridView2.Rows[index].Cells[4].Value = "16：50";
            //this.dataGridView2.Rows[index].Cells[5].Value = "通过";
            //dataGridView2.Rows.Add();
            //this.dataGridView2.Rows[1].Cells[0].Value = "G44";
            //this.dataGridView2.Rows[1].Cells[1].Value = "IIG";
            //this.dataGridView2.Rows[1].Cells[2].Value = "济南西站";
            //this.dataGridView2.Rows[1].Cells[3].Value = "10：00";
            //this.dataGridView2.Rows[1].Cells[4].Value = "12：00";
            //this.dataGridView2.Rows[1].Cells[5].Value = "通过";
            //dataGridView2.Rows.Add();
            //this.dataGridView2.Rows[2].Cells[0].Value = "G30";
            //this.dataGridView2.Rows[2].Cells[1].Value = "4G";
            //this.dataGridView2.Rows[2].Cells[2].Value = "济南西站";
            //this.dataGridView2.Rows[2].Cells[3].Value = "14：00";
            //this.dataGridView2.Rows[2].Cells[4].Value = "16：00";
            //this.dataGridView2.Rows[2].Cells[5].Value = "通过";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string date = dt.ToLongDateString();
            string time = dt.ToLongTimeString();
            Nowtime.Text = date+time;
            //Nowtime.Text = "2016年3月29日" + "20:01";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using MySql.Data.MySqlClient;
using ControlLib;

namespace ctc
{
    public partial class chewunanjing : Form
    {
        #region 链表——存储站场布置数据
        List<string> list_cz = new List<string>();              //链表按键
        List<string> list_lsb_ed = new List<string>();          //链表联锁表
        List<Image> list_czbmp = new List<Image>();             //链表按键图片
        List<Xinhaoji2> list_XHJX = new List<Xinhaoji2>();    //链表信号机下行
        List<Xinhaoji2> list_XHJS = new List<Xinhaoji2>();    //链表信号机上行
        List<Dangui> list_SGD = new List<Dangui>();             //链表上行轨道
        List<Dangui> list_XGD = new List<Dangui>();             //链表下行轨道
        List<Thread> list_thread = new List<Thread>();          //链表线程
        List<Dangui> list_qjxx = new List<Dangui>();            //链表区间占用信息
        List<string> list_sx = new List<string>();              //链表走车顺序
        //List<ValueType> list_shujuku = new List<ValueType>();         //链表数据库内容

        Hashtable ht_zc = new Hashtable();      //哈希表战场
        Hashtable ht_bt = new Hashtable();      //哈希表按键
        Hashtable ht_DC_1 = new Hashtable();    //哈希表单动道岔
        Hashtable ht_DC_2 = new Hashtable();    //哈希表双动道岔
        //Hashtable ht_qjxx = new Hashtable();    //哈希表区间信息
        Hashtable ht_lsb = new Hashtable();     //哈希表联锁表
        Hashtable ht_jlfcxx = new Hashtable();  //哈希表进路发车信息
        Hashtable ht_jljcxx = new Hashtable();  //哈希表进路接车信息
        Hashtable ht_lcjl = new Hashtable();    //哈希表列车进路信息
        Hashtable ht_jlxx = new Hashtable();    //哈希表进路信息
        Hashtable ht_lclsxx = new Hashtable();  //哈希表列车联锁信息
        Hashtable ht_lcgdxx = new Hashtable();  //哈希表列车股道信息
        #endregion
        #region 变量设置
        public static MySqlCommand com = new MySqlCommand();
        public static MySqlConnection conn = null;//创建MySqlConnection对象，连接MySql数据库
        public string fcsj = "";//发车时间
        public string ddsj = "";//到达时间
        public static string zcsj = "";//走车时间
        public string sa = "";            //区间占用信息
        public string jcplwch = "";       //接车排列完成
        public string fcplwch = "";       //发车排列完成
        #endregion
        public chewunanjing()
        {
            InitializeComponent();
        }
        #region 初始化函数
        private void chewu_Load(object sender, EventArgs e)
        {
            ZHCHCSH();  //战场初始化
            QJCSH();    //区间初始化
            LSCSH();    //联锁表初始化
            
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

        //从小到大
        private static int ComparebyNameS(Control t1, Control t2)
        {
            return t1.Name.CompareTo(t2.Name);
        }
        //从大到小
        private static int ComparebyNameX(Control t1, Control t2)
        {
            return t2.Name.CompareTo(t1.Name);
        }

        /// <summary>
        /// 站场初始化函数
        /// </summary>
        private void ZHCHCSH()
        {
            ht_zc.Clear();
            list_cz.Clear();
            foreach (Control ct in this.Controls)
            {
                if (ct.Name.Contains("dangui") || ct.Name.Contains("G") || ct.Name.Contains("xinhaoji") || ct.Name.Contains("daocha") || ct.Name.Contains("LQ"))
                {
                    ht_zc.Add(ct.Name, ct);
                }
            }
        }
        /// <summary>
        /// 区间初始化函数
        /// </summary>
        private void QJCSH()
        {
            ht_DC_1.Clear();
            ht_DC_2.Clear();
            list_XHJX.Clear();
            list_XHJS.Clear();
            list_SGD.Clear();
            list_XGD.Clear();
            ht_bt.Clear();

            foreach (Control ct in this.Controls)
            {
                //轨道初始化
                if (ct.Name.Contains("dangui") || ct.Name.Contains("G") || ct.Name.Contains("LQ"))
                {
                    Dangui dg = (Dangui)ct;
                    string[] str = dg.Name.Split('_');
                    dg.ID = dg.Name;
                    dg.p_form = this.Text;
                    dg.handle = this.Handle;
                    //dg.wparam = new IntPtr(Convert.ToInt32(str[1]));
                    //if (Convert.ToInt16(str[1]) % 2 == 0)       //双数上行股道
                    //{
                    //    list_SGD.Add(dg);
                    //}
                    //else                                        //单数下行股道
                    //{
                    //    list_XGD.Add(dg);
                    //}
                }

                //信号机初始化
                else if (ct.Name.Contains("xinhaoji"))
                {
                    Xinhaoji2 xhj = (Xinhaoji2)ct;
                    string[] str = xhj.Name.Split('_');
                    xhj.ID号 = xhj.Name;
                    //xhj.p_form = this.MdiParent.Text;
                    //xhj.s_form = this.Text;
                    //xhj.handle = this.Handle;
                    //if (Convert.ToInt16(str[2]) % 2 == 0)
                    //{
                    //    list_XHJS.Add(xhj);
                    //}
                    //else
                    //{
                    //    list_XHJX.Add(xhj);
                    //}
                }

                //1类道岔初始化
                else if (ct.Name.Contains("daocha_1"))
                {
                    Daocha_1 dc1 = (Daocha_1)ct;
                    string[] str = dc1.Name.Split('_');
                    dc1.ID号 = str[2];
                    dc1.handle = this.Handle;
                    //list_DC1.Add(dc1);
                    ht_DC_1.Add(dc1.Name, dc1);
                }

                //按钮的初始化
                else if (ct.Name.Contains("button"))
                {
                    ht_bt.Add(ct.Name, ct);
                }
            }
            list_SGD.Sort(ComparebyNameS);
            list_XGD.Sort(ComparebyNameX);
            list_XHJS.Sort(ComparebyNameS);
            list_XHJX.Sort(ComparebyNameX);
            list_qjxx.Clear();

        }
        /// <summary>
        /// 联锁表初始化
        /// 自动搜索进路、人工填写进路联锁表
        /// value协议 信号机#道岔%道岔#轨道%轨道
        /// 
        /// 取消进路已经删除部分股道
        /// </summary>
        private void LSCSH()
        {
            list_lsb_ed.Clear();
            ht_lsb.Clear();
            #region 正常进路联锁表
            //下行
            //@1下行3G侧线接车
            string key = "button_XJ+button_S3+button_bljl";
            string value = "xinhaoji_X|UU#daocha_1_1|F|S#X1JG|S%X2JG|S%IAG|S%G3|S";
            ht_lsb.Add(key, value);
            //下行3G侧线接车进路取消
            key = "button_XJ+button_S3+button_zqx";
            value = "xinhaoji_X|H#daocha_1_1|F|K#X1JG|K%X2JG|K%IAG|K%G3|K";
            ht_lsb.Add(key, value);
            //@2下行3G发车
            key = "button_X3+button_SFJ+button_bljl";
            value = "xinhaoji_X3|L#daocha_1_2|F|S#X2LQ|S%X3LQ|S%IBG|S";
            ht_lsb.Add(key, value);
            //下行3G发车进路取消
            key = "button_X3+button_SFJ+button_zqx";
            value = "xinhaoji_X3|H#daocha_1_2|F|K#X2LQ|K%X3LQ|K%IBG|K";
            ht_lsb.Add(key, value);
            //@3下行1G正线接车
            key = "button_XJ+button_S1+button_bljl";
            value = "xinhaoji_X|U#daocha_1_1|D|S#X1JG|S%X2JG|S%IAG|S%G1|S";
            ht_lsb.Add(key, value);
            //下行1G正线接车进路取消
            key = "button_XJ+button_S1+button_zqx";
            value = "xinhaoji_X|H#daocha_1_1|D|K#X1JG|K%X2JG|K%IAG|K%G1|K";
            ht_lsb.Add(key, value);
            //@4下行1G正线发车
            key = "button_X1+button_SFJ+button_bljl";
            value = "xinhaoji_X1|L#daocha_1_2|D|S#X2LQ|S%X3LQ|S%IBG|S";
            ht_lsb.Add(key, value);
            //下行1G正线发车进路取消
            key = "button_X1+button_SFJ+button_zqx";
            value = "xinhaoji_X1|H#daocha_1_2|D|K#X2LQ|K%X3LQ|K%IBG|K";
            ht_lsb.Add(key, value);
            //@5下行2G正线接车
            key = "button_XFJ+button_S2+button_bljl";
            value = "xinhaoji_XF|U#daocha_1_3|D|S#S3LQ|S%S2LQ|S%IIAG|S%G2|S";
            ht_lsb.Add(key, value);
            //下行2G正线接车进路取消
            key = "button_XFJ+button_S2+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_3|D|K#S3LQ|K%S2LQ|K%IIAG|K%G2|K";
            ht_lsb.Add(key, value);
            //@6下行2G正线发车
            key = "button_X2+button_SJ+button_bljl";
            value = "xinhaoji_X2|L#daocha_1_4|D|S#S2JG|S%S1JG|S%IIBG|S";
            ht_lsb.Add(key, value);
            //下行2G正线发车进路取消
            key = "button_X2+button_SJ+button_zqx";
            value = "xinhaoji_X2|H#daocha_1_4|D|K#S2JG|K%S1JG|K%IIBG|K";
            ht_lsb.Add(key, value);
            //@7下行4G侧线接车
            key = "button_XFJ+button_S4+button_bljl";
            value = "xinhaoji_XF|UU#daocha_1_4|F|S#S3LQ|S%S2LQ|S%IIAG|S%G4|S";
            ht_lsb.Add(key, value);
            //下行4G侧线接车进路取消
            key = "button_XFJ+button_S4+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_4|F|K#S3LQ|K%S2LQ|K%IIAG|K%G4|K";
            ht_lsb.Add(key, value);
            //@8下行4G侧线发车
            key = "button_X4+button_SJ+button_bljl";
            value = "xinhaoji_X4|L#daocha_1_4|F|S#S2JG|S%S1JG|S%IIBG|S";
            ht_lsb.Add(key, value);
            //下行4G侧线发车进路取消
            key = "button_X4+button_SJ+button_zqx";
            value = "xinhaoji_X4|H#daocha_1_4|F|K#S2JG|K%S1JG|K%IIBG|K";
            ht_lsb.Add(key, value);
            //////////////////////////////////////////////////////////////////////////////////////////
            //上行
            //@1上行3G侧线接车
            key = "button_SFJ+button_X3+button_bljl";
            value = "xinhaoji_SF|UU#daocha_1_2|F|S#X2LQ|S%X3LQ|S%IBG|S%G3|S";
            ht_lsb.Add(key, value);
            //上行3G侧线接车进路取消
            key = "button_SFJ+button_X3+button_zqx";
            value = "xinhaoji_SF|H#daocha_1_2|F|K#X2LQ|K%X3LQ|K%IBG|K%G3|K";
            ht_lsb.Add(key, value);
            //@2上行3G侧线发车
            key = "button_S3+button_XJ+button_bljl";
            value = "xinhaoji_S3|L#daocha_1_1|F|S#X1JG|S%X2JG|S%IAG|S";
            ht_lsb.Add(key, value);
            //上行3G侧线发车进路取消
            key = "button_S3+button_XJ+button_zqx";
            value = "xinhaoji_S3|H#daocha_1_1|F|K#X1JG|K%X2JG|K%IAG|K";
            ht_lsb.Add(key, value);
            //@3上行1G正线接车
            key = "button_SFJ+button_X1+button_bljl";
            value = "xinhaoji_SF|U#daocha_1_2|D|S#X2LQ|S%X3LQ|S%IBG|S%G1|S";
            ht_lsb.Add(key, value);
            //上行1G正线接车进路取消
            key = "button_SFJ+button_X1+button_zqx";
            value = "xinhaoji_SF|H#daocha_1_2|D|K#X2LQ|K%X3LQ|K%IBG|K%G1|K";
            ht_lsb.Add(key, value);
            //@4上行1G正线发车
            key = "button_S1+button_XJ+button_bljl";
            value = "xinhaoji_S1|L#daocha_1_1|D|S#X1JG|S%X2JG|S%IAG|S";
            ht_lsb.Add(key, value);
            //上行1G正线发车进路取消
            key = "button_S1+button_XJ+button_zqx";
            value = "xinhaoji_S1|H#daocha_1_1|D|K#X1JG|K%X2JG|K%IAG|K";
            ht_lsb.Add(key, value);
            //@5上行2G正线接车
            key = "button_SJ+button_X2+button_bljl";
            value = "xinhaoji_S|U#daocha_1_4|D|S#S2JG|S%S1JG|S%IIBG|S%G2|S";
            ht_lsb.Add(key, value);
            //上行2G正线接车进路取消
            key = "button_SJ+button_X2+button_zqx";
            value = "xinhaoji_S|H#daocha_1_4|D|K#S2JG|K%S1JG|K%IIBG|K%G2|K";
            ht_lsb.Add(key, value);
            //@6上行2G正线发车
            key = "button_S2+button_XFJ+button_bljl";
            value = "xinhaoji_S2|L#daocha_1_3|D|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行2G正线发车进路取消
            key = "button_S2+button_XFJ+button_zqx";
            value = "xinhaoji_S2|H#daocha_1_3|D|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@7上行4G侧线接车
            key = "button_SJ+button_X4+button_bljl";
            value = "xinhaoji_S|UU#daocha_1_4|F|S#S2JG|S%S1JG|S%IIBG|S%G4|S";
            ht_lsb.Add(key, value);
            //上行4G侧线接车进路取消
            key = "button_SJ+button_X4+button_zqx";
            value = "xinhaoji_S|H#daocha_1_4|F|K#S2JG|K%S1JG|K%IIBG|K%G4|K";
            ht_lsb.Add(key, value);
            //@8上行4G侧线发车
            key = "button_S4+button_XFJ+button_bljl";
            value = "xinhaoji_S4|L#daocha_1_3|F|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行4G侧线发车进路取消
            key = "button_S4+button_XFJ+button_zqx";
            value = "xinhaoji_S4|H#daocha_1_3|F|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            #endregion
        }
        #endregion
        #region 按键信息处理函数
        //办理进路，取消进路信息处理
        //单机按键事件处理
        #region button单击事件
        private void button_CZ_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)(e)).Button == MouseButtons.Left)
            {
                string str = ((Button)(sender)).Name;
                if (list_cz.Contains(str))
                {
                    MessageBox.Show("已选中");
                }
                else
                {
                    if (((Button)(sender)).Enabled == true)
                    {
                        ((Button)(sender)).Enabled = false;
                    }
                    list_cz.Add(((Button)(sender)).Name);
                    Image bt = (Image)((Button)sender).BackgroundImage.Clone();
                    list_czbmp.Add(bt);
                    Graphics g = Graphics.FromImage(((Button)(sender)).BackgroundImage);
                    g.Clear(Color.Red);
                    g.Save();
                    ((Button)(sender)).Refresh();
                }
            }
        }
        #endregion
        //按键信息清除
        #region 清除键事件
        private void button_clear_Click(object sender, EventArgs e)
        {
            //按钮恢复
            int count = 0;
            foreach (string st in list_cz)
            {
                if (st != ((Button)sender).Name)
                {
                    foreach (Control ct in Controls)
                    {
                        if (ct.Name == st)
                        {
                            ((Button)ct).Enabled = true;
                            ((Button)ct).BackgroundImage = list_czbmp[count];
                            ((Button)ct).Refresh();
                        }
                    }
                }
                count++;
            }
            list_czbmp.Clear();
            list_cz.Clear();
        }
        #endregion
        /// <summary>
        /// 办理进路按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void button_bljl_Click(object sender, EventArgs e)
        {
            czzx(sender);
            if (ZCXL[0] != null)
            {
                for (int i1 = 0; i1 < dataGridView1.Rows.Count; i1++)
                {
                    //列车进路等待出发状态修改为排列完成
                    if (ZCXL[0].Contains("xinhaoji_S"))
                    {
                        if (this.dataGridView1.Rows[i1].Cells[0].Value != null)
                        {
                            this.dataGridView1.Rows[i1].Cells[5].Value = "排列完成";
                            this.dataGridView1.Rows[i1].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
                for (int i2 = 0; i2 < dataGridView2.Rows.Count; i2++)
                {
                    if (ZCXL[0].Contains("xinhaoji_X"))
                    {
                        if (this.dataGridView2.Rows[i2].Cells[0].Value != null)
                        {
                            this.dataGridView2.Rows[i2].Cells[5].Value = "排列完成";
                            this.dataGridView2.Rows[i2].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
            }
        }

        private void button_zqx_Click(object sender, EventArgs e)
        {
            czzx(sender);
        }
        public string sx= "";
        public void czzx(object sender)
        {
            list_cz.Add(((Button)sender).Name);
              
             string cz=""; 
            foreach (string str in list_cz)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
                sx = cz;
            }

              if (ht_lsb.Contains(cz))
            {
                //将联锁表信息分离 信号灯#道岔#股道
                string info = (string)(ht_lsb[cz]);
                string first_button = cz.Split('+')[0];
                string xhj = info.Split('#')[0];
                string dc = info.Split('#')[1];
                string gd = info.Split('#')[2];

                  if (jinlu_zy_jc(info,sender))
                {
                    if (jinlu_function(info, sender))
                    {
                    }
                    else
                    {
                        MessageBox.Show("进路办理失败");
                    }
                }
                  else
                  {
                      MessageBox.Show("进路被占用");
                  }
                  dingfanweijiance(dc);
            }
              else
              {
                  MessageBox.Show("操作不合法");
              }
              zoucheshunxushibie(sx);
              #region   按钮恢复
              int count = 0;
              foreach (string st in list_cz)
              {
                  if (st != ((Button)sender).Name)
                  {
                      foreach (Control ct in Controls)
                      {
                          if (ct.Name == st)
                          {
                              ((Button)ct).Enabled = true;
                              ((Button)ct).BackgroundImage = list_czbmp[count];
                              ((Button)ct).Refresh();
                          }
                      }
                  }
                  count++;
              }
              list_czbmp.Clear();
              list_cz.Clear();
              #endregion
        }
        #endregion
        #region  走车顺序辨别、走车
         string[] ZCXL = new string[100];//走车序列
         int j = 0;
         public void zoucheshunxushibie(string sx)
         {
             if (ht_lsb.Contains(sx))
             {
                 //将联锁表信息分离 信号灯#道岔#股道
                 string info = (string)(ht_lsb[sx]);
                 string first_button = sx.Split('+')[0];
                 //string xhj = info.Split('#')[0];
                 //string dc = info.Split('#')[1];
                 //string gd = info.Split('#')[2];

                 #region 判断下行，排序
                 //根据第一个键判断上下行
                 if (first_button == "button_XJ" || first_button == "button_XFJ" || first_button == "button_X1" || first_button == "button_X2" || first_button == "button_X3" || first_button == "button_X4")
                 {
                     //判断为下行线，控件横坐标递增顺序
                     string[] kongjian = info.Split(new char[3] { '|', '#', '%' });//通过这3个符合切割字符串

                     for (int i = 0; i < kongjian.Length; i++)
                     {
                         if (kongjian[i].Contains("xin") || kongjian[i].Contains("dao") || kongjian[i].Contains("G") || kongjian[i].Contains("dangui") || kongjian[i].Contains("LQ"))
                         {
                             ZCXL[j] = kongjian[i];
                             list_sx.Add(ZCXL[j]);
                             j++;
                         }
                     }
                     //ZCXL =ZCXL.ToList().Where(x => !x.ToString().Equals("")).ToArray();
                     int a, b;
                     string temp;
                     b = 1;
                     bool done = false;
                     while ((b < ZCXL.Length) && (!done))
                     {
                         done = true;
                         for (a = 1; a < ZCXL.Length - b; a++)
                         {
                             if (ZCXL[a + 1] != null)
                             {
                                 if (((Control)ht_zc[ZCXL[a]]).Location.X > ((Control)ht_zc[ZCXL[a + 1]]).Location.X)
                                 {
                                     done = false;
                                     temp = ZCXL[a];
                                     ZCXL[a] = ZCXL[a + 1];
                                     ZCXL[a + 1] = temp;
                                 }
                                 b++;
                             }
                         }
                     }
                     //((Control)ht_zc[ZCXL[1]]).Location.X
                 }
                 #endregion
                 #region 判断上行，排序
                 //根据第一个键判断上行
                 else if (first_button == "button_SJ" || first_button == "button_SFJ" || first_button == "button_S1" || first_button == "button_S2" || first_button == "button_S3" || first_button == "button_S4")
                 {
                     //判断为上行线，控件横坐标递减顺序
                     string[] kongjian = info.Split(new char[3] { '|', '#', '%' });//通过这3个符合切割字符串

                     for (int i = 0; i < kongjian.Length; i++)
                     {
                         if (kongjian[i].Contains("xin") || kongjian[i].Contains("dao") || kongjian[i].Contains("G") || kongjian[i].Contains("dangui") || kongjian[i].Contains("LQ"))
                         {
                             ZCXL[j] = kongjian[i];
                             j++;
                         }
                     }
                     int a, b;
                     string temp;
                     b = 1;
                     bool done = false;
                     while ((b < ZCXL.Length) && (!done))
                     {
                         done = true;
                         for (a = 1; a < ZCXL.Length - b; a++)
                         {
                             if (ZCXL[a + 1] != null)
                             {
                                 if (((Control)ht_zc[ZCXL[a]]).Location.X < ((Control)ht_zc[ZCXL[a + 1]]).Location.X)
                                 {
                                     done = false;
                                     temp = ZCXL[a];
                                     ZCXL[a] = ZCXL[a + 1];
                                     ZCXL[a + 1] = temp;
                                 }
                                 b++;
                             }
                         }
                     }
                 }
                 #endregion
             }
         }
        #endregion
        #region 监测定反位方法 与button关联第二部分
         public void dingfanweijiance(string dc)
         { 
         if (dc != "")
            {
                string[] dcs = dc.Split('%');
                foreach (string d in dcs)
                {
                    if (d.Contains("daocha_1_1"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha1.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha1.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha1.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha1.Refresh();
                        }
                    }
                    if (d.Contains("daocha_1_2"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha2.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha2.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha2.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha2.Refresh();
                        }
                    }
                    if (d.Contains("daocha_1_3"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha3.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha3.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha3.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha3.Refresh();
                        }
                    } 
                    if (d.Contains("daocha_1_4"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha4.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha4.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha4.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha4.Refresh();
                        }
                    }
                }
            }
         }
        #endregion
        #region 进路处理
         //进路处理函数
         /*
         进路办理过程
          * 检查列车股道是否有占用
          * 转辙机办理
          * 检查转辙机位置
          * 开放信号
          */
         #region 进路检测
         private bool jinlu_zy_jc(string info, object sender)
        {
            string xhj = info.Split('#')[0];
            string dc = info.Split('#')[1];
            string gd = info.Split('#')[2];

            if(gd!="")
            {
                string[] gds = gd.Split('%');
                foreach (string d in gds)
                {
                    string name = d.Split('|')[0];
                    if (((Dangui)ht_zc[name]).flag_zt == 1)
                    {
                        return false;
                    }
                    else
                    { 
                    }
                }
            }
             if (dc != "")
            {
                 string[] dcs = dc.Split('%');
                foreach (string d in dcs)
                {
                    if (d.Contains("daocha_1"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];

                        if (((Daocha_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1.STATE.占用)
                        {
                            return false;
                        }
                    }
                }
            }
             return true;
        }
         #endregion
        #endregion
        #region 进路标记
         private bool jinlu_biaoji(string xhj, string dc, string gd)
        {
            //道岔显示
            #region 道岔显示
            if (dc != "")
            {
                string[] dcs = dc.Split('%');
                #region 道岔1标记
                foreach (string d in dcs)
                {                   
                    if (d.Contains("daocha_1"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        switch (df)
                        {
                            case "D":
                                if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                }
                                else
                                {
                                    ((Daocha_1)ht_DC_1[name]).定反位 = ControlLib.Daocha_1.DingFan.定位;
                                }
                               // jinlu_dc_xianshi(name, df);
                                //Show_nanzhan();
                                if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由反位转换为定位");
                                    return false;
                                }
                                break;
                            case "F":
                                if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.反位)
                                {
                                }
                                else
                                {
                                    ((Daocha_1)ht_DC_1[name]).定反位 = ControlLib.Daocha_1.DingFan.反位;
                                }
                               // jinlu_dc_xianshi(name, df);
                                //Show_nanzhan();
                                if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.反位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由定位转换为反位");
                                    return false;
                                }
                                break;
                        }
                    }
                }
                foreach (string d in dcs)
                {
                     if (d.Contains("daocha_1"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (zt == "S")
                        {
                            if (((Daocha_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1.STATE.锁闭)
                            {
                            }
                            else
                            {
                                ((Daocha_1)ht_DC_1[name]).锁闭状态 = ControlLib.Daocha_1.STATE.锁闭;
                            }
                        }
                        else if (zt == "Z")
                        {
                            if (((Daocha_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1.STATE.占用)
                            {
                            }
                            else
                            {
                                ((Daocha_1)ht_DC_1[name]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                            }
                        }
                        else if (zt == "K")
                        {
                            if (((Daocha_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1.STATE.空闲)
                            {
                            }
                            else
                            {
                                //((Daocha_1)ht_DC_1[name]).锁闭状态 = DaoCha.Daocha_1.STATE.空闲;
                                ((Daocha_1)ht_DC_1[name]).锁闭状态 = Daocha_1.STATE.空闲;
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            //轨道显示
            #region 轨道显示
            if (gd != "")
            {
                string[] gds = gd.Split('%');
                foreach (string d in gds)
                {
                    string name = d.Split('|')[0];
                    string zt = d.Split('|')[1];
                    if (zt == "S")
                    {
                        if (((Dangui)ht_zc[name]).flag_zt == 2)
                        {
                        }
                        else
                        {
                            ((Dangui)ht_zc[name]).flag_zt = 2;
                            ((Dangui)ht_zc[name]).Drawpic();
                        }

                    }

                    else if (zt == "Z")
                    {
                        if (((Dangui)ht_zc[name]).flag_zt == 1)
                        {
                        }
                        else
                        {
                            ((Dangui)ht_zc[name]).flag_zt = 1;
                            ((Dangui)ht_zc[name]).Drawpic();
                        }
                    }

                    else if (zt == "K")
                    {
                        if (((Dangui)ht_zc[name]).flag_zt == 3)
                        {
                        }
                        else
                        {
                            ((Dangui)ht_zc[name]).flag_zt = 3;
                            ((Dangui)ht_zc[name]).Drawpic();
                        }
                    }
                }
            }
            #endregion

            //信号机名称不为空
            #region 信号机显示
            if (xhj != "")
            {
                string[] xhjmz = xhj.Split('%');
                foreach (string x in xhjmz)
                {
                    string xhjname = x.Split('|')[0];
                    string xhjxs = x.Split('|')[1];
                    if (!xhjname.Contains("diaoxin"))
                    {

                        //Control sk = GetPbControl(xhjname);
                        switch (xhjxs)
                        {
                            case "UU":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 2;
                                break;
                            case "U":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 1;
                                break;
                            case "H":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 5;
                                break;
                            case "B":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 4;
                                break;
                            case "L":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 3;
                                break;
                            case "HL":
                                ((Xinhaoji2)(ht_zc[xhjname])).X_flag = 6;
                                break;
                        }
                        ((Xinhaoji2)(ht_zc[xhjname])).drawpic();
                        //jinlu_xhj_xianshi(xhjname, xhjxs);

                    }

                    else
                    {
                        //string xhjname = x.Split('|')[0];
                        //string xhjxs = x.Split('|')[1];
                        switch (xhjxs)
                        {
                            case "N":
                                ((Diaoxin)(ht_zc[xhjname])).X_flag = 1;
                                break;
                            case "B":
                                ((Diaoxin)(ht_zc[xhjname])).X_flag = 2;
                                break;
                        }
                        ((Diaoxin)(ht_zc[xhjname])).drawpic();
                    }
                }
            }
            #endregion
            return true;
        }
         #region 进路信息保存
         private bool jinlu_function(string info, object sender)
         {
             string xhj = info.Split('#')[0];
             string dc = info.Split('#')[1];
             string gd = info.Split('#')[2];

             if (jinlu_biaoji(xhj, dc, gd))
             {
                 //保存进路信息
                 if (((Button)sender).Name.Contains("button_bljl"))
                 {
                     list_lsb_ed.Add(info);

                 }
                 else if (((Button)sender).Name.Contains("button_zqx"))
                 {
                     foreach (string t in list_lsb_ed)
                     {
                         if (t.Contains(info.Split('|')[0]))
                         {
                             list_lsb_ed.Remove(t);
                             break;
                         }
                     }
                 }
                 return true;
             }
             else
             {
                 return false;
             }

         }
         #endregion
         #region 进路排列完成显示
         private void jinlu_pailie(string jinlu_xinxi)
         {
             foreach (DictionaryEntry jinlu in ht_jlxx)
             {
                 if (jinlu_xinxi == jinlu.Value.ToString())
                 {
                     if ((jinlu_xinxi.Split('+')[0] == "button_XJ") || (jinlu_xinxi.Split('+')[0] == "button_XFJ") || (jinlu_xinxi.Split('+')[0] == "button_X1") || (jinlu_xinxi.Split('+')[0] == "button_X2") || (jinlu_xinxi.Split('+')[0] == "button_X3") || (jinlu_xinxi.Split('+')[0] == "button_X4"))
                     {
                         fcplwch = ht_jlfcxx[jinlu.Key.ToString().Split(',')[0]].ToString();
                         ht_jlfcxx[jinlu.Key.ToString().Split(',')[0]] = fcplwch.Split(',')[0] + "," + fcplwch.Split(',')[1] + "," + fcplwch.Split(',')[2] + "," + fcplwch.Split(',')[3] + "," + fcplwch.Split(',')[4] + ",排列完成";
                     }

                     if ((jinlu_xinxi.Split('+')[0] == "button_SJ") || (jinlu_xinxi.Split('+')[0] == "button_SFJ") || (jinlu_xinxi.Split('+')[0] == "button_S1") || (jinlu_xinxi.Split('+')[0] == "button_S2") || (jinlu_xinxi.Split('+')[0] == "button_S3") || (jinlu_xinxi.Split('+')[0] == "button_S4"))
                     {
                         jcplwch = ht_jljcxx[jinlu.Key.ToString().Split(',')[0]].ToString();
                         ht_jljcxx[jinlu.Key.ToString().Split(',')[0]] = jcplwch.Split(',')[0] + "," + jcplwch.Split(',')[1] + "," + jcplwch.Split(',')[2] + "," + jcplwch.Split(',')[3] + "," + jcplwch.Split(',')[4] + ",排列完成";
                     }
                 }
             }
         }
         #endregion
        #endregion
        #region 屏幕显示时间
         private void tmDate_Tick(object sender, EventArgs e)//屏幕实时显示时间
         {
             DateTime dt = DateTime.Now;
             string date = dt.ToLongDateString();
             string time = dt.ToLongTimeString();
             nowTime.Text = "当前时间\n" + date + time;
             if (!chewushanghai.LoadStatus)
                 login.Text = "未登录";
             if (chewushanghai.LoadStatus)
                 login.Text = "值班员:" + chewushanghai.user;
         }
         #endregion
        #region 切换窗口
         private void 北京南站场图ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             chewu C1 = new chewu();
             C1.Show();
         }
         private void 南京南站场图ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             chewunanjing C2 = new chewunanjing();
             C2.Show();
         }
         private void 上海虹桥站场图ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             chewushanghai C3 = new chewushanghai();
             C3.Show();
         }
         private void 多站显示ToolStripSplitButton_Click(object sender, EventArgs e)
         {
             duozhanxianshi D2 = new duozhanxianshi();
             D2.Show();
         }
         private void 北京南站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             xingcherizhi1 X1 = new xingcherizhi1();
             X1.Show();
         }
         private void 南京南站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             xingcherizhi2 X2 = new xingcherizhi2();
             X2.Show();
         }
         private void 上海虹桥站行车日志ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             xingcherizhi3 X3 = new xingcherizhi3();
             X3.Show();
         }
         #endregion
        #region 走车设置
         int t = 0;
         private void 自动走车ToolStripMenuItem_Click(object sender, EventArgs e)
         {
             走车.Enabled = true;
             for (int i1 = 0; i1 < dataGridView1.Rows.Count; i1++)
             {
                 if (ZCXL[0].Contains("xinhaoji_S"))
                 {
                     if (this.dataGridView1.Rows[i1].Cells[0].Value != null)
                     {
                         this.dataGridView1.Rows[i1].Cells[5].Value = "进路已占用";
                         this.dataGridView1.Rows[i1].DefaultCellStyle.BackColor = Color.Red;
                     }
                 }
             }
             for (int i2 = 0; i2 < dataGridView2.Rows.Count; i2++)
             {
                 if (ZCXL[0].Contains("xinhaoji_X"))
                 {
                     if (this.dataGridView2.Rows[i2].Cells[0].Value != null)
                     {
                         this.dataGridView2.Rows[i2].Cells[5].Value = "进路已占用";
                         this.dataGridView2.Rows[i2].DefaultCellStyle.BackColor = Color.Red;
                     }
                 }
             }
         }
         #endregion
        #region 道岔控件与button关联第一部分
         private void daocha_1_1_Custom(object sender, Daocha_1.CustomEventArgs e)
         {
             if (e.Flag)
             {
                 Graphics c = Graphics.FromImage(button_Daocha1.BackgroundImage);
                 c.Clear(Color.ForestGreen);
                 button_Daocha1.Refresh();
             }
             else
             {
                 Graphics c = Graphics.FromImage(button_Daocha1.BackgroundImage);
                 c.Clear(Color.Yellow);
                 button_Daocha1.Refresh();
             }
         }
         private void daocha_1_2_Custom(object sender, Daocha_1.CustomEventArgs e)
         {
             if (e.Flag)
             {
                 Graphics c = Graphics.FromImage(button_Daocha2.BackgroundImage);
                 c.Clear(Color.ForestGreen);
                 button_Daocha2.Refresh();
             }
             else
             {
                 Graphics c = Graphics.FromImage(button_Daocha2.BackgroundImage);
                 c.Clear(Color.Yellow);
                 button_Daocha2.Refresh();
             }
         }
         private void daocha_1_3_Custom(object sender, Daocha_1.CustomEventArgs e)
         {
             if (e.Flag)
             {
                 Graphics c = Graphics.FromImage(button_Daocha3.BackgroundImage);
                 c.Clear(Color.ForestGreen);
                 button_Daocha3.Refresh();
             }
             else
             {
                 Graphics c = Graphics.FromImage(button_Daocha3.BackgroundImage);
                 c.Clear(Color.Yellow);
                 button_Daocha3.Refresh();
             }
         }
         private void daocha_1_4_Custom(object sender, Daocha_1.CustomEventArgs e)
         {
             if (e.Flag)
             {
                 Graphics c = Graphics.FromImage(button_Daocha4.BackgroundImage);
                 c.Clear(Color.ForestGreen);
                 button_Daocha4.Refresh();
             }
             else
             {
                 Graphics c = Graphics.FromImage(button_Daocha4.BackgroundImage);
                 c.Clear(Color.Yellow);
                 button_Daocha4.Refresh();
             }
         }
         #endregion
        private void 走车_Tick(object sender, EventArgs e)
        {
            if (ZCXL[t]!= null)
            {
                    #region 列车进路
                    if (((Xinhaoji2)(ht_zc[ZCXL[0]])).X_flag != 5)
                    {
                        //改变信号机状态
                        ((Xinhaoji2)(ht_zc[ZCXL[0]])).X_flag = 5;
                        ((Xinhaoji2)(ht_zc[ZCXL[0]])).drawpic();
                        t++;
                        //与信号机改变第一次状态
                        if ((ZCXL[t].Contains("G")) || (ZCXL[t].Contains("dangui")) || (ZCXL[t].Contains("LQ")))
                        {
                            if (((Dangui)ht_zc[ZCXL[t]]).flag_zt == 2)
                            {
                                ((Dangui)ht_zc[ZCXL[t]]).flag_zt = 1;
                                ((Dangui)ht_zc[ZCXL[t]]).Drawpic();
                                //改变车次号位置
                                label56.Location = new Point(((Dangui)ht_zc[ZCXL[t]]).Location.X + (((Dangui)ht_zc[ZCXL[t]]).Size.Width-23)/2, ((Dangui)ht_zc[ZCXL[t]]).Location.Y);
                                t++;
                            }
                        }
                        else if (ZCXL[t].Contains("daocha_1"))
                        {
                            if (ZCXL[t].Contains("daocha_1_3") || ZCXL[t].Contains("daocha_1_4"))
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y );
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            else
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            t++;
                        }
                    }
                    #endregion
                 //改变剩下状态
                    #region 改变剩下状态
                    else
                    {
                        if (ZCXL[t].Contains("xinhaoji"))
                        {
                            if (((Xinhaoji2)(ht_zc[ZCXL[t]])).X_flag != 5)
                            //改变信号机状态
                            {
                                ((Xinhaoji2)(ht_zc[ZCXL[t]])).X_flag = 5;
                                ((Xinhaoji2)(ht_zc[ZCXL[t]])).drawpic();
                                t++;
                            }
                            //与信号机改变第一次状态
                            if ((ZCXL[t].Contains("G")) || (ZCXL[t].Contains("dangui")) || (ZCXL[t].Contains("LQ")))
                            {
                                if (((Dangui)ht_zc[ZCXL[t]]).flag_zt == 2)
                                {
                                    ((Dangui)ht_zc[ZCXL[t]]).flag_zt = 1;
                                    ((Dangui)ht_zc[ZCXL[t]]).Drawpic();
                                    //改变车次号位置
                                    label56.Location = new Point(((Dangui)ht_zc[ZCXL[t]]).Location.X + (((Dangui)ht_zc[ZCXL[t]]).Size.Width - 23) / 2, ((Dangui)ht_zc[ZCXL[t]]).Location.Y);
                                    t++;
                                }
                            }
                            else if (ZCXL[t].Contains("daocha_1"))
                        {
                            if (ZCXL[t].Contains("daocha_1_3") || ZCXL[t].Contains("daocha_1_4"))
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y );
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            else
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            t++;
                        }
                    }
                        else if ((ZCXL[t].Contains("G")) || (ZCXL[t].Contains("dangui")) || (ZCXL[t].Contains("LQ")))
                        {
                            if (((Dangui)ht_zc[ZCXL[t]]).flag_zt == 2)
                            {
                                ((Dangui)ht_zc[ZCXL[t]]).flag_zt = 1;
                                ((Dangui)ht_zc[ZCXL[t]]).Drawpic();
                                //改变车次号位置
                                label56.Location = new Point(((Dangui)ht_zc[ZCXL[t]]).Location.X + (((Dangui)ht_zc[ZCXL[t]]).Size.Width - 23) / 2, ((Dangui)ht_zc[ZCXL[t]]).Location.Y);
                                t++;
                            }
                        }
                        else if (ZCXL[t].Contains("daocha_1"))
                        {
                            if (ZCXL[t].Contains("daocha_1_3") || ZCXL[t].Contains("daocha_1_4"))
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y );
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            else
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            t++;
                        }
                    }
                    #endregion
            }
            #region 空闲
            if (ZCXL[t] == null)
                { 
                    int k;
                    for (k=1; k<t-1; k++)
                    {
                        if ((ZCXL[k].Contains("G")) || (ZCXL[k].Contains("dangui")) || (ZCXL[k].Contains("LQ")))
                        {
                            if (((Dangui)ht_zc[ZCXL[k]]).flag_zt ==1)
                            {
                                ((Dangui)ht_zc[ZCXL[k]]).flag_zt = 3;
                                ((Dangui)ht_zc[ZCXL[k]]).Drawpic();
                            }
                        }
                        else if (ZCXL[k].Contains("daocha_1"))
                        { 
                        ((Daocha_1)ht_DC_1[ZCXL[k]]).锁闭状态 = ControlLib.Daocha_1.STATE.空闲;
                        }
                    }
                    //清空ZCXL字符串组
                    for (int i = 0; i < ZCXL.Length; i++)
                    {
                        ZCXL[i] = null;
                        j = 0;
                        t = 0;
                    }
                    走车.Enabled = false; //跳出timer
                }
            #endregion
        }
        private void 当前站申请自律模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            danniu_left.显示状态 = Danniu.Xianshi.默认;
            danniu_middle.显示状态 = Danniu.Xianshi.默认;
            danniu0.显示状态 = Danniu.Xianshi.默认;
            danniu1.显示状态 = Danniu.Xianshi.绿;
            danniu2.显示状态 = Danniu.Xianshi.默认;
        }
        private void 当前站退出自律模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            danniu_left.显示状态 = Danniu.Xianshi.红;
            danniu_middle.显示状态 = Danniu.Xianshi.黄;
            danniu0.显示状态 = Danniu.Xianshi.默认;
            danniu1.显示状态 = Danniu.Xianshi.默认;
            danniu2.显示状态 = Danniu.Xianshi.默认;
        }
        //用以提取数据库数据
        public string shangxiaxing = "";//上下行
        public string JCGD = "";//接车股道
        public string FCGD = "";//发车股道
        public string jiefa = "";//接发车
        public string JLAN = "";//进路按钮
        public string cch = "";//车次号
        #region 读取数据库信息并显示
        private void 更新列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            this.dataGridView2.Rows.Clear();
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                if (dr.GetString(dr.GetOrdinal("接收方")) == "南京南")
                {
                    JCGD = dr.GetString(dr.GetOrdinal("接车股道"));
                    FCGD = dr.GetString(dr.GetOrdinal("发车股道"));
                    cch = dr.GetString(dr.GetOrdinal("车次号"));
                    车次号();
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "接车")
                    {
                        if (dr.GetString(dr.GetOrdinal("发车车站")) == "北京南")
                            shangxiaxing = "下行";
                        if (dr.GetString(dr.GetOrdinal("发车车站")) == "上海虹桥")
                            shangxiaxing = "上行";
                        int index1 = this.dataGridView1.Rows.Add();
                        this.dataGridView1.Rows[index1].Cells[0].Value = cch;
                        this.dataGridView1.Rows[index1].Cells[1].Value = dr["接车股道"];
                        this.dataGridView1.Rows[index1].Cells[2].Value = dr["计划到达时间"];
                        this.dataGridView1.Rows[index1].Cells[3].Value = true;
                        this.dataGridView1.Rows[index1].Cells[5].Value = "未触发";
                        jinluanniu(shangxiaxing, jiefa, JCGD, FCGD);
                        this.dataGridView1.Rows[index1].Cells[4].Value = JLAN;
                    }
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "发车")
                    {
                        if (dr.GetString(dr.GetOrdinal("到达车站")) == "北京南")
                            shangxiaxing = "上行";
                        if (dr.GetString(dr.GetOrdinal("到达车站")) == "上海虹桥")
                            shangxiaxing = "下行";
                        int index2 = this.dataGridView2.Rows.Add();
                        this.dataGridView2.Rows[index2].Cells[0].Value = cch;
                        this.dataGridView2.Rows[index2].Cells[1].Value = dr["发车股道"];
                        this.dataGridView2.Rows[index2].Cells[2].Value = dr["计划发车时间"];
                        this.dataGridView2.Rows[index2].Cells[3].Value = true;
                        this.dataGridView2.Rows[index2].Cells[5].Value = "未触发";
                        jinluanniu(shangxiaxing, jiefa, JCGD ,FCGD);
                        this.dataGridView2.Rows[index2].Cells[4].Value = JLAN;
                    }
                }
            }
            dr.Close();
        }
        #endregion
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
        //列车进路按钮
        #region 列车进路按钮
        public void jinluanniu(string shangxiaxing, string jiefa, string JCGD, string FCGD)
        {
            if (shangxiaxing == "上行")
            {
                if (jiefa == "接车")
                {
                    if (JCGD == "1G" && FCGD =="1G")
                    {
                        JLAN = "SFJ-X1";
                    }
                    if (JCGD == "2G" && FCGD =="2G")
                    {
                        JLAN = "SJ-X2";
                    }
                    if (JCGD == "3G" && FCGD == "1G")
                    {
                        JLAN = "SFJ-X3";
                    }
                    if (JCGD == "4G" && FCGD == "2G")
                    {
                        JLAN = "SJ-X4";
                    }
                }
                else
                {
                    if (FCGD == "1G" && JCGD == "1G")
                    {
                        JLAN = "S1-XJ";
                    }
                    if (FCGD == "2G" && JCGD == "2G")
                    {
                        JLAN = "S2-XFJ";
                    }
                    if (FCGD == "3G" && JCGD == "1G")
                    {
                        JLAN = "S3-XJ";
                    }
                    if (FCGD == "4G" && JCGD == "2G")
                    {
                        JLAN = "S4-XFJ";
                    }
                }
            }
            else
            {
                if (jiefa == "接车")
                {
                    if (JCGD == "1G" && FCGD == "1G")
                    {
                        JLAN = "XJ-S1";
                    }
                    if (JCGD == "2G" && FCGD == "2G")
                    {
                        JLAN = "XFJ-S2";
                    }
                    if (JCGD == "3G" && FCGD == "1G")
                    {
                        JLAN = "XJ-S3";
                    }
                    if (JCGD == "4G" && FCGD == "2G")
                    {
                        JLAN = "XFJ-S4";
                    }
                }
                else
                {
                    if (FCGD == "1G" && JCGD == "1G")
                    {
                        JLAN = "X1-SFJ";
                    }
                    if (FCGD == "2G" && JCGD == "2G")
                    {
                        JLAN = "X2-SJ";
                    }
                    if (FCGD == "3G" && JCGD == "1G")
                    {
                        JLAN = "X3-SFJ";
                    }
                    if (FCGD == "4G" && JCGD == "2G")
                    {
                        JLAN = "X4-SJ";
                    }
                }
            }
        }
        #endregion
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            jiefa = "接车";
            if (dataGridView1.SelectedCells.Count != 0)
            {
                int selRow = dataGridView1.SelectedRows[0].Index;
                cch = dataGridView1.Rows[selRow].Cells[0].Value.ToString();
                车次号1();
                this.dataGridView1.Rows[selRow].Cells[3].Value = false;
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            jiefa = "发车";
            if (dataGridView2.SelectedCells.Count != 0)
            {
                int selRow = dataGridView2.SelectedRows[0].Index;
                cch = dataGridView2.Rows[selRow].Cells[0].Value.ToString();
                车次号1();
                this.dataGridView2.Rows[selRow].Cells[3].Value = false;
            }
        }
    }
}
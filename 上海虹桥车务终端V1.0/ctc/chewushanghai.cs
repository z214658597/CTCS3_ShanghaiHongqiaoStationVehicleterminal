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
using System.Collections;
using System.Threading;
using MySql.Data.MySqlClient;
using ControlLib;

namespace ctc
{
    public partial class chewushanghai : Form
    {
        public static chewushanghai current = null;
        #region 链表——存储站场布置数据
        List<string> list_cz = new List<string>();              //链表按键
        List<string> list_button_Switch = new List<string>();    //新加入链表道岔按钮
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

        Hashtable ht_zc = new Hashtable();      //哈希表站场
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
        //创建MySqlConnection对象，连接MySql数据库
        public static MySqlCommand com = new MySqlCommand();
        public static MySqlConnection conn = null;
        //存储用户信息
        public static string user = "", number = "", loadtime = "";
        public static bool LoadStatus = false;//登录状态标记
        //网络连接
        public static Socket socketClient = null;
        public static Thread threadClient = null;
        public string recieve = "";//接收到的信息
        public string strRecMsg = "";//分割开的信息
        public static string moshi = "";//模式转换信息
        //接收阶段计划
        public string jsf = "";//接收方
        public string xh = "";//序号
        public static string xdsj = "";//下达时间
        public string cch = "";//车次号
        public string fccz = "";//发车车站
        public string ddcz = "";//到达车站
        public string fcsj = "";//发车时间
        public string ddsj = "";//到达时间
        public string jcgd = "";//接车股道
        public string fcgd = "";//发车股道
        //阶段计划回执
        public string fsf = "";//发送方
        public string hzsj = "";//回执时间
        public string senddata = "";//阶段计划回执
        //删除阶段计划
        public string jhsj = "";//计划时间
        public string scxh = "";//删除序号
        public string scsj = "";//删除时间
        //接收调度命令
        public string sldw = "";//受令单位
        public string mlxh = "";//命令序号
        public string mlbh = "";//命令编号
        public string mllx = "";//命令类型
        public string mlnr = "";//命令内容
        public static string flsj = "";//发令时间
        //调度命令回执
        public string fldw = "";//发令单位
        public string slsj = "";//受令时间
        public string senddata2 = "";//调度命令回执
        //请求调度命令
        public static int mlxh1 = 0;//请求命令序号
        public string qqjg = "";//请求结果
        //办理进路
        public string dddc1 = "";//单动道岔1
        public string ddzt1 = "";//单动状态1
        public string name1 = "";//单动道岔名称1
        public string dddc2 = "";//单动道岔2
        public string ddzt2 = "";//单动状态2
        public string name4 = "";//单动道岔名称2
        public string sddc = "";//双动道岔
        public string sdzt = "";//双动状态
        public string gd = "";//股道
        public string gdzt = "";//股道状态
        public string name2 = "";//股道名称
        public string name_dc = "";//道岔名称
        public string xhj = "";//信号机
        public string xhzt = "";//信号机状态
        public string name3 = "";//信号机名称
        //走车
        public static string zcsj = "";//走车时间
        public string sa = "";            //区间占用信息
        public string jcplwch = "";       //接车排列完成
        public string fcplwch = "";       //发车排列完成
        #endregion
        public chewushanghai()
        {
            InitializeComponent();
            current = this;
            CheckForIllegalCrossThreadCalls = false;
        }
        #region 根据窗体大小调整控件大小位置
        private float P, Q;
        //获得控件的长度、宽度、位置、字体大小的数据
        private void setTag(Control cons)//Control类，定义控件的基类
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;//获取或设置包含有关控件的数据的对象
                if (con.Controls.Count > 0)
                    setTag(con);//递归算法
            }
        }

        private void setControls(float newx, float newy, Control cons)//实现控件以及字体的缩放
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);//递归
                }
            }
        }

        private void MyForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / P;//当前宽度与变化前宽度之比
            float newy = this.Height / Q;//当前高度与变化前宽度之比
            setControls(newx, newy, this);
            this.Text = this.Width.ToString() + "*" + this.Height.ToString();  //窗体标题显示长度和宽度
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Resize += new EventHandler(MyForm_Resize);
            P = this.Width;
            Q = this.Height;
            setTag(this);
        }
        #endregion

        string ipaddress_test = "192.168.1.211";
        int port_test = 21000;

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
            com = new MySqlCommand("DELETE FROM jiefachejihua", conn);
            com.ExecuteNonQuery();
            com = new MySqlCommand("DELETE FROM diaodumingling", conn);
            com.ExecuteNonQuery();
            com = new MySqlCommand("DELETE FROM qingqiumingling", conn);
            com.ExecuteNonQuery();

            Thread th1 = new Thread(connectionInniti);
            th1.IsBackground = true;
            th1.Start();
            
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
                if (ct.Name.Contains("dangui") || ct.Name.Contains("G") || ct.Name.Contains("xinhaoji") || ct.Name.Contains("daocha")|| ct.Name.Contains("LQ"))
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
                
                //2类道岔初始化
                else if (ct.Name.Contains("daocha_2"))
                {
                    Daocha_2 dc2 = (Daocha_2)ct;
                    string[] str = dc2.Name.Split('_');
                    //dc2.ID号上 = str[2];
                    //dc2.ID号下 = str[3];
                    dc2.handle = this.Handle;
                    //list_DC2.Add(dc2);
                    ht_DC_2.Add(dc2.Name, dc2);
                }
                
                //1类道岔初始化
                else if (ct.Name.Contains("daocha_1_2") || ct.Name.Contains("daocha_1_4") || ct.Name.Contains("daocha_1_5") || ct.Name.Contains("daocha_1_7"))
                {
                    Daocha_1 dc1 = (Daocha_1)ct;
                    string[] str = dc1.Name.Split('_');
                    dc1.ID号 = str[2];
                    dc1.handle = this.Handle;
                    //list_DC1.Add(dc1);
                    ht_DC_1.Add(dc1.Name, dc1);
                }

                //1类道岔初始化
                else if (ct.Name.Contains("daocha_1_6") || ct.Name.Contains("daocha_1_8") || ct.Name.Contains("daocha_1_9") || ct.Name.Contains("daocha_1_11"))
                {
                    Daocha_1_1 dc1 = (Daocha_1_1)ct;
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
            //@1下行5G侧线接车
            string key = "button_XJ+button_S5+button_bljl";
            string value = "xinhaoji_X|UU#daocha_1_9|F|S%daocha_1_5|F|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S%G5|S";
            ht_lsb.Add(key, value);
            //下行5G侧线接车取消进路
            key = "button_XJ+button_S5+button_zqx";
            value = "xinhaoji_X|H#daocha_1_9|F|K%daocha_1_5|F|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K%G5|K";
            ht_lsb.Add(key, value);
            //@2下行3G侧线接车
            key = "button_XJ+button_S3+button_bljl";
            value = "xinhaoji_X|UU#daocha_1_9|D|S%daocha_1_5|F|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S%G3|S";
            ht_lsb.Add(key, value);
            //下行3G侧线接车取消进路
            key = "button_XJ+button_S3+button_zqx";
            value = "xinhaoji_X|H#daocha_1_9|D|K%daocha_1_5|F|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K%G3|K";
            ht_lsb.Add(key, value);
            //@3下行1G正线接车
            key = "button_XJ+button_S1+button_bljl";
            value = "xinhaoji_X|U#daocha_1_5|D|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S%G1|S";
            ht_lsb.Add(key, value);
            //下行1G正线接车取消进路
            key = "button_XJ+button_S1+button_zqx";
            value = "xinhaoji_X|U#daocha_1_5|D|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K%G1|K";
            ht_lsb.Add(key, value);
            //@4下行2G正线接车
            key = "button_XFJ+button_S2+button_bljl";
            value = "xinhaoji_XF|U#daocha_1_7|D|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S%G2|S";
            ht_lsb.Add(key, value);
            //下行2G正线接车取消进路
            key = "button_XFJ+button_S2+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_7|D|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K%G2|K";
            ht_lsb.Add(key, value);
            //@5下行2G向1G正线接车
            key = "button_XFJ+button_S1+button_bljl";
            value = "xinhaoji_XF|U#daocha_1_5|D|S%daocha_2_1_3|F|S|S#S3LQ|S%S2LQ|S%IIAG|S%G1|S";
            ht_lsb.Add(key, value);
            //下行2G向1G正线接车取消进路
            key = "button_XFJ+button_S1+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_5|D|K%daocha_2_1_3|F|S|K#S3LQ|K%S2LQ|K%IIAG|K%G1|K";
            ht_lsb.Add(key, value);
            //@6下行2G向3G接车
            key = "button_XFJ+button_S3+button_bljl";
            value = "xinhaoji_XF|UU#daocha_1_9|D|S%daocha_1_5|F|S%daocha_2_1_3|F|S|S#S3LQ|S%S2LQ|S%IIAG|S%G3|S";
            ht_lsb.Add(key, value);
            //下行2G向3G接车取消进路
            key = "button_XFJ+button_S3+button_zqx";
            value = "xinhaoji_XF|UU#daocha_1_9|D|K%daocha_1_5|F|K%daocha_2_1_3|F|S|K#S3LQ|K%S2LQ|K%IIAG|K%G3|K";
            ht_lsb.Add(key, value);
            //@7下行2G向5G接车
            key = "button_XFJ+button_S5+button_bljl";
            value = "xinhaoji_XF|UU#daocha_1_9|F|S%daocha_1_5|F|S%daocha_2_1_3|F|S|S#S3LQ|S%S2LQ|S%IIAG|S%G5|S";
            ht_lsb.Add(key, value);
            //下行2G向5G接车取消进路
            key = "button_XFJ+button_S5+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_9|F|K%daocha_1_5|F|K%daocha_2_1_3|F|S|K#S3LQ|K%S2LQ|K%IIAG|K%G5|K";
            ht_lsb.Add(key, value);
            //@8下行4G侧线接车
            key = "button_XFJ+button_S4+button_bljl";
            value = "xinhaoji_XF|UU#daocha_1_11|D|S%daocha_1_7|F|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S%G4|S";
            ht_lsb.Add(key, value);
            //下行4G侧线接车取消进路
            key = "button_XFJ+button_S4+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_11|D|K%daocha_1_7|F|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K%G4|K";
            ht_lsb.Add(key, value);
            //@9下行6G侧线接车
            key = "button_XFJ+button_S6+button_bljl";
            value = "xinhaoji_XF|UU#daocha_1_7|F|S%daocha_1_11|F|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S%G6|S";
            ht_lsb.Add(key, value);
            //下行6G侧线接车取消进路
            key = "button_XFJ+button_S6+button_zqx";
            value = "xinhaoji_XF|H#daocha_1_7|F|K%daocha_1_11|F|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K%G6|K";
            ht_lsb.Add(key, value);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //上行
            //@1上行5G向1G侧线发车
            key = "button_S5+button_XJ+button_bljl";
            value = "xinhaoji_S5|L#daocha_1_9|F|S%daocha_1_5|F|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S";
            ht_lsb.Add(key, value);
            //上行5G向1G侧线发车取消进路
            key = "button_S5+button_XJ+button_zqx";
            value = "xinhaoji_S5|H#daocha_1_9|F|K%daocha_1_5|F|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K";
            ht_lsb.Add(key, value);
            //@2上行5G向2G侧线发车
            key = "button_S5+button_XFJ+button_bljl";
            value = "xinhaoji_S5|L#daocha_1_9|F|S%daocha_1_5|F|S%daocha_2_1_3|F|X|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行5G向2G侧线发车取消进路
            key = "button_S5+button_XFJ+button_zqx";
            value = "xinhaoji_S5|H#daocha_1_9|F|K%daocha_1_5|F|K%daocha_2_1_3|F|X|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@3上行3G向1G侧线发车
            key = "button_S3+button_XJ+button_bljl";
            value = "xinhaoji_S3|L#daocha_1_9|D|S%daocha_1_5|F|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S";
            ht_lsb.Add(key, value);
            //上行3G向1G侧线发车取消进路
            key = "button_S3+button_XJ+button_zqx";
            value = "xinhaoji_S3|H#daocha_1_9|D|K%daocha_1_5|F|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K";
            ht_lsb.Add(key, value);
            //@4上行3G向2G侧线发车
            key = "button_S3+button_XFJ+button_bljl";
            value = "xinhaoji_S3|L#daocha_1_9|D|S%daocha_1_5|F|S%daocha_2_1_3|F|X|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行3G向2G侧线发车取消进路
            key = "button_S3+button_XFJ+button_zqx";
            value = "xinhaoji_S3|H#daocha_1_9|D|K%daocha_1_5|F|K%daocha_2_1_3|F|X|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@5上行1G正线发车
            key = "button_S1+button_XJ+button_bljl";
            value = "xinhaoji_S1|L#daocha_1_5|D|S%daocha_2_1_3|D|X|S#X1JG|S%X2JG|S%IAG|S";
            ht_lsb.Add(key, value);
            //上行1G正线发车取消进路
            key = "button_S1+button_XJ+button_zqx";
            value = "xinhaoji_S1|H#daocha_1_5|D|K%daocha_2_1_3|D|X|K#X1JG|K%X2JG|K%IAG|K";
            ht_lsb.Add(key, value);
            //@6上行1G向2G正线发车
            key = "button_S1+button_XFJ+button_bljl";
            value = "xinhaoji_S1|L#daocha_1_5|D|S%daocha_2_1_3|F|X|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行1G向2G正线发车取消进路
            key = "button_S1+button_XFJ+button_zqx";
            value = "xinhaoji_S1|H#daocha_1_5|D|K%daocha_2_1_3|F|X|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@7上行2G正线发车
            key = "button_S2+button_XFJ+button_bljl";
            value = "xinhaoji_S2|L#daocha_1_7|D|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行2G正线发车取消进路
            key = "button_S2+button_XFJ+button_zqx";
            value = "xinhaoji_S2|H#daocha_1_7|D|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@8上行4G侧线发车
            key = "button_S4+button_XFJ+button_bljl";
            value = "xinhaoji_S4|L#daocha_1_12|D|S%daocha_1_7|F|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行4G侧线发车取消进路
            key = "button_S4+button_XFJ+button_zqx";
            value = "xinhaoji_S4|H#daocha_1_12|D|K%daocha_1_7|F|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            //@9上行6G侧线发车
            key = "button_S6+button_XFJ+button_bljl";
            value = "xinhaoji_S6|L#daocha_1_7|F|S%daocha_1_11|F|S%daocha_2_1_3|D|S|S#S3LQ|S%S2LQ|S%IIAG|S";
            ht_lsb.Add(key, value);
            //上行6G侧线发车取消进路
            key = "button_S6+button_XFJ+button_zqx";
            value = "xinhaoji_S6|H#daocha_1_7|F|K%daocha_1_11|F|K%daocha_2_1_3|D|S|K#S3LQ|K%S2LQ|K%IIAG|K";
            ht_lsb.Add(key, value);
            #endregion
        }
        #endregion
        #region 办理进路与总取消按键
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
            if (jiefa == "接车")
            {
                senddata1 = "AB92010906" + cch + "04AC";
                ClientSendMsg(senddata1);
            }
            else if (jiefa == "发车")
            {
                senddata1 = "AB92020906" + cch + "04AC";
                ClientSendMsg(senddata1);
            }
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
            /*if (ZCXL[0] != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    //列车进路等待出发状态修改为排列完成
                    if (ZCXL[0].Contains("xinhaoji_S"))
                    {
                        if (this.dataGridView1.Rows[i].Cells[0].Value != null)
                        {
                            this.dataGridView1.Rows[i].Cells[6].Value = "排列完成";
                            this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
                for (int j = 0; j < dataGridView2.Rows.Count-1; j++)
                {
                    if (ZCXL[0].Contains("xinhaoji_X"))
                    {
                        if (this.dataGridView2.Rows[j].Cells[0].Value != null)
                        {
                            this.dataGridView2.Rows[j].Cells[6].Value = "排列完成";
                            this.dataGridView2.Rows[j].DefaultCellStyle.BackColor = Color.Green;
                        }
                    }
                }
            }*/
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
                if (jinlu_zy_jc(info, sender))
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
        #region 道岔单操单封按键操作
        private void btnFuction_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)(e)).Button == MouseButtons.Left)
            {
                string str = ((Button)(sender)).Name;
                if (list_button_Switch.Contains(str))
                {
                    MessageBox.Show("已选中");
                }
                else
                {
                    list_button_Switch.Add(((Button)(sender)).Name);
                }
            }
        }
        #region Daocha_1_1类包括6，8，9，11道岔
        //道岔6的单操

        private void button_Daocha6_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.空闲)//判断道岔6是否空闲
                {
                    button_Daocha6.BackColor = Color.Green;
                    daocha_1_6.定反位 = Daocha_1_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔6处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.空闲)
                {
                    button_Daocha6.BackColor = Color.Yellow;
                    daocha_1_6.定反位 = Daocha_1_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔6处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔6已经锁闭！");
                }
                else
                {
                    if (label82.BackColor != Color.Purple)
                    {
                        daocha_1_6.道岔单操 = Daocha_1_1.DanCao.单操;
                        daocha_1_6.锁闭状态 = Daocha_1_1.STATE.锁闭;
                        label82.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔6处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.锁闭)
                {
                    daocha_1_6.锁闭状态 = Daocha_1_1.STATE.空闲;
                    daocha_1_6.道岔单操 = Daocha_1_1.DanCao.进路;
                    label82.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔6没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔6已经锁闭！");
                }
                else
                {
                    daocha_1_6.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label82.BackColor = Color.Purple;
                    label49.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_6.state == Daocha_1_1.STATE.空闲 && label82.BackColor == Color.Purple)
                {
                    //  daocha_1_6.锁闭状态 = Single_Switch.STATE.空闲;
                    label82.BackColor = Color.Black;
                    label49.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔6没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        //道岔8的单操
        private void button_Daocha8_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")          //目标转向定位
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.空闲)
                {
                    button_Daocha8.BackColor = Color.Green;
                    daocha_1_8.定反位 = Daocha_1_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔8处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//反位
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.空闲)
                {
                    button_Daocha8.BackColor = Color.Yellow;
                    daocha_1_8.定反位 = Daocha_1_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔8处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//单锁
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔8已经锁闭！");
                }
                else
                {
                    if (label83.BackColor != Color.Purple)//判断是否岔封
                    {
                        daocha_1_8.道岔单操 = Daocha_1_1.DanCao.单操;
                        daocha_1_8.锁闭状态 = Daocha_1_1.STATE.锁闭;
                        label83.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔8处于单封状态");
                    }

                }
            }
            else if (first_button == "button_dj")//单解
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.锁闭)
                {
                    daocha_1_8.锁闭状态 = Daocha_1_1.STATE.空闲;
                    daocha_1_8.道岔单操 = Daocha_1_1.DanCao.进路;
                    label83.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔8没有锁闭！");
                }
            }
            else if (first_button == "button_df")//单封
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔8已经锁闭！");
                }
                else
                {
                    daocha_1_8.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label83.BackColor = Color.Purple;
                    label5.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_8.state == Daocha_1_1.STATE.空闲 && label83.BackColor == Color.Purple)
                {
                    // daocha_1_8.锁闭状态 = Single_Switch.STATE.空闲;
                    label83.BackColor = Color.Black;
                    label5.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔8没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        //道岔9的操作
        private void button_Daocha9_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.空闲)//判断道岔6是否空闲
                {
                    button_Daocha9.BackColor = Color.Green;
                    daocha_1_9.定反位 = Daocha_1_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔9处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.空闲)
                {
                    button_Daocha9.BackColor = Color.Yellow;
                    daocha_1_9.定反位 = Daocha_1_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔9处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔9已经锁闭！");
                }
                else
                {
                    if (label60.BackColor != Color.Purple)
                    {
                        daocha_1_9.锁闭状态 = Daocha_1_1.STATE.锁闭;
                        label60.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔9处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.锁闭)
                {
                    daocha_1_9.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label60.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔9没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔9已经锁闭！");
                }
                else
                {
                    daocha_1_9.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label60.BackColor = Color.Purple;
                    label45.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_9.state == Daocha_1_1.STATE.空闲 && label60.BackColor == Color.Purple)
                {
                    //  daocha_1_6.锁闭状态 = Single_Switch.STATE.空闲;
                    label60.BackColor = Color.Black;
                    label45.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔9没有单封！");
                }
            }
            list_button_Switch.Clear();
        }

        //道岔11的操作
        private void button_Daocha11_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.空闲)//判断道岔6是否空闲
                {
                    button_Daocha11.BackColor = Color.Green;
                    daocha_1_11.定反位 = Daocha_1_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔11处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.空闲)
                {
                    button_Daocha11.BackColor = Color.Yellow;
                    daocha_1_11.定反位 = Daocha_1_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔11处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔11已经锁闭！");
                }
                else
                {
                    if (label53.BackColor != Color.Purple)
                    {
                        daocha_1_11.锁闭状态 = Daocha_1_1.STATE.锁闭;
                        label53.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔11处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.锁闭)
                {
                    daocha_1_11.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label53.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔11没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔11已经锁闭！");
                }
                else
                {
                    daocha_1_11.锁闭状态 = Daocha_1_1.STATE.空闲;
                    label53.BackColor = Color.Purple;
                    label48.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_11.state == Daocha_1_1.STATE.空闲 && label53.BackColor == Color.Purple)
                {
                    label53.BackColor = Color.Black;
                    label48.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔11没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        #endregion
        #region 道岔Daocha_1类，包括2，4，5，7号道岔
        //道岔5的操作
        private void button_Daocha5_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_5.state == Daocha_1.STATE.空闲)//判断道岔6是否空闲
                {
                    button_Daocha5.BackColor = Color.Green;
                    daocha_1_5.定反位 = Daocha_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔5处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_5.state == Daocha_1.STATE.空闲)
                {
                    button_Daocha5.BackColor = Color.Yellow;
                    daocha_1_5.定反位 = Daocha_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔5处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_5.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔5已经锁闭！");
                }
                else
                {
                    if (label55.BackColor != Color.Purple)
                    {
                        daocha_1_5.锁闭状态 = Daocha_1.STATE.锁闭;
                        label55.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔5处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_5.state == Daocha_1.STATE.锁闭)
                {
                    daocha_1_5.锁闭状态 = Daocha_1.STATE.空闲;
                    label55.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔5没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_5.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔5已经锁闭！");
                }
                else
                {
                    daocha_1_5.锁闭状态 = Daocha_1.STATE.空闲;
                    label55.BackColor = Color.Purple;
                    label46.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_5.state == Daocha_1.STATE.空闲 && label55.BackColor == Color.Purple)
                {
                    //  daocha_1_6.锁闭状态 = Single_Switch.STATE.空闲;
                    label55.BackColor = Color.Black;
                    label46.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔5没有单封！");
                }
            }
            list_button_Switch.Clear();
        }

        //道岔7的操作
        private void button_Daocha7_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_7.state == Daocha_1.STATE.空闲)//判断道岔6是否空闲
                {
                    button_Daocha7.BackColor = Color.Green;
                    daocha_1_7.定反位 = Daocha_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔7处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_7.state == Daocha_1.STATE.空闲)
                {
                    button_Daocha7.BackColor = Color.Yellow;
                    daocha_1_7.定反位 = Daocha_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔7处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_7.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔7已经锁闭！");
                }
                else
                {
                    if (label59.BackColor != Color.Purple)
                    {
                        daocha_1_7.锁闭状态 = Daocha_1.STATE.锁闭;
                        label59.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔7处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_7.state == Daocha_1.STATE.锁闭)
                {
                    daocha_1_7.锁闭状态 = Daocha_1.STATE.空闲;
                    label59.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔7没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_7.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔7已经锁闭！");
                }
                else
                {
                    daocha_1_7.锁闭状态 = Daocha_1.STATE.空闲;
                    label59.BackColor = Color.Purple;
                    label47.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_7.state == Daocha_1.STATE.空闲 && label59.BackColor == Color.Purple)
                {
                    //  daocha_1_6.锁闭状态 = Single_Switch.STATE.空闲;
                    label59.BackColor = Color.Black;
                    label47.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔7没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        //道岔2的操作
        private void button_Daocha2_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标转向定位
            {
                if (daocha_1_2.state == Daocha_1.STATE.空闲)//判断道岔2是否空闲
                {
                    button_Daocha2.BackColor = Color.Green;
                    daocha_1_2.定反位 = Daocha_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔2处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标转向反位
            {
                if (daocha_1_2.state == Daocha_1.STATE.空闲)
                {
                    button_Daocha2.BackColor = Color.Yellow;
                    daocha_1_2.定反位 = Daocha_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔2处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标是单锁
            {
                if (daocha_1_2.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔2已经锁闭！");
                }
                else
                {
                    if (label52.BackColor != Color.Purple)
                    {
                        daocha_1_2.道岔单操 = Daocha_1.DanCao.单操;
                        daocha_1_2.锁闭状态 = Daocha_1.STATE.锁闭;
                        label52.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔2处于单锁状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_1_2.state == Daocha_1.STATE.锁闭)
                {
                    daocha_1_2.锁闭状态 = Daocha_1.STATE.空闲;
                    daocha_1_2.道岔单操 = Daocha_1.DanCao.进路;
                    label52.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔2没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_1_2.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔2已经锁闭！");
                }
                else
                {
                    daocha_1_2.锁闭状态 = Daocha_1.STATE.空闲;
                    label52.BackColor = Color.Purple;
                    label63.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_2.state == Daocha_1.STATE.空闲 && label52.BackColor == Color.Purple)
                {
                    //  daocha_1_1.锁闭状态 = Single_Switch.STATE.空闲;
                    label52.BackColor = Color.Black;
                    label63.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔2没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        //道岔4的单操
        private void button_Daocha4_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";

            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")          //目标转向定位
            {
                if (daocha_1_4.state == Daocha_1.STATE.空闲)
                {
                    button_Daocha4.BackColor = Color.Green;
                    daocha_1_4.定反位 = Daocha_1.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔4处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//反位
            {
                if (daocha_1_4.state == Daocha_1.STATE.空闲)
                {
                    button_Daocha4.BackColor = Color.Yellow;
                    daocha_1_4.定反位 = Daocha_1.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔4处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//单锁
            {
                if (daocha_1_4.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔8已经锁闭！");
                }
                else
                {
                    if (label81.BackColor != Color.Purple)//判断是否岔封
                    {
                        daocha_1_4.道岔单操 = Daocha_1.DanCao.单操;
                        daocha_1_4.锁闭状态 = Daocha_1.STATE.锁闭;
                        btn_dc3.Visible = true;
                        btn_dc3.BackColor = Color.Red;
                        label81.BackColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("道岔4处于单封状态");
                    }

                }
            }
            else if (first_button == "button_dj")//单解
            {
                if (daocha_1_4.state == Daocha_1.STATE.锁闭)
                {
                    daocha_1_4.锁闭状态 = Daocha_1.STATE.空闲;
                    daocha_1_4.道岔单操 = Daocha_1.DanCao.进路;
                    btn_dc3.Visible = false;
                    btn_dc3.BackColor = Color.White;
                    label81.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔4没有锁闭！");
                }
            }
            else if (first_button == "button_df")//单封
            {
                if (daocha_1_4.state == Daocha_1.STATE.锁闭)
                {
                    MessageBox.Show("道岔4已经锁闭！");
                }
                else
                {
                    daocha_1_4.锁闭状态 = Daocha_1.STATE.空闲;
                    btn_dc3.Visible = true;
                    btn_dc3.BackColor = Color.Purple;
                    label81.BackColor = Color.Purple;
                    label51.BackColor = Color.Purple;
                }
            }
            else if (first_button == "button_jf")//解封
            {
                if (daocha_1_4.state == Daocha_1.STATE.空闲 && label81.BackColor == Color.Purple)
                {
                    // daocha_1_4.锁闭状态 = Single_Switch.STATE.空闲;
                    btn_dc3.Visible = false;
                    btn_dc3.BackColor = Color.White;
                    label81.BackColor = Color.Black;
                    label51.BackColor = Color.Black;
                }
                else
                {
                    MessageBox.Show("道岔4没有单封！");
                }
            }
            list_button_Switch.Clear();
        }
        #endregion
        //双动道岔的单操

        private void button_Daocha1_3_Click(object sender, EventArgs e)
        {
            list_button_Switch.Add(((Button)sender).Name);
            string cz = "";
            foreach (string str in list_button_Switch)
            {
                if (cz != "")
                {
                    cz = cz + "+" + str;
                }
                else
                {
                    cz = str;//记录下对于单操的操作和道岔
                }
            }

            string first_button = cz.Split('+')[0];

            if (first_button == "button_zdw")//目标定位操作
            {
                if (daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.空闲 && daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.空闲)
                {
                    button_Daocha1_3.BackColor = Color.Green;
                    daocha_2_1_3.定反位上 = Daocha_2.DingFan.定位;
                    daocha_2_1_3.定反位下 = Daocha_2.DingFan.定位;
                }
                else
                {
                    MessageBox.Show("道岔1-3处于锁闭状态，不能转换为定位状态！");
                }
            }
            else if (first_button == "button_zfw")//目标反位操作
            {
                if (daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.空闲 && daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.空闲)
                {
                    button_Daocha1_3.BackColor = Color.Yellow;
                    daocha_2_1_3.定反位上 = Daocha_2.DingFan.反位;
                    daocha_2_1_3.定反位下 = Daocha_2.DingFan.反位;
                }
                else
                {
                    MessageBox.Show("道岔1-3处于锁闭状态，不能转换为反位状态！");
                }
            }
            else if (first_button == "button_ds")//目标操作单锁
            {
                if (daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.锁闭 && daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.锁闭)
                {
                    MessageBox.Show("道岔1-3已经锁闭！");
                }
                else
                {
                    if (label69.BackColor != Color.Purple)
                    {
                        daocha_2_1_3.道岔单操 = Daocha_2.DanCao.单操;
                        daocha_2_1_3.锁闭状态下 = Daocha_2.STATE.锁闭;
                        daocha_2_1_3.锁闭状态上 = Daocha_2.STATE.锁闭;

                        label69.BackColor = Color.Red;
                        btn_dc3.BackColor = Color.Red;
                        btn_dc3.Visible = true;
                        btn_dc1.BackColor = Color.Red;
                        btn_dc1.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("道岔1-3处于单封状态");
                    }

                }
            }
            else if (first_button == "button_dj")//目标单解
            {
                if (daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.锁闭 && daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.锁闭)
                {
                    daocha_2_1_3.锁闭状态下 = Daocha_2.STATE.空闲;
                    daocha_2_1_3.锁闭状态上 = Daocha_2.STATE.空闲;
                    daocha_2_1_3.道岔单操 = Daocha_2.DanCao.进路;
                    label69.BackColor = Color.Black;
                    btn_dc3.Visible = false;
                    btn_dc1.Visible = false;
                    btn_dc3.BackColor = Color.White;
                    btn_dc1.BackColor = Color.White;
                }
                else
                {
                    MessageBox.Show("道岔1-3没有锁闭！");
                }
            }
            else if (first_button == "button_df")//目标单封
            {
                if (daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.空闲 && daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.空闲)
                {
                    label69.BackColor = Color.Purple;
                    btn_dc3.BackColor = Color.Purple;
                    btn_dc3.Visible = true;
                    btn_dc1.BackColor = Color.Purple;
                    btn_dc1.Visible = true;
                }
                else
                {
                    MessageBox.Show("道岔1-3出于锁闭状态");
                }
            }
            else if (first_button == "button_dj")//单解
            {
                if (daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.空闲 && daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.空闲 && label69.BackColor == Color.Purple)
                {
                    // daocha_2_1_3.锁闭状态下 = Daocha_2.STATE.空闲;
                    label69.BackColor = Color.Black;
                    btn_dc3.Visible = false;
                    btn_dc1.Visible = false;
                    btn_dc3.BackColor = Color.White;
                    btn_dc1.BackColor = Color.White;
                }
                else //if (daocha_2_1_3.锁闭状态下 == Daocha_2.STATE.空闲 && daocha_2_1_3.锁闭状态上 == Daocha_2.STATE.空闲 && label69.BackColor != Color.Purple)
                {
                    MessageBox.Show("道岔1-3未单封！");
                }
            }
            list_button_Switch.Clear();
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
                if (first_button == "button_XJ" || first_button == "button_XFJ" || first_button == "button_X5" || first_button == "button_X3" || first_button == "button_X1" || first_button == "button_X2" || first_button == "button_X4" || first_button == "button_X6")
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
                else if (first_button == "button_SJ" || first_button == "button_SFJ" || first_button == "button_S5" || first_button == "button_S3" || first_button == "button_S1" || first_button == "button_S2" || first_button == "button_S4" || first_button == "button_S6")
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
                    if (d=="daocha_1_9")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha9.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha9.Refresh();
                        }
                    }
                    if (d=="daocha_1_5")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha5.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha5.Refresh();
                        }
                    }
                    if (d=="daocha_1_7")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha7.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha7.Refresh();
                        }
                    }
                    if (d=="daocha_1_11")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha11.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha11.Refresh();
                        }
                    }
                    if (d=="daocha_1_6")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha6.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha6.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha6.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha6.Refresh();
                        }
                    }
                    if (d=="daocha_1_2")
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
                    if (d=="daocha_1_4")
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
                    if (d=="daocha_1_8")
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha8.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha8.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha8.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha8.Refresh();
                        }
                    }
                    if (d.Contains("daocha_2_1_3"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (((Daocha_2)ht_DC_2[name]).定反位下 == ControlLib.Daocha_2.DingFan.定位 || ((Daocha_2)ht_DC_2[name]).定反位上 == ControlLib.Daocha_2.DingFan.定位)
                        {
                            Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                            c.Clear(Color.ForestGreen);
                            button_Daocha1_3.Refresh();
                        }
                        else
                        {
                            Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                            c.Clear(Color.Yellow);
                            button_Daocha1_3.Refresh();
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

            if (gd != "")
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
                    if (d.Contains("daocha_2"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string sx = st[2];
                        string zt = st[3];

                        switch (sx)
                        {
                            case "X":
                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.占用)
                                {
                                    return false;
                                }
                                break;
                            case "S":
                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                                {
                                    return false;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (d.Contains("daocha_1_2") || d.Contains("daocha_1_4") || d.Contains("daocha_1_5") || d.Contains("daocha_1_7"))
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
                    if (d.Contains("daocha_1_6") || d.Contains("daocha_1_8") || d.Contains("daocha_1_9") || d.Contains("daocha_1_11"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];

                        if (((Daocha_1_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1_1.STATE.占用)
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
                #region 道岔2标记
                foreach (string d in dcs)
                {
                    if (d.Contains("daocha_2"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string sx = st[2];
                        string zt = st[3];

                        switch (df)
                        {
                            //定位情况下
                            case "D":
                                //下行路定位
                                if (((Daocha_2)ht_DC_2[name]).定反位下 == ControlLib.Daocha_2.DingFan.定位)
                                {
                                    if (d.Contains("daocha_2"))
                                    {
                                        #region 道岔2空闲状态
                                        switch (sx)
                                        {
                                            case "S":
                                                if (zt == "S")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                                    }
                                                }
                                                else if (zt == "Z")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                                    }

                                                }
                                                else if (zt == "K")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.空闲)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                                    }
                                                }
                                                else
                                                {
                                                }
                                                break;
                                            case "X":
                                                if (zt == "S")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.锁闭)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                                    }
                                                }
                                                else if (zt == "Z")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.占用)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                                    }
                                                }
                                                else if (zt == "K")
                                                {
                                                    if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.空闲)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                                    }
                                                }
                                                break;
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    ((Daocha_2)ht_DC_2[name]).定反位下 = ControlLib.Daocha_2.DingFan.定位;
                                    #region 道岔2空闲状态
                                    switch (sx)
                                    {
                                        case "S":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            else
                                            {
                                            }
                                            break;
                                        case "X":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            break;
                                    }
                                    #endregion
                                }
                                //上行路定位
                                if (((Daocha_2)ht_DC_2[name]).定反位上 == ControlLib.Daocha_2.DingFan.定位)
                                {
                                    #region 道岔2空闲状态
                                    switch (sx)
                                    {
                                        case "S":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            else
                                            {
                                            }
                                            break;
                                        case "X":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            break;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ((Daocha_2)ht_DC_2[name]).定反位上 = ControlLib.Daocha_2.DingFan.定位;
                                    #region 道岔2空闲状态
                                    switch (sx)
                                    {
                                        case "S":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态上 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            else
                                            {
                                            }
                                            break;
                                        case "X":
                                            if (zt == "S")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.锁闭)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                                }
                                            }
                                            else if (zt == "Z")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.占用)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                                }
                                            }
                                            else if (zt == "K")
                                            {
                                                if (((Daocha_2)ht_DC_2[name]).锁闭状态下 == ControlLib.Daocha_2.STATE.空闲)
                                                {
                                                }
                                                else
                                                {
                                                    ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                                }
                                            }
                                            break;
                                    }
                                    #endregion
                                }
                                //特殊情况反馈
                                if (((Daocha_2)ht_DC_2[name]).定反位下 == ControlLib.Daocha_2.DingFan.定位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由反位转换为定位");
                                    return false;
                                }
                                if (((Daocha_2)ht_DC_2[name]).定反位上 == ControlLib.Daocha_2.DingFan.定位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由反位转换为定位");
                                    return false;
                                }
                                break;
                            //反位情况下
                            case "F":
                                //下行路线反位
                                if (((Daocha_2)ht_DC_2[name]).定反位下 == ControlLib.Daocha_2.DingFan.反位)
                                {
                                    #region 道岔轨道占用状态 S锁闭 Z占用 K空闲
                                    switch (zt)
                                    {
                                        case "S":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                            break;
                                        case "Z":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                            break;
                                        case "K":

                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                            break;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ((Daocha_2)ht_DC_2[name]).定反位下 = ControlLib.Daocha_2.DingFan.反位;
                                    #region 道岔轨道占用状态 S锁闭 Z占用 K空闲
                                    switch (zt)
                                    {
                                        case "S":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                            break;
                                        case "Z":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                            break;
                                        case "K":

                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                            break;
                                    }
                                    #endregion
                                }
                                //上行路线反位
                                if (((Daocha_2)ht_DC_2[name]).定反位上 == ControlLib.Daocha_2.DingFan.反位)
                                {
                                    #region 道岔轨道占用状态 S锁闭 Z占用 K空闲
                                    switch (zt)
                                    {
                                        case "S":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                            break;
                                        case "Z":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                            break;
                                        case "K":

                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                            break;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ((Daocha_2)ht_DC_2[name]).定反位上 = ControlLib.Daocha_2.DingFan.反位;
                                    #region 道岔轨道占用状态 S锁闭 Z占用 K空闲
                                    switch (zt)
                                    {
                                        case "S":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.锁闭;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.锁闭;
                                            break;
                                        case "Z":
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                            break;
                                        case "K":

                                            ((Daocha_2)ht_DC_2[name]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                                            ((Daocha_2)ht_DC_2[name]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                                            break;
                                    }
                                    #endregion
                                }
                                //特殊情况反馈
                                if (((Daocha_2)ht_DC_2[name]).定反位下 == ControlLib.Daocha_2.DingFan.反位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由定位转换为反位");
                                    return false;
                                }
                                if (((Daocha_2)ht_DC_2[name]).定反位上 == ControlLib.Daocha_2.DingFan.反位)
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
                #endregion
                #region 道岔1标记
                foreach (string d in dcs)
                {
                    if (d.Contains("daocha_1_2") || d.Contains("daocha_1_4") || d.Contains("daocha_1_5") || d.Contains("daocha_1_7"))
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
                    if (d.Contains("daocha_1_6") || d.Contains("daocha_1_8") || d.Contains("daocha_1_9") || d.Contains("daocha_1_11"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        switch (df)
                        {
                            case "D":
                                if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                                {
                                }
                                else
                                {
                                    ((Daocha_1_1)ht_DC_1[name]).定反位 = ControlLib.Daocha_1_1.DingFan.定位;
                                }
                                // jinlu_dc_xianshi(name, df);
                                //Show_nanzhan();
                                if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                                {
                                }
                                else
                                {
                                    MessageBox.Show("道岔" + name + "无法由反位转换为定位");
                                    return false;
                                }
                                break;
                            case "F":
                                if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.反位)
                                {
                                }
                                else
                                {
                                    ((Daocha_1_1)ht_DC_1[name]).定反位 = ControlLib.Daocha_1_1.DingFan.反位;
                                }
                                // jinlu_dc_xianshi(name, df);
                                //Show_nanzhan();
                                if (((Daocha_1_1)ht_DC_1[name]).定反位 == ControlLib.Daocha_1_1.DingFan.反位)
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
                    if (d.Contains("daocha_1_2") || d.Contains("daocha_1_4") || d.Contains("daocha_1_5") || d.Contains("daocha_1_7"))
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
                    if (d.Contains("daocha_1_6") || d.Contains("daocha_1_8") || d.Contains("daocha_1_9") || d.Contains("daocha_1_11"))
                    {
                        string[] st = d.Split('|');
                        string name = st[0];
                        string df = st[1];
                        string zt = st[2];
                        if (zt == "S")
                        {
                            if (((Daocha_1_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1_1.STATE.锁闭)
                            {
                            }
                            else
                            {
                                ((Daocha_1_1)ht_DC_1[name]).锁闭状态 = ControlLib.Daocha_1_1.STATE.锁闭;
                            }
                        }
                        else if (zt == "Z")
                        {
                            if (((Daocha_1_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1_1.STATE.占用)
                            {
                            }
                            else
                            {
                                ((Daocha_1_1)ht_DC_1[name]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
                            }
                        }
                        else if (zt == "K")
                        {
                            if (((Daocha_1_1)ht_DC_1[name]).锁闭状态 == ControlLib.Daocha_1_1.STATE.空闲)
                            {
                            }
                            else
                            {
                                //((Daocha_1)ht_DC_1[name]).锁闭状态 = DaoCha.Daocha_1.STATE.空闲;
                                ((Daocha_1_1)ht_DC_1[name]).锁闭状态 = Daocha_1_1.STATE.空闲;
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
                    if ((jinlu_xinxi.Split('+')[0] == "button_XJ") || (jinlu_xinxi.Split('+')[0] == "button_XFJ") || (jinlu_xinxi.Split('+')[0] == "button_X5") || (jinlu_xinxi.Split('+')[0] == "button_X3") || (jinlu_xinxi.Split('+')[0] == "button_X1") || (jinlu_xinxi.Split('+')[0] == "button_X2") || (jinlu_xinxi.Split('+')[0] == "button_X4") || (jinlu_xinxi.Split('+')[0] == "button_X6"))
                    {
                        fcplwch = ht_jlfcxx[jinlu.Key.ToString().Split(',')[0]].ToString();
                        ht_jlfcxx[jinlu.Key.ToString().Split(',')[0]] = fcplwch.Split(',')[0] + "," + fcplwch.Split(',')[1] + "," + fcplwch.Split(',')[2] + "," + fcplwch.Split(',')[3] + "," + fcplwch.Split(',')[4] + ",排列完成";
                    }

                    if ((jinlu_xinxi.Split('+')[0] == "button_SJ") || (jinlu_xinxi.Split('+')[0] == "button_SFJ") || (jinlu_xinxi.Split('+')[0] == "button_S5") || (jinlu_xinxi.Split('+')[0] == "button_S3") || (jinlu_xinxi.Split('+')[0] == "button_S1") || (jinlu_xinxi.Split('+')[0] == "button_S2") || (jinlu_xinxi.Split('+')[0] == "button_S4") || (jinlu_xinxi.Split('+')[0] == "button_S6"))
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
        private void 值班员登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LoadStatus)//系统未登陆
            {
                denglu D1 = new denglu();
                D1.Show();
            }
        }
        private void 值班员注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadStatus)//系统已登陆
            {
                zhuxiao Z1 = new zhuxiao();
                Z1.Show();
            }
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
        private void button1_Click(object sender, EventArgs e)//签收计划
        {
            button1.BackColor = Color.White;
            qianshoujihua J1 = new qianshoujihua();
            J1.Show();
        }
        private void button2_Click(object sender, EventArgs e)//签收命令
        {
            button2.BackColor = Color.White;
            qianshoumingling M1 = new qianshoumingling();
            M1.Show();
        }
        private void button3_Click(object sender, EventArgs e)//请求命令
        {
            button3.BackColor = Color.White;
            qingqiumingling Q1 = new qingqiumingling();
            Q1.Show();
        }
        private void button4_Click(object sender, EventArgs e)//模式转换
        {
            button4.BackColor = Color.White;
            moshizhuanhuan M1 = new moshizhuanhuan();
            M1.Show();
        }
        #endregion
        private void 网络连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建 1个客户端套接字 和1个负责监听服务端请求的线程
            Control.CheckForIllegalCrossThreadCalls = false;
            //定义一个套字节监听  包含3个参数(IP4寻址协议,流式连接,TCP协议)
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //需要获取文本框中的IP地址
            IPAddress ipaddress = IPAddress.Parse("192.168.1.211");
            //将获取的ip地址和端口号绑定到网络节点endpoint上
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse("21000"));
            //这里客户端套接字连接到网络节点(服务端)用的方法是Connect 而不是Bind
            try
            {
                socketClient.Connect(endpoint);
                MessageBox.Show("连接自律机成功！");
            }
            catch
            {
                MessageBox.Show("连接自律机失败！");
                return;
            }
            //创建一个线程 用于监听服务端发来的消息
            threadClient = new Thread(RecMsg);
            //将窗体线程设置为与后台同步
            threadClient.IsBackground = true;
            //启动线程
            threadClient.Start();
        }
        //接收信息
        public void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                try
                {
                    //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                    byte[] arrRecMsg = new byte[1024 * 1024];
                    //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                    int length = socketClient.Receive(arrRecMsg);
                    //将套接字获取到的字节数组转换为人可以看懂的字符串
                    recieve = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                    if (recieve.Length != 0)
                    {
                        if (recieve.Substring(2, 2) == "92")
                        {
                            if (recieve.Substring(14, 2) == "02")
                                MessageBox.Show("允许人工办理进路！");
                            else if (recieve.Substring(14, 2) == "03")
                                MessageBox.Show("不允许人工办理进路！");
                        }
                    
                        处理信息();
                        recieve = "";
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    //socketClient.Shutdown(SocketShutdown.Both);
                    //socketClient.Close();
                    break;
                }
            }
        }
        //发送信息
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            socketClient.Send(arrClientSendMsg);
        }
        private void 处理信息()
        {
            if (recieve.Length != 0)
            {
                if (recieve.Substring(2, 2) == "81")
                {
                    if (recieve.Substring(4, 2) == "01" || recieve.Substring(4, 2) == "02")
                    {
                        int n = recieve.Length / 44;
                       // xingcherizhi3.current.textBox5.Text = "车务终端收到" + n + "条增加阶段计划信息" + "\r\n";
                        for (int i = 1; i <= n; i++)
                        {
                            strRecMsg = recieve.Substring(0, 44);
                            阶段计划();
                            存储阶段计划();
                            更新列表();
                            阶段计划回执();
                            button1.BackColor = Color.Red;
                            recieve = recieve.Replace(strRecMsg, "");
                        }
                    }
                    else if (recieve.Substring(4, 2) == "03")
                    {
                    //    xingcherizhi3.current.textBox5.Text = "车务终端收到删除阶段计划信息" + "\r\n";
                        删除计划();
                    }
                }
                else if (recieve.Substring(2, 2) == "8A")
                {
                    if (recieve.Substring(4, 2) == "01")
                    {
                        //xingcherizhi3.current.textBox5.Text = "车务终端收到调度命令信息" + "\r\n";
                        调度命令();
                        存储调度命令();
                        调度命令回执();
                        button2.BackColor = Color.Red;
                    }
                    else if (recieve.Substring(4, 2) == "04")
                    {
                        //xingcherizhi3.current.textBox5.Text = "车务终端收到请求调度命令回执" + "\r\n";
                        请求命令();
                        button3.BackColor = Color.Red;
                    }
                }
                else if (recieve.Substring(2, 2) == "8F")
                    办理进路();
                else if (recieve.Substring(2, 2) == "7F")
                {
                    if (recieve.Length == 52)
                    {
                        红光带显示();
                    }
                }
                else if (recieve.Substring(2, 2) == "D8")
                    控制模式转换();
            }
        }
        #region 阶段计划
        private void 阶段计划()
        {
            if (strRecMsg.Length != 0)
            {
                if (strRecMsg.Substring(2, 2) == "81")//阶段计划
                {
                    if (strRecMsg.Substring(8, 2) == "07")
                        jsf = "北京南";
                    if (strRecMsg.Substring(8, 2) == "08")
                        jsf = "南京南";
                    if (strRecMsg.Substring(8, 2) == "09")
                        jsf = "上海虹桥";
                    xh = strRecMsg.Substring(10, 2);
                    xdsj = strRecMsg.Substring(12, 4) + "." + strRecMsg.Substring(16, 2) + "." + strRecMsg.Substring(18, 2) + " " + strRecMsg.Substring(20, 2) + ":" + strRecMsg.Substring(22, 2) + ":" + strRecMsg.Substring(24, 2);
                    cch = strRecMsg.Substring(26, 4);
                    if (strRecMsg.Substring(4, 2) == "01")//接车
                    {
                        jiefa = "接车";
                        ddcz = jsf;
                        if (strRecMsg.Substring(30, 2) == "07")
                            fccz = "北京南";
                        if (strRecMsg.Substring(30, 2) == "08")
                            fccz = "南京南";
                        if (strRecMsg.Substring(30, 2) == "09")
                            fccz = "上海虹桥";
                        ddsj = strRecMsg.Substring(32, 2) + ":" + strRecMsg.Substring(34, 2);
                        if (strRecMsg.Substring(36, 2) == "01")
                            jcgd = "1G";
                        if (strRecMsg.Substring(36, 2) == "02")
                            jcgd = "2G";
                        if (strRecMsg.Substring(36, 2) == "03")
                            jcgd = "3G";
                        if (strRecMsg.Substring(36, 2) == "04")
                            jcgd = "4G";
                        if (strRecMsg.Substring(36, 2) == "05")
                            jcgd = "5G";
                        if (strRecMsg.Substring(36, 2) == "06")
                            jcgd = "6G";
                        if (strRecMsg.Substring(38, 2) == "01")
                            fcgd = "1G";
                        if (strRecMsg.Substring(38, 2) == "02")
                            fcgd = "2G";
                        if (strRecMsg.Substring(38, 2) == "03")
                            fcgd = "3G";
                        if (strRecMsg.Substring(38, 2) == "04")
                            fcgd = "4G";
                        if (strRecMsg.Substring(38, 2) == "05")
                            fcgd = "5G";
                        if (strRecMsg.Substring(38, 2) == "06")
                            fcgd = "6G";
                    }
                    if (strRecMsg.Substring(4, 2) == "02")//发车
                    {
                        jiefa = "发车";
                        fccz = jsf;
                        if (strRecMsg.Substring(30, 2) == "07")
                            ddcz = "北京南";
                        if (strRecMsg.Substring(30, 2) == "08")
                            ddcz = "南京南";
                        if (strRecMsg.Substring(30, 2) == "09")
                            ddcz = "上海虹桥";
                        fcsj = strRecMsg.Substring(32, 2) + ":" + strRecMsg.Substring(34, 2);
                        if (strRecMsg.Substring(36, 2) == "01")
                            fcgd = "1G";
                        if (strRecMsg.Substring(36, 2) == "02")
                            fcgd = "2G";
                        if (strRecMsg.Substring(36, 2) == "03")
                            fcgd = "3G";
                        if (strRecMsg.Substring(36, 2) == "04")
                            fcgd = "4G";
                        if (strRecMsg.Substring(36, 2) == "05")
                            fcgd = "5G";
                        if (strRecMsg.Substring(36, 2) == "06")
                            fcgd = "6G";
                        if (strRecMsg.Substring(38, 2) == "01")
                            jcgd = "1G";
                        if (strRecMsg.Substring(38, 2) == "02")
                            jcgd = "2G";
                        if (strRecMsg.Substring(38, 2) == "03")
                            jcgd = "3G";
                        if (strRecMsg.Substring(38, 2) == "04")
                            jcgd = "4G";
                        if (strRecMsg.Substring(38, 2) == "05")
                            jcgd = "5G";
                        if (strRecMsg.Substring(38, 2) == "06")
                            jcgd = "6G";
                    }
                }
            }
        }
        #endregion
        #region 存储阶段计划
        private void 存储阶段计划()
        {
            if (strRecMsg.Substring(2, 2) == "81")
            {
                if (strRecMsg.Substring(4, 2) == "01")//接车
                {
                    string InsertSql = "insert into jiefachejihua(序号,接收方,接发车,下达时间,车次号,发车车站,到达车站,接车股道,发车股道,计划到达时间)values('" + xh + "','" + jsf + "','" + jiefa + "','" + xdsj + "','" + cch + "','" + fccz + "','" + ddcz + "','" + jcgd + "','" + fcgd + "','" + ddsj + "')";
                    com = new MySqlCommand(InsertSql, conn);
                    com.ExecuteNonQuery();
                }
                if (strRecMsg.Substring(4, 2) == "02")//发车
                {
                    string InsertSql2 = "insert into jiefachejihua(序号,接收方,接发车,下达时间,车次号,发车车站,到达车站,接车股道,发车股道,计划发车时间)values('" + xh + "','" + jsf + "','" + jiefa + "','" + xdsj + "','" + cch + "','" + fccz + "','" + ddcz + "','" + jcgd + "','" + fcgd + "','" + fcsj + "')";
                    com = new MySqlCommand(InsertSql2, conn);
                    com.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region 更新列表
        private void 更新列表()
        {
            if (jsf == "上海虹桥")
            {
                车次号();
                if (jiefa == "接车")
                {
                    int index1 = this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[index1].Cells[0].Value = xh;
                    this.dataGridView1.Rows[index1].Cells[1].Value = cch;
                    this.dataGridView1.Rows[index1].Cells[2].Value = jcgd;
                    this.dataGridView1.Rows[index1].Cells[3].Value = ddsj;
                    this.dataGridView1.Rows[index1].Cells[4].Value = true;
                    jinluanniu(jiefa, jcgd, fcgd);
                    this.dataGridView1.Rows[index1].Cells[5].Value = jlan;
                    this.dataGridView1.Rows[index1].Cells[6].Value = "未触发";
                }
                if (jiefa == "发车")
                {
                    int index2 = this.dataGridView2.Rows.Add();
                    this.dataGridView2.Rows[index2].Cells[0].Value = xh;
                    this.dataGridView2.Rows[index2].Cells[1].Value = cch;
                    this.dataGridView2.Rows[index2].Cells[2].Value = fcgd;
                    this.dataGridView2.Rows[index2].Cells[3].Value = fcsj;
                    this.dataGridView2.Rows[index2].Cells[4].Value = true;
                    jinluanniu(jiefa, jcgd, fcgd);
                    this.dataGridView2.Rows[index2].Cells[5].Value = jlan;
                    this.dataGridView2.Rows[index2].Cells[6].Value = "未触发";
                }
            }
        }
        #endregion
        #region 阶段计划回执
        private void 阶段计划回执()
        {
            //xingcherizhi3.current.textBox5.Text = "车务终端发送阶段计划自动回执" + "\r\n";
            if (jsf == "北京南")
            {
                fsf = "07";
            }
            if (jsf == "南京南")
            {
                fsf = "08";
            }
            if (jsf == "上海虹桥")
            {
                fsf = "09";
            }
            DateTime dt = DateTime.Now;
            string date = dt.ToLongDateString();
            string time = dt.ToLongTimeString();
            hzsj = DateTime.Now.ToString("yyyyMMddhhmmss");
            senddata = "AB8104" + fsf + "01" + xh + xdsj.Substring(0, 4) + xdsj.Substring(5, 2) + xdsj.Substring(8, 2) + xdsj.Substring(11, 2) +xdsj.Substring(14, 2) +xdsj.Substring(17, 2) + hzsj + "AC";
            ClientSendMsg(senddata);
        }
        #endregion
        #region 删除计划
        private void 删除计划()
        {
            if (recieve.Substring(8, 2) == "07")
                jsf = "北京南";
            if (recieve.Substring(8, 2) == "08")
                jsf = "南京南";
            if (recieve.Substring(8, 2) == "09")
                jsf = "上海虹桥";
            xh = recieve.Substring(10, 2);
            jhsj = recieve.Substring(12, 2) + ":" + recieve.Substring(14, 2);
            if (jsf == "上海虹桥")
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count-1; i++)
                {
                    if (xh == this.dataGridView1.Rows[i].Cells[0].Value.ToString() && jhsj == this.dataGridView1.Rows[i].Cells[3].Value.ToString())
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        this.dataGridView1.Rows.Remove(row);
                    }
                }
                for (int j = 0; j < this.dataGridView2.Rows.Count-1; j++)
                {
                    if (xh == this.dataGridView2.Rows[j].Cells[0].Value.ToString() && jhsj == this.dataGridView2.Rows[j].Cells[3].Value.ToString())
                    {
                        DataGridViewRow row = dataGridView2.Rows[j];
                        this.dataGridView2.Rows.Remove(row);
                    }
                }
            }
            MySqlDataReader dr = null;
            com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                if (jsf == dr.GetString(dr.GetOrdinal("接收方")) && xh == dr.GetString(dr.GetOrdinal("序号")))
                {
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "接车")
                    {
                        if (jhsj == dr.GetString(dr.GetOrdinal("计划到达时间")))
                        {
                            scxh = xh;
                            scsj = "接车";
                        }
                    }
                    if (dr.GetString(dr.GetOrdinal("接发车")) == "发车")
                    {
                        if (jhsj == dr.GetString(dr.GetOrdinal("计划发车时间")))
                        {
                            scxh = xh;
                            scsj = "发车";
                        }
                    }
                }
            }
            dr.Close();
            if (scsj == "接车")
            {
                com = new MySqlCommand("delete from jiefachejihua where 序号='" + scxh + "' and 计划到达时间='" + jhsj + "'", conn);
                com.ExecuteNonQuery();
            }
            if (scsj == "发车")
            {
                com = new MySqlCommand("delete from jiefachejihua where 序号='" + scxh + "' and 计划发车时间='" + jhsj + "'", conn);
                com.ExecuteNonQuery();
            }
        }
        #endregion
        #region 调度命令
        private void 调度命令()
        {
            if (recieve.Length != 0)
            {
                if (recieve.Substring(2, 2) == "8A")//调度命令
                {
                    mlxh = recieve.Substring(10, 2);
                    mlbh = recieve.Substring(12, 4);
                    mllx = recieve.Substring(16, 2);
                    flsj = recieve.Substring(18, 4) + "." + recieve.Substring(22, 2) + "." + recieve.Substring(24, 2) + " " + recieve.Substring(26, 2) + ":" + recieve.Substring(28, 2) + ":" + recieve.Substring(30, 2);
                    mlnr = recieve.Substring(32, recieve.Length - 34);
                    if (recieve.Substring(8, 2) == "07")
                        sldw = "北京南";
                    if (recieve.Substring(8, 2) == "08")
                        sldw = "南京南";
                    if (recieve.Substring(8, 2) == "09")
                        sldw = "上海虹桥";
                }
            }
        }
        private void 存储调度命令()
        {
            if (recieve.Length != 0)
            {
                if (recieve.Substring(2, 2) == "8A")//调度命令
                {
                    string InsertSql1 = "insert into diaodumingling(命令序号,命令编号,受令单位,命令类型,发令时间,命令内容)values('" + mlxh + "','" + mlbh + "','" + sldw + "','" + mllx + "','" + flsj + "','" + mlnr + "')";
                    com = new MySqlCommand(InsertSql1, conn);
                    com.ExecuteNonQuery();
                }
            }
        }
        private void 调度命令回执()
        {
            xingcherizhi3.current.textBox5.Text = "车务终端发送调度命令自动回执" + "\r\n";
            if (sldw == "北京南")
            {
                fldw = "07";
            }
            if (sldw == "南京南")
            {
                fldw = "08";
            }
            if (sldw == "上海虹桥")
            {
                fldw = "09";
            }
            DateTime dt = DateTime.Now;
            string date = dt.ToLongDateString();
            string time = dt.ToLongTimeString();
            slsj = DateTime.Now.ToString("yyyyMMddhhmmss");
            senddata2 = "AB8A0201" + fldw + mlxh + mlbh + slsj + "AC";
            ClientSendMsg(senddata2);
        }
        private void 请求命令()
        {
            if (recieve.Substring(recieve.Length - 4, 2) == "01")
                qqjg = "同意";
            else if (recieve.Substring(recieve.Length - 4, 2) == "02")
                qqjg = "拒绝";
            string InsertSql2 = "update qingqiumingling set 命令编号='" + recieve.Substring(12, 4) + "',请求结果='" + qqjg + "' where 命令序号='" + recieve.Substring(10, 2) + "'";
            com = new MySqlCommand(InsertSql2, conn);
            com.ExecuteNonQuery();
        }
        #endregion
        #region 办理进路
        private void 办理进路()
        {
            cch = recieve.Substring(10, 4);
            车次号();
            if (recieve.Substring(4, 2) == "01")
            {
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() == cch)
                        dataGridView1.Rows[i].Cells[6].Value = "排列完成";
                }
            }
            if (recieve.Substring(4, 2) == "02")
            {
                for (int j = 0; j < dataGridView2.Rows.Count-1; j++)
                {
                    if (dataGridView2.Rows[j].Cells[1].Value.ToString() == cch)
                        dataGridView2.Rows[j].Cells[6].Value = "排列完成";
                }
            }
            dddc1 = recieve.Substring(14, 2);
            ddzt1 = recieve.Substring(16, 2);
            if (dddc1 == "14")
                name1 = "daocha_1_5";
            if (dddc1 == "15")
                name1 = "daocha_1_7";
            dddc2 = recieve.Substring(18, 2);
            ddzt2 = recieve.Substring(20, 2);
            if (dddc2 == "13")
                name4 = "daocha_1_9";
            if (dddc2 == "16")
                name4 = "daocha_1_11";
            单动道岔1();
            单动道岔2();
            sddc = recieve.Substring(22, 2);
            sdzt = recieve.Substring(24, 2);
            if (sddc == "11")
                双动道岔();
            道岔按钮();
            gd = recieve.Substring(26, 2);
            gdzt = recieve.Substring(28, 2);
            if (gd == "50")
                name2 = "G3";
            if (gd == "51")
                name2 = "G1";
            if (gd == "52")
                name2 = "G2";
            if (gd == "53")
                name2 = "G4";
            股道();
            xhj = recieve.Substring(30, 2);
            xhzt = recieve.Substring(32, 2);
            if (xhj == "21")
                name3 = "xinhaoji_X";
            if (xhj == "22")
                name3 = "xinhaoji_XF";
            if (xhj == "24")
                name3 = "xinhaoji_S3";
            if (xhj == "25")
                name3 = "xinhaoji_S1";
            if (xhj == "26")
                name3 = "xinhaoji_S2";
            if (xhj == "27")
                name3 = "xinhaoji_S4";
            信号机();
        }
        #endregion
        #region 单动道岔1
        private void 单动道岔1()
        {
            if (ddzt1 == "01")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.定位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.空闲;
            }
            else if (ddzt1 == "02")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.定位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.锁闭;
            }
            else if (ddzt1 == "03")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.定位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
            }
            else if (ddzt1 == "04")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.反位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.空闲;
            }
            else if (ddzt1 == "05")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.反位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.锁闭;
            }
            else if (ddzt1 == "06")
            {
                ((Daocha_1)ht_DC_1[name1]).定反位 = ControlLib.Daocha_1.DingFan.反位;
                ((Daocha_1)ht_DC_1[name1]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
            }
        }
        #endregion
        #region 单动道岔2
        private void 单动道岔2()
        {
            if (ddzt2 == "01")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.定位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.空闲;
            }
            else if (ddzt2 == "02")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.定位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.锁闭;
            }
            else if (ddzt2 == "03")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.定位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
            }
            else if (ddzt2 == "04")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.反位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.空闲;
            }
            else if (ddzt2 == "05")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.反位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.锁闭;
            }
            else if (ddzt2 == "06")
            {
                ((Daocha_1_1)ht_DC_1[name4]).定反位 = ControlLib.Daocha_1_1.DingFan.反位;
                ((Daocha_1_1)ht_DC_1[name4]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
            }
        }
        #endregion
        #region 双动道岔
        private void 双动道岔()
        {
          
            if (sdzt == "01")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
   
            }
            
            else if (sdzt == "02")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "03")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            
            else if (sdzt == "04")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down= ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            } 
            else if (sdzt == "05")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "06")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "07")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "08")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
           
            else if (sdzt == "09")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.定位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "10")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up= ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            
            else if (sdzt == "11")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up= ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "12")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            
            else if (sdzt == "13")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up= ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "14")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "15")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "16")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.锁闭;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }
            else if (sdzt == "17")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }          
            else if (sdzt == "18")
            {
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_down = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_down = ControlLib.Daocha_2.STATE.占用;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).DF_flag_up = ControlLib.Daocha_2.DingFan.反位;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).state_up = ControlLib.Daocha_2.STATE.空闲;
                ((Daocha_2)ht_DC_2["daocha_2_1_3"]).Draw();
            }

        }
        #endregion
        #region 道岔按钮
        private void 道岔按钮()
        {
            if (name1 == "daocha_1_5")
            {
                if (daocha_1_5.定反位 == Daocha_1.DingFan.定位)
                {
                    Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                    c.Clear(Color.ForestGreen);
                    button_Daocha5.Refresh();
                }
                else
                {
                    Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                    c.Clear(Color.Yellow);
                    button_Daocha5.Refresh();
                }
            }
            else if (name1 == "daocha_1_7")
            {
                if (daocha_1_7.定反位 == Daocha_1.DingFan.定位)
                {
                    Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                    c.Clear(Color.ForestGreen);
                    button_Daocha7.Refresh();
                }
                else
                {
                    Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                    c.Clear(Color.Yellow);
                    button_Daocha7.Refresh();
                }
            }
            else if (name4 == "daocha_1_9")
            {
                if (daocha_1_9.定反位 == Daocha_1_1.DingFan.定位)
                {
                    Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                    c.Clear(Color.ForestGreen);
                    button_Daocha9.Refresh();
                }
                else
                {
                    Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                    c.Clear(Color.Yellow);
                    button_Daocha9.Refresh();
                }
            }
            else if (name4 == "daocha_1_11")
            {
                if (daocha_1_11.定反位 == Daocha_1_1.DingFan.定位)
                {
                    Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                    c.Clear(Color.ForestGreen);
                    button_Daocha11.Refresh();
                }
                else
                {
                    Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                    c.Clear(Color.Yellow);
                    button_Daocha11.Refresh();
                }
            }
            if (sddc == "05")
            {
                if (daocha_2_1_3.定反位上 == Daocha_2.DingFan.定位 || daocha_2_1_3.定反位下 == Daocha_2.DingFan.定位)
                {
                    Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                    c.Clear(Color.ForestGreen);
                    button_Daocha1_3.Refresh();
                }
                else
                {
                    Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                    c.Clear(Color.Yellow);
                    button_Daocha1_3.Refresh();
                }
            }
        }
        #endregion
        private void 股道()
        {
            if (gdzt == "01")
            {
                ((Dangui)ht_zc[name2]).flag_zt = 3;
                ((Dangui)ht_zc[name2]).Drawpic();
            }
            else if (gdzt == "02")
            {
                ((Dangui)ht_zc[name2]).flag_zt = 2;
                ((Dangui)ht_zc[name2]).Drawpic();
            }
            else if (gdzt == "03")
            {
                ((Dangui)ht_zc[name2]).flag_zt = 1;
               ((Dangui)ht_zc[name2]).Drawpic();
            }
            //G3.flag_zt = 1;
           // G3.Drawpic();
        }
        private void 信号机()
        {
            if (xhzt == "01")
            {
                ((Xinhaoji2)(ht_zc[name3])).X_flag = 5;
                ((Xinhaoji2)(ht_zc[name3])).drawpic();
            }
            else if (xhzt == "02")
            {
                ((Xinhaoji2)(ht_zc[name3])).X_flag = 3;
                ((Xinhaoji2)(ht_zc[name3])).drawpic();
            }
            else if (xhzt == "03")
            {
                ((Xinhaoji2)(ht_zc[name3])).X_flag = 1;
                ((Xinhaoji2)(ht_zc[name3])).drawpic();
            }
            else if (xhzt == "04")
            {
                ((Xinhaoji2)(ht_zc[name3])).X_flag = 2;
                ((Xinhaoji2)(ht_zc[name3])).drawpic();
            }
        }
        List <string> gdname=new List<string>(){"G3","G1","G2","G4","X1JG","X2JG","IAG","S3LQ","S2LQ","IIAG"};
        List<string> dcname1 = new List<string>(){ "daocha_1_5", "daocha_1_7"};
        List<string> dcname2 = new List<string>(){ "daocha_2_1_3"};
        #region 红光带显示
        private void 红光带显示()
        {

            for(int i=0;i<10;i++)
            {
              name2=gdname[i];
              gdzt=recieve.Substring(10+i*2,2);
              股道();
            } 
            for(int j=0;j<1;j++)
            {
              name1=dcname1[j];
              ddzt1=recieve.Substring(32,2);
              单动道岔1();
            }
             for(int l=1;l<2;l++)
            {
              name1=dcname1[l];
              ddzt1=recieve.Substring(34,2);
              单动道岔1();
            }
             for (int k = 0; k < 1; k++)
             {
                 name_dc = dcname2[k];
                 sdzt = recieve.Substring(38, 2);

                 双动道岔();
             }
        }

        #endregion

        private void 控制模式转换()
        {
            if (recieve.Substring(10, 2) == "01")//中心控制模式
            {
                if (recieve.Substring(8, 2) == "01")
                {
                    button4.BackColor = Color.Red;
                    moshi = recieve;
                }
                else if (recieve.Substring(8, 2) == "05")
                {
                    danniu_left.显示状态 = Danniu.Xianshi.默认;
                    danniu_middle.显示状态 = Danniu.Xianshi.默认;
                    danniu0.显示状态 = Danniu.Xianshi.绿;
                    danniu1.显示状态 = Danniu.Xianshi.默认;
                    danniu2.显示状态 = Danniu.Xianshi.默认;
                }
            }
            else if (recieve.Substring(10, 2) == "02")//车站控制模式
            {
                if (recieve.Substring(8, 2) == "01")
                {
                    button4.BackColor = Color.Red;
                    moshi = recieve;
                }
            }
            else if (recieve.Substring(10, 2) == "03")//分散自律模式
            {
                if (recieve.Substring(8, 2) == "01")
                {
                    button4.BackColor = Color.Red;
                    moshi = recieve;
                }
                else if (recieve.Substring(8, 2) == "05")
                {
                    danniu_left.显示状态 = Danniu.Xianshi.默认;
                    danniu_middle.显示状态 = Danniu.Xianshi.默认;
                    danniu0.显示状态 = Danniu.Xianshi.默认;
                    danniu1.显示状态 = Danniu.Xianshi.绿;
                    danniu2.显示状态 = Danniu.Xianshi.默认;
                }
            }
            else if (recieve.Substring(10, 2) == "04")//非常站控模式
            {
                if (recieve.Substring(8, 2) == "04")
                {
                    danniu_left.显示状态 = Danniu.Xianshi.红;
                    danniu_middle.显示状态 = Danniu.Xianshi.黄;
                    danniu0.显示状态 = Danniu.Xianshi.默认;
                    danniu1.显示状态 = Danniu.Xianshi.默认;
                    danniu2.显示状态 = Danniu.Xianshi.默认;
                }
            }
        }
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
                        this.dataGridView1.Rows[i1].Cells[6].Value = "进路已占用";
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
                        this.dataGridView2.Rows[i2].Cells[6].Value = "进路已占用";
                        this.dataGridView2.Rows[i2].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }
        #endregion
        #region 道岔控件与button关联第一部分
        private void daocha_1_9_Custom(object sender, Daocha_1_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha9.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha9.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha9.Refresh();
            }
        }
        private void daocha_1_5_Custom(object sender, Daocha_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha5.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha5.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha5.Refresh();
            }
        }
        private void daocha_1_7_Custom(object sender, Daocha_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha7.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha7.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha7.Refresh();
            }
        }
        private void daocha_1_11_Custom(object sender, Daocha_1_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha11.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha11.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha11.Refresh();
            }
        }
        private void daocha_1_6_Custom(object sender, Daocha_1_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha6.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha6.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha6.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha6.Refresh();
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
        private void daocha_1_8_Custom(object sender, Daocha_1_1.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha8.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha8.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha8.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha8.Refresh();
            }
        }
        private void daocha_2_1_3_Custom(object sender, Daocha_2.CustomEventArgs e)
        {
            if (e.Flag)
            {
                Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                c.Clear(Color.ForestGreen);
                button_Daocha1_3.Refresh();
            }
            else
            {
                Graphics c = Graphics.FromImage(button_Daocha1_3.BackgroundImage);
                c.Clear(Color.Yellow);
                button_Daocha1_3.Refresh();
            }
        }
        #endregion
        private void 走车_Tick(object sender, EventArgs e)
        {
            if (ZCXL[t] != null)
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
                            label56.Location = new Point(((Dangui)ht_zc[ZCXL[t]]).Location.X + (((Dangui)ht_zc[ZCXL[t]]).Size.Width - 23) / 2, ((Dangui)ht_zc[ZCXL[t]]).Location.Y);
                            t++;
                        }
                    }
                    else if (ZCXL[t].Contains("daocha_2"))
                    {
                        //反位情况下，直接改成占用
                        if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.反位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.反位))
                        {
                            ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                            ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                            //改变车次号位置
                            label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 12) / 2);
                        }
                        //定位情况下，先判断锁闭位置，再改成占用
                        else if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.定位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.定位))
                        {
                            if (((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 6));
                            }
                            else
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y);
                            }
                        }
                        t++;
                    }
                    else if (ZCXL[t].Contains("daocha_1"))
                    {
                        if (ZCXL[t].Contains("daocha_1_7"))
                        {
                            ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                            //车次号操作
                            if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y);
                            }
                            else
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                            }
                        }
                        else if (ZCXL[t].Contains("daocha_1_5"))
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
                        else if (ZCXL[t].Contains("daocha_1_9") || ZCXL[t].Contains("daocha_1_11"))
                        {
                            ((Daocha_1_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
                            //车次号操作
                            if (((Daocha_1_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                            }
                            else
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
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
                        else if (ZCXL[t].Contains("daocha_2"))
                        {
                            //反位情况下，直接改成占用
                            if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.反位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.反位))
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 12) / 2);
                            }
                            //定位情况下，先判断锁闭位置，再改成占用
                            else if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.定位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.定位))
                            {
                                if (((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                                {
                                    ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 6));
                                }
                                else
                                {
                                    ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y);
                                }
                            }
                            t++;
                        }
                        else if (ZCXL[t].Contains("daocha_1"))
                        {
                            if (ZCXL[t].Contains("daocha_1_7"))
                            {
                                ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y);
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                                }
                            }
                            else if (ZCXL[t].Contains("daocha_1_5"))
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
                            else if (ZCXL[t].Contains("daocha_1_9") || ZCXL[t].Contains("daocha_1_11"))
                            {
                                ((Daocha_1_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
                                //车次号操作
                                if (((Daocha_1_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                                }
                                else
                                {
                                    //改变车次号位置
                                    label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
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
                    else if (ZCXL[t].Contains("daocha_2"))
                    {
                        //反位情况下，直接改成占用
                        if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.反位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.反位))
                        {
                            ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                            ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                            //改变车次号位置
                            label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 12) / 2);
                        }
                        //定位情况下，先判断锁闭位置，再改成占用
                        else if ((((Daocha_2)ht_DC_2[ZCXL[t]]).定反位上 == ControlLib.Daocha_2.DingFan.定位) && (((Daocha_2)ht_DC_2[ZCXL[t]]).定反位下 == ControlLib.Daocha_2.DingFan.定位))
                        {
                            if (((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 == ControlLib.Daocha_2.STATE.锁闭)
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态上 = ControlLib.Daocha_2.STATE.占用;
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Height - 6));
                            }
                            else
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[t]]).锁闭状态下 = ControlLib.Daocha_2.STATE.占用;
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_2)ht_DC_2[ZCXL[t]]).Location.X + (((Daocha_2)ht_DC_2[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_2)ht_DC_2[ZCXL[t]]).Location.Y);
                            }
                        }
                        t++;
                    }
                    else if (ZCXL[t].Contains("daocha_1"))
                    {
                        if (ZCXL[t].Contains("daocha_1_7"))
                        {
                            ((Daocha_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1.STATE.占用;
                            //车次号操作
                            if (((Daocha_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1.DingFan.定位)
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y);
                            }
                            else
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
                            }
                        }
                        else if (ZCXL[t].Contains("daocha_1_5"))
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
                        else if (ZCXL[t].Contains("daocha_1_9") || ZCXL[t].Contains("daocha_1_11"))
                        {
                            ((Daocha_1_1)ht_DC_1[ZCXL[t]]).锁闭状态 = ControlLib.Daocha_1_1.STATE.占用;
                            //车次号操作
                            if (((Daocha_1_1)ht_DC_1[ZCXL[t]]).定反位 == ControlLib.Daocha_1_1.DingFan.定位)
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 6));
                            }
                            else
                            {
                                //改变车次号位置
                                label56.Location = new Point(((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.X + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Width - 23) / 2, ((Daocha_1_1)ht_DC_1[ZCXL[t]]).Location.Y + (((Daocha_1_1)ht_DC_1[ZCXL[t]]).Size.Height - 12) / 2);
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
                for (k = 1; k < t - 1; k++)
                {
                    if ((ZCXL[k].Contains("G")) || (ZCXL[k].Contains("dangui")) || (ZCXL[k].Contains("LQ")))
                    {
                        if (((Dangui)ht_zc[ZCXL[k]]).flag_zt == 1)
                        {
                            ((Dangui)ht_zc[ZCXL[k]]).flag_zt = 3;
                            ((Dangui)ht_zc[ZCXL[k]]).Drawpic();
                        }
                    }
                    else if (ZCXL[k].Contains("daocha_1"))
                    {
                        if (ZCXL[k].Contains("daocha_1_5") || ZCXL[k].Contains("daocha_1_7"))
                            ((Daocha_1)ht_DC_1[ZCXL[k]]).锁闭状态 = ControlLib.Daocha_1.STATE.空闲;
                        else if(ZCXL[k].Contains("daocha_1_9") || ZCXL[k].Contains("daocha_1_11"))
                            ((Daocha_1_1)ht_DC_1[ZCXL[k]]).锁闭状态 = ControlLib.Daocha_1_1.STATE.空闲;
                    }
                    else if (ZCXL[k].Contains("daocha_2"))
                    {
                        if ((((Daocha_2)ht_DC_2[ZCXL[k]]).定反位上 == ControlLib.Daocha_2.DingFan.反位) && (((Daocha_2)ht_DC_2[ZCXL[k]]).定反位下 == ControlLib.Daocha_2.DingFan.反位))
                        {
                            ((Daocha_2)ht_DC_2[ZCXL[k]]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                            ((Daocha_2)ht_DC_2[ZCXL[k]]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                        }
                        //定位情况下，先判断占用位置，再改成空闲
                        else if ((((Daocha_2)ht_DC_2[ZCXL[k]]).定反位上 == ControlLib.Daocha_2.DingFan.定位) && (((Daocha_2)ht_DC_2[ZCXL[k]]).定反位下 == ControlLib.Daocha_2.DingFan.定位))
                        {
                            if (((Daocha_2)ht_DC_2[ZCXL[k]]).锁闭状态上 == ControlLib.Daocha_2.STATE.占用)
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[k]]).锁闭状态上 = ControlLib.Daocha_2.STATE.空闲;
                            }
                            else
                            {
                                ((Daocha_2)ht_DC_2[ZCXL[k]]).锁闭状态下 = ControlLib.Daocha_2.STATE.空闲;
                            }
                        }
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
        private void 申请中心控制模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string senddata = "ABD809020101AC";
            ClientSendMsg(senddata);
        }
        private void 申请车站控制模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string senddata = "ABD809020102AC";
            ClientSendMsg(senddata);
        }
        private void 申请分散自律模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string senddata = "ABD809020103AC";
            ClientSendMsg(senddata);
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
        string JCGD = "";//接车股道
        string FCGD = "";//发车股道
        #region 列车进路按钮
        public void jinluanniu(string jiefa, string jcgd, string fcgd)
        {
            if (jiefa == "接车")
            {
                JCGD = jcgd;
                if (fcgd == "1G" || fcgd == "3G")
                    FCGD = "1G";
                if (fcgd == "2G" || fcgd == "4G")
                    FCGD = "2G";
                if (JCGD == "1G" && FCGD == "1G")
                {
                    jlan = "XJ-S1";
                }
                if (JCGD == "1G" && FCGD == "2G")
                {
                    jlan = "XFJ-S1";
                }
                if (JCGD == "2G" && FCGD == "2G")
                {
                    jlan = "XFJ-S2";
                }
                if (JCGD == "3G" && FCGD == "1G")
                {
                    jlan = "XJ-S3";
                }
                if (JCGD == "3G" && FCGD == "2G")
                {
                    jlan = "XFJ-S3";
                }
                if (JCGD == "4G" && FCGD == "2G")
                {
                    jlan = "XFJ-S4";
                }
                if (JCGD == "5G" && FCGD == "1G")
                {
                    jlan = "XJ-S5";
                }
                if (JCGD == "5G" && FCGD == "2G")
                {
                    jlan = "XFJ-S5";
                }
                if (JCGD == "6G" && FCGD == "2G")
                {
                    jlan = "XFJ-S6";
                }
            }
            if (jiefa == "发车")
            {
                FCGD = fcgd;
                if (jcgd == "1G" || jcgd == "3G")
                    JCGD = "1G";
                if (jcgd == "2G" || jcgd == "4G")
                    JCGD = "2G";
                if (FCGD == "1G" && JCGD == "1G")
                {
                    jlan = "S1-XJ";
                }
                if (FCGD == "1G" && JCGD == "2G")
                {
                    jlan = "S1-XFJ";
                }
                if (FCGD == "2G" && JCGD == "2G")
                {
                    jlan = "S2-XFJ";
                }
                if (FCGD == "3G" && JCGD == "1G")
                {
                    jlan = "S3-XJ";
                }
                if (FCGD == "3G" && JCGD == "2G")
                {
                    jlan = "S3-XFJ";
                }
                if (FCGD == "4G" && JCGD == "2G")
                {
                    jlan = "S4-XFJ";
                }
                if (FCGD == "5G" && JCGD == "1G")
                {
                    jlan = "S5-XJ";
                }
                if (FCGD == "5G" && JCGD == "2G")
                {
                    jlan = "S5-XFJ";
                }
                if (FCGD == "6G" && JCGD == "2G")
                {
                    jlan = "S6-XFJ";
                }
            }
        }
        #endregion
        public string jiefa = "";//接发车
        public string jlan = "";//进路按钮
        public string senddata1 = "";//人工办理进路信息
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            jiefa = "接车";
            if (dataGridView1.SelectedCells.Count != 0)
            {
                int selRow = dataGridView1.SelectedRows[0].Index;
                cch = dataGridView1.Rows[selRow].Cells[1].Value.ToString();
                车次号1();
                this.dataGridView1.Rows[selRow].Cells[4].Value = false;
                senddata1 = "AB92010906" + cch + "01AC";
                ClientSendMsg(senddata1);
            }
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            jiefa = "发车";
            if (dataGridView2.SelectedCells.Count != 0)
            {
                int selRow = dataGridView2.SelectedRows[0].Index;
                cch = dataGridView2.Rows[selRow].Cells[1].Value.ToString();
                车次号1();
                this.dataGridView2.Rows[selRow].Cells[4].Value = false;
                senddata1 = "AB92020906" + cch + "01AC";
                ClientSendMsg(senddata1);
            }
        }
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Right)
            {
                DialogResult result;
                result = MessageBox.Show("删除列车进路！", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    int selRow = dataGridView1.SelectedRows[0].Index;
                    cch = dataGridView1.Rows[selRow].Cells[1].Value.ToString();
                    车次号1();
                    jhsj = dataGridView1.Rows[selRow].Cells[3].Value.ToString();
                    DataGridViewRow row = dataGridView1.Rows[selRow];
                    this.dataGridView1.Rows.Remove(row);
                    MySqlDataReader dr = null;
                    com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (jhsj == dr.GetString(dr.GetOrdinal("计划到达时间")) && cch == dr.GetString(dr.GetOrdinal("车次号")))
                        {
                            string scjh = jhsj;
                            string sclc = cch;
                        }
                    }
                    dr.Close();
                    com = new MySqlCommand("delete from jiefachejihua where 计划到达时间='" + jhsj + "' and 车次号='" + cch + "'", conn);
                    com.ExecuteNonQuery();
                }
            }
        }
        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Right)
            {
                DialogResult result;
                result = MessageBox.Show("删除列车进路！", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    int selRow = dataGridView2.SelectedRows[0].Index;
                    cch = dataGridView2.Rows[selRow].Cells[1].Value.ToString();
                    车次号1();
                    jhsj = dataGridView2.Rows[selRow].Cells[3].Value.ToString();
                    DataGridViewRow row = dataGridView2.Rows[selRow];
                    this.dataGridView2.Rows.Remove(row);
                    MySqlDataReader dr = null;
                    com = new MySqlCommand("SELECT*FROM jiefachejihua", conn);
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (jhsj == dr.GetString(dr.GetOrdinal("计划发车时间")) && cch == dr.GetString(dr.GetOrdinal("车次号")))
                        {
                            string scjh = jhsj;
                            string sclc = cch;
                        }
                    }
                    dr.Close();
                    com = new MySqlCommand("delete from jiefachejihua where 计划发车时间='" + jhsj + "' and 车次号='" + cch + "'", conn);
                    com.ExecuteNonQuery();
                }
            }
        }
        private void connectionInniti()
        {
            while (true)//连接断开
            {
                try
                {
                    socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress serverip = IPAddress.Parse(ipaddress_test);
                    socketClient.Connect(serverip, port_test);


                }
                catch (Exception a)
                {
                    // a.ToString();
                }

                if (socketClient.Connected == true)//已连接
                {
                    //创建一个线程 用于监听服务端发来的消息
                    threadClient = new Thread(RecMsg);
                    //将窗体线程设置为与后台同步
                    threadClient.IsBackground = true;
                    //启动线程
                    threadClient.Start();

                    break;
                }
            }
            label_test.Text = "已连接";
            button1.Enabled = true;
        }

        private void Timer_Check()
        {
            Socket savesocket = socketClient;
            if (socketClient.Poll(10, SelectMode.SelectRead))//连接断开
            {
                //socketclient.Close();
                label_test.Text = "连接断开";
                // button1.Enabled = false;//禁用发送键
                try
                {
                    //socketclient.Close();
                    socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress serverip = IPAddress.Parse(ipaddress_test);
                    socketClient.Connect(serverip, port_test);
                    if (!socketClient.Poll(1000, SelectMode.SelectRead))
                    {
                        label_test.Text = "连接成功";
                        //textBox1.AppendText("重新连接：成功！\r\n");
                        //button1.Enabled = true;

                        //创建一个线程 用于监听服务端发来的消息
                        threadClient = new Thread(RecMsg);
                        //将窗体线程设置为与后台同步
                        threadClient.IsBackground = true;
                        //启动线程
                        threadClient.Start();
                    }
                }
                catch (Exception a)
                {
                    //textBox1.AppendText("重新连接:失败！\r\n");
                    socketClient = savesocket;
                }
            }
        }

        private void timer_connChecker_Tick(object sender, EventArgs e)
        {
            Thread th2 = new Thread(Timer_Check);
            th2.IsBackground = true;
            th2.Start();
        }
    }
}

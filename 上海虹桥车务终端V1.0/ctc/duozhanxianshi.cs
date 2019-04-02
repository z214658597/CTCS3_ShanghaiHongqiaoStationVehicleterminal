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
using ControlLib;

namespace ctc
{
    public partial class duozhanxianshi : Form
    {
        public static duozhanxianshi current = null;
        public duozhanxianshi()
        {
            InitializeComponent();
            current = this;
        }
        #region 链表——存储站场布置数据
        List<Xinhaoji2> list_XHJX = new List<Xinhaoji2>();    //链表信号机下行
        List<Xinhaoji2> list_XHJS = new List<Xinhaoji2>();    //链表信号机上行
        List<Dangui> list_SGD = new List<Dangui>();             //链表上行轨道
        List<Dangui> list_XGD = new List<Dangui>();             //链表下行轨道
        List<Dangui> list_qjxx = new List<Dangui>();            //链表区间占用信息
        Hashtable ht_zc = new Hashtable();      //哈希表站场
        Hashtable ht_DC_1 = new Hashtable();    //哈希表单动道岔
        Hashtable ht_DC_2 = new Hashtable();    //哈希表双动道岔
        #endregion
        #region 初始化函数
        private void duozhanxianshi_Load(object sender, EventArgs e)
        {
            ZHCHCSH();  //站场初始化
            QJCSH();    //区间初始化
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
            foreach (Control ct in this.Controls)
            {
                if (ct.Name.Contains("dangui") || ct.Name.Contains("G") || ct.Name.Contains("xinhaoji") || ct.Name.Contains("daocha"))
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
        
        foreach (Control ct in this.Controls)
            {
                //轨道初始化
                if (ct.Name.Contains("dangui")||ct.Name.Contains("G"))
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
                else if (ct.Name == "daocha_1_2" || ct.Name == "daocha_1_3" || ct.Name == "daocha_1_4" || ct.Name == "daocha_1_6" || ct.Name == "daocha_1_8" || ct.Name == "daocha_1_11" || ct.Name == "daocha_1_13" || ct.Name == "daocha_1_14" || ct.Name == "daocha_1_15" || ct.Name == "daocha_1_17" || ct.Name == "daocha_1_21" || ct.Name == "daocha_1_22")
                {
                    Daocha_1 dc1 = (Daocha_1)ct;
                    string[] str = dc1.Name.Split('_');
                    dc1.ID号 = str[2];
                    dc1.handle = this.Handle;
                    //list_DC1.Add(dc1);
                    ht_DC_1.Add(dc1.Name, dc1);
                }

                //1类道岔初始化
                else if (ct.Name == "daocha_1_5" || ct.Name == "daocha_1_7" || ct.Name == "daocha_1_9" || ct.Name == "daocha_1_10" || ct.Name == "daocha_1_12" || ct.Name == "daocha_1_16" || ct.Name == "daocha_1_18" || ct.Name == "daocha_1_31")
                {
                    Daocha_1_1 dc1 = (Daocha_1_1)ct;
                    string[] str = dc1.Name.Split('_');
                    dc1.ID号 = str[2];
                    dc1.handle = this.Handle;
                    //list_DC1.Add(dc1);
                    ht_DC_1.Add(dc1.Name, dc1);
                }
            }
            list_SGD.Sort(ComparebyNameS);
            list_XGD.Sort(ComparebyNameX);
            list_XHJS.Sort(ComparebyNameS);
            list_XHJX.Sort(ComparebyNameX);
            list_qjxx.Clear();
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
        #endregion
        #region 屏幕显示时间
        private void timer1_Tick(object sender, EventArgs e)
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
    }
}

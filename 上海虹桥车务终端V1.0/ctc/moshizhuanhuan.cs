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
using ControlLib;

namespace ctc
{
    public partial class moshizhuanhuan : Form
    {
        public moshizhuanhuan()
        {
            InitializeComponent();
        }
        private void moshizhuanhuan_Load(object sender, EventArgs e)
        {
            if (chewushanghai.moshi.Substring(10, 2) == "01")
            {
                if (chewushanghai.moshi.Substring(8, 2) == "01")
                    label1.Text = "请求转为中心控制模式";
            }
            else if (chewushanghai.moshi.Substring(10, 2) == "02")
            {
                if (chewushanghai.moshi.Substring(8, 2) == "01")
                    label1.Text = "请求转为车站控制模式";
            }
            else if (chewushanghai.moshi.Substring(10, 2) == "03")
            {
                if (chewushanghai.moshi.Substring(8, 2) == "01")
                    label1.Text = "请求转为分散自律模式";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "请求转为中心控制模式")
            {
                string senddata = "ABD809020201AC";
                ClientSendMsg(senddata);
                chewushanghai.current.danniu_left.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu_middle.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu0.显示状态 = Danniu.Xianshi.绿;
                chewushanghai.current.danniu1.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu2.显示状态 = Danniu.Xianshi.默认;
                this.Hide();
            }
            else if (label1.Text == "请求转为车站控制模式")
            {
                string senddata = "ABD809020202AC";
                ClientSendMsg(senddata);
                chewushanghai.current.danniu_left.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu_middle.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu0.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu1.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu2.显示状态 = Danniu.Xianshi.绿;
                this.Hide();
            }
            else if (label1.Text == "请求转为分散自律模式")
            {
                string senddata = "ABD809020203AC";
                ClientSendMsg(senddata);
                chewushanghai.current.danniu_left.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu_middle.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu0.显示状态 = Danniu.Xianshi.默认;
                chewushanghai.current.danniu1.显示状态 = Danniu.Xianshi.绿;
                chewushanghai.current.danniu2.显示状态 = Danniu.Xianshi.默认;
                this.Hide();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "请求转为中心控制模式")
            {
                string senddata = "ABD809020301AC";
                ClientSendMsg(senddata);
                this.Hide();
            }
            if (label1.Text == "请求转为车站控制模式")
            {
                string senddata = "ABD809020302AC";
                ClientSendMsg(senddata);
                this.Hide();
            }
            if (label1.Text == "请求转为分散自律模式")
            {
                string senddata = "ABD809020303AC";
                ClientSendMsg(senddata);
                this.Hide();
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

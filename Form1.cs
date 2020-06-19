using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using RestSharp;
using System.Text.RegularExpressions;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("Cookie.txt"))
            {

                try
                {
                    string all = System.IO.File.ReadAllText("Cookie.txt");
                    string[] bufs = all.Split('@');
                    textBox1.Text = bufs[0];
                    textBox2.Text = bufs[1];
                    textBox3.Text = bufs[2];
                    textBox4.Text = bufs[3];
                    label1.Text = "读取历史记录成功";
                }
                catch
                {
                    label1.Text = "无法读取历史记录";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(textBox1.Text==""|| textBox2.Text == ""||textBox3.Text == ""|| textBox4.Text == "")
            {
                label1.Text = "请输入正确的数据";
                return;
            }
            var client = new RestClient("https://www.mosoteach.cn/web/index.php?c=res&m=save_watch_to");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            string cookies = textBox1.Text;
            string buf = cookies + '@' + textBox2.Text + '@' + textBox3.Text + '@' + textBox4.Text;
            System.IO.File.WriteAllText("Cookie.txt", buf);

            string[] sArray = Regex.Split(cookies, "; ", RegexOptions.IgnoreCase);
            foreach (string i in sArray)
            {
                string[] cookie = i.Split('=');
                request.AddCookie(cookie[0].ToString(), cookie[1].ToString());
            }
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("clazz_course_id", textBox2.Text);
            request.AddParameter("res_id", textBox3.Text);
            request.AddParameter("watch_to", textBox4.Text);
            request.AddParameter("duration", textBox4.Text);
            request.AddParameter("current_watch_to", textBox4.Text);
            IRestResponse response = client.Execute(request);
            if(response.Content.Contains("[\"success\"]"))
            {
                label1.Text = "成功";
                
             }
            else
            {
                label1.Text = "失败";
            }
        }
    }
}

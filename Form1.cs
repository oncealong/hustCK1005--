using System;
//using System.Guid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;   //添加引用 System.Web.Extensions

namespace 百度API翻译
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //private Dictionary<string, string> NowLanguage = new Dictionary<string, string>();
        ////NowLanguage.Add
        //private Dictionary<string, string> TargetLanguage = new Dictionary<string, string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            var NowLanguage = new Dictionary<string, string>();
            NowLanguage.Add("auto", "自动翻译");
            NowLanguage.Add("zh", "简体中文");
            NowLanguage.Add("en", "英语");
            NowLanguage.Add("de", "德语");
            NowLanguage.Add("jp", "日语");
            NowLanguage.Add("spa", "西班牙语");
            NowLanguage.Add("th", "泰语");
            NowLanguage.Add("ru", "俄罗斯语");
            NowLanguage.Add("yue", "粤语");
            NowLanguage.Add("kor", "韩语");
            NowLanguage.Add("fra", "法语");
            NowLanguage.Add("it", "意大利语");
            NowLanguage.Add("ara", "阿拉伯语");
            NowLanguage.Add("pt", "葡萄牙语");
            NowLanguage.Add("wyw", "文言文");

            var TargetLanguage = new Dictionary<string, string>();
            TargetLanguage = NowLanguage;
            cboNowLanguage.DataSource = new BindingSource(NowLanguage, null);
            cboNowLanguage.DisplayMember = "value";
            cboNowLanguage.ValueMember = "key";
            //cboNowLanguage.Text = NowLanguage["auto"];
            cboNowLanguage.SelectedIndex = 0;


            cboTargetLanguage.DataSource = new BindingSource(TargetLanguage, null);
            cboTargetLanguage.DisplayMember = "value";
            cboTargetLanguage.ValueMember = "key";
            //cboTargetLanguage.Text = NowLanguage["auto"];
            cboTargetLanguage.SelectedIndex = 0;


            //texbSource.Focus();
            //cboNowLanguage.Focus(); //load的时候窗体未加载,Focus函数无效,可放在Actived或Shown函数中
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //object temp = cboNowLanguage.SelectedItem;
            //string key = ((KeyValuePair<string, string>)temp).Value;
            //texbTarget.Text = (key).ToString();
            string nowLanguage = cboNowLanguage.SelectedValue as string;
            string targetLanguage = cboTargetLanguage.SelectedValue as string;
            string source = texbSource.Text;
            if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source))
            {
                return;
            }
            string target = Translate(nowLanguage, targetLanguage, source);
            texbTarget.Text = target;
        }
        public string Translate(string nowLanguage, string targetLanguage, string content)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            string api =
                string.Format(
                    "http://openapi.baidu.com/public/2.0/bmt/translate?client_id=hte1ckEz4abPsAUGsvnGQyj0&q={0}&from={1}&to={2}",
                    content, nowLanguage, targetLanguage);

            string json = JsonDownload(api);
            //stopWatch.Stop();
            //TimeSpan tsJsonDownload = stopWatch.Elapsed;


            // stopWatch.Start();
            JsonTranslation jsonTranslation = JsonDeserialize(json);
            // stopWatch.Stop();
            // TimeSpan tsJsonDeserialize = stopWatch.Elapsed;

            // string elapsedTime = String.Format("\r\ntsJsonDownload:{0:00}:{1:00}\r\ntsJsonDeserialize:{2:00}.{3:00}",
            //     tsJsonDownload.Seconds, tsJsonDownload.Milliseconds / 10, tsJsonDeserialize.Seconds, tsJsonDeserialize.Milliseconds / 10);
            // Console.WriteLine("\r\nRunTime " + elapsedTime);
            string result = "";
            foreach (var item in jsonTranslation.trans_result)
            {
                result += item.dst + "\r\n";
            }
            return result;
        }
        public string JsonDownload(string api)
        {
            WebClient client = new WebClient();
            return client.DownloadString(api);
        }

        public JsonTranslation JsonDeserialize(string strJson)
        {
            texbTarget.Text = strJson;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<JsonTranslation>(strJson);

        }


        UInt32 hotkeyid = HotKey.GlobalAddAtom(System.Guid.NewGuid().ToString());
        private void Form1_Activated(object sender, EventArgs e)
        {
            //cboNowLanguage.Focus(); 
            texbSource.Focus();
            //注册热键Ctrl+H
            HotKey.RegisterHotKey(Handle, hotkeyid, HotKey.KeyModifiers.Ctrl, Keys.H);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //注销热键Ctrl+H
            HotKey.UnregisterHotKey(Handle, hotkeyid);
            HotKey.GlobalDeleteAtom(hotkeyid);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    //switch (m.WParam.ToInt32())
                    //{
                    //    case (hotkeyid):
                    //        this.Visible = !this.Visible;
                    //        break;
                    //}
                    if (m.WParam.ToInt32()==hotkeyid)
                    {
                        this.Visible = !this.Visible;
                        if (this.Visible==true)
                        {
                            this.Activate();
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ctrl+H\r\n  隐藏显示窗体\r\nCtrl+回车\r\n  输入多行");
        }
    }
}

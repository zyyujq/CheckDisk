using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;  //添加IO命名空间 
using System.Management;


namespace CheckUdisk
{
    public partial class Form2 : Form
    {

        //定义常量  
        public const int WM_DEVICECHANGE = 0x219;//
        public const int DBT_DEVNODES_CHANGED = 0x0007;//设备已添加到系统或从系统中删除
        public const int DBT_QUERYCHANGECONFIG = 0x0017;//请求权限以更改当前配置 (停靠或取消停靠) 
        public const int DBT_CONFIGCHANGED = 0x0018;//由于停靠或取消停靠，当前配置已更改
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;//已取消更改当前配置 (停靠或取消停靠) 的请求
        public const int DBT_DEVICEARRIVAL = 0x8000;//已插入设备或介质片段，现已可用
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;//请求权限以删除设备或介质。 任何应用程序都可以拒绝此请求并取消删除
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;//删除设备或介质的请求已取消
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;//即将删除设备或介质片段。 不可拒绝
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;//已删除设备或介质片段
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;//发生了特定于设备的事件
        public const int DBT_CUSTOMEVENT = 0x8006;//发生了自定义事件
        public const int DBT_USERDEFINED = 0xFFFF;//此消息的含义是用户定义的



        public Form2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override void WndProc(ref Message m)
        {

            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            richTextBox1.AppendText("WM_DEVICECHANGE=设备变化" + "\r\n");
                            break;
                        case DBT_DEVICEARRIVAL:
                            richTextBox1.AppendText("DBT_DEVICEARRIVAL=插入" + "\r\n");
                            DriveInfo[] drives = DriveInfo.GetDrives();
                            // foreach (DriveInfo drive in drives)
                            foreach (var drive in drives)
                            {
                                //if (drive.DriveType == DriveType.Removable)
                                //{
                                //    this.richTextBox1.AppendText("U盘已插入，盘符是" + drive.Name.ToString() + "\r\n");
                                //    // break;
                                //}
                                //else
                                //{

                                //}
                                switch (drive.DriveType)
                                {
                                    case DriveType.Removable:   //可以移动磁盘
                                        {
                                            richTextBox1.AppendText("U盘已插入，盘符是" + drive.Name.ToString() + "\r\n");
                                            break;
                                        }
                                    case DriveType.Fixed:   //本地磁盘     
                                        {
                                            richTextBox1.AppendText("本地固定磁盘" + drive.Name.ToString() + "\r\n");
                                            break;
                                        }
                                    case DriveType.CDRom:   //CD   rom   drives     
                                        {
                                            richTextBox1.AppendText("光驱" + drive.Name.ToString() + "\r\n");//MessageBox.Show("CD   rom   drives ");
                                            break;
                                        }
                                    case DriveType.Network:   //网络驱动   
                                        {
                                            richTextBox1.AppendText("网络驱动器" + drive.Name.ToString() + "\r\n");//MessageBox.Show(" ");
                                            break;
                                        }
                                    case DriveType.Ram:
                                        {
                                            richTextBox1.AppendText("驱动器是一个 RAM 磁盘" + drive.Name.ToString() + "\r\n");//MessageBox.Show(" ");
                                            break;
                                        }
                                    case DriveType.NoRootDirectory:
                                        {
                                            richTextBox1.AppendText("驱动器没有根目录" + drive.Name.ToString() + "\r\n");//MessageBox.Show(" ");
                                            break;
                                        }
                                    default:
                                        {
                                            richTextBox1.AppendText("驱动器类型未知" + drive.Name.ToString() + "\r\n");
                                            break;
                                        }
                                }
                            }
                            break;
                        case DBT_CONFIGCHANGECANCELED:
                            richTextBox1.AppendText("DBT_CONFIGCHANGECANCELED" + "\r\n");
                            // MessageBox.Show("2");
                            break;
                        case DBT_CONFIGCHANGED:
                            richTextBox1.AppendText("DBT_CONFIGCHANGED" + "\r\n");
                            //  MessageBox.Show("3");
                            break;
                        case DBT_CUSTOMEVENT:
                            richTextBox1.AppendText("DBT_CUSTOMEVENT" + "\r\n");
                            // MessageBox.Show("4");
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            richTextBox1.AppendText("DBT_DEVICEQUERYREMOVE" + "\r\n");
                            // MessageBox.Show("5");
                            break;
                        case DBT_DEVICEQUERYREMOVEFAILED:
                            richTextBox1.AppendText("DBT_DEVICEQUERYREMOVEFAILED" + "\r\n");
                            // MessageBox.Show("6");
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:
                            this.richTextBox1.AppendText("U盘已卸载=DBT_DEVICEREMOVECOMPLETE" + "\r\n");
                            break;
                        case DBT_DEVICEREMOVEPENDING:
                            richTextBox1.AppendText("DBT_DEVICEREMOVEPENDING" + "\r\n");
                            // MessageBox.Show("7");
                            break;
                        case DBT_DEVICETYPESPECIFIC:
                            richTextBox1.AppendText("DBT_DEVICETYPESPECIFIC" + "\r\n");
                            // MessageBox.Show("8");
                            break;
                        case DBT_DEVNODES_CHANGED:
                            richTextBox1.AppendText("DBT_DEVNODES_CHANGED" + "\r\n");
                            // MessageBox.Show("9");
                            break;
                        case DBT_QUERYCHANGECONFIG:
                            richTextBox1.AppendText("DBT_QUERYCHANGECONFIG" + "\r\n");
                            // MessageBox.Show("10");
                            break;
                        case DBT_USERDEFINED:
                            richTextBox1.AppendText("DBT_USERDEFINED" + "\r\n");
                            // MessageBox.Show("11");
                            break;
                        default:
                            richTextBox1.AppendText("其它操作" + "\r\n");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            base.WndProc(ref m);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {//关闭中
            this.Dispose();
            //Application.Exit();//退出整个应用程序
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {//关闭后
            this.Close();
         //   Application.Exit();
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
            //Application.Exit();//退出整个应用程序
        }

        private void Form1button_Click(object sender, EventArgs e)
        {
            this.Visible = false;//使本窗体不可见
            Form1 form1 = new Form1();//打开窗体2
            form1.ShowDialog(this);// 以模态方式打开
        }
    }
}
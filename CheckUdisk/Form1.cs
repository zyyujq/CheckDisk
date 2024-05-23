using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace CheckUdisk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {//关闭中
            // 在这里执行你的清理工作
            try
            {
                this.Dispose();
            }
            catch
            {

            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭后
            this.Close();
            // Application.Exit();
        }

        private void Form2button_Click(object sender, EventArgs e)
        {
            this.Visible = false;//使本窗体不可见
            Form2 form2 = new Form2();//打开窗体2
            form2.ShowDialog(this);// 以模态方式打开
                                   // form2.Show();// 以非模态方式打开
                                   //创建新窗体的实例
                                   //Form newForm = new Form();
                                   //设置新窗体的一些属性，如大小和位置
                                   //newForm.Size = new System.Drawing.Size(300, 200);
                                   //newForm.StartPosition = FormStartPosition.CenterScreen;
                                   //显示新窗体
                                   //newForm.ShowDialog(); 
                                   //newForm.Show(); // 以非模态方式打开
                                   // this.Close(); // 卸载当前窗体
        }

        //取消注释:Ctrl+k+u 注释:Ctrl+k+c 缩进Ctrl+k+d
        private void button1_Click(object sender, EventArgs e)
        {
            ScanDisk();
        }

        private void ScanDisk()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                // 可移动存储设备，且不是A盘
                if ((drive.DriveType == DriveType.Removable) && false == drive.Name.Substring(0, 1).Equals("A"))
                {
                    textBox1.AppendText("找到一个U盘：" + drive.Name + "\r\n");
                    textBox1.AppendText("\r\n");
                    // Console.WriteLine("找到一个U盘：" + drive.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetDiskID();
        }
        string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                    textBox1.AppendText("搜索所有磁盘：\r\n");
                    textBox1.AppendText("型号：" + HDid.ToString() + "\r\n");
                    textBox1.AppendText("类型：" + mo.Properties["MediaType"].Value + "\r\n");
                    textBox1.AppendText("硬盘SN：" + mo.Properties["SerialNumber"].Value + "\r\n");
                    textBox1.AppendText("\r\n");
                    // Console.WriteLine("型号：" + HDid.ToString());
                    // Console.WriteLine("类型：" + mo.Properties["MediaType"].Value);
                    // Console.WriteLine("硬盘SN：" + mo.Properties["SerialNumber"].Value);
                    // Console.WriteLine();
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        //型号：SSK SCSI Disk Device
        //类型：External hard disk media
        //硬盘SN：DD56419883935

        //型号：SSK USB3.2 USB Device
        //类型：Removable Media
        //硬盘SN：0000000005

        //型号：Micron MTFDKBA512TFH
        //类型：Fixed hard disk media
        //硬盘SN：00A0_7501_352A_99A5.

        //型号：WD_BLACK SN850X 1000GB
        //类型：Fixed hard disk media
        //硬盘SN：E823_8FA6_BF53_0001_001B_444A_48AE_E4DF.

        private void button3_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(Math.Log(1792,2));
            getVol();
        }
        public void getVol()
        {
            if (getlistdisk().Count > 0)
            {
                foreach (var d in getlistdisk())

                {
                    textBox1.AppendText("移动硬盘或U盘：" + d + "\r\n");
                    textBox1.AppendText("\r\n");
                    //Console.WriteLine(d);

                }
            }
        }
        public static List<string> getlistdisk()
        {
            List<string> lstdisk = new List<string>();
            ManagementClass mgtcls = new ManagementClass("Win32_DiskDrive");
            var disks = mgtcls.GetInstances();
            foreach (ManagementObject mo in disks)
            {
                //if (mo.properties["interfacetype"].value.tostring() != "scsi" 
                //  && mo.properties["interfacetype"].value.tostring() != "usb"
                //  )
                //  continue;

                if (mo.Properties["mediatype"].Value == null || mo.Properties["mediatype"].Value.ToString() == "External hard disk media" || mo.Properties["mediatype"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject diskpartition in mo.GetRelated("win32_diskpartition"))
                    {
                        foreach (ManagementObject disk in diskpartition.GetRelated("win32_logicaldisk"))
                        {
                            lstdisk.Add(disk.Properties["name"].Value.ToString());
                        }
                    }
                    // continue;
                }

                //foreach (var prop in mo.properties)
                //{
                //  console.writeline(prop.name + "\t" + prop.value);
                //}
            }
            return lstdisk;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SDD();
        }
        //CDRom=5	驱动器是一个光盘设备，如 CD 或 DVD-ROM。
        //Fixed=3	驱动器是一个固定磁盘。
        //Network=4	驱动器是一个网络驱动器。
        //NoRootDirectory=1	驱动器没有根目录。
        //Ram=6	驱动器是一个 RAM 磁盘。
        //Removable=2	驱动器是一个可移动存储设备，如 U 盘。
        //Unknown=0	驱动器类型未知。
        private void SDD()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            textBox1.AppendText("搜索所有磁盘：\r\n");
            textBox1.AppendText("\r\n");
            foreach (DriveInfo d in allDrives)
            {
                textBox1.AppendText("驱动名称：" + d.Name + "\r\n");
                textBox1.AppendText("驱动类型：" + d.DriveType + "\r\n");
                textBox1.AppendText("---------------------------------------------\r\n");
                //   Console.WriteLine("Drive {0}", d.Name);
                //  Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    textBox1.AppendText("卷标:          " + d.VolumeLabel + "\r\n");
                    textBox1.AppendText("文件系统:      " + d.DriveFormat + "\r\n");
                    textBox1.AppendText("该用户可用空间:" + d.AvailableFreeSpace + " bytes\r\n");
                    textBox1.AppendText("总可用空间:    " + d.TotalFreeSpace + " bytes\r\n");
                    textBox1.AppendText("驱动器的总大小:" + d.TotalSize + " bytes\r\n");
                    textBox1.AppendText("\r\n");
                    //  Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    //  Console.WriteLine("  File system: {0}", d.DriveFormat);
                    //  Console.WriteLine("  Available space to current user:{0, 15} bytes", d.AvailableFreeSpace);
                    //  Console.WriteLine("  Total available space:          {0, 15} bytes",d.TotalFreeSpace);
                    //  Console.WriteLine("  Total size of drive:            {0, 15} bytes ",d.TotalSize);
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var d in Get_UsbDisk_List())

            {
                textBox1.AppendText("移动U盘： "+d + "\r\n");
                textBox1.AppendText("\r\n");

                //Console.WriteLine(d);

            }
        }
        /// <summary> 获取U盘或移动硬盘等设备 </summary> 
        public static List<string> Get_UsbDisk_List()
        {
            DriveInfo[] di = DriveInfo.GetDrives();//检索计算机上的所有逻辑驱动器的驱动器名称
            List<string> Div = new List<string>();
            foreach (DriveInfo d in di)
            {
                string DriveName = d.Name.ToUpper();
                if (d.DriveType == DriveType.Removable && !DriveName.Contains("A") && !DriveName.Contains("B"))
                {//获取U盘或移动硬盘等设备
                    Div.Add(d.Name);
                }
            }
            return Div;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            foreach (var d in GetDVN())
            {
                textBox1.AppendText("外部硬盘或移动U盘： " + d + "\r\n");
                textBox1.AppendText("\r\n");
                //  Console.WriteLine(d);

            }
        }

        /// <summary>获取U盘和可移动硬盘盘符名称</summary>
        /// <returns></returns>
        public static List<string> GetDVN()
        {
            List<string> lstdisk = new List<string>();
            ManagementClass mgtcls = new ManagementClass("Win32_DiskDrive");
            var disks = mgtcls.GetInstances();
            foreach (ManagementObject mo in disks)
            {
                if (mo.Properties["mediatype"].Value == null || mo.Properties["mediatype"].Value.ToString() == "External hard disk media" || mo.Properties["mediatype"].Value.ToString() == "Removable Media")
                {
                    foreach (ManagementObject diskpartition in mo.GetRelated("win32_diskpartition"))
                    {
                        foreach (ManagementObject disk in diskpartition.GetRelated("win32_logicaldisk"))
                        {
                            lstdisk.Add(disk.Properties["name"].Value.ToString());
                        }
                    }
                    // continue;
                }

            }
            return lstdisk;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (var d in GetRemovableDeviceID())
            {
                textBox1.AppendText("本地磁盘或移动U盘: " + d + "\r\n");
                textBox1.AppendText("\r\n");
                //   Console.WriteLine(d);

            }
        }

        //C# 获取本地电脑所有的盘符

        public List<string> GetRemovableDeviceID()
        {
            List<string> deviceIDs = new List<string>();
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT  *  From  Win32_LogicalDisk ");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {

                switch (int.Parse(mo["DriveType"].ToString()))
                {
                    case (int)DriveType.Removable:   //可以移动磁盘     
                        {
                            //MessageBox.Show("可以移动磁盘");
                            deviceIDs.Add(mo["DeviceID"].ToString());
                            break;
                        }
                    case (int)DriveType.Fixed:   //本地磁盘     
                        {
                            //MessageBox.Show("本地磁盘");
                            deviceIDs.Add(mo["DeviceID"].ToString());
                            break;
                        }
                    case (int)DriveType.CDRom:   //CD   rom   drives     
                        {
                            //MessageBox.Show("CD   rom   drives ");
                            break;
                        }
                    case (int)DriveType.Network:   //网络驱动   
                        {
                            //MessageBox.Show("网络驱动器 ");
                            break;
                        }
                    case (int)DriveType.Ram:
                        {
                            //MessageBox.Show("驱动器是一个 RAM 磁盘 ");
                            break;
                        }
                    case (int)DriveType.NoRootDirectory:
                        {
                            //MessageBox.Show("驱动器没有根目录 ");
                            break;
                        }
                    default:   //defalut   to   folder     
                        {
                            //MessageBox.Show("驱动器类型未知 ");
                            break;
                        }
                }

            }
            return deviceIDs;
        }




        [StructLayout(LayoutKind.Sequential)]

        public struct DEV_BROADCAST_VOLUME

        {

            public int dbcv_size;

            public int dbcv_devicetype;

            public int dbcv_reserved;

            public int dbcv_unitmask;

        }


        protected override void WndProc(ref Message m)
        {
            // 发生设备变动
            const int WM_DEVICECHANGE = 0x0219;
            // 系统检测到一个新设备
            const int DBT_DEVICEARRIVAL = 0x8000;
            // 系统完成移除一个设备
            const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
            // 逻辑卷标
            const int DBT_DEVTYP_VOLUME = 0x00000002;

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch (m.WParam.ToInt32())
                    {
                        case DBT_DEVICEARRIVAL:
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DEV_BROADCAST_VOLUME vol;
                                vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                //MessageBox.Show(vol.dbcv_unitmask.ToString("x"));

                                textBox1.AppendText("插入移动磁盘盘符：" + "\r\n");
                                textBox1.AppendText("\r\n");
                                textBox1.AppendText("盘符：" + ((char)('A' + (uint)Math.Floor(Math.Log(vol.dbcv_unitmask, 2)))).ToString() + "，16进制：" + vol.dbcv_unitmask.ToString("x") + "，10进制：" + vol.dbcv_unitmask.ToString()+ "\r\n");
                                textBox1.AppendText("\r\n");
                                //Console.WriteLine("盘符：" + ((char)('A' + (uint)Math.Floor(Math.Log(vol.dbcv_unitmask, 2)))).ToString() + " 16进制 " + vol.dbcv_unitmask.ToString("x") + " 10进制 " + vol.dbcv_unitmask.ToString());
                                //Console.WriteLine();
                            }
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:
                            textBox1.AppendText("拔出移动磁盘！"+ "\r\n");
                            textBox1.AppendText("\r\n");
                            //Console.WriteLine("拔出移动磁盘！");
                            //Console.WriteLine();
                            //   MessageBox.Show("Removal");
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            //  Application.Exit();//退出整个应用程序

            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}


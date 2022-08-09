using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace R2000Demo
{
   
    public partial class FormNET : Form
    {

        public FormNET()
        {
            InitializeComponent();

        }
        UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 1500);
        public delegate void TagInvoke(string str1, string str2 , string str3);
        public delegate void messasgeinvoke(string str);
        public delegate void UpdateInoke(string IP, string ID, string NAME,
                                    string PASSWD, string TAGIP, string IPTYPE,
                                    string str7, string str8, string str9);
        public delegate void TimeInvoke(object sender, EventArgs e);
        char[] oldname;
        char[] oldpasswd;
        Thread thread;//用线程接收，避免UI卡住
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Quest")
            {
                button1.Text = "Stop";
                byte[] ipByte = { 0xff, 0x01, 0x01, 0x02 };
                client.Send(ipByte, ipByte.Length, endpoint);
                thread = new Thread(receiveUdpMsg);
                thread.IsBackground = true;
                thread.Start(client);
                //thread = new Thread(receiveUdp);
                //thread.Start();
            }
            else
            {
                button1.Text = "Quest";
                if (thread != null)
                {
                    thread.Abort();
                }
            }
            
        }
        void receiveUdp() 
        {
            var clientudp = new UdpClient(1500);
                var multicast = new IPEndPoint (IPAddress .Parse ("255.255.255.255") ,0);
                while (true)
                {
                    byte[] buf = clientudp.Receive(ref multicast);
                    string message = Encoding.UTF8.GetString(buf);
                    Console.WriteLine(buf);
                }
        }
        void receiveUdpMsg(object obj)
        {
             TimeInvoke b = new TimeInvoke(button1_Click);
             UdpClient client = obj as UdpClient;
             object sender = new object();
             EventArgs e = new EventArgs();
             //TagInvoke a = new TagInvoke(update);
             IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
             //IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("224.0.0.1"), 0);
             UdpState s = new UdpState();
             s.e = endpoint;
             s.u = client;
             messageReceived = false;

            client.BeginReceive (new AsyncCallback (queryReceiveCallback ),s); 
            while(!messageReceived )
            {
                Thread.Sleep(100);
             }
            //client.Close();
            this.BeginInvoke(b, new Object[] { sender, e });
            if ((thread != null) && (thread.IsAlive == true))
            {
                thread.Abort();
            }
        }

        public void update(string str1, string str2,string str3)
        {
           
            if(str1=="BACK")
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    if (str2 == "FF01064B")
                    {
                        label2.Text += "基础设置成功 ";
                    }
                    else if (str2 == "FF01054B")
                    {
                        label2.Text += "串口设置成功 ";
                    }
                    else if (str2 == "FF01024B")
                    {
                        label2.Text += "重启成功 ";
                    }
                }
                else 
                {
                    if (str2 == "FF01064B")
                    {
                        label2.Text += "Base Param Set Success ";
                    }
                    if (str2 == "FF01054B")
                    {
                        label2.Text += "Port Param Set Success ";
                    }
                    if (str2 == "FF01024B")
                    {
                        label2.Text += "Rest Success ";
                    }
                }
            }
            else
            {
                    ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());

                    item.SubItems.Add(str1);
                    item.SubItems.Add(str2);
                    item.SubItems.Add(str3);
                    listView1.Items.Add(item);
                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
            }
        }
        public void meassageshow(string str) 
        {
            MessageBox.Show(str);
        }
        public void parameterupdate(string IP,string ID,string NAME,
                                    string PASSWD,string TAGIP,string IPTYPE,
                                    string worktype,string gateway,string str9) 
        {
            //string ip = "";
            //string id = "";
            //string name = "";
            //string username="";
            //string passwd = "";
            //string worktype = "";
            //string gateway = "";
            //string tageip = "";
            tB_DestIP.Text =IP ;
            tB_ID.Text =int.Parse (ID).ToString () ;
            tB_NAME .Text =NAME;
            tB_PASSWD .Text =PASSWD ;
            tB_tagetip .Text =TAGIP ;
            cB_iptype.SelectedIndex = int.Parse(IPTYPE);
            cB_NetWork.SelectedIndex = int.Parse(worktype);
            tB_Gateway.Text = gateway;
            bt_SetNetPara.Enabled = true;
            if (0 == ReaderParams.LanguageFlag)
            {
                if (label2.Text != "获取参数成功")
                {
                    label2.Text = "获取参数成功";
                }
            }
            else
            {
                if (label2.Text != "Success")
                {
                    label2.Text = "Success";
                }
            }
            
        }
        private void FormNET_Load(object sender, EventArgs e)
        {
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            oldname = new char[6]; 
            oldpasswd=new char[6];
            label2.Text = "";
            if (0 == ReaderParams.LanguageFlag)
            {
                groupBox5.Text = "网口参数设置";
                label11.Text ="注意: 网口参数设置只针对带有网口通信的阅读器";
                string[] array = { "静态IP",
                                   "动态IP" };
                cB_iptype.DataSource = array;
            }
            else
            {
                groupBox5 .Text ="Ethernet module Setting";
                label11.Text = "Note: The Net Para Setting only for the reader with the network";
                string[] array = { "Static IP",
                                   "Auto IP" };
                cB_iptype.DataSource = array;
            }
        }
        
        public static bool messageReceived = false;
        public string str = "";
        public  void queryReceiveCallback(IAsyncResult ar)
       {
        
        UdpClient u = ((UdpState)(ar.AsyncState)).u;
        IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
        //string receiveString = "";
        string ip = "";
        string mac = "";
        string name = "";
        byte[] receiveBytes = u.EndReceive(ar, ref e);
        if (receiveBytes[2]==0x01)
        {
        for (int i = 0; i < (receiveBytes .Length); i++)
        {
            //receiveString += receiveBytes[i].ToString("X2");
            if((i>=5)&&(i<=8))
            {
                ip += receiveBytes[i].ToString("D2");
                if (i!=8)
                ip += ".";
               
            }
            else if ((i >= 9) && (i <= 14))
            {
                mac += receiveBytes[i].ToString("X2");
                if (i != 14)
                    mac += " ";
            }
            //else if ((i >= 19) && (i <= 34))
            //{
            //    name += receiveBytes[i].ToString("X2");
            //}

        }
        //string receiveString = Encoding.ASCII.GetString(receiveBytes);
        name = Encoding.ASCII.GetString(receiveBytes, 19, 15);
        TagInvoke a = new TagInvoke(update);
        if (receiveBytes[0]==0xff)
        {
        this.BeginInvoke(a, new Object[] { ip, mac, name });
        }
        }
        //update(receiveString, "", "");
        //ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());
        //Console.WriteLine($"Received: {receiveString}");
        messageReceived = true;
       }

        private void bt_SetNetPara_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            byte[] ipByte = new byte[200];
            byte[] comByte = new byte[200];
            byte[] reset = new byte[100];
            int cnt = 0;
            int cnt1 = 0;
            int cnt2 = 0;
            int i = 0;
            string[] strArray = tB_MAC.Text.Split(' ');
            string[] ip = tB_DestIP.Text.Split('.');
            string[] gateway = tB_Gateway.Text.Split('.');
            char[] name = new char[6];
            char[] passwd = new char[6];
            name = tB_NAME.Text.ToCharArray();
            passwd = tB_PASSWD.Text.ToCharArray();
            //client.Send(ipByte, ipByte.Length, endpoint);
            ipByte[cnt++] = 0xff;
            ipByte[cnt++] = 0x56;
            ipByte[cnt++] = 0x05;
            ipByte[cnt++] = Convert.ToByte(strArray[0], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[1], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[2], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[3], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[4], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[5], 16);
            for (i = 0; i < 6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)oldname[i];
                }
                else
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            for (i = 0; i < 6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)oldpasswd[i];
                }
                else
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            if (datatosave.Length == 0) 
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("长度异常，操作失败");
                }
                else 
                {
                    MessageBox.Show("Fail");
                }
                return;

            }
            for (i = 0; i < 67; i++) 
            {
                ipByte[cnt++] = datatosave[i];
            }

            if (cB_iptype.SelectedIndex == 0)//iptype
            {
                ipByte[24] = 0x80;
            }
            else
            {
                ipByte[24] = 0x00;
            }
            //ipadress
            ipByte[30] = (byte)int.Parse (ip[3]);
            ipByte[31] = (byte)int.Parse (ip[2]);
            ipByte[32] = (byte)int.Parse (ip[1]);
            ipByte[33] = (byte)int.Parse (ip[0]);
            //gateway
            ipByte[34] = (byte)int.Parse(gateway[3]);
            ipByte[35] = (byte)int.Parse(gateway[2]);
            ipByte[36] = (byte)int.Parse(gateway[1]);
            ipByte[37] = (byte)int.Parse(gateway[0]);
            //name
            for (i = 0; i <6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[58 + i] = (byte)name[i];
                }
                else
                {
                    ipByte[58 + i] = 0x00;
                }
            }
            
            //passwd
            for (i = 0; i < 6; i++)
            {
                if (i <passwd.Length)
                {
                    ipByte[64+i] = (byte)passwd[i];
                }
                else
                {
                    ipByte[64+i] = 0x00;
                }
            }
            //id
            ipByte[72] = (byte)(int.Parse(tB_ID.Text) >>   8);
            ipByte[71] = (byte)(int.Parse(tB_ID.Text) & 0xff);
            int num = 0;
            for (i = 1; i < (cnt + 1); i++)
            {
                num += ipByte[i];
            }
            ipByte[88] = (byte)(num & 0xff);




            //comset
            comByte[cnt1++]=0xff;
            comByte[cnt1++] = 0x52;
            comByte[cnt1++] = 0x06;
            comByte[cnt1++] = Convert.ToByte(strArray[0], 16);
            comByte[cnt1++] = Convert.ToByte(strArray[1], 16);
            comByte[cnt1++] = Convert.ToByte(strArray[2], 16);
            comByte[cnt1++] = Convert.ToByte(strArray[3], 16);
            comByte[cnt1++] = Convert.ToByte(strArray[4], 16);
            comByte[cnt1++] = Convert.ToByte(strArray[5], 16);
            for (i = 0; i < 6; i++)
            {
                if (i < oldname.Length)
                {
                    comByte[cnt1++] = (byte)oldname[i];
                }
                else
                {
                    comByte[cnt1++] = 0x00;
                }
            }
            for (i = 0; i < 6; i++)
            {
                if (i < oldpasswd.Length)
                {
                    comByte[cnt1++] = (byte)oldpasswd[i];
                }
                else
                {
                    comByte[cnt1++] = 0x00;
                }
            }
            for (i = 0; i < 62; i++)
            {
                comByte[cnt1++] = datatosave[67 + i];
            }
            //tagip
            char[]tagip = tB_tagetip .Text .ToCharArray ();
            for (i = 0; i < 30;i++ )
            {
                if(i<tagip .Length )
                {
                    comByte[37 + i] = (byte)tagip[i];
                }
                else
                {
                    comByte[37 + i] = 0x00;
                }

            }
            //worktpye
            comByte[72] = (byte)cB_NetWork.SelectedIndex;
            num = 0;
            for (i = 1; i < 84; i++)
            {
                num += comByte[i];
            }
            comByte [84]=(byte)(num&0xff);

            //reset
            reset[cnt2++] = 0xff;
            reset[cnt2++] = 0x13;
            reset[cnt2++] = 0x02;
            reset[cnt2++] = Convert.ToByte(strArray[0], 16);
            reset[cnt2++] = Convert.ToByte(strArray[1], 16);
            reset[cnt2++] = Convert.ToByte(strArray[2], 16);
            reset[cnt2++] = Convert.ToByte(strArray[3], 16);
            reset[cnt2++] = Convert.ToByte(strArray[4], 16);
            reset[cnt2++] = Convert.ToByte(strArray[5], 16);
            for (i = 0; i < 6; i++)
            {
                if (i < oldname.Length)
                {
                    reset[cnt2++] = (byte)oldname[i];
                }
                else
                {
                    reset[cnt2++] = 0x00;
                }
            }
            for (i = 0; i < 6; i++)
            {
                if (i < oldpasswd.Length)
                {
                    reset[cnt2++] = (byte)oldpasswd[i];
                }
                else
                {
                    reset[cnt2++] = 0x00;
                }
            }
            num = 0;
            for (i = 1; i < 20; i++)
            {
                num += reset[i];
            }
            reset[cnt2++] = (byte)(num & 0xff);



            client.Send(ipByte, 89, endpoint);
            client.Send(comByte, 85, endpoint);
            client.Send(reset,22,endpoint);
            thread = new Thread(receiveUdpMsg2);
            thread.IsBackground = true;
            thread.Start(client);   
        }
        public struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
        }
        
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (0 == ReaderParams.LanguageFlag)
            {
                label2.Text = "获取参数失败";
            }
            else
            {
                label2.Text = "Fail to get";
            }

            
            int si=listView1.SelectedItems[0].Index;
            tB_DestIP.Text = this.listView1.Items[si].SubItems[1].Text;
            tB_MAC.Text    = this.listView1.Items[si].SubItems[2].Text;
            byte[] ipByte = new byte[22];
            int cnt=0;
            int i = 0;
            string[] strArray = tB_MAC.Text.Split(' ');
            char[] name   = new char[6];
            char[] passwd = new char[6]; 
             name   = tB_NAME.Text.ToCharArray();
             passwd = tB_PASSWD.Text.ToCharArray();
             oldname = name;
             oldpasswd = passwd;
             //for (i = name.Length; i < 6; i++) 
             //{
             //    name[i] = '0';
             //}
             //for (i = passwd.Length; i < 6; i++)
             //{
             //    passwd[i] = '0';
             //}
             client.Send(ipByte, ipByte.Length, endpoint);
            ipByte[cnt++] = 0xff;
            ipByte[cnt++] = 0x13;
            ipByte[cnt++] = 0x03;
            ipByte[cnt++] = Convert.ToByte(strArray[0], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[1], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[2], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[3], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[4], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[5], 16);
            for (i = 0; i < 6; i++) 
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)name[i];
                }
                else 
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            for (i = 0; i < 6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)passwd[i];
                }
                else
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            int num = 0;
            for (i = 1; i < (cnt+1); i++)
            {
                num += ipByte[i];
            }
            ipByte [cnt++]=(byte)(num&0xff);
            client.Send(ipByte, ipByte.Length, endpoint);
            thread = new Thread(receiveUdpMsg1);
            thread.IsBackground = true;
            thread.Start(client);
        }
        void receiveUdpMsg1(object obj)
        {
            UdpClient client = obj as UdpClient;
            object sender = new object();
            EventArgs e = new EventArgs();
            //TagInvoke a = new TagInvoke(update);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            UdpState s = new UdpState();
            s.e = endpoint;
            s.u = client;
            for (int i = 0; i < 3; i++)
            {
                messageReceived = false;
                
                    client.BeginReceive(new AsyncCallback(setReceiveCallback), s);
                
                while (!messageReceived)
                {
                    Thread.Sleep(100);
                }
            }
            //client.Close();
            if ((thread != null) && (thread.IsAlive == true))
            {
                thread.Abort();
            }
        }
        void receiveUdpMsg2(object obj)
        {
            UdpClient client = obj as UdpClient;
            object sender = new object();
            EventArgs e = new EventArgs();
            //TagInvoke a = new TagInvoke(update);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            UdpState s = new UdpState();
            s.e = endpoint;
            s.u = client;
            for (int i = 0; i < 3; i++)
            {
                messageReceived = false; 
                    client.BeginReceive(new AsyncCallback(nullReceiveCallback2), s);
                while (!messageReceived)
                {
                    Thread.Sleep(100);
                }
            }
            //client.Close();
            if ((thread != null) && (thread.IsAlive == true))
            {
                thread.Abort();
            }
            //this.Controls.Clear();
            //InitializeComponent();
        }
        void receiveUdpMsg3(object obj)
        {
            UdpClient client = obj as UdpClient;
            object sender = new object();
            EventArgs e = new EventArgs();
            TagInvoke a = new TagInvoke(update);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            UdpState s = new UdpState();
            s.e = endpoint;
            s.u = client;
            messageReceived = false;
            client.BeginReceive(new AsyncCallback(nullReceiveCallback3), s);
            while (!messageReceived)
                {
                    this.BeginInvoke(a, new Object[] { "1BACK", "waiting", "" });
                    Thread.Sleep(100);
                }
            //client.Close();
            if ((thread != null) && (thread.IsAlive == true))
            {
                thread.Abort();
            }
            //this.Controls.Clear();
            //InitializeComponent();
        }
        public void nullReceiveCallback(IAsyncResult ar) 
        {
            UdpClient u  = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
            //string receiveString = "";
            byte[] receiveBytes = u.EndReceive(ar, ref e);
            //if(receiveBytes[0]==0xff)
            //for (int i = 0; i < (receiveBytes .Length); i++)
            //{
            // receiveString += receiveBytes[i].ToString("X2");
            //}
            //TagInvoke a = new TagInvoke(update);
            //if (receiveBytes[0] == 0xff)
            //{
            //    this.BeginInvoke(a, new Object[] { "BACK", receiveString, "" });
            //}
            messageReceived = true;
        }
        public void nullReceiveCallback1(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
            TagInvoke a = new TagInvoke(update);
            string receiveString = "";
            byte[] receiveBytes = u.EndReceive(ar, ref e);
            for (int i = 0; i < (receiveBytes.Length); i++)
            {
                receiveString += receiveBytes[i].ToString("X2");
            }
            if (receiveBytes[0] == 0xff)
            {
                //this.BeginInvoke(a, new Object[] { "BACK", receiveString, "" });
            }
            messageReceived = true;
        }
        public void nullReceiveCallback2(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
            TagInvoke a = new TagInvoke(update);
            string receiveString = "";
            byte[] receiveBytes = u.EndReceive(ar, ref e);
            for (int i = 0; i < (receiveBytes.Length); i++)
            {
                receiveString += receiveBytes[i].ToString("X2");
            }
            if ((receiveBytes[0] == 0xff)&&(receiveBytes[2] != 0x01))
            {
                this.BeginInvoke(a, new Object[] { "BACK", receiveString, "" });
            }
            messageReceived = true;
        }
        public void nullReceiveCallback3(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
            TagInvoke a = new TagInvoke(update);
            string receiveString = "";
            byte[] receiveBytes = u.EndReceive(ar, ref e);
            for (int i = 0; i < (receiveBytes.Length); i++)
            {
                receiveString += receiveBytes[i].ToString("X2");
            } 
                this.BeginInvoke(a, new Object[] { "BACK", receiveString, "" });
            messageReceived = true;
        }
        public void setReceiveCallback(IAsyncResult ar)
        {

            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;
            string ip = "";
            string id = "";
            string name = "";
            string username="";
            string passwd = "";
            string worktype = "";
            string gateway = "";
            string tageip = "";
            string iptype = "0";
            byte[] receiveBytes = u.EndReceive(ar, ref e);
            if(receiveBytes .Length !=130)
            {
               // messasgeinvoke m = new messasgeinvoke(meassageshow);
               // this.BeginInvoke(m, new Object[] {"获取长度异常"});

                return;
            }
            
            if ((receiveBytes[0] != 0xff) && (receiveBytes[0] != 0x07))
            {

                datatosave = null;
                datatosave = receiveBytes;
            }
            if(receiveBytes .Length ==130)
            {
                if (receiveBytes[3] == 0x80)
                {
                    iptype = "0";
                }
                else 
                {
                    iptype = "1";
                }
            username = Encoding.ASCII.GetString(receiveBytes, 37, 6);
            passwd = Encoding.ASCII.GetString(receiveBytes, 43, 6); 
            //string receiveString = Encoding.ASCII.GetString(receiveBytes)
            ip = receiveBytes[12].ToString("D2")+"."+receiveBytes[11].ToString("D2")+"."+receiveBytes[10].ToString("D2")+"."+receiveBytes[9].ToString("D2");
            gateway = receiveBytes[16].ToString("D2") + "." + receiveBytes[15].ToString("D2") + "." + receiveBytes[14].ToString("D2") + "." + receiveBytes[13].ToString("D2");
            id = receiveBytes[51].ToString("X2") + receiveBytes[50].ToString("X2");
            name = Encoding.ASCII.GetString(receiveBytes, 21, 15);
            tageip = Encoding.ASCII.GetString(receiveBytes, 83, 30);
            worktype = receiveBytes[118].ToString("X2");
            UpdateInoke B = new UpdateInoke(parameterupdate);
            this.BeginInvoke(B, new Object[] { ip, id, username, passwd, tageip, iptype, worktype, gateway,  "" });
            }
            //update(receiveString, "", "");
            //ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString());
            //Console.WriteLine($"Received: {receiveString}");
            messageReceived = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        public byte[] datatosave { get; set; }

        private void cB_iptype_TextChanged(object sender, EventArgs e)
        {
            if (cB_iptype.SelectedIndex  == 1)
            {
                tB_DestIP.Enabled = false;
            }
            else 
            {
                tB_DestIP.Enabled = true;
            }
        }

        private void tB_NAME_TextChanged(object sender, EventArgs e)
        {
            if(tB_NAME .Text .Length >6)
            {
                tB_NAME.Text = tB_NAME.Text.Substring(0, 6);
            }
        }

        private void tB_PASSWD_TextChanged(object sender, EventArgs e)
        {
            if (tB_PASSWD.Text.Length > 6)
            {
                tB_PASSWD.Text = tB_PASSWD.Text.Substring(0, 6);
            }
        }

        private void tB_ID_TextChanged(object sender, EventArgs e)
        {
            if (tB_ID.Text.Length > 2)
            {
                tB_ID.Text = tB_ID.Text.Substring(0, 2);
            }
        }

        private void tB_tagetip_TextChanged(object sender, EventArgs e)
        {
            if (tB_tagetip.Text.Length > 30)
            {
                tB_tagetip.Text = tB_tagetip.Text.Substring(0, 30);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            byte[] ipByte = new byte[200];
            byte[] comByte = new byte[200];
            byte[] reset = new byte[100];
            int cnt = 0;
            int i = 0;
            string[] strArray = tB_MAC.Text.Split(' ');
            string[] ip = tB_DestIP.Text.Split('.');
            string[] gateway = tB_Gateway.Text.Split('.');
            char[] name = new char[6];
            char[] passwd = new char[6];
            name = tB_NAME.Text.ToCharArray();
            passwd = tB_PASSWD.Text.ToCharArray();
            //client.Send(ipByte, ipByte.Length, endpoint);
            ipByte[cnt++] = 0xff;
            ipByte[cnt++] = 0x13;
            ipByte[cnt++] = 0x0b;
            ipByte[cnt++] = Convert.ToByte(strArray[0], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[1], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[2], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[3], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[4], 16);
            ipByte[cnt++] = Convert.ToByte(strArray[5], 16);
            for (i = 0; i < 6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)oldname[i];
                }
                else
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            for (i = 0; i < 6; i++)
            {
                if (i < name.Length)
                {
                    ipByte[cnt++] = (byte)oldpasswd[i];
                }
                else
                {
                    ipByte[cnt++] = 0x00;
                }
            }
            int num = 0;
            for (i = 1; i < (cnt + 1); i++)
            {
                num += ipByte[i];
            }
            ipByte[cnt++] = (byte)(num & 0xff);
            client.Send(ipByte, 21, endpoint);
            thread = new Thread(receiveUdpMsg3);
            thread.IsBackground = true;
            thread.Start(client);
        }

        private void cB_NetWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cB_NetWork.SelectedIndex == 1)
                || (cB_NetWork.SelectedIndex == 0)
                || (cB_NetWork.SelectedIndex == 4))
            {
                tB_tagetip.Enabled = true;
            }
            else
            {
                tB_tagetip.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            

        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;//端口操作
using System.Threading;             //多线程
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Configuration;
using R2000Demo.Model;

namespace R2000Demo
{
    public partial class Form1 : Form
    {
        List<TagInfo> TagList = new List<TagInfo>();
        Dictionary<string, TagInfo> m_Tags = new Dictionary<string, TagInfo>();
        Dictionary<string, TagInfo> m_SortTag = new Dictionary<string, TagInfo>();
        Dictionary<string, int> m_IndTag = new Dictionary<string, int>();
        public delegate void MyInvoke(TagInfo tag);
        public delegate void TimeInvoke();
        public delegate void BeepInvoke();
        ConcurrentQueue<bool> alarmQueue = new ConcurrentQueue<bool>();
        private Thread playsound;
        DateTime StartTime;
        private Thread RevDataFrom232;
        UInt16 threadFlag = 0;
        ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();
        int RemovedTagNums = 0;
        Byte u8HeadCnt;
        Byte u8DataPointer;
        Byte checkbyte;
        Byte[] g_Revbuf = new Byte[1024];               //接收缓存
        UInt16 g_RevDataLen;							//接收帧数据长度
        bool bCheckRet;
        bool bGetDataComplete;
        DateTime MulStartTime;
        int tagnum;
        DateTime beeptime2;
        DateTime beeptime1;
        DateTime netovertime;
        bool beepflag;

       
        public string guardarLecturaTipoC;
        public string guardarLecturaTipoA;
        //variable global guardarLecturaTipoCTime
        public DateTime guardarLecturaTipoCTime; 
        public List<AsignacionTag> listaTagsAsignados = new List<AsignacionTag>();
        public List<AsignacionTag> listaTagsLeidos = new List<AsignacionTag>();

        //System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/warning.wav");
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(R2000Demo.Properties.Resources.warning);

        // 声明 
        //uint beepI = 0x00000030;  
        //public static extern bool MessageBeep(uint uType);
        UInt16[] RxAdcTable = new UInt16[] {0x0000,          /* -25dBm */
                                       0x0000,          /* -24dBm */
                                       0x0000,          /* -23dBm */
                                       0x0001,          /* -22dBm */
                                       0x0002,          /* -21dBm */
                                       0x0005,          /* -20dBm */
                                       0x0007,          /* -19dBm */
                                       0x000B,          /* -18dBm */
                                       0x0010,          /* -17dBm */
                                       0x0116,          /* -16dBm */
                                       0x011D,          /* -15dBm */
                                       0x0126,          /* -14dBm */
                                       0x0131,          /* -13dBm */
                                       0x013E,          /* -12dBm */
                                       0x024C,          /* -11dBm */
                                       0x0260,          /* -10dBm */
                                       0x0374,          /* -09dBm */
                                       0x048B,          /* -08dBm */
                                       0x05A5,          /* -07dBm */
                                       0x06C3,          /* -06dBm */
                                       0x08E6,          /* -05dBm */
                                       0x09FF,          /* -04dBm */
                                       0x0BFF,          /* -03dBm */
                                       0x0EFF,          /* -02dBm */
                                       0x10FF,          /* -01dBm */
                                       0x14FF,          /*  00dBm */
                                       0x17FF,          /*  01dBm */
                                       0x1CFF,          /*  02dBm */
                                       0x21FF,          /*  03dBm */
                                       0x26FF,          /*  04dBm */
                                       0x2DFF,          /*  05dBm */
                                       0x34FF,          /*  06dBm */
                                       0x3CFF,          /*  07dBm */
                                       0x46FF,          /*  08dBm */
                                       0x50FF,          /*  09dBm */
                                       0x5DFF,          /*  10dBm */
                                       0x6AFF,          /*  11dBm */
                                       0x7AFF,          /*  12dBm */
                                       0x8BFF,          /*  13dBm */
                                       0x9EFF,          /*  14dBm */
                                       0xB4FF,          /*  15dBm */
                                       0xCCFF,          /*  16dBm */
                                       0xE7FF,          /*  17dBm */
                                       0xFFFF,          /*  18dBm */
                                       };

        public Form1()
        {
            InitializeComponent();
            this.listView_Disp.ListViewItemSorter = lvwColumnSorter;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void ParaSet_Click(object sender, EventArgs e)
        {
            BasicParaSet frm = new BasicParaSet();
            frm.ShowDialog();
        }
        /*********************************************************************
        *函数名称：Form1_Load()
        *函数功能：初始化函数
        *参    数：object sender, EventArgs e
        *返 回 值：无  
        *创 建 者：雷    彪 
        *创建日期：2011-04-26
        *修改记录：无  
        **********************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            //cbB_COMID.Items.AddRange(new object[] {
            //"NET181.65.15.45",
            //"NET10.10.100.254",
            //"NET192.168.1.13"});
            cbB_COMID.SelectedIndex = cbB_COMID.Items.Count > 0 ? 0 : -1;
            //cbBPortId.SelectedIndex = comboBaudrate.Items.IndexOf("9600");

            Array.Sort(ports);
            cbB_COMID.Items.AddRange(ports);

            //初始化SerialPort对象
            ReadWriteIO.comm.NewLine = "\r\n";
            ReadWriteIO.comm.RtsEnable = true;//根据实际情况吧。
            try
            {
                //player.Load();
                player.LoadAsync();
            }
            catch (Exception ex)
            {
                cB_Beep.Visible = false;
            }

            listView_Disp.GridLines = true;
            listView_Disp.FullRowSelect = true;
            listView_Disp.MultiSelect = false;


            ReaderParams param = new ReaderParams();
            ReadWriteIO readwriteio = new ReadWriteIO();

            TagList.Clear();
            m_Tags.Clear();

            //timer2.Enabled = true;
            threadFlag = 0;

            /*  */
            DisableOPT();

            cB_Language.SelectedIndex = ReaderParams.LanguageFlag; //Actualización - Jose Liza: 20210123 
            cbB_Baud.SelectedIndex = 1;
            cB_protocoltype.SelectedIndex = 0;
            cB_OutLineClear.Checked = true;
            cB_Beep.Enabled = false;
            comboBox1.SelectedIndex = ReaderParams.LanguageFlag;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            if (cbB_COMID.CanFocus)
            {
                cbB_COMID.Focus();
            }
        }
        private void menu(bool set)
        {
            BasicParaSet.Enabled = set;
            AdvanceParaSet.Enabled = set;
            TagOperate.Enabled = set;
            RegOperate.Enabled = set;
            OtherSet.Enabled = set;
            天线设置ToolStripMenuItem.Enabled = set;
            AboutusSet.Enabled = set;
            NETToolStripMenuItem.Enabled = !set;
        }
        private void DisableOPT()
        {
            //menuStrip1.Enabled = false;
            //NETToolStripMenuItem.Enabled = true;
            menu(false);
            button_singleInv.Enabled = false;
            button_inv_mul.Enabled = false;
            button_export.Enabled = false;
            button_clr.Enabled = false;
            cB_OutLineClear.Enabled = false;
            cB_Language.Enabled = true;
            cB_FastID.Enabled = false;
            cB_TagFocus.Enabled = false;
            cbB_Baud.Enabled = false;
            textBox2.Enabled = true;
        }

        private void EnableOPT()
        {
            //menuStrip1.Enabled = true;
            menu(true);
            //NETToolStripMenuItem.Enabled = false;
            cB_Beep.Enabled = true;
            button_singleInv.Enabled = true;
            button_inv_mul.Enabled = true;
            button_export.Enabled = true;
            button_clr.Enabled = true;
            cB_OutLineClear.Enabled = true;
            cB_Language.Enabled = false;
            cB_FastID.Enabled = true;
            cB_TagFocus.Enabled = true;
            textBox2.Enabled = false;
        }

        private void 标签操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdvanceParaSet aif = new AdvanceParaSet();
            aif.ShowDialog();
        }

        private void 空口协议设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReaderParams.select_TagID = "";
            TagOperate tagOpe = new TagOperate();
            tagOpe.ShowDialog();

            ReaderParams.NonFilterInSelect();
        }

        private void button_clr_Click(object sender, EventArgs e)
        {
            m_Tags.Clear();
            m_IndTag.Clear();
            m_SortTag.Clear();
            tagnum = 0;
            lb_current.Text = "0";
            lb_count.Text = "0";
            listView_Disp.Items.Clear();
            label_speed.Text = "";
            label_NumOfTags.Text = "";
            lB_times.Text = "";
            StartTime = DateTime.Now;
            LastTotalNumOfTags = 0;
            lb_totaltimes.Text = "";
            //limpiar variables (1) y (2)
            guardarLecturaTipoC = "";
            listaTagsAsignados.Clear();


        }
        private void button_inv_mul_Click(object sender, EventArgs e)
        {
            UInt32[] data = new UInt32[1];
            ReaderParams.Read_Reg_Data((byte)1, 0x0000000B, data);
            ReaderParams.ModuloId = data[0].ToString("D8");
            ReaderParams.ModuloRol = ConfigurationManager.AppSettings["modulorol"];

            button_inv_mul.Enabled = false;
            multiread();
            System.Threading.Thread.Sleep(100);
            button_inv_mul.Enabled = true;
        }
        private void multiread()//inicia las lecturas
        {
            Byte[] revbuf = new Byte[500];           //Recibir búfer
            MulStartTime = DateTime.Now;
            OutLineTime = DateTime.Now;
            if ((ReaderParams.tcpClient != null) && (ReaderParams.tcpClient.Connected))
            {
                netovertime = DateTime.Now;
                timer7.Enabled = true;
            }
            if (cB_Beep.Checked == true)
            {
                timer4.Enabled = true;
                beepflag = false;
            }
            if (0 == ReaderParams.ProtocolFlag)
            {
                RevDataFrom232 = new Thread(new ThreadStart(ReceiveDataFromUART));
            }
            else
            {
                RevDataFrom232 = new Thread(new ThreadStart(ReceiveDataFromUARTSUM));
            }

            threadFlag = 1;

            if ((button_inv_mul.Text == "连续寻卡") || (button_inv_mul.Text == "Multiple"))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    button_inv_mul.Text = "停止";
                }
                else if (1 == ReaderParams.LanguageFlag)
                {
                    button_inv_mul.Text = "Stop";
                }
                // Actualización 2021-03-10 Jose Liza
                else if (2 == ReaderParams.LanguageFlag)
                {
                    button_inv_mul.Text = "Parar";
                }
                //menuStrip1.Enabled = false;

                menu(false);
                NETToolStripMenuItem.Enabled = false;
                //NETToolStripMenuItem.Enabled = true;
                button_singleInv.Enabled = false;
                cB_OutLineClear.Enabled = false;
                cB_FastID.Enabled = false;
                cB_TagFocus.Enabled = false;
                listView_Disp.FullRowSelect = false;
                button_export.Enabled = false;
                btn_OPEN_CLOSE.Enabled = false;
                comboBox1.Enabled = false;
                button1.Enabled = false;
                //timer4.Enabled = true;
                cB_Beep.Enabled = false;
                UInt16 len = 2;
                byte[] buf = new byte[2];
                button2.Enabled = false;
                /* 获取当前时间，计算标签识别速率用的 */
                StartTime = DateTime.Now;
                lb_current.Text = "0";
                lb_count.Text = "0";
                tagnum = 0;
                m_Tags.Clear();
                m_IndTag.Clear();
                m_SortTag.Clear();
                //listView_Disp.Items.Clear();
                timer6.Enabled = true;
                buf[0] = (byte)(UInt16.Parse(textBox1.Text) >> 8);
                buf[1] = (byte)(UInt16.Parse(textBox1.Text));

                if (0 == ReaderParams.ProtocolFlag)
                {
                    ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_INVENTORY_MUL, len);
                }
                else
                {
                    ReadWriteIO.sendFrameBuild(buf, 0x17, len);
                }

                if (1 == ReaderParams.CommIntSelectFlag)
                {
                    if (ReadWriteIO.comm.IsOpen)
                    {
                        ReadWriteIO.comm.DiscardInBuffer();
                        ReadWriteIO.comm.DiscardOutBuffer();
                        if (0 == ReaderParams.ProtocolFlag)
                        {
                            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
                        }
                        else
                        {
                            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
                        }
                    }
                    else
                    {
                        if (0 == ReaderParams.LanguageFlag)
                        {
                            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }

                        return;
                    }
                }
                else
                {
                    if (true == ReaderParams.nsStream.CanRead)
                    {
                        while (true == ReaderParams.nsStream.DataAvailable)
                        {
                            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                        }
                        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                    }
                    else
                    {
                        if (0 == ReaderParams.LanguageFlag)
                        {
                            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }

                        return;
                    }
                }

                timer1.Enabled = true;

                RevDataFrom232.Start();
                //Task.Factory.StartNew(ReceiveDataFromUART);

                //Task.Factory.StartNew(PlaySound);
                //                ReceiveDataFromUART();
            }
            else if ((button_inv_mul.Text == "停止") || ((button_inv_mul.Text == "Stop")) || ((button_inv_mul.Text == "Parar")))
            {
                //timer2.Enabled = false; // Actualización 2021-03-10 Jose Liza
                if (0 == ReaderParams.LanguageFlag)
                {
                    button_inv_mul.Text = "连续寻卡";
                }
                else
                {
                    button_inv_mul.Text = "Multiple";
                }
                if ((playsound != null) && (playsound.IsAlive))
                {
                    playsound.Abort();
                }
                if ((ReaderParams.tcpClient != null) && (ReaderParams.tcpClient.Connected))
                {
                    timer7.Enabled = false;
                }
                //timer4.Enabled = false;
                System.Threading.Thread.Sleep(100);
                StopInvMul();
                button_singleInv.Enabled = true;
                menu(true);
                NETToolStripMenuItem.Enabled = false;
                //menuStrip1.Enabled = true;
                //NETToolStripMenuItem.Enabled = false;
                timer1.Enabled = false;
                cB_OutLineClear.Enabled = true;
                cB_FastID.Enabled = true;
                cB_TagFocus.Enabled = true;
                listView_Disp.FullRowSelect = true;
                button_export.Enabled = true;
                btn_OPEN_CLOSE.Enabled = true;
                LastTotalNumOfTags = 0;
                comboBox1.Enabled = true;
                button1.Enabled = true;
                timer4.Enabled = false;
                cB_Beep.Enabled = true;
                button2.Enabled = true;
                timer6.Enabled = false;
                OutLineTime = DateTime.Now;

            }
        }
        private void StopInvMul()
        {
            UInt16 len = 0;
            byte[] buf = new byte[2];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[5000];           //接收缓冲

            if (0 == ReaderParams.ProtocolFlag)
            {
                ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_STOP_INVENTORY, len);
            }
            else
            {
                ReadWriteIO.sendFrameBuild(buf, 0x18, len);
            }


            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (ReadWriteIO.comm.IsOpen)
                {
                    ReadWriteIO.comm.DiscardInBuffer();
                    ReadWriteIO.comm.DiscardOutBuffer();
                    revlen = 0;
                    ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    return;
                }

                if (0 == ReaderParams.ProtocolFlag)
                {
                    while ((revlen == 0) && (recount != 0))
                    {
                        recount--;
                        revlen = ReadWriteIO.comm.BytesToRead;
                    }
                }
                else
                {
                    while ((revlen == 0) && (recount != 0))
                    {
                        recount--;
                        revlen = ReadWriteIO.comm.BytesToRead;
                    }
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                    revlen = ReadWriteIO.comm.BytesToRead;
                    ReadWriteIO.comm.Read(revbuf, 0, revlen);
                }
            }
            else
            {
                recount = ReaderParams.Netrecount;
                if (true == ReaderParams.nsStream.CanRead)
                {
                    while (true == ReaderParams.nsStream.DataAvailable)
                    {
                        ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    }
                    revlen = 0;
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    return;
                }


                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                }
            }

            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                    && (revbuf[4] == CMD.FRAME_CMD_STOP_INVENTORY_RSP)
                    && (revbuf[5] == 0x01)))
                {
                    return;
                }
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x98)
                    && (revbuf[2] == 0x01)))
                {
                    return;
                }
            }

        }
        private void btn_OPEN_CLOSE_Click(object sender, EventArgs e)
        {
            string str = cbB_COMID.Text.Substring(0, 3);
            ReadWriteIO.device = int.Parse(textBox2.Text);
            //ManualResetEvent event();

            //Juzgue la operación de acuerdo con el objeto de puerto serie actual
            if (("关闭" == btn_OPEN_CLOSE.Text) || ("Close" == btn_OPEN_CLOSE.Text))
            {
                if (1 == threadFlag)
                {
                    if (RevDataFrom232.IsAlive)
                    {
                        RevDataFrom232.Abort();
                    }
                }

                //Haga clic cuando esté abierto para cerrar el puerto 
                if ("NET" == str)
                {
                    ReaderParams.tcpClient.Close();
                }
                else
                {
                    ReadWriteIO.comm.Close();
                }

                if (0 == ReaderParams.LanguageFlag)
                {
                    btn_OPEN_CLOSE.Text = "打开";
                }
                else
                {
                    btn_OPEN_CLOSE.Text = "Open";
                }

                cbB_COMID.Enabled = true;
                cbB_Baud.Enabled = true;
                //button4.Enabled = true;
                DisableOPT();
                cbB_Baud.Enabled = true;
            }
            else
            {
                // Operación del puerto de red
                if ("NET" == str)
                {
                    ReaderParams.CommIntSelectFlag = 0;
                    //Cree un socket de cliente,
                    ReaderParams.tcpClient = new TcpClient();
                    //Envíe una solicitud de conexión al servidor de la dirección IP especificada
                    str = cbB_COMID.Text.Substring(3);
                    IPAddress ipA = IPAddress.Parse(str);

                    try
                    {

                        IAsyncResult ar = ReaderParams.tcpClient.BeginConnect(ipA, ReaderParams.ProtocoloTCPIP, null, null);
                        bool success = ar.AsyncWaitHandle.WaitOne(1000);
                        if (!success)
                            if (0 == ReaderParams.LanguageFlag)
                            {
                                throw new Exception("El período de tiempo de espera ha expirado y el servidor especificado no está conectado");
                            }
                            else
                            {
                                throw new Exception("OVER TIME");
                            }
                        //throw new Exception("超时时间已到，未连接到指定服务器");

                    }
                    catch (Exception ex)
                    {
                        str = ex.Message;
                        if (0 == ReaderParams.LanguageFlag)
                        {
                            str += "\r\n连接失败";
                        }
                        else
                        {
                            str += "\r\nConnect failed";
                        }

                        //显示异常信息给客户。   
                        MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    //获得与服务器数据交互的流通道（NetworkStream)
                    ReaderParams.nsStream = ReaderParams.tcpClient.GetStream();

                    //Actualización José Liza 2021-03-14
                    UInt32[] data = new UInt32[1];
                    ReaderParams.Read_Reg_Data((byte)1, 0x0000000B, data);
                    ReaderParams.ModuloId = data[0].ToString("D8");


                }
                else if ("COM" == str)
                {
                    ReaderParams.CommIntSelectFlag = 1;
                    //关闭时点击，则设置好端口，波特率后打开   
                    ReadWriteIO.comm.PortName = cbB_COMID.Text;
                    ReadWriteIO.baud = int.Parse(cbB_Baud.Text);
                    ReadWriteIO.comm.BaudRate = ReadWriteIO.baud;
                    try
                    {
                        ReadWriteIO.comm.Open();
                    }
                    catch (Exception ex)
                    {
                        //捕获到异常信息，创建一个新的ReadWriteIO.comm对象，之前的不能用了。   
                        //ReadWriteIO.comm = new SerialPort();
                        str = ex.Message;
                        if (0 == ReaderParams.LanguageFlag)
                        {
                            str += "\r\n当前串口可能被占用";
                        }
                        else
                        {
                            str += "\r\nThe port is used";
                        }
                        //显示异常信息给客户。   
                        MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    ///* 获取模块ID号 */
                    //UInt32[] ID = new UInt32[2];
                    //int result = ReaderParams.GetModuleID(ID);
                    //if (result != 0)
                    //{
                    //    ReadWriteIO.comm.Close();
                    //    if (0 == ReaderParams.LanguageFlag)
                    //    {
                    //        MessageBox.Show("模块异常", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Module Error", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    }
                    //    return;
                    //}
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("端口输入错误", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("The Port ID Error", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    return;
                }

                if (0 == ReaderParams.LanguageFlag)
                {
                    btn_OPEN_CLOSE.Text = "关闭";
                }
                else
                {
                    btn_OPEN_CLOSE.Text = "Close";
                }

                cbB_COMID.Enabled = false;
                cbB_Baud.Enabled = false;
                //button4.Enabled = false;
                EnableOPT();
                button_inv_mul.Focus();
            }

        }
        private int delay(int time2delay, byte[] WriteBuf, byte CMD2, ushort len)
        {
            ReadWriteIO.sendFrameBuild(WriteBuf, CMD2, len);

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (ReadWriteIO.comm.IsOpen)
                {
                    ReadWriteIO.comm.DiscardInBuffer();
                    ReadWriteIO.comm.DiscardOutBuffer();
                    //revlen = 0;
                    ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return 0;
                }

            }
            else
            {
                // recount = ReaderParams.Netrecount;
                if (true == ReaderParams.nsStream.CanRead)
                {
                    byte[] revbuf = new byte[500];
                    while (true == ReaderParams.nsStream.DataAvailable)
                    {
                        ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    }
                    //revlen = 0;
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return 0;
                }

            }
            del d1 = new del(ReceiveDataFromUARTdel);
            IAsyncResult re1 = d1.BeginInvoke(time2delay, null, null);
            while (!re1.IsCompleted) { }
            int result = d1.EndInvoke(re1);
            return result;
        }
        public delegate int del(int a);
        private int ReceiveDataFromUARTdel(int overtime)
        {
            byte[] buf = new byte[2];
            DateTime starttime = DateTime.Now;
            //timer

            while (true)
            {
                //timer
                if ((DateTime.Now - starttime).TotalMilliseconds > overtime)
                {
                    return 0;
                }
                if (IsReceiveDatadel())
                {
                    if (GetOneByteRxDatadel(buf))					// 从接收数据中取出一个字节
                    {
                        starttime = DateTime.Now;
                        PraseMFrameDatadel(buf[0]);
                    }
                }

                if ((true == bGetDataComplete) && (true == bCheckRet))
                {


                    bCheckRet = false;
                    bGetDataComplete = false;
                    break;
                }
            }
            return 1;
        }
        private bool IsReceiveDatadel()
        {
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            bool retval = false;

            //if (RevDataFrom232.IsAlive == false)
            //{
            //    return false;
            //}

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                while ((revlen == 0) && (recount != 0))
                {
                    recount--;
                    //if (RevDataFrom232.IsAlive == false)
                    //{
                    //    return retval;
                    //}
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if ((recount != 0) || (revlen != 0))       //未收到数据
                {
                    retval = true;
                }
            }
            else
            {
                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount != 0)       //未收到数据
                {
                    retval = true;
                }
            }

            return retval;
        }
        private bool GetOneByteRxDatadel(Byte[] ch)
        {
            byte[] tmpBuf = new byte[10];
            int tmpSize = 0;

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                tmpSize = ReadWriteIO.comm.Read(tmpBuf, 0, 1);
            }
            else
            {
                tmpSize = ReaderParams.nsStream.Read(tmpBuf, 0, 1);
            }

            if (1 == tmpSize)
            {
                ch[0] = tmpBuf[0];
                return true;
            }
            else
            {
                return false;
            }
        }
        void PraseMFrameDatadel(Byte ch)
        {
            if (u8HeadCnt < 5)
            {
                switch (u8HeadCnt)
                {
                    case 0:															// 帧头
                        if (CMD.FRAME_HEAD_FIRST == ch)
                        {
                            g_Revbuf[0] = ch;
                            u8HeadCnt++;
                        }
                        break;

                    case 1:
                        if (CMD.FRAME_HEAD_SECOND == ch)								// 帧头
                        {
                            g_Revbuf[1] = ch;
                            u8HeadCnt++;
                        }
                        else
                        {
                            u8HeadCnt = 0;
                        }
                        checkbyte = 0;
                        break;

                    case 2:															// 帧长度，高字节
                        if (ch >= 0x01)
                        {
                            u8HeadCnt = 0;
                        }
                        else
                        {
                            g_Revbuf[2] = ch;
                            checkbyte ^= ch;
                            g_RevDataLen = (UInt16)(ch << 8);
                            u8HeadCnt++;
                        }
                        break;

                    case 3:                                                         // 帧长度，低字节
                        g_Revbuf[3] = ch;
                        checkbyte ^= ch;
                        g_RevDataLen += ch;
                        u8HeadCnt++;

                        break;

                    case 4:															// 帧类型
                        g_Revbuf[4] = ch;
                        u8HeadCnt++;
                        checkbyte ^= ch;
                        u8DataPointer = 0;

                        break;

                    default:
                        break;
                }
            }
            else if (u8DataPointer < (g_RevDataLen - CMD.FRAME_HEADEND_LEN))            // 帧数据
            {
                g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                checkbyte ^= ch;
                u8DataPointer++;
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN))           // 校验位
            {
                if (checkbyte == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else																// 校验位错误
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 1))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_FIRST == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 2))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_SECOND == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    if (g_RevDataLen >= 0x08)
                    {
                        bCheckRet = true;
                        bGetDataComplete = true;
                    }
                }

                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
            else
            {
                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
        }
        private void button_singleInv_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            byte[] buf = new byte[2];
            //int recount = 100000;     //número de reintentos
            //int revlen = 0;         //Longitud de los datos recibidos
            Byte[] revbuf = new Byte[500];           //Recibir búfer
            DateTime clk_time = System.DateTime.Now;

            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                buf[0] = (byte)((ReaderParams.InvTimeOut >> 8) & 0xff);
                buf[1] = (byte)(ReaderParams.InvTimeOut & 0xff);
                result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_INVENTORY_SINGLE, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, buf, 0x16, len);
            }
            //= delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_RF_LINK, len);
            if (result == 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("接收数据超时", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("over time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
            revbuf = g_Revbuf;

            if (0 == ReaderParams.ProtocolFlag)
            {
                //if (CMD.FRAME_CMD_INVENTORY_SINGLE_RSP == checkRevData(revbuf, revlen))
                //{
                string epc_tmp = "";
                string tid_tmp = "";
                byte[] byte_epc = new byte[64];
                byte[] byte_tid = new byte[64];
                byte[] byte_rssi = new byte[2];
                Int16 rssi;
                int antid;
                DateTime nowTime = System.DateTime.Now;

                TimeSpan t = nowTime - clk_time;

                //TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);
                TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime, "White");
                int length = 0;
                int rlength = 0;

                length = (revbuf[5] >> 3) * 2;
                rlength = (revbuf[2] << 8) + revbuf[3];

                System.Array.Copy(revbuf, 7, byte_epc, 0, length);
                for (int i = 0; i < length; i++)
                {
                    epc_tmp += byte_epc[i].ToString("X2");
                    if (i < length - 1)
                    {
                        epc_tmp += "-";
                    }
                }

                if (rlength > (length + 8 + 5))
                {
                    System.Array.Copy(revbuf, (7 + length), byte_tid, 0, (rlength - 13 - length));
                    for (int i = 0; i < (rlength - length - 8 - 5); i++)
                    {
                        tid_tmp += byte_tid[i].ToString("X2");

                        if (i >= 11)
                        {
                            break;
                        }

                        if (i < (rlength - length - 8 - 5 - 1))
                        {
                            tid_tmp += "-";
                        }
                    }
                }

                rssi = (Int16)(revbuf[rlength - 6] << 8);
                rssi += (Int16)(revbuf[rlength - 5]);
                antid = revbuf[rlength - 4];
                tmp.epcid = epc_tmp;
                tmp.tid = tid_tmp;
                tmp.rxrssi = (Int16)(rssi / 10);
                tmp.readcnt = 1;
                tmp.antID = antid;
                tmp.rptime = t.ToString();
                tmp.color = "white"; //Actualización 2021-03-10 Jose Liza


                AddTagToBuf(tmp); //(1)
                //UpdataListViewDisp(m_Tags[tmp.epcid + "-" + tmp.tid]);

                int sum = tagnum;
                int shang = sum / 100;
                int yushu = sum % 100;
                label_NumOfTags.Text = tagnum.ToString();
                if (yushu > 0)
                {
                    lb_current.Text = (shang + 1).ToString();
                    lb_count.Text = (shang + 1).ToString();
                    tb_P2J.Text = lb_current.Text;
                }
                else
                {
                    lb_current.Text = shang.ToString();
                    lb_count.Text = shang.ToString();
                    tb_P2J.Text = lb_current.Text;
                }
                PageShow(sum);
                //}
            }
            else
            {
                //if (0x96 == checkRevDataSUM(revbuf, revlen))
                //{
                string epc_tmp = "";
                string tid_tmp = "";
                byte[] byte_epc = new byte[64];
                byte[] byte_tid = new byte[64];
                byte[] byte_rssi = new byte[2];
                Int16 rssi;
                int antid;
                DateTime nowTime = DateTime.Now;
                TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);
                int length = 0;
                int rlength = 0;

                length = (revbuf[3] >> 3) * 2;
                rlength = revbuf[2] + 6;

                System.Array.Copy(revbuf, 5, byte_epc, 0, length);
                for (int i = 0; i < length; i++)
                {
                    epc_tmp += byte_epc[i].ToString("X2");
                    if (i < length - 1)
                    {
                        epc_tmp += "-";
                    }
                }

                if (rlength > (length + 8 + 5 - 2))
                {
                    System.Array.Copy(revbuf, (7 + length), byte_tid, 0, (rlength - 13 - length));
                    for (int i = 0; i < (rlength - length - 8 - 5 + 2); i++)
                    {
                        tid_tmp += byte_tid[i].ToString("X2");

                        if (i >= 11)
                        {
                            break;
                        }

                        if (i < (rlength - length - 8 - 5 - 1 + 2))
                        {
                            tid_tmp += "-";
                        }
                    }
                }

                rssi = (Int16)(revbuf[rlength - 6] << 8);
                rssi += (Int16)(revbuf[rlength - 5]);
                antid = revbuf[rlength - 4];
                tmp.epcid = epc_tmp;
                tmp.tid = tid_tmp;
                tmp.rxrssi = (Int16)(rssi / 10);
                tmp.readcnt = 1;
                tmp.antID = antid;

                AddTagToBuf(tmp); //(2)
                int sum = tagnum;
                int shang = sum / 100;
                int yushu = sum % 100;
                label_NumOfTags.Text = tagnum.ToString();
                if (yushu > 0)
                {
                    lb_current.Text = (shang + 1).ToString();
                    lb_count.Text = (shang + 1).ToString();
                    tb_P2J.Text = lb_current.Text;
                }
                else
                {
                    lb_current.Text = shang.ToString();
                    lb_count.Text = shang.ToString();
                    tb_P2J.Text = lb_current.Text;
                }
                PageShow(sum);
                //UpdataListViewDisp(m_Tags[tmp.epcid + "-" + tmp.tid]);
                //}         
            }


        }

        /*********************************************************************
        *函数名称：checkRevData()
        *函数功能：检验接收的数据是否有效
        *参    数：byte[] buf--数据；byte cmd--帧类型；UInt16 len--帧长度
        *返 回 值：0--无效数据，1--数据有效，但操作失败，2--单次寻卡操作应答 
        *          3--其他操作应答
        *创 建 者：雷    彪 
        *创建日期：2011-05-12
        *修改记录：无  
        **********************************************************************/
        public int checkRevData(byte[] buf, int len)
        {
            //检验帧头是否正确
            if ((buf[0] != CMD.FRAME_HEAD_FIRST) || (buf[1] != CMD.FRAME_HEAD_SECOND))
            {
                return 0;
            }
            //检验帧尾是否正确
            if ((buf[len - 2] != CMD.FRAME_END_MRK_FIRST) || (buf[len - 1] != CMD.FRAME_END_MRK_SECOND))
            {
                return 0;
            }
            //检验校验位是否正确
            if (CheckCRC(buf, (UInt16)len) == false)
            {
                return 0;
            }
            //检验长度是否正确
            if (len != ((buf[2] << 8) + buf[3]))
            {
                return 0;
            }
            //检验cmd位
            switch (buf[4])
            {
                case CMD.FRAME_CMD_FAILD_RSP:

                    return 1;

                case CMD.FRAME_CMD_INVENTORY_SINGLE_RSP:

                    return CMD.FRAME_CMD_INVENTORY_SINGLE_RSP;

                case CMD.FRAME_CMD_INVENTORY_MUL_RSP:

                    return CMD.FRAME_CMD_INVENTORY_MUL_RSP;

                default:
                    break;
            }
            return 3;
        }

        /*********************************************************************
        *函数名称：checkRevDataSUM()
        *函数功能：检验接收的数据是否有效
        *参    数：byte[] buf--数据；byte cmd--帧类型；UInt16 len--帧长度
        *返 回 值：0--无效数据，1--数据有效，但操作失败，2--单次寻卡操作应答 
        *          3--其他操作应答
        *创 建 者：雷    彪 
        *创建日期：2011-05-12
        *修改记录：无  
        **********************************************************************/
        public int checkRevDataSUM(byte[] buf, int len)
        {
            //检验帧头是否正确
            if ((buf[0] != 0xBB))
            {
                return 0;
            }
            //检验帧尾是否正确
            if ((buf[len - 2] != CMD.FRAME_END_MRK_FIRST) || (buf[len - 1] != CMD.FRAME_END_MRK_SECOND))
            {
                return 0;
            }
            //检验校验位是否正确
            if (CheckCRCSUM(buf, (UInt16)len) == false)
            {
                return 0;
            }
            //检验cmd位
            switch (buf[1])
            {
                case CMD.FRAME_CMD_FAILD_RSP:

                    return 1;

                case 0x96:

                    return 0x96;

                case 0x97:

                    return 0x97;

                default:
                    break;
            }
            return 3;
        }

        /*********************************************************************
        *函数名称：ProductCRC()
        *函数功能：生成校验字节
        *参    数：byte[] p，帧数据；UInt16 len--长度
        *返 回 值：byte--CRC的值
        *创 建 者：雷    彪 
        *创建日期：2011-05-03
        *修改记录：无  
        **********************************************************************/
        byte ProductCRC(byte[] p, UInt16 len)
        {
            UInt16 i;
            byte crc = 0;

            for (i = 2; i < len; i++)         //计算校验时，帧头和帧尾不计算
            {
                crc ^= p[i];
            }

            return crc;
        }
        /*********************************************************************
        *函数名称：CheckCRC()
        *函数功能：检验帧校验字节是否正确 
        *参    数：byte[] p，帧数据；UInt16 len--长度
        *返 回 值：true--校验正确；false--校验错误
        *创 建 者：雷    彪 
        *创建日期：2011-05-03
        *修改记录：无  
        **********************************************************************/
        bool CheckCRC(byte[] p, UInt16 len)
        {
            UInt16 i;
            byte crc = 0;

            for (i = 2; i < (len - 3); i++)         //计算校验时，帧头和帧尾不计算
            {
                crc ^= p[i];
            }

            if (crc != p[len - 3])
            {
                return false;
            }

            return true;
        }

        /*********************************************************************
        *函数名称：CheckCRCSUM()
        *函数功能：检验帧校验字节是否正确 
        *参    数：byte[] p，帧数据；UInt16 len--长度
        *返 回 值：true--校验正确；false--校验错误
        *创 建 者：雷    彪 
        *创建日期：2011-05-03
        *修改记录：无  
        **********************************************************************/
        bool CheckCRCSUM(byte[] p, UInt16 len)
        {
            UInt16 i;
            byte crc = 0;

            for (i = 1; i < (len - 3); i++)         //计算校验时，帧头和帧尾不计算
            {
                crc += p[i];
            }

            if (crc != p[len - 3])
            {
                return false;
            }

            return true;
        }

        void AddTagToBuf(TagInfo tag)
        {
            string keystr = tag.epcid + "-" + tag.tid;
            int vlue = 0;
            if (m_Tags.ContainsKey(keystr)) //Si se encuentra en la lista del formulario
            {
                if (m_Tags[keystr].tid.Equals(tag.tid))  //Se ubica en el registro del tag leido
                {
                    m_Tags[keystr].readcnt += 1;
                    //m_Tags[keystr].epcid = tag.;
                    m_Tags[keystr].rxrssi = tag.rxrssi;
                    m_Tags[keystr].antID = tag.antID;
                    m_Tags[keystr].times = tag.times;
                    m_Tags[keystr].tid = tag.tid;

                    m_IndTag.TryGetValue(keystr, out vlue);
                    m_SortTag[vlue.ToString()].readcnt = m_Tags[keystr].readcnt;
                    m_SortTag[vlue.ToString()].rxrssi = tag.rxrssi;
                    m_SortTag[vlue.ToString()].antID = tag.antID;
                    m_SortTag[vlue.ToString()].times = tag.times;
                    m_SortTag[vlue.ToString()].tid = tag.tid;

                    //Actualización Jose Liza 2021-03-14
                    m_SortTag[vlue.ToString()].moduloid = tag.moduloid;
                    m_SortTag[vlue.ToString()].modulorol = tag.modulorol;
                }
                else
                {
                    m_Tags.Add(keystr, tag);
                    m_IndTag.TryGetValue(keystr, out vlue);
                    m_SortTag.Add(vlue.ToString(), tag);
                }
            }
            else
            {
                tagnum++;
                m_Tags.Add(keystr, tag);
                m_IndTag.Add(keystr, tagnum);
                string Sort_keystr = tagnum.ToString();
                m_SortTag.Add(Sort_keystr, tag);
            }
        }

        void UpdataListViewDisp(TagInfo tag)
        {
            int flag = 0;
            foreach (ListViewItem viewitem in listView_Disp.Items)
            {
                if ((viewitem.SubItems[1].Text == tag.epcid) && (viewitem.SubItems[2].Text == tag.tid))
                {
                    viewitem.SubItems[2].Text = tag.tid;
                    viewitem.SubItems[3].Text = tag.readcnt.ToString();
                    viewitem.SubItems[4].Text = tag.rxrssi.ToString();
                    viewitem.SubItems[5].Text = tag.antID.ToString();
                    viewitem.SubItems[6].Text = tag.times.ToString();

                    //Actualizacion - Jose Liza: 20210123
                    //viewitem.SubItems[7].Text = tag.color.ToString();

                    flag = 1;
                }

                //No se considerará colores pues solo se leera una vez el tag
                if (cB_OutLineClear.Checked == true)
                {
                    /* Calcular el tiempo sin conexión */
                    TimeSpan span = DateTime.Now - DateTime.Parse(viewitem.SubItems[6].Text);
                    if ((span.Seconds >= 4) && (span.Seconds < 8))
                    {
                        //Actualización 2021-03-10 Jose Liza
                        viewitem.SubItems[7].Text = Color.Orange.Name.ToString();
                        viewitem.BackColor = Color.Orange;
                    }
                    else if (span.Seconds >= 8)
                    {
                        //Actualización 2021-03-10 Jose Liza
                        viewitem.SubItems[7].Text = Color.Red.Name.ToString();
                        viewitem.BackColor = Color.Red;
                    }
                    else
                    {
                        //Actualización 2021-03-10 Jose Liza
                        viewitem.SubItems[7].Text = Color.White.Name.ToString();
                        viewitem.BackColor = Color.White;
                    }
                }

            }
            if (flag == 0)
            {
                ListViewItem item = new ListViewItem((listView_Disp.Items.Count + 1).ToString());
                item.SubItems.Add(tag.epcid);
                item.SubItems.Add(tag.tid);
                item.SubItems.Add(tag.readcnt.ToString());
                item.SubItems.Add(tag.rxrssi.ToString());
                item.SubItems.Add(tag.antID.ToString());
                item.SubItems.Add(tag.times.ToString());
                item.SubItems.Add(tag.rptime);

                //Agregar un registro a la Grid
                listView_Disp.Items.Add(item);
                this.listView_Disp.Items[this.listView_Disp.Items.Count - 1].EnsureVisible();
            }
        }


        private void 辅助信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegOperate assistinfo = new RegOperate();
            assistinfo.ShowDialog();
        }

        private void 在线下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OtherSet oset = new OtherSet();
            //oset.ShowDialog();
            if (oset.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {

                switch (oset.Epctype)
                {
                    case "epc":
                        columnHeader7.Text = "TID";
                        break;
                    case "tid":
                        columnHeader7.Text = "TID";
                        break;
                    case "usr":
                        columnHeader7.Text = "USR";
                        break;
                    default:
                        columnHeader7.Text = "TID";
                        break;
                }
            }

            //switch (cbB_EPCTIDTOG.SelectedIndex)
            //{
            //    case 0:

            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //    default:
            //        break;
            //}
        }

        //TODO: UNO
        private bool IsReceiveData()
        {
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            bool retval = false;

            if (RevDataFrom232.IsAlive == false)
            {
                return false;
            }

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                while ((revlen == 0) && (recount != 0))
                {
                    recount--;
                    if (RevDataFrom232.IsAlive == false)
                    {
                        return retval;
                    }
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if ((recount != 0) || (revlen != 0))       //未收到数据
                {
                    retval = true;
                }
            }
            else
            {
                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount != 0)       //未收到数据
                {
                    retval = true;
                }
            }

            return retval;
        }

        // Obtener un byte del búfer de recepción
        private bool GetOneByteRxData(Byte[] ch)
        {
            byte[] tmpBuf = new byte[10];
            int tmpSize = 0;

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                tmpSize = ReadWriteIO.comm.Read(tmpBuf, 0, 1);
            }
            else
            {
                tmpSize = ReaderParams.nsStream.Read(tmpBuf, 0, 1);
            }

            if (1 == tmpSize)
            {
                ch[0] = tmpBuf[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        // 解帧函数
        void PraseMFrameData(Byte ch)
        {
            if (u8HeadCnt < 5)
            {
                switch (u8HeadCnt)
                {
                    case 0:															// 帧头
                        if (CMD.FRAME_HEAD_FIRST == ch)
                        {
                            g_Revbuf[0] = ch;
                            u8HeadCnt++;
                        }
                        break;

                    case 1:
                        if (CMD.FRAME_HEAD_SECOND == ch)								// 帧头
                        {
                            g_Revbuf[1] = ch;
                            u8HeadCnt++;
                        }
                        else
                        {
                            u8HeadCnt = 0;
                        }
                        checkbyte = 0;
                        break;

                    case 2:															// 帧长度，高字节
                        if (ch >= 0x01)
                        {
                            u8HeadCnt = 0;
                        }
                        else
                        {
                            g_Revbuf[2] = ch;
                            checkbyte ^= ch;
                            g_RevDataLen = (UInt16)(ch << 8);
                            u8HeadCnt++;
                        }
                        break;

                    case 3:                                                         // 帧长度，低字节
                        g_Revbuf[3] = ch;
                        checkbyte ^= ch;
                        g_RevDataLen += ch;
                        u8HeadCnt++;

                        break;

                    case 4:															// 帧类型
                        g_Revbuf[4] = ch;
                        u8HeadCnt++;
                        checkbyte ^= ch;
                        u8DataPointer = 0;

                        break;

                    default:
                        break;
                }
            }
            else if (u8DataPointer < (g_RevDataLen - CMD.FRAME_HEADEND_LEN))            // 帧数据
            {
                g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                checkbyte ^= ch;
                u8DataPointer++;
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN))           // 校验位
            {
                if (checkbyte == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else																// 校验位错误
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 1))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_FIRST == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 2))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_SECOND == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    if (g_RevDataLen >= 0x08)
                    {
                        bCheckRet = true;
                        bGetDataComplete = true;
                    }
                }

                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
            else
            {
                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
        }


        // 解帧函数
        void PraseMFrameDataSUM(Byte ch)
        {
            if (u8HeadCnt < 3)
            {
                switch (u8HeadCnt)
                {
                    case 0:															// 帧头
                        if (0xBB == ch)
                        {
                            g_Revbuf[0] = ch;
                            u8HeadCnt++;
                        }
                        else
                        {
                            u8HeadCnt = 0;
                        }
                        checkbyte = 0;
                        break;

                    case 1:															// 帧长度，高字节
                        g_Revbuf[1] = ch;
                        u8HeadCnt++;
                        checkbyte += ch;
                        u8DataPointer = 0;
                        break;

                    case 2:															// 帧长度，低字节
                        g_Revbuf[2] = ch;
                        checkbyte += ch;
                        g_RevDataLen = ch;
                        u8HeadCnt++;
                        break;

                    default:
                        break;
                }
            }
            else if (u8DataPointer < (g_RevDataLen))            // 帧数据
            {
                g_Revbuf[3 + u8DataPointer] = ch;
                checkbyte += ch;
                u8DataPointer++;
            }
            else if (u8DataPointer == (g_RevDataLen))           // 校验位
            {
                if (checkbyte == ch)
                {
                    g_Revbuf[3 + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else																// 校验位错误
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen + 1))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_FIRST == ch)
                {
                    g_Revbuf[3 + u8DataPointer] = ch;
                    u8DataPointer++;
                }
                else
                {
                    u8HeadCnt = 0;
                    u8DataPointer = 0;
                }
            }
            else if (u8DataPointer == (g_RevDataLen + 2))       // 帧尾
            {
                if (CMD.FRAME_END_MRK_SECOND == ch)
                {
                    g_Revbuf[3 + u8DataPointer] = ch;
                    if (g_RevDataLen > 0x06)
                    {
                        bCheckRet = true;
                        bGetDataComplete = true;
                    }
                }

                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
            else
            {
                u8HeadCnt = 0;
                u8DataPointer = 0;
            }
        }


        // 解析连续寻卡应答帧的ID
        bool ParseMulReadFrameDataProcess()
        {
            string epc_tmp = "";
            string tid_tmp = "";
            byte[] byte_epc = new byte[64];
            byte[] byte_tid = new byte[64];
            byte[] byte_rssi = new byte[2];
            Int16 rssi;
            int antid;
            DateTime nowTime = System.DateTime.Now;
            TimeSpan MulT = nowTime - MulStartTime;

            TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);
            int length = 0;
            int rlength = 0;
            int i;

            length = (g_Revbuf[5] >> 3) * 2;
            rlength = (g_Revbuf[2] << 8) + g_Revbuf[3];

            System.Array.Copy(g_Revbuf, 7, byte_epc, 0, length);
            for (i = 0; i < length; i++)
            {
                epc_tmp += byte_epc[i].ToString("X2");
                if (i < length - 1)
                {
                    epc_tmp += "-";
                }
            }

            if (rlength > (length + 8 + 5))
            {
                System.Array.Copy(g_Revbuf, (7 + length), byte_tid, 0, (rlength - length - 8 - 5));
                for (i = 0; i < (rlength - length - 8 - 5); i++)
                {
                    tid_tmp += byte_tid[i].ToString("X2");

                    //if (i >= 11)
                    //{
                    //    break;
                    //}

                    if (i < (rlength - length - 8 - 5 - 1))
                    {
                        tid_tmp += "-";
                    }
                }
            }

            rssi = (Int16)(g_Revbuf[rlength - 6] << 8);
            rssi += (Int16)(g_Revbuf[rlength - 5]);
            antid = g_Revbuf[rlength - 4];
            tmp.epcid = epc_tmp;
            tmp.tid = tid_tmp;
            tmp.rxrssi = (Int16)(rssi / 10);
            tmp.readcnt = 1;
            tmp.antID = antid;
            tmp.rptime = MulT.ToString();
            if (tmp.rptime.Length <= 11)
            {
                tmp.rptime += ".000";
            }
            tmp.rptime = tmp.rptime.Substring(0, 12);

            //Actualización José Liza 2021-03-14
            tmp.moduloid = ReaderParams.ModuloId;
            tmp.modulorol = ReaderParams.ModuloRol;

            AddTagToBuf(tmp); //(3)
            //MyInvoke mi = new MyInvoke(UpdataListViewDisp);            
            //this.BeginInvoke(mi, new Object[] { m_Tags[tmp.epcid+"-"+tmp.tid] });
            BeepInvoke beep = new BeepInvoke(beeping);
            this.BeginInvoke(beep);
            //if (cB_Beep.Checked == true)
            //{
            //    //alarmQueue.Enqueue(true);    //加入队列  
            //    beeptime1 = DateTime.Now;
            //    beepflag = true;
            //}
            return true;
        }
        void beeping()
        {
            if (cB_Beep.Checked == true)
            {
                //    //alarmQueue.Enqueue(true);    //加入队列  
                beeptime1 = DateTime.Now;
                beepflag = true;
            }
        }


        // 解析连续寻卡应答帧的ID
        bool ParseMulReadFrameDataProcessSUM()
        {
            string epc_tmp = "";
            string tid_tmp = "";
            byte[] byte_epc = new byte[64];
            byte[] byte_tid = new byte[64];
            byte[] byte_rssi = new byte[2];
            Int16 rssi;
            int antid;
            DateTime nowTime = System.DateTime.Now;
            TimeSpan MulTSum = nowTime - MulStartTime;
            TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);
            int length = 0;
            int rlength = 0;
            int i;

            length = (g_Revbuf[3] >> 3) * 2;
            rlength = g_Revbuf[2] + 6;

            System.Array.Copy(g_Revbuf, 5, byte_epc, 0, length);
            for (i = 0; i < length; i++)
            {
                epc_tmp += byte_epc[i].ToString("X2");
                if (i < length - 1)
                {
                    epc_tmp += "-";
                }
            }

            if (rlength > (length + 8 + 5 - 2))
            {
                System.Array.Copy(g_Revbuf, (7 + length), byte_tid, 0, (rlength - length - 8 - 5 + 2));
                for (i = 0; i < (rlength - length - 8 - 5 + 2); i++)
                {
                    tid_tmp += byte_tid[i].ToString("X2");

                    //if (i >= 11)
                    //{
                    //    break;
                    //}

                    if (i < (rlength - length - 8 - 5 - 1 + 2))
                    {
                        tid_tmp += "-";
                    }
                }
            }

            rssi = (Int16)(g_Revbuf[rlength - 6] << 8);
            rssi += (Int16)(g_Revbuf[rlength - 5]);
            antid = g_Revbuf[rlength - 4];
            tmp.epcid = epc_tmp;
            tmp.tid = tid_tmp;
            tmp.rxrssi = (Int16)(rssi / 10);
            tmp.readcnt = 1;
            tmp.antID = antid;
            tmp.rptime = MulTSum.ToString();

            AddTagToBuf(tmp);
            //MyInvoke mi = new MyInvoke(UpdataListViewDisp);
            //this.BeginInvoke(mi, new Object[] { m_Tags[tmp.epcid + "-" + tmp.tid] });

            return true;
        }


        void ShowTime()
        {
            TimeSpan span = DateTime.Now - MulStartTime;
            label8.Text = span.ToString();
        }

        private void PlaySound() //播放声音方法
        {
            if (playsound.IsAlive == false)
            {
                return;
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Application.StartupPath + @"/warning.wav");
            player.Load();
            while (true)
            {
                if (playsound.IsAlive == false)
                {
                    player.Stop();
                    player.Dispose();
                    return;
                }
                if (true == beepflag)
                {
                    beepflag = false;
                    player.PlayLooping();
                    break;
                }
            }
            while (true)
            {
                if (playsound.IsAlive == false)
                {
                    player.Stop();
                    player.Dispose();
                    return;
                }
                beeptime2 = DateTime.Now;
                if ((beeptime2 - beeptime1).TotalMilliseconds < 1000)
                {
                    if (true == beepflag)
                    {
                        beeptime1 = DateTime.Now;
                        beepflag = false;

                    }
                    else
                    {

                    }
                }
                else
                {
                    if (true == beepflag)
                    {
                        //开始

                        beepflag = false;
                        player.Load();
                        player.PlayLooping();

                    }
                    else
                    {
                        player.Stop();
                        //停止
                    }
                }
            }
        }

        // 解帧处理函数
        Byte Handle_Uart_Command()
        {
            Byte retval = 0;

            if ((true == bGetDataComplete) && (true == bCheckRet))
            {
                netovertime = DateTime.Now;
                switch (g_Revbuf[4])
                {
                    case CMD.FRAME_CMD_INVENTORY_MUL_RSP:
                        {
                            if (g_Revbuf[3] == 8)
                            {
                                TimeInvoke tim = new TimeInvoke(ShowTime);
                                this.BeginInvoke(tim);
                                bCheckRet = false;
                                bGetDataComplete = false;
                                return 1;
                            }

                            if (true == ParseMulReadFrameDataProcess())
                            {
                                retval = 1;
                            }
                            break;
                        }

                    default:                            // default reply error
                        {
                            break;
                        }
                }

                bCheckRet = false;
                bGetDataComplete = false;
            }

            return retval;
        }

        // 解帧处理函数
        Byte Handle_Uart_CommandSUM()
        {
            Byte retval = 0;

            if ((true == bGetDataComplete) && (true == bCheckRet))
            {
                switch (g_Revbuf[1])
                {
                    case 0x97:
                        {
                            if (true == ParseMulReadFrameDataProcessSUM())
                            {

                                retval = 1;
                            }
                            break;
                        }

                    default:                            // default reply error
                        {
                            break;
                        }
                }

                bCheckRet = false;
                bGetDataComplete = false;
            }

            return retval;
        }

        private void beep()
        {

        }

        //TODO: DOS
        private void ReceiveDataFromUART()
        {
            byte[] buf = new byte[2];

            if (RevDataFrom232.IsAlive == false)
            {
                return;
            }

            while (true)
            {
                if (RevDataFrom232.IsAlive == false)
                {
                    return;
                }

                if (IsReceiveData())
                {
                    if (GetOneByteRxData(buf))					// Sacar un byte de los datos recibidos
                    {
                        PraseMFrameData(buf[0]);
                    }
                }

                if (1 == Handle_Uart_Command())		            // Recibió con éxito un marco de datos
                {

                }
            }
        }

        private void ReceiveDataFromUARTSUM()
        {
            byte[] buf = new byte[2];

            if (RevDataFrom232.IsAlive == false)
            {
                return;
            }

            while (true)
            {
                if (RevDataFrom232.IsAlive == false)
                {
                    return;
                }

                if (IsReceiveData())
                {
                    if (GetOneByteRxData(buf))					// 从接收数据中取出一个字节
                    {
                        PraseMFrameDataSUM(buf[0]);
                    }
                }

                if (1 == Handle_Uart_CommandSUM())		            // 成功接收一帧数据
                {

                }
            }
        }


        private void listView_Disp_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查点击的列是不是现在的排序列.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 重新设置此列的排序方法.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            // 用新的排序方法对ListView排序
            this.listView_Disp.Sort();
        }

        private void listView_Disp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((button_inv_mul.Text == "连续寻卡") || (button_inv_mul.Text == "Multiple"))
                 && (("关闭" == btn_OPEN_CLOSE.Text) || ("Close" == btn_OPEN_CLOSE.Text)))
            {
                foreach (ListViewItem tempItem in this.listView_Disp.SelectedItems)
                {
                    ReaderParams.select_TagID = tempItem.SubItems[1].Text;
                }

                TagOperate tagOpe = new TagOperate();
                tagOpe.ShowDialog();

                ReaderParams.NonFilterInSelect();
            }
        }

        private void cB_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReaderParams.LanguageFlag = (UInt16)cB_Language.SelectedIndex;

            // 简体中文
            if (0 == ReaderParams.LanguageFlag)
            {
                label1.Text = "端口号:";
                label5.Text = "波特率:";
                label2.Text = "标签个数:";
                label3.Text = "识别速率:";
                label4.Text = "识别时间:";
                label6.Text = "读取次数:";

                btn_OPEN_CLOSE.Text = "打开";
                button_singleInv.Text = "单次寻卡";
                button_inv_mul.Text = "连续寻卡";
                button_export.Text = "导出";
                button_clr.Text = "清除";

                cB_OutLineClear.Text = "离线清除";
                cB_FastID.Text = "FastID";
                cB_TagFocus.Text = "TagFocus";

                BasicParaSet.Text = "基本设置";
                AdvanceParaSet.Text = "高级设置";
                TagOperate.Text = "标签操作";
                RegOperate.Text = "寄存器";
                OtherSet.Text = "其他设置";
                AboutusSet.Text = "关于我们";
                天线设置ToolStripMenuItem.Text = "天线设置";
                NETToolStripMenuItem.Text = "网口设置";

                groupBox1.Text = "工作区:";
                groupBox2.Text = "端口:";
                groupBox3.Text = "信息统计:";

                columnHeader1.Text = "标签";
                columnHeader2.Text = "EPC";
                columnHeader3.Text = "读取次数";
                columnHeader4.Text = "RSSI(dBm)";
                columnHeader5.Text = "天线号";
                columnHeader6.Text = "Last Time";
                columnHeader7.Text = "TID";
                columnHeader8.Text = "首次读取耗时(ms)";

                cB_Beep.Text = "寻卡响声";
                bt_FPage.Text = "首页";
                button6.Text = "上一页";
                button9.Text = "下一页";
                button8.Text = "尾页";
                bt_J2.Text = "跳转";
                label8.Text = "持续时间：";
            }
            else if (0 == ReaderParams.LanguageFlag)
            {
                label1.Text = "Port:";
                label5.Text = "Baud:";
                label2.Text = "Number:";
                label3.Text = "Speed:";
                label4.Text = "Time:";
                label6.Text = "TotalTimes:";

                btn_OPEN_CLOSE.Text = "Open";
                button_singleInv.Text = "Single Inv";
                button_inv_mul.Text = "Multiple";
                button_export.Text = "Export";
                button_clr.Text = "Clean";

                cB_OutLineClear.Text = "Off-line Clean";
                cB_FastID.Text = "FastID";
                cB_TagFocus.Text = "TagFocus";

                BasicParaSet.Text = "Basic Settings";
                AdvanceParaSet.Text = "Advanced Settings";
                TagOperate.Text = "Tag Operate";
                RegOperate.Text = "Regs";
                OtherSet.Text = "Other Settings";
                天线设置ToolStripMenuItem.Text = "ANT SET";
                NETToolStripMenuItem.Text = "Ethernet Setting";
                AboutusSet.Text = "About us";

                groupBox1.Text = "WorkSpace:";
                groupBox2.Text = "Port:";
                groupBox3.Text = "Information:";

                columnHeader1.Text = "Tag";
                columnHeader2.Text = "EPC";
                columnHeader3.Text = "Inv Times";
                columnHeader4.Text = "RSSI(dBm)";
                columnHeader5.Text = "ANT ID";
                columnHeader6.Text = "Last Time";
                columnHeader7.Text = "TID";
                columnHeader8.Text = "First Read time Cost(ms)";

                cB_Beep.Text = "Beep";
                bt_FPage.Text = "First";
                button6.Text = "PREV";
                button9.Text = "NEXT";
                button8.Text = "Last";
                bt_J2.Text = "JUMP";
                label8.Text = "Duration:";


            }
            //Actualización 2021-03-10 Jose Liza
            else
            {
                label1.Text = "Port:";
                label5.Text = "Baud:";
                label2.Text = "Numero:";
                label3.Text = "Velocidad:";
                label4.Text = "Hora:";
                label6.Text = "Total Veces:";

                btn_OPEN_CLOSE.Text = "Abrir";
                button_singleInv.Text = "Inv individual";
                button_inv_mul.Text = "Multiple";
                button_export.Text = "Exportar";
                button_clr.Text = "Limpiar";

                cB_OutLineClear.Text = "Limpiar Off-line";
                cB_FastID.Text = "FastID";
                cB_TagFocus.Text = "TagFocus";

                BasicParaSet.Text = "Ajustes básicos";
                AdvanceParaSet.Text = "Ajustes avanzados";
                TagOperate.Text = "Operar Tag";
                RegOperate.Text = "Regs";
                OtherSet.Text = "Otros ajustes";
                天线设置ToolStripMenuItem.Text = "ANT SET";
                NETToolStripMenuItem.Text = "Configuración de Ethernet";
                AboutusSet.Text = "Sobre nosotros";

                groupBox1.Text = "WorkSpace:";
                groupBox2.Text = "Port:";
                groupBox3.Text = "Information:";

                columnHeader1.Text = "Tag";
                columnHeader2.Text = "EPC";
                columnHeader3.Text = "Inv Times";
                columnHeader4.Text = "RSSI(dBm)";
                columnHeader5.Text = "ANT ID";
                columnHeader6.Text = "Last Time";
                columnHeader7.Text = "TID";
                columnHeader8.Text = "First Read time Cost(ms)";

                cB_Beep.Text = "Beep";
                bt_FPage.Text = "First";
                button6.Text = "PREV";
                button9.Text = "NEXT";
                button8.Text = "Last";
                bt_J2.Text = "JUMP";
                label8.Text = "Duration:";
            }

            label_speed.Location = new Point((label3.Location.X + label3.Size.Width + 4), 38);
            lB_times.Location = new Point((label4.Location.X + label4.Size.Width + 4), 38);
            label_NumOfTags.Location = new Point((label2.Location.X + label2.Size.Width + 4), 79);
            lb_totaltimes.Location = new Point((label6.Location.X + label6.Size.Width + 4), 79);
        }

        private void cB_FastID_CheckedChanged(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (cB_FastID.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            WriteBuf[1] = 0x00;


            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_FASTID, len);

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (ReadWriteIO.comm.IsOpen)
                {
                    ReadWriteIO.comm.DiscardInBuffer();
                    ReadWriteIO.comm.DiscardOutBuffer();
                    revlen = 0;
                    ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }

                while ((revlen < 9) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    revlen = ReadWriteIO.comm.BytesToRead;
                    ReadWriteIO.comm.Read(revbuf, 0, revlen);
                }
            }
            else
            {
                recount = ReaderParams.Netrecount;
                if (true == ReaderParams.nsStream.CanRead)
                {
                    while (true == ReaderParams.nsStream.DataAvailable)
                    {
                        ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    }
                    revlen = 0;
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }


                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                }
            }

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_FASTID_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("FastID设置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("FastID Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void cB_TagFocus_CheckedChanged(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (cB_TagFocus.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            WriteBuf[1] = 0x00;


            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_TAGFOCUS, len);

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (ReadWriteIO.comm.IsOpen)
                {
                    ReadWriteIO.comm.DiscardInBuffer();
                    ReadWriteIO.comm.DiscardOutBuffer();
                    revlen = 0;
                    ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }

                while ((revlen < 9) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    revlen = ReadWriteIO.comm.BytesToRead;
                    ReadWriteIO.comm.Read(revbuf, 0, revlen);
                }
            }
            else
            {
                recount = ReaderParams.Netrecount;
                if (true == ReaderParams.nsStream.CanRead)
                {
                    while (true == ReaderParams.nsStream.DataAvailable)
                    {
                        ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    }
                    revlen = 0;
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }


                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount == 0)       //未收到数据
                {
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                }
            }

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_TAGFOCUS_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("TagFocus设置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("TagFocus Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void cbB_COMID_MouseClick(object sender, MouseEventArgs e)
        {
            int oldcur = cbB_COMID.SelectionStart;
            int oldinx = cbB_COMID.SelectedIndex;

            //cbB_COMID.SelectedIndex = 0;
            // string str = cbB_COMID.Text;
            //cbB_COMID.Items.Clear();
            //cbB_COMID.Items.Add(str);
            //cbB_COMID.Items.AddRange(new object[] {
            //"NET192.168.1.13",
            //"NET181.65.15.45",
            //"NET10.10.100.254"
            //});

            string[] ports = SerialPort.GetPortNames();
            //Array.Sort(ports);
            //cbB_COMID.Items.AddRange(ports);
            cbB_COMID.SelectedIndex = cbB_COMID.Items.Count > 0 ? 0 : -1;

            cbB_COMID.SelectedIndex = (oldinx > (cbB_COMID.Items.Count - 1) ? (cbB_COMID.Items.Count - 1) : oldinx);
            cbB_COMID.SelectionStart = oldcur;
            //cbB_COMID.Focus();
            //cbB_COMID.Select(this.cbB_COMID.Text.Length, 0);
        }

        private void cbB_COMID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == cbB_COMID.SelectedIndex)
            {
                cbB_Baud.Enabled = false;
            }
            else
            {
                cbB_Baud.Enabled = true;
            }
        }

        private void AboutusSet_Click(object sender, EventArgs e)
        {
            Aboutus iap = new Aboutus();
            iap.ShowDialog();
        }

        private void TagSpecial_Click(object sender, EventArgs e)
        {
            TagSpecial spe = new TagSpecial();
            spe.ShowDialog();
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        /// <summary>
        /// 执行导出数据
        /// </summary>
        public void ExportToExcel()
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            //sfd.DefaultExt = ".csv";
            //sfd.Filter = "*.csv|*.csv";
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    // DoExport(this.listView_Disp, sfd.FileName);
            //    //  DoExporttag(sfd.FileName);
            //    SaveCSV(this.listView_Disp, sfd.FileName);
            //}
            string folder = ReaderParams.FolderCSV;
            string fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv";
            string path = folder + fileName;

            SaveCSV(this.listView_Disp, path);

        }
        /// <summary>
        /// 具体导出的方法
        /// </summary>
        /// <param name="listView">ListView</param>
        /// <param name="strFileName">导出到的文件名</param>
        //private void DoExport(ListView listView, string strFileName)
        //{
        //    try
        //    {
        //        int rowNum = listView.Items.Count;
        //        int columnNum = listView.Items[0].SubItems.Count;
        //        int rowIndex = 1;
        //        int columnIndex = 0;
        //        if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
        //        {
        //            return;
        //        }
        //        if (rowNum > 0)
        //        {

        //            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        //            if (xlApp == null)
        //            {
        //                MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
        //                return;
        //            }
        //            xlApp.DefaultFilePath = "";
        //            xlApp.DisplayAlerts = true;
        //            xlApp.SheetsInNewWorkbook = 1;
        //            Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
        //            //将ListView的列名导入Excel表第一行
        //            foreach (ColumnHeader dc in listView.Columns)
        //            {
        //                columnIndex++;
        //                xlApp.Cells[rowIndex, columnIndex] = dc.Text;
        //            }
        //            //将ListView中的数据导入Excel中
        //            for (int i = 0; i < rowNum; i++)
        //            {
        //                rowIndex++;
        //                columnIndex = 0;
        //                for (int j = 0; j < columnNum; j++)
        //                {
        //                    columnIndex++;
        //                    //注意这个在导出的时候加了“\t” 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
        //                    xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
        //                }
        //            }
        //            //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。
        //            xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //            xlApp = null;
        //            xlBook = null;
        //            MessageBox.Show("Export OK!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //        if (0 == ReaderParams.LanguageFlag)
        //        {
        //            str += "\r\n导出失败";
        //        }
        //        else
        //        {
        //            str += "\r\nExport failed";
        //        }

        //        //显示异常信息给客户。   
        //        MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return;
        //    }
        //}
        public void SaveCSV(ListView listView, string fullPath)
        {
            int rowNum = tagnum;
            if (listView.Items.Count < 1)
            {
                return;
            }
            int columnNum = listView.Items[0].SubItems.Count;
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
            //写出列名称
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                data += listView.Columns[i].Text;
                if (i < listView.Columns.Count - 1)
                {
                    data += ",";
                }
            }

            StringBuilder sb = new StringBuilder();

            //Actualización - Jose Liza: 20210124
            //sw.WriteLine(data);
            sb.AppendLine(data);

            foreach (ListViewItem item in listView.Items)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                    item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text,
                    item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text,
                    item.SubItems[6].Text, item.SubItems[7].Text, item.SubItems[8].Text));
            }
            sw.WriteLine(sb.ToString());

            sw.Close();
            fs.Close();

            //Actualizacion - Jose Liza: 20210125

            //EmitirSonido();
            ActivarGPIO("3", true);
            ///////////////

        }

        //Actualizacion - Jose Liza: 20210125
        //Procedimientos Nuevos
        private static void EmitirSonido()
        {
            Byte[] WriteBuf = new Byte[100];
            WriteBuf[0] = (byte)0;
            WriteBuf[1] = (byte)0;

            UInt16 len = 2;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_BUZZER, len);
            int recount = 80000;
            recount = ReaderParams.Netrecount;
            Byte[] revbuf = new Byte[500];
            if (true == ReaderParams.nsStream.CanRead)
            {
                while (true == ReaderParams.nsStream.DataAvailable)
                {
                    ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                }
                ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (8 + CMD.FRAME_HEADEND_LEN));//发送测试信息
            }
        }

        private void ActivarGPIO(string gpioNum, bool estado)
        {
            byte mask = 0;
            byte data = 0;
            int result = 0;
            if (gpioNum == "1")
            {
                mask = 0x01;
                data = 0x01;
            }
            if (gpioNum == "2")
            {
                mask = 0x02;
                data = 0x02;
            }
            if (gpioNum == "3")
            {
                mask = 0x04;
                data = 0x04;
            }
            if (gpioNum == "4")
            {
                mask = 0x08;
                data = 0x08;
            }

            if (estado == true)
            {
                result = SendSetGPIOStatus(mask, data);
            }
            else
            {
                data = 0x00;
                result = SendSetGPIOStatus(mask, data);
            }

        }

        private int SendSetGPIOStatus(byte mask, byte data)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;                     //重试次数
            int revlen = 0;                          //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = mask;
            WriteBuf[1] = data;
            int result = 0;

            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_GPIO, len);

            revbuf = g_Revbuf;

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_GPIO_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -2;
            }

            return 0;
        }
        //Fin de procedimientos nuevos
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string str = cbB_COMID.Text.Substring(0, 3);

            //根据当前串口对象，来判断操作   
            if (("关闭" == btn_OPEN_CLOSE.Text) || ("Close" == btn_OPEN_CLOSE.Text))
            {
                if (1 == threadFlag)
                {
                    if (RevDataFrom232.IsAlive)
                    {
                        RevDataFrom232.Abort();
                    }
                }

                StopInvMul();

                //打开时点击，则关闭端口   
                if ("NET" == str)
                {
                    ReaderParams.tcpClient.Close();
                }
                else
                {
                    ReadWriteIO.comm.Close();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (true == Cb_Test.Checked)
            {
                Test frm = new Test();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    timer3.Enabled = true;
                }
            }
            else
            {
                timer3.Enabled = false;
            }


        }

        private void Cbut_Test_Click(object sender, EventArgs e)
        {
            Test frm = new Test();
            frm.ShowDialog();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    timer3.Enabled = true;
            //}
        }

        private void cB_protocoltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReaderParams.ProtocolFlag = (UInt16)cB_protocoltype.SelectedIndex;
        }

        private void Mul_Key_Sum(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 500000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = (Byte)(0);
            WriteBuf[1] = (Byte)(0);
            WriteBuf[2] = (Byte)(0);
            WriteBuf[3] = (Byte)(0);



            if ((comboBox1.SelectedIndex == 2) || (comboBox1.SelectedIndex == 3))//c1sp
            {
                WriteBuf[2] |= 0x20;
            }
            else
            {
                WriteBuf[2] &= 0xDF;
            }

            if ((comboBox1.SelectedIndex == 1) || (comboBox1.SelectedIndex == 3))//c2sp
            {
                WriteBuf[2] |= 0x10;
            }
            else
            {
                WriteBuf[2] &= 0xEF;
            }





            UInt16 len = 0;

            len = 3;
            ReadWriteIO.sendFrameBuild(WriteBuf, 0xE6, len);

            if (ReadWriteIO.comm.IsOpen)
            {
                ReadWriteIO.comm.DiscardInBuffer();
                ReadWriteIO.comm.DiscardOutBuffer();
                revlen = 0;

                ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));

            }
            else
            {
                MessageBox.Show("端口未打开", "ERROR", MessageBoxButtons.OK);
                //  label2.Text = "端口未打开";
            }

            while ((revlen < 9) && (recount != 0))
            {
                recount--;
                revlen = ReadWriteIO.comm.BytesToRead;
            }




            if (recount == 0)       //未收到数据
            {
                MessageBox.Show("未收到数据", "ERROR", MessageBoxButtons.OK);
                //label2.Text = "未收到数据";
            }
            else
            {
                revlen = ReadWriteIO.comm.BytesToRead;
                ReadWriteIO.comm.Read(revbuf, 0, revlen);
            }



            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == 0xE7)
                && (revbuf[5] == 0x01)))
            {

                //label2.Text = "设置失败";
                MessageBox.Show("设置失败", "ERROR", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("设置成功", "success", MessageBoxButtons.OK);
                //label2.Text = "设置成功";

            }
        }


        private void 读卡统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
        }

        private void 测试板切换分路功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label9.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[] Array1;
            Array1 = new int[10] { 1, 2, 3, 4, 5, 10, 20, 30, 40, 60 };
            int[] intArray2;
            intArray2 = new int[10];
            string str1 = "";
            string str2 = "";
            for (int i = 0; i <= 9; i++)
            {
                int j = 0;
                for (long ii = 0; ii < tagnum; ii++)
                {
                    string a = m_SortTag[(ii + 1).ToString()].rptime.Substring(6, 6);
                    //double k = Convert.ToDouble(m_SortTag[(ii + 1).ToString()].rptime);
                    double k = double.Parse(a);
                    if (k <= Array1[i])
                    {
                        j++;
                    }
                }

                intArray2[i] = j;
                str1 += "前" + Array1[i] + "秒共读取" + intArray2[i] + "个标签\r\n";
                str2 += intArray2[i] + "\t";
            }
            Clipboard.SetDataObject(str2, true);

            MessageBox.Show(str1, "读取结果", MessageBoxButtons.OK);

        }

        private void 天线设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test frm = new Test(); if (0 == ReaderParams.LanguageFlag)
            {
                frm.Text = "天线设置";
            }
            else
            {
                frm.Text = "ANT Setting";
            }
            frm.ShowDialog();
        }
        DateTime time1;

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "开始")
            {
                button3.Text = "停止";
                button_inv_mul.Visible = false;
                textBox3.Visible = false;
                time1 = DateTime.Now;
                button_inv_mul_Click(sender, e);
                timer5.Enabled = true;
            }
            else
            {
                button3.Text = "开始";
                button_inv_mul.Visible = true;
                textBox3.Visible = true;
                timer5.Enabled = false;
            }
        }

        private void 双间隙读取测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3.Visible = true;
            textBox3.Visible = true;
            label10.Visible = true;
        }

        private void 网络设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNET fn = new FormNET();
            fn.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormNET fn = new FormNET();
            fn.ShowDialog();
        }

        DateTime OutLineTime;
        public void PageShow(int num)
        {
            //listView_Disp.Items.Clear();

            if (num >= 100)
            {
                if (tagnum - num >= 0)
                {
                    for (long i = (num - 100); i < num; i++)
                    {
                        ListViewItem item = new ListViewItem((listView_Disp.Items.Count + num - 100 + 1).ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].epcid);
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].tid);
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].readcnt.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].rxrssi.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].antID.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].times.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].rptime);
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].color);

                        //Actualizacion - José Liza: 2021-03-14 - Agregar columnas ModuloId y ModuloRol
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].moduloid.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].modulorol);

                        if ((cB_OutLineClear.Checked == true))
                        {
                            /* Calcular el tiempo sin conexión */
                            if ((button_inv_mul.Text != "连续寻卡") && (button_inv_mul.Text != "Multiple"))
                            {
                                OutLineTime = DateTime.Now;
                            }
                            TimeSpan span = OutLineTime - m_SortTag[(i + 1).ToString()].times;
                            if ((span.TotalSeconds >= 4) && (span.TotalSeconds < 8))
                            {
                                item.BackColor = Color.Orange;
                                item.SubItems[8].Text = Color.Orange.Name.ToString();
                            }
                            else if (span.TotalSeconds >= 8)
                            {
                                item.BackColor = Color.Red;
                                item.SubItems[8].Text = Color.Red.Name.ToString();
                            }
                            else
                            {
                                item.BackColor = Color.White;
                                item.SubItems[8].Text = Color.White.Name.ToString();
                            }
                        }


                        //Verificar si ya fue leido el EPC
                        string epc_leido = item.SubItems[1].Text;
                        string ant_leido = item.SubItems[5].Text;
                        IEnumerable<ListViewItem> lv = listView_Disp.Items.Cast<ListViewItem>();
                        bool epc_existe = lv.Where(r => r.SubItems[1].Text == epc_leido && r.SubItems[5].Text == ant_leido).Count() > 0;

                        if (!epc_existe)
                        {
                            listView_Disp.Items.Add(item);
                            this.listView_Disp.Items[this.listView_Disp.Items.Count - 1].EnsureVisible();

                            AsignacionTag tagTipoC = Guardarlectura(item);

                            if (tagTipoC.Tipo == "C")
                            {
                                //TODO: Guardar en una variable Global

                                guardarLecturaTipoC = tagTipoC.Tipo;

                                //TODO: Traer la lista de los tag asignado al usuario y guardarlo en una lista (A)
                                listaTagsAsignados = GetTagsAsignados(tagTipoC.UsuarioId);


                                return;
                            }
                            else
                            {
                                //TODO:Verificar que existe una lectura de tipo C sino sonar la alarma
                                if(tagTipoC.Tipo != "C")
                                {
                                    ActivarAlarma(100);
                                }

                                //TODO: Verificar si el tag se encuentra en la lista (A) 

                                AsignacionTag tagEncontrado= listaTagsAsignados.Find(a=>a.Epc.Equals(epc_existe));
                                if (tagEncontrado != null)
                                {
                                    ActivarAlarma(100);
                                }
                                /*
                                    foreach (AsignacionTag asignacionTagA in listaTagsAsignados)
                                {
                                    if (asignacionTagA.Epc == tagTipoC.Epc)
                                    {
                                        //mostrar el tag leido
                                        asignacionTagA.Tipo = tagTipoC.Tipo;
                                    }
                                    //TODO: Sino se encuentra en la lista (A) sonar alarma'
                                    else
                                    {
                                        ActivarAlarma(100);
                                    }
                                }
                                */

                                
                            }

                        }

                        bool epc_paso_por_caja = new ReadRepository().GetReadInBox(item.SubItems[1].Text);
                        if (epc_existe && !epc_paso_por_caja)
                        {
                            ActivarAlarma(100);
                        }

                    }
                }
                else
                {
                    for (long i = (num - 100); i < tagnum; i++)
                    {
                        ListViewItem item = new ListViewItem((listView_Disp.Items.Count + num - 100 + 1).ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].epcid);
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].tid);
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].readcnt.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].rxrssi.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].antID.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].times.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].rptime);
                        //Actualizacion - Jose Liza: 20210123
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].color);

                        //Actualizacion - José Liza: 2021-03-14 - Agregar columnas ModuloId y ModuloRol
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].moduloid.ToString());
                        item.SubItems.Add(m_SortTag[(i + 1).ToString()].modulorol);

                        if ((cB_OutLineClear.Checked == true))
                        {
                            /* Calcular el tiempo sin conexión */
                            if ((button_inv_mul.Text != "连续寻卡") && (button_inv_mul.Text != "Multiple"))
                            {
                                OutLineTime = DateTime.Now;
                            }
                            TimeSpan span = OutLineTime - m_SortTag[(i + 1).ToString()].times;
                            if ((span.TotalSeconds >= 4) && (span.TotalSeconds < 8))
                            {
                                item.BackColor = Color.Orange;
                                item.SubItems[8].Text = Color.Orange.Name.ToString();
                            }
                            else if (span.TotalSeconds >= 8)
                            {
                                item.BackColor = Color.Red;
                                item.SubItems[8].Text = Color.Red.Name.ToString();
                            }
                            else
                            {
                                item.BackColor = Color.White;
                                item.SubItems[8].Text = Color.White.Name.ToString();
                            }
                        }


                        //Verificar si ya fue leido el EPC
                        string epc_leido = item.SubItems[1].Text;
                        string ant_leido = item.SubItems[5].Text;
                        IEnumerable<ListViewItem> lv = listView_Disp.Items.Cast<ListViewItem>();
                        bool epc_existe = lv.Where(r => r.SubItems[1].Text == epc_leido && r.SubItems[5].Text == ant_leido).Count() > 0;

                        //TODO: Comprobar si tag ya fue leido
                        if (!epc_existe)
                        {
                            listView_Disp.Items.Add(item);
                            this.listView_Disp.Items[this.listView_Disp.Items.Count - 1].EnsureVisible();

                            //int id = Guardarlectura(item);
                            //if (ReaderParams.ModuloRol == item.SubItems[10].Text && ReaderParams.ModuloRol == "Puerta") //Si el modulo se esta ejecutando para una puerta
                            //{
                            //    if (!PasoPorCaja(id))
                            //    {
                            //        ActivarAlarma(100);
                            //        return;
                            //    }
                            //}
                        }

                        bool epc_paso_por_caja = new ReadRepository().GetReadInBox(item.SubItems[1].Text);
                        if (epc_existe && !epc_paso_por_caja)
                        {
                            ActivarAlarma(100);
                        }

                    }
                }
            }
            else
            {
                for (long i = 0; i < num; i++)
                {
                    ListViewItem item = new ListViewItem((listView_Disp.Items.Count + 1).ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].epcid);
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].tid);
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].readcnt.ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].rxrssi.ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].antID.ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].times.ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].rptime);

                    //Actualizacion - Jose Liza: 2021-01-24
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].color);

                    //Actualizacion - José Liza: 2021-03-14 - Agregar columnas ModuloId y ModuloRol
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].moduloid.ToString());
                    item.SubItems.Add(m_SortTag[(i + 1).ToString()].modulorol);

                    //Verificar si ya fue leido el EPC
                    string epc_leido = item.SubItems[1].Text;
                    string ant_leido = item.SubItems[5].Text;
                    IEnumerable<ListViewItem> lv = listView_Disp.Items.Cast<ListViewItem>();
                    bool epc_existe = lv.Where(r => r.SubItems[1].Text == epc_leido && r.SubItems[5].Text == ant_leido).Count() > 0;

                    if ((cB_OutLineClear.Checked == true))
                    {
                        /* Calcular el tiempo sin conexión */
                        if ((button_inv_mul.Text != "连续寻卡") && (button_inv_mul.Text != "Multiple"))
                        {
                            OutLineTime = DateTime.Now;
                        }
                        TimeSpan span = OutLineTime - m_SortTag[(i + 1).ToString()].times;
                        if ((span.TotalSeconds >= 4) && (span.TotalSeconds < 8))
                        {
                            item.BackColor = Color.Orange;
                            item.SubItems[8].Text = Color.Orange.Name.ToString();
                        }
                        else if (span.TotalSeconds >= 8)
                        {
                            item.BackColor = Color.Red;
                            item.SubItems[8].Text = Color.Red.Name.ToString();
                        }
                        else
                        {
                            item.BackColor = Color.White;
                            item.SubItems[8].Text = Color.White.Name.ToString();
                        }
                        
                    }


                    //TODO: Comprobar si tag ya fue leido
                    AsignacionTag tagTipoC = Guardarlectura(item);  
                    //AsignacionTag tagTipoC = new AsignacionTag();
                    
                    if (!epc_existe)
                    {
                        //tagTipoC = Guardarlectura(item);
                        listView_Disp.Items.Add(item);
                        this.listView_Disp.Items[this.listView_Disp.Items.Count - 1].EnsureVisible();

                        //TODO: Si el tag es de tipo C traer la lista de tags de tipo A
                        if (tagTipoC.Tipo == "C")
                        {                            
                            guardarLecturaTipoC = tagTipoC.Tipo;
                            
                            listaTagsAsignados = GetTagsAsignados(tagTipoC.UsuarioId);

                            listView_Disp.Items[listView_Disp.Items.Count - 1].BackColor = Color.DodgerBlue;
                            listView_Disp.Items[listView_Disp.Items.Count - 1].SubItems[8].Text = Color.DodgerBlue.Name.ToString();
                            GuardarColor(item.SubItems[1].Text, item.SubItems[5].Text, Color.DodgerBlue.Name.ToString());
                            GuardarColorAsignacionTag(item.SubItems[1].Text, item.SubItems[8].Text, Convert.ToInt32(item.SubItems[9].Text));

                            return;
                        }

                        //TODO: Si el tag es de tipo A se almacena la lectura en una lista
                        if (tagTipoC.Tipo == "A")
                        {
                            guardarLecturaTipoA = tagTipoC.Tipo;

                            listaTagsLeidos.Add(tagTipoC); 
                        }     
                        
                    }
                    else{
                        //TODO: Actualizar el color que tiene el epc de tipo A en el View
                        if (tagTipoC.Tipo == "A")
                        {
                            var itemToUpdate = listView_Disp.Items.Cast<ListViewItem>().Where(x => x.SubItems[1].Text == tagTipoC.Epc).FirstOrDefault();
                            itemToUpdate.SubItems[8].Text = tagTipoC.Color;
                            itemToUpdate.BackColor = Color.FromName(tagTipoC.Color);
                        }
                        
                    }
                    
                    if (guardarLecturaTipoC == "C" && guardarLecturaTipoA == "A")
                    {
                        //TODO: Se busca el tag en la lista de tags tipo C
                        AsignacionTag tagTipoA = listaTagsAsignados.Where(x => x.Epc == tagTipoC.Epc).FirstOrDefault();
                        
                        foreach (AsignacionTag tag in listaTagsLeidos)
                        {
                            if (tag.Epc == tagTipoC.Epc || item.SubItems[8].Text == Color.SkyBlue.Name.ToString())
                            {
                                //TODO: Si el tag tipo A fue encontrado en la lista de tags tipo C se guarda la lectura
                                if (tagTipoA != null)
                                {
                                    //se almacena la lectura
                                    item.BackColor = Color.Green;
                                    item.SubItems[8].Text = Color.Green.Name.ToString();
                                    GuardarColor(item.SubItems[1].Text, item.SubItems[5].Text, Color.Green.Name.ToString());
                                    GuardarColorAsignacionTag(item.SubItems[1].Text, item.SubItems[8].Text, Convert.ToInt32(item.SubItems[9].Text));
                                }
                                else
                                {
                                    //TODO: Si el tag tipo A no fue encontrado en la lista de tags tipo C se activa la alarma
                                    item.BackColor = Color.Red;
                                    item.SubItems[8].Text = Color.Red.Name.ToString();
                                    GuardarColor(item.SubItems[1].Text, item.SubItems[5].Text, Color.Red.Name.ToString());
                                    GuardarColorAsignacionTag(item.SubItems[1].Text, item.SubItems[8].Text, Convert.ToInt32(item.SubItems[9].Text));
                                    ActivarAlarma(100);
                                }
                            }
                        }
                    }
                    else if (guardarLecturaTipoC != "C" && tagTipoC.Tipo == "A")
                    {
                        //TODO: Si el epc es de tipo A y no se ha leido un epc de tipo C
                        item.BackColor = Color.LightGray;
                        item.SubItems[8].Text = Color.LightGray.Name.ToString();
                    }

                }                      
            }
            
        }

        private void GuardarColor(string epc, string ant, string color)
        {
            ReadRepository readRepository = new ReadRepository();
            readRepository.UpdateReadTag(epc, ant, color);
        }

        //Actualizar AsignacionagColor
        private void GuardarColorAsignacionTag(string epc, string color, int modulo) 
        {
            ReadRepository readRepository = new ReadRepository();
            readRepository.UpdateAsignacionTag(epc, color, modulo);
        }

        //private bool PasoPorCaja(ListViewItem item)
        private bool PasoPorCaja(int id)
        {
            return ValidarPago(id);
        }
        //Actualizacion - Jose Liza: 20210131
        private AsignacionTag Guardarlectura(ListViewItem item)
        {
            ReadRepository obj = new ReadRepository();
            return obj.AddReadTag(new ReadTag(
                item.SubItems[0].Text,
                item.SubItems[1].Text,
                item.SubItems[2].Text,
                int.Parse(item.SubItems[3].Text),
                int.Parse(item.SubItems[4].Text),
                int.Parse(item.SubItems[5].Text),
                DateTime.Parse(item.SubItems[6].Text),
                DateTime.Parse(item.SubItems[7].Text),
                item.SubItems[8].Text,
                int.Parse(item.SubItems[9].Text),
                item.SubItems[10].Text));
        }

        private List<AsignacionTag> GetTagsAsignados(int idUsuario)
        {
            ReadRepository obj = new ReadRepository();
            return obj.GetTagsAsignados(idUsuario);
        }
        private int GuardarIncidencia(ListViewItem item)
        {
            ReadRepository obj = new ReadRepository();

            return obj.AddIncidenciaReadTag(new ReadTag(
                item.SubItems[0].Text,
                item.SubItems[1].Text,
                item.SubItems[2].Text,
                int.Parse(item.SubItems[3].Text),
                int.Parse(item.SubItems[4].Text),
                int.Parse(item.SubItems[5].Text),
                DateTime.Parse(item.SubItems[6].Text),
                DateTime.Parse(item.SubItems[7].Text),
                item.SubItems[8].Text,
                int.Parse(item.SubItems[9].Text),
                item.SubItems[10].Text));
        }
        private bool ValidarPago(int id)
        {
            bool result;
            ReadRepository obj = new ReadRepository();
            result = obj.GetReadInBox(id);
            return result;
        }

        private void bt_FPage_Click(object sender, EventArgs e)
        {
            if (lb_current.Text != "0")
            {
                PageShow(100);

                lb_current.Text = "1";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (int.Parse(lb_current.Text) > 1)
            {
                lb_current.Text = (int.Parse(lb_current.Text) - 1).ToString();
                PageShow((int.Parse(lb_current.Text)) * 100);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (int.Parse(lb_current.Text) < int.Parse(lb_count.Text))
            {
                lb_current.Text = (int.Parse(lb_current.Text) + 1).ToString();
                PageShow((int.Parse(lb_current.Text)) * 100);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PageShow((int.Parse(lb_count.Text)) * 100);
            lb_current.Text = lb_count.Text;
        }

        private void bt_J2_Click(object sender, EventArgs e)
        {
            if ((int.Parse(tb_P2J.Text) > 0)

                )
            {
                if (int.Parse(tb_P2J.Text) <= int.Parse(lb_count.Text))
                {
                    PageShow((int.Parse(tb_P2J.Text)) * 100);
                    lb_current.Text = tb_P2J.Text;
                }
            }
        }

        private void 网口模块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNET net = new FormNET();
            if (0 == ReaderParams.LanguageFlag)
            {
                net.Text = "网口设置";
                //MessageBox.Show("获取天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                net.Text = "Ethernet module Setting";
                //MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //"Ethernet module Setting
            net.ShowDialog();
        }

        int LastTotalNumOfTags = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //DateTime nowTime = DateTime.Now;
            //double tOfDay = nowTime.TimeOfDay.TotalMilliseconds; 
            //double tStart = StartTime.TimeOfDay.TotalMilliseconds;
            //double totalMs = nowTime.TimeOfDay.TotalMilliseconds - StartTime.TimeOfDay.TotalMilliseconds;
            int totalNumOfTags = 0;

            for (int i = 0; i < tagnum; i++)
            {
                totalNumOfTags += m_SortTag[(i + 1).ToString()].readcnt;
            }

            //totalNumOfTags = tagnum;
            double speed = 0.0;
            speed = (totalNumOfTags - LastTotalNumOfTags + RemovedTagNums);
            LastTotalNumOfTags = totalNumOfTags;
            RemovedTagNums = 0;

            label_speed.Text = speed.ToString();

            TimeSpan span = DateTime.Now - StartTime;
            lB_times.Text = ((int)span.TotalSeconds).ToString();
            lb_totaltimes.Text = totalNumOfTags.ToString();

            if (0 == ReaderParams.LanguageFlag)
            {

                label_speed.Text += "个/秒";
                lB_times.Text += "秒";
            }
            else
            {
                label_speed.Text += " tags/s..";
                lB_times.Text += " s";
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            //Actualizacion - Jose Liza: 20210127
            int regs = listView_Disp.Items.Count;
            if (regs > 0)
            {
                //parar lectura
                button_inv_mul.Enabled = false;
                multiread();
                System.Threading.Thread.Sleep(500);
                button_inv_mul.Enabled = true;

                //Exportar a excell
                //ExportToExcel();

                //Guardas lectuas en SQLServer
                //Guardarlecturas();

                //Iniciar lectura
                button_inv_mul.Enabled = false;
                multiread();
                System.Threading.Thread.Sleep(1000);
                button_inv_mul.Enabled = true;
            }
            //EmitirSonido();

            //ActivarGPIO("3", true);
            //Thread.Sleep(300);
            //ActivarGPIO("3", false);

            //label_NumOfTags.Text = (listView_Disp.Items.Count).ToString();
            //tB_NowTimes.Text = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            beeptime2 = DateTime.Now;
            if ((beeptime2 - beeptime1).TotalMilliseconds < 1000)
            {
                if (beepflag)
                {
                    //player.PlayLooping();
                    player.PlaySync();
                    beeptime1 = DateTime.Now;
                    beepflag = false;

                }
            }
            else
            {
                if (beepflag)
                {
                    //开始

                    beepflag = false;
                    //label10.Text = "on";
                    //player .Load();
                    //player.PlayLooping();
                    player.PlaySync();

                }
                else
                {
                    //label10.Text = "off";
                    //player.Stop();
                    //timer3.Enabled = true;
                    // timer4.Enabled = false;
                    //停止
                }
            }
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (beepflag)
            {
                beepflag = false;
                player.PlayLooping();
                player.PlaySync();
                timer3.Enabled = true;
                timer4.Enabled = false;
            }
        }
        int flag = 0;
        private void timer5_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - time1).TotalMilliseconds > int.Parse(textBox3.Text))
            {
                if (flag == 0)
                {
                    flag = 1;
                    time1 = DateTime.Now;
                    button_inv_mul_Click(sender, e);
                }
                else
                {
                    flag = 0;
                    time1 = DateTime.Now;
                    button_inv_mul_Click(sender, e);
                }
            }
        }
        private void timer6_Tick(object sender, EventArgs e)
        {
            //listView_Disp.Items.Clear();
            int sum = tagnum;
            int shang = sum / 100;
            int yushu = sum % 100;
            label_NumOfTags.Text = tagnum.ToString();
            if (yushu > 0)
            {
                lb_current.Text = (shang + 1).ToString();
                lb_count.Text = (shang + 1).ToString();
                tb_P2J.Text = lb_current.Text;
            }
            else
            {
                lb_current.Text = shang.ToString();
                lb_count.Text = shang.ToString();
                tb_P2J.Text = lb_current.Text;
            }
            PageShow(sum);
        }
        private void timer7_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - netovertime).TotalSeconds > 10)
            {
                netovertime = DateTime.Now;
                ReaderParams.tcpClient.Close();
                RevDataFrom232.Abort();
                //button_inv_mul_Click(sender, e);
                ReaderParams.CommIntSelectFlag = 0;
                //Cree un socket de cliente,
                ReaderParams.tcpClient = new TcpClient();
                //Envíe una solicitud de conexión al servidor de la dirección IP especificada
                string str = cbB_COMID.Text.Substring(3);
                IPAddress ipA = IPAddress.Parse(str);
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        IAsyncResult ar = ReaderParams.tcpClient.BeginConnect(ipA, ReaderParams.ProtocoloTCPIP, null, null);
                        bool success = ar.AsyncWaitHandle.WaitOne(1000);
                        if (!success)
                        {

                        }
                        else
                        {
                            if (0 == ReaderParams.ProtocolFlag)
                            {
                                RevDataFrom232 = new Thread(new ThreadStart(ReceiveDataFromUART));
                            }
                            else
                            {
                                RevDataFrom232 = new Thread(new ThreadStart(ReceiveDataFromUARTSUM));
                            }
                            ReaderParams.nsStream = ReaderParams.tcpClient.GetStream();
                            netovertime = DateTime.Now;
                            RevDataFrom232.Start();

                            return;
                        }


                    }
                    catch (Exception ex)
                    {

                    }
                }
                //throw new Exception("超时时间已到，未连接到指定服务器");
                //str = ex.Message;
                if (0 == ReaderParams.LanguageFlag)
                {
                    str += "超时时间已到，未连接到指定服务器\r\n连接失败";
                }
                else
                {
                    str += "over time\r\nConnect failed";
                }

                //显示异常信息给客户。   
                MessageBox.Show(str, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                timer7.Enabled = false;
            }
        }

        bool EstaAlarmaActivada = false;
        private void ActivarAlarma(int intervalMiliSeg)
        {
            if (EstaAlarmaActivada)
                return;

            EstaAlarmaActivada = true;
            timPIO.Interval = intervalMiliSeg;
            timPIO.Enabled = true;
            timPIO.Start();
            ActivarGPIO("3", true);
            multiread();
        }
        private void timPIO_Tick(object sender, EventArgs e)
        {

            ActivarGPIO("3", false);
            EstaAlarmaActivada = false;
            timPIO.Enabled = false;
            timPIO.Stop();
            multiread();
            EstaAlarmaActivada = false;
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            天线设置ToolStripMenuItem.Visible = !天线设置ToolStripMenuItem.Visible;
            //ActivarGPIO("3", chkTest.Checked);
            //ActivarAlarma(300);
        }

        private void tmr_Limpiar_TipoC_Tick(object sender, EventArgs e)
        {
            button_clr.Enabled = true;
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox4.Text=="jose")
            {

            天线设置ToolStripMenuItem.Visible = !天线设置ToolStripMenuItem.Visible;
                textBox4.Text = String.Empty;
            }
        }
    }


    public class ListViewColumnSorter : IComparer
    {
        /**/
        /// <summary>
        /// 指定按照哪个列排序
        /// </summary>
        private int ColumnToSort;
        /**/
        /// <summary>
        /// 指定排序的方式
        /// </summary>
        private SortOrder OrderOfSort;
        /**/
        /// <summary>
        /// 声明CaseInsensitiveComparer类对象，
        /// 参见ms-help://MS.VSCC.2003/MS.MSDNQTR.2003FEB.2052/cpref/html/frlrfSystemCollectionsCaseInsensitiveComparerClassTopic.htm
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;
        /**/
        /// <summary>
        /// 构造函数
        /// </summary>
        public ListViewColumnSorter()
        {
            // 默认按第一列排序
            ColumnToSort = 0;
            // 排序方式为不排序
            OrderOfSort = SortOrder.None;
            // 初始化CaseInsensitiveComparer类对象
            ObjectCompare = new CaseInsensitiveComparer();
        }
        /**/
        /// <summary>
        /// 重写IComparer接口.
        /// </summary>
        /// <param name="x">要比较的第一个对象</param>
        /// <param name="y">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;
            // 将比较对象转换为ListViewItem对象
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;
            // 比较
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            // 根据上面的比较结果返回正确的比较结果
            if (OrderOfSort == SortOrder.Ascending)
            {
                // 因为是正序排序，所以直接返回结果
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // 如果是反序排序，所以要取负值再返回
                return (-compareResult);
            }
            else
            {
                // 如果相等返回0
                return 0;
            }
        }
        /**/
        /// <summary>
        /// 获取或设置按照哪一列排序.
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }
        /**/
        /// <summary>
        /// 获取或设置排序方式.
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }

}

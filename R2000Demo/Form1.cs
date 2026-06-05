using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Web.Script.Serialization;
using R2000Demo.Model;

namespace R2000Demo
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Diccionarios de tags en memoria
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

        // ── P1: Paginación virtual del ListView (anti memory-leak) ───────────
        // El ListView nunca tendrá más de PageSize en el control WinForms.
        // Todos los datos viven en m_SortTag; PageShow solo renderiza la página actual.
        private int _currentPage = 1;

        // Calcula cuántas filas caben visualmente en el ListView según su altura actual

        private int _pageSize = 20;
        private int PageSize
        {
            get { return _pageSize; }
        }

        // Llama este método desde el UI thread para recalcular cuántas filas caben
        private void RecalcularPageSize()
        {
            if (listView_Disp.Items.Count > 0)
            {
                int rowHeight = listView_Disp.GetItemRect(0).Height;
                if (rowHeight > 0)
                {
                    _pageSize = Math.Max(1, listView_Disp.ClientSize.Height / rowHeight);
                    return;
                }
            }
            // Fallback con fuente SimSun 10.5pt ≈ 18px por fila
            _pageSize = Math.Max(1, listView_Disp.ClientSize.Height / 18);
        }

        // ────────────────────────────────────────────────────────────────────

        Byte u8HeadCnt;
        Byte u8DataPointer;
        Byte checkbyte;
        Byte[] g_Revbuf = new Byte[1024];
        UInt16 g_RevDataLen;
        bool bCheckRet;
        bool bGetDataComplete;
        DateTime MulStartTime;
        int tagnum;
        DateTime beeptime2;
        DateTime beeptime1;
        DateTime netovertime;
        bool beepflag;
        bool estadoLectura = false;

        System.Media.SoundPlayer player =
            new System.Media.SoundPlayer(R2000Demo.Properties.Resources.warning);

        // Tabla ADC para cálculo de RSSI (valores del hardware R2000)
        UInt16[] RxAdcTable = new UInt16[]
        {
            0x0000, 0x0000, 0x0000, 0x0001, 0x0002, 0x0005, 0x0007, 0x000B,
            0x0010, 0x0116, 0x011D, 0x0126, 0x0131, 0x013E, 0x024C, 0x0260,
            0x0374, 0x048B, 0x05A5, 0x06C3, 0x08E6, 0x09FF, 0x0BFF, 0x0EFF,
            0x10FF, 0x14FF, 0x17FF, 0x1CFF, 0x21FF, 0x26FF, 0x2DFF, 0x34FF,
            0x3CFF, 0x46FF, 0x50FF, 0x5DFF, 0x6AFF, 0x7AFF, 0x8BFF, 0x9EFF,
            0xB4FF, 0xCCFF, 0xE7FF, 0xFFFF
        };

        bool estadoAlarmaActivada = false;

        public Form1()
        {
            InitializeComponent();
            this.listView_Disp.ListViewItemSorter = lvwColumnSorter;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        // ════════════════════════════════════════════════════════════════════
        // INICIALIZACIÓN
        // ════════════════════════════════════════════════════════════════════

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            cbB_COMID.SelectedIndex = cbB_COMID.Items.Count > 0 ? 0 : -1;
            cbB_COMID.Items.AddRange(ports);

            ReadWriteIO.comm.NewLine = "\r\n";
            ReadWriteIO.comm.RtsEnable = true;
            try { player.LoadAsync(); }
            catch { cB_Beep.Visible = false; }

            listView_Disp.GridLines = true;
            listView_Disp.FullRowSelect = true;
            listView_Disp.MultiSelect = false;

            new ReaderParams();
            new ReadWriteIO();
            TagList.Clear();
            m_Tags.Clear();
            threadFlag = 0;

            DisableOPT();
            cB_Language.SelectedIndex = ReaderParams.LanguageFlag;
            cbB_Baud.SelectedIndex = 1;
            cB_protocoltype.SelectedIndex = 0;
            cB_OutLineClear.Checked = true;
            cB_Beep.Enabled = false;
            comboBox1.SelectedIndex = ReaderParams.LanguageFlag;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            if (cbB_COMID.CanFocus) cbB_COMID.Focus();

            InicializarSincronizacion();
            RecalcularPageSize(); // calcular PageSize inicial
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
            NETToolStripMenuItem.Enabled = set;
        }

        private void controles(bool set)
        {
            BasicParaSet.Visible = set;
            AdvanceParaSet.Visible = set;
            TagOperate.Visible = set;
            RegOperate.Visible = set;
            OtherSet.Visible = set;
            天线设置ToolStripMenuItem.Visible = set;
            AboutusSet.Visible = set;
            NETToolStripMenuItem.Visible = set;
            chkTest.Visible = set;
            button_export.Visible = set;
            tb_P2J.Visible = set;
            bt_J2.Visible = set;
            button8.Visible = set;
            button9.Visible = set;
            bt_FPage.Visible = set;
            button6.Visible = set;
            lb_current.Visible = set;
            label13.Visible = set;
            lb_count.Visible = set;
            textBox2.Visible = set;
            cB_Language.Visible = set;
        }

        private void DisableOPT()
        {
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
            menu(true);
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

        // ════════════════════════════════════════════════════════════════════
        // CONEXIÓN Y CONTROL DE LECTURA
        // ════════════════════════════════════════════════════════════════════

        private void btn_OPEN_CLOSE_Click(object sender, EventArgs e)
        {
            string str = cbB_COMID.Text.Substring(0, 3);
            ReadWriteIO.device = int.Parse(textBox2.Text);

            if (("关闭" == btn_OPEN_CLOSE.Text) || ("Close" == btn_OPEN_CLOSE.Text))
            {
                if (1 == threadFlag && RevDataFrom232.IsAlive)
                    RevDataFrom232.Abort();

                if ("NET" == str) ReaderParams.tcpClient.Close();
                else ReadWriteIO.comm.Close();

                btn_OPEN_CLOSE.Text = "Abierto";
                cbB_COMID.Enabled = true;
                cbB_Baud.Enabled = true;
                DisableOPT();
                cbB_Baud.Enabled = true;
            }
            else
            {
                if ("NET" == str)
                {
                    ReaderParams.CommIntSelectFlag = 0;
                    ReaderParams.tcpClient = new TcpClient();
                    str = cbB_COMID.Text.Substring(3);
                    IPAddress ipA = IPAddress.Parse(str);
                    try
                    {
                        IAsyncResult ar = ReaderParams.tcpClient.BeginConnect(
                            ipA, ReaderParams.ProtocoloTCPIP, null, null);
                        bool success = ar.AsyncWaitHandle.WaitOne(1000);
                        if (!success)
                            throw new Exception(
                                "El período de tiempo de espera ha expirado y el servidor especificado no está conectado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\nConnect failed",
                            "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    ReaderParams.nsStream = ReaderParams.tcpClient.GetStream();
                    ReaderParams.ModuloId = ConfigurationManager.AppSettings["tiempo01"];
                }
                else if ("COM" == str)
                {
                    ReaderParams.CommIntSelectFlag = 1;
                    ReadWriteIO.comm.PortName = cbB_COMID.Text;
                    ReadWriteIO.baud = int.Parse(cbB_Baud.Text);
                    ReadWriteIO.comm.BaudRate = ReadWriteIO.baud;
                    try { ReadWriteIO.comm.Open(); }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\nThe port is used",
                            "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("The Port ID Error",
                        "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                btn_OPEN_CLOSE.Text = "Close";
                cbB_COMID.Enabled = false;
                cbB_Baud.Enabled = false;
                EnableOPT();
                button_inv_mul.Focus();
            }
        }

        private void button_inv_mul_Click(object sender, EventArgs e) => clicBtnMultiple();

        private void clicBtnMultiple()
        {
            ReaderParams.ModuloId = ConfigurationManager.AppSettings["tiempo01"];
            ReaderParams.ModuloRol = ConfigurationManager.AppSettings["modulorol"];
            button_inv_mul.Enabled = false;
            multiread();
            System.Threading.Thread.Sleep(100);
            button_inv_mul.Enabled = true;
        }

        private void multiread()
        {
            if (button_inv_mul.Text == "连续寻卡" || button_inv_mul.Text == "Multiple")
                ActivarLecturas();
            else if (button_inv_mul.Text == "停止" || button_inv_mul.Text == "Stop" || button_inv_mul.Text == "Parar")
                DetenerLecturas();
        }

        private void ActivarLecturas()
        {
            Byte[] revbuf = new Byte[500];
            MulStartTime = DateTime.Now;
            OutLineTime = DateTime.Now;

            if (ReaderParams.tcpClient != null && ReaderParams.tcpClient.Connected)
            {
                netovertime = DateTime.Now;
                timer7.Enabled = true;
            }
            if (cB_Beep.Checked)
            {
                timer4.Enabled = true;
                beepflag = false;
            }

            RevDataFrom232 = (0 == ReaderParams.ProtocolFlag)
                ? new Thread(new ThreadStart(ReceiveDataFromUART))
                : new Thread(new ThreadStart(ReceiveDataFromUARTSUM));

            threadFlag = 1;
            if (2 == ReaderParams.LanguageFlag) button_inv_mul.Text = "Parar";

            menu(false);
            NETToolStripMenuItem.Enabled = false;
            button_singleInv.Enabled = false;
            cB_OutLineClear.Enabled = false;
            cB_FastID.Enabled = false;
            cB_TagFocus.Enabled = false;
            listView_Disp.FullRowSelect = false;
            button_export.Enabled = false;
            btn_OPEN_CLOSE.Enabled = false;
            comboBox1.Enabled = false;
            button1.Enabled = false;
            cB_Beep.Enabled = false;
            button2.Enabled = false;

            StartTime = DateTime.Now;
            lb_current.Text = "0";
            lb_count.Text = "0";
            tagnum = 0;
            _currentPage = 1;
            m_Tags.Clear();
            m_IndTag.Clear();
            m_SortTag.Clear();
            timer6.Enabled = true;

            UInt16 len = 2;
            byte[] buf = new byte[2];
            buf[0] = (byte)(UInt16.Parse(textBox1.Text) >> 8);
            buf[1] = (byte)(UInt16.Parse(textBox1.Text));

            if (0 == ReaderParams.ProtocolFlag)
                ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_INVENTORY_MUL, len);
            else
                ReadWriteIO.sendFrameBuild(buf, 0x17, len);

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (!ReadWriteIO.comm.IsOpen)
                {
                    MessageBox.Show("Puerto no esta abierto", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                ReadWriteIO.comm.DiscardInBuffer();
                ReadWriteIO.comm.DiscardOutBuffer();
                ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0,
                    len + CMD.FRAME_HEADEND_LEN - (0 == ReaderParams.ProtocolFlag ? 0 : 2));
            }
            else
            {
                if (!ReaderParams.nsStream.CanRead)
                {
                    MessageBox.Show("Puerto de red no esta conectada", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                while (ReaderParams.nsStream.DataAvailable)
                    ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
            }

            timer1.Enabled = true;
            RevDataFrom232.Start();
        }

        private void DetenerLecturas()
        {
            MulStartTime = DateTime.Now;
            OutLineTime = DateTime.Now;

            if (ReaderParams.tcpClient != null && ReaderParams.tcpClient.Connected)
                timer7.Enabled = false;
            if (cB_Beep.Checked) { timer4.Enabled = true; beepflag = false; }

            threadFlag = 1;
            button_inv_mul.Text = "Multiple";

            if (playsound != null && playsound.IsAlive) playsound.Abort();

            System.Threading.Thread.Sleep(100);
            StopInvMul();

            button_singleInv.Enabled = true;
            menu(true);
            NETToolStripMenuItem.Enabled = false;
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

        private void StopInvMul()
        {
            UInt16 len = 0;
            byte[] buf = new byte[2];
            int recount = 50000;
            int revlen = 0;
            Byte[] revbuf = new Byte[5000];

            if (0 == ReaderParams.ProtocolFlag)
                ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_STOP_INVENTORY, len);
            else
                ReadWriteIO.sendFrameBuild(buf, 0x18, len);

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (!ReadWriteIO.comm.IsOpen)
                {
                    MessageBox.Show("No abre el puerto", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                ReadWriteIO.comm.DiscardInBuffer();
                ReadWriteIO.comm.DiscardOutBuffer();
                ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);

                while (revlen == 0 && recount != 0) { recount--; revlen = ReadWriteIO.comm.BytesToRead; }
                if (recount == 0) return;
                System.Threading.Thread.Sleep(10);
                ReadWriteIO.comm.Read(revbuf, 0, ReadWriteIO.comm.BytesToRead);
            }
            else
            {
                recount = ReaderParams.Netrecount;
                if (!ReaderParams.nsStream.CanRead)
                {
                    MessageBox.Show("No abre el puerto", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                while (ReaderParams.nsStream.DataAvailable)
                    ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);

                while (recount != 0 && !ReaderParams.nsStream.DataAvailable) recount--;
                if (recount == 0) return;
                System.Threading.Thread.Sleep(100);
                revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            }

            // Validar respuesta de stop
            if (0 == ReaderParams.ProtocolFlag)
            {
                if (!(revbuf[0] == CMD.FRAME_HEAD_FIRST && revbuf[1] == CMD.FRAME_HEAD_SECOND &&
                      revbuf[2] == 0x00 && revbuf[3] == 0x09 &&
                      revbuf[4] == CMD.FRAME_CMD_STOP_INVENTORY_RSP && revbuf[5] == 0x01))
                    return;
            }
            else
            {
                if (!(revbuf[0] == 0xBB && revbuf[1] == 0x98 && revbuf[2] == 0x01))
                    return;
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // PROTOCOLO DE COMUNICACIÓN CON EL LECTOR RFID
        // ════════════════════════════════════════════════════════════════════

        private int delay(int time2delay, byte[] WriteBuf, byte CMD2, ushort len)
        {
            ReadWriteIO.sendFrameBuild(WriteBuf, CMD2, len);
            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (!ReadWriteIO.comm.IsOpen)
                {
                    MessageBox.Show("Do not open the port", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return 0;
                }
                ReadWriteIO.comm.DiscardInBuffer();
                ReadWriteIO.comm.DiscardOutBuffer();
                ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
            }
            else
            {
                if (!ReaderParams.nsStream.CanRead)
                {
                    MessageBox.Show("Network port is not connected", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return 0;
                }
                byte[] revbuf = new byte[500];
                while (ReaderParams.nsStream.DataAvailable)
                    ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
            }
            del d1 = new del(ReceiveDataFromUARTdel);
            IAsyncResult re1 = d1.BeginInvoke(time2delay, null, null);
            while (!re1.IsCompleted) { }
            return d1.EndInvoke(re1);
        }

        public delegate int del(int a);

        private int ReceiveDataFromUARTdel(int overtime)
        {
            byte[] buf = new byte[2];
            DateTime starttime = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - starttime).TotalMilliseconds > overtime) return 0;
                if (IsReceiveDatadel() && GetOneByteRxDatadel(buf))
                {
                    starttime = DateTime.Now;
                    PraseMFrameDatadel(buf[0]);
                }
                if (bGetDataComplete && bCheckRet)
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
            int recount = 50000, revlen = 0;
            if (1 == ReaderParams.CommIntSelectFlag)
            {
                while (revlen == 0 && recount != 0) { recount--; revlen = ReadWriteIO.comm.BytesToRead; }
                return recount != 0 || revlen != 0;
            }
            else
            {
                while (recount != 0 && !ReaderParams.nsStream.DataAvailable) recount--;
                return recount != 0;
            }
        }

        private bool GetOneByteRxDatadel(Byte[] ch)
        {
            byte[] tmpBuf = new byte[10];
            int tmpSize = (1 == ReaderParams.CommIntSelectFlag)
                ? ReadWriteIO.comm.Read(tmpBuf, 0, 1)
                : ReaderParams.nsStream.Read(tmpBuf, 0, 1);
            if (tmpSize == 1) { ch[0] = tmpBuf[0]; return true; }
            return false;
        }

        // Decodificador de tramas (protocolo RealID)
        void PraseMFrameDatadel(Byte ch) => PraseMFrameData(ch);

        // Decodificador de tramas protocolo RealID (lectura continua y comando)
        void PraseMFrameData(Byte ch)
        {
            if (u8HeadCnt < 5)
            {
                switch (u8HeadCnt)
                {
                    case 0: if (CMD.FRAME_HEAD_FIRST == ch) { g_Revbuf[0] = ch; u8HeadCnt++; } break;
                    case 1:
                        if (CMD.FRAME_HEAD_SECOND == ch) { g_Revbuf[1] = ch; u8HeadCnt++; }
                        else u8HeadCnt = 0;
                        checkbyte = 0; break;
                    case 2:
                        if (ch >= 0x01) { u8HeadCnt = 0; }
                        else { g_Revbuf[2] = ch; checkbyte ^= ch; g_RevDataLen = (UInt16)(ch << 8); u8HeadCnt++; }
                        break;
                    case 3: g_Revbuf[3] = ch; checkbyte ^= ch; g_RevDataLen += ch; u8HeadCnt++; break;
                    case 4: g_Revbuf[4] = ch; u8HeadCnt++; checkbyte ^= ch; u8DataPointer = 0; break;
                }
            }
            else if (u8DataPointer < (g_RevDataLen - CMD.FRAME_HEADEND_LEN))
            { g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch; checkbyte ^= ch; u8DataPointer++; }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN))
            {
                if (checkbyte == ch) { g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch; u8DataPointer++; }
                else { u8HeadCnt = 0; u8DataPointer = 0; }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 1))
            {
                if (CMD.FRAME_END_MRK_FIRST == ch) { g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch; u8DataPointer++; }
                else { u8HeadCnt = 0; u8DataPointer = 0; }
            }
            else if (u8DataPointer == (g_RevDataLen - CMD.FRAME_HEADEND_LEN + 2))
            {
                if (CMD.FRAME_END_MRK_SECOND == ch)
                {
                    g_Revbuf[CMD.FRAME_HEAD_LEN + u8DataPointer] = ch;
                    if (g_RevDataLen >= 0x08) { bCheckRet = true; bGetDataComplete = true; }
                }
                u8HeadCnt = 0; u8DataPointer = 0;
            }
            else { u8HeadCnt = 0; u8DataPointer = 0; }
        }

        // Decodificador de tramas protocolo SUM
        void PraseMFrameDataSUM(Byte ch)
        {
            if (u8HeadCnt < 3)
            {
                switch (u8HeadCnt)
                {
                    case 0:
                        if (0xBB == ch) { g_Revbuf[0] = ch; u8HeadCnt++; } else u8HeadCnt = 0;
                        checkbyte = 0; break;
                    case 1: g_Revbuf[1] = ch; u8HeadCnt++; checkbyte += ch; u8DataPointer = 0; break;
                    case 2: g_Revbuf[2] = ch; checkbyte += ch; g_RevDataLen = ch; u8HeadCnt++; break;
                }
            }
            else if (u8DataPointer < g_RevDataLen)
            { g_Revbuf[3 + u8DataPointer] = ch; checkbyte += ch; u8DataPointer++; }
            else if (u8DataPointer == g_RevDataLen)
            {
                if (checkbyte == ch) { g_Revbuf[3 + u8DataPointer] = ch; u8DataPointer++; }
                else { u8HeadCnt = 0; u8DataPointer = 0; }
            }
            else if (u8DataPointer == (g_RevDataLen + 1))
            {
                if (CMD.FRAME_END_MRK_FIRST == ch) { g_Revbuf[3 + u8DataPointer] = ch; u8DataPointer++; }
                else { u8HeadCnt = 0; u8DataPointer = 0; }
            }
            else if (u8DataPointer == (g_RevDataLen + 2))
            {
                if (CMD.FRAME_END_MRK_SECOND == ch)
                {
                    g_Revbuf[3 + u8DataPointer] = ch;
                    if (g_RevDataLen > 0x06) { bCheckRet = true; bGetDataComplete = true; }
                }
                u8HeadCnt = 0; u8DataPointer = 0;
            }
            else { u8HeadCnt = 0; u8DataPointer = 0; }
        }

        bool CheckCRC(byte[] p, UInt16 len)
        {
            byte crc = 0;
            for (UInt16 i = 2; i < (len - 3); i++) crc ^= p[i];
            return crc == p[len - 3];
        }

        bool CheckCRCSUM(byte[] p, UInt16 len)
        {
            byte crc = 0;
            for (UInt16 i = 1; i < (len - 3); i++) crc += p[i];
            return crc == p[len - 3];
        }

        // ════════════════════════════════════════════════════════════════════
        // HILOS DE RECEPCIÓN DE DATOS
        // ════════════════════════════════════════════════════════════════════

        private bool IsReceiveData()
        {
            int recount = 50000, revlen = 0;
            if (!RevDataFrom232.IsAlive) return false;

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                while (revlen == 0 && recount != 0)
                {
                    recount--;
                    if (!RevDataFrom232.IsAlive) return false;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }
                return recount != 0 || revlen != 0;
            }
            else
            {
                while (recount != 0 && !ReaderParams.nsStream.DataAvailable) recount--;
                return recount != 0;
            }
        }

        private bool GetOneByteRxData(Byte[] ch)
        {
            byte[] tmpBuf = new byte[10];
            int tmpSize = (1 == ReaderParams.CommIntSelectFlag)
                ? ReadWriteIO.comm.Read(tmpBuf, 0, 1)
                : ReaderParams.nsStream.Read(tmpBuf, 0, 1);
            if (tmpSize == 1) { ch[0] = tmpBuf[0]; return true; }
            return false;
        }

        private void ReceiveDataFromUART()
        {
            byte[] buf = new byte[2];
            if (!RevDataFrom232.IsAlive) return;
            while (true)
            {
                if (!RevDataFrom232.IsAlive) return;
                if (IsReceiveData() && GetOneByteRxData(buf))
                    PraseMFrameData(buf[0]);
                Handle_Uart_Command();
            }
        }

        private void ReceiveDataFromUARTSUM()
        {
            byte[] buf = new byte[2];
            if (!RevDataFrom232.IsAlive) return;
            while (true)
            {
                if (!RevDataFrom232.IsAlive) return;
                if (IsReceiveData() && GetOneByteRxData(buf))
                    PraseMFrameDataSUM(buf[0]);
                Handle_Uart_CommandSUM();
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // PARSEO DE TRAMAS RFID
        // ════════════════════════════════════════════════════════════════════

        Byte Handle_Uart_Command()
        {
            if (!bGetDataComplete || !bCheckRet) return 0;
            Byte retval = 0;
            netovertime = DateTime.Now;
            if (g_Revbuf[4] == CMD.FRAME_CMD_INVENTORY_MUL_RSP)
            {
                if (g_Revbuf[3] == 8)
                {
                    this.BeginInvoke(new TimeInvoke(ShowTime));
                    bCheckRet = false; bGetDataComplete = false;
                    return 1;
                }
                if (ParseMulReadFrameDataProcess()) retval = 1;
            }
            bCheckRet = false; bGetDataComplete = false;
            return retval;
        }

        Byte Handle_Uart_CommandSUM()
        {
            if (!bGetDataComplete || !bCheckRet) return 0;
            Byte retval = 0;
            if (g_Revbuf[1] == 0x97 && ParseMulReadFrameDataProcessSUM()) retval = 1;
            bCheckRet = false; bGetDataComplete = false;
            return retval;
        }

        bool ParseMulReadFrameDataProcess()
        {
            string epc_tmp = "", tid_tmp = "";
            byte[] byte_epc = new byte[64], byte_tid = new byte[64];
            Int16 rssi; int antid;
            DateTime nowTime = DateTime.Now;
            TimeSpan MulT = nowTime - MulStartTime;
            TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);

            int length = (g_Revbuf[5] >> 3) * 2;
            int rlength = (g_Revbuf[2] << 8) + g_Revbuf[3];

            System.Array.Copy(g_Revbuf, 7, byte_epc, 0, length);
            for (int i = 0; i < length; i++)
                epc_tmp += byte_epc[i].ToString("X2") + (i < length - 1 ? "-" : "");

            if (rlength > (length + 13))
            {
                System.Array.Copy(g_Revbuf, 7 + length, byte_tid, 0, rlength - length - 13);
                int tidLen = rlength - length - 13;
                for (int i = 0; i < tidLen; i++)
                    tid_tmp += byte_tid[i].ToString("X2") + (i < tidLen - 1 ? "-" : "");
            }

            rssi = (Int16)((g_Revbuf[rlength - 6] << 8) + g_Revbuf[rlength - 5]);
            antid = g_Revbuf[rlength - 4];
            tmp.epcid = epc_tmp;
            tmp.tid = tid_tmp;
            tmp.rxrssi = (Int16)(rssi / 10);
            tmp.readcnt = 1;
            tmp.antID = antid;
            tmp.rptime = MulT.ToString();
            if (tmp.rptime.Length <= 11) tmp.rptime += ".000";
            tmp.rptime = tmp.rptime.Substring(0, 12);
            tmp.moduloid = ReaderParams.ModuloId;
            tmp.modulorol = ReaderParams.ModuloRol;

            AddTagToBuf(tmp);
            this.BeginInvoke(new BeepInvoke(beeping));
            return true;
        }

        bool ParseMulReadFrameDataProcessSUM()
        {
            string epc_tmp = "", tid_tmp = "";
            byte[] byte_epc = new byte[64], byte_tid = new byte[64];
            Int16 rssi; int antid;
            DateTime nowTime = DateTime.Now;
            TimeSpan MulTSum = nowTime - MulStartTime;
            TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime);

            int length = (g_Revbuf[3] >> 3) * 2;
            int rlength = g_Revbuf[2] + 6;

            System.Array.Copy(g_Revbuf, 5, byte_epc, 0, length);
            for (int i = 0; i < length; i++)
                epc_tmp += byte_epc[i].ToString("X2") + (i < length - 1 ? "-" : "");

            if (rlength > (length + 11))
            {
                int tidLen = rlength - length - 11;
                System.Array.Copy(g_Revbuf, 7 + length, byte_tid, 0, tidLen);
                for (int i = 0; i < tidLen; i++)
                    tid_tmp += byte_tid[i].ToString("X2") + (i < tidLen - 1 ? "-" : "");
            }

            rssi = (Int16)((g_Revbuf[rlength - 6] << 8) + g_Revbuf[rlength - 5]);
            antid = g_Revbuf[rlength - 4];
            tmp.epcid = epc_tmp;
            tmp.tid = tid_tmp;
            tmp.rxrssi = (Int16)(rssi / 10);
            tmp.readcnt = 1;
            tmp.antID = antid;
            tmp.rptime = MulTSum.ToString();

            AddTagToBuf(tmp);
            return true;
        }

        // Lectura individual (botón "Inv individual")
        private void button_singleInv_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            byte[] buf = new byte[2];
            DateTime clk_time = DateTime.Now;

            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                buf[0] = (byte)((ReaderParams.InvTimeOut >> 8) & 0xff);
                buf[1] = (byte)(ReaderParams.InvTimeOut & 0xff);
                result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_INVENTORY_SINGLE, len);
            }
            else
                result = delay(CMD.TIMEOUT, buf, 0x16, len);

            if (result == 0)
            {
                MessageBox.Show("over time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Byte[] revbuf = g_Revbuf;
            if (0 == ReaderParams.ProtocolFlag)
            {
                string epc_tmp = "", tid_tmp = "";
                byte[] byte_epc = new byte[64], byte_tid = new byte[64];
                DateTime nowTime = DateTime.Now;
                TimeSpan t = nowTime - clk_time;
                TagInfo tmp = new TagInfo("", 0, 0, 1, nowTime, "White");

                int length = (revbuf[5] >> 3) * 2;
                int rlength = (revbuf[2] << 8) + revbuf[3];

                System.Array.Copy(revbuf, 7, byte_epc, 0, length);
                for (int i = 0; i < length; i++)
                    epc_tmp += byte_epc[i].ToString("X2") + (i < length - 1 ? "-" : "");

                if (rlength > length + 13)
                {
                    int tidLen = rlength - length - 13;
                    System.Array.Copy(revbuf, 7 + length, byte_tid, 0, tidLen);
                    for (int i = 0; i < tidLen && i <= 11; i++)
                        tid_tmp += byte_tid[i].ToString("X2") + (i < tidLen - 1 && i < 11 ? "-" : "");
                }

                Int16 rssi = (Int16)((revbuf[rlength - 6] << 8) + revbuf[rlength - 5]);
                tmp.epcid = epc_tmp;
                tmp.tid = tid_tmp;
                tmp.rxrssi = (Int16)(rssi / 10);
                tmp.readcnt = 1;
                tmp.antID = revbuf[rlength - 4];
                tmp.rptime = t.ToString();
                tmp.color = "white";
                tmp.moduloid = ReaderParams.ModuloId;
                tmp.modulorol = ReaderParams.ModuloRol;

                AddTagToBuf(tmp);
                label_NumOfTags.Text = tagnum.ToString();
                ActualizarPaginacion();
                PageShow(tagnum);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        // BUFFER DE TAGS Y LÓGICA DE NEGOCIO
        // ════════════════════════════════════════════════════════════════════

        void AddTagToBuf(TagInfo tag)
        {
            string keystr = tag.epcid + "-" + tag.tid;
            int vlue = 0;

            if (m_Tags.ContainsKey(keystr))
            {
                if (m_Tags[keystr].tid.Equals(tag.tid))
                {
                    // Tag ya conocido — actualizar contadores
                    m_Tags[keystr].readcnt++;
                    m_Tags[keystr].rxrssi = tag.rxrssi;
                    m_Tags[keystr].antID = tag.antID;
                    m_Tags[keystr].times = tag.times;
                    m_Tags[keystr].tid = tag.tid;
                    m_Tags[keystr].moduloid = tag.moduloid;
                    m_Tags[keystr].modulorol = tag.modulorol;

                    m_IndTag.TryGetValue(keystr, out vlue);
                    string vk = vlue.ToString();
                    m_SortTag[vk].readcnt = m_Tags[keystr].readcnt;
                    m_SortTag[vk].rxrssi = tag.rxrssi;
                    m_SortTag[vk].antID = tag.antID;
                    m_SortTag[vk].times = tag.times;
                    m_SortTag[vk].tid = tag.tid;
                    m_SortTag[vk].moduloid = tag.moduloid;
                    m_SortTag[vk].modulorol = tag.modulorol;
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
                // Tag nuevo — clasificar PALLET vs LPN, guardar en SQL, enviar HTTP
                tagnum++;
                m_Tags.Add(keystr, tag);
                m_IndTag.Add(keystr, tagnum);
                string sortKey = tagnum.ToString();

                try
                {
                    var repo = new ReadRepository();
                    string tipo = repo.ObtenerTipoTag(tag.epcid, tag.tid);
                    tag.color = (tipo == "PALLET") ? Color.Cyan.Name : Color.LightGreen.Name;
                }
                catch { tag.color = Color.LightGreen.Name; }

                m_SortTag.Add(sortKey, tag);

                // Auto-avanzar la vista a la última página al agregar un tag nuevo
                _currentPage = (int)Math.Ceiling((double)tagnum / PageSize);
                if (_currentPage < 1) _currentPage = 1;

                // Guardar en SQL + HTTP, una sola vez por tag nuevo.
                // BeginInvoke porque AddTagToBuf se llama desde el hilo lector.
                int idxGuardar = tagnum;
                this.BeginInvoke(new Action(() =>
                {
                    string k = idxGuardar.ToString();
                    if (!m_SortTag.ContainsKey(k)) return;
                    TagInfo t = m_SortTag[k];

                    ListViewItem itemGuardar = new ListViewItem(k);
                    itemGuardar.SubItems.Add(t.epcid);
                    itemGuardar.SubItems.Add(t.tid);
                    itemGuardar.SubItems.Add(t.readcnt.ToString());
                    itemGuardar.SubItems.Add(t.rxrssi.ToString());
                    itemGuardar.SubItems.Add(t.antID.ToString());
                    itemGuardar.SubItems.Add(t.times.ToString());
                    itemGuardar.SubItems.Add(t.rptime);
                    itemGuardar.SubItems.Add(t.color);
                    itemGuardar.SubItems.Add(t.moduloid.ToString());
                    itemGuardar.SubItems.Add(t.modulorol);

                    Guardarlectura(itemGuardar);
                    PageShow(tagnum);
                }));
            }
        }

        void beeping()
        {
            if (cB_Beep.Checked) { beeptime1 = DateTime.Now; beepflag = true; }
        }

        void ShowTime()
        {
            label8.Text = (DateTime.Now - MulStartTime).ToString();
        }

        // ════════════════════════════════════════════════════════════════════
        // P1: PAGINACIÓN VIRTUAL DEL LISTVIEW
        // ════════════════════════════════════════════════════════════════════

        // Actualiza solo los labels de página sin redibujar el ListView.
        // Llamado por timer6_Tick cada segundo.
        private void ActualizarPaginacion()
        {
            int total = m_SortTag.Count;
            int totalPages = (total == 0) ? 1 : (int)Math.Ceiling((double)total / PageSize);
            lb_count.Text = totalPages.ToString();
            lb_current.Text = _currentPage.ToString();
            tb_P2J.Text = _currentPage.ToString();
        }

        // Renderiza en el ListView solo los PageSize ítems de la página _currentPage.
        // No hace Clear() — actualiza filas existentes para evitar parpadeo.
        public void PageShow(int num)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => PageShow(num)));
                return;
            }
            RecalcularPageSize(); // actualizar según tamaño actual del ListView

            int total = m_SortTag.Count;
            int totalPages = (total == 0) ? 1 : (int)Math.Ceiling((double)total / PageSize);

            if (_currentPage < 1) _currentPage = 1;
            if (_currentPage > totalPages) _currentPage = totalPages;

            lb_current.Text = _currentPage.ToString();
            lb_count.Text = totalPages.ToString();
            tb_P2J.Text = _currentPage.ToString();

            int idxStart = (_currentPage - 1) * PageSize + 1;
            int idxEnd = Math.Min(idxStart + PageSize - 1, total);
            int rowCount = Math.Max(0, idxEnd - idxStart + 1);

            listView_Disp.BeginUpdate();

            for (int i = 0; i < rowCount; i++)
            {
                string key = (idxStart + i).ToString();
                if (!m_SortTag.ContainsKey(key)) continue;
                TagInfo t = m_SortTag[key];

                if (i < listView_Disp.Items.Count)
                {
                    // Fila ya existe — solo actualizar valores
                    ListViewItem item = listView_Disp.Items[i];
                    item.Text = key;
                    item.SubItems[1].Text = t.epcid;
                    item.SubItems[2].Text = t.tid;
                    item.SubItems[3].Text = t.readcnt.ToString();
                    item.SubItems[4].Text = t.rxrssi.ToString();
                    item.SubItems[5].Text = t.antID.ToString();
                    item.SubItems[6].Text = t.times.ToString();
                    item.SubItems[7].Text = t.rptime;
                    item.SubItems[8].Text = t.color;
                    item.SubItems[9].Text = t.moduloid.ToString();
                    item.SubItems[10].Text = t.modulorol;
                    try { item.BackColor = Color.FromName(t.color); } catch { }
                }
                else
                {
                    // Fila nueva
                    ListViewItem item = new ListViewItem(key);
                    item.SubItems.Add(t.epcid);
                    item.SubItems.Add(t.tid);
                    item.SubItems.Add(t.readcnt.ToString());
                    item.SubItems.Add(t.rxrssi.ToString());
                    item.SubItems.Add(t.antID.ToString());
                    item.SubItems.Add(t.times.ToString());
                    item.SubItems.Add(t.rptime);
                    item.SubItems.Add(t.color);
                    item.SubItems.Add(t.moduloid.ToString());
                    item.SubItems.Add(t.modulorol);
                    try { item.BackColor = Color.FromName(t.color); } catch { }
                    listView_Disp.Items.Add(item);
                }
            }

            // Eliminar filas sobrantes si la página tiene menos de PageSize ítems
            while (listView_Disp.Items.Count > rowCount)
                listView_Disp.Items.RemoveAt(listView_Disp.Items.Count - 1);

            listView_Disp.EndUpdate();
        }

        // Botones de navegación de páginas
        private void bt_FPage_Click(object sender, EventArgs e)
        { _currentPage = 1; PageShow(tagnum); }

        private void button6_Click(object sender, EventArgs e)
        { if (_currentPage > 1) { _currentPage--; PageShow(tagnum); } }

        private void button9_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)m_SortTag.Count / PageSize);
            if (_currentPage < Math.Max(1, totalPages)) { _currentPage++; PageShow(tagnum); }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _currentPage = Math.Max(1, (int)Math.Ceiling((double)m_SortTag.Count / PageSize));
            PageShow(tagnum);
        }

        private void bt_J2_Click(object sender, EventArgs e)
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)m_SortTag.Count / PageSize));
            if (int.TryParse(tb_P2J.Text, out int dest) && dest >= 1 && dest <= totalPages)
            { _currentPage = dest; PageShow(tagnum); }
        }

        // ════════════════════════════════════════════════════════════════════
        // SPRINT 7 / T-C#: HTTP + SINCRONIZACIÓN OFFLINE
        // ════════════════════════════════════════════════════════════════════

        // HttpClient estático: una sola instancia por proceso
        private static readonly HttpClient _httpClient = new HttpClient
        { Timeout = TimeSpan.FromSeconds(8) };

        // SemaphoreSlim(1,1): mutex async-compatible, no bloquea el ThreadPool
        private readonly System.Threading.SemaphoreSlim _syncLock =
            new System.Threading.SemaphoreSlim(1, 1);

        // P3: contador de fallos consecutivos para backoff exponencial
        private int _syncFallosConsecutivos = 0;
        private DateTime _syncProximoIntento = DateTime.MinValue;

        private System.Timers.Timer _syncTimer;

        /// <summary>
        /// Inicializa el timer de sincronización HTTP y el evento de cambio de red.
        /// Llamar una sola vez desde Form1_Load.
        /// </summary>
        private void InicializarSincronizacion()
        {
            int intervaloMs = 60000;
            if (int.TryParse(ConfigurationManager.AppSettings["SyncIntervalMs"], out int cfg))
                intervaloMs = cfg;

            _syncTimer = new System.Timers.Timer(intervaloMs) { AutoReset = true };
            _syncTimer.Elapsed += (s, ev) =>
            {
                log.Debug("[SYNC] Timer disparado — revisando pendientes.");
                SincronizarPendientesAsync();
            };
            _syncTimer.Start();

            NetworkChange.NetworkAvailabilityChanged += (s, ev) =>
            {
                if (ev.IsAvailable)
                {
                    log.Info("[SYNC] Red disponible — sincronizando pendientes.");
                    _syncFallosConsecutivos = 0; // resetear backoff al recuperar red
                    SincronizarPendientesAsync();
                }
            };

            log.InfoFormat("[SYNC] Sincronización iniciada — intervalo {0} ms.", intervaloMs);
        }

        /// <summary>
        /// Envía un tag al servidor HTTP.
        /// Retorna false sin lanzar excepción — el reintento lo hace SincronizarPendientesAsync.
        /// </summary>
        private async Task<bool> EnviarTagHttpAsync(ReadTag tag)
        {
            string url = ConfigurationManager.AppSettings["RfidServerUrl"]
                         ?? "http://38.253.180.55/api/v1/read-tags";
            try
            {
                string json = SerializarTag(tag);
                log.DebugFormat("[HTTP] Enviando EPC {0}", tag.EPC);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    log.DebugFormat("[HTTP] OK — EPC {0}", tag.EPC);
                    return true;
                }
                log.WarnFormat("[HTTP] {0} {1} — EPC {2}",
                    (int)response.StatusCode, response.ReasonPhrase, tag.EPC);
                return false;
            }
            catch (TaskCanceledException)
            { log.WarnFormat("[HTTP] Timeout — EPC {0}", tag.EPC); return false; }
            catch (HttpRequestException ex)
            { log.WarnFormat("[HTTP] Sin conexión — EPC {0}: {1}", tag.EPC, ex.Message); return false; }
            catch (Exception ex)
            { log.ErrorFormat("[HTTP] Error inesperado — EPC {0}: {1}", tag.EPC, ex.Message); return false; }
        }

        /// <summary>
        /// P3: Sincroniza pendientes con backoff exponencial.
        /// Si hay 3 o más fallos consecutivos espera hasta 5 minutos antes de reintentar.
        /// </summary>
        private void SincronizarPendientesAsync()
        {
            if (DateTime.Now < _syncProximoIntento)
            {
                log.DebugFormat("[SYNC] Backoff activo — próximo intento en {0:hh\\:mm\\:ss}.",
                    _syncProximoIntento - DateTime.Now);
                return;
            }

            Task.Run(async () =>
            {
                if (!await _syncLock.WaitAsync(0).ConfigureAwait(false))
                {
                    log.Debug("[SYNC] Ya en curso — omitiendo.");
                    return;
                }
                try
                {
                    var repo = new ReadRepository();
                    List<ReadTag> pendientes = repo.GetPendientesHttp();
                    if (pendientes.Count == 0) { log.Debug("[SYNC] Sin pendientes."); return; }

                    log.InfoFormat("[SYNC] {0} pendiente(s) a sincronizar.", pendientes.Count);
                    int enviados = 0;
                    foreach (ReadTag tag in pendientes)
                    {
                        bool ok = await EnviarTagHttpAsync(tag).ConfigureAwait(false);
                        if (ok)
                        {
                            repo.MarcarEnviadoHttp(tag.Id);
                            enviados++;
                        }
                        else
                        {
                            log.WarnFormat("[SYNC] Abortando — error en EPC {0}.", tag.EPC);
                            break;
                        }
                    }
                    log.InfoFormat("[SYNC] {0}/{1} enviados.", enviados, pendientes.Count);

                    // P3: Backoff exponencial si ninguno fue enviado
                    if (enviados == 0)
                    {
                        _syncFallosConsecutivos++;
                        if (_syncFallosConsecutivos >= 3)
                        {
                            int minEspera = Math.Min(5, _syncFallosConsecutivos - 2);
                            _syncProximoIntento = DateTime.Now.AddMinutes(minEspera);
                            log.WarnFormat(
                                "[SYNC] {0} fallos consecutivos — próximo intento en {1} min.",
                                _syncFallosConsecutivos, minEspera);
                        }
                    }
                    else
                    {
                        _syncFallosConsecutivos = 0;
                        _syncProximoIntento = DateTime.MinValue;
                    }
                }
                catch (Exception ex) { log.Error("[SYNC] Error en sincronización.", ex); }
                finally { _syncLock.Release(); }
            });
        }

        private static string SerializarTag(ReadTag tag)
        {
            var payload = new
            {
                epc = tag.EPC.Replace("-", ""),
                tag = tag.TAG,
                tid = tag.TID.Replace("-", ""),
                rssi = tag.RSSI,
                antId = tag.AntID,
                moduloId = tag.ModuloId,
                moduloRol = tag.ModuloRol,
                color = tag.Color,
                lastTime = tag.LastTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                firstUpdate = tag.FirstReadTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            return new JavaScriptSerializer().Serialize(payload);
        }

        // ════════════════════════════════════════════════════════════════════
        // GUARDADO LOCAL (SQL) Y HTTP FIRE-AND-FORGET
        // ════════════════════════════════════════════════════════════════════

        // Sprint 7: SQL local siempre; HTTP fire-and-forget
        private AsignacionTag Guardarlectura(ListViewItem item)
        {
            var tag = new ReadTag(
                item.SubItems[0].Text,   // Tag #
                item.SubItems[1].Text,   // EPC
                item.SubItems[2].Text,   // TID
                int.Parse(item.SubItems[3].Text),  // InvTimes
                int.Parse(item.SubItems[4].Text),  // RSSI
                int.Parse(item.SubItems[5].Text),  // AntID
                DateTime.Parse(item.SubItems[6].Text),  // LastTime
                DateTime.Parse(item.SubItems[7].Text),  // FirstReadTime
                item.SubItems[8].Text,   // Color
                int.Parse(item.SubItems[9].Text),  // ModuloId
                item.SubItems[10].Text); // ModuloRol

            var repo = new ReadRepository();
            var result = repo.AddReadTag(tag);

            tag.Id = result.Idlectura;
            Task.Run(async () =>
            {
                bool ok = await EnviarTagHttpAsync(tag).ConfigureAwait(false);
                if (ok) repo.MarcarEnviadoHttp(tag.Id);
            });

            return result;
        }

        private bool ValidarPago(int id)
        {
            return new ReadRepository().GetReadInBox(id);
        }

        private void GuardarColor(string epc, string ant, string color)
        {
            new ReadRepository().UpdateReadTag(epc, ant, color);
        }

        // ════════════════════════════════════════════════════════════════════
        // TIMERS
        // ════════════════════════════════════════════════════════════════════

        int LastTotalNumOfTags = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int totalNumOfTags = 0;
            for (int i = 0; i < tagnum; i++)
                totalNumOfTags += m_SortTag[(i + 1).ToString()].readcnt;

            double speed = totalNumOfTags - LastTotalNumOfTags + RemovedTagNums;
            LastTotalNumOfTags = totalNumOfTags;
            RemovedTagNums = 0;

            label_speed.Text = speed + (0 == ReaderParams.LanguageFlag ? "个/秒" : " tags/s..");
            lB_times.Text = ((int)(DateTime.Now - StartTime).TotalSeconds)
                                  + (0 == ReaderParams.LanguageFlag ? "秒" : " s");
            lb_totaltimes.Text = totalNumOfTags.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Ciclo automático de lectura si hay ítems (actualmente inactivo)
            if (listView_Disp.Items.Count > 0)
            {
                button_inv_mul.Enabled = false;
                multiread();
                System.Threading.Thread.Sleep(500);
                button_inv_mul.Enabled = true;
                button_inv_mul.Enabled = false;
                multiread();
                System.Threading.Thread.Sleep(1000);
                button_inv_mul.Enabled = true;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            beeptime2 = DateTime.Now;
            if ((beeptime2 - beeptime1).TotalMilliseconds < 1000)
            { if (beepflag) { player.PlaySync(); beeptime1 = DateTime.Now; beepflag = false; } }
            else
            { if (beepflag) { beepflag = false; player.PlaySync(); } }
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
                flag = flag == 0 ? 1 : 0;
                time1 = DateTime.Now;
                button_inv_mul_Click(sender, e);
            }
        }

        // timer6: actualiza el contador de tags y el label de página (1 seg).
        // NO llama PageShow — evita parpadeo. PageShow solo se llama cuando llega un tag nuevo.
        private void timer6_Tick(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            { this.Invoke(new Action(() => timer6_Tick(sender, e))); return; }

            label_NumOfTags.Text = tagnum.ToString();
            PageShow(tagnum);
        }

        // P2: timer7 — reconexión TCP al lector RFID.
        // Intervalo configurado en Designer a 10000ms (10 seg).
        // Sin MessageBox — no bloquea el UI thread.
        private void timer7_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - netovertime).TotalSeconds <= 10) return;

            netovertime = DateTime.Now;
            ReaderParams.tcpClient.Close();
            RevDataFrom232.Abort();
            ReaderParams.CommIntSelectFlag = 0;
            ReaderParams.tcpClient = new TcpClient();

            string str = cbB_COMID.Text.Substring(3);
            IPAddress ipA = IPAddress.Parse(str);

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    IAsyncResult ar = ReaderParams.tcpClient.BeginConnect(
                        ipA, ReaderParams.ProtocoloTCPIP, null, null);
                    bool success = ar.AsyncWaitHandle.WaitOne(1000);
                    if (!success)
                        throw new Exception("Timeout de conexión al lector RFID");

                    RevDataFrom232 = (2 == ReaderParams.ProtocolFlag)
                        ? new Thread(new ThreadStart(ReceiveDataFromUART))
                        : new Thread(new ThreadStart(ReceiveDataFromUARTSUM));
                    ReaderParams.nsStream = ReaderParams.tcpClient.GetStream();
                    netovertime = DateTime.Now;
                    RevDataFrom232.Start();
                    log.InfoFormat("[TCP] Reconexión exitosa a {0}", str);
                    return;
                }
                catch (Exception ex)
                {
                    log.WarnFormat("[TCP] Intento {0}/3 fallido: {1}", i + 1, ex.Message);
                }
            }

            // 3 intentos fallidos — loguear y dejar que el timer reintente
            log.WarnFormat("[TCP] Reconexión fallida a {0} — se reintentará en {1}s.",
                str, timer7.Interval / 1000);
        }

        private void timPIO_Tick(object sender, EventArgs e)
        {
            ActivarGPIO("3", false);
            estadoAlarmaActivada = false;
            timPIO.Enabled = false;
            timPIO.Stop();
            multiread();
            estadoAlarmaActivada = false;
        }

        private void tmr_Limpiar_TipoC_Tick(object sender, EventArgs e)
        {
            button_clr.Enabled = true;
        }

        // ════════════════════════════════════════════════════════════════════
        // BOTONES Y CONTROLES DE UI
        // ════════════════════════════════════════════════════════════════════

        private void button_clr_Click(object sender, EventArgs e) => limpiar();

        private void limpiar()
        {
            m_Tags.Clear();
            m_IndTag.Clear();
            m_SortTag.Clear();
            tagnum = 0;
            _currentPage = 1;
            lb_current.Text = "0";
            lb_count.Text = "0";
            listView_Disp.Items.Clear();
            label_speed.Text = "";
            label_NumOfTags.Text = "";
            lB_times.Text = "";
            LastTotalNumOfTags = 0;
            lb_totaltimes.Text = "";
        }

        private void StopClear_Click(object sender, EventArgs e) => detenerLectura_Clear();

        private void detenerLectura_Clear()
        {
            System.Threading.Thread.Sleep(1000);
            listView_Disp.Items.Clear();
            label_speed.Text = "";
            label_NumOfTags.Text = "";
            lB_times.Text = "";
            LastTotalNumOfTags = 0;
            lb_totaltimes.Text = "";
            System.Threading.Thread.Sleep(1000);
            ActivarLecturas();
        }

        private void btnChangeVisibility_Click(object sender, EventArgs e)
        {
            controles(!BasicParaSet.Visible);
            btnChangeVisibility.Text = BasicParaSet.Visible ? "Menú Visible: SI" : "Menú Visible: NO";
        }

        private void listView_Disp_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending
                    ? SortOrder.Descending : SortOrder.Ascending;
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            this.listView_Disp.Sort();
        }

        private void listView_Disp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((button_inv_mul.Text == "连续寻卡" || button_inv_mul.Text == "Multiple")
                && (btn_OPEN_CLOSE.Text == "关闭" || btn_OPEN_CLOSE.Text == "Close"))
            {
                foreach (ListViewItem tempItem in this.listView_Disp.SelectedItems)
                    ReaderParams.select_TagID = tempItem.SubItems[1].Text;
                TagOperate tagOpe = new TagOperate();
                tagOpe.ShowDialog();
                ReaderParams.NonFilterInSelect();
            }
        }

        private void cB_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReaderParams.LanguageFlag = (UInt16)cB_Language.SelectedIndex;
            if (2 == ReaderParams.LanguageFlag)
            {
                label1.Text = "Port:"; label5.Text = "Baud:"; label2.Text = "Numero:";
                label3.Text = "Velocidad:"; label4.Text = "Hora:"; label6.Text = "Total Veces:";
                btn_OPEN_CLOSE.Text = "Abrir"; button_singleInv.Text = "Inv individual";
                button_inv_mul.Text = "Multiple"; button_export.Text = "Exportar";
                button_clr.Text = "Limpiar"; cB_OutLineClear.Text = "Limpiar Off-line";
                cB_FastID.Text = "FastID"; cB_TagFocus.Text = "TagFocus";
                BasicParaSet.Text = "Ajustes básicos"; AdvanceParaSet.Text = "Ajustes avanzados";
                TagOperate.Text = "Operar Tag"; RegOperate.Text = "Regs";
                OtherSet.Text = "Otros ajustes"; 天线设置ToolStripMenuItem.Text = "ANT SET";
                NETToolStripMenuItem.Text = "Configuración de Ethernet";
                AboutusSet.Text = "Sobre nosotros";
                groupBox1.Text = "WorkSpace:"; groupBox2.Text = "Port:"; groupBox3.Text = "Information:";
                columnHeader1.Text = "Tag"; columnHeader2.Text = "EPC"; columnHeader3.Text = "Inv Times";
                columnHeader4.Text = "RSSI(dBm)"; columnHeader5.Text = "ANT ID";
                columnHeader6.Text = "Last Time"; columnHeader7.Text = "TID";
                columnHeader8.Text = "First Read time Cost(ms)";
                cB_Beep.Text = "Beep"; bt_FPage.Text = "First"; button6.Text = "PREV";
                button9.Text = "NEXT"; button8.Text = "Last"; bt_J2.Text = "JUMP";
                label8.Text = "Duration:";
            }
            label_speed.Location = new Point(label3.Location.X + label3.Size.Width + 4, 38);
            lB_times.Location = new Point(label4.Location.X + label4.Size.Width + 4, 38);
            label_NumOfTags.Location = new Point(label2.Location.X + label2.Size.Width + 4, 79);
            lb_totaltimes.Location = new Point(label6.Location.X + label6.Size.Width + 4, 79);
        }

        private void cB_FastID_CheckedChanged(object sender, EventArgs e)
        {
            EnviarComandoCheckbox(
                cB_FastID.Checked,
                CMD.FRAME_CMD_SET_FASTID,
                CMD.FRAME_CMD_SET_FASTID_RSP,
                "Falló la configuracion de FastID");
        }

        private void cB_TagFocus_CheckedChanged(object sender, EventArgs e)
        {
            EnviarComandoCheckbox(
                cB_TagFocus.Checked,
                CMD.FRAME_CMD_SET_TAGFOCUS,
                CMD.FRAME_CMD_SET_TAGFOCUS_RSP,
                "Falló configuracion del TagFocus");
        }

        // Helper compartido para FastID y TagFocus (misma estructura de trama)
        private void EnviarComandoCheckbox(bool estado, byte cmdSet, byte cmdRsp, string msgError)
        {
            UInt16 len = 2;
            Byte[] buf = new Byte[100];
            buf[0] = estado ? (byte)0x01 : (byte)0x00;
            buf[1] = 0x00;
            ReadWriteIO.sendFrameBuild(buf, cmdSet, len);

            int recount = 50000, revlen = 0;
            Byte[] revbuf = new Byte[500];

            if (1 == ReaderParams.CommIntSelectFlag)
            {
                if (!ReadWriteIO.comm.IsOpen)
                { MessageBox.Show("No abre el puerto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
                ReadWriteIO.comm.DiscardInBuffer();
                ReadWriteIO.comm.DiscardOutBuffer();
                ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
                while (revlen < 9 && recount != 0) { recount--; revlen = ReadWriteIO.comm.BytesToRead; }
                if (recount == 0) return;
                ReadWriteIO.comm.Read(revbuf, 0, ReadWriteIO.comm.BytesToRead);
            }
            else
            {
                recount = ReaderParams.Netrecount;
                if (!ReaderParams.nsStream.CanRead)
                { MessageBox.Show("Puerto de red no conectado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
                while (ReaderParams.nsStream.DataAvailable) ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
                while (recount != 0 && !ReaderParams.nsStream.DataAvailable) recount--;
                if (recount == 0) return;
                System.Threading.Thread.Sleep(100);
                ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            }

            if (!(revbuf[0] == CMD.FRAME_HEAD_FIRST && revbuf[1] == CMD.FRAME_HEAD_SECOND &&
                  revbuf[2] == 0x00 && revbuf[3] == 0x09 && revbuf[4] == cmdRsp && revbuf[5] == 0x01))
                MessageBox.Show(msgError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void cbB_COMID_MouseClick(object sender, MouseEventArgs e)
        {
            int oldcur = cbB_COMID.SelectionStart;
            int oldinx = cbB_COMID.SelectedIndex;
            cbB_COMID.SelectedIndex = cbB_COMID.Items.Count > 0 ? 0 : -1;
            cbB_COMID.SelectedIndex = oldinx > cbB_COMID.Items.Count - 1
                ? cbB_COMID.Items.Count - 1 : oldinx;
            cbB_COMID.SelectionStart = oldcur;
        }

        private void cbB_COMID_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbB_Baud.Enabled = cbB_COMID.SelectedIndex != 0;
        }

        private void button_export_Click(object sender, EventArgs e) => ExportToExcel();

        public void ExportToExcel()
        {
            string folder = ReaderParams.FolderCSV;
            string fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv";
            SaveCSV(this.listView_Disp, folder + fileName);
        }

        public void SaveCSV(ListView listView, string fullPath)
        {
            if (listView.Items.Count < 1) return;
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists) fi.Directory.Create();

            using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var sb = new StringBuilder();
                // Cabecera
                sb.AppendLine(string.Join(",",
                    Enumerable.Range(0, listView.Columns.Count)
                               .Select(i => listView.Columns[i].Text)));
                // Filas
                foreach (ListViewItem item in listView.Items)
                    sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                        item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text,
                        item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text,
                        item.SubItems[6].Text, item.SubItems[7].Text, item.SubItems[8].Text));
                sw.WriteLine(sb.ToString());
            }
            ActivarGPIO("3", true);
        }

        // ════════════════════════════════════════════════════════════════════
        // GPIO Y ALARMA
        // ════════════════════════════════════════════════════════════════════

        private void ActivarAlarma(int intervalMiliSeg)
        {
            if (estadoAlarmaActivada) return;
            estadoAlarmaActivada = true;
            timPIO.Interval = intervalMiliSeg;
            timPIO.Enabled = true;
            timPIO.Start();
            ActivarGPIO("3", true);
            multiread();
        }

        private void ActivarGPIO(string gpioNum, bool estado)
        {
            byte mask = 0, data = 0;
            switch (gpioNum)
            {
                case "1": mask = 0x01; data = 0x01; break;
                case "2": mask = 0x02; data = 0x02; break;
                case "3": mask = 0x04; data = 0x04; break;
                case "4": mask = 0x08; data = 0x08; break;
            }
            if (!estado) data = 0x00;
            SendSetGPIOStatus(mask, data);
        }

        private int SendSetGPIOStatus(byte mask, byte data)
        {
            UInt16 len = 2;
            Byte[] buf = new Byte[100];
            buf[0] = mask; buf[1] = data;
            delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_SET_GPIO, len);
            Byte[] revbuf = g_Revbuf;
            return (revbuf[0] == CMD.FRAME_HEAD_FIRST && revbuf[1] == CMD.FRAME_HEAD_SECOND &&
                    revbuf[2] == 0x00 && revbuf[3] == 0x09 &&
                    revbuf[4] == CMD.FRAME_CMD_SET_GPIO_RSP && revbuf[5] == 0x01) ? 0 : -2;
        }

        // ════════════════════════════════════════════════════════════════════
        // MENÚS Y EVENTOS DEL FORMULARIO
        // ════════════════════════════════════════════════════════════════════

        private void ParaSet_Click(object sender, EventArgs e) => new BasicParaSet().ShowDialog();
        private void 标签操作ToolStripMenuItem_Click(object sender, EventArgs e) => new AdvanceParaSet().ShowDialog();
        private void 空口协议设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReaderParams.select_TagID = "";
            new TagOperate().ShowDialog();
            ReaderParams.NonFilterInSelect();
        }
        private void 辅助信息ToolStripMenuItem_Click(object sender, EventArgs e) => new RegOperate().ShowDialog();
        private void AboutusSet_Click(object sender, EventArgs e) => new Aboutus().ShowDialog();
        private void Cbut_Test_Click(object sender, EventArgs e) => new Test().ShowDialog();
        private void 网口模块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNET net = new FormNET();
            net.Text = (0 == ReaderParams.LanguageFlag) ? "网口设置" : "Ethernet module Setting";
            net.ShowDialog();
        }
        private void 天线设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Test frm = new Test();
            frm.Text = (0 == ReaderParams.LanguageFlag) ? "天线设置" : "ANT Setting";
            frm.ShowDialog();
        }
        private void 在线下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OtherSet oset = new OtherSet();
            if (oset.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                columnHeader7.Text = (oset.Epctype == "usr") ? "USR" : "TID";
            }
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            天线设置ToolStripMenuItem.Visible = !天线设置ToolStripMenuItem.Visible;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Cb_Test.Checked)
            {
                Test frm = new Test();
                if (frm.ShowDialog() == DialogResult.OK) timer3.Enabled = true;
            }
            else timer3.Enabled = false;
        }
        private void cB_protocoltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReaderParams.ProtocolFlag = (UInt16)cB_protocoltype.SelectedIndex;
        }
        private void Mul_Key_Sum(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
                e.Handled = true;
        }

        // Menú oculto: configuración de placa de pruebas
        private void 测试板切换分路功能ToolStripMenuItem_Click(object sender, EventArgs e)
        { label9.Visible = true; comboBox1.Visible = true; button1.Visible = true; }
        private void 读卡统计ToolStripMenuItem_Click(object sender, EventArgs e) { button2.Visible = true; }
        private void 双间隙读取测试ToolStripMenuItem_Click(object sender, EventArgs e)
        { button3.Visible = true; textBox3.Visible = true; label10.Visible = true; }

        // button1: configuración de splitter en placa de pruebas (solo COM)
        private void button1_Click(object sender, EventArgs e)
        {
            Byte[] buf = new Byte[100];
            buf[2] = (byte)((comboBox1.SelectedIndex == 2 || comboBox1.SelectedIndex == 3) ? (buf[2] | 0x20) : (buf[2] & 0xDF));
            buf[2] = (byte)((comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 3) ? (buf[2] | 0x10) : (buf[2] & 0xEF));
            UInt16 len = 3;
            ReadWriteIO.sendFrameBuild(buf, 0xE6, len);
            if (!ReadWriteIO.comm.IsOpen)
            { MessageBox.Show("Puerto no abierto", "ERROR", MessageBoxButtons.OK); return; }
            ReadWriteIO.comm.DiscardInBuffer();
            ReadWriteIO.comm.DiscardOutBuffer();
            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, len + CMD.FRAME_HEADEND_LEN);
            int recount = 500000, revlen = 0;
            while (revlen < 9 && recount != 0) { recount--; revlen = ReadWriteIO.comm.BytesToRead; }
            if (recount == 0) { MessageBox.Show("Sin respuesta", "ERROR", MessageBoxButtons.OK); return; }
            Byte[] revbuf = new Byte[500];
            ReadWriteIO.comm.Read(revbuf, 0, ReadWriteIO.comm.BytesToRead);
            MessageBox.Show(
                (revbuf[0] == CMD.FRAME_HEAD_FIRST && revbuf[4] == 0xE7 && revbuf[5] == 0x01)
                    ? "Configurado correctamente" : "Falló la configuración",
                revbuf[5] == 0x01 ? "OK" : "ERROR", MessageBoxButtons.OK);
        }

        // button2: estadísticas de tiempo de lectura por rango de segundos
        private void button2_Click(object sender, EventArgs e)
        {
            int[] rangos = { 1, 2, 3, 4, 5, 10, 20, 30, 40, 60 };
            var sb = new StringBuilder();
            for (int i = 0; i < rangos.Length; i++)
            {
                int j = 0;
                for (long ii = 0; ii < tagnum; ii++)
                {
                    double k = double.Parse(m_SortTag[(ii + 1).ToString()].rptime.Substring(6, 6));
                    if (k <= rangos[i]) j++;
                }
                sb.AppendFormat("Primeros {0}s: {1} tags\r\n", rangos[i], j);
            }
            MessageBox.Show(sb.ToString(), "Estadísticas de lectura", MessageBoxButtons.OK);
        }

        DateTime time1;
        DateTime OutLineTime;

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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string str = cbB_COMID.Text.Substring(0, 3);
            if (btn_OPEN_CLOSE.Text == "关闭" || btn_OPEN_CLOSE.Text == "Close")
            {
                if (threadFlag == 1 && RevDataFrom232.IsAlive)
                    RevDataFrom232.Abort();
                StopInvMul();
                if ("NET" == str) ReaderParams.tcpClient.Close();
                else ReadWriteIO.comm.Close();
            }
        }
    }


    // ════════════════════════════════════════════════════════════════════════
    // ORDENADOR DE COLUMNAS DEL LISTVIEW
    // ════════════════════════════════════════════════════════════════════════

    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort = 0;
        private SortOrder OrderOfSort = SortOrder.None;
        private CaseInsensitiveComparer ObjectCompare = new CaseInsensitiveComparer();

        public int Compare(object x, object y)
        {
            int result = ObjectCompare.Compare(
                ((ListViewItem)x).SubItems[ColumnToSort].Text,
                ((ListViewItem)y).SubItems[ColumnToSort].Text);

            if (OrderOfSort == SortOrder.Ascending) return result;
            if (OrderOfSort == SortOrder.Descending) return -result;
            return 0;
        }

        public int SortColumn { get => ColumnToSort; set => ColumnToSort = value; }
        public SortOrder Order { get => OrderOfSort; set => OrderOfSort = value; }
    }
}

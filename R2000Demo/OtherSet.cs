using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace R2000Demo
{
    public partial class OtherSet : Form
    {
        private string epctype = string.Empty;
        public string Epctype
        {
            get { return epctype; }
            set { epctype = value; }
        }
        public OtherSet()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            if (0 == ReaderParams.LanguageFlag)
            {
                Text                        = "其他设置";
                groupBox1.Text              = "波特率设置";
                bt_BaudSet.Text             = "设置";
                bt_SetDual.Text             = "设置";
                bt_SetNetPara.Text          = "设置";
                bt_SetEpcTidTogether.Text   = "设置";
                bt_GetDual.Text             = "获取";
                bt_GetRSSI.Text             = "获取";
                bt_GetEpcTidTogether.Text   = "获取";
                groupBox2.Text              = "软件复位:";
                bt_softreset.Text           = "软件复位";
                bt_FactorySet.Text          = "恢复设置";
                groupBox3.Text              = "Dual和Single模式设置:";
                groupBox4.Text              = "获取环境RSSI值:";
                groupBox5.Text              = "网口参数设置:";
                groupBox6.Text = "数据上报模式设置:";
                groupBox7.Text              = "恢复出厂设置:";
                label3.Text                 = "工作方式:";
                label4.Text                 = "目标IP:";
                label5.Text                 = "目标端口:";
                label6.Text                 = "本机IP:";
                label7.Text                 = "本机网关:";
                label8.Text                 = "本机端口:";
                cB_DualSaveFlag.Text        = "保存";
                cB_epctidSaveFlag.Text      = "保存";
                label11.Text                = "注意: 网口参数设置只针对带有网口通信的阅读器";
                groupBox8.Text              = "盘点模式设置与获取";
                string[] array={"模式1：多标签模式"  ,
                                "模式2：快速读取模式",
                                "模式3",
                                "模式4",
                                "模式5",
                                "模式6",
                                "模式7",
                                "模式8",
                                "模式9"
                               };
                cB_pandian.DataSource =array;
                cB_pandianSaveflag.Text = "保存";
                bt_Getpandian.Text = "获取";
                Bt_Setpandian.Text = "设置";


            }
            else
            {
                Text                        = "Other Setting";
                groupBox1.Text              = "Baud rate setting";
                bt_BaudSet.Text             = "set";
                bt_SetDual.Text             = "set";
                bt_SetNetPara.Text          = "set";
                bt_SetEpcTidTogether.Text   = "set";
                bt_GetDual.Text             = "get";
                bt_GetRSSI.Text             = "get";
                bt_GetEpcTidTogether.Text   = "get";
                groupBox2.Text              = "software reset:";
                bt_softreset.Text           = "reset";
                bt_FactorySet.Text          = "restore";
                groupBox3.Text              = "Dual and Single mode setting:";
                groupBox4.Text              = "Get background RSSI:";
                groupBox5.Text              = "Net Para Setting:";
                groupBox6.Text              = "The Data Update mode Setting:";
                groupBox7.Text              = "Return to factory Setting:";
                label3.Text                 = "Operate Mode:";
                label4.Text                 = "Dest IPaddr:";
                label5.Text                 = "Dest Port:";
                label6.Text                 = "Local IPaddr:";
                label7.Text                 = "Local Getway:";
                label8.Text                 = "Local Port:";
                cB_DualSaveFlag.Text        = "Saved";
                cB_epctidSaveFlag.Text      = "Saved";
                label11.Text                = "Note: The Net Para Setting only for the reader with the network";
                groupBox8.Text = "Inventory Mode Setting";
                string[] array ={"Mode1：MultiTag Mode"  ,
                                "Mode2：Fast Read Mode",
                                "Mode3",
                                "Mode4",
                                "Mode5",
                                "Mode6",
                                "Mode7",
                                "Mode8",
                                "Mode9"
                               };
                cB_pandian.DataSource = array;
                cB_pandianSaveflag.Text = "Save";
                bt_Getpandian.Text = "Get";
                Bt_Setpandian.Text = "Set";
            }

            cbB_Baud.SelectedIndex          = 4;
            cbB_DualAndSing.SelectedIndex   = 0;
            cbB_EPCTIDTOG.SelectedIndex     = 0;
            cB_NetWork.SelectedIndex        = 3;
            label1.Text                     = "";
            label2.Text                     = "";
            label10.Text                    = "";
            label12.Text                    = "";
            label13.Text                    = "";
            label14.Text                    = "";
            cB_pandian.SelectedIndex = 0;
            //epctype = "usr";
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
            del d1 = new del(ReceiveDataFromUART);
            IAsyncResult re1 = d1.BeginInvoke(time2delay, null, null);
            while (!re1.IsCompleted) { }
            int result = d1.EndInvoke(re1);
            return result;
        }
        public delegate int del(int a);
        private int ReceiveDataFromUART(int overtime)
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
                if (IsReceiveData())
                {
                    if (GetOneByteRxData(buf))					// 从接收数据中取出一个字节
                    {
                        starttime = DateTime.Now;
                        PraseMFrameData(buf[0]);
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
        private bool IsReceiveData()
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
        Byte u8HeadCnt;
        Byte u8DataPointer;
        Byte checkbyte;
        Byte[] g_Revbuf = new Byte[1024];               //接收缓存
        UInt16 g_RevDataLen;                            //接收帧数据长度
        bool bCheckRet;
        bool bGetDataComplete;
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
        private void bt_BaudSet_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label2.Text = "";
            WriteBuf[0] = (byte)cbB_Baud.SelectedIndex;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_BAUD, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_BAUD, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label2.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label2.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label2.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label2.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_BAUD_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label2.Text = "设置失败";
                }
                else
                {
                    label2.Text = "Set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label2.Text = "设置成功";
            }
            else
            {
                label2.Text = "set OK";
            }
        }

        private void bt_softreset_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label1.Text = "";

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SOFTRESET, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SOFTRESET, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label1.Text = "复位失败";
            //        }
            //        else
            //        {
            //            label1.Text = "Reset failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label1.Text = "复位失败";
            //        }
            //        else
            //        {
            //            label1.Text = "Reset failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SOFTRESET_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label1.Text = "复位失败";
                }
                else
                {
                    label1.Text = "Reset failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label1.Text = "复位成功";
            }
            else
            {
                label1.Text = "Reset OK";
            }
        }

        private void bt_GetRSSI_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            tB_RSSI.Text = "";
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_RSSI, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_RSSI, len);

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 0x0B) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_RSSI.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_RSSI.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_RSSI.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_RSSI.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                && (revbuf[4] == CMD.FRAME_CMD_GET_RSSI_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_RSSI.Text = "获取失败";
                }
                else
                {
                    tB_RSSI.Text = "Get failed";
                }
                return;
            }

            Int16 rssi;
            rssi = (Int16)(revbuf[6]<<8);
            rssi += (Int16)(revbuf[7]);
            rssi = (Int16)(rssi / 10);
            tB_RSSI.Text = rssi.ToString();
        }

        private void bt_SetDual_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label10.Text = "";
            if (true == cB_DualSaveFlag.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }
            WriteBuf[1] = (byte)cbB_DualAndSing.SelectedIndex;
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_DUAL, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_DUAL, len);

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label10.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label10.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label10.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label10.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_DUAL_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label10.Text = "设置失败";
                }
                else
                {
                    label10.Text = "Set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label10.Text = "设置成功";
            }
            else
            {
                label10.Text = "set OK";
            }
        }

        private void bt_GetDual_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label10.Text = "";

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_DUAL, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_DUAL, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 0x0A) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label10.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label10.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label10.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label10.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0A)
                && (revbuf[4] == CMD.FRAME_CMD_GET_DUAL_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label10.Text = "获取失败";
                }
                else
                {
                    label10.Text = "Get failed";
                }
                return;
            }

            if (0x01 == revbuf[6])
            {
                cbB_DualAndSing.SelectedIndex = 0x01;
            }
            else if (0x00 == revbuf[6])
            {
                cbB_DualAndSing.SelectedIndex = 0x00;
            }
            else
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label10.Text = "获取失败";
                }
                else
                {
                    label10.Text = "Get failed";
                }           
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label10.Text = "获取成功";
            }
            else
            {
                label10.Text = "Get OK";
            }
        }

        private void bt_SetEpcTidTogether_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label12.Text = "";
            if (true == cB_epctidSaveFlag.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }
            WriteBuf[1] = (byte)cbB_EPCTIDTOG.SelectedIndex;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_EPCTIDTOGETHER, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_EPCTIDTOGETHER, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label12.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label12.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label12.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label12.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_EPCTIDTOGETHER_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label12.Text = "设置失败";
                }
                else
                {
                    label12.Text = "Set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label12.Text = "设置成功";               
            }
            else
            {
                label12.Text = "set OK";
            }
            switch (cbB_EPCTIDTOG.SelectedIndex)
            {
                case 0:
                    epctype = "epc";
                    break;
                case 1:
                    epctype = "tid";
                    break;
                case 2:
                    epctype = "usr";
                    break;
                default:
                    break;
            }
            
        }

        private void bt_GetEpcTidTogether_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label12.Text = "";

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_EPCTIDTOGETHER, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_EPCTIDTOGETHER, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 0x0A) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label12.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label12.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label12.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label12.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0A)
                && (revbuf[4] == CMD.FRAME_CMD_GET_EPCTIDTOGETHER_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label12.Text = "获取失败";
                }
                else
                {
                    label12.Text = "Get failed";
                }
                return;
            }

            if (0x01 == revbuf[6])
            {
                cbB_EPCTIDTOG.SelectedIndex = 0x01;
            }
            else if (0x00 == revbuf[6])
            {
                cbB_EPCTIDTOG.SelectedIndex = 0x00;
            }
            else if (0x02 == revbuf[6])
            {
                cbB_EPCTIDTOG.SelectedIndex = 0x02;
            }
            else
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label12.Text = "获取失败";
                }
                else
                {
                    label12.Text = "Get failed";
                }           
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label12.Text = "获取成功";
            }
            else
            {
                label12.Text = "Get OK";
            }
        }

        private void bt_FactorySet_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label13.Text = "";
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_RESTORY_FACTORY, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_RESTORY_FACTORY, len);

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label13.Text = "set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount     = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label13.Text = "set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_RESTORY_FACTORY_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label13.Text = "设置失败";
                }
                else
                {
                    label13.Text = "set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label13.Text = "设置成功";
            }
            else
            {
                label13.Text = "set OK";
            }
        }

        private void bt_Getpandian_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label14.Text = "";

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_WORK_PATTERN, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_WORK_PATTERN, len);
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
            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 0x0A) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label14.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label14.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label14.Text = "获取失败";
            //        }
            //        else
            //        {
            //            label14.Text = "Get failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0A)
                && (revbuf[4] == CMD.FRAME_CMD_GET_WORK_PATTERN_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label14.Text = "获取失败";
                }
                else
                {
                    label14.Text = "Get failed";
                }
                return;
            }

            if ((0x01 <= revbuf[6]) && (0x09 >= revbuf[6]))
            {
                cB_pandian.SelectedIndex = revbuf[6];
            }
            else
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label14.Text = "获取失败";
                }
                else
                {
                    label14.Text = "Get failed";
                }
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label14.Text = "获取成功";
            }
            else
            {
                label14.Text = "Get OK";
            }
        }

        private void Bt_Setpandian_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[20];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            label14.Text = "";
            if (true == cB_pandianSaveflag.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }
            WriteBuf[1] = (byte)cB_pandian.SelectedIndex;
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_WORK_PATTERN, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_WORK_PATTERN , len);

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }

            //    while ((revlen < 9) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label14.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label14.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //    }
            //}
            //else
            //{
            //    recount = ReaderParams.Netrecount;
            //    if (true == ReaderParams.nsStream.CanRead)
            //    {
            //        while (true == ReaderParams.nsStream.DataAvailable)
            //        {
            //            ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        }
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            label14.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label14.Text = "Set failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_WORK_PATTERN_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label14.Text = "设置失败";
                }
                else
                {
                    label14.Text = "Set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label14.Text = "设置成功";
            }
            else
            {
                label14.Text = "set OK";
            }
        }


        private void OtherSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel =false;
        }
    }
}

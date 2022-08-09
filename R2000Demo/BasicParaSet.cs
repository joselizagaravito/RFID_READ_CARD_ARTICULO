using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Math;

namespace R2000Demo
{
    public partial class BasicParaSet : Form
    {
        string SoftWareVersion = "V2.1.25.8";
        float[] FrequencyTable = new float[50];

        public BasicParaSet()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            cbB_TxPower.SelectedIndex = 25;
            cbB_SetRegion.SelectedIndex = 1;
            cB_SaveFlag.Checked = true;
            tB_FreqNum.Text = "0";
            cbB_RxPower.SelectedIndex = 25;
            cbB_PowerAnt.SelectedIndex = 0;
            tB_Frequency.Text = "922.375";
            comboBox_recommand.SelectedIndex = ReaderParams.Recommand;


            /* 获取温度保护状态 */
            byte[] status = new byte[1];
            if (ReaderParams.GetTempProtect(status) == 0)
            {
                if (status[0] == 0x00)
                {
                    cB_TempProtect.Checked = false;
                }
                else
                {
                    cB_TempProtect.Checked = true;
                }
            }

            tB_Ant1WorkTime.Text = "300";
            tB_Ant2WorkTime.Text = "300";
            tB_Ant3WorkTime.Text = "300";
            tB_Ant4WorkTime.Text = "300";
            tB_Intervaltime.Text = "0";

            lB_GetGpioDisp.Text = "";
            lb_setIntTime.Text = "";
            label13.Text = "";
            label26.Text = "";
            if (0 == ReaderParams.LanguageFlag)
            {
                Text = "基本设置";

                groupBox5.Text = "功率";
                groupBox6.Text = "区域设置";
                groupBox7.Text = "跳频设置";
                groupBox10.Text = "版本信息";
                groupBox3.Text = "温度";
                groupBox1.Text = "链路设置";
                groupBox4.Text = "温度保护";
                groupBox8.Text = "天线设置";
                groupBox9.Text = "连续寻卡工作及等待时间设置(只适用单端口)";
                groupBox12.Text = "天线工作时间设置（单位为毫秒）。工作时间最小10ms，最大65535ms，推荐300ms-1500ms(只是用四端口)";
                groupBox13.Text = "多天线工作间隔时间设置（单位为毫秒）。间隔时间最小为0ms，最大为65535ms(只适用四端口)";
                groupBox14.Text = "蜂鸣器";
                groupBox11.Text = "GPIO（GPIO1~4为输出，GPIO5~8为输入）";
                groupBox16.Text = "BOD功能查询";
                cB_TxPowerSaveFlag.Text = "保存";
                cB_SaveFlag.Text = "保存";
                cB_MulantSaveFlag.Text = "保存";
                cB_IntSaveFlag.Text = "保存";
                cB_recommandSaveFlag.Text = "保存";
                cB_TempProtect.Text = "温度保护";

                label8.Text = "读功率：";
                label24.Text = "写功率：";
                label9.Text = "区域设置：";
                label10.Text = "频点个数：";
                label11.Text = "跳频频点：";
                label16.Text = "软件版本：";
                label17.Text = "硬件版本：";
                label18.Text = "固件版本：";
                label1.Text = "模块 ID ：";
                label2.Text = "温度：";
                label19.Text = "推荐组合：";
                label4.Text = "天线设置：";
                label5.Text = "工作时间：";
                label21.Text = "间隔时间:";

                bt_SetTxPower.Text = "设置";
                bt_GetTxPower.Text = "获取";
                bt_SetRegion.Text = "设置";
                bt_GetRegion.Text = "获取";
                bt_AddFreq.Text = "添加";
                bt_SetFreqTable.Text = "设置";
                bt_FreqClean.Text = "默认值";
                bt_GetFreqTable.Text = "获取";
                bt_GetVersion.Text = "获取";
                bt_GetTemp.Text = "获取";
                bt_SetRecom.Text = "设置";
                bt_GetRecomm.Text = "获取";
                bt_AntSet.Text = "设置";
                bt_AntGet.Text = "获取";
                bt_MulInvDeyTimeSet.Text = "设置";
                bt_MulInvDeyTimeGet.Text = "获取";
                bt_AntWorkTimeSet.Text = "设置";
                bt_AntWorkTimeGet.Text = "获取";
                bt_AntIntTimeSet.Text = "设置";
                bt_AntIntTimeGet.Text = "获取";
                bt_Buzzer.Text = "嘀嗒";
                bt_Clear.Text = "清除";
                bt_exit.Text = "退出";
                bt_GetGpioStatus.Text = "获取";
                button1.Text = "高级设置";
                label25.Text = "天线：";
                cB_selffreqSaveFlag.Text = "保存";
                button4.Text = "获取";
                groupBox15.Text = "GPIO触发连续寻标签";
                cBGPIOSAVE.Text = "保存";
                button2.Text = "设置";
                button3.Text = "获取";
            }
            else
            {
                Text = "Basic Settings";

                groupBox5.Text = "Power";
                groupBox6.Text = "Region Set";
                groupBox7.Text = "FHSS";
                groupBox10.Text = "Version Info";
                groupBox3.Text = "Temperature";
                groupBox1.Text = "Link Set";
                groupBox4.Text = "Temperature protect";
                groupBox8.Text = "Ant Set";
                groupBox9.Text = "The Work and Delay Time(Only for 1 Port)";
                groupBox12.Text = "Ant work time（ms）.the range is 10ms to 65535ms.(Only for 4 Ports Reader)";
                groupBox13.Text = "Interval time（ms）.the range is 0ms to 65535ms.(Only for 4 Ports Reader)";
                groupBox14.Text = "Buzzer";
                groupBox11.Text = "GPIO（GPIO1~4 is output，GPIO5~8 is input）";
                groupBox16.Text = "BOD Setting";
                cB_TxPowerSaveFlag.Text = "Saved";
                cB_SaveFlag.Text = "Saved";
                cB_MulantSaveFlag.Text = "Saved";
                cB_IntSaveFlag.Text = "Saved";
                cB_recommandSaveFlag.Text = "Saved";
                cB_WkDeTimeSaveFlag.Text = "Saved";
                cB_antSaveFlag.Text = "Saved";
                cB_TempProtect.Text = "Temp Protect";

                label8.Text = "Read Power：";
                label24.Text = "Write Power：";
                label9.Text = "Region：";
                label10.Text = "num：";
                label11.Text = "Freq：";
                label16.Text = "Software：";
                label17.Text = "Hardware：";
                label18.Text = "Firmware：";
                label1.Text = "ModuleID ：";
                label2.Text = "Temp：";
                label19.Text = "Commend：";
                label4.Text = "Ant Set";
                label5.Text = "work time：";
                label22.Text = "delay time:";
                label21.Text = "Interval time:";

                bt_SetTxPower.Text = "Set";
                bt_GetTxPower.Text = "Get";
                bt_SetRegion.Text = "Set";
                bt_GetRegion.Text = "Get";
                bt_AddFreq.Text = "Add";
                bt_SetFreqTable.Text = "Set";
                bt_FreqClean.Text = "Default";
                bt_GetFreqTable.Text = "Get";
                bt_GetVersion.Text = "Get";
                bt_GetTemp.Text = "Get";
                bt_SetRecom.Text = "Set";
                bt_GetRecomm.Text = "Get";
                bt_AntSet.Text = "Set";
                bt_AntGet.Text = "Get";
                bt_MulInvDeyTimeSet.Text = "Set";
                bt_MulInvDeyTimeGet.Text = "Get";
                bt_AntWorkTimeSet.Text = "Set";
                bt_AntWorkTimeGet.Text = "Get";
                bt_AntIntTimeSet.Text = "Set";
                bt_AntIntTimeGet.Text = "Get";
                bt_Buzzer.Text = "Ring";
                bt_Clear.Text = "Clean";
                bt_exit.Text = "Exit";
                bt_GetGpioStatus.Text = "Get";
                button1.Text = "Pro Setting";
                label25.Text = "ANT：";
                cB_selffreqSaveFlag.Text = "Saved";
                button4.Text = "Get";
                groupBox15.Text = "GPIO to trigger Multiple inv";
                cBGPIOSAVE.Text = "Saved";
                button2.Text = "Set";
                button3.Text = "Get";
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
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//Enviar información de prueba
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
            //while (!re1.IsCompleted) { }
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
        Byte[] g_Revbuf = new Byte[1024];               //Recibir búfer
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

        private void bt_SetRecom_Click(object sender, EventArgs e)
        {
            UInt16 len = 3;
            byte[] txpower = new byte[2];
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = 0x00;
            WriteBuf[1] = 0x00;
            WriteBuf[2] = (Byte)comboBox_recommand.SelectedIndex;

            if (cB_recommandSaveFlag.Checked == true)
            {
                WriteBuf[1] = 0x01;
            }
            else
            {
                WriteBuf[1] = 0x00;
            }

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_RF_LINK, len);

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

            //}
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_RF_LINK, len);
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
            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_RF_LINK_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("推荐链路组合设置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int Get_HardWare_Version()
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_HARDWARE_ID, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x0A, len);
            //}

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
            //        }
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_Hardware.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_Hardware.Text = "Get Failed";
            //        }

            //        return -1;
            //    }


            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        while ((revlen < 0x0B) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }               
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_Hardware.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_Hardware.Text = "Get Failed";
            //        }
            //        return -1;
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
            //        return -1;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        return -1;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    } 
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_HARDWARE_ID, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x0A, len);
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
                return -1;
            }
            revbuf = g_Revbuf;

            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_HARDWARE_ID_RSP)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Hardware.Text = "获取失败";
                    }
                    else
                    {
                        tB_Hardware.Text = "Get Failed";
                    }
                    return -1;
                }
                tB_Hardware.Text = revbuf[5].ToString();
                tB_Hardware.Text += ".".ToString();
                tB_Hardware.Text += revbuf[6].ToString();
                tB_Hardware.Text += revbuf[7].ToString();
            }
            else
            {
                if (!((revbuf[0] == 0xBB)
                && (revbuf[1] == 0x8A)
                && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Hardware.Text = "获取失败";
                    }
                    else
                    {
                        tB_Hardware.Text = "Get Failed";
                    }
                    return -1;
                }
                tB_Hardware.Text = revbuf[3].ToString();
                tB_Hardware.Text += ".".ToString();
                tB_Hardware.Text += revbuf[4].ToString();
                tB_Hardware.Text += revbuf[5].ToString();
            }

            return 0;
        }

        private int Get_FirmWare_Version()
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            //if (ReaderParams.ProtocolFlag == 0)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_FIRMWARE_ID, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x0B, len);
            //}

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;

            //        if (ReaderParams.ProtocolFlag == 0)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
            //        }
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_Firmware.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_Firmware.Text = "Get Failed";
            //        }

            //        return -1;
            //    }

            //    if (ReaderParams.ProtocolFlag == 0)
            //    {
            //        while ((revlen < 0x0B) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_Firmware.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_Firmware.Text = "Get Failed";
            //        }
            //        return -1;
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
            //        return -1;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        return -1;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_FIRMWARE_ID, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
                return -1;
            }
            revbuf = g_Revbuf;

            if (ReaderParams.ProtocolFlag == 0)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_FIRMWARE_ID_RSP)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Firmware.Text = "获取失败";
                    }
                    else
                    {
                        tB_Firmware.Text = "Get Failed";
                    }
                    return -1;
                }
                tB_Firmware.Text = revbuf[5].ToString();
                tB_Firmware.Text += ".".ToString();
                tB_Firmware.Text += revbuf[6].ToString();
                tB_Firmware.Text += revbuf[7].ToString();
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x8B)
                    && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Firmware.Text = "获取失败";
                    }
                    else
                    {
                        tB_Firmware.Text = "Get Failed";
                    }
                    return -1;
                }
                tB_Firmware.Text = revbuf[3].ToString();
                tB_Firmware.Text += ".".ToString();
                tB_Firmware.Text += revbuf[4].ToString();
                tB_Firmware.Text += revbuf[5].ToString();
            }
            return 0;
        }


        private int Get_Moduel_FirmWare_Version()
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            //if (ReaderParams.ProtocolFlag == 0)
            //{
            //    len = 1;
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_FIRMWARE_ID, len);
            //}
            //else
            //{
            //    len = 1;
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x0B, len);
            //}

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;

            //        if (ReaderParams.ProtocolFlag == 0)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
            //        }
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tb_MFirmV.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tb_MFirmV.Text = "Get Failed";
            //        }

            //        return -1;
            //    }

            //    if (ReaderParams.ProtocolFlag == 0)
            //    {
            //        while ((revlen < 0x0B) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tb_MFirmV.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tb_MFirmV.Text = "Get Failed";
            //        }
            //        return -1;
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
            //        return -1;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        return -1;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_FIRMWARE_ID, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
                return -1;
            }
            revbuf = g_Revbuf;

            if (ReaderParams.ProtocolFlag == 0)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_FIRMWARE_ID_RSP)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tb_MFirmV.Text = "获取失败";
                    }
                    else
                    {
                        tb_MFirmV.Text = "Get Failed";
                    }
                    return -1;
                }
                tb_MFirmV.Text = revbuf[5].ToString();
                tb_MFirmV.Text += ".".ToString();
                tb_MFirmV.Text += revbuf[6].ToString();
                tb_MFirmV.Text += revbuf[7].ToString();
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x8B)
                    && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tb_MFirmV.Text = "获取失败";
                    }
                    else
                    {
                        tb_MFirmV.Text = "Get Failed";
                    }
                    return -1;
                }
                tb_MFirmV.Text = revbuf[3].ToString();
                tb_MFirmV.Text += ".".ToString();
                tb_MFirmV.Text += revbuf[4].ToString();
                tb_MFirmV.Text += revbuf[5].ToString();
            }
            return 0;
        }

        private void bt_GetVersion_Click(object sender, EventArgs e)
        {
            int result = 0;

            tB_SoftWare.Text = "";
            tB_Hardware.Text = "";
            tB_Firmware.Text = "";
            tB_ModuleID.Text = "";
            tb_MFirmV.Text = "";
            groupBox10.Refresh();

            tB_SoftWare.Text = SoftWareVersion;

            result = Get_FirmWare_Version();
            if (-1 == result)
            {
                return;
            }

            result = Get_HardWare_Version();
            if (-1 == result)
            {
                return;
            }

            result = Get_Moduel_FirmWare_Version();
            if (-1 == result)
            {
                return;
            }


            UInt32[] data = new UInt32[1];
            ReaderParams.Read_Reg_Data((byte)1, 0x0000000B, data);
            tB_ModuleID.Text = data[0].ToString("D8");
            //           ReaderParams.Read_Reg_Data((byte)1, 0x0000000C, data);
            //           tB_ModuleID.Text += data[0].ToString("X8");
        }

        private void bt_GetTemp_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            byte[] buf = new byte[2];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[50];           //接收缓冲

            tB_Temperature.Text = "";
            groupBox3.Refresh();

            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_GET_TEMPERATURE, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(buf, 0x12, len); 
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_GET_TEMPERATURE, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, buf, 0x12, len);
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

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));      
            //        }

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

            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        while ((revlen < 0x0B) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }


            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_Temperature.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_Temperature.Text = "Get Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(10);
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
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x0b)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_TEMPERATURE_RSP)
                    && (revbuf[5] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Temperature.Text = "获取失败";
                    }
                    else
                    {
                        tB_Temperature.Text = "Get Failed";
                    }
                    return;
                }

                Int16 tmp = (Int16)(revbuf[6] * 256 + revbuf[7]);

                float temperature = tmp;
                temperature = (float)(temperature / 100.0);

                tB_Temperature.Text = temperature.ToString();
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x92)
                    && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_Temperature.Text = "获取失败";
                    }
                    else
                    {
                        tB_Temperature.Text = "Get Failed";
                    }
                    return;
                }

                Int16 tmp = (Int16)(revbuf[6 - 2] * 256 + revbuf[7 - 2]);

                float temperature = tmp;
                temperature = (float)(temperature / 100.0);

                tB_Temperature.Text = temperature.ToString();
            }

        }

        private void bt_SetTxPower_Click(object sender, EventArgs e)
        {
            UInt16 len = 6;
            byte[] txpower = new byte[2];
            byte[] rxpower = new byte[2];
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            //txpower[0] = (byte)(((cbB_TxPower.SelectedIndex + 5) * 100) >> 8);
            //txpower[1] = (byte)(((cbB_TxPower.SelectedIndex + 5) * 100) & 0xff);

            //WriteBuf[0] = 0x00; WriteBuf[1] = 0x00;
            //WriteBuf[2] = txpower[0]; WriteBuf[3] = txpower[1];
            //WriteBuf[4] = txpower[0]; WriteBuf[5] = txpower[1];
            if ((cbB_TxPower.Text == "") || (cbB_RxPower.Text == "") || (cbB_PowerAnt.Text == ""))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("输入信息有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("input info error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            //if (cB_TxPowerSaveFlag.Checked == true)
            //{
            //    WriteBuf[0] |= 0x02;
            //}
            //else
            //{
            //    WriteBuf[0] &= 0xFD;
            //}

            if (0 == ReaderParams.ProtocolFlag)
            {

                //txpower[0] = (byte)(((UInt32)(double.Parse(cbB_TxPower.Text)) * 100)  >> 8);
                //txpower[1] = (byte)((UInt32)(double.Parse(cbB_TxPower.Text) * 100) & 0x00ff);
                int a = (UInt16)(double.Parse(cbB_TxPower.Text) * 100) >> 8;
                int b = (UInt16)(double.Parse(cbB_TxPower.Text) * 100) & 0xff;
                txpower[0] = (byte)a;
                txpower[1] = (byte)b;
                //label8.Text  = a.ToString();
                int c = (UInt16)(double.Parse(cbB_RxPower.Text) * 100) >> 8;
                int d = (UInt16)(double.Parse(cbB_RxPower.Text) * 100) & 0xff;
                rxpower[0] = (byte)c;
                rxpower[1] = (byte)d;
                WriteBuf[0] = 0x00; WriteBuf[1] = 0x00;
                WriteBuf[2] = txpower[0]; WriteBuf[3] = txpower[1];
                WriteBuf[4] = rxpower[0]; WriteBuf[5] = rxpower[1];

                if (cB_TxPowerSaveFlag.Checked == true)
                {
                    WriteBuf[0] |= 0x02;
                }
                else
                {
                    WriteBuf[0] &= 0xFD;
                }
                WriteBuf[1] = (byte)(Int32.Parse(cbB_PowerAnt.Text));
                //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_TX_POWER, len);
            }
            else
            {
                if (cB_TxPowerSaveFlag.Checked == true)
                {
                    WriteBuf[0] |= 0x01;
                }
                else
                {
                    WriteBuf[0] &= 0xFE;
                }
                WriteBuf[1] = (byte)((UInt32)(float.Parse(cbB_TxPower.Text)));
                WriteBuf[2] = (byte)((UInt32)(float.Parse(cbB_RxPower.Text)));
                len = 3;

                //ReadWriteIO.sendFrameBuild(WriteBuf, 0x00, len);
            }


            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
            //        }

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

            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        while ((revlen < 9) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 7) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }


            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("未接收到数据");
            //        }
            //        else
            //        {
            //            MessageBox.Show("Set Failed");
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
            //        MessageBox.Show("未收到数据");
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    } 
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_TX_POWER, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x00, len);
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
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                    && (revbuf[4] == CMD.FRAME_CMD_SET_TX_POWER_RSP)
                    && (revbuf[5] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("功率设置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }
                else
                {
                    // MessageBox.Show("功率设置成功");
                }
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x80)
                    && (revbuf[2] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("功率设置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }
                else
                {
                    //  MessageBox.Show("功率设置成功");
                }
            }

        }

        private void bt_GetTxPower_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 800000;     //重试次数
            int revlen = 0;         //接收数据长度
            int lastlen = 0;
            Byte[] revbuf = new Byte[500];           //接收缓冲

            tB_TxPower.Text = "";
            groupBox5.Refresh();

            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_TX_POWER, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x0C, len);
            //}


            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));
            //        }

            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_TxPower.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_TxPower.Text = "Get failed";
            //        }

            //        return;
            //    }

            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        System.Threading.Thread.Sleep(20);
            //        while ((revlen < 0x30) && (recount != 0))
            //        //while ((recount != 0))
            //        {                  
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;

            //            if ((lastlen == revlen) && (lastlen > 4))
            //            {
            //                break;
            //            }

            //            lastlen = revlen;

            //            if(lastlen >0)
            //                System.Threading.Thread.Sleep(100);

            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }



            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("未接收到数据");
            //        }
            //        else
            //        {
            //            MessageBox.Show("Set Failed");
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
            //            MessageBox.Show("未接收到数据");
            //        }
            //        else
            //        {
            //            MessageBox.Show("Set Failed");
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_TX_POWER, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
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
                //if (revbuf[3] != revlen)
                //{
                //    if (0 == ReaderParams.LanguageFlag)
                //    {
                //        MessageBox.Show("未接收到数据");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Set Failed");
                //    }
                //}

                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_TX_POWER_RSP)
                    && (revbuf[5] == 0x00)
                    //&& (revbuf[6] == 0x00)
                    ))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_TxPower.Text = "获取失败";
                    }
                    else
                    {
                        tB_TxPower.Text = "Get failed";
                    }
                    return;
                }
                string str = "";
                int antnum = (revbuf[3] - 9) / 5;
                if (antnum > 0)
                {
                    for (int i = 0; i < antnum; i++)
                    {
                        if ((i == 0) && (revbuf[6 + 5 * i] == 0x00))
                        {
                            if (0 == ReaderParams.LanguageFlag)
                            {
                                str +=
                             "全局" + "\r\n" +
                             "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                             "\r\n";
                            }
                            else
                            {
                                str +=
                             //"全局" + "\r\n" +
                             "Read Power：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "Write Power：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                             "\r\n";
                            }
                            //str +=
                            // "全局" + "\r\n" +
                            // "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                            // "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                            // "\r\n";
                        }
                        else
                        {
                            if (0 == ReaderParams.LanguageFlag)
                            {
                                str +=
                             "anti:" + revbuf[6 + 5 * i] + "\t" +
                             "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            }
                            else
                            {
                                str +=
                             "anti:" + revbuf[6 + 5 * i] + "\t" +
                             "Read Power：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "Write Power：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            }
                            //str +=
                            // "anti:" + revbuf[6 + 5 * i] + "\t" +
                            // "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                            // "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            if (antnum < 30)
                            {
                                str += "\r\n" + "\r\n";
                            }
                            else
                            {
                                if (antnum < 50)
                                {
                                    str += "\r\n";
                                }
                                else
                                {
                                    if ((antnum / 4) == 0)
                                    {
                                        str += "\r\n";
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(str);
                //int power = revbuf[7] * 256 + revbuf[8];
                //power = power / 100;

                //tB_TxPower.Text = power.ToString();
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x8C)
                    && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_TxPower.Text = "获取失败";
                    }
                    else
                    {
                        tB_TxPower.Text = "Get failed";
                    }
                    return;
                }

                //int power = revbuf[4];
                //power = power / 100;

                //tB_TxPower.Text = power.ToString();
                string str = "";
                int antnum = (revbuf[3] - 9) / 5;
                if (antnum > 0)
                {
                    for (int i = 0; i < antnum; i++)
                    {
                        if ((i == 0) && (revbuf[6 + 5 * i] == 0x00))
                        {
                            if (0 == ReaderParams.LanguageFlag)
                            {
                                str +=
                             "全局" + "\r\n" +
                             "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                             "\r\n";
                            }
                            else
                            {
                                str +=
                             //"全局" + "\r\n" +
                             "Read Power：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "Write Power：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                             "\r\n";
                            }
                            //str +=
                            // "全局" + "\r\n" +
                            // "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                            // "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\r\n" +
                            // "\r\n";
                        }
                        else
                        {
                            if (0 == ReaderParams.LanguageFlag)
                            {
                                str +=
                             "anti:" + revbuf[6 + 5 * i] + "\t" +
                             "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            }
                            else
                            {
                                str +=
                             "anti:" + revbuf[6 + 5 * i] + "\t" +
                             "Read Power：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                             "Write Power：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            }
                            //str +=
                            // "anti:" + revbuf[6 + 5 * i] + "\t" +
                            // "读功率：" + ((float)((revbuf[7 + 5 * i] * 256 + revbuf[8 + 5 * i]) * 0.01)).ToString("f2") + " dBm" + "\t" +
                            // "写功率：" + ((float)((revbuf[9 + 5 * i] * 256 + revbuf[10 + 5 * i]) * 0.01)).ToString("f2") + " dBm";
                            if (antnum < 30)
                            {
                                str += "\r\n" + "\r\n";
                            }
                            else
                            {
                                if (antnum < 50)
                                {
                                    str += "\r\n";
                                }
                                else
                                {
                                    if ((antnum / 4) == 0)
                                    {
                                        str += "\r\n";
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show(str);
            }
        }

        private void bt_SetRegion_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (cB_SaveFlag.Checked == true)
            {
                WriteBuf[0] = 1;
            }
            else
            {
                WriteBuf[0] = 0;
            }


            if (0 == ReaderParams.ProtocolFlag)
            {
                switch (cbB_SetRegion.SelectedIndex)
                {
                    case 0:
                        WriteBuf[1] = 0x01;

                        break;
                    case 1:
                        WriteBuf[1] = 0x02;

                        break;
                    case 2:
                        WriteBuf[1] = 0x04;

                        break;
                    case 3:
                        WriteBuf[1] = 0x08;

                        break;
                    case 4:
                        WriteBuf[1] = 0x16;

                        break;
                    case 5:
                        WriteBuf[1] = 0x32;

                        break;
                    case 6:
                        WriteBuf[1] = 0x33;

                        break;
                    case 7:
                        WriteBuf[1] = 0x34;

                        break;
                    case 8:
                        WriteBuf[1] = 0x35;

                        break;
                    case 9:
                        WriteBuf[1] = 0x36;

                        break;
                    case 10:
                        WriteBuf[1] = 0x37;

                        break;
                    case 11:
                        WriteBuf[1] = 0x38;

                        break;
                    case 12:
                        WriteBuf[1] = 0x39;

                        break;
                    case 13:
                        WriteBuf[1] = 0x3A;
                        break;
                    case 14:
                        WriteBuf[1] = 0x3b;
                        break;
                    case 15:
                        WriteBuf[1] = 0x3c;
                        break;
                    default:
                        break;
                }
                //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_REGION, len);
            }
            else
            {
                switch (cbB_SetRegion.SelectedIndex)
                {
                    case 0:
                        WriteBuf[1] = 0x01;

                        break;
                    case 1:
                        WriteBuf[1] = 0x02;

                        break;
                    case 2:
                        WriteBuf[1] = 0x03;

                        break;
                    case 3:
                        WriteBuf[1] = 0x04;

                        break;
                    case 4:
                        WriteBuf[1] = 0x05;

                        break;
                    case 5:
                        WriteBuf[1] = 0x06;

                        break;
                    default:
                        break;
                }
                // ReadWriteIO.sendFrameBuild(WriteBuf, 0x09, len);   
            }
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_REGION, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x09, len);
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

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        ReadWriteIO.comm.DiscardInBuffer();
            //        ReadWriteIO.comm.DiscardOutBuffer();
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));        
            //        }

            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            cbB_GetRegion.Text = "设置失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            cbB_GetRegion.Text = "Set Failed";
            //        }

            //        return;
            //    }

            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        while ((revlen < 0x09) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x07) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }    
            //    }



            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            cbB_GetRegion.Text = "设置失败";
            //        }
            //        else
            //        {
            //            cbB_GetRegion.Text = "Set Failed";
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
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    } 
            //}

            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00)
                    && (revbuf[4] == CMD.FRAME_CMD_SET_REGION_RSP)
                    && (revbuf[5] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        cbB_GetRegion.Text = "设置失败";
                    }
                    else
                    {
                        cbB_GetRegion.Text = "Set Failed";
                    }
                    return;
                }

                if (0 == ReaderParams.LanguageFlag)
                {
                    cbB_GetRegion.Text = "设置成功";
                }
                else
                {
                    cbB_GetRegion.Text = "Set OK";
                }
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x89)
                    && (revbuf[2] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        cbB_GetRegion.Text = "设置失败";
                    }
                    else
                    {
                        cbB_GetRegion.Text = "Set Failed";
                    }
                    return;
                }

                if (0 == ReaderParams.LanguageFlag)
                {
                    cbB_GetRegion.Text = "设置成功";
                }
                else
                {
                    cbB_GetRegion.Text = "Set OK";
                }
            }

        }

        private void bt_GetRegion_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_REGION, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x11, len);     
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_REGION, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x11, len);
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

            //if (1 == ReaderParams.CommIntSelectFlag)
            //{
            //    if (ReadWriteIO.comm.IsOpen)
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        ReadWriteIO.comm.Read(revbuf, 0, revlen);
            //        revlen = 0;
            //        if (0 == ReaderParams.ProtocolFlag)
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
            //        }
            //        else
            //        {
            //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN - 2));      
            //        }

            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            cbB_GetRegion.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            cbB_GetRegion.Text = "Get Failed";
            //        }

            //        return;
            //    }

            //    if (0 == ReaderParams.ProtocolFlag)
            //    {
            //        while ((revlen < 0x0A) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x08) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }     
            //    }



            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            cbB_GetRegion.Text = "获取失败";
            //        }
            //        else
            //        {
            //            cbB_GetRegion.Text = "Get Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //        revlen = 15;
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
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    } 
            //}
            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_REGION_RSP)
                    && (revbuf[5] == 0x01)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        cbB_GetRegion.Text = "获取失败";
                    }
                    else
                    {
                        cbB_GetRegion.Text = "Get Failed";
                    }
                    return;
                }

                switch (revbuf[6])
                {
                    case 0x01:
                        cbB_GetRegion.SelectedIndex = 0;

                        break;

                    case 0x02:
                        cbB_GetRegion.SelectedIndex = 1;

                        break;

                    case 0x04:
                        cbB_GetRegion.SelectedIndex = 2;

                        break;

                    case 0x08:
                        cbB_GetRegion.SelectedIndex = 3;

                        break;

                    case 0x16:
                        cbB_GetRegion.SelectedIndex = 4;

                        break;

                    case 0x32:
                        cbB_GetRegion.SelectedIndex = 5;

                        break;
                    case 0x33:
                        cbB_GetRegion.SelectedIndex = 6;

                        break;
                    case 0x34:
                        cbB_GetRegion.SelectedIndex = 7;

                        break;
                    case 0x35:
                        cbB_GetRegion.SelectedIndex = 8;

                        break;
                    case 0x36:
                        cbB_GetRegion.SelectedIndex = 9;

                        break;
                    case 0x37:
                        cbB_GetRegion.SelectedIndex = 10;

                        break;
                    case 0x38:
                        cbB_GetRegion.SelectedIndex = 11;

                        break;
                    case 0x39:
                        cbB_GetRegion.SelectedIndex = 12;

                        break;
                    case 0x3A:
                        cbB_GetRegion.SelectedIndex = 13;

                        break;
                    case 0x3B:
                        cbB_GetRegion.SelectedIndex = 14;

                        break;
                    case 0x3C:
                        cbB_GetRegion.SelectedIndex = 15;

                        break;
                    default:
                        break;
                }
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x91)
                    && (revbuf[2] == 0x02)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        cbB_GetRegion.Text = "获取失败";
                    }
                    else
                    {
                        cbB_GetRegion.Text = "Get Failed";
                    }
                    return;
                }

                switch (revbuf[6 - 2])
                {
                    case 0x01:
                        cbB_GetRegion.SelectedIndex = 0;

                        break;

                    case 0x02:
                        cbB_GetRegion.SelectedIndex = 1;

                        break;

                    case 0x04:
                        cbB_GetRegion.SelectedIndex = 2;

                        break;

                    case 0x08:
                        cbB_GetRegion.SelectedIndex = 3;

                        break;

                    case 0x16:
                        cbB_GetRegion.SelectedIndex = 4;

                        break;

                    case 0x32:
                        cbB_GetRegion.SelectedIndex = 5;

                        break;
                    case 0x33:
                        cbB_GetRegion.SelectedIndex = 6;

                        break;
                    case 0x34:
                        cbB_GetRegion.SelectedIndex = 7;

                        break;
                    case 0x35:
                        cbB_GetRegion.SelectedIndex = 8;

                        break;
                    case 0x36:
                        cbB_GetRegion.SelectedIndex = 9;

                        break;
                    case 0x37:
                        cbB_GetRegion.SelectedIndex = 10;

                        break;
                    case 0x38:
                        cbB_GetRegion.SelectedIndex = 11;

                        break;
                    case 0x39:
                        cbB_GetRegion.SelectedIndex = 12;

                        break;
                    case 0x3A:
                        cbB_GetRegion.SelectedIndex = 13;

                        break;
                    case 0x3B:
                        cbB_GetRegion.SelectedIndex = 14;

                        break;
                    case 0x3C:
                        cbB_GetRegion.SelectedIndex = 15;

                        break;
                    default:
                        break;

                }
            }

        }

        private void bt_Clear_Click(object sender, EventArgs e)
        {
            tB_TxPower.Text = "";
            cbB_GetRegion.SelectedIndex = -1;
            tB_SoftWare.Text = "";
            tB_Firmware.Text = "";
            tB_Hardware.Text = "";
            tB_ModuleID.Text = "";
            comboBox_recommand.SelectedIndex = -1;
            tB_DispFreq.Text = "";
            tB_FreqNum.Text = "0";
            tB_Frequency.Text = "";
            cB_ANT1.Checked = false;
            cB_ANT2.Checked = false;
            cB_ANT3.Checked = false;
            cB_ANT4.Checked = false;
            tB_Ant1WorkTime.Text = "";
            tB_Ant2WorkTime.Text = "";
            tB_Ant3WorkTime.Text = "";
            tB_Ant4WorkTime.Text = "";
            tB_Intervaltime.Text = "";
            tB_MulInvworkTime.Text = "";
            tB_MulInvDelayTime.Text = "";
        }

        private void bt_GetRecomm_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = 0x00; WriteBuf[1] = 0x00;
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_RF_LINK, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_RF_LINK, len);

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
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_GET_RF_LINK_RSP)
                && (revbuf[5] == 0x01)))
            {
                return;
            }

            ReaderParams.Recommand = (UInt16)revbuf[7];
            comboBox_recommand.SelectedIndex = ReaderParams.Recommand;
        }

        private void bt_AddFreq_Click(object sender, EventArgs e)
        {
            int freqNum;
            string str;

            str = tB_FreqNum.Text;
            if (str.Length == 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请先点击清除", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please Clean First", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            freqNum = int.Parse(str);
            if (freqNum >= 50)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("最大跳频点数不能超过50个", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The Num of Freq must be less than 50", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            str = tB_Frequency.Text;
            if (str.Length < 3)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("输入的跳频频点有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("input Freq error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            FrequencyTable[freqNum] = float.Parse(str);
            FrequencyTable[freqNum] = (float)(FrequencyTable[freqNum] * 1000.0);

            freqNum++;
            tB_FreqNum.Text = freqNum.ToString();
        }

        private void bt_FreqClean_Click(object sender, EventArgs e)
        {
            tB_FreqNum.Text = "0";
            tB_Frequency.Text = "922.375";
            tB_DispFreq.Text = "";
        }

        private void bt_GetFreqTable_Click(object sender, EventArgs e)
        {
            byte[] txpower = new byte[2];
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str = "";
            int i;
            UInt16 len = 0;

            tB_DispFreq.Text = "";

            len = 0;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_FHSS, len);
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_FHSS, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x00, len);
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
            //            tB_DispFreq.Text = "Get Failed";
            //        }
            //        return;
            //    }

            //    while ((revlen == 0) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            tB_DispFreq.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_DispFreq.Text = "Get Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
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
            //            tB_DispFreq.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_DispFreq.Text = "Get Failed";
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
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
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_GET_FHSS_RSP)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_DispFreq.Text = "获取失败";
                }
                else
                {
                    tB_DispFreq.Text = "Get Failed";
                }
                return;
            }

            tB_FreqNum.Text = revbuf[5].ToString();

            for (i = 0; i < revbuf[5]; i++)
            {
                float freq;
                freq = (revbuf[6 + 3 * i] << 16) + (revbuf[6 + 3 * i + 1] << 8) + (revbuf[6 + 3 * i + 2]);
                freq = (float)(freq / 1000.0);
                str += freq.ToString();
                str += " ";
            }
            tB_DispFreq.Text = str;
        }

        private void bt_SetFreqTable_Click(object sender, EventArgs e)
        {
            byte[] txpower = new byte[2];
            Byte[] WriteBuf = new Byte[500];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            int i;
            UInt16 len = 0;

            tB_DispFreq.Text = "";

            str = tB_FreqNum.Text;
            WriteBuf[len++] = byte.Parse(str);
            for (i = 0; i < WriteBuf[0]; i++)
            {
                WriteBuf[len++] = (byte)((UInt32)FrequencyTable[i] >> 16);
                WriteBuf[len++] = (byte)((UInt32)FrequencyTable[i] >> 8);
                WriteBuf[len++] = (byte)((UInt32)FrequencyTable[i]);
            }
            if (cB_selffreqSaveFlag.Checked == true)
            {
                WriteBuf[len++] = 1;
            }
            else
            {
                WriteBuf[len++] = 0;
            }
            //            len = (byte)(3 * WriteBuf[1] + 2);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_FHSS, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_FHSS, len);

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
            //            tB_DispFreq.Text = "Set Failed";
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
            //            tB_DispFreq.Text = "设置失败";
            //        }
            //        else
            //        {
            //            tB_DispFreq.Text = "Set Failed";
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
            //        //ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //        revlen = 0;
            //        ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
            //    }
            //    else
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_DispFreq.Text = "设置失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_DispFreq.Text = "Set Failed";
            //        }
            //        return;
            //    }


            //    while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
            //    {
            //        recount--;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_FHSS_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_DispFreq.Text = "设置失败";
                }
                else
                {
                    tB_DispFreq.Text = "Set Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                tB_DispFreq.Text = "设置成功";
            }
            else
            {
                tB_DispFreq.Text = "Set OK";
            }
            System.Array.Clear(FrequencyTable, 0, FrequencyTable.Length);
        }

        private void cB_TempProtect_CheckedChanged(object sender, EventArgs e)
        {

            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            /* 是否温度保护 */
            if (cB_TempProtect.Checked == true)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            UInt16 len = 1;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_TEMPPROTECT, len);

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
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("温度保护设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("温度保护设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_TEMPPROTECT_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("温度保护设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (true == cB_antSaveFlag.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            WriteBuf[1] = (Byte)(0);
            WriteBuf[2] = (Byte)(0);

            if (true == cB_ANT1.Checked)
            {
                WriteBuf[2] |= 0x01;
            }
            else
            {
                WriteBuf[2] &= 0xFE;
            }

            if (true == cB_ANT2.Checked)
            {
                WriteBuf[2] |= 0x02;
            }
            else
            {
                WriteBuf[2] &= 0xFD;
            }

            if (true == cB_ANT3.Checked)
            {
                WriteBuf[2] |= 0x04;
            }
            else
            {
                WriteBuf[2] &= 0xFB;
            }

            if (true == cB_ANT4.Checked)
            {
                WriteBuf[2] |= 0x08;
            }
            else
            {
                WriteBuf[2] &= 0xF7;
            }

            //switch (cB_AntSet.SelectedIndex)
            //{
            //    case 0:
            //        WriteBuf[0] = (Byte)(0);
            //        WriteBuf[1] = (byte)(1);
            //        break;
            //    case 1:
            //        WriteBuf[0] = (Byte)(0);
            //        WriteBuf[1] = (byte)(2);
            //        break;
            //    case 2:
            //        WriteBuf[0] = (Byte)(0);
            //        WriteBuf[1] = (byte)(4);
            //        break;
            //    case 3:
            //        WriteBuf[0] = (Byte)(0);
            //        WriteBuf[1] = (byte)(8);
            //        break;

            //    default:
            //        break;
            //}

            UInt16 len = 3;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_ANT_STATUE, len);
            int result = 0;

            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_ANT_STATUE, len);

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
            //            MessageBox.Show("天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_ANT_STATUE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
        }

        private void bt_AntGet_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            UInt16 len = 0;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_ANT_STATUE, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_ANT_STATUE, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //            MessageBox.Show("获取天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(100);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            ////判断是否设置成功
            //if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST) 
            //    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
            //    && (revbuf[2] == 0x00) && (revbuf[3] == 0x0A)
            //    && (revbuf[4] == CMD.FRAME_CMD_GET_ANT_STATUE_RSP)))
            //{
            //    if (0 == ReaderParams.LanguageFlag)
            //    {
            //        MessageBox.Show("获取天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    return;
            //}

            int antselect = (int)((revbuf[5] << 8) + revbuf[6]);
            if ((antselect & 0x01) == 0x01)
            {
                cB_ANT1.Checked = true;
            }
            else
            {
                cB_ANT1.Checked = false;
            }

            if ((antselect & 0x02) == 0x02)
            {
                cB_ANT2.Checked = true;
            }
            else
            {
                cB_ANT2.Checked = false;
            }

            if ((antselect & 0x04) == 0x04)
            {
                cB_ANT3.Checked = true;
            }
            else
            {
                cB_ANT3.Checked = false;
            }

            if ((antselect & 0x08) == 0x08)
            {
                cB_ANT4.Checked = true;
            }
            else
            {
                cB_ANT4.Checked = false;
            }
        }

        private void bt_MulInvDeyTimeSet_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;          //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (true == cB_WkDeTimeSaveFlag.Checked)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }
            if (tB_MulInvworkTime.Text == "")
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("输入信息有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("input info error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            WriteBuf[1] = (Byte)(UInt16.Parse(tB_MulInvworkTime.Text) >> 8);
            WriteBuf[2] = (byte)(UInt16.Parse(tB_MulInvworkTime.Text));

            WriteBuf[3] = (Byte)(UInt16.Parse(tB_MulInvDelayTime.Text) >> 8);
            WriteBuf[4] = (byte)(UInt16.Parse(tB_MulInvDelayTime.Text));

            UInt16 len = 5;
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_MULWAITTIME, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_MULWAITTIME, len);

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
            //            MessageBox.Show("连续寻卡等待时间设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_MULWAITTIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("连续寻卡等待时间设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
        }

        private void bt_MulInvDeyTimeGet_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            UInt16 len = 0;

            tB_MulInvworkTime.Text = "";
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_MULWAITTIME, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_MULWAITTIME, len);

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

            //    while ((revlen < 0x0D) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            MessageBox.Show("获取连续寻卡等待时间设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0D)
                && (revbuf[4] == CMD.FRAME_CMD_GET_MULWAITTIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("获取连续寻卡等待时间设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            UInt16 waittime = (UInt16)((revbuf[6] << 8) + revbuf[7]);
            tB_MulInvworkTime.Text = waittime.ToString();
            waittime = (UInt16)((revbuf[8] << 8) + revbuf[9]);
            tB_MulInvDelayTime.Text = waittime.ToString();
        }

        private void tB_MulInvDelayTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的字符不是十进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请输入数字", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter the Arabic numerals", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
                return;
            }
        }

        private void tB_MulInvDelayTime_TextChanged(object sender, EventArgs e)
        {
            if (tB_MulInvworkTime.Text.Length > 0)
            {
                int MulWaitTime = int.Parse(tB_MulInvworkTime.Text);
                if (MulWaitTime > 0xFFFF)
                {
                    tB_MulInvworkTime.Text = "65535";
                }
            }
        }

        private void bt_GetGpioStatus_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;                    //número de reintentos
            int revlen = 0;                         //Longitud de datos recibidos
            Byte[] revbuf = new Byte[500];          //Recibir búfer

            lB_GetGpioDisp.Text = "";
            WriteBuf[0] = 0xFF;
            WriteBuf[1] = 0x00;
            int result = 0;

            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_GPIO, len);

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

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                && (revbuf[4] == CMD.FRAME_CMD_GET_GPIO_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lB_GetGpioDisp.Text = "获取失败";
                }
                else
                {
                    lB_GetGpioDisp.Text = "Get Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lB_GetGpioDisp.Text = "获取成功";
            }
            else
            {
                lB_GetGpioDisp.Text = "Get OK";
            }

            if ((revbuf[7] & 0x01) == 0x01)           /* 判断GPIO1状态 */
            {
                cB_gpio1.Checked = true;
            }
            else
            {
                cB_gpio1.Checked = false;
            }

            if ((revbuf[7] & 0x02) == 0x02)           /* 判断GPIO2状态 */
            {
                cB_gpio2.Checked = true;
            }
            else
            {
                cB_gpio2.Checked = false;
            }

            if ((revbuf[7] & 0x04) == 0x04)           /* 判断GPIO3状态 */
            {
                cB_gpio3.Checked = true;
            }
            else
            {
                cB_gpio3.Checked = false;
            }

            if ((revbuf[7] & 0x08) == 0x08)           /* 判断GPIO4状态 */
            {
                cB_gpio4.Checked = true;
            }
            else
            {
                cB_gpio4.Checked = false;
            }

            if ((revbuf[7] & 0x10) == 0x10)           /* 判断GPIO5状态 */
            {
                cB_gpio5.Checked = true;
            }
            else
            {
                cB_gpio5.Checked = false;
            }

            if ((revbuf[7] & 0x20) == 0x20)           /* 判断GPIO6状态 */
            {
                cB_gpio6.Checked = true;
            }
            else
            {
                cB_gpio6.Checked = false;
            }

            if ((revbuf[7] & 0x40) == 0x40)           /* 判断GPIO7状态 */
            {
                cB_gpio7.Checked = true;
            }
            else
            {
                cB_gpio7.Checked = false;
            }

            if ((revbuf[7] & 0x80) == 0x80)           /* Determinar el estado de GPIO8 */
            {
                cB_gpio8.Checked = true;
            }
            else
            {
                cB_gpio8.Checked = false;
            }
        }

        private int SendSetGPIOStatus(byte mask, byte data)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = mask;
            WriteBuf[1] = data;
            int result = 0;
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_GPIO, len);
            if (result == 0)
                if (0 == ReaderParams.LanguageFlag)
                    MessageBox.Show("接收数据超时", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    MessageBox.Show("over time", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            revbuf = g_Revbuf;

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_GPIO_RSP)
                && (revbuf[5] == 0x01)))
                return -2;

            return 0;
        }

        private void cB_gpio1_MouseClick(object sender, MouseEventArgs e)
        {
            byte mask = 0;
            byte data = 0;
            int result = 0;

            lB_GetGpioDisp.Text = "";

            if (cB_gpio1.Checked == true)
            {
                mask = 0x01;
                data = 0x01;

                result = SendSetGPIOStatus(mask, data);

                if (0 == result)
                    if (0 == ReaderParams.LanguageFlag)
                        lB_GetGpioDisp.Text = "设置成功";
                    else
                        lB_GetGpioDisp.Text = "Set OK";
                else
                    if (0 == ReaderParams.LanguageFlag)
                    lB_GetGpioDisp.Text = "设置失败";
                else
                    lB_GetGpioDisp.Text = "Set Failed";
            }
            else
            {
                mask = 0x01;
                data = 0x00;

                result = SendSetGPIOStatus(mask, data);

                if (0 == result)
                    if (0 == ReaderParams.LanguageFlag)
                        lB_GetGpioDisp.Text = "设置成功";
                    else
                        lB_GetGpioDisp.Text = "Set OK";
                else
                    if (0 == ReaderParams.LanguageFlag)
                    lB_GetGpioDisp.Text = "设置失败";
                else
                    lB_GetGpioDisp.Text = "Set Failed";
            }
        }

        private void cB_gpio2_MouseClick(object sender, MouseEventArgs e)
        {
            //Actualización - José Liza: 20210130
            ActivarPIO_N(cB_gpio2);
        }

        private void cB_gpio3_MouseClick(object sender, MouseEventArgs e)
        {
            //Actualización - José Liza: 20210130
            ActivarPIO_N(cB_gpio3);
        }

        //Actualización - José Liza: 20210130
        private void ActivarPIO_N(Control pioN)
        {
            byte mask = 0;
            byte data = 0;
            int result = 0;

            lB_GetGpioDisp.Text = "";

            bool bpio = ((CheckBox)pioN).Checked;
            string nombre = ((CheckBox)pioN).Text;

            if (bpio)
            {
                switch (nombre)
                {
                    case "GPIO1":
                        mask = 0x01;
                        data = 0x01;
                        break;
                    case "GPIO2":
                        mask = 0x02;
                        data = 0x02;
                        break;
                    case "GPIO3":
                        mask = 0x04;
                        data = 0x04;
                        break;
                    case "GPIO4":
                        mask = 0x08;
                        data = 0x08;
                        break;
                    default:
                        break;
                }

                result = SendSetGPIOStatus(mask, data);
                if (0 == result)
                    lB_GetGpioDisp.Text = "Set OK";
                else
                    lB_GetGpioDisp.Text = "Set Failed";
            }
            else
            {
                switch (nombre)
                {
                    case "GPIO1":
                        mask = 0x01;
                        data = 0x00;
                        break;
                    case "GPIO2":
                        mask = 0x02;
                        data = 0x00;
                        break;
                    case "GPIO3":
                        mask = 0x04;
                        data = 0x00;
                        break;
                    case "GPIO4":
                        mask = 0x08;
                        data = 0x00;
                        break;
                    default:
                        break;
                }

                result = SendSetGPIOStatus(mask, data);
                if (0 == result)
                    lB_GetGpioDisp.Text = "Set OK";
                else
                    lB_GetGpioDisp.Text = "Set Failed";
            }
        }

        private int SetAntWorkTime(int id, UInt16 time)
        {
            UInt16 len = 3;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = (byte)(id & 0x0F);
            if (cB_MulantSaveFlag.Checked == true)
            {
                WriteBuf[0] |= 0x10;
            }

            WriteBuf[1] = (byte)((time >> 8) & 0xFF);
            WriteBuf[2] = (byte)(time & 0xFF);

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_ANT_TIME, len);

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
                    return -3;
                }

                while ((revlen < 0x09) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    return -1;
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
                    return -3;
                }


                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount == 0)       //未收到数据
                {
                    return -1;
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_ANT_TIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -2;
            }

            return 0;
        }

        private int GetAntWorkTime(int id, byte[] time)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = (byte)id;
            WriteBuf[1] = 0;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_ANT_TIME, len);

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
                    return -3;
                }

                while ((revlen < 0x0C) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    return -1;
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
                    return -3;
                }


                while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
                {
                    recount--;
                }

                if (recount == 0)       //未收到数据
                {
                    return -1;
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
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0C)
                && (revbuf[4] == CMD.FRAME_CMD_GET_ANT_TIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -2;
            }

            time[0] = revbuf[7];
            time[1] = revbuf[8];

            return 0;
        }

        private void bt_AntWorkTimeSet_Click(object sender, EventArgs e)
        {
            UInt16 worktime = 0;
            if ((tB_Ant1WorkTime.Text == "") || (tB_Ant2WorkTime.Text == "") || (tB_Ant3WorkTime.Text == "") || (tB_Ant4WorkTime.Text == ""))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("输入信息有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("input info error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            worktime = UInt16.Parse(tB_Ant1WorkTime.Text);
            SetAntWorkTime(1, worktime);
            worktime = UInt16.Parse(tB_Ant2WorkTime.Text);
            SetAntWorkTime(2, worktime);
            worktime = UInt16.Parse(tB_Ant3WorkTime.Text);
            SetAntWorkTime(3, worktime);
            worktime = UInt16.Parse(tB_Ant4WorkTime.Text);
            SetAntWorkTime(4, worktime);
        }

        private void bt_AntWorkTimeGet_Click(object sender, EventArgs e)
        {
            byte[] worktime = new byte[2];

            tB_Ant1WorkTime.Text = "";
            tB_Ant2WorkTime.Text = "";
            tB_Ant3WorkTime.Text = "";
            tB_Ant4WorkTime.Text = "";
            groupBox12.Refresh();

            GetAntWorkTime(1, worktime);
            UInt16 time = (UInt16)((worktime[0] << 8) + worktime[1]);
            tB_Ant1WorkTime.Text = time.ToString();

            GetAntWorkTime(2, worktime);
            time = (UInt16)((worktime[0] << 8) + worktime[1]);
            tB_Ant2WorkTime.Text = time.ToString();

            GetAntWorkTime(3, worktime);
            time = (UInt16)((worktime[0] << 8) + worktime[1]);
            tB_Ant3WorkTime.Text = time.ToString();

            GetAntWorkTime(4, worktime);
            time = (UInt16)((worktime[0] << 8) + worktime[1]);
            tB_Ant4WorkTime.Text = time.ToString();
        }

        private void bt_AntIntTimeSet_Click(object sender, EventArgs e)
        {
            if (tB_Intervaltime.Text == "")
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("输入信息有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("input info error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            UInt16 len = 3;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            lb_setIntTime.Text = "";
            groupBox13.Refresh();

            if (cB_IntSaveFlag.Checked == true)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            WriteBuf[1] = (byte)((UInt16.Parse(tB_Intervaltime.Text) >> 8) & 0xFF);
            WriteBuf[2] = (byte)((UInt16.Parse(tB_Intervaltime.Text)) & 0xFF);

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_MULANT_TIME, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_MULANT_TIME, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //            lb_setIntTime.Text = "设置失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_setIntTime.Text = "Set Failed";
            //        }

            //        return;
            //    }

            //    while ((revlen < 0x09) && (recount != 0))
            //    {
            //        recount--;
            //        revlen = ReadWriteIO.comm.BytesToRead;
            //    }

            //    if (recount == 0)       //未收到数据
            //    {
            //        if (0 == ReaderParams.LanguageFlag)
            //        {
            //            lb_setIntTime.Text = "设置失败";
            //        }
            //        else
            //        {
            //            lb_setIntTime.Text = "Set Failed";
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_MULANT_TIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_setIntTime.Text = "设置失败";
                }
                else
                {
                    lb_setIntTime.Text = "Set Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_setIntTime.Text = "设置成功";
            }
            else
            {
                lb_setIntTime.Text = "Set OK";
            }
        }

        private void bt_AntIntTimeGet_Click(object sender, EventArgs e)
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            lb_setIntTime.Text = "";
            tB_Intervaltime.Text = "";
            groupBox13.Refresh();

            WriteBuf[0] = (byte)0;
            WriteBuf[1] = (byte)0;

            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_MULANT_TIME, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_MULANT_TIME, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0C, len);
            //}
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
            //            lb_setIntTime.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_setIntTime.Text = "Get Failed";
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
            //            lb_setIntTime.Text = "获取失败";
            //        }
            //        else
            //        {
            //            lb_setIntTime.Text = "Get Failed";
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
                && (revbuf[4] == CMD.FRAME_CMD_GET_MULANT_TIME_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_setIntTime.Text = "获取失败";
                }
                else
                {
                    lb_setIntTime.Text = "Get Failed";
                }
                return;
            }

            UInt16 time = (UInt16)((revbuf[6] << 8) + revbuf[7]);
            tB_Intervaltime.Text = time.ToString();
            if (0 == ReaderParams.LanguageFlag)
            {
                lb_setIntTime.Text = "获取成功";
            }
            else
            {
                lb_setIntTime.Text = "Get OK";
            }
        }

        private void bt_Buzzer_Click(object sender, EventArgs e)
        {
            EmitirSonido();
        }
        //Actualizacion - Jose Liza: 20210125
        public void EmitirSonido()
        {
            UInt16 len = 2;
            Byte[] WriteBuf = new Byte[100];
            int recount = 100000;
            int revlen = 0;
            Byte[] revbuf = new Byte[500];

            WriteBuf[0] = (byte)0;
            WriteBuf[1] = (byte)0;

            int result = 0;

            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_BUZZER, len);

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
        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            byte mask = 0;
            byte data = 0;
            int result = 0;

            lB_GetGpioDisp.Text = "";

            if (cB_gpio4.Checked == true)
            {
                mask = 0x08;
                data = 0x08;

                result = SendSetGPIOStatus(mask, data);

                if (0 == result)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        lB_GetGpioDisp.Text = "设置成功";
                    }
                    else
                    {
                        lB_GetGpioDisp.Text = "Set Ok";
                    }
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        lB_GetGpioDisp.Text = "设置失败";
                    }
                    else
                    {
                        lB_GetGpioDisp.Text = "Set Failed";
                    }
                }
            }
            else
            {
                mask = 0x08;
                data = 0x00;

                result = SendSetGPIOStatus(mask, data);

                if (0 == result)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        lB_GetGpioDisp.Text = "设置成功";
                    }
                    else
                    {
                        lB_GetGpioDisp.Text = "Set Ok";
                    }
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        lB_GetGpioDisp.Text = "设置失败";
                    }
                    else
                    {
                        lB_GetGpioDisp.Text = "Set Failed";
                    }
                }
            }
        }

        private void tB_MulInvDelayTime_TextChanged_1(object sender, EventArgs e)
        {
            if (tB_MulInvDelayTime.Text.Length > 0)
            {
                int MulWaitTime = int.Parse(tB_MulInvDelayTime.Text);
                if (MulWaitTime > 0xFFFF)
                {
                    tB_MulInvDelayTime.Text = "65535";
                }
            }
        }

        private void tB_MulInvworkTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的字符不是十进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请输入数字", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter the Arabic numerals", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
                return;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PROFRECTESE frm = new PROFRECTESE();
            if (0 == ReaderParams.LanguageFlag)
            {
                frm.Text = "高级频点设置";
            }
            else
            {
                frm.Text = "PRO Frequency-Hopping Spread Spectrum Setting";
            }
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UInt16 len = 6;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            WriteBuf[0] = 0x04;
            if (cBGPIOSAVE.Checked == true)
            {
                WriteBuf[1] = 0x01;
            }
            else
            {
                WriteBuf[1] = 0x00;
            }

            if (true == cBGPIO5.Checked)
            {
                WriteBuf[2] |= 0x10;
            }
            else
            {
                WriteBuf[2] &= 0xEF;
            }

            if (true == cBGPIO6.Checked)
            {
                WriteBuf[2] |= 0x20;
            }
            else
            {
                WriteBuf[2] &= 0xDF;
            }

            if (true == cBGPIO7.Checked)
            {
                WriteBuf[2] |= 0x40;
            }
            else
            {
                WriteBuf[2] &= 0xBF;
            }

            if (true == cBGPIO8.Checked)
            {
                WriteBuf[2] |= 0x80;
            }
            else
            {
                WriteBuf[2] &= 0x7F;
            }

            //ReadWriteIO.sendFrameBuild(WriteBuf, 0x7A, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_TX_POWER, len);
            //}
            //else
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, 0x7A, len);
            //}
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
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            label13.Text = "Set Failed";
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
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label13.Text = "Set Failed";
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
                && (revbuf[4] == 0x7B)
                && (revbuf[5] == 0x05)
                && (revbuf[6] == 0x01)
                ))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label13.Text = "设置失败";
                }
                else
                {
                    label13.Text = "Set Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label13.Text = "设置成功";
            }
            else
            {
                label13.Text = "Set OK";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = 0x06;
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_TX_POWER, len);
            //}
            //else
            //{
            result = delay(CMD.TIMEOUT, WriteBuf, 0x7A, len);
            //}
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, 0x7A, len);

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
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label13.Text = "Get Failed";
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
            //            label13.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label13.Text = "Get Failed";
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
                && (revbuf[2] == 0x00)
                && (revbuf[4] == 0x7B)
                && (revbuf[5] == 0x07)
                && (revbuf[6] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label13.Text = "设置失败";
                }
                else
                {
                    label13.Text = "Get Failed";
                }
                return;
            }
            if (0 == ReaderParams.LanguageFlag)
            {
                label13.Text = "设置成功";
            }
            else
            {
                label13.Text = "Get OK";
            }
            if (revbuf[7] == 0x05)
            {
                cBGPIO5.Checked = true;
            }
            else
                if (revbuf[7] == 0x06)
            {
                cBGPIO6.Checked = true;
            }
            else
                    if (revbuf[7] == 0x07)
            {
                cBGPIO7.Checked = true;
            }
            else
                        if (revbuf[7] == 0x08)
            {
                cBGPIO8.Checked = true;
            }
            else

                return;
        }

        private void cBGPIO5_CheckedChanged(object sender, EventArgs e)
        {
            if (cBGPIO5.Checked == true)
            {
                cBGPIO6.Checked = false;
                cBGPIO7.Checked = false;
                cBGPIO8.Checked = false;
            }
        }

        private void cBGPIO6_CheckedChanged(object sender, EventArgs e)
        {
            if (cBGPIO6.Checked == true)
            {
                cBGPIO5.Checked = false;
                cBGPIO7.Checked = false;
                cBGPIO8.Checked = false;
            }
        }

        private void cBGPIO7_CheckedChanged(object sender, EventArgs e)
        {
            if (cBGPIO7.Checked == true)
            {
                cBGPIO6.Checked = false;
                cBGPIO5.Checked = false;
                cBGPIO8.Checked = false;
            }
        }

        private void cBGPIO8_CheckedChanged(object sender, EventArgs e)
        {
            if (cBGPIO8.Checked == true)
            {
                cBGPIO6.Checked = false;
                cBGPIO7.Checked = false;
                cBGPIO5.Checked = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = 0xF0;
            int result = 0;

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


            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == 0x7B)
                && (revbuf[5] == 0xF1)
                && (revbuf[6] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label26.Text = "设置成功";
                }
                else
                {
                    label26.Text = "Set OK";
                }
                return;
            }
            if (revbuf[7] == 0X03)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label26.Text = "BOD功能已开启";
                }
                else
                {
                    label26.Text = "BOD Opened";
                }
                return;
            }
            if (0 == ReaderParams.LanguageFlag)
            {
                label26.Text = "BOD功能未开启";
            }
            else
            {
                label26.Text = "BOD did not Open";
            }
            return;
        }

    }
}

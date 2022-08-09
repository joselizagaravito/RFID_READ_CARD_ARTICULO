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
    public partial class AdvanceParaSet : Form
    {
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

        public AdvanceParaSet()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            comboBox_Selecttarget.SelectedIndex = ReaderParams.Selecttarget;
            comboBox_action.SelectedIndex = ReaderParams.action;
            comboBox_MB.SelectedIndex = ReaderParams.MB;
            comboBox_trun.SelectedIndex = ReaderParams.trun;

            textBox_len.Text = Convert.ToString(ReaderParams.select_len);
            textBox_pointer.Text = Convert.ToString(ReaderParams.select_pointer);
            textBox_mask.Text = Convert.ToString(ReaderParams.select_mask);

            comboBox_DR.SelectedIndex = ReaderParams.DR;
            comboBox_M.SelectedIndex = ReaderParams.M;
            comboBox_TRext.SelectedIndex = ReaderParams.TRext;
            comboBox_Sel.SelectedIndex = ReaderParams.Sel;
            comboBox_Session.SelectedIndex = ReaderParams.Session;
            comboBox_QuerTarget.SelectedIndex = ReaderParams.QuerTarget;

            lB_NumOfData.Text = ((textBox_mask.Text.Length + 2) / 3).ToString();

            //寻卡超时时间
            textBox_InvTimeOut.Text = Convert.ToString(ReaderParams.InvTimeOut);
            textBox_startQ.Text = Convert.ToString(ReaderParams.startQ);
            textBox_minQ.Text = Convert.ToString(ReaderParams.minQ);
            textBox_maxQ.Text = Convert.ToString(ReaderParams.maxQ);
            comboBox_InventoryAlg.SelectedIndex = ReaderParams.InventoryAlg;
            comboBox_LinkProfile.SelectedIndex = ReaderParams.LinkProfile;
            comboBox_ModulateWay.SelectedIndex = ReaderParams.ModulateWay;

            if (0 == ReaderParams.LanguageFlag)
            {
                Text                        = "高级设置";
                groupBox2.Text              = "寻标签过滤设置:";
                groupBox4.Text              = "RX端检测功率";
                groupBox5.Text              = "超时设置";
                groupBox6.Text              = "链路设置";
                groupBox1.Text              = "select和query命令参数设置:";

                label15.Text                = "CW设置：";
                label19.Text                = "CW设置：";
                label17.Text                = "RX端功率：";
                label18.Text                = "寻卡：";
                label22.Text                = "清点算法：";
                label23.Text                = "起始Q值：";
                label24.Text                = "最小Q值：";
                label25.Text                = "最大Q值：";
                label20.Text                = "链路频率:";
                label21.Text                = "调制方式:";
                label14.Text                = "字节";

                bt_SetCWOn.Text             = "开";
                bt_SetCWOff.Text            = "关";
                bt_TestCWSetOn.Text         = "开";
                bt_TestCWSetOff.Text        = "关";
                bt_GetLoss.Text             = "获取";
                bt_SetInvTimeOut.Text       = "设置";
                bt_SetQInfo.Text            = "设置";
                bt_GetQInfo.Text            = "获取";
                bt_SetLinkFrq.Text          = "设置";
                bt_GetLinkFrq.Text          = "获取";
                bt_SetAirPara.Text          = "设置";
                bt_GetAirPara.Text          = "获取";
                bt_SetDefault.Text          = "默认值";
                bt_SetSelectMaskData.Text   = "设置";
                cB_SelMaskSaveflag.Text     = "保存";

                groupBox9.Text              = "声光设置";
                cB_SLSaveflag.Text          = "保存";
                cB_Soundflag.Text           = "成功声音提示";
                cB_Lightflag.Text           = "成功灯光提示";
                soundget.Text               = "获取";
                soundset.Text               = "设置";
                cB_SelQSaveflag.Text        = "保存";
                cB_SelAirParaSaveflag.Text  = "保存";

            }
            else
            {
                Text                        = "Advanced Settings";
                groupBox2.Text              = "Inventory Mask Setting:";
                groupBox4.Text              = "RX ADC";
                groupBox5.Text              = "Timeout Set";
                groupBox6.Text              = "Link set";
                groupBox1.Text              = "select query parameter Set";

                label15.Text                = "CW Set：";
                label19.Text                = "CW Set：";
                label17.Text                = "RX ADC：";
                label18.Text                = "Single：";
                label22.Text                = "algorithm：";
                label23.Text                = "Start Q：";
                label24.Text                = "Min Q：";
                label25.Text                = "Max Q：";
                label20.Text                = "Link Freq:";
                label21.Text                = "Modu mode:";
                label14.Text                = "Byte";

                bt_SetCWOn.Text             = "ON";
                bt_SetCWOff.Text            = "OFF";
                bt_TestCWSetOn.Text         = "ON";
                bt_TestCWSetOff.Text        = "OFF";
                bt_GetLoss.Text             = "Get";
                bt_SetInvTimeOut.Text       = "Set";
                bt_SetQInfo.Text            = "Set";
                bt_GetQInfo.Text            = "Get";
                bt_SetLinkFrq.Text          = "Set";
                bt_GetLinkFrq.Text          = "Get";
                bt_SetAirPara.Text          = "Set";
                bt_GetAirPara.Text          = "Get";
                bt_SetDefault.Text          = "Default";
                bt_SetSelectMaskData.Text   = "Set";
                cB_SelMaskSaveflag.Text     = "Saved";

                groupBox9.Text             = "Sound & Light Setting";
                cB_SLSaveflag.Text         = "Saved";
                cB_Soundflag.Text          = "Sound Warning";
                cB_Lightflag.Text          = "Light Warning";
                soundget.Text              = "Get";
                soundset.Text              = "Set";
                cB_SelQSaveflag.Text       = "Saved";
                cB_SelAirParaSaveflag.Text = "Saved";
            }

            bt_SetLinkFrq.Enabled = false;
            bt_GetLinkFrq.Enabled = false;
            label27.Text = "";
            label28.Text = "";
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
        private void button_clear_Click(object sender, EventArgs e)
        {
            comboBox_Selecttarget.SelectedIndex = -1;
            comboBox_action.SelectedIndex = -1;
            comboBox_MB.SelectedIndex = -1;
            comboBox_trun.SelectedIndex = -1;

            textBox_len.Text = "";
            textBox_pointer.Text = "";
            textBox_mask.Text = "";

            comboBox_DR.SelectedIndex = -1;
            comboBox_M.SelectedIndex = -1;
            comboBox_TRext.SelectedIndex = -1;
            comboBox_Sel.SelectedIndex = -1;
            comboBox_Session.SelectedIndex = -1;
            comboBox_QuerTarget.SelectedIndex = -1;
        }

        public int SetSelectQueryPara()
        {
            int result = 0;
            byte[] buf = new byte[8];
            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            buf[0] &= 0x01;
            buf[2] &= 0xF0;
            buf[3] &= 0x07;

            UInt16 Selecttarget = (UInt16)comboBox_Selecttarget.SelectedIndex;
            UInt16 action = (UInt16)comboBox_action.SelectedIndex;
            UInt16 MB = (UInt16)comboBox_MB.SelectedIndex;
            UInt16 trun = (UInt16)comboBox_trun.SelectedIndex;

            UInt16 DR = (UInt16)comboBox_DR.SelectedIndex;
            UInt16 M = (UInt16)comboBox_M.SelectedIndex;
            UInt16 TRext = (UInt16)comboBox_TRext.SelectedIndex;
            UInt16 Sel = (UInt16)comboBox_Sel.SelectedIndex;
            UInt16 Session = (UInt16)comboBox_Session.SelectedIndex;
            UInt16 QuerTarget = (UInt16)comboBox_QuerTarget.SelectedIndex;

            buf[0] |= (byte)((Selecttarget & 0x07) << 5);
            buf[0] |= (byte)((action & 0x07) << 2);
            buf[0] |= (byte)((trun & 0x01) << 1); 
            buf[2] |= (byte)((DR & 0x01) << 3);
            buf[2] |= (byte)((M & 0x03) << 1);
            buf[2] |= (byte)((TRext & 0x01));
            buf[3] |= (byte)((Sel & 0x03) << 6);
            buf[3] |= (byte)((Session & 0x03) << 4);
            buf[3] |= (byte)((QuerTarget & 0x01) << 3);

            if (cB_SelAirParaSaveflag .Checked ==true)
            {
                buf[7] |= 0x01;
            }
            else
            {
                buf[7] &= 0xFE;
            }
            result = ReaderParams.SetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            return 0;
        }

        private void textBox_mask_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8)
                && (!((e.KeyChar >= 'A') && (e.KeyChar <= 'F'))))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请输入十六进制数", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("please input hex data", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
            }

            lB_NumOfData.Text = ((textBox_mask.Text.Length + 2) / 3).ToString();
            if (e.KeyChar == 8)
            {
                return;
            }

            if ((textBox_mask.Text.Length % 3) == 2)
            {
                textBox_mask.Text += "-";
                this.textBox_mask.SelectionStart = this.textBox_mask.Text.Length;
            }            
        }

        private void bt_SetCW_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            byte cwflag = 0;
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            cwflag = 0;

            WriteBuf[0] = cwflag;
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_CW_STATUE, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_CW_STATUE, len);
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
            //            MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_CW_STATUE_RSP) 
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void bt_TestCWSet_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            byte cwflag = 0;
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            cwflag = 0;

            WriteBuf[0] = cwflag;
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_TEST_SET_CW_STATUE, len);
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
        //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_TEST_SET_CW_STATUE, len);

        //    if (1 == ReaderParams.CommIntSelectFlag)
        //    {
        //        if (ReadWriteIO.comm.IsOpen)
        //        {
        //            ReadWriteIO.comm.DiscardInBuffer();
        //            ReadWriteIO.comm.DiscardOutBuffer();
        //            revlen = 0;
        //            ReadWriteIO.comm.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));
        //        }
        //        else
        //        {
        //            if (0 == ReaderParams.LanguageFlag)
        //            {
        //                MessageBox.Show("端口未打开，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            return;
        //        }

        //        while ((revlen < 9) && (recount != 0))
        //        {
        //            recount--;
        //            revlen = ReadWriteIO.comm.BytesToRead;
        //        }

        //        if (recount == 0)       //未收到数据
        //        {
        //            if (0 == ReaderParams.LanguageFlag)
        //            {
        //                MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            else
        //            {
        //                MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            revlen = ReadWriteIO.comm.BytesToRead;
        //            ReadWriteIO.comm.Read(revbuf, 0, revlen);
        //        }
        //    }
        //    else
        //    {
        //        recount     = ReaderParams.Netrecount;
        //        if (true == ReaderParams.nsStream.CanRead)
        //        {
        //            while (true == ReaderParams.nsStream.DataAvailable)
        //            {
        //                ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
        //            }
        //            revlen = 0;
        //            ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
        //        }
        //        else
        //        {
        //            if (0 == ReaderParams.LanguageFlag)
        //            {
        //                MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //            return;
        //        }


        //        while ((recount != 0) && (false == ReaderParams.nsStream.DataAvailable))
        //        {
        //            recount--;
        //        }

        //        if (recount == 0)       //未收到数据
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            System.Threading.Thread.Sleep(100);
        //            revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
        //        }          
        //    }

            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_TEST_SET_CW_STATUE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void bt_GetLoss_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            UInt16 Rxadc = 0;
            int i;

            tB_RevLoss.Text = "";
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_REVPWR, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_REVPWR, len);

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
            //            tB_RevLoss.Text = "获取失败";
            //        }
            //        else
            //        {
            //            tB_RevLoss.Text = "Get Failed";
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
                && (revbuf[2] == 0x00) 
                //&& (revbuf[3] == 0x0B)
                && (revbuf[4] == CMD.FRAME_CMD_GET_REVPWR_RSP)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_RevLoss.Text = "获取失败";
                }
                else
                {
                    tB_RevLoss.Text = "Get Failed";
                }
                return;
            }
            Rxadc = (UInt16)(revbuf[6] * 256 + revbuf[7]);
            for (i = 0; i < RxAdcTable.Length - 1; i++)
            {
                if ((Rxadc >= RxAdcTable[i]) && (Rxadc <= RxAdcTable[i + 1]))
                {
                    break;
                }
            }

            i = i - 25;
            //            int[] txpower = new int[1];

            //ReaderParams.GetTxPower(txpower);
            //i = txpower[0] - i;

            if (i >= 10)
            {
                tB_RevLoss.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                tB_RevLoss.ForeColor = System.Drawing.SystemColors.WindowText;
            }

            tB_RevLoss.Text = i.ToString();
        }

        private void bt_SetInvTimeOut_Click(object sender, EventArgs e)
        {
            //寻卡超时时间

            if (textBox_InvTimeOut.Text == "")
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
            ReaderParams.InvTimeOut = Convert.ToUInt16(textBox_InvTimeOut.Text);
        }

        private void bt_SetCWOn_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            byte cwflag = 0;
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            cwflag = 1;

            WriteBuf[0] = cwflag;
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_CW_STATUE, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_CW_STATUE, len);
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
            //            MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_CW_STATUE_RSP) 
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        private void bt_TestCWSetOn_Click(object sender, EventArgs e)
        {
            UInt16 len = 1;
            Byte[] WriteBuf = new Byte[100];
            byte cwflag = 0;
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            cwflag = 1;

            WriteBuf[0] = cwflag;
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_TEST_SET_CW_STATUE, len);
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_TEST_SET_CW_STATUE, len);
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
            //            MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //        }
            //        else
            //        {
            //            MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                && (revbuf[4] == CMD.FRAME_CMD_TEST_SET_CW_STATUE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("CW操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("CW operate Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
        }

        public int SetQValue()
        {
            int result = 0;
            byte[] buf = new byte[8];
            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            buf[0] &= 0xFE;
            buf[1] &= 0x00;
            buf[2] &= 0x0F;

            UInt16 InventoryAlg = (UInt16)(comboBox_InventoryAlg.SelectedIndex);
            UInt16 startQ = Convert.ToUInt16(textBox_startQ.Text);
            UInt16 minQ = Convert.ToUInt16(textBox_minQ.Text);
            UInt16 maxQ = Convert.ToUInt16(textBox_maxQ.Text);

            buf[0] |= (byte)((InventoryAlg) & 0x01);
            buf[1] |= (byte)((startQ << 4) & 0xF0);
            buf[1] |= (byte)((minQ) & 0x0F);
            buf[2] |= (byte)((maxQ << 4) & 0xF0);

            if(cB_SelQSaveflag .Checked ==true)
            {
                buf[7] |= 0x01;
            }
            else
            {
                buf[7] &= 0xFE;
            }
            result = ReaderParams.SetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            return 0;
        }

        public int GetQValue(UInt16[] bt)
        {
            int result = 0;
            byte[] buf = new byte[8];
            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            bt[0] = buf[1];
            bt[0] |= (UInt16)((buf[0] & 0x01) << 8);
            bt[0] |= (UInt16)((buf[2] & 0xF0) << 8);

            return 0;
        }

        private void bt_SetQInfo_Click(object sender, EventArgs e)
        {
            int result;

            /* 判断用户设置的合法性 */
            if ((textBox_startQ.Text.Length == 0) 
                || (textBox_minQ.Text.Length == 0)
                || (textBox_maxQ.Text.Length == 0))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("参数不能为空,请输入Q值", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You mast input Q value", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            int minq = int.Parse(textBox_minQ.Text);
            int maxq = int.Parse(textBox_maxQ.Text);
            int startq = int.Parse(textBox_startQ.Text);
            if ((maxq < minq) || (minq > startq))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("最小Q不能大于最大Q或起始Q", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_minQ.Text = "0";
                }
                else
                {
                    MessageBox.Show("MinQ mast less than Max Q and Start Q", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_minQ.Text = "0";
                }
                return;
            }

            if ((maxq < minq) || (maxq < startq))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("最大Q不能小于最小Q或起始Q", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_maxQ.Text = "15";
                }
                else
                {
                    MessageBox.Show("Max Q mast greater than Min Q and Start Q", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_maxQ.Text = "15";
                }
                return;
            }

            /* 设置Q值 */
            result = SetQValue();
            if (result != 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("设置Q值出错", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Q value error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            ReaderParams.InventoryAlg = (UInt16)(comboBox_InventoryAlg.SelectedIndex);
            ReaderParams.startQ = Convert.ToUInt16(textBox_startQ.Text);
            ReaderParams.minQ = Convert.ToUInt16(textBox_minQ.Text);
            ReaderParams.maxQ = Convert.ToUInt16(textBox_maxQ.Text);
        }

        private void bt_GetQInfo_Click(object sender, EventArgs e)
        {
            int result;
            UInt16[] data = new UInt16[1];

            comboBox_InventoryAlg.SelectedIndex = -1;
            textBox_startQ.Text = "";
            textBox_minQ.Text = "";
            textBox_maxQ.Text = "";

            /* 获取Q值 */
            result = GetQValue(data);
            if (result != 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("获取Q值出错", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            comboBox_InventoryAlg.SelectedIndex = (data[0] >> 8) & 0x01;
            ReaderParams.InventoryAlg = (UInt16)comboBox_InventoryAlg.SelectedIndex;
            ReaderParams.startQ = (UInt16)((data[0] >> 4) & 0x0F);
            ReaderParams.minQ = (UInt16)(data[0] & 0x0F);
            ReaderParams.maxQ = (UInt16)((data[0] >> 12) & 0x0F);
            textBox_startQ.Text = Convert.ToString(ReaderParams.startQ);
            textBox_minQ.Text = Convert.ToString(ReaderParams.minQ);
            textBox_maxQ.Text = Convert.ToString(ReaderParams.maxQ);
        }

        public int SetModulateWay()
        {
            int result;
            byte type;
            UInt32 regaddr;
            UInt32 regdata;

            type = 0x00;
            regaddr = 0x00000B67;
            regdata = (UInt32)comboBox_ModulateWay.SelectedIndex;

            result = ReaderParams.Write_Reg_Data(type, regaddr, regdata);
            if (result != 0)
            {
                return -1;
            }

            return 0;
        }

        public int GetModulateWay(UInt16[] way)
        {
            int result;
            byte type;
            UInt32 regaddr;
            UInt32[] regdata = new UInt32[1];

            type = 0x00;
            regaddr = 0x00000B67;

            result = ReaderParams.Read_Reg_Data(type, regaddr, regdata);
            if (result != 0)
            {
                return -1;
            }

            way[0] = (UInt16)regdata[0];
            return 0;
        }

        public int SetLinkFrequency()
        {
            int result = 0;
            byte[] buf = new byte[8];
            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            buf[3] &= 0xF8;
            buf[3] |= (byte)((comboBox_LinkProfile.SelectedIndex) & 0x07);

            result = ReaderParams.SetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            return 0;
        }

        public int GetLinkFrequency(UInt16[] bt)
        {
            int result = 0;
            byte[] buf = new byte[8];
            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return -1;
            }

            bt[0] = (UInt16)(buf[3] & 0x07);

            return 0;
        }

        private void bt_SetLinkFrq_Click(object sender, EventArgs e)
        {
            int result;

            /* 设置调制方式 */
            result = SetModulateWay();
            if (result != 0)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("设置调制方式出错,请确认端口打开", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set modulate mode error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            ReaderParams.ModulateWay = (UInt16)comboBox_ModulateWay.SelectedIndex;

            /* 设置链路频率 */
            result = SetLinkFrequency();
            if (result != 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("设置链路频率出错", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Link Frequency error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            ReaderParams.LinkProfile = (UInt16)comboBox_LinkProfile.SelectedIndex;
        }

        private void bt_GetLinkFrq_Click(object sender, EventArgs e)
        {
            int result;
            UInt16[] data = new UInt16[1];

            /* 清除相关参数 */
            comboBox_ModulateWay.SelectedIndex = -1;
            comboBox_LinkProfile.SelectedIndex = -1;

            /* 获取调制方式 */
            result = GetModulateWay(data);
            if (result != 0)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("获取调制方式出错,请确认端口打开", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Get modulate mode error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            comboBox_ModulateWay.SelectedIndex = data[0];
            ReaderParams.ModulateWay = (UInt16)comboBox_ModulateWay.SelectedIndex;

            /* 获取链路频率 */
            result = GetLinkFrequency(data);
            if (result != 0)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("获取链路频率出错", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Get Link Frequency error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            comboBox_LinkProfile.SelectedIndex = data[0];
            ReaderParams.LinkProfile = (UInt16)comboBox_LinkProfile.SelectedIndex;
        }

        private void comboBox_InventoryAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_InventoryAlg.SelectedIndex == 0)
            {
                /* 固定Q值算法 */
                if (0 == ReaderParams.LanguageFlag)
                {
                    label23.Text = "Q值：";
                }
                else
                {
                    label23.Text = "Q：";
                }
                textBox_maxQ.Enabled = false;
                textBox_minQ.Enabled = false;
            }
            else
            {
                /* 动态Q值算法 */
                if (0 == ReaderParams.LanguageFlag)
                {
                    label23.Text = "起始Q值：";
                }
                else
                {
                    label23.Text = "Start Q：";
                }
                textBox_maxQ.Enabled = true;
                textBox_minQ.Enabled = true;
            }
        }

        private void textBox_startQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的字符不是十六进制数字，也不是backspace，则提示
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

        private void textBox_startQ_TextChanged(object sender, EventArgs e)
        {
            if (textBox_startQ.Text.Length > 0)
            {
                int startq = int.Parse(textBox_startQ.Text);
                if (startq > 15)
                {
                    textBox_startQ.Text = "15";
                }
            }
        }

        private void textBox_minQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的字符不是十六进制数字，也不是backspace，则提示
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

        private void textBox_maxQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的字符不是十六进制数字，也不是backspace，则提示
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

        private void textBox_minQ_TextChanged(object sender, EventArgs e)
        {
            if (textBox_minQ.Text.Length > 0)
            {
                int minq = int.Parse(textBox_minQ.Text);
                if (minq > 15)
                {
                    textBox_minQ.Text = "15";
                }
            }
        }

        private void textBox_maxQ_TextChanged(object sender, EventArgs e)
        {
            if (textBox_maxQ.Text.Length > 0)
            {
                int maxq = int.Parse(textBox_maxQ.Text);
                if (maxq > 15)
                {
                    textBox_maxQ.Text = "15";
                }
            }
        }

        private void bt_SetDefault_Click(object sender, EventArgs e)
        {
            comboBox_Selecttarget.SelectedIndex = 0;
            comboBox_action.SelectedIndex = 0;
            comboBox_MB.SelectedIndex = 0;
            comboBox_trun.SelectedIndex = 0;
            textBox_len.Text = "0";
            textBox_pointer.Text = "32";
            textBox_mask.Text = "00-00-00-00-00-00-00-00-00-00-00-00";

            comboBox_DR.SelectedIndex = 1;
            comboBox_M.SelectedIndex = 2;
            comboBox_TRext.SelectedIndex = 1;
            comboBox_Sel.SelectedIndex = 1;
            comboBox_Session.SelectedIndex = 1;
            comboBox_QuerTarget.SelectedIndex = 0;
        }

        //private void comboBox_MB_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (comboBox_MB.SelectedIndex == 0)
        //    {
        //        textBox_pointer.Text = "32";
        //        textBox_len.Text = "96";
        //    }
        //    else if (comboBox_MB.SelectedIndex == 1)
        //    {
        //        textBox_pointer.Text = "0";
        //        textBox_len.Text = "64";
        //    }
        //    else if (comboBox_MB.SelectedIndex == 2)
        //    {
        //        textBox_pointer.Text = "0";
        //        textBox_len.Text = "64";
        //    }
        //}

        private void bt_SetAirPara_Click(object sender, EventArgs e)
        {
            int result = 0;

            /* 设置调制方式 */
            result = SetSelectQueryPara();
            if (result != 0)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("设置参数出错", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Set Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            
            ReaderParams.Selecttarget = (UInt16)comboBox_Selecttarget.SelectedIndex;
            ReaderParams.action = (UInt16)comboBox_action.SelectedIndex;
            ReaderParams.MB = (UInt16)comboBox_MB.SelectedIndex;
            ReaderParams.trun = (UInt16)comboBox_trun.SelectedIndex;

            ReaderParams.select_len = textBox_len.Text;
            ReaderParams.select_pointer = textBox_pointer.Text;
            ReaderParams.select_mask = textBox_mask.Text;

            ReaderParams.DR = (UInt16)comboBox_DR.SelectedIndex;
            ReaderParams.M = (UInt16)comboBox_M.SelectedIndex;
            ReaderParams.TRext = (UInt16)comboBox_TRext.SelectedIndex;
            ReaderParams.Sel = (UInt16)comboBox_Sel.SelectedIndex;
            ReaderParams.Session = (UInt16)comboBox_Session.SelectedIndex;
            ReaderParams.QuerTarget = (UInt16)comboBox_QuerTarget.SelectedIndex;
        }

        private void bt_GetAirPara_Click(object sender, EventArgs e)
        {
            int result = 0;
            byte[] buf = new byte[8];

            /* 清除要获取的信息初值 */
            comboBox_Selecttarget.SelectedIndex = -1;
            comboBox_action.SelectedIndex = -1;
            comboBox_MB.SelectedIndex = -1;
            comboBox_trun.SelectedIndex = -1;

            comboBox_DR.SelectedIndex = -1;
            comboBox_M.SelectedIndex = -1;
            comboBox_TRext.SelectedIndex = -1;
            comboBox_Sel.SelectedIndex = -1;
            comboBox_Session.SelectedIndex = -1;
            comboBox_QuerTarget.SelectedIndex = -1;

            /* 获取Gen2参数 */
            result = ReaderParams.GetGen2Parameter(buf);
            if (result != 0)
            {
                return;
            }

            ReaderParams.Selecttarget = (UInt16)((buf[0] >> 5) & 0x07);
            ReaderParams.action = (UInt16)((buf[0] >> 2) & 0x07);
            ReaderParams.trun = (UInt16)((buf[0] >> 1) & 0x01);

            ReaderParams.DR = (UInt16)((buf[2] >> 3) & 0x01);
            ReaderParams.M = (UInt16)((buf[2] >> 1) & 0x03);
            ReaderParams.TRext = (UInt16)((buf[2]) & 0x01);
            ReaderParams.Sel = (UInt16)((buf[3] >> 6) & 0x03);
            ReaderParams.Session = (UInt16)((buf[3] >> 4) & 0x03);
            ReaderParams.QuerTarget = (UInt16)((buf[3] >> 3) & 0x01);

            comboBox_Selecttarget.SelectedIndex = ReaderParams.Selecttarget;
            comboBox_action.SelectedIndex = ReaderParams.action;
            comboBox_MB.SelectedIndex = ReaderParams.MB;
            comboBox_trun.SelectedIndex = ReaderParams.trun;

            comboBox_DR.SelectedIndex = ReaderParams.DR;
            comboBox_M.SelectedIndex = ReaderParams.M;
            comboBox_TRext.SelectedIndex = ReaderParams.TRext;
            comboBox_Sel.SelectedIndex = ReaderParams.Sel;
            comboBox_Session.SelectedIndex = ReaderParams.Session;
            comboBox_QuerTarget.SelectedIndex = ReaderParams.QuerTarget;
        }

        private void bt_SetSelectMaskData_Click(object sender, EventArgs e)
        {
            UInt16      len = 1;
            Byte[]      WriteBuf = new Byte[100];
            int         recount = 50000;     //重试次数
            int         revlen = 0;         //接收数据长度
            Byte[]      revbuf = new Byte[500];           //接收缓冲
            UInt16      cur = 0;
            string      str;
            UInt16      tmp;
            byte[]      bytetmp = new byte[500];
            byte[]      bytetmp_t = new byte[500];
            int         i;

            label27.Text = "";
            if (true == cB_SelMaskSaveflag.Checked)
            {
                WriteBuf[cur++] = 0x01;
            }
            else
            {
                WriteBuf[cur++] = 0x00;
            }
            WriteBuf[cur++] = (byte)(comboBox_MB.SelectedIndex+1);
            str = textBox_pointer.Text;                                                     // MSA
            tmp = UInt16.Parse(str);
            WriteBuf[cur++] = (byte)(tmp >> 8);
            WriteBuf[cur++] = (byte)(tmp & 0xFF);
            str = textBox_len.Text;                                                         // MDL
            tmp = UInt16.Parse(str);
            WriteBuf[cur++] = (byte)(tmp >> 8);
            WriteBuf[cur++] = (byte)(tmp & 0xFF);
            if (0 != tmp)
            {
                bytetmp = Encoding.Default.GetBytes(textBox_mask.Text);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        label27.Text = "Mask Data长度不够";
                    }
                    else
                    {
                        label27.Text = "Mask data length error";
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    WriteBuf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }                 
            }

            len = cur;
            int result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_SELECTMASKRULE, len);
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
            //ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_SELECTMASKRULE, len);

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
            //            label27.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label27.Text = "set failed";
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
            //            label27.Text = "设置失败";
            //        }
            //        else
            //        {
            //            label27.Text = "set failed";
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
            if (!( (revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) 
                && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_SELECTMASKRULE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label27.Text = "设置失败";
                }
                else
                {
                    label27.Text = "set failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label27.Text = "设置成功";
            }
            else
            {
                label27.Text = "set ok";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UInt16 len = 11;    
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
                
                WriteBuf[0] = 0x00;
                if(cB_SLSaveflag .Checked ==true)
                {
                   WriteBuf[1] = 0x01;
                }
                else
                {
                   WriteBuf[1] = 0x00;
                }

                if (cB_Soundflag .Checked == true)
                {
                    WriteBuf[2] = 0x01;
                }
                else
                {
                    WriteBuf[2] = 0x00;
                }

                if (cB_Lightflag .Checked  == true)
                {
                    WriteBuf[3] = 0x01;
                }
                else
                {
                    WriteBuf[3] = 0x00;
                }

                //int a = (UInt16)(double.Parse(tB_SoundDelay.Text))>>8;
                //int b = (UInt16)(double.Parse(tB_SoundDelay.Text))&0xff;
                //WriteBuf[4] = (byte)a;
                //WriteBuf[5] = (byte)b;

                ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SOUNDLIGHT_STATUE, len);

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
                            label28.Text = "设置失败";
                        }
                        else
                        {
                            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            label28.Text = "Set Failed";
                        }

                        return;
                    }

                    while ((revlen < 0x09) && (recount != 0))
                    {
                        recount--;
                        revlen = ReadWriteIO.comm.BytesToRead;
                    }

                    if (recount == 0)       //未收到数据
                    {
                        if (0 == ReaderParams.LanguageFlag)
                        {
                            label28.Text = "设置失败";
                        }
                        else
                        {
                            label28.Text = "Set Failed";
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
                        return;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    }
                }


                if (recount == 0)       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("未收到数据");
                    }
                    else
                    {
                        MessageBox.Show("Set Failed");
                    }
                    //MessageBox.Show("未收到数据");
                    return;
                }
                else
                {
                    revlen = ReadWriteIO.comm.BytesToRead;
                    ReadWriteIO.comm.Read(revbuf, 0, revlen);
                }
                //判断是否设置成功
                if (!(    (revbuf[0] == CMD.FRAME_HEAD_FIRST)
                       && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                       && (revbuf[2] == 0x00) 
                       && (revbuf[3] == 0x0B)
                       && (revbuf[4] == CMD.FRAME_CMD_SOUNDLIGHT_STATUE_RSP)
                       && (revbuf[5] == 0x01)
                       && (revbuf[6] == 0x01)
                        ))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("设置灯声光提示模式", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Set Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }
                else
                {
                    
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("设置声光提示模式成功");
                    }
                    else
                    {
                        MessageBox.Show("Ligrht & Sound Set OK");
                    }
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        label28.Text = "设置成功";
                    }
                    else
                    {
                        label28.Text = "OK";
                    }
                }
            
 
        }

        private void soundget_Click(object sender, EventArgs e)
        {
            UInt16 len = 3;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = 0x02;


            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SOUNDLIGHT_STATUE, len);




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
                        label28.Text = "获取失败";
                    }
                    else
                    {
                        MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        label28.Text = "Get Failed";
                    }

                    return;
                }

                while ((revlen < 0x0B) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        label28.Text = "获取失败";
                    }
                    else
                    {
                        label28.Text = "Get Failed";
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
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                }
            }


            if (recount == 0)       //未收到数据
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("未收到数据");
                }
                else
                {
                    MessageBox.Show("Get Failed");
                }
                return;
            }
            else
            {
                revlen = ReadWriteIO.comm.BytesToRead;
                ReadWriteIO.comm.Read(revbuf, 0, revlen);
            }
            //判断是否设置成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                   && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                   && (revbuf[2] == 0x00)
                   && (revbuf[3] == 0x13)
                   && (revbuf[4] == CMD.FRAME_CMD_SOUNDLIGHT_STATUE_RSP)
                   && (revbuf[5] == 0x03)
                   && (revbuf[6] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("获取声音工作模式失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Get Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }
            else
            {
                //if (revbuf[7] == 0x01)
                //{
                //    cB_SLSaveflag.Checked = true;
                //}
                //else 
                //{
                //    cB_SLSaveflag.Checked = false;
                //}

                if (revbuf[7] == 0x01)
                {
                    cB_Soundflag.Checked = true;
                }
                else
                {
                    cB_Soundflag.Checked = false;
                }

                if (revbuf[8] == 0x01)
                {
                    cB_Lightflag.Checked = true;
                }
                else
                {
                    cB_Lightflag.Checked =  false;
                }
                //tB_SoundDelay.Text = ((float)(revbuf[10] * 256 + revbuf[11])).ToString();
                 if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("获取声光提示模式成功");
                    }
                    else
                    {
                        MessageBox.Show("Ligrht & Sound GET OK");
                    }
                 if (0 == ReaderParams.LanguageFlag)
                 {
                     label28.Text = "获取成功";
                 }
                 else
                 {
                     label28.Text = "OK";
                 }
                }

            
        }

        
        
   }
}

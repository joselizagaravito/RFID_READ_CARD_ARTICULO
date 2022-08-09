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
    public partial class TagOperate : Form
    {
        public TagOperate()
        {
            //frm1 = frm;
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            comboBox_MB.SelectedIndex = 1;
            comboBox_MMB.SelectedIndex = 0;
            textBox_Mstartadd.Text = "32";
            textBox_Mlength.Text = "96";
            textBox_startadd.Text = "2";
            textBox_length.Text = "6";
            textBox_data.Text = "";
            textBox_accpwd.Text = "00000000";
            label40.Text = "";
            textBox_accforlock.Text = "00000000";
            textBox_lockdata.Text = "00000000000000000000";

            textBox_killpwd.Text = "00000000";

            tB_SelectTagID.Text = ReaderParams.select_TagID;

            if (tB_SelectTagID.Text.Length == 0)
            {
                checkBox_Choose.Checked = false;
                UInt32 RegAddr = 0;
                UInt32[] RegData = new UInt32[1];
                RegAddr = 0x911;
                RegData[0] = 0;

                ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

                ReaderParams.FilterFlag = 0;
            }
            else
            {                
                ReaderParams.select_mask = ReaderParams.select_TagID;
                ReaderParams.select_len = ((ReaderParams.select_TagID.Length + 1) / 6).ToString();
                checkBox_Choose.Checked = true;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                Text                            = "标签操作";

                groupBox1.Text                  = "读/写操作";
                groupBox2.Text                  = "锁定操作";
                groupBox3.Text                  = "kill操作";
                groupBox4.Text                  = "过滤设置";

                label19.Text                    = "标签ID号:";
                label2.Text                     = "起始地址:(字)";
                label3.Text                     = "长度:(字)";
                label4.Text                     = "access password:(十六进制)";
                label16.Text                    = "启用过滤";
                label5.Text                     = "数据:(十六进制)";
                label18.Text                    = "字节";
                label7.Text                     = "access password:(十六进制)";
                label6.Text                     = "Kill password:(十六进制)";
                label17.Text                    = "注意：Kill掉的Tag无法恢复";
                label21.Text                    = "起始地址:(bit)";
                label22.Text                    = "长度:(bit)";

                button_read.Text                = "读操作";
                button_write.Text               = "写操作";
                bt_Clean.Text                   = "清除";
                bt_QTRead.Text                  = "QT读操作";
                bt_QTWrite.Text                 = "QT写操作";
                label24.Text                    = "距离控制";
                bt_QTParaSet.Text               = "设置";
                bt_QTParaGet.Text               = "获取";
                bt_Blockperset.Text             = "设置";
                bt_Untraceableset.Text          = "设置";
                bt_AuthenticateSet.Text         = "设置";
            }
            else
            {
                Text                            = "Tag Operate";

                groupBox1.Text                  = "Read/Write";
                groupBox2.Text                  = "Lock/Unlock";
                groupBox3.Text                  = "kill";
                groupBox4.Text                  = "Mask Setting";

                label19.Text                    = "Tag ID:";
                label2.Text                     = "Start Addr:(Words)";
                label3.Text                     = "Length:(Words)";
                label4.Text                     = "access password:(Hex)";
                label16.Text                    = "Start Filter";
                label5.Text                     = "Data:(Hex)";
                label18.Text                    = "Byte";
                label7.Text                     = "access password:(Hex)";
                label6.Text                     = "Kill password:(Hex)";
                label17.Text                    = "";
                label21.Text                    = "Start Addr:(bit)";
                label22.Text                    = "Length:(bit)";

                button_read.Text                = "Read";
                button_write.Text               = "Write";
                bt_Clean.Text                   = "Clean";
                bt_QTRead.Text                  = "QT Read";
                bt_QTWrite.Text                 = "QT Write";

                label24.Text                    = "Range Control";
                bt_QTParaSet.Text               = "Set";
                bt_QTParaGet.Text               = "Get";
                bt_Blockperset.Text             = "Set";
                bt_Untraceableset.Text          = "Set";
                bt_AuthenticateSet.Text         = "Set";
                cB_RangeCt.Location             = new System.Drawing.Point(90, 12); 
            }

            cB_RangeCt.SelectedIndex            = 1;
            cB_MemoryMap.SelectedIndex          = 1;
            lb_QTResult.Text                    = "";
            lb_BlockPerRt.Text                  = "";
            cB_BlockperRL.SelectedIndex         = 0;
            cB_BlockperMB.SelectedIndex         = 3;
            tB_BlockperPtr.Text                 = "0";
            tB_BlockperRange.Text               = "1";
            tB_BlockperMask.Text                = "F000";

            cB_UntraceableU.SelectedIndex = 0; 
            cB_UntraceableTID.SelectedIndex = 0;
            cB_UntraceableUser.SelectedIndex = 0;
            cB_UntraceableRange.SelectedIndex = 0;
            cB_AuthenticateSenRep.SelectedIndex = 0;
            cB_AuthenticateIncRepLen.SelectedIndex = 0;
            tB_UntraceableEPC.Text = "0";
            tB_AuthenticateCSI.Text = "0";
            tB_AuthenticateCSI.Text = "0";

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
        private void button_read_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_ReadWriteResult.Text = "";
            textBox_data.Text = "";
            lB_NumOfData.Text = (0).ToString();

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++ )
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2*i+1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;                    
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length+1)/3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for(i=4;i<9;i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* Memory Bank */
            buf[cur++] = (byte)(comboBox_MB.SelectedIndex);
            /* 起始地址 */
            str = textBox_startadd.Text;
            tmp = UInt16.Parse(str);
            buf[cur++] = (byte)(tmp >> 8);
            buf[cur++] = (byte)(tmp & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);

            len = cur;
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
                result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_READ_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
                return ;
            }
            revbuf = g_Revbuf;
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_READ_DATA, len);

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
            //            lb_ReadWriteResult.Text = "读取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Read Failed";
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
            //            lb_ReadWriteResult.Text = "读取失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Read Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }  
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST) 
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) 
                && (revbuf[4] == CMD.FRAME_CMD_READ_DATA_RSP) 
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "读取失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Read Failed";
                }
                return;
            }

            datalength = (UInt16)(revbuf[7]*256 + revbuf[8]);

            for (i = 0; i < datalength * 2; i++ )
            {
                textBox_data.Text += revbuf[9 + i].ToString("X2");
                if (i < datalength * 2 - 1)
                {
                    textBox_data.Text += "-";
                }
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "读取成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Read OK";
            }
            lB_NumOfData.Text = (datalength * 2).ToString();
        }

        private void textBox_accpwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18) 
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
        }

        private void textBox_accforlock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18)
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
        }

        private void textBox_killpwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18)
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
        }

        private void textBox_data_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18)
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

            lB_NumOfData.Text = ((textBox_data.Text.Length + 2) / 3).ToString();
            if ((e.KeyChar == 8) || (e.KeyChar == 3) || (e.KeyChar == 0x16))
            {
                return;
            }

            if ((textBox_data.Text.Length % 3) == 2)
            {
                textBox_data.Text += "-";
                this.textBox_data.SelectionStart = this.textBox_data.Text.Length;
            }            
        }

        private void textBox_startadd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18))
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
        }

        private void checkBox_mask9_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask9.Checked == true)
            {
                bytetmp[0] = (byte)'1';
            }
            else
            {
                bytetmp[0] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask8_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask8.Checked == true)
            {
                bytetmp[1] = (byte)'1';
            }
            else
            {
                bytetmp[1] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask7_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask7.Checked == true)
            {
                bytetmp[2] = (byte)'1';
            }
            else
            {
                bytetmp[2] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask6_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask6.Checked == true)
            {
                bytetmp[3] = (byte)'1';
            }
            else
            {
                bytetmp[3] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask5_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask5.Checked == true)
            {
                bytetmp[4] = (byte)'1';
            }
            else
            {
                bytetmp[4] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask4_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask4.Checked == true)
            {
                bytetmp[5] = (byte)'1';
            }
            else
            {
                bytetmp[5] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask3_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask3.Checked == true)
            {
                bytetmp[6] = (byte)'1';
            }
            else
            {
                bytetmp[6] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask2_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask2.Checked == true)
            {
                bytetmp[7] = (byte)'1';
            }
            else
            {
                bytetmp[7] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask1_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask1.Checked == true)
            {
                bytetmp[8] = (byte)'1';
            }
            else
            {
                bytetmp[8] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_mask0_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_mask0.Checked == true)
            {
                bytetmp[9] = (byte)'1';
            }
            else
            {
                bytetmp[9] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action9_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action9.Checked == true)
            {
                bytetmp[10] = (byte)'1';
            }
            else
            {
                bytetmp[10] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action8_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action8.Checked == true)
            {
                bytetmp[11] = (byte)'1';
            }
            else
            {
                bytetmp[11] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action7_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action7.Checked == true)
            {
                bytetmp[12] = (byte)'1';
            }
            else
            {
                bytetmp[12] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action6_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action6.Checked == true)
            {
                bytetmp[13] = (byte)'1';
            }
            else
            {
                bytetmp[13] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action5_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action5.Checked == true)
            {
                bytetmp[14] = (byte)'1';
            }
            else
            {
                bytetmp[14] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action4_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action4.Checked == true)
            {
                bytetmp[15] = (byte)'1';
            }
            else
            {
                bytetmp[15] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action3_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action3.Checked == true)
            {
                bytetmp[16] = (byte)'1';
            }
            else
            {
                bytetmp[16] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action2_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action2.Checked == true)
            {
                bytetmp[17] = (byte)'1';
            }
            else
            {
                bytetmp[17] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action1_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action1.Checked == true)
            {
                bytetmp[18] = (byte)'1';
            }
            else
            {
                bytetmp[18] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void checkBox_action0_CheckedChanged(object sender, EventArgs e)
        {
            string str;
            byte[] bytetmp = new byte[20];

            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);

            if (checkBox_action0.Checked == true)
            {
                bytetmp[19] = (byte)'1';
            }
            else
            {
                bytetmp[19] = (byte)'0';
            }

            textBox_lockdata.Text = System.Text.Encoding.Default.GetString(bytetmp);

        }

        private void button_write_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 500000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[2048];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[2048];
            byte[] bytetmp_t = new byte[2048];
            int i;

            lb_ReadWriteResult.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }

                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* Memory Bank */
            buf[cur++] = (byte)comboBox_MB.SelectedIndex;
            /* 起始地址 */
            str = textBox_startadd.Text;
            UInt16 startaddr = UInt16.Parse(str);
            buf[cur++] = (byte)(startaddr >> 8);
            buf[cur++] = (byte)(startaddr & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            //if (datalength > 32)
            //{
            //    if (0 == ReaderParams.LanguageFlag)
            //    {
            //        MessageBox.Show("写数据长度不能超过32个字", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        lb_ReadWriteResult.Text = "写入失败";
            //    }
            //    else
            //    {
            //        MessageBox.Show("Write data can not be more than 32 words in length", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    return;
            //}
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);
            /* 数据 */
            str = textBox_data.Text;
            if ((datalength * 2 * 3 - 1) > str.Length)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("数据长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lb_ReadWriteResult.Text = "Write error";
                }
                return;
            }

            bytetmp = Encoding.Default.GetBytes(str);
            for ( i = 0; i < str.Length; i++)
            {
                if ((bytetmp[i] >= '0') && (bytetmp[i] <= '9'))
                {
                }
                else if ((bytetmp[i] >= 'A') && (bytetmp[i] <= 'F'))
                {
                }
                else if (bytetmp[i] == '-')
                {
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("输入的数据格式有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "写操作失败";
                    }
                    else
                    {
                        MessageBox.Show("error data format", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "Write error";
                    }
                    return;
                }
            }
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (datalength*2*3-1));
            for (i = 0; i < datalength * 2; i++ )
            {
                buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
            }

            len = cur;
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_WRITE_DATA, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_WRITE_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //            lb_ReadWriteResult.Text = "写入失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Write Failed";
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
            //            lb_ReadWriteResult.Text = "写入失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Write Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(300);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST) 
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FARAM_CMD_WRITE_DATA_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "写入失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Write Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "写入成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Write OK";
            }

            /* 若写入的是EPC数据，则相应的更新selsect命令的过滤数据 */
            if ((comboBox_MB.SelectedIndex == 1) && (checkBox_Choose.Checked == true) && (comboBox_MMB.Text == "EPC"))
            {
                /* 根据起始地址和长度，实时更新 */
                byte[] bufnewid = new byte[500];
                byte[] bufnewid_t = new byte[500];
                bufnewid = Encoding.Default.GetBytes(ReaderParams.select_mask);
                ReaderParams.ByteArrayToUInt8Array(bufnewid, bufnewid_t, ReaderParams.select_mask.Length);
                for (i = 0; i < (ReaderParams.select_mask.Length+1)/3; i++)
                {
                    bufnewid[i] = (byte)((bufnewid_t[3 * i] << 4) + bufnewid_t[3 * i + 1]);
                }

                int min;
                min = ((int.Parse(textBox_length.Text) * 2) > ((ReaderParams.select_mask.Length + 1) / 3)) ? ((ReaderParams.select_mask.Length + 1) / 3) : (int.Parse(textBox_length.Text) * 2);

                for (i = (int.Parse(textBox_startadd.Text) * 2); i < (min+(int.Parse(textBox_startadd.Text) * 2)); i++)
                {
                    if (i>=4)
                    {
                        bufnewid[i - 4] = buf[cur - 4 + i - (int.Parse(textBox_length.Text) * 2) + (4 - int.Parse(textBox_startadd.Text) * 2)];
                    }                    
                }

                ReaderParams.select_TagID = "";
                for (i = 0; i < (ReaderParams.select_mask.Length+1)/3; i++)
                {
                    ReaderParams.select_TagID += bufnewid[i].ToString("X2");
                    if (i < (ReaderParams.select_mask.Length + 1) / 3 - 1)
                    {
                        ReaderParams.select_TagID += "-";
                    }                    
                }

                ReaderParams.select_mask = ReaderParams.select_TagID;
                tB_SelectTagID.Text = ReaderParams.select_TagID;
            }
        }

        private void button_lock_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[50];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[100];
            byte[] bytetmp_t = new byte[100];
            int i;

            lb_LockFlag.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accforlock.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    textBox_data.Text = "access password长度有误";
                }
                else
                {
                    textBox_data.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* LD */
            str = textBox_lockdata.Text;
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (str.Length));
            buf[cur] = 0x00;
            buf[cur++] |= (byte)((bytetmp_t[0] << 3) + (bytetmp_t[1] << 2) + (bytetmp_t[2] << 1) + (bytetmp_t[3]));
            buf[cur] = 0x00;
            buf[cur++] |= (byte)((bytetmp_t[4] << 7) + (bytetmp_t[5] << 6) + (bytetmp_t[6] << 5) + (bytetmp_t[7] << 4)
                             +(bytetmp_t[8]<<3) + (bytetmp_t[9]<<2) + (bytetmp_t[10]<<1) + (bytetmp_t[11]));
            buf[cur] = 0x00;
            buf[cur++] |= (byte)((bytetmp_t[12] << 7) + (bytetmp_t[13] << 6) + (bytetmp_t[14] << 5) + (bytetmp_t[15] << 4)
                             +(bytetmp_t[16]<<3) + (bytetmp_t[17]<<2) + (bytetmp_t[18]<<1) + (bytetmp_t[19]));


            len = cur;
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FARAM_CMD_LOCK_TAG, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FARAM_CMD_LOCK_TAG, len);

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
            //            lb_LockFlag.Text = "操作失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_LockFlag.Text = "Operate Failed";
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
            //            lb_LockFlag.Text = "操作失败";
            //        }
            //        else
            //        {
            //            lb_LockFlag.Text = "Operate Failed";
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

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST) 
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_LOCK_TAG_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_LockFlag.Text = "操作失败";
                }
                else
                {
                    lb_LockFlag.Text = "Operate Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_LockFlag.Text = "操作成功";
            }
            else
            {
                lb_LockFlag.Text = "Operate OK";
            }
        }

        private void bt_Clean_Click(object sender, EventArgs e)
        {
            textBox_data.Text = "";
        }

        private void button_kill_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[50];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[100];
            byte[] bytetmp_t = new byte[100];
            int i;

            tB_KillFlag.Text = "";

            /* 获取KillPwd值 */
            str = textBox_killpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    textBox_data.Text = "Kill password长度有误";
                }
                else
                {
                    textBox_data.Text = "Kill password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }

            len = cur;
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_KILL_TAG, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_KILL_TAG, len);

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
            //            tB_KillFlag.Text = "Kill操作失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            tB_KillFlag.Text = "Kill Failed";
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
            //            tB_KillFlag.Text = "Kill操作失败";
            //        }
            //        else
            //        {
            //            tB_KillFlag.Text = "Kill Failed";
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

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST) 
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_KILL_TAG_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_KillFlag.Text = "Kill操作失败";
                }
                else
                {
                    tB_KillFlag.Text = "Kill Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                tB_KillFlag.Text = "Kill操作成功";
            }
            else
            {
                tB_KillFlag.Text = "Kill OK";
            }
        }

        private void comboBox_MB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_MB.SelectedIndex == 0)
            {
                textBox_startadd.Text = "0";
                textBox_length.Text = "4";
            }
            else if (comboBox_MB.SelectedIndex == 1)
            {
                textBox_startadd.Text = "2";
                textBox_length.Text = "6";
            }
            else if (comboBox_MB.SelectedIndex == 2)
            {
                textBox_startadd.Text = "0";
                textBox_length.Text = "4";
            }
            else
            {
                textBox_startadd.Text = "0";
                textBox_length.Text = "8";
            }
        }

        //private void checkBox_Choose_CheckedChanged(object sender, EventArgs e)
        //{
        //    UInt32 RegAddr = 0;
        //    UInt32[] RegData = new UInt32[1];
        //    byte[] bytetmp = new byte[50];
        //    byte[] bytetmp_t = new byte[50];
            
        //    RegAddr = 0x911;

        //    if (true == checkBox_Choose.Checked)       /* 启用过滤 */
        //    {
        //        RegData[0] = (UInt32)(((ReaderParams.select_mask.Length + 1) / 3 * 8) << 2 | 0x01); 
        //        ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

        //        bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
        //        ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                
        //        RegAddr = 0x912;
        //        RegData[0] = (UInt32)((bytetmp_t[0] << 4) + (bytetmp_t[1]) + (bytetmp_t[3] << 12) + (bytetmp_t[4] << 8)
        //                               + (bytetmp_t[6] << 20) + (bytetmp_t[7] << 16) + (bytetmp_t[9] << 28) + (bytetmp_t[10] << 24));
        //        ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

        //        RegAddr = 0x913;
        //        RegData[0] = (UInt32)((bytetmp_t[11] << 4) + (bytetmp_t[12]) + (bytetmp_t[14] << 12) + (bytetmp_t[15] << 8)
        //                               + (bytetmp_t[17] << 20) + (bytetmp_t[18] << 16) + (bytetmp_t[20] << 28) + (bytetmp_t[21] << 24));
        //        ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

        //        RegAddr = 0x914;
        //        RegData[0] = (UInt32)((bytetmp_t[23] << 4) + (bytetmp_t[24]) + (bytetmp_t[26] << 12) + (bytetmp_t[27] << 8)
        //                               + (bytetmp_t[29] << 20) + (bytetmp_t[30] << 16) + (bytetmp_t[32] << 28) + (bytetmp_t[33] << 24));
        //        ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

        //        ReaderParams.FilterFlag = 1;
        //    }
        //    else/* 不启用过滤 */
        //    {
        //        RegAddr = 0x911;
        //        RegData[0] = 0;
        //        ReaderParams.Write_Reg_Data(0, RegAddr, RegData[0]);

        //        ReaderParams.FilterFlag = 0;
        //    }
        //}

        private void button_Bkwrite_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[2048];
            int recount = 500000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[2048];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[2048];
            byte[] bytetmp_t = new byte[2048];
            int i;

            lb_ReadWriteResult.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }

                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* Memory Bank */
            buf[cur++] = (byte)comboBox_MB.SelectedIndex;
            /* 起始地址 */
            str = textBox_startadd.Text;
            UInt16 startaddr = UInt16.Parse(str);
            buf[cur++] = (byte)(startaddr >> 8);
            buf[cur++] = (byte)(startaddr & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);
            /* 数据 */
            str = textBox_data.Text;
            if ((datalength * 2 * 3 - 1) > str.Length)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("数据长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            bytetmp = Encoding.Default.GetBytes(str);
            for (i = 0; i < str.Length; i++)
            {
                if ((bytetmp[i] >= '0') && (bytetmp[i] <= '9'))
                {
                }
                else if ((bytetmp[i] >= 'A') && (bytetmp[i] <= 'F'))
                {
                }
                else if (bytetmp[i] == '-')
                {
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("输入的数据格式有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "Block write失败";
                    }
                    else
                    {
                        MessageBox.Show("error data format", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "Block Write error";
                    }
                    return;
                }
            }
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (datalength * 2 * 3 - 1));
            for (i = 0; i < datalength * 2; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
            }

            len = cur;
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_BLOCK_WRITE_DATA, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_BLOCK_WRITE_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //            lb_ReadWriteResult.Text = "Block write失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Block Write Failed";
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
            //            lb_ReadWriteResult.Text = "Block write失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Block Write Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(300);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_BLOCK_WRITE_DATA_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "Block write失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Block Write Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "Block write 成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Block Write OK";
            }

            ///* 若写入的是EPC数据，则相应的更新selsect命令的过滤数据 */
            //if ((comboBox_MB.SelectedIndex == 1) && (checkBox_Choose.Checked == true))
            //{
            //    /* 根据起始地址和长度，实时更新 */
            //    byte[] bufnewid = new byte[50];
            //    byte[] bufnewid_t = new byte[50];
            //    bufnewid = Encoding.Default.GetBytes(ReaderParams.select_mask);
            //    ReaderParams.ByteArrayToUInt8Array(bufnewid, bufnewid_t, ReaderParams.select_mask.Length);
            //    for (i = 0; i < (ReaderParams.select_mask.Length + 1) / 3; i++)
            //    {
            //        bufnewid[i] = (byte)((bufnewid_t[3 * i] << 4) + bufnewid_t[3 * i + 1]);
            //    }

            //    for (i = (int.Parse(textBox_startadd.Text) * 2); i < ((ReaderParams.select_mask.Length + 1) / 6 + int.Parse(textBox_startadd.Text)) * 2; i++)
            //    {
            //        if (i >= 4)
            //        {
            //            bufnewid[i - 4] = buf[23 + i - (int.Parse(textBox_startadd.Text) * 2)];
            //        }
            //    }

            //    ReaderParams.select_TagID = "";
            //    for (i = 0; i < (ReaderParams.select_mask.Length + 1) / 3; i++)
            //    {
            //        ReaderParams.select_TagID += bufnewid[i].ToString("X2");
            //        if (i < (ReaderParams.select_mask.Length + 1) / 3 - 1)
            //        {
            //            ReaderParams.select_TagID += "-";
            //        }
            //    }

            //    ReaderParams.select_mask = ReaderParams.select_TagID;
            //    tB_SelectTagID.Text = ReaderParams.select_TagID;
            //}
        }

        private void button_Bkerase_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 500000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_ReadWriteResult.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }

                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* Memory Bank */
            buf[cur++] = (byte)comboBox_MB.SelectedIndex;
            /* 起始地址 */
            str = textBox_startadd.Text;
            UInt16 startaddr = UInt16.Parse(str);
            buf[cur++] = (byte)(startaddr >> 8);
            buf[cur++] = (byte)(startaddr & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);

            len = cur;
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_BLOCK_ERASE_DATA, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_BLOCK_ERASE_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //            lb_ReadWriteResult.Text = "Block erase失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Block erase Failed";
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
            //            lb_ReadWriteResult.Text = "Block erase失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Block erase Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(300);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_BLOCK_ERASE_DATA_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "Block erase失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Block erase Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "Block erase 成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Block erase OK";
            }

            ///* 若写入的是EPC数据，则相应的更新selsect命令的过滤数据 */
            //if ((comboBox_MB.SelectedIndex == 1) && (checkBox_Choose.Checked == true))
            //{
            //    /* 根据起始地址和长度，实时更新 */
            //    byte[] bufnewid = new byte[50];
            //    byte[] bufnewid_t = new byte[50];
            //    bufnewid = Encoding.Default.GetBytes(ReaderParams.select_mask);
            //    ReaderParams.ByteArrayToUInt8Array(bufnewid, bufnewid_t, ReaderParams.select_mask.Length);
            //    for (i = 0; i < (ReaderParams.select_mask.Length + 1) / 3; i++)
            //    {
            //        bufnewid[i] = (byte)((bufnewid_t[3 * i] << 4) + bufnewid_t[3 * i + 1]);
            //    }

            //    for (i = (int.Parse(textBox_startadd.Text) * 2); i < ((ReaderParams.select_mask.Length + 1) / 6 + int.Parse(textBox_startadd.Text)) * 2; i++)
            //    {
            //        if (i >= 4)
            //        {
            //            bufnewid[i - 4] = buf[23 + i - (int.Parse(textBox_startadd.Text) * 2)];
            //        }
            //    }

            //    ReaderParams.select_TagID = "";
            //    for (i = 0; i < (ReaderParams.select_mask.Length + 1) / 3; i++)
            //    {
            //        ReaderParams.select_TagID += bufnewid[i].ToString("X2");
            //        if (i < (ReaderParams.select_mask.Length + 1) / 3 - 1)
            //        {
            //            ReaderParams.select_TagID += "-";
            //        }
            //    }

            //    ReaderParams.select_mask = ReaderParams.select_TagID;
            //    tB_SelectTagID.Text = ReaderParams.select_TagID;
            //}
        }

        private void textBox_Mstartadd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a') && (e.KeyChar <= 'f'))
            {
                e.KeyChar = (char)(e.KeyChar + ('A' - 'a'));
            }

            //如果输入的字符不是十六进制数字，也不是backspace，则提示
            if ((!((e.KeyChar >= '0') && (e.KeyChar <= '9')))
                && (e.KeyChar != 8) && (e.KeyChar != 3) && (e.KeyChar != 0x16) && (e.KeyChar != 0x18))
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
        }

        private void bt_QTParaSet_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_QTResult.Text            = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }

            /* QTData */
            buf[cur] = 0x00;
            if (1 == cB_RangeCt.SelectedIndex)                  // Reduces range
            {
                buf[cur]        |= 0x01;
            }

            if (1 == cB_MemoryMap.SelectedIndex)                // Public memory map
            {
                buf[cur]        |= 0x02;
            }

            len = (UInt16)(cur + 1);
            //len = cur;
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_SET_QT_COMMAND, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_SET_QT_COMMAND, len);

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
            //            lb_QTResult.Text = "设置失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_QTResult.Text = "Set Failed";
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
            //            lb_QTResult.Text = "设置失败";
            //        }
            //        else
            //        {
            //            lb_QTResult.Text = "Set Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_SET_QT_COMMAND_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_QTResult.Text = "设置失败";
                }
                else
                {
                    lb_QTResult.Text = "Set Failed";
                }
                return;
            }

             if (0 == ReaderParams.LanguageFlag)
            {
                lb_QTResult.Text = "设置成功";
            }
            else
            {
                lb_QTResult.Text = "Set OK";
            }
        }

        private void bt_QTParaGet_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_QTResult.Text            = "";
            //cB_RangeCt.Text             = "";
            //cB_MemoryMap.Text           = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }

            len = (UInt16)(cur);
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_GET_QT_COMMAND, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_GET_QT_COMMAND, len);

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
            //            lb_QTResult.Text = "获取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_QTResult.Text = "Get Failed";
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
            //            lb_QTResult.Text = "获取失败";
            //        }
            //        else
            //        {
            //            lb_QTResult.Text = "Get Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_GET_QT_COMMAND_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_QTResult.Text = "获取失败";
                }
                else
                {
                    lb_QTResult.Text = "Get Failed";
                }
                return;
            }

            cB_RangeCt.SelectedIndex        = (revbuf[6] & 0x01);
            cB_RangeCt.Refresh();
            cB_MemoryMap.SelectedIndex      = ((revbuf[6]>>1) & 0x01);
            cB_MemoryMap.Refresh();
            //groupBox5.Refresh();
            
            if (0 == ReaderParams.LanguageFlag)
            {
                lb_QTResult.Text = "获取成功";
            }
            else
            {
                lb_QTResult.Text = "Get OK";
            }
        }

        private void bt_QTRead_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_ReadWriteResult.Text = "";
            textBox_data.Text = "";
            lB_NumOfData.Text = (0).ToString();

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            // QTData
            if (0 == cB_RangeCt.SelectedIndex)
            {
                buf[cur++] = (byte)(0x00);
            }
            else
            {
                buf[cur++] = (byte)(0x01);
            }
            /* Memory Bank */
            buf[cur++] = (byte)(comboBox_MB.SelectedIndex);
            /* 起始地址 */
            str = textBox_startadd.Text;
            tmp = UInt16.Parse(str);
            buf[cur++] = (byte)(tmp >> 8);
            buf[cur++] = (byte)(tmp & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);

            len = cur;
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_QT_READ_DATA, len);
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_QT_READ_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //            lb_ReadWriteResult.Text = "读取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Read Failed";
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
            //            lb_ReadWriteResult.Text = "读取失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Read Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_QT_READ_DATA_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "读取失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Read Failed";
                }
                return;
            }

            datalength = (UInt16)(revbuf[7] * 256 + revbuf[8]);

            for (i = 0; i < datalength * 2; i++)
            {
                textBox_data.Text += revbuf[9 + i].ToString("X2");
                if (i < datalength * 2 - 1)
                {
                    textBox_data.Text += "-";
                }
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "读取成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Read OK";
            }
            lB_NumOfData.Text = (datalength * 2).ToString();
        }

        private void bt_QTWrite_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 500000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_ReadWriteResult.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "access password长度有误";
                }
                else
                {
                    lb_ReadWriteResult.Text = "access password length error";
                }

                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            // QTData
            if (0 == cB_RangeCt.SelectedIndex)
            {
                buf[cur++] = (byte)(0x00);
            }
            else
            {
                buf[cur++] = (byte)(0x01);
            }
            /* Memory Bank */
            buf[cur++] = (byte)comboBox_MB.SelectedIndex;
            /* 起始地址 */
            str = textBox_startadd.Text;
            UInt16 startaddr = UInt16.Parse(str);
            buf[cur++] = (byte)(startaddr >> 8);
            buf[cur++] = (byte)(startaddr & 0xFF);
            /* 长度 */
            str = textBox_length.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);
            /* 数据 */
            str = textBox_data.Text;
            if ((datalength * 2 * 3 - 1) > str.Length)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("数据长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            bytetmp = Encoding.Default.GetBytes(str);
            for (i = 0; i < str.Length; i++)
            {
                if ((bytetmp[i] >= '0') && (bytetmp[i] <= '9'))
                {
                }
                else if ((bytetmp[i] >= 'A') && (bytetmp[i] <= 'F'))
                {
                }
                else if (bytetmp[i] == '-')
                {
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("输入的数据格式有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "写操作失败";
                    }
                    else
                    {
                        MessageBox.Show("error data format", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lb_ReadWriteResult.Text = "Write error";
                    }
                    return;
                }
            }
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (datalength * 2 * 3 - 1));
            for (i = 0; i < datalength * 2; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
            }

            len = cur;
            int result = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_QT_WRITE_DATA, len);
            //}
            //else
            //{
            //    result = delay(CMD.TIMEOUT, WriteBuf, 0x0B, len);
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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_QT_WRITE_DATA, len);

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
            //            lb_ReadWriteResult.Text = "写入失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Write Failed";
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
            //            lb_ReadWriteResult.Text = "写入失败";
            //        }
            //        else
            //        {
            //            lb_ReadWriteResult.Text = "Write Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(300);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_QT_WRITE_DATA_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_ReadWriteResult.Text = "写入失败";
                }
                else
                {
                    lb_ReadWriteResult.Text = "Write Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_ReadWriteResult.Text = "写入成功";
            }
            else
            {
                lb_ReadWriteResult.Text = "Write OK";
            }
        }

        private void bt_Blockperset_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            lb_BlockPerRt.Text = "";
   
            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_BlockPerRt.Text = "access password长度有误";
                }
                else
                {
                    lb_BlockPerRt.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /* Read lock */
            buf[cur++] = (byte)(cB_BlockperRL.SelectedIndex);
            /* Memory Bank */
            buf[cur++] = (byte)(cB_BlockperMB.SelectedIndex);
            /* 起始块地址 */
            str = tB_BlockperPtr.Text;
            tmp = UInt16.Parse(str);
            buf[cur++] = (byte)(tmp >> 8);
            buf[cur++] = (byte)(tmp & 0xFF);
            /* 块长度 */
            str = tB_BlockperRange.Text;
            UInt16 datalength = UInt16.Parse(str);
            buf[cur++] = (byte)(datalength >> 8);
            buf[cur++] = (byte)(datalength & 0xFF);

            if (((byte)cB_BlockperRL.SelectedIndex) == 1)
            {
                str = tB_BlockperMask.Text;

                bytetmp = Encoding.Default.GetBytes(str);
                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (datalength*4));
                for (i = 0; i < (datalength * 2); i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
                }
            }

            len = cur;
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_BLOCKPERMALOCK, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_BLOCKPERMALOCK, len);

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
            //            lb_ReadWriteResult.Text = "读取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            lb_ReadWriteResult.Text = "Read Failed";
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
            //            lb_BlockPerRt.Text = "失败";
            //        }
            //        else
            //        {
            //            lb_BlockPerRt.Text = "Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_BLOCKPERMALOCK_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    lb_BlockPerRt.Text = "失败";
                }
                else
                {
                    lb_BlockPerRt.Text = "Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                lb_BlockPerRt.Text = "成功";
            }
            else
            {
                lb_BlockPerRt.Text = "OK";
            }

            if (((byte)cB_BlockperRL.SelectedIndex) == 0)
            {
                tB_BlockperMask.Text = "";
                for (i = 0; i < datalength * 2; i++)
                {
                    tB_BlockperMask.Text += revbuf[7 + i].ToString("X2");
                }
            }
        }

        private void bt_Untraceableset_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            label39.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label39.Text = "access password长度有误";
                }
                else
                {
                    label39.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /*RFU*/
            buf[cur++] = 0x00;
            /* U */
            buf[cur++] = (byte)(cB_UntraceableU .SelectedIndex);
            /* EPC */
            str = tB_UntraceableEPC.Text;
            UInt16 epc = UInt16.Parse(str);
            buf[cur++] = (byte)(epc & 0xFF);

            /* TID */
            buf[cur++] = (byte)(cB_UntraceableTID.SelectedIndex);

            /* User */
            buf[cur++] = (byte)(cB_UntraceableUser.SelectedIndex);

            /* Range */
            buf[cur++] = (byte)(cB_UntraceableRange.SelectedIndex);
            buf[cur++] = 0x00;
            buf[cur++] = 0x00;
            buf[cur++] = 0x00;
            buf[cur++] = 0x00;
            len = cur;
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_UNTRACEABLE, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_UNTRACEABLE, len);

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
            //            label39.Text = "读取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            label39.Text = "Read Failed";
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
            //            label39.Text = "失败";
            //        }
            //        else
            //        {
            //            label39.Text = "Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_UNTRACEABLE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    if (revbuf[6] == 0x01)
                    {
                        label39.Text = "无标签";
                    }
                    else if (revbuf[6] == 0x02)
                    {
                        label39.Text = "访问密码错误";
                    }
                    else if (revbuf[6] == 0x03)
                    {
                        label39.Text = "Untraceable操作失败";
                    }
                }
                else
                {
                    if (revbuf[6] == 0x01)
                    {
                        label39.Text = "无标签";
                    }
                    else if (revbuf[6] == 0x02)
                    {
                        label39.Text = "访问密码错误";
                    }
                    else if (revbuf[6] == 0x03)
                    {
                        label39.Text = "Untraceable操作失败";
                    }
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label39.Text = "成功";
            }
            else
            {
                label39.Text = "OK";
            }
        }

        private void bt_AuthenticateSet_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            UInt16 cur = 0;
            UInt16 tmp;
            byte[] buf = new byte[500];
            int recount = 200000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            byte[] bytetmp = new byte[500];
            byte[] bytetmp_t = new byte[500];
            int i;

            label40.Text = "";

            /* 获取AccPwd值 */
            str = textBox_accpwd.Text;
            if (str.Length != 8)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label40.Text = "access password长度有误";
                }
                else
                {
                    label40.Text = "access password length error";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);
            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, 8);
            for (i = 0; i < 4; i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            /* 过滤操作 */
            if (checkBox_Choose.Checked == true)
            {
                buf[cur++] = (byte)(comboBox_MMB.SelectedIndex + 1);                                // MMB
                str = textBox_Mstartadd.Text;                                                       // MSA
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                str = textBox_Mlength.Text;                                                         // MDL
                tmp = UInt16.Parse(str);
                buf[cur++] = (byte)(tmp >> 8);
                buf[cur++] = (byte)(tmp & 0xFF);
                ReaderParams.select_TagID = tB_SelectTagID.Text;
                ReaderParams.select_mask = ReaderParams.select_TagID;
                bytetmp = Encoding.Default.GetBytes(ReaderParams.select_mask);
                if (((bytetmp.Length + 1) / 3 * 8) < tmp)
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("Mask Data长度不够", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Mask data length error", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, bytetmp.Length);
                for (i = 0; i < (bytetmp.Length + 1) / 3; i++)
                {
                    buf[cur++] = (byte)((bytetmp_t[3 * i] << 4) + bytetmp_t[3 * i + 1]);
                    if (i * 8 > tmp)
                    {
                        break;
                    }
                }
            }
            else
            {/* 不过滤操作 */
                for (i = 4; i < 9; i++)
                {
                    buf[cur++] = 0x00;
                }
                tmp = 0;
            }
            cur = (UInt16)(9 + (tmp / 8));
            if ((tmp % 8) != 0)
            {
                cur++;
            }
            /*RFU*/
            buf[cur++] = 0x00;
            /* SenRep */
            buf[cur++] = (byte)(cB_AuthenticateSenRep .SelectedIndex );
            /* IncRepLen */
            buf[cur++] = (byte)(cB_AuthenticateIncRepLen.SelectedIndex);
            /* SCI */
            str = tB_AuthenticateCSI.Text;
            tmp = UInt16.Parse(str);
            buf[cur++] = (byte)(tmp & 0xFF);

            str = tB_AuthenticateMessagelength.Text;
            
            if(str=="0")
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label40.Text = "Message长度有误";
                }
                else 
                {
                    label40.Text = "Message length fail";
                }
                return;
            }
            try
            {
                tmp = UInt16.Parse(str);
            }
            catch (Exception ex)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label40.Text = "Message长度有误";
                }
                else
                {
                    label40.Text = "Message length fail";
                }
                return;
            }
            buf[cur++] = (byte)(tmp >> 8);
            buf[cur++] = (byte)(tmp & 0xFF);


            str = tB_AuthenticateMessage.Text;
            if (str == "")
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    label40.Text = "Message长度有误";
                }
                else
                {
                    label40.Text = "Message length fail";
                }
                return;
            }
            bytetmp = Encoding.Default.GetBytes(str);

            ReaderParams.ByteArrayToUInt8Array(bytetmp, bytetmp_t, (tmp * 4));
            for (i = 0; i < (tmp * 2); i++)
            {
                buf[cur++] = (byte)((bytetmp_t[2 * i] << 4) + bytetmp_t[2 * i + 1]);
            }
            

            len = cur;
            int result = 0;

            result = delay(CMD.TIMEOUT, buf, CMD.FRAME_CMD_AUTHENTICATE, len);

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
            //ReadWriteIO.sendFrameBuild(buf, CMD.FRAME_CMD_AUTHENTICATE, len);

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
            //            label40.Text = "读取失败";
            //        }
            //        else
            //        {
            //            MessageBox.Show("Do not open the port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            label40.Text = "Read Failed";
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
            //            label40.Text = "失败";
            //        }
            //        else
            //        {
            //            label40.Text = "Failed";
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        System.Threading.Thread.Sleep(300);
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
            //        System.Threading.Thread.Sleep(400);
            //        revlen = ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
            //    }
            //}

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_AUTHENTICATE_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    if (revbuf[6] == 0x01)
                    {
                        label40.Text = "无标签";
                    }
                    else if (revbuf[6] == 0x02)
                    {
                        label40.Text = "访问密码错误";
                    }
                    else if (revbuf[6] == 0x03)
                    {
                        label40.Text = "Untraceable操作失败";
                    }
                }
                else
                {
                    if (revbuf[6] == 0x01)
                    {
                        label40.Text = "No tag";
                    }
                    else if (revbuf[6] == 0x02)
                    {
                        label40.Text = "access password error";
                    }
                    else if (revbuf[6] == 0x03)
                    {
                        label40.Text = "Untraceable set fail";
                    }
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                label40.Text = "成功";
            }
            else
            {
                label40.Text = "OK";
            }
            Authenticatedata.Text="";
            int datalen = revbuf[7] * 256 + revbuf[8];
            for (i = 0; i < datalen; i++) 
            {
                
                Authenticatedata.Text += revbuf[8 + i].ToString("X2");
            }
        }

        private void tB_AuthenticateMessage_TextChanged(object sender, EventArgs e)
        {
            tB_AuthenticateMessagelength.Text = (tB_AuthenticateMessage.Text.Length/4).ToString();
        }
    }
}

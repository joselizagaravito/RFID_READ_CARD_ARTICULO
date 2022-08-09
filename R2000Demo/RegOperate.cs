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
    public partial class RegOperate : Form
    {


        float[] FrequencyTable = new float[50];

        public RegOperate()
        {
            InitializeComponent();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            cB_RegType.SelectedIndex = 0;
            tB_RegAddr.Text = "00000B15";

            if (0 == ReaderParams.LanguageFlag)
            {
                Text                        = "寄存器";
                
                groupBox4.Text              = "工作区";
                groupBox3.Text              = "寄存器";
                groupBox1.Text              = "Error Flag";

                label5.Text                 = "寄存器类型：";
                label6.Text                 = "寄存器地址：";
                label7.Text                 = "寄存器值：";
                label1.Text                 = "错误标志：";

                bt_GetErrorFlag.Text        = "获取";
                bt_ClearErrorFlag.Text      = "清除";
                bt_exit.Text                = "退出";
            }
            else
            {
                Text = "Reg";

                groupBox4.Text              = "Work Regions";
                groupBox3.Text              = "Reg";
                groupBox1.Text              = "Error Flag";

                label5.Text                 = "Reg Type：";
                label6.Text                 = "Addr：";
                label7.Text                 = "Data：";
                label1.Text                 = "Error flag：";

                bt_GetErrorFlag.Text        = "Get";
                bt_ClearErrorFlag.Text      = "Clean";
                bt_exit.Text                = "exit";
            }
        }

        

        private void tB_RegAddr_KeyPress(object sender, KeyPressEventArgs e)
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
                    MessageBox.Show("Please enter a hexadecimal number", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
            }
        }

        private void tB_RegData_KeyPress(object sender, KeyPressEventArgs e)
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
                    MessageBox.Show("Please enter a hexadecimal number", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
            }
        }

        private void bt_RegRead_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            Byte[] bytetmp = new Byte[20];
            int regaddr = 0;
            UInt32[] data =new UInt32[1];
            int result = 0;
            byte len;

            tB_RegData.Text = "";

            bytetmp = Encoding.Default.GetBytes(tB_RegAddr.Text);
            len = (byte)tB_RegAddr.Text.Length;
            if (0 == len)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请先输入寄存器地址", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("please input Reg Address", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            regaddr = ReaderParams.ByteToHexData(bytetmp, len);

            result = ReaderParams.Read_Reg_Data((byte)cB_RegType.SelectedIndex, (UInt32)regaddr, data);
            if (result != 0)
            {

                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("读取寄存器失败,请确认端口已打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Read Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                return;
            }

            tB_RegData.Text = data[0].ToString("X8");
        }

        private void bt_RegWrite_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            Byte[] bytetmp = new Byte[20];
            int regaddr = 0;
            int regdata = 0;
            byte len = 0;
            int result = 0;


            bytetmp = Encoding.Default.GetBytes(tB_RegAddr.Text);
            len = (byte)tB_RegAddr.Text.Length;
            if (0 == len)
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请先输入寄存器地址", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("please input Reg Address", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            regaddr = ReaderParams.ByteToHexData(bytetmp, len);

            bytetmp = Encoding.Default.GetBytes(tB_RegData.Text);
            len = (byte)tB_RegData.Text.Length;
            if (0 == len)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("请输入写入的值", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter a value to be written", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            for (int i = 0; i < len; i++ )
            {
                if ((bytetmp[i] >= '0') && (bytetmp[i] <= '9'))
                {
                }
                else if ((bytetmp[i] >= 'A') && (bytetmp[i] <= 'F'))
                {
                }
                else
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("输入的数据格式有误", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tB_RegData.Text = "写操作失败";
                    }
                    else
                    {
                        MessageBox.Show("Input data format error", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tB_RegData.Text = "Write Failed";
                    }
                    return;
                }
            }

            regdata = ReaderParams.ByteToHexData(bytetmp, len);

            result = ReaderParams.Write_Reg_Data((byte)cB_RegType.SelectedIndex, (UInt32)regaddr, (UInt32)regdata);
            if (result != 0)
            {                
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_RegData.Text = "写操作失败";
                }
                else
                {
                    tB_RegData.Text = "Write Failed";
                }
                return;
            }

            if (0 == ReaderParams.LanguageFlag)
            {
                tB_RegData.Text = "写操作成功";
            }
            else
            {
                tB_RegData.Text = "Write OK";
            }
        }         

        private void cB_RegType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cB_RegType.SelectedIndex == 0)
            {
                tB_RegAddr.Text = "00000B15";
            }
            else if (cB_RegType.SelectedIndex == 1)
            {
                tB_RegAddr.Text = "0000009F";
            }
            else
            {
                tB_RegAddr.Text = "00000000";
            }
        }

        
        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_GetErrorFlag_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            Byte[] bytetmp = new Byte[20];

            tB_ErrorFlag.Text = "";

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_ERRORFLAG, len);

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

                while ((revlen < 0x0B) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_ErrorFlag.Text = "获取失败";
                    }
                    else
                    {
                        tB_ErrorFlag.Text = "Get Failed";
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
                recount     = ReaderParams.Netrecount;
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
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0B)
                && (revbuf[4] == CMD.FRAME_CMD_GET_ERRORFLAG_RSP) 
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_ErrorFlag.Text = "获取失败";
                }
                else
                {
                    tB_ErrorFlag.Text = "Get Failed";
                }
                return;
            }
            UInt32 errFlag;
            errFlag = (UInt32)((revbuf[6] << 8) + revbuf[7]);
            tB_ErrorFlag.Text = errFlag.ToString("X4");
        }

        private void bt_ClearErrorFlag_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            Byte[] bytetmp = new Byte[20];

            tB_ErrorFlag.Text = "";

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_CLEAR_ERRORFLAG, len);

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

                while ((revlen < 0x09) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;
                }

                if (recount == 0)       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        tB_ErrorFlag.Text = "清除失败";
                    }
                    else
                    {
                        tB_ErrorFlag.Text = "Clean Failed";
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
                recount     = ReaderParams.Netrecount;
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
                && (revbuf[4] == CMD.FRAME_CMD_CLEAR_ERRORFLAG_RSP)
                && (revbuf[5] == 0x01)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    tB_ErrorFlag.Text = "清除失败";
                }
                else
                {
                    tB_ErrorFlag.Text = "Clean Failed";
                }
                return;
            }
            
            if (0 == ReaderParams.LanguageFlag)
            {
                tB_ErrorFlag.Text = "清除成功";
            }
            else
            {
                tB_ErrorFlag.Text = "Clean OK";
            }
        }
    }
}

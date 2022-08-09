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
    public partial class PROFRECTESE : Form
    {
        public PROFRECTESE()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(
                   (textBox1 .Text.Trim() !="")
                && (textBox2.Text.Trim() != "")
                && (textBox3.Text.Trim() != "")
                && (double .Parse (textBox1 .Text)>0)
                && (double.Parse(textBox2.Text) > 0)
                && (double.Parse(textBox3.Text) > 0)
                )
            
            {
                richTextBox1.Text = "";
                double startfreq= double .Parse (textBox1 .Text);
                double endfreq = double.Parse(textBox2.Text);
                double step = double.Parse(textBox3.Text);
                int ii = 0;
                for (double i = startfreq; i<=endfreq ;i+=step)
                {
                    ii++;
                    if(ii>50 )
                    {
                        break;
                    }
                    richTextBox1.Text +=i.ToString ("f3")+ ";";
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int ii = 0;
            char[] c = richTextBox1.Text.ToCharArray();
            foreach (char i in c) 
            {
                if (i == ';')
                {
                    ii++;
                }
            }
            label5.Text = ii.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] txpower = new byte[2];
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str = "";
            int i;
            int lastlen = 0;
            UInt16 len = 0;

            richTextBox1.Text = "";

            len = 0;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_FHSS, len);

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
                        //tB_DispFreq.Text = "Get Failed";
                    }
                    return;
                }

                while ((recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;

                    if ((lastlen == revlen) && (lastlen > 4))
                    {
                        break;
                    }

                    lastlen = revlen;

                    if (lastlen > 0)
                        System.Threading.Thread.Sleep(100);
                }

                if (recount == 0)       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("获取失败");
                    }
                    else
                    {
                        MessageBox.Show( "Get Failed");
                    }
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
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
                        //tB_DispFreq.Text = "获取失败";
                    }
                    else
                    {
                        MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //tB_DispFreq.Text = "Get Failed";
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
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_GET_FHSS_RSP)))
            {
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show( "获取失败");
                }
                else
                {
                    MessageBox.Show( "Get Failed");
                }
                return;
            }

            //tB_FreqNum.Text = revbuf[5].ToString();

            for (i = 0; i < revbuf[5]; i++)
            {
                float freq;
                freq = (revbuf[6 + 3 * i] << 16) + (revbuf[6 + 3 * i + 1] << 8) + (revbuf[6 + 3 * i + 2]);
                freq = (float)(freq / 1000.0);
                str += freq.ToString("f3");
                str += ";";
            }
            richTextBox1.Text = str;
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] txpower = new byte[2];
            Byte[] WriteBuf = new Byte[300];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            string str;
            int i;
            int lastlen = 0;
            UInt16 len = 0;
            
            //tB_DispFreq.Text = "";
            i = 0;
            str = label5 .Text;
            WriteBuf[len++] = byte.Parse(str);
            //float[] FrequencyTable = new float[50];
            string[] sArray = richTextBox1.Text.Split(new char[1] { ';' });
            float a = 0; 
            for (i = 0; i < WriteBuf[0]; i++)
            {
                
                a = float.Parse(sArray [i])*1000;
                WriteBuf[len++] = (byte)((UInt32)a >> 16);
                WriteBuf[len++] = (byte)((UInt32)a >> 8);
                WriteBuf[len++] = (byte)((UInt32)a);
            }
            if (checkBox1 .Checked ==true)
            {
                WriteBuf[len++] = 1;
            }
            else
            {
                WriteBuf[len++] = 0;
            }
            //            len = (byte)(3 * WriteBuf[1] + 2);

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_FHSS, len);

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
                        //tB_DispFreq.Text = "Set Failed";
                    }
                    return;
                }
                
                while ((revlen < 9) && (recount != 0))
                {
                    recount--;
                    revlen = ReadWriteIO.comm.BytesToRead;

                    if ((lastlen == revlen) && (lastlen > 4))
                    {
                        break;
                    }

                    lastlen = revlen;

                    if (lastlen > 0)
                        System.Threading.Thread.Sleep(100);
                }

                if ((recount == 0) && (revlen!=0))       //未收到数据
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("设置失败");
                    }
                    else
                    {
                        MessageBox.Show("Set Failed");
                        //tB_DispFreq.Text = "Set Failed";
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
                    //ReaderParams.nsStream.Read(revbuf, 0, revbuf.Length);
                    revlen = 0;
                    ReaderParams.nsStream.Write(ReadWriteIO.SendBuf, 0, (len + CMD.FRAME_HEADEND_LEN));//发送测试信息
                }
                else
                {
                    //if (0 == ReaderParams.LanguageFlag)
                    //{
                    //    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    tB_DispFreq.Text = "设置失败";
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Network port is not connected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    tB_DispFreq.Text = "Set Failed";
                    //}
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("设置失败");
                    }
                    else
                    {
                        MessageBox.Show("Set Failed");
                        //tB_DispFreq.Text = "Set Failed";
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_FHSS_RSP)
                && (revbuf[5] == 0x01)))
            {
                //if (0 == ReaderParams.LanguageFlag)
                //{
                //    tB_DispFreq.Text = "设置失败";
                //}
                //else
                //{
                //    tB_DispFreq.Text = "Set Failed";
                //}
                if (0 == ReaderParams.LanguageFlag)
                {
                    MessageBox.Show("设置失败");
                }
                else
                {
                    MessageBox.Show("Set Failed");
                    //tB_DispFreq.Text = "Set Failed";
                }
                return;
            }

            //if (0 == ReaderParams.LanguageFlag)
            //{
            //    tB_DispFreq.Text = "设置成功";
            //}
            //else
            //{
            //    tB_DispFreq.Text = "Set OK";
            //}
            if (0 == ReaderParams.LanguageFlag)
            {
                MessageBox.Show("设置成功");
            }
            else
            {
                MessageBox.Show("Set OK");
                //tB_DispFreq.Text = "Set Failed";
            }
            //System.Array.Clear(FrequencyTable, 0, FrequencyTable.Length);
        }

        private void PROFRECTESE_Load(object sender, EventArgs e)
        {
            if (0 == ReaderParams.LanguageFlag)
            {
                //tb_MFirmV.Text = "获取失败";
                label1.Text = "初始频率";
                label2.Text = "结束频率";
                label3.Text = "步长";
                label4.Text = "数目";
                label6.Text = "掉电保存";
                button1.Text = "生成";
                button3.Text = "设置";
                button4.Text = "获取";

            }
            else
            {
                //tb_MFirmV.Text = "Get Failed";
                label1.Text = "START";
                label2.Text = "END";
                label3.Text = "STEP";
                label4.Text = "NUM";
                label6.Text = "SAVE";
                button1.Text = "CREAT";
                button3.Text = "SET";
                button4.Text = "GET";
            }
        }
    }
    }


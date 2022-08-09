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
    public partial class Test : Form
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

        public Test()
        {
            InitializeComponent();

        }


        //全选
        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (true == Ck_Chose.Checked)
            {
                //Ck_NoChose.Checked = false;
                Cb_ANT1.Checked = true;
                Cb_ANT2.Checked = true;
                Cb_ANT3.Checked = true;
                Cb_ANT4.Checked = true;
                Cb_ANT5.Checked = true;
                Cb_ANT6.Checked = true;
                Cb_ANT7.Checked = true;
                Cb_ANT8.Checked = true;
                Cb_ANT9.Checked = true;
                Cb_ANT10.Checked = true;
                Cb_ANT11.Checked = true;
                Cb_ANT12.Checked = true;
                Cb_ANT13.Checked = true;
                Cb_ANT14.Checked = true;
                Cb_ANT15.Checked = true;
                Cb_ANT16.Checked = true;
                Cb_ANT17.Checked = true;
                Cb_ANT18.Checked = true;
                Cb_ANT19.Checked = true;
            }
            else
            {
                Cb_ANT1.Checked = false;
                Cb_ANT2.Checked = false;
                Cb_ANT3.Checked = false;
                Cb_ANT4.Checked = false;
                Cb_ANT5.Checked = false;
                Cb_ANT6.Checked = false;
                Cb_ANT7.Checked = false;
                Cb_ANT8.Checked = false;
                Cb_ANT9.Checked = false;
                Cb_ANT10.Checked = false;
                Cb_ANT11.Checked = false;
                Cb_ANT12.Checked = false;
                Cb_ANT13.Checked = false;
                Cb_ANT14.Checked = false;
                Cb_ANT15.Checked = false;
                Cb_ANT16.Checked = false;
                Cb_ANT17.Checked = false;
                Cb_ANT18.Checked = false;
                Cb_ANT19.Checked = false;
            }

        }
        //全不选
        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {

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

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_GPIO, len);

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
                && (revbuf[4] == CMD.FRAME_CMD_SET_GPIO_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -2;
            }

            return 0;
        }





        //设置
        private void Cb_Set_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            if (cB_antSaveFlag.Checked == true)
            {
                WriteBuf[0] = 0x01;
            }
            else
            {
                WriteBuf[0] = 0x00;
            }

            WriteBuf[1] = (Byte)(0);
            WriteBuf[2] = (Byte)(0);
            WriteBuf[3] = (Byte)(0);

            if (0 == ReaderParams.ProtocolFlag)
            {
                if (true == Cb_ANT1.Checked)
                {
                    WriteBuf[2] |= 0x01;
                }
                else
                {
                    WriteBuf[2] &= 0xFE;
                }

                if (true == Cb_ANT2.Checked)
                {
                    WriteBuf[2] |= 0x02;
                }
                else
                {
                    WriteBuf[2] &= 0xFD;
                }

                if (true == Cb_ANT3.Checked)
                {
                    WriteBuf[2] |= 0x04;
                }
                else
                {
                    WriteBuf[2] &= 0xFB;
                }

                if (true == Cb_ANT4.Checked)
                {
                    WriteBuf[2] |= 0x08;
                }
                else
                {
                    WriteBuf[2] &= 0xF7;
                }

                if (true == Cb_ANT5.Checked)
                {
                    WriteBuf[2] |= 0x10;
                }
                else
                {
                    WriteBuf[2] &= 0xEF;
                }

                if (true == Cb_ANT6.Checked)
                {
                    WriteBuf[2] |= 0x20;
                }
                else
                {
                    WriteBuf[2] &= 0xDF;
                }

                if (true == Cb_ANT7.Checked)
                {
                    WriteBuf[2] |= 0x40;
                }
                else
                {
                    WriteBuf[2] &= 0xBF;
                }

                if (true == Cb_ANT8.Checked)
                {
                    WriteBuf[2] |= 0x80;
                }
                else
                {
                    WriteBuf[2] &= 0x7F;
                }

                if (true == Cb_ANT9.Checked)
                {
                    WriteBuf[1] |= 0x01;
                }
                else
                {
                    WriteBuf[1] &= 0xFE;
                }

                if (true == Cb_ANT10.Checked)
                {
                    WriteBuf[1] |= 0x02;
                }
                else
                {
                    WriteBuf[1] &= 0xFD;
                }

                if (true == Cb_ANT11.Checked)
                {
                    WriteBuf[1] |= 0x04;
                }
                else
                {
                    WriteBuf[1] &= 0xFB;
                }

                if (true == Cb_ANT12.Checked)
                {
                    WriteBuf[1] |= 0x08;
                }
                else
                {
                    WriteBuf[1] &= 0xF7;
                }

                if (true == Cb_ANT13.Checked)
                {
                    WriteBuf[1] |= 0x10;
                }
                else
                {
                    WriteBuf[1] &= 0xEF;
                }

                if (true == Cb_ANT14.Checked)
                {
                    WriteBuf[1] |= 0x20;
                }
                else
                {
                    WriteBuf[1] &= 0xDF;
                }

                if (true == Cb_ANT15.Checked)
                {
                    WriteBuf[1] |= 0x40;
                }
                else
                {
                    WriteBuf[1] &= 0xBF;
                }

                if (true == Cb_ANT16.Checked)
                {
                    WriteBuf[1] |= 0x80;
                }
                else
                {
                    WriteBuf[1] &= 0x7F;
                }

                if (true == Cb_ANT17.Checked)
                {
                    WriteBuf[3] |= 0x01;
                }
                else
                {
                    WriteBuf[3] &= 0xFE;
                }

                if (true == Cb_ANT18.Checked)
                {
                    WriteBuf[3] |= 0x02;
                }
                else
                {
                    WriteBuf[3] &= 0xFD;
                }

                if (true == Cb_ANT19.Checked)
                {
                    WriteBuf[3] |= 0x04;
                }
                else
                {
                    WriteBuf[3] &= 0xFB;
                }

                if (true == Cb_ANT20.Checked)
                {
                    WriteBuf[3] |= 0x08;
                }
                else
                {
                    WriteBuf[3] &= 0xF7;
                }

                if (true == Cb_ANT21.Checked)
                {
                    WriteBuf[3] |= 0x10;
                }
                else
                {
                    WriteBuf[3] &= 0xEF;
                }

                if (true == Cb_ANT22.Checked)
                {
                    WriteBuf[3] |= 0x20;
                }
                else
                {
                    WriteBuf[3] &= 0xDF;
                }

                if (true == Cb_ANT23.Checked)
                {
                    WriteBuf[3] |= 0x40;
                }
                else
                {
                    WriteBuf[3] &= 0xBF;
                }

                if (true == Cb_ANT24.Checked)
                {
                    WriteBuf[3] |= 0x80;
                }
                else
                {
                    WriteBuf[3] &= 0x7F;
                }

                if (true == Cb_ANT25.Checked)
                {
                    WriteBuf[4] |= 0x01;
                }
                else
                {
                    WriteBuf[4] &= 0xFE;
                }

                if (true == Cb_ANT26.Checked)
                {
                    WriteBuf[4] |= 0x02;
                }
                else
                {
                    WriteBuf[4] &= 0xFD;
                }

                if (true == Cb_ANT27.Checked)
                {
                    WriteBuf[4] |= 0x04;
                }
                else
                {
                    WriteBuf[4] &= 0xFB;
                }

                if (true == Cb_ANT28.Checked)
                {
                    WriteBuf[4] |= 0x08;
                }
                else
                {
                    WriteBuf[4] &= 0xF7;
                }

                if (true == Cb_ANT29.Checked)
                {
                    WriteBuf[4] |= 0x10;
                }
                else
                {
                    WriteBuf[4] &= 0xEF;
                }

                if (true == Cb_ANT30.Checked)
                {
                    WriteBuf[4] |= 0x20;
                }
                else
                {
                    WriteBuf[4] &= 0xDF;
                }

                if (true == Cb_ANT31.Checked)
                {
                    WriteBuf[4] |= 0x40;
                }
                else
                {
                    WriteBuf[4] &= 0xBF;
                }

                if (true == Cb_ANT32.Checked)
                {
                    WriteBuf[4] |= 0x80;
                }
                else
                {
                    WriteBuf[4] &= 0x7F;
                }

                if (true == Cb_ANT33.Checked)
                {
                    WriteBuf[5] |= 0x01;
                }
                else
                {
                    WriteBuf[5] &= 0xFE;
                }

                if (true == Cb_ANT34.Checked)
                {
                    WriteBuf[5] |= 0x02;
                }
                else
                {
                    WriteBuf[5] &= 0xFD;
                }

                if (true == Cb_ANT35.Checked)
                {
                    WriteBuf[5] |= 0x04;
                }
                else
                {
                    WriteBuf[5] &= 0xFB;
                }

                if (true == Cb_ANT36.Checked)
                {
                    WriteBuf[5] |= 0x08;
                }
                else
                {
                    WriteBuf[5] &= 0xF7;
                }

                if (true == Cb_ANT37.Checked)
                {
                    WriteBuf[5] |= 0x10;
                }
                else
                {
                    WriteBuf[5] &= 0xEF;
                }

                if (true == Cb_ANT38.Checked)
                {
                    WriteBuf[5] |= 0x20;
                }
                else
                {
                    WriteBuf[5] &= 0xDF;
                }

                if (true == Cb_ANT39.Checked)
                {
                    WriteBuf[5] |= 0x40;
                }
                else
                {
                    WriteBuf[5] &= 0xBF;
                }

                if (true == Cb_ANT40.Checked)
                {
                    WriteBuf[5] |= 0x80;
                }
                else
                {
                    WriteBuf[5] &= 0x7F;
                }

                if (true == Cb_ANT41.Checked)
                {
                    WriteBuf[6] |= 0x01;
                }
                else
                {
                    WriteBuf[6] &= 0xFE;
                }

                if (true == Cb_ANT42.Checked)
                {
                    WriteBuf[6] |= 0x02;
                }
                else
                {
                    WriteBuf[6] &= 0xFD;
                }

                if (true == Cb_ANT43.Checked)
                {
                    WriteBuf[6] |= 0x04;
                }
                else
                {
                    WriteBuf[6] &= 0xFB;
                }

                if (true == Cb_ANT44.Checked)
                {
                    WriteBuf[6] |= 0x08;
                }
                else
                {
                    WriteBuf[6] &= 0xF7;
                }

                if (true == Cb_ANT45.Checked)
                {
                    WriteBuf[6] |= 0x10;
                }
                else
                {
                    WriteBuf[6] &= 0xEF;
                }

                if (true == Cb_ANT46.Checked)
                {
                    WriteBuf[6] |= 0x20;
                }
                else
                {
                    WriteBuf[6] &= 0xDF;
                }

                if (true == Cb_ANT47.Checked)
                {
                    WriteBuf[6] |= 0x40;
                }
                else
                {
                    WriteBuf[6] &= 0xBF;
                }

                if (true == Cb_ANT48.Checked)
                {
                    WriteBuf[6] |= 0x80;
                }
                else
                {
                    WriteBuf[6] &= 0x7F;
                }

                if (true == Cb_ANT49.Checked)
                {
                    WriteBuf[7] |= 0x01;
                }
                else
                {
                    WriteBuf[7] &= 0xFE;
                }

                if (true == Cb_ANT50.Checked)
                {
                    WriteBuf[7] |= 0x02;
                }
                else
                {
                    WriteBuf[7] &= 0xFD;
                }

                if (true == Cb_ANT51.Checked)
                {
                    WriteBuf[7] |= 0x04;
                }
                else
                {
                    WriteBuf[7] &= 0xFB;
                }

                if (true == Cb_ANT52.Checked)
                {
                    WriteBuf[7] |= 0x08;
                }
                else
                {
                    WriteBuf[7] &= 0xF7;
                }

                if (true == Cb_ANT53.Checked)
                {
                    WriteBuf[7] |= 0x10;
                }
                else
                {
                    WriteBuf[7] &= 0xEF;
                }

                if (true == Cb_ANT54.Checked)
                {
                    WriteBuf[7] |= 0x20;
                }
                else
                {
                    WriteBuf[7] &= 0xDF;
                }

                if (true == Cb_ANT55.Checked)
                {
                    WriteBuf[7] |= 0x40;
                }
                else
                {
                    WriteBuf[7] &= 0xBF;
                }

                if (true == Cb_ANT56.Checked)
                {
                    WriteBuf[7] |= 0x80;
                }
                else
                {
                    WriteBuf[7] &= 0x7F;
                }

                if (true == Cb_ANT57.Checked)
                {
                    WriteBuf[8] |= 0x01;
                }
                else
                {
                    WriteBuf[8] &= 0xFE;
                }

                if (true == Cb_ANT58.Checked)
                {
                    WriteBuf[8] |= 0x02;
                }
                else
                {
                    WriteBuf[8] &= 0xFD;
                }

                if (true == Cb_ANT59.Checked)
                {
                    WriteBuf[8] |= 0x04;
                }
                else
                {
                    WriteBuf[8] &= 0xFB;
                }

                if (true == Cb_ANT60.Checked)
                {
                    WriteBuf[8] |= 0x08;
                }
                else
                {
                    WriteBuf[8] &= 0xF7;
                }

                if (true == Cb_ANT61.Checked)
                {
                    WriteBuf[8] |= 0x10;
                }
                else
                {
                    WriteBuf[8] &= 0xEF;
                }

                if (true == Cb_ANT62.Checked)
                {
                    WriteBuf[8] |= 0x20;
                }
                else
                {
                    WriteBuf[8] &= 0xDF;
                }

                if (true == Cb_ANT63.Checked)
                {
                    WriteBuf[8] |= 0x40;
                }
                else
                {
                    WriteBuf[8] &= 0xBF;
                }

                if (true == Cb_ANT64.Checked)
                {
                    WriteBuf[8] |= 0x80;
                }
                else
                {
                    WriteBuf[8] &= 0x7F;
                }
            }
            else
            {
                if (true == Cb_ANT1.Checked)
                {
                    WriteBuf[0] |= 0x01;
                }
                else
                {
                    WriteBuf[0] &= 0xFE;
                }

                if (true == Cb_ANT2.Checked)
                {
                    WriteBuf[0] |= 0x02;
                }
                else
                {
                    WriteBuf[0] &= 0xFD;
                }

                if (true == Cb_ANT3.Checked)
                {
                    WriteBuf[0] |= 0x04;
                }
                else
                {
                    WriteBuf[0] &= 0xFB;
                }

                if (true == Cb_ANT4.Checked)
                {
                    WriteBuf[0] |= 0x08;
                }
                else
                {
                    WriteBuf[0] &= 0xF7;
                }

                if (true == Cb_ANT5.Checked)
                {
                    WriteBuf[0] |= 0x10;
                }
                else
                {
                    WriteBuf[0] &= 0xEF;
                }

                if (true == Cb_ANT6.Checked)
                {
                    WriteBuf[0] |= 0x20;
                }
                else
                {
                    WriteBuf[0] &= 0xDF;
                }

                if (true == Cb_ANT7.Checked)
                {
                    WriteBuf[0] |= 0x40;
                }
                else
                {
                    WriteBuf[0] &= 0xBF;
                }

                if (true == Cb_ANT8.Checked)
                {
                    WriteBuf[0] |= 0x80;
                }
                else
                {
                    WriteBuf[0] &= 0x7F;
                }

                if (true == Cb_ANT9.Checked)
                {
                    WriteBuf[1] |= 0x01;
                }
                else
                {
                    WriteBuf[1] &= 0xFE;
                }

                if (true == Cb_ANT10.Checked)
                {
                    WriteBuf[1] |= 0x02;
                }
                else
                {
                    WriteBuf[1] &= 0xFD;
                }

                if (true == Cb_ANT11.Checked)
                {
                    WriteBuf[1] |= 0x04;
                }
                else
                {
                    WriteBuf[1] &= 0xFB;
                }

                if (true == Cb_ANT12.Checked)
                {
                    WriteBuf[1] |= 0x08;
                }
                else
                {
                    WriteBuf[1] &= 0xF7;
                }

                if (true == Cb_ANT13.Checked)
                {
                    WriteBuf[1] |= 0x10;
                }
                else
                {
                    WriteBuf[1] &= 0xEF;
                }

                if (true == Cb_ANT14.Checked)
                {
                    WriteBuf[1] |= 0x20;
                }
                else
                {
                    WriteBuf[1] &= 0xDF;
                }

                if (true == Cb_ANT15.Checked)
                {
                    WriteBuf[1] |= 0x40;
                }
                else
                {
                    WriteBuf[1] &= 0xBF;
                }

                if (true == Cb_ANT16.Checked)
                {
                    WriteBuf[1] |= 0x80;
                }
                else
                {
                    WriteBuf[1] &= 0x7F;
                }

                if (true == Cb_ANT17.Checked)
                {
                    WriteBuf[2] |= 0x01;
                }
                else
                {
                    WriteBuf[2] &= 0xFE;
                }

                if (true == Cb_ANT18.Checked)
                {
                    WriteBuf[2] |= 0x02;
                }
                else
                {
                    WriteBuf[2] &= 0xFD;
                }

                if (true == Cb_ANT19.Checked)
                {
                    WriteBuf[2] |= 0x04;
                }
                else
                {
                    WriteBuf[2] &= 0xFB;
                }
            }





            UInt16 len = 0;
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    len = 9;
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_ANT_STATUE, len);
            //}
            //else
            //{
            //    len = 8;
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x08, 8);
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                len = 9;
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_SET_ANT_STATUE, len);
            }
            else
            {
                len = 8;
                result = delay(CMD.TIMEOUT, WriteBuf, 0x08, len);
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

            if (0 == ReaderParams.ProtocolFlag)
            {
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
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x88)
                    && (revbuf[2] == 0x01)))
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
            if (checkBox2.Checked == true)
            {
                this.Close();
                return;
            }
        }
        //获取
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

        private void Cb_Get_Click(object sender, EventArgs e)
        {
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            UInt16 len = 0;

            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_ANT_STATUE, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x10, len);
            //}
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
            result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_ANT_STATUE, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x10, len);
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
            //        while ((revlen < 0x0A) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
            //    }
            //    else
            //    {
            //        while ((revlen < 0x0E) && (recount != 0))
            //        {
            //            recount--;
            //            revlen = ReadWriteIO.comm.BytesToRead;
            //        }
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


            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_ANT_STATUE_RSP)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("获取天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
                
                int antselect = (int)((revbuf[5] << 8) + revbuf[6]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT1.Checked = true;
                }
                else
                {
                    Cb_ANT1.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT2.Checked = true;
                }
                else
                {
                    Cb_ANT2.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT3.Checked = true;
                }
                else
                {
                    Cb_ANT3.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT4.Checked = true;
                }
                else
                {
                    Cb_ANT4.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT5.Checked = true;
                }
                else
                {
                    Cb_ANT5.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT6.Checked = true;
                }
                else
                {
                    Cb_ANT6.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT7.Checked = true;
                }
                else
                {
                    Cb_ANT7.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT8.Checked = true;
                }
                else
                {
                    Cb_ANT8.Checked = false;
                }

                if ((antselect & 0x0100) == 0x0100)
                {
                    Cb_ANT9.Checked = true;
                }
                else
                {
                    Cb_ANT9.Checked = false;
                }

                if ((antselect & 0x0200) == 0x0200)
                {
                    Cb_ANT10.Checked = true;
                }
                else
                {
                    Cb_ANT10.Checked = false;
                }

                if ((antselect & 0x0400) == 0x0400)
                {
                    Cb_ANT11.Checked = true;
                }
                else
                {
                    Cb_ANT11.Checked = false;
                }

                if ((antselect & 0x0800) == 0x0800)
                {
                    Cb_ANT12.Checked = true;
                }
                else
                {
                    Cb_ANT12.Checked = false;
                }

                if ((antselect & 0x1000) == 0x1000)
                {
                    Cb_ANT13.Checked = true;
                }
                else
                {
                    Cb_ANT13.Checked = false;
                }

                if ((antselect & 0x2000) == 0x2000)
                {
                    Cb_ANT14.Checked = true;
                }
                else
                {
                    Cb_ANT14.Checked = false;
                }

                if ((antselect & 0x4000) == 0x4000)
                {
                    Cb_ANT15.Checked = true;
                }
                else
                {
                    Cb_ANT15.Checked = false;
                }

                if ((antselect & 0x8000) == 0x8000)
                {
                    Cb_ANT16.Checked = true;
                }
                else
                {
                    Cb_ANT16.Checked = false;
                }
                if(revlen <11)
                {
                    return;
                }
                antselect = (int)((revbuf[8] << 8) + revbuf[7]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT17.Checked = true;
                }
                else
                {
                    Cb_ANT17.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT18.Checked = true;
                }
                else
                {
                    Cb_ANT18.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT19.Checked = true;
                }
                else
                {
                    Cb_ANT19.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT20.Checked = true;
                }
                else
                {
                    Cb_ANT20.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT21.Checked = true;
                }
                else
                {
                    Cb_ANT21.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT22.Checked = true;
                }
                else
                {
                    Cb_ANT22.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT23.Checked = true;
                }
                else
                {
                    Cb_ANT23.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT24.Checked = true;
                }
                else
                {
                    Cb_ANT24.Checked = false;
                }

                if ((antselect & 0x0100) == 0x0100)
                {
                    Cb_ANT25.Checked = true;
                }
                else
                {
                    Cb_ANT25.Checked = false;
                }

                if ((antselect & 0x0200) == 0x0200)
                {
                    Cb_ANT26.Checked = true;
                }
                else
                {
                    Cb_ANT26.Checked = false;
                }

                if ((antselect & 0x0400) == 0x0400)
                {
                    Cb_ANT27.Checked = true;
                }
                else
                {
                    Cb_ANT27.Checked = false;
                }

                if ((antselect & 0x0800) == 0x0800)
                {
                    Cb_ANT28.Checked = true;
                }
                else
                {
                    Cb_ANT28.Checked = false;
                }

                if ((antselect & 0x1000) == 0x1000)
                {
                    Cb_ANT29.Checked = true;
                }
                else
                {
                    Cb_ANT29.Checked = false;
                }

                if ((antselect & 0x2000) == 0x2000)
                {
                    Cb_ANT30.Checked = true;
                }
                else
                {
                    Cb_ANT30.Checked = false;
                }

                if ((antselect & 0x4000) == 0x4000)
                {
                    Cb_ANT31.Checked = true;
                }
                else
                {
                    Cb_ANT31.Checked = false;
                }

                if ((antselect & 0x8000) == 0x8000)
                {
                    Cb_ANT32.Checked = true;
                }
                else
                {
                    Cb_ANT32.Checked = false;
                }

                antselect = (int)((revbuf[10] << 8) + revbuf[9]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT33.Checked = true;
                }
                else
                {
                    Cb_ANT33.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT34.Checked = true;
                }
                else
                {
                    Cb_ANT34.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT35.Checked = true;
                }
                else
                {
                    Cb_ANT35.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT36.Checked = true;
                }
                else
                {
                    Cb_ANT36.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT37.Checked = true;
                }
                else
                {
                    Cb_ANT37.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT38.Checked = true;
                }
                else
                {
                    Cb_ANT38.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT39.Checked = true;
                }
                else
                {
                    Cb_ANT39.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT40.Checked = true;
                }
                else
                {
                    Cb_ANT40.Checked = false;
                }

                if ((antselect & 0x0100) == 0x0100)
                {
                    Cb_ANT41.Checked = true;
                }
                else
                {
                    Cb_ANT41.Checked = false;
                }

                if ((antselect & 0x0200) == 0x0200)
                {
                    Cb_ANT42.Checked = true;
                }
                else
                {
                    Cb_ANT42.Checked = false;
                }

                if ((antselect & 0x0400) == 0x0400)
                {
                    Cb_ANT43.Checked = true;
                }
                else
                {
                    Cb_ANT43.Checked = false;
                }

                if ((antselect & 0x0800) == 0x0800)
                {
                    Cb_ANT44.Checked = true;
                }
                else
                {
                    Cb_ANT44.Checked = false;
                }

                if ((antselect & 0x1000) == 0x1000)
                {
                    Cb_ANT45.Checked = true;
                }
                else
                {
                    Cb_ANT45.Checked = false;
                }

                if ((antselect & 0x2000) == 0x2000)
                {
                    Cb_ANT46.Checked = true;
                }
                else
                {
                    Cb_ANT46.Checked = false;
                }

                if ((antselect & 0x4000) == 0x4000)
                {
                    Cb_ANT47.Checked = true;
                }
                else
                {
                    Cb_ANT47.Checked = false;
                }

                if ((antselect & 0x8000) == 0x8000)
                {
                    Cb_ANT48.Checked = true;
                }
                else
                {
                    Cb_ANT48.Checked = false;
                }

                antselect = (int)((revbuf[12] << 8) + revbuf[11]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT49.Checked = true;
                }
                else
                {
                    Cb_ANT49.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT50.Checked = true;
                }
                else
                {
                    Cb_ANT50.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT51.Checked = true;
                }
                else
                {
                    Cb_ANT51.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT52.Checked = true;
                }
                else
                {
                    Cb_ANT52.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT53.Checked = true;
                }
                else
                {
                    Cb_ANT53.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT54.Checked = true;
                }
                else
                {
                    Cb_ANT54.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT55.Checked = true;
                }
                else
                {
                    Cb_ANT55.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT56.Checked = true;
                }
                else
                {
                    Cb_ANT56.Checked = false;
                }

                if ((antselect & 0x0100) == 0x0100)
                {
                    Cb_ANT57.Checked = true;
                }
                else
                {
                    Cb_ANT57.Checked = false;
                }

                if ((antselect & 0x0200) == 0x0200)
                {
                    Cb_ANT58.Checked = true;
                }
                else
                {
                    Cb_ANT58.Checked = false;
                }

                if ((antselect & 0x0400) == 0x0400)
                {
                    Cb_ANT59.Checked = true;
                }
                else
                {
                    Cb_ANT59.Checked = false;
                }

                if ((antselect & 0x0800) == 0x0800)
                {
                    Cb_ANT60.Checked = true;
                }
                else
                {
                    Cb_ANT60.Checked = false;
                }

                if ((antselect & 0x1000) == 0x1000)
                {
                    Cb_ANT61.Checked = true;
                }
                else
                {
                    Cb_ANT61.Checked = false;
                }

                if ((antselect & 0x2000) == 0x2000)
                {
                    Cb_ANT62.Checked = true;
                }
                else
                {
                    Cb_ANT62.Checked = false;
                }

                if ((antselect & 0x4000) == 0x4000)
                {
                    Cb_ANT63.Checked = true;
                }
                else
                {
                    Cb_ANT63.Checked = false;
                }

                if ((antselect & 0x8000) == 0x8000)
                {
                    Cb_ANT64.Checked = true;
                }
                else
                {
                    Cb_ANT64.Checked = false;
                }
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0x90)
                    && (revbuf[2] == 0x08)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        MessageBox.Show("获取天线设置失败", "有提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Get Failed", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }


                int antselect = (int)(revbuf[3]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT1.Checked = true;
                }
                else
                {
                    Cb_ANT1.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT2.Checked = true;
                }
                else
                {
                    Cb_ANT2.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT3.Checked = true;
                }
                else
                {
                    Cb_ANT3.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT4.Checked = true;
                }
                else
                {
                    Cb_ANT4.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT5.Checked = true;
                }
                else
                {
                    Cb_ANT5.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT6.Checked = true;
                }
                else
                {
                    Cb_ANT6.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT7.Checked = true;
                }
                else
                {
                    Cb_ANT7.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT8.Checked = true;
                }
                else
                {
                    Cb_ANT8.Checked = false;
                }

                antselect = (int)(revbuf[4]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT9.Checked = true;
                }
                else
                {
                    Cb_ANT9.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT10.Checked = true;
                }
                else
                {
                    Cb_ANT10.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT11.Checked = true;
                }
                else
                {
                    Cb_ANT11.Checked = false;
                }

                if ((antselect & 0x08) == 0x08)
                {
                    Cb_ANT12.Checked = true;
                }
                else
                {
                    Cb_ANT12.Checked = false;
                }

                if ((antselect & 0x10) == 0x10)
                {
                    Cb_ANT13.Checked = true;
                }
                else
                {
                    Cb_ANT13.Checked = false;
                }

                if ((antselect & 0x20) == 0x20)
                {
                    Cb_ANT14.Checked = true;
                }
                else
                {
                    Cb_ANT14.Checked = false;
                }

                if ((antselect & 0x40) == 0x40)
                {
                    Cb_ANT15.Checked = true;
                }
                else
                {
                    Cb_ANT15.Checked = false;
                }

                if ((antselect & 0x80) == 0x80)
                {
                    Cb_ANT16.Checked = true;
                }
                else
                {
                    Cb_ANT16.Checked = false;
                }

                antselect = (int)(revbuf[5]);
                if ((antselect & 0x01) == 0x01)
                {
                    Cb_ANT17.Checked = true;
                }
                else
                {
                    Cb_ANT17.Checked = false;
                }

                if ((antselect & 0x02) == 0x02)
                {
                    Cb_ANT18.Checked = true;
                }
                else
                {
                    Cb_ANT18.Checked = false;
                }

                if ((antselect & 0x04) == 0x04)
                {
                    Cb_ANT19.Checked = true;
                }
                else
                {
                    Cb_ANT19.Checked = false;
                }
            }
        }

        

        private void Test_Load(object sender, EventArgs e)
        {
            string name;
            Control cl;
            if (0 == ReaderParams.LanguageFlag)
            {
                groupBox1.Text = "天线设置";
                GetLoss.Text = "反射";
                cB_antSaveFlag.Text = "保存";
                Cb_Get.Text = "获取";
                Cb_Set.Text = "设置";
                checkBox2.Text = "设置成功退出";
                for (int i = 1; i < 65;i++ )
                {   cl=null;
                    name = "Cb_ANT"+i.ToString ();
                    try 
                    {
                        //groupBox1 .Controls .Find
                        cl = groupBox1.Controls.Find(name, true)[0];
                    }
                    catch(Exception ex)
                    {

                    }
                    if (cl != null) 
                    {
                        CheckBox cb = cl as CheckBox;
                        cb.Text= "天线"+i.ToString ();
                    }
                }
            }
            else
            {
                groupBox1.Text = "Antenna Setting";
                GetLoss.Text = "RX ADC";
                cB_antSaveFlag.Text = "Save";
                Cb_Get.Text = "GET";
                Cb_Set.Text = "SET";
                checkBox2.Text = "If success quit";
                for (int i = 1; i < 65; i++)
                {
                    cl = null;
                    name = "Cb_ANT" + i.ToString();
                    try
                    {
                        cl = this.Controls.Find(name, true)[0];
                    }
                    catch (Exception ex)
                    {

                    }
                    if (cl != null)
                    {
                        CheckBox cb = cl as CheckBox;
                        cb.Text = "Ant"+i.ToString();
                    }
                }
            }
        }

        private void GetLoss_Click(object sender, EventArgs e)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            UInt16 Rxadc = 0;
            int i;

            Show_Loss.Text = "";
            int result = 0;
            if (0 == ReaderParams.ProtocolFlag)
            {
                result = delay(CMD.TIMEOUT, WriteBuf, CMD.FRAME_CMD_GET_REVPWR, len);
            }
            else
            {
                result = delay(CMD.TIMEOUT, WriteBuf, 0x31, len);
            }
            
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
            //if (0 == ReaderParams.ProtocolFlag)
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_REVPWR, len);
            //}
            //else
            //{
            //    ReadWriteIO.sendFrameBuild(WriteBuf, 0x31, len);
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
            //            Show_Loss.Text = "获取失败";
            //        }
            //        else
            //        {
            //            Show_Loss.Text = "Get Failed";
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

            if (0 == ReaderParams.ProtocolFlag)
            {
                //判断是否设置成功
                if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                    && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                    && (revbuf[2] == 0x00) 
                    //&& (revbuf[3] == 0x0B)
                    && (revbuf[4] == CMD.FRAME_CMD_GET_REVPWR_RSP)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        Show_Loss.Text = "获取失败";
                    }
                    else
                    {
                        Show_Loss.Text = "Get Failed";
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
                    Show_Loss.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Show_Loss.ForeColor = System.Drawing.SystemColors.WindowText;
                }

                Show_Loss.Text = i.ToString();
            }
            else
            {
                //判断是否设置成功
                if (!((revbuf[0] == 0xBB)
                    && (revbuf[1] == 0xB1)
                    && (revbuf[2] == 0x03)))
                {
                    if (0 == ReaderParams.LanguageFlag)
                    {
                        Show_Loss.Text = "获取失败";
                    }
                    else
                    {
                        Show_Loss.Text = "Get Failed";
                    }
                    return;
                }
                Rxadc = (UInt16)(revbuf[6 - 2] * 256 + revbuf[7 - 2]);
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
                    Show_Loss.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Show_Loss.ForeColor = System.Drawing.SystemColors.WindowText;
                }

                Show_Loss.Text = i.ToString();
            }

        }

        private void Ck_NoChose_CheckedChanged(object sender, EventArgs e)
        {
            if (true == Ck_NoChose.Checked)
            {
                //Ck_Chose.Checked = false;
                Cb_ANT1.Checked = true;
                Cb_ANT2.Checked = true;
                Cb_ANT3.Checked = true;
                Cb_ANT4.Checked = true;
                Cb_ANT5.Checked = true;
                Cb_ANT6.Checked = true;
                Cb_ANT7.Checked = true;
                Cb_ANT8.Checked = true;
                Cb_ANT9.Checked = true;
                Cb_ANT10.Checked = true;
            }
            else
            {
                Cb_ANT1.Checked = false;
                Cb_ANT2.Checked = false;
                Cb_ANT3.Checked = false;
                Cb_ANT4.Checked = false;
                Cb_ANT5.Checked = false;
                Cb_ANT6.Checked = false;
                Cb_ANT7.Checked = false;
                Cb_ANT8.Checked = false;
                Cb_ANT9.Checked = false;
                Cb_ANT10.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                foreach (Control c in this.groupBox1 .Controls)
                {
                    if ((c.Name.StartsWith("Cb_ANT"))&&(c.GetType ().Name =="CheckBox"))
                    {
                        ((CheckBox)c).Checked = true;
                    }
                } 
            }
            else 
            {
                foreach (Control c in this.groupBox1.Controls)
                {
                    if ((c.Name.StartsWith("Cb_ANT")) && (c.GetType().Name == "CheckBox"))
                    {
                        ((CheckBox)c).Checked = false;
                    }
                } 
            }
        }



    }
}

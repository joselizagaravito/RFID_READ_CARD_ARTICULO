using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace R2000Demo
{
    public class ReaderParams
    {
        public ReaderParams()
        {
            InvTimeOut = 50;
            ReadTimeOut = 100;
            WriteTimeOut = 100;
            LockTimeOut = 100;
            KillTimeOut = 100;

            LinkProfile = 3;
            DataFormat = 0;
            InventoryAlg = 1;
            OperationMode = 1;
            ModulateWay = 2;
            Recommand = 1;

            startQ = 4;
            maxQ = 15;
            minQ = 0;

            Selecttarget = 0;
            action = 0;
            MB = 0;
            trun = 0;
            select_len = "0";
            select_pointer = "32";
            select_mask = "00-00-00-00-00-00-00-00-00-00-00-00";

            DR = 1;
            M = 2;
            TRext = 1;
            Sel = 1;
            Session = 1;
            QuerTarget = 0;

            FilterFlag = 0;
            select_TagID = "";

            CommIntSelectFlag = 0;

            LanguageFlag = 2; //Actualización Jose Liza 2021-03-11
            FolderCSV = Properties.Resources.ResourceManager.GetString("folderCSV");
            ProtocolFlag = 0;
            ProtocoloTCPIP = 20108; //Actualizacion - Jose Liza: 20210123 
            Netrecount = 200000;
        }

        public static UInt16 InvTimeOut;
        public static UInt16 ReadTimeOut;
        public static UInt16 WriteTimeOut;
        public static UInt16 LockTimeOut;
        public static UInt16 KillTimeOut;

        public static UInt16 LinkProfile;
        public static UInt16 DataFormat;
        public static UInt16 InventoryAlg;
        public static UInt16 OperationMode;
        public static UInt16 ModulateWay;
        public static UInt16 Recommand;

        public static UInt16 startQ;
        public static UInt16 minQ;
        public static UInt16 maxQ;

        public static UInt16 Selecttarget;
        public static UInt16 action;
        public static UInt16 MB;
        public static UInt16 trun;
        public static string select_len;
        public static string select_pointer;
        public static string select_mask;

        public static UInt16 DR;
        public static UInt16 M;
        public static UInt16 TRext;
        public static UInt16 Sel;
        public static UInt16 Session;
        public static UInt16 QuerTarget;

        public static UInt16 FilterFlag;
        public static string select_TagID;

        //与服务器的连接
        public static TcpClient tcpClient;

        //与服务器交流的流通道
        public static NetworkStream nsStream;

        public static UInt16 CommIntSelectFlag;             // 0--网口，1--串口

        public static UInt16 LanguageFlag;                  // 0--简体中文；1--English；2--Español

        public static UInt16 ProtocolFlag;                  // 0--RealID；1--Other
        public static string FolderCSV;                      // Carpeta donde se guardan los archivos CSV
        public static UInt16 ProtocoloTCPIP;                  // 0--RealID；1--Other

        public static int Netrecount;                       // 网口通信，接收超时延迟

        //Actualización Jose Liza 2021-03-14
        public static string ModuloId;                      // Identificador unico del modulo
        public static string ModuloRol;                     // Caja, Salida

        public static int ByteToHexData(byte[] bt, byte len)
        {
            int i;
            int result = 0;
            int res_temp = 0;
            byte[] tmp = new byte[4];

            for (i = 0; i < len; i++)
            {
                if ((bt[i] >= '0') && bt[i] <= '9')
                {
                    bt[i] = (byte)(bt[i] - '0');
                }
                else
                {
                    bt[i] = (byte)(bt[i] - 'A' + 10);
                }
            }

            for (i = len - 1; i >= 0; i--)
            {
                res_temp = bt[i];
                result += res_temp << (4 * (len - 1 - i));
            }

            return result;
        }

        public static int ByteArrayToUInt8Array(Byte[] InputArray, Byte[] OutputArray, int len)
        {
            int i;

            for (i = 0; i < len; i++)
            {
                if ((InputArray[i] >= '0') && (InputArray[i] <= '9'))
                {
                    OutputArray[i] = (byte)(InputArray[i] - '0');
                }
                else if ((InputArray[i] >= 'A') && (InputArray[i] <= 'Z'))
                {
                    OutputArray[i] = (byte)(InputArray[i] - 'A' + 10);
                }
                else if ((InputArray[i] >= 'a') && (InputArray[i] <= 'z'))
                {
                    OutputArray[i] = (byte)(InputArray[i] - 'a' + 10);
                }
            }

            return 0;
        }

        public static void NonFilterInSelect()
        {
            UInt32 RegAddr = 0x911;
            UInt32 RegData = 0;

            ReaderParams.Write_Reg_Data(0, RegAddr, RegData);

            ReaderParams.FilterFlag = 0;
        }

        public static int Read_Reg_Data(byte RegType, UInt32 addr, UInt32[] data)
        {
            UInt16 len = 5;
            Byte[] WriteBuf = new Byte[100];
            int recount = 100000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            Byte[] bytetmp = new Byte[20];

            WriteBuf[0] = RegType;
            WriteBuf[1] = (byte)(addr >> 24);
            WriteBuf[2] = (byte)(addr >> 16);
            WriteBuf[3] = (byte)(addr >> 8);
            WriteBuf[4] = (byte)addr;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_REQ, len);

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
                    return -1;
                }

                while ((revlen < 0x11) && (recount != 0))
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x11)
                && (revbuf[4] == CMD.FRAME_CMD_GET_REQ_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -1;
            }

            data[0] = (UInt32)((revbuf[10] << 24) + (revbuf[11] << 16) + (revbuf[12] << 8) + revbuf[13]);

            return 0;
        }

        public static int Write_Reg_Data(byte RegType, UInt32 addr, UInt32 data)
        {
            UInt16 len = 9;
            Byte[] WriteBuf = new Byte[100];
            int recount = 100000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            WriteBuf[0] = RegType;
            WriteBuf[1] = (byte)(addr >> 24);
            WriteBuf[2] = (byte)(addr >> 16);
            WriteBuf[3] = (byte)(addr >> 8);
            WriteBuf[4] = (byte)addr;
            WriteBuf[5] = (byte)(data >> 24);
            WriteBuf[6] = (byte)(data >> 16);
            WriteBuf[7] = (byte)(data >> 8);
            WriteBuf[8] = (byte)data;

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_SET_REQ, len);

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
                    return -1;
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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
                && (revbuf[4] == CMD.FRAME_CMD_SET_REQ_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -1;
            }

            return 0;
        }

        public static int GetModuleID(UInt32[] data)
        {
            int result = 0;
            UInt32[] datatmp = new UInt32[1];

            result = ReaderParams.Read_Reg_Data((byte)1, 0x0000000B, datatmp);
            if (result != 0)
            {
                return -1;
            }
            data[0] = datatmp[0];

            result = ReaderParams.Read_Reg_Data((byte)1, 0x0000000C, data);
            if (result != 0)
            {
                return -1;
            }
            data[1] = datatmp[0];

            return 0;
        }

        public static int GetGen2Parameter(byte[] value)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_GEN2_PARA, len);

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
                    return -1;
                }

                while ((revlen < 0x10) && (recount != 0))
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x10)
                && (revbuf[4] == CMD.FRAME_CMD_GET_GEN2_PARA_RSP)))
            {
                return -1;
            }

            System.Array.Copy(revbuf, 5, value, 0, 8);

            return 0;
        }

        public static int SetGen2Parameter(byte[] value)
        {
            UInt16 len = 8;
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲

            ReadWriteIO.sendFrameBuild(value, CMD.FRAME_CMD_SET_GEN2_PARA, len);

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
                    return -1;
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x09)
                && (revbuf[4] == CMD.FRAME_CMD_SET_GEN2_PARA_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -1;
            }

            return 0;
        }
        public static int GetTxPower(int[] value)
        {
            UInt16 len = 0;
            Byte[] WriteBuf = new Byte[100];
            int recount = 50000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲


            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_TX_POWER, len);

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
                    return -1;
                }

                while ((revlen < 0x0e) && (recount != 0))
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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
                && (revbuf[2] == 0x00)
                && (revbuf[4] == CMD.FRAME_CMD_GET_TX_POWER_RSP)
                && (revbuf[5] == 0x00) && (revbuf[6] == 0x00)))
            {
                return -1;
            }

            int power = revbuf[7] * 256 + revbuf[8];
            value[0] = power / 100;

            return 0;
        }

        public static int GetTempProtect(byte[] Status)
        {
            UInt16 len = 0;
            int recount = 80000;     //重试次数
            int revlen = 0;         //接收数据长度
            Byte[] revbuf = new Byte[500];           //接收缓冲
            Byte[] WriteBuf = new Byte[10];

            ReadWriteIO.sendFrameBuild(WriteBuf, CMD.FRAME_CMD_GET_TEMPPROTECT, len);

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
                    return -1;
                }

                while ((revlen < 0x0A) && (recount != 0))
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
                    MessageBox.Show("网口未连接，操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return -1;
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

            //判断是否成功
            if (!((revbuf[0] == CMD.FRAME_HEAD_FIRST)
                && (revbuf[1] == CMD.FRAME_HEAD_SECOND)
                && (revbuf[2] == 0x00) && (revbuf[3] == 0x0A)
                && (revbuf[4] == CMD.FRAME_CMD_GET_TEMPPROTECT_RSP)
                && (revbuf[5] == 0x01)))
            {
                return -1;
            }

            Status[0] = revbuf[6];
            return 0;
        }
    }

    public class TagInfo
    {
        public TagInfo(string epc, int rcnt, Int16 rssi, int id, DateTime time/*, DateTime ret*/ , string color = "blanco", string moduloid = "00000000", string modulorol = "Ninguno")
        {
            epcid = epc;
            readcnt = rcnt;
            rxrssi = rssi;
            antID = id;
            times = time;
            //rptime = ret;
            this.color = color;
            this.moduloid = moduloid;
            this.modulorol = modulorol;
        }

        public string epcid;
        public string tid;
        public int readcnt;
        public Int16 rxrssi;
        public int antID;
        public DateTime times;
        //public TimeSpan rptime;
        //public double rptime;
        public string rptime;
        public string color;
        public string moduloid;
        public string modulorol;
    }
}
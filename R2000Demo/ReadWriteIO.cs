using System;
using System.IO.Ports;              //端口操作

namespace R2000Demo
{
    public class ReadWriteIO
    {
        public ReadWriteIO()
        {
            InvTimeOut = 50;

        }

        public static SerialPort comm = new SerialPort();        //定义一个串口事件
        public static int baud = 115200;                  //波特率固定为115200
        public static UInt16 InvTimeOut;
        public static byte[] SendBuf = new byte[300];        //发送缓冲
        public static int device = 0;
        /*********************************************************************
        *函数名称：sendFrameBuild()
        *函数功能：发送组帧函数
        *参    数：byte[] buf--数据；byte cmd--帧类型；UInt16 len--帧长度
        *返 回 值：无  
        *创 建 者：雷    彪 
        *创建日期：2011-04-26
        *修改记录：无  
        **********************************************************************/
        public static void sendFrameBuild(byte[] buf, byte cmd, UInt16 len)
        {
            if(0 == ReaderParams.ProtocolFlag)
            {
                SendBuf[0] = CMD.FRAME_HEAD_FIRST;
                SendBuf[1] = CMD.FRAME_HEAD_SECOND;
                SendBuf[2] = (byte)(((len + CMD.FRAME_HEADEND_LEN) >> 8) & 0xff);
                if (SendBuf[2]==0) 
                {
                    SendBuf[2] = (byte)device;
                }
                //SendBuf[2] = (byte)device;
                
                SendBuf[3] = (byte)((len + CMD.FRAME_HEADEND_LEN) & 0xff);
                SendBuf[4] = cmd;
                System.Array.Copy(buf, 0, SendBuf, 5, len);
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 3] = ProductCRC(SendBuf, (UInt16)(len + 5)); //CRC
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 2] = CMD.FRAME_END_MRK_FIRST; //帧尾
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 1] = CMD.FRAME_END_MRK_SECOND; //帧尾
            }
            else
            {
                SendBuf[0] = 0xBB;
                SendBuf[1] = cmd;
                SendBuf[2] = (byte)((len ) & 0xff);
                System.Array.Copy(buf, 0, SendBuf, 3, len);
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 5] = ProductSUM(SendBuf, (UInt16)(len + 3)); //CRC
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 4] = CMD.FRAME_END_MRK_FIRST; //帧尾
                SendBuf[len + CMD.FRAME_HEADEND_LEN - 3] = CMD.FRAME_END_MRK_SECOND; //帧尾            
            }
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
        public static byte ProductCRC(byte[] p, UInt16 len)
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
*函数名称：ProductSUM()
*函数功能：生成校验字节
*参    数：byte[] p，帧数据；UInt16 len--长度
*返 回 值：byte--CRC的值
*创 建 者：雷    彪 
*创建日期：2011-05-03
*修改记录：无  
**********************************************************************/
        public static byte ProductSUM(byte[] p, UInt16 len)
        {
            UInt16 i;
            byte crc = 0;

            for (i = 1; i < len; i++)         //计算校验时，帧头和帧尾不计算
            {
                crc += p[i];
            }

            return crc;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGLedInterface
{
    public class CLedShow
    {       
        private readonly byte pColor = 0; //颜色：0-红色，1-绿色，2-黄色
        private byte pAddress;  //2位数
        private byte pEffcetNum;
        private byte pRate;
        private byte pSingleTime;
        private byte pTotalTime;
       
        public CLedShow(byte eAddrs,byte effectNum,byte eRate,byte eSingleTime,byte eStay) 
        {
            pAddress = eAddrs;
            pEffcetNum = effectNum;
            pRate = eRate;
            pSingleTime = eSingleTime;
            pTotalTime = eStay;
        }

        /// <summary>
        /// 显示数据处理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public byte[] GetSendBuffer(string message) 
        {
            byte[] content = Encoding.Default.GetBytes(message);

            return getSendMessage(content, pAddress, pEffcetNum, pRate, pSingleTime, pColor, pTotalTime);
        }

        /// <summary>
        /// 显示数据处理
        /// </summary>
        /// <param name="pContentBuffer">显示内容</param>
        /// <param name="pAddress">address：硬件地址1-255，必须为控制卡的实际地址，或广播地址255</param>
        /// <param name="pEffcet">功能：1-8，播放显示功能定义如下： //1 -- "连续左移" //2 -- "连续上移" //3 -- "连续下移" //4 -- "往下覆盖" //5 -- "往上覆盖" //6 -- "翻白显示"//7 -- "闪烁显示"</param>
        /// <param name="pRate">速度：0-7，八级速度</param>        
        /// <param name="singleTime">单幅停留时间：0-99，>=99表示永久停留</param>
        /// <param name="pTotalTime">数据总有效时间：本条指定的数据的有效时间</param>       
        /// <returns></returns>
        private byte[] getSendMessage(byte[] contentBuffer, byte address, byte effect, byte rate, byte singleTime,byte color, byte totalTime)
        {
            byte[] bSendBuffer = new byte[contentBuffer.Length + 12];
            bSendBuffer[0] = 0xAA; //0xAA：固定引导码1
            bSendBuffer[1] = address;      //address：硬件地址1-255，必须为控制卡的实际地址，或广播地址255
            bSendBuffer[2] = 0xBB; //0xBB：固定引导码2
            bSendBuffer[3] = 0x51; // 0x5:1命令码1
            bSendBuffer[4] = 0x54; //0x54:命令码2

            bSendBuffer[6] = effect;    //功能：1-8，播放显示功能定义如下： //1 -- "连续左移" //2 -- "连续上移" //3 -- "连续下移" //4 -- "往下覆盖" //5 -- "往上覆盖" //6 -- "翻白显示"//7 -- "闪烁显示"
            bSendBuffer[7] = rate;    //速度：0-7，八级速度
            bSendBuffer[8] = singleTime;  //单幅停留时间：0-99，>=99表示永久停留
            bSendBuffer[9] = color;  //颜色：0-红色，1-绿色，2-黄色
            bSendBuffer[10] = totalTime; //数据总有效时间：本条指定的数据的有效时间
           
            byte bMax = 0;
            for (int k = 6; k < 11; k++)
            {
                bMax += bSendBuffer[k];
            }
            for (int j = 0; j < contentBuffer.Length; j++)
            {
                bSendBuffer[j + 11] = contentBuffer[j];
                bMax += contentBuffer[j];
            }
            bSendBuffer[5] = bMax;  //数据累加和：红色标示部份所有字节的累加和，取低位字节
            bSendBuffer[bSendBuffer.Length - 1] = 0xFF;

            return bSendBuffer;
        }
        
    }
}

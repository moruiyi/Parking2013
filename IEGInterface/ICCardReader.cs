using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGInterface
{
    public class ICCardReader
    {
        private CICCardRW moICCard;
        private int mHallID;
        private readonly int BandRate = 57600;
        private bool isConn = false;

        public ICCardReader(int hallID, int hallCom) 
        {
            if (hallID == 11 || hallID == 12 || hallID == 13 || hallID == 14)
            {
                moICCard = new CIcCardRWOne(hallCom, BandRate, 0, 0);
                mHallID = hallID;
            }
        }
        /// <summary>
        /// 刷卡器连接
        /// </summary>
        public void ConnectCom() 
        {
            try
            {
                isConn = moICCard.ConnectCOM();
            }
            catch (Exception ex)
            {
                throw new Exception("刷卡器函数ConnectCom异常："+ex.ToString());
            }
        }
        /// <summary>
        /// 刷卡器断开
        /// </summary>
        public void DisconnectCom()
        {
            try
            {
                int rit = moICCard.disConnectCOM();
                if (rit == 0) 
                {
                    isConn = false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("刷卡器函数DisConnectCom异常：" + ex.ToString());
            }
        }
        /// <summary>
        /// 刷卡操作
        /// </summary>
        public void ICCardRead() 
        {
            try
            {
                uint ICCardNo = 0;
                if (isConn)
                {
                    int nback = 1;
                    Int16 nICType = 0;
                    nback = moICCard.RequestICCard(ref nICType);  //寻卡
                    if (nback == 0) 
                    {
                        nback = moICCard.SelectCard(ref ICCardNo);   //选择卡
                        if (nback == 0) 
                        {
                            //将读取的IC卡号传送到Web服务器上
                            Program.mng.DealCardMessage(mHallID,ICCardNo.ToString());
                            //记录
                            CWSException.WriteLog(mHallID.ToString() + "      " + ICCardNo.ToString(), 3);
                        }
                    }
                }
                else    
                {
                    this.ConnectCom();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("函数ICCardRead异常：" + ex.ToString());
            }
        }
    }
}

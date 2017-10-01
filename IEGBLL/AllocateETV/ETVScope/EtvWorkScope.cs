using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public class EtvWorkScope
    {
        private readonly int maxColumn = 20;  //库内最大列
        private readonly int minColumn = 1;
        private readonly int etvID_Max = 2;
        private readonly int etvID_Min = 1;
        private readonly int safeDistCol= 4;  //安全列

        private CSMG cETV = null;
        private IList<CSMG> etvList = null;

        public EtvWorkScope(CSMG mETV, IList<CSMG> mEtvList) 
        {
            cETV = mETV;
            etvList = mEtvList;
        }

        /// <summary>
        /// 返回ETV当前可活动（工作）左侧范围
        /// </summary>
        /// <returns></returns>
        public int GetLeftWorkScope() 
        {
            try 
            {
                bool isAuto = new CWSMG().CheckEtvMode(cETV.ID);
                string eAddrs = new CWSMG().GetEtvCurrAddress(cETV.ID);
                if (eAddrs == null) 
                {
                    return 0;
                }
                if (cETV.ID - 1 < etvID_Min)    //是ETV1
                {
                    return minColumn;
                }
                else   //是ETV2
                {
                    if (isAuto == false) 
                    {
                        return clcCommom.GetColOfAddress(eAddrs) - safeDistCol;
                    }
                    int lefEtv = cETV.ID - etvID_Min;  //左侧ETV
                    CSMG Etv = new CWSMG().SelectSMG(lefEtv);
                    int leftCol=0;
                    if (Etv.Available && Etv.nIsWorking != 0)  //左侧ETV正在执行作业
                    {
                        isNeedMove(Etv.nIsWorking, out leftCol, true);
                        return leftCol + 1;
                    }
                    else 
                    {
                        string leftAddrs = new CWSMG().GetEtvCurrAddress(Etv.ID);
                        return clcCommom.GetColOfAddress(leftAddrs) + safeDistCol;
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回ETV当前可活动（工作）右侧范围
        /// </summary>
        /// <returns></returns>
        public int GetRightWorkScope()
        {
            try 
            {
                bool isAuto = new CWSMG().CheckEtvMode(cETV.ID);
                string eAddrs = new CWSMG().GetEtvCurrAddress(cETV.ID);
                if (eAddrs == null)
                {
                    return 0;
                }
                if (cETV.ID + 1 > etvID_Max)  //ETV2
                {
                    return maxColumn;
                }
                else  //ETV1
                {
                    if (isAuto == false)  //etv1故障时
                    {
                        return clcCommom.GetColOfAddress(eAddrs) + safeDistCol;
                    }
                    int rightEtv = cETV.ID + 1;
                    CSMG Etv = new CWSMG().SelectSMG(rightEtv);    //ETV2               
                    int RightCol = 0;
                    if (Etv.Available && Etv.nIsWorking != 0)
                    {
                        isNeedMove(Etv.nIsWorking, out RightCol, false);
                        return RightCol - 1;
                    }
                    else 
                    {
                        string rightAddrs = new CWSMG().GetEtvCurrAddress(Etv.ID);  //以ETV2的地址为左侧地址
                        return clcCommom.GetColOfAddress(rightAddrs) - safeDistCol;
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定作业的最大/小列
        /// </summary>        
        private void isNeedMove(int tskID, out int pDestCol,bool le) 
        {
            pDestCol = 0;
            CTask tsk = new CWTask().GetCTaskFromtskID(tskID);
            string frAddress = tsk.FromLctAdrs;
            string toAddress = tsk.ToLctAdrs;
            int fromCol = clcCommom.GetColOfAddress(frAddress);
            int toCol = clcCommom.GetColOfAddress(toAddress);
            if (le == true)
            {
                pDestCol = fromCol > toCol ? fromCol : toCol;
            }
            else 
            {
                pDestCol = fromCol < toCol ? fromCol : toCol;
            }
        }
    }
}

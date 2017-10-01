using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public class EtvMaxScope
    {
        private readonly int safeDistCol = 4;  //安全列

        private CSMG cETV = null;
        private IList<CSMG> etvList = null;
        private CScope mPhysicWorkScope = null;
        
        public EtvMaxScope(CSMG pEtv, IList<CSMG> pEtvList, CScope physicScope) 
        {
            cETV = pEtv;
            etvList = pEtvList;
            mPhysicWorkScope = physicScope;
        }

        /// <summary>
        /// 获取最大可达的左侧范围
        /// </summary>
        /// <returns></returns>
        public int GetMaxLeftScope() 
        {
            try 
            {
                string eAddrs = new CWSMG().GetEtvCurrAddress(cETV.ID);
                if (eAddrs == null) 
                {
                    return 0;
                }
                int etvCol = clcCommom.GetColOfAddress(eAddrs);
                int disableCol = GetPhysicLeftCol(mPhysicWorkScope.LeftCol,etvCol);
                if (disableCol != mPhysicWorkScope.LeftCol) 
                {
                    return disableCol + safeDistCol;
                }
                return mPhysicWorkScope.LeftCol;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取最大可达的右侧范围
        /// </summary>
        /// <returns></returns>
        public int GetMaxRightScope()
        {
            try
            {
                string eAddrs = new CWSMG().GetEtvCurrAddress(cETV.ID);
                if (eAddrs == null)
                {
                    return 0;
                }
                int etvCol = clcCommom.GetColOfAddress(eAddrs);
                int disableCol = GetPhysicRightCol(etvCol,mPhysicWorkScope.RightCol);
                if (disableCol != mPhysicWorkScope.RightCol) 
                {
                    return disableCol - safeDistCol;
                }
                return mPhysicWorkScope.RightCol;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 物理左范围
        /// </summary>
        /// <param name="pLeft"></param>
        /// <param name="pRight"></param>
        /// <returns></returns>
        private int GetPhysicLeftCol(int pLeft, int pRight)
        {           
            int leftCol = pLeft;
            foreach (CSMG smg in etvList) 
            {               
                string eAddrs = new CWSMG().GetEtvCurrAddress(smg.ID);
                if (eAddrs == null)
                {
                    return 0;
                }
                bool isAuto = new CWSMG().CheckEtvMode(smg.ID);
                int etvCol = clcCommom.GetColOfAddress(eAddrs);
                if (!isAuto && (etvCol > pLeft && etvCol < pRight)) 
                {
                    leftCol = etvCol;
                }
            }
            return leftCol;
        }

        /// <summary>
        /// 物理右范围
        /// </summary>
        /// <param name="pLeft"></param>
        /// <param name="pRight"></param>
        /// <returns></returns>
        private int GetPhysicRightCol(int pLeft, int pRight) 
        {
            int rightCol = pRight;
            foreach (CSMG smg in etvList) 
            {
                string eAddrs = new CWSMG().GetEtvCurrAddress(smg.ID);
                if (eAddrs == null)
                {
                    return 0;
                }
                bool isAuto = new CWSMG().CheckEtvMode(smg.ID);
                int etvCol = clcCommom.GetColOfAddress(eAddrs);
                if (!isAuto && (etvCol > pLeft && etvCol < pRight))
                {
                    rightCol = etvCol;
                }
            }
            return rightCol;
        }
    }
}

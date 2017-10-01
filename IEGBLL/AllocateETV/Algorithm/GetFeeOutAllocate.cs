using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public class GetFeeOutAllocate:AbsAllocate
    {
        public GetFeeOutAllocate() 
        {
        }

        /// <summary>
        /// 缴费出车：依作业范围内找出可达车位的ETV集合1，找出空闲的ETV集合2，
        /// 依集合2找出最近的ETV,如果找不到，查找集合2，
        /// 后依ETV可达的作业范围找出距离车位最近的出车厅，如果找不到，则依ETV的物理范围找出
        /// 距离最近的车厅
        /// </summary>      
        public void AllocateEtvAndHall(IList<CSMG> Etvs, IList<CSMG> Halls, CLocation lct, out CSMG Etv, out CSMG Hall) 
        {
            Etv = null;
            Hall = null;            
            ETVList = Etvs;
            Dictionary<CSMG, CScope> dicWork = mScopeService.DicWorkScope;
            Dictionary<CSMG, CScope> dicMaxWork = mScopeService.DicMaxScope;

            #region 记录日志
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (CSMG esmg in Etvs)
                {
                    CScope workspe = dicWork[esmg];
                    CScope maxspe = dicMaxWork[esmg];
                    sb.AppendLine(string.Format("Etv" + esmg.ID + "  工作范围：" + workspe.LeftCol + "-" + workspe.RightCol + "  物理范围：" + maxspe.LeftCol + "-" + maxspe.RightCol));
                }
                new CWSException(sb.ToString(), 2);
            }
            catch
            { }
            #endregion

            IList<CSMG> EtvWorkList = AddressInEtvWorkScope(lct.Region, lct.Address, dicWork);
            if (EtvWorkList.Count > 0)
            {
                IList<CSMG> freeWorkList = FreeDeviceListOfWorkScope(EtvWorkList);
                if (freeWorkList.Count == 0)
                {
                    Etv = NearestEtvToAddress(lct.Address, EtvWorkList, lct.Region);
                }
                else if (freeWorkList.Count == 1)
                {
                    Etv = freeWorkList[0];
                }
                else //从空闲ETV集合中查找
                {
                    Etv = NearestEtvToAddress(lct.Address, freeWorkList, lct.Region);
                }
            }
            else   //从最大作业范围内找出
            {
                IList<CSMG> maxWorkList = AddressInEtvWorkScope(lct.Region, lct.Address, dicMaxWork);
                foreach (CSMG allocateEtv in maxWorkList) 
                {
                    if (new CWSMG().CheckEtvMode(allocateEtv.ID)) 
                    {
                        Etv = allocateEtv;
                        break;
                    }
                }
            }
            //找好ETV后，找出合适的车厅
            if (Etv != null) 
            {
                CScope scope = dicWork[Etv];
                //先分配在此区域内的车厅，如果两个车厅都不可达，才能考虑跨区分配
                IList<CSMG> availHalls = NearHallLists(scope, Halls,lct.Address,lct.Region);
                if (availHalls.Count > 0)
                {
                    IList<CSMG> freeHalls = new List<CSMG>();
                    foreach (CSMG hall in availHalls) 
                    {
                        if (hall.nIsWorking == 0) 
                        {
                            freeHalls.Add(hall);
                        }
                    }
                    if (freeHalls.Count > 0)
                    {
                        Hall = freeHalls[0];
                    }
                    else 
                    {
                        Hall = availHalls[0];
                    }
                }
                //ETV工作范围内不包含可用的出车厅，则从物理范围内查找
                if (Hall == null) 
                {
                    CScope mScope = dicMaxWork[Etv];
                    IList<CSMG> maxHalls = NearHallLists(mScope, Halls, lct.Address, lct.Region);
                    if (maxHalls.Count > 0) 
                    {
                        Hall = maxHalls[0];
                    }
                }
            }
        }

        /// <summary>
        /// 找出ETV作业范围内包含的出车厅的集合
        /// </summary>
        /// <param name="eScope">etv的作业范围</param>
        /// <param name="halls">车厅集合</param>
        /// <param name="address">车位地址</param>
        /// <param name="region">车位区域</param>
        /// <returns></returns>
        private IList<CSMG> NearHallLists(CScope eScope, IList<CSMG> halls,string address,int region) 
        {
            IList<CSMG> lists = new List<CSMG>();
            foreach (CSMG hall in halls) 
            {
                int col = clcCommom.GetColOfAddress(hall.Address);
                if (col >= eScope.LeftCol && col <= eScope.RightCol)
                {
                    lists.Add(hall);
                }
            }
            if (lists.Count > 1) 
            {
                return SortedHallByAddress(address, region, lists);
            }
            return lists;
        }
    }
}

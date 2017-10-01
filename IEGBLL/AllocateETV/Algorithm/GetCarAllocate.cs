using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;


namespace IEGBLL.AllocateETV
{
    public class GetCarAllocate:AbsAllocate
    {
        public GetCarAllocate() 
        {
        }
        /// <summary>
        /// 厅外刷卡取车：先从ETV工作范围内找出可达车位的ETV集合(依工作区域大小排序)，再找出空闲的ETV集合
        /// 先从空闲ETV集合中找出可达出车厅的ETV，如果找不到，再从工作范围内找出车位的ETV集合找出可达出车厅的ETV
        /// 再找不到的，则从最大作业范围内找出可达车位的ETV集合，再从中找出可达车厅的且是可用的ETV
        /// </summary>        
        public int AllocateEtv(CLocation lct,string hall_Address, IList<CSMG> Etvs) 
        {
            int eID = 0;
            int hallCol = clcCommom.GetColOfAddress(hall_Address);

            ETVList = Etvs;
            Dictionary<CSMG, CScope> workScope = mScopeService.DicWorkScope;
            Dictionary<CSMG, CScope> maxWorkScope = mScopeService.DicMaxScope;

            #region 记录日志
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (CSMG esmg in Etvs)
                {
                    CScope workspe = workScope[esmg];
                    CScope maxspe = maxWorkScope[esmg];
                    sb.AppendLine(string.Format("Etv" + esmg.ID + "  工作范围：" + workspe.LeftCol + "-" + workspe.RightCol + "  物理范围：" + maxspe.LeftCol + "-" + maxspe.RightCol));
                }
                new CWSException(sb.ToString(), 2);
            }
            catch
            { }
            #endregion

            //找出工作范围内可达车位的ETV集合
            IList<CSMG> EtvWorkList = AddressInEtvWorkScope(lct.Region,lct.Address, workScope);
            if (EtvWorkList.Count > 0)
            {
                IList<CSMG> FreeEtvList = FreeDeviceListOfWorkScope(EtvWorkList);
                if (FreeEtvList.Count == 0)
                {
                    //无空闲的ETV，从工作范围可达车位的ETV集合中找出可达出车厅的ETV
                    eID = FindEtvScopeInLocation(EtvWorkList, workScope, hall_Address);
                }
                else if (FreeEtvList.Count == 1)
                {
                    CSMG etv = FreeEtvList[0];
                    CScope scope = workScope[etv];
                    if (hallCol >= scope.LeftCol && hallCol <= scope.RightCol)
                    {
                        eID = etv.ID;
                    }
                    else
                    {
                        eID = FindEtvScopeInLocation(EtvWorkList, workScope, hall_Address);
                    }
                }
                else
                {
                    foreach (CSMG smg in FreeEtvList)
                    {
                        CScope scope = workScope[smg];
                        if (hallCol >= scope.LeftCol && hallCol <= scope.RightCol)
                        {
                            eID = smg.ID;
                            break;
                        }
                    }
                }
            }

            //如果从工作范围内找不到可达出车厅的ETV，则从物理作业范围内找出可用的ETV
            if (eID == 0)
            {               
                IList<CSMG> maxWorkEtvList = AddressInEtvWorkScope(lct.Region,lct.Address, maxWorkScope); //到达车位
                if (maxWorkEtvList.Count > 0)
                {
                    eID = FindEtvScopeInLocation(maxWorkEtvList, maxWorkScope, hall_Address); //到达车厅
                }
            }

            //找出模式为全自动的，强制分配
            if (eID == 0)
            {
                CWSMG cwsmg = new CWSMG();
                List<CSMG> etvsList = new List<CSMG>();
                var lastList = from ee in Etvs
                               where cwsmg.CheckEtvMode(ee.ID) == true
                               orderby Math.Abs(ee.Region - lct.Region)
                               select ee;
                etvsList.AddRange(lastList.ToList());
                if (etvsList.Count > 1)
                {
                    eID = etvsList[0].ID;
                }               
            }
            return eID;
        }
    }
}

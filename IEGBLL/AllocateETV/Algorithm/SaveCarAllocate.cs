using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public class SaveCarAllocate:AbsAllocate
    {
        private CSMG pHall = null;
        private IList<CLocation> locations = null;
        private string carsize = "";

        public SaveCarAllocate() 
        { }       

        /// <summary>
        /// 临时卡/定期卡 分配原则：先从ETV的作业范围找出可达车厅的ETV集合，从集合中找出与车厅区域一致的ETV
        /// 依ETV作业范围，找出适合车型的最近的车位。
        /// </summary>
        /// <param name="smgID">移动设备ID</param>
        /// <param name="CarSize">车辆外形</param>
        /// <param name="hall">当前存车车厅</param>
        /// <param name="lcts">所有车位集合</param>
        /// <param name="SMGs">所有移动设备集合</param>
        /// <returns></returns>
        public CLocation Allocate(out int smgID, string CarSize, CSMG hall, IList<CLocation> lcts, IList<CSMG> SMGs)
        {
            pHall = hall;
            carsize = CarSize;
            locations = lcts;
            ETVList = SMGs;   //建立ETV工作区域

            CLocation lct = null;
            CSMG smg = null;
            smgID = 0;
            Dictionary<CSMG, CScope> workScope = mScopeService.DicWorkScope;
            Dictionary<CSMG, CScope> maxWorkScope = mScopeService.DicMaxScope;
            #region 记录日志
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (CSMG esmg in SMGs)
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
            //获取工作范围内能到达车厅列的ETV的集合
            IList<CSMG> workEtvList = AddressInEtvWorkScope(pHall.Region, pHall.Address, workScope);
            if (workEtvList.Count > 0)
            {
                AllocateLctAndEtv(out smg, out lct, workEtvList, workScope, false);  //先找ETV，再找车位的
            }
            if (lct == null || smg == null)   //工作范围内没有可达的ETV,则从最大工作范围内查找
            {
                IList<CSMG> maxWorkList = AddressInEtvWorkScope(pHall.Region, pHall.Address, maxWorkScope);
                if (maxWorkList.Count > 0)
                {
                    AllocateLctAndEtv(out smg, out lct, maxWorkList, maxWorkScope, true);
                }
            }
            //如果再找不到的话，则判断当前ETV集合，找出模式为全自动的，强制分配，车位也依全库分配
            CWSMG cwsmg = new CWSMG();
            if (lct == null || smg == null)
            {
                List<CSMG> etvsList = new List<CSMG>();
                var lastList = from ee in SMGs
                               where cwsmg.CheckEtvMode(ee.ID) == true&&
                                     ee.Available==true
                               orderby Math.Abs(ee.Region - hall.Region)
                               select ee;
                etvsList.AddRange(lastList.ToList());
                foreach(CSMG se in lastList)
                {                    
                    CScope scope = maxWorkScope[se];
                    lct = this.AllocateLocation(scope, true);
                    if (lct != null) 
                    {
                        smg = se;
                        break;
                    }
                }
            }
            if (smg != null)
            {
                smgID = smg.ID;
            }
            return lct;
        }       

        /// <summary>
        /// 分配车位、ETV
        /// </summary>       
        private void AllocateLctAndEtv(out CSMG etv,out CLocation lct, IList<CSMG> workList, Dictionary<CSMG, CScope> dicWorkScope,bool isMax)
        {
            etv = null;
            lct = null;           
            IList<CSMG> freeEtvList = FreeDeviceListOfWorkScope(workList);
            int count = freeEtvList.Count;
            switch (count) 
            {
                case 0:
                    FindNearestEtvAndLct(out etv,out lct, workList,dicWorkScope,isMax);
                    break;
                case 1:
                    etv = freeEtvList[0];
                    lct = AllocateLocation(dicWorkScope[etv],isMax);                    
                    break;
                case 2:
                    FindNearestEtvAndLct(out etv,out lct,freeEtvList,dicWorkScope,isMax);
                    break;
                default:                   
                    break;
            }           
        }

        /// <summary>
        /// 找出离车厅最近的可用ETV
        /// </summary>  
        private bool FindNearestEtvAndLct(out CSMG etv, out CLocation lct, IList<CSMG> EtvList, Dictionary<CSMG, CScope> dicScope,bool isMax) 
        {
            etv = null;
            lct = null;
            CWSMG cwsmg = new CWSMG();
            IList<CSMG> fNeareasEtvs = SortedByAddress(pHall.Address,EtvList,pHall.Region); //排序
            if (fNeareasEtvs.Count == 0) 
            {              
                return false;
            }
            foreach (CSMG smg in fNeareasEtvs) 
            {
                if (cwsmg.CheckEtvMode(smg.ID))
                {                    
                    lct = AllocateLocation(dicScope[smg],isMax);
                    if (lct != null)
                    {
                        etv = smg;
                        return true;
                    }
                }
            }          
            return false;
        }

        /// <summary>
        /// 依ETV作业区域找出区域内最近的车位
        /// </summary>
        /// <param name="scope">当前ETV作业区域</param>
        /// <returns></returns>
        private CLocation AllocateLocation(CScope scope,bool isMax) 
        {
            int hallCol = clcCommom.GetColOfAddress(pHall.Address);
            List<CLocation> locationList = new List<CLocation>();
            //同区域低车
            var lcta = from lct in locations
                       where lct.Type==CLocation.EnmLocationType.Normal&&                       
                       (lct.List >= scope.LeftCol && lct.List <= scope.RightCol) &&
                       lct.Status == CLocation.EnmLocationStatus.Space &&                       
                       lct.Region == pHall.Region &&
                       string.Compare(lct.Size, carsize) == 0
                       orderby lct.Index ascending
                       select lct;
            locationList.AddRange(lcta);
            //不同区域低车
            var lctb = from lct in locations
                       where lct.Type == CLocation.EnmLocationType.Normal &&
                       (lct.List >= scope.LeftCol && lct.List <= scope.RightCol) &&
                       lct.Status == CLocation.EnmLocationStatus.Space &&
                       lct.Region != pHall.Region &&
                       string.Compare(lct.Size, carsize) == 0
                       orderby Math.Abs(hallCol-lct.List), lct.Index ascending
                       select lct;
            locationList.AddRange(lctb);

            if (isMax)
            {
                //同区域高车
                var lctc = from lct in locations
                           where lct.Type == CLocation.EnmLocationType.Normal &&
                           (lct.List >= scope.LeftCol && lct.List <= scope.RightCol) &&
                           lct.Status == CLocation.EnmLocationStatus.Space &&
                           lct.Region == pHall.Region &&
                           string.Compare(lct.Size, carsize) > 0
                           orderby lct.Index ascending
                           select lct;
                locationList.AddRange(lctc);
                //不同区域高车
                var lctd = from lct in locations
                           where lct.Type == CLocation.EnmLocationType.Normal &&
                           (lct.List >= scope.LeftCol && lct.List <= scope.RightCol) &&
                           lct.Status == CLocation.EnmLocationStatus.Space &&
                           lct.Region != pHall.Region &&
                           string.Compare(lct.Size, carsize) > 0
                           orderby Math.Abs(hallCol - lct.List), lct.Index ascending
                           select lct;
                locationList.AddRange(lctd);
            }
            //取第一个车位
            if (locationList.Count > 0) 
            {
                foreach (CLocation cltn in locationList) 
                {
                    if (cltn.Status == CLocation.EnmLocationStatus.Space && cltn.Type == CLocation.EnmLocationType.Normal) 
                    {
                        if (new CWICCard().FindFixICCard(cltn.Address) == null && cltn.ICCardCode == "")
                        {
                            return cltn;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 全库查找
        /// </summary>
        /// <returns></returns>
        private CLocation AllocateLocation(int region) 
        {
            CWICCard cwiccd=new CWICCard();
            List<CLocation> locationList = new List<CLocation>();

            var lcta = from lct in locations
                       where lct.Type == CLocation.EnmLocationType.Normal &&
                       lct.Status == CLocation.EnmLocationStatus.Space &&
                       string.Compare(lct.Size, carsize) == 0 &&
                       cwiccd.FindFixICCard(lct.Address) == null&&
                       lct.ICCardCode==""
                       orderby  Math.Abs(lct.Region-region), lct.Index ascending
                       select lct;
            locationList.AddRange(lcta);

            var lctb = from lct in locations
                       where lct.Type == CLocation.EnmLocationType.Normal &&
                       lct.Status == CLocation.EnmLocationStatus.Space &&
                       string.Compare(lct.Size, carsize) > 0 &&
                       cwiccd.FindFixICCard(lct.Address) == null&&
                        lct.ICCardCode == ""
                       orderby Math.Abs(lct.Region - region), lct.Index ascending
                       select lct;
            locationList.AddRange(lctb);

            if (locationList.Count > 0) 
            {
                return locationList[0];
            }
            return null;
        }

        /// <summary>
        /// 固定卡存车：可达进车厅的Etv集合，找出空闲的，可达车位的ETV
        /// </summary>
        /// <param name="hall"></param>
        /// <param name="etvs"></param>
        /// <returns></returns>
        public int AllocateEtv(CSMG hall,CLocation toLct, IList<CSMG> etvs) 
        {
            int etvID = 0;
            int lctCol = clcCommom.GetColOfAddress(toLct.Address);

            ETVList = etvs;           
            Dictionary<CSMG, CScope> workScope = mScopeService.DicWorkScope;

            IList<CSMG> workETVList = AddressInEtvWorkScope(hall.Region, hall.Address, workScope);
            if (workETVList.Count > 0)
            {
                IList<CSMG> freeEtvList = FreeDeviceListOfWorkScope(workETVList);
                if (freeEtvList.Count == 0)
                {
                    //无空闲ETV,则从工作范围内找出离车厅最近且可达车位的ETV
                    etvID = FindEtvScopeInLocation(workETVList, workScope, toLct.Address);
                }
                else if (freeEtvList.Count == 1)
                {
                    CSMG etv = freeEtvList[0];
                    CScope scope = mScopeService.DicWorkScope[etv];
                    if (scope.LeftCol <= lctCol && scope.RightCol >= lctCol)
                    {
                        etvID = etv.ID;
                    }
                    else
                    {
                        etvID = FindEtvScopeInLocation(workETVList, workScope, toLct.Address);
                    }
                }
                else
                {
                    foreach (CSMG smg in freeEtvList)
                    {
                        CScope scope = mScopeService.DicWorkScope[smg];
                        if (scope.LeftCol <= lctCol && scope.RightCol >= lctCol)
                        {
                            etvID = smg.ID;
                            break;
                        }
                    }
                }
            }
           
            //如果工作范围内找不出可达车位的ETV，则从物理范围内找出可达的
            if (etvID == 0) 
            {
                Dictionary<CSMG, CScope> maxWorkScope = mScopeService.DicMaxScope;
                IList<CSMG> maxWorkEtvList = AddressInEtvWorkScope(hall.Region, hall.Address, maxWorkScope); //找出能到达车厅的
                if (maxWorkEtvList.Count > 0)
                {
                    etvID = FindEtvScopeInLocation(maxWorkEtvList, maxWorkScope, toLct.Address); //找出能到达车位的
                }
            }

            //找出模式为全自动的，强制分配
            if (etvID == 0)
            {
                CWSMG cwsmg = new CWSMG();
                List<CSMG> etvsList = new List<CSMG>();
                var lastList = from ee in etvs
                               where cwsmg.CheckEtvMode(ee.ID) == true
                               orderby Math.Abs(ee.Region - hall.Region)
                               select ee;
                etvsList.AddRange(lastList.ToList());
                if (etvsList.Count > 1)
                {
                    etvID = etvsList[0].ID;
                }                  
            }
            return etvID;
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public abstract class AbsAllocate
    {
        private IList<CSMG> mETVList = null;
        protected EtvServiceScope mScopeService=null;

        protected IList<CSMG> ETVList 
        {
            set 
            {
                mETVList = value;
                if (mScopeService == null)
                {
                    mScopeService = new EtvServiceScope(mETVList);
                }
                else 
                {
                    mScopeService.EtvList = mETVList;
                }
            }
        }

        /// <summary>
        /// 获取作业范围内包含该车地址的所有ETV集合,依区域近远排序
        /// </summary>
        /// <param name="lctAddrs"></param>
        /// <param name="dicWorkScope"></param>
        /// <returns></returns>
        protected IList<CSMG> AddressInEtvWorkScope(int region,string lctAddrs,Dictionary<CSMG,CScope> dicScope) 
        {
            try
            {
                List<CSMG> etvList = new List<CSMG>();
                int lctCol = clcCommom.GetColOfAddress(lctAddrs);
                foreach (KeyValuePair<CSMG, CScope> pair in dicScope)
                {
                    if (pair.Value.LeftCol <= lctCol && lctCol <= pair.Value.RightCol)
                    {
                        etvList.Add(pair.Key);
                    }
                }
                var eList = from etv in etvList
                           orderby Math.Abs(etv.Region - region)
                           select etv; 
               
                return eList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 依所有可达该地址的ETV集合，找出空闲的ETV集合
        /// </summary>
        /// <param name="pWorkList"></param>
        /// <returns></returns>
        protected IList<CSMG> FreeDeviceListOfWorkScope(IList<CSMG> pWorkList) 
        {
            try 
            {
                CWSMG cwsmg = new CWSMG();
                IList<CSMG> devices = new List<CSMG>();
                foreach (CSMG smg in pWorkList) 
                {
                    if (cwsmg.CheckEtvMode(smg.ID) && smg.nIsWorking == 0)
                    {
                        devices.Add(smg);
                    }
                }
                return devices;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从ETV集合中，找出作业范围包含车位地址的ETV
        /// </summary>
        /// <param name="EtvLists">ETV集合</param>
        /// <param name="dicScope">ETV的作业范围集合</param>
        /// <param name="address">要包含的地址</param>
        /// <returns></returns>
        protected int FindEtvScopeInLocation(IList<CSMG> EtvLists, Dictionary<CSMG, CScope> dicScope, string address) 
        {
            try 
            {
                CWSMG cwsmg = new CWSMG();
                int col = clcCommom.GetColOfAddress(address);
                foreach (CSMG smg in EtvLists) 
                {
                    if (cwsmg.CheckEtvMode(smg.ID)) 
                    {
                        CScope scope = dicScope[smg];
                        if (col >= scope.LeftCol && col <= scope.RightCol)
                        {
                            return smg.ID;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex) 
            {
                throw ex;
            }           
        }

        /// <summary>
        /// 查找距离车位最近的移动设备
        /// </summary>
        /// <param name="pAddress"></param>
        /// <param name="deviceList"></param>
        /// <returns></returns>
        protected CSMG NearestEtvToAddress(string pAddress, IList<CSMG> deviceList,int region)
        {
            try
            {
                if (deviceList.Count == 0)
                {
                    return null;
                }
                else
                {
                    IList<CSMG> pEtvList = SortedByAddress(pAddress, deviceList, region);
                    return pEtvList[0];
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 找出与距离目标车位一致的最近的ETV集合
        /// </summary>
        /// <param name="pAddress">目标车位</param>
        /// <param name="pDeviceList">设备集合</param>
        /// <param name="region">目标车位的区域</param>
        /// <returns></returns>
        protected IList<CSMG> SortedByAddress(string pAddress,IList<CSMG> pDeviceList,int region)
        {
            var devices = from dev in pDeviceList
                          orderby Math.Abs(dev.Region - region)                           
                          select dev;
             return devices.ToList();
        }

        /// <summary>
        /// 将车厅集合进行排序
        /// </summary>
        /// <param name="address"></param>
        /// <param name="region"></param>
        /// <param name="pHalls"></param>
        /// <returns></returns>
        protected IList<CSMG> SortedHallByAddress(string address, int region, IList<CSMG> pHalls) 
        {
            var devices = from dev in pHalls
                          orderby Math.Abs(dev.Region - region),
                                  dev.HallType ascending,
                                  Math.Abs((clcCommom.GetColOfAddress(address) - clcCommom.GetColOfAddress(dev.Address)))
                          select dev;
            return devices.ToList();
        }
    }
}

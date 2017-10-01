using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;
using System.Web;
using System.Web.Caching;

namespace IEGBLL
{
    public class CWLocation
    {
        public CWLocation() 
        { }

        private List<CLocation> Locations 
        {
            get
            {
                List<CLocation> lcts = (List<CLocation>)HttpRuntime.Cache["CLocations"];
                if (lcts == null) 
                {
                    lcts = CWData.myDB.LoadLocations();
                    CWICCard cwiccd = new CWICCard();
                    foreach (CLocation lct in lcts) 
                    {
                        CICCard iccd = cwiccd.SelectByUserCode(lct.ICCardCode);  //创建新对象                       
                        lct.SetICCard(iccd);  //对象间引用
                    }
                    lcts.Sort();
                    HttpRuntime.Cache.Add("CLocation",lcts,null,DateTime.Now.AddHours(CWData.Timeout),Cache.NoSlidingExpiration,CacheItemPriority.High,null);                    
                }
                return lcts;
            }
        }

        /// <summary>
        /// 依用户卡号查询存车车位
        /// </summary>        
        public CLocation SelectLctFromICCode(string iccode) 
        {
            lock (Locations)
            {
                return Locations.Find(delegate(CLocation lct) { return lct.ICCardCode == iccode
                    &&(lct.Type==CLocation.EnmLocationType.Normal||
                    lct.Type==CLocation.EnmLocationType.Disable); });
            }
        }

        /// <summary>
        /// 依车位地址查找对应车位
        /// </summary>      
        public CLocation SelectLctFromAddrs(string addrs) 
        {
            return Locations.Find(delegate(CLocation lct) { return lct.Address == addrs; });
        }

        /// <summary>
        /// 查找所有车位信息，重新加载下数据库吗？
        /// </summary>
        /// <returns></returns>
        public CLocation[] SelectAllLocations() 
        {
            lock (Locations)
            {
                return Locations.ToArray();
            }
        }

        /// <summary>
        /// 查找空余车位
        /// </summary>       
        public int SelectSpaceLctCounts(out int total, out int space, out int occupy, out int fixLct,out int spaceBigLct) 
        {
            total=0;
            space=0;
            occupy=0;
            fixLct=0;
            spaceBigLct = 0;

            try
            {
                CWICCard wiccd = new CWICCard();
                foreach (CLocation lct in Locations)
                {
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status != CLocation.EnmLocationStatus.Init)
                    {
                        total++;
                    }
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status == CLocation.EnmLocationStatus.Space)
                    {
                        if (wiccd.FindFixICCard(lct.Address) == null)
                        {
                            space++;
                        }
                    }
                    if (lct.Type == CLocation.EnmLocationType.Normal && (lct.Status != CLocation.EnmLocationStatus.Init && lct.Status != CLocation.EnmLocationStatus.Space))
                    {
                        occupy++;
                    }
                    if (lct.Type == CLocation.EnmLocationType.Normal || lct.Type == CLocation.EnmLocationType.Disable)
                    {
                        if (wiccd.FindFixICCard(lct.Address) != null)
                        {
                            fixLct++;
                        }
                    }
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status == CLocation.EnmLocationStatus.Space && lct.Size == "112")
                    {
                        if (wiccd.FindFixICCard(lct.Address) == null)
                        {
                            spaceBigLct++;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }
            return 0;
        }       

        /// <summary>
        /// 车位数据手动出库
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int ManualOutLocation(string address) 
        {
            try
            {
                CLocation lct = Locations.Find(c => c.Address == address);
                if (lct != null)
                {
                    if (lct.Type == CLocation.EnmLocationType.Normal)
                    {
                        if (lct.Status != CLocation.EnmLocationStatus.Space)
                        {
                            lct.Status = CLocation.EnmLocationStatus.Space;
                            lct.SetICCard(null);
                            lct.CarSize = "";
                            lct.Distance = 0;
                            lct.InDate = CObject.DefDatetime;

                            string msg = "数据出库- 车位：" + lct.Address + " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                            new CWSException(msg, 0);

                            CWData.myDB.ManUpdateLocation(lct);
                            return 100;
                        }
                        else
                        {
                            return 103;
                        }
                    }
                    else
                    {
                        return 102; //
                    }
                }
                return 101; //找不到车位
            }
            catch (Exception ex) 
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }
        }

        /// <summary>
        /// 手动开启、禁用车位
        /// </summary>
        /// <param name="addrs">车位地址</param>
        /// <param name="isDis">true:禁用，false:启用</param>
        /// <returns></returns>
        public int ManualDisLoction(string addrs,bool isDis)
        {
            try
            {
                CLocation lct = this.SelectLctFromAddrs(addrs);
                if (lct != null)
                {
                    if (lct.Type != CLocation.EnmLocationType.Hall && lct.Status != CLocation.EnmLocationStatus.Init)
                    {
                        if (isDis == true) //禁用
                        {
                            lct.Type = CLocation.EnmLocationType.Disable;
                        }
                        else //启用
                        {
                            lct.Type = CLocation.EnmLocationType.Normal;
                        }
                        CWData.myDB.ManDisableLocation(lct, lct.Type);
                        return 100;
                    }
                    else
                    {
                        return 102;
                    }
                }
                else
                {
                    return 101;
                }
            }
            catch (Exception ex)
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }
        }

        /// <summary>
        /// 车位数据的手动挪移
        /// </summary>
        /// <param name="fAddrs"></param>
        /// <param name="tAddrs"></param>
        /// <returns></returns>
        public int ManualTransposeLocation(string fAddrs, string tAddrs) 
        {
            try
            {
                CLocation frLct = this.SelectLctFromAddrs(fAddrs);
                CLocation toLct = this.SelectLctFromAddrs(tAddrs);
                if (frLct != null && toLct != null)
                {
                    if (frLct.Type == CLocation.EnmLocationType.Normal && toLct.Type == CLocation.EnmLocationType.Normal)
                    {
                        if (frLct.Status != CLocation.EnmLocationStatus.Occupy)
                        {
                            return 103;
                        }
                        if (toLct.Status != CLocation.EnmLocationStatus.Space)
                        {
                            return 104;
                        }
                        if (frLct.CarSize.CompareTo(toLct.Size) > 0) 
                        {
                            return 105;
                        }
                        if (frLct.ICCardCode == "") 
                        {
                            return 103;
                        }
                        CICCard iccd = new CICCard();
                        iccd = new CWICCard().SelectByUserCode(frLct.ICCardCode);
                        toLct.SetICCard(iccd);

                        toLct.Status = CLocation.EnmLocationStatus.Occupy;                       
                        toLct.Distance = frLct.Distance;
                        toLct.CarSize = frLct.CarSize;
                        toLct.InDate = DateTime.Now;

                        string mss = "数据挪移- 目的车位：" + toLct.Address + " 状态：" + toLct.Status.ToString() + " 卡号：" + toLct.ICCardCode;
                        new CWSException(mss, 0);

                        frLct.Status = CLocation.EnmLocationStatus.Space;
                        frLct.SetICCard(null);
                        frLct.Distance = 0;
                        frLct.CarSize = "";
                        frLct.InDate = CObject.DefDatetime;

                        string msg = "数据挪移- 源车位：" + frLct.Address + " 状态：" + frLct.Status.ToString() + " 卡号：" + frLct.ICCardCode;
                        new CWSException(msg, 0);

                        CWData.myDB.ManTransportLocation(frLct, toLct);
                        return 100;
                    }
                    else
                    {
                        return 102; //车位不可用
                    }
                }
                else
                {
                    return 101;
                }
            }
            catch(Exception ex)
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }
        }

        /// <summary>
        /// 手动入库
        /// </summary>        
        public int ManInLocation(string addrs, string iccode, string carSize, int distance, DateTime InDate) 
        {
            try 
            {
                CLocation lct = this.SelectLctFromAddrs(addrs);
                CICCard iccd = new CICCard();   //在使用seticcard时是对象间赋值,应先生成新对象,
                iccd = new CWICCard().SelectByUserCode(iccode);
               
                if (lct == null || iccd == null) 
                {
                    return 101;   
                }
                if (iccd.Status != CICCard.EnmICCardStatus.Normal)
                {
                    return 106;
                }
                if (lct.Type != CLocation.EnmLocationType.Normal || lct.Status != CLocation.EnmLocationStatus.Space)
                {
                    return 102;  //该车位不合格
                }
                if (lct.ICCardCode != "") 
                {
                    return 103; //车位上有车
                }
                CLocation lctn = this.SelectLctFromICCode(iccd.Code);
                if (lctn != null) 
                {
                    return 104; //该卡已存车
                }
                if (carSize.CompareTo(lct.Size) > 0)
                {
                    return 105;//外形不合格
                }

                lct.SetICCard(iccd);
                lct.Distance = distance;
                lct.CarSize = carSize;
                lct.InDate = InDate;
                lct.Status = CLocation.EnmLocationStatus.Occupy;

                string msg = "数据入库- 源车位：" + lct.Address + " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                new CWSException(msg, 0);

                CWData.myDB.ManUpdateLocation(lct);
                return 100;                
            }
            catch (Exception ex) 
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }
        }

        /// <summary>
        /// 制固定卡时核查车位可用性
        /// </summary>
        /// <param name="addrs"></param>
        /// <returns></returns>
        public bool CheckFixLocation(string addrs) 
        {
            foreach (CLocation lct in Locations) 
            {
                if (lct.Type == CLocation.EnmLocationType.Normal || lct.Type == CLocation.EnmLocationType.Disable) 
                {
                    if (lct.Address == addrs) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// LED  查询库内空余车位及可分配大车位
        /// </summary>
        /// <param name="pSpace"></param>
        /// <param name="pBigSpace"></param>
        /// <returns></returns>
        public int GetSpaceLctsAndBigLcts(out int pSpace, out int pBigSpace)
        {
            pBigSpace = 0;
            pSpace = 0;
            try
            {
                CWICCard wiccd = new CWICCard();
                foreach (CLocation lct in Locations)
                {
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status == CLocation.EnmLocationStatus.Space)
                    {
                        if (wiccd.FindFixICCard(lct.Address) == null)
                        {
                            pSpace++;
                        }
                    }
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status == CLocation.EnmLocationStatus.Space && lct.Size == "112")
                    {
                        if (wiccd.FindFixICCard(lct.Address) == null)
                        {
                            pBigSpace++;
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }            
        }

        /// <summary>
        /// 查询空余车位，状态字用
        /// </summary>
        /// <returns></returns>
        public int GetSpaceLocations() 
        {
            try
            {
                int pSpace = 0;
                CWICCard wiccd = new CWICCard();
                foreach (CLocation lct in Locations)
                {
                    if (lct.Type == CLocation.EnmLocationType.Normal && lct.Status == CLocation.EnmLocationStatus.Space)
                    {
                        if (wiccd.FindFixICCard(lct.Address) == null)
                        {
                            pSpace++;
                        }
                    }
                }
                return pSpace;   
            }
            catch (Exception ex)
            {
                HttpRuntime.Cache.Remove("CLocations");
                throw ex;
            }           
        }
    }
}

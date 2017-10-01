using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;
using System.Web;
using System.Web.Caching;

namespace IEGBLL
{
    public class CWICCard
    {
        public CWICCard() 
        {
        }

        public List<CICCard> ICCards
        {
            get 
            {
                List<CICCard> iccds =(List<CICCard>)HttpRuntime.Cache["ICCards"];
                if (iccds == null) 
                {
                    iccds = CWData.myDB.LoadICCards();

                    HttpRuntime.Cache.Add("ICCards", iccds, null,DateTime.Now.AddHours(CWData.Timeout),Cache.NoSlidingExpiration,CacheItemPriority.High,null);
                }
                return iccds;
            }
        }

        /// <summary>
        /// 通过物理卡号查询卡
        /// </summary>       
        public CICCard SelectByPhysicCard(string physicCode) 
        {
            return ICCards.Find(delegate(CICCard iccd) { return iccd.PhysicCode == physicCode; });
        }

        /// <summary>
        /// 通过用户卡号查询卡
        /// </summary>       
        public CICCard SelectByUserCode(string ccode)
        {
            return ICCards.Find(delegate(CICCard iccd) { return iccd.Code == ccode; });
        }

        /// <summary>
        /// 查找固定车位卡号
        /// </summary>     
        public CICCard FindFixICCard(string lctAddrs) 
        {
            return ICCards.Find(delegate(CICCard iccd) { return iccd.Address == lctAddrs; });
        }

        /// <summary>
        /// 检查登录用户名及密码
        /// </summary>        
        public COperator CheckLoginPassword(string mCode, string mPassword) 
        {
            try
            {
                List<COperator> oprs = CWData.myDB.LoadOperators();
                if (oprs.Count > 0)
                {
                    foreach (COperator opr in oprs)
                    {
                        if (opr.Code == mCode && opr.Password == mPassword)
                        {
                            return opr;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载操作员信息
        /// </summary>
        /// <returns></returns>
        public List<COperator> LoadAllOperators() 
        {
            List<COperator> opertors = new List<COperator>();
            try 
            {
                opertors = CWData.myDB.LoadOperators();
                return opertors;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int ChangePassword(string name,string pwd) 
        {
            try
            {
                CWData.myDB.UpdateUserPassword(name, pwd);
                return 100;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加操作员信息
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public int InsertNewOperator(COperator opr) 
        {
            try
            {
                COperator oprt = new COperator();
                oprt.Code = opr.Code;
                oprt.Name = opr.Name;
                oprt.Password = opr.Password;
                oprt.Type = opr.Type;
                CWData.myDB.InsertOperator(oprt);
                return 100;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除操作员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int DeleteOperator(string code) 
        {
            try 
            {
                CWData.myDB.DeleteOperator(code);
                return 100;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回所有的用户
        /// </summary>
        /// <returns></returns>
        public CCustomer[] SelectAllCustomers() 
        {
            List<CCustomer> customers = CWData.myDB.LoadCustomers();
            foreach (CCustomer cust in customers) 
            {
                CICCard iccd = new CICCard();
                iccd=ICCards.Find(c => c.CustomerID == cust.ID);
                cust.SetICCard(iccd); 
            }
            return customers.ToArray();
        }

        /// <summary>
        /// 更新用户信息，如果原卡号与更新卡号不一致，则释放原卡号
        /// </summary>      
        public int UpdateCustomInfo(CCustomer nctm, CICCard nIccd, string oriCode) 
        {
            try 
            {
                CWLocation wlctn = new CWLocation();
                CICCard oriIccd = this.SelectByUserCode(oriCode); //原卡               

                if (oriIccd.Code != nIccd.Code) //原卡与新卡不一致
                {
                    if (nIccd.Status == CICCard.EnmICCardStatus.Lost || nIccd.Status == CICCard.EnmICCardStatus.Disposed) 
                    {
                        return 101; 
                    }
                    if (nIccd.CustomerID != 0) 
                    {
                        return 102; //一卡只允许一个人用
                    }                  
                }

                if (nIccd.Type==CICCard.EnmICCardType.FixedLocation && oriIccd.Address != nIccd.Address)  //车位不一致 
                {
                    if (this.FindFixICCard(nIccd.Address) != null)
                    {
                        return 104; //新卡已被其车位绑定
                    }
                }

                if (wlctn.SelectLctFromICCode(nIccd.Code) != null)
                {
                    return 103; //该卡已存车
                }

                if (oriIccd.Code != nIccd.Code)
                {
                    //释放新卡
                    oriIccd.CustomerID = 0;
                    oriIccd.DueDtime = CObject.DefDatetime;
                    oriIccd.Type = CICCard.EnmICCardType.Temp;
                    oriIccd.Address = "";
                    oriIccd.PlatNumber = "";

                    CICCard iccrd = this.SelectByUserCode(nIccd.Code);
                    iccrd = nIccd; //更新新卡号
                }
                else 
                {
                    oriIccd.PlatNumber = nIccd.PlatNumber;
                    oriIccd.DueDtime = nIccd.DueDtime;
                    oriIccd.Status = nIccd.Status;
                    oriIccd.Type = nIccd.Type;
                    oriIccd.Address = nIccd.Address;
                    oriIccd.CustomerID = nIccd.CustomerID;
                }
               
                CWData.myDB.UpdateCustAndICcard(nctm, oriIccd, nIccd);
               
                return 100;
            }
            catch (Exception ex) 
            {              
                new CWSException();
                throw ex;
            }
        }
        /// <summary>
        /// 增加新顾客
        /// </summary>     
        public int InsertCustomer(CCustomer nctm,CICCard iccd)
        {
            try
            {                
                CWLocation wlctn = new CWLocation();
                CICCard oicd = this.SelectByUserCode(iccd.Code);                

                if (oicd.CustomerID != 0 || oicd.Status == CICCard.EnmICCardStatus.Lost || oicd.Status == CICCard.EnmICCardStatus.Disposed) 
                {
                    return 101; //该卡不可用
                }
                if (wlctn.SelectLctFromICCode(oicd.Code) != null) 
                {
                    return 102; //该卡已用于存车
                }
                if (oicd.Type == CICCard.EnmICCardType.FixedLocation) 
                {
                    if (this.FindFixICCard(oicd.Address) != null)
                    {
                        return 103; //新卡已被其车位绑定
                    }
                }

                oicd.PlatNumber = iccd.PlatNumber;  //更新
                oicd.DueDtime = iccd.DueDtime;
                oicd.Type = iccd.Type;
                oicd.Status = iccd.Status;
                oicd.Address = iccd.Address;

                CWData.myDB.InsertCustomer(nctm, oicd);

                return 100;
            }
            catch (Exception ex) 
            {              
                new CWSException();
                throw ex;
            }
        }

        /// <summary>
        /// 删除顾客
        /// </summary>       
        public int DeleteCustomer(int custID,string ccode) 
        {
            try
            { 
                CICCard iccd = this.SelectByUserCode(ccode);
                CLocation lct = new CWLocation().SelectLctFromICCode(iccd.Code);
                if (lct != null) 
                {
                    return 101;  //该卡在存车
                }

                iccd.CustomerID = 0;
                iccd.DueDtime = CObject.DefDatetime;
                iccd.PlatNumber = "";
                iccd.Address = "";
                iccd.Type = CICCard.EnmICCardType.Temp;

                CWData.myDB.DeleteCustomer(iccd,custID);
                return 100;
            }
            catch (Exception ex) 
            {
                new CWSException();
                throw ex;
            }
        }

        /// <summary>
        /// 制卡
        /// </summary>
        /// <param name="iccd">新卡对象</param>
        /// <returns></returns>
        public int InsertICCard(CICCard iccd) 
        {
            try
            {
                CICCard iccd1 = this.SelectByPhysicCard(iccd.PhysicCode);  //物理卡号是否重复
                CICCard iccd2 = this.SelectByUserCode(iccd.Code);          //用户卡号是否重复

                if (iccd1 == null)
                {
                    if (iccd2 == null)
                    {
                        CWData.myDB.InsertICCard(iccd);
                        ICCards.Add(iccd);
                    }
                    //else if (iccd2.Status == CICCard.EnmICCardStatus.Disposed)  //用户卡号已注销，则卡号就可再利用-----------屏蔽，有问题
                    //{
                    //    CWData.myDB.InsertICCard(iccd);
                    //    ICCards.Add(iccd);
                    //}
                    else
                    {
                        return 101; //物理卡不存在，用户卡号已存在
                    }
                }
                else   //物理卡存在
                {                   
                    if (iccd2 == null)   //用户卡不存在,则更新
                    {
                        iccd1.Code = iccd.Code;
                        iccd1.Status = iccd.Status;
                        iccd1.Type = iccd.Type;
                        CWData.myDB.UpdateICCard(iccd1);
                    }
                    else  //用户卡存在
                    {
                        if (iccd1.Code == iccd2.Code)
                        {
                            iccd1.Status = iccd.Status;
                            iccd1.Type = iccd.Type;
                            CWData.myDB.UpdateICCard(iccd1);
                        }
                        else 
                        {
                            return 102; //物理卡存在，用户卡号存在
                        }
                    }
                }
                return 100;
            }
            catch (Exception ex)
            {
                new CWSException();
                throw ex;
            }
        }

        //挂失、注销卡
        public int UpdateICCardStatus(string code, CICCard.EnmICCardStatus status) 
        {
            try 
            {
                CICCard iccd = this.SelectByUserCode(code);
                if (iccd == null) 
                {
                    return 101;
                }
                if (status == CICCard.EnmICCardStatus.Lost) 
                {
                    iccd.LossDtime = DateTime.Now;
                }
                else if (status == CICCard.EnmICCardStatus.Disposed) 
                {
                    iccd.DisposeDtime = DateTime.Now;
                }
                else if (status == CICCard.EnmICCardStatus.Normal) 
                {
                    iccd.LossDtime = CObject.DefDatetime;
                    iccd.DisposeDtime = CObject.DefDatetime;
                }
                iccd.Status = status;
                CWData.myDB.UpdateIccardStat(iccd);
                return 100;
            }
            catch (Exception ex) 
            {
                new CWSException();
                throw ex;
            }
        }

        #region 收费
        /// <summary>
        /// 查询临时卡收费
        /// </summary>
        /// <param name="iccode">卡号</param>
        /// <param name="tempLog">收费对象</param>
        /// <returns></returns>
        public int GetTempCardUserInfo(string iccode,out CTempCardChargeLog tempLog)
        {
            tempLog = null;
            CICCard iccd = this.SelectByUserCode(iccode);
            if (iccd == null) 
            {
                return 101; //不是本系统用卡
            }
            if (iccd.Type != CICCard.EnmICCardType.Temp) 
            {
                return 102; //该卡不是临时卡
            }
          
            CLocation lct=new CWLocation().SelectLctFromICCode(iccode);
            if (lct == null) 
            {
                return 103; //该卡没有存车
            }
            CMasterTask mtsk = new CWTask().GetMasterTaskFromICCode(iccode);
            if (mtsk != null) 
            {
                return 104; //该卡正在作业
            }

            tempLog = new CTempCardChargeLog();
            tempLog.ICCode = iccode;
            tempLog.LocationAddress = lct.Address;
            tempLog.InDate = lct.InDate;
            tempLog.OutDate = DateTime.Now;

            tempLog.RecivFee =new CWNewTrariff().CalculateTempFee(tempLog.InDate, tempLog.OutDate);
            return 100;
        }

        /// <summary>
        /// 固定卡的临时收费
        /// </summary>
        /// <param name="iccode"></param>
        /// <param name="fxLog"></param>
        /// <returns></returns>
        public int GetFixCardCurrentFee(string iccode, out CFixCardChargeLog fxLog) 
        {
            fxLog = null;
            CICCard iccd = this.SelectByUserCode(iccode);
            if (iccd == null)
            {
                return 101; //不是本系统用卡
            }
            if (iccd.Type != CICCard.EnmICCardType.Temp)
            {
                return 102; //该卡不是临时卡
            }
            CLocation lct = new CWLocation().SelectLctFromICCode(iccode);
            if (lct == null)
            {
                return 103; //该卡没有存车
            }
            fxLog = new CFixCardChargeLog();
            fxLog.ICCode = iccd.Code;
            fxLog.InDate = lct.InDate;
            fxLog.RecivFee = new CWTariff().CalculateCardFee(DateTime.Now - fxLog.InDate);
            return 100;
        }

        /// <summary>
        /// 更新缴费
        /// </summary>
        /// <param name="nfcdlog"></param>
        /// <returns></returns>
        public int SetFixCardFee(CFixCardChargeLog nfcdlog)
        {
            try
            {
                CICCard iccd = this.SelectByUserCode(nfcdlog.ICCode);
                iccd.DueDtime = nfcdlog.DueDtime;               
                CWData.myDB.UpdateICCardAndLog(iccd, nfcdlog);
                return 100;
            }
            catch (Exception e1)
            {
                new CWSException();                
                throw e1;
            }
        }


        #endregion

    }
}

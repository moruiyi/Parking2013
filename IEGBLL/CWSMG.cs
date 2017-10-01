using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;
using System.Web;
using System.Web.Caching;

namespace IEGBLL
{
    public class CWSMG
    {       
        private static CStatCode[] EqpStates = null;
        private static Dictionary<int, List<CErrorCode>> dicErrorCode = null;

        public CWSMG() 
        {
        }

        //private static List<CSMG> smgsList;
        //public List<CSMG> SMGs
        //{
        //    get
        //    {
        //        if (smgsList == null)
        //        {
        //            smgsList = CWData.myDB.LoadSMGs();
        //        }
        //        return smgsList;
        //    }
        //}

        public List<CSMG> SMGs
        {
            get
            {
                List<CSMG> lstSMGs = (List<CSMG>)HttpRuntime.Cache["SMGs"];
                if (lstSMGs == null)
                {
                    lstSMGs = CWData.myDB.LoadSMGs();
                    //加载到缓存中
                    HttpRuntime.Cache.Add("SMGs", lstSMGs, null, DateTime.Now.AddHours(CWData.Timeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
                return lstSMGs;
            }
        }       

        /// <summary>
        /// 判断设备的可用性
        /// </summary>      
        public bool CheckSMGAvail(string code) 
        {
            bool isAvail = false;
            foreach (CSMG smg in SMGs) 
            {
                if (smg.Code == code) 
                {
                    if (smg.Available == true)
                    {
                        isAvail = true;
                    }
                    else 
                    {
                        isAvail = false;
                    }
                    break;
                }
            }
            return isAvail;
        }

        /// <summary>
        /// 查找对应设备
        /// </summary>       
        public CSMG SelectSMG(int id) 
        {            
            return SMGs.Find(delegate(CSMG smg) { return smg.ID == id; });
        }

        /// <summary>
        /// 获取所有的设备
        /// </summary>
        /// <returns></returns>
        public CSMG[] GetAllSMGs() 
        {
            return SMGs.ToArray();
        }

        /// <summary>
        /// 判断车厅模式是否为全自动
        /// </summary>       
        public bool CheckHallMode(int hallID)
        {            
            bool isAuto=false;
            if (EqpStates != null)
            {
                CStatCode[] statusInfo = this.SelectStatusCodes(hallID);

                if (statusInfo != null && statusInfo[4].CurrentValue == 4)
                {
                    isAuto = true;
                }
            }
            return isAuto;
        }

        /// <summary>
        /// 判断ETV模式是否为全自动
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public bool CheckEtvMode(int eid) 
        {
            bool isAuto = false;
            if(EqpStates != null) 
            {
                CStatCode[] codes = this.SelectStatusCodes(eid);
                if (codes != null && codes[10].CurrentValue == 4) 
                {
                    return true;
                }
            }
            return isAuto;
        }

        /// <summary>
        /// 更新报警信息-20141203增
        /// </summary>
        /// <param name="errCodeAddrs">报警位为1的位地址的集合</param>
        public void UpdateErrorAlarm(short[] errCodeAddrs, int scNo)
        {
            try
            {
                if (dicErrorCode == null)
                {
                    dicErrorCode = new Dictionary<int, List<CErrorCode>>();
                    foreach (CSMG smg in SMGs)
                    {
                        List<CErrorCode> errs = CWData.myDB.LoadErrorCodes(smg.ID);
                        dicErrorCode.Add(smg.ID, errs);
                    }
                }
                if (dicErrorCode.ContainsKey(scNo))
                {
                    List<CErrorCode> errCodes = dicErrorCode[scNo];
                    if (errCodes != null)
                    {
                        foreach (CErrorCode err in errCodes)
                        {
                            if (Array.Exists(errCodeAddrs, delegate(short rr) { return rr == err.StartBit; }))
                            {
                                err.CurrentValue = 1;
                            }
                            else
                            {
                                err.CurrentValue = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实时保存ETV坐标
        /// </summary>
        public void UpdateEtvCurrAddress() 
        {
            try
            {
                foreach (CSMG smg in SMGs) 
                {
                    if (smg.SMGType == CSMG.EnmSMGType.ETV) 
                    {            
                        if (smg.ID == 1)
                        {
                            smg.CurrAddress = "1011";
                        }
                        else 
                        {
                            smg.CurrAddress = "1201";
                        }                 
                        CStatCode[] codes = this.SelectStatusCodes(smg.ID);
                        if (codes != null) 
                        {
                            if (codes[1].CurrentValue != 0 && (codes[1].CurrentValue > 0 && codes[1].CurrentValue < 21))
                            {
                                string line = codes[0].CurrentValue.ToString();
                                string layer = codes[2].CurrentValue.ToString();
                                string list = "";
                                if (codes[1].CurrentValue < 10)
                                {
                                    list = "0" + codes[1].CurrentValue.ToString();
                                }
                                else
                                {
                                    list = codes[1].CurrentValue.ToString();
                                }
                                smg.CurrAddress = string.Concat(line, list, layer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 依设备号返回报警描述信息
        /// </summary>       
        public CErrorCode[] LoadErrorCodeDescp(int type)
        {
            try
            {
                CErrorCode[] errorCodes = CWData.myDB.LoadErrCodesDesc(type);
                return errorCodes;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        #region 用于故障记录
        /// <summary>
        /// 返回所有故障信息
        /// </summary>
        /// <returns></returns>
        public CErrorCode[] LoadErrorsDescp() 
        {
            try 
            {
                return CWData.myDB.LoadErrorsDesc();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找正在报警的故障信息集合
        /// </summary>
        /// <returns></returns>
        public CErrorCode[] SelectAllErrs() 
        {
            List<CErrorCode> Errs = new List<CErrorCode>();
            if (dicErrorCode != null) 
            {
                foreach (CSMG smg in SMGs) 
                {
                    if (dicErrorCode.ContainsKey(smg.ID)) 
                    {
                        List<CErrorCode> errors = dicErrorCode[smg.ID];
                        foreach (CErrorCode err in errors)
                        {
                            if (err.CurrentValue == 1 && err.Color == 1)
                            {
                                Errs.Add(err);
                            }
                        }
                    }
                }               
            }
            return Errs.ToArray();
        }

        #endregion

        /// <summary>
        /// 依设备号找出其有报警的位信息
        /// </summary>
        /// <param name="etype"></param>
        /// <returns>返回有报警的位信息的集合</returns>
        public CErrorCode[] SelectErrorCodesOfeId(int etype) 
        {
            if (dicErrorCode != null) 
            {
                if (dicErrorCode.ContainsKey(etype))
                {
                    List<CErrorCode> errCodes = new List<CErrorCode>();
                    foreach (CErrorCode err in dicErrorCode[etype])
                    {
                        if (err.CurrentValue == 1)
                        {
                            errCodes.Add(err);
                        }
                    }
                    return errCodes.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// 更新设备状态字，由后台发起的
        /// </summary>
        /// <param name="statCode"></param>
        public void UpdateStatusCode(short[] statCode) 
        {
            int statNum=72;
            if (EqpStates == null)
            {
                EqpStates = new CStatCode[statNum];  //一个设备占用12个字
                for (short i = 1; i < statNum+1&&i<statCode.Length+1; i++)
                {
                    EqpStates[i-1] = new CStatCode(i, statCode[i-1]);
                }
            }
            else 
            {
                for (int i = 1; i < statNum+1 && i < statCode.Length+1; i++) 
                {
                    EqpStates[i-1].CurrentValue = statCode[i-1];
                }
            }            
        }
        /// <summary>
        /// 判断车厅可接收刷卡取车位-----20141013 取消该位
        /// </summary>
        /// <param name="hallID">车厅ID</param>
        /// <returns></returns>
        public bool CheckHallStateBitOfGetCar(int hallID)
        {
            #region
            //if (EqpErrors != null)
            //{
            //    if (hallID == 11) 
            //    {
            //        CErrorCode errcode = SelectErrorCode(719);
            //        if (errcode.CurrentValue == 1)
            //        {
            //            return true;
            //        }
            //    }
            //   else if (hallID == 12)
            //    {
            //        CErrorCode errcode = SelectErrorCode(959);
            //        if (errcode.CurrentValue == 1)
            //        {
            //            return true;
            //        }
            //    }
            //    else if (hallID == 13)
            //    {
            //        CErrorCode errcode = SelectErrorCode(1199);
            //        if (errcode.CurrentValue == 1)
            //        {
            //            return true;
            //        }
            //    }
            //    else if (hallID == 14) 
            //    {
            //        CErrorCode errcode = SelectErrorCode(1439);
            //        if (errcode.CurrentValue == 1)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //} 
            //else 
            //{
            //    return false;
            //}
            return false;
            #endregion
        }

        /// <summary>
        /// 判断是否可接收新指令
        /// </summary>
        /// <param name="hallID">设备ID</param>
        /// <returns></returns>
        public bool CheckAcceptNewCommand(int scNo)
        {
            #region
            if (dicErrorCode != null)
            {
                if (dicErrorCode.ContainsKey(scNo))
                {
                    short startbit = 0;
                    switch (scNo)
                    {
                        case 1:
                            startbit = 240;
                            break;
                        case 2:
                            startbit = 480;
                            break;
                        case 11:
                            startbit = 720;
                            break;
                        case 12:
                            startbit = 960;
                            break;
                        case 13:
                            startbit = 1200;
                            break;
                        case 14:
                            startbit = 1440;
                            break;
                    }
                    List<CErrorCode> errs = dicErrorCode[scNo];
                    CErrorCode code = errs.Find(ce => ce.StartBit == startbit);
                    if (code != null)
                    {
                        if (code.CurrentValue == 1)
                        {
                            return true;
                        }
                    }
                }
            }          
            return false;
            #endregion
        }

        /// <summary>
        /// 判断车辆是否跑位位
        /// </summary>
        /// <returns></returns>
        public bool CheckCarInMoveOut(int hid) 
        {
            if (dicErrorCode != null) 
            {
                if (dicErrorCode.ContainsKey(hid)) 
                {
                    short startbit = 0;
                    switch (hid) 
                    {
                        case 11:
                            startbit = 579;
                            break;
                        case 13:
                            startbit = 1059;
                            break;
                    }
                    List<CErrorCode> errs = dicErrorCode[hid];
                    CErrorCode code = errs.Find(ce => ce.StartBit == startbit);
                    if (code != null)
                    {
                        if (code.CurrentValue == 1)
                        {
                            return true;
                        }
                    }
                }               
            }
            return false;
        }

        /// <summary>
        /// 获取类型一致的设备集合
        /// </summary>        
        public CSMG[] SelectSMGsOfType(CSMG.EnmSMGType etype) 
        {
            List<CSMG> smgs = new List<CSMG>();
            foreach (CSMG smg in SMGs) 
            {
                if (smg.SMGType == etype) 
                {
                    smgs.Add(smg);
                }
            }
            return smgs.ToArray();
        }

        /// <summary>
        /// 找出模式为可出车模式且可用的车厅集合
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="htype">车厅模式</param>
        /// <returns></returns>
        public CSMG[] SelectSMGsCanGetCar() 
        {
            List<CSMG> smgs = new List<CSMG>();
            foreach (CSMG smg in SMGs) 
            {
                if (smg.SMGType == CSMG.EnmSMGType.Hall && (smg.HallType == CSMG.EnmHallType.Exit ||
                    smg.HallType == CSMG.EnmHallType.EnterorExit) && smg.Available)
                {
                    smgs.Add(smg);
                }
            }
            return smgs.ToArray();
        }

        /// <summary>
        /// 更新设备的available
        /// </summary>        
        public void UpdateSMGStatus(int EtvID,bool isAvail,CSMG.EnmModel model) 
        {            
            CSMG etv = SelectSMG(EtvID);
            if (etv == null)
            {
                throw new Exception("SMG ID无效！");
            }
            else 
            {
                etv.Available = isAvail;
                etv.Model = model;
                //更新数据库
                CWData.myDB.UpdateSMGStat(etv.ID,etv.Available,etv.Model);
            }
        }

        /// <summary>
        /// 更新设备的作业ID
        /// </summary>
        /// <returns></returns>
        public int UpdateAllSMGsWorkStatus() 
        {
            try
            {
                foreach (CSMG smg in SMGs) 
                {
                    if (smg.nIsWorking != 0)
                    {
                        CTask tsk = new CWTask().GetCTaskFromtskID(smg.nIsWorking);
                        if (tsk == null) 
                        {
                            smg.MTskID = 0;
                            smg.nIsWorking = 0;                            
                            smg.NextTaskId = 0;
                            CWData.myDB.UpdateSMGTaskStat(smg);
                        }
                        //如果是移动作业，已发生过移动，则强制完成其作业
                        if (smg.SMGType == CSMG.EnmSMGType.ETV)
                        {
                            if (tsk != null) 
                            {
                                if (tsk.Type == CTask.EnmTaskType.TVMove) 
                                {
                                    //已经发送过了，且确认了10S过后
                                    if (tsk.StatusDetail == CTask.EnmTaskStatusDetail.Asked && 
                                        DateTime.Compare(DateTime.Now, tsk.SendDtime.AddSeconds(10)) > 0)
                                    {
                                        if (smg.Available && CheckAcceptNewCommand(smg.ID)) 
                                        {
                                            if (smg.CurrAddress != null && smg.CurrAddress != tsk.ToLctAdrs) 
                                            {
                                                smg.nIsWorking = 0;
                                                CWData.myDB.UpdateSMGTaskStat(smg);

                                                CSysLog log = new CSysLog("0000", DateTime.Now, "强制复位移动：目标车位" + tsk.ToLctAdrs + "- 时间" + DateTime.Now, "系统");
                                                CWData.myDB.InsertSysLog(log);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                   
                }


                                
            }
            catch (Exception ex) 
            {
                HttpRuntime.Cache.Remove("SMGs");
                throw ex;
            }
            return 0;
        }       

        /// <summary>
        /// 更新设备的子作业及主作业号
        /// </summary>
        /// <param name="smg"></param>
        public void UpdateSMGTaskStat(CSMG smg) 
        {
            try 
            {               
                CSMG eqp = this.SelectSMG(smg.ID); //更新内存实际引用对象
                eqp.nIsWorking = smg.nIsWorking;
                eqp.NextTaskId = smg.NextTaskId;
                eqp.MTskID = smg.MTskID;               
                CWData.myDB.UpdateSMGTaskStat(eqp);
            } 
            catch (Exception ex) 
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// 读取相应设备的状态字
        /// </summary>       
        public CStatCode[] SelectStatusCodes(int smgID) 
        {
            if (EqpStates != null) 
            {
                if (smgID == 1) 
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return (ce.StartBit >= 1 && ce.StartBit <= 12); });
                }
                else if (smgID == 2) 
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return ce.StartBit >= 13 && ce.StartBit <= 24; });
                } 
                else if (smgID == 11) 
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return ce.StartBit >= 25 && ce.StartBit <=36; });
                }
                else if (smgID == 12)
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return ce.StartBit >= 37 && ce.StartBit <= 48; });
                }
                else if (smgID == 13)
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return ce.StartBit >= 49 && ce.StartBit <= 60; });
                }
                else if (smgID == 14)
                {
                    return Array.FindAll(EqpStates, delegate(CStatCode ce) { return ce.StartBit >= 61 && ce.StartBit <= 72; });
                }
            }
            return null;
        }

        /// <summary>
        /// 判断相应设备是否存在报警
        /// </summary>
        /// <param name="smgID">设备号</param>
        /// <returns></returns>
        public bool JudgeEqpHasErrorBitOfID(int smgID) 
        {
            if (dicErrorCode != null) 
            {
                if (dicErrorCode.ContainsKey(smgID))
                {
                    List<CErrorCode> errors = dicErrorCode[smgID];
                    byte type = Convert.ToByte(smgID); //设备号
                    foreach (CErrorCode cecde in errors)
                    {
                        if (cecde.CurrentValue == 1 && cecde.Color == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 依车厅地址找出车厅设备
        /// </summary>
        /// <param name="addrs"></param>
        /// <returns></returns>
        public CSMG SelectHallFromAddress(string addrs) 
        {
            foreach (CSMG smg in SMGs) 
            {
                if (smg.Address == addrs&&smg.SMGType==CSMG.EnmSMGType.Hall) 
                {
                    return smg;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定ETV当前地址
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public string GetEtvCurrAddress(int eid) 
        {            
            if (EqpStates != null)
            {
                CStatCode[] codes = this.SelectStatusCodes(eid);
                if (codes[1].CurrentValue == 0 || codes[1].CurrentValue < 1 || codes[1].CurrentValue > 20)
                {
                    CSMG etv = this.SelectSMG(eid);
                    return etv.CurrAddress;   //已经实时保存了ETV的当前地址的
                }
                string line = codes[0].CurrentValue.ToString();
                string layer = codes[2].CurrentValue.ToString();
                string list = "";
                if (codes[1].CurrentValue < 10)
                {
                    list = "0" + codes[1].CurrentValue.ToString();
                }
                else
                {
                    list = codes[1].CurrentValue.ToString();
                }
                string address = line + list + layer;
                return address;
            }           
            return null;
        }

        /// <summary>
        /// 获取ETV当前所在列
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public short GetEtvCurrList(int eid) 
        {
            if (EqpStates != null) 
            {
                if (eid == 1 || eid == 2)
                {
                    CStatCode[] etvStatCode = this.SelectStatusCodes(eid);                    
                    return etvStatCode[1].CurrentValue;
                }              
            }
            return 0;
        }

        /// <summary>
        /// 手动创建移动作业时，选择ETV
        /// </summary>
        /// <param name="tAddrs">移动源地址,须是ETV所在列</param>
        /// <returns></returns>
        public CSMG SelectEtvOfRegionDependM(string FrAddrs) 
        {
            try
            {
                short lctList = Convert.ToInt16(FrAddrs.Substring(1, 2));
                foreach (CSMG smg in SMGs)
                {
                    if (smg.SMGType == CSMG.EnmSMGType.ETV)
                    {
                        short eList = this.GetEtvCurrList(smg.ID);
                        if (eList == lctList)
                        {
                            return smg;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// 设置车厅出车类型
        /// </summary>
        /// <param name="hnmb"></param>
        /// <param name="htype"></param>
        /// <returns></returns>
        public int SetHallType(int hnmb,CSMG.EnmHallType htype)
        {
            try
            {
                CSMG smg = this.SelectSMG(hnmb);
                if (smg != null && smg.SMGType == CSMG.EnmSMGType.Hall)
                {
                    if (smg.HallType != htype)
                    {
                        CWTask wtsk = new CWTask();

                        if (htype == CSMG.EnmHallType.EnterorExit)
                        {
                            smg.HallType = htype;
                            CWData.myDB.UpdateHallType(smg.ID, htype);
                            return 0;
                        }
                        else if (htype == CSMG.EnmHallType.Entance)
                        {
                            if (wtsk.GetMasterTaskNumOfHid(hnmb, CMasterTask.EnmMasterTaskType.GetCar) == 0)
                            {
                                smg.HallType = htype;
                                CWData.myDB.UpdateHallType(smg.ID, htype);
                                return 0;
                            }
                            else
                            {
                                return 11;
                            }
                        }
                        else if (htype == CSMG.EnmHallType.Exit)
                        {
                            if (wtsk.GetMasterTaskNumOfHid(hnmb, CMasterTask.EnmMasterTaskType.SaveCar) == 0)
                            {
                                smg.HallType = htype;
                                CWData.myDB.UpdateHallType(smg.ID, htype);
                                return 0;
                            }
                            else
                            {
                                return 12;
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex) 
            {
                HttpRuntime.Cache.Remove("SMGs");
                throw ex;
            }
        }

        /// <summary>
        /// 记录故障
        /// </summary>
        /// <param name="elog"></param>
        /// <returns></returns>
        public int WriteErrorLog(byte eqp,string descp,DateTime dt)
        {
            try 
            {
                CSysLog elog = new CSysLog(Convert.ToString(eqp),dt,descp,"");
                CWData.myDB.InsertErrorLog(elog);
                return 100;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 找出所有可接收新指令的车厅
        /// </summary>
        /// <returns></returns>
        public int[] GetSpaceHallList() 
        {
            List<int> hallList = new List<int>();
            if (dicErrorCode != null) 
            {
                foreach (CSMG smg in SMGs) 
                {
                    if (smg.SMGType == CSMG.EnmSMGType.Hall) 
                    {
                        if (this.CheckAcceptNewCommand(smg.ID)) 
                        {
                            hallList.Add(smg.ID);
                        }
                    }
                }               
            }
            return hallList.ToArray();
        }

    }
}

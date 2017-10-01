using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL
{
    public class CWTask
    {
        private static List<string> lstSounds = new List<string>();
        private static List<CMasterTask> mtsksList;

        private static object lockObj = new object();  //2015-12-14

        public CWTask() 
        { }

        public List<CMasterTask> Tasks 
        {
            get 
            {
                if (mtsksList == null)
                {
                    mtsksList = CWData.myDB.LoadMasterTasks();
                }
                return mtsksList;
            }
        }

        /// <summary>
        /// 添加声音文件
        /// </summary>
        /// <param name="hallID">车厅号</param>
        /// <param name="fSound">声音文件</param>
        public void AddNotification(int hallID,string fSound) 
        {
            lock(lstSounds)
            {
                string sfile = hallID + fSound;
                if (!lstSounds.Contains(sfile)) 
                {
                    lstSounds.Add(sfile);
                }
            }
        }

        /// <summary>
        /// 移除声音文件
        /// </summary>
        /// <param name="hid">车厅号</param>
        public void ClearNotification(int hid) 
        {
            lock (lstSounds) 
            {
                for (int i = 0; i < lstSounds.Count; i++)
                {
                    string snd = lstSounds[i];
                    if (snd.Substring(0, 2) == hid.ToString())
                    {
                        lstSounds.Remove(snd);
                    }
                }
            }
        }

        /// <summary>
        /// 获取声音文件
        /// </summary>        
        public string GetNotification(int hallNumb)
        {
            lock (lstSounds)
            {
                string sfile = lstSounds.Find(delegate(string s) { return Convert.ToInt32(s.Substring(0, 2)) == hallNumb; });
                if (sfile != null)
                {
                    string sound = sfile.Substring(2);
                    lstSounds.Remove(sfile);
                    return sound;
                }
            }
            return null;
        }

        /// <summary>
        /// 同一张卡是否在别的车厅有作业
        /// </summary>     
        public bool CheckSameMTaskInHallFromICCd(string iccode,int hallID) 
        {
            bool isExits = false;
            foreach (CMasterTask mtsk in Tasks) 
            {               
                if (mtsk.HID != hallID&&mtsk.ICCardCode==iccode) 
                {
                    isExits = true;
                }
            }
            return isExits;
        }

        /// <summary>
        /// 获取同车厅的取车作业数量
        /// </summary> 
        public int GetMTskCountFromHall(int hallID,CMasterTask.EnmMasterTaskType mtype) 
        {
            int count=0;
            foreach(CMasterTask mtsk in Tasks)
            {
                if (mtsk.HID == hallID && mtsk.Type == mtype) 
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 依用户卡号找出其主作业
        /// </summary>        
        public CMasterTask GetMasterTaskFromICCode(string iccode) 
        {
            foreach (CMasterTask mtsk in Tasks) 
            {
                if (mtsk.ICCardCode == iccode) 
                {
                    return mtsk;
                }
            }
            return null;
        } 
       
        /// <summary>
        /// 依子作业ID查找其作业
        /// </summary>       
        public CTask GetCTaskFromtskID(int tskID)
        {
            foreach (CMasterTask mtsk in Tasks) 
            {
                foreach (CTask tsk in mtsk.Tasks) 
                {
                    if (tsk.ID == tskID) 
                    {
                        return tsk;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 依子作业ID查找其主、子作业
        /// </summary>  
        public int GetMTaskAndCTaskOfTid(int tskID,out CMasterTask mtsk,out CTask tsk) 
        {
            mtsk = null;
            tsk = null;
            foreach (CMasterTask mstTsk in Tasks) 
            {
                foreach (CTask ctsk in mstTsk.Tasks) 
                {
                    if (ctsk.ID == tskID) 
                    {
                        mtsk = mstTsk;
                        tsk = ctsk;
                        return 1;
                    }
                }
            }
            return 0;
        }       

        /// <summary>
        /// 依主作业ID查找主作业
        /// </summary>        
        public CMasterTask GetMasterTaskFromID(int mid) 
        {
           return  Tasks.Find(delegate(CMasterTask mtsk) { return mtsk.ID == mid; });
        }

        /// <summary>
        /// 查找主作业中对应设备类型的子作业
        /// </summary>   
        public CTask GetCTaskOfSMGTypeAndMtsk(int mid, CSMG.EnmSMGType type)
        {
            CMasterTask mtsk = GetMasterTaskFromID(mid);
            foreach (CTask tsk in mtsk.Tasks) 
            {
                CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                if (smg.SMGType == type) 
                {
                    return tsk;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取对应车厅的所有主作业
        /// </summary>
        /// <param name="hnmb"></param>
        /// <returns></returns>
        public int GetMasterTaskCountOfHid(int hnmb) 
        {           
            int count = 0;
            foreach (CMasterTask mtsk in Tasks) 
            {
                if (mtsk.HID == hnmb) 
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 查找当前执行的作业
        /// </summary>
        /// <param name="smgID"></param>
        /// <returns></returns>
        public CTask GetCurrentTaskOfSMG(int smgID) 
        {
            CSMG smg = new CWSMG().SelectSMG(smgID);
            if (smg.nIsWorking != 0)
            {
                foreach (CMasterTask mtsk in Tasks)
                {
                    foreach (CTask tsk in mtsk.Tasks)
                    {
                        if (tsk.ID == smg.nIsWorking)
                        {
                            return tsk;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 依车厅号查找当前主作业
        /// </summary>
        /// <param name="hnmb"></param>
        /// <returns></returns>
        public CMasterTask GetCurrentMasterTaskOfSMG(int hnmb) 
        {
            int mID = 0;
            CSMG smg = new CWSMG().SelectSMG(hnmb);
            if (smg.nIsWorking != 0) 
            {
                CTask tsk;
                CMasterTask mtsk;
                this.GetMTaskAndCTaskOfTid(smg.nIsWorking, out mtsk, out tsk);

                if (mtsk != null) 
                {
                    mID = mtsk.ID;
                }
            }
            if (mID != 0)
            {
                foreach (CMasterTask cmtsk in Tasks)
                {
                    if (cmtsk.ID == mID)
                    {
                        return cmtsk;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取库内主作业数量
        /// </summary>
        /// <returns></returns>
        public int GetAllMasterTaskCount() 
        {
            return Tasks.Count;
        }

        /// <summary>
        /// 获取库内所有的主作业
        /// </summary>
        /// <returns></returns>
        public CMasterTask[] SelectAllMasterTask() 
        {
            return Tasks.ToArray();
        }

        /// <summary>
        /// 获取对应车厅的指定类型的主作业的数量
        /// </summary>
        /// <param name="hnmb"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public int GetMasterTaskNumOfHid(int hnmb, CMasterTask.EnmMasterTaskType mtype) 
        {
            int count = 0;
            foreach (CMasterTask mtsk in Tasks) 
            {
                if (mtsk.HID == hnmb && mtsk.Type == mtype) 
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 处理第一次刷卡
        /// </summary>    
        public void DealISwipedFirstCard(int tid,string iccd)
        {
            try
            {
                lock (lockObj)
                {
                    CMasterTask mtsk;
                    CTask tsk;
                    this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);
                    mtsk.ICCardCode = iccd;
                    tsk.ICCardCode = iccd;
                    tsk.Status = CTask.EnmTaskStatus.IFirstSwipedWaitforCheckSize;
                    tsk.StatusDetail = CTask.EnmTaskStatusDetail.NoSend;
                    //更新数据库内容
                    CWData.myDB.UpdateMtskAndCtskInfo(tsk);
                    //请再次刷卡
                    this.AddNotification(tsk.HID, "19.wav");
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 处理第二次刷卡
        /// </summary>       
        public void DealISwipedSecondCard(int tid, string iccd)
        {
            try
            {      
                CMasterTask mtsk;
                CTask tsk;
                lock (lockObj)
                {
                    this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);
                    mtsk.ICCardCode = iccd;
                    tsk.ICCardCode = iccd;
                    tsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCheckSize;
                    tsk.StatusDetail = CTask.EnmTaskStatusDetail.NoSend;
                    //更新数据库内容
                    CWData.myDB.UpdateMtskAndCtskInfo(tsk);
                    //请稍后，听到指示后离开
                    this.AddNotification(tsk.HID, "21.wav");
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 处理取物完成后刷卡存车(第三次刷卡)
        /// </summary>
        /// <param name="tsk">车厅作业</param>
        /// <param name="iccd">卡信息</param>
        public void DealISwipeThirdCard(CTask tsk, CICCard iccd)
        {
            try 
            {
                if (tsk.ICCardCode != iccd.Code)
                {
                    this.AddNotification(tsk.HID, "20.wav");
                    return;
                }
                lock (lockObj)
                {
                    CMasterTask mtsk = this.GetMasterTaskFromICCode(iccd.Code);
                    if (mtsk.Type != CMasterTask.EnmMasterTaskType.TempGetCar)
                    {
                        //当前流程不是取物时，刷卡不响应,提示该卡没有存车
                        this.AddNotification(tsk.HID, "14.wav");
                        return;
                    }
                    int warehouse = mtsk.WID;
                    int hnmb = tsk.HID;
                    string addressOfLct = tsk.FromLctAdrs;  //车位地址            
                    string carSize = tsk.CarSize;           //车辆外形

                    mtsk.IsCompleted = true;
                    tsk.Status = CTask.EnmTaskStatus.Finished;
                    //完成作业,修改数据库
                    CWData.myDB.CompleteMasterTaskAndCTask(mtsk);
                    mtsk.RemoveTask(tsk);

                    //释放车厅设备
                    CSMG hall = new CWSMG().SelectSMG(tsk.SMG);
                    hall.MTskID = 0;
                    hall.nIsWorking = 0;
                    hall.NextTaskId = 0;
                    CWData.myDB.UpdateSMGTaskStat(hall);

                    //建立再次入库作业
                    CMasterTask tempMtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.SaveCar, iccd.Code, hnmb, warehouse, true);
                    CTask htsk = new CTask(hall.Address, addressOfLct, CTask.EnmTaskType.HallCarEntrance, CTask.EnmTaskStatus.IFirstSwipedWaitforCheckSize,
                        iccd.Code, hnmb, CSMG.EnmSMGType.Hall, 0, hnmb, carSize);
                    tempMtsk.AddTask(htsk);

                    Tasks.Remove(mtsk);
                    Tasks.Add(tempMtsk);
                    //插入作业
                    CWData.myDB.InsertAMasterTask(tempMtsk);
                    this.AddNotification(hnmb, "41.wav");  //再次刷卡
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 刷卡取车处理,分配移动设备，建立主作业，车厅子作业及ETV作业
        /// </summary>        
        public void DealOswipedFirstCard(int HallID,CICCard iccd,CLocation lct)
        {
            try
            {
                CSMG hallEqp = new CWSMG().SelectSMG(HallID);
                string FromAddress = lct.Address;
                string ToAddress = hallEqp.Address;
               
                CSMG[] etvs = new CWSMG().SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                
                int etvID = new AllocateETV.GetCarAllocate().AllocateEtv(lct, hallEqp.Address, etvs);

                if (etvID==0)  
                {
                    //库内无可用的移动设备
                    this.AddNotification(HallID,"38.wav");   
                    return;
                }

                string mse = "取车- 分配ETV" + etvID + "  取车位：" + lct.Address+"  出车厅："+HallID;
                new CWSException(mse, 0);

                lock (lockObj)
                {
                    //建立主作业
                    CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.GetCar,
                        iccd.Code, hallEqp.ID, lct.Warehouse);

                    //建立车厅子作业
                    CTask htsk = new CTask(FromAddress, ToAddress, CTask.EnmTaskType.HallCarExit,
                        CTask.EnmTaskStatus.OWaitforEVDown, iccd.Code, HallID, CSMG.EnmSMGType.Hall,
                        lct.Distance, HallID, lct.CarSize);
                    mtsk.AddTask(htsk);

                    string currAddrs = new CWSMG().GetEtvCurrAddress(etvID);
                    //添加移动作业
                    CTask vtsk = new CTask(currAddrs, FromAddress, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                        iccd.Code, etvID, CSMG.EnmSMGType.ETV, 0, HallID);
                    mtsk.AddTask(vtsk);

                    //建立ETV子作业
                    CTask etsk = new CTask(FromAddress, ToAddress, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad,
                        iccd.Code, etvID, CSMG.EnmSMGType.ETV, lct.Distance, HallID, lct.CarSize);
                    mtsk.AddTask(etsk);


                    string msg = "取车-更新前车位：" + lct.Address + " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                    new CWSException(msg, 0);

                    lct.Status = CLocation.EnmLocationStatus.Outing;

                    //数据库(主作业，子作业)写入,车位信息更新
                    CWData.myDB.InsertAMasterTaskAndLct(mtsk, lct, null);
                    Tasks.Add(mtsk);

                    //正在出库，请稍后
                    this.AddNotification(HallID, "30.wav");
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 车子初始入库处理，建立主作业，车厅子作业
        /// </summary>      
        public int DealIFirstCarEntrance(int hallID) 
        {
            try
            {
                CSMG hall = new CWSMG().SelectSMG(hallID);
                string FrAddres = hall.Address;

                CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.SaveCar,
                    "", hall.ID, hall.Warehouse);
                //建立车厅作业
                CTask htsk = new CTask(FrAddres, "", CTask.EnmTaskType.HallCarEntrance, CTask.EnmTaskStatus.ICarInWaitFirstSwipeCard,
                    "", hallID, CSMG.EnmSMGType.Hall, 0, hallID);
                mtsk.AddTask(htsk);

                lock (Tasks)
                {
                    //更新数据库
                    CWData.myDB.InsertAMasterTask(mtsk);
                    Tasks.Add(mtsk);
                }
                this.AddNotification(hallID, "18.wav");
                return htsk.ID;
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 更新子作业的发送时间及发送状态
        /// </summary>  
        public void SetTskStatusDetail(CTask.EnmTaskStatusDetail detail,int tskID,DateTime SDtime) 
        {
            CTask tsk =this.GetCTaskFromtskID(tskID);
            if (tsk != null)
            {
                tsk.StatusDetail = detail;
                tsk.SendDtime = SDtime;
                //不更新数据库
            }
        }

        /// <summary>
        /// 更新子作业状态
        /// </summary>
        public void DUpdateTaskStatus(CTask.EnmTaskStatus status, int tskID) 
        {
            try
            {
                CTask tsk = this.GetCTaskFromtskID(tskID);
                if (tsk != null)
                {
                    tsk.Status = status;
                    //更新数据库
                    CWData.myDB.UpdateCTaskStatus(status, tsk.ID);
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 入库后，因某个原因车辆离开后删除作业
        /// </summary>
        public void ICancelInAndDelHallTsk(CTask tsk) 
        {
            try
            {
                CWSMG wsmg = new CWSMG();
                CSMG hall = wsmg.SelectSMG(tsk.SMG);
                hall.MTskID = 0;
                hall.nIsWorking = 0;
                hall.NextTaskId = 0;
                CWData.myDB.UpdateSMGTaskStat(hall);
                lock (lockObj)
                {
                    //删除作业
                    CMasterTask mtsk = this.GetMasterTaskFromID(tsk.MID);
                    if (mtsk.IsTemp == true || tsk.ToLctAdrs != "")  //是取物后存车时
                    {
                        CLocation lct = new CWLocation().SelectLctFromAddrs(tsk.ToLctAdrs);
                        if (lct != null&&lct.Status!=CLocation.EnmLocationStatus.Entering)
                        {
                            lct.Status = CLocation.EnmLocationStatus.Space;
                            CWData.myDB.UpdateLocationStatus(lct);
                        }
                    }

                    this.AddNotification(hall.ID, "22.wav");

                    //删除主作业、子作业
                    CWData.myDB.DeleteMasterTaskAndCTask(mtsk.ID);
                    Tasks.Remove(mtsk);
                }                
            }
            catch (Exception ex)
            {
                new CWSException();   //同时有更新车位,设备信息的,异常时可以给重新加载车位设备信息
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 出库时，车辆完全开出处理
        /// </summary>
        public void ODealCarDriveaway(int tid)
        {
            try
            {
                CMasterTask mtsk = null;
                CTask tsk = null;
                this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);
                lock (lockObj)
                {
                    CSMG hall = new CWSMG().SelectSMG(tsk.SMG);
                    hall.MTskID = 0;
                    hall.nIsWorking = 0;
                    hall.NextTaskId = 0;
                    CWData.myDB.UpdateSMGTaskStat(hall);

                    CLocation frmLct = new CWLocation().SelectLctFromAddrs(tsk.FromLctAdrs);  //车位坐标
                    if (frmLct != null && mtsk.Type == CMasterTask.EnmMasterTaskType.TempGetCar)
                    {
                        frmLct.Status = CLocation.EnmLocationStatus.Space; //取物时更新吧
                        frmLct.ICCardCode = "";
                        frmLct.Distance = 0;
                        frmLct.CarSize = "";

                        string msg = "取物车辆开走- 更新车位：" + frmLct.Address + " 状态：" + frmLct.Status.ToString() + " 卡号：" + frmLct.ICCardCode;
                        new CWSException(msg, 0);

                        CWData.myDB.UpdateLocationInfo(frmLct);
                    }                   

                    tsk.Status = CTask.EnmTaskStatus.Finished;
                    CWData.myDB.UpdateCTaskAndLct(tsk.ID, tsk.Status, null,null);
                    this.AddNotification(hall.ID, "22.wav");
                    mtsk.RemoveTask(tsk);

                    mtsk.IsCompleted = true;
                    CWData.myDB.UpdateMTaskCompleted(mtsk);
                    Tasks.Remove(mtsk);
                }
            }
            catch (Exception ex)
            {
                new CWSException();
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 收到(1001,104)外形检测失败，更新作业状态为icheckcarfail
        /// </summary>
        public void IDealCheckCarFail(int tskID)
        {
            try
            {
                CTask tsk = this.GetCTaskFromtskID(tskID);
                tsk.Status = CTask.EnmTaskStatus.ICheckCarFail;
                CWData.myDB.UpdateCTaskStatus(tsk.Status, tsk.ID);
                this.AddNotification(tsk.HID, "23.wav");
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 入库时外形检测处理，其分配ETV，生成ETV作业
        /// </summary>
        public void IDealCheckedCar(int tid,string checkCode,int nDist) 
        {
            try
            {
                CTask htsk = this.GetCTaskFromtskID(tid);
                htsk.CarSize = checkCode;
                htsk.Distance = nDist;

                if (checkCode.Length != 3)
                {
                    htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                    CWData.myDB.UpdateACTask(htsk.ID, htsk.Distance, htsk.Status, htsk.CarSize);
                    this.AddNotification(htsk.HID, "23.wav");
                    return;
                }

                int cLong = int.Parse(checkCode.Substring(0, 1));
                int cWidth = int.Parse(checkCode.Substring(1, 1));
                int cHigh = int.Parse(checkCode.Substring(2, 1));
                if (cLong > 1 || cWidth > 1 || cHigh > 2)
                {
                    htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                    CWData.myDB.UpdateACTask(htsk.ID, htsk.Distance, htsk.Status, htsk.CarSize);
                    this.AddNotification(htsk.HID, "23.wav");
                    return;
                }

                lock (lockObj)
                {
                    CICCard iccd = new CWICCard().SelectByUserCode(htsk.ICCardCode);
                    int smgEntity = 0;
                    //分配移动设备及车位
                    CLocation ToLct = this.IAllocateLocation(htsk.ID, iccd, checkCode, ref smgEntity);

                    if (smgEntity == 0)
                    {
                        //库内无可用ETV                    
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        CWData.myDB.UpdateCTaskStatus(htsk.Status, htsk.ID);
                        this.AddNotification(htsk.HID, "38.wav");
                        return;
                    }                   

                    if (ToLct == null)
                    {
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        CWData.myDB.UpdateACTask(htsk.ID, htsk.Distance, htsk.Status, htsk.CarSize);
                        this.AddNotification(htsk.HID, "34.wav");
                        return;
                    }

                    string mse = "存车- 分配ETV" + smgEntity + "   车辆尺寸：" + checkCode + "   车厅：" + htsk.HID + "   分配车位：" + ToLct.Address;
                    new CWSException(mse, 0);

                    //取出车位，再对比下-2015-12-14
                    if (ToLct != null)
                    {
                        string msg = "存车- 分配前车位地址：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode + "  存车卡号：" + htsk.ICCardCode;
                        new CWSException(msg, 0);

                        CLocation tlt = new CWLocation().SelectLctFromAddrs(ToLct.Address);
                        if (tlt.Status != CLocation.EnmLocationStatus.Space || string.Compare(tlt.Size, checkCode) < 0)
                        {
                            htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                            CWData.myDB.UpdateACTask(htsk.ID, htsk.Distance, htsk.Status, htsk.CarSize);
                            this.AddNotification(htsk.HID, "34.wav");
                            return;
                        }
                    }

                    int lctLong = Convert.ToInt32(ToLct.Size.Substring(0, 1));
                    int lctWidth = Convert.ToInt32(ToLct.Size.Substring(1, 1));
                    int lctHigh = Convert.ToInt32(ToLct.Size.Substring(2, 1));
                    if (cLong > lctLong || cWidth > lctWidth || cHigh > lctHigh)
                    {
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        CWData.myDB.UpdateACTask(htsk.ID, htsk.Distance, htsk.Status, htsk.CarSize);
                        this.AddNotification(htsk.HID, "34.wav");
                        return;
                    }

                    CMasterTask mtsk = this.GetMasterTaskFromID(htsk.MID);

                    //更新车厅作业、车位信息、插入ETV作业                    
                    ToLct.CarSize = checkCode;
                    ToLct.ICCardCode = htsk.ICCardCode;
                    ToLct.Distance = nDist;
                    ToLct.InDate = DateTime.Now;
                    ToLct.Status = CLocation.EnmLocationStatus.Entering;
                    CICCard iccard = new CICCard();
                    iccard = new CWICCard().SelectByUserCode(htsk.ICCardCode);
                    ToLct.SetICCard(iccard);

                    string mss = "存车- 分配后修改车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                    new CWSException(mss, 0);

                    htsk.ToLctAdrs = ToLct.Address;
                    htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforEVDown;

                    List<CTask> taskList = new List<CTask>();
                    string etvAddrs = new CWSMG().GetEtvCurrAddress(smgEntity);
                    //生成移动作业
                    CTask vtsk = new CTask(etvAddrs, htsk.FromLctAdrs, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                        iccd.Code, smgEntity, CSMG.EnmSMGType.ETV, 0, htsk.HID);
                    vtsk.MID = mtsk.ID;
                    mtsk.AddTask(vtsk);
                    taskList.Add(vtsk);
                    //生成装载作业
                    CTask etsk = new CTask(htsk.FromLctAdrs, ToLct.Address, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad,
                        iccd.Code, smgEntity, CSMG.EnmSMGType.ETV, nDist, htsk.HID, checkCode);
                    etsk.MID = mtsk.ID;
                    mtsk.AddTask(etsk);
                    taskList.Add(etsk);
                    //执行事务更新数据库
                    CWData.myDB.InsertCTasks(mtsk, taskList);
                    CWData.myDB.UpdateCTaskAndLct(htsk, ToLct);

                    this.AddNotification(mtsk.HID, "26.wav");
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                new CWSException();
                throw ex;
            }
        }

        /// <summary>
        /// 取物后存车，处理检测外形，分配ETV
        /// </summary>
        /// <param name="tID"></param>
        /// <param name="distance"></param>
        /// <param name="carSize"></param>
        public void ITempDealCheckCar(int tID, int mID,int distance, string carSize,string toAddrs) 
        {
            try
            {
                lock (lockObj)
                {
                    CTask htsk = this.GetCTaskFromtskID(tID);
                    CLocation ToLct = new CWLocation().SelectLctFromAddrs(toAddrs);
                    if (ToLct == null)
                    {
                        //找不到对应车位
                        this.AddNotification(htsk.HID, "39.wav");
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        return;
                    }
                    if (ToLct.Type == CLocation.EnmLocationType.Disable)
                    {
                        //该车位出现故障
                        this.AddNotification(htsk.HID, "28.wav");
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        return;
                    }
                    CSMG hall = new CWSMG().SelectSMG(htsk.HID);
                    CSMG[] etvs = new CWSMG().SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                    int Etv = new IEGBLL.AllocateETV.SaveCarAllocate().AllocateEtv(hall, ToLct, etvs);
                    if (Etv == 0)
                    {
                        //库内无可用ETV
                        this.AddNotification(htsk.ID, "38.wav");
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        return;
                    }
                    CMasterTask mtsk = this.GetMasterTaskFromID(mID);

                    string mss = "转存- 修改前车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                    new CWSException(mss, 0);

                    //更新车厅作业、车位信息、插入ETV作业                    
                    ToLct.CarSize = carSize;
                    ToLct.ICCardCode = htsk.ICCardCode;
                    ToLct.Distance = distance;
                    ToLct.InDate = DateTime.Now;
                    ToLct.Status = CLocation.EnmLocationStatus.Entering;
                    CICCard iccard = new CICCard();
                    iccard = new CWICCard().SelectByUserCode(htsk.ICCardCode);
                    ToLct.SetICCard(iccard);

                    string mess = "转存- 修改后车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                    new CWSException(mess, 0);

                    htsk.ToLctAdrs = ToLct.Address;
                    htsk.Distance = distance;
                    htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforEVDown;

                    List<CTask> taskList = new List<CTask>();
                    string etvAddrs = new CWSMG().GetEtvCurrAddress(Etv);
                    //生成移动作业
                    CTask vtsk = new CTask(etvAddrs, htsk.FromLctAdrs, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                        mtsk.ICCardCode, Etv, CSMG.EnmSMGType.ETV, 0, htsk.HID);
                    vtsk.MID = mtsk.ID;
                    mtsk.AddTask(vtsk);
                    taskList.Add(vtsk);
                    //生成装载作业
                    CTask etsk = new CTask(htsk.FromLctAdrs, ToLct.Address, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad,
                        mtsk.ICCardCode, Etv, CSMG.EnmSMGType.ETV, distance, htsk.HID, carSize);
                    etsk.MID = mtsk.ID;
                    mtsk.AddTask(etsk);
                    taskList.Add(etsk);
                    //执行事务更新数据库
                    CWData.myDB.InsertCTasks(mtsk, taskList);
                    CWData.myDB.UpdateCTaskAndLct(htsk, ToLct);

                    this.AddNotification(mtsk.HID, "26.wav");
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                new CWSException();
                throw ex;
            }

        }

        /// <summary>
        /// 装、卸载完成时，信息处理
        /// </summary>
        /// <param name="tskID">ETV作业ID</param>
        /// <param name="isLoad">装载时为true,卸载时为false</param>
        /// <param name="nDist">装载完成时实时轴距</param>
        public void DealTVActionFinishing(int tskID,bool isLoad,int nDist) 
        {
            try
            {
                CSysLog log = null;
                CMasterTask mtsk = null;
                CTask etsk = null;
                this.GetMTaskAndCTaskOfTid(tskID,out mtsk,out etsk);
                if (mtsk == null || etsk == null)
                {
                    Exception e = new Exception("DealTVActionFinishing mtsk为空");
                    new CWSException(e);
                    return;
                }
                DateTime inDate = CObject.DefDatetime;
                List<CTask> taskList = new List<CTask>();

                CWLocation cltn=new CWLocation();
                CLocation frLct = cltn.SelectLctFromAddrs(etsk.FromLctAdrs);  //源地址
                CLocation toLct = cltn.SelectLctFromAddrs(etsk.ToLctAdrs);    //目标地址           
                if (frLct == null || toLct == null) 
                {
                    Exception e = new Exception("DealTVActionFinishing frlct tolct为空");
                    new CWSException(e);
                    return;
                }
                lock (lockObj)
                {
                    #region 存车
                    if (mtsk.Type == CMasterTask.EnmMasterTaskType.SaveCar)
                    {
                        if (isLoad)  //装载完成
                        {
                            //释放车厅
                            CTask htsk = GetCTaskOfSMGTypeAndMtsk(mtsk.ID, CSMG.EnmSMGType.Hall);
                            if (htsk == null)
                            {
                                Exception e = new Exception("DealTVActionFinishing htsk为空");
                                new CWSException(e);
                                return;
                            }
                            htsk.Status = CTask.EnmTaskStatus.Finished;
                            CSMG hall = new CWSMG().SelectSMG(htsk.SMG);
                            if (hall == null)
                            {
                                Exception e = new Exception("DealTVActionFinishing hall为空");
                                new CWSException(e);
                                return;
                            }
                            hall.MTskID = 0;
                            hall.nIsWorking = 0;
                            hall.NextTaskId = 0;
                            try
                            {
                                CWData.myDB.UpdateSMGTaskStat(hall);
                            }
                            catch (Exception ex)
                            {
                                new CWSException(ex);
                            }

                            etsk.Status = CTask.EnmTaskStatus.LoadFinishing;
                            etsk.Distance = nDist;

                            frLct.SetICCard(null);
                            frLct.Status = CLocation.EnmLocationStatus.Space;
                            frLct.Distance = 0;
                            frLct.CarSize = "";
                            frLct.InDate = CObject.DefDatetime;

                            //更新车位
                            CICCard iccard = new CWICCard().SelectByUserCode(etsk.ICCardCode);
                            if (iccard != null)
                            {
                                toLct.SetICCard(iccard);
                            }
                            else
                            {
                                toLct.ICCardCode = etsk.ICCardCode;
                            }
                            toLct.Distance = nDist;  //更新轴距
                            CWData.myDB.UpdateLocationInfo(toLct);

                            string msg = "存车- 装载完成：" + toLct.Address + " 状态：" + toLct.Status.ToString() + " 卡号：" + toLct.ICCardCode;
                            new CWSException(msg, 0);

                            //添加移动作业
                            CTask vtsk = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, 0, mtsk.HID);
                            vtsk.MID = mtsk.ID;
                            mtsk.AddTask(vtsk);
                            taskList.Add(vtsk);

                            //添加卸载作业
                            CTask etvTask = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVUnload, CTask.EnmTaskStatus.TWaitforUnload,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, nDist, mtsk.HID, etsk.CarSize);
                            etvTask.MID = mtsk.ID;
                            mtsk.AddTask(etvTask);
                            taskList.Add(etvTask);
                           
                            //更新数据库
                            try
                            {
                                CWData.myDB.InsertCTasks(mtsk, taskList);
                            }
                            catch (Exception ex)
                            {
                                new CWSException(ex);
                            }
                        }
                        else   //卸载完成
                        {
                            etsk.Status = CTask.EnmTaskStatus.UnLoadFinishing;

                            toLct.Status = CLocation.EnmLocationStatus.Occupy;
                            CICCard iccard = new CICCard();
                            iccard = new CWICCard().SelectByUserCode(etsk.ICCardCode);
                            if (iccard != null)
                            {
                                toLct.SetICCard(iccard);
                            }
                            else
                            {
                                toLct.ICCardCode = etsk.ICCardCode;
                            }
                            toLct.Distance = etsk.Distance;
                            toLct.CarSize = etsk.CarSize;
                            CWData.myDB.UpdateLocationInfo(toLct);

                            string msg = "存车- 卸载完成：" + toLct.Address + " 状态：" + toLct.Status.ToString() + " 卡号：" + toLct.ICCardCode;
                            new CWSException(msg, 0);

                            //记录 
                            if (mtsk.IsTemp)
                            {
                                log = new CSysLog(iccard.Code, DateTime.Now, "转存：车位" + toLct.Address + "-卡号" + iccard.Code + "-轴距" + toLct.Distance +
                                       "-车型" + toLct.CarSize + "-入库" + DateTime.Now, "系统");
                            }
                            else
                            {
                                log = new CSysLog(iccard.Code, DateTime.Now, "存车：车位" + toLct.Address + "-卡号" + iccard.Code + "-轴距" + toLct.Distance +
                                        "-车型" + toLct.CarSize + "-入库" + DateTime.Now, "系统");
                            }
                        }
                    }
                    #endregion
                    #region 取车
                    else if (mtsk.Type == CMasterTask.EnmMasterTaskType.GetCar || mtsk.Type == CMasterTask.EnmMasterTaskType.TempGetCar)
                    {
                        if (isLoad)
                        {
                            frLct.Status = CLocation.EnmLocationStatus.Outing;
                            frLct.Distance = nDist;

                            string msg = "取车- 装载完成：" + frLct.Address + " 状态：" + frLct.Status.ToString() + " 卡号：" + frLct.ICCardCode;
                            new CWSException(msg, 0);

                            //车厅地址
                            toLct.Status = CLocation.EnmLocationStatus.Space;
                            toLct.Distance = 0;
                            toLct.CarSize = "";
                            toLct.SetICCard(null);

                            etsk.Status = CTask.EnmTaskStatus.LoadFinishing;
                            etsk.Distance = nDist;

                            //添加移动作业
                            CTask vtsk = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, 0, mtsk.HID);
                            vtsk.MID = mtsk.ID;
                            mtsk.AddTask(vtsk);
                            taskList.Add(vtsk);
                            //添加卸载作业
                            CTask etvTask = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVUnload, CTask.EnmTaskStatus.TWaitforUnload,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, nDist, mtsk.HID, etsk.CarSize);
                            etvTask.MID = mtsk.ID;
                            mtsk.AddTask(etvTask);
                            taskList.Add(etvTask);

                            //更新数据库
                            CWData.myDB.InsertCTasks(mtsk, taskList);
                        }
                        else
                        {
                            inDate = frLct.InDate;
                            frLct.Distance = 0;
                            frLct.CarSize = "";
                            frLct.InDate = CObject.DefDatetime;
                            frLct.SetICCard(null);
                            string message = "";
                            if (mtsk.Type == CMasterTask.EnmMasterTaskType.TempGetCar)
                            {
                                frLct.Status = CLocation.EnmLocationStatus.Temping; //取物时先将车位预订
                                message = "取物";
                            }
                            else
                            {
                                frLct.Status = CLocation.EnmLocationStatus.Space;
                                message = "取车";
                            }

                            string msg = message + "- 卸载完成：" + frLct.Address + " 状态：" + frLct.Status.ToString() + " 卡号：" + frLct.ICCardCode;
                            new CWSException(msg, 0);

                            //车厅地址
                            toLct.Status = CLocation.EnmLocationStatus.Space;
                            toLct.SetICCard(null);

                            etsk.Status = CTask.EnmTaskStatus.UnLoadFinishing;

                            CTask htsk = GetCTaskOfSMGTypeAndMtsk(mtsk.ID, CSMG.EnmSMGType.Hall);
                            if (htsk != null)
                            {
                                htsk.Status = CTask.EnmTaskStatus.OWaitforEVUp;                     //卸载完成，修改车厅状态
                            }
                            //记录

                            log = new CSysLog(htsk.ICCardCode, DateTime.Now, message + "：车位" + frLct.Address + "-卡号" + etsk.ICCardCode + "-轴距" + etsk.Distance +
                               "-车型" + etsk.CarSize + "-入库" + inDate, "系统");
                        }
                    }
                    #endregion
                    #region 挪移
                    else if (mtsk.Type == CMasterTask.EnmMasterTaskType.Transpose)
                    {
                        if (isLoad)
                        {
                            frLct.Status = CLocation.EnmLocationStatus.Outing;
                            frLct.Distance = nDist;

                            string msg = "挪移- 装载完成：" + frLct.Address + " 状态：" + frLct.Status.ToString() + " 卡号：" + frLct.ICCardCode;
                            new CWSException(msg, 0);

                            toLct.Status = CLocation.EnmLocationStatus.Entering;
                            CICCard iccard = new CICCard();
                            iccard = new CWICCard().SelectByUserCode(etsk.ICCardCode);    //提前写入卡号
                            toLct.SetICCard(iccard);

                            etsk.Status = CTask.EnmTaskStatus.LoadFinishing;
                            etsk.Distance = nDist;

                            //添加移动作业
                            CTask vtsk = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, 0, mtsk.HID);
                            vtsk.MID = mtsk.ID;
                            mtsk.AddTask(vtsk);
                            taskList.Add(vtsk);

                            //添加卸载作业
                            CTask etvTask = new CTask(frLct.Address, toLct.Address, CTask.EnmTaskType.TVUnload, CTask.EnmTaskStatus.TWaitforUnload,
                                etsk.ICCardCode, etsk.SMG, CSMG.EnmSMGType.ETV, nDist, mtsk.HID, etsk.CarSize);
                            etvTask.MID = mtsk.ID;
                            mtsk.AddTask(etvTask);
                            taskList.Add(etvTask);
                            //更新数据库
                            CWData.myDB.InsertCTasks(mtsk, taskList);
                        }
                        else
                        {
                            inDate = frLct.InDate;
                            frLct.Status = CLocation.EnmLocationStatus.Space;
                            frLct.Distance = 0;
                            frLct.CarSize = "";
                            frLct.InDate = CObject.DefDatetime;
                            frLct.SetICCard(null);

                            string mss = "挪移- 卸载完成，源车位：" + frLct.Address + " 状态：" + frLct.Status.ToString() + " 卡号：" + frLct.ICCardCode;
                            new CWSException(mss, 0);

                            toLct.Status = CLocation.EnmLocationStatus.Occupy;
                            toLct.Distance = etsk.Distance;
                            toLct.CarSize = etsk.CarSize;
                            toLct.InDate = DateTime.Now;

                            string msg = "挪移- 卸载完成，目的车位：" + toLct.Address + " 状态：" + toLct.Status.ToString() + " 卡号：" + toLct.ICCardCode;
                            new CWSException(msg, 0);

                            etsk.Status = CTask.EnmTaskStatus.UnLoadFinishing;

                            //记录
                            log = new CSysLog(etsk.ICCardCode, DateTime.Now, "挪移：源车位" + frLct.Address + "目的车位：" + toLct.Address + "-卡号" +
                                etsk.ICCardCode + "-轴距" + etsk.Distance + "-车型" + etsk.CarSize + "-入库" + inDate, "系统");                          
                        }
                    }
                    #endregion

                    lock (Tasks)
                    {                       
                        //更新数据库
                        CWData.myDB.UpdateCTaskAndLctOfMTsk(mtsk, frLct, toLct);

                        if (log != null)
                        {
                            CWData.myDB.InsertSysLog(log);
                        }

                        if (mtsk.IsCompleted)
                        {
                            Tasks.Remove(mtsk);
                        }
                        else
                        {
                            foreach (CTask tsk in mtsk.Tasks)
                            {
                                if (tsk.Status == CTask.EnmTaskStatus.Finished)
                                {
                                    mtsk.RemoveTask(tsk);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                new CWSException();
                throw ex;
            }
        }

        /// <summary>
        /// 处理移动完成
        /// </summary>
        /// <param name="tskID"></param>
        public void DealTVMoveFinishing(int tskID) 
        {
            try
            {
                CSysLog log = null;
                CTask tsk = this.GetCTaskFromtskID(tskID);
                tsk.Status = CTask.EnmTaskStatus.MoveFinishing;

                CWData.myDB.UpdateCTaskStatus(tsk.Status, tsk.ID);
                if (log != null)
                {
                    CWData.myDB.InsertSysLog(log);
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 完成作业,完全释放对应设备
        /// </summary>
        /// <param name="tid">子作业ID</param>
        public void CompletedCTask(int tid)
        {
            try 
            {
                CMasterTask mtsk = null;
                CTask tsk = null;
                this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);
                if (mtsk == null || tsk == null) 
                {
                    return;
                }
                //完全释放对应设备
                CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                smg.MTskID = 0;
                smg.nIsWorking = 0;                
                smg.NextTaskId = 0;
                CWData.myDB.UpdateSMGTaskStat(smg);

                //2016-4-15 add
                if (mtsk.Type == CMasterTask.EnmMasterTaskType.SaveCar && tsk.Type == CTask.EnmTaskType.TVUnload) 
                {
                    CLocation frLctn = new CWLocation().SelectLctFromAddrs(tsk.ToLctAdrs);
                    if (frLctn != null)
                    {
                        if (frLctn.Status != CLocation.EnmLocationStatus.Occupy)
                        {
                            frLctn.Status = CLocation.EnmLocationStatus.Occupy;
                            frLctn.ICCardCode = tsk.ICCardCode;
                            frLctn.CarSize = tsk.CarSize;
                            frLctn.Distance = tsk.Distance;

                            CWData.myDB.UpdateLocationInfo(frLctn);
                        }
                    }
                }

                tsk.Status = CTask.EnmTaskStatus.Finished;
                CWData.myDB.UpdateCTaskStatus(tsk.Status,tid);
                mtsk.RemoveTask(tsk);

                lock (Tasks)
                {
                    if (mtsk.TaskCount==0)
                    {
                        mtsk.IsCompleted = true;
                        CWData.myDB.UpdateMTaskCompleted(mtsk);
                        Tasks.Remove(mtsk);
                    }
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 移动完成时，处理作业，释放且仅释放ETV的nIsWorking状态位
        /// </summary>
        /// <param name="tid"></param>
        public void DealCompleteTVMoveFinish(int tid)
        {
            try
            {
                CMasterTask mtsk = null;
                CTask tsk = null;
                this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);

                CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                smg.nIsWorking = 0;
                CWData.myDB.UpdateSMGTaskStat(smg);

                tsk.Status = CTask.EnmTaskStatus.Finished;
                CWData.myDB.UpdateCTaskStatus(tsk.Status, tid);
                mtsk.RemoveTask(tsk);

                lock (Tasks)
                {
                    if (mtsk.TaskCount == 0)
                    {
                        mtsk.IsCompleted = true;
                        CWData.myDB.UpdateMTaskCompleted(mtsk);
                        Tasks.Remove(mtsk);
                    }
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 装载完成时，完成作业，释放ETV的ETV的nIsWorking/NextTaskId状态
        /// </summary>
        /// <param name="tid"></param>
        public void DealCompleteTVLoadFinish(int tid) 
        {
            try 
            {
                CMasterTask mtsk = null;
                CTask tsk = null;
                this.GetMTaskAndCTaskOfTid(tid, out mtsk, out tsk);
                lock (lockObj)
                {
                    CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                    smg.nIsWorking = 0;
                    smg.NextTaskId = 0;
                    CWData.myDB.UpdateSMGTaskStat(smg);

                    tsk.Status = CTask.EnmTaskStatus.Finished;
                    CWData.myDB.UpdateCTaskStatus(tsk.Status, tid);
                    mtsk.RemoveTask(tsk);
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }
        }
       
        /// <summary>
        /// 作业维护，完成作业
        /// </summary>
        /// <param name="mid">主作业</param>
        public int CompleteMasterTask(int mid) 
        {
            try 
            {
                lock (lockObj) 
                {
                    CWLocation wlctn = new CWLocation();
                    CMasterTask mtsk = this.GetMasterTaskFromID(mid);

                    mtsk.IsCompleted = true;  //完成主作业
                    //移除车厅声音
                    ClearNotification(mtsk.HID);
                    AddNotification(mtsk.HID, "end");
                    //记录
                    string addrsFr = "";
                    string addrsTo = "";
                    string distance = "";
                    foreach (CTask tsk in mtsk.Tasks)
                    {
                        if (tsk.SMGType == CSMG.EnmSMGType.ETV)
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad || tsk.Type == CTask.EnmTaskType.TVUnload)
                            {
                                addrsFr = tsk.FromLctAdrs;
                                addrsTo = tsk.ToLctAdrs;
                                distance = tsk.Distance.ToString();
                            }
                        }
                    }
                    CSysLog log = null;
                    if (distance != "") 
                    {
                        log = new CSysLog(mtsk.ICCardCode, DateTime.Now, "手动完成作业-" + MtskTypeFormat(mtsk.Type) + "-车厅"
                            + mtsk.HID + "-卡号" + mtsk.ICCardCode + " 源：" + addrsFr + " 往：" + addrsTo + " 轴距：" + distance, "");
                    }
                    else
                    {
                        log = new CSysLog(mtsk.ICCardCode, DateTime.Now, "手动完成作业-" + MtskTypeFormat(mtsk.Type) + "-车厅" + mtsk.HID + "-卡号" + mtsk.ICCardCode, "");
                    }

                    if (log != null)
                    {
                        CWData.myDB.InsertSysLog(log);
                    }

                    for (int i = 0; i < mtsk.Tasks.Length; i++)
                    {
                        CTask tsk = mtsk.Tasks[i];    
                        tsk.Status = CTask.EnmTaskStatus.Finished;  //完成子作业

                        #region 挪移
                        if (mtsk.Type == CMasterTask.EnmMasterTaskType.Transpose)
                        {
                            if (tsk.FromLctAdrs != "")
                            {
                                CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                flt.Status = CLocation.EnmLocationStatus.Space;
                                flt.InDate = CObject.DefDatetime;
                                flt.SetICCard(null);
                                flt.Distance = 0;
                                flt.CarSize = "";

                                string msg = "挪移- 手动完成，源车位：" + flt.Address + " 状态：" + flt.Status.ToString() + " 卡号：" + flt.ICCardCode;
                                new CWSException(msg, 0);

                                CWData.myDB.UpdateLocationInfo(flt);                               
                            }
                            if (tsk.ToLctAdrs != "")
                            {
                                CLocation toLct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                toLct.Status = CLocation.EnmLocationStatus.Occupy;
                                toLct.InDate = DateTime.Now;
                                toLct.Distance = tsk.Distance;
                                toLct.CarSize = tsk.CarSize;

                                CICCard iccard = new CWICCard().SelectByUserCode(tsk.ICCardCode);
                                toLct.SetICCard(iccard);

                                string msg = "挪移- 手动完成，目的车位：" + toLct.Address + " 状态：" + toLct.Status.ToString() + " 卡号：" + toLct.ICCardCode;
                                new CWSException(msg, 0);

                                CWData.myDB.UpdateLocationInfo(toLct);
                            }
                        }
                        #endregion
                        #region 移动
                        else if (mtsk.Type == CMasterTask.EnmMasterTaskType.Move)
                        {
                           
                        }
                        #endregion
                        #region 存车
                        else if (mtsk.Type == CMasterTask.EnmMasterTaskType.SaveCar)
                        {
                            if (tsk.Type == CTask.EnmTaskType.HallCarEntrance)
                            {
                                #region 2015-12-14 
                                //if (tsk.FromLctAdrs != "")  //车厅
                                //{
                                //    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                //    flt.Status = CLocation.EnmLocationStatus.Space;
                                //    flt.InDate = CObject.DefDatetime;
                                //    flt.SetICCard(null);
                                //    flt.Distance = 0;
                                //    flt.CarSize = "";
                                   
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, flt, null);
                                //}
                                //if (tsk.ToLctAdrs != "")
                                //{
                                //    CLocation Lct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                //    Lct.Status = CLocation.EnmLocationStatus.Space;
                                //    Lct.InDate = CObject.DefDatetime;
                                //    Lct.Distance = 0;
                                //    Lct.CarSize = "";
                                //    Lct.SetICCard(null);

                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, Lct);
                                //}
                                #endregion
                            }
                            else if (tsk.Type == CTask.EnmTaskType.TVLoad || tsk.Type == CTask.EnmTaskType.TVUnload)
                            {
                                if (tsk.FromLctAdrs != "")
                                {
                                    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                    flt.Status = CLocation.EnmLocationStatus.Space;
                                    flt.InDate = CObject.DefDatetime;
                                    flt.SetICCard(null);
                                    flt.Distance = 0;
                                    flt.CarSize = "";

                                    string msg = "存车- 手动完成，源车位：" + flt.Address + " 状态：" + flt.Status.ToString() + " 卡号：" + flt.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(flt);
                                }
                                if (tsk.ToLctAdrs != "")
                                {
                                    CLocation Lct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                    Lct.Status = CLocation.EnmLocationStatus.Occupy;
                                    Lct.InDate = DateTime.Now;
                                    Lct.Distance = tsk.Distance;
                                    Lct.CarSize = tsk.CarSize;

                                    CICCard iccad = new CICCard();
                                    iccad = new CWICCard().SelectByUserCode(tsk.ICCardCode);
                                    Lct.SetICCard(iccad);

                                    string msg = "存车- 手动完成，目的车位：" + Lct.Address + " 状态：" + Lct.Status.ToString() + " 卡号：" + Lct.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(Lct);
                                }
                            }
                        }
                        #endregion
                        #region 取车，取物
                        else
                        {
                            if (tsk.Type == CTask.EnmTaskType.HallCarExit)
                            {
                                #region
                                //if (tsk.FromLctAdrs != "")  //车厅
                                //{
                                //    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                //    flt.Status = CLocation.EnmLocationStatus.Space;
                                //    flt.InDate = CObject.DefDatetime;
                                //    flt.SetICCard(null);
                                //    flt.Distance = 0;
                                //    flt.CarSize = "";
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, flt, null);
                                //}
                                //if (tsk.ToLctAdrs != "")
                                //{
                                //    CLocation toLct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                //    toLct.Status = CLocation.EnmLocationStatus.Space;
                                //    toLct.InDate = CObject.DefDatetime;
                                //    toLct.SetICCard(null);
                                //    toLct.Distance = 0;
                                //    toLct.CarSize = "";
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, toLct);
                                //}
                                #endregion
                            }
                            else if (tsk.Type == CTask.EnmTaskType.TVLoad || tsk.Type == CTask.EnmTaskType.TVUnload)
                            {
                                if (tsk.FromLctAdrs != "")
                                {
                                    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                    flt.Status = CLocation.EnmLocationStatus.Space;
                                    flt.InDate = CObject.DefDatetime;
                                    flt.SetICCard(null);
                                    flt.Distance = 0;
                                    flt.CarSize = "";

                                    string msg = "取车- 手动完成，源车位：" + flt.Address + " 状态：" + flt.Status.ToString() + " 卡号：" + flt.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(flt);
                                }
                                if (tsk.ToLctAdrs != "")
                                {
                                    CLocation Lct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                    Lct.Status = CLocation.EnmLocationStatus.Space;
                                    Lct.InDate = CObject.DefDatetime;
                                    Lct.Distance = 0;
                                    Lct.CarSize = "";
                                    Lct.SetICCard(null);

                                    string msg = "取车- 手动完成，目的车位：" + Lct.Address + " 状态：" + Lct.Status.ToString() + " 卡号：" + Lct.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(Lct);
                                }
                            }
                        }
                        #endregion

                        //更新设备作业号
                        CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                        smg.nIsWorking = 0;
                        smg.MTskID = 0;
                        smg.NextTaskId = 0;
                        CWData.myDB.UpdateSMGTaskStat(smg);
                    }

                    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, null);
                    Tasks.Remove(mtsk);
                }
                return 100;
            }
            catch (Exception ex)
            {
                new CWSException();
                mtsksList = null;                
                throw ex;
            }
        }

        /// <summary>
        /// 作业维护，手动复位
        /// </summary>
        /// <param name="mid">主作业</param>
        /// <returns></returns>
        public int ResetMasterTask(int mid) 
        {
            try 
            {
                lock (lockObj) 
                {
                    CWLocation wlctn = new CWLocation();
                    CMasterTask mtsk = this.GetMasterTaskFromID(mid);

                    mtsk.IsCompleted = true;  //完成主作业
                    //移除车厅声音
                    ClearNotification(mtsk.HID);
                    AddNotification(mtsk.HID, "end");
                    //记录
                    string addrsFr = "";
                    string addrsTo = "";
                    string distance = "";
                    foreach (CTask tsk in mtsk.Tasks) 
                    {
                        if (tsk.SMGType == CSMG.EnmSMGType.ETV) 
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad || tsk.Type == CTask.EnmTaskType.TVUnload) 
                            {
                                addrsFr = tsk.FromLctAdrs;
                                addrsTo = tsk.ToLctAdrs;
                                distance = tsk.Distance.ToString();
                            }
                        }
                    }
                    CSysLog log =null;
                    if (distance != "") 
                    {
                        log = new CSysLog(mtsk.ICCardCode, DateTime.Now, "手动复位作业：" + MtskTypeFormat(mtsk.Type) + "-车厅" + mtsk.HID
                            + "-卡号" + mtsk.ICCardCode + " 源：" + addrsFr + " 往：" + addrsTo + " 轴距：" + distance, "");
                    }
                    else
                    {
                        log = new CSysLog(mtsk.ICCardCode, DateTime.Now, "手动复位作业：" + MtskTypeFormat(mtsk.Type) + "-车厅" + mtsk.HID + "-卡号" + mtsk.ICCardCode, "");
                    }

                    if (log != null)
                    {
                        CWData.myDB.InsertSysLog(log);
                    }

                    for (int i = 0; i < mtsk.Tasks.Length; i++)
                    {
                        CTask tsk = mtsk.Tasks[i];                       
                        tsk.Status = CTask.EnmTaskStatus.Finished;  //完成子作业  
                        #region 挪移
                        if (mtsk.Type == CMasterTask.EnmMasterTaskType.Transpose)
                        {
                            if (tsk.FromLctAdrs != "")
                            {
                                CLocation Lct = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);

                                Lct.Status = CLocation.EnmLocationStatus.Occupy;
                                Lct.InDate = DateTime.Now;

                                CICCard iccd = new CICCard();
                                iccd = new CWICCard().SelectByUserCode(tsk.ICCardCode);
                                Lct.SetICCard(iccd);    //副本复制
                                Lct.Distance = tsk.Distance;
                                Lct.CarSize = tsk.CarSize;

                                string msg = "挪移- 手动复位，源车位：" + Lct.Address + " 状态：" + Lct.Status.ToString() + " 卡号：" + Lct.ICCardCode;
                                new CWSException(msg, 0);

                                CWData.myDB.UpdateLocationInfo(Lct);
                            }
                            if (tsk.ToLctAdrs != "")
                            {
                                CLocation tlt = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                tlt.Status = CLocation.EnmLocationStatus.Space;
                                tlt.InDate = CObject.DefDatetime;
                                tlt.SetICCard(null);
                                tlt.Distance = 0;
                                tlt.CarSize = "";

                                string msg = "挪移- 手动复位，源车位：" + tlt.Address + " 状态：" + tlt.Status.ToString() + " 卡号：" + tlt.ICCardCode;
                                new CWSException(msg, 0);

                                CWData.myDB.UpdateLocationInfo(tlt);
                            }
                        }
                        #endregion
                        #region 移动
                        else if (mtsk.Type == CMasterTask.EnmMasterTaskType.Move)
                        {
                            
                        }
                        #endregion;
                        #region  存车
                        else if (mtsk.Type == CMasterTask.EnmMasterTaskType.SaveCar)
                        {
                            if (tsk.Type == CTask.EnmTaskType.HallCarEntrance)       //存车
                            {                               
                                #region
                                //if (tsk.FromLctAdrs != "")  //车厅
                                //{
                                //    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                //    flt.Status = CLocation.EnmLocationStatus.Space;
                                //    flt.InDate = CObject.DefDatetime;
                                //    flt.SetICCard(null);
                                //    flt.Distance = 0;
                                //    flt.CarSize = "";
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, flt, null);
                                //}
                                //if (tsk.ToLctAdrs != "")
                                //{
                                //    CLocation toLct = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                //    toLct.Status = CLocation.EnmLocationStatus.Space;
                                //    toLct.InDate = CObject.DefDatetime;
                                //    toLct.SetICCard(null);
                                //    toLct.Distance = 0;
                                //    toLct.CarSize = "";
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, toLct);
                                //}
                                #endregion
                            }                          
                            else if (tsk.Type == CTask.EnmTaskType.TVLoad||tsk.Type==CTask.EnmTaskType.TVUnload)
                            {
                                if (tsk.FromLctAdrs != "")
                                {
                                    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                    flt.Status = CLocation.EnmLocationStatus.Space;
                                    flt.InDate = CObject.DefDatetime;
                                    flt.SetICCard(null);
                                    flt.Distance = 0;
                                    flt.CarSize = "";

                                    string msg = "存车- 手动复位，源车位：" + flt.Address + " 状态：" + flt.Status.ToString() + " 卡号：" + flt.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(flt);
                                }
                                if (tsk.ToLctAdrs != "")
                                {
                                    CLocation tlt = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                    tlt.Status = CLocation.EnmLocationStatus.Space;
                                    tlt.InDate = CObject.DefDatetime;
                                    tlt.SetICCard(null);
                                    tlt.Distance = 0;
                                    tlt.CarSize = "";

                                    string msg = "存车- 手动复位，目的车位：" + tlt.Address + " 状态：" + tlt.Status.ToString() + " 卡号：" + tlt.ICCardCode;
                                    new CWSException(msg, 0);

                                    CWData.myDB.UpdateLocationInfo(tlt);
                                }
                            }                           
                        }
                        #endregion
                        #region   取车，取物
                        else   
                        {
                            if (tsk.Type == CTask.EnmTaskType.HallCarExit)   //取车
                            {                               
                                #region
                                //if (tsk.FromLctAdrs != "")
                                //{
                                //    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                //    flt.Status = CLocation.EnmLocationStatus.Space;
                                //    flt.InDate = CObject.DefDatetime;                                   
                                //    flt.Distance = 0;
                                //    flt.CarSize = "";
                                //    flt.SetICCard(null);

                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, flt, null);
                                //}
                                //if (tsk.ToLctAdrs != "")
                                //{
                                //    CLocation tlt = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                //    tlt.Status = CLocation.EnmLocationStatus.Space;
                                //    tlt.InDate = CObject.DefDatetime;
                                //    tlt.SetICCard(null);
                                //    tlt.Distance = 0;
                                //    tlt.CarSize = "";
                                //    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, tlt);
                                //}
                                #endregion
                            }
                            else if (tsk.Type == CTask.EnmTaskType.TVLoad || tsk.Type == CTask.EnmTaskType.TVUnload)
                            {
                                if (tsk.FromLctAdrs != "")
                                {
                                    CLocation flt = wlctn.SelectLctFromAddrs(tsk.FromLctAdrs);
                                    flt.Status = CLocation.EnmLocationStatus.Occupy;
                                    flt.InDate = DateTime.Now;
                                    flt.Distance = tsk.Distance;
                                    flt.CarSize = tsk.CarSize;

                                    CICCard iccad = new CICCard();
                                    iccad = new CWICCard().SelectByUserCode(tsk.ICCardCode);
                                    flt.SetICCard(iccad);

                                    string msg = "取车- 手动复位，源车位：" + flt.Address + " 状态：" + flt.Status.ToString() + " 卡号：" + flt.ICCardCode;
                                    new CWSException(msg, 0);
                                    CWData.myDB.UpdateLocationInfo(flt);                                    
                                }
                                if (tsk.ToLctAdrs != "")
                                {
                                    CLocation tlt = wlctn.SelectLctFromAddrs(tsk.ToLctAdrs);
                                    tlt.Status = CLocation.EnmLocationStatus.Space;
                                    tlt.InDate = CObject.DefDatetime;
                                    tlt.SetICCard(null);
                                    tlt.Distance = 0;
                                    tlt.CarSize = "";

                                    string msg = "取车- 手动复位，目的车位：" + tlt.Address + " 状态：" + tlt.Status.ToString() + " 卡号：" + tlt.ICCardCode;
                                    new CWSException(msg, 0);
                                    CWData.myDB.UpdateLocationInfo(tlt);                                    
                                }
                            }                        
                        }
                        #endregion
                        //更新设备作业号
                        CSMG smg = new CWSMG().SelectSMG(tsk.SMG);
                        smg.nIsWorking = 0;
                        smg.MTskID = 0;
                        smg.NextTaskId = 0;
                        CWData.myDB.UpdateSMGTaskStat(smg);
                    }

                    CWData.myDB.CompleteMasterTaskAndLctn(mtsk, null, null);
                    Tasks.Remove(mtsk);
                }

                return 100;
            }
            catch (Exception ex) 
            {
                new CWSException();
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 完成车厅卸载时处理
        /// </summary>
        /// <param name="HTid"></param>
        public void ODealEVUp(int tskID)
        {
            try 
            {
                CTask htsk =null;
                CMasterTask mtsk = null;
                this.GetMTaskAndCTaskOfTid(tskID, out mtsk, out htsk);
                lock (lockObj)
                {
                    htsk.Status = CTask.EnmTaskStatus.OCarOutWaitforDriveaway;

                    CLocation lctn = new CWLocation().SelectLctFromAddrs(htsk.ToLctAdrs);  //获取车厅地址
                    lctn.Status = CLocation.EnmLocationStatus.Space;    //释放车厅地址
                    lctn.CarSize = "";
                    lctn.InDate = CObject.DefDatetime;
                    lctn.Distance = 0;
                    lctn.ICCardCode = "";
                    //更新数据库
                    CWData.myDB.UpdateCTaskAndLoctation(htsk.ID, lctn, htsk.Status);

                    if (mtsk.Type == CMasterTask.EnmMasterTaskType.TempGetCar)
                    {
                        this.AddNotification(htsk.HID, "40.wav");
                    }
                    else
                    {
                        this.AddNotification(htsk.HID, "32.wav");
                    }
                }
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 查找指定车厅的所有主作业,索引为0则查找所有主作业
        /// </summary>
        /// <param name="hid"></param>
        /// <returns></returns>
        public CMasterTask[] GetAllCMasterTaskOfHid(int hid) 
        {
            if (hid == 0)
            {
                return Tasks.ToArray();
            }
            else
            {
                return Tasks.FindAll(delegate(CMasterTask mtsk) { return mtsk.HID == hid; }).ToArray();
            }
        }

        /// <summary>
        /// 车位分配及ETV分配
        /// </summary>
        /// <param name="tid">作业号</param>
        /// <param name="iccd"></param>
        /// <param name="carSize"></param>
        /// <param name="smgID"></param>
        /// <returns></returns>
        public CLocation IAllocateLocation(int tid, CICCard iccd, string carSize, ref int smgID) 
        {
            try 
            {
                CWSMG cwsmg=new CWSMG();
                CWLocation Wlct=new CWLocation();

                CTask htsk = this.GetCTaskFromtskID(tid);
                CSMG hall = cwsmg.SelectSMG(htsk.SMG);

                CSMG[] etvs = cwsmg.SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                CLocation[] locations = Wlct.SelectAllLocations();
                CLocation lct = null;
                int asmg;
                //临时卡及定期卡
                if (iccd.Type == CICCard.EnmICCardType.Temp || iccd.Type == CICCard.EnmICCardType.Fixed) 
                {
                    lct= new IEGBLL.AllocateETV.SaveCarAllocate().Allocate(out asmg, carSize, hall, locations, etvs);
                    smgID = asmg;
                }
                else if (iccd.Type == CICCard.EnmICCardType.FixedLocation)  //固定卡存车
                {
                    lct = Wlct.SelectLctFromAddrs(iccd.Address);
                    if (lct == null) 
                    {
                        //找不到对应车位
                        this.AddNotification(hall.ID,"39.wav");
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        return null;
                    }
                    if (lct.Type == CLocation.EnmLocationType.Disable || lct.Status != CLocation.EnmLocationStatus.Space)
                    {
                        //该车位出现故障
                        this.AddNotification(hall.ID, "28.wav");
                        htsk.Status = CTask.EnmTaskStatus.ISecondSwipedWaitforCarLeave;
                        return null;
                    }
                    //分配ETV
                    smgID = new AllocateETV.SaveCarAllocate().AllocateEtv(hall, lct, etvs);                    
                }

                return lct;
            }
            catch (Exception ex)
            {
                new CWSException();
                throw ex;
            }
        }

        /// <summary>
        /// 手动指令创建
        /// </summary>       
        public int CreateManualTask(CLocation flct, CLocation tlct,CSMG hall, CMasterTask.EnmMasterTaskType mtype)
        {
            try
            {
                lock (lockObj)
                {
                    #region  挪移
                    if (mtype == CMasterTask.EnmMasterTaskType.Transpose)
                    {
                        if (flct.Type == CLocation.EnmLocationType.Hall || tlct.Type == CLocation.EnmLocationType.Hall)
                        {
                            return 110; //源、目的地不正确
                        }
                        if (flct.ICCardCode == "" || tlct.ICCardCode != "")
                        {
                            return 111; //源、目的地用户卡号不正确
                        }
                        if (flct.Status != CLocation.EnmLocationStatus.Occupy || tlct.Status != CLocation.EnmLocationStatus.Space)
                        {
                            return 112;//源、目的车位状态不正确
                        }
                        if (flct.CarSize.CompareTo(tlct.Size) > 0)
                        {
                            return 113; //目的车位尺寸不合格
                        }

                        //分配ETV                
                        CSMG[] Etvs = new CWSMG().SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                        int Etv = new AllocateETV.GetCarAllocate().AllocateEtv(flct, tlct.Address, Etvs);

                        if (Etv != 0)
                        {
                            flct.Status = CLocation.EnmLocationStatus.Outing;
                            tlct.Status = CLocation.EnmLocationStatus.Entering;
                            //创建作业
                            CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.Transpose, flct.ICCardCode, 11, flct.Warehouse);
                            //建立移动作业
                            string currAddrs = new CWSMG().GetEtvCurrAddress(Etv);
                            CTask vtsk = new CTask(currAddrs, flct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                                flct.ICCardCode, Etv, CSMG.EnmSMGType.ETV, 0, 11);
                            mtsk.AddTask(vtsk);
                            //建立装载作业
                            CTask tsk = new CTask(flct.Address, tlct.Address, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad,
                                flct.ICCardCode, Etv, CSMG.EnmSMGType.ETV, flct.Distance, 11, flct.CarSize);
                            mtsk.AddTask(tsk);

                            lock (Tasks)
                            {
                                CWData.myDB.InsertAMasterTaskAndLct(mtsk, flct, tlct);
                                Tasks.Add(mtsk);
                            }
                            return 100;
                        }
                        else
                        {
                            return 114; //没有可达的ETV
                        }
                    }
                    #endregion
                    #region 移动
                    if (mtype == CMasterTask.EnmMasterTaskType.Move)
                    {
                        CSMG etv = new CWSMG().SelectEtvOfRegionDependM(flct.Address);
                        if (etv != null)
                        {
                            if (new CWSMG().CheckEtvMode(etv.ID) == false)
                            {
                                return 120; //当前ETV不可用
                            }

                            CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.Move, "", 11, flct.Warehouse);

                            CTask tsk = new CTask(flct.Address, tlct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove, "", etv.ID,
                                CSMG.EnmSMGType.ETV, 0, 11);
                            mtsk.AddTask(tsk);

                            lock (Tasks)
                            {
                                CWData.myDB.InsertAMasterTask(mtsk);
                                Tasks.Add(mtsk);
                            }

                            CSysLog log = new CSysLog("", DateTime.Now, "移动：源地址" + flct.Address + "-目的地址" + tlct.Address + "-时间" + DateTime.Now, "手动");
                            CWData.myDB.InsertSysLog(log);
                            return 100;
                        }
                        else
                        {
                            return 120; //请选择ETV所在列
                        }
                    }
                    #endregion
                    #region 出库
                    if (mtype == CMasterTask.EnmMasterTaskType.GetCar)
                    {
                        if (flct.ICCardCode == "")
                        {
                            return 130;
                        }
                        if (flct.Status != CLocation.EnmLocationStatus.Occupy)
                        {
                            return 131;
                        }

                        CSMG[] Etvs = new CWSMG().SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                        int Etv = new AllocateETV.GetCarAllocate().AllocateEtv(flct, hall.Address, Etvs);
                        if (Etv != 0)
                        {
                            StringBuilder msg = new StringBuilder();
                            msg.AppendLine(string.Format("界面出车- 分配ETV" + Etv + "  取车位：" + flct.Address + "  出车厅：" + hall.ID));
                            msg.AppendLine(string.Format("界面出车- 取车前车位：" + flct.Address + " 状态：" + flct.Status.ToString() + " 卡号：" + flct.ICCardCode));
                            new CWSException(msg.ToString(), 0);

                            flct.Status = CLocation.EnmLocationStatus.Outing;
                            //创建作业
                            CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.GetCar, flct.ICCardCode, hall.ID, flct.Warehouse);

                            CTask htsk = new CTask(flct.Address, tlct.Address, CTask.EnmTaskType.HallCarExit, CTask.EnmTaskStatus.OWaitforEVDown, flct.ICCardCode,
                                hall.ID, CSMG.EnmSMGType.Hall, flct.Distance, hall.ID, flct.CarSize);
                            mtsk.AddTask(htsk);

                            string currAddrs = new CWSMG().GetEtvCurrAddress(Etv);
                            CTask vtsk = new CTask(currAddrs, flct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove, flct.ICCardCode, Etv,
                                CSMG.EnmSMGType.ETV, 0, hall.ID);
                            mtsk.AddTask(vtsk);

                            CTask etsk = new CTask(flct.Address, tlct.Address, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad, flct.ICCardCode, Etv,
                                CSMG.EnmSMGType.ETV, flct.Distance, hall.ID, flct.CarSize);
                            mtsk.AddTask(etsk);

                            lock (Tasks)
                            {
                                CWData.myDB.InsertAMasterTaskAndLct(mtsk, flct, null);
                                Tasks.Add(mtsk);
                            }
                            this.AddNotification(hall.ID, "30.WAV"); //正在出车，请稍后
                            return 100;
                        }
                        else
                        {
                            return 132; //没有可达ETV
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }            
            return 0;
        }

        /// <summary>
        /// 取物时创建作业
        /// </summary>
        /// <param name="hnmb">车厅号</param>
        /// <param name="taddrs">车厅地址</param>
        /// <param name="iccd">用户卡</param>
        /// <param name="lct">出车车位</param>
        /// <returns></returns>
        public int OTemp_GetCar(int hnmb,string taddrs, CICCard iccd, CLocation flct) 
        {
            try
            {
                CSMG[] Etvs = new CWSMG().SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                int Etv = new AllocateETV.GetCarAllocate().AllocateEtv(flct, taddrs, Etvs);
                if (Etv == 0)
                {
                    this.AddNotification(hnmb, "38.wav");
                    return 108;
                }

                string mse = "取物出车- 分配ETV" + Etv + "  取车位：" + taddrs + "  出车厅：" + hnmb;
                new CWSException(mse, 0);

                lock (lockObj)
                {
                    CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.TempGetCar, iccd.Code, hnmb, flct.Warehouse, true);
                    //增加车厅作业
                    CTask htsk = new CTask(flct.Address, taddrs, CTask.EnmTaskType.HallCarExit, CTask.EnmTaskStatus.TempOWaitforEVDown,
                        iccd.Code, hnmb, CSMG.EnmSMGType.Hall, flct.Distance, hnmb, flct.CarSize);
                    mtsk.AddTask(htsk);

                    string currAddrs = new CWSMG().GetEtvCurrAddress(Etv);
                    //添加移动作业
                    CTask vtsk = new CTask(currAddrs, flct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                        iccd.Code, Etv, CSMG.EnmSMGType.ETV, 0, hnmb);
                    mtsk.AddTask(vtsk);

                    //增加装载作业
                    CTask etsk = new CTask(flct.Address, taddrs, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad, iccd.Code,
                        Etv, CSMG.EnmSMGType.ETV, flct.Distance, hnmb, flct.CarSize);
                    mtsk.AddTask(etsk);

                    string msg = "取物- 取前车位：" + flct.Address + " 状态：" + flct.Status.ToString() + " 卡号：" + flct.ICCardCode;
                    new CWSException(msg, 0);

                    flct.Status = CLocation.EnmLocationStatus.Outing;

                    CWData.myDB.InsertAMasterTaskAndLct(mtsk, flct, null);
                    Tasks.Add(mtsk);

                    this.AddNotification(hnmb, "30.wav");
                }
                return 100;
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 处理车辆开离车厅
        /// </summary>
        /// <param name="tID"></param>
        /// <returns></returns>
        public int DealCarDriveAway(int tID) 
        {
            try 
            {
                CTask htsk = null;
                CMasterTask mtsk = null;
                int rit= this.GetMTaskAndCTaskOfTid(tID, out mtsk, out htsk);
                if (rit == 1)
                {

                    CLocation flct = null;
                    if (mtsk.Type == CMasterTask.EnmMasterTaskType.TempGetCar)
                    {
                        flct = new CWLocation().SelectLctFromAddrs(htsk.FromLctAdrs);
                        if (flct != null)
                        {
                            flct.Status = CLocation.EnmLocationStatus.Space;      //修改取物车位状态为空
                        }
                    }
                    htsk.Status = CTask.EnmTaskStatus.HallFinishing;
                    //更新数据库
                    CWData.myDB.UpdateCTaskAndLctOfStatus(htsk, flct);
                    return 100;
                }
                return 101;
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 处理车位跑偏
        /// </summary>
        /// <param name="tID"></param>
        public void DealCarDriveOffTracing(int tID) 
        {
            try 
            {
                CTask htsk = null;
                CMasterTask mtsk = null;
                int rit= this.GetMTaskAndCTaskOfTid(tID, out mtsk, out htsk);
                lock (lockObj)
                {
                    if (rit == 1)
                    {
                        this.AddNotification(htsk.SMG, "42.wav");  //车辆跑位
                        if (mtsk.IsTemp == true) //是取物存车
                        {
                            htsk.Status = CTask.EnmTaskStatus.IFirstSwipedWaitforCheckSize;
                            htsk.StatusDetail = CTask.EnmTaskStatusDetail.NoSend;
                            CWData.myDB.UpdateCTaskStatus(htsk.Status, tID);
                            this.AddNotification(htsk.HID, "44.wav");
                        }
                        else
                        {
                            this.AddNotification(htsk.HID, "43.wav");
                            this.AddNotification(mtsk.HID, "end");
                            //释放车厅
                            CSMG smg = new CWSMG().SelectSMG(htsk.SMG);
                            smg.nIsWorking = 0;
                            smg.NextTaskId = 0;
                            smg.MTskID = 0;
                            CWData.myDB.UpdateSMGTaskStat(smg);

                            mtsk.IsCompleted = true;
                            for (int i = 0; i < mtsk.TaskCount; i++)
                            {
                                CTask tsk = mtsk.Tasks[i];
                                tsk.Status = CTask.EnmTaskStatus.Finished;
                            }

                            lock (Tasks)
                            {
                                CWData.myDB.CompleteMasterTask(mtsk);   //完成作业                           
                                Tasks.Remove(mtsk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                mtsksList = null;
                throw ex;
            }
        }

        public void OTempICcdGetCar(int hallID,int etvID, string iccode, string hallAddrs, CLocation fLct,string EtvAddrs) 
        {
            try 
            {
                lock (lockObj)
                {
                    //建立主作业
                    CMasterTask mtsk = new CMasterTask(CMasterTask.EnmMasterTaskType.GetCar, iccode, hallID, fLct.Warehouse);
                    //建立车厅作业
                    CTask htsk = new CTask(fLct.Address, hallAddrs, CTask.EnmTaskType.HallCarExit, CTask.EnmTaskStatus.OWaitforEVDown,
                        iccode, hallID, CSMG.EnmSMGType.Hall, fLct.Distance, hallID, fLct.CarSize);
                    mtsk.AddTask(htsk);

                    //建立移动作业
                    CTask vtsk = new CTask(EtvAddrs, fLct.Address, CTask.EnmTaskType.TVMove, CTask.EnmTaskStatus.TWaitforMove,
                        iccode, etvID, CSMG.EnmSMGType.ETV, 0, hallID);
                    mtsk.AddTask(vtsk);

                    //建立装载作业
                    CTask etsk = new CTask(fLct.Address, hallAddrs, CTask.EnmTaskType.TVLoad, CTask.EnmTaskStatus.TWaitforLoad, iccode,
                        etvID, CSMG.EnmSMGType.ETV, fLct.Distance, hallID, fLct.CarSize);
                    mtsk.AddTask(etsk);

                    string msg = "缴费出车- 取前车位：" + fLct.Address + " 状态：" + fLct.Status.ToString() + " 卡号：" + fLct.ICCardCode;
                    new CWSException(msg, 0);

                    fLct.Status = CLocation.EnmLocationStatus.Outing;

                    CWData.myDB.InsertAMasterTaskAndLct(mtsk, fLct, null);
                    Tasks.Add(mtsk);
                    this.AddNotification(hallID, "30.wav");
                }          
            }
            catch (Exception ex)
            {
                mtsksList = null;
                throw ex;
            }
        }

        /// <summary>
        /// 检查移动路径是否被堵住，是否需要生成避让作业
        /// </summary>
        /// <param name="tid">移动作业的子作业</param>
        /// <returns>false- 不允许下发 true- 允许下发</returns>
        public bool CheckMovePathIsBlock(int tid) 
        {
            try
            {
                CWSMG cwsmg = new CWSMG();
                CTask etsk;
                CMasterTask mtask;
                this.GetMTaskAndCTaskOfTid(tid, out mtask, out etsk);
                if (mtask == null) 
                {
                    return false;
                }

                int maxCol = 0; //移动上限
                int minCol = 0; //移动下限
                #region
                int cToCol = GetColumnOfAddress(etsk.ToLctAdrs);  //移动终点
                string cFromAddrs = cwsmg.GetEtvCurrAddress(etsk.SMG);
                int cFromCol = GetColumnOfAddress(cFromAddrs);    //起始点

                if (cFromCol > cToCol)
                {
                    minCol = cToCol - 4;
                    if (minCol < 1) 
                    {
                        minCol = 1;
                    }
                    maxCol = cFromCol;
                }
                else
                {
                    minCol = cFromCol;
                    maxCol = cToCol + 4;
                    if (maxCol > 20) 
                    {
                        maxCol = 20;
                    }
                }
                #endregion
                CSMG other = null; //另一ETV
                #region
                CSMG[] Etvs = cwsmg.SelectSMGsOfType(CSMG.EnmSMGType.ETV);                
                foreach (CSMG etv in Etvs) 
                {
                    if (etv.ID != etsk.SMG) 
                    {
                        other = etv;
                        break;
                    }
                }
                #endregion
                if (other != null) 
                {
                    string cEtvAddrs = cwsmg.GetEtvCurrAddress(other.ID);
                    int cEtvCol = GetColumnOfAddress(cEtvAddrs);  //当前列       
                    #region
                    int curEtvID = etsk.SMG;
                    bool isReturn = false;
                    if (curEtvID < other.ID)  // 1< 2
                    {
                        if (cFromCol > cEtvCol)
                        {
                            isReturn = true;
                        }
                    }
                    else 
                    {
                        if (cFromCol < cEtvCol) 
                        {
                            isReturn = true;
                        }
                    }
                    if (isReturn) 
                    {
                        string msg = String.Format("异常： ETV{0} 当前列{1},ETV{2} 当前列{3}", curEtvID, cFromCol, other.ID, cEtvCol);
                        new CWSException(msg, 2);
                        return false;
                    }
                    #endregion
                    if (other.nIsWorking == 0)  //注意，没有作业，但可能正处于要下发移动卸载阶段
                    {
                        if (other.MTskID == 0)
                        {
                            #region 没有作业
                            if (minCol < cEtvCol && maxCol > cEtvCol)
                            {
                                #region 生成避让作业
                                string oLayer = cEtvAddrs.Substring(3);
                                string oList = "";
                                if (cFromCol > cToCol)     //需向左避让
                                {                                    
                                    oList = minCol.ToString().PadLeft(2, '0');
                                }
                                else
                                {
                                    oList = maxCol.ToString().PadLeft(2, '0');

                                }
                                lock (Tasks)
                                {
                                    string toAddress = String.Concat("1", oList, oLayer);
                                    CTask avoidTask = new CTask(cEtvAddrs, toAddress, CTask.EnmTaskType.TVAvoid, CTask.EnmTaskStatus.TWaitforMove,
                                        mtask.ICCardCode, other.ID, CSMG.EnmSMGType.ETV, 0, mtask.HID);
                                    avoidTask.MID = mtask.ID;
                                    CWData.myDB.InsertCTask(avoidTask);
                                    mtask.AddTask(avoidTask);
                                    //绑定于设备上,注意只绑定避让的子作业号，主作业号不处理
                                    other.nIsWorking = avoidTask.ID;
                                    CWData.myDB.UpdateSMGTaskStat(other);
                                }
                                #endregion
                            }
                            return true;
                            #endregion
                        }
                        else   //处于要下发移动卸载阶段
                        {
                            CMasterTask otherMaster = this.GetMasterTaskFromID(other.MTskID);
                            if (otherMaster != null)
                            {
                                CTask nextTask = null;
                                foreach (CTask ctsk in otherMaster.Tasks) 
                                {
                                    if (ctsk.SMGType == CSMG.EnmSMGType.ETV) 
                                    {
                                        nextTask = ctsk;
                                    }
                                }
                                if (nextTask != null)
                                {
                                    return false;  //如果另一台车在等待中，就让其先动作，当前ETV先不下发，
                                }
                            }
                        }
                    }
                    else  
                    {
                        //有作业在进行，则判断是否是装卸载作业，如果是，则判断ETV是否在范围内，
                        //如果是移动作业，则判断移动的方向和终点，看看当前的是不是能动作
                        #region
                        CTask currtsk = this.GetCTaskFromtskID(other.nIsWorking);
                        if (currtsk == null) 
                        {
                            return true;
                        }
                        if (currtsk.Type != CTask.EnmTaskType.Move) //是装卸载作业
                        {
                            if (minCol < cEtvCol && maxCol > cEtvCol)
                            {
                                return false; //在移动范围内进行装卸载
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else //正在进行移动作业
                        {                            
                            #region 简单化点，如果移动区域存在有交集就不允许当前指令下发，待移动完全后再处理
                            int currToCol = GetColumnOfAddress(currtsk.ToLctAdrs);
                            int curMin=0;
                            int curMax=0;
                            if (cEtvCol > currToCol)
                            {
                                curMax = cEtvCol;
                                curMin = currToCol - 4;
                            }
                            else
                            {
                                curMax = currToCol + 4;
                                curMin = cEtvCol;
                            }
                            if (curMin < minCol && minCol < curMax) 
                            {
                                return false;
                            }
                            if (curMin < maxCol &&maxCol < curMax)
                            {
                                return false;
                            }
                            if (minCol < curMin && curMin < maxCol) 
                            {
                                return false;
                            }
                            if (minCol < curMax && curMax < maxCol)
                            {
                                return false;
                            }
                            #endregion

                            return true;
                        }
                        #endregion
                    }
                }
                return false;    
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断是否可以执行移动，如果是，是否需要生成避让
        /// 1、当前作业未开始（以绑定的主作业来判断），判断是否可以开始，
        ///    1.1 另一ETV处于空闲时
        ///    1.2 另一ETV已绑定了作业
        ///        1.2.1 处于等待中，如果在范围内，则不允许开始
        ///        1.2.2 在进行装卸载时
        ///        1.2.3 在进行移动时，如果在移动范围内，则不允许开始
        /// 2、当前主作业已下发，处于等待 移动->卸载 状态
        ///   2.1 另一ETV处于空闲状态或等待中时，则判断是否需生成避让
        ///  
        ///   2.2 另一个ETV处于作业中，则让当前等待
        /// </summary>
        /// <param name="tid">ETV子作业</param>
        public void DealEtvBiRang(int tid) 
        {
            try
            {
                CTask etsk;
                CMasterTask mtask;
                this.GetMTaskAndCTaskOfTid(tid, out mtask, out etsk);

                CWSMG cwsmg = new CWSMG();
                CSMG[] Etvs = cwsmg.SelectSMGsOfType(CSMG.EnmSMGType.ETV);
                //当前ETV
                CSMG etv = cwsmg.SelectSMG(etsk.SMG);
                string curAddrs = cwsmg.GetEtvCurrAddress(etv.ID);
                int curEtvCol = GetColumnOfAddress(curAddrs);
                int curToCol = GetColumnOfAddress(etsk.ToLctAdrs);  //移动终点
                
                int curMax;
                int curMin;
                if (curEtvCol > curToCol)
                {
                    curMax = curEtvCol;
                    curMin = curToCol - 4;
                    if (curMin < 1)
                    {
                       curMin = 1;
                    }
                }
                else 
                {
                    curMax = curToCol + 4;
                    if (curMax > 20) 
                    {
                        curMax = 20;
                    }
                    curMin = curEtvCol;                    
                }               
                //对面ETV
                CSMG otherEtv = null;
                #region
                foreach (CSMG et in Etvs) 
                {
                    if (et.ID != etv.ID) 
                    {
                        otherEtv = et;
                        break;
                    }
                }
                #endregion
                string otherAddrs = cwsmg.GetEtvCurrAddress(otherEtv.ID);
                int otherCol = GetColumnOfAddress(otherAddrs);

                #region 列数与实际不合的，先不让其去执行
                bool isReturn = false;
                if (etv.ID < otherEtv.ID)   //1 < 2
                {
                    if (curEtvCol > otherCol)
                    {
                        isReturn = true;
                    }
                }
                else   //  2>  1
                {
                    if (curEtvCol < otherCol) 
                    {
                        isReturn = true;
                    }
                }

                if (isReturn) 
                {
                    string msg= String.Format("异常： ETV{0} 当前列{1},ETV{2} 当前列{3}", etv.ID, curEtvCol, otherEtv.ID, otherCol);
                    new CWSException(msg, 2);
                    return;
                }
                #endregion

                //当前作业未开始
                if (etv.MTskID == 0)
                {
                    #region
                    if (otherEtv.MTskID == 0)
                    {
                        #region
                        if (otherCol > curMin && otherCol < curMax)
                        {
                            #region 生成避让作业
                            string oLayer = otherAddrs.Substring(3);
                            string oList = "";
                            if (curEtvCol > curToCol)     //需向左避让
                            {
                                oList = curMin.ToString().PadLeft(2, '0');
                            }
                            else
                            {
                                oList = curMax.ToString().PadLeft(2, '0');
                            }

                            lock (Tasks)
                            {
                                string lasttToAddress = String.Concat("1", oList, oLayer);
                                CTask avoidTask = new CTask(otherAddrs, lasttToAddress, CTask.EnmTaskType.TVAvoid, CTask.EnmTaskStatus.TWaitforMove,
                                    mtask.ICCardCode, otherEtv.ID, CSMG.EnmSMGType.ETV, 0, mtask.HID);
                                avoidTask.MID = mtask.ID;
                                CWData.myDB.InsertCTask(avoidTask);
                                mtask.AddTask(avoidTask);
                                //绑定于设备上,注意只绑定避让的子作业号，主作业号不处理
                                otherEtv.nIsWorking = avoidTask.ID;
                                CWData.myDB.UpdateSMGTaskStat(otherEtv);
                            }
                            #endregion
                        }
                        int next = 0;
                        #region
                        foreach (CTask tsk in mtask.Tasks)
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad)
                            {
                                next = tsk.ID;
                                break;
                            }
                        }
                        #endregion
                        etv.nIsWorking = etsk.ID;
                        etv.NextTaskId = next;
                        etv.MTskID = mtask.ID;
                        CWData.myDB.UpdateSMGTaskStat(etv);
                        #endregion
                    }
                    else //另一ETV绑定了作业
                    {
                        #region
                        CMasterTask otherMaster = this.GetMasterTaskFromID(otherEtv.MTskID);
                        if (otherMaster != null)
                        {
                            if (otherEtv.nIsWorking == 0)  //处于等待中，可能是在等待装卸载，可能是在等待下个移动
                            {
                                #region
                                CTask nextTask = null;
                                foreach (CTask ctsk in otherMaster.Tasks)
                                {
                                    if (ctsk.SMGType == CSMG.EnmSMGType.ETV)
                                    {
                                        nextTask = ctsk;
                                    }
                                }
                                if (nextTask != null)
                                {
                                    return;  //先让有作业的先去动作先
                                    #region  屏蔽
                                    //if (nextTask.Type == CTask.EnmTaskType.TVLoad ||
                                    //    nextTask.Type == CTask.EnmTaskType.TVUnload)
                                    //{
                                    //    if (otherCol > curMin && otherCol < curMax)
                                    //    {
                                    //        return;
                                    //    }
                                    //}
                                    //if (nextTask.Type == CTask.EnmTaskType.TVMove) 
                                    //{
                                    //    int nextToCol = GetColumnOfAddress(nextTask.ToLctAdrs);
                                    //    //ETV位于当前在最大值外，但终点在当前范围左侧，则当前不允许下发
                                    //    if (otherCol > curMax && curMax > nextToCol)
                                    //    {
                                    //        return;
                                    //    }
                                    //    //ETV位于当前在最小值外，但终点在当前范围右侧，则当前不允许下发
                                    //    if (curMin > otherCol && nextToCol > curMin)
                                    //    {
                                    //        return;
                                    //    }
                                    //    //有点落到区域内时不允许下发
                                    //    if ((curMax > otherCol && curMin < otherCol) ||
                                    //        (curMax > nextToCol && curMin < nextToCol))
                                    //    {
                                    //        return;
                                    //    }
                                    //}
                                    #endregion
                                }
                                #endregion
                            }
                            else  //正在进行作业，注意包含装卸载、移动作业
                            {
                                #region
                                CTask ectask = this.GetCTaskFromtskID(otherEtv.nIsWorking);
                                if (ectask != null)
                                {
                                    if (ectask.Type == CTask.EnmTaskType.TVLoad || ectask.Type == CTask.EnmTaskType.TVUnload)
                                    {
                                        if (otherCol > curMin && otherCol < curMax)
                                        {
                                            return;
                                        }
                                    }
                                    if (ectask.Type == CTask.EnmTaskType.TVMove)
                                    {
                                        int toColumn = GetColumnOfAddress(ectask.ToLctAdrs);
                                        //ETV位于当前在最大值外，但终点在当前范围左侧，则当前不允许下发
                                        if (otherCol > curMax && curMax > toColumn)
                                        {
                                            return;
                                        }
                                        //ETV位于当前在最小值外，但终点在当前范围右侧，则当前不允许下发
                                        if (curMin > otherCol && toColumn > curMin)
                                        {
                                            return;
                                        }
                                        //有点落到区域内时不允许下发
                                        if ((curMax > otherCol && curMin < otherCol) ||
                                            (curMax > toColumn && curMin < toColumn))
                                        {
                                            return;
                                        }
                                        //在范围内也不允许动作
                                        if (otherCol > curMin && otherCol < curMax)
                                        {
                                            return;
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion
                        int next = 0;
                        #region
                        foreach (CTask tsk in mtask.Tasks)
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad)
                            {
                                next = tsk.ID;
                                break;
                            }
                        }
                        #endregion
                        etv.nIsWorking = etsk.ID;
                        etv.NextTaskId = next;
                        etv.MTskID = mtask.ID;
                        CWData.myDB.UpdateSMGTaskStat(etv);
                    }
                    #endregion
                }
                else  //已经完成装载作业，等待移动卸载中
                {
                    if (otherEtv.nIsWorking == 0)
                    {
                        #region 另一台车要进行装卸载时，先让其执行
                        if (otherEtv.MTskID != 0)
                        {
                             CMasterTask otherMaster = this.GetMasterTaskFromID(otherEtv.MTskID);
                             if (otherMaster != null)
                             {
                                 CTask nextTask = null;
                                 foreach (CTask ctsk in otherMaster.Tasks)
                                 {
                                     if (ctsk.SMGType == CSMG.EnmSMGType.ETV)
                                     {
                                         nextTask = ctsk;
                                         break;
                                     }
                                 }
                                 if (nextTask != null)
                                 {
                                     if (nextTask.Type == CTask.EnmTaskType.TVLoad ||
                                        nextTask.Type == CTask.EnmTaskType.TVUnload)
                                     {
                                         return;
                                     }
                                 }
                             }
                        }
                        #endregion
                        #region
                        if (otherCol > curMin && otherCol < curMax)
                        {
                            #region 生成避让作业
                            string oLayer = otherAddrs.Substring(3);
                            string oList = "";
                            if (curEtvCol > curToCol)     //需向左避让
                            {
                                oList = curMin.ToString().PadLeft(2, '0');
                            }
                            else
                            {
                                oList = curMax.ToString().PadLeft(2, '0');
                            }

                            lock (Tasks)
                            {
                                string lasttToAddress = String.Concat("1", oList, oLayer);
                                CTask avoidTask = new CTask(otherAddrs, lasttToAddress, CTask.EnmTaskType.TVAvoid, CTask.EnmTaskStatus.TWaitforMove,
                                    mtask.ICCardCode, otherEtv.ID, CSMG.EnmSMGType.ETV, 0, mtask.HID);
                                avoidTask.MID = mtask.ID;
                                CWData.myDB.InsertCTask(avoidTask);
                                mtask.AddTask(avoidTask);
                                //绑定于设备上,注意只绑定避让的子作业号，主作业号不处理
                                otherEtv.nIsWorking = avoidTask.ID;
                                CWData.myDB.UpdateSMGTaskStat(otherEtv);
                            }
                            #endregion
                        }
                        int next = 0;
                        #region
                        foreach (CTask tsk in mtask.Tasks)
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad)
                            {
                                next = tsk.ID;
                                break;
                            }
                        }
                        #endregion
                        etv.nIsWorking = etsk.ID;
                        etv.NextTaskId = next;
                        etv.MTskID = mtask.ID;
                        CWData.myDB.UpdateSMGTaskStat(etv);
                        #endregion
                    }
                    else
                    {
                        #region
                        CTask ectask = this.GetCTaskFromtskID(otherEtv.nIsWorking);
                        if (ectask != null)
                        {
                            if (ectask.Type == CTask.EnmTaskType.TVLoad || ectask.Type == CTask.EnmTaskType.TVUnload)
                            {
                                if (otherCol > curMin && otherCol < curMax)
                                {
                                    return;
                                }
                            }
                            if (ectask.Type == CTask.EnmTaskType.TVMove)
                            {
                                int toColumn = GetColumnOfAddress(ectask.ToLctAdrs);
                                //ETV位于当前在最大值外，但终点在当前范围左侧，则当前不允许下发
                                if (curMax < otherCol)
                                {
                                    if (toColumn < curMax)
                                    {
                                        return;
                                    }
                                }
                                //ETV位于当前在最小值外，但终点在当前范围右侧，则当前不允许下发
                                if (curMin > otherCol)
                                {
                                    if (toColumn > curMin)
                                    {
                                        return;
                                    }
                                }
                                //有点落到区域内时不允许下发
                                if ((curMax > otherCol && curMin < otherCol) ||
                                    (curMax > toColumn && curMin < toColumn))
                                {
                                    return;
                                }

                                if (otherCol > curMin && otherCol < curMax)
                                {
                                    return;
                                }
                            }
                        }
                        int next = 0;
                        #region
                        foreach (CTask tsk in mtask.Tasks)
                        {
                            if (tsk.Type == CTask.EnmTaskType.TVLoad)
                            {
                                next = tsk.ID;
                                break;
                            }
                        }
                        #endregion
                        etv.nIsWorking = etsk.ID;
                        etv.NextTaskId = next;
                        etv.MTskID = mtask.ID;
                        CWData.myDB.UpdateSMGTaskStat(etv);
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        /// <summary>
        /// 依地址获取列值
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetColumnOfAddress(string address) 
        {
            return Convert.ToInt32(address.Substring(1, 2));
        }

        #region
        /// <summary>
        /// 主作业类型解析
        /// </summary>
        /// <param name="mType"></param>
        /// <returns></returns>
        private string MtskTypeFormat(CMasterTask.EnmMasterTaskType mType)
        {
            string s = "";
            switch (mType)
            {
                case CMasterTask.EnmMasterTaskType.GetCar:
                    s = "取车";
                    break;
                case CMasterTask.EnmMasterTaskType.Move:
                    s = "移动";
                    break;
                case CMasterTask.EnmMasterTaskType.SaveCar:
                    s = "存车";
                    break;
                case CMasterTask.EnmMasterTaskType.TempGetCar:
                    s = "取物";
                    break;
                case CMasterTask.EnmMasterTaskType.Transpose:
                    s = "搬移";
                    break;
                default:
                    s = mType.ToString();
                    break;
            }
            return s;
        }
        #endregion

    }
}

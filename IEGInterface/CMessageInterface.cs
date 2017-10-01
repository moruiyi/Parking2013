using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGInterface.localhost;

namespace IEGInterface
{
    public class CMessageInterface
    {
        private COpcServerCOM OpcServer=null;        
        private static Int16 messageID = 0;
        private static bool isSend19Asked = false;

        public CMessageInterface() 
        {
        }

        /// <summary>
        /// 建立OPC连接
        /// </summary>
        public void ConnectPLC() 
        {
            if (OpcServer == null) 
            {
                OpcServer = new COpcServerCOM();
            }
           
            OpcServer.mOPC_ConnToServer();      //建立连接
            if (OpcServer.CheckOpcConn)
            {
                OpcServer.CreateSubscription();  //建立订阅
            }           
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisconnectPLC()
        {
            OpcServer.mOpc_DisConn();
        }

        /// <summary>      
        /// 车厅作业下发的前提是-车厅可接受新指令。
        /// ETV提前装载-如果前面作业是取车作业时，就允许下发。
        /// </summary>
        public void TaskAssign() 
        {
            try
            {
                #region
                //检查作业绑定于设备，但作业已不在主作业中的，则此时强制释放设备
                Program.mng.UpdateSMGsWorkStat();
                //获取所有可接收新指令的车厅集合
                int[] hallSpaceList = Program.mng.GetCarHallsSpace();
                if (hallSpaceList == null) 
                {
                    hallSpaceList = new int[1] { 0 };
                }
                #region 将执行队列的作业分配给空闲的设备,保证执行队列的作业优先得到分配,即如果ETV空闲时就一定要被分配下去
                for (int i = 0; i < Program.TaskList.Count; i++)
                {
                    CMasterTask mtsk = Program.mng.GetMasterTaskFromID(Program.TaskList[i]);
                    if (mtsk == null)
                    {
                        CWSException.WriteLog("删除主作业，作业ID:" + Program.TaskList[i], 4);
                        Program.TaskList.Remove(Program.TaskList[i]);
                        i--;
                        continue;
                    }

                    foreach (CTask tsk in mtsk.Tasks)
                    {
                        CSMG smg = Program.mng.SelectSMG(tsk.SMG);
                        if (smg.nIsWorking == 0)
                        {
                            if (smg.SMGType == EnmSMGType.Hall)
                            {
                                smg.nIsWorking = tsk.ID;
                                smg.MTskID = 0;
                                Program.mng.UpdateWorkingStatOfSMG(smg);
                            }
                            else if (smg.SMGType == EnmSMGType.ETV)
                            {
                                if (smg.MTskID == mtsk.ID || smg.MTskID == 0)
                                {
                                    if (tsk.Type == EnmTaskType.TVMove) //先让移动优先完成
                                    {
                                        //判断是否可以继续移动，如果是，是否需要生成避让
                                        Program.mng.DealEtvAvoid(tsk.ID);
                                        break;
                                    }
                                    else //装卸载时一定要进行的
                                    {
                                        smg.nIsWorking = tsk.ID;
                                        smg.MTskID = mtsk.ID;
                                        Program.mng.UpdateWorkingStatOfSMG(smg);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                //获取所有的主作业
                CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(0);  //获取所有主作业  
                if (mtsks == null)
                {
                    return;
                }
                for (int i = 0; i < mtsks.Length; i++)
                {
                    CMasterTask mtsk = mtsks[i];
                    if (!Program.TaskList.Exists(m => m == mtsk.ID))
                    {
                        CSMG hall = null;
                        CSMG etv = null;
                        int hid = 0;
                        int eid = 0;
                        #region 判断设备是否空闲,如果作业已被分配到设备,则将作业加入加入执行队列,否则判断
                        foreach (CTask tsk in mtsk.Tasks)
                        {
                            CSMG smg = Program.mng.SelectSMG(tsk.SMG);
                            if (!smg.Available)
                            {
                                break;
                            }
                            if (smg.nIsWorking == tsk.ID) //如果当前作业已实行（存车/转存时），则将任务直接加入
                            {
                                Program.TaskList.Add(mtsk.ID);
                                break;
                            }
                            //设备空闲时
                            if (smg.nIsWorking == 0 && smg.MTskID == 0)
                            {
                                if (smg.SMGType == EnmSMGType.Hall && hall == null && tsk.Status != EnmTaskStatus.Finished)
                                {
                                    hall = smg;
                                    hid = tsk.ID;
                                }
                                else if (smg.SMGType == EnmSMGType.ETV && etv == null && tsk.Status != EnmTaskStatus.Finished)
                                {
                                    etv = smg;
                                    eid = tsk.ID;  //一定是移动作业的ID的
                                }
                            }
                        }
                        #endregion
                        #region 设备空闲了，判断是否允许实行
                        if (hall != null && etv == null)  //仅车厅需作业或TV处于忙的时候
                        {
                            Program.TaskList.Add(mtsk.ID);
                            hall.nIsWorking = hid;
                            hall.MTskID = 0;
                            Program.mng.UpdateWorkingStatOfSMG(hall);
                        }
                        else if (hall == null && etv != null) //车厅在进行作业或没有车厅作业，
                        {
                            //判断当前ETV的移动作业有没有被堵住，如果堵住，则不允许下发的
                            if (Program.mng.CheckMovePathIsBlock(eid) == false) 
                            {
                                return;
                            }
                                                       
                            if (mtsk.Type == EnmMasterTaskType.Move)
                            {
                                Program.TaskList.Add(mtsk.ID);
                                etv.MTskID = 0;
                                etv.nIsWorking = eid;
                                etv.NextTaskId = 0;
                                Program.mng.UpdateWorkingStatOfSMG(etv);
                            }
                            else if (mtsk.Type == EnmMasterTaskType.Transpose) 
                            {
                                Program.TaskList.Add(mtsk.ID);
                                int next = 0;
                                #region
                                foreach (CTask tsk in mtsk.Tasks) 
                                {
                                    if (tsk.Type == EnmTaskType.TVLoad) 
                                    {
                                        next = tsk.ID;
                                        break;
                                    }
                                }
                                #endregion
                                etv.MTskID = mtsk.ID;
                                etv.nIsWorking = eid;
                                etv.NextTaskId = next;
                                Program.mng.UpdateWorkingStatOfSMG(etv);
                            }
                            else
                            {
                                bool allow = true;
                                #region 判断是否允许去装载
                                if (mtsk.Tasks.Length > 1)
                                {
                                    CSMG hallEqp = Program.mng.SelectSMG(mtsk.HID);
                                    #region 
                                    //如果车厅有作业，则判断作业是否为存车或取物,如果是，则TV作业先不下发,
                                    //同时如果当前车厅的ETV作业还未得到过执行的，也不允许ETV作业下发
                                    if (hallEqp != null)
                                    {
                                        if (hallEqp.nIsWorking != 0)
                                        {
                                            CMasterTask cmtsk = null;
                                            foreach (CMasterTask cmtask in mtsks)
                                            {
                                                foreach (CTask ctsk in cmtask.Tasks)
                                                {
                                                    if (ctsk.ID == hallEqp.nIsWorking)
                                                    {
                                                        cmtsk = cmtask;  //找出车厅当前进行的作业
                                                    }
                                                }
                                                if (cmtsk != null)
                                                {
                                                    break;
                                                }
                                            }
                                            if (cmtsk != null)
                                            {
                                                if (cmtsk.Type == EnmMasterTaskType.SaveCar || cmtsk.Type == EnmMasterTaskType.TempGetCar)
                                                {
                                                    allow = false;
                                                }
                                                //如果当前正在作业的车厅还有ETV作业时，即由于路径被堵，其ETV作业没有获得执行过的，这时也不允许提前装载
                                                foreach (CTask etask in cmtsk.Tasks) 
                                                {
                                                    if (etask.SMGType == EnmSMGType.ETV) 
                                                    {
                                                        allow = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                                if (allow)  //车厅允许TV动作
                                {
                                    Program.TaskList.Add(mtsk.ID);
                                    int next = 0;
                                    #region
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (tsk.Type == EnmTaskType.TVLoad)
                                        {
                                            next = tsk.ID;
                                            break;
                                        }
                                    }
                                    #endregion
                                    etv.MTskID = mtsk.ID;
                                    etv.nIsWorking = eid;
                                    etv.NextTaskId = next;
                                    Program.mng.UpdateWorkingStatOfSMG(etv);
                                }
                            }                           
                        }
                        else if (hall != null && etv != null)  //车厅、TV都空闲
                        {
                            if (Array.Exists(hallSpaceList, delegate(int hh) { return hh == hall.ID; }))
                            {
                                Program.TaskList.Add(mtsk.ID);
                                hall.nIsWorking = hid;
                                hall.MTskID = 0;
                                Program.mng.UpdateWorkingStatOfSMG(hall);
                                //etv能不能去执行移动呢，如果不能则先不下发，
                                if (Program.mng.CheckMovePathIsBlock(eid)) 
                                {                                   
                                    int next = 0;
                                    #region
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (tsk.Type == EnmTaskType.TVLoad)
                                        {
                                            next = tsk.ID;
                                            break;
                                        }
                                    }
                                    #endregion
                                    etv.MTskID = mtsk.ID;
                                    etv.nIsWorking = eid;
                                    etv.NextTaskId = next;
                                    Program.mng.UpdateWorkingStatOfSMG(etv);
                                }
                            }
                            else
                            {
                                CWSException.WriteLog("作业下发前 " + (hall.ID - 10).ToString() + "#车厅不可接收新指令", 4);
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex) 
            {
                CWSException.WriteError("TaskAssign发生异常：" + ex.ToString());
            }
        } 

        /// <summary>
        /// 发送信息
        /// </summary>
        public void SendMessage() 
        {
            try
            {
                #region
                foreach (int tid in Program.TaskList)
                {
                    CMasterTask mtsk = Program.mng.GetMasterTaskFromID(tid);
                    if (mtsk != null) 
                    {
                        foreach (CTask tsk in mtsk.Tasks)
                        {
                            CSMG smg = Program.mng.SelectSMG(tsk.SMG);
                            #region 车厅
                            if (smg.SMGType == EnmSMGType.Hall)
                            {
                                if (smg.Available && smg.nIsWorking == tsk.ID&&(tsk.StatusDetail==EnmTaskStatusDetail.NoSend||
                                    (tsk.StatusDetail==EnmTaskStatusDetail.SendWaitAsk&&DateTime.Now>tsk.SendDtime.AddSeconds(10))))
                                {
                                    if (tsk.Status == EnmTaskStatus.ISecondSwipedWaitforCheckSize)
                                    {                                      
                                        bool isOK = this.sendData(this.packageMessage(1, 9, smg.ID, tsk)); //发送(1,9)
                                        if (isOK) 
                                        {
                                            //修改作业状态
                                            Program.mng.SetTskStatusDetail(tsk.ID,EnmTaskStatusDetail.SendWaitAsk,DateTime.Now);
                                            isSend19Asked = false;
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.ISecondSwipedWaitforEVDown) 
                                    {
                                        bool isSend = this.sendData(packageMessage(1,1,smg.ID,tsk));   //发送入库(1,1)
                                        if (isSend) 
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID,EnmTaskStatusDetail.SendWaitAsk,DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.IEVDownFinishing)
                                    {
                                        bool isSend = this.sendData(packageMessage(1, 54, smg.ID, tsk));  //发送(1,54)
                                        if (isSend) 
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.HallFinishing)
                                    {
                                        bool isSend = this.sendData(packageMessage(3, 54, smg.ID, tsk));  //发送(3,54)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.OWaitforEVDown)     //出库开始
                                    {
                                        if (Program.mng.CheckSMGAcceptCommad(tsk.SMG))
                                        {
                                            bool isSend = this.sendData(packageMessage(1, 1, smg.ID, tsk));  //发送出库(1,1)
                                            if (isSend)
                                            {
                                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                            }
                                        }
                                        else 
                                        {
                                            CWSException.WriteLog(tsk.SMG+"  不可接收新指令",4);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.OEVDownFinishing)
                                    {
                                        bool isSend = this.sendData(packageMessage(1, 54, smg.ID, tsk));  //发送出库(1,54)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);                                            
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.OCarOutWaitforDriveaway||tsk.Status==EnmTaskStatus.ISecondSwipedWaitforCarLeave)
                                    {
                                        bool isSend = this.sendData(packageMessage(3, 1, smg.ID, tsk));  //发送出库(3,1)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.TempOWaitforEVDown) 
                                    { 
                                        bool isSend = this.sendData(packageMessage(2, 1, smg.ID, tsk));   //取物发送(2,1)
                                        if (isSend) 
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.TempOEVDownFinishing)         
                                    {
                                        bool isSend = this.sendData(packageMessage(2, 54, smg.ID, tsk));    //取物确认出车（2，54）
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);  
                                        }
                                    }
                                }
                                else if (smg.Available && smg.nIsWorking == tsk.ID && isSend19Asked &&
                                    tsk.Status == EnmTaskStatus.ISecondSwipedWaitforCheckSize)             //车辆跑位
                                {
                                    if (Program.mng.CheckCarOffTracing(smg.ID))
                                    {
                                        Program.mng.DealCarOffTracing(tsk.ID);     //处理跑位
                                        isSend19Asked = false;

                                        CWSException.WriteLog((tsk.SMG%10).ToString() + "#车厅出现车辆跑位", 4);
                                    }
                                }
                            }
                            #endregion

                            #region ETV
                            if (smg.SMGType == EnmSMGType.ETV) 
                            {
                                if (smg.Available && smg.nIsWorking == tsk.ID && (tsk.StatusDetail == EnmTaskStatusDetail.NoSend ||
                                    (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk && DateTime.Now > tsk.SendDtime.AddSeconds(10)))) 
                                {
                                    if (tsk.Status == EnmTaskStatus.TWaitforLoad) 
                                    {
                                        if (Program.mng.CheckSMGAcceptCommad(tsk.SMG))
                                        {
                                            if (smg.Available)
                                            {
                                                bool isSend = this.sendData(packageMessage(13, 1, smg.ID, tsk));  //发送出库(13,1)
                                                if (isSend)
                                                {
                                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                                }
                                            }
                                            else 
                                            {
                                                CWSException.WriteLog(tsk.SMG + "  状态不可用", 4);
                                            }
                                        }
                                        else 
                                        {
                                            CWSException.WriteLog(tsk.SMG + "  不可接收新指令", 4);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.LoadFinishing) 
                                    {
                                        bool isSend = this.sendData(packageMessage(13, 51, smg.ID, tsk));  //发送出库(13,51)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.TWaitforUnload)
                                    {
                                        if (Program.mng.CheckSMGAcceptCommad(tsk.SMG))
                                        {
                                            if (smg.Available)
                                            {
                                                bool isSend = this.sendData(packageMessage(14, 1, smg.ID, tsk));  //发送出库(14,1)
                                                if (isSend)
                                                {
                                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                                }
                                            }
                                            else
                                            {
                                                CWSException.WriteLog(tsk.SMG + "  状态不可用", 4);
                                            }
                                        }
                                        else
                                        {
                                            CWSException.WriteLog(tsk.SMG + "  不可接收新指令", 4);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.UnLoadFinishing)
                                    {
                                        bool isSend = this.sendData(packageMessage(14, 51, smg.ID, tsk));  //发送出库(14,51)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }                                   
                                    else if (tsk.Status == EnmTaskStatus.TWaitforMove)
                                    {
                                        if (Program.mng.CheckSMGAcceptCommad(tsk.SMG))
                                        {
                                            if (smg.Available)
                                            {
                                                bool isSend = this.sendData(packageMessage(11, 1, smg.ID, tsk));  //发送出库(11,1)
                                                if (isSend)
                                                {
                                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                                }
                                            }
                                            else
                                            {
                                                CWSException.WriteLog(tsk.SMG + "  状态不可用", 4);
                                            }
                                        }
                                        else
                                        {
                                            CWSException.WriteLog(tsk.SMG + "  不可接收新指令", 4);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.MoveFinishing) 
                                    {
                                        bool isSend = this.sendData(packageMessage(11, 51, smg.ID, tsk));  //发送出库(11,51)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                    else if (tsk.Status == EnmTaskStatus.TMURO) 
                                    {
                                        bool isSend = this.sendData(packageMessage(74, 1, smg.ID, tsk));  //发送故障确认(74,1)
                                        if (isSend)
                                        {
                                            Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.SendWaitAsk, DateTime.Now);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex) 
            {
                CWSException.WriteError("SendMessage发生异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        public void ReceiveMessage() 
        {
            try
            {
                #region
                Int16[] data;
                int isOK;
                unpackMessage(out isOK,out data);  //读取接收缓冲区
                
                if (isOK == 0) 
                {
                    //没有收到数据
                    return;
                }

                CSMG smg = Program.mng.SelectSMG(data[6]);
                if (smg == null) 
                {
                    return;
                }
                CTask tsk = Program.mng.GetCTaskFromID(smg.nIsWorking); //获取设备正在实行的作业

                if (tsk != null)                 
                {
                    #region   Hall
                    if (smg.SMGType == EnmSMGType.Hall)
                    {
                        if (tsk.Status == EnmTaskStatus.ISecondSwipedWaitforCheckSize)
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 1 && data[3] == 9 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                    isSend19Asked = true;
                                }
                            }
                            if (data[2] == 1001 && data[4] == 101) 
                            { 
                                //解析外形、分配车位及ETV、修改作业状态
                                Program.mng.DealCheckedCar(tsk.ID,data[6],data[23].ToString(),data[25]);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID,EnmTaskStatusDetail.NoSend,DateTime.Now);
                            }
                            else if (data[2] == 1001 && data[4] == 104)   //外形尺寸超限
                            {
                                Program.mng.DealCheckCarFail(tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                       else  if (tsk.Status == EnmTaskStatus.ISecondSwipedWaitforEVDown) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk) 
                            {
                                if (data[2] == 1 && data[3] == 1 && data[4] == 9999) 
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID,EnmTaskStatusDetail.Asked,tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1001 && data[4] == 54)
                            {
                                //修改作业状态为IEVDownFinishing
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.IEVDownFinishing,tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.IEVDownFinishing) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk) 
                            {
                                if (data[2] == 1 && data[3] == 54 && data[4] == 9999)
                                {                                   
                                    //修改作业状态Detail
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                    //修改作业状态为IEVDownFinished--可以忽略
                                    Program.mng.DealUpdateTaskStatus(EnmTaskStatus.IEVDownFinished, tsk.ID);
                                }
                            }
                        }
                        //未刷卡或刷一次卡就走,或外形检测失败后退出
                       else if (tsk.Status == EnmTaskStatus.ICarInWaitFirstSwipeCard || tsk.Status == EnmTaskStatus.IFirstSwipedWaitforCheckSize||
                            tsk.Status==EnmTaskStatus.ICheckCarFail)
                        {
                            if (data[2] == 1003 && data[4] == 4) 
                            {
                                //修改作业状态为HallFinishing
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.HallFinishing,tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        //车辆完全离开
                       else if (tsk.Status == EnmTaskStatus.HallFinishing) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 3 && data[3] == 54 && data[4] == 9999)
                                {
                                    //修改作业状态Detail
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                    //更新作业状态为Finishing,如果主作业是入库时，清除其在数据库中记录,出库时，则完成
                                    Program.mng.DealCarLeave(smg.ID, tsk.ID);
                                }
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.OWaitforEVDown)
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 1 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1001 & data[4] == 54)
                            {
                                //修改作业状态为OEVDownFinishing
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.OEVDownFinishing, tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.OEVDownFinishing)    //出库时车厅下降完成，不再修改车厅状态
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 1 && data[3] == 54 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }                           
                        }
                        else if (tsk.Status == EnmTaskStatus.ISecondSwipedWaitforCarLeave)   //车辆离开车厅
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 3 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1003 && data[4] == 4)
                            {
                                Program.mng.DealCarDriveLeave(tsk.ID);   //修改车厅作业状态
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.OWaitforEVUp) 
                        {
                            if (data[2] == 1003 && data[4] == 1)
                            {                              
                                //修改作业状态
                                Program.mng.ODealEVUp(tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.OCarOutWaitforDriveaway) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 3 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1003 && data[4] == 4) 
                            {
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.HallFinishing, tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }                       
                        else if (tsk.Status == EnmTaskStatus.TempOWaitforEVDown)   //取物
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 2 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1002 && data[4] == 54)
                            {
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.TempOEVDownFinishing, tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.TempOEVDownFinishing)    //待装载完成时，才修改车厅的作业状态
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 2 && data[3] == 54 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                        }
                    }
                    #endregion
                    #region ETV
                    else if (smg.SMGType == EnmSMGType.ETV) 
                    {
                        if (tsk.Status == EnmTaskStatus.TWaitforLoad) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk) 
                            {
                                if (data[2] == 13 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1013 && data[4] == 1) 
                            {
                                //装载完成，修改作业状态LoadFinishing，完成车厅作业
                                Program.mng.DealTVLoadFinishing(tsk.ID,data[25]);
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.LoadFinishing) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 13 && data[3] == 51 && data[4] == 9999) 
                                {
                                    //处理装载完成                                    
                                    Program.mng.DealCompleteTVLoadFinish(tsk.ID);
                                    //修改作业状态Detail
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                                }
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.TWaitforUnload) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 14 && data[3] == 1 && data[4] == 9999)
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1014 && data[4] == 1)
                            {
                                //修改作业状态unLoadFinishing，完成车厅作业
                                Program.mng.DealTVUnLoadFinishing(tsk.ID,data[25]);
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.UnLoadFinishing) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 14 && data[3] == 51 && data[4] == 9999)
                                {                                   
                                    //ETV作业完成处理
                                    Program.mng.DealCompleteCTask(tsk.ID);
                                }
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.TWaitforMove) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk) 
                            {
                                if (data[2] == 11 && data[3] == 1 & data[4] == 9999) 
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                            if (data[2] == 1011 && data[4] == 1) 
                            {
                                Program.mng.DealTvMoveFinishing(tsk.ID);
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.MoveFinishing) 
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk)
                            {
                                if (data[2] == 11 && data[3] == 51 & data[4] == 9999)
                                {                                    
                                    //处理移动完成
                                    Program.mng.DealCompleteTVMoveFinish(tsk.ID);                                    
                                }
                            }
                        }
                        else if (tsk.Status == EnmTaskStatus.TMURO) //故障确认
                        {
                            if (tsk.StatusDetail == EnmTaskStatusDetail.SendWaitAsk) 
                            {
                                if (data[2] == 74 && data[3] == 1 & data[4] == 9999) 
                                {
                                    Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.Asked, tsk.SendDtime);
                                }
                            }
                        }

                        //故障时
                        if (data[2] == 1074 && data[4] == 7)
                        {
                            if (data[18] == 0)
                            {
                                //调用改变作业状态为故障
                                Program.mng.UpdateSMGStatus(data[6], false, EnmModel.AutoStopped);
                                Program.mng.DealUpdateTaskStatus(EnmTaskStatus.TMURO, tsk.ID);
                                //修改作业状态Detail
                                Program.mng.SetTskStatusDetail(tsk.ID, EnmTaskStatusDetail.NoSend, DateTime.Now);
                            }
                        }

                    }
                    #endregion
                }               
                #region 车辆第一次入库
                else 
                {
                    if (data[2] == 1001 && data[4] == 1) 
                    {
                        //处理第一次入库
                        Program.mng.DealCarEntrance(data[6]);
                    }
                    else if (data[2] == 1074 && data[4] == 7) 
                    {                        
                        if (data[18] == 0) 
                        {
                            //更新设备为不可用
                            Program.mng.UpdateSMGStatus(data[6],false,EnmModel.AutoStopped);
                            this.sendData(this.packageMessage(74, 1, data[6], null));
                        }
                        else if (data[18] == 2)    //设备恢复后,发送(23,1) ----新协议中没有的,
                        {
                            Program.mng.UpdateSMGStatus(data[6],true,EnmModel.AutoStarted);
                            this.sendData(this.packageMessage(23,1,data[6],null));
                        }                        
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex) 
            {
                CWSException.WriteError("ReceiveMessage发生异常："+ex.ToString());
            }

        }

        /// <summary>
        /// 更新报警及状态信息-每次都更新
        /// </summary>
        public void DealErrAlarmAndSMGStat() 
        {
            try 
            {                
                List<short> statsValue = new List<short>();
                int idex = 1;

                #region ETV1
                byte[] alarm1 = new byte[54];
                bool isOK = OpcServer.ReadnewBytes(ref alarm1, 4);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(alarm1,idex));   //获取值为1的位地址的集合
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 1);

                    statsValue.AddRange(buildStatusValues(alarm1));
                }
                #endregion
                #region ETV2
                idex = idex + (alarm1.Length - 24) * 8;     //修改索引
                byte[] alarm2=new byte[54];
                isOK = OpcServer.ReadnewBytes(ref alarm2, 5);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(alarm2, idex));
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 2);

                    statsValue.AddRange(buildStatusValues(alarm2));
                }
                #endregion
                #region Hall1
                idex = idex + (alarm1.Length - 24) * 8;     //修改索引
                byte[] hall1 = new byte[54];
                isOK = OpcServer.ReadnewBytes(ref hall1, 6);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(hall1, idex));
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 11);

                    statsValue.AddRange(buildStatusValues(hall1));
                }
                #endregion
                #region hall2
                idex = idex + (alarm1.Length - 24) * 8;     //修改索引
                byte[] hall2 = new byte[54];
                isOK = OpcServer.ReadnewBytes(ref hall2, 7);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(hall2, idex));
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 12);

                    statsValue.AddRange(buildStatusValues(hall2));
                }
                #endregion
                #region hall3
                idex = idex + (alarm1.Length - 24) * 8;     //修改索引
                byte[] hall3 = new byte[54];
                isOK = OpcServer.ReadnewBytes(ref hall3, 8);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(hall3, idex));
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 13);

                    statsValue.AddRange(buildStatusValues(hall3));
                }
                #endregion
                #region hall4
                idex = idex + (alarm1.Length - 24) * 8;     //修改索引
                byte[] hall4 = new byte[54];
                isOK = OpcServer.ReadnewBytes(ref hall4, 9);
                if (isOK)
                {
                    List<short> errsbList = new List<short>();
                    errsbList.AddRange(readBitValueOfIsTrue(hall4, idex));
                    Program.mng.UpdateErrorInfo(errsbList.ToArray(), 14);

                    statsValue.AddRange(buildStatusValues(hall4));
                }
                #endregion
                Program.mng.UpdateStatCodes(statsValue.ToArray());  //更新状态字
                Program.mng.UpdateEtvCurrAddress();
            }
            catch (Exception ex) 
            {
                CWSException.WriteError("函数DealErrAlarmAndSMGStat发生异常：" + ex.ToString());
            }
        }

        /// <summary>
        /// 更新车厅的设定状态,及回馈库内空余车位
        /// </summary>
        public void UpdateHallType()
        {
            try
            {
                #region 库内空余车位数量
                int spaceLct = Program.mng.GetSpaceLocations();
                if (spaceLct == 0)
                {
                    spaceLct = 999;
                }
                byte[] value = new byte[4];
                for (int i = value.Length - 1, n = spaceLct; i >= 0; i--)
                {
                    value[i] = (byte)(n & 0xFF);
                    n >>= 8;
                }
                #endregion

                #region 获取车厅设定状态 更改状态值
                byte[] data = new byte[1];
                CSMG[] halls = Program.mng.SelectSMGsOfType(EnmSMGType.Hall);
                foreach (CSMG smg in halls)
                {
                    int item = smg.ID - 5;
                    switch (smg.HallType)
                    {
                        case EnmHallType.Entance:
                            data[0] = 1;
                            break;
                        case EnmHallType.Exit:
                            data[0] = 2;
                            break;
                        case EnmHallType.EnterorExit:
                            data[0] = 3;
                            break;
                        default:
                            data[0] = 0;
                            break;
                    }
                    //更新状态字
                    byte[] alarm = new byte[54];
                    if (OpcServer.CheckOpcConn)
                    {
                        bool isOK = OpcServer.ReadnewBytes(ref alarm, item);
                        if (isOK)
                        {
                            alarm[30] = value[2];
                            alarm[31] = value[3];

                            alarm[34] = 0;
                            alarm[35] = data[0];
                            OpcServer.WritenewBytes(alarm, item);
                        }
                    }
                    else
                    {
                        this.ConnectPLC();
                    }
                }
                #endregion
            }
            catch (Exception ex) 
            {
                CWSException.WriteError("函数UpdateHallType发生异常： "+ex.ToString());
            }
        }

        //记录报警为1的位地址
        private List<short> readBitValueOfIsTrue(byte[] alarm,int offset) 
        {
            List<short> bitIdex = new List<short>();
            for (int i = 0; i < 30; i++)      //报警字节为30  
            {
                for (int j = 0; j < 8; j++)   //8位
                {
                    int temp = (int)Math.Pow(2,j);
                    if ((temp & alarm[i]) != 0) 
                    {
                        bitIdex.Add((short)(offset+i*8+j));  //位的索引集合
                    }
                }
            }
            return bitIdex;
        }

        //将状态字体现出来
        private List<short> buildStatusValues(byte[] alarm) 
        {
            List<short> data = new List<short>();
            byte[] temp=new byte[2];
            for (int i = 30; i < 54; i=i+2)   //12个状态字
            {
                temp[0] = alarm[i + 1];
                temp[1]=alarm[i];
                data.Add(BitConverter.ToInt16(temp,0));
            }
            return data;
        }

        //检查报警位是否发生变化
        private bool CheckErrorsHasChange(byte[] oldErrors,byte[] NowErrors) 
        {
            if (oldErrors != null)
            {
                for (int i = 0; i < 30; i++)  //30个字节报警变量
                {
                    if (oldErrors[i] != NowErrors[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //找出报警位发生变化的位地址的集合
        private List<short> readBValuesHasNew(byte[] origAlarm,byte[] alarm, int idex) 
        {
            List<short> data = new List<short>();
            for (int i = 0; i < 30; i++) 
            {
                for (int j = 0; j < 8; j++) 
                {
                    int temp = (int)Math.Pow(2,j);
                    if ((temp & (alarm[i] ^ origAlarm[i])) != 0)   //^ 异或
                    {
                        data.Add((short)(idex + i * 8 + j));
                    }
                }
            }
            return data;
        }

        //报文接收
        private void unpackMessage(out int mtype, out Int16[] data) 
        {
            mtype = 0;
            data=null;
            bool isOK = false;

            if (OpcServer.CheckOpcConn) 
            {
                Int16 recvFlag = 0;
                bool isExits = OpcServer.Read(ref recvFlag,3);   //读取接收缓冲区
                if (isExits && recvFlag == 9999) 
                {
                    bool hasDb = OpcServer.Readnew(ref data,2);  //读取报文
                    if (hasDb) 
                    {                   
                        Int16 temp = 0;
                        OpcServer.Write(temp,3);     //给接收缓冲区清零
                        isOK = true;
                    }
                }
                if (isOK) 
                {
                    mtype = 1;
                    CWSException.WriteLog(data,0);   //日志
                }                
            } 
            else 
            {
                this.ConnectPLC();
            }
        }
        
        //报文发送
        private bool sendData(Int16[] message) 
        {
            bool isOK = false;
            if (OpcServer.CheckOpcConn) 
            {
                Int16 sendFlag = 1;
                bool isData = OpcServer.Read(ref sendFlag,1);  //读取发送缓冲区标志位
                if (isData && sendFlag == 0) 
                {
                    bool temp = OpcServer.Writenew(message,0);   //发送数据                 
                    if (temp) 
                    {
                        sendFlag = 9999;
                        OpcServer.Write(sendFlag,1);     //更新发送缓冲区标志
                        isOK=true;
                    }
                }

                if (isOK) 
                {
                    CWSException.WriteLog(message,1);
                }
            } 
            else
            {
                this.ConnectPLC();
            }
            return isOK;
        }

        //报文数据打包
        private short[] packageMessage(int mtype,int stype,int smg,CTask tsk)
        {
            short[] data = new short[50];
            if (tsk != null)
            {
                data[2] = Convert.ToInt16(mtype);
                data[3] = Convert.ToInt16(stype);
                data[6] = Convert.ToInt16(smg);
                data[11] = Convert.ToInt16(tsk.ICCardCode == "" ? "0" : tsk.ICCardCode);
                data[23] = Convert.ToInt16(tsk.CarSize == "" ? "0" : tsk.CarSize);
                data[25] = Convert.ToInt16(tsk.Distance > 0 ? tsk.Distance : 0);
                if (tsk.FromLctAdrs != "")
                {
                    data[30] = Convert.ToInt16(tsk.FromLctAdrs.Substring(0, 1));
                    data[31] = Convert.ToInt16(tsk.FromLctAdrs.Substring(1, 2));
                    data[32] = Convert.ToInt16(tsk.FromLctAdrs.Substring(3, 1));
                }
                if (tsk.ToLctAdrs != "")
                {
                    data[35] = Convert.ToInt16(tsk.ToLctAdrs.Substring(0, 1));
                    data[36] = Convert.ToInt16(tsk.ToLctAdrs.Substring(1, 2));
                    data[37] = Convert.ToInt16(tsk.ToLctAdrs.Substring(3, 1));
                }
            }
            else 
            {
                data[2] = Convert.ToInt16(mtype);
                data[3] = Convert.ToInt16(stype);
                data[6] = Convert.ToInt16(smg);
            }
            data[0] = (short)1;
            data[48]=getSerial();
            data[49]=(short)9999;
            return data;
        }

        //获取报文ID
        private short getSerial() 
        {
            if (messageID < (short)4999)
            {
                messageID++;
            }
            else 
            {
                messageID = 1;
            }
            return messageID;
        }

        #region 故障信息记录
        private List<CErrorCode> oldErrors=new List<CErrorCode>();

        private List<CErrorCode> descriptions;
        public List<CErrorCode> Descriptions 
        {
            get 
            {                
                if (descriptions == null) 
                {
                    CErrorCode[] cerrcds = Program.mng.LoadErrorsDescp();
                    if (cerrcds == null)
                    {
                        return null;
                    }
                    else
                    {
                        descriptions = cerrcds.ToList();
                    }
                }
                return descriptions;
            }
        }
       
        /// <summary>
        /// 处理报警信息录入
        /// </summary>
        public void DealRecordErrLog() 
        {
            CErrorCode[] cerrcdes = Program.mng.SelectCurrErrCodes();
            if (cerrcdes == null) 
            {
                return;
            }
            List<CErrorCode> currErrors = cerrcdes.ToList();
            if (currErrors.Count > 0) 
            {
                if (oldErrors.Count == 0) //初始值
                {
                    foreach (CErrorCode err in currErrors)
                    {
                        oldErrors.Add(err);
                        WriteRecord(err);
                    }
                }
                else
                {
                    foreach (CErrorCode err in currErrors)
                    {
                        if (!oldErrors.Exists(e => e.StartBit == err.StartBit))
                        {
                            WriteRecord(err);
                        }
                    }

                    oldErrors.Clear();
                    foreach (CErrorCode err in currErrors)
                    {
                        oldErrors.Add(err);
                    }
                }
            }
        }

        //写记录
        private void WriteRecord(CErrorCode err) 
        {
            string desc = "";
            if (Descriptions == null) 
            {
                return;
            }
            foreach (CErrorCode code in Descriptions) 
            {
                if (err.StartBit == code.StartBit) 
                {
                    desc = code.Description;
                }
            }
            Program.mng.InsertErrorLog(err.Type, desc, DateTime.Now);
        }
        #endregion

    }
}

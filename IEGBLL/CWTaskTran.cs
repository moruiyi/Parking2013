using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL
{
    public class CWTaskTran
    {
        private CWTask motsk;
        private CSMG moHall;

        public CWTaskTran() 
        {
            motsk = new CWTask();
        }

        public CWTaskTran(int hallID) :this()
        {
            moHall = (new CWSMG()).SelectSMG(hallID);
        }

        /// <summary>
        /// 刷卡取车处理
        /// </summary>
        /// <param name="physicCard">物理卡号</param>
        public void DealCardMessage(string physicCard) 
        {
            //检查车厅模式是否全自动
            if (!(new CWSMG()).CheckHallMode(moHall.ID)) 
            {
                motsk.AddNotification(moHall.ID,"5.wav");
                return;
            }
            CICCard iccd = new CWICCard().SelectByPhysicCard(physicCard);
            if (iccd == null) 
            {
                //不是本系统用卡
                motsk.AddNotification(moHall.ID, "6.wav");
                return;
            }
            if (iccd.Status == CICCard.EnmICCardStatus.Lost || iccd.Status == CICCard.EnmICCardStatus.Disposed) 
            {
                //卡已注销或挂失
                motsk.AddNotification(moHall.ID, "7.wav");
                return;
            }
            //判断该卡在其他车厅有没有进行作业
            if ((new CWTask()).CheckSameMTaskInHallFromICCd(iccd.Code, moHall.ID)) 
            {
                motsk.AddNotification(moHall.ID,"8.wav");
                return;
            }
            CLocation lct = new CWLocation().SelectLctFromICCode(iccd.Code);
            //获取该车厅的取车数量
            int getCarCount = motsk.GetMTskCountFromHall(moHall.ID,CMasterTask.EnmMasterTaskType.GetCar);

            #region 进车厅
            if (moHall.HallType == CSMG.EnmHallType.Entance) 
            {
                if (lct == null) 
                {
                    if (moHall.nIsWorking != 0)
                    {
                        //获取车厅子作业
                        CTask tsk = motsk.GetCTaskFromtskID(moHall.nIsWorking);
                        if (tsk.Status == CTask.EnmTaskStatus.ICarInWaitFirstSwipeCard)   //等待第一次刷卡
                        {
                            //处理第一次刷卡
                            motsk.DealISwipedFirstCard(tsk.ID,iccd.Code);                           
                        }
                        else if (tsk.Status == CTask.EnmTaskStatus.IFirstSwipedWaitforCheckSize)  //等待第二次刷卡
                        {
                            //处理第二次刷卡
                            if (tsk.ICCardCode != iccd.Code)
                            {
                                motsk.AddNotification(tsk.HID, "20.wav");
                                return;
                            }
                            motsk.DealISwipedSecondCard(tsk.ID, iccd.Code);
                        }
                        else
                        {
                            //刷卡错误请重新刷卡 或该卡正在作业请稍后
                            motsk.AddNotification(moHall.ID, "9.wav");
                        }
                    }
                    else 
                    {
                        CMasterTask mtsk = motsk.GetMasterTaskFromICCode(iccd.Code);
                        if (mtsk != null)
                        {
                            //该卡正在作业,请稍后
                            motsk.AddNotification(moHall.ID,"9.wav");
                            return;
                        }
                       //车厅无车，不能存车
                        motsk.AddNotification(moHall.ID,"10.wav");
                    }
                } 
                else 
                {
                    //请到出车厅刷卡取车
                    motsk.AddNotification(moHall.ID,"11.wav");
                    return;
                }
            }
            #endregion

            #region 出车厅
            if (moHall.HallType == CSMG.EnmHallType.Exit) 
            {
                if (lct != null)
                {
                    CMasterTask mtsk = motsk.GetMasterTaskFromICCode(iccd.Code);
                    if (mtsk == null)
                    {
                        if (getCarCount > CWData.MaxGetCarCount)
                        {
                            //取车人数多，请稍后
                            motsk.AddNotification(moHall.ID, "12.wav");
                            return;
                        }
                        if (CWData.ChargeEnable)   //收费功能开启
                        {
                            if (iccd.Type == CICCard.EnmICCardType.Temp)  //临时卡
                            {
                                motsk.AddNotification(moHall.ID,"29.wav");
                                return;
                            }
                            else if (iccd.Type == CICCard.EnmICCardType.Fixed || iccd.Type == CICCard.EnmICCardType.FixedLocation) 
                            {
                                if (iccd.DueDtime > DateTime.Now)    //定期卡，使用期限到
                                {
                                    motsk.AddNotification(moHall.ID,"31.wav");
                                    return;
                                }
                            }
                        }
                        //建立刷卡取车作业
                        motsk.DealOswipedFirstCard(moHall.ID,iccd,lct);
                    }
                    else 
                    {
                        //正在出库请稍后
                        motsk.AddNotification(moHall.ID,"13.wav");
                    }
                }
                else 
                {
                    //该卡没有存车
                    motsk.AddNotification(moHall.ID,"14.wav");
                }
            }
            #endregion

            #region 进出车厅
            if (moHall.HallType == CSMG.EnmHallType.EnterorExit)
            {
                #region 存车
                if (lct == null) //应该是进车状态
                {
                    if (moHall.nIsWorking != 0)
                    {
                        //获取车厅子作业
                        CTask tsk = motsk.GetCTaskFromtskID(moHall.nIsWorking);
                        if (tsk.Status == CTask.EnmTaskStatus.ICarInWaitFirstSwipeCard)   //等待第一次刷卡
                        {
                            //处理第一次刷卡
                            motsk.DealISwipedFirstCard(tsk.ID,iccd.Code);                            
                        }
                        else  if (tsk.Status == CTask.EnmTaskStatus.IFirstSwipedWaitforCheckSize)  //等待第二次刷卡
                        {
                            //处理第二次刷卡                           
                            if (tsk.ICCardCode != iccd.Code)
                            {
                                motsk.AddNotification(tsk.HID, "20.wav");
                            }
                            else
                            {
                                motsk.DealISwipedSecondCard(tsk.ID, iccd.Code);
                            }                           
                        }
                        else if (tsk.Status == CTask.EnmTaskStatus.OCarOutWaitforDriveaway)    //处理取物后存车,第三次刷卡
                        {
                            motsk.DealISwipeThirdCard(tsk, iccd);
                        }
                        else
                        {
                            //刷卡错误请重新刷卡 或该卡正在作业请稍后
                            motsk.AddNotification(moHall.ID, "9.wav");
                        }
                    }
                    else
                    {
                        CMasterTask mtsk = motsk.GetMasterTaskFromICCode(iccd.Code);
                        if (mtsk != null)
                        {
                            //该卡正在作业,请稍后
                            motsk.AddNotification(moHall.ID, "9.wav");
                            return;
                        }
                        //车厅无车，不能存车
                        motsk.AddNotification(moHall.ID, "10.wav");
                    }
                }
                #endregion

                #region 取车
                else     //应该是刷卡取车
                {
                    CMasterTask mtsk = motsk.GetMasterTaskFromICCode(iccd.Code);
                    if (mtsk == null)
                    {
                        if (getCarCount > CWData.MaxGetCarCount)
                        {
                            //取车人数多，请稍后
                            motsk.AddNotification(moHall.ID, "12.wav");
                            return;
                        }
                        if (CWData.ChargeEnable)   //收费功能开启
                        {
                            if (iccd.Type == CICCard.EnmICCardType.Temp)  //临时卡
                            {
                                motsk.AddNotification(moHall.ID, "29.wav");
                                return;
                            }
                            else if (iccd.Type == CICCard.EnmICCardType.Fixed || iccd.Type == CICCard.EnmICCardType.FixedLocation)
                            {
                                if (iccd.DueDtime > DateTime.Now)    //定期卡，使用期限到
                                {
                                    motsk.AddNotification(moHall.ID, "31.wav");
                                    return;
                                }
                            }
                        }
                        //建立刷卡取车作业
                        motsk.DealOswipedFirstCard(moHall.ID, iccd, lct);
                    }
                    else
                    {
                        //正在出库请稍后
                        motsk.AddNotification(moHall.ID, "13.wav");
                    }
                }
                #endregion
            }
            #endregion 
        }

        /// <summary>
        /// 有车入库处理
        /// </summary>
        public void DealCarEntrance() 
        {
            //检查车厅模式
            if ((new CWSMG().CheckHallMode(moHall.ID)) == false)
            {
                motsk.AddNotification(moHall.ID, "5.wav");
                return;
            }
            if (moHall.nIsWorking == 0)
            {
                if (moHall.HallType == CSMG.EnmHallType.Exit)
                {
                    //出车厅不允许存车
                    motsk.AddNotification(moHall.ID, "4.wav");
                }
                else 
                {
                    //生成作业
                    int tskID = motsk.DealIFirstCarEntrance(moHall.ID);                   
                    moHall.nIsWorking = tskID;
                    moHall.NextTaskId = 0;
                    moHall.MTskID = 0;
                    //更新数据库
                    CWData.myDB.UpdateSMGTaskStat(moHall);
                }
            } 
            else
            {
                //车厅正在进行取车
                motsk.AddNotification(moHall.ID, "3.wav");
            }
        }

        /// <summary>
        /// 取车完成处理或存车时中途离开处理
        /// </summary>
        public void DealCarLeave(int tskID) 
        {
            CMasterTask mtsk;
            CTask tsk;
            int i = motsk.GetMTaskAndCTaskOfTid(tskID,out mtsk,out tsk);
            if (i == 1) 
            {
                if (mtsk.Type == CMasterTask.EnmMasterTaskType.SaveCar)
                {
                    motsk.ICancelInAndDelHallTsk(tsk);   //中途中断时就清除数据库记录
                }
                else if (mtsk.Type == CMasterTask.EnmMasterTaskType.GetCar||mtsk.Type==CMasterTask.EnmMasterTaskType.TempGetCar)
                {
                    motsk.ODealCarDriveaway(tsk.ID);   //出库时完成作业
                }
            }
        }

        /// <summary>
        /// 手动指令创建
        /// </summary>        
        public int CreateManageMasterTask(string frLct, string toLct, CMasterTask.EnmMasterTaskType mtskType,out int hallID) 
        {
            try
            {
                hallID = 0;
                //int count = new CWTask().GetAllMasterTaskCount();
                //if (count > 0)
                //{
                //    return 101;  //只有库内无作业时才能允许建立手动作业
                //}

                CWLocation wlct = new CWLocation();
                CLocation flct = wlct.SelectLctFromAddrs(frLct);
                CLocation tlct = wlct.SelectLctFromAddrs(toLct);
                if (flct == null || tlct == null)
                {
                    return 102;  //请输入正确的源地址及目的地址
                }

                if (mtskType == CMasterTask.EnmMasterTaskType.Move||mtskType==CMasterTask.EnmMasterTaskType.Transpose)
                {
                    return new CWTask().CreateManualTask(flct, tlct,null, mtskType);
                }
                else if (mtskType == CMasterTask.EnmMasterTaskType.GetCar)
                {
                    if (tlct.Type == CLocation.EnmLocationType.Hall)
                    {
                        CSMG hall = new CWSMG().SelectHallFromAddress(tlct.Address);
                        if (hall != null)
                        {
                            hallID = hall.ID - 10;
                            if (new CWSMG().CheckHallMode(hall.ID) == false)  //车厅为全自动
                            {
                                return 105;
                            }                            
                            if (hall.HallType == CSMG.EnmHallType.Exit || hall.HallType == CSMG.EnmHallType.EnterorExit)
                            {
                                return new CWTask().CreateManualTask(flct, tlct,hall, mtskType);
                            }
                            else
                            {
                                return 104; //所选车厅不是出车厅
                            }                           
                        }
                        else
                        {
                            return 103;
                        }
                    }
                    else
                    {
                        return 103;  //目的地必须是车厅
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return 0;
        }

        /// <summary>
        /// 临时取物,需等待其他的车厅作业全部完成才允许添加
        /// </summary>
        /// <param name="iccode"></param>
        /// <param name="hall_ID"></param>
        /// <returns></returns>
        public int TempGetCar(string iccode, int hall_ID) 
        {
            CICCard iccd = new CWICCard().SelectByUserCode(iccode);
            if (iccd == null) 
            {
                return 109;
            }
            if (iccd.Status == CICCard.EnmICCardStatus.Disposed || iccd.Status == CICCard.EnmICCardStatus.Lost) 
            {
                return 110;
            }
            CLocation lct = new CWLocation().SelectLctFromICCode(iccode);  //依存车卡号查询存车位
            if (lct == null) 
            {
                return 101;
            }         
            CSMG hall = new CWSMG().SelectSMG(hall_ID);
            if (hall.HallType != CSMG.EnmHallType.EnterorExit) //车厅类型
            {
                return 102;
            }
            if (!new CWSMG().CheckHallMode(hall.ID))    //判断车厅模式
            {
                return 103;
            }
            if (!new CWSMG().CheckAcceptNewCommand(hall.ID))  //车厅是否可接收新指令
            {
                return 104;
            }
            CMasterTask mtsk = new CWTask().GetMasterTaskFromICCode(iccode);
            if (mtsk != null) 
            {
                return 105;    //该卡正在作业
            }
            int GetCarCount = new CWTask().GetMasterTaskNumOfHid(hall.ID, CMasterTask.EnmMasterTaskType.GetCar);
            if (GetCarCount >0) 
            {
                return 106;   //需待其他车厅的取车作业完成才允许添加
            }
           
            int rit = new CWTask().OTemp_GetCar(hall.ID, hall.Address, iccd, lct);
            return rit;
        }

        /// <summary>
        /// 处理外形检测
        /// </summary>
        /// <param name="tskID">子作业ID</param>
        /// <param name="distance">轴距</param>
        /// <param name="carSize">车辆尺寸-取物时为无效</param>
        /// <returns></returns>
        public int DealCheckCar(int tskID, int distance, string carSize) 
        {

            CMasterTask mtsk = null;
            CTask htsk = null;
            motsk.GetMTaskAndCTaskOfTid(tskID,out mtsk,out htsk);
            if (mtsk == null || htsk == null) 
            {
                return 101;
            }
            if (mtsk.IsTemp == true)  //取物后存车流程
            {
                CLocation toLct = new CWLocation().SelectLctFromAddrs(htsk.ToLctAdrs);
                if (toLct.Status != CLocation.EnmLocationStatus.Temping || toLct.ICCardCode != "") //车位上有车
                {
                    motsk.IDealCheckedCar(htsk.ID, htsk.CarSize, distance);   //依就近原则分配
                }
                else
                {
                    motsk.ITempDealCheckCar(htsk.ID,mtsk.ID, distance, htsk.CarSize, toLct.Address);
                }
            }
            else 
            {
                motsk.IDealCheckedCar(htsk.ID, carSize, distance);                
            }
            return 100;
        }

        /// <summary>
        /// 临时卡取车-界面收费出车
        /// </summary>
        /// <param name="hallID">车厅号</param>
        /// <returns></returns>
        public int OCreateTempICcardGetCar(string iccode, out int hallID)
        {
            hallID = 0;
            CICCard iccd = new CWICCard().SelectByUserCode(iccode);
            if (iccd.Status == CICCard.EnmICCardStatus.Lost || iccd.Status == CICCard.EnmICCardStatus.Disposed) 
            {
                return 107;
            }
            CLocation lct = new CWLocation().SelectLctFromICCode(iccode);  //依存车卡号查询车位
            if (lct == null)
            {
                return 101;  //该卡没有存车
            }
            CWSMG wsmg = new CWSMG();
            CSMG[] etvs = wsmg.SelectSMGsOfType(CSMG.EnmSMGType.ETV);
            CSMG[] halls = wsmg.SelectSMGsCanGetCar();  //选择模式为进出、出车且可用的车厅集合          

            CSMG etv; //所选ETV
            CSMG hall; //所选Hall
            //分配车厅、ETV
            new IEGBLL.AllocateETV.GetFeeOutAllocate().AllocateEtvAndHall(etvs, halls, lct, out etv, out hall);
            if (etv == null || hall == null)
            {
                return 102;   //没有可用的ETV或车厅
            }
            //车厅模式
            if (!wsmg.CheckHallMode(hall.ID))
            {
                return 103;
            }
            //ETV模式
            if (!wsmg.CheckEtvMode(etv.ID))
            {
                return 104;
            }
            CMasterTask mtsk = motsk.GetMasterTaskFromICCode(iccode);
            if (mtsk != null)
            {
                return 105; //当前卡号正在实行
            }
            int getCarCount = motsk.GetMTskCountFromHall(hall.ID, CMasterTask.EnmMasterTaskType.GetCar);
            if (getCarCount > CWData.MaxGetCarCount)
            {
                return 106; //当前作业已满
            }

            string mse = "收费出车- 分配ETV" + etv.ID + "  取车位：" + lct.Address + "  出车厅：" + hall.ID;
            new CWSException(mse, 0);

            string etvAddrs = wsmg.GetEtvCurrAddress(etv.ID);
            //生成作业
            motsk.OTempICcdGetCar(hall.ID, etv.ID, iccode, hall.Address, lct, etvAddrs);

            hallID = hall.ID;
            return 100;

        }

    }
}

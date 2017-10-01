using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using IEGInterface.localhost;

namespace IEGInterface
{
    partial class EqpService : ServiceBase
    {
        #region 端口号
        private static readonly string hall1Com = IEGInterface.Properties.Settings.Default.Hall1ICardCom;
        private static readonly string hall2Com = IEGInterface.Properties.Settings.Default.Hall2ICardCom;
        private static readonly string hall3Com = IEGInterface.Properties.Settings.Default.Hall3ICardCom;
        private static readonly string hall4Com = IEGInterface.Properties.Settings.Default.Hall4ICardCom;

        private static readonly string hall1SoundCom = IEGInterface.Properties.Settings.Default.Hall1SoundCom;
        private static readonly string hall2SoundCom = IEGInterface.Properties.Settings.Default.Hall2SoundCom;
        private static readonly string hall3SoundCom = IEGInterface.Properties.Settings.Default.Hall3SoundCom;
        private static readonly string hall4SoundCom = IEGInterface.Properties.Settings.Default.Hall4SoundCom;
        #endregion
        #region 设备变量
        private SoundControl hall1SoundControl;
        private SoundControl hall2SoundControl;
        private SoundControl hall3SoundControl;
        private SoundControl hall4SoundControl;

        private ICCardReader icrdOne;
        private ICCardReader icrdTwo;
        private ICCardReader icrdThree;
        private ICCardReader icrdFour;
        #endregion
        #region 线程变量
        //消息线程
        private Thread DealMessageThread;
        //刷卡器线程
        private Thread ICCardH1Thread;
        private Thread ICCardH2Thread;
        private Thread ICCardH3Thread;
        private Thread ICCardH4Thread;
        //声音线程
        private Thread SoundH1Thread;
        private Thread SoundH2Thread;
        private Thread SoundH3Thread;
        private Thread SoundH4Thread;
        //故障记录
        private Thread LogThread;
        #endregion

        private CMessageInterface cmsgif;
        private bool isStart = false;

        public EqpService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (isStart == false)
            {
                #region
                isStart = true;
                cmsgif = new CMessageInterface();

                icrdOne = new ICCardReader(11, Convert.ToInt32(hall1Com));
                icrdTwo = new ICCardReader(12, Convert.ToInt32(hall2Com));
                icrdThree = new ICCardReader(13, Convert.ToInt32(hall3Com));
                icrdFour = new ICCardReader(14, Convert.ToInt32(hall4Com));

                //消息线程
                try
                {
                    DealMessageThread = new Thread(new ThreadStart(DealMessage));
                    DealMessageThread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("PLC线程启动异常：" + ex.ToString());
                }

                //刷卡器1线程
                try
                {
                    ICCardH1Thread = new Thread(new ThreadStart(DealH1ICCardAction));
                    ICCardH1Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("刷卡器1线程启动异常：" + ex.ToString());
                }

                //刷卡器2线程
                try
                {
                    ICCardH2Thread = new Thread(new ThreadStart(DealH2ICCardAction));
                    ICCardH2Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("刷卡器2线程启动异常：" + ex.ToString());
                }

                //刷卡器3线程
                try
                {
                    ICCardH3Thread = new Thread(new ThreadStart(DealH3ICCardAction));
                    ICCardH3Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("刷卡器3线程启动异常：" + ex.ToString());
                }

                //刷卡器4线程
                try
                {
                    ICCardH4Thread = new Thread(new ThreadStart(DealH4ICCardAction));
                    ICCardH4Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("刷卡器4线程启动异常：" + ex.ToString());
                }

                //车厅1声音线程
                try
                {
                    SoundH1Thread = new Thread(new ThreadStart(DealHall1Sound));
                    SoundH1Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅1声音线程启动异常:" + ex.ToString());
                }

                //车厅2声音线程
                try
                {
                    SoundH2Thread = new Thread(new ThreadStart(DealHall2Sound));
                    SoundH2Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅2声音线程启动异常:" + ex.ToString());
                }

                //车厅3声音线程
                try
                {
                    SoundH3Thread = new Thread(new ThreadStart(DealHall3Sound));
                    SoundH3Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅3声音线程启动异常:" + ex.ToString());
                }

                //车厅4声音线程
                try
                {
                    SoundH4Thread = new Thread(new ThreadStart(DealHall4Sound));
                    SoundH4Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅4声音线程启动异常:" + ex.ToString());
                }

                //故障记录
                try
                {
                    LogThread = new Thread(new ThreadStart(DealRecordLog));
                    LogThread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("故障记录线程启动异常:" + ex.ToString());
                }

                CWSException.WriteLog("服务已经启动！", 4);
                #endregion
            }
        }

        protected override void OnStop()
        {         
            #region
                isStart = false;

                bool isStop = true;
                try
                {
                    isStop = isStop && DealMessageThread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && ICCardH1Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && ICCardH2Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && ICCardH3Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && ICCardH4Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && SoundH1Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && SoundH2Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && SoundH3Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && SoundH4Thread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                try
                {
                    isStop = isStop && LogThread.Join(500);
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                }
                //日志
                if (isStop == true)
                {
                    CWSException.WriteLog("服务已经停止！", 4);                    
                }
                else
                {
                    CWSException.WriteLog("服务未全停止！", 4);
                }

                #endregion            
        }

        #region 消息处理方法
        //消息处理
        private void DealMessage()
        {
            try
            {
                cmsgif.ConnectPLC();
            }
            catch (Exception ex) 
            {
                CWSException.WriteError(ex.ToString());
                return;
            }

            while (isStart) 
            {
                try 
                {
                    cmsgif.TaskAssign();
                    cmsgif.ReceiveMessage();
                    cmsgif.SendMessage();                   
                    cmsgif.DealErrAlarmAndSMGStat();
                    cmsgif.UpdateHallType();                    
                }
                catch (Exception ex) 
                {
                    CWSException.WriteError(ex.ToString());
                    Thread.Sleep(2000);
                }

                if (isStart) 
                {
                    Thread.Sleep(200);
                }
            }

            try 
            {
                cmsgif.DisconnectPLC();
            }
            catch (Exception ex) 
            {
                CWSException.WriteError(ex.ToString());
            }
        }
        #endregion
        #region 刷卡器处理方法
        //刷卡器1处理
        private void DealH1ICCardAction()
        {
            #region
            try
            {
                icrdOne.ConnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅刷卡器1连接异常： " + ex.ToString());
            }

            while (isStart)
            {
                try
                {
                    icrdOne.ICCardRead();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅1 " + ex.ToString());
                    Thread.Sleep(10000);
                }
                if (isStart)
                {
                    Thread.Sleep(800);
                }
            }

            try
            {
                icrdOne.DisconnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("刷卡器1断开异常：" + ex.ToString());
            }
            #endregion
        }

        //刷卡器2
        private void DealH2ICCardAction()
        {
            #region
            try
            {
                icrdTwo.ConnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅刷卡器2连接异常： " + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    icrdTwo.ICCardRead();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅2 " + ex.ToString());
                    Thread.Sleep(10000);
                }
                if (isStart)
                {
                    Thread.Sleep(800);
                }
            }
            try
            {
                icrdTwo.DisconnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("刷卡器2断开异常：" + ex.ToString());
            }
            #endregion
        }

        //刷卡器3
        private void DealH3ICCardAction()
        {
            #region
            try
            {
                icrdThree.ConnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅刷卡器3连接异常： " + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    icrdThree.ICCardRead();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅3 " + ex.ToString());
                    Thread.Sleep(10000);
                }
                if (isStart)
                {
                    Thread.Sleep(800);
                }
            }
            try
            {
                icrdThree.DisconnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("刷卡器3断开异常：" + ex.ToString());
            }
            #endregion
        }

        //刷卡器4
        private void DealH4ICCardAction()
        {
            #region
            try
            {
                icrdFour.ConnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅刷卡器4连接异常： " + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    icrdFour.ICCardRead();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅4 " + ex.ToString());
                    Thread.Sleep(10000);
                }

                if (isStart)
                {
                    Thread.Sleep(800);
                }
            }
            try
            {
                icrdFour.DisconnectCom();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("刷卡器4断开异常：" + ex.ToString());
            }
            #endregion
        }
        #endregion
        #region 声音处理方法
        //车厅1
        private void DealHall1Sound()
        {
            hall1SoundControl = new SoundControl();
            try
            {
                hall1SoundControl.SearchSoundDevice();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅声音SearchSoundDevice异常：" + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    hall1SoundControl.MakeSound(11, Convert.ToInt32(hall1SoundCom));
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅1声音MakeSound异常：" + ex.ToString());
                    Thread.Sleep(5000);
                }
                if (isStart)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        //车厅2
        private void DealHall2Sound()
        {
            hall2SoundControl = new SoundControl();
            try
            {
                hall2SoundControl.SearchSoundDevice();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅声音SearchSoundDevice异常：" + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    hall2SoundControl.MakeSound(12, Convert.ToInt32(hall2SoundCom));
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅2声音MakeSound异常：" + ex.ToString());
                    Thread.Sleep(5000);
                }

                if (isStart)
                {
                    Thread.Sleep(1000);
                }
            }
        }
        //车厅3
        private void DealHall3Sound()
        {
            hall3SoundControl = new SoundControl();
            try
            {
                hall3SoundControl.SearchSoundDevice();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅声音SearchSoundDevice异常：" + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    hall3SoundControl.MakeSound(13, Convert.ToInt32(hall3SoundCom));
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅3声音MakeSound异常：" + ex.ToString());
                    Thread.Sleep(5000);
                }

                if (isStart)
                {
                    Thread.Sleep(1000);
                }
            }

        }
        //车厅4
        private void DealHall4Sound()
        {
            hall4SoundControl = new SoundControl();
            try
            {
                hall4SoundControl.SearchSoundDevice();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅声音SearchSoundDevice异常：" + ex.ToString());
            }
            while (isStart)
            {
                try
                {
                    hall4SoundControl.MakeSound(14, Convert.ToInt32(hall4SoundCom));
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("车厅4声音MakeSound异常：" + ex.ToString());
                    Thread.Sleep(5000);
                }

                if (isStart)
                {
                    Thread.Sleep(1000);
                }
            }
        }
        #endregion
        #region 处理报警信息记录
        private void DealRecordLog() 
        {
            while (isStart) 
            {
                try
                {
                    cmsgif.DealRecordErrLog();
                }
                catch (Exception ex) 
                {
                    CWSException.WriteError("函数DealRecordLog发生异常："+ex.ToString());
                    Thread.Sleep(10000);
                }

                if (isStart) 
                {
                    Thread.Sleep(2000);
                }
            }
        }
        #endregion
    }
}

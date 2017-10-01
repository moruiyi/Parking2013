using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IEGLedInterface.localhost;
using System.IO.Ports;

namespace IEGLedInterface
{
    public partial class FrmLed : Form
    {
         public FrmLed()
        {
            InitializeComponent();

            #region LED
            byte effect = Convert.ToByte(pEffcetNum);
            byte rate = Convert.ToByte(pRate);
            byte stime = Convert.ToByte(pSingleTime);
            byte ttime = Convert.ToByte(pTotalTime);

            led1 = new CLedShow(Convert.ToByte(pLed1Address), effect, rate, stime, ttime);
            led2 = new CLedShow(Convert.ToByte(pLed2Address), effect, rate, stime, ttime);
            led3 = new CLedShow(Convert.ToByte(pLed3Address), effect, rate, stime, ttime);
            led4 = new CLedShow(Convert.ToByte(pLed4Address), effect, rate, stime, ttime);

            #region LED通讯端口初始化
            try
            {
                port_Led1 = new SerialPort();
                port_Led1.PortName = "COM" + portNum_Led1;
                port_Led1.BaudRate = 9600;
                port_Led1.DataBits = 8;
                port_Led1.Handshake = Handshake.None;
                port_Led1.DiscardNull = false;
                port_Led1.StopBits = StopBits.One;
                port_Led1.WriteBufferSize = 4096;
                port_Led1.WriteTimeout = -1;
                if (port_Led1.IsOpen)
                {
                    port_Led1.Close();
                }
                port_Led1.Open();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("SerialPort_LED1端口初始化异常：" + ex.ToString());
            }

            //try
            //{
            //    port_Led2 = new SerialPort();
            //    port_Led2.PortName = "COM" + portNum_Led2;
            //    port_Led2.BaudRate = 9600;
            //    port_Led2.DataBits = 8;
            //    port_Led2.Handshake = Handshake.None;
            //    port_Led2.DiscardNull = false;
            //    port_Led2.StopBits = StopBits.One;
            //    port_Led2.WriteBufferSize = 4096;
            //    port_Led2.WriteTimeout = -1;
            //    if (port_Led2.IsOpen)
            //    {
            //        port_Led2.Close();
            //    }
            //    port_Led2.Open();
            //}
            //catch (Exception ex)
            //{
            //    CWSException.WriteError("SerialPort_LED2端口初始化异常：" + ex.ToString());
            //}

            try
            {
                port_Led3 = new SerialPort();
                port_Led3.PortName = "COM" + portNum_Led3;
                port_Led3.BaudRate = 9600;
                port_Led3.DataBits = 8;
                port_Led3.Handshake = Handshake.None;
                port_Led3.DiscardNull = false;
                port_Led3.StopBits = StopBits.One;
                port_Led3.WriteBufferSize = 4096;
                port_Led3.WriteTimeout = -1;
                if (port_Led3.IsOpen)
                {
                    port_Led3.Close();
                }
                port_Led3.Open();
            }
            catch (Exception ex)
            {
                CWSException.WriteError("SerialPort_LED3端口初始化异常：" + ex.ToString());
            }

            //try
            //{
            //    port_Led4 = new SerialPort();
            //    port_Led4.PortName = "COM" + portNum_Led4;
            //    port_Led4.BaudRate = 9600;
            //    port_Led4.DataBits = 8;
            //    port_Led4.Handshake = Handshake.None;
            //    port_Led4.DiscardNull = false;
            //    port_Led4.StopBits = StopBits.One;
            //    port_Led4.WriteBufferSize = 4096;
            //    port_Led4.WriteTimeout = -1;
            //    if (port_Led4.IsOpen)
            //    {
            //        port_Led4.Close();
            //    }
            //    port_Led4.Open();
            //}
            //catch (Exception ex)
            //{
            //    CWSException.WriteError("SerialPort_LED4端口初始化异常：" + ex.ToString());
            //}
            #endregion
            #endregion         
            myDelegate = new CDelegate();
            myDelegate.DisplayLed1TitleEvent += new DisplayLed1TitleDelegate(myDelegate_DisplayLed1TitleEvent);
            myDelegate.DisplayLed2TitleEvent += new DisplayLed2TitleDelegate(myDelegate_DisplayLed2TitleEvent);
            myDelegate.DisplayLed3TitleEvent += new DisplayLed3TitleDelegate(myDelegate_DisplayLed3TitleEvent);
            myDelegate.DisplayLed4TitleEvent += new DisplayLed4TitleDelegate(myDelegate_DisplayLed4TitleEvent);
        }

        #region
         void myDelegate_DisplayLed4TitleEvent(string message)
        {
            byte[] sendBuffer = led4.GetSendBuffer(message);
            if (port_Led3.IsOpen) //发送字幕
            {
                port_Led3.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }

        void myDelegate_DisplayLed3TitleEvent(string message)
        {
            byte[] sendBuffer = led3.GetSendBuffer(message);
            if (port_Led3.IsOpen) //发送字幕
            {
                port_Led3.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }

        void myDelegate_DisplayLed2TitleEvent(string message)
        {
            byte[] sendBuffer = led2.GetSendBuffer(message);
            if (port_Led1.IsOpen) //发送字幕
            {
                port_Led1.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }

        void myDelegate_DisplayLed1TitleEvent(string message)
        {
            byte[] sendBuffer = led1.GetSendBuffer(message);
            if (port_Led1.IsOpen) //发送字幕
            {
                port_Led1.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }
        #endregion
        #region 变量
        private static readonly string portNum_Led1 = Properties.Settings.Default.Led1Com;
        private static readonly string portNum_Led2 = Properties.Settings.Default.Led2Com;
        private static readonly string portNum_Led3 = Properties.Settings.Default.Led3Com;
        private static readonly string portNum_Led4 = Properties.Settings.Default.Led4Com;

        private static readonly string pEffcetNum = Properties.Settings.Default.LEDEffect;
        private static readonly string pRate = Properties.Settings.Default.LEDEffect;
        private static readonly string pSingleTime = Properties.Settings.Default.ledSingleTime;
        private static readonly string pTotalTime = Properties.Settings.Default.ledTotalTime;

        private static readonly string pLed1Address = Properties.Settings.Default.Led1Addr;
        private static readonly string pLed2Address = Properties.Settings.Default.Led2Addr;
        private static readonly string pLed3Address = Properties.Settings.Default.Led3Addr;
        private static readonly string pLed4Address = Properties.Settings.Default.Led4Addr;

        private CLedShow led1;
        private CLedShow led2;
        private CLedShow led3;
        private CLedShow led4;

        private SerialPort port_Led1;
        //private SerialPort port_Led2;
        private SerialPort port_Led3;
        //private SerialPort port_Led4;

        private Thread Led1Thread;
        private Thread Led2Thread;
        private Thread Led3Thread;
        private Thread Led4Thread;
        #endregion

        private bool isStart = false;       
        private CDelegate myDelegate;

        private void btnStart_Click(object sender, EventArgs e)
        {
            #region
            if (!isStart) 
            {
                isStart = true;
                btnStop.Enabled = true;
                btnStart.Enabled = false;

                try 
                {
                    Led1Thread = new Thread(new ThreadStart(DealLed1Message));
                    Led1Thread.Start();
                }
                catch (Exception ex) 
                {
                    CWSException.WriteError("LED1线程启动异常：" + ex.ToString());
                }

                try
                {
                    Led2Thread = new Thread(new ThreadStart(DealLed2Message));
                    Led2Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("LED2线程启动异常：" + ex.ToString());
                }

                try
                {
                    Led3Thread = new Thread(new ThreadStart(DealLed3Message));
                    Led3Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("LED3线程启动异常：" + ex.ToString());
                }

                try
                {
                    Led4Thread = new Thread(new ThreadStart(DealLed4Message));
                    Led4Thread.Start();
                }
                catch (Exception ex)
                {
                    CWSException.WriteError("LED4线程启动异常：" + ex.ToString());
                }
            }
            #endregion
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            #region
            isStart = false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            bool isStop = true;
            try 
            {
                isStop = isStop & Led1Thread.Join(200);
            }
            catch (Exception ex) 
            {
                CWSException.WriteError(ex.ToString());
            }
            try
            {
                isStop = isStop & Led2Thread.Join(200);
            }
            catch (Exception ex)
            {
                CWSException.WriteError(ex.ToString());
            }
            try
            {
                isStop = isStop & Led3Thread.Join(200);
            }
            catch (Exception ex)
            {
                CWSException.WriteError(ex.ToString());
            }
            try
            {
                isStop = isStop & Led4Thread.Join(200);
            }
            catch (Exception ex)
            {
                CWSException.WriteError(ex.ToString());
            }
            if (isStop)
            {
                CWSException.WriteError("LED线程已全部停止！");
            }
            else 
            {
                CWSException.WriteError("LED线程未全部停止！");
            }
            #endregion
        }

        #region
        private string dataStr1 = "";
        private void DealLed1Message()
        {
            #region
            while (isStart)
            {
                try
                {
                    int total;
                    int space;
                    int occupy;
                    int fixLct;
                    int spaceBigLct;                    
                    Program.mng.SelectLctofInfo(out total, out space, out occupy, out fixLct, out spaceBigLct);

                    CMasterTask[] mtsks=Program.mng.GetAllMasterTaskOfHid(11);                   
                    string dataStr = "";

                    CSMG hall = Program.mng.SelectSMG(11);
                    if (hall.Available)
                    {
                        #region 作业
                        if (hall.nIsWorking != 0)
                        {
                            CMasterTask gmtsk = null;
                            if (mtsks != null)
                            {
                                foreach (CMasterTask mtsk in mtsks)
                                {
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (hall.nIsWorking == tsk.ID)
                                        {
                                            gmtsk = mtsk;
                                        }
                                    }
                                }
                            }
                            if (gmtsk != null)
                            {
                                if (gmtsk.ICCardCode != "")
                                {
                                    if (gmtsk.Type == EnmMasterTaskType.GetCar)
                                    {
                                        dataStr = "当前取车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else if (gmtsk.Type == EnmMasterTaskType.SaveCar)
                                    {
                                        dataStr= "当前存车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else
                                    {
                                        dataStr = "当前作业卡号：" + gmtsk.ICCardCode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dataStr= "剩余空闲车位：" + space.ToString()+"  其中大车位："+spaceBigLct.ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dataStr = " 正在维护，请稍后存取车";
                    }

                    if (dataStr != "")
                    {
                        if (string.Compare(dataStr, dataStr1) != 0) 
                        {
                            myDelegate.DisplayLed1Title(dataStr);
                            dataStr1 = dataStr;
                        }
                    }

                    if (isStart)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                    Thread.Sleep(10000);
                }
            }
            #endregion
        }

        private string dataStr2="";
        private void DealLed2Message()
        {
            #region
            while (isStart)
            {
                try
                {
                    int total;
                    int space;
                    int occupy;
                    int fixLct;
                    int spaceBigLct;
                    Program.mng.SelectLctofInfo(out total, out space, out occupy, out fixLct, out spaceBigLct);

                    CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(12);      
                    string dataStr = "";
                    CSMG hall = Program.mng.SelectSMG(12);
                    if (hall.Available)
                    {
                        #region 作业
                        if (hall.nIsWorking != 0)
                        {
                            CMasterTask gmtsk = null;
                            if (mtsks != null)
                            {
                                foreach (CMasterTask mtsk in mtsks)
                                {
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (hall.nIsWorking == tsk.ID)
                                        {
                                            gmtsk = mtsk;
                                        }
                                    }
                                }
                            }
                            if (gmtsk != null)
                            {
                                if (gmtsk.ICCardCode != "")
                                {
                                    if (gmtsk.Type == EnmMasterTaskType.GetCar)
                                    {
                                        dataStr = "当前取车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else if (gmtsk.Type == EnmMasterTaskType.SaveCar)
                                    {
                                        dataStr = "当前存车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else
                                    {
                                        dataStr = "当前作业卡号：" + gmtsk.ICCardCode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dataStr = "剩余空闲车位：" + space.ToString() + "  其中大车位：" + spaceBigLct.ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dataStr = " 正在维护，请稍后存取车";
                    }

                    if (dataStr != "")
                    {
                        if (string.Compare(dataStr, dataStr2) != 0)
                        {
                            myDelegate.DisplayLed2Title(dataStr);
                            dataStr2 = dataStr;
                        }
                    }

                    if (isStart)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                    Thread.Sleep(10000);
                }
            }
            #endregion
        }

        private string dataStr3="";
        private void DealLed3Message()
        {
            #region
            while (isStart)
            {
                try
                {
                    int total;
                    int space;
                    int occupy;
                    int fixLct;
                    int spaceBigLct;
                    Program.mng.SelectLctofInfo(out total, out space, out occupy, out fixLct, out spaceBigLct);

                    CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(13);
                    string dataStr = "";
                    CSMG hall = Program.mng.SelectSMG(13);
                    if (hall.Available)
                    {
                        #region 作业
                        if (hall.nIsWorking != 0)
                        {
                            CMasterTask gmtsk = null;
                            if (mtsks != null)
                            {
                                foreach (CMasterTask mtsk in mtsks)
                                {
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (hall.nIsWorking == tsk.ID)
                                        {
                                            gmtsk = mtsk;
                                        }
                                    }
                                }
                            }
                            if (gmtsk != null)
                            {
                                if (gmtsk.ICCardCode != "")
                                {
                                    if (gmtsk.Type == EnmMasterTaskType.GetCar)
                                    {
                                        dataStr = "当前取车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else if (gmtsk.Type == EnmMasterTaskType.SaveCar)
                                    {
                                        dataStr = "当前存车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else
                                    {
                                        dataStr = "当前作业卡号：" + gmtsk.ICCardCode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dataStr = "剩余空闲车位：" + space.ToString() + "  其中大车位：" + spaceBigLct.ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dataStr = " 正在维护，请稍后存取车";
                    }

                    if (dataStr != "")
                    {
                        if (string.Compare(dataStr, dataStr3) != 0) 
                        {
                            myDelegate.DisplayLed3Title(dataStr);
                            dataStr3 = dataStr;
                        }
                    }

                    if (isStart)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                    Thread.Sleep(10000);
                }
            }
            #endregion
        }

        private string dataStr4 = "";
        private void DealLed4Message()
        {
            #region
            while (isStart)
            {
                try
                {
                    int total;
                    int space;
                    int occupy;
                    int fixLct;
                    int spaceBigLct;
                    Program.mng.SelectLctofInfo(out total, out space, out occupy, out fixLct, out spaceBigLct);

                    CMasterTask[] mtsks = Program.mng.GetAllMasterTaskOfHid(14);
                    string dataStr = "";
                    CSMG hall = Program.mng.SelectSMG(14);
                    if (hall.Available)
                    {
                        #region 作业
                        if (hall.nIsWorking != 0)
                        {
                            CMasterTask gmtsk = null;
                            if (mtsks != null)
                            {
                                foreach (CMasterTask mtsk in mtsks)
                                {
                                    foreach (CTask tsk in mtsk.Tasks)
                                    {
                                        if (hall.nIsWorking == tsk.ID)
                                        {
                                            gmtsk = mtsk;
                                        }
                                    }
                                }
                            }
                            if (gmtsk != null)
                            {
                                if (gmtsk.ICCardCode != "")
                                {
                                    if (gmtsk.Type == EnmMasterTaskType.GetCar)
                                    {
                                        dataStr = "当前取车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else if (gmtsk.Type == EnmMasterTaskType.SaveCar)
                                    {
                                        dataStr = "当前存车卡号：" + gmtsk.ICCardCode;
                                    }
                                    else
                                    {
                                        dataStr = "当前作业卡号：" + gmtsk.ICCardCode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            dataStr = "剩余空闲车位：" + space.ToString() + "  其中大车位：" + spaceBigLct.ToString();
                        }
                        #endregion
                    }
                    else
                    {
                        dataStr = " 正在维护，请稍后存取车";
                    }

                    if (dataStr != "")
                    {
                        if (string.Compare(dataStr, dataStr4) != 0) 
                        {
                            myDelegate.DisplayLed4Title(dataStr);
                            dataStr4 = dataStr;
                        }
                    }

                    if (isStart)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    CWSException.WriteError(ex.ToString());
                    Thread.Sleep(10000);
                }
            }
            #endregion
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CTask
    {
        #region enum数据类型
        public enum EnmTaskStatus 
        {
            Init = 0,//初始
            ICarInWaitFirstSwipeCard,// 车厅内已经有车停好，等待刷卡
            IFirstSwipedWaitforCheckSize,// 入库车辆的第一次刷卡结束，提示用户第二次刷卡(等待下发1-9)
            ISecondSwipedWaitforCheckSize,//第二次刷卡，等待检测尺寸
            ISecondSwipedWaitforEVDown,// 第二次刷卡成功，等待检测车辆(等待下发1-1)
            ISecondSwipedWaitforCarLeave,// 第二次刷卡分配车位失败，等待车辆离开(等待下发3-1)

            TWaitforLoad,//车厅开始下降,等待执行装载(等待下发13-1)
            TWaitforUnload,  // 装载完成等待卸载

            UnLoadFinishing,//卸载完成

            OWaitforGetCar,//等待取车
            OTVLoadWaitforEVDown,//装载完成等待升降机下降
            OEVDownWaitforTVLoad,//升降机下降等待装载
            OWaitforEVUp,//出车卸载完成，等待升降机上升
            OCarOutWaitforDriveaway,// 车已取出，等待用户开车(等待下发3-1)

            Finished,   //作业完成(收到 3,54)
            ICheckCarFail,//检测失败(收到 1001,104)

            TMURO,// TV故障
            TMUROFinished,// 备用
            TMURORecoverNocar,// 故障人工确认继续
            TMURORecoverHascar,// TV故障恢复
            OTVLoadingWaitforEV,//手动完成
            TMUORWaitforUnload,
            OWaitforEVDown,   //出库开始，升降机等待下降(等待下发1-1)

            TWaitforMove,// 等待移动

            MoveFinishing,//移动完成
            HallFinishing,//出厅完成(收到 1003，4,下发3,54)
            LoadFinishing,//装载完成
            IEVDownFinished,//确认入库 收到(1,54,9999)后修改
            IEVDownFinishing,//升降机下降完成(等待下发1,54)
            OEVDownFinishing,//升降机下降完成（取）

            ICarInWaitPressButton,

            TempOWaitforEVDown,  //暂时取车
            TempOEVDownFinishing  //暂时取车时，升降机下降完成
        }

        public enum EnmTaskType 
        { 
            HallCarEntrance=0,  //有车进入车厅
            HallCarExit,        //取车使用车厅-1
            TVLoad,
            TVUnload,
            TVTranspose,     //库内搬移-4
            TVMove,          //移动-5
            TVGetCar,        //取车-6
            TVSaveCar,       //存车-7
            SaveCar,
            GetCar,
            Transpose,
            Move,
            TVAvoid   //避让-12
        }

        public enum EnmTaskStatusDetail 
        {
            NoSend=0,
            SendWaitAsk,
            Asked
        }
        #endregion
        private EnmTaskType meType;
        private EnmTaskStatus meStatus;
        private DateTime meCreateDtime;
        private DateTime meSendDtime;
        private DateTime meFinishDtime;
        private int meID;    //数据库分配ID
        private int meHid;   //车厅ID
        private int meMid;   //主作业ID
        private string mefrAddrs;
        private string metoAddrs;
        private string meICcode = "";
        private string meCarSize = "";
        private int meDist;
        private int meEqp;
        private CSMG.EnmSMGType meSMGType;
        private EnmTaskStatusDetail meDetail;
        private int meSerial;  //报文ID        

        public CTask() 
        {            
            meDetail = EnmTaskStatusDetail.NoSend;
            meCreateDtime = CObject.DefDatetime;
            meSendDtime = CObject.DefDatetime;
            meFinishDtime = CObject.DefDatetime;
            meSerial = 0;  //暂时不用的            
        }

        public CTask(string fladrs, string tladrs, EnmTaskType etype, EnmTaskStatus estatus, string ICcode, int eqpID, CSMG.EnmSMGType eqpType, int dist, int hid)
            : this() 
        {
            mefrAddrs = fladrs;
            metoAddrs = tladrs;
            meType = etype;
            meStatus = estatus;
            meICcode = ICcode;
            meEqp = eqpID;
            meSMGType = eqpType;
            meDist = dist;
            meHid = hid;    //车厅ID
        }

        public CTask(string fladrs, string tladrs, EnmTaskType etype, EnmTaskStatus estatus, string ICcode, int eqpID, CSMG.EnmSMGType eqpType, int dist, int hid, string carSize)
            : this(fladrs,tladrs,etype,estatus,ICcode,eqpID,eqpType,dist,hid) 
        {
            meCarSize = carSize;
        }

        //数据库检索用
        public CTask(int nID,int nDist,string nFrLct,string nToLct,EnmTaskType nType,EnmTaskStatus nStatus,string nICcode,int nHid,string nCarSize,int eqp,int SMGType)
            : this() 
        {
            meID = nID;
            meDist = nDist;
            mefrAddrs = nFrLct;
            metoAddrs = nToLct;
            meType = nType;
            meStatus = nStatus;
            meICcode = nICcode;
            meHid = nHid;
            meCarSize = nCarSize;
            meEqp = eqp;
            meSMGType = (CSMG.EnmSMGType)SMGType;
        }

        public string FromLctAdrs 
        {
            get 
            {
                return mefrAddrs;
            }
            set 
            {
                mefrAddrs = value;
            }
        }

        public string ToLctAdrs 
        {
            get { return metoAddrs; }
            set { metoAddrs = value; }
        }

        public EnmTaskType Type 
        {
            get { return meType; }
            set { meType = value; }
        }

        public EnmTaskStatus Status
        {
            get { return meStatus; }
            set { meStatus = value; }
        }

        public EnmTaskStatusDetail StatusDetail 
        {
            get { return meDetail; }
            set { meDetail = value; }
        }

        public DateTime CreateDtime 
        {
            get { return meCreateDtime; }
            set { meCreateDtime = value; }
        }

        public DateTime SendDtime 
        {
            get { return meSendDtime; }
            set { meSendDtime = value; }
        }

        public DateTime FinishDtime
        {
            get { return meFinishDtime; }
            set { meFinishDtime = value; }
        }

        public string ICCardCode 
        {
            get { return meICcode; }
            set { meICcode = value; }
        }
        //由数据库自动ID所得
        public int ID 
        {
            get { return meID; }
            set { meID = value; }
        }
        //车厅ID号
        public int HID 
        {
            get { return meHid; }
            set { meHid = value; }
        }

        public int MID 
        {
            get { return meMid; }
            set { meMid = value; }
        }

        public int Serial 
        {
            get { return meSerial; }
            set { meSerial = value; }
        }

        public string CarSize 
        {
            get { return meCarSize; }
            set { meCarSize = value; }
        }

        public int Distance 
        {
            get { return meDist; }
            set { meDist = value; }
        }

        public int SMG 
        {
            get { return meEqp; }
            set { meEqp = value; }
        }

        public CSMG.EnmSMGType SMGType 
        {
            get { return meSMGType; }
            set { meSMGType = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CSMG:CObject
    {
        #region 枚举类型
        public enum EnmSMGType 
        {
            Init=0,
            Hall,
            ETV
        }

        public enum EnmHallType 
        {
            Init=0,
            Entance,
            Exit,
            EnterorExit,
            ManageHouse
        }

        public enum EnmModel 
        {
            Init=0,
            AutoPending,   //自动挂起
            AutoAligning,  //自动分配
            AutoStopped,   //自动停止
            AutoStarting,  //自动开启中
            AutoStarted,   //全自动
            AutoStopping   //自动停止中
        }
        #endregion 

        private EnmSMGType mType;
        private EnmModel mModel;
        private EnmHallType mHallType;
        private int mWhouse;
        private string mAddress;   //物理地址
        private bool mIsAvl;
        private int mnIsWorking;   //子作业ID
        private int mMtskID;      //本项目用于保存主作业号
        private int mNextTask;
        private int mLayer;
        private int mRegion;
        private int mTaskCount;
        private string mCurAddrs;

        public CSMG() 
        {
            mModel = EnmModel.Init;
        }

        public CSMG(int nid,string nCode,bool nAvalible,EnmModel nMode,EnmSMGType nType,EnmHallType nHtype,int nWh,int nMtsk,string nAddrs,int ntskID,int nNextTask,int nLayer,int nRegion) 
            : base(nid,nCode)
        {
            mType = nType;
            mHallType = nHtype;
            mModel = nMode;
            mWhouse = nWh;
            mMtskID=nMtsk;         //用于保存主作业号
            mIsAvl = nAvalible;
            mnIsWorking = ntskID;
            mNextTask = nNextTask;
            mLayer = nLayer;
            mRegion = nRegion;
            mAddress = nAddrs;
        }

        public bool Available 
        {
            get { return mIsAvl; }
            set { mIsAvl = value; }
        }

        public EnmModel Model
        {
            get { return mModel; }
            set { mModel = value; }
        }

        public int Warehouse 
        {
            get { return mWhouse; }
            set { ; }
        }

        public int nIsWorking 
        {
            get { return mnIsWorking; }
            set { mnIsWorking = value; }
        }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string Address 
        {
            get { return mAddress; }
            set { ; }
        }        

        public int Layer 
        {
            get { return mLayer; }
            set { mLayer = value; }
        }

        public int Region 
        {
            get { return mRegion; }
            set { mRegion = value; }
        }

        public int TaskCount 
        {
            get { return mTaskCount; }
            set { mTaskCount = value; }
        }

        public EnmHallType HallType 
        {
            get { return mHallType; }
            set { mHallType = value; }
        }

        public EnmSMGType SMGType 
        {
            get { return mType; }
            set { mType = value; }
        }

        /// <summary>
        /// 当前执行的主作业号
        /// </summary>
        public int MTskID 
        {
            get { return mMtskID; }
            set { mMtskID = value; }
        }

        /// <summary>
        /// 用于保存装卸载作业号，以保证移动后，不去执行避让
        /// (只有在判断是否产生避让时才用到)
        /// </summary>
        public int NextTaskId 
        {
            get { return mNextTask; }
            set { mNextTask = value; }
        }

        /// <summary>
        /// 用于保存ETV当前地址
        /// </summary>
        public string CurrAddress 
        {
            get 
            {
                if (mCurAddrs == null) 
                {
                    if (Code == "1")
                    {
                        mCurAddrs = "1011";
                    }
                    else 
                    {
                        mCurAddrs = "1201";
                    }
                }
                return mCurAddrs;
            }
            set { mCurAddrs = value; }
        }
    }
}

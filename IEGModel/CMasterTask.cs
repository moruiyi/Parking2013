using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CMasterTask
    {
        public enum EnmMasterTaskType 
        {
            SaveCar=0,   //存车
            GetCar,      //取车
            Transpose,   //挪移
            Move,        //移动
            TempGetCar,   //取物    
            Avoid
        }

        private int mID;
        private bool mIsComplete;
        private List<CTask> mLstTasks;
        private EnmMasterTaskType mType;
        private int mHID;
        private string mICcode = "";
        private int mWID;   //库号
        private bool mIsTemp;

        public CMasterTask() 
        {
            mLstTasks=new List<CTask>();
            mIsComplete = false;
            mIsTemp = false;
        }
        //新建主作业
        public CMasterTask(EnmMasterTaskType ntype,string nICCode,int nHID,int nWID) : this() 
        {
            mType = ntype;
            mICcode = nICCode;
            mHID = nHID;
            mWID = nWID;
        }
        //建立取物作业
        public CMasterTask(EnmMasterTaskType ntype, string nICcode, int nhid, int nwid, bool nisTemp) 
            : this(ntype,nICcode,nhid,nwid) 
        {
            mIsTemp = nisTemp;
        }

        //数据库检索
        public CMasterTask(int nid,EnmMasterTaskType ntype,bool iscpl,string iccd,int nhid,int nwid) : this()
        {
            mID = nid;
            mType = ntype;
            mIsComplete = iscpl;
            mICcode = iccd;
            mHID = nhid;
            mWID = nwid;
        }

        public int ID 
        {
            get { return mID; }
            set { mID = value; }
        }

        public int AddTask(CTask tsk) 
        {
            if (!mLstTasks.Contains(tsk)) 
            {
                mLstTasks.Add(tsk);
            }
            return mLstTasks.Count;
        }

        public int InsertTask(int idx,CTask tsk)
        {
            if (!mLstTasks.Contains(tsk)) 
            {
                mLstTasks.Insert(idx,tsk);
            }
            return mLstTasks.Count;
        }

        public int RemoveTask(CTask tsk) 
        {
            if (mLstTasks.Contains(tsk)) 
            {
                mLstTasks.Remove(tsk);
            }
            return mLstTasks.Count;
        }

        public CTask[] Tasks 
        {
            get 
            {
                return mLstTasks.ToArray();
            }
            set { ;}
        }

        public EnmMasterTaskType Type 
        {
            get { return mType; }
            set { mType = value; }
        }

        public string ICCardCode 
        {
            get { return mICcode; }
            set { mICcode = value; }
        }

        public bool IsCompleted 
        {
            get { return mIsComplete; }
            set { mIsComplete = value; }
        }

        public bool IsTemp 
        {
            get { return mIsTemp; }
            set { mIsTemp = value; }
        }

        public int HID 
        {
            get { return mHID; }
            set { mHID = value; }
        }

        public int WID 
        {
            get { return mWID; }
            set { mWID = value; }
        }

        public int TaskCount 
        {
            get 
            {
                return mLstTasks.Count;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CLocation:CObject,IComparable
    {
        public enum EnmLocationStatus 
        {
            Init=0,  //初始
            Space,   //空闲
            Occupy,  //占用-2
            Entering, //正在入库-3
            Outing,    //正在出库-4
            Temping,    //临时取物车位
            Idleness    //无效车位-6
        }

        public enum EnmLocationType 
        {
            Init = 0,  //初始
            Normal,   //正常
            Hall,     //车厅
            Disable,  //禁用
            ETV,       //ETV      
            Invalid   //无效车位-5
        }

        /// <summary>
        /// 预留
        /// </summary>
        public enum EnmLocationSize 
        {
            Init=0,
        }

        private EnmLocationType meType;
        private EnmLocationStatus meStatus;
        private int meLine;   //边、排
        private int meLayer;  //层
        private int meList;   //列        
        private DateTime meInDtime;  //入库时间
        private string meAddress;    //车位地址，另一个边列层
        private int meWhouse;
        private string meSize;      //车位容纳外形尺寸
        private string meCarSize;   //入库车辆外形
        private bool meIsLocked;
        private CICCard meICcard;
        private string meIcCode;    //用户卡ID  
        private int meIdx;          //索引
        private int meIsqx;         //前悬长度
        private int mepri;
        private int meRegion;       //区域
        private int meDistance;     //轴距

        public CLocation() : base() 
        {
            meStatus = EnmLocationStatus.Init;
            meType = EnmLocationType.Init;
            meInDtime = CObject.DefDatetime;
            meIsLocked = false;
            meIcCode = "";
            meCarSize = "";
            meDistance = 0;
        }

        public CLocation(string nAddrs,int nLine,int nLayer,int nList,EnmLocationType nType,EnmLocationStatus nStatus,int nWarehouse,string nICcard,DateTime ndINdtime,
            int nDist,int nIdx,string nCSize,int nIsqx,int nRegion,int pri,string nSize) 
            : this()
        {
            meType = nType;
            meStatus = nStatus;
            meLine = nLine;
            meList = nList;
            meLayer = nLayer;
            meWhouse = nWarehouse;
            meAddress = nAddrs;
            meIcCode = nICcard;
            meInDtime = ndINdtime;
            meDistance = nDist;
            meSize = nSize;        //车位尺寸
            meCarSize = nCSize;    //车辆尺寸
            meIdx = nIdx;
            meIsqx = nIsqx;        //前悬长度
            meRegion = nRegion;
            mepri = pri;
        }

        public EnmLocationType Type 
        {
            get { return meType; }
            set { meType = value; }
        }

        public EnmLocationStatus Status 
        {
            get { return meStatus; }
            set { meStatus = value; }
        }
        /// <summary>
        /// 边
        /// </summary>
        public int Line
        {
            get { return meLine; }
            set { meLine = value; }
        }
        /// <summary>
        /// 列
        /// </summary>
        public int List 
        {
            get { return meList; }
            set { meList = value; }
        }

        public int Layer 
        {
            get { return meLayer; }
            set { meLayer = value; }
        }
        /// <summary>
        /// 车位地址
        /// </summary>
        public string Address 
        {
            get { return meAddress; }
            set { meAddress = value; }
        }

        public int Warehouse
        {
            get { return meWhouse; }
            set { meWhouse = value; }
        }

        //绑定变量
        public void SetICCard(CICCard oCard)
        {
            if (oCard == null)
            {
                meIcCode = "";
            }
            meICcard = oCard;
        }

        public string ICCardPhyCode 
        {
            get { return meICcard == null ? "" : meICcard.PhysicCode; }
        }

        public string ICCardCode 
        {
            get 
            { 
                return meICcard == null ? meIcCode : meICcard.Code;
            }
            set 
            { 
                meIcCode = value;
            }
        }

        public CICCard.EnmICCardType ICCardType 
        {
            get 
            {
                return meICcard == null ? CICCard.EnmICCardType.Init : meICcard.Type;
            }
        }

        public DateTime InDate 
        {
            get { return meInDtime; }
            set { meInDtime = value; }
        }
        
        /// <summary>
        /// 车位容纳的尺寸
        /// </summary>
        public string Size
        {
            get { return meSize; }
            set { meSize = value; }
        }
        /// <summary>
        /// 入库车位尺寸
        /// </summary>
        public string CarSize 
        {
            get { return meCarSize; }
            set { meCarSize = value; }
        }       

        public bool IsLocked 
        {
            get { return meIsLocked; }
            set { meIsLocked = value; }
        }

        public int Distance 
        {
            get { return meDistance; }
            set { meDistance = value; }
        }

        public int Index 
        {
            get { return meIdx; }
            set { meIdx = value; }
        }
        /// <summary>
        /// 前悬长度
        /// </summary>
        public int IsQianxuan 
        {
            get { return meIsqx; }
            set { meIsqx = value; }
        }

        public int Region 
        {
            get { return meRegion; }
            set { meRegion = value; }
        }

        public int Pri
        {
            get { return mepri; }
            set { mepri = value; }
        }

        #region  IComparable members 依库区号，索引号升序排列

        public int CompareTo(object obj) 
        {
            if (obj is CLocation)
            {
                CLocation lct = obj as CLocation;
                if ((this.Region).CompareTo(lct.Region) > 0)
                {
                    return 1;
                }
                else if ((this.Region).CompareTo(lct.Region) < 0)
                {
                    return -1;
                }
                else
                {
                    if ((this.Index).CompareTo(lct.Index) > 0)
                    {
                        return 1;
                    }
                    else if ((this.Index).CompareTo(lct.Index) < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else 
            {
                throw new ArgumentException("Object is not a CLocation object");
            }
        }

        #endregion
    }
}

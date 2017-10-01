using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CICCard:CObject
    {
        public enum EnmICCardType 
        {
            Init=0,  //初始
            Temp,    //临时
            Fixed,   //定期
            FixedLocation  //vip卡
        }
        public enum EnmICCardStatus 
        {
            Init=0,
            Lost,     //挂失
            Normal,   //正常
            Disposed  //注销
        }

        private string mPhysicCode;
        private EnmICCardType meType;        
        private EnmICCardStatus meStatus;
        private DateTime mCDtime;     //卡创建时间
        private DateTime mLosstime;
        private DateTime mDuetime;
        private DateTime mdDDtime;    //消销时间
        private int mOwedDays;
        private int mnUid;           //固定卡时，用户ID
        private string msPlatNumber; //车牌号
        private string msAddrs;      

        public CICCard() 
        {
            meType = EnmICCardType.Temp;
            meStatus = EnmICCardStatus.Normal;
            mCDtime = DateTime.Now;
            msAddrs = "";
        }
        //ID不可用
        public CICCard(string sCard,string sPCard,EnmICCardType etype,EnmICCardStatus estatus,DateTime losstime,DateTime duetime,int oweddays,int uid,string pnmd,DateTime cdt,string address,DateTime disposetime) 
            :base(0,sCard)                
        {
            mPhysicCode = sPCard;
            meType = etype;
            meStatus = estatus;
            mLosstime = losstime;
            mDuetime = duetime;
            mOwedDays = oweddays;
            mnUid = uid;          //固定卡时，用户ID
            msPlatNumber = pnmd;
            mCDtime = cdt;       //创建卡时间
            msAddrs = address;
            mdDDtime = disposetime;  //注销时间
        }

        public string PhysicCode 
        {
            get 
            {
                return mPhysicCode;
            }
            set 
            {
                mPhysicCode = value;
            }
        }

        public EnmICCardType Type
        {
            get
            {
                return meType;
            }
            set 
            {
                meType = value;
            }
        }

        public EnmICCardStatus Status
        {
            get 
            {
                return meStatus;
            }
            set
            {
                meStatus = value;
            }
        }
        //建立定期卡时日期
        public DateTime DueDtime 
        {
            get
            {
                return mDuetime;
            }
            set
            {
                mDuetime = value;
            }
        }

        public DateTime CreateDtime
        {
            get
            {
                return mCDtime;
            }
            set 
            {
                mCDtime = value;
            }
        }

        public DateTime LossDtime 
        {
            get
            {
                return mLosstime;
            }
            set
            {
                mLosstime = value;
            }
        }
        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime DisposeDtime
        {
            set 
            {
                mdDDtime = value;
            }
            get
            {
                return mdDDtime;
            }
        }

        public int CustomerID 
        {
            get 
            {
                return mnUid;
            }
            set 
            {
                mnUid = value;
            }
        }

        public int OweDays 
        {
            get
            {
                return mOwedDays;
            }
            set 
            {
                mOwedDays = value;
            }
        }

        public string PlatNumber 
        {
            get 
            {
                return msPlatNumber;
            }
            set 
            {
                msPlatNumber = value;
            }
        }
        /// <summary>
        /// 绑定车位地址
        /// </summary>
        public string Address 
        {
            get 
            {
                return msAddrs;
            }
            set
            {
                msAddrs = value;
            }
        }
    }
}

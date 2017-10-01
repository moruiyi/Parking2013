using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CCustomer : CObject
    {
        private string msMobile;
        private string msAddress;
        private string msTelphone;
        private CICCard moCard;

        //opm1添加绑定车位信息

        public CCustomer()
        {
            msMobile = "";
            msAddress = "";
            msTelphone = "";
        }

        public CCustomer(int nid,string sname,string sadrs,string stel,string smbl)
            :base(nid,sname)
        {
            msAddress = sadrs;
            msTelphone = stel;
            msMobile = smbl;
        }       

        public string ICCardCode
        {
            get { return moCard == null ? "" : moCard.Code; }
            set { ;}
        } 

        public CICCard.EnmICCardType ICCardType
        {
            get { return moCard == null ? CICCard.EnmICCardType.Init : moCard.Type; }
            set { ;}
        }

        public string LctAddress
        {
            get { return moCard == null ? "" : moCard.Address; }
            set { ;}
        }

        public CICCard.EnmICCardStatus ICCardStat
        {
            get { return moCard == null ? CICCard.EnmICCardStatus.Init : moCard.Status; }
            set { ;}
        } 
        
        public DateTime IccdCdTime 
        {
            get 
            {
                return moCard == null ? CObject.DefDatetime : moCard.CreateDtime;
            }
            set { ;}
        }

        public DateTime ICCardDueDate
        {
            get { return moCard == null ? CObject.DefDatetime : moCard.DueDtime; }
            set { ;}
        }
        //用户住址
        public string Address
        {
            get
            {
                return msAddress;
            }
            set
            {
                msAddress = value;
            }
        }

        public string PlatNumber
        {
            get { return moCard == null ? "" : moCard.PlatNumber; }
            set { ;}
        }

        public string Telphone
        {
            get
            {
                return msTelphone;
            }
            set
            {
                msTelphone = value;
            }
        }

        public string Mobile
        {
            get
            {
                return msMobile;
            }
            set
            {
                msMobile = value;
            }
        }

        public void SetICCard(CICCard iccd) 
        {
            moCard = iccd;
        }
       
    }
}

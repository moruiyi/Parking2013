using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CTempCardChargeLog
    {
        private string msIccd;
        private DateTime mdInDate;
        private DateTime mdOutDate;
        private float mcRecFee;
        private float mcActFee;
        private string msOpt;
        private string msadrs;

        public string ICCode
        {
            get
            {
                return msIccd;
            }
            set { msIccd = value; }
        }

        public DateTime InDate
        {
            get
            {
                return mdInDate;
            }
            set
            {
                mdInDate = value;
            }
        }

        public DateTime OutDate
        {
            get
            {
                return mdOutDate;
            }
            set
            {
                mdOutDate = value;
            }
        }

        public float RecivFee
        {
            get
            {
                return mcRecFee;
            }
            set
            {
                mcRecFee = value;
            }
        }

        public float ActualFee
        {
            get
            {
                return mcActFee;
            }
            set
            {
                mcActFee = value;
            }
        }

        public string OperatorCode
        {
            get
            {
                return msOpt;
            }
            set { msOpt = value; }
        }

        public string LocationAddress
        {
            get { return msadrs; }
            set { msadrs = value; }
        }

    }

    [Serializable]
    public class CFixCardChargeLog
    {
        private string msIccd;
        private DateTime mdlastDate;
        private DateTime mdthisDate;
        private float mcthisFee;
        private float mclastFee;
        private string msOpt;
        private String msicpcd;
        private string msuser;
        private CICCard.EnmICCardType meictype;
        private int mnfunit;
        private DateTime mdduetime;
        private DateTime mdInDate;
        private float mcRecFee;

        public float RecivFee
        {
            get
            {
                return mcRecFee;
            }
            set
            {
                mcRecFee = value;
            }
        }

        public DateTime InDate
        {
            get
            {
                return mdInDate;
            }
            set
            {
                mdInDate = value;
            }
        }

        public string ICCode
        {
            get
            {
                return msIccd;
            }
            set { msIccd = value; }
        }

        public DateTime LastDate
        {
            get
            {
                return mdlastDate;
            }
            set
            {
                mdlastDate = value;
            }
        }

        public DateTime ThisDate
        {
            get
            {
                return mdthisDate;
            }
            set
            {
                mdthisDate = value;
            }
        }

        public float ThisFee
        {
            get
            {
                return mcthisFee;
            }
            set
            {
                mcthisFee = value;
            }
        }

        public float LastFee
        {
            get
            {
                return mclastFee;
            }
            set
            {
                mclastFee = value;
            }
        }

        public string OperatorCode
        {
            get
            {
                return msOpt;
            }
            set { msOpt = value; }
        }

        public CICCard.EnmICCardType ICType
        {
            get
            {
                return meictype;
            }
            set
            {
                meictype = value;
            }
        }

        public DateTime DueDtime
        {
            get
            {
                return mdduetime;
            }
            set
            {
                mdduetime = value;
            }
        }

        public int FeeUnit
        {
            get
            {
                return mnfunit;
            }
            set
            {
                mnfunit = value;
            }
        }

        public string UserName
        {
            get
            {
                return msuser;
            }
            set
            {
                msuser = value;
            }
        }

        public string ICPCode
        {
            get
            {
                return msicpcd;
            }
            set
            {
                msicpcd = value;
            }
        }

    }
}

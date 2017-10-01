using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CTariff:IComparable
    {
        public enum EnmFeeType
        {
            Init = 0,
            Charging,    //计费
            Limited,     //限额
            FirstCharge  //首计
        }

        public enum EnmFeeUnit
        {
            Init = 0,
            Hour,
            Month,
            Season,
            Year
        }

        private int mnid;
        private CICCard.EnmICCardType metype;
        private EnmFeeUnit meunit;
        private float mffee;
        private float mdtime;
        private EnmFeeType meftype;
        private bool isbusy;

        public CTariff()
        {
            metype = CICCard.EnmICCardType.Init;
            meunit = EnmFeeUnit.Init;
            meftype = EnmFeeType.Init;
        }

        public CTariff(int nid, CICCard.EnmICCardType etype, EnmFeeUnit eunt, float ffee, float dtime, EnmFeeType eftype, bool busy)
            : this()
        {
            mnid = nid;
            metype = etype;  //卡类型
            meunit = eunt;
            mffee = ffee;
            mdtime = dtime;   //计费时间-days
            meftype = eftype;  //收费类型
            isbusy = busy;
        }

        public bool ISbusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;
            }
        }

        public IEGModel.CICCard.EnmICCardType Type
        {
            get
            {
                return metype;
            }
            set
            {
                metype=value;
            }
        }

        public int ID
        {
            get
            {
                return mnid;
            }
            set
            {
                mnid=value;
            }
        }

        public EnmFeeUnit Unit
        {
            get
            {
                return meunit;
            }
            set
            {
                meunit = value;
            }
        }

        public EnmFeeType FeeType
        {
            get
            {
                return meftype;
            }
            set
            {
                meftype = value;
            }
        }

        public float Time
        {
            get
            {
                return mdtime;
            }
            set
            {
                mdtime = value;
            }
        }

        public float Fee
        {
            get
            {
                return mffee;
            }
            set
            {
                mffee = value;
            }
        }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            CTariff trf = obj as CTariff;
            if (trf != null)
            {
                if (this.Time > trf.mdtime)
                {
                    return 1;
                }
                else if (this.Time < trf.mdtime)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new ArgumentException("obj is not a CTariff object");
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    public class CConstant
    {
        private DateTime mdOutStTime1;
        private DateTime mdOutEnTime1;
        private DateTime mdOutStTime2;
        private DateTime mdOutEnTime2;

        public CConstant()
        {
            mdOutEnTime1 = CObject.DefDatetime;
            mdOutEnTime2 = CObject.DefDatetime;
            mdOutStTime1 = CObject.DefDatetime;
            mdOutStTime2 = CObject.DefDatetime;
        }

        public DateTime OutStartTime1
        {
            get
            {
                return mdOutStTime1;
            }
            set
            {
                mdOutStTime1 = value;
            }
        }

        public DateTime OutStartTime2
        {
            get
            {
                return mdOutStTime2;
            }
            set
            {
                mdOutStTime2 = value;
            }
        }

        public DateTime OutEndTime1
        {
            get
            {
                return mdOutEnTime1;
            }
            set
            {
                mdOutEnTime1 = value;
            }
        }


        public DateTime OutEndTime2
        {
            get
            {
                return mdOutEnTime2;
            }
            set
            {
                mdOutEnTime2 = value;
            }
        }
    }
}

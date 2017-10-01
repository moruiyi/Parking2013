using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{    
    public class CSysLog
    {      
        private DateTime mddtime;
        private string msdesc;
        private string msopcd;  //用户代码
        private string mcard;   //日志时为卡号，故障时为地址

        public CSysLog()
        {
            msdesc = "";
            mddtime = CObject.DefDatetime;
            msopcd = "";
            mcard = "";
        }

        public CSysLog(string card, DateTime ddtime, string sdsc, string sopcd)
            : this()
        {           
            mddtime = ddtime;
            msdesc = sdsc;
            msopcd = sopcd;
            mcard = card;
        }     

        public DateTime Dtime
        {
            get
            {
                return mddtime;
            }
            set
            {
                mddtime = value;
            }
        }

        public string Description
        {
            get
            {
                return msdesc;
            }
            set
            {
                msdesc = value;
            }
        }

        public string OperatCode
        {
            get
            {
                return msopcd;
            }
            set
            {
                msopcd = value;
            }
        }

        //public string OperatName
        //{
        //    get
        //    {
        //        return msopnm;
        //    }
        //    set
        //    {
        //        msopnm = value;
        //    }
        //}
        public string Mcard
        {
            get
            {
                return mcard;
            }
            set
            {
                mcard = value;
            }
        }
    }
}

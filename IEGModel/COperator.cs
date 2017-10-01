using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class COperator : CObject
    {
        private string mspwd;
        private EnmOperatorType meType;
        private string msname;

        public enum EnmOperatorType
        {
            Operator = 0,
            Manager
        }

        public COperator()
        {
            mspwd = "";
            meType = EnmOperatorType.Operator;
            msname = "";
        }

        public COperator(string scode, string sname, string spwd, EnmOperatorType etype)
            : base(0, scode)
        {
            msname = sname;
            mspwd = spwd;
            meType = etype;
        }

        public EnmOperatorType Type
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

        public string Password
        {
            get { return mspwd; }
            set { mspwd = value; }
        }

        public string Name
        {
            get
            {
                return msname;
            }
            set
            {
                msname = value;
            }
        }
    }
}

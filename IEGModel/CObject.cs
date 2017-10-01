using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CObject
    {
        private int mID;
        private string mCode;
        public static readonly DateTime DefDatetime = new DateTime(2014,8,11,0,0,0);

        public CObject(int nID, string nCode):this()
        {
            mID = nID;
            mCode = nCode;
        }
        public CObject()
        {
            mCode = "";
        }

        public int ID 
        {
            get 
            {
                return mID;
            }
            set 
            {
                mID = value;
            }
        }

        public string Code
        {
            get 
            {
                return mCode;
            }
            set 
            {
                mCode = value;
            }
        }
    }
}

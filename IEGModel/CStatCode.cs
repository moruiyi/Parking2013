using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGModel
{
    [Serializable]
    public class CStatCode
    {
        private short meStartBit;
        private short meCurrentValue;

        public CStatCode() 
        {
            meStartBit = 0;
            meCurrentValue = 0;
        }

        public CStatCode(short sb,short value) : this() 
        {
            meStartBit = sb;
            meCurrentValue = value;
        }

        public short StartBit 
        {
            get { return meStartBit; }
            set { meStartBit = value; }
        }

        public short CurrentValue 
        {
            get { return meCurrentValue; }
            set { meCurrentValue = value; }
        }
    }


    [Serializable]
    public class CErrorCode
    {
        private short meStartBit;
        private byte meCurrentValue;
        private byte meColor;
        private byte meType;     //所属设备
        private string meDesc;   //描述

        public CErrorCode() 
        {
            meStartBit = 0;
            meCurrentValue = 0;
            meColor = 0;
            meDesc = "";
        }

        public CErrorCode(short sb,byte color,byte type) : this() 
        {
            meStartBit = sb;
            meColor = color;
            meType = type;
        }

        public CErrorCode(short sb,string desc) : this() 
        {
            meStartBit = sb;
            meDesc = desc;
        }

        public short StartBit 
        {
            get { return meStartBit; }
            set { meStartBit = value; }
        }

        public byte CurrentValue 
        {
            get { return meCurrentValue; }
            set { meCurrentValue = value; }
        }

        public byte Type 
        {
            get { return meType; }
            set { meType = value; }
        }

        public byte Color 
        {
            get { return meColor; }
            set { meColor = value; }
        }

        public string Description 
        {
            get { return meDesc; }
            set { meDesc = value; }
        }
    }
}

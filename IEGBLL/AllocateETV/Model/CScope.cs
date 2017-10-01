﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGBLL.AllocateETV
{
    public class CScope
    {
        private int leftCol;   //左端列
        private int rightCol;  //右端列

        public CScope() 
        {
        }

        public CScope(int left, int right) 
        {
            leftCol = left;
            rightCol = right;
        }

        public int LeftCol 
        {
            get 
            {
                return leftCol;
            }
            set 
            {
                leftCol = value;
            }
        }

        public int RightCol 
        {
            get
            { 
                return rightCol;
            }
            set 
            { 
                rightCol = value;
            }
        }
    }
}

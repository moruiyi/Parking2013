using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGBLL.AllocateETV
{
    public class CEquip
    {
        public CScope WorkScope { get; set; }     //当前运行可达区域
        public CScope MaxWorkScope { get; set; }  //当前物理可达区域

        private CScope physicWorkScope;
        //原始物理可达区域
        public CScope PhysicWorkScope 
        {
            get 
            {
                return physicWorkScope; 
            }
            set 
            { 
                physicWorkScope = value;
            }
        }

        public CEquip(int lefCol, int rightCol) 
        {
            PhysicWorkScope = new CScope(lefCol,rightCol);
            WorkScope = new CScope();
            MaxWorkScope = new CScope();
        }
    }
}

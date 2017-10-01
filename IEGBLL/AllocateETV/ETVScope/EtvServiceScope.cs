using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;

namespace IEGBLL.AllocateETV
{
    public class EtvServiceScope
    {
        private IList<CSMG> mEtvList = null;
        private readonly int Etv1LefPhysCol = 1;
        private readonly int Etv1RightPhysCol = 16;
        private readonly int Etv2LefPhysCol = 5;
        private readonly int Etv2RightPhysCol = 20;

        public EtvServiceScope(IList<CSMG> pEtvList)
        {
            mEtvList = pEtvList;
        }

        /// <summary>
        /// 实时更新ETV作业集合
        /// </summary>
        public IList<CSMG> EtvList 
        {
            set 
            {
                mEtvList = value;
            }
        }

        /// <summary>
        /// 属性，ETV的作业范围/物理范围
        /// </summary>
        public Dictionary<CSMG, CEquip> DicEtvScope 
        {
            get 
            {
                return GetEtvScope();
            }
        }

        /// <summary>
        /// 属性，ETV的可达的范围
        /// </summary>
        public Dictionary<CSMG, CScope> DicWorkScope 
        {
            get 
            {
                Dictionary<CSMG, CScope> dicWork = new Dictionary<CSMG, CScope>();
                foreach (KeyValuePair<CSMG, CEquip> pair in DicEtvScope)
                {
                    dicWork.Add(pair.Key,pair.Value.WorkScope);
                }
                return dicWork;
            }
        }

        /// <summary>
        /// 属性，ETV的可达的物理范围
        /// </summary>
        public Dictionary<CSMG, CScope> DicMaxScope 
        {
            get 
            {
                Dictionary<CSMG, CScope> dicMaxWork = new Dictionary<CSMG, CScope>();
                foreach (KeyValuePair<CSMG, CEquip> pair in DicEtvScope) 
                {
                    dicMaxWork.Add(pair.Key,pair.Value.MaxWorkScope);
                }
                return dicMaxWork;
            }
        }

        /// <summary>
        /// 建立ETV的作业区域
        /// </summary>
        /// <returns></returns>
        private Dictionary<CSMG, CEquip> GetEtvScope() 
        {
            Dictionary<CSMG,CEquip> DicEscope=new Dictionary<CSMG,CEquip>();
            foreach (CSMG smg in mEtvList)
            {
                CEquip etvScope = InitialEtvScope(smg.ID);

                //建立ETV可达最大区域（物理区域）
                EtvMaxScope maxScope = new EtvMaxScope(smg, mEtvList, etvScope.PhysicWorkScope);

                int maxLeft = maxScope.GetMaxLeftScope();
                etvScope.MaxWorkScope.LeftCol = maxLeft;

                int maxRight = maxScope.GetMaxRightScope();
                etvScope.MaxWorkScope.RightCol = maxRight;

                //建立ETV可达的工作区域
                EtvWorkScope workScope = new EtvWorkScope(smg, mEtvList);

                int workLeft = workScope.GetLeftWorkScope();
                etvScope.WorkScope.LeftCol = workLeft > maxLeft ? workLeft : maxLeft;

                int workRight = workScope.GetRightWorkScope();
                etvScope.WorkScope.RightCol = workRight < maxRight ? workRight : maxRight;

                DicEscope.Add(smg, etvScope);
            }
            return DicEscope;
        }

        /// <summary>
        /// 设定ETV物理区域
        /// </summary>
        /// <param name="eID"></param>
        /// <returns></returns>
        private CEquip InitialEtvScope(int eID)
        {
            CEquip ETV;
            switch (eID) 
            {
                case 1:
                    ETV = new CEquip(Etv1LefPhysCol,Etv1RightPhysCol);
                    break;
                case 2:
                    ETV = new CEquip(Etv2LefPhysCol,Etv2RightPhysCol);
                    break;
                default:
                    ETV = null;
                    break;
            }
            return ETV;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEGBLL.AllocateETV
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public static class clcCommom
    {
        /// <summary>
        /// 获取指定地址列
        /// </summary>       
        public static int GetColOfAddress(string pAddress) 
        {
            try
            {
                return Convert.ToInt32(pAddress.Substring(1,2));
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}

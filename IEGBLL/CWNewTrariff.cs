using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGModel;
using System.Web.Caching;
using System.Web;

namespace IEGBLL
{
    /// <summary>
    /// 停放时间不足30min的，免费停车，前1个小时3元，以后每个小时1元，不足1个小时的以1小时算，连续停车限额是26元，收费超出限额后，以新的计费周期算
    /// </summary>
    public class CWNewTrariff
    {
        private static List<CTariff> tempCardTrariff;
       
        public CWNewTrariff() 
        {
        }

        /// <summary>
        /// 收费标准的集合
        /// </summary>
        public List<CTariff> FeeList
        {
            get
            {
                List<CTariff> trfs = (List<CTariff>)HttpRuntime.Cache["Tariffs"];
                if (trfs == null)
                {
                    trfs = CWData.myDB.LoadTariff();
                    HttpRuntime.Cache.Add("Tariffs", trfs, null, DateTime.Now.AddHours(CWData.Timeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
                return trfs;
            }
        }

        /// <summary>
        /// 获取所有的收费标准
        /// </summary>
        /// <returns></returns>
        public CTariff[] SelectTariff()
        {
            return FeeList.ToArray();
        }

        /// <summary>
        /// 加载临时卡收费标准
        /// </summary>
        private void UpdateTempCardTrariff() 
        {
            try 
            {
                if (FeeList != null) 
                {
                    tempCardTrariff = FeeList.FindAll(tr=>tr.Type==CICCard.EnmICCardType.Temp&&tr.Unit==CTariff.EnmFeeUnit.Hour);
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 临时卡收费计算
        /// </summary>
        /// <param name="Indate"></param>
        /// <param name="Outdate"></param>
        /// <returns></returns>
        public float CalculateTempFee(DateTime Indate, DateTime Outdate)
        {
            try
            {
                if (tempCardTrariff == null) 
                {
                    UpdateTempCardTrariff();
                }

                if (tempCardTrariff != null)
                {                   
                    TimeSpan span = Outdate - Indate;
                    int days = span.Days;
                    int hours = span.Hours;
                    int minutes = span.Minutes;
                    if (days == 0 && hours == 0 && minutes < 30) 
                    {
                        return 0;
                    }
                    if (minutes > 0) 
                    {
                        ++hours;
                    }
                    if (days > 0) 
                    {
                        hours += days * 24;
                    }
                    return  this.calculateHourAmount(hours);
                }
                else 
                {
                    throw new Exception("无法加载临时卡收费标准！");
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 计算小时数的收费金额
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        private float calculateHourAmount(int hours) 
        {
            CTariff firstTariff = tempCardTrariff.Find(tr=>tr.FeeType==CTariff.EnmFeeType.FirstCharge&&tr.Time==1);
            CTariff normalTariff = tempCardTrariff.Find(tr => tr.FeeType == CTariff.EnmFeeType.Charging && tr.Time == 1);           
            if (firstTariff == null || normalTariff == null) 
            {
                throw new Exception("无法加载临时卡收费标准！");
            }
            if (hours < 1) 
            {
                return firstTariff.Fee;
            }
            float tempFee = firstTariff.Fee + normalTariff.Fee * (hours - firstTariff.Time);           
            return tempFee;
        }

        /// <summary>
        /// 修改收费标准
        /// </summary>
        /// <param name="ntrf"></param>
        /// <returns></returns>
        public int UpdateTariff(CTariff ntrf)
        {
            try
            {
                CTariff trf = FeeList.Find(delegate(CTariff t) { return t.ID == ntrf.ID; });
                if (trf != null)
                {
                    CWData.myDB.UpdateTariff(ntrf);                   
                    trf.Fee = ntrf.Fee;                   
                    return 100;
                }
                else
                {
                    return 101;
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}

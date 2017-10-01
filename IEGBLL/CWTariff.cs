using System;
using System.Web;
using System.ComponentModel;
using IEGModel;
using System.Collections.Generic;
using System.Web.Caching;
using System.Configuration;

namespace IEGBLL
{
    public class CWTariff
    {
        private static List<CTariff> tcgtrfs;
        private static CTariff tlmtrf;
        private static readonly DateTime s_time = DateTime.Parse(ConfigurationManager.AppSettings["biganTime"]);
        private static readonly DateTime e_time = DateTime.Parse(ConfigurationManager.AppSettings["endTime"]);
        private static TimeSpan beginTime = new TimeSpan(0, 0, 0);
        private static TimeSpan endTime = new TimeSpan(0, 0, 0);

        public CWTariff()
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
        /// 插入新的收费标准
        /// </summary>
        /// <param name="ntrf"></param>
        /// <returns></returns>
        public int InsertTariff(CTariff ntrf)
        {
            try
            {
                FeeList.Add(ntrf);
                CWData.myDB.InsertTariff(ntrf);
                this.UpdateTempCardTariff();
                return 100;
            }
            catch (Exception e1)
            {
                throw e1;
            }
            finally
            {
                HttpRuntime.Cache.Remove("Tariffs");
            }
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
                    trf.Time = ntrf.Time;
                    trf.Fee = ntrf.Fee;
                    trf.FeeType = ntrf.FeeType;
                    trf.ISbusy = ntrf.ISbusy;
                    return 100;
                }
                else
                {
                    return 101;
                }
            }
            catch (Exception e1)
            {
                HttpRuntime.Cache.Remove("Tariffs");
                throw e1;
            }
        }
        /// <summary>
        /// 删除收费标准
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteTariff(int tid)
        {
            try
            {
                CTariff trf = FeeList.Find(delegate(CTariff t) { return t.ID == tid; });
                if (trf != null)
                {
                    CWData.myDB.DeleteTariff(tid);
                    FeeList.Remove(trf);
                    this.UpdateTempCardTariff();
                    return 100;
                }
                else
                {
                    return 101;
                }
            }
            catch (Exception e1)
            {
                throw e1;
            }
            finally
            {
                HttpRuntime.Cache.Remove("Tariffs");
            }
        }

        /// <summary>
        /// 查询时间段内的收费
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public float CalculateCardFee(TimeSpan ts)
        {
            if (tcgtrfs == null || tlmtrf == null)
            {
                this.UpdateTempCardTariff();
            }

            if (tcgtrfs != null && tlmtrf != null && tcgtrfs.Count > 0)
            {
                int days = ts.Days;
                float hours = 0;

                if (ts.Hours == 0)
                {
                    hours = ts.Minutes / 60;
                }
                else
                {
                    hours = ts.Hours + (ts.Minutes > 0 ? 1 : 0);
                }



                float dfee = days * tlmtrf.Fee;
                float hfee = 0;

                foreach (CTariff trf in tcgtrfs)
                {
                    if (trf.Time >= hours)
                    {
                        hfee = trf.Fee * hours;
                        break;
                    }
                }


                return dfee + (hfee < tlmtrf.Fee ? hfee : tlmtrf.Fee);
            }
            else
            {
                throw new Exception("无法读取收费标准，或者无临时收费标准");
            }
        }

        /// <summary>
        /// 临时卡收费
        /// </summary>
        /// <param name="Indate"></param>
        /// <param name="Outdate"></param>
        /// <returns>返回收费金额</returns>
        public float CalculateTempFee(DateTime Indate, DateTime Outdate)
        {
            DateTime i_time = new DateTime(Indate.Year, Indate.Month, Indate.Day, s_time.Hour, s_time.Minute, s_time.Second);
            DateTime i_time1 = new DateTime(Outdate.Year, Outdate.Month, Outdate.Day, s_time.Hour, s_time.Minute, s_time.Second);
            DateTime o_time = new DateTime(Outdate.Year, Outdate.Month, Outdate.Day, e_time.Hour, e_time.Minute, e_time.Second);
            DateTime o_time1 = new DateTime(Indate.Year, Indate.Month, Indate.Day, e_time.Hour, e_time.Minute, e_time.Second);

            if (tcgtrfs == null || tlmtrf == null)
            {
                this.UpdateTempCardTariff();
            }
            if (tcgtrfs != null && tlmtrf != null && tcgtrfs.Count > 0)
            {
                TimeSpan ts = Outdate - Indate;
                int days = ts.Days;
                TimeSpan time_Leisure = new TimeSpan(0, 0, 0, 0);
                TimeSpan time_Busy = new TimeSpan(0, 0, 0, 0);
                float hours = ts.Hours + (ts.Minutes > 0 ? 1 : 0);
                if ((ts.Hours == 0) && (ts.Minutes < 30))
                {
                    return 0;
                }
                float dfee = days * tlmtrf.Fee;
                if (Indate < i_time)
                {
                    if (Outdate >= o_time)
                    {
                        time_Leisure = (i_time - Indate) + (Outdate - o_time);
                        time_Busy = o_time - i_time;
                    }
                    else if (Outdate >= i_time1 && Outdate < o_time)
                    {
                        time_Leisure = i_time1 - Indate;
                        time_Busy = Outdate - i_time1;
                    }
                    else if (Outdate < i_time1)
                    {
                        time_Leisure = Outdate - Indate;
                    }
                }
                else if (Indate >= i_time && Indate < o_time1)
                {
                    if (Outdate >= o_time)
                    {
                        time_Leisure = Outdate - o_time;
                        time_Busy = o_time - Indate;
                    }
                    else if (Outdate >= i_time1 && Outdate < o_time)
                    {
                        time_Busy = Outdate - Indate;
                    }
                    else if (Outdate < i_time1)
                    {
                        time_Busy = o_time1 - Indate;
                        time_Leisure = Outdate - o_time1;
                    }
                }
                else if (Indate >= o_time1)
                {
                    if (Outdate >= o_time)
                    {
                        time_Leisure = Outdate - Indate;
                    }
                    else if (Outdate >= i_time1 && Outdate < o_time)
                    {
                        time_Busy = Outdate - i_time1;
                        time_Leisure = i_time1 - Indate;
                    }
                    else if (Outdate < i_time1)
                    {
                        time_Leisure = Outdate - Indate;
                    }

                }
                float fee_Busy = 0, fee_Leisure = 0;
                foreach (CTariff trf in tcgtrfs)
                {
                    if (trf.Time == 1)
                    {
                        if (trf.ISbusy)
                        {
                            fee_Busy = trf.Fee;
                        }
                        else
                        {
                            fee_Leisure = trf.Fee;
                        }
                    }
                }
                float busyHourFee = fee_Busy * time_Busy.Hours;
                float leisureHourFee = fee_Leisure * time_Leisure.Hours;
                //计算分钟的费用
                float minuteFee = 0;

                if (time_Leisure.Minutes > 0 && time_Busy.Minutes > 0)
                {
                    if (time_Leisure.Minutes > time_Busy.Minutes)
                    {
                        minuteFee = fee_Leisure;
                    }
                    else
                    {
                        minuteFee = fee_Busy;
                    }
                }
                else if (time_Leisure.Minutes > 0 && time_Busy.Minutes == 0)
                {
                    minuteFee = fee_Leisure;
                }
                else if (time_Busy.Minutes > 0 && time_Leisure.Minutes == 0)
                {
                    minuteFee = fee_Busy;
                }
                //计算小时的费用
                float hfee = busyHourFee + leisureHourFee + minuteFee;

                return dfee + (hfee < tlmtrf.Fee ? hfee : tlmtrf.Fee);
            }
            else
            {
                throw new Exception("无法读取收费标准，或者无临时收费标准");
            }
        }

        /// <summary>
        /// 重新加载临时卡收费标准
        /// </summary>
        private void UpdateTempCardTariff()
        {
            tcgtrfs = FeeList.FindAll( delegate(CTariff t)
                {
                    return t.Type == CICCard.EnmICCardType.Temp && t.FeeType == CTariff.EnmFeeType.Charging;   //EnmFeeType:1-Charging 计费 2-限额
                }
                );
            tcgtrfs.Sort();
            tlmtrf = FeeList.Find( delegate(CTariff t)
                {
                    return t.Type == CICCard.EnmICCardType.Temp && t.FeeType == CTariff.EnmFeeType.Limited && t.Time == 24;   //限额
                }
                );
        }
    }
}

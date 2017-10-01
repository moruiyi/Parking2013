using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using IEGModel;

namespace IEGBLL
{
    public class CWData
    {
        private string OleDbString;  //数据连接字符串
        private static readonly CWData moDB = new CWData();  //注意定义为静态只读变量
        public static CWData myDB 
        {
            get 
            {
                return moDB;
            }
        }
        private CWData() 
        {            
            OleDbString = ConfigurationManager.ConnectionStrings["IEGDB"].ConnectionString;
        }

        public static readonly double Timeout = double.Parse(ConfigurationManager.AppSettings["CacheDuration"]);
        public static readonly int MaxGetCarCount = int.Parse(ConfigurationManager.AppSettings["MaxGetCarNumber"]);
        public static readonly bool ChargeEnable = Convert.ToInt16(ConfigurationManager.AppSettings["ChargeEnable"]) == 0 ? false : true;

        public List<CSMG> LoadSMGs() 
        {
            List<CSMG> smgs = new List<CSMG>();
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {                
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select id,code,avail,mode,type,htype,wh,mtskID,address,taskID,nextTask,layer,region from ieg_smg order by id";
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        smgs.Add(
                            new CSMG(dr.GetInt32(0),
                                dr.GetString(1),
                                dr.GetInt32(2) == 0 ? false : true,
                                (CSMG.EnmModel)dr.GetInt32(3),
                                (CSMG.EnmSMGType)dr.GetInt32(4),
                                (CSMG.EnmHallType)dr.GetInt32(5),
                                dr.GetInt32(6),
                                dr.IsDBNull(7) ? 0 : dr.GetInt32(7),
                                dr.GetString(8),
                                dr.GetInt32(9),
                                dr.IsDBNull(10) ? 0 : dr.GetInt32(10),
                                 dr.IsDBNull(11) ? 0 : dr.GetInt32(11),
                                dr.GetInt32(12))
                            );
                    }
                }
                catch (Exception ex)
                {
                    new CWSException(ex);
                }                
            }
            return smgs;
        }
        //加载所有用户卡信息
        public List<CICCard> LoadICCards()
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                List<CICCard> ciccds = new List<CICCard>();
                try 
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select ccode,pcode,type,status,lossdtime,duedtime,oweddays,uid,pnmb,cdtime,addrs,dispdtime from ieg_iccd";
                    conn.Open();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) 
                    {
                        ciccds.Add(
                            new CICCard(dr.GetString(0),
                                dr.GetString(1),
                                (CICCard.EnmICCardType)dr.GetInt32(2),
                                (CICCard.EnmICCardStatus)dr.GetInt32(3),
                                dr.IsDBNull(4)?CObject.DefDatetime:dr.GetDateTime(4),
                                dr.IsDBNull(5)?CObject.DefDatetime:dr.GetDateTime(5),
                                dr.IsDBNull(6)?0:dr.GetInt32(6),
                                dr.IsDBNull(7)?0:dr.GetInt32(7),
                                dr.IsDBNull(8)?"":dr.GetString(8),
                                dr.IsDBNull(9)?CObject.DefDatetime:dr.GetDateTime(9),
                                dr.IsDBNull(10)?"":dr.GetString(10),
                                dr.IsDBNull(11)?CObject.DefDatetime:dr.GetDateTime(11))
                            );
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
                }

                return ciccds;
            }
        }
        //加载报警变量
        public List<CErrorCode> LoadErrorCodes(int scNo)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                List<CErrorCode> errcde = new List<CErrorCode>();
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select adrs,color,type from ieg_stat where type=? order by adrs";
                    cmd.Parameters.Add("type",OleDbType.TinyInt).Value = scNo;
                    conn.Open();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) 
                    {
                        errcde.Add(
                            new CErrorCode(dr.GetInt16(0),
                                dr.GetByte(1),
                                dr.GetByte(2))
                            );
                    }
                    cmd.Parameters.Clear();
                    return errcde;
                }
                catch (Exception ex) 
                {
                    throw ex;
                }                
            }
        }
        //加载主作业
        public List<CMasterTask> LoadMasterTasks() 
        {
            List<CMasterTask> mtsksList = new List<CMasterTask>();
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                try 
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select id,type,iscpl,iccd,hid,wid from ieg_mtsk where iscpl=? order by id";
                    cmd.Parameters.Add("iscpl",OleDbType.Integer).Value=0;
                    conn.Open();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        mtsksList.Add(
                            new CMasterTask(dr.GetInt32(0),
                                (CMasterTask.EnmMasterTaskType)dr.GetInt32(1),
                                dr.GetInt32(2)==0?false:true,
                                dr.GetString(3),
                                dr.GetInt32(4),
                                dr.GetInt32(5)
                                )
                            );
                    }
                    cmd.Parameters.Clear();
                    dr.Close();
                    //建立初期mLstTasks=new List<CTask>(),现将子作业加入到主作业中，
                    foreach (CMasterTask mtsk in mtsksList) 
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select id,dist,flct,tlct,type,status,ccode,hid,csize,eqp,smgtype from ieg_ctsk where mid=? and status<>? order by id";
                        cmd.Parameters.Add("mid",OleDbType.Integer).Value=mtsk.ID;
                        cmd.Parameters.Add("status",OleDbType.Integer).Value=CTask.EnmTaskStatus.Finished;
                        dr = cmd.ExecuteReader();
                        while (dr.Read()) 
                        {
                            CTask tsk = new CTask(
                                dr.GetInt32(0),
                                dr.IsDBNull(1) ? 0 : dr.GetInt32(1),
                                dr.IsDBNull(2) ? "" : dr.GetString(2),
                                dr.IsDBNull(3) ? "" : dr.GetString(3),
                                (CTask.EnmTaskType)dr.GetInt32(4),
                                (CTask.EnmTaskStatus)dr.GetInt32(5),
                                dr.IsDBNull(6) ? "" : dr.GetString(6),
                                dr.IsDBNull(7) ? 0 : dr.GetInt32(7),
                                dr.IsDBNull(8) ? "" : dr.GetString(8),
                                dr.IsDBNull(9) ? 0 : dr.GetInt32(9),
                                dr.IsDBNull(10) ? 0 : dr.GetInt32(10)
                                );
                            tsk.MID = mtsk.ID;
                            mtsk.AddTask(tsk);
                        }
                        cmd.Parameters.Clear();
                        dr.Close();
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }

            return mtsksList;
        }

        //加载车位数据信息
        public List<CLocation> LoadLocations() 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))             
            {
                List<CLocation> lcts = new List<CLocation>();
                try 
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select address,line_plc,layer_plc,list_plc,type,status,wh,iccode,indtime,dist,idx,csize,isqx,region,pri,size from ieg_lctn  order by id";
                    conn.Open();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lcts.Add(new CLocation(dr.GetString(0),
                            dr.GetInt32(1),
                            dr.GetInt32(2),
                            dr.GetInt32(3),
                            (CLocation.EnmLocationType)dr.GetInt32(4),
                            (CLocation.EnmLocationStatus)dr.GetInt32(5),
                            dr.GetInt32(6),
                            dr.IsDBNull(7) ? "" : dr.GetString(7),
                            dr.IsDBNull(8) ? CObject.DefDatetime : dr.GetDateTime(8),
                            dr.IsDBNull(9) ? 0 : dr.GetInt32(9),
                            dr.GetInt32(10),
                            dr.IsDBNull(11) ? "" : dr.GetString(11),
                            dr.IsDBNull(12) ? 0 : dr.GetInt32(12),
                            dr.IsDBNull(13) ? 0 : dr.GetInt32(13),
                            dr.IsDBNull(14) ? 0 : dr.GetInt32(14),
                            dr.GetString(15))
                            );                        
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
                return lcts;
            }
        }
        //入库时刷卡时更新主作业及其相应子作业信息
        public void UpdateMtskAndCtskInfo(CTask htsk)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                //同时更新，使用事务处理
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_mtsk set iccd=? where id=?";
                    cmd.Parameters.Add("iccd",OleDbType.VarChar).Value=htsk.ICCardCode;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = htsk.MID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //入库初期主作业下只存在一个车厅的子作业
                    cmd.CommandText = "update ieg_ctsk set ccode=?,status=? where id=?";
                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = htsk.ICCardCode;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = htsk.Status;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = htsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    //提交事务
                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message ="1-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e1) 
                    {
                        message += "    执行事务回滚异常： " + e1.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //添加主作业及子作业，同时更新车位信息
        public void InsertAMasterTaskAndLct(CMasterTask mtsk,CLocation FrLct,CLocation ToLct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into ieg_mtsk(type,iscpl,iccd,hid,wid) values(?,?,?,?,?)";
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = mtsk.Type;
                    cmd.Parameters.Add("iscpl",OleDbType.Integer).Value=mtsk.IsCompleted?1:0;
                    cmd.Parameters.Add("iccd",OleDbType.VarChar).Value=mtsk.ICCardCode;
                    cmd.Parameters.Add("hid", OleDbType.Integer).Value = mtsk.HID;
                    cmd.Parameters.Add("wid",OleDbType.Integer).Value = mtsk.WID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "select @@identity";
                    mtsk.ID = int.Parse(cmd.ExecuteScalar().ToString());   //绑定ID

                    for (int i = 0; i < mtsk.TaskCount; i++)
                    {
                        CTask tsk = mtsk.Tasks[i];
                        cmd.CommandText = "insert into ieg_ctsk(dist,flct,tlct,type,ccode,cdtime,status,hid,mid,csize,eqp,smgtype) values(?,?,?,?,?,?,?,?,?,?,?,?)";
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = tsk.Distance;
                        cmd.Parameters.Add("flct", OleDbType.VarChar).Value = tsk.FromLctAdrs;
                        cmd.Parameters.Add("tlct", OleDbType.VarChar).Value = tsk.ToLctAdrs;
                        cmd.Parameters.Add("type", OleDbType.Integer).Value = tsk.Type;
                        cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = tsk.ICCardCode;
                        cmd.Parameters.Add("cdtime", OleDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = tsk.Status;
                        cmd.Parameters.Add("hid", OleDbType.Integer).Value = tsk.HID;
                        cmd.Parameters.Add("mid", OleDbType.Integer).Value = mtsk.ID;
                        cmd.Parameters.Add("csize", OleDbType.VarChar).Value = tsk.CarSize;
                        cmd.Parameters.Add("eqp", OleDbType.Integer).Value = tsk.SMG;
                        cmd.Parameters.Add("smgtype",OleDbType.Integer).Value = tsk.SMGType;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = "select @@identity";
                        tsk.ID = Convert.ToInt32(cmd.ExecuteScalar().ToString());   //绑定ID
                        tsk.MID = mtsk.ID;      //绑定主作业ID
                    }

                    if (FrLct != null)  //更新出库车位,通用的
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,indtime=?,dist=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = FrLct.Status;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = FrLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = FrLct.Distance;
                        cmd.Parameters.Add("address", OleDbType.VarChar).Value = FrLct.Address;
                        int pam1= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam1 > -1)
                        {
                            string msg = "InsertAMasterTaskAndLct更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    if (ToLct != null) 
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,indtime=?,dist=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = ToLct.Status;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = ToLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = ToLct.Distance;
                        cmd.Parameters.Add("address", OleDbType.VarChar).Value = ToLct.Address;
                        int pam2= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam2 > -1)
                        {
                            string msg = "InsertAMasterTaskAndLct更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();                  
                }
                catch (Exception ex) 
                {
                    string message ="2-"+ ex.ToString();
                    try 
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
       
        /// <summary>
        /// 更新设备的可用性
        /// </summary>
        /// <param name="smg"></param>
        /// <param name="avail"></param>
        /// <param name="emodel"></param>
        public void UpdateSMGStat(int smg,bool avail,CSMG.EnmModel emodel) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_smg set avail=?,mode=? where id=?";
                    cmd.Parameters.Add("avail",OleDbType.Integer).Value = avail ? 1 : 0;
                    cmd.Parameters.Add("mode",OleDbType.Integer).Value = emodel;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = smg;                    
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch(Exception ex)
                {
                    string message ="3-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //插入主作业及相应的子作业
        public void InsertAMasterTask(CMasterTask mtsk)
        {
             using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into ieg_mtsk(type,iscpl,iccd,hid,wid) values(?,?,?,?,?)";
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = mtsk.Type;
                    cmd.Parameters.Add("iscpl",OleDbType.Integer).Value=mtsk.IsCompleted?1:0;
                    cmd.Parameters.Add("iccd",OleDbType.VarChar).Value=mtsk.ICCardCode;
                    cmd.Parameters.Add("hid",OleDbType.Integer).Value=mtsk.HID;
                    cmd.Parameters.Add("wid",OleDbType.Integer).Value = mtsk.WID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "select @@identity";
                    mtsk.ID = int.Parse(cmd.ExecuteScalar().ToString());   //绑定ID

                    foreach (CTask tsk in mtsk.Tasks)
                    {
                        cmd.CommandText = "insert into ieg_ctsk(dist,flct,tlct,type,ccode,cdtime,status,hid,mid,csize,eqp,smgtype) values(?,?,?,?,?,?,?,?,?,?,?,?)";
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = tsk.Distance;
                        cmd.Parameters.Add("flct", OleDbType.VarChar).Value = tsk.FromLctAdrs;
                        cmd.Parameters.Add("tlct",OleDbType.VarChar).Value = tsk.ToLctAdrs;
                        cmd.Parameters.Add("type",OleDbType.Integer).Value = tsk.Type;
                        cmd.Parameters.Add("ccode",OleDbType.VarChar).Value = tsk.ICCardCode;
                        cmd.Parameters.Add("cdtime",OleDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = tsk.Status;
                        cmd.Parameters.Add("hid",OleDbType.Integer).Value = tsk.HID;
                        cmd.Parameters.Add("mid",OleDbType.Integer).Value = mtsk.ID;
                        cmd.Parameters.Add("csize",OleDbType.VarChar).Value = tsk.CarSize;
                        cmd.Parameters.Add("eqp",OleDbType.Integer).Value = tsk.SMG;
                        cmd.Parameters.Add("smgtype",OleDbType.Integer).Value = tsk.SMGType;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        cmd.CommandText = "select @@identity";
                        tsk.ID = int.Parse(cmd.ExecuteScalar().ToString());   //绑定ID
                        tsk.MID = mtsk.ID;      //绑定ID
                    } 
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message ="4-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
      
        /// <summary>
        /// 更新设备的作业状态
        /// </summary>
        /// <param name="smg"></param>
        public void UpdateSMGTaskStat(CSMG smg)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.CommandText = "update ieg_smg set mtskID=?,taskID=?,nextTask=? where id=?";
                    cmd.Parameters.Add("mtskID",OleDbType.Integer).Value = smg.MTskID;
                    cmd.Parameters.Add("taskID", OleDbType.Integer).Value = smg.nIsWorking;
                    cmd.Parameters.Add("nextTask", OleDbType.Integer).Value = smg.NextTaskId;

                    cmd.Parameters.Add("id", OleDbType.Integer).Value = smg.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

       //更新子作业状态
        public void UpdateCTaskStatus(CTask.EnmTaskStatus status,int tskID) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                    cmd.Parameters.Add("status",OleDbType.Integer).Value = status;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = tskID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message ="6-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //删除中途退出的作业
        public void DeleteMasterTaskAndCTask(int mid)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                   
                    cmd.CommandText = "delete from ieg_ctsk where mid=?";
                    cmd.Parameters.Add("mid",OleDbType.Integer).Value = mid;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();                       
                    
                    cmd.CommandText = "delete from ieg_mtsk where id=?";
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = mid;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message ="7-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //更新子作业状态，及车位信息表
        public void UpdateCTaskAndLct(int tskID,CTask.EnmTaskStatus stat,CLocation FrLct,CLocation ToLct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = tskID;
                    cmd.Parameters.Add("status",OleDbType.Integer).Value=stat;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (FrLct != null)
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = FrLct.Status;
                        cmd.Parameters.Add("iccode",OleDbType.VarWChar).Value="";
                        cmd.Parameters.Add("indtime",OleDbType.Date).Value = CObject.DefDatetime;
                        cmd.Parameters.Add("dist",OleDbType.Integer).Value = 0;
                        cmd.Parameters.Add("csize",OleDbType.VarWChar).Value="";
                        cmd.Parameters.Add("address",OleDbType.VarWChar).Value = FrLct.Address;
                        int pam1= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam1 > -1)
                        {
                            string msg = "UpdateCTaskAndLct更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }
                    if (ToLct != null) 
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = ToLct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = "";
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = CObject.DefDatetime;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = 0;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = "";
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = ToLct.Address;
                        int pam2= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam2 > -1)
                        {
                            string msg = "UpdateCTaskAndLct更新车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message ="8-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        /// <summary>
        /// TV装卸完成时更新作业状态及车位信息
        /// </summary> 
        public void UpdateCTaskAndLctOfMTsk(CMasterTask mtsk, CLocation FrLct, CLocation ToLct)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;

                    foreach (CTask tsk in mtsk.Tasks)
                    {
                        cmd.CommandText = "update ieg_ctsk set status=?,dist=? where id=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = tsk.Status;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = tsk.Distance;
                        cmd.Parameters.Add("id", OleDbType.Integer).Value = tsk.ID;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    if (FrLct != null)
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = FrLct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = FrLct.ICCardCode;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = FrLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = FrLct.Distance;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = FrLct.CarSize;
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = FrLct.Address;
                        int pam1= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam1 > -1)
                        {
                            string msg = "UpdateCTaskAndLctOfMTsk更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }
                    if (ToLct != null)
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = ToLct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = ToLct.ICCardCode;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = ToLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = ToLct.Distance;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = ToLct.CarSize;
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = ToLct.Address;
                        int pam2= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam2 > -1)
                        {
                            string msg = "UpdateCTaskAndLctOfMTsk更新车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    string message = "12-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //更新主作业状态
        public void UpdateMTaskCompleted(CMasterTask mtsk)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_mtsk set iscpl=? where id=?";
                    cmd.Parameters.Add("iscpl",OleDbType.Integer).Value = mtsk.IsCompleted ? 1 : 0;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = mtsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message ="9-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //完成主作业及子作业
        public void CompleteMasterTask(CMasterTask mtsk) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_mtsk set iscpl=? where id=?";
                    cmd.Parameters.Add("iscpl", OleDbType.Integer).Value = mtsk.IsCompleted ? 1 : 0;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = mtsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    for (int i = 0; i < mtsk.TaskCount; i++) 
                    {
                        CTask tsk=mtsk.Tasks[i];
                        cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = CTask.EnmTaskStatus.Finished;
                        cmd.Parameters.Add("id",OleDbType.Integer).Value = tsk.ID;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message = "函数CompleteMasterTask异常：" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        /// <summary>
        /// 入库更新车厅子作业的状态，写入车辆外形及轴距
        /// </summary>       
        public void UpdateACTask(int tid,int dist,CTask.EnmTaskStatus stat,string checkCode)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_ctsk set status=?,csize=?,dist=? where id=?";
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = tid;
                    cmd.Parameters.Add("dist",OleDbType.Integer).Value = dist;
                    cmd.Parameters.Add("csize",OleDbType.VarChar).Value = checkCode;
                    cmd.Parameters.Add("status",OleDbType.Integer).Value=stat;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message ="10-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        /// <summary>
        /// 用于入库时建立ETV作业，更新车厅作业及入库车位信息
        /// </summary> 
        public void UpdateCTaskAndLct(CTask htsk,CLocation ToLct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    //更新车厅作业
                    cmd.CommandText = "Update ieg_ctsk set dist=?,tlct=?,status=?,csize=? where id=?";
                    cmd.Parameters.Add("dist",OleDbType.Integer).Value = htsk.Distance;
                    cmd.Parameters.Add("tlct",OleDbType.VarWChar).Value=htsk.ToLctAdrs;
                    cmd.Parameters.Add("status",OleDbType.Integer).Value = htsk.Status;
                    cmd.Parameters.Add("csize",OleDbType.VarWChar).Value = htsk.CarSize;

                    cmd.Parameters.Add("id",OleDbType.Integer).Value = htsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                  
                    //更新车位信息                   
                    if (ToLct != null) 
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = ToLct.Status;
                        cmd.Parameters.Add("iccode",OleDbType.VarWChar).Value = ToLct.ICCardCode;
                        cmd.Parameters.Add("indtime",OleDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("dist",OleDbType.Integer).Value = ToLct.Distance;
                        cmd.Parameters.Add("csize",OleDbType.VarWChar).Value = ToLct.CarSize;

                        cmd.Parameters.Add("address",OleDbType.VarWChar).Value = ToLct.Address;
                        int pam= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam > -1)
                        {
                            string msg = "UpdateCTaskAndLct更新车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    string message ="11-"+ ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //向主作业中添加子作业
        public void InsertCTasks(CMasterTask mtsk, List<CTask> taskList) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                   
                    //插入ETV作业
                    for (int i = 0; i < mtsk.TaskCount; i++)
                    {
                        CTask etsk = mtsk.Tasks[i];
                        bool flag = false;
                        foreach (CTask tsk in taskList) 
                        {
                            if (tsk == etsk)       //如果这样，无法完成操作，则以作业的类型作为判断条件
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            cmd.CommandText = "insert into ieg_ctsk(dist,flct,tlct,type,ccode,cdtime,status,hid,mid,csize,eqp,smgtype) values(?,?,?,?,?,?,?,?,?,?,?,?)";
                            cmd.Parameters.Add("dist", OleDbType.Integer).Value = etsk.Distance;
                            cmd.Parameters.Add("flct", OleDbType.VarChar).Value = etsk.FromLctAdrs;
                            cmd.Parameters.Add("tlct", OleDbType.VarChar).Value = etsk.ToLctAdrs;
                            cmd.Parameters.Add("type", OleDbType.Integer).Value = etsk.Type;
                            cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = etsk.ICCardCode;
                            cmd.Parameters.Add("cdtime", OleDbType.Date).Value = DateTime.Now;
                            cmd.Parameters.Add("status", OleDbType.Integer).Value = etsk.Status;
                            cmd.Parameters.Add("hid", OleDbType.Integer).Value = etsk.HID;
                            cmd.Parameters.Add("mid", OleDbType.Integer).Value = etsk.MID;
                            cmd.Parameters.Add("csize", OleDbType.VarChar).Value = etsk.CarSize;
                            cmd.Parameters.Add("eqp", OleDbType.Integer).Value = etsk.SMG;
                            cmd.Parameters.Add("smgtype",OleDbType.Integer).Value = etsk.SMGType;
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            cmd.CommandText = "select @@identity";
                            etsk.ID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        }
                    }
                   
                    cmd.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    string message = "28-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //插入子作业
        public void InsertCTask(CTask etsk) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "insert into ieg_ctsk(dist,flct,tlct,type,ccode,cdtime,status,hid,mid,csize,eqp,smgtype) values(?,?,?,?,?,?,?,?,?,?,?,?)";
                    cmd.Parameters.Add("dist", OleDbType.Integer).Value = etsk.Distance;
                    cmd.Parameters.Add("flct", OleDbType.VarChar).Value = etsk.FromLctAdrs;
                    cmd.Parameters.Add("tlct", OleDbType.VarChar).Value = etsk.ToLctAdrs;
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = etsk.Type;
                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = etsk.ICCardCode;
                    cmd.Parameters.Add("cdtime", OleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = etsk.Status;
                    cmd.Parameters.Add("hid", OleDbType.Integer).Value = etsk.HID;
                    cmd.Parameters.Add("mid", OleDbType.Integer).Value = etsk.MID;
                    cmd.Parameters.Add("csize", OleDbType.VarChar).Value = etsk.CarSize;
                    cmd.Parameters.Add("eqp", OleDbType.Integer).Value = etsk.SMG;
                    cmd.Parameters.Add("smgtype",OleDbType.Integer).Value = etsk.SMGType;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "select @@identity";
                    etsk.ID = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message = "30-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
     
        //车辆卸载到车厅后更新作业状态及车厅地址车位状态
        public void UpdateCTaskAndLoctation(int hid,CLocation lct,CTask.EnmTaskStatus status) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                    cmd.Parameters.Add("status",OleDbType.Integer).Value = status;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = hid;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (lct != null)   //更新车厅车位状态
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = lct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = lct.ICCardCode;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = lct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = lct.Distance;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = lct.CarSize;
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = lct.Address;
                        int pam= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam > -1)
                        {
                            string msg = "UpdateCTaskAndLoctation更新车位：" + lct.Address + " 状态：" +lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message = "13-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //查询并返回所有管理员信息
        public List<COperator> LoadOperators() 
        {           
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                List<COperator> oprs = new List<COperator>();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select code,name,password,authtype from ieg_oprt";
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) 
                    {
                        oprs.Add(new COperator(dr.GetString(0),
                            dr.IsDBNull(1)?"":dr.GetString(1),
                            dr.GetString(2),
                            (COperator.EnmOperatorType)dr.GetInt32(3)));
                    }
                    return oprs;
                }
                catch (Exception ex) 
                {
                    new Exception("数据库操作LoadOperators异常：" + ex.ToString());
                }
                return null;
            }
        }
        //依设备号返回报警描述信息
        public CErrorCode[] LoadErrCodesDesc(int btype) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                List<CErrorCode> errCodes = new List<CErrorCode>();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    conn.Open();
                    cmd.CommandText = "select adrs,descp from ieg_stat where isable=? and type=? order by adrs";
                    cmd.Parameters.Add("isable",OleDbType.Integer).Value = 0;
                    cmd.Parameters.Add("type",OleDbType.TinyInt).Value = btype;
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) 
                    {
                        errCodes.Add(new CErrorCode(dr.GetInt16(0), dr.GetString(1)));
                    }
                    cmd.Parameters.Clear();
                    dr.Close();

                    return errCodes.ToArray();
                }
                catch (Exception ex) 
                {
                    throw new Exception("LoadErrCodesDesc函数异常："+ex.ToString());
                }                
            }
        }
        //加载所有报警信息--color=1
        public CErrorCode[] LoadErrorsDesc()
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                List<CErrorCode> errCodes = new List<CErrorCode>();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    conn.Open();
                    cmd.CommandText = "select adrs,descp from ieg_stat where isable=? and color=?  order by adrs";
                    cmd.Parameters.Add("isable", OleDbType.Integer).Value = 0;
                    cmd.Parameters.Add("color", OleDbType.TinyInt).Value = 1;
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        errCodes.Add(new CErrorCode(dr.GetInt16(0), dr.GetString(1)));
                    }
                    cmd.Parameters.Clear();
                    dr.Close();

                    return errCodes.ToArray();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }
        //手动完成作业时，更新主作业及车位信息
        public void CompleteMasterTaskAndLctn(CMasterTask mtsk,CLocation FrLct, CLocation ToLct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                OleDbTransaction transaction = null;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandText = "update ieg_mtsk set iscpl=? where id=?";
                    cmd.Parameters.Add("iscpl", OleDbType.Integer).Value = mtsk.IsCompleted ? 1 : 0;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = mtsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    for (int i = 0; i < mtsk.Tasks.Length; i++) 
                    {
                        CTask tsk = mtsk.Tasks[i];
                        cmd.CommandText = "update ieg_ctsk set status=?,dist=? where id=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = tsk.Status;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = tsk.Distance;
                        cmd.Parameters.Add("id", OleDbType.Integer).Value = tsk.ID;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    if (FrLct != null)
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = FrLct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = FrLct.ICCardCode;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = FrLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = FrLct.Distance;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = FrLct.CarSize;
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = FrLct.Address;
                        int pam1= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam1 > -1)
                        {
                            string msg = "CompleteMasterTaskAndLctn更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }
                    if (ToLct != null)
                    {
                        cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = ToLct.Status;
                        cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = ToLct.ICCardCode;
                        cmd.Parameters.Add("indtime", OleDbType.Date).Value = ToLct.InDate;
                        cmd.Parameters.Add("dist", OleDbType.Integer).Value = ToLct.Distance;
                        cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = ToLct.CarSize;
                        cmd.Parameters.Add("address", OleDbType.VarWChar).Value = ToLct.Address;
                        int pam2= cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        if (pam2 > -1)
                        {
                            string msg = "CompleteMasterTaskAndLctn更新车位：" + ToLct.Address + " 状态：" + ToLct.Status.ToString() + " 卡号：" + ToLct.ICCardCode;
                            new CWSException(msg, 1);
                        }
                    }

                    cmd.Transaction.Commit();                    
                }
                catch (Exception ex)
                {
                    string message = "14-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //车位出库、入库
        public void ManUpdateLocation(CLocation lct)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_lctn set iccode=?,status=?,indtime=?,dist=?,csize=? where address=?";
                    cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = lct.ICCardCode;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = lct.Status;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = lct.InDate;
                    cmd.Parameters.Add("dist", OleDbType.Integer).Value = lct.Distance;
                    cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = lct.CarSize;

                    cmd.Parameters.Add("address", OleDbType.VarWChar).Value = lct.Address;
                    int pam= cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (pam > -1)
                    {
                        string msg = "ManUpdateLocation更新车位：" + lct.Address + " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                        new CWSException(msg, 1);
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }

        //车位禁用
        public void ManDisableLocation(CLocation lct, CLocation.EnmLocationType type)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                connection.Open();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "update ieg_lctn set type=? where address=?";
                cmd.Parameters.Add("type", OleDbType.Integer).Value = type;
                cmd.Parameters.Add("address", OleDbType.VarWChar).Value = lct.Address;
                int pam= cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                if (pam > -1)
                {
                    string msg = " ManDisableLocation更新车位：" + lct.Address +" 类型："+lct.Type.ToString()+ " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                    new CWSException(msg, 1);
                }
            }
        }

        //车位挪移
        public void ManTransportLocation(CLocation flct, CLocation tlct)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.CommandText = "update ieg_lctn set iccode=?,status=?,indtime=?,csize=?,dist=? where address=?";
                    cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = flct.ICCardCode;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = flct.Status;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = flct.InDate;
                    cmd.Parameters.Add("csize",OleDbType.VarWChar).Value = flct.CarSize;
                    cmd.Parameters.Add("dist", OleDbType.Integer).Value = flct.Distance;

                    cmd.Parameters.Add("address", OleDbType.VarWChar).Value = flct.Address;
                    int pam1= cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    if (pam1 > -1)
                    {
                        string msg = "ManTransportLocation更新车位：" + flct.Address + " 状态：" + flct.Status.ToString() + " 卡号：" + flct.ICCardCode;
                        new CWSException(msg, 1);
                    }

                    cmd.CommandText = "update ieg_lctn set iccode=?,status=?,indtime=?,csize=?,dist=? where address=?";
                    cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = tlct.ICCardCode;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = tlct.Status;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = tlct.InDate;
                    cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = tlct.CarSize;
                    cmd.Parameters.Add("dist", OleDbType.Integer).Value = tlct.Distance;

                    cmd.Parameters.Add("address", OleDbType.VarWChar).Value = tlct.Address;
                    int pam2= cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    if (pam2 > -1)
                    {
                        string msg = "ManTransportLocation更新车位：" + tlct.Address + " 状态：" + tlct.Status.ToString() + " 卡号：" + tlct.ICCardCode;
                        new CWSException(msg, 1);
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    string sErrors = e.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e1)
                    {
                        sErrors += " Error 2:" + e1.ToString();
                    }
                    throw new Exception(sErrors);

                }
            }
        }

        //更新车厅的进出车模式
        public void UpdateHallType(int hallID, CSMG.EnmHallType htype) 
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.CommandText = "update ieg_smg set htype=? where id=?";
                    cmd.Parameters.Add("htype",OleDbType.Integer).Value = htype;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = hallID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    string sErrors = e.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e1)
                    {
                        sErrors += " Error 3:" + e1.ToString();
                    }
                    throw new Exception(sErrors);

                }
            }
        }

        //加载顾客信息
        public List<CCustomer> LoadCustomers() 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                List<CCustomer> customers = new List<CCustomer>();
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select id,uname,uadrs,udtel,umobile from ieg_user order by id";
                    conn.Open();
                    OleDbDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        customers.Add(new CCustomer(dr.GetInt32(0),
                            dr.IsDBNull(1) ? "" : dr.GetString(1),
                            dr.IsDBNull(2) ? "" : dr.GetString(2),
                            dr.IsDBNull(3) ? "" : dr.GetString(3),
                            dr.IsDBNull(4) ? "" : dr.GetString(4)));
                    }
                    return customers;
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //更新顾客信息及对应卡号变化
        public void UpdateCustAndICcard(CCustomer cust, CICCard oriICcd, CICCard nwICcd) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try 
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_user set uname=?,uadrs=?,udtel=?,umobile=? where id=?";
                    cmd.Parameters.Add("uname",OleDbType.VarChar).Value = cust.Code;
                    cmd.Parameters.Add("uadrs",OleDbType.VarChar).Value = cust.Address;
                    cmd.Parameters.Add("udtel",OleDbType.VarChar).Value = cust.Telphone;
                    cmd.Parameters.Add("umobile",OleDbType.VarChar).Value = cust.Mobile;

                    cmd.Parameters.Add("id",OleDbType.Integer).Value = cust.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    if (oriICcd.Code != nwICcd.Code)
                    {
                        //删除旧的IC卡信息
                        cmd.CommandText = "update ieg_iccd set pnmb=?,uid=?,duedtime=?,type=?,addrs=? where ccode=?";
                        cmd.Parameters.Add("pnmb", OleDbType.VarWChar).Value = oriICcd.PlatNumber;
                        cmd.Parameters.Add("uid", OleDbType.Integer).Value = oriICcd.CustomerID;
                        cmd.Parameters.Add("duedtime", OleDbType.Date).Value = oriICcd.DueDtime;
                        cmd.Parameters.Add("type", OleDbType.Integer).Value = oriICcd.Type;
                        cmd.Parameters.Add("addrs", OleDbType.VarWChar).Value = oriICcd.Address;
                        cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = oriICcd.Code;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();


                        //添加新的IC卡信息
                        cmd.CommandText = "update ieg_iccd set pnmb=?,uid=?,duedtime=?,type=?,status=?,addrs=? where ccode=?";
                        cmd.Parameters.Add("pnmb", OleDbType.VarWChar).Value = nwICcd.PlatNumber;
                        cmd.Parameters.Add("uid", OleDbType.Integer).Value = nwICcd.CustomerID;
                        cmd.Parameters.Add("duedtime", OleDbType.Date).Value = nwICcd.DueDtime;
                        cmd.Parameters.Add("type", OleDbType.Integer).Value = nwICcd.Type;
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = nwICcd.Status;
                        cmd.Parameters.Add("addrs", OleDbType.VarWChar).Value = nwICcd.Address;
                        cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = nwICcd.Code;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    else
                    {
                        //添加新的IC卡信息
                        cmd.CommandText = "update ieg_iccd set pnmb=?,uid=?,duedtime=?,type=?,status=?,addrs=? where ccode=?";
                        cmd.Parameters.Add("pnmb", OleDbType.VarWChar).Value = nwICcd.PlatNumber;
                        cmd.Parameters.Add("uid", OleDbType.Integer).Value = nwICcd.CustomerID;
                        cmd.Parameters.Add("duedtime", OleDbType.Date).Value = nwICcd.DueDtime;
                        cmd.Parameters.Add("type", OleDbType.Integer).Value = nwICcd.Type;
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = nwICcd.Status;
                        cmd.Parameters.Add("addrs", OleDbType.VarWChar).Value = nwICcd.Address;
                        cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = nwICcd.Code;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message = "18-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //插入新用户并更新卡号
        public void InsertCustomer(CCustomer nctm, CICCard nicd) 
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.CommandText = "insert ieg_user(uname,uadrs,udtel,umobile) values (?,?,?,?)";
                    cmd.Parameters.Add("uname", OleDbType.VarChar).Value = nctm.Code;
                    cmd.Parameters.Add("uadrs", OleDbType.VarChar).Value = nctm.Address;
                    cmd.Parameters.Add("udtel", OleDbType.VarChar).Value = nctm.Telphone;
                    cmd.Parameters.Add("umobile", OleDbType.VarChar).Value = nctm.Mobile;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "Select @@Identity";
                    nctm.ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    nicd.CustomerID = nctm.ID;

                    cmd.CommandText = "update ieg_iccd set pnmb=?,uid=?,duedtime=?,type=?,status=?,addrs=? where ccode=?";
                    cmd.Parameters.Add("pnmb", OleDbType.VarWChar).Value = nicd.PlatNumber;
                    cmd.Parameters.Add("uid", OleDbType.Integer).Value = nicd.CustomerID;
                    cmd.Parameters.Add("duedtime", OleDbType.Date).Value = nicd.DueDtime;
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = nicd.Type;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = nicd.Status;
                    cmd.Parameters.Add("addrs", OleDbType.VarWChar).Value = nicd.Address;

                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = nicd.Code;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();

                }
                catch (Exception e)
                {
                    string sErrors = e.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e1)
                    {
                        sErrors += " Error 4:" + e1.ToString();
                    }
                    throw new Exception(sErrors);
                }
            }
        }
        //删除顾客
        public void DeleteCustomer(CICCard iccd,int custID) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from ieg_user where id=?";
                    cmd.Parameters.Add("id",OleDbType.Integer).Value = custID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "update ieg_iccd set pnmb=?,uid=?,addrs=?,duedtime=?,type=? where ccode=?";
                    cmd.Parameters.Add("pnmb",OleDbType.VarWChar).Value = iccd.PlatNumber;
                    cmd.Parameters.Add("uid",OleDbType.Integer).Value = iccd.CustomerID;
                    cmd.Parameters.Add("addrs",OleDbType.VarWChar).Value = iccd.Address;
                    cmd.Parameters.Add("duedtime",OleDbType.Date).Value = iccd.DueDtime;
                    cmd.Parameters.Add("type",OleDbType.Integer).Value=iccd.Type;

                    cmd.Parameters.Add("ccode",OleDbType.VarChar).Value = iccd.Code;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message = "19-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //添加用户卡
        public void InsertICCard(CICCard iccd)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                try 
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Insert into ieg_iccd(ccode,pcode,cdtime,type,status) values(?,?,?,?,?)";
                    cmd.Parameters.Add("ccode",OleDbType.VarChar).Value = iccd.Code;
                    cmd.Parameters.Add("pcode",OleDbType.VarChar).Value = iccd.PhysicCode;
                    cmd.Parameters.Add("cdtime",OleDbType.Date).Value = iccd.CreateDtime;
                    cmd.Parameters.Add("type",OleDbType.Integer).Value = iccd.Type;
                    cmd.Parameters.Add("status",OleDbType.Integer).Value = iccd.Status;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //更新用户卡
        public void UpdateICCard(CICCard iccd)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;                
                cmd.CommandType = CommandType.Text;
                try
                {
                    connection.Open();
                    cmd.CommandText = "update ieg_iccd set ccode=?,type=?,status=? where pcode=?";
                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = iccd.Code;
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = iccd.Type;
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = iccd.Status;

                    cmd.Parameters.Add("pcode", OleDbType.VarChar).Value = iccd.PhysicCode;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //更新挂失卡及注销卡信息
        public void UpdateIccardStat(CICCard iccd) 
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;                
                try
                {
                    connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_iccd set status=?,lossdtime=?,dispdtime=? where ccode=?";
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = iccd.Status;
                    cmd.Parameters.Add("lossdtime", OleDbType.Date).Value = iccd.LossDtime;
                    cmd.Parameters.Add("dispdtime", OleDbType.Date).Value = iccd.DisposeDtime;

                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = iccd.Code;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //更新操作员密码
        public void UpdateUserPassword(string code, string pwd) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_oprt set password=? where code=?";
                    cmd.Parameters.Add("password", OleDbType.VarChar).Value = pwd;
                    cmd.Parameters.Add("code",OleDbType.VarChar).Value = code;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //添加操作员
        public void InsertOperator(COperator opr) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into ieg_oprt(name,code,password,authtype) values (?,?,?,?)";
                    cmd.Parameters.Add("name", OleDbType.VarChar).Value = opr.Name;
                    cmd.Parameters.Add("code",OleDbType.VarChar).Value = opr.Code;
                    cmd.Parameters.Add("password",OleDbType.VarChar).Value = opr.Password;
                    cmd.Parameters.Add("authtype",OleDbType.Integer).Value = opr.Type;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //删除操作员
        public void DeleteOperator(string code) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from ieg_oprt where code=?";                  
                    cmd.Parameters.Add("code", OleDbType.VarChar).Value = code;                   
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //添加日志
        public void InsertSysLog(CSysLog log) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into ieg_slog(cards,ldesc,ddtime,opcd) values(?,?,?,?)";
                    cmd.Parameters.Add("cards",OleDbType.VarWChar).Value = log.Mcard;
                    cmd.Parameters.Add("ldesc",OleDbType.VarWChar).Value = log.Description;
                    cmd.Parameters.Add("ddtime",OleDbType.Date).Value = log.Dtime;
                    cmd.Parameters.Add("opcd",OleDbType.VarWChar).Value = log.OperatCode;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message = "20-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //添加故障
        public void InsertErrorLog(CSysLog log)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into ieg_elog(code,ldesc,ddtime,opcd) values(?,?,?,?)";
                    cmd.Parameters.Add("code", OleDbType.VarWChar).Value = log.Mcard;
                    cmd.Parameters.Add("ldesc", OleDbType.VarWChar).Value = log.Description;
                    cmd.Parameters.Add("ddtime", OleDbType.Date).Value = log.Dtime;
                    cmd.Parameters.Add("opcd", OleDbType.VarWChar).Value = log.OperatCode;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    string message = "21-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //依时间段加载日志信息，制成表格形式
        public DataTable LoadSysLogs(DateTime start, DateTime end) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                DataTable dt = new DataTable("slog");
                OleDbDataAdapter da = new OleDbDataAdapter("select cards,ldesc,ddtime from ieg_slog where ddtime>='"+start+"' and ddtime <='"+end+"'", conn);
                da.Fill(dt);
                return dt;
            }
        }
        //依时间段加载故障信息，制成表格形式
        public DataTable LoadErrLogs(DateTime start, DateTime end)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                DataTable dt = new DataTable("elog");
                OleDbDataAdapter da = new OleDbDataAdapter("select code,ldesc,ddtime from ieg_elog where ddtime>='" + start + "' and ddtime <='" + end + "'", conn);
                da.Fill(dt);
                return dt;
            }
        }
        //删除指定时间段的故障记录
        public void DeleteErrorLogs(DateTime start, DateTime end) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                try 
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "delete from ieg_elog where ddtime>='" + start + "' and ddtime <='" + end + "'";
                    cmd.ExecuteNonQuery();                   
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //车辆离开时修改作业状态及取物车位状态
        public void UpdateCTaskAndLctOfStatus(CTask tsk, CLocation lct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString)) 
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = conn;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                    cmd.Parameters.Add("status",OleDbType.Integer).Value = tsk.Status;
                    cmd.Parameters.Add("id",OleDbType.Integer).Value=tsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    if (lct != null) 
                    {
                        cmd.CommandText = "update ieg_lctn set status=? where address=?";
                        cmd.Parameters.Add("status",OleDbType.Integer).Value = lct.Status;
                        cmd.Parameters.Add("address",OleDbType.VarWChar).Value = lct.Address;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message = "22-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //单独完成主作业及相应的子作业
        public void CompleteMasterTaskAndCTask(CMasterTask mtsk)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandText = "update ieg_mtsk set iscpl=? where id=?";
                    cmd.Parameters.Add("iscpl", OleDbType.Integer).Value = mtsk.IsCompleted ? 1 : 0;
                    cmd.Parameters.Add("id", OleDbType.Integer).Value = mtsk.ID;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    for (int i = 0; i < mtsk.Tasks.Length; i++)
                    {
                        CTask tsk = mtsk.Tasks[i];
                        cmd.CommandText = "update ieg_ctsk set status=? where id=?";
                        cmd.Parameters.Add("status", OleDbType.Integer).Value = CTask.EnmTaskStatus.Finished;
                        cmd.Parameters.Add("id", OleDbType.Integer).Value = tsk.ID;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

                    cmd.Transaction.Commit();
                }
                catch (Exception ex) 
                {
                    string message = "23-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }
        //更新车位状态
        public void UpdateLocationStatus(CLocation lct) 
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                OleDbTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_lctn set status=? where address=?";
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = lct.Status;

                    cmd.Parameters.Add("address", OleDbType.VarWChar).Value = lct.Address;
                    int pam= cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    if (pam > -1)
                    {
                        string msg = "UpdateLocationStatus更新车位：" + lct.Address + " 状态：" + lct.Status.ToString() + " 卡号：" + lct.ICCardCode;
                        new CWSException(msg, 1);
                    }

                    cmd.Transaction.Commit();
                }
                catch(Exception ex)
                {
                    string message = "24-" + ex.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e2)
                    {
                        message += "      事件回滚异常：" + e2.ToString();
                    }
                    throw new Exception(message);
                }
            }
        }

        //更新车位信息
        public void UpdateLocationInfo(CLocation FrLct)
        {
            using (OleDbConnection conn = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    conn.Open();
                    cmd.CommandText = "update ieg_lctn set status=?,iccode=?,indtime=?,dist=?,csize=? where address=?";
                    cmd.Parameters.Add("status", OleDbType.Integer).Value = FrLct.Status;
                    cmd.Parameters.Add("iccode", OleDbType.VarWChar).Value = FrLct.ICCardCode;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = FrLct.InDate;
                    cmd.Parameters.Add("dist", OleDbType.Integer).Value = FrLct.Distance;
                    cmd.Parameters.Add("csize", OleDbType.VarWChar).Value = FrLct.CarSize;

                    cmd.Parameters.Add("address", OleDbType.VarWChar).Value = FrLct.Address;
                    int pam1 = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    string msg = "UpdateLocationInfo更新车位：" + FrLct.Address + " 状态：" + FrLct.Status.ToString() + " 卡号：" + FrLct.ICCardCode;
                    new CWSException(msg, 1);
                }
                catch (Exception ex)
                {
                    new CWSException(ex);
                }
            }
        }

        #region 收费相关
        //加载收费标准
        public List<CTariff> LoadTariff()
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                 List<CTariff> csts = new List<CTariff>();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select id,ictype,unit,fee,days,feetype,isbusy from ieg_cgst order by id";
                    OleDbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        csts.Add(new CTariff(reader.GetInt32(0)
                        , (CICCard.EnmICCardType)reader.GetInt32(1)
                        , (CTariff.EnmFeeUnit)reader.GetInt32(2)
                        , Convert.ToSingle(reader.GetValue(3))
                        , Convert.ToSingle(reader.GetValue(4))
                        , (CTariff.EnmFeeType)reader.GetInt32(5)
                        , reader.GetInt32(6) == 0 ? false : true));
                    }
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
                return csts;
            }
        }
        //添加收费标准
        public void InsertTariff(CTariff ntrf)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert ieg_cgst(ictype,unit,fee,days,feetype,isbusy) values (?,?,?,?,?,?)";
                    cmd.Parameters.Add("type", OleDbType.Integer).Value = ntrf.Type;
                    cmd.Parameters.Add("unit", OleDbType.Integer).Value = ntrf.Unit;
                    cmd.Parameters.Add("fee", OleDbType.Currency).Value = ntrf.Fee;
                    cmd.Parameters.Add("days", OleDbType.Numeric).Value = ntrf.Time;
                    cmd.Parameters.Add("feetype", OleDbType.Integer).Value = ntrf.FeeType;
                    cmd.Parameters.Add("isbusy", OleDbType.Integer).Value = ntrf.ISbusy ? 1 : 0;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "Select @@Identity";
                    ntrf.ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                } 
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }
        //更新收费标准
        public void UpdateTariff(CTariff ntrf)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try
                {
                connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update ieg_cgst set fee=? where id=?";
                cmd.Parameters.Add("fee", OleDbType.Currency).Value = ntrf.Fee;                
                cmd.Parameters.Add("id", OleDbType.Integer).Value = ntrf.ID;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //删除收费标准
        public void DeleteTariff(int tid)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try{
                connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from ieg_cgst where id=?";
                cmd.Parameters.Add("id", OleDbType.Integer).Value = tid;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //更新卡的收费时间，及插入固定卡收费记录
        public void UpdateICCardAndLog(CICCard icd, CFixCardChargeLog nfcdlog)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                OleDbTransaction transaction = null;
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ieg_iccd set duedtime=? where ccode=?";
                    cmd.Parameters.Add("duedtime", OleDbType.Date).Value = icd.DueDtime;
                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = icd.Code;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "insert ieg_fxcg(ccode,ictype,cgdtime,duedtime,fee,uid,opcd,indtime) values(?,?,?,?,?,?,?,?)";
                    cmd.Parameters.Add("ccode", OleDbType.VarChar).Value = nfcdlog.ICCode;
                    cmd.Parameters.Add("ictype", OleDbType.Integer).Value = nfcdlog.ICType;
                    cmd.Parameters.Add("cgdtime", OleDbType.Date).Value = nfcdlog.ThisDate;
                    cmd.Parameters.Add("duedtime", OleDbType.Date).Value = nfcdlog.DueDtime;
                    cmd.Parameters.Add("fee", OleDbType.Single).Value = nfcdlog.ThisFee;
                    cmd.Parameters.Add("uid", OleDbType.Integer).Value = icd.CustomerID;
                    cmd.Parameters.Add("opcd", OleDbType.VarWChar).Value = nfcdlog.OperatorCode;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.Transaction.Commit();
                }
                catch (Exception e)
                {
                    string sErrors ="27-" +e.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e1)
                    {
                        sErrors += " Error 2:" + e1.ToString();
                    }
                    throw new Exception(sErrors);
                }
            }
        }

        /// <summary>
        /// 添加临时卡收费记录
        /// </summary>
        public void InsertTempCardChargeLog(CTempCardChargeLog tclog)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert ieg_tpcg(iccode,indtime,outdtime,revfee,actfee,optcode) values (?,?,?,?,?,?)";
                    cmd.Parameters.Add("iccode", OleDbType.VarChar).Value = tclog.ICCode;
                    cmd.Parameters.Add("indtime", OleDbType.Date).Value = tclog.InDate;
                    cmd.Parameters.Add("outdtime", OleDbType.Date).Value = tclog.OutDate;
                    cmd.Parameters.Add("revfee", OleDbType.Single).Value = tclog.RecivFee;
                    cmd.Parameters.Add("actfee", OleDbType.Single).Value = tclog.ActualFee;
                    cmd.Parameters.Add("optcode", OleDbType.VarChar).Value = tclog.OperatorCode;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex) 
                {
                    throw ex;
                }
            }
        }

        //固定卡缴费记录
        public DataTable SelectFixCardChargRcds(int idx, string ctt, DateTime st, DateTime en)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                connection.Open();
                DataSet d = new System.Data.DataSet();
                string sql = "";
                switch (idx)
                {
                    case 0:
                        sql = "select uname,ccode,ictype,cgdtime,duedtime,fee,opcd from ieg_user a inner join ieg_fxcg b on a.id=b.uid where ccode='" + ctt + "'  order by cgdtime";
                        break;
                    case 1:
                        sql = "select uname,ccode,ictype,cgdtime,duedtime,fee,opcd from ieg_user a inner join ieg_fxcg b on a.id=b.uid  where uname='" + ctt + "' order by cgdtime";
                        break;
                    case 2:
                        sql = "select uname,ccode,ictype,cgdtime,duedtime,fee,opcd from ieg_user a inner join ieg_fxcg b on a.id=b.uid  where opcd='" + ctt + "' order by cgdtime";
                        break;
                    case 3:
                        sql = "select uname,ccode,ictype,cgdtime,duedtime,fee,opcd from ieg_user a inner join ieg_fxcg b on a.id=b.uid where cgdtime>='" + st + "' and cgdtime<='" + en + "' order  by cgdtime";
                        break;
                    case 4:
                        sql = "select uname,ccode,ictype,cgdtime,duedtime,fee,opcd from ieg_user a inner join ieg_fxcg b on a.id=b.uid where duedtime>='" + st + "' and duedtime<='" + en + "'order by cgdtime";
                        break;

                }

                OleDbDataAdapter ad = new OleDbDataAdapter(sql, connection);
                ad.Fill(d, "fixcgrcd");

                return d.Tables[0];
            }
        }
        //临时卡缴费记录
        public DataTable SelectTempCardChargRcds(int idx, string ctt, DateTime st, DateTime en)
        {
            using (OleDbConnection connection = new OleDbConnection(OleDbString))
            {
                connection.Open();
                DataSet d = new System.Data.DataSet();
                string sql = "";
                switch (idx)
                {
                    case 0:
                        sql = "select iccode,indtime,outdtime,revfee,actfee,optcode from ieg_tpcg where iccode='" + ctt + "' order by id";
                        break;
                    case 1:
                        sql = "select iccode,indtime,outdtime,revfee,actfee,optcode from ieg_tpcg where indtime>='" + st + "' and indtime<='" + en + "' order  by id";
                        break;
                    case 2:
                        sql = "select iccode,indtime,outdtime,revfee,actfee,optcode from ieg_tpcg where outdtime>='" + st + "' and outdtime<='" + en + "'order by id";
                        break;
                    case 3:
                        sql = "select iccode,indtime,outdtime,revfee,actfee,optcode from ieg_tpcg where optcode='" + ctt + "'  order by id";
                        break;

                }

                OleDbDataAdapter ad = new OleDbDataAdapter(sql, connection);
                ad.Fill(d, "tempcgrcd");

                return d.Tables[0];
            }
        }

        #endregion

    }
}

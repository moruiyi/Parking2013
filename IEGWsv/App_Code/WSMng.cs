using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using IEGModel;
using IEGBLL;

namespace IEGWsv
{
    [System.Web.Services.WebServiceBinding(Name = "WSMng", ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1, EmitConformanceClaims = true), System.Web.Services.Protocols.SoapDocumentService()]
    public class WSMng : System.Web.Services.WebService
    {

        public WSMng()
        {
            if (Application["Clientlist"] == null)
            {
                Application["Clientlist"] = new List<string>();   //建立的客户端
            }
        }

        /// <summary>
        /// 接口模板
        /// </summary>
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InterfaceTemplate(int hnmb)
        {
            try
            {
                return 0;
            }
            catch (Exception e1)
            {
                try
                {
                    new CWSException(e1);
                }
                catch
                {
                    //return 1;
                }
                return 600;
            }
        }

        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool AddClient(string client)
        {
            List<string> Clist = (List<string>)Application["Clientlist"];

            if (Clist.Contains(client))
            {
                return false;
            }
            else
            {
                Clist.Add(client);
                return true;
            }
        }

        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void RemoveClient(string client)
        {
            List<string> Clist = (List<string>)Application["Clientlist"];

            if (Clist.Contains(client))
            {
                Clist.Remove(client);
            }
        }

        //以下为自定义方法

        /// <summary>
        /// 获取车厅声音文件
        /// </summary>        
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public string GetCurrentSound(int hallNum) 
        {
            try
            {
                return (new CWTask()).GetNotification(hallNum);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch 
                {
                }
                return null;
            }
        }
        //处理刷卡
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCardMessage(int hallID,string physicCard) 
        {
            try
            {
                (new CWTaskTran(hallID)).DealCardMessage(physicCard);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
            }
        }
        
        //更新设备的状态字信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void UpdateStatCodes(short[] codes)
        {
            try
            {
                new CWSMG().UpdateStatusCode(codes);
            }
            catch (Exception ex) 
            {
                try 
                {
                    new CWSException(ex);
                }
                catch { }
            }
        }
       
        //20141203 更新报警信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void UpdateErrorInfo(short[] errAddrs,int scNo) 
        {
            try
            {
                new CWSMG().UpdateErrorAlarm(errAddrs,scNo);
            }
            catch (Exception ex)
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
            }
        }

        //查找主作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CMasterTask GetMasterTaskFromID(int MID) 
        {
            try
            {
                return new CWTask().GetMasterTaskFromID(MID);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //查找主作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CMasterTask GetMasterTaskOfTid(int tskID) 
        {
            try 
            {
                CMasterTask mtsk;
                CTask tsk;
                int i= new CWTask().GetMTaskAndCTaskOfTid(tskID,out mtsk,out tsk);
                if (i == 1)
                {
                    return mtsk;
                }
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
            }
            return null;
        }

        //查找设备
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CSMG SelectSMG(int smg) 
        {
            try 
            {
                return new CWSMG().SelectSMG(smg);
            }
            catch (Exception ex) 
            {
                try 
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //查找子作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CTask GetCTaskFromID(int id) 
        {
            try 
            {
                return new CWTask().GetCTaskFromtskID(id);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //依车位地址查找车位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CLocation SelectLctFromAddrs(string address)
        {
            try 
            {
                return new CWLocation().SelectLctFromAddrs(address);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //查找卡
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CICCard SelectByUserCode(string code) 
        {
            try 
            {
                return new CWICCard().SelectByUserCode(code);
            }
            catch (Exception ex)
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //依设备类型查找设备集合
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CSMG[] SelectSMGsOfType(CSMG.EnmSMGType etype) 
        {
            try 
            {
                return new CWSMG().SelectSMGsOfType(etype);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //ETV故障时，更新设备的状态
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void UpdateSMGStatus(int eqp,bool aval,CSMG.EnmModel emodel) 
        {
            try
            {
                new CWSMG().UpdateSMGStatus(eqp,aval,emodel);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
            }
        }

        //处理第一次入库
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCarEntrance(int hallID) 
        {
            try
            {
                new CWTaskTran(hallID).DealCarEntrance();
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch { }
            }
        }
        //更新子作业的发送时间及发送状态
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void SetTskStatusDetail(int tsk,CTask.EnmTaskStatusDetail detail,DateTime sdtime)
        {
            try
            {
                new CWTask().SetTskStatusDetail(detail,tsk,sdtime);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //更新子作业状态
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealUpdateTaskStatus(CTask.EnmTaskStatus estat,int tsk) 
        {
            try 
            {
                new CWTask().DUpdateTaskStatus(estat,tsk);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //车辆离开车厅时处理
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCarLeave(int hallID,int tskID) 
        {
            try
            {
                new CWTaskTran(hallID).DealCarLeave(tskID);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //外形检测失败处理
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCheckCarFail(int hallID) 
        {
            try 
            {
                new CWTask().IDealCheckCarFail(hallID);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //处理外形数据，分配车位及ETV
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCheckedCar(int tskID,int hnmb, string CCheckResult, int distance) 
        {
            try
            {
                new CWTaskTran(hnmb).DealCheckCar(tskID, distance, CCheckResult);                
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //处理ETV装载完成
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealTVLoadFinishing(int tid, int ndist) 
        {
            try 
            {
                new CWTask().DealTVActionFinishing(tid,true,ndist);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //处理ETV卸载完成
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealTVUnLoadFinishing(int tid, int ndist)
        {
            try
            {
                new CWTask().DealTVActionFinishing(tid, false, ndist);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //完成子作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCompleteCTask(int tskID) 
        {
            try
            {
                new CWTask().CompletedCTask(tskID);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //处理移动完成
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCompleteTVMoveFinish(int tid) 
        {
            try 
            {
                new CWTask().DealCompleteTVMoveFinish(tid);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch
                {
                }
            }
        }

        //处理装载完成
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCompleteTVLoadFinish(int tid)
        {
            try
            {
                new CWTask().DealCompleteTVLoadFinish(tid);
            }
            catch (Exception ex)
            {
                try
                {
                    new CWSException(ex);
                }
                catch
                {
                }
            }
        }

        //处理车厅卸载完成
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void ODealEVUp(int tid)
        {
            try
            {
                new CWTask().ODealEVUp(tid);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //依设备的作业号查找该作业，如果不存在的，则清除当前设备的作业号
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void UpdateSMGsWorkStat() 
        {
            try
            {
                new CWSMG().UpdateAllSMGsWorkStatus();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //更新设备绑定的子作业及主作业号
        [System.Web.Services.WebMethod(),System.Web.Services.Protocols.SoapDocumentMethod(Binding="WSMng")]
        public void UpdateWorkingStatOfSMG(CSMG smg) 
        {
            try
            {
                new CWSMG().UpdateSMGTaskStat(smg);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //查找对应车厅的所有主作业，如果索引为0则返回所有主作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void GetAllMasterTaskOfHid(int hid,out CMasterTask[] mtsks)
        {
            mtsks = null;
            try
            {
                mtsks=new CWTask().GetAllCMasterTaskOfHid(hid);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //判断用户登录信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public COperator CheckPassword(string code, string pwd) 
        {
            try
            {
               return new CWICCard().CheckLoginPassword(code, pwd);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }

                return null;
            }
        }
        //返回库内车位数量
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectLctofInfo(out int ptotal, out int pspace, out int poccupy, out int pfixLct,out int pBigLct) 
        {
            ptotal = 0;
            pspace = 0;
            poccupy = 0;
            pfixLct = 0;
            pBigLct = 0;
            try 
            {
                new CWLocation().SelectSpaceLctCounts(out ptotal,out pspace,out poccupy,out pfixLct,out pBigLct);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }               
            }
            return 0;
        }
        //查询库内大车数及空余车位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectSpaceLctsAndBigLcts(out int pspace, out int bigspace)
        {
            pspace = 0;
            bigspace = 0;
            try 
            {
                new CWLocation().GetSpaceLctsAndBigLcts(out pspace, out bigspace);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
            return 0;
        }

        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int GetSpaceLocations() 
        {
            try
            {
                return new CWLocation().GetSpaceLocations();
            }
            catch (Exception ex) 
            {
                try 
                {
                     new CWSException(ex);
                }
                catch { }
                return -1;
            }
           
        }

        //返回库内车位对象
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CLocation[] SelectAllLocations() 
        {
            try 
            {
                return new CWLocation().SelectAllLocations();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            } 
            return null;
        }
        //查询指定设备的状态字
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CStatCode[] SelectStatusCodes(int smg) 
        {
            try
            {
                return new CWSMG().SelectStatusCodes(smg);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
            return null;
        }
        //依车厅号查询车厅所有的主作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int GetMasterTaskCountOfHid(int hnmb) 
        {
            try
            {

                return new CWTask().GetMasterTaskCountOfHid(hnmb);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }              
            }
            return 0;
        }
        //依设备号查找当前设备的作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CTask GetCurrentTaskOfSMG(int id) 
        {
            try
            {
                return new CWTask().GetCurrentTaskOfSMG(id);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //依车厅号查找当前执行的主作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CMasterTask GetCurrentMasterTaskOfSMG(int hnmb) 
        {
            try 
            {
                return new CWTask().GetCurrentMasterTaskOfSMG(hnmb);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //判断设备是否存在报警
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool JudgeEqpHasErrorBit(int id) 
        {
            try 
            {
                return new CWSMG().JudgeEqpHasErrorBitOfID(id);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return false;
            }
        }
        //返回对应设备的报警描述信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CErrorCode[] LoadErrorCodeDescp(int type) 
        {
            try 
            {
                return new CWSMG().LoadErrorCodeDescp(type);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //加载所有故障信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CErrorCode[] LoadErrorsDescp() 
        {
            try
            {
                return new CWSMG().LoadErrorsDescp();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //返回正在报警的信息位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CErrorCode[] SelectCurrErrCodes() 
        {
            try 
            {
                return new CWSMG().SelectAllErrs();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }

        //返回报警位点亮的位信息集合
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CErrorCode[] GetErrorCodesOfEqpID(int type) 
        {
            try
            {
                return new CWSMG().SelectErrorCodesOfeId(type);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //手动作业创建
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int CreateManageMasterTask(string FrLct, string ToLct, CMasterTask.EnmMasterTaskType mtype,out int hallID) 
        {            
            try
            {
                int hall;
                int rit= new CWTaskTran().CreateManageMasterTask(FrLct,ToLct,mtype,out hall);
                hallID = hall;

                return rit;
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }

                hallID = 0;
                return 0;
            }
        }
        //手动完成作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualCompleteMasterTask(int mid) 
        {
            try 
            {
                return new CWTask().CompleteMasterTask(mid);               
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //手动复位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualResetMasterTask(int mid) 
        {
            try 
            {
                return new CWTask().ResetMasterTask(mid);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //依存车卡号找出存车车位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CLocation SelectLocationOfIccd(string iccode) 
        {
            try
            {
                return new CWLocation().SelectLctFromICCode(iccode);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //手动车位出库
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualOutLocation(string addrs) 
        {
            try
            {
                return new CWLocation().ManualOutLocation(addrs);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //手动车位入库
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualInLocation(string addrs,string iccode,string carSize,int distance,DateTime dt)
        {
            try
            {
                return new CWLocation().ManInLocation(addrs,iccode,carSize,distance,dt);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //手动车位禁用
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualDisLocation(string addrs) 
        {
            try
            {
                return new CWLocation().ManualDisLoction(addrs,true);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //手动车位启用
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualEnableLocation(string addrs)
        {
            try
            {
                return new CWLocation().ManualDisLoction(addrs, false);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //手动车位挪移
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int ManualTranspose(string fAddrs, string tAddrs) 
        {
            try
            {
                return new CWLocation().ManualTransposeLocation(fAddrs,tAddrs);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //获取库内所有作业
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CSMG[] SelectAllSMGs() 
        {
            try 
            {
                return new CWSMG().GetAllSMGs();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //更新设备的可用性
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int UpdateSMGAvailable(int eid, bool avail) 
        {
            try 
            {
                new CWSMG().UpdateSMGStatus(eid, avail, CSMG.EnmModel.Init);
                return 1;
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //更新设备的可用性
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SetHallType(int hnmb, int idx) 
        {
            try
            {
                return new CWSMG().SetHallType(hnmb, (CSMG.EnmHallType)(idx + 1));
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 1;
            }
        }
        //返回所有车主信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CCustomer[] SelectAllCustomer() 
        {
            try
            {
                return new CWICCard().SelectAllCustomers();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //制固定卡前判断车位有效性
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool CheckFixLocationAvail(string adrs) 
        {
            try
            {
                return new CWLocation().CheckFixLocation(adrs);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return false;
            }
        }
        //用户管理，更新顾客信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int UpdateCustomInfo(CCustomer cust,CICCard niccd,string oriCode) 
        {
            try
            {
                return new CWICCard().UpdateCustomInfo(cust, niccd, oriCode);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

         //用户管理，插入新顾客信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InsertCustomer(CCustomer cut, CICCard iccd) 
        {
            try 
            {
                return new CWICCard().InsertCustomer(cut, iccd);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //删除顾客对象
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DeleteCustomer(int custID,string ccode) 
        {
            try
            {
                return new CWICCard().DeleteCustomer(custID,ccode);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //插入用户卡
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InsertICCard(CICCard iccd)
        {
            try
            {
                return new CWICCard().InsertICCard(iccd);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //挂失、注销
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int UpdateICCardStatus(string code, CICCard.EnmICCardStatus stat) 
        {
            try
            {
                return new CWICCard().UpdateICCardStatus(code,stat);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //更新操作员密码
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int UpdateOperatorPassword(string code, string pwd) 
        {
            try
            {
                return new CWICCard().ChangePassword(code, pwd); ;
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //添加新操作员
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InsertOperator(COperator opr) 
        {
            try 
            {
                return new CWICCard().InsertNewOperator(opr);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //查询所有操作员信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public COperator[] SelectAllOperators() 
        {
            try
            {
                return new CWICCard().LoadAllOperators().ToArray();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //删除操作员信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DeleteOprt(string code) 
        {
            try
            {
                return new CWICCard().DeleteOperator(code);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //插入故障信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InsertErrorLog(byte eqp, string descp, DateTime dt) 
        {
            try
            {               
                return new CWSMG().WriteErrorLog(eqp,descp,dt);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //查询指定范围内所有的日志信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectSysLogs(DateTime start, DateTime end, out DataTable dt) 
        {
            dt = null;
            try 
            {                
                dt = CWData.myDB.LoadSysLogs(start, end);
                return 100;
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //查询指定范围内所有的故障信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectErrLogs(DateTime start, DateTime end, out DataTable dt)
        {
            dt = null;
            try
            {
                dt = CWData.myDB.LoadErrLogs(start, end);
                return 100;
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //删除指定范围内所有的故障信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DeleteErrorLogs(DateTime st, DateTime end) 
        {
            try 
            {
                CWData.myDB.DeleteErrorLogs(st, end);
                return 100;
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //处理取物
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DealOTempGetCar(string iccode, int hallID) 
        {
            try
            {
                return new CWTaskTran().TempGetCar(iccode, hallID);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //处理车辆离开车厅
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DealCarDriveLeave(int tskID) 
        {
            try
            {
                return new CWTask().DealCarDriveAway(tskID);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //处理车辆跑位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealCarOffTracing(int tid) 
        {
            try 
            {
                new CWTask().DealCarDriveOffTracing(tid); 
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //判断车辆是否跑位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool CheckCarOffTracing(int hnmb) 
        {
            try
            {
                return new CWSMG().CheckCarInMoveOut(hnmb);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return false;
            }
        }
        //获取临时卡收费信息
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int GetTempCardFeeInfo(string iccode,out CTempCardChargeLog tlog) 
        {
            tlog = null;
            try 
            {                
                return  new CWICCard().GetTempCardUserInfo(iccode, out tlog);                
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

         //获取固定卡的临时收费
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int GetFixCardCurrFee(string iccode, out CFixCardChargeLog log) 
        {
            log = null;
            try
            {
                return new CWICCard().GetFixCardCurrentFee(iccode,out log);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //加载收费标准
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CTariff[] SelectTariff() 
        {
            try 
            {
                //return new CWTariff().SelectTariff();
                return new CWNewTrariff().SelectTariff();
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }
        //确认固定卡缴费
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SetFixCardFee(CFixCardChargeLog tlog)
        {
            try
            {
                return new CWICCard().SetFixCardFee(tlog);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //插入临时卡收费记录
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void InsertTempCardChargeLog(CTempCardChargeLog tlog) 
        {
            try 
            {
                CWData.myDB.InsertTempCardChargeLog(tlog);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }

        //删除计费标准项
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int DeleteTariff(int tID) 
        {
            try
            {
                return new CWTariff().DeleteTariff(tID);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //添加计费标准项
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int InsertTariff(CTariff ctff) 
        {
            try
            {
                return new CWTariff().InsertTariff(ctff);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //更新计费标准项
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int UpdateTariff(CTariff ctff) 
        {
            try 
            {
                //return new CWTariff().UpdateTariff(ctff);
                return new CWNewTrariff().UpdateTariff(ctff);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }

        //查询固定卡缴费记录
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectFixCardChargRcds(int idx, string ctt, DateTime st, DateTime en, out DataTable dt)
        {
            dt = null;
            try
            {
                //0=用户卡号，1=车主姓名，2=操作员，3=缴费日期，4=使用日期
                dt = CWData.myDB.SelectFixCardChargRcds(idx, ctt, st, en);
                return 100;
            }
            catch (Exception e1)
            {
                try
                {
                     new CWSException(e1);
                }
                catch
                {                    
                }
                return 101;
            }
        }

         //查询临时卡缴费记录
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int SelectTempCardChargRcds(int idx, string ctt, DateTime st, DateTime en, out DataTable dt)
        {
            dt = null;
            try
            {
                //0=卡号，1=入库时间，2=出库时间，3=操作员
                dt = CWData.myDB.SelectTempCardChargRcds(idx, ctt, st, en);
                return 100;
            }
            catch (Exception e1)
            {
                try
                {
                     new CWSException(e1);
                }
                catch
                {                    
                }
                return 101;
            }
        }
        //临时卡取车
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public int CreateTempICcardOut(string iccode, out int hnmb) 
        {
            hnmb = 0;
            try 
            {
                return new CWTaskTran().OCreateTempICcardGetCar(iccode, out hnmb);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return 0;
            }
        }
        //获取库内所有的主作业集合
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public CMasterTask[] GetAllMasterTask() 
        {
            try
            {
                return new CWTask().SelectAllMasterTask();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }  
                return null;
            }
        }
       
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealTvMoveFinishing(int tid)
        {
            try
            {
                new CWTask().DealTVMoveFinishing(tid);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
            }
        }
        //可接收新指令位
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool CheckSMGAcceptCommad(int hnmb) 
        {
            try 
            {
                return new CWSMG().CheckAcceptNewCommand(hnmb);
            }
            catch (Exception ex) 
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return false;
            }
        }
        //获取所有可接收新指令的车厅的集合
        [System.Web.Services.WebMethod(),System.Web.Services.Protocols.SoapDocumentMethod(Binding="WSMng")]
        public int[] GetCarHallsSpace() 
        {
            try
            {
                return new CWSMG().GetSpaceHallList();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// 加入执行队列前判断是否允许下发ETV作业
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public bool CheckMovePathIsBlock(int tid) 
        {
            try 
            {
                return new CWTask().CheckMovePathIsBlock(tid);
            }
            catch (Exception ex) 
            {
                try
                {
                    new CWSException(ex);
                }
                catch 
                { }
                return false;
            }
        }

        /// <summary>
        /// 判断是否可以继续移动，如果是，是否需要生成避让
        /// </summary>
        /// <param name="tid"></param>
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void DealEtvAvoid(int tid) 
        {
            try
            {
                new CWTask().DealEtvBiRang(tid);
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch 
                { }               
            }
        }

        //更新ETV实时坐标
        [System.Web.Services.WebMethod(), System.Web.Services.Protocols.SoapDocumentMethod(Binding = "WSMng")]
        public void UpdateEtvCurrAddress() 
        {
            try
            {
                new CWSMG().UpdateEtvCurrAddress();
            }
            catch (Exception ex)
            {
                try
                {
                     new CWSException(ex);
                }
                catch { }              
            }
        }
    }
}
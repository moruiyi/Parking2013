using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IEGConsole.localhost;

namespace IEGConsole
{
    public abstract  class CHelper
    { 
        /// <summary>
        /// 子作业状态解析
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TaskStatusFormatting(EnmTaskStatus ts)
        {
            string s = "";
            switch (ts)
            {
                case EnmTaskStatus.Init:
                    s = "待命";
                    break;
                case EnmTaskStatus.ICarInWaitFirstSwipeCard:
                    s = "等待车主刷卡";
                    break;
                case EnmTaskStatus.ICheckCarFail:
                    s = "检测车辆失败";
                    break;
                case EnmTaskStatus.IFirstSwipedWaitforCheckSize:
                    s = "等待第二次刷卡";
                    break;
                case  EnmTaskStatus.ISecondSwipedWaitforCheckSize:
                    s = "等待检测车辆";
                    break;
                case EnmTaskStatus.ISecondSwipedWaitforEVDown:
                    s = "等待升降机下降";
                    break;
                case EnmTaskStatus.OCarOutWaitforDriveaway:
                    s = "等待车辆出车厅";
                    break;
                case EnmTaskStatus.OWaitforEVUp:
                    s = "车厅允许取车";
                    break;
                case EnmTaskStatus.TWaitforUnload:
                    s = "等待卸载车辆";
                    break;
                case EnmTaskStatus.TMURO:
                    s = "设备故障";
                    break;
                case EnmTaskStatus.OTVLoadingWaitforEV:
                    s = "TV装载中EV未下降";
                    break;
                case EnmTaskStatus.TWaitforLoad:
                    s = "等待装载车辆";
                    break;
                case EnmTaskStatus.OTVLoadWaitforEVDown:
                    s = "TV已装载EV未下降";
                    break;
                case EnmTaskStatus.OEVDownWaitforTVLoad:
                    s = "TV装载中EV已下降";
                    break;
                case EnmTaskStatus.Finished:
                    s = "作业完成";
                    break;
                case EnmTaskStatus.OWaitforGetCar:
                    s = "等待车主取车";
                    break;
                case EnmTaskStatus.TMURORecoverHascar:
                    s = "故障恢复有车";
                    break;
                case EnmTaskStatus.TMURORecoverNocar:
                    s = "故障恢复无车";
                    break;
                case EnmTaskStatus.TMUORWaitforUnload:
                    s = "等待卸载车辆";
                    break;
                case EnmTaskStatus.TWaitforMove:
                    s = "等待移动";
                    break;
                case EnmTaskStatus.ISecondSwipedWaitforCarLeave:
                    s = "无可用ETV或无匹配车位";
                    break;
                case EnmTaskStatus.ICarInWaitPressButton:
                    s = "等待按确认按钮";
                    break;
                case EnmTaskStatus.OWaitforEVDown:
                    s = "等待升降机下降";
                    break;
                case EnmTaskStatus.MoveFinishing :
                    s = "TV移动完成";
                    break;
                case EnmTaskStatus .HallFinishing:
                    s = "车辆离开车厅";
                    break;
                case EnmTaskStatus.IEVDownFinishing :
                    s = "接受入库指令";
                    break;
                case EnmTaskStatus.IEVDownFinished:
                    s = "确认入库";
                    break;
                case EnmTaskStatus.OEVDownFinishing :
                    s = "车厅确认出车";
                    break;
                case EnmTaskStatus .LoadFinishing :
                    s = "确认装载完成中";
                    break;
                case EnmTaskStatus .UnLoadFinishing :
                    s = "确认卸载完成中";
                    break;
                case EnmTaskStatus.TempOWaitforEVDown:
                    s = "等待升降机下降";
                    break;
                case EnmTaskStatus.TempOEVDownFinishing:
                    s = "车厅确认出车";
                    break;
                default:
                    s = ts.ToString();   //测试
                    break;
            }
            return s;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smg"></param>
        /// <returns></returns>
        public static string SMGModelFormatting(CSMG smg)
        {
            string s = "";
            switch (smg.Model)
            {
                case EnmModel.AutoStarting:
                    s = "自动启动中";
                    break;
                case EnmModel.AutoStarted:
                    s = "单机全自动";
                    break;
                case EnmModel.AutoAligning:
                    s = "自动分配";
                    break;
                case EnmModel.AutoPending:
                    s = "自动挂起";
                    break;
                case EnmModel.AutoStopped:
                    s = "自动停止";
                    break;
                case EnmModel.AutoStopping:
                    s = "自动停止中";
                    break;
                default:
                    s = "";
                    break;
            }

            return s;
        }
        /// <summary>
        /// 设备模式
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        public static string EqpModelFormatting(int mdl)
        {
            string s = "";
            switch (mdl)
            {
                case 1:
                    s = "维修模式";
                    break;
                case 2:
                    s = "手动模式";
                    break;
                case 3:
                    s = "单机自动";
                    break;
                case 4:
                    s = "全自动";
                    break;
                default:
                    s = "";
                    break;
            }

            return s;
        }

        /// <summary>
        /// 主作业类型解析
        /// </summary>
        /// <param name="mType"></param>
        /// <returns></returns>
        public static string MtskTypeFormat(EnmMasterTaskType mType)
        {
            string s = "";
            switch (mType)
            {
                case EnmMasterTaskType.GetCar:
                    s = "取车";
                    break;
                case EnmMasterTaskType.Move:
                    s = "移动";
                    break;
                case EnmMasterTaskType.SaveCar:
                    s = "存车";
                    break;
                case EnmMasterTaskType.TempGetCar:
                    s = "取物";
                    break;
                case EnmMasterTaskType.Transpose:
                    s = "搬移";
                    break;             
                default:
                    s = mType.ToString();
                    break;
            }
            return s;
        }

        /// <summary>
        /// 子作业类型解析
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CtaskTypeFormat(EnmTaskType type) 
        {
            string s = "";
            switch (type) 
            {
                case EnmTaskType.HallCarEntrance:
                    s = "进车";
                    break;
                case EnmTaskType.HallCarExit:
                    s = "出车";
                    break;
                case EnmTaskType.TVGetCar:
                    s = "存车";
                    break;
                case EnmTaskType.TVSaveCar:
                    s = "取车";
                    break;
                case EnmTaskType.TVTranspose:
                    s = "搬移";
                    break;
                case EnmTaskType.TVLoad:
                    s = "装载";
                    break;
                case EnmTaskType.TVUnload:
                    s = "卸载";
                    break;
                case EnmTaskType.TVMove:
                    s = "移动";
                    break;
                case EnmTaskType.TVAvoid:
                    s = "避让";
                    break;
                default:
                    s = type.ToString();
                    break;
            }
            return s;
        }

        /// <summary>
        /// 设备种类解析
        /// </summary>
        /// <param name="eqp"></param>
        /// <returns></returns>
        public static string CtaskEqpFormat(int eqp) 
        {
            string ss = "";
            switch (eqp) 
            {
                case 1:
                    ss = "ETV1";
                    break;
                case 2:
                    ss = "ETV2";
                    break;
                case 11:
                    ss = "Hall1";
                    break;
                case 12:
                    ss = "Hall2";
                    break;
                case 13:
                    ss = "Hall3";
                    break;
                case 14:
                    ss = "Hall4";
                    break;
                default:
                    ss = eqp.ToString();
                    break;
            }
            return ss;
        }
      
    }
}

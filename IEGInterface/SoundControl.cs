using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace IEGInterface
{
    public class SoundControl
    {        
        private List<Device> devs = new List<Device>();        
        private string cycleSoundFile;
        private SecondaryBuffer buf;
        private string[] hallsSound=new string[4];  //

        public SoundControl() 
        {           
           
        }
        /// <summary>
        /// 查找服务器上的声卡设备
        /// </summary>
        public void SearchSoundDevice()
        {
            cycleSoundFile = IEGInterface.Properties.Settings.Default.CycleSound;
            try
            {
                DevicesCollection devColl = new DevicesCollection();
                Form frm = new Form();
                //正常情况下，服务器自带一个声卡设备，故设备号从1开始
                for (int i = 1; i < devColl.Count; i++)
                {
                    DeviceInformation info=(DeviceInformation)devColl[i];
                    if (string.IsNullOrEmpty(info.ModuleName)) 
                    {
                        continue;
                    }
                    CWSException.WriteLog("声卡-"+(i-1).ToString()+"    描述："+info.Description,4);
                    Guid Dguid = info.DriverGuid;
                    Device dev = new Device(Dguid);
                    dev.SetCooperativeLevel(frm, CooperativeLevel.Normal);
                    devs.Add(dev);
                }
            }
            catch (Exception ex) 
            {
                CWSException.WriteError(ex.ToString());
            }
        }
        /// <summary>
        /// 发送数据至相应的声卡设备
        /// </summary>
        /// <param name="hallID">车厅号</param>
        /// <param name="soundCom">声卡端口</param>
        public void MakeSound(int hallID,int soundCom) 
        {
            try
            {
                if (devs.Count >= soundCom)
                {
                    int idx = hallID - 11;
                    //从Web服务上获取相应车厅的声音文件,待写
                    string soundFile = Program.mng.GetCurrentSound(hallID);
                    //声音归类
                    if (soundFile != null)
                    {
                        hallsSound[idx] = soundFile;   //将各个车厅的声音进行集合
                    }
                    else
                    {
                        if (hallsSound[idx] == null)   //各个车厅没有要播放的声音
                        {
                            return;
                        }
                        //循环播放的声音文件
                        string[] sounds = cycleSoundFile.Split(new char[] { ',' });
                        if (!sounds.Contains(hallsSound[idx].Substring(0, hallsSound[idx].Length-4)))
                        {
                            return;
                        }
                        else
                        {
                            Thread.Sleep(12000); //循环播放的间隔
                        }
                    }

                    //强制结束语音
                    if (hallsSound[idx] == "end")
                    {
                        hallsSound[idx] = null;
                        return;
                    }
                    try
                    {
                        //播放语音
                        string path = Application.StartupPath + @"\sound\" + hallsSound[idx];
                        if (File.Exists(path))
                        {
                            if (buf != null)
                            {
                                buf.Stop();
                                buf.Dispose();
                                buf = null;
                            }
                            using (BufferDescription desc = new BufferDescription())
                            {
                                desc.Flags = BufferDescriptionFlags.GlobalFocus;
                                buf = new SecondaryBuffer(path, desc, devs[soundCom]);   //向指定声卡号soundCom播放语音
                                buf.Play(0, BufferPlayFlags.Default);

                                CWSException.WriteLog(hallID.ToString() + "      " + "声卡-" + soundCom.ToString() + "    语音：" + hallsSound[idx], 2);
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            CWSException.WriteError("缺少音频文件：" + path);
                        }
                    }
                    catch (Exception e1) 
                    {
                        string mess = "异常1：" + e1.ToString();
                        try 
                        {
                            buf.Dispose();
                            buf = null;
                        }
                        catch (Exception e2) 
                        {
                            mess += "     异常2：" + e2.ToString();
                        }
                        throw new Exception(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                CWSException.WriteError("车厅－" + hallID.ToString() + "  发声时异常：" + ex.ToString());
            }
        }
    }
}

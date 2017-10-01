using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace IEGInterface
{
    public static class CWSException
    {
        private static  readonly string Logpath = Application.StartupPath + IEGInterface.Properties.Settings.Default.LogPath;
        private static  readonly string Errorpath = Application.StartupPath + IEGInterface.Properties.Settings.Default.ErrorPath;

        /// <summary>
        /// 记录日志 0-报文接收，1-报文发送，2-语音，3-刷卡，4-其他
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public static void WriteLog(object log,int type) 
        {
            try
            {
                string slog = " ";
                if (type == 0 || type == 1)
                {
                    StringBuilder strBuild = new StringBuilder();
                    short[] data = (short[])log;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (i % 10 == 0 && i != 0)
                        {
                            strBuild.Append(Environment.NewLine + "[" + data[i] + "]");
                        }
                        else
                        {
                            strBuild.Append("[" + data[i] + "]");
                        }
                    }
                    slog = strBuild.ToString();
                }
                else
                {
                    slog = log.ToString();
                }

                string fileName = Logpath + "\\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";
                StreamWriter sw = null;
                if (File.Exists(fileName) == false)
                {
                    sw = File.CreateText(fileName);
                }
                else
                {
                    FileInfo fi = new FileInfo(fileName);
                    long len = fi.Length;
                    if (len < 4 * Math.Pow(1024, 2))
                    {
                        sw = File.AppendText(fileName);
                    }
                }

                if (sw != null)
                {
                    switch (type)
                    {
                        case 0:
                            sw.WriteLine(DateTime.Now + "     " + "报文接收" + Environment.NewLine + slog);
                            break;
                        case 1:
                            sw.WriteLine(DateTime.Now + "     " + "报文发送" + Environment.NewLine + slog);
                            break;
                        case 2:
                            sw.WriteLine(DateTime.Now + "     " + "语音" + Environment.NewLine + slog);
                            break;
                        case 3:
                            sw.WriteLine(DateTime.Now + "     " + "刷卡" + Environment.NewLine + slog);
                            break;
                        default:
                            sw.WriteLine(DateTime.Now + "     " + "其他" + Environment.NewLine + slog);
                            break;
                    }
                }
                sw.Close();
                sw.Dispose();

            }
            catch (IOException)
            {
                try
                {
                    if (Directory.Exists(Logpath) == false)
                    {
                        Directory.CreateDirectory(Logpath);
                    }
                }
                catch
                { }
            }
            catch 
            {

            }
        }

        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="message">异常事件信息</param>
        public static void WriteError(string message) 
        {
            try
            {
                string fileName = Errorpath + "\\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";
                StreamWriter sw = null;
                if (File.Exists(fileName) == true)
                {
                    FileInfo fi = new FileInfo(fileName);
                    long leg = fi.Length;
                    if (leg < 4 * Math.Pow(1024, 2))
                    {
                        sw = File.AppendText(fileName);
                    }
                }
                else
                {
                    sw = File.CreateText(fileName);
                }
                if (sw != null)
                {
                    sw.WriteLine(DateTime.Now.ToString() + "      " + "异常：" + Environment.NewLine + message);
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException)
            {
                try
                {
                    if (Directory.Exists(Errorpath) == false)
                    {
                        Directory.CreateDirectory(Errorpath);
                    }
                }
                catch
                { }
            }
            catch
            {

            }
        }
    }
}

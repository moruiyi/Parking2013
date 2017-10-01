using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IEGLedInterface
{
    public static class CWSException
    {      
        private static  readonly string Errorpath = Application.StartupPath + IEGLedInterface.Properties.Settings.Default.ErrorPath;
       
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
                catch { }
            }
            catch { }
        }
    }
}

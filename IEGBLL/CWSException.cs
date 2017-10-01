using System;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Configuration;
using System.Collections;


namespace IEGBLL
{
    public class CWSException : Exception
    {
        private Exception moEx;
        private static readonly string path = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["Logpath"];
        private static readonly string LPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["lpath"];

        public CWSException()
        {
            lock (HttpRuntime.Cache)
            {
                System.Web.Caching.Cache _cache = HttpRuntime.Cache;
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                ArrayList al = new ArrayList();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
                foreach (string key in al)
                {
                    _cache.Remove(key);
                }
            }
        }

        public override string ToString()
        {
            return moEx.ToString();
        }

        public CWSException(Exception e1)
        {
            try
            {
                moEx = e1;
                string logPath = path + @"\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";
                StreamWriter sw = null;
                if (File.Exists(logPath) == true)
                {
                    FileInfo fi = new FileInfo(logPath);
                    long len = fi.Length;

                    if (len < 2 * Math.Pow(1024, 2))
                    {
                        sw = File.AppendText(logPath);
                    }
                    else 
                    {
                        return;
                    }              
                }
                else
                {
                    sw = File.CreateText(logPath);
                }

                if (sw != null)
                {
                    sw.WriteLine(DateTime.Now + "     " + e1.ToString());
                    sw.Close();
                    sw.Dispose();
                }

            }
            catch (IOException)
            {
                try
                {
                    if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                }
                catch
                {
                }
            }
            catch
            {

            }
        }
      

        /// <summary>
        /// 记录日志，0-作业记录，1-数据库
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public CWSException(string message, int type) 
        {
            try
            {
                string logPath = LPath + @"\" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";
                StreamWriter sw = null;
                if (File.Exists(logPath) == true)
                {
                    FileInfo fi = new FileInfo(logPath);
                    long len = fi.Length;

                    if (len < 4 * Math.Pow(1024, 2))
                    {
                        sw = File.AppendText(logPath);
                    }
                    else 
                    {
                        return;
                    }                  
                }
                else
                {
                    sw = File.CreateText(logPath);
                }

                if (sw != null)
                {
                    switch (type)
                    {
                        case 0:
                            sw.WriteLine(DateTime.Now + "     " + "作业更新" + Environment.NewLine + message);
                            break;
                        case 1:
                            sw.WriteLine(DateTime.Now + "     " + "数据库更新" + Environment.NewLine + message);
                            break;
                        default:
                            sw.WriteLine(DateTime.Now + "     " + "其它" + Environment.NewLine + message);
                            break;
                    }

                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException)
            {
                try
                {
                    if (System.IO.Directory.Exists(LPath) == false)//如果不存在就创建file文件夹
                    {
                        System.IO.Directory.CreateDirectory(LPath);
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
        }

    }
}

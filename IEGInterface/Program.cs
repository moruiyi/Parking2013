using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IEGInterface.localhost;
using System.ServiceProcess;

namespace IEGInterface
{
    static class Program
    {
        public static WSMng mng;
        public static List<int> TaskList;
        private static int sysModel = Properties.Settings.Default.SystemModel;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            mng = new WSMng();
            TaskList = new List<int>();

            if (sysModel == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmTest());
            }
            else if (sysModel == 0)
            {
                //WINDOWS服务启动
                try 
                {
                    ServiceBase[] servicesToRun;
                    servicesToRun = new ServiceBase[] { new EqpService()};
                    ServiceBase.Run(servicesToRun);
                }
                catch (Exception ex) 
                {
                    try
                    {
                        CWSException.WriteError(ex.ToString());
                    }
                    catch { }
                }
            }
            else 
            {
                MessageBox.Show("参数SystemModel需要配置");
            }
        }
    }
}

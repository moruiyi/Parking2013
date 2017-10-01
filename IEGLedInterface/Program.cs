using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IEGLedInterface.localhost;
using System.ServiceProcess;

namespace IEGLedInterface
{
    static class Program
    {

        public static WSMng mng;
        private static int sysModel = Properties.Settings.Default.SysModel;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            mng = new WSMng();
            if (sysModel == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmLed());
            }
            else if (sysModel == 0)
            {
                //WINDOWS服务启动
                try
                {
                    ServiceBase[] servicesToRun;
                    servicesToRun = new ServiceBase[] { new LedEqpService() };
                    ServiceBase.Run(servicesToRun);
                }
                catch (Exception ex)
                {
                    try
                    {
                        CWSException.WriteError("服务开启异常：" + ex.ToString());
                    }
                    catch { }
                }
            }
            else 
            {
                MessageBox.Show("请配置sysModel为O或1");
            }
        }
    }
}

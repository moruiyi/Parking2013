using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IEGConsole.localhost;

namespace IEGConsole
{
    static class Program
    {
        public static WSMng mng;
        public static COperator currOpr;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                mng = new WSMng();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());               
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法连接服务端："+ex.ToString());
            }
        }
    }
}

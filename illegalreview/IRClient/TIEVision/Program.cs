using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using System.Threading;
using TIEVision.COMMON;

namespace TIEVision
{
    static class Program
    {
        static ApplicationContext MainContext = new ApplicationContext();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Dark");//DevExpress Dark Style  Visual Studio 2013 Dark  Sharp Plus  Metropolis Dark
            MainContext.MainForm = new FrmUserLogin();
            Application.Run(MainContext);
        }

        public static void InitApp(Object parm)
        {
            FrmStartup startup = parm as FrmStartup;
            startup.Invoke(new UiThreadProc(startup.PrintMsg), "正在初始化...");
            Thread.Sleep(100);
            startup.Invoke(new UiThreadProc(startup.PrintMsg), "正在加载资源...");
            Thread.Sleep(100);
            //startup.Invoke(new UiThreadProc(startup.PrintMsg), "用户身份验证成功。");
            //Thread.Sleep(500);
            startup.Invoke(new UiThreadProc(startup.PrintMsg), "加载成功，欢迎使用！");
            
            if (true)
            {
                startup.Invoke(new UiThreadProc(startup.CloseForm), false);
            }
            else
            {
                startup.Invoke(new UiThreadProc(startup.CloseForm), true);
            }

        }

        public delegate void UiThreadProc(Object o);

        public static void SetMainForm(Form MainForm)
        {
            MainContext.MainForm = MainForm;
        }

        public static void ShowMainForm()
        {
            MainContext.MainForm.Show();
        }
        public static void HideMainForm()
        {
            MainContext.MainForm.Hide();
        }

        
    }
}
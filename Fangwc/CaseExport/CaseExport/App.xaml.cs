using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CaseExport
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            //if the client is run, then close the new client application
            Process process = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(process.ProcessName))
            {
                if (p.Id != process.Id)
                {
                    Shutdown();
                    return;
                }
            }

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }


        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception);
            e.Handled = true;
        }

        void HandleException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            CurrentData.Instance.Logger.Error(ex);
            MessageBox.Show(ex.Message);
            if (ex.InnerException != null)
            {
                CurrentData.Instance.Logger.Error(ex.InnerException);
                MessageBox.Show(ex.InnerException.Message);
            }

        }

    }
}

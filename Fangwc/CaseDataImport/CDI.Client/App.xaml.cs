using Common.Logging;
using CDI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace CDI.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
            this.SessionEnding += App_SessionEnding;
            this.Exit += App_Exit;

        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            //if the client is run, then close the new client application
            //Process process = Process.GetCurrentProcess();
            //foreach (Process p in Process.GetProcessesByName(process.ProcessName))
            //{
            //    if (p.Id != process.Id)
            //    {
            //        Shutdown();
            //        return;
            //    }
            //}

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
                //check mac address
                var errMsg = "地址验证失败,非法访问!";
                var macAddr = MacHelper.GetLocalMac();
                if (string.IsNullOrEmpty(macAddr))
                {
                    MessageBox.Show(errMsg);
                    this.Shutdown();
                    return;
                }
                IProxy proxyServer = CurrentData.Instance.ProxyServer;
                var result = proxyServer.ValidateAddress(new AddressRequestModel() { MacAddress = macAddr });
                if (result.Status != 1)
                {
                    var info = result.Message ?? errMsg;
                    MessageBox.Show(info);
                    this.Shutdown();
                    return;
                }

                LoginWindow loginWin = new LoginWindow();
                loginWin.ShowDialog();
                if (!loginWin.DialogResult.Value)
                {
                    this.Shutdown();
                    return;
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
                this.Shutdown();
            }
        }

        void App_Exit(object sender, ExitEventArgs e)
        {
            CurrentData.Instance.LogOff();
        }

        void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            CurrentData.Instance.LogOff();
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

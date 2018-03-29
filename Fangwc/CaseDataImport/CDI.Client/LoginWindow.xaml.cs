using Common.Logging;
using CDI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CDI.Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        IProxy proxyServer = CurrentData.Instance.ProxyServer;
        ILog logger = CurrentData.Instance.Logger;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            try
            {
                LoginRequestModel loginModel = new LoginRequestModel();
                loginModel.UserName = txtUserName.Text.Trim();
                loginModel.Password = txtPwd.Password;
                loginModel.MacAddress = MacHelper.GetLocalMac();
                var result = proxyServer.Login(loginModel);
                if (result.Status == 1)
                {
                    CurrentData.Instance.UserName = loginModel.UserName;
                    CurrentData.Instance.Token = new TokenRequestModel() { Token = result.Token };
                    DialogResult = true;
                }
                else
                {
                    var info = result.Message ?? "登录失败!";
                    ShowError(info);
                    txtUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ShowError(ex.Message);
            }
            btnLogin.IsEnabled = true;
        }

        private Tuple<bool, string> TestConnect()
        {
            bool result;
            string resp;
            try
            {
                string str = "test proxy!";
                resp = proxyServer.Echo(str);
                logger.Debug(resp);
                result = resp.Contains(str);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                resp = ex.Message;
                result = false;
            }
            return new Tuple<bool, string>(result, resp);
        }

        private void btnTestConnect_Click(object sender, RoutedEventArgs e)
        {
            var r = TestConnect();
            if (r.Item1)
            {
                MessageBox.Show("测试连接成功!\r\n" + r.Item2, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ShowError("测试连接失败!\r\n" + r.Item2);
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

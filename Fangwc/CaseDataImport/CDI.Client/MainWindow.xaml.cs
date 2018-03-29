using CDI.Models;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CDI.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            cityDict.Add("北京市", 1);
            cityDict.Add("上海市", 2);
            cityDict.Add("天津市", 3);
            cityDict.Add("重庆市", 4);
            cityDict.Add("深圳市", 6);
            cityDict.Add("广州市", 7);
            cityDict.Add("杭州市", 74);
            cityDict.Add("苏州市", 88);
            cityDict.Add("成都市", 115);
            cityDict.Add("其它所有城市", -1);
        }

        IProxy proxyServer = CurrentData.Instance.ProxyServer;
        ILog logger = CurrentData.Instance.Logger;
        bool isWaiting;
        CancellationTokenSource tokenSource;
        Dictionary<string, int> cityDict = new Dictionary<string, int>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            txtLog.Text = string.Empty;
            dpEndDate.SelectedDate = DateTime.Now;
            dpBeginDate.SelectedDate = dpEndDate.SelectedDate.Value.AddDays(-30);
            var s = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = string.Format("{0}-v{1}", Title, s);
            cmbCity.ItemsSource = cityDict.Keys;
        }
        
        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            isWaiting = false;
            txtLog.Text = string.Empty;
            btnStart.IsEnabled = false;
            dpBeginDate.IsEnabled = false;
            dpEndDate.IsEnabled = false;
            cmbCity.IsEnabled = false;
            btnCancel.IsEnabled = true;
            string startDate = dpBeginDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string endDate = dpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            try
            {
                if (cmbCity.SelectedIndex < 0)
                {
                    ShowWarning("请选择城市！");
                    return;
                }
                if (CheckRecord(startDate, endDate))
                {
                    tc1.SelectedIndex = 0;
                    txtLog.Text += "操作取消!";
                    return;
                }
                string sCityName = cmbCity.SelectedItem.ToString();
                int sCityId = cityDict[sCityName];
                var cityIds = cityDict.Values.TakeWhile(c => c != -1).ToArray();
                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                var task = Task.Factory.StartNew<Dictionary<string, int>>(() => MainThread.RunMain(startDate, endDate, sCityId, cityIds, token), token);
                await task.ContinueWith((t) =>
                {
                    string errMsg = null;
                    var aex = t.Exception as AggregateException;
                    if (aex != null)
                    {
                        if (aex.InnerException != null)
                            errMsg = aex.InnerException.Message;
                        logger.Error(aex.InnerException ?? aex);
                    }
                    else
                    {
                        logger.Error(t.Exception);
                    }
                    Dictionary<string, int> totalCityCase = t.Result;
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        btnCancel.IsEnabled = false;
                        btnStart.IsEnabled = true;
                        dpBeginDate.IsEnabled = true;
                        dpEndDate.IsEnabled = true;
                        cmbCity.IsEnabled = true;
                        if (!string.IsNullOrWhiteSpace(errMsg))
                            txtLog.Text += errMsg + "\r\n";
                        if (isWaiting)
                            txtLog.Text += "操作取消!";
                        if (totalCityCase != null && totalCityCase.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine();
                            sb.AppendFormat("城市名称\t入库案例数量");
                            sb.AppendLine();
                            foreach (var item in totalCityCase)
                            {
                                sb.AppendFormat("{0}\t{1}", item.Key, item.Value);
                                sb.AppendLine();
                            }
                            txtLog.Text += sb.ToString();
                            tc1.SelectedIndex = 0;
                        }
                        SetEnabled(true);
                    }));
                    tokenSource.Dispose();
                    tokenSource = null;
                });
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                btnCancel.IsEnabled = false;
                btnStart.IsEnabled = true;
                dpBeginDate.IsEnabled = true;
                dpEndDate.IsEnabled = true;
                cmbCity.IsEnabled = true;
            }
        }

        private void Cancel()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                btnCancel.IsEnabled = false;
                SetEnabled(false);
                if (!string.IsNullOrEmpty(txtLog.Text))
                {
                    txtLog.Text += "\r\n";
                }
                txtLog.Text += "等待操作取消中......";
            }
        }

        private bool CheckRecord(string startDate, string endDate)
        {
            var r = FindImportData(startDate, endDate);
            tc1.SelectedIndex = 1;
            dgRecord.DataContext = r;
            if (r != null && r.Rows.Count > 0)
            {
                var dr = MessageBox.Show("已经存在导入记录，请查看，需要继续导入吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dr == MessageBoxResult.No)
                {
                    return true;
                }
            }
            return false;
        }


        private void SetEnabled(bool status)
        {
            this.IsEnabled = status;
            this.isWaiting = !status;
            if (this.isWaiting)
            {
                Cursor = Cursors.Wait;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cancel();
            if (isWaiting)
            {
                e.Cancel = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CurrentData.Instance.LogOff();
            App.Current.Shutdown();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            string startDate = dpBeginDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string endDate = dpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            dgRecord.DataContext = FindImportData(startDate, endDate);
        }

        private DataTable FindImportData(string startDate, string endDate)
        {
            string pageSql = string.Format("SELECT top 100 *  FROM [FxtData_Case].[dbo].[ImportRecord] with (nolock) where [CaseBeginDate]>='{0}' and [CaseBeginDate]<='{1}' or [CaseEndDate]>='{0}' and [CaseEndDate]<='{1}'", startDate, endDate);
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["FxtData_Case"];
            DbProviderFactory factory = null;
            DbConnection conn = null;
            DbCommand command = null;
            try
            {
                factory = DbProviderFactories.GetFactory(connSettings.ProviderName);
                conn = factory.CreateConnection();
                conn.ConnectionString = connSettings.ConnectionString;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                #region read table
                //读取100条
                command.CommandText = pageSql;
                command.CommandTimeout = 200;
                using (DbDataAdapter reader = factory.CreateDataAdapter())
                {
                    reader.SelectCommand = command;
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    reader.Dispose();
                    return ds.Tables[0];
                }
                #endregion
            }
            finally
            {
                #region dispose
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
                #endregion
            }

        }

        

    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Windows.Threading;
using System.Threading;


namespace CaseExport
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Fields

        const string DATA_PROJECT_TABLE = "入库记录表";
        const string DATA_EXCEPTION_TABLE = "异常数据表";
        bool isWaiting;
        Dictionary<int, object> cityIds = new Dictionary<int, object>();
        string excelDir;
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbTable.Items.Add(DATA_PROJECT_TABLE);
            cmbTable.Items.Add(DATA_EXCEPTION_TABLE);
            cmbTable.SelectedIndex = 0;
            var dt = DateTime.Now;
            dpEndDate.SelectedDate = dt;
            dpBeginDate.SelectedDate = dt.AddMonths(-1);
            pageNextGrid.Visibility = Visibility.Collapsed;
            //创建Excel导出目录
            excelDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excel");
            if (!Directory.Exists(excelDir))
            {
                Directory.CreateDirectory(excelDir);
            }
        }

        private void cmbTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string citySql = "select 城市表.CityId as CityId, 城市表.CityName as CityName FROM [FxtDataCenter].[dbo].[SYS_City] as 城市表 inner join (select distinct [ID] FROM [FxtData_Case].[dbo].[ImportRecord]) as m on m.[ID]=城市表.[CityId]";
            if (cmbTable.SelectedIndex == 1)
            {
                citySql = "select 城市表.CityId as CityId, 城市表.CityName as CityName FROM [FxtDataCenter].[dbo].[SYS_City] as 城市表 inner join (select distinct [城市ID] FROM [FxtData_Case].[dbo].[住宅案例_出售_异常案例]) as m on m.[城市ID]=城市表.[CityId]";
            }
            var table = DbHelper.QueryTable(citySql);
            cmbCity.DataContext = table;
            if (CurrentData.Instance.QueryResult.ContainsKey(cmbTable.SelectedIndex))
            {
                UpdateStatus(CurrentData.Instance.QueryResult[cmbTable.SelectedIndex]);
            }
            else
            {
                pageNextGrid.Visibility = Visibility.Collapsed;
            }
            if (cityIds.ContainsKey(cmbTable.SelectedIndex))
            {
                cmbCity.SelectedValue = cityIds[cmbTable.SelectedIndex];                
            }
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            PageQuery(1);
        }

        private void PageQuery(int pageNumber)
        {
            if (cmbCity.SelectedIndex == -1)
            {
                ShowWarning("请选择城市!");
                cmbCity.Focus();
                return;
            }
            cityIds[cmbTable.SelectedIndex] = cmbCity.SelectedValue;
            TableQueryResponseModel response = new TableQueryResponseModel();
            CurrentData.Instance.QueryResult[cmbTable.SelectedIndex] = response;
            SetEnabled(false);
            try
            {
                var inputObj = GetTableQueryCriteria(cmbTable.SelectedIndex, pageNumber, (int)cmbCity.SelectedValue, dpBeginDate.SelectedDate.Value, dpEndDate.SelectedDate.Value);
                DbHelper.PageQueryTable(inputObj, response);
                response.Status = 1;
                dgResult.DataContext = response.Result;
                UpdateStatus(response);
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Message = ex.Message;
                CurrentData.Instance.Logger.Error(ex);
                ShowError(ex.Message);
            }
            SetEnabled(true);
        }

        private void UpdateStatus(TableQueryResponseModel response)
        {
            if (response == null)
            {
                pageNextGrid.Visibility = Visibility.Collapsed;
                return;
            }
            int totalPages = response.TotalPage;
            if (totalPages > 0)
            {
                lblTotalPages.Content = string.Format("总页数:{0}", totalPages);
                lblCurrentPage.Content = string.Format("当前页:{0}", response.PageNumber);
                lblTotalCount.Content = string.Format("总记录数:{0}", response.TotalCount);
                txtPageNumber.Text = response.PageNumber.ToString();
                btnPreview.IsEnabled = response.PageNumber > 1 ? true : false;
                btnNext.IsEnabled = response.PageNumber < totalPages ? true : false;
                btnGoPage.IsEnabled = totalPages > 1 ? true : false;
                pageNextGrid.Visibility = Visibility.Visible;
            }
            else
            {
                pageNextGrid.Visibility = Visibility.Collapsed;
            }
        }

        private TableQueryCriteria GetTableQueryCriteria(int selectedIndex, int pageNumber, int cityId, DateTime beginDate, DateTime endDate)
        {
            TableQueryCriteria queryCriteria = new TableQueryCriteria();
            queryCriteria.DBServer = "FxtData_Case";
            queryCriteria.DBName = "FxtData_Case";
            queryCriteria.PageNumber = pageNumber;
            if (selectedIndex == 0)
            {
                queryCriteria.TableName = "ImportRecord";
                queryCriteria.DefaultOrderByColumn = "ID";
                queryCriteria.Where = string.Format("[ID]={0} and ([CaseBeginDate]>='{1}' and [CaseBeginDate]<='{2}' or [CaseEndDate]>='{1}' and [CaseEndDate]<='{2}')", cityId, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                queryCriteria.TableName = "住宅案例_出售_异常案例";
                queryCriteria.DefaultOrderByColumn = "城市ID";
                queryCriteria.Where = string.Format("[城市ID]={0} and [案例时间]>='{1}' and [案例时间]<='{2}'", cityId, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            }

            return queryCriteria;
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            int p = CurrentData.Instance.QueryResult[cmbTable.SelectedIndex].PageNumber;
            PageQuery(--p);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int p = CurrentData.Instance.QueryResult[cmbTable.SelectedIndex].PageNumber;
            PageQuery(++p);
        }

        private void btnGoPage_Click(object sender, RoutedEventArgs e)
        {
            int pageNumber;
            bool r = int.TryParse(txtPageNumber.Text, out pageNumber);
            int totalPage = CurrentData.Instance.QueryResult[cmbTable.SelectedIndex].TotalPage;
            if (!r || pageNumber < 1 || pageNumber > totalPage)
            {
                MessageBox.Show(string.Format("请输入1-{0}之间的页码数!", totalPage), "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPageNumber.Focus();
                return;
            }
            PageQuery(pageNumber);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCity.SelectedIndex == -1)
            {
                ShowWarning("请选择城市!");
                cmbCity.Focus();
                return;
            }
            var dr = MessageBox.Show("确定导出数据到Excel吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dr != MessageBoxResult.Yes)
            {
                return;
            }
            string fileName;
            string dataSql;
            int cityId = (int)cmbCity.SelectedValue;
            DateTime beginDate=dpBeginDate.SelectedDate.Value;
            DateTime endDate=dpEndDate.SelectedDate.Value;
            string tableName;
            string cityName = (cmbCity.SelectedItem as DataRowView)["CityName"].ToString();
            if (cmbTable.SelectedIndex == 0)
            {
                tableName = DATA_PROJECT_TABLE;
                fileName = string.Format("{0}-{1}-{2:yyyyMMdd}-{3:yyyyMMdd}.xlsx", DATA_PROJECT_TABLE, cityName, beginDate, endDate);
                dataSql = string.Format("select * FROM [FxtData_Case].[dbo].[ImportRecord] WHERE [ID]={0} and ([CaseBeginDate]>='{1}' and [CaseBeginDate]<='{2}' or [CaseEndDate]>='{1}' and [CaseEndDate]<='{2}')", cityId, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                tableName = DATA_EXCEPTION_TABLE;
                fileName = string.Format("{0}-{1}-{2:yyyyMMdd}-{3:yyyyMMdd}.xlsx", DATA_EXCEPTION_TABLE, cityName, beginDate, endDate);
                dataSql = string.Format("select 城市表.CityId as CityId, 城市表.CityName as CityName, 异常案例表.* FROM [FxtData_Case].[dbo].[住宅案例_出售_异常案例] as 异常案例表 left join [FxtDataCenter].[dbo].[SYS_City] as 城市表 on 异常案例表.[城市ID]=城市表.[CityId] WHERE 异常案例表.[城市ID]={0} and 异常案例表.[案例时间]>='{1}' and 异常案例表.[案例时间]<='{2}'", cityId, beginDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            }
            fileName = System.IO.Path.Combine(excelDir, fileName);

            SetEnabled(false);
            try
            {
                var result = DbHelper.QueryTable(dataSql);
                if (result.Rows.Count == 0)
                {
                    ShowWarning("查询没有符合条件的数据,不能导出!");
                    return;
                }
                result.TableName = tableName;
                if (cmbTable.SelectedIndex > 0)
                {
                    result.Columns.Remove("城市");
                    result.Columns.Remove("城市ID");
                }
                ExcelHelper.Export(result, fileName);
                ShowWarning("导出成功,文件名:" + fileName);
            }
            catch (Exception ex)
            {
                CurrentData.Instance.Logger.Error(ex);
                ShowError(ex.Message);
            }
            finally
            {
                SetEnabled(true);
            }
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

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        
    }
}

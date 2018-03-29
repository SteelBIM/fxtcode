using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FxtSpider.Bll.SpiderCommon.Models;
using FxtSpider.Bll;
using FxtSpider.DAL.LinqToSql;
using System.Reflection;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.IO;
using log4net;
using FxtSpider.Common;
using FxtSpider.Bll.SpiderCommon;
using System.Data.OleDb;
using System.Diagnostics;
using FxtSpider.Bll.ProxyIpCommom;
using System.Collections;

namespace FxtSpiderDataGetWindowsForms
{
    public partial class FxtSpiderData : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FxtSpiderData));
        private delegate void SetPos(int ipos); 
        private string excelSaveFileName = "";
        public FxtSpiderData()
        {
            InitializeComponent();
        }


        private void FxtSpiderData_Load(object sender, EventArgs e)
        {
            //SpiderHelp.GetHtml("http://ts.cityhouse.cn/forsale/flist.html?ob=10&page=4", "utf-8");
            dTime选择开始时间.Format = DateTimePickerFormat.Custom;
            dTime选择结束时间.Format = DateTimePickerFormat.Custom;
            dTime选择开始时间.CustomFormat = "yyyy-MM-dd";
            dTime选择结束时间.CustomFormat = "yyyy-MM-dd";
            //CityManager.GetCityIdByCityName("深圳");
            dataGridView1.DataSource = new object();
            lbl记录数.Text = "";
            初始化DataGridView的列数据源名称绑定();
            绑定所有城市();
            绑定所有网站();
            设置DataGridView适应窗口大小();
            panelExcel.Visible = false;
            panelImport.Visible = false;

        }
        private void btn查询_Click(object sender, EventArgs e)
        {

            //查询案例();
            Thread fThread = new Thread(new ThreadStart(查询案例));//开辟一个新的线程
            fThread.Start();
        }
        private void FxtSpiderData_Resize(object sender, EventArgs e)
        {
            设置DataGridView适应窗口大小();
        }
        //直接导出excel
        private void btnExcel2_Click(object sender, EventArgs e)
        {

            if (!验证查询条件())
            {
                return;
            }
            string 选择城市Name = cboxCity.Text.ToString();
            string 选择网站Name = cb网站.Text.ToString();
            string 选择开始时间 = Convert.ToDateTime(dTime选择开始时间.Text).ToString("yyyy-MM-dd");
            string 选择结束时间 = Convert.ToDateTime(dTime选择结束时间.Text).ToString("yyyy-MM-dd");
            DateTime 开始时间 = Convert.ToDateTime(选择开始时间 + " 00:00:00");
            DateTime 结束时间 = Convert.ToDateTime(选择结束时间 + " 23:59:59");
            string _excelSaveFileName = string.Format("{0}_{1}_({2}_{3})", 选择城市Name, 选择网站Name, 选择开始时间, 选择结束时间);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel(*.xls)|*.xls";
                saveDialog.AddExtension = true;
                saveDialog.FileName = _excelSaveFileName;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string saveFilePath = saveDialog.FileName;
                    string[] strings = saveFilePath.Split('.');
                    StringBuilder sb = new StringBuilder();
                    for (int i=0;i<strings.Length;i++)
                    {
                        sb.Append(strings[i]);
                        if (i == (strings.Length - 2))
                        {
                            sb.Append("_({0})");
                        }
                        if (i < (strings.Length - 1))
                        {
                            sb.Append(".");
                        }
                    } 
                     



                    excelSavePath = sb.ToString();
                    Thread fThread = new Thread(new ThreadStart(导出到Excel操作));//开辟一个新的线程
                    fThread.Start();
                    //导出到Excel操作();
                }
            }
            

        }


        #region 方法
        private bool 验证查询条件()
        {
            if (cboxCity.SelectedValue == null)
            {
                MessageBox.Show("请选择城市");
                return false;
            }
            string 选择城市 = cboxCity.SelectedValue.ToString();
            string 选择网站 = cb网站.SelectedValue.ToString();
            string 选择开始时间 = dTime选择开始时间.Text;
            string 选择结束时间 = dTime选择结束时间.Text;
            if (选择城市.Equals("-1"))
            {
                MessageBox.Show("请选择城市");
                return false;
            }
            if (string.IsNullOrEmpty(选择开始时间))
            {
                MessageBox.Show("请选择日期");
                return false;
            }
            if (string.IsNullOrEmpty(选择结束时间))
            {
                MessageBox.Show("选择结束时间");
                return false;
            }
            DateTime 开始时间 = Convert.ToDateTime(选择开始时间 + " 00:00:00");
            DateTime 结束时间 = Convert.ToDateTime(选择结束时间 + " 23:59:59");
            if (开始时间 > 结束时间)
            {
                MessageBox.Show("请选择正确的结束时间(开始时间不能晚于开始时间)");
                return false;
            }

            TimeSpan midTime = 结束时间 - 开始时间;
            int day = midTime.Days;
            if (day > 64)
            {
                MessageBox.Show("选择的间隔天数不能大于64天");
                return false;
            }
            return true;
        }
        private void 查询案例()
        {
            if (!验证查询条件())
            {
                return;
            }
            string 选择城市 = cboxCity.SelectedValue.ToString();
            string 选择网站 = cb网站.SelectedValue.ToString();
            string 选择开始时间 = dTime选择开始时间.Text;
            string 选择结束时间 = dTime选择结束时间.Text;
            excelSaveFileName = string.Format("{0}_{1}_({2}_{3})", 选择城市, 选择网站, 选择开始时间, 选择结束时间);
            DateTime 开始时间 = Convert.ToDateTime(选择开始时间 + " 00:00:00");
            DateTime 结束时间 = Convert.ToDateTime(选择结束时间 + " 23:59:59");
            lbl记录数.Text = "数据查询中.....";
            panel查询条件.Enabled = false;
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            int count = 0;
            string message = "";
            if (选择网站.Equals("-1"))
            {
                list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(Convert.ToInt32(选择城市), null, 开始时间, 结束时间, 1, 40000, true, out count, out message);
            }
            else
            {
                list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(Convert.ToInt32(选择城市), Convert.ToInt32(选择网站), 开始时间, 结束时间, 1, 40000, true, out count, out message);
            }
            list.ForEach(delegate(VIEW_案例信息_城市表_网站表 _obj) {

                string projectName = "";
                string areaName = "";
                string subAreaName = "";
                if (_obj.ProjectName == null)
                {
                    if (!string.IsNullOrEmpty(_obj.楼盘名))
                    {
                        projectName = _obj.楼盘名;
                    }
                }
                else
                {
                    projectName = _obj.ProjectName;
                }
                if (_obj.AreaName == null)
                {
                    if (!string.IsNullOrEmpty(_obj.行政区))
                    {
                        areaName = _obj.行政区;
                    }
                }
                else
                {
                    areaName = _obj.AreaName;
                }
                if (_obj.SubAreaName == null)
                {
                    if (!string.IsNullOrEmpty(_obj.片区))
                    {
                        subAreaName = _obj.片区;
                    }
                }
                else
                {
                    subAreaName = _obj.SubAreaName;
                }
                _obj.楼盘名 = projectName;
                _obj.行政区 = areaName;
                _obj.片区 = subAreaName;
            });
            lbl记录数.Text = string.Format("共{0}条记录", count);
            dataGridView1.DataSource = list;
            panel查询条件.Enabled = true;
        }
        private string excelSavePath = "";
        private void 导出到Excel操作()
        {
            lbl记录数.Text = "数据查询中.....";
            int pageLength = 40000;
            int start = 1;
            int count = 0;
            int pageCount = 0;
            //禁用其他按钮
            panel查询条件.Enabled = false;
            btnExcel2.Enabled = false;
            btnExcel2.Visible = false;
            //获取查询条件
            string 选择城市Id = cboxCity.SelectedValue.ToString();
            string 选择网站Id = cb网站.SelectedValue.ToString();
            string 选择城市Name = cboxCity.SelectedText.ToString();
            string 选择网站Name = cb网站.SelectedText.ToString();
            string 选择开始时间 = Convert.ToDateTime(dTime选择开始时间.Text).ToString("yyyy-MM-dd");
            string 选择结束时间 = Convert.ToDateTime(dTime选择结束时间.Text).ToString("yyyy-MM-dd"); 
            DateTime 开始时间 = Convert.ToDateTime(选择开始时间 + " 00:00:00");
            DateTime 结束时间 = Convert.ToDateTime(选择结束时间 + " 23:59:59");
            //查询数据
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            int 城市ID = Convert.ToInt32(选择城市Id);
            int? 网站ID = null;
            if (!选择网站Id.Equals("-1"))
            {
                网站ID = Convert.ToInt32(选择网站Id);
            }
            string _message = "";
            list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(城市ID, 网站ID, 开始时间, 结束时间, start, pageLength, true, out count, out _message);
            if (!string.IsNullOrEmpty(_message))
            {
                MessageBox.Show(_message);
            }


            DialogResult result = MessageBox.Show(string.Format("查询到总记录数{0}条,点击确定继续生成excel",count.ToString()),"消息" , MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                pageCount = ((count - 1) / pageLength) + 1;
                //进度条设置
                panelExcel.Visible = true;
                panelExcel.Top = btnExcel2.Top;
                panelExcel.Left = btnExcel2.Left;
                progressBar进度条.Maximum = count;
                progressBar进度条.Minimum = 0;
                string msg = "";
                将数据保存到Excel(string.Format(excelSavePath, start), list, out msg);
                try
                {
                    for (int i = start; i <= pageCount; i++)
                    {
                        //System.Threading.Thread.Sleep(1600);
                        if (i == start)
                        {
                            continue;
                        }
                        int _count = 0;
                        list = CaseManager.GetVIEW案例信息_根据城市网站案例日期的区间获取案例(城市ID, 网站ID, 开始时间, 结束时间, i, pageLength, false, out _count, out _message);
                        if (list == null || list.Count <1)
                        {
                            string message = string.Format("由于系统异常,当前第{0}个文件生成失败,总共需生成Excel文件{1}个,成功生成{2}个,点击'重试'则重新生成当前文件",
                                               i, pageCount, i - 1);
                            DialogResult result2 = MessageBox.Show(message, "消息", MessageBoxButtons.RetryCancel);
                            if (result2 == DialogResult.Retry)
                            {
                                i = i - 1;
                                continue;
                            }
                            else
                            {
                                progressBar进度条.Value = progressBar进度条.Maximum;
                                lbl百分比.Text = 计算百分比(progressBar进度条.Value, progressBar进度条.Maximum);
                                break;
                            }
                        }
                        将数据保存到Excel(string.Format(excelSavePath, i), list, out msg);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("系统异常:"), ex);
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("成功");
            }
            //进度条设置
            panelExcel.Visible = false;
            btnExcel2.Enabled = true;
            btnExcel2.Visible = true;
            panel查询条件.Enabled = true;
            lbl记录数.Text = "";
        }
        private bool 将数据保存到Excel(string path,List<VIEW_案例信息_城市表_网站表> list, out string message)
        {
            message = "";
            bool result = false;
            try
            {
                //Excel导出
                StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    sb.Append(dataGridView1.Columns[i].HeaderText + "\t");
                }
                sb.Append(Environment.NewLine);
                for (int i = 0; i < list.Count; i++)
                {
                    progressBar进度条.Value = progressBar进度条.Value + 1;
                    lbl百分比.Text = 计算百分比(progressBar进度条.Value, progressBar进度条.Maximum);
                    //根据城市的字典提取关键字
                    list[i].信息 = WinFormCommon.GetCaseRemark(list[i].城市, list[i].信息);
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        string value = "";
                        Type type = list[i].GetType();
                        if (dataGridView1.Columns[j].DataPropertyName == "楼盘名")
                        {
                            string projectName = "";
                            if (list[i].ProjectName == null)
                            {
                                if (!string.IsNullOrEmpty(list[i].楼盘名))
                                {
                                    projectName = list[i].楼盘名;
                                }
                            }
                            else
                            {
                                projectName = list[i].ProjectName;
                            }
                            value = projectName;
                        }
                        else if (dataGridView1.Columns[j].DataPropertyName == "行政区")
                        {
                            string areaName = "";
                            if (list[i].AreaName == null)
                            {
                                if (!string.IsNullOrEmpty(list[i].行政区))
                                {
                                    areaName = list[i].行政区;
                                }
                            }
                            else
                            {
                                areaName = list[i].AreaName;
                            }
                            value = areaName;
                        }
                        else if (dataGridView1.Columns[j].DataPropertyName == "片区")
                        {
                            string subAreaName = "";
                            if (list[i].SubAreaName == null)
                            {
                                if (!string.IsNullOrEmpty(list[i].片区))
                                {
                                    subAreaName = list[i].片区;
                                }
                            }
                            else
                            {
                                subAreaName = list[i].SubAreaName;
                            }
                            value = subAreaName;
                        }
                        else
                        {
                            PropertyInfo property = type.GetProperty(dataGridView1.Columns[j].DataPropertyName);
                            if (property != null)
                            {
                                object objValue = property.GetValue(list[i], null);
                                if (objValue != null)
                                {
                                    value = Convert.ToString(objValue);
                                }
                            }
                        }
                        sb.Append(value + "\t");
                    }
                    sb.Append(Environment.NewLine);

                }

                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                result = true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("系统异常:文件可能正在被使用!"), ex);
                message = "文件可能正在被使用!";
            }
            return result;
        }
        private void 初始化DataGridView的列数据源名称绑定()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns["Column楼盘名"].DataPropertyName = "楼盘名";
            dataGridView1.Columns["Column案例时间"].DataPropertyName = "案例时间";
            dataGridView1.Columns["Column行政区"].DataPropertyName = "行政区";
            dataGridView1.Columns["Column片区"].DataPropertyName = "片区";
            dataGridView1.Columns["Column楼栋"].DataPropertyName = "楼栋";
            dataGridView1.Columns["Column房号"].DataPropertyName = "房号";
            dataGridView1.Columns["Column用途"].DataPropertyName = "SysData用途";
            dataGridView1.Columns["Column面积"].DataPropertyName = "面积";
            dataGridView1.Columns["Column单价"].DataPropertyName = "单价";
            dataGridView1.Columns["Column案例类型"].DataPropertyName = "SysData案例类型";
            dataGridView1.Columns["Column结构"].DataPropertyName = "SysData结构";
            dataGridView1.Columns["Column建筑类型"].DataPropertyName = "SysData建筑类型";
            dataGridView1.Columns["Column总价"].DataPropertyName = "总价";
            dataGridView1.Columns["Column所在楼层"].DataPropertyName = "所在楼层";
            dataGridView1.Columns["Column总楼层"].DataPropertyName = "总楼层";
            dataGridView1.Columns["Column户型"].DataPropertyName = "SysData户型";
            dataGridView1.Columns["Column朝向"].DataPropertyName = "SysData朝向";
            dataGridView1.Columns["Column装修"].DataPropertyName = "SysData装修";
            dataGridView1.Columns["Column建筑年代"].DataPropertyName = "建筑年代";
            dataGridView1.Columns["Column信息"].DataPropertyName = "信息";
            dataGridView1.Columns["Column电话"].DataPropertyName = "电话";
            dataGridView1.Columns["ColumnURL"].DataPropertyName = "URL";
            dataGridView1.Columns["Column币种"].DataPropertyName = "SysData币种";
            dataGridView1.Columns["Column地址"].DataPropertyName = "地址";
            dataGridView1.Columns["Column创建时间"].DataPropertyName = "创建时间";
            dataGridView1.Columns["Column来源"].DataPropertyName = "来源";
            dataGridView1.Columns["Column建筑形式"].DataPropertyName = "建筑形式";
            dataGridView1.Columns["Column花园面积"].DataPropertyName = "花园面积";
            dataGridView1.Columns["Column厅结构"].DataPropertyName = "厅结构";
            dataGridView1.Columns["Column车位数量"].DataPropertyName = "车位数量";
            dataGridView1.Columns["Column配套设施"].DataPropertyName = "配套设施";
            dataGridView1.Columns["Column地下室面积"].DataPropertyName = "地下室面积";
            dataGridView1.Columns["Column城市"].DataPropertyName = "城市";
            dataGridView1.DataSource = new List<VIEW_案例信息_城市表_网站表>();
        }
        private void 绑定所有城市()
        {
            List<城市表> list = CityManager.GetSpiderCity();
            城市表 obj = new 城市表 { ID = -1, 城市名称 = "请选择", 省份ID = -1 };
            list.Insert(0, obj);
            cboxCity.DisplayMember = "城市名称";
            cboxCity.ValueMember = "ID";
            cboxCity.DataSource = list;

        }
        private void 绑定所有网站()
        {
            List<网站表> list = new List<网站表>();// WebsiteManager.所有网站;           
            网站表 obj = new 网站表 { ID = -1, 网站名称 = "全部"};
            list.Insert(0, obj);
            cb网站.DisplayMember = "网站名称";
            cb网站.ValueMember = "ID";
            //cb网站.DataSource = list;

        }
        private void 设置DataGridView适应窗口大小()
        {
            dataGridView1.Top = 80;
            dataGridView1.Left = 10;
            dataGridView1.Height = this.Height - 150;
            dataGridView1.Width = this.Width - 35;
        }
        private string 计算百分比(int nowValue, int maxValue)
        {
            double a = Convert.ToDouble(nowValue) / Convert.ToDouble(maxValue);
            a = Math.Round(a, 2);
            string str = (a * 100).ToString() + "%";
            return str;
        }
        #endregion
        private void cboxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectCityId = Convert.ToString(cboxCity.SelectedValue);
            if (!string.IsNullOrEmpty(selectCityId) && StringHelp.IsInteger(selectCityId))
            {
                List<网站表> list = WebsiteManager.GetWebByCityId(Convert.ToInt32(selectCityId));
                网站表 obj = new 网站表 { ID = -1, 网站名称 = "全部" };
                list.Insert(0, obj);
                cb网站.DataSource = list;
            }
        }
        //导入代理IP
        private void btnImportIp_Click(object sender, EventArgs e)
        {
            string fileName = "";//excel路径
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openDialog.FileName;
                }
            }
            DialogResult result = MessageBox.Show("导入代理IP时为了监测IP是否有效请保持本机的网络正常!","消息" , MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Thread m_thread = new Thread(new ParameterizedThreadStart(ImportIp));
                m_thread.Start(fileName);
                //ImportIp(fileName);
            }

        }

        public void ImportIp(object param)
        {
            string fileName = Convert.ToString(param);
            int count = 0;//成功个数
            //禁用其他按钮
            panel查询条件.Enabled = false;
            btnExcel2.Enabled = false;
            btnImportIp.Visible = false;
            //进度条位置设置
            panelImport.Visible = true;
            panelImport.Top = btnImportIp.Top;
            panelImport.Left = btnImportIp.Left;
            labelImportBar.Text = "0%";
            progressBarImportIp.Value = 0;
            SysData_ProxyIp existsObj = new SysData_ProxyIp();
            string message = "";
            object missing = System.Reflection.Missing.Value;
            Excel.Application excel = new Excel.Application();
            if (excel == null)
            {
                MessageBox.Show("Can't access excel");
            }
            else
            {

                excel.Visible = false; excel.UserControl = true;
                // 以只读的形式打开EXCEL文件
                Excel.Workbook wb = excel.Application.Workbooks.Open(fileName, missing, true, missing, missing, missing,
                 missing, missing, missing, true, missing, missing, missing, missing, missing);
                //取得第一个工作薄
                Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets.get_Item(1);
                //取得总记录行数
                int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数
                //设置进度条范围
                progressBarImportIp.Maximum = rowsint;
                progressBarImportIp.Minimum = 0;
                //取得数据范围区域 (从第一行第一列到最后一行第二列)
                Excel.Range rng1 = ws.Cells.get_Range("A1", "B" + rowsint);   //item
                object[,] arryItem = (object[,])rng1.Value2;
                for (int i = 1; i <= rowsint - 1; i++)
                {
                    string ip = arryItem[i, 1].ToString();
                    string ipArea = arryItem[i, 2].ToString();
                    int result = ProxyIpHelp.ImportProxyIp(ip, ipArea, out existsObj, out message);
                    if (result == 1)
                    {
                        count = count + 1;
                    }

                    progressBarImportIp.Value = progressBarImportIp.Value + 1;
                    labelImportBar.Text = 计算百分比(progressBarImportIp.Value, progressBarImportIp.Maximum);
                }
                progressBarImportIp.Value = progressBarImportIp.Maximum;
                labelImportBar.Text = 计算百分比(progressBarImportIp.Value, progressBarImportIp.Maximum);
            }
            excel.Quit(); excel = null;
            Process[] procs = Process.GetProcessesByName("excel");
            foreach (Process pro in procs)
            {
                pro.Kill();//没有更好的方法,只有杀掉进程
            }
            GC.Collect();
            MessageBox.Show(string.Format("导入完成,成功导入可用的代理IP{0}个",count));
            //进度条设置
            panelImport.Visible = false;
            btnExcel2.Enabled = true;
            btnImportIp.Visible = true;
            panel查询条件.Enabled = true;
            labelImportBar.Text = "";
        }

        private void cboxCity_TextUpdate(object sender, EventArgs e)
        {
        }

        private void cboxCity_TextChanged(object sender, EventArgs e)
        {
        }
        //用于在输入城市名称后自动匹配的下拉框城市ID和自动联动网站下拉框
        private void cboxCity_Leave(object sender, EventArgs e)
        {
            string 选择城市Name = cboxCity.Text.ToString();
            List<城市表> list = cboxCity.DataSource as List<城市表>;
            城市表 obj = list.Where(_obj => _obj.城市名称 == 选择城市Name).FirstOrDefault();
            if (obj != null)
            {
                cboxCity.SelectedValue = obj.ID;
            }
            else
            {
                cboxCity.SelectedValue = -1;
            }
        }
    }
}

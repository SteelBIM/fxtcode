using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace FXTExcelAddIn
{
    /// <summary>
    /// 城市选择控件
    /// </summary>
    public partial class UC_Case : UCBase
    {
        private const string _tempProjectsSheetName = "temp_Projects";
        private const string _tempAreaSheetName = "temp_Areas";
        /// <summary>
        /// 数据库楼盘总数
        /// </summary>
        private int _projectCount=0;

        public UC_Case()
        {
            InitializeComponent();
        }


        private void CitySelect_Load(object sender, EventArgs e)
        {
            
        }

        protected void SetStatus(EnumHelper.LabelStatus status, string str)
        {
            SetLabelStatus(status, txtStatus, str);
        }
       
        
        /// <summary>
        /// 下载城市楼盘
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagerecords"></param>
        /// <param name="projects"></param>
        /// <param name="done"></param>
        private void DownLoadProjects(int pageindex,int pagerecords, List<Project> projects,out bool done)
        {
            done = false;
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(FxtCommon.SignName, "1003104", "108746028", "855190548");
            data.sinfo.functionname = "plist";// "splist";"allplist"
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = FxtAddIn.CityID,
                key = "",
                pageindex = pageindex,
                pagerecords = pagerecords
            };
            string str = data.GetJsonString();
            try
            {
                string list = FxtCommon.APIPostBack(FxtCommon.API_Datacenter, str);
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取楼盘列表
                    string strdata = rtn.data.ToString();
                    //StreamWriter write = File.CreateText(@"c:\windows\temp\temp_projects.txt");
                    //write.Write(strdata);
                    //write.Close();
                    List<Project> cp = JSONHelper.JSONStringToList<Project>(strdata);
                    if(cp.Count>0)
                        projects.AddRange(cp);// = projects.Concat(cp).ToList();
                    if (cp.Count < pagerecords)
                    {
                        done = true;
                    }
                    else
                    {                        
                        pageindex++;
                        DownLoadProjects(pageindex, pagerecords, projects, out done);
                    }
                }
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
            }
            
        }

        private List<Area> DownloadAreas()
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            JSONHelper.JsonData data = new JSONHelper.JsonData();
            data.sinfo = new SecurityInfo(FxtCommon.SignName, "1003104", "108746028", "855190548");
            data.sinfo.functionname = "garealist";
            data.info.funinfo = new
            {
                fxtcompanyid = 25,
                typecode = 1003302,
                cityid = FxtAddIn.CityID,
            };
            string str = data.GetJsonString();
            List<Area> areas = new List<Area>();
            try
            {
                string list = FxtCommon.APIPostBack(FxtCommon.API_Datacenter, str);
                JSONHelper.ReturnData rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(list);
                if (rtn.returntype > 0)
                {
                    //获取行政区列表
                    string strdata = rtn.data.ToString();
                    areas = JSONHelper.JSONStringToList<Area>(strdata);                    
                }
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
            }
            return areas;
        }

        /// <summary>
        /// 下载楼盘、处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownProjectList_Click(object sender, EventArgs e)
        {
            if (FxtAddIn.CityID <= 0)
            {
                MessageBox.Show("请先选择城市");
                return;
            }
            //禁止控件使用
            EnableControls(false);
            //下载数据
            int pageindex = 1;
            int pagerecords = 500;
            bool done = false;
            List<Project> projects = new List<Project>();
            try 
            {
                SetTimeStatus(txtStatus, "下载行政区", true);
                //下载行政区
                List<Area> areas = DownloadAreas();
                if (areas.Count > 0)
                {
                    FxtAddIn.GoToSheet(_tempAreaSheetName);
                    FxtRibbon.GetUsedRange().Clear();
                    int _areaCount = areas.Count;
                    //有数据
                    if (_areaCount > 1)
                    {
                        dynamic[,] ary = new dynamic[_areaCount, 2];
                        for (int i = 0; i < _areaCount; i++)
                        {
                            ary[i, 0] = areas[i].areaname;
                            ary[i, 1] = areas[i].areaname.Length;
                            //worksheet.Cells[i + 1, 1].Value = projects[i].projectname;
                        }
                        Range rg = FxtAddIn.FxtWorkSheet.get_Range("A1", "B" + _areaCount.ToString());
                        rg.Value2 = ary;
                        rg.Sort(rg.Cells[1, 2], XlSortOrder.xlDescending);
                        rg.Columns.AutoFit();
                        SetStatus(EnumHelper.LabelStatus.Success, string.Format("共下载{0}个行政区", _areaCount));
                    }
                    else
                    {
                        SetStatus(EnumHelper.LabelStatus.Faild, string.Format("共下载{0}个行政区", 0));
                    }                    
                }
                SetTimeStatus(txtStatus, "下载行政区", false);
                SetTimeStatus(txtStatus, "下载楼盘", true);
                //下载楼盘
                DownLoadProjects(pageindex, pagerecords, projects,out done);
                if (done)
                {
                    FxtAddIn.GoToSheet(_tempProjectsSheetName);
                    FxtRibbon.GetUsedRange().Clear();
                    _projectCount = projects.Count;
                    //有数据
                    if (_projectCount > 1)
                    {
                        dynamic[,] ary = new dynamic[_projectCount, 2];
                        for (int i = 0; i < _projectCount; i++)
                        {
                            ary[i, 0] = projects[i].projectname;
                            ary[i, 1] = projects[i].projectname.Length;
                            //worksheet.Cells[i + 1, 1].Value = projects[i].projectname;
                        }
                        Range rg = FxtAddIn.FxtWorkSheet.get_Range("A1", "B" + _projectCount.ToString());
                        rg.Value2 = ary;
                        //_projectCount = FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count;                    
                        //按照字符串长度倒序排序（便于含一期二期等同楼盘处理，先处理名称长的，避免短名称覆盖长名称）
                        //Range rg = FxtAddIn.FxtWorkSheet.get_Range("b1", "b" + _projectCount.ToString());
                        //rg.Cells[1, 1].Formula = "=Len(A1)";
                        //rg.FillDown();
                        //排序
                        //rg = FxtAddIn.FxtWorkSheet.get_Range("a1", "b" + _projectCount.ToString());
                        rg.Sort(rg.Cells[1, 2], XlSortOrder.xlDescending);
                        rg.Columns.AutoFit();
                        SetStatus(EnumHelper.LabelStatus.Success, string.Format("共下载{0}个楼盘", _projectCount));
                        lblProjectStatus.Visible=true;
                    }
                    else
                    {
                        SetStatus(EnumHelper.LabelStatus.Faild, string.Format("共下载{0}个楼盘", 0));
                    }                    
                }
                //恢复控件使用
                EnableControls(true);
                SetTimeStatus(txtStatus, "下载楼盘", false);
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
                btnDownProjectList.Enabled = true;
            }
        }

        private void CitySelect_VisibleChanged(object sender, EventArgs e)
        {

        }        

        /// <summary>
        /// 标准化楼盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            FxtAddIn.EnableAppWindow(false);
            //标准化行政区
            FxtAddIn.GoToSheet(_tempAreaSheetName);
            int areaCount = FxtRibbon.GetLastRow();                       
            Range rg = FxtAddIn.FxtWorkSheet.get_Range("a1", "a" + areaCount.ToString());
            //用数组代替range循环，性能更高，注意这样的数组下标是从1,1开始的
            //如果用数组转置方法统一下标从1开始，有65535的下标限制
            var areas = rg.Value;
            if (areas != null)
            {                                
                //处理，回到sheet1
                FxtAddIn.GoToSheet(1);
                //回到sheet1才能调用取消锁定，这里一定要注意
                FxtAddIn.FxtApp.ActiveWindow.FreezePanes = false;
                //取消筛选，显示全部数据
                FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
                int caseCount = FxtRibbon.GetLastRow();
                //开始替换
                SetTimeStatus(txtStatus, "标准化行政区", true);
                Dictionary<string, string> cols = FxtRibbon.FindCol();
                rg = FxtAddIn.FxtWorkSheet.get_Range(cols["行政区"] + "1");
                rg = rg.get_Resize(caseCount, 1);
                for (int i = 1; i <= areaCount; i++)
                {
                    string sKey = areas[i, 1];
                    if (sKey == null || sKey.Trim().Length == 0) continue;                   
                    rg.Replace("*" + sKey.Replace("区","").Replace("县","") + "*", sKey, Type.Missing, Type.Missing, false, Type.Missing, false, false);
                }
                rg = FxtRibbon.GetUsedRange();
                //在锁定前要处理ScreenUpdating=true，否则会出现锁定错误，一定要注意
                FxtAddIn.EnableAppWindow(true);
                SetStatus(EnumHelper.LabelStatus.Success, "标准化行政区完成");
                SetTimeStatus(txtStatus, "标准化行政区", false);
            }
            //取楼盘数据源
            FxtAddIn.GoToSheet(_tempProjectsSheetName);
            _projectCount = FxtRibbon.GetLastRow();
            if (_projectCount <= 0) {
                MessageBox.Show("无楼盘数据");
                return;
            }
            rg = FxtAddIn.FxtWorkSheet.get_Range("a1", "a" + _projectCount.ToString());            
            //用数组代替range循环，性能更高，注意这样的数组下标是从1,1开始的
            //如果用数组转置方法统一下标从1开始，有65535的下标限制
            var projects = rg.Value;
            if (projects != null)
            {
                //处理，回到sheet1
                FxtAddIn.GoToSheet(1);
                //回到sheet1才能调用取消锁定，这里一定要注意
                FxtAddIn.FxtApp.ActiveWindow.FreezePanes = false;
                //取消筛选，显示全部数据
                //if(FxtWorkSheet.FilterMode)
                //    FxtWorkSheet.ShowAllData();
                FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
                int caseCount = FxtRibbon.GetLastRow();
                //COPY一列备用
                Range rg1 = FxtAddIn.FxtWorkSheet.get_Range("A1", "A" + caseCount.ToString());
                rg1.EntireColumn.Insert(XlInsertShiftDirection.xlShiftDown, false);
                rg1.Copy(FxtAddIn.FxtWorkSheet.get_Range("A1"));
                FxtAddIn.FxtWorkSheet.Cells[1, 1].Value2 = "原楼盘名称";
                //开始替换
                SetTimeStatus(txtStatus, "标准化楼盘", true);
                var cases = rg1.Value;
                //使用新数组代替辅助列处理，测试10万数据速度一样
                dynamic[,] newCases = new dynamic[caseCount, 1];
                newCases[0, 0] = "楼盘名称";
                //多线程处理
                for (int i = 1; i <= _projectCount; i++)
                {
                    string sKey = projects[i, 1];
                    if (sKey == null || sKey.Trim().Length == 0) continue;
                    //findnext方法效率太低，放弃
                    //Range f = rg1.Find("*" + sKey + "*");
                    //while (f != null)
                    //{
                    //    f.Value2 = sKey;
                    //    f = rg1.FindNext(f);
                    //}
                    //rg1.Replace("*" + sKey + "*", "OK_" + sKey, Type.Missing, Type.Missing, false, Type.Missing, false, false);    
                    //使用数组，效率高，且灵活                
                    //下标从2开始（第二行）
                    for (int j = 2; j <= caseCount; j++)
                    {
                        if (cases[j, 1].ToString().IndexOf("OK_") < 0
                            && cases[j, 1].ToString().IndexOf(sKey) >= 0)
                        {
                            cases[j, 1] = "OK_" + sKey;
                            newCases[j - 1, 0] = sKey;
                        }
                    }
                }
                rg1.Value2 = newCases;
                /*//使用新数组代替辅助列处理，测试10万数据速度一样
                rg1.Value2 = cases;
                //添加辅助列，设定公式
                rg1.EntireColumn.Insert(XlInsertShiftDirection.xlShiftDown, false);
                FxtAddIn.FxtWorkSheet.Cells[1, 2].Formula = "=IF(ISNUMBER(FIND(\"OK_\",C1)),REPLACE(C1,1,3,\"\"),\"\")";
                Range rg2 = FxtAddIn.FxtWorkSheet.get_Range("B1", "B" + caseCount.ToString());
                rg2.FillDown();
                //去掉公式
                rg2.Value = rg2.Value;
                //删除过渡列
                FxtAddIn.FxtWorkSheet.Columns[3].Delete();
                 * */
                rg = FxtRibbon.GetUsedRange();
                //自动列宽
                //FxtAddIn.FxtWorkSheet.Columns.AutoFit();
                //在锁定前要处理ScreenUpdating=true，否则会出现锁定错误，一定要注意
                FxtAddIn.EnableAppWindow(true);
                //锁定行头
                FxtAddIn.FxtWorkSheet.get_Range("B2").Select();//.Cells[2, 2].Select();            
                //FxtAddIn.FxtApp.ActiveWindow.SplitRow = 1;
                //FxtAddIn.FxtApp.ActiveWindow.SplitColumn = 1;
                FxtAddIn.FxtApp.ActiveWindow.FreezePanes = true;
                //筛选为空的，需要再次人工处理
                rg.AutoFilter(2, "=");
                rg = FxtAddIn.FxtWorkSheet.get_Range("a2", "a" + caseCount.ToString());
                int leftRows = rg.SpecialCells(XlCellType.xlCellTypeVisible).Count;
                string status = string.Format("共处理{0}条数据\n匹配成功{1}条\n待处理{2}条",
                    caseCount - 1, caseCount - leftRows - 1,leftRows);
                SetStatus(EnumHelper.LabelStatus.Success, status);
                SetTimeStatus(txtStatus, "标准化楼盘", false);
                lblBZHProject.Visible = true;
                FxtAddIn.FxtApp.ActiveWindow.SmallScroll(Type.Missing, FxtAddIn.FxtApp.ActiveWindow.ScrollRow);
            }
            //恢复控件使用
            EnableControls(true);
            MessageBox.Show("处理完成！");
        }
        
        private void btnGetProvine_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGetCity_Click(object sender, EventArgs e)
        {
           
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnDeleteProjects_Click(object sender, EventArgs e)
        {
            FxtAddIn.DeleteSheet(_tempProjectsSheetName);
            FxtAddIn.DeleteSheet(_tempAreaSheetName);
        }

        /// <summary>
        /// 案例整理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear2_Click(object sender, EventArgs e)
        {
            //Dictionary<string,string> headcols = FxtRibbon.FindCol();            
            /*判断重复案例的字段：楼盘名、行政区、楼层、总楼层、用途、建筑面积、单价、总价、朝向、建筑类型、户型
             * */
            //headcols = headcols.Where(o => "楼盘名称,行政区,楼层,总楼层,用途,建筑面积,单价,总价,朝向,建筑类型,户型".Split(new char[] { ',' }).Contains(o.Key)).ToDictionary(o=>o.Key,o=>o.Value);
            //去除重复
            List<string> headcols = "楼盘名称,行政区,楼层,总楼层,用途,建筑面积,单价,总价,朝向,建筑类型,户型".Split(new char[] { ',' }).ToList();
            FxtAddIn.GoToSheet(1);
            try
            {
                SetTimeStatus(txtStatus, "清理重复案例", true);
                //取消筛选，显示全部数据
                FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
                Range rg = FxtRibbon.GetUsedRange();
                int oCnt = rg.Rows.Count-1;
                dynamic[] cols = new dynamic[11];
                dynamic[,] colnames = rg.Value2;
                int colindex = 0;
                for (int i = 1; i <= rg.Columns.Count; i++)
                {
                    if (headcols.Contains(colnames[1, i].ToString().Replace("*", "")))
                    {
                        cols[colindex] = i;
                        colindex++;
                    }
                }
                rg.RemoveDuplicates(cols, XlYesNoGuess.xlYes);
                rg = FxtRibbon.GetUsedRange();
                rg.Select();
                //string status = string.Format("共处理{0}条数据\n共剩下{1}条数据\n共清理{2}条重复数据",oCnt,rg.Rows.Count-1,oCnt-(rg.Rows.Count-1));
                string status = string.Format("共清理{0}条重复数据",  oCnt - (rg.Rows.Count - 1));
                SetStatus(EnumHelper.LabelStatus.Success, status);
                lblClearCase.Visible = true;
                FxtAddIn.FxtApp.ActiveWindow.SmallScroll(Type.Missing, FxtAddIn.FxtApp.ActiveWindow.ScrollRow);
                SetTimeStatus(txtStatus, "清理重复案例", false);
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
            }
        }

        /// <summary>
        /// 标准化建筑类型
        /// </summary>
        public bool BiaoZhunHua_Type()
        {
            try
            {
                SetTimeStatus(txtStatus, "标准化建筑类型", true);
                Dictionary<string, string> dict = FxtRibbon.FindCol();
                //取消筛选，显示全部数据
                FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
                string col1 = dict["总楼层"];
                string col2 = dict["配套"];
                string col = dict["建筑类型"];
                string formula = string.Format(Dict.MathDict["建筑类型"], col1, col2, col);
                //添加辅助列
                Range rg = FxtAddIn.FxtWorkSheet.Cells[2, FxtAddIn.FxtWorkSheet.UsedRange.Columns.Count + 1];
                rg.Value = formula;
                rg = rg.get_Resize(FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count - 1, 1);
                rg.FillDown();
                dynamic[,] vals = rg.Value2;
                Range rg1 = FxtAddIn.FxtWorkSheet.get_Range(col + "2", col + FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count.ToString());
                rg1.Value = vals;
                rg.EntireColumn.Delete();
                SetTimeStatus(txtStatus, "标准化建筑类型", false);
                return true;
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 标准化用途
        /// </summary>
        public bool BiaoZhunHua_Use()
        {
            try
            {
                SetTimeStatus(txtStatus, "标准化用途", true);
                Dictionary<string, string> dict = FxtRibbon.FindCol();
                //取消筛选，显示全部数据
                FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
                string col1 = dict["总楼层"];
                string col2 = dict["建筑面积"];
                string col3 = dict["建筑类型"];
                string col = dict["用途"];
                string formula = string.Format(Dict.MathDict["用途"], col3, col2, col1);
                //添加辅助列
                Range rg = FxtAddIn.FxtWorkSheet.Cells[2, FxtAddIn.FxtWorkSheet.UsedRange.Columns.Count + 1];
                rg.Value = formula;
                rg = rg.get_Resize(FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count - 1, 1);
                rg.FillDown();
                dynamic[,] vals = rg.Value2;
                Range rg1 = FxtAddIn.FxtWorkSheet.get_Range(col + "2", col + FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count.ToString());
                rg1.Value = vals;
                rg.EntireColumn.Delete();
                SetTimeStatus(txtStatus, "标准化用途", false);
                return true;
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 标准化建筑类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (BiaoZhunHua_Type()) lblBZHType.Visible = true;
        }

        private void UC_Case_Load(object sender, EventArgs e)
        {
            lblBZHProject.Visible = false;
            lblBZHType.Visible = false;
            lblBZHUse.Visible = false;
            lblClearUse.Visible = false;
            lblProjectStatus.Visible = false;
            lblClearCase.Visible = false;
        }

        /// <summary>
        /// 标准化用途
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (BiaoZhunHua_Use()) lblBZHUse.Visible = true;
        }

  
        /// <summary>
        /// 清理用途、建筑类型偏差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearUse_Click(object sender, EventArgs e)
        {
            try { 
            SetTimeStatus(txtStatus, "清理价格偏离", true);
            Dictionary<string,string> dict = FxtRibbon.FindCol();
            //FxtAddIn.FxtWorkSheet.get_Range(dict.Last().Value + "1")
            //取消筛选，显示全部数据
            FxtAddIn.FxtWorkSheet.AutoFilterMode = false;
            string col1 = dict["单价"];
            string col2 = dict["楼盘名称"];
            string col3 = dict["行政区"];
            string col4 = dict["用途"];
            string col5 = dict["总价"];
            string col6 = dict["建筑面积"];
            string col7 = dict["建筑类型"];
            FxtAddIn.EnableAppWindow(false);
            
            //均价
            /*OLEDB引起打开新文件的问题，暂时抛弃
            ADODB.Connection conn = new ADODB.Connection();
            conn.Open("provider=Microsoft.ACE.OLEDB.12.0;extended properties='Excel 12.0 Xml;HDR=YES; IMEX=1';Data Source = " + FxtAddIn.FxtWorkBook.FullName);
            //当已经有EXCEL文件打开时，会打开一个新文件，进程会归在之前打开的文件，应该是改了多进程后，只读打开文件会找进程ID，然后打开
            //string sql = "select 楼盘名称+行政区+用途,sum(val(总价)) / sum(val(建筑面积)) as price from [sheet1$] group by 楼盘名称+行政区+用途";
            string sql = "select 楼盘名称,sum(val(总价))  as price from [sheet1$] group by 楼盘名称";
            FxtAddIn.GoToSheet("testdb");
            object rows;//记录数，得0，不知道原因
            //ADODB.Recordset records = conn.Execute(sql, out rows,1);
            FxtAddIn.FxtWorkSheet.get_Range("A1").CopyFromRecordset(conn.Execute(sql,out rows,1));
            conn.Close();
            return;
            */
            
            //文本转数字，用数组效率高,union不能连续，这里要改
            FxtAddIn.GoToSheet(1);
            string sheetName = FxtAddIn.FxtWorkSheet.Name;
            int rowCount = FxtRibbon.GetLastRow();
            int colCount = FxtRibbon.GetLastColumn();
             //= FxtAddIn.FxtWorkSheet.get_Range(col5 + "2", col5 + rowCount);
            Range rg = FxtAddIn.FxtApp.Application.Union(FxtAddIn.FxtWorkSheet.get_Range(col6 + "2", col6 + rowCount.ToString()),
                FxtAddIn.FxtWorkSheet.get_Range(col1 + "2", col1 + rowCount.ToString()),
                FxtAddIn.FxtWorkSheet.get_Range(col5 + "2", col5 + rowCount.ToString()));
            dynamic[,] val = rg.Value2;
            for (int i = 1; i < rowCount; i++)
            {
                if (FxtCommon.IsNumeric(val[i, 1].ToString()))
                    val[i, 1] = Convert.ToDecimal(val[i, 1].ToString());
                else
                    val[i, 1] = 0;
                if (FxtCommon.IsNumeric(val[i, 2].ToString()))
                    val[i, 2] = Convert.ToDecimal(val[i, 2].ToString());
                else
                    val[i, 2] = 0; 
                if (FxtCommon.IsNumeric(val[i, 3].ToString()))
                    val[i, 3] = Convert.ToDecimal(val[i, 3].ToString());
                else
                    val[i, 3] = 0;
            }
            rg.Value2 = val;

            int pl = Convert.ToInt32(txtPrice.Text);
            //用途清理
            ClearPrice(col1, col2, col3, col4, col5, col6, pl);
            FxtAddIn.GoToSheet(1);
            //lblStatus.Text += FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count.ToString() + "\n";
            //建筑类型清理
            ClearPrice(col1, col2, col3, col7, col5, col6, pl);
            FxtAddIn.GoToSheet(1);
            //lblStatus.Text += FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count.ToString() + "\n";
            //楼盘清理
            pl = Convert.ToInt32(txtPrice1.Text);
            ClearPrice(col1, col2, null, col4, col5, col6, pl);
            FxtAddIn.GoToSheet(1);
            //lblStatus.Text += FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count.ToString() + "\n";
            
            SetStatus(EnumHelper.LabelStatus.Success, string.Format("共清理{0}条单价偏离数据",rowCount - FxtRibbon.GetLastRow()));
            lblClearUse.Visible = true;
            FxtAddIn.EnableAppWindow(true);
            FxtAddIn.FxtApp.ActiveWindow.SmallScroll(Type.Missing, FxtAddIn.FxtApp.ActiveWindow.ScrollRow);
            FxtRibbon.GetUsedRange().Select();
                SetTimeStatus(txtStatus, "清理价格偏离", false);
            /*透视表
            //数据源
            Range dataRangeForPivot = FxtAddIn.FxtWorkSheet.UsedRange;
            Excel.PivotCache pivotCache = FxtAddIn.FxtWorkBook.PivotCaches().Add(Excel.XlPivotTableSourceType.xlDatabase, dataRangeForPivot);

            FxtAddIn.GoToSheet(2);
            Range dataRangeForPivotTable = FxtAddIn.FxtWorkSheet.get_Range("A1"); 
            Excel.PivotTable pivotTable = pivotCache.CreatePivotTable(dataRangeForPivotTable, @"DTable", Type.Missing, Type.Missing);

            Excel.PivotField demoField = ((Excel.PivotField)pivotTable.PivotFields(1));
            demoField.Orientation =Excel.XlPivotFieldOrientation.xlRowField;
            demoField = ((Excel.PivotField)pivotTable.PivotFields(col4));
            demoField.Orientation =Excel.XlPivotFieldOrientation.xlDataField;
            demoField.Function = Excel.XlConsolidationFunction.xlCount;
            */
            /*
            Range rg = FxtAddIn.FxtWorkSheet.get_Range(col + "2");
            rg.Value = formula;
            rg.get_Resize(FxtAddIn.FxtWorkSheet.UsedRange.Rows.Count - 1, 1).FillDown();
            Range rg = FxtAddIn.FxtApp.Application.Union(FxtAddIn.FxtWorkSheet.get_Range("A1", "A20"),
                FxtAddIn.FxtWorkSheet.get_Range("d1", "d20"));
            rg.Select();
             * */
            }
            catch (Exception ex)
            {
                SetStatus(EnumHelper.LabelStatus.Faild, ex.Message);
            }
        }

        /// <summary>
        /// 价格偏离清理
        /// </summary>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="col3"></param>
        /// <param name="col4"></param>
        /// <param name="col5"></param>
        /// <param name="col6"></param>
        /// <param name="pl"></param>
        private void ClearPrice(string col1,string col2,string col3,string col4,string col5,string col6,int pl)
        {
            FxtAddIn.GoToSheet(1);
            string sheetName = FxtAddIn.FxtWorkSheet.Name;
            FxtAddIn.FxtWorkSheet.AutoFilterMode = false;   
            int rowCount = FxtRibbon.GetLastRow();
            int colCount = FxtRibbon.GetLastColumn();

            //辅助列，楼盘+行政区+用途
            Range rg = FxtAddIn.FxtWorkSheet.Cells[2, colCount + 1];
            if (col3 != null)
            {
                rg.Value = string.Format("=CONCATENATE({0}2,{1}2,{2}2)", col2, col3, col4);
            }
            else 
            {
                rg.Value = string.Format("=if({1}2=\"别墅\",\"\",{0}2)", col2, col4);
            }
            rg = rg.get_Resize(rowCount - 1, 1);
            rg.FillDown();
            string targetCol = rg.get_Address().Split(new char[] { '$' })[1].ToString();
            dynamic[,] val = rg.Value2;
            
            //开始算总价
            FxtAddIn.GoToSheet("testdb");
            rg = FxtAddIn.FxtWorkSheet.get_Range("A1", "A" + (rowCount - 1).ToString());
            rg.Value2 = val;
            //去重
            rg.RemoveDuplicates(1, XlYesNoGuess.xlNo);
            Range useRg = FxtRibbon.GetUsedRange();
            //计算总价
            rg = FxtAddIn.FxtWorkSheet.get_Range("B1");
            rg.Value = string.Format("=SUMIF({0}!${1}$2:${1}${2},testdb!A1,{0}!${3}$2:${3}${2})", sheetName, targetCol, rowCount, col5);
            rg = rg.get_Resize(useRg.Rows.Count, 1);
            rg.FillDown();
            rg.Value = rg.Value;
            //计算总面积
            rg = FxtAddIn.FxtWorkSheet.get_Range("C1");
            rg.Value = string.Format("=SUMIF({0}!${1}$2:${1}${2},testdb!A1,{0}!${3}$2:${3}${2})", sheetName, targetCol, rowCount, col6);
            rg = rg.get_Resize(useRg.Rows.Count, 1);
            rg.FillDown();
            rg.Value = rg.Value;
            //计算均价
            rg = FxtAddIn.FxtWorkSheet.get_Range("D1");
            rg.Value = "=B1/C1";
            rg = rg.get_Resize(useRg.Rows.Count, 1);
            rg.FillDown();
            rg.Value = rg.Value;

            FxtAddIn.GoToSheet(1);
            
            //辅助列，均价
            rg = FxtAddIn.FxtWorkSheet.Cells[2, colCount + 2];
            if (col3 != null)
            {
                rg.Value = string.Format("=VLOOKUP(CONCATENATE({0}2,{1}2,{2}2),testdb!A:D,4,FALSE)", col2, col3, col4);
            }
            else
            {
                rg.Value = string.Format("=VLOOKUP({0}2,testdb!A:D,4,FALSE)", col2);
            }
            rg = rg.get_Resize(rowCount - 1, 1);
            rg.FillDown();
            rg.Value = rg.Value;
            targetCol = rg.get_Address().Split(new char[] { '$' })[1];
            //辅助列，价差数
            rg = FxtAddIn.FxtWorkSheet.Cells[2, rg.Column + 1];
            rg.Value = string.Format("=ROUND(ABS(({0}2-{1}2)/{1}2),2)", col1, targetCol);
            rg = rg.get_Resize(rowCount - 1, 1);
            rg.FillDown();
            rg.Value = rg.Value;
            targetCol = rg.get_Address().Split(new char[] { '$' })[1];
            //辅助列，价差标记            
            string formula = (pl / 100.00).ToString();
            rg = FxtAddIn.FxtWorkSheet.Cells[2, rg.Column + 1];
            rg.Value = string.Format("=if({0}2>{1},1,0)", targetCol, formula);
            rg.get_Resize(rowCount - 1, 1).FillDown();
            //筛选
            useRg = FxtAddIn.FxtWorkSheet.UsedRange;
            //不能用useRg来filter，报异常：类 Range 的 AutoFilter 方法无效
            FxtAddIn.FxtWorkSheet.UsedRange.AutoFilter(rg.Column, "=1");
            //删除,可见单元格数量要大于零，这里无法判断是否有可见单元格，只能TRY
            try
            {
                FxtAddIn.FxtWorkSheet.get_Range("A2").get_Resize(useRg.Rows.Count - 1, useRg.Columns.Count).SpecialCells(XlCellType.xlCellTypeVisible).Delete();
            }
            catch { }
            FxtAddIn.FxtWorkSheet.AutoFilterMode = false;            
            //删除辅助列
            FxtAddIn.FxtWorkSheet.Cells[1, colCount + 4].EntireColumn.Delete();
            FxtAddIn.FxtWorkSheet.Cells[1, colCount + 3].EntireColumn.Delete();
            FxtAddIn.FxtWorkSheet.Cells[1, colCount + 2].EntireColumn.Delete();
            FxtAddIn.FxtWorkSheet.Cells[1, colCount + 1].EntireColumn.Delete();

            //FxtAddIn.FxtWorkSheet.UsedRange.Select();
            FxtAddIn.GoToSheet("testdb");
            FxtAddIn.FxtWorkSheet.Delete();
        }
    }
    public class Project
    {
        public int projectid { get; set; }
        public string projectname { get; set; }
    }
    public class Area
    {
        public int areaid { get; set; }
        public string areaname { get; set; }
    }
    public class Province
    {
        public int provinceid { get; set; }
        public string provincename { get; set; }
    }

    public class City
    {
        public int cityid { get; set; }
        public string cityname { get; set; }
        public int provinceid { get; set; }
    }
}

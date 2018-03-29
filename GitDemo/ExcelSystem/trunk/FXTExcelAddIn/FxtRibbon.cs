using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace FXTExcelAddIn
{
    public partial class FxtRibbon
    {
        
        /// <summary>
        /// 案例处理面板
        /// </summary>
        public CustomTaskPane CasePane;
        /// <summary>
        /// 选择城市面板
        /// </summary>
        public CustomTaskPane CityPane;
        private void FxtRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            toggleButtonCase.ScreenTip = "房讯通案例处理";
            toggleButtonCase.SuperTip = "楼盘名称标准化、去除重复数据、去除价格偏离数据";
            //强制一行显示图标LABEL
            toggleButtonCase.Label += "\n";
            toggleButtonAddress.Label += "\n";
            toggleButtonAddress.Enabled = false;
            toggleButtonProjects.Label += "\n";
            toggleButtonProjects.Enabled = false;
            galleryMath.Label += "\n";
            galleryMath.Enabled = false;
            galleryDataMap.Label += "\n";
            galleryStat.Label += "\n";
            splitButtonMap.Label += "\n";
            btnDataCenter.Label += "\n";
            splitButtonHouseWeb.Label += "\n";
            splitButtonHouseWeb.Enabled = false;
            galleryHelp.Label += "\n";
            btnSelCity.Label += "\n";
            //常用公式按钮
            foreach (RibbonButton item in galleryMath.Buttons)
            {
                item.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.setMath);
            }    
        }
        private void FxtRibbon_Close(object sender, EventArgs e)
        {
            //保存设置
            FXTExcelAddIn.Properties.Settings.Default.Save();
        }

        private void CasePane_VisibleChanged(object sender, EventArgs e)
        {
            // 同步Help Ribbon下的"帮助"按钮的状态
            toggleButtonCase.Checked = CasePane.Visible;
        }

        public void SetCity(int cityid, string cityname)
        {
            btnSelCity.Label = cityname;
        }

        //public Dictionary<string, CustomTaskPane> AllPanes = new Dictionary<string, CustomTaskPane>();
        /// <summary>
        /// kevin 2015-11-14
        /// office 2013 采用SDI(单进程）导致不能多窗口控制当前的面板，所以采用集合来处理。 
        /// kevin 2015-11-14
        /// 通过修改注册表，让excel每个工作薄都新开一个进程，用于解决控件混乱不可控的问题。代码作废
        /// </summary>
        /// <returns></returns>
        //public CustomTaskPane GetCurPane(string key)
        //{            
        //    string pKey = Globals.FxtAddIn.Application.Hwnd.ToString() + "_" + key;
        //    var taskPane = (AllPanes.Where(kvp => kvp.Key == pKey).Select(kvp => kvp.Value).FirstOrDefault());
        //    if (taskPane == null)
        //    {
        //        switch (key)
        //        {
        //            case "project":
        //                UC_Case cs = new UC_Case();
        //                taskPane = Globals.FxtAddIn.CustomTaskPanes.Add(cs, "楼盘获取");
        //                taskPane.VisibleChanged += new EventHandler(CasePane_VisibleChanged);
        //                break;
        //        }                
        //        AllPanes[pKey] = taskPane;
        //    }
        //    return (CustomTaskPane)taskPane;
        //}

        private void toggleButtonCase_Click(object sender, RibbonControlEventArgs e)
        {
            if (CasePane == null)
            {
                UC_Case cs = new UC_Case();
                CasePane = Globals.FxtAddIn.CustomTaskPanes.Add(cs, "案例处理");
                CasePane.VisibleChanged += new EventHandler(CasePane_VisibleChanged);
            }
            CasePane.Width = 250;
            CasePane.Visible = toggleButtonCase.Checked;
        }

        /// <summary>
        /// 地图
        /// </summary>
        /// <param name="url"></param>
        private void OpenMap(string url)
        {
            string key="";
            getSelectValue(out key);
            string mapurl = string.Format(url, key,FxtAddIn.CityName);
            Process.Start(mapurl);
        }

        private void getSelectValue(out string key)
        {
            Excel.Range rg = FxtAddIn.FxtApp.ActiveWindow.RangeSelection;
            key = "";
            if (rg != null)
            {
                FxtAddIn.FxtWorkSheet = FxtAddIn.FxtWorkBook.ActiveSheet;                         
                if (rg.Cells[1, 1].Value2 != null)
                {
                    key = rg.Cells[1, 1].Value2.ToString();
                    Regex reg = new Regex("[^0-9a-zA-Z\u4e00-\u9fa5_-]");
                    key = reg.Replace(key, "%20");
                    key = System.Web.HttpUtility.UrlEncode(key);
                }                               
            }
        }


        private void button4_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show(FxtCommon.About);
        }

        private void button5_Click(object sender, RibbonControlEventArgs e)
        {
            Process.Start(FxtCommon.Url_DataCenter);
        }

        /// <summary>
        /// 常用公式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setMath(object sender, RibbonControlEventArgs e)
        {            
            Range rg = FxtAddIn.FxtApp.ActiveCell;
            if (rg != null)
            {
                try
                {
                    rg.Value = Dict.MathDict[((RibbonButton)sender).Label];
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
        }



        private void gallery1_Click(object sender, RibbonControlEventArgs e)
        {
            
        }

        private void btnSelCity_Click(object sender, RibbonControlEventArgs e)
        {
            if (CityPane == null)
            {
                UC_CitySelect cs = new UC_CitySelect();
                CityPane = Globals.FxtAddIn.CustomTaskPanes.Add(cs, "选择城市");
                //CityPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionFloating;
                CityPane.DockPosition = FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectPosition;
                if (CityPane.DockPosition == Office.MsoCTPDockPosition.msoCTPDockPositionLeft
                    || CityPane.DockPosition == Office.MsoCTPDockPosition.msoCTPDockPositionRight)
                {
                    CityPane.Width = FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectWidth;
                }
                if (CityPane.DockPosition == Office.MsoCTPDockPosition.msoCTPDockPositionTop
                    || CityPane.DockPosition == Office.MsoCTPDockPosition.msoCTPDockPositionBottom)
                {
                    CityPane.Height = FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectHeight;
                }
                CityPane.DockPositionChanged += new EventHandler(this.CityPaneChange);                
            }
            if(!CityPane.Visible)
                CityPane.Visible = true;                      
        }

        /// <summary>
        /// 保存改变位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CityPaneChange(object sender, EventArgs e)
        {
            FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectPosition = CityPane.DockPosition;
            FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectWidth = CityPane.Width;
            FXTExcelAddIn.Properties.Settings.Default.UC_CitySelectHeight = CityPane.Height;            
        }

        private void splitButtonMap_Click(object sender, RibbonControlEventArgs e)
        {
            OpenMap("http://map.baidu.com/?s=s%26wd%3d{1}%20{0}");
        }        
        public static Dictionary<string,string> FindCol()
        {
            FxtAddIn.GoToSheet(1);
            Range rg = FxtAddIn.FxtWorkSheet.get_Range("A1").get_Resize(1, FxtAddIn.FxtWorkSheet.UsedRange.Columns.Count);
            dynamic[,] cols = rg.Value2;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 1; i <= cols.Length; i++)
            {
                if (cols[1, i] == null) continue;
                string key = cols[1, i].ToString().Replace("*", "");
                if (dict.ContainsKey(key)) continue;
                string str = rg.Cells[1, i].Address.Split(new char[]{'$'})[1].ToString();
                dict.Add(key, str);
            }
            return dict;
        }

        /// <summary>
        /// 最后一列
        /// </summary>
        /// <returns></returns>
        public static int GetLastColumn()
        {
            Range rg = FxtAddIn.FxtWorkSheet.get_Range("A1");
            int cnt = rg.get_End(XlDirection.xlToRight).Column;
            return cnt;
        }
        /// <summary>
        /// 最后一行
        /// </summary>
        /// <returns></returns>
        public static int GetLastRow()
        {
            Range rg = FxtAddIn.FxtWorkSheet.get_Range("A1048576");
            int cnt=rg.get_End(XlDirection.xlUp).Row;            
            return cnt;
        }
        /// <summary>
        /// 数据区域
        /// </summary>
        /// <returns></returns>
        public static Range GetUsedRange()
        {
            int colCount = GetLastColumn();
            int rowCount = GetLastRow();
            return FxtAddIn.FxtWorkSheet.get_Range("A1").get_Resize(rowCount, colCount);
        }

       
        private void btnType_Click(object sender, RibbonControlEventArgs e)
        {
            
        }

        private void btnUse_Click(object sender, RibbonControlEventArgs e)
        {
           
        }

        private void gallery1_Click_1(object sender, RibbonControlEventArgs e)
        {

        }

        private void gallery1_ButtonClick(object sender, RibbonControlEventArgs e)
        {
            switch (e.Control.Id)
            {
                case "btn_help_1":
                    Process.Start("http://club.excelhome.net/");
                    break;
                case "btn_help_about":
                    MessageBox.Show(FxtCommon.About);
                    break;
            }
        }

        private void btnStat1_Click(object sender, RibbonControlEventArgs e)
        {
            
        }

        private void galleryDataMap_ButtonClick(object sender, RibbonControlEventArgs e)
        {
            switch (e.Control.Id)
            {
                case "btn_DataMap_Hot":
                    OpenDataMap_Hot();
                    break;
                case "btn_DataMap_Selected":
                    OpenDataMap_Selected();
                    break;
            }
        }

        /// <summary>
        /// 省市数据地图
        /// </summary>
        private void OpenDataMap_Selected()
        {
            Range rg = FxtAddIn.FxtApp.Application.Selection;
            if (rg == null || rg[2, 1].Value == null)
            {
                MessageBox.Show("请选择数据区域");
                return;
            }
            try
            {
                int rowCount = 0;
                int colCount = 0;
                dynamic[,] data = HandleRange(out rowCount, out colCount);                
                string strLegend = data[1,2];
                string strData1 = "";
                string strData2 = "";
                double max = 1000;
                for (int i = 2; i <= rowCount; i++)
                {
                    if (data[i, 2] != null)
                    {
                        strData1 += string.Format(@"{{
                        name: '{0}',
                        value: {1}}}", data[i, 1].Replace("自治区", "")
                                         .Replace("壮族", "")
                                         .Replace("回族", "")
                                         .Replace("维吾尔", "")
                                         .Replace("直辖市", "")
                                         .Replace("省", ""), data[i, 2]);
                        if (i < rowCount) strData1 += ",";
                    }
                    if (data[i, 4] != null)
                    {
                        strData2 += string.Format(@"{{
                        name: '{0}',
                        value: {1}}}", data[i, 3], data[i, 4]);
                        if (i < rowCount) strData2 += ",";
                        if (Convert.ToDouble(data[i, 4].ToString()) > max) max = Convert.ToDouble(data[i, 4].ToString());
                    }                    

                }
                if (strData1.EndsWith(",")) strData1 = strData1.Substring(0, strData1.Length - 1);
                string str = FXTExcelAddIn.Properties.Resources.echart_map;
                str = str.Replace("@legend", strLegend).Replace("@data1", strData1).Replace("@data2", strData2).Replace("@max", max.ToString());
                OpenStat(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 热力图
        /// </summary>
        private void OpenDataMap_Hot()
        {
            Range rg = FxtAddIn.FxtApp.Application.Selection;
            if (rg == null || rg[1,1].Value == null)
            {
                MessageBox.Show("请选择数据区域，并保证按经度、维度、数据排列");
                return;
            }
            try
            {
                dynamic[,] data = rg.Value;
                string datastr = "";
                for (int i = 1; i <= rg.Rows.Count; i++)
                {
                    if (isNumeric(data[i, 1].ToString()))
                    {
                        datastr += string.Format("{{ \"lng\": {0}, \"lat\": {1}, \"count\": {2} }}", data[i, 1], data[i, 2], data[i, 3]);
                        if (i < rg.Rows.Count) datastr += ",";
                    }
                }
                string str = FXTExcelAddIn.Properties.Resources.map;
                str = str.Replace("@data", datastr);
                OpenStat(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool isNumeric(string str)
        {
            //判断是否是数值，有小数点
            return Regex.IsMatch(str, @"^\d*[.]?\d*$");
        }

        private void galleryStat_Click(object sender, RibbonControlEventArgs e)
        {

        }

        /// <summary>
        /// 统计图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void galleryStat_ButtonClick(object sender, RibbonControlEventArgs e)
        {
            Range rg = FxtAddIn.FxtApp.Application.Selection;
            if (rg == null || rg[2, 1].Value == null)
            {
                MessageBox.Show("请选择数据区域");
                return;
            }
            switch (e.Control.Id)
            {
                case "btn_Stat_Bar": //条形图
                    OpenStat_Line("bar");
                    break;
                case "btn_Stat_Line": //折线图
                    OpenStat_Line("line");
                    break;
                case "btn_Stat_Pie": //饼图
                    OpenStat_Pie("pie");
                    break;
                case "btn_Stat_Scatter":
                    //OpenStat_Scatter("scatter");
                    break;
                case "btn_Stat_Bar1": //柱状图
                    OpenStat_Line("bar1");
                    break;
                case "btn_Stat_Pie1": //环形图
                    OpenStat_Pie("pie1");
                    break;
                case "btn_Stat_Bar2": //瀑布图
                    OpenStat_Line("bar2");
                    break;
            }
        }

        /// <summary>
        /// 饼图
        /// </summary>
        private void OpenStat_Pie(string type)
        {
            try
            {
                int rowCount = 0;
                int colCount = 0;
                dynamic[,] data = HandleRange(out rowCount, out colCount);
                string datastr = @"
                    legend: {{ 
                        orient : 'vertical',
                        x : 'left',
                        data: [{0}] }},
                    series: [{{
                        name:'{1}',
                        type:'pie',
                        radius : {4},
                        center: ['50%', '60%'],
                        {3}
                        data:[
                            {2}
                        ]
                    }}]
                ";
                string strLegend = "";
                string strY = "";
                List<string> series = new List<string>();
                double max = 0;
                for (int i = 2; i <= rowCount; i++)
                {   
                    strLegend += "'" + data[i, 1] + "'";
                    if (i < rowCount) strLegend += ",";
                    strY +=string.Format(@"{{
                        name: '{0}',
                        value: {1}}}",data[i,1],data[i,2]);
                    if (i < rowCount) strY += ",";
                    if (Convert.ToDouble(data[i, 2].ToString()) > max) max = Convert.ToDouble(data[i, 2].ToString());
                }
                string str = FXTExcelAddIn.Properties.Resources.echart_pie;
                string strStype = @"itemStyle : { 
                        normal : {
                            label : {
                                show : true,
                                formatter: '{b} : {c} ({d}%)' 
                            },
                            labelLine : {
                                show : true
                            }
                        },";
                string strRadius = "'55%'";
                if (type == "pie1")
                {
                    strStype += @"                       
                        emphasis : {
                            label : {
                                show : true,
                                position : 'center',
                                textStyle : {
                                    fontSize : '30',
                                    fontWeight : 'bold'
                                }
                            }
                        }";                    
                    strRadius = "['50%', '70%']";
                }
                strStype += "},";
                datastr = string.Format(datastr, strLegend, data[1,2], strY,strStype,strRadius);
                str = str.Replace("@data", datastr).Replace("@max",max.ToString());
                OpenStat(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 折线图、柱状图、条形图、堆积图
        /// </summary>
        /// <param name="type"></param>
        private void OpenStat_Line(string type)
        {
            
            try
            {
                int rowCount = 0;
                int colCount = 0;
                dynamic[,] data = HandleRange(out rowCount,out colCount);
                string tooltip = "";
                string magictype = "magicType: { show: true, type: ['line', 'bar','stack','tiled']},";
                string datastr = @"
                    legend: {{ data: [{0}] }},
                    {3}Axis: [
                        {{
                            type: 'category',
                            data: [{1}]
                        }}
                    ],
                    {4}
                    series: [
                       {2}
                    ]
                ";
                string strLegend = "";
                string strY = "";
                List<string> series = new List<string>();
                //瀑布图只处理一列
                double maxValue = 0;
                if (type == "bar2")
                {
                    colCount = 2;
                    tooltip = @",axisPointer : {            // 坐标轴指示器，坐标轴触发有效
                                    type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                                },
                                formatter: function (params) {
                                    var tar = params[0];
                                    return tar.name + '<br/>' + tar.seriesName + ' : ' + tar.value;
                                }";
                    magictype = "";
                }
                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 2; j <= colCount; j++)
                    {
                        if (i == 1)
                        {
                            strLegend += "'" + data[i, j] + "'";
                            if (j < colCount) strLegend += ",";
                            string s = string.Format(@"{{
                                name: '{0}',
                                type: '{1}',{3}
                                itemStyle: {{
                                    normal: {{
                                        label: {{
                                            show: true, position: '{2}'
                                        }}
                                    }}
                                }},
                                data: [", data[i, j], type == "line" ? "line" : "bar",
                                        type == "bar" ? "insideRight" : (type == "bar1" ? "insideTop" : (type == "bar2" ? "inside" : ""))
                                        , type == "bar2" ? "stack: '总量'," : "");
                            series.Add(s);
                            if (type == "bar2")
                            {
                                s = @"{
                                    name:'辅助',
                                    type:'bar',
                                    stack: '总量',
                                    itemStyle:{
                                        normal:{
                                            barBorderColor:'rgba(0,0,0,0)',
                                            color:'rgba(0,0,0,0)'
                                        },
                                        emphasis:{
                                            barBorderColor:'rgba(0,0,0,0)',
                                            color:'rgba(0,0,0,0)'
                                        }
                                    },
                                    data:[";
                                series.Insert(0,s);
                            }
                        }
                        else
                        {
                            if (type == "bar2")
                            {
                                //保证第一个值为总值
                                if (i == 2)
                                {
                                    maxValue = Convert.ToDouble(data[i, j]);
                                    series[j - 2] += maxValue - Convert.ToDouble(data[i, j]);
                                }
                                else if (i < rowCount)
                                {
                                    maxValue = maxValue - Convert.ToDouble(data[i, j]);
                                    series[j - 2] += maxValue;
                                }
                                if (i < rowCount)
                                {
                                    series[j - 2] += ",";
                                }
                                else if (i == rowCount)
                                {
                                    series[j - 2] += "0]}";
                                }
                                series[j - 1] += data[i, j];
                                if (i < rowCount)
                                {
                                    series[j - 1] += ",";
                                }
                                else if (i == rowCount)
                                {
                                    series[j - 1] += "]}";
                                }
                            }
                            else
                            {
                                series[j - 2] += data[i, j];
                                if (i < rowCount)
                                {
                                    series[j - 2] += ",";
                                }
                                else if (i == rowCount)
                                {
                                    series[j - 2] += "]}";
                                }
                            }
                        }
                    }
                    if (i > 1)
                    {
                        strY += "'" + data[i, 1] + "'";
                        if (i < rowCount) strY += ",";
                    }
                }
                string str = FXTExcelAddIn.Properties.Resources.echart_bar;
                string strSeries = "";
                for (int i = 0; i < series.Count; i++)
                {
                    strSeries += series[i];
                    if (i < series.Count-1) strSeries += ",";
                }
                string strXY="";
                string strYAxis = "";
                switch (type)
                {
                    default:
                        strXY="x";
                        strYAxis = @"yAxis : [
                            {
                                type : 'value'
                            }
                        ],";
                        break;
                    case "bar": //条形图
                        strXY="y";
                        strYAxis = "";
                        break;
                }
                datastr = string.Format(datastr, strLegend, strY, strSeries, strXY, strYAxis);
                str = str.Replace("@data", datastr).Replace("@tooltip", tooltip).Replace("@magictype", magictype);
                OpenStat(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 处理range,是否需要转置
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="data"></param>
        private dynamic[,] HandleRange(out int rowCount,out int colCount)
        {
            Range rg = FxtAddIn.FxtApp.Application.Selection;
            dynamic[,] data = rg.Value;
            //如果列数大于行数，判断为横表，转置为竖表
            rowCount = rg.Rows.Count;
            colCount = rg.Columns.Count;
            if (rg.Columns.Count > rg.Rows.Count)
            {
                data = FxtAddIn.FxtApp.Application.WorksheetFunction.Transpose(data);
                rowCount = rg.Columns.Count;
                colCount = rg.Rows.Count;
            }
            return data;
        }
        /// <summary>
        /// 打开图表
        /// </summary>
        /// <param name="str"></param>
        private void OpenStat(string str)
        {
            try
            {
                string path = "c:\\windows\\temp\\" + Guid.NewGuid() + ".htm";
                StreamWriter sw = File.CreateText(path);
                sw.Write(str);
                sw.Close();
                sw.Dispose();
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

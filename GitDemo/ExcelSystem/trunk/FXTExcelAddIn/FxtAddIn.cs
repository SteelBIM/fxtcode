using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools;

namespace FXTExcelAddIn
{
    /// <summary>
    /// 房讯通数据插件 kevin 2015-11-12
    /// </summary>
    public partial class FxtAddIn
    {
        /// <summary>
        /// 当前APPlication
        /// </summary>
        public static Excel.Application FxtApp;      
        /// <summary>
        /// 当前sheet
        /// </summary>
        public static Excel.Worksheet FxtWorkSheet;

        private Office.CommandBarButton clearCaseAddressCommand;
       
        //定义菜单Tag
        private string menuTag = "case process";

        public static int CityID = 0;
        public static string CityName = string.Empty;

        private static Excel.Workbook _fxtWorkBook;
        /// <summary>
        /// 当前工作薄
        /// </summary>
        public static Excel.Workbook FxtWorkBook {
            get {
                if(_fxtWorkBook==null) _fxtWorkBook = (Excel.Workbook)FxtApp.ActiveWorkbook; ;
                return _fxtWorkBook;
            }
        }

        /// <summary>
        /// 激活sheet，找不到自动创建
        /// </summary>
        /// <param name="sheetName"></param>
        public static void GoToSheet(string sheetName)
        {
            bool find = false;
            Excel.Sheets sheets = FxtWorkBook.Sheets;
            foreach (Excel.Worksheet sheet in sheets)
            {
                if (sheet.Name == sheetName)
                {
                    FxtWorkSheet = sheets[sheet.Index];
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                sheets.Add(Type.Missing, sheets[sheets.Count]);
                FxtWorkSheet = sheets[sheets.Count];
                FxtWorkSheet.Name = sheetName;
            }
            ((Excel._Worksheet)FxtWorkSheet).Activate();
        }

        public static void GoToSheet(int index)
        {
            if (FxtWorkBook.Sheets.Count < index)
            {
                FxtWorkBook.Sheets.Add(Type.Missing, FxtWorkBook.Sheets[index]);
            }
            FxtWorkSheet = FxtWorkBook.Sheets[index];
            ((Excel._Worksheet)FxtWorkSheet).Activate();
        }
        /// <summary>
        /// 删除sheet
        /// </summary>
        /// <param name="sheetName"></param>
        public static void DeleteSheet(string sheetName)
        {
            Excel.Sheets sheets = FxtWorkBook.Sheets;
            foreach (Excel.Worksheet sheet in sheets)
            {
                if (sheet.Name == sheetName)
                {
                    //避免弹窗确认
                    FxtApp.DisplayAlerts = false;
                    sheet.Delete();
                    FxtApp.DisplayAlerts = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 提升效率
        /// </summary>
        /// <param name="enable"></param>
        public static void EnableAppWindow(bool enable)
        {
            //避免替换时弹窗（找不到会弹窗）
            FxtApp.DisplayAlerts = enable;
            FxtApp.EnableEvents = enable;
            FxtApp.DisplayStatusBar = enable;
            FxtApp.DisplayFormulaAutoComplete = enable;
            FxtApp.ScreenUpdating = enable;
        }

        // 如果菜单存在则删除它.
        public void CheckIfMenuBarExists()
        {
            try
            {
                Office.CommandBarPopup foundMenu = (Office.CommandBarPopup)
                    this.Application.CommandBars.ActiveMenuBar.FindControl(
                    Office.MsoControlType.msoControlPopup, System.Type.Missing, menuTag, true, true);

                if (foundMenu != null)
                {
                    foundMenu.Delete(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 如果菜单不存在则创建它.
        private void AddMenuBar()
        {
            try
            {
                Office.CommandBarPopup cmdBarControl = null;
                Office.CommandBar menubar = (Office.CommandBar)Application.CommandBars.ActiveMenuBar;
                int controlCount = menubar.Controls.Count;
                string menuCaption = "案例处理(&P)";
                // Add the menu.
                cmdBarControl = (Office.CommandBarPopup)menubar.Controls.Add(Office.MsoControlType.msoControlPopup, missing, missing, controlCount, true);
                cmdBarControl.Tag = menuTag;
                if (cmdBarControl != null)
                {
                    cmdBarControl.Caption = menuCaption;
                    clearCaseAddressCommand = (Office.CommandBarButton)cmdBarControl.Controls.Add(Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                    clearCaseAddressCommand.Caption = "自动清理地址->楼盘(&S)";
                    clearCaseAddressCommand.Tag = "importCaseCommand";
                    clearCaseAddressCommand.FaceId = 0162;
                    clearCaseAddressCommand.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(clearCaseAddressCommand_Click);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //点击菜单事件
        private void clearCaseAddressCommand_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            
           
            Excel.Workbook workbook = (Excel.Workbook)Globals.FxtAddIn.Application.ActiveWorkbook;
            Excel.Worksheet worksheet = workbook.Sheets[2];
            worksheet.Activate();
            Range rg = worksheet.UsedRange;
            rg.AdvancedFilter(XlFilterAction.xlFilterCopy,Type.Missing,worksheet.get_Range("B1"),true);
            rg = worksheet.get_Range("b1", "b" + rg.Rows.Count.ToString());
            int lRow = rg.get_End(XlDirection.xlDown).Row;
            
            rg = worksheet.get_Range("c1","C" + lRow.ToString());
            rg.Cells[1,1].Formula = "=Len(B1)";
            rg.FillDown();
            
            //排序
            rg = worksheet.get_Range("B1", "C" + lRow.ToString());
            rg.Sort(rg.Cells[1, 2], XlSortOrder.xlDescending);

            //处理
            worksheet = workbook.Sheets[1];
            worksheet.Activate();
            
            Globals.FxtAddIn.Application.DisplayAlerts = false;
            Globals.FxtAddIn.Application.EnableEvents = true;
            Range rg1 = worksheet.get_Range("A1","A" + worksheet.UsedRange.Rows.Count.ToString());
            rg1.EntireColumn.Insert(XlInsertShiftDirection.xlShiftDown,false);
            rg1.Copy(worksheet.get_Range("A1"));
            
            for (int i = 1; i <= rg.Rows.Count; i++)
            {
                string sKey = rg.Cells[i, 1].Value;
                rg1.Replace("*" + sKey + "*", sKey, Type.Missing, Type.Missing, false, Type.Missing,false,false);
            }
            
            Globals.FxtAddIn.Application.DisplayAlerts = true;
            MessageBox.Show("处理完成！");
        }
        private void FxtAddIn_Startup(object sender, System.EventArgs e)
        {
            FxtApp = Globals.FxtAddIn.Application;            
            //当工作簿启动时初始化菜单
            //CheckIfMenuBarExists();
            //AddMenuBar();
        }      

        private void FxtAddIn_Shutdown(object sender, System.EventArgs e)
        {
            
        }

        #region VSTO 生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(FxtAddIn_Startup);
            this.Shutdown += new System.EventHandler(FxtAddIn_Shutdown);
        }
        
        #endregion
    }
}

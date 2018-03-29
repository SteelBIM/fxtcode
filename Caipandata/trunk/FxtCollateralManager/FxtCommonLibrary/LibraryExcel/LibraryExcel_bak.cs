using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
#region 操作记录
/**
 * 作者:李晓东
 * 摘要:2014.02.19 新建
 * **/
#endregion
namespace FxtCommonLibrary.LibraryExcel
{
    /// <summary>
    /// Excel组件 操作类
    /// </summary>
    public class LibraryExcel_bak : IDisposable
    {
        private Excel.Application excel = null;
        private Excel.Workbook wb = null;
        private Excel.Workbooks workbooks = null;
        private Excel.Workbook workbook = null;
        private Excel.Worksheet worksheet = null;
        private string excelFile = string.Empty;

        public LibraryExcel_bak()
        {
            excel = new Excel.Application() { Visible = false, DisplayAlerts = false };
        }
        /// <summary>
        /// 打开Excel
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        public Excel.Workbook WorkbookOpen(string file)
        {
            excelFile = file;
            object missing = System.Reflection.Missing.Value;
            wb = excel.Application.Workbooks.Open(file, missing, true, missing, missing, missing,
               missing, missing, missing, true, missing, missing, missing, missing, missing);
            return wb;
        }

        /// <summary>
        /// 获得已打开的工作簿对象
        /// </summary>
        /// <returns></returns>
        public Excel.Worksheet Worksheet()
        {
            workbooks = excel.Workbooks;
            workbook = workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            
            return worksheet;
        }

        /// <summary>
        /// 写入Excel
        /// </summary>
        /// <param name="Worksheet">工作簿对象</param>
        /// <param name="file">文件</param>
        public bool WorkbookWrite(Excel.Worksheet worksheets, string file)
        {
            excelFile = file;
            if (worksheet != null && workbook != null)
            {
                try
                {
                    worksheet = worksheets;
                    //设置禁止弹出保存和覆盖的询问提示框   
                    excel.DisplayAlerts = false;
                    excel.AlertBeforeOverwriting = false;
                    //保存工作簿   
                    workbook.SaveAs(file);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 关闭Excel
        /// </summary>
        public void Close()
        {
            if (worksheet != null)
            {
                Marshal.ReleaseComObject(worksheet);
            }
            if (workbook != null)
            {
                workbook.Close(false, null, null);
                Marshal.ReleaseComObject(workbook);
            }
            worksheet = null;
            workbook = null;
            //if (wb != null)
            //{
            //    wb.Close(true, excelFile, System.Reflection.Missing.Value);
            //    wb = null;
            //}
            //if (excel != null)
            //{
            //    excel.Quit();
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            //    Kill(excel);
            //}
        }

        public void Dispose()
        {
            try
            {
                Close();

                if (workbooks != null)
                {
                    Marshal.ReleaseComObject(workbooks);
                }
                if (excel != null)
                {
                    excel.Quit();
                    Marshal.ReleaseComObject(excel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("dispose ExcelApp object failed", ex);
            }

            workbooks = null;
            excel = null;
        }

        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        ///// <summary>
        ///// 关闭Excel进程
        ///// </summary>
        ///// <param name="excel">Excel对象</param>
        //void Kill(Excel.Application excel)
        //{
        //    //IntPtr t = new IntPtr(excel.Hwnd); //得到这个句柄，具体作用是得到这块内存入口
        //    int k = 0;
        //    GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k); //得到本进程唯一标志k
        //    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k); //得到对进程k的引用
        //    p.Kill(); //关闭进程k
        //}
    }
}

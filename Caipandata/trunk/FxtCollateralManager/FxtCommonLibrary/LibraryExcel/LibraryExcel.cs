using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

/*作者:李晓东
 *摘要:2014.02.24 修改人:李晓东 新建 LibraryExcel操作帮助类
 * **/
namespace FxtCommonLibrary.LibraryExcel
{
    public class LibraryExcel : IDisposable
    {
        private Application _excel;
        private Workbooks _workbooks;
        private Workbook _workbook;
        private Worksheet _worksheet;
        private Sheets _worksheets;

        public LibraryExcel()
        {
            _excel = new Application { Visible = false, DisplayAlerts = false };
            _workbooks = _excel.Workbooks;
        }

        public int ColCount { get; set; }

        public int RowCount { get; set; }

        public string WorksheetName
        {
            get
            {
                return _worksheet != null ? _worksheet.Name : null;
            }
            set
            {
                if (_worksheet != null)
                {
                    _worksheet.Name = value;
                }
            }
        }

        public void GeRowColumn()
        {
            try
            {
                this.RowCount = _worksheet.UsedRange.Cells.Rows.Count;
                this.ColCount = _worksheet.UsedRange.Cells.Columns.Count;
            }
            finally
            {
            }
        }

        #region Get Excel Range
        public Range GetCell(int rowIndex, int cellIndex)
        {
            Range cells = null;
            Range range = null;

            try
            {
                cells = _excel.Cells;
                range = (Range)cells[1 + rowIndex, 1 + cellIndex];
            }
            finally
            {
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }

        public Range GetColumn(int cellIndex)
        {
            Range range = null;
            Range cells = null;
            object rangeX = null;
            object rangeY = null;

            try
            {
                cells = _excel.Cells;
                rangeX = cells[1, 1 + cellIndex];
                rangeY = cells[RowCount, 1 + cellIndex];
                range = _worksheet.get_Range(rangeX, rangeY);
            }
            finally
            {
                Marshal.ReleaseComObject(rangeX);
                Marshal.ReleaseComObject(rangeY);
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }

        public Range GetRange(int xRowIndex, int xCellIndex, int yRowIndex, int yCellIndex)
        {
            Range range = null;
            Range cells = null;
            object rangeX = null;
            object rangeY = null;

            try
            {
                cells = _excel.Cells;
                rangeX = cells[1 + xRowIndex, 1 + xCellIndex];
                rangeY = cells[yRowIndex + 1, yCellIndex + 1];
                range = _worksheet.get_Range(rangeX, rangeY);
            }
            finally
            {
                Marshal.ReleaseComObject(rangeX);
                Marshal.ReleaseComObject(rangeY);
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }
        #endregion

        public void Save(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentNullException("fullFilePath");
            }

            string directory = Path.GetDirectoryName(fullFilePath);

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("fullFilePath is not a valid file path.");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _workbook.SaveAs(fullFilePath);
        }

        public void Open(string fullFilePath)
        {
            _workbook = _workbooks._Open(fullFilePath,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value);

            _worksheet = (Worksheet)_workbook.ActiveSheet;

            ColCount = 0;
            RowCount = 0;
        }

        public void AddWorkbook()
        {
            _workbook = _workbooks.Add(true);
            _worksheet = (Worksheet)_workbook.ActiveSheet;

            ColCount = 0;
            RowCount = 0;
        }

        public void Reset()
        {
            Close();
            AddWorkbook();
        }

        private void Close()
        {
            if (_worksheet != null)
            {
                Marshal.ReleaseComObject(_worksheet);
            }
            if (_workbook != null)
            {
                _workbook.Close(false, null, null);
                Marshal.ReleaseComObject(_workbook);
            }
            _worksheet = null;
            _workbook = null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Close();

                if (_workbooks != null)
                {
                    Marshal.ReleaseComObject(_workbooks);
                }
                if (_excel != null)
                {
                    _excel.Quit();
                    Marshal.ReleaseComObject(_excel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("dispose ExcelApp object failed", ex);
            }
            _workbooks = null;
            _excel = null;
        }
        #endregion
    }


    public class Disposable
    {
        public static Disposable<T> Create<T>(T o) where T : class
        {
            return new Disposable<T>(o);
        }
    }

    public class Disposable<T> : IDisposable where T : class
    {
        public T Value;

        internal Disposable(T o)
        {
            Value = o;
        }

        public void Dispose()
        {
            if (Value != null)
            {
                Marshal.ReleaseComObject(Value);
            }
        }
    }
}

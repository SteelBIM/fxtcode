using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.SS.Util;
using NPOI.XSSF.Model;
using NPOI.XWPF.UserModel;
using System.Collections;

namespace CAS.Office.NPOI
{
    /// <summary>
    /// byte-侯湘岳 2015-04-09
    /// </summary>
    public class Xls
    {
        public IWorkbook workbook;
        FileStream input;
        public Xls(string openTargetFile)
        {
            input = File.OpenRead(openTargetFile);
            if (input.Length > 0)
            {
                if (openTargetFile.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(input);
                }
                else
                {
                    workbook = new HSSFWorkbook(input);
                }
            }

        }

        public Xls(string openTargetFile, bool fileShare)
        {
            if (!fileShare)
            {
                input = File.OpenRead(openTargetFile);
            }
            else
            {
                input = new FileStream(openTargetFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);        //共享锁打开文件
            }
            if (input.Length > 0)
            {
                if (openTargetFile.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(input);
                }
                else
                {
                    workbook = new HSSFWorkbook(input);
                }
            }
        }
        /// <summary>
        /// 获得名称的总个数
        /// </summary>
        /// <returns></returns>
        public int NumberOfNames
        {
            get
            {
                return workbook.NumberOfNames;
            }
        }

        /// <summary>
        /// 修改人rock 20151121，添加后去图片的byte[]集合
        /// 获得特定字符打头的所有名称和图片的byte[]集合
        /// </summary>
        /// <param name="prefixRegionName"></param>
        /// <param name="imgByteList">输出获取图片的byte[]集合</param>
        /// <returns></returns>
        public List<string> GetAllPrefixRegionName(string prefixRegionName, string sheetName, out  List<byte[]>  imgByteList)
        {
            List<string> list = null;
            for (int index = 0; index < workbook.NumberOfNames; index++)
            {
                IName name = workbook.GetNameAt(index);
                if (name.NameName.StartsWith(prefixRegionName) && name.SheetName == sheetName)
                {                    
                    if (null == list)
                        list = new List<string>();
                    list.Add(name.NameName);
                }
            }
            //读取excel中图片
            ISheet sheet = workbook.GetSheet(sheetName);
            imgByteList = new List<byte[]>();
            if (null != sheet)
            {
                IList pictures = sheet.Workbook.GetAllPictures();
                foreach (IPictureData pic in pictures)
                {
                    string ext = pic.SuggestFileExtension();//获取扩展名
                    imgByteList.Add(pic.Data);
                }
                /** 对2007以上版本的默写excel文件读取有异常
                var shapeContainer = sheet.DrawingPatriarch as HSSFShapeContainer;
                if (null != shapeContainer)
                {
                    var shapeList = shapeContainer.Children;
                    foreach (var shape in shapeList)
                    {
                        if (shape is HSSFPicture && shape.Anchor is HSSFClientAnchor)
                        {
                            var picture = (HSSFPicture)shape;
                            var anchor = (HSSFClientAnchor)shape.Anchor;
                            byte[] imgB = picture.PictureData.Data;
                            imgByteList.Add(imgB);
                        }
                    }
                }**/
            }
            return list;
        }

        /// <summary>
        /// 获取合并的行列
        /// byte-侯湘岳
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="regionName">名称管理器中的某一名称</param>
        /// <param name="regionValues">返回区域中的值，返回的数据类似二维数组，键0,0表示第1行第1列</param>
        /// <returns></returns>
        internal List<Assist.MergedRegion> GetMergedRegion(string sheetName, string regionName, out Dictionary<string, string> regionValues, out int rows, out int columns)
        {
            List<Assist.MergedRegion> result = null; regionValues = null; rows = 0; columns = 0;
            ISheet sheet = workbook.GetSheet(sheetName);
            
            if (null == sheet)
                return result;
            IName name = workbook.GetName(regionName);
            if (null == name)
                return result;
            CellRangeAddress cellRange = CellRangeAddress.ValueOf(name.RefersToFormula);        //如："E3:H7";
            if (-1 == cellRange.FirstRow)
                return result;
            rows = cellRange.LastRow - (cellRange.FirstRow - 1);           //总行数
            columns = cellRange.LastColumn - (cellRange.FirstColumn - 1);        //总列数
            string key = string.Empty;
            regionValues = new Dictionary<string, string>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    key = row.ToString() + "," + column.ToString();
                    regionValues.Add(key, "");
                }
            }
            for (var i = cellRange.FirstRow; i <= cellRange.LastRow; i++)
            {
                IRow row = sheet.GetRow(i);

                for (var j = cellRange.FirstColumn; j <= cellRange.LastColumn; j++)
                {
                    key = (i - cellRange.FirstRow).ToString() + "," + (j - cellRange.FirstColumn).ToString();
                    if (null == row)
                    {
                        regionValues[key] = "";
                    }
                    else
                    {
                        ICell cell = row.GetCell(j);
                        if (null == cell)
                        {
                            regionValues[key] = "";
                        }
                        else
                        {
                            string stringVale = string.Empty;
                            switch (cell.CellType)
                            {
                                case CellType.Boolean:
                                    stringVale = cell.BooleanCellValue.ToString();
                                    break;
                                case CellType.Numeric:
                                    stringVale = GetNumericCellValue(cell);
                                    break;
                                case CellType.Formula:
                                    if (cell.CachedFormulaResultType == CellType.Numeric)
                                    {
                                        stringVale = GetNumericCellValue(cell);
                                    }
                                    else if (cell.CachedFormulaResultType == CellType.Boolean)
                                        stringVale = cell.BooleanCellValue.ToString();
                                    else if (cell.CachedFormulaResultType == CellType.Error)
                                        stringVale = "";
                                    else
                                        stringVale = cell.StringCellValue;
                                    break;
                                case CellType.Error:
                                    stringVale = "";
                                    break;  
                                default:
                                    stringVale = cell.StringCellValue;
                                    break;
                            }
                            regionValues[key] = stringVale;
                        }
                    }
                }
            }
            result = GetMergedRegion(sheet, cellRange);
            return result;
        }

        private string GetNumericCellValue(ICell cell)
        {
            //很奇怪，为数值类型但是实际类型又可能是日期 
            string result = string.Empty;
            string format = cell.CellStyle.GetDataFormatString();
            
            if ("General" == format)
            {
                //常规
                result = cell.NumericCellValue.ToString();
            }
            else if (DateUtil.IsCellDateFormatted(cell))
            {
                result = cell.DateCellValue.ToString("yyyy-MM-dd");                
            }
            else
            {
                if (format.StartsWith("0") && format.EndsWith("_ "))
                {
                    int startIndex = format.Length - 2;
                    if (0 < startIndex)
                    {
                        format = format.Remove(format.Length - 2, 1);   //删除后面两"_ "个字符  //returns 0.0_ 
                        result = cell.NumericCellValue.ToString(format);        //保留小数位数。
                    }
                    else
                    {
                        result = cell.NumericCellValue.ToString();
                    }
                }
                else
                    result = cell.NumericCellValue.ToString();
            }
            return result;
        }
        /// <summary>
        /// 获取所在sheet表中的名称区域合并项
        /// byte-侯湘岳
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rangeAddress"></param>
        /// <returns></returns>
        private List<Assist.MergedRegion> GetMergedRegion(ISheet sheet, CellRangeAddress rangeAddress)
        {
            //0行0列开始
            List<Assist.MergedRegion> result = null;
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                var cellRange = sheet.GetMergedRegion(i);       //合并的单元格
                if (rangeAddress.FirstRow <= cellRange.FirstRow && rangeAddress.LastRow >= cellRange.LastRow &&
                   rangeAddress.FirstColumn <= cellRange.FirstColumn && rangeAddress.LastColumn >= cellRange.LastColumn)
                {
                    //该名词管理器存在合并的单元格
                    if (null == result)
                        result = new List<Assist.MergedRegion>();
                    if (cellRange.FirstRow == cellRange.LastRow)
                    {
                        //没有跨行合并
                        result.Add(new Assist.MergedRegion()
                        {
                            MergeType = Assist.EnumMergeType.Horizontal,
                            StartRowIndex = cellRange.FirstRow - rangeAddress.FirstRow,
                            StartCellIndex = cellRange.FirstColumn - rangeAddress.FirstColumn,
                            EndCellIndex = cellRange.LastColumn - rangeAddress.FirstColumn
                        });
                    }
                    else
                    {
                        //跨行合并
                        //分为两步1，水平合并 2，垂直合并
                        for (int row = cellRange.FirstRow; row <= cellRange.LastRow; row++)
                        {
                            result.Add(new Assist.MergedRegion()
                            {
                                MergeType = Assist.EnumMergeType.Horizontal,
                                StartRowIndex = row - rangeAddress.FirstRow,
                                StartCellIndex = cellRange.FirstColumn - rangeAddress.FirstColumn,
                                EndCellIndex = cellRange.LastColumn - rangeAddress.FirstColumn
                            });
                        }
                        result.Add(new Assist.MergedRegion()
                        {
                            MergeType = Assist.EnumMergeType.Vertical,
                            StartRowIndex = cellRange.FirstRow - rangeAddress.FirstRow,
                            EndRowIndex = cellRange.LastRow - rangeAddress.FirstRow,
                            StartCellIndex = cellRange.FirstColumn - rangeAddress.FirstColumn
                        });
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// 释放对象
        /// byte-侯湘岳
        /// </summary>
        public void Close()
        {
            if (null != input)
                input.Close();
        }
    }
}

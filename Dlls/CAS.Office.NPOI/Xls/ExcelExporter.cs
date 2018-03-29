using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Threading;
using System.Globalization;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOIM = NPOI.XWPF;//.UserModel;
using NPOI.HPSF;
using System.Reflection;
using CAS.Entity.AttributeHelper;
using NPOI.SS.Converter;


namespace CAS.Office.NPOI
{
    /// <summary>
    /// byte 2014-12-05
    /// excel导出
    /// </summary>
    public class ExcelExporter
    {
        private HSSFWorkbook hssfWorkBook;
        private XSSFWorkbook xssfWorkBook;
        /// <summary>
        /// "xssf"为导出到2007及以上版本
        /// </summary>
        /// <param name="type"></param>
        public ExcelExporter(string type)
        {
            if (type == "xssf")
            {
                xssfWorkBook = new XSSFWorkbook();
            }
            else
            {
                hssfWorkBook = new HSSFWorkbook();
            }
            InitializeWorkbook();
        }
        public ExcelExporter()
        {
            hssfWorkBook = new HSSFWorkbook();
        }
        /// <summary>
        /// 往excel中插入图片
        /// </summary>
        /// <param name="imgInfos">要插入的图片信息，匿名类集合，格式:{sheetName:存放到哪个sheet里(不存在则会新增),path:图片物理路径,imgName:图片类型名称,col1:图片左上角从第几列开始,row1:图片左上角从第几行开始,col2:图片右下角在第几列结束,row2:图片右下角在第几行结束}</param>
        public void InsertImg(List<object> imgInfos)
        {
            if (imgInfos == null || imgInfos.Count < 1)
            {
                return;
            }            
            int insertRow = imgInfos.Max(_obj => Convert.ToInt32(_obj.GetType().GetProperty("row2").GetValue(_obj, null)))+3;//所有图片最大占用多少行
            int insertCol = imgInfos.Max(_obj => Convert.ToInt32(_obj.GetType().GetProperty("col2").GetValue(_obj, null)))+3;//所有图片最大占用多少列
            HSSFSheet sheet = null;
            foreach (object imgObj in imgInfos)
            {
                string sheetName = Convert.ToString(imgObj.GetType().GetProperty("sheetName").GetValue(imgObj, null));//要生成到哪个sheet,
                string imgPath = Convert.ToString(imgObj.GetType().GetProperty("path").GetValue(imgObj, null));//图片物理路径
                string imgName = Convert.ToString(imgObj.GetType().GetProperty("imgName").GetValue(imgObj, null));//图片名称
                int col1 = Convert.ToInt32(imgObj.GetType().GetProperty("col1").GetValue(imgObj, null));//图片左上角从第几列开始
                int row1 = Convert.ToInt32(imgObj.GetType().GetProperty("row1").GetValue(imgObj, null));//图片左上角从第几行开始
                int col2 = Convert.ToInt32(imgObj.GetType().GetProperty("col2").GetValue(imgObj, null));//图片右下角在第几列结束
                int row2 = Convert.ToInt32(imgObj.GetType().GetProperty("row2").GetValue(imgObj, null));//图片右下角在第几行结束
                if (!File.Exists(imgPath))
                {
                    continue;
                }
                string extension = Path.GetExtension(imgPath);//获取扩展名
                PictureType pictureType;
                //判断扩展名
                switch (extension.ToLower())
                {
                    #region
                    case "png":
                        pictureType = PictureType.PNG;
                        break;
                    case "dib":
                        pictureType = PictureType.DIB;
                        break;
                    case "emf":
                        pictureType = PictureType.EMF;
                        break;
                    case "pict":
                        pictureType = PictureType.PICT;
                        break;
                    case "wmf":
                        pictureType = PictureType.WMF;
                        break;
                    default:
                        pictureType = PictureType.JPEG;
                        break;
                    #endregion
                }
                //获取sheet，没有就创建sheet
                sheet = (HSSFSheet)hssfWorkBook.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = (HSSFSheet)hssfWorkBook.CreateSheet(sheetName);
                    sheet.InsertRow(insertRow, insertCol);
                }
                //将图片插到sheet里
                byte[] bytes = System.IO.File.ReadAllBytes(imgPath);
                int pictureIdx = hssfWorkBook.AddPicture(bytes, pictureType);
                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col1, row1, col2, row2);
                HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                //插如图片名称
                var tempRow = sheet.GetRow(row2);
                var tempCell = tempRow.GetCell(col1);
                tempCell.SetCellValue(imgName);
                CellRangeAddress cellRangeAddress = new CellRangeAddress(row2, row2, col1, col2-1);//合并单元格
                sheet.AddMergedRegion(cellRangeAddress);
            }
        }

        //public void testimg(string[] imgpath, string sh, string saveTargetFile)
        //{
        //    List<string[]> list = new List<string[]>();
        //    HSSFSheet sheet = (HSSFSheet)hssfWorkBook.CreateSheet(sh);
        //    for (int i = 0; i < imgpath.Length; i++)
        //    {
        //        sheet = (HSSFSheet)hssfWorkBook.CreateSheet(sh);
        //        string imgPath = imgpath[i];
        //        byte[] bytes = System.IO.File.ReadAllBytes(imgPath);
        //        int pictureIdx = hssfWorkBook.AddPicture(bytes, PictureType.JPEG);
        //        HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
        //        //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
        //        int h = 15;
        //        int col1 = 0;
        //        int row1 = 0;
        //        int col2 = 0;
        //        int row2 = 0;
        //        if ((i + 1) % 2 == 0)
        //        {
        //            col1 = 5;
        //            row1 = (i/2) * (h + 1);
        //            col2 = 9;
        //            row2 = h + row1;
        //        }
        //        else
        //        {
        //            col1 = 0;
        //            row1 = (i / 2) * (h + 1);
        //            col2 = 4;
        //            row2 = h + row1; 
 
        //        }
        //        HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col1, row1, col2, row2);
        //        HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);                

        //    }
        //    using (FileStream fs = new FileStream(saveTargetFile, FileMode.Create))
        //    {
        //        hssfWorkBook.Write(fs);
        //        fs.Flush();
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveTargetFile"></param>
        /// <param name="source"></param>
        /// <param name="availableFields"></param>
        /// <param name="imgInfos">要插入的图片信息，匿名类集合，格式:{sheetName:存放到哪个sheet里(不存在则会新增),path:图片物理路径,col1:图片左上角从第几列开始,row1:图片左上角从第几行开始,col2:图片右下角在第几列结束,row2:图片右下角在第几行结束}</param>
        public void Export<T>(string saveTargetFile, IList<T> source, string[] availableFields, List<object> imgInfos = null) where T : class
        {
            Export<T>(saveTargetFile, source, availableFields, "Sheet1", imgInfos: imgInfos);
        }
        /// <summary>
        /// 集合导出Excel到2007及以上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveTargetFile">要保存的路径</param>
        /// <param name="source">数据List</param>
        /// <param name="columnsNameMapping">要导出集合中实体的那些属性和属性描述例如dic["companyname"]="公司名称"</param>
        /// <param name="sheetName">要存放到哪个sheetName里面，没有则创建</param>
        /// <param name="imgInfos">要插入的图片信息，匿名类集合，格式:{sheetName:存放到哪个sheet里(不存在则会新增),path:图片物理路径,col1:图片左上角从第几列开始,row1:图片左上角从第几行开始,col2:图片右下角在第几列结束,row2:图片右下角在第几行结束}</param>
        public void Export2007<T>(string saveTargetFile, IList<T> source, Dictionary<string, string> columnsNameMapping, string sheetName, List<object> imgInfos = null) where T : class
        {            
                
            #region 参数验证
            if (string.IsNullOrWhiteSpace(saveTargetFile))
            {
                throw new ArgumentNullException("saveTargetFile");
            }
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentNullException("sheetName");
            }
            #endregion
            if (0 == source.Count)
                return;
            Type type = source.GetType();
            PropertyInfo[] infos = type.GetGenericArguments()[0].GetProperties();
            string[] availableFields = columnsNameMapping.Select(_d => _d.Key).ToArray();
            availableFields = availableFields.Select(q => q.ToLower()).ToArray();
            int exportFieldCount = availableFields.Length;
            Dictionary<string, PropertyInfo> fieldNameMapping = new Dictionary<string, PropertyInfo>();
            GetColumnsName(infos, availableFields, ref fieldNameMapping);
            var sheet = (XSSFSheet)xssfWorkBook.CreateSheet(sheetName);
            ApplyDetailCell<T>(source, sheet, availableFields, fieldNameMapping, columnsNameMapping);
            //格式化当前sheet，用于数据total计算
            sheet.ForceFormulaRecalculation = true;
            sheet = null;

            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 1023, 0, 0, 0, 1, 3);
            //往excel中插入图片
            InsertImg(imgInfos);
            using (FileStream fs = new FileStream(saveTargetFile, FileMode.Create))
            {
                xssfWorkBook.Write(fs);
                //fs.Flush();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveTargetFile"></param>
        /// <param name="source"></param>
        /// <param name="availableFields"></param>
        /// <param name="sheetName"></param>
        /// <param name="imgInfos">要插入的图片信息，匿名类集合，格式:{sheetName:存放到哪个sheet里(不存在则会新增),path:图片物理路径,col1:图片左上角从第几列开始,row1:图片左上角从第几行开始,col2:图片右下角在第几列结束,row2:图片右下角在第几行结束}</param>
        public void Export<T>(string saveTargetFile, IList<T> source, string[] availableFields, string sheetName, List<object> imgInfos = null) where T : class
        {
            #region 参数验证
            if (string.IsNullOrWhiteSpace(saveTargetFile))
            {
                throw new ArgumentNullException("saveTargetFile");
            }
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentNullException("sheetName");
            }
            #endregion
            if (0 == source.Count)
                return;
            Type type = source.GetType();
            PropertyInfo[] infos = type.GetGenericArguments()[0].GetProperties();
            availableFields = availableFields.Select(q => q.ToLower()).ToArray();
            int exportFieldCount = availableFields.Length;
            Dictionary<string, PropertyInfo> fieldNameMapping = new Dictionary<string, PropertyInfo>();
            Dictionary<string, string> columnsNameMapping = GetColumnsName(infos, availableFields, ref fieldNameMapping);


            var sheet = (HSSFSheet)hssfWorkBook.CreateSheet(sheetName);
            ApplyDetailCell<T>(source, sheet, availableFields, fieldNameMapping, columnsNameMapping);



            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 1023, 0, 0, 0, 1, 3);
            //格式化当前sheet，用于数据total计算
            sheet.ForceFormulaRecalculation = true;
            //往excel中插入图片
            InsertImg(imgInfos);
            using (FileStream fs = new FileStream(saveTargetFile, FileMode.Create))
            {
                hssfWorkBook.Write(fs);
                fs.Flush();
            }
            sheet = null;
        }

        /// <summary>
        /// 使用格式应用明细单元格
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="sheet">当前单元格</param>
        private void ApplyDetailCell<T>(
            IList<T> source, ISheet sheet, string[] availableFields, Dictionary<string, PropertyInfo> fieldNameMapping, Dictionary<string, string> columnsNameMapping) where T : class
        {
            if (source == null || source.Count <= 0)
            {
                return;
            }
            int columnIndex;
            sheet.InsertRow(source.Count + 1, availableFields.Length);
            columnIndex = 0;
            //标头
            foreach (var columns in columnsNameMapping)
            {
                var tempRow = sheet.GetRow(0);
                var tempCell = tempRow.GetCell(columnIndex);
                tempCell.SetCellValue(columns.Value);
                tempCell.CellStyle.Alignment = HorizontalAlignment.Center;
                columnIndex++;
            }
            //填充数据
            for (int rowIndex = 1; rowIndex < source.Count + 1; rowIndex++)
            {
                columnIndex = 0;
                foreach (string field in availableFields)
                {
                    var tempRow = sheet.GetRow(rowIndex);
                    var tempCell = tempRow.GetCell(columnIndex);
                    PropertyInfo p = fieldNameMapping[field];
                    SetCellValue<T>(tempCell, p.GetValue(source[rowIndex - 1], null));
                    columnIndex++;
                }
            }
            columnIndex = 0;
            //标头
            foreach (var columns in columnsNameMapping)
            {
                sheet.AutoSizeColumn(columnIndex);
                columnIndex++;
            }
        }
        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cell"></param>
        /// <param name="data"></param>
        private void SetCellValue<T>(ICell cell, object val)
        {
            if (cell == null)
            {
                return;
            }
            if (val != null)
            {
                var valType = val.GetType();

                switch (valType.ToString())
                {
                    case "System.String"://字符串类型
                        cell.SetCellValue(Convert.ToString(val));
                        break;
                    case "System.DateTime"://日期类型
                        cell.SetCellValue((DateTime)val);
                        ICellStyle cellStyle = hssfWorkBook.CreateCellStyle();
                        IDataFormat format = hssfWorkBook.CreateDataFormat();   
                        cellStyle.DataFormat = format.GetFormat("yyyy-m-d");                        
                        cell.CellStyle = cellStyle;                        
                        break;
                    case "System.Boolean"://布尔型
                        cell.SetCellValue((bool)val);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        cell.SetCellValue(Convert.ToDouble(val));
                        break;
                    case "System.DBNull"://空值处理
                        cell.SetCellValue("");
                        break;
                    default:
                        cell.SetCellValue("");
                        break;
                }
            }
            else
            {
                cell.SetCellValue("");
            }
        }
        private Dictionary<string, string> GetColumnsName(PropertyInfo[] infos, string[] availableFields, ref Dictionary<string, PropertyInfo> fieldNameMapping)
        {
            int exportFieldCount = availableFields.Length;
            Dictionary<string, string> columnsName = new Dictionary<string, string>();
            foreach (string field in availableFields)
            {
                foreach (PropertyInfo info in infos)
                {
                    if (field.ToLower() == info.Name.ToLower())
                    {
                        if (0 < info.GetCustomAttributes(typeof(ExportDisplayNameAttribute), false).Length)
                        {
                            ExportDisplayNameAttribute[] atts = (ExportDisplayNameAttribute[])info.GetCustomAttributes(typeof(ExportDisplayNameAttribute), false);
                            ExportDisplayNameAttribute[] nameAtt = atts.Where(t => !string.IsNullOrEmpty(t.Name)).ToArray<ExportDisplayNameAttribute>();
                            string displayName = nameAtt.Length > 0 ? nameAtt[0].Name : info.Name;
                            columnsName.Add(info.Name.ToLower(), displayName);
                        }
                        else
                        {
                            columnsName.Add(info.Name.ToLower(), "未指定名称");
                        }
                        fieldNameMapping.Add(info.Name.ToLower(), info);
                    }
                }
            }
            return columnsName;
        }
        private void InitializeWorkbook()
        {
            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "http://www.yungujia.com/";
            if (hssfWorkBook != null)
            {
                hssfWorkBook.DocumentSummaryInformation = dsi;
            }

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "http://www.yungujia.com/";
            si.Title = "云估价";
            if (hssfWorkBook != null)
            {
                hssfWorkBook.SummaryInformation = si;
            }
        }


    }
}

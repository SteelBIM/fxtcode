using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Spire.Doc;
//using Spire.Doc.Documents;
//using Spire.Doc.Fields;
//using Spire.Doc.Interface;
//using Spire.Doc.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using NPOIM = NPOI.XWPF.UserModel;
using NPOIOW=NPOI.OpenXmlFormats.Wordprocessing;
//using DOC=Novacode;
//using SD = Spire.Doc;
//using SDD = Spire.Doc.Documents;
//using SDF = Spire.Doc.Fields;
//using SDC=Spire.Doc.Collections;
using System.Drawing;


namespace CAS.Office.NPOI
{
    /// <summary>
    /// 封装各个项目使用封装的帮助类
    /// 目的各项目的逻辑与WORD与EXCEL的封装，在各项目层中不暴露Doc,Xls的具体子对象.
    /// byte-侯湘岳
    /// </summary>
    class MixedHelper
    {
        #region 使用Spire.doc实现的功能
        /*
        /// <summary>
        /// 将Excel中的sheet表中的某名称中的表格内容绘制一份到word特定的表格中
        /// </summary>
        public static void ConvertExcelToWordTable(List<string> openTargetExcelFile, Doc docHelper, string bookmarkName, string sheetName, string regionName)
        {
            if (null == docHelper)
                throw new Exception("docHelper is null");
            Xls xlsHeper = null;
            try
            {
                BookmarkCollection bookmarks = docHelper.Bookmarks;
                Bookmark bookmark = bookmarks.FindByName(bookmarkName);
                if (null != bookmark)
                {
                    docHelper.ClearBookmarkContent(bookmarkName);
                    foreach (string targetExcelFile in openTargetExcelFile)
                    {
                        if (!File.Exists(targetExcelFile))
                            continue;
                        xlsHeper = new Xls(targetExcelFile, true);
                        List<Assist.MergedRegion> mergedRegionList = null;
                        Dictionary<string, string> regionValues; int rows, columns;
                        mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
                        if (null != regionValues)           //找到了该sheet表下面的该名称管理器
                        {
                            Table table = docHelper.InitTable(mergedRegionList, regionValues, rows, columns);
                            docHelper.InsertTableByBookmark(bookmarkName, table);
                            docHelper.InsertEmptyParagraphByBookmark(bookmarkName); //如果连续向一个书签中插入表格，表格内容会黏在一起，需要用换行符分割。
                        }
                        xlsHeper.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != xlsHeper)
                    xlsHeper.Close();
            }
        }
        */
        #endregion
        /// <summary>
        /// 修改人rock 20151121,添加测算表中图片生成到word
        /// excel内容生成到报告word中
        /// </summary>
        /// <param name="openTargetExcelFile">多个测算表excel文件</param>
        /// <param name="insertImageExcelFile">要获取图片并插入到报告的excel</param>
        /// <param name="document">当前正在生成的word</param>
        /// <param name="specifyTableText">测算过程要生成到以哪个名称为内容的表格</param>
        /// <param name="sheetName">获取excel的哪个sheet</param>
        /// <param name="prefixRegionName">测算过程要生成的命名管理器前缀</param>
        /// <param name="tempPath">在生成图片时要存放的临时文件的文件夹路径</param>
        /// <param name="imgRegionName">测算图片要生成到以哪个名称为内容的书签</param>
        public static void ConvertExcelToWordTable(List<string> openTargetExcelFile, List<string> insertImageExcelFile, Document document, string specifyTableText, string sheetName, string prefixRegionName, string tempPath, string imgRegionName)
        {

            Xls xlsHeper = null;
            try
            {
                if (null != document)
                {
                    List<string> imgPathList = new List<string>();//存储excel中的图片路径
                    Table table = null;                    
                    for (int tableIndex = 1; tableIndex <= document.Tables.Count; tableIndex++)
                    {                        
                        Range range = document.Tables[tableIndex].Cell(1, 1).Range;
                        range.MoveEnd(1, -1);   //返回表格单元格中的文字，不包括单元格的结束标记         
                        string rangeText = range.Text;
                        if (specifyTableText == rangeText)
                        {
                            table = document.Tables[tableIndex];
                            break;
                        }
                    }
                    if (null != table)//找到指定的表格
                    {
                        #region
                        if (!document.Bookmarks.Exists(specifyTableText)) //没找到指定的书签
                        {
                            Range tableRange = table.Range;
                            document.Bookmarks.Add(specifyTableText, tableRange);
                        }
                        foreach (string targetExcelFile in openTargetExcelFile)
                        {

                            if (!File.Exists(targetExcelFile))
                                continue;

                            #region

                            xlsHeper = new Xls(targetExcelFile, true);
                            if (xlsHeper.workbook == null)
                            {
                                continue;
                            }
                            List<byte[]> imgByteList = new List<byte[]>();//存储excel中的图片字节信息
                            List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName, out imgByteList);
                            //如果此excel不需要获取图片
                            if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() == null)
                            {
                                imgByteList = new List<byte[]>();
                            }
                            if (null != regionNameList)
                            {
                                foreach (string regionName in regionNameList)
                                {

                                    #region

                                    List<Assist.MergedRegion> mergedRegionList = null;
                                    Dictionary<string, string> regionValues; int rows, columns;
                                    mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
                                    if (null != regionValues)           //找到了该sheet表下面的该名称管理器
                                    {
                                        Row row = table.Rows.Add();
                                        row.Cells[1].Select();
                                        Table newTable = row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
                                        for (int r = 1; r <= rows; r++)
                                        {
                                            if (newTable.Rows.Count < rows)
                                            {
                                                newTable.Rows.Add();
                                            }
                                            string key = string.Empty;
                                            for (int c = 1; c <= columns; c++)
                                            {
                                                key = (r - 1).ToString() + "," + (c - 1).ToString();
                                                newTable.Cell(r, c).Range.Text = regionValues[key];
                                            }
                                        }
                                        if (null != mergedRegionList)
                                        {
                                            #region
                                            /*在Word的表格中，行列的下标都是从1开始*/
                                            Dictionary<string, int> dic = new Dictionary<string, int>();
                                            var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
                                            //先水平
                                            foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
                                            {
                                                if (mergedRegion.StartRowIndex == 9)
                                                {
                                                    ;
                                                }
                                                int mergeCell = 0;
                                                int rowNumber = mergedRegion.StartRowIndex + 1;
                                                if (!dic.ContainsKey(rowNumber.ToString()))
                                                {
                                                    dic[rowNumber.ToString()] = mergeCell;
                                                }
                                                else
                                                {
                                                    mergeCell = dic[rowNumber.ToString()];
                                                }
                                                Cell cell = newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
                                                for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
                                                {
                                                    cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。
                                                    dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
                                                    mergeCell = dic[rowNumber.ToString()];
                                                }
                                            }
                                            dic.Clear();
                                            /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
                                            var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
                                            //后垂直
                                            foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
                                            {
                                                int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
                                                int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引

                                                Cell cell = newTable.Cell(rowNumber, cellNumber);
                                                for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
                                                {
                                                    cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
                                                    cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
                                                }
                                            }
                                            #endregion
                                        }
                                        newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
                                        newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
                                        row.Cells[1].Range.Paragraphs.Add();//换行符
                                    }

                                    #endregion

                                }
                            }
                            #endregion

                            //将测算表中图片插入到当前测算过程的末尾
                            #region

                            if (imgByteList != null && imgByteList.Count() > 0)
                            {
                                Row row = table.Rows.Add();
                                row.Cells[1].Range.Text = imgRegionName;
                                Row row2 = table.Rows.Add();
                                foreach (byte[] imgB in imgByteList)
                                {
                                    MemoryStream ms = new MemoryStream(imgB);
                                    Image image = System.Drawing.Image.FromStream(ms);
                                    string s = Guid.NewGuid().ToString().Replace("-", "");
                                    s = s.Substring(0, 10 > s.Length ? s.Length : 10);
                                    string imgFileName = tempPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + s + ".jpg";
                                    image.Save(imgFileName);
                                    imgPathList.Add(imgFileName);
                                    row2.Cells[1].Range.InlineShapes.AddPicture(imgFileName, false, true, row2.Cells[1].Range);
                                    row2.Cells[1].Range.Paragraphs.Add();//换行符
                                }
                            }

                            #endregion

                            xlsHeper.Close();
                        }
                        table.Rows[1].Delete();

                        #endregion
                    }
                    else //没找到指定表格时 则要获取excel中图片
                    {
                        #region
                        foreach (string targetExcelFile in openTargetExcelFile)
                        {

                            if (!File.Exists(targetExcelFile))
                            {
                                continue;
                            }

                            xlsHeper = new Xls(targetExcelFile, true);
                            if (xlsHeper.workbook == null)
                            {
                                continue;
                            }
                            List<byte[]> imgByteList = new List<byte[]>();//存储excel中的图片字节信息
                            List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName, out imgByteList);
                            //如果此excel不需要获取图片
                            if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() == null)
                            {
                                imgByteList = new List<byte[]>();
                            }
                            //将测算表中图片插入到当前测算过程的末尾
                            #region

                            if (imgByteList != null && imgByteList.Count() > 0)
                            {
                                foreach (byte[] imgB in imgByteList)
                                {
                                    MemoryStream ms = new MemoryStream(imgB);
                                    Image image = System.Drawing.Image.FromStream(ms);
                                    string s = Guid.NewGuid().ToString().Replace("-", "");
                                    s = s.Substring(0, 10 > s.Length ? s.Length : 10);
                                    string imgFileName = tempPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + s + ".jpg";
                                    image.Save(imgFileName);
                                    imgPathList.Add(imgFileName);
                                }
                            }

                            #endregion
                        }
                        #endregion
                    }
                    //如果有指定将测算图片生成到某个书签的话则进行插入
                    if (imgPathList != null && imgPathList.Count() > 0 && document.Bookmarks.Exists(imgRegionName))
                    {
                        Range imgRange = document.Bookmarks[imgRegionName].Range;
                        foreach (string imgPath in imgPathList)
                        {
                            imgRange.InlineShapes.AddPicture(imgPath, false, true, imgRange);
                            imgRange.Paragraphs.Add();//换行符
                        }

                    }
                    //图片插入成功后删除图片临时文件
                    foreach (string imgPath in imgPathList)
                    {
                        try
                        {
                            File.Delete(imgPath);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != xlsHeper)
                    xlsHeper.Close();
            }
        }

        //public static void ConvertExcelToWordTable2(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
        //    FileStream input = File.OpenRead(path);
        //    NPOIM.XWPFDocument document = new NPOIM.XWPFDocument(input);
        //    input.Close();
        //    //NPOIM.XWPFDocument aa = new NPOIM.XWPFDocument();    
        //    //aa.Document.body
        //    //NPOIM.XWPFParagraph p1 = aa.CreateParagraph();
        //    //NPOIM.XWPFTable tab = aa.Tables[0];                   
        //    Xls xlsHeper = null;
        //    try
        //    {
        //        if (null != document)
        //        {
        //            NPOIM.XWPFTable table = null;
        //            for (int tableIndex = 1; tableIndex <= document.Tables.Count; tableIndex++)
        //            {
        //                ////Range range = document.Tables[tableIndex].Cell(1, 1).Range;
        //                //// range.MoveEnd(1, -1);   //返回表格单元格中的文字，不包括单元格的结束标记         
        //                string rangeText = document.Tables[tableIndex - 1].GetRow(0).GetCell(0).GetText();////range.Text;
        //                if (specifyTableText == rangeText)
        //                {
        //                    table = document.Tables[tableIndex - 1];
        //                    break;
        //                }
        //            }
        //            if (null == table)      //没找到指定的表格
        //                return;
        //            //if (!document.Bookmarks.Exists(specifyTableText)) //没找到指定的书签
        //            //{
        //            //    Range tableRange = table.Range;
        //            //    document.Bookmarks.Add(specifyTableText, tableRange); 
        //            //}                   
        //            foreach (string targetExcelFile in openTargetExcelFile)
        //            {
        //                if (!File.Exists(targetExcelFile))
        //                    continue;
        //                xlsHeper = new Xls(targetExcelFile, true);
        //                List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName);
        //                if (null == regionNameList)
        //                    continue;
        //                foreach (string regionName in regionNameList)
        //                {
        //                    List<Assist.MergedRegion> mergedRegionList = null;
        //                    Dictionary<string, string> regionValues; int rows, columns;
        //                    mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
        //                    if (null != regionValues)           //找到了该sheet表下面的该名称管理器
        //                    {
        //                        NPOIM.XWPFTableRow row = table.CreateRow();//// table.Rows.Add();                                
        //                        ////row.Cells[1].Select();
        //                        ////Table newTable = row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
        //                        //NPOIM.XWPFTable newTable =row.GetCell(0).in .GetParagraphArray(0).Document.CreateTable(rows, columns);// row.GetCell(0).GetXWPFDocument().CreateTable(rows, columns);
        //                        NPOIM.IBody b = row.GetCell(0);
                                
        //                        row.GetCell(0).SetText(regionName);
        //                        NPOIM.XWPFTable newTable = new NPOIM.XWPFTable(new NPOIOW.CT_Tbl(), b, rows, columns);
        //                       // row.GetCell(0).InsertTable(1,newTable                                
        //                         //row.GetCell(0).GetParagraphArray(0).Document.CreateTable

        //                        for (int r = 1; r <= rows; r++)
        //                        {
        //                            if (newTable.Rows.Count < rows)
        //                            {
        //                                ////newTable.Rows.Add();
        //                                newTable.CreateRow();
        //                            }
        //                            string key = string.Empty;
        //                            for (int c = 1; c <= columns; c++)
        //                            {
        //                                key = (r - 1).ToString() + "," + (c - 1).ToString();
        //                                //newTable.Cell(r, c).Range.Text = regionValues[key];
        //                                newTable.GetRow(r - 1).GetCell(c - 1).SetText(regionValues[key]);
        //                            }
        //                        }
        //                        if (null != mergedRegionList)
        //                        {
        //                            /*在Word的表格中，行列的下标都是从1开始*/
        //                            Dictionary<string, int> dic = new Dictionary<string, int>();
        //                            var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
        //                            //先水平
        //                            foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
        //                            {
        //                                if (mergedRegion.StartRowIndex == 9)
        //                                {
        //                                    ;
        //                                }
        //                                int mergeCell = 0;
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;
        //                                if (!dic.ContainsKey(rowNumber.ToString()))
        //                                {
        //                                    dic[rowNumber.ToString()] = mergeCell;
        //                                }
        //                                else
        //                                {
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                                NPOIM.XWPFTableCell cell = newTable.GetRow(rowNumber - 1).GetCell((mergedRegion.StartCellIndex + 1 - mergeCell) - 1);
        //                                ////Cell cell = newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
        //                                for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
        //                                {

        //                                    newTable.SetCellMargins(rowNumber - 1, (mergedRegion.StartCellIndex + 1 - mergeCell) - 1, rowNumber - 1, c - mergeCell - 1);
        //                                    ////cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。
        //                                    dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                            }
        //                            dic.Clear();
        //                            /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
        //                            var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
        //                            //后垂直
        //                            foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
        //                            {
        //                                int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引

        //                                ////Cell cell = newTable.Cell(rowNumber, cellNumber);
        //                                for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
        //                                {
        //                                    cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
        //                                    newTable.SetCellMargins(rowNumber - 1, cellNumber - 1, r - 1, cellNumber - 1);
        //                                    ////cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
        //                                }
        //                            }
        //                        } 
        //                        row.GetCell(0).InsertTable(0, newTable);
        //                        //row.GetCell(0).Tables.Add(newTable);
        //                        //newTable.SetBottomBorder(NPOIM.XWPFTable.XWPFBorderType.THICK, 1, 1, "black");
        //                        ////newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
        //                        ////newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
        //                        ////row.Cells[1].Range.Paragraphs.Add();//换行符

        //                        row.GetCell(0).GetXWPFDocument().CreateParagraph();
        //                    }
        //                }
        //                xlsHeper.Close();
        //            }
        //            ////table.Rows[1].Delete();
        //            table.RemoveRow(0);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (null != xlsHeper)
        //            xlsHeper.Close();
        //    }

        //    FileStream sw = File.Create("F:\\testnpoi.docx");
        //    document.Write(sw);
        //    sw.Close();
        //    FileStream fs = File.OpenWrite(path);
        //    document.Write(fs);
        //    fs.Close();
        //}
        //public static void ConvertExcelToWordTable3(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
        //    //DOC.DocX document = DOC.DocX.Load(path);
        //    //Xls xlsHeper = null;
        //    //try
        //    //{
        //    //    if (null != document)
        //    //    {
        //    //        DOC.Table table = null;
        //    //        for (int tableIndex = 1; tableIndex <= document.Tables.Count; tableIndex++)
        //    //        {
        //    //            ////Range range = document.Tables[tableIndex].Rows[1].Cells[1].ToString();//// document.Tables[tableIndex].Cell(1, 1).Range;
        //    //            ////range.MoveEnd(1, -1);   //返回表格单元格中的文字，不包括单元格的结束标记         
        //    //            string rangeText = document.Tables[tableIndex].Rows[1].Cells[1].ToString();
        //    //            if (specifyTableText == rangeText)
        //    //            {
        //    //                table = document.Tables[tableIndex];
        //    //                break;
        //    //            }
        //    //        }
        //    //        if (null == table)      //没找到指定的表格
        //    //            return;

        //    //        //if (!document.Bookmarks.Exists(specifyTableText)) //没找到指定的书签
        //    //        //{
        //    //        //    Range tableRange = table.Range;
        //    //        //    document.Bookmarks.Add(specifyTableText, tableRange);
        //    //        //}
        //    //        foreach (string targetExcelFile in openTargetExcelFile)
        //    //        {
        //    //            if (!File.Exists(targetExcelFile))
        //    //                continue;
        //    //            xlsHeper = new Xls(targetExcelFile, true);
        //    //            List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName);
        //    //            if (null == regionNameList)
        //    //                continue;
        //    //            foreach (string regionName in regionNameList)
        //    //            {
        //    //                List<Assist.MergedRegion> mergedRegionList = null;
        //    //                Dictionary<string, string> regionValues; int rows, columns;
        //    //                mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
        //    //                if (null != regionValues)           //找到了该sheet表下面的该名称管理器
        //    //                {
        //    //                    DOC.Row row = table.InsertRow();//// table.Rows.Add();
        //    //                    ////row.Cells[1].Select();

        //    //                    DOC.Table newTable = row.Cells[1].InsertTable(rows, columns);//// row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
        //    //                    for (int r = 1; r <= rows; r++)
        //    //                    {
        //    //                        if (newTable.Rows.Count < rows)
        //    //                        {
        //    //                            table.InsertRow();
        //    //                            ////newTable.Rows.Add();
        //    //                        }
        //    //                        string key = string.Empty;
        //    //                        for (int c = 1; c <= columns; c++)
        //    //                        {
        //    //                            key = (r - 1).ToString() + "," + (c - 1).ToString();
        //    //                            newTable.Rows[r].Cells[c].Paragraphs[0].Append(regionValues[key]);
        //    //                            ////newTable.Cell(r, c).Range.Text = regionValues[key];
        //    //                        }
        //    //                    }
        //    //                    if (null != mergedRegionList)
        //    //                    {
        //    //                        /*在Word的表格中，行列的下标都是从1开始*/
        //    //                        Dictionary<string, int> dic = new Dictionary<string, int>();
        //    //                        var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
        //    //                        //先水平
        //    //                        foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
        //    //                        {
        //    //                            if (mergedRegion.StartRowIndex == 9)
        //    //                            {
        //    //                                ;
        //    //                            }
        //    //                            int mergeCell = 0;
        //    //                            int rowNumber = mergedRegion.StartRowIndex + 1;
        //    //                            if (!dic.ContainsKey(rowNumber.ToString()))
        //    //                            {
        //    //                                dic[rowNumber.ToString()] = mergeCell;
        //    //                            }
        //    //                            else
        //    //                            {
        //    //                                mergeCell = dic[rowNumber.ToString()];
        //    //                            }
        //    //                            DOC.Cell cell = newTable.Rows[rowNumber].Cells[mergedRegion.StartCellIndex + 1 - mergeCell]; ////newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
        //    //                            for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
        //    //                            {
        //    //                                newTable.Rows[rowNumber].MergeCells(mergedRegion.StartCellIndex + 1 - mergeCell, c - mergeCell);
        //    //                                ////cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。
        //    //                                dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
        //    //                                mergeCell = dic[rowNumber.ToString()];
        //    //                            }
        //    //                        }
        //    //                        dic.Clear();
        //    //                        /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
        //    //                        var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
        //    //                        //后垂直
        //    //                        foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
        //    //                        {
        //    //                            int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
        //    //                            int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引
        //    //                            Cell cell = newTable.Cell(rowNumber, cellNumber);
        //    //                            for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
        //    //                            {
        //    //                                cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
        //    //                                cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
        //    //                            }
        //    //                        }
        //    //                    }
        //    //                    newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
        //    //                    newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
        //    //                    row.Cells[1].Range.Paragraphs.Add();//换行符
        //    //                }
        //    //            }
        //    //            xlsHeper.Close();
        //    //        }
        //    //        table.Rows[1].Delete();
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //    //finally
        //    //{
        //    //    if (null != xlsHeper)
        //    //        xlsHeper.Close();
        //    //}
        //}
        //public static void ConvertExcelToWordTable4(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{

        //    FileStream input = File.OpenRead(path);
        //    SD.Document document = new SD.Document(input);
        //    input.Close();
        //    Xls xlsHeper = null;
        //    try
        //    {
        //        if (null != document)
        //        {

        //            SDC.BookmarkCollection bookmarks = document.Bookmarks;
        //            SD.Bookmark bookmark = bookmarks.FindByName(specifyTableText);
        //            if (bookmark != null) //没找到指定的书签
        //            {
        //                ConvertExcelToWordTableByBookMark2(openTargetExcelFile, path, specifyTableText, sheetName, prefixRegionName);
        //                return;
        //                //table.row
        //                //bookmark = new SD.Bookmark(new SD.BookmarkStart(document, specifyTableText));
        //                ////SDD.BookmarksNavigator bookmarkNavigator = new SDD.BookmarksNavigator(document);
        //                ////table.Document.Sections[0].Paragraphs[0].
        //                ////bookmarkNavigator.MoveToBookmark(specifyTableText);
        //                ////bookmarkNavigator.InsertTable(table);
        //                //table.Document.Sections[0].Paragraphs[0].AppendBookmarkStart(specifyTableText);
        //            }
        //            SD.Table table = null;
        //            for (int tableIndex = 1; tableIndex <= document.Sections[0].Tables.Count; tableIndex++)
        //            {
        //                ////Range range = document.Sections[0].Tables[tableIndex - 1].Rows[0].Cells[0].ToString();
        //                ////range.MoveEnd(1, -1);   //返回表格单元格中的文字，不包括单元格的结束标记         
        //                string rangeText = document.Sections[0].Tables[tableIndex - 1].Rows[0].Cells[0].Paragraphs[0].Text;
        //                if (specifyTableText == rangeText)
        //                {
        //                    table = document.Sections[0].Tables[tableIndex - 1] as SD.Table;
        //                    break;
        //                }
        //            }
        //            if (null == table)      //没找到指定的表格
        //                return;

        //            //SDC.BookmarkCollection bookmarks = document.Bookmarks;
        //            //SD.Bookmark bookmark = bookmarks.FindByName(specifyTableText);
        //            //if (bookmark == null) //没找到指定的书签
        //            //{
        //            //    SDD.BookmarksNavigator bookmarkNavigator = new SDD.BookmarksNavigator(document);
        //            //    bookmarkNavigator.MoveToBookmark(specifyTableText);
        //            //    //table.row
        //            //    //bookmark = new SD.Bookmark(new SD.BookmarkStart(document, specifyTableText));
        //            //    ////SDD.BookmarksNavigator bookmarkNavigator = new SDD.BookmarksNavigator(document);
        //            //    ////table.Document.Sections[0].Paragraphs[0].
        //            //    ////bookmarkNavigator.MoveToBookmark(specifyTableText);
        //            //    ////bookmarkNavigator.InsertTable(table);
        //            //    //table.Document.Sections[0].Paragraphs[0].AppendBookmarkStart(specifyTableText);
        //            //}
        //            ////if (!document.Bookmarks.Exists(specifyTableText)) //没找到指定的书签
        //            ////{
        //            ////    Range tableRange = table.Range;
        //            ////    document.Bookmarks.Add(specifyTableText, tableRange);
        //            ////}
        //            foreach (string targetExcelFile in openTargetExcelFile)
        //            {
        //                if (!File.Exists(targetExcelFile))
        //                    continue;
        //                xlsHeper = new Xls(targetExcelFile, true);
        //                List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName);
        //                if (null == regionNameList)
        //                    continue;
        //                foreach (string regionName in regionNameList)
        //                {
        //                    List<Assist.MergedRegion> mergedRegionList = null;
        //                    Dictionary<string, string> regionValues; int rows, columns;
        //                    mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
        //                    if (null != regionValues)           //找到了该sheet表下面的该名称管理器
        //                    {
        //                        SD.TableRow row = table.AddRow();// table.Rows.Add();
        //                        ////row.Cells[1].Select();
        //                        ////Table newTable = row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
        //                        SD.Table newTable = row.Cells[0].AddTable(true);
        //                        newTable.ResetCells(rows, columns);
        //                        for (int r = 1; r <= rows; r++)
        //                        {
        //                            if (newTable.Rows.Count < rows)
        //                            {
        //                                ////newTable.Rows.Add();
        //                                newTable.AddRow();
        //                            }
        //                            string key = string.Empty;
        //                            for (int c = 1; c <= columns; c++)
        //                            {
        //                                key = (r - 1).ToString() + "," + (c - 1).ToString();
        //                                ////newTable.Cell(r, c).Range.Text = regionValues[key];
        //                                SDD.Paragraph p = newTable.Rows[r - 1].Cells[c - 1].AddParagraph();
        //                                p.Text = regionValues[key];
        //                            }
        //                        }
        //                        if (null != mergedRegionList)
        //                        {
        //                            /*在Word的表格中，行列的下标都是从1开始*/
        //                            Dictionary<string, int> dic = new Dictionary<string, int>();
        //                            var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
        //                            //先水平
        //                            foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
        //                            {
        //                                if (mergedRegion.StartRowIndex == 9)
        //                                {
        //                                    ;
        //                                }
        //                                int mergeCell = 0;
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;
        //                                if (!dic.ContainsKey(rowNumber.ToString()))
        //                                {
        //                                    dic[rowNumber.ToString()] = mergeCell;
        //                                }
        //                                else
        //                                {
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                                ////Cell cell = newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
        //                                int mergeRowIndex = rowNumber - 1;
        //                                int mergeStartColIndex = (mergedRegion.StartCellIndex + 1) - 1;
        //                                for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
        //                                {
        //                                    int mergeEndColIndex = (c) - 1;
        //                                    if (mergeStartColIndex > mergeEndColIndex)
        //                                    {
        //                                        int _sindex = mergeStartColIndex;
        //                                        mergeStartColIndex = mergeEndColIndex;
        //                                        mergeEndColIndex = _sindex;
        //                                    }
        //                                    newTable.ApplyHorizontalMerge(mergeRowIndex, mergeStartColIndex, mergeEndColIndex);
        //                                    ////cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。
        //                                    dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                            }
        //                            dic.Clear();
        //                            /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
        //                            var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
        //                            //后垂直
        //                            foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
        //                            {
        //                                int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引

        //                                ////Cell cell = newTable.Cell(rowNumber, cellNumber);
        //                                for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
        //                                {
        //                                    cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
        //                                    ////cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
        //                                    newTable.ApplyVerticalMerge(cellNumber - 1, rowNumber - 1, r - 1);
        //                                }
        //                            }
        //                        }
        //                        ////newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
        //                        ////newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
        //                        ////row.Cells[1].Range.Paragraphs.Add();//换行符

        //                        row.Cells[0].AddParagraph();
        //                        row.Cells[0].AddParagraph();
        //                    }
        //                }
        //                xlsHeper.Close();
        //            }
        //            ////table.Rows[1].Delete();

        //            table.Rows.RemoveAt(0);
        //            if (bookmark == null) //没找到指定的书签
        //            {
        //                //bookmark.BookmarkEnd = new SD.BookmarkEnd(document, specifyTableText);
        //                //SDD.BookmarksNavigator bookmarkNavigator = new SDD.BookmarksNavigator(document);
        //                //bookmarkNavigator.MoveToBookmark(specifyTableText);
        //                //bookmarkNavigator.InsertTable(table);
        //                ////table.Document.Sections[0].Paragraphs[0].AppendBookmarkStart(specifyTableText);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (null != xlsHeper)
        //            xlsHeper.Close();
        //    }
        //    document.SaveToFile(path);
        //}

        //public static void ConvertExcelToWordTableByBookMark2(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
            
        //    FileStream input = File.OpenRead(path);
        //    SD.Document document = new SD.Document(input);
        //    input.Close();
        //    Xls xlsHeper = null;
        //    try
        //    {
        //        if (null != document)
        //        {

        //            SDC.BookmarkCollection bookmarks = document.Bookmarks;
        //            SD.Bookmark bookmark = bookmarks.FindByName(specifyTableText);
        //            if (bookmark==null) //没找到指定的书签
        //            {
        //                return;
        //            }
        //            ////Range tableRange = document.Bookmarks[specifyTableText].Range;
        //            //删除以前的内容
        //            SDD.BookmarksNavigator bookmarkNavigator = new SDD.BookmarksNavigator(document);
        //            bookmarkNavigator.MoveToBookmark(specifyTableText);
        //            //bookmarkNavigator.InsertTable

        //            //////删除以前的表格
        //            ////for (; tableRange.Tables.Count > 0; )
        //            ////{
        //            ////    tableRange.Tables[1].Delete();
        //            ////}
        //            ////tableRange.Text = " ";

        //            SD.Table table = new SD.Table(document, true);
        //            table.ResetCells(1, 1);
        //            ////Table table = tableRange.Tables.Add(document.Application.Selection.Range, 1, 1, Type.Missing, Type.Missing);
        //            ////table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;    //设置表格有无边框 
        //            ////table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;     //设置表格有无边框 
        //            foreach (string targetExcelFile in openTargetExcelFile)
        //            {
        //                #region
        //                if (!File.Exists(targetExcelFile))
        //                    continue;
        //                xlsHeper = new Xls(targetExcelFile, true);
        //                List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName);
        //                if (null == regionNameList)
        //                    continue;
        //                foreach (string regionName in regionNameList)
        //                {
        //                    List<Assist.MergedRegion> mergedRegionList = null;
        //                    Dictionary<string, string> regionValues; int rows, columns;
        //                    mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
        //                    if (null != regionValues)           //找到了该sheet表下面的该名称管理器
        //                    {
        //                        SD.TableRow row = table.AddRow();// table.Rows.Add();
        //                        ////row.Cells[1].Select();
        //                        SD.Table newTable = row.Cells[0].AddTable(true);
        //                        newTable.ResetCells(rows, columns);
        //                        //Table newTable = row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
        //                        for (int r = 1; r <= rows; r++)
        //                        {
        //                            if (newTable.Rows.Count < rows)
        //                            {
        //                                newTable.AddRow();
        //                                ////newTable.Rows.Add();
        //                            }
        //                            string key = string.Empty;
        //                            for (int c = 1; c <= columns; c++)
        //                            {
        //                                key = (r - 1).ToString() + "," + (c - 1).ToString();
        //                                SDD.Paragraph p = newTable.Rows[r - 1].Cells[c - 1].AddParagraph();
        //                                p.Text = regionValues[key];
        //                                ////newTable.Cell(r, c).Range.Text = regionValues[key];
        //                            }
        //                        }
        //                        if (null != mergedRegionList)
        //                        {
        //                            /*在Word的表格中，行列的下标都是从1开始*/
        //                            Dictionary<string, int> dic = new Dictionary<string, int>();
        //                            var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
        //                            //先水平
        //                            foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
        //                            {
        //                                if (mergedRegion.StartRowIndex == 9)
        //                                {
        //                                    ;
        //                                }
        //                                int mergeCell = 0;
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;
        //                                if (!dic.ContainsKey(rowNumber.ToString()))
        //                                {
        //                                    dic[rowNumber.ToString()] = mergeCell;
        //                                }
        //                                else
        //                                {
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                                SD.TableCell cell = newTable.Rows[rowNumber - 1].Cells[(mergedRegion.StartCellIndex + 1) - 1];
        //                                //// Cell cell = newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
        //                                int mergeRowIndex = rowNumber-1;
        //                                int mergeStartColIndex = (mergedRegion.StartCellIndex + 1) - 1;
        //                                for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
        //                                {
        //                                    int mergeEndColIndex = (c) - 1;
        //                                    if (mergeStartColIndex > mergeEndColIndex)
        //                                    {
        //                                        int _sindex = mergeStartColIndex;
        //                                        mergeStartColIndex = mergeEndColIndex;
        //                                        mergeEndColIndex = _sindex;
        //                                    }
        //                                    newTable.ApplyHorizontalMerge(mergeRowIndex, mergeStartColIndex, mergeEndColIndex);
        //                                    ////cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。//在Spire.Doc中不会自减所以不用减mergeCell
        //                                    dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
        //                                    mergeCell = dic[rowNumber.ToString()];
        //                                }
        //                            }
        //                            dic.Clear();
        //                            /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
        //                            var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
        //                            //后垂直
        //                            foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
        //                            {
        //                                int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
        //                                int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引

        //                                ////Cell cell = newTable.Cell(rowNumber, cellNumber);
        //                                for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
        //                                {
        //                                    cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
        //                                    ////cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
        //                                    newTable.ApplyVerticalMerge(cellNumber - 1, rowNumber - 1, r - 1);
        //                                }
        //                            }
        //                        }
        //                        ////newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
        //                        ////newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
        //                        ////row.Cells[1].Range.Paragraphs.Add();//换行符
        //                        row.Cells[0].AddParagraph();
        //                        row.Cells[0].AddParagraph();
        //                        row.Cells[0].AddParagraph();
        //                    }
        //                }
        //                xlsHeper.Close();
        //                #endregion
        //            }
        //            table.Rows.RemoveAt(0);
        //            bookmarkNavigator.InsertTable(table);
        //            //bookmarkNavigator
        //            //table.Rows[1].Delete();
        //            //document.Bookmarks.Add(specifyTableText, tableRange);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (null != xlsHeper)
        //            xlsHeper.Close();
        //    }
        //    document.SaveToFile("F:\\testnpoi.docx");
        //    document.SaveToFile(path);
        //    //FileStream sw = File.Create("F:\\testnpoi.docx");
        //    //document.Write(sw);
        //    //sw.Close();
        //    //FileStream fs = File.OpenWrite(path);
        //    //document.Write(fs);
        //    //fs.Close();
        //}

        public static void ConvertExcelToWordTableByBookMark(List<string> openTargetExcelFile, List<string> insertImageExcelFile, Document document, string specifyTableText, string sheetName, string prefixRegionName, string tempPath, string imgRegionName)
        {
            Xls xlsHeper = null;
            try
            {
                if (null != document)
                {

                    List<string> imgPathList = new List<string>();//存储excel中的图片路径
                    if (document.Bookmarks.Exists(specifyTableText)) //找到指定的书签
                    {

                        Range tableRange = document.Bookmarks[specifyTableText].Range;
                        //删除以前的内容
                        tableRange.Select();
                        //删除以前的表格
                        for (; tableRange.Tables.Count > 0; )
                        {
                            tableRange.Tables[1].Delete();
                        }
                        tableRange.Text = " ";
                        Table table = tableRange.Tables.Add(document.Application.Selection.Range, 1, 1, Type.Missing, Type.Missing);
                        table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;    //设置表格有无边框 
                        table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;     //设置表格有无边框 
                        foreach (string targetExcelFile in openTargetExcelFile)
                        {
                            #region
                            if (!File.Exists(targetExcelFile))
                                continue;
                            xlsHeper = new Xls(targetExcelFile, true);
                            if (xlsHeper.workbook == null)
                            {
                                continue;
                            }
                            List<byte[]> imgByteList = new List<byte[]>();
                            List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName, out imgByteList);
                            //如果此excel不需要获取图片
                            if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() == null)
                            {
                                imgByteList = new List<byte[]>();
                            }
                            if (null != regionNameList)
                            {
                                foreach (string regionName in regionNameList)
                                {
                                    #region

                                    List<Assist.MergedRegion> mergedRegionList = null;
                                    Dictionary<string, string> regionValues; int rows, columns;
                                    mergedRegionList = xlsHeper.GetMergedRegion(sheetName, regionName, out regionValues, out rows, out columns);
                                    if (null != regionValues)           //找到了该sheet表下面的该名称管理器
                                    {
                                        Row row = table.Rows.Add();
                                        row.Cells[1].Select();
                                        Table newTable = row.Cells[1].Tables.Add(document.Application.Selection.Range, rows, columns, Type.Missing, Type.Missing);
                                        for (int r = 1; r <= rows; r++)
                                        {
                                            if (newTable.Rows.Count < rows)
                                            {
                                                newTable.Rows.Add();
                                            }
                                            string key = string.Empty;
                                            for (int c = 1; c <= columns; c++)
                                            {
                                                key = (r - 1).ToString() + "," + (c - 1).ToString();
                                                newTable.Cell(r, c).Range.Text = regionValues[key];
                                            }
                                        }
                                        if (null != mergedRegionList)
                                        {
                                            /*在Word的表格中，行列的下标都是从1开始*/
                                            Dictionary<string, int> dic = new Dictionary<string, int>();
                                            var horizontalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Horizontal).OrderBy(q => q.StartRowIndex).ThenBy(q => q.StartCellIndex).ToList();
                                            //先水平
                                            foreach (Assist.MergedRegion mergedRegion in horizontalRegionList)
                                            {
                                                if (mergedRegion.StartRowIndex == 9)
                                                {
                                                    ;
                                                }
                                                int mergeCell = 0;
                                                int rowNumber = mergedRegion.StartRowIndex + 1;
                                                if (!dic.ContainsKey(rowNumber.ToString()))
                                                {
                                                    dic[rowNumber.ToString()] = mergeCell;
                                                }
                                                else
                                                {
                                                    mergeCell = dic[rowNumber.ToString()];
                                                }
                                                Cell cell = newTable.Cell(rowNumber, mergedRegion.StartCellIndex + 1 - mergeCell);
                                                for (int c = mergedRegion.StartCellIndex + 2; c <= mergedRegion.EndCellIndex + 1; c++)
                                                {
                                                    cell.Merge(newTable.Cell(rowNumber, c - mergeCell));            //合并一列，总列数会自减1。
                                                    dic[rowNumber.ToString()] = ++dic[rowNumber.ToString()];
                                                    mergeCell = dic[rowNumber.ToString()];
                                                }
                                            }
                                            dic.Clear();
                                            /*因水平合并一列，当前行的列总数会自减，导致在垂直合并时，需要特殊处理*/
                                            var verticalRegionList = mergedRegionList.Where(q => q.MergeType == Assist.EnumMergeType.Vertical).OrderBy(q => q.StartCellIndex).ThenBy(q => q.StartRowIndex).ToList();
                                            //后垂直
                                            foreach (Assist.MergedRegion mergedRegion in verticalRegionList)
                                            {
                                                int cellNumber = GetCellNumber(horizontalRegionList, mergedRegion.StartRowIndex, mergedRegion.StartCellIndex);   //列索引
                                                int rowNumber = mergedRegion.StartRowIndex + 1;     //行索引

                                                Cell cell = newTable.Cell(rowNumber, cellNumber);
                                                for (int r = mergedRegion.StartRowIndex + 2; r <= mergedRegion.EndRowIndex + 1; r++)
                                                {
                                                    cellNumber = GetCellNumber(horizontalRegionList, r, mergedRegion.StartCellIndex);
                                                    cell.Merge(newTable.Cell(r, cellNumber));                    //合并一行，总行数不会自减
                                                }
                                            }
                                        }
                                        newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;    //设置表格有无边框 
                                        newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;     //设置表格有无边框 
                                        row.Cells[1].Range.Paragraphs.Add();//换行符
                                    }

                                    #endregion
                                }
                            }
                            //将测算表中图片插入到当前测算过程的末尾
                            #region

                            if (imgByteList != null && imgByteList.Count() > 0)
                            {
                                Row row = table.Rows.Add();
                                row.Cells[1].Range.Text = imgRegionName;
                                Row row2 = table.Rows.Add();
                                foreach (byte[] imgB in imgByteList)
                                {
                                    MemoryStream ms = new MemoryStream(imgB);
                                    Image image = System.Drawing.Image.FromStream(ms);
                                    string s = Guid.NewGuid().ToString().Replace("-", "");
                                    s = s.Substring(0, 10 > s.Length ? s.Length : 10);
                                    string imgFileName = tempPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + s + ".jpg";
                                    image.Save(imgFileName);
                                    imgPathList.Add(imgFileName);
                                    row2.Cells[1].Range.InlineShapes.AddPicture(imgFileName, false, true, row2.Cells[1].Range);
                                    row2.Cells[1].Range.Paragraphs.Add();//换行符
                                }
                            }

                            #endregion
                            xlsHeper.Close();
                            #endregion
                        }
                        table.Rows[1].Delete();
                        document.Bookmarks.Add(specifyTableText, tableRange);
                    }
                    else //没找到指定书签时 则要获取excel中图片
                    {
                        #region
                        foreach (string targetExcelFile in openTargetExcelFile)
                        {

                            if (!File.Exists(targetExcelFile))
                            {
                                continue;
                            }

                            xlsHeper = new Xls(targetExcelFile, true);
                            if (xlsHeper.workbook == null)
                            {
                                continue;
                            }
                            List<byte[]> imgByteList = new List<byte[]>();//存储excel中的图片字节信息
                            List<string> regionNameList = xlsHeper.GetAllPrefixRegionName(prefixRegionName, sheetName, out imgByteList);
                            //如果此excel不需要获取图片
                            if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() == null)
                            {
                                imgByteList = new List<byte[]>();
                            }
                            //将测算表中图片插入到当前测算过程的末尾
                            #region

                            if (imgByteList != null && imgByteList.Count() > 0)
                            {
                                foreach (byte[] imgB in imgByteList)
                                {
                                    MemoryStream ms = new MemoryStream(imgB);
                                    Image image = System.Drawing.Image.FromStream(ms);
                                    string s = Guid.NewGuid().ToString().Replace("-", "");
                                    s = s.Substring(0, 10 > s.Length ? s.Length : 10);
                                    string imgFileName = tempPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + s + ".jpg";
                                    image.Save(imgFileName);
                                    imgPathList.Add(imgFileName);
                                }
                            }

                            #endregion
                        }
                        #endregion
                    }
                    //如果有指定将测算图片生成到某个书签的话则进行插入
                    if (imgPathList != null && imgPathList.Count() > 0 && document.Bookmarks.Exists(imgRegionName))
                    {
                        Range imgRange = document.Bookmarks[imgRegionName].Range;
                        foreach (string imgPath in imgPathList)
                        {
                            imgRange.InlineShapes.AddPicture(imgPath, false, true, imgRange);
                            imgRange.Paragraphs.Add();//换行符
                        }

                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != xlsHeper)
                    xlsHeper.Close();
            }
        }


        /// <summary>
        /// 获得水平合并后垂直需要合并的单元格在WORD表格中的实际列索引值
        /// </summary>
        /// <param name="mergedRegionList">水平合并区域的列表</param>
        /// <param name="positionRow">所在行</param>
        /// <param name="positionCell">所在列</param>
        /// <returns></returns>
        private static int GetCellNumber(List<Assist.MergedRegion> horizontalRegionList, int positionRow, int positionCell)
        {
            int cellNumber;
            List<Assist.MergedRegion> regionList = horizontalRegionList.Where(q => q.StartRowIndex == positionRow && q.EndCellIndex < positionCell).ToList();
            int spanCell = 0;
            cellNumber = 0;
            foreach (Assist.MergedRegion mergedRegion in regionList)
            {
                spanCell += mergedRegion.EndCellIndex - mergedRegion.StartCellIndex;
            }
            cellNumber = positionCell + 1 - spanCell;        //word中的表格下标从1开始算起
            return cellNumber;
        }
    }

    /// <summary>
    /// 估价宝OA
    /// byte-侯湘岳
    /// </summary>
    public class GJBOA
    {
        public static void CopySheetContentToWord(List<string> openTargetExcelFile, List<string> insertImageExcelFile, Document document, string specifyTableText, string sheetName, string prefixRegionName, string tempPath, string imgRegionName)
        {
            MixedHelper.ConvertExcelToWordTable(openTargetExcelFile, insertImageExcelFile, document, specifyTableText, sheetName, prefixRegionName, tempPath, imgRegionName);
        }
        //public static void CopySheetContentToWord2(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
        //    MixedHelper.ConvertExcelToWordTable2(openTargetExcelFile, path, specifyTableText, sheetName, prefixRegionName);
        //}
        //public static void CopySheetContentToWord4(List<string> openTargetExcelFile, string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
        //    MixedHelper.ConvertExcelToWordTable4(openTargetExcelFile, path, specifyTableText, sheetName, prefixRegionName);
        //}

        public static void CopySheetContentToWordByBookMark(List<string> openTargetExcelFile, List<string> insertImageExcelFile, Document document, string specifyTableText, string sheetName, string prefixRegionName, string tempPath, string imgRegionName)
        {
            MixedHelper.ConvertExcelToWordTableByBookMark(openTargetExcelFile, insertImageExcelFile, document, specifyTableText, sheetName, prefixRegionName, tempPath, imgRegionName);
        }
        //public static void CopySheetContentToWordByBookMark2(List<string> openTargetExcelFile,string path, string specifyTableText, string sheetName, string prefixRegionName)
        //{
        //    MixedHelper.ConvertExcelToWordTableByBookMark2(openTargetExcelFile,  path, specifyTableText, sheetName, prefixRegionName);
        //}

        #region 使用Spire.doc实现的功能
        /*
        public static void CopySheetContentToWord(List<string> openTargetExcelFile, Doc docHelper, string specifyTableText, string sheetName, string regionName)
        {
            MixedHelper.ConvertExcelToWordTable(openTargetExcelFile, docHelper, specifyTableText, sheetName, regionName);
        }
        /// <summary>
        /// 估价宝中生成个别因素
        /// </summary>
        /// <param name="docHelper"></param>
        /// <param name="specifyTableText"></param>
        /// <param name="objectResult"></param>
        /// <param name="objectSurroundingElements"></param>
        public static void eplaceTableRowContent(Doc docHelper, string specifyTableText, List<Dictionary<string, string>> objectResult, Dictionary<string, string> objectSurroundingElements)
        {
            if (null == docHelper)
                throw new Exception("docHelper is null");
            List<Table> tables = null;
            tables = docHelper.GetSpecifyTablesByFirstCellText(specifyTableText);
            if (null != tables)
            {
                foreach (var table in tables)
                {
                    if (2 <= table.Rows.Count)
                    {
                        TableCell cell = table[1, 0];   //第二行第一列
                        if (0 < cell.Paragraphs.Count)
                        {
                            List<Paragraph> paragraphList = new List<Paragraph>();
                            for (int paragraphIndex = 0; paragraphIndex < cell.Paragraphs.Count; paragraphIndex++)      //遍历段落
                            {
                                paragraphList.Add(cell.Paragraphs[paragraphIndex]);
                            }
                            table.Rows.RemoveAt(1);     //删除模板行
                            for (int index = 0; index < objectResult.Count; index++)
                            {
                                TableRow row = table.AddRow();
                                cell = row.Cells[0];
                                foreach (var paragraph in paragraphList)
                                {
                                    Paragraph newPara = (Paragraph)paragraph.Clone();       //复制段落
                                    cell.Paragraphs.Add(newPara);
                                }
                                for (int paragraphIndex = 0; paragraphIndex < cell.Paragraphs.Count; paragraphIndex++)      //遍历段落
                                {
                                    foreach (KeyValuePair<string, string> kv in objectResult[index])
                                    {
                                        cell.Paragraphs[paragraphIndex].Replace("{" + kv.Key + "}", kv.Value, false, false);
                                    }

                                    foreach (KeyValuePair<string, string> kv in objectSurroundingElements)
                                    {
                                        cell.Paragraphs[paragraphIndex].Replace("{" + kv.Key + "}", kv.Value, false, false);
                                    }
                                    //如下的正则是将没有匹配到的大括号内容替换为空         bug:6592
                                    Regex r = new Regex("\\{[^\\{\\}]*\\}", RegexOptions.IgnoreCase);
                                    MatchCollection mc = r.Matches(cell.Paragraphs[paragraphIndex].Text);
                                    if (mc.Count > 0)
                                    {
                                        for (int i = 0; i < mc.Count; i++)
                                        {
                                            cell.Paragraphs[paragraphIndex].Replace(mc[i].Value, "", false, false);
                                        }
                                    }
                                }
                            }
                        }
                        table.Rows.RemoveAt(0);     //删除作为表头标签的行    
                    }
                }
            }
        }
        */
        #endregion
    }
}

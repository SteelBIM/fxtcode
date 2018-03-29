using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Data;
using System.IO;
using AsposeExcel = Aspose.Cells;
using Aspose.Words.Drawing;
using System.Drawing;
using Aspose.Words.Fields;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;

namespace CAS.Common.Aspose
{
    public class WordHelper
    {
        #region 初始化 word,Builder,shapes,shape
        public Document word;
        public DocumentBuilder builder;
        public NodeCollection shapes;
        public Shape shape;
        public WordHelper()
        {
            word = new Document();
            builder = new DocumentBuilder(word);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="str">外部需要用到的哪些对象
        ///                   Shapes  ----获取word中所有文本框的集合
        /// </param>
        public WordHelper(string path, string[] str = null)
        {
            word = new Document(path);
            builder = new DocumentBuilder(word);
            if (str != null && str.Contains("Shapes"))
                shapes = word.GetChildNodes(NodeType.Shape, true);
        }
        #endregion

        /// <summary>
        /// 生成测算过程
        /// </summary>
        /// <param name="openTargetExcelFile">测算表文件路径集合</param>
        /// <param name="insertImageExcelFile">比较法测算表</param>
        /// <param name="prefixBookMarkName">测算表书签名称</param>
        /// <param name="sheetName">测算表sheet名称</param>
        /// <param name="prefixRegionName">测算表区域命名管理器名称</param>
        /// <param name="imgRegionName">报告中要生成案例图片的书签名</param>
        public void ConvertExcelToWordTable(List<string> openTargetExcelFile, List<string> insertImageExcelFile,string prefixBookMarkName, string sheetName, string prefixRegionName, string imgRegionName)
        {
            foreach (string targetExcelFile in openTargetExcelFile)
            {
                try
                {
                    ExcelHelper eh = new ExcelHelper(targetExcelFile,sheetName);
                    AsposeExcel.Workbook workbook = eh.workbook;
                    if (workbook.Worksheets.Names.Count > 0)
                    {
                        //获取sheet
                        AsposeExcel.Worksheet ws = eh.sheet;
                        #region 获取测算表中的圈定区域
                        foreach (AsposeExcel.Range r in workbook.Worksheets.GetNamedRanges())
                        {
                            //包含测算过程前缀 例如测算过程1、测算过程2
                            if (r.Name.StartsWith(prefixRegionName))
                            {
                                //确定有没有此书签 以及excel中的测算表区域是否存在
                                if (r != null && word.Range.Bookmarks[prefixBookMarkName] != null)
                                {
                                    builder.MoveToBookmark(prefixBookMarkName);
                                    List<string> mergedRangeList = new List<string>();
                                    List<string> mergedCellList = new List<string>();

                                    #region  创建table到word中
                                    Table curTable = builder.StartTable();
                                    for (int i = 0; i < r.RowCount; i++)
                                    {
                                        for (int j = 0; j < r.ColumnCount; j++)
                                        {
                                            //word中的当前行
                                            Cell curCell = builder.InsertCell();
                                            //excel中的单元格
                                            AsposeExcel.Cell cell = r[i, j];
                                            //获取样式
                                            AsposeExcel.Style style = cell.GetStyle();

                                            #region 合并单元格
                                            if (cell.IsMerged)
                                            {
                                                AsposeExcel.Range mergedRange = cell.GetMergedRange();

                                                if (mergedRange.ColumnCount > 1)
                                                {
                                                    //横向合并 开始合并用CellMerge.First 结束用CellMerge.Previous
                                                    if (mergedCellList.Contains(cell.Name))
                                                        curCell.CellFormat.HorizontalMerge = CellMerge.Previous;
                                                    else
                                                        curCell.CellFormat.HorizontalMerge = CellMerge.First;

                                                    //curCell.CellFormat.VerticalMerge = CellMerge.None;
                                                }
                                                if (mergedRange.RowCount > 1)
                                                {
                                                    //垂直合并
                                                    if (mergedCellList.Contains(cell.Name))
                                                        curCell.CellFormat.VerticalMerge = CellMerge.Previous;
                                                    else
                                                        curCell.CellFormat.VerticalMerge = CellMerge.First;

                                                    //curCell.CellFormat.HorizontalMerge = CellMerge.None;
                                                }

                                                //把有合并的单元格都存入集合 避免重复添加
                                                if (!mergedRangeList.Contains(mergedRange.RefersTo))
                                                {
                                                    for (int x = 0; x < mergedRange.RowCount; x++)
                                                    {
                                                        for (int y = 0; y < mergedRange.ColumnCount; y++)
                                                        {
                                                            AsposeExcel.Cell mergedCell = mergedRange[x, y];
                                                            mergedCellList.Add(mergedCell.Name);
                                                        }
                                                    }
                                                    mergedRangeList.Add(mergedRange.RefersTo);
                                                }
                                            }
                                            else
                                            {
                                                curCell.CellFormat.HorizontalMerge = CellMerge.None;
                                                curCell.CellFormat.VerticalMerge = CellMerge.None;
                                            }
                                            #endregion

                                            #region 单元格样式
                                            builder.Font.Color = style.Font.Color;
                                            builder.Font.Size = style.Font.Size;
                                            builder.Font.Bold = style.Font.IsBold;//是否粗体
                                            builder.Font.Italic = style.Font.IsItalic;//是否斜体

                                            //边框样式
                                            curCell.CellFormat.Borders.LineStyle = LineStyle.Single;//ConvertExcelBorderToWord(style.Borders.DiagonalStyle)
                                            curCell.CellFormat.Borders.Color = style.Borders.DiagonalColor;
                                            curCell.CellFormat.Width = r.Worksheet.Cells.Columns[cell.Column].Width + 10;
                                            curCell.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                                            #endregion

                                            builder.Write(cell.StringValue);
                                        }
                                        builder.EndRow();
                                    }
                                    #endregion

                                    #region 添加案例位置图与照片
                                    if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() != null)
                                    {
                                        //获取excel中的图片
                                        List<byte[]> imgData = GetExcelImageData(targetExcelFile, sheetName, eh);
                                        if (imgData.Count > 0)
                                        {
                                            //创建行、列 
                                            for (int i = 0; i < r.ColumnCount; i++)
                                            {
                                                Cell imgCell = builder.InsertCell();
                                                if (i == 0)
                                                    imgCell.CellFormat.HorizontalMerge = CellMerge.First;
                                                else
                                                    imgCell.CellFormat.HorizontalMerge = CellMerge.Previous;
                                                imgCell.CellFormat.VerticalMerge = CellMerge.None;
                                                //row.AppendChild(imgCell);
                                            } 
                                            builder.EndRow();
                                            builder.EndTable();//结束table
                                            Run r1 = new Run(word, "");//创建段落
                                            curTable.LastRow.Cells[0].FirstParagraph.AppendChild(r1);//插入段落标记插入图片的位置
                                            builder.InsertBreak(BreakType.ParagraphBreak);
                                            builder.MoveTo(r1);//移动到固定位置
                                            InsertImgToWord(imgData);//插入图片
                                        }
                                        else
                                        {
                                            builder.EndTable();//结束table
                                            builder.InsertBreak(BreakType.ParagraphBreak);
                                        }
                                    }
                                    else
                                    {
                                        builder.EndTable();//结束table
                                        builder.InsertBreak(BreakType.ParagraphBreak);
                                    }
                                    #endregion 
                                }
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                }
            }
            if (word.Range.Bookmarks[prefixBookMarkName]!=null)
                word.Range.Bookmarks[prefixBookMarkName].Text = "";

            #region 案例位置图与照片
            bool isMarkImg=builder.MoveToBookmark(imgRegionName);
            if (isMarkImg)
            {
                //word.Range.Bookmarks[imgRegionName].Text = "";
                foreach (string imageExcelFile in insertImageExcelFile)
                {
                    ExcelHelper eh = new ExcelHelper(imageExcelFile, sheetName);
                    List<byte[]> imgData=GetExcelImageData(imageExcelFile, sheetName, eh);
                    InsertImgToWord(imgData);
                }
            }
            #endregion 
        }

        #region 自动生成新生成测算过程
        /// <summary>
        /// 生成测算过程
        /// </summary>
        /// <param name="openTargetExcelFile">测算表文件路径集合</param>
        /// <param name="insertImageExcelFile">比较法测算表</param>
        /// <param name="sheetName">测算表sheet名称</param>
        /// <param name="prefixRegionName">指定能够生成出来的书签前缀</param>
        /// <param name="imgRegionName">报告中要生成案例图片的书签名</param>
        public void ConvertExcelToWordTable(List<string> openTargetExcelFile, List<string> insertImageExcelFile, string sheetName, string[] prefixRegionName, string imgRegionName)
        {
            foreach (Table table in word.Sections.Cast<Section>().SelectMany(section => section.Body.Tables.Cast<Table>().Where(table => table.StyleOptions == TableStyleOptions.None && table.TextWrapping == TextWrapping.None)))
            {
                table.Remove();
            }
            foreach (string targetExcelFile in openTargetExcelFile)
            {
                ExcelHelper eh = new ExcelHelper(targetExcelFile, sheetName);
                AsposeExcel.Workbook workbook = eh.workbook;
                if (workbook.Worksheets.Names.Count > 0)
                {
                    //获取sheet
                    AsposeExcel.Worksheet ws = eh.sheet;

                    #region 获取测算表中的圈定区域

                    foreach (Bookmark bookmark in word.Range.Bookmarks)
                    {
                        try
                        {
                            if (prefixRegionName.Contains(bookmark.Name.Split('_')[0]))
                            {
                                AsposeExcel.Range r = workbook.Worksheets.GetRangeByName(bookmark.Name);
                                builder.MoveToBookmark(bookmark.Name);
                                List<string> mergedRangeList = new List<string>();
                                //List<string> mergedCellList = new List<string>();
                                List<int[]> mergedCellIndexList = new List<int[]>();
                                if (r != null)
                                {
                                    #region  创建table到word中

                                    Table curTable = builder.StartTable();
                                    for (int i = 0; i < r.RowCount; i++)
                                    {
                                        for (int j = 0; j < r.ColumnCount; j++)
                                        {
                                            //word中的当前行
                                            Cell curCell = builder.InsertCell();
                                            //excel中的单元格
                                            AsposeExcel.Cell cell = r[i, j];
                                            //获取样式
                                            AsposeExcel.Style style = cell.GetStyle();

                                            #region 合并单元格

                                            if (cell.IsMerged)
                                            {
                                                AsposeExcel.Range mergedRange = cell.GetMergedRange();

                                                //if (mergedRange.ColumnCount > 1)
                                                //{
                                                //    //横向合并 开始合并用CellMerge.First 结束用CellMerge.Previous
                                                //    if (mergedCellList.Contains(cell.Name))
                                                //        curCell.CellFormat.HorizontalMerge = CellMerge.Previous;
                                                //    else
                                                //        curCell.CellFormat.HorizontalMerge = CellMerge.First;

                                                //    //curCell.CellFormat.VerticalMerge = CellMerge.None;
                                                //}
                                                //if (mergedRange.RowCount > 1)
                                                //{
                                                //    //垂直合并
                                                //    if (mergedCellList.Contains(cell.Name))
                                                //        curCell.CellFormat.VerticalMerge = CellMerge.Previous;
                                                //    else
                                                //        curCell.CellFormat.VerticalMerge = CellMerge.First;

                                                //    //curCell.CellFormat.HorizontalMerge = CellMerge.None;
                                                //}

                                                //把有合并的单元格都存入集合 避免重复添加
                                                if (!mergedRangeList.Contains(mergedRange.RefersTo))
                                                {
                                                    //for (int x = 0; x < mergedRange.RowCount; x++)
                                                    //{
                                                    //    for (int y = 0; y < mergedRange.ColumnCount; y++)
                                                    //    {
                                                    //        AsposeExcel.Cell mergedCell = mergedRange[x, y];
                                                    //        mergedCellList.Add(mergedCell.Name);
                                                    //    }
                                                    //}
                                                    mergedRangeList.Add(mergedRange.RefersTo);
                                                    mergedCellIndexList.Add(
                                                        new[]
                                                        {
                                                            i,
                                                            j,
                                                            mergedRange.ColumnCount,
                                                            mergedRange.RowCount
                                                        });
                                                }
                                            }
                                            else
                                            {
                                                curCell.CellFormat.HorizontalMerge = CellMerge.None;
                                                curCell.CellFormat.VerticalMerge = CellMerge.None;
                                            }

                                            #endregion

                                            #region 单元格样式

                                            builder.Font.Color = style.Font.Color;
                                            builder.Font.Size = style.Font.Size;
                                            builder.Font.Bold = style.Font.IsBold; //是否粗体
                                            builder.Font.Italic = style.Font.IsItalic; //是否斜体

                                            //curCell.CellFormat.Borders.LineStyle =LineStyle.Single;//ConvertExcelBorderToWord(style.Borders.DiagonalStyle)
                                            curCell.CellFormat.Borders.Color = style.Borders.DiagonalColor;
                                            curCell.CellFormat.Width = r.Worksheet.Cells.Columns[cell.Column].Width;
                                            curCell.CellFormat.VerticalAlignment =
                                                ConvertExcelVerticalAlignmentToWord(style.VerticalAlignment); //垂直居中对齐
                                            builder.ParagraphFormat.Alignment =
                                                ConvertExcelHorizontalAlignmentToWord(style.HorizontalAlignment);
                                            //水平居中对齐
                                            builder.ParagraphFormat.SpaceBefore = 0; //段前
                                            //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;//固定值12磅
                                            //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;//1.5倍行距
                                            //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;//最小值18磅
                                            builder.ParagraphFormat.LineSpacing = 12; //单倍行距
                                            builder.ParagraphFormat.FirstLineIndent = 0; //首行缩进

                                            #region 边框样式

                                            curCell.CellFormat.Borders[BorderType.DiagonalDown].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.DiagonalDown].LineStyle);
                                            curCell.CellFormat.Borders[BorderType.DiagonalUp].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.DiagonalUp].LineStyle);
                                            curCell.CellFormat.Borders[BorderType.Left].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.LeftBorder].LineStyle);
                                            curCell.CellFormat.Borders[BorderType.Right].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.RightBorder].LineStyle);
                                            curCell.CellFormat.Borders[BorderType.Top].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.TopBorder].LineStyle);
                                            curCell.CellFormat.Borders[BorderType.Bottom].LineStyle =
                                                ConvertExcelBorderToWord(
                                                    style.Borders[AsposeExcel.BorderType.BottomBorder].LineStyle);

                                            #endregion


                                            //Builder.CellFormat.Shading.BackgroundPatternColor = style.ForegroundColor;//单元格颜色

                                            #endregion

                                            builder.Write(cell.StringValue);
                                        }
                                        builder.EndRow();
                                    }
                                    MergedCell(curTable, mergedCellIndexList);
                                    curTable.AutoFit(AutoFitBehavior.AutoFitToWindow); //根据窗口调整表格
                                    #endregion

                                    #region 添加案例位置图与照片 暂时屏蔽掉

                                    //if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() != null)
                                    //{
                                    //    //获取excel中的图片
                                    //    List<byte[]> imgData = GetExcelImageData(targetExcelFile, sheetName, eh);
                                    //    if (imgData.Count > 0)
                                    //    {
                                    //        //创建行、列 
                                    //        for (int i = 0; i < r.ColumnCount; i++)
                                    //        {
                                    //            Cell imgCell = Builder.InsertCell();
                                    //            if (i == 0)
                                    //                imgCell.CellFormat.HorizontalMerge = CellMerge.First;
                                    //            else
                                    //                imgCell.CellFormat.HorizontalMerge = CellMerge.Previous;
                                    //            imgCell.CellFormat.VerticalMerge = CellMerge.None;
                                    //            //row.AppendChild(imgCell);
                                    //        }
                                    //        Builder.EndTable();//结束table
                                    //        Run r1 = new Run(word, "");//创建段落
                                    //        curTable.LastRow.Cells[0].FirstParagraph.AppendChild(r1);//插入段落标记插入图片的位置
                                    //        Builder.InsertBreak(BreakType.ParagraphBreak);
                                    //        Builder.MoveTo(r1);//移动到固定位置
                                    //        InsertImgToWord(imgData);//插入图片
                                    //    }
                                    //    else
                                    //    {
                                    //        Builder.EndTable();//结束table
                                    //        Builder.InsertBreak(BreakType.ParagraphBreak);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //builder.InsertBreak(BreakType.ParagraphBreak);
                                    //builder.Write(curTable);
                                    //builder.EndBookmark(bookmark.Name);
                                    //}

                                    #endregion
                                    //builder.InsertBreak(BreakType.ParagraphBreak);
                                    builder.EndTable(); //结束table

                                    #region 自动生成是根据这两个属性来识别是否为系统生成的表格  当用户的报告生成后出现表格丢失的情况  先检查下客户报告中表格的这两个属性是不是和一下两个一样

                                    curTable.StyleOptions = TableStyleOptions.None;
                                    curTable.TextWrapping = TextWrapping.None;

                                    #endregion

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex + "书签名称：" + bookmark.Name);
                        }
                    }

                    #endregion
                }
            }
            //if (word.Range.Bookmarks[prefixBookMarkName] != null)
            //    word.Range.Bookmarks[prefixBookMarkName].Text = "";

            #region 案例位置图与照片

            #region 新方法生成案例照片
            GetExcelImageData(insertImageExcelFile, "可比实例照片");
            GetExcelImageData(insertImageExcelFile, "可比实例1照片");
            GetExcelImageData(insertImageExcelFile, "可比实例2照片");
            GetExcelImageData(insertImageExcelFile, "可比实例3照片");
            #endregion

            #region 老方法生成案例位置图与照片
            //bool isMarkImg = Builder.MoveToBookmark(imgRegionName);
            //if (isMarkImg)
            //{
            //    //word.Range.Bookmarks[imgRegionName].Text = "";
            //    foreach (string imageExcelFile in insertImageExcelFile)
            //    {
            //        ExcelHelper eh = new ExcelHelper(imageExcelFile, sheetName);
            //        List<byte[]> imgData = GetExcelImageData(imageExcelFile, sheetName, eh);
            //        InsertImgToWord(imgData);
            //    }
            //}
            #endregion
            #endregion
        }
        #endregion

        //#region 新生成测算过程
        ///// <summary>
        ///// 生成测算过程
        ///// </summary>
        ///// <param name="openTargetExcelFile">测算表文件路径集合</param>
        ///// <param name="insertImageExcelFile">比较法测算表</param>
        ///// <param name="sheetName">测算表sheet名称</param>
        ///// <param name="prefixRegionName">指定能够生成出来的书签前缀</param>
        ///// <param name="imgRegionName">报告中要生成案例图片的书签名</param>
        //public void ConvertExcelToWordTable(List<string> openTargetExcelFile, List<string> insertImageExcelFile, string sheetName, string[] prefixRegionName, string imgRegionName)
        //{
        //    foreach (string targetExcelFile in openTargetExcelFile)
        //    {
        //        ExcelHelper eh = new ExcelHelper(targetExcelFile, sheetName);
        //        AsposeExcel.Workbook workbook = eh.workbook;
        //        if (workbook.Worksheets.Names.Count > 0)
        //        {
        //            //获取sheet
        //            AsposeExcel.Worksheet ws = eh.sheet;

        //            #region 获取测算表中的圈定区域

        //            foreach (Bookmark bookmark in word.Range.Bookmarks)
        //            {
        //                try
        //                {
        //                    if (prefixRegionName.Contains(bookmark.Name.Split('_')[0]))
        //                    {
        //                        AsposeExcel.Range r = workbook.Worksheets.GetRangeByName(bookmark.Name);
        //                        builder.MoveToBookmark(bookmark.Name);
        //                        List<string> mergedRangeList = new List<string>();
        //                        //List<string> mergedCellList = new List<string>();
        //                        List<int[]> mergedCellIndexList = new List<int[]>();
        //                        if (r != null)
        //                        {
        //                            #region  创建table到word中

        //                            Table curTable = builder.StartTable();
        //                            for (int i = 0; i < r.RowCount; i++)
        //                            {
        //                                for (int j = 0; j < r.ColumnCount; j++)
        //                                {
        //                                    //word中的当前行
        //                                    Cell curCell = builder.InsertCell();
        //                                    //excel中的单元格
        //                                    AsposeExcel.Cell cell = r[i, j];
        //                                    //获取样式
        //                                    AsposeExcel.Style style = cell.GetStyle();

        //                                    #region 合并单元格

        //                                    if (cell.IsMerged)
        //                                    {
        //                                        AsposeExcel.Range mergedRange = cell.GetMergedRange();

        //                                        //if (mergedRange.ColumnCount > 1)
        //                                        //{
        //                                        //    //横向合并 开始合并用CellMerge.First 结束用CellMerge.Previous
        //                                        //    if (mergedCellList.Contains(cell.Name))
        //                                        //        curCell.CellFormat.HorizontalMerge = CellMerge.Previous;
        //                                        //    else
        //                                        //        curCell.CellFormat.HorizontalMerge = CellMerge.First;

        //                                        //    //curCell.CellFormat.VerticalMerge = CellMerge.None;
        //                                        //}
        //                                        //if (mergedRange.RowCount > 1)
        //                                        //{
        //                                        //    //垂直合并
        //                                        //    if (mergedCellList.Contains(cell.Name))
        //                                        //        curCell.CellFormat.VerticalMerge = CellMerge.Previous;
        //                                        //    else
        //                                        //        curCell.CellFormat.VerticalMerge = CellMerge.First;

        //                                        //    //curCell.CellFormat.HorizontalMerge = CellMerge.None;
        //                                        //}

        //                                        //把有合并的单元格都存入集合 避免重复添加
        //                                        if (!mergedRangeList.Contains(mergedRange.RefersTo))
        //                                        {
        //                                            //for (int x = 0; x < mergedRange.RowCount; x++)
        //                                            //{
        //                                            //    for (int y = 0; y < mergedRange.ColumnCount; y++)
        //                                            //    {
        //                                            //        AsposeExcel.Cell mergedCell = mergedRange[x, y];
        //                                            //        mergedCellList.Add(mergedCell.Name);
        //                                            //    }
        //                                            //}
        //                                            mergedRangeList.Add(mergedRange.RefersTo);
        //                                            mergedCellIndexList.Add(
        //                                                new[]
        //                                                {
        //                                                    i,
        //                                                    j,
        //                                                    mergedRange.ColumnCount,
        //                                                    mergedRange.RowCount
        //                                                });
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        curCell.CellFormat.HorizontalMerge = CellMerge.None;
        //                                        curCell.CellFormat.VerticalMerge = CellMerge.None;
        //                                    }

        //                                    #endregion

        //                                    #region 单元格样式

        //                                    builder.Font.Color = style.Font.Color;
        //                                    builder.Font.Size = style.Font.Size;
        //                                    builder.Font.Bold = style.Font.IsBold; //是否粗体
        //                                    builder.Font.Italic = style.Font.IsItalic; //是否斜体

        //                                    //curCell.CellFormat.Borders.LineStyle =LineStyle.Single;//ConvertExcelBorderToWord(style.Borders.DiagonalStyle)
        //                                    curCell.CellFormat.Borders.Color = style.Borders.DiagonalColor;
        //                                    curCell.CellFormat.Width = r.Worksheet.Cells.Columns[cell.Column].Width;
        //                                    curCell.CellFormat.VerticalAlignment =
        //                                        ConvertExcelVerticalAlignmentToWord(style.VerticalAlignment); //垂直居中对齐
        //                                    builder.ParagraphFormat.Alignment =
        //                                        ConvertExcelHorizontalAlignmentToWord(style.HorizontalAlignment);
        //                                    //水平居中对齐
        //                                    builder.ParagraphFormat.SpaceBefore = 0; //段前
        //                                    //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;//固定值12磅
        //                                    //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;//1.5倍行距
        //                                    //Builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;//最小值18磅
        //                                    builder.ParagraphFormat.LineSpacing = 12; //单倍行距
        //                                    builder.ParagraphFormat.FirstLineIndent = 0; //首行缩进
        //                                    //边框样式
        //                                    curCell.CellFormat.Borders[BorderType.DiagonalDown].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.DiagonalDown].LineStyle);
        //                                    curCell.CellFormat.Borders[BorderType.DiagonalUp].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.DiagonalUp].LineStyle);
        //                                    curCell.CellFormat.Borders[BorderType.Left].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.LeftBorder].LineStyle);
        //                                    curCell.CellFormat.Borders[BorderType.Right].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.RightBorder].LineStyle);
        //                                    curCell.CellFormat.Borders[BorderType.Top].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.TopBorder].LineStyle);
        //                                    curCell.CellFormat.Borders[BorderType.Bottom].LineStyle =
        //                                        ConvertExcelBorderToWord(
        //                                            style.Borders[AsposeExcel.BorderType.BottomBorder].LineStyle);
        //                                    //Builder.CellFormat.Shading.BackgroundPatternColor = style.ForegroundColor;//单元格颜色

        //                                    #endregion

        //                                    builder.Write(cell.StringValue);
        //                                }
        //                                builder.EndRow();
        //                            }
        //                            MergedCell(curTable, mergedCellIndexList);
        //                            curTable.AutoFit(AutoFitBehavior.AutoFitToWindow); //根据窗口调整表格
        //                            #endregion

        //                            #region 添加案例位置图与照片 暂时屏蔽掉

        //                            //if (insertImageExcelFile.Where(_obj => _obj == targetExcelFile).FirstOrDefault() != null)
        //                            //{
        //                            //    //获取excel中的图片
        //                            //    List<byte[]> imgData = GetExcelImageData(targetExcelFile, sheetName, eh);
        //                            //    if (imgData.Count > 0)
        //                            //    {
        //                            //        //创建行、列 
        //                            //        for (int i = 0; i < r.ColumnCount; i++)
        //                            //        {
        //                            //            Cell imgCell = Builder.InsertCell();
        //                            //            if (i == 0)
        //                            //                imgCell.CellFormat.HorizontalMerge = CellMerge.First;
        //                            //            else
        //                            //                imgCell.CellFormat.HorizontalMerge = CellMerge.Previous;
        //                            //            imgCell.CellFormat.VerticalMerge = CellMerge.None;
        //                            //            //row.AppendChild(imgCell);
        //                            //        }
        //                            //        Builder.EndTable();//结束table
        //                            //        Run r1 = new Run(word, "");//创建段落
        //                            //        curTable.LastRow.Cells[0].FirstParagraph.AppendChild(r1);//插入段落标记插入图片的位置
        //                            //        Builder.InsertBreak(BreakType.ParagraphBreak);
        //                            //        Builder.MoveTo(r1);//移动到固定位置
        //                            //        InsertImgToWord(imgData);//插入图片
        //                            //    }
        //                            //    else
        //                            //    {
        //                            //        Builder.EndTable();//结束table
        //                            //        Builder.InsertBreak(BreakType.ParagraphBreak);
        //                            //    }
        //                            //}
        //                            //else
        //                            //{
        //                            builder.EndTable(); //结束table
        //                            builder.InsertBreak(BreakType.ParagraphBreak);
        //                            //}

        //                            #endregion
        //                        }
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new Exception(ex + "书签名称：" + bookmark.Name);
        //                }
        //            }

        //            #endregion
        //        }
        //    }
        //    //if (word.Range.Bookmarks[prefixBookMarkName] != null)
        //    //    word.Range.Bookmarks[prefixBookMarkName].Text = "";

        //    #region 案例位置图与照片

        //    #region 新方法生成案例照片
        //    GetExcelImageData(insertImageExcelFile, "可比实例照片");
        //    GetExcelImageData(insertImageExcelFile, "可比实例1照片");
        //    GetExcelImageData(insertImageExcelFile, "可比实例2照片");
        //    GetExcelImageData(insertImageExcelFile, "可比实例3照片");
        //    #endregion

        //    #region 老方法生成案例位置图与照片
        //    //bool isMarkImg = Builder.MoveToBookmark(imgRegionName);
        //    //if (isMarkImg)
        //    //{
        //    //    //word.Range.Bookmarks[imgRegionName].Text = "";
        //    //    foreach (string imageExcelFile in insertImageExcelFile)
        //    //    {
        //    //        ExcelHelper eh = new ExcelHelper(imageExcelFile, sheetName);
        //    //        List<byte[]> imgData = GetExcelImageData(imageExcelFile, sheetName, eh);
        //    //        InsertImgToWord(imgData);
        //    //    }
        //    //}
        //    #endregion
        //    #endregion
        //}
        //#endregion

        #region 把测算表中的图片插入到报告中

        /// <summary>
        /// 把测算表中的图片插入到报告中
        /// </summary>
        /// <param name="insertImageExcelFile">图片集合</param>
        /// <param name="sheetName"></param>
        private void GetExcelImageData(List<string> insertImageExcelFile, string sheetName)
        {
            List<byte[]> imgList = new List<byte[]>();
            if (builder.MoveToBookmark(sheetName))
            {
                Bookmark bookmark = word.Range.Bookmarks[sheetName];
                bookmark.Text = "";
                bookmark.Remove();
                builder.StartBookmark(sheetName);
                if (sheetName == "可比实例照片")
                {
                    //特殊处理当为可比实例照片生成其他三种所有的照片
                    foreach (string imageExcelFile in insertImageExcelFile)
                    {
                        ExcelHelper eh = new ExcelHelper(imageExcelFile);
                        imgList.AddRange(eh.GetPictureData("可比实例1照片"));
                        imgList.AddRange(eh.GetPictureData("可比实例2照片"));
                        imgList.AddRange(eh.GetPictureData("可比实例3照片"));
                        InsertImgToWord(imgList);
                        imgList.Clear();
                    }
                }
                else 
                {
                    foreach (string imageExcelFile in insertImageExcelFile)
                    {
                        ExcelHelper eh = new ExcelHelper(imageExcelFile);
                        imgList.AddRange(eh.GetPictureData(sheetName));
                        InsertImgToWord(imgList);
                        imgList.Clear();
                    }
                }
                builder.EndBookmark(sheetName);
            }
        }
        #endregion

        /// <summary>
        /// 把测算表中的图片插入到报告中
        /// </summary>
        /// <param name="imageExcelFile">图片所在excel</param>
        /// <param name="sheetName"></param>
        /// <param name="eh"></param>
        private List<byte[]> GetExcelImageData(string imageExcelFile, string sheetName, ExcelHelper eh)
        {
            #region 新方法生成案例照片
            List<byte[]> imgList = eh.GetPictureData(sheetName);
            #endregion
            #region 老方法生成案例位置图与照片
            //if (imageExcelFile.EndsWith(".xls"))
            //{
            //    Xls xls = new Xls(imageExcelFile, true);
            //    imgList = xls.GetPictureData(sheetName);
            //    xls.Close();
            //}
            //else
            //{
            //    imgList = eh.GetPictureData();
            //}
            #endregion
            return imgList;
        }

        #region 将图片插入word
        private void InsertImgToWord(List<byte[]> imgList)
        {
            //写入word中
            foreach (byte[] item in imgList)
            {
                builder.InsertImage(AsposeHelper.ScalingImage(item, 580, 0));
            }
        }
        #endregion

        #region  转换excel边框到word中
        /// <summary>
        /// 转换excel边框到word中
        /// </summary>
        /// <param name="borderType">excel中边框样式</param>
        /// <returns></returns>
        private LineStyle ConvertExcelBorderToWord(AsposeExcel.CellBorderType borderType)
        {
            switch (borderType)
            {
                case AsposeExcel.CellBorderType.None:
                    return LineStyle.None;
                case AsposeExcel.CellBorderType.Thin:
                    return LineStyle.Single;
                case AsposeExcel.CellBorderType.Thick:
                    return LineStyle.Thick;
                case AsposeExcel.CellBorderType.Double:
                    return LineStyle.Double;
                case AsposeExcel.CellBorderType.Hair:
                    return LineStyle.Hairline;
                case AsposeExcel.CellBorderType.Dotted:
                    return LineStyle.Dot;
                case AsposeExcel.CellBorderType.DashDot:
                    return LineStyle.DotDash;
                case AsposeExcel.CellBorderType.DashDotDot:
                    return LineStyle.DotDotDash;
                case AsposeExcel.CellBorderType.Dashed:
                    return LineStyle.DashLargeGap;
                default:
                    return LineStyle.None;
            }
        }
        #endregion
        #region  转换excel垂直居中到word中

        /// <summary>
        /// 转换excel垂直居中到word中
        /// </summary>
        /// <param name="alignmenttype">Excel中的垂直居中</param>
        /// <returns></returns>
        private CellVerticalAlignment ConvertExcelVerticalAlignmentToWord(AsposeExcel.TextAlignmentType alignmenttype)
        {
            switch (alignmenttype)
            {
                case AsposeExcel.TextAlignmentType.Center:
                    return CellVerticalAlignment.Center;
                case AsposeExcel.TextAlignmentType.Top:
                    return CellVerticalAlignment.Top;
                case AsposeExcel.TextAlignmentType.Bottom:
                    return CellVerticalAlignment.Bottom;
                default:
                    return CellVerticalAlignment.Center;
            }
        }
        #endregion
        #region  转换excel水平居中到word中

        /// <summary>
        /// 转换excel水平居中到word中
        /// </summary>
        /// <param name="alignmenttype">Excel中的水平居中</param>
        /// <returns></returns>
        private ParagraphAlignment ConvertExcelHorizontalAlignmentToWord(AsposeExcel.TextAlignmentType alignmenttype)
        {
            switch (alignmenttype)
            {
                case AsposeExcel.TextAlignmentType.Center:
                    return ParagraphAlignment.Center;
                case AsposeExcel.TextAlignmentType.Left:
                    return ParagraphAlignment.Left;
                case AsposeExcel.TextAlignmentType.Right:
                    return ParagraphAlignment.Right;
                default:
                    return ParagraphAlignment.Center;
            }
        }
        #endregion

        #region 自动生成个别因素

        /// <summary>
        /// 生成个别因素
        /// </summary>
        /// <param name="word"></param>
        /// <param name="builder"></param>
        /// <param name="objectResult">多个委估对象的数据</param>
        /// <param name="objectSurroundingElements">委估对象的周围数据</param>
        /// <param name="surveyMapDic">物业位置图</param>
        /// <param name="isCreatBookMake">是否自动生成书签</param>
        public void GenerateSingleFactors(List<Dictionary<string, string>> objectResult, Dictionary<string, string> objectSurroundingElements, Dictionary<string, string> surveyMapDic, bool isCreatBookMake=false)
        {
            foreach (Section section in word.Sections)
            {
                foreach (Table table in section.Body.Tables)
                {
                    var tagText = table.FirstRow.FirstCell.Range.Text.Replace("\a", "");
                    //两行一列的表格且为加行表格
                    //个别因素
                    if (2 == table.Rows.Count && 1 == table.FirstRow.Cells.Count)
                    {
                        if (tagText == "加行")
                        {
                            for (int index = 0; index < objectResult.Count; index++)
                            {
                                #region 创建信行
                                //创建新行
                                Row row = new Row(word);
                                row.RowFormat.Borders.LineStyle = table.Rows[1].RowFormat.Borders.LineStyle;//复制行的样式
                                table.Rows.Add(row);
                                Cell cell = new Cell(word);
                                //物业位置图节点
                                List<Node> nodeList = new List<Node>();
                                //物业卫星图节点
                                List<Node> wxnodeList = new List<Node>();
                                //可比实例位置图节点
                                List<Node> photonodeList = new List<Node>();
                                //复制模板内容到新行
                                foreach (Paragraph paragraph in table.Rows[1].Cells[0].Paragraphs)
                                {
                                    //创建新的段落
                                    Paragraph curP = new Paragraph(word);
                                    foreach (Run r in paragraph.Runs)
                                    {
                                        //创建Paragraph子类
                                        Run run = new Run(word, r.Text);
                                        curP.AppendChild(run);
                                        //如果有配置物业位置图
                                        if (paragraph.Range.Text.IndexOf("物业位置图") >= 0)
                                        {
                                            if (nodeList.Count < 1) //一个委估对象只有一个物业位置图
                                            {
                                                nodeList.Add(run);
                                            }
                                        }
                                        //如果有配置物业位置图
                                        if (paragraph.Range.Text.IndexOf("物业卫星图") >= 0)
                                        {
                                            if (wxnodeList.Count < 1)//一个委估对象只有一个物业卫星图
                                            {
                                                wxnodeList.Add(run);
                                            }
                                        }
                                        //如果有配置可比实例位置图
                                        if (paragraph.Range.Text.IndexOf("可比实例位置图") >= 0)
                                        {
                                            if (photonodeList.Count < 1)//一个委估对象只有一个可比实例位置图
                                            {
                                                photonodeList.Add(run);
                                            }
                                        }
                                    }
                                    //设置样式
                                    curP.ParagraphFormat.Style = paragraph.ParagraphFormat.Style;
                                    //添加段落到cell
                                    cell.Paragraphs.Add(curP);
                                }
                                //添加单元格
                                row.Cells.Add(cell);
                                #endregion

                                #region 物业位置图和卫星图和可比实例位置图的插入
                                // 20160225添加卫星图的插入  潘锦发
                                if (surveyMapDic.ContainsKey("物业位置图_" + index) && nodeList.Count > 0)
                                {
                                    string showMapPath = surveyMapDic["物业位置图_" + index];//用来存储生成的物业位置图的虚拟路径
                                    //输出物业位置图照片
                                    foreach (Node node in nodeList)
                                    {
                                        builder.MoveTo(node);//定位到{物业位置图} 的"{" 那里
                                        Node n = new Run(word, objectResult[index]["委估对象全称"] + " 物业位置图");  //创建一个节点 输出委估对象名称
                                        builder.InsertNode(n); //将节点插入
                                        builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                        //Builder.MoveTo(node); //移动到 物业位置图 的位置上
                                        // Builder.InsertImage(showMapPath, 500, 300); //替换图片
                                        builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
                                        row.Range.Replace("{物业位置图}", "", false, false);
                                    }
                                }
                                if (surveyMapDic.ContainsKey("物业卫星图_" + index) && wxnodeList.Count > 0)
                                {
                                    string showMapPath = surveyMapDic["物业卫星图_" + index];//用来存储生成的物业卫星图的虚拟路径
                                    //输出物业卫星图照片
                                    foreach (Node node in wxnodeList)
                                    {
                                        builder.MoveTo(node);//定位到{物业卫星图} 的"{" 那里
                                        Node n = new Run(word, objectResult[index]["委估对象全称"] + " 物业卫星图");  //创建一个节点 输出委估对象名称
                                        builder.InsertNode(n); //将节点插入
                                        builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                        //Builder.MoveTo(node); //移动到 物业卫星图 的位置上
                                        //Builder.InsertImage(showMapPath, 500, 300); //替换图片
                                        builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
                                        row.Range.Replace("{物业卫星图}", "", false, false);
                                    }
                                }
                                if (surveyMapDic.ContainsKey("可比实例位置图_" + index) && photonodeList.Count > 0)
                                {
                                    string showMapPath = surveyMapDic["可比实例位置图_" + index];//用来存储生成的可比实例位置图的虚拟路径
                                    //输出可比实例位置图照片
                                    foreach (Node node in photonodeList)
                                    {
                                        builder.MoveTo(node);//定位到{可比实例位置图} 的"{" 那里
                                        Node n = new Run(word, objectResult[index]["委估对象全称"] + " 可比实例位置图");  //创建一个节点 输出委估对象名称
                                        builder.InsertNode(n); //将节点插入
                                        builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                        //Builder.MoveTo(node); //移动到 可比实例位置图 的位置上
                                        //Builder.InsertImage(showMapPath, 500, 300); //替换图片
                                        builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
                                        row.Range.Replace("{可比实例位置图}", "", false, false);
                                    }
                                }
                                #endregion

                                #region 替换文本
                                ReplaceWordData(objectResult[index], row, true, isCreatBookMake, "_SingleFactors" + (index + 1) + "_");
                                ReplaceWordData(objectSurroundingElements, row, true, isCreatBookMake);
                                //foreach (KeyValuePair<string, string> kv in objectResult[index])
                                //{
                                //    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
                                //}
                                //foreach (KeyValuePair<string, string> kv in objectSurroundingElements)
                                //{
                                //    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
                                //}
                                #endregion

                            }
                        }
                        table.Rows[0].Remove();//删除标记行
                        table.Rows[0].Remove();//删除模板行 
                    }
                }
            }
        }
        #endregion

        //#region 生成个别因素
        ///// <summary>
        ///// 生成个别因素
        ///// </summary>
        ///// <param name="word"></param>
        ///// <param name="builder"></param>
        ///// <param name="objectResult">多个委估对象的数据</param>
        ///// <param name="objectSurroundingElements">委估对象的周围数据</param>
        ///// <param name="surveyMapDic">物业位置图</param>
        //public void GenerateSingleFactors(List<Dictionary<string, string>> objectResult, Dictionary<string, string> objectSurroundingElements, Dictionary<string, string> surveyMapDic)
        //{
        //    foreach (Section section in word.Sections)
        //    {
        //        foreach (Table table in section.Body.Tables)
        //        {
        //            var tagText = table.FirstRow.FirstCell.Range.Text.Replace("\a", "");
        //            //两行一列的表格且为加行表格
        //            //个别因素
        //            if (2 == table.Rows.Count && 1 == table.FirstRow.Cells.Count)
        //            {
        //                if (tagText == "加行")
        //                {
        //                    for (int index = 0; index < objectResult.Count; index++)
        //                    {
        //                        #region 创建信行
        //                        //创建新行
        //                        Row row = new Row(word);
        //                        row.RowFormat.Borders.LineStyle = table.Rows[1].RowFormat.Borders.LineStyle;//复制行的样式
        //                        table.Rows.Add(row);
        //                        Cell cell = new Cell(word);
        //                        //物业位置图节点
        //                        List<Node> nodeList = new List<Node>();
        //                        //物业卫星图节点
        //                        List<Node> wxnodeList = new List<Node>();
        //                        //可比实例位置图节点
        //                        List<Node> photonodeList = new List<Node>();
        //                        //复制模板内容到新行
        //                        foreach (Paragraph paragraph in table.Rows[1].Cells[0].Paragraphs)
        //                        {
        //                            //创建新的段落
        //                            Paragraph curP = new Paragraph(word);
        //                            foreach (Run r in paragraph.Runs)
        //                            {
        //                                //创建Paragraph子类
        //                                Run run = new Run(word, r.Text);
        //                                curP.AppendChild(run);
        //                                //如果有配置物业位置图
        //                                if (paragraph.Range.Text.IndexOf("物业位置图") >= 0)
        //                                {
        //                                    if (nodeList.Count < 1) //一个委估对象只有一个物业位置图
        //                                    {
        //                                        nodeList.Add(run);
        //                                    }
        //                                }
        //                                //如果有配置物业位置图
        //                                if (paragraph.Range.Text.IndexOf("物业卫星图") >= 0)
        //                                {
        //                                    if (wxnodeList.Count < 1)//一个委估对象只有一个物业卫星图
        //                                    {
        //                                        wxnodeList.Add(run);
        //                                    }
        //                                }
        //                                //如果有配置可比实例位置图
        //                                if (paragraph.Range.Text.IndexOf("可比实例位置图") >= 0)
        //                                {
        //                                    if (photonodeList.Count < 1)//一个委估对象只有一个可比实例位置图
        //                                    {
        //                                        photonodeList.Add(run);
        //                                    }
        //                                }
        //                            }
        //                            //设置样式
        //                            curP.ParagraphFormat.Style = paragraph.ParagraphFormat.Style;
        //                            //添加段落到cell
        //                            cell.Paragraphs.Add(curP);
        //                        }
        //                        //添加单元格
        //                        row.Cells.Add(cell);
        //                        #endregion

        //                        #region 物业位置图和卫星图和可比实例位置图的插入
        //                        // 20160225添加卫星图的插入  潘锦发
        //                        if (surveyMapDic.ContainsKey("物业位置图_" + index) && nodeList.Count > 0)
        //                        {
        //                            string showMapPath = surveyMapDic["物业位置图_" + index];//用来存储生成的物业位置图的虚拟路径
        //                            //输出物业位置图照片
        //                            foreach (Node node in nodeList)
        //                            {
        //                                builder.MoveTo(node);//定位到{物业位置图} 的"{" 那里
        //                                Node n = new Run(word, objectResult[index]["委估对象全称"] + " 物业位置图");  //创建一个节点 输出委估对象名称
        //                                builder.InsertNode(n); //将节点插入
        //                                builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
        //                                //Builder.MoveTo(node); //移动到 物业位置图 的位置上
        //                                // Builder.InsertImage(showMapPath, 500, 300); //替换图片
        //                                builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
        //                                row.Range.Replace("{物业位置图}", "", false, false);
        //                            }
        //                        }
        //                        if (surveyMapDic.ContainsKey("物业卫星图_" + index) && wxnodeList.Count > 0)
        //                        {
        //                            string showMapPath = surveyMapDic["物业卫星图_" + index];//用来存储生成的物业卫星图的虚拟路径
        //                            //输出物业卫星图照片
        //                            foreach (Node node in wxnodeList)
        //                            {
        //                                builder.MoveTo(node);//定位到{物业卫星图} 的"{" 那里
        //                                Node n = new Run(word, objectResult[index]["委估对象全称"] + " 物业卫星图");  //创建一个节点 输出委估对象名称
        //                                builder.InsertNode(n); //将节点插入
        //                                builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
        //                                //Builder.MoveTo(node); //移动到 物业卫星图 的位置上
        //                                //Builder.InsertImage(showMapPath, 500, 300); //替换图片
        //                                builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
        //                                row.Range.Replace("{物业卫星图}", "", false, false);
        //                            }
        //                        }
        //                        if (surveyMapDic.ContainsKey("可比实例位置图_" + index) && photonodeList.Count > 0)
        //                        {
        //                            string showMapPath = surveyMapDic["可比实例位置图_" + index];//用来存储生成的可比实例位置图的虚拟路径
        //                            //输出可比实例位置图照片
        //                            foreach (Node node in photonodeList)
        //                            {
        //                                builder.MoveTo(node);//定位到{可比实例位置图} 的"{" 那里
        //                                Node n = new Run(word, objectResult[index]["委估对象全称"] + " 可比实例位置图");  //创建一个节点 输出委估对象名称
        //                                builder.InsertNode(n); //将节点插入
        //                                builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
        //                                //Builder.MoveTo(node); //移动到 可比实例位置图 的位置上
        //                                //Builder.InsertImage(showMapPath, 500, 300); //替换图片
        //                                builder.InsertImage(AsposeHelper.ScalingImage(showMapPath, 580, 0)); //替换图片
        //                                row.Range.Replace("{可比实例位置图}", "", false, false);
        //                            }
        //                        }
        //                        #endregion

        //                        #region 替换文本
        //                        ReplaceWordData(objectResult[index], row, true);
        //                        ReplaceWordData(objectSurroundingElements, row, true);
        //                        //foreach (KeyValuePair<string, string> kv in objectResult[index])
        //                        //{
        //                        //    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
        //                        //}
        //                        //foreach (KeyValuePair<string, string> kv in objectSurroundingElements)
        //                        //{
        //                        //    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
        //                        //}
        //                        #endregion

        //                    }
        //                }
        //                table.Rows[0].Remove();//删除标记行
        //                table.Rows[0].Remove();//删除模板行 
        //            }
        //        }
        //    }
        //}
        //#endregion

        #region 自动生成动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）

        /// <summary>
        /// 动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <param name="list">对象的字段集合</param>
        /// <param name="isCreatBookMake">是否自动生成书签</param>
        /// <param name="changeBackground">是否将没有被替换的改变背景颜色并添加✖来标识</param>
        /// <param name="isReplaceEmpty">是否将没有被替换的替换为空</param>
        public void DynamicGeneratedTable(string tableName, List<Dictionary<string, string>> list,bool isCreatBookMake=false, bool changeBackground = false, bool isReplaceEmpty = true)
        {
            if (tableName == "" || list.Count <= 0) return;
            string name = "";
            if (tableName == "委估对象表格")
                name = "_ObjectTable";
            else if (tableName == "附属房屋表格")
                name = "_SubHouseTable";
            else
                name = "_table";
            foreach (Section section in word.Sections)
            {
                foreach (Table table in section.Body.Tables)//获取所有表格
                {
                    if (table.Count > 0)
                    {
                        string flat = table.FirstRow.FirstCell.Range.Text.Replace("\a", "");
                        if (!string.IsNullOrEmpty(flat) && flat.Contains(tableName) && flat.Split(',', '，').Length > 1
                            && AsposeHelper.IsNumeric((flat.Split(',', '，')[1] ?? "").Trim()))
                        {
                            int num = Convert.ToInt32(flat.Split(',', '，')[1]) - 1;
                            int cellsNum = table.Rows[num].Cells.Count;
                            for (int j = 0; j < list.Count; j++)
                            {
                                //获取表格的模版行
                                Row clonedRow = (Row)table.Rows[num].Clone(true);
                                //移除该行中的所有内容
                                //foreach (Cell cell in clonedRow.Cells)
                                //    cell.RemoveAllChildren();//移除单元格里面的内容
                                //将clonedRow追加到某一行后面
                                table.InsertAfter(clonedRow, table.Rows[num + j]);
                                for (int i = 0; i < cellsNum; i++)
                                {
                                    ReplaceWordData(list[j], table.Rows[num + j + 1].Cells[i], changeBackground, isCreatBookMake, name + (j + 1) + "_", isReplaceEmpty);
                                }
                            }
                            table.Rows[num].Remove();//删除导出数据模版行
                            table.Rows[0].Remove();//删除标记行
                        }
                    }
                }
            }
        }
        #endregion

        //#region 动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）
        ///// <summary>
        ///// 动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）
        ///// </summary>
        ///// <param name="tableName">表格名称</param>
        ///// <param name="list">对象的字段集合</param>
        ///// <param name="changeBackground">是否将没有被替换的改变背景颜色并添加✖来标识</param>
        ///// <param name="isReplaceEmpty">是否将没有被替换的替换为空</param>
        //public void DynamicGeneratedTable(string tableName, List<Dictionary<string, string>> list, bool changeBackground = false,bool isReplaceEmpty=true)
        //{
        //    if (tableName != "" && list.Count > 0)
        //    {
        //        foreach(Section section in word.Sections)
        //        {
        //            foreach (Table table in section.Body.Tables)//获取所有表格
        //            {
        //                if (table.Count > 0)
        //                {
        //                    string flat = table.FirstRow.FirstCell.Range.Text.Replace("\a", "");
        //                    if (!string.IsNullOrEmpty(flat) && flat.Contains(tableName) && flat.Split(',', '，').Length>1
        //                        && AsposeHelper.IsNumeric((flat.Split(',', '，')[1] ?? "").Trim()))
        //                    {
        //                        int num = Convert.ToInt32(flat.Split(',', '，')[1]) - 1;
        //                        int cellsNum = table.Rows[num].Cells.Count;
        //                        for (int j = 0; j < list.Count; j++)
        //                        {
        //                            //获取表格的模版行
        //                            Row clonedRow = (Row)table.Rows[num].Clone(true);
        //                            //移除该行中的所有内容
        //                            //foreach (Cell cell in clonedRow.Cells)
        //                            //    cell.RemoveAllChildren();//移除单元格里面的内容
        //                            //将clonedRow追加到某一行后面
        //                            table.InsertAfter(clonedRow, table.Rows[num + j]);
        //                            for (int i = 0; i < cellsNum; i++)
        //                            {
        //                                ReplaceWordData(list[j], table.Rows[num + j + 1].Cells[i],changeBackground, isReplaceEmpty);
        //                                //Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
        //                                //string str = rg.Match(table.Rows[num].Cells[i].GetText()).Value;
        //                                //if (str != null && str != "")
        //                                //{
        //                                //    string Text = str.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "");
        //                                //    if (list[j].Keys.Contains(Text) && list[j][Text] != null)
        //                                //        table.Rows[num + j + 1].Cells[i].Range.Replace(str, list[j][Text], false, false);//替换新增的单元格里面的内容
        //                                //    else if (ChangeBackground)
        //                                //    {
        //                                //        table.Rows[num + j + 1].Cells[i].Range.Replace(new System.Text.RegularExpressions.Regex(RegexReplace(str)), new ReplaceAndInsertText("", Builder, ChangeBackground), false);
        //                                //        if (str.Substring(str.Length - 2, 1) != "✖")
        //                                //            table.Rows[num + j + 1].Cells[i].Range.Replace(str, str.Replace(Text, Text + "✖"), false, false);
        //                                //    }
        //                                //    // table.Rows[num + j + 1].Cells[i].Range.Replace(str, "", false, false);//解决表格中如果不清空的话，后面进行全文替换时会把表格中的也替换导致数据不准确
        //                                //}
        //                            }
        //                        }
        //                        table.Rows[num].Remove();//删除导出数据模版行
        //                        table.Rows[0].Remove();//删除标记行
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion

        #region 自动生成正则匹配含{}包含的内容 并进行替换

        /// <summary>
        /// 正则匹配含{}包含的内容 并进行替换
        /// </summary>
        /// <param name="dicExcel">数据字段</param>
        /// <param name="compositenode">要替换的对象</param>
        /// <param name="changeBackground">是否将没有被替换的改变背景颜色并添加✖来标识</param>
        /// <param name="isCreatBookMake">是否自动生成书签</param>
        /// <param name="falt"></param>标识插入书签是书签的名称规则”_PuTong_“是普通替换，“_ObjectTable1_”是委估对象表格替换，“_SubHouseTable1_“附属房屋表格，“_SingleFactors1_”是个别因素替换
        /// <param name="isReplaceEmpty">是否将没有被替换的替换为空</param>
        public void ReplaceWordData(Dictionary<string, string> dicExcel, CompositeNode compositenode, bool changeBackground,bool isCreatBookMake=false, string falt = "_PuTong_", bool isReplaceEmpty = false)
        {
            string text = compositenode.GetText();
            //foreach (Node node in compositenode.ChildNodes)
            //{
            //    if (node.Range.Text.IndexOf("｛查勘开始时间｝", StringComparison.Ordinal) > 0)
            //    {

            //    }
            //}
            Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
            foreach (Match m in rg.Matches(text))
            {
                string key = m.Value.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "");
                try
                {
                    var bookMakeName = "auto_"+key + falt;
                    if (dicExcel.Keys.Contains(key) && dicExcel[key] != null)
                    {
                        compositenode.Range.Replace(new Regex(RegexReplace(m.Value)), new ReplaceAndInsertText(bookMakeName, dicExcel[key], isCreatBookMake, false), false);
                    }
                    else if (changeBackground)
                    {
                        //if (m.Value.Substring(m.Value.Length - 2, 1) != "✖")
                        //    compositenode.Range.Replace(new System.Text.RegularExpressions.Regex(RegexReplace(m.Value)), new ReplaceAndInsertText(m.Value.Replace(key, key + "✖"), Builder, false), false);
                        compositenode.Range.Replace(new Regex(RegexReplace(m.Value)), new ReplaceAndInsertText(bookMakeName, "", isCreatBookMake, true), false);
                        //if (m.Value.Substring(m.Value.Length - 2, 1) != "✖")
                        //    compositenode.Range.Replace(m.Value, m.Value.Replace(key, key + "✖"), false, false);
                    }
                    else if (isReplaceEmpty)
                        compositenode.Range.Replace(m.Value, "", false, false);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex + "字段名称:" + text + "******字段内容:" + dicExcel[key]);
                }
            }
        }
        #endregion

        //#region 正则匹配含{}包含的内容 并进行替换

        ///// <summary>
        ///// 正则匹配含{}包含的内容 并进行替换
        ///// </summary>
        ///// <param name="dicExcel">数据字段</param>
        ///// <param name="compositenode">要替换的对象</param>
        ///// <param name="changeBackground">是否将没有被替换的改变背景颜色并添加✖来标识</param>
        ///// <param name="isReplaceEmpty">是否将没有被替换的替换为空</param>
        //public void ReplaceWordData(Dictionary<string, string> dicExcel, CompositeNode compositenode, bool changeBackground, bool isReplaceEmpty = false)
        //{
        //    string text = compositenode.GetText();
        //    Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
        //    foreach (Match m in rg.Matches(text))
        //    {
        //        string key =
        //            m.Value.Replace("{", "")
        //                .Replace("}", "")
        //                .Replace("｛", "")
        //                .Replace("｝", "");
        //        //                    .Replace("\a", "")
        //        //                    .Replace("\r", "")

        //        try
        //        {
        //            if (dicExcel.Keys.Contains(key) && dicExcel[key] != null)
        //            {
        //                compositenode.Range.Replace(new Regex(RegexReplace(m.Value)),
        //                    new ReplaceAndInsertText(dicExcel[key], builder, false), false);
        //            }
        //            else if (changeBackground)
        //            {
        //                //if (m.Value.Substring(m.Value.Length - 2, 1) != "✖")
        //                //    compositenode.Range.Replace(new System.Text.RegularExpressions.Regex(RegexReplace(m.Value)), new ReplaceAndInsertText(m.Value.Replace(key, key + "✖"), builder, false), false);
        //                compositenode.Range.Replace(new Regex(RegexReplace(m.Value)),
        //                    new ReplaceAndInsertText("", builder, true), false);
        //                if (m.Value.Substring(m.Value.Length - 2, 1) != "✖")
        //                    compositenode.Range.Replace(m.Value, m.Value.Replace(key, key + "✖"), false, false);
        //            }
        //            else if (isReplaceEmpty)
        //                compositenode.Range.Replace(m.Value, "", false, false);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw new Exception(ex + "字段名称:" + text + "******字段内容:" + dicExcel[key]);
        //        }
        //    }
        //}
        //#endregion

        #region 正则匹配含{}包含的内容 并进行替换
        /// <summary>
        /// 正则匹配含{}包含的内容 并进行替换
        /// </summary>
        /// <param name="dicExcel">数据字段</param>
        public void ReplaceWordData(Dictionary<string, string> dicExcel)
        {
            ReplaceWordData(dicExcel, word, false);
        }
        #endregion

        #region 正则中特殊字符替换为原字符
        //正则中特殊字符替换为原字符
        public string RegexReplace(string text)
        {
            return text.Replace("*", @"\*").Replace("(", @"\(").Replace(")", @"\)").Replace("[", @"\[").Replace("]", @"\]").Replace("{", @"\{").Replace("}", @"\}").Replace("?", @"\?").Replace(".", @"\.").Replace("^", @"\^").Replace("$", @"\$").Replace("+", @"\+").Replace("|", @"\|");
        }
        #endregion

        #region 在书签位置插入数据
        public void DataInsertToBookmark(List<Dictionary<string, string>> multipleObjectDic,string flat)
        {
            for (int i = 0; i < multipleObjectDic.Count; i++)
            {
                DataInsertToBookmark(multipleObjectDic[i], i+1,flat);
            }
        }
        public void DataInsertToBookmark(Dictionary<string, string> dicExcel)
        {
            DataInsertToBookmark(dicExcel, 0,"PuTong");
        }

        /// <summary>
        /// 在书签位置插入数据
        /// </summary>
        /// <param name="dicExcel">数据字典</param>
        /// <param name="index">表格和个别因素生成时标识现在生成到了哪个委估对象</param>
        /// <param name="flat">标识替换委估对象还是附属房屋还是个别因素</param>
        public void DataInsertToBookmark(Dictionary<string, string> dicExcel,int index,string flat)
        {
            BookmarkCollection bookmarkcollection = word.Range.Bookmarks;
            double width = 580, height = 0;
            bool isTransition = true;
            GetShapeWh("估价师证书", ref width, ref height);
            if (width == 580 && height == 0)
            { isTransition = false; }
            foreach (Bookmark bookmark in bookmarkcollection)
            {
                string name = bookmark.Name.Contains("PO_") ? bookmark.Name.Replace("PO_", "") : bookmark.Name;
                if ("机构证书复印件,估价师证书复印件,第一报告人证书,参与报告人证书,审核人证书".Contains(name) && dicExcel.Keys.Contains(name))
                {
                    #region //机构证书复印件,估价师证书复印件,第一报告人证书,参与报告人证书,审核人证书。机构证书复印件,估价师证书复印件这两个是很久以前就有的不知道干啥的先放在这里

                    if (builder.MoveToBookmark(bookmark.Name) && dicExcel[name] != null && dicExcel[name] != "")
                    {
                        for (int i = 0; i < dicExcel[name].Split(',').Length; i++)
                        {
                            try
                            {
                                if (
                                    File.Exists(
                                        HttpContext.Current.Server.MapPath(dicExcel[name].Split(',')[i])))
                                {
                                    Image image =
                                        AsposeHelper.ScalingImage(
                                            HttpContext.Current.Server.MapPath(dicExcel[name].Split(',')[i]),
                                            width, height, isTransition);
                                    if (image != null)
                                    {
                                        bookmark.Text = "";
                                        builder.InsertImage(image);
                                        builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                        image.Dispose();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex + "字段名称：" + name + "******字段内容：" + dicExcel[name]);
                            }
                        }
                    }

                    #endregion
                }
                else if ("区位因素分析,估价方法描述,最高最佳使用方法描述,市场背景分析".Contains(name) && dicExcel.Keys.Contains(name))
                {
                    #region 区位因素分析,估价方法描述,最高最佳使用方法描述,市场背景分析

                    try
                    {
                        if (builder.MoveToBookmark(bookmark.Name))
                        {
                            //因为要把数据插入到书签里面才能进行替换所以只能删除该书签了在重新插入书签
                            string bookmarkname = bookmark.Name;
                            bookmark.Text = "";
                            bookmark.Remove();
                            builder.StartBookmark(bookmarkname);
                            //bookmark.Text = dicExcel[name];
                            builder.InsertHtml(dicExcel[name]);
                            builder.EndBookmark(bookmarkname);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex + "字段名称：" + name + "******字段内容：" + dicExcel[name]);
                    }

                    #endregion
                }
                else
                {
                    #region 自动生成表格与表单字段

                    try
                    {
                        if (bookmark.Name.Contains("_")&&flat!="")
                        {
                            string[] arrStrings = bookmark.Name.Split('_');
                            string key = arrStrings[1];
                            if (!string.IsNullOrEmpty(key) && dicExcel.Keys.Contains(key))
                            {
                                if (arrStrings[2] == flat)
                                {
                                    if (builder.MoveToBookmark(bookmark.Name))
                                    {
                                        //因为要把数据插入到书签里面才能进行替换所以只能删除该书签了在重新插入书签
                                        string bookmarkname = bookmark.Name;
                                        bookmark.Text = "";
                                        bookmark.Remove();
                                        builder.StartBookmark(bookmarkname);
                                        builder.Write(dicExcel[key]);
                                        builder.EndBookmark(bookmarkname);
                                        //bookmark.Text = "";
                                        //builder.Write(dicExcel[key]);
                                        //bookmark.Text = dicExcel[key];
                                    }
                                }
                                else if (index > 0 && arrStrings[2] == (flat + index))
                                {
                                    if (builder.MoveToBookmark(bookmark.Name))
                                    {
                                        //因为要把数据插入到书签里面才能进行替换所以只能删除该书签了在重新插入书签
                                        string bookmarkname = bookmark.Name;
                                        bookmark.Text = "";
                                        bookmark.Remove();
                                        builder.StartBookmark(bookmarkname);
                                        builder.Write(dicExcel[key]);
                                        builder.EndBookmark(bookmarkname);
                                        //bookmark.Text = dicExcel[key];
                                        //builder.Write(dicExcel[key]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex + "字段名称：" + name + "******字段内容：" );
                    }

                    #endregion
                }
            }
        }
        #endregion

        #region 在文本框中插入图片
        /// <summary>
        /// 在文本框中插入图片
        /// </summary>
        /// <param name="dicExcel">数据字典</param>
        public void ImageInsertToShape(Dictionary<string, string> dicExcel, bool isScaling)
        {
            NodeCollection shapes = word.GetChildNodes(NodeType.Shape, true);
            foreach (Shape shape in shapes)
            { 
                string text = shape.GetText();//获取文本框中的内容
                Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
                text = rg.Match(text).ToString();
                if ((text.Contains("{") || text.Contains("｛")) && (text.Contains("}") || text.Contains("｝")))//如果文本框中没有包括｛｝就继续查找
                {
                    Paragraph run = (Paragraph) shape.ChildNodes[0];
                    string textType = text.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "").Trim();
                    if (dicExcel.Keys.Contains(textType)&&File.Exists(dicExcel[textType]))
                    {
                        if (isScaling)
                        {
                            shape.Range.Replace(text, "", false, false);
                            Image image = AsposeHelper.ScalingImage(dicExcel[textType], shape.Width, shape.Height, true);
                            if (image != null)
                            {
                                builder.MoveTo(run);
                                builder.InsertImage(image);
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                                builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                image.Dispose();
                            }
                        }
                        else
                        {
                            shape.RemoveAllChildren();//删除文本框中的内容
                            shape.ImageData.SetImage(dicExcel[textType]);
                        }
                    }
                }
            }
        }
        #endregion

        #region 保存文件
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            word.Save(fileName);
        }
        #endregion

        #region 把word保存成pdf
        /// <summary>
        /// 把word保存成pdf
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAsPDF(string fileName)
        {
            word.Save(fileName,SaveFormat.Pdf);
        }
        #endregion

        #region 清除word中的修改痕迹
        /// <summary>
        /// 清除word中的修改痕迹
        /// </summary>
        public void AcceptAllRevisions()
        {            
            word.AcceptAllRevisions();
        }
        #endregion

        #region 获取指定文本框的宽度和高度 在生成估价师证书是用来限制生成的高度和高度
        /// <summary>
        /// 获取指定文本框的宽度和高度 在生成估价师证书是用来限制生成的高度和高度
        /// </summary>
        /// <param name="shapename"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void GetShapeWh(string shapename, ref double width, ref double height)
        {
            foreach (Shape shape in shapes)//遍历所有文本框
            {
                string text = shape.GetText();//获取文本框中的内容
                Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
                text = rg.Match(text).ToString();
                if ((text.Contains("{") || text.Contains("｛")) && (text.Contains("}") || text.Contains("｝")))//如果文本框中没有包括｛｝就继续查找
                {
                    if (text.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "").Trim() == shapename)
                    {
                        height = shape.Height;
                        width = shape.Width;
                        shape.Remove();
                    }
                }
            }
        }
        #endregion

        #region 合并单元格

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="curTable">需要合并单元格的表</param>
        /// <param name="mergedCellIndexList">合并单元格的起始位置以及横向和纵向合并的数目</param>
        public void MergedCell(Table curTable, List<int[]> mergedCellIndexList)
        {
            //暂时先这样发现更好的思路了在修改
            foreach (var item in mergedCellIndexList)
            {
                //针对既有横向合并又有纵向合并的单元格经行特殊处理
                for (int i = 0; i < item[3]; i++)
                {
                    //横向合并单元格
                    for (int j = 0; j < item[2]; j++)
                    {
                        try
                        {
                            curTable.Rows[item[0] + i].Cells[item[1] + j].CellFormat.HorizontalMerge =
                                j == 0 ? CellMerge.First : CellMerge.Previous;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex + "单元格详细：" + item.ToArray() + "******横向合并出错索引：行" + (item[0] + i) + ",列" +
                                                (item[1] + j));
                        }
                    }
                }
            }
            foreach (var item in mergedCellIndexList)
            {
                //纵向合并单元格
                for (int j = 0; j < item[3]; j++)
                {
                    try
                    {
                    curTable.Rows[item[0] + j].Cells[item[1]].CellFormat.VerticalMerge = j == 0
                        ? CellMerge.First
                        : CellMerge.Previous;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex + "单元格详细：" + item.ToArray() + "******横向合并出错索引：行" + (item[0] + j) + ",列" +
                                                item[1]);
                        }
                }
            }
        }

        #endregion

    }

    #region 自动生成重写ReplacingCallback 用来替换word里面的文本
    /// <summary>
    /// 重写ReplacingCallback
    /// </summary>
    public class ReplaceAndInsertText : IReplacingCallback
    {
        /// <summary>
        /// </summary>
        public string Text { get; set; }
        //public DocumentBuilder Builder;
        public bool ChangeBackground;
        public string BookMakeName;
        public bool IsCreatBookMake;
        public ReplaceAndInsertText(string bookMakeName, string text,bool isCreatBookMake, bool changeBackground)
        {
            this.Text = text;
            //this.Builder = builder;
            this.ChangeBackground = changeBackground;
            this.BookMakeName = bookMakeName;
            IsCreatBookMake = isCreatBookMake;
        }
        public ReplaceAction Replacing(ReplacingArgs e)
        {
            DocumentBuilder builder1 = new DocumentBuilder((Document)e.MatchNode.Document);
            ReplaceAction replaceaction;
            Node currentNode = e.MatchNode;
            //当有可能第一个run在正则匹配区域之前 这时要对run进行处理
            if (e.MatchOffset > 0)
                currentNode = SplitRun((Run)currentNode, e.MatchOffset);
            ArrayList runs = GetAllRunsArrayList(currentNode, e.Match.Value.Length);
            if (ChangeBackground)
            {
                #region 改变为替换文字的背景颜色

                // 高亮显示所有的run
                foreach (Run run in runs)
                    run.Font.HighlightColor = Color.Yellow;
                replaceaction = ReplaceAction.Skip;

                #endregion

            }
            else if (IsCreatBookMake)
            {
                #region word内容替换时生成书签

                string guid = Guid.NewGuid().ToString().Replace("-", "");
                //将光标移动到指定节点
                builder1.MoveTo(currentNode);
                builder1.StartBookmark(BookMakeName + guid);
                //特殊处理客户的数据模版中包含^p的代表换行
                if (Text.ToLower().Contains("^p"))
                {
                    string[] str = Text.Split(new string[2] {"^p", "^P"}, StringSplitOptions.None);
                    for (int i = 1; i < str.Length; i++)
                    {
                        builder1.Writeln();
                        builder1.Write(str[i]);
                    }
                }
                else
                    builder1.Write(Text);
                builder1.EndBookmark(BookMakeName + guid);

                // 清空所有的run的值
                foreach (Run run in runs)
                    run.Text = ""; //清空匹配区域的值
                replaceaction = ReplaceAction.Skip;

                #endregion

            }
            else
            {

                #region word内容普通替换

                if (Text.Contains("\r") || Text.Contains("\a") || Text.Contains("\n") ||
                    Text.ToLower().Contains("^p"))
                {
                    //获取当前节点
                    //将光标移动到指定节点
                    builder1.MoveTo(currentNode);
                    //特殊处理客户的数据模版中包含^p的代表换行
                    if (Text.ToLower().Contains("^p"))
                    {
                        string[] str = Text.Split(new string[2] {"^p", "^P"}, StringSplitOptions.None);
                        for (int i = 1; i < str.Length; i++)
                        {
                            builder1.Writeln();
                            builder1.Write(str[i]);
                        }
                    }
                    else
                        builder1.Write(Text);
                }
                else
                    e.Replacement = Text;
                replaceaction = ReplaceAction.Replace;

                #endregion

            }
            return replaceaction;
        }

        #region 获取指定位置的节点

        /// <summary>
        /// 获取指定位置的节点
        /// </summary>
        private static Run SplitRun(Run run, int position)
        {
            Run afterRun = (Run) run.Clone(true);
            afterRun.Text = run.Text.Substring(position);
            run.Text = run.Text.Substring(0, position);
            run.ParentNode.InsertAfter(afterRun, run);
            return afterRun;
        }

        #endregion

        #region  获取匹配字段的run集合

        /// <summary>
        /// 获取匹配字段的run集合
        /// </summary>
        private static ArrayList GetAllRunsArrayList(Node currentNode, int remainingLength)
        {
            ArrayList runs = new ArrayList();
            while (
                (remainingLength > 0) &&
                (currentNode != null) &&
                (currentNode.GetText().Length <= remainingLength))
            {
                runs.Add(currentNode);
                remainingLength = remainingLength - currentNode.GetText().Length;
                //根据匹配字段一个一个查找run
                do
                {
                    currentNode = currentNode.NextSibling;
                } while ((currentNode != null) && (currentNode.NodeType != NodeType.Run));
            }

            //根据匹配字段的长度从最后都run中取出需要的字
            if ((currentNode != null) && (remainingLength > 0))
            {
                SplitRun((Run) currentNode, remainingLength);
                runs.Add(currentNode);
            }
            return runs;
        }

        #endregion

    }
    #endregion

    //#region 重写ReplacingCallback 用来替换word里面的文本
    ///// <summary>
    ///// 重写ReplacingCallback
    ///// </summary>
    //public class ReplaceAndInsertText : IReplacingCallback
    //{
    //    /// <summary>
    //    /// </summary>
    //    public string text { get; set; }
    //    public DocumentBuilder builder;
    //    public bool changeBackground;
    //    public ReplaceAndInsertText(string text, DocumentBuilder builder, bool changeBackground)
    //    {
    //        this.text = text;
    //        this.builder = builder;
    //        this.changeBackground = changeBackground;
    //    }
    //    public ReplaceAction Replacing(ReplacingArgs e)
    //    {
    //        ReplaceAction replaceaction;
    //        if (changeBackground)
    //        {
    //            Node currentNode = e.MatchNode;

    //            //当有可能第一个run在正则匹配区域之前 这时要对run进行处理
    //            if (e.MatchOffset > 0)
    //                currentNode = SplitRun((Run)currentNode, e.MatchOffset);

    //            ArrayList runs = new ArrayList();

    //            int remainingLength = e.Match.Value.Length;
    //            while (
    //                (remainingLength > 0) &&
    //                (currentNode != null) &&
    //                (currentNode.GetText().Length <= remainingLength))
    //            {
    //                runs.Add(currentNode);
    //                remainingLength = remainingLength - currentNode.GetText().Length;
    //                //循环为了防止有其他类型的节点 像书签
    //                do
    //                {
    //                    currentNode = currentNode.NextSibling;
    //                } while ((currentNode != null) && (currentNode.NodeType != NodeType.Run));
    //            }

    //            //run后判断该run后是否还包含了run 有的话在组合一次在添加到runs
    //            if ((currentNode != null) && (remainingLength > 0))
    //            {
    //                SplitRun((Run)currentNode, remainingLength);
    //                runs.Add(currentNode);
    //            }

    //            // 高亮显示所有的run
    //            foreach (Run run in runs)
    //                run.Font.HighlightColor = Color.Yellow;
    //            replaceaction = ReplaceAction.Skip;
    //        }
    //        else
    //        {
    //            if (text.Contains("\r") || text.Contains("\a") || text.Contains("\n") ||
    //                text.ToLower().Contains("^p"))
    //            {
    //                //Node currentNode = e.MatchNode;

    //                ////当有可能第一个run在正则匹配区域之前 这时要对run进行处理
    //                //if (e.MatchOffset > 0)
    //                //    currentNode = SplitRun((Run)currentNode, e.MatchOffset);
    //                //获取当前节点
    //                var currentNode = e.MatchNode;
    //                //将光标移动到指定节点
    //                builder.MoveTo(currentNode);
    //                //特殊处理客户的数据模版中包含^p的代表换行
    //                if (text.ToLower().Contains("^p"))
    //                {
    //                    string[] str = text.Split(new string[2] { "^p", "^P" }, System.StringSplitOptions.None);
    //                    for (int i = 1; i < str.Length; i++)
    //                    {
    //                        builder.Writeln();
    //                        builder.Write(str[i]);
    //                    }
    //                }
    //                else
    //                    builder.Write(text);
    //            }
    //            else
    //                e.Replacement = text;
    //            replaceaction = ReplaceAction.Replace;
    //        }
    //        return replaceaction;

    //    }
    //    /// <summary>
    //    /// 将指定位置的run与匹配的run进行分隔在拼接
    //    /// </summary>
    //    private static Run SplitRun(Run run, int position)
    //    {
    //        Run afterRun = (Run)run.Clone(true);
    //        afterRun.Text = run.Text.Substring(position);
    //        run.Text = run.Text.Substring(0, position);
    //        run.ParentNode.InsertAfter(afterRun, run);
    //        return afterRun;
    //    }
    //}
    //#endregion
}


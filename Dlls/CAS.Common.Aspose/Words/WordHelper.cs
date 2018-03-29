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

namespace CAS.Common.Aspose
{
    public class WordHelper
    {
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

        /// <summary>
        /// 生成测算过程
        /// </summary>
        /// <param name="openTargetExcelFile">测算表文件路径集合</param>
        /// <param name="insertImageExcelFile">比较法测算表</param>
        /// <param name="document">文档对象</param>
        /// <param name="prefixBookMarkName">测算表书签名称</param>
        /// <param name="sheetName">测算表sheet名称</param>
        /// <param name="prefixRegionName">测算表区域命名管理器名称</param>
        /// <param name="tempPath">测算表区域命名管理器名称</param>
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
        /// <summary>
        /// 把测算表中的图片插入到报告中
        /// </summary>
        /// <param name="imageExcelFile">图片所在excel</param>
        /// <param name="sheetName"></param>
        /// <param name="eh"></param>
        private List<byte[]> GetExcelImageData(string imageExcelFile, string sheetName, ExcelHelper eh)
        {
            List<byte[]> imgList = new List<byte[]>();
            if (imageExcelFile.EndsWith(".xls"))
            {
                Xls xls = new Xls(imageExcelFile, true);
                imgList = xls.GetPictureData(sheetName);
                xls.Close();
            }
            else
            {
                imgList = eh.GetPictureData();
            }
            return imgList;
        }
        private void InsertImgToWord(List<byte[]> imgList)
        {
            //写入word中
            foreach (byte[] item in imgList)
            {
                builder.InsertImage(item, 400, 280);
            }
        }
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
                    return LineStyle.Dot;
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
        /// <summary>
        /// 生成个别因素
        /// </summary>
        /// <param name="word"></param>
        /// <param name="builder"></param>
        /// <param name="objectResult">多个委估对象的数据</param>
        /// <param name="objectSurroundingElements">委估对象的周围数据</param>
        /// <param name="surveyMapDic">物业位置图</param>
        public void GenerateSingleFactors(List<Dictionary<string, string>> objectResult, Dictionary<string, string> objectSurroundingElements, Dictionary<string, string> surveyMapDic)
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
                            for (int index = objectResult.Count - 1; index >= 0; index--)
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

                                #region 替换文本
                                foreach (KeyValuePair<string, string> kv in objectResult[index])
                                {
                                    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
                                }
                                foreach (KeyValuePair<string, string> kv in objectSurroundingElements)
                                {
                                    row.Range.Replace("{" + kv.Key + "}", kv.Value, false, false);
                                }
                                #endregion

                                #region 物业位置图和卫星图和可比实例位置图的插入
                                // 20160225添加卫星图的插入  潘锦发
                                if (surveyMapDic.ContainsKey("物业位置图_" + index) && nodeList.Count>0)
                                {
                                    string showMapPath = surveyMapDic["物业位置图_" + index];//用来存储生成的物业位置图的虚拟路径
                                    //输出物业位置图照片
                                    foreach (Node node in nodeList)
                                    {
                                        builder.MoveTo(node);//定位到{物业位置图} 的"{" 那里
                                        Node n= new Run(word, objectResult[index]["委估对象全称"] + " 物业位置图");  //创建一个节点 输出委估对象名称
                                        builder.InsertNode(n); //将节点插入
                                        builder.InsertBreak(BreakType.ParagraphBreak); //插入换行符
                                        //builder.MoveTo(node); //移动到 物业位置图 的位置上
                                        builder.InsertImage(showMapPath, 500, 300); //替换图片
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
                                        //builder.MoveTo(node); //移动到 物业卫星图 的位置上
                                        builder.InsertImage(showMapPath, 500, 300); //替换图片
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
                                        //builder.MoveTo(node); //移动到 可比实例位置图 的位置上
                                        builder.InsertImage(showMapPath, 500, 300); //替换图片
                                        row.Range.Replace("{可比实例位置图}", "", false, false);
                                    }
                                }
                                #endregion
                            }
                        }
                        table.Rows[0].Remove();//删除标记行
                        table.Rows[0].Remove();//删除模板行 
                    }
                }
            }
        }
        ///<summary>
        ///验证输入的数据是不是正整数
        ///</summary>
        ///<param name="str">传入字符串</param>
        ///<returns>返回true或者false</returns>
        static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            return reg1.IsMatch(str);
        }
        #region 动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）
        /// <summary>
        /// 动态生成表格 例如报告生成多套中的委估对象表格（委估对象表格，5）
        /// </summary>
        /// <param name="TableName">表格名称</param>
        /// <param name="list">对象的字段集合</param>
        public void DynamicGeneratedTable(string TableName, List<Dictionary<string, string>> list)
        {
            if (TableName != "" && list.Count > 0)
            {
                foreach(Section section in word.Sections)
                {
                    foreach (Table table in section.Body.Tables)//获取所有表格
                    {
                        if (table.Count > 0)
                        {
                            string flat = table.FirstRow.FirstCell.Range.Text.Replace("\a", "");
                            int num = 0;
                            if (!string.IsNullOrEmpty(flat) && flat.Contains(TableName) && flat.Split(',', '，').Length>1
                                && IsNumeric((flat.Split(',', '，')[1] == null ? "" : flat.Split(',', '，')[1]).Trim()))
                            {
                                num = Convert.ToInt32(flat.Split(',', '，')[1]) - 1;
                                int CellsNum = table.Rows[num].Cells.Count;
                                string[] name = new string[CellsNum];
                                for (int j = 0; j < list.Count; j++)
                                {
                                    //获取表格的模版行
                                    Row clonedRow = (Row)table.Rows[num].Clone(true);
                                    //移除该行中的所有内容
                                    //foreach (Cell cell in clonedRow.Cells)
                                    //    cell.RemoveAllChildren();//移除单元格里面的内容
                                    //将clonedRow追加到某一行后面
                                    table.InsertAfter(clonedRow, table.Rows[num + j]);
                                    for (int i = 0; i < CellsNum; i++)
                                    {
                                        Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
                                        string str = rg.Match(table.Rows[num].Cells[i].GetText()).Value;
                                        if (str != null && str != "")
                                        {
                                            string text = str.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "");
                                            if (list[j].Keys.Contains(text))
                                                table.Rows[num + j + 1].Cells[i].Range.Replace(str, list[j][text], false, false);//替换新增的单元格里面的内容
                                            else
                                                table.Rows[num + j + 1].Cells[i].Range.Replace(str, "", false, false);//解决表格中如果不清空的话，后面进行全文替换时会把表格中的也替换导致数据不准确
                                        }
                                    }
                                }
                                table.Rows[num].Remove();//删除导出数据模版行
                                table.Rows[0].Remove();//删除标记行
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 正则匹配含{}包含的内容 并进行替换
        /// <summary>
        /// 正则匹配含{}包含的内容 并进行替换
        /// </summary>
        /// <param name="word"></param>
        /// <param name="dicExcel">数据字段</param>
        public void ReplaceWordData(Dictionary<string, string> dicExcel)
        {
            string text = word.GetText();
            Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
            foreach (Match m in rg.Matches(text))
            {
                string key = m.Value.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "").Replace("\a", "").Replace("\r", "");
                if (dicExcel.Keys.Contains(key) && dicExcel[key] != null)
                {
                    word.Range.Replace(new System.Text.RegularExpressions.Regex(RegexReplace(m.Value)), new ReplaceAndInsertText(dicExcel[key], builder), false);
                }
            }
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
        /// <summary>
        /// 在书签位置插入数据
        /// </summary>
        /// <param name="dicExcel">数据字典</param>
        public void DataInsertToBookmark(Dictionary<string, string> dicExcel)
        {
            BookmarkCollection bookmarkcollection = word.Range.Bookmarks;
            foreach (Bookmark bookmark in bookmarkcollection)
            {
                string name = "";
                if (bookmark.Name.Contains("PO_"))
                    name = bookmark.Name.Replace("PO_", "");
                else
                    name = bookmark.Name;
                if ("机构证书复印件,估价师证书复印件,第一报告人证书,参与报告人证书,审核人证书".Contains(name) && dicExcel.Keys.Contains(name))
                {
                    if (builder.MoveToBookmark(bookmark.Name) && dicExcel[name] != null && dicExcel[name] != "")
                    {
                        for (int i = 0; i < dicExcel[name].Split(',').Length; i++)
                        {
                            builder.InsertImage(System.Web.HttpContext.Current.Server.MapPath(dicExcel[name].Split(',')[i]), 430, 300);
                        }
                    }
                }
                else if ("区位因素分析,估价方法描述,最高最佳使用方法描述,市场背景分析".Contains(name) && dicExcel.Keys.Contains(name))
                {
                    if (builder.MoveToBookmark(bookmark.Name))
                        builder.InsertHtml(dicExcel[name]);
                }
            }
        }
        #endregion

        #region 在文本框中插入图片
        /// <summary>
        /// 在文本框中插入图片
        /// </summary>
        /// <param name="dicExcel">数据字典</param>
        public void ImageInsertToShape(Dictionary<string, string> dicExcel)
        {
            NodeCollection shapes = word.GetChildNodes(NodeType.Shape, true);
            System.Collections.ArrayList usedPictureIndexs = new System.Collections.ArrayList();
            foreach (Shape shape in shapes)
            {
                string text = shape.GetText();
                Regex rg = new Regex(@"(\{|｛)[^＼{＼}｛｝]+(\}|｝)");
                shape.RemoveAllChildren();
                shape.WrapType = WrapType.Inline;
                foreach (Match m in rg.Matches(text))
                {
                    string key = m.Value.Replace("{", "").Replace("}", "").Replace("｛", "").Replace("｝", "");
                    if(usedPictureIndexs.Contains(key))
                    if (dicExcel.Keys.Contains(key))
                    {
                        Image image = Image.FromFile(dicExcel[key]);
                        shape.ImageData.SetImage(image);
                    }
                    usedPictureIndexs.Add(key);
                }
            }
        }
        #endregion

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            word.Save(fileName);
        }
        /// <summary>
        /// 把word保存成pdf
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAsPDF(string fileName)
        {
            word.Save(fileName,SaveFormat.Pdf);
        }
        /// <summary>
        /// 清除word中的修改痕迹
        /// </summary>
        public void AcceptAllRevisions()
        {            
            word.AcceptAllRevisions();
        }
    }

    /// <summary>
    /// 重写ReplacingCallback
    /// </summary>
    public class ReplaceAndInsertText : IReplacingCallback
    {
        /// <summary>
        /// </summary>
        public string text { get; set; }
        public DocumentBuilder builder;

        public ReplaceAndInsertText(string text, DocumentBuilder builder)
        {
            this.text = text;
            this.builder = builder;
        }
        public ReplaceAction Replacing(ReplacingArgs e)
        {
            if (text.Contains("\r") || text.Contains("\a") || text.Contains("\n") || text.ToLower().Contains("^p"))
            {
                //获取当前节点
                var node = e.MatchNode;
                //将光标移动到指定节点
                builder.MoveTo(node);
                //特殊处理客户的数据模版中包含^p的代表换行
                if (text.ToLower().Contains("^p"))
                {
                    string[] str = text.Split(new string[2] { "^p", "^P" }, System.StringSplitOptions.None);
                    for (int i = 1; i < str.Length; i++)
                    {
                        builder.Writeln();
                        builder.Write(str[i]);
                    }
                }
                else
                    builder.Write(text);
            }
            else
                e.Replacement = text;
            return ReplaceAction.Replace;
        }
    }
}


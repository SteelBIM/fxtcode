
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.IO;
using System.Data;
using Microsoft.Office.Interop.Word;
using System.Data.OleDb;
using System.Reflection;

namespace CAS.Common.Office
{
    public class WordHelper
    {
        #region - 属性 -
        private Microsoft.Office.Interop.Word.ApplicationClass oWord;
        // a reference to Word application，应用程序
        private Microsoft.Office.Interop.Word.Document oDoc;
        // a reference to the document，具体文档
        object missing = System.Reflection.Missing.Value;
        public Microsoft.Office.Interop.Word.ApplicationClass WordApplication
        {
            get { return oWord; }
        }
        //byte 2014-11-26
        public Microsoft.Office.Interop.Word.Document WordDocument
        {
            get { return oDoc; }
        }
        public string ActiveWindowCaption
        {
            get
            {
                return oWord.ActiveWindow.Caption;
            }
            set
            {
                oWord.ActiveWindow.Caption = value;
            }
        }
        public enum OwdWrapType
        {
            嵌入型, //wdWrapInline
            四周型, //Square.
            紧密型, //Tight.
            衬于文字下方,//Behind text.
            衬于文字上方 //Top and bottom.
        }
        #endregion
        #region  - 创建关闭文档 -
        public WordHelper() //构造函数 1
        {
            // activate the interface with the COM object of Microsoft Word
            try
            {
                oWord = new Microsoft.Office.Interop.Word.ApplicationClass();
                oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }
        public WordHelper(Microsoft.Office.Interop.Word.ApplicationClass wordapp) //构造函数 2
        {
            try
            {
                oWord = wordapp;
                oWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }
        // Open a file (the file must exists) and activate it，打开已存在
        public void Open(string strFileName)
        {
            object fileName = strFileName;
            object readOnly = false;
            object isVisible = true;            
            try
            {
                oDoc = oWord.Documents.Open(ref fileName, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                oDoc.Activate();
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }
        // Open a file (the file must exists) and activate it，打开已存在
        public void OpenReadonly(string strFileName)
        {
            object fileName = strFileName;
            object readOnly = true;
            object isVisible = true; 
            try
            {
                oDoc = oWord.Documents.Open(ref fileName, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                oDoc.Activate();
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }
        // Open a new document，创建新文档
        public void Open()
        {
            try
            {
                oDoc = oWord.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                oDoc.Activate();
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }
        public void Quit()
        {
            try
            {
                object saveOption = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                oDoc.Close(ref saveOption, ref missing, ref missing);
                if (oDoc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDoc);
                    oDoc = null;
                }
                // oWord.Application.Quit(ref missing, ref missing, ref missing); tjt                
                oWord.Quit(ref saveOption, ref missing, ref missing);
                if (oWord != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oWord);
                    oWord = null;
                }
                //释放word进程
                GC.Collect();
            }
            catch (Exception ex)
            {
                //释放word进程
                GC.Collect();
            }
        }
        /// <summary>  
        /// 从模板创建新的Word文档，  
        /// </summary>  
        /// <param name="templateName">模板文件名</param>  
        /// <returns></returns>  
        public bool LoadDotFile(string templateName)
        {
            if (!string.IsNullOrEmpty(templateName))
            {
                oWord.Visible = false;
                oWord.Caption = "";
                oWord.Options.CheckSpellingAsYouType = false;
                oWord.Options.CheckGrammarAsYouType = false;
                Object Template = templateName;// Optional Object. The name of the template to be used for the new document. If this argument is omitted, the Normal template is used.  
                Object NewTemplate = false;// Optional Object. True to open the document as a template. The default value is False.  
                Object DocumentType = Microsoft.Office.Interop.Word.WdNewDocumentType.wdNewBlankDocument; // Optional Object. Can be one of the following WdNewDocumentType constants: wdNewBlankDocument, wdNewEmailMessage, wdNewFrameset, or wdNewWebPage. The default constant is wdNewBlankDocument.  
                Object Visible = true;//Optional Object. True to open the document in a visible window. If this value is False, Microsoft Word opens the document but sets the Visible property of the document window to False. The default value is True.  
                try
                {
                    oDoc = oWord.Documents.Add(ref Template, ref NewTemplate, ref DocumentType, ref Visible);
                    return true;
                }
                catch (Exception ex)
                {
                    string err = string.Format("创建Word文档出错，错误原因：{0}", ex.Message);
                    throw new Exception(err, ex);
                }
            }
            return false;
        }
        ///  
        /// 打开Word文档,并且返回对象oDoc
        /// 完整Word文件路径+名称  
        /// 返回的Word.Document oDoc对象 
        public Microsoft.Office.Interop.Word.Document CreateWordDocument(string FileName, bool HideWin)
        {
            if (FileName == "") return null;
            oWord.Visible = HideWin;
            oWord.Caption = "";
            oWord.Options.CheckSpellingAsYouType = false;
            oWord.Options.CheckGrammarAsYouType = false;
            Object filename = FileName;
            Object ConfirmConversions = false;
            Object ReadOnly = true;
            Object AddToRecentFiles = false;
            Object PasswordDocument = System.Type.Missing;
            Object PasswordTemplate = System.Type.Missing;
            Object Revert = System.Type.Missing;
            Object WritePasswordDocument = System.Type.Missing;
            Object WritePasswordTemplate = System.Type.Missing;
            Object Format = System.Type.Missing;
            Object Encoding = System.Type.Missing;
            Object Visible = System.Type.Missing;
            Object OpenAndRepair = System.Type.Missing;
            Object DocumentDirection = System.Type.Missing;
            Object NoEncodingDialog = System.Type.Missing;
            Object XMLTransform = System.Type.Missing;

            Microsoft.Office.Interop.Word.Document wordDoc = oWord.Documents.Open(ref filename, ref ConfirmConversions,
            ref ReadOnly, ref AddToRecentFiles, ref PasswordDocument, ref PasswordTemplate,
            ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format,
            ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection,
            ref NoEncodingDialog, ref XMLTransform);
            return wordDoc;
        }
        public void SaveAs(Microsoft.Office.Interop.Word.Document oDoc, string strFileName)
        {
            object fileName = strFileName;
            object oformat = WdSaveFormat.wdFormatDocument;
            if (File.Exists(strFileName))
            {

                oDoc.SaveAs(ref fileName, ref oformat, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref
 missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            }
            else
            {
                oDoc.SaveAs(ref fileName, ref oformat, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            }
        }
        public void SaveAsHtml(Microsoft.Office.Interop.Word.Document oDoc, string strFileName)
        {
            object fileName = strFileName;
            //wdFormatWebArchive保存为单个网页文件
            //wdFormatFilteredHTML保存为过滤掉word标签的htm文件，缺点是有图片的话会产生网页文件夹
            if (File.Exists(strFileName))
            {

                object Format = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatWebArchive;
                oDoc.SaveAs(ref fileName, ref Format, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

            }
            else
            {
                object Format = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatWebArchive;
                oDoc.SaveAs(ref fileName, ref Format, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
            }
        }
        public void Save()
        {
            oDoc.Save();
        }

        public void SaveAs(string strFileName)
        {
            object fileName = strFileName;
            object oformat = WdSaveFormat.wdFormatDocument;
            oDoc.SaveAs(ref fileName, ref oformat, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        }
        // Save the document in HTML format
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="encoding">编码，utf8、gb2312、gbk</param>
        public void SaveAsHtml(string strFileName, string encoding = null)
        {
            object fileName = strFileName;
            object Format = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            if (!string.IsNullOrEmpty(encoding))
            {
                if (encoding.ToLower() == "utf8")
                {
                    oDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
                }
                else if (encoding.ToLower() == "gb2312")
                {

                    oDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingMacSimplifiedChineseGB2312;
                }
                else if(encoding.ToLower() == "gbk")
                {

                    oDoc.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingSimplifiedChineseGBK;
                }
            }
            oDoc.SaveAs(ref fileName, ref Format, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        }
        #endregion
        #region 添加菜单(工具栏)项
        //添加单独的菜单项
        public void AddMenu(Microsoft.Office.Core.CommandBarPopup popuBar)
        {
            Microsoft.Office.Core.CommandBar menuBar = null;
            menuBar = this.oWord.CommandBars["Menu Bar"];
            popuBar = (Microsoft.Office.Core.CommandBarPopup)this.oWord.CommandBars.FindControl(Microsoft.Office.Core.MsoControlType.msoControlPopup,
 missing, popuBar.Tag, true);
            if (popuBar == null)
            {
                popuBar =
(Microsoft.Office.Core.CommandBarPopup)menuBar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlPopup,
 missing, missing, missing, missing);
            }
        }
        //添加单独工具栏
        public void AddToolItem(string strBarName, string strBtnName)
        {
            Microsoft.Office.Core.CommandBar toolBar = null;
            toolBar =
(Microsoft.Office.Core.CommandBar)this.oWord.CommandBars.FindControl(Microsoft.Office.Core.MsoControlType.msoControlButton,
 missing, strBarName, true);
            if (toolBar == null)
            {
                toolBar =
(Microsoft.Office.Core.CommandBar)this.oWord.CommandBars.Add(

Microsoft.Office.Core.MsoControlType.msoControlButton,
                     missing, missing, missing);
                toolBar.Name = strBtnName;
                toolBar.Visible = true;
            }
        }
        #endregion
        #region 移动光标位置
        // Go to a predefined bookmark, if the bookmark doesn't exists the application will raise an error
        public void GotoBookMark(string strBookMarkName)
        {
            // VB :  Selection.GoTo What:=wdGoToBookmark, Name:="nome"
            object Bookmark = (int)Microsoft.Office.Interop.Word.WdGoToItem.wdGoToBookmark;
            object NameBookMark = strBookMarkName;
            oWord.Selection.GoTo(ref Bookmark, ref missing, ref missing, ref NameBookMark);
        }
        public void GoToTheEnd()
        {
            // VB :  Selection.EndKey Unit:=wdStory
            object unit;
            unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            oWord.Selection.EndKey(ref unit, ref missing);
        }
        public void GoToLineEnd()
        {
            object unit = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            object ext =
Microsoft.Office.Interop.Word.WdMovementType.wdExtend;
            oWord.Selection.EndKey(ref unit, ref ext);
        }
        public void GoToTheBeginning()
        {
            // VB : Selection.HomeKey Unit:=wdStory
            object unit;
            unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            oWord.Selection.HomeKey(ref unit, ref missing);
        }
        public void GoToTheTable(int ntable)
        {
            //    Selection.GoTo What:=wdGoToTable, Which:=wdGoToFirst, Count:=1, Name:=""
            //    Selection.Find.ClearFormatting
            //    With Selection.Find
            //        .Text = ""
            //        .Replacement.Text = ""
            //        .Forward = True
            //        .Wrap = wdFindContinue
            //        .Format = False
            //        .MatchCase = False
            //        .MatchWholeWord = False
            //        .MatchWildcards = False
            //        .MatchSoundsLike = False
            //        .MatchAllWordForms = False
            //    End With
            object what;
            what = Microsoft.Office.Interop.Word.WdUnits.wdTable;
            object which;
            which =
Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToFirst;
            object count;
            count = 1;
            oWord.Selection.GoTo(ref what, ref which, ref count, ref 
missing);
            oWord.Selection.Find.ClearFormatting();
            oWord.Selection.Text = "";
        }
        public void GoToRightCell()
        {
            // Selection.MoveRight Unit:=wdCell
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdCell;
            oWord.Selection.MoveRight(ref direction, ref missing, ref 
missing);
        }
        public void GoToLeftCell()
        {
            // Selection.MoveRight Unit:=wdCell
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdCell;
            oWord.Selection.MoveLeft(ref direction, ref missing, ref 
missing);
        }
        public void GoToDownCell()
        {
            // Selection.MoveRight Unit:=wdCell
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            oWord.Selection.MoveDown(ref direction, ref missing, ref 
missing);
        }
        public void GoToUpCell()
        {
            // Selection.MoveRight Unit:=wdCell
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            oWord.Selection.MoveUp(ref direction, ref missing, ref 
missing);
        }
        #endregion
        #region  - 插入操作  -
        public void InsertText(string strText) //插入文本
        {
            oWord.Selection.TypeText(strText);
        }
        public void InsertLineBreak() //插入换行符
        {
            oWord.Selection.TypeParagraph();
        }
        /// <summary>
        /// 插入多个空行
        /// </summary>
        /// <param name="nline"></param>
        public void InsertLineBreak(int nline)
        {
            for (int i = 0; i < nline; i++)
                oWord.Selection.TypeParagraph();
        }
        public void InsertPagebreak() //插入分页符
        {
            // VB : Selection.InsertBreak Type:=wdPageBreak
            object pBreak =
(int)Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            oWord.Selection.InsertBreak(ref pBreak);
        }
        // 插入页码
        public void InsertPageNumber() //在正文中插入页码
        {
            object wdFieldPage =
Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            object preserveFormatting = true;
            oWord.Selection.Fields.Add(oWord.Selection.Range, ref 
wdFieldPage, ref missing, ref preserveFormatting);
        }
        // 插入页码
        public void InsertPageNumber(string strAlign)
        {
            object wdFieldPage =
Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            object preserveFormatting = true;
            oWord.Selection.Fields.Add(oWord.Selection.Range, ref 
wdFieldPage, ref missing, ref preserveFormatting);
            SetAlignment(strAlign);
        }
        #region - 插入页脚 -
        public bool InsertPageFooter(string text)
        {
            try
            {
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;//页脚 
                oWord.Selection.InsertAfter(text); //.InsertAfter(text);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InsertPageHeader(string text)
        {
            try
            {
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader;//页眉
                oWord.Selection.InsertAfter(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InsertPageFooterNumber()
        {
            try
            {
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader; //页眉
                oWord.Selection.WholeStory();

                oWord.Selection.ParagraphFormat.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineStyle
                 = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone; //取消页眉的下划线
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument; //转到正文
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;//页脚 
                oWord.Selection.TypeText("第");
                object page =
Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage; //当前页码
                oWord.Selection.Fields.Add(oWord.Selection.Range, ref 
page, ref missing, ref missing);
                oWord.Selection.TypeText("页/共");
                object pages =
Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages; //总页码
                oWord.Selection.Fields.Add(oWord.Selection.Range, ref 
pages, ref missing, ref missing);
                oWord.Selection.TypeText("页");
                oWord.ActiveWindow.View.SeekView =
Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        public void InsertLine(float left, float top, float width, float
 weight, int r, int g, int b)
        {
            //SetFontColor("red");
            //SetAlignment("Center");
            object Anchor = oWord.Selection.Range;
            //int pLeft = 0, pTop = 0, pWidth = 0, pHeight = 0;
            //oWord.ActiveWindow.GetPoint(out pLeft, out pTop, out pWidth, out pHeight,missing);
            //MessageBox.Show(pLeft + "," + pTop + "," + pWidth + "," + pHeight);
            object rep = false;
            //left += oWord.ActiveDocument.PageSetup.LeftMargin;
            left = oWord.CentimetersToPoints(left);
            top = oWord.CentimetersToPoints(top);
            width = oWord.CentimetersToPoints(width);
            Microsoft.Office.Interop.Word.Shape s =
oWord.ActiveDocument.Shapes.AddLine(0, top, width, top, ref Anchor);
            s.Line.ForeColor.RGB = RGB(r, g, b);
            s.Line.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;
            s.Line.Style =
Microsoft.Office.Core.MsoLineStyle.msoLineSingle;
            s.Line.Weight = weight;
        }
        #endregion
        #region - 插入图片 -
        public void InsertImage(string strPicPath, float picWidth, float picHeight)
        {
            string FileName = strPicPath;
            object LinkToFile = false;
            object SaveWithDocument = true;
            object Anchor = oWord.Selection.Range;
            oWord.ActiveDocument.InlineShapes.AddPicture(FileName, ref LinkToFile, ref SaveWithDocument, ref Anchor).Select();
            oWord.Selection.InlineShapes[1].Width = picWidth; // 图片宽度 
            oWord.Selection.InlineShapes[1].Height = picHeight; // 图片高度
        }
        //public void InsertImage(string strPicPath, float picWidth, float picHeight, OwdWrapType owdWrapType)
        //{
        //    string FileName = strPicPath;
        //    object LinkToFile = false;
        //    object SaveWithDocument = true;
        //    object Anchor = oWord.Selection.Range;
        //    oWord.ActiveDocument.InlineShapes.AddPicture(FileName, ref LinkToFile, ref SaveWithDocument, ref Anchor).Select();
        //    oWord.Selection.InlineShapes[1].Width = picWidth; // 图片宽度 
        //    oWord.Selection.InlineShapes[1].Height = picHeight; // 图片高度
        //    // 将图片设置为四面环绕型 
        //  //  Microsoft.Office.Interop.Word.Shape s = oWord.Selection.InlineShapes[1].ConvertToShape();
        //  //  s.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapNone; //wdWrapSquare 四周环绕型
        //}
        #endregion
        #region - 插入表格 -
        public bool InsertTable(System.Data.DataTable dt, bool haveBorder, double[]
colWidths)
        {
            try
            {
                object Nothing = System.Reflection.Missing.Value;
                int lenght = oDoc.Characters.Count - 1;
                object start = lenght;
                object end = lenght;
                //表格起始坐标
                Microsoft.Office.Interop.Word.Range tableLocation =
oDoc.Range(ref start, ref end);
                //添加Word表格     
                Microsoft.Office.Interop.Word.Table table =
oDoc.Tables.Add(tableLocation, dt.Rows.Count, dt.Columns.Count, ref 
Nothing, ref Nothing);
                if (colWidths != null)
                {
                    for (int i = 0; i < colWidths.Length; i++)
                    {
                        table.Columns[i + 1].Width = (float)(28.5F *
colWidths[i]);
                    }
                }
                ///设置TABLE的样式
                table.Rows.HeightRule =
Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightAtLeast;
                table.Rows.Height =
oWord.CentimetersToPoints(float.Parse("0.8"));
                table.Range.Font.Size = 10.5F;
                table.Range.Font.Name = "宋体";
                table.Range.Font.Bold = 0;
                table.Range.ParagraphFormat.Alignment =
Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                table.Range.Cells.VerticalAlignment =
Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                if (haveBorder == true)
                {
                    //设置外框样式
                    table.Borders.OutsideLineStyle =
Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle =
Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    //样式设置结束
                }
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        table.Cell(row + 1, col + 1).Range.Text =
dt.Rows[row][col].ToString();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
            }
        }
        public bool InsertTable(System.Data.DataTable dt, bool haveBorder)
        {
            return InsertTable(dt, haveBorder, null);
        }
        public bool InsertTable(System.Data.DataTable dt)
        {
            return InsertTable(dt, false, null);
        }
        //插入表格结束
        #endregion
        #region 设置样式
        /// <summary>
        /// Change the paragraph alignement
        /// </summary>
        /// <param name="strType"></param>
        public void SetAlignment(string strType)
        {
            switch (strType.ToLower())
            {
                case "center":
                    oWord.Selection.ParagraphFormat.Alignment =
Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    break;
                case "left":
                    oWord.Selection.ParagraphFormat.Alignment =
Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    break;
                case "right":
                    oWord.Selection.ParagraphFormat.Alignment =
Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                    break;
                case "justify":
                    oWord.Selection.ParagraphFormat.Alignment =
Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    break;
            }
        }

        // if you use thif function to change the font you should call it again with 
        // no parameter in order to set the font without a particular format
        public void SetFont(string strType)
        {
            switch (strType)
            {
                case "Bold":
                    oWord.Selection.Font.Bold = 1;
                    break;
                case "Italic":
                    oWord.Selection.Font.Italic = 1;
                    break;
                case "Underlined":
                    oWord.Selection.Font.Subscript = 0;
                    break;
                case "Normal":
                    oWord.Selection.Font.Bold = 0;
                    oWord.Selection.Font.Italic = 0;
                    oWord.Selection.Font.Subscript = 0;
                    SetFontName("宋体"); //默认宋体，tjt
                    SetFontSize(10.5f);  //默认五号字体，tjt
                    break;
            }
        }
        // disable all the style 
        public void SetFont()
        {
            oWord.Selection.Font.Bold = 0;
            oWord.Selection.Font.Italic = 0;
            oWord.Selection.Font.Subscript = 0;
            SetFontName("宋体"); //默认宋体，tjt
            SetFontSize(10.5f);  //默认五号字体，tjt
        }
        public void SetFontName(string strType)
        {
            oWord.Selection.Font.Name = strType;
        }
        public void SetFontSize(float nSize)
        {
            SetFontSize(nSize, 100);
        }
        public void SetFontSize(float nSize, int scaling)
        {
            if (nSize > 0f)
                oWord.Selection.Font.Size = nSize;
            if (scaling > 0)
                oWord.Selection.Font.Scaling = scaling;
        }
        public void SetFontColor(string strFontColor)
        {
            switch (strFontColor.ToLower())
            {
                case "blue":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                    break;
                case "gold":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorGold;
                    break;
                case "gray":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorGray875;
                    break;
                case "green":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
                    break;
                case "lightblue":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorLightBlue;
                    break;
                case "orange":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorOrange;
                    break;
                case "pink":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorPink;
                    break;
                case "red":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorRed;
                    break;
                case "yellow":
                    oWord.Selection.Font.Color =
Microsoft.Office.Interop.Word.WdColor.wdColorYellow;
                    break;
            }
        }
        public void SetPageNumberAlign(string strType, bool bHeader)
        {
            object alignment;
            object bFirstPage = false;
            object bF = true;
            //if (bHeader == true)

            //WordApplic.Selection.HeaderFooter.PageNumbers.ShowFirstPageNumber = bF;
            switch (strType)
            {
                case "Center":
                    alignment = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;

                    //WordApplic.Selection.HeaderFooter.PageNumbers.Add(ref alignment,ref bFirstPage);  
                    //Microsoft.Office.Interop.Word.Selection objSelection = WordApplic.pSelection;

                    oWord.Selection.HeaderFooter.PageNumbers[1].Alignment =
                    Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;
                    break;
                case "Right":
                    alignment =
Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberRight;

                    oWord.Selection.HeaderFooter.PageNumbers[1].Alignment =
                    Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberRight;
                    break;
                case "Left":
                    alignment =
Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberLeft;
                    oWord.Selection.HeaderFooter.PageNumbers.Add(ref 
alignment, ref bFirstPage);
                    break;
            }
        }
        /// <summary>
        /// 设置页面为标准A4公文样式
        /// </summary>
        private void SetA4PageSetup()
        {
            oWord.ActiveDocument.PageSetup.TopMargin =
oWord.CentimetersToPoints(3.7f);
            //oWord.ActiveDocument.PageSetup.BottomMargin = 
            oWord.CentimetersToPoints(1f);
            oWord.ActiveDocument.PageSetup.LeftMargin =
oWord.CentimetersToPoints(2.8f);
            oWord.ActiveDocument.PageSetup.RightMargin =
oWord.CentimetersToPoints(2.6f);
            //oWord.ActiveDocument.PageSetup.HeaderDistance = 
            oWord.CentimetersToPoints(2.5f);
            //oWord.ActiveDocument.PageSetup.FooterDistance = 
            oWord.CentimetersToPoints(1f);
            oWord.ActiveDocument.PageSetup.PageWidth =
oWord.CentimetersToPoints(21f);
            oWord.ActiveDocument.PageSetup.PageHeight =
oWord.CentimetersToPoints(29.7f);
        }
        /// <summary>
        /// 设置页面为指定标准
        /// </summary>
        public void PageSetup(bool isPaperA4, float topMargin, float rightMargin, float bottomMargin, float leftMargin, float headerMargin, float footerMargin)
        {
            if (isPaperA4)
            {
                oWord.ActiveDocument.PageSetup.PageWidth = oWord.CentimetersToPoints(21f);
                oWord.ActiveDocument.PageSetup.PageHeight = oWord.CentimetersToPoints(29.7f);
            }
            oWord.ActiveDocument.PageSetup.TopMargin = oWord.CentimetersToPoints(topMargin);
            oWord.ActiveDocument.PageSetup.RightMargin = oWord.CentimetersToPoints(rightMargin);
            oWord.ActiveDocument.PageSetup.BottomMargin = oWord.CentimetersToPoints(bottomMargin);
            oWord.ActiveDocument.PageSetup.LeftMargin = oWord.CentimetersToPoints(leftMargin);
            oWord.ActiveDocument.PageSetup.HeaderDistance = oWord.CentimetersToPoints(headerMargin);
            oWord.ActiveDocument.PageSetup.FooterDistance = oWord.CentimetersToPoints(footerMargin);
        }
        #endregion
        #region 替换
        ///<summary>
        /// 在word 中查找一个字符串直接替换所需要的文本
        /// </summary>
        /// <param name="strOldText">原文本</param>
        /// <param name="strNewText">新文本</param>
        /// <returns></returns>
        public bool Replace(string strOldText, string strNewText)
        {
            if (oDoc == null)
                oDoc = oWord.ActiveDocument;
            this.oDoc.Content.Find.Text = strOldText;            
            object FindText, ReplaceWith, Replace;// 
            FindText = strOldText;//要查找的文本
            Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;/**//*wdReplaceAll - 替换找到的所有项。
                                                      * wdReplaceNone - 不替换找到的任何项。
                                                    * wdReplaceOne - 替换找到的第一项。
                                                    * */
            oDoc.Content.Find.ClearFormatting();//移除Find的搜索文本和段落格式设置
            strNewText = strNewText.Replace("\r", "").Replace("\n", ((char)10).ToString()).Replace(((char)10).ToString(), "^p");
            ReplaceWith = strNewText;//替换文本   
            if (strNewText.Length > 200)
            {
                int index = 0;
                while (strNewText.Length > (index + 1) * 200)
                {
                    ReplaceWith = strNewText.Substring(index * 200, 200) + FindText;
                    oDoc.Content.Find.Execute(
                    ref FindText, ref missing,
                    ref missing, ref missing,
                    ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref ReplaceWith, ref Replace,
                    ref missing, ref missing,
                    ref missing, ref missing);
                    index += 1;
                }
                ReplaceWith = strNewText.Substring(index * 200);
                oDoc.Content.Find.Execute(
                ref FindText, ref missing,
                ref missing, ref missing,
                ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref ReplaceWith, ref Replace,
                ref missing, ref missing,
                ref missing, ref missing);
            }
            else
            {

                oDoc.Content.Find.Execute(
                    ref FindText, ref missing,
                    ref missing, ref missing,
                    ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref ReplaceWith, ref Replace,
                    ref missing, ref missing,
                    ref missing, ref missing);

            }
            //图形
            foreach (Shape shape in oDoc.Shapes)
            {
                if (shape.TextFrame.HasText != 0)
                {
                    shape.TextFrame.TextRange.Find.Execute(ref FindText,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref ReplaceWith, ref Replace, ref missing, ref missing, ref missing, ref missing);
                }
            }
            //页眉
            oDoc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Find.Execute(
                ref FindText, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing
                , ref missing, ref missing, ref ReplaceWith, ref Replace, ref missing, ref missing, ref missing, ref missing);
            return true;
        }
        /// <summary>
        /// 替换批注 kevin
        /// </summary>
        /// <param name="commentname"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ReplaceComments(string commentname, string value)
        {
            if (oDoc == null)
                oDoc = oWord.ActiveDocument;
            Microsoft.Office.Interop.Word.Comments comments = oDoc.Comments;
            for (int i = 1; i <= comments.Count; i++)
            {
                if (commentname == comments[i].Range.Text)
                {
                    comments[i].Scope.Text = value;
                    comments[i].Delete();
                    break;
                }
            }
            return true;
        }
        public bool SearchReplace(string strOldText, string strNewText)
        {
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            //首先清除任何现有的格式设置选项，然后设置搜索字符串 strOldText。
            oWord.Selection.Find.ClearFormatting();
            oWord.Selection.Find.Text = strOldText;
            oWord.Selection.Find.Replacement.ClearFormatting();
            oWord.Selection.Find.Replacement.Text = strNewText;
            if (oWord.Selection.Find.Execute(
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region - 表格操作 -
        public bool FindTable(string bookmarkTable)
        {
            try
            {
                object bkObj = bookmarkTable;
                if (oWord.ActiveDocument.Bookmarks.Exists(bookmarkTable)
 == true)
                {
                    oWord.ActiveDocument.Bookmarks.get_Item(ref 
bkObj).Select();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MoveNextCell()
        {
            try
            {
                Object unit =
Microsoft.Office.Interop.Word.WdUnits.wdCell;
                Object count = 1;
                oWord.Selection.Move(ref unit, ref count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetCellValue(string value)
        {
            try
            {
                oWord.Selection.TypeText(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetCellValue(string strPicPath, float picWidth, float picHeight)
        {
            string FileName = strPicPath;
            object LinkToFile = false;
            object SaveWithDocument = true;
            object Anchor = oWord.Selection.Range;
            oWord.Selection.InlineShapes.AddPicture(FileName, ref LinkToFile, ref SaveWithDocument, ref Anchor).Select();
            oWord.Selection.InlineShapes[1].Width = picWidth; // 图片宽度 
            oWord.Selection.InlineShapes[1].Height = picHeight; // 图片高度

            //移动到前一个单元格
            Object unit = Microsoft.Office.Interop.Word.WdUnits.wdCell;
            Object count = -1;
            oWord.Selection.Move(ref unit, ref count);
        }
        public void MoveNextRow()
        {
            try
            {
                Object extend =
Microsoft.Office.Interop.Word.WdMovementType.wdExtend;
                Object unit =
Microsoft.Office.Interop.Word.WdUnits.wdCell;
                Object count = 1;
                oWord.Selection.MoveRight(ref unit, ref count, ref 
extend);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //表格操作结束
        #endregion
        #region 填充书签

        public bool ExistsBookmark(string bookmark)
        {
            return oWord.ActiveDocument.Bookmarks.Exists(bookmark);
        }

        /// <summary>  
        /// 填充书签  
        /// </summary>  
        /// <param name="bookmark">书签</param>  
        /// <param name="value">值</param>  
        public void bookmarkReplace(string bookmark, string value)
        {
            //替换书签内容的同时，书签同时还保留下来。
            if (oWord.ActiveDocument.Bookmarks.Exists(bookmark) == true)
            {
                Range range = oWord.ActiveDocument.Bookmarks[bookmark].Range;
                range.Text = value;
                range.Select();
                oWord.ActiveDocument.Bookmarks.Add(bookmark, range);
            }
        }
        #endregion
        #region 替换书签 ,  string dataPath, string savePath
        public string ReplaceBookmarksByTable(System.Data.DataTable table, string datapath, string savepath)
        {
            string guid = "";
            string savefilename = "";
            try
            {
                //读取Word文档
                //oDoc = CreateWordDocument(datapath, true);//("E:\\住宅询价单.doc", false);
                OpenReadonly(datapath);
                if (oDoc == null)
                    return "";
                //读取数据
                System.Data.DataTable dt = table;//FillDataTable(table);
                //设定标签数据
                System.Collections.IEnumerator enu = oWord.ActiveDocument.Bookmarks.GetEnumerator();
                object tempobject = null;
                int length = 0;
                int i = 0;
                string[] strbook = new string[dt.Columns.Count];
                Microsoft.Office.Interop.Word.Bookmark bk = null;
                while (enu.MoveNext())
                {
                    bk = (Microsoft.Office.Interop.Word.Bookmark)enu.Current;

                    if (bk.Name.ToString().Trim() != "Table")
                    {
                        strbook[i] = bk.Name.ToString();
                        i++;
                    }
                }
                //循环找到书签
                if (oDoc.Bookmarks.Count > 0)
                {
                    for (i = 0; i < strbook.Length; i++)
                    {
                        tempobject = strbook[i].ToString();

                        if (oWord.ActiveDocument.Bookmarks.Exists(strbook[i].ToString())) //判断书签在Word中是否存在
                        {
                            oWord.ActiveDocument.Bookmarks.get_Item(ref tempobject).Select(); //找到匹配的书签

                            oWord.Selection.Text = dt.Rows[0][strbook[i]].ToString(); //对匹配的书签进行赋值

                            length = dt.Rows[0][strbook[i]].ToString().Length;
                            oWord.ActiveDocument.Bookmarks.get_Item(ref tempobject).End = oWord.ActiveDocument.Bookmarks.get_Item(ref tempobject).End + length;
                        }
                    }
                }
                //收尾工作

                guid = System.Guid.NewGuid().ToString();
                if (!Directory.Exists(savepath))
                    Directory.CreateDirectory(savepath);
                object sFileName = guid + ".doc";
                savefilename = sFileName.ToString();
                if (oDoc.SaveFormat == (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument)
                {
                    SaveAs(savepath + sFileName.ToString());
                }

                if (oDoc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDoc);
                    oDoc = null;
                }

                if (oWord != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oWord);
                    oWord = null;
                }
            }
            finally
            {
                Quit();
                GC.Collect();
            }
            return savefilename;//guid + ".doc";
        }
        #endregion
        #region 邮件合并域检查模板，用于报告批量生成
        /// <summary>
        /// 邮件合并域检查模板，用于报告批量生成 kevin
        /// </summary>
        /// <param name="tmpFile"></param>
        /// <param name="dataPath"></param>
        /// <param name="savePath"></param>
        public List<string> mailmergeCheck(string tmpFile, string dataPath)
        {
            object mis = System.Reflection.Missing.Value;
            object falseValue = false;
            object trueValue = true;
            List<string> err = new List<string>();
            try
            {
                oWord.Visible = false;
                Open(tmpFile);

                string query = "select * from [Sheet1$]";
                object oquery = query;

                MailMerge mailMerge = oDoc.MailMerge;
                //打开数据文件
                mailMerge.OpenDataSource(dataPath, ref mis, ref mis, ref mis, ref trueValue, ref mis, ref mis,
                    ref mis, ref mis, ref mis, ref mis, ref mis, ref oquery, ref mis, ref mis, ref mis);

                for (int i = 1; i <= mailMerge.Fields.Count; i++)
                {
                    bool find = false;
                    for (int j = 1; j <= mailMerge.DataSource.FieldNames.Count; j++)
                    {
                        object index = j;
                        MailMergeFieldName field = mailMerge.DataSource.FieldNames[index];
                        if (mailMerge.Fields[i].Code.Text.IndexOf(" " + field.Name + " ") > 0)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        err.Add(mailMerge.Fields[i].Code.Text.Replace("MERGEFIELD  ", "").Replace("  \\* MERGEFORMAT", ""));
                        break;
                    }
                }
                Quit();
                return err;
            }
            catch (Exception ex)
            {
                err.Add(ex.Message);
                return err;
            }
            finally { Quit(); }
        }
        #endregion
        #region 自动生成报告 使用bookmark方式，保留导入时读取数据的可能
        /// <summary>
        /// 自动生成报告 使用bookmark方式，保留导入时读取数据的可能 kevin
        /// </summary>
        /// <param name="tmpFile"></param>
        /// <param name="dataPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public string AutoReportByReplace(string tmpFile, string dataPath, string savePath)
        {
            object mis = System.Reflection.Missing.Value;
            object falseValue = false;
            object trueValue = true;
            try
            {
                //使用测算表来生成
                ExcelHelper excel = null;
                Dictionary<string, string> comments = new Dictionary<string, string>();
                try
                {
                    excel = new ExcelHelper(dataPath);
                    string sheetname = "报告自动生成";
                    if (!excel.ChangeCurrentWorkSheet(sheetname))
                    {
                        excel.Quit();
                        //删除错误数据文件
                        try { File.Delete(dataPath); }
                        catch { }
                        return null;
                    }
                    comments = excel.GetCommentsFromCurrent();
                    if (comments.Count <= 0)
                    {
                        excel.Quit();
                        //删除错误数据文件
                        try { File.Delete(dataPath); }
                        catch { }
                        return null;
                    }

                    //打开模板文件
                    oWord.Visible = false;
                    OpenReadonly(tmpFile);
                    //替换表格，使用书签功能，从excel直接拷贝过来，如果大批量使用的话，会不会粘贴他人拷贝的数据？
                    if (oDoc.Bookmarks.Count > 0)
                    {
                        foreach (Bookmark bm in oDoc.Bookmarks)
                        {
                            if (comments.ContainsKey(bm.Name))
                            {
                                string link = comments[bm.Name];
                                if (link.IndexOf("!") > 0 && link.IndexOf(":") > 0)
                                {
                                    string[] split = link.Split('!');
                                    string[] rc = split[1].Split(':');
                                    excel.RangeCopy(split[0], rc[0], rc[1], null);
                                    //这里放表格也比较麻烦，书签不能直接赋值
                                    object rng = bm.Range;
                                    string name = bm.Name;
                                    ((Range)rng).Text = " ";
                                    oDoc.Bookmarks.Add(name, ref rng);
                                    //还有要保留EXCEL表格的格式，不能被WORD的格式覆盖，想了很多办法才解决
                                    //插入一个段落
                                    ((Range)rng).InsertParagraphBefore();
                                    //PASTER表格
                                    ((Range)rng).Paragraphs[1].Range.PasteExcelTable(false, false, false);
                                    //设置格式
                                    ((Range)rng).Select();
                                    oWord.Selection.Paragraphs.SpaceAfter = 0;
                                    oWord.Selection.Paragraphs.SpaceBefore = 0;
                                    oWord.Selection.Paragraphs.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                                    //必须把comment移除，避免死循环
                                    comments.Remove(name);
                                }
                                else
                                {
                                    foreach (KeyValuePair<string, string> kv in comments)
                                    {
                                        if (bm.Name == kv.Key)
                                        {
                                            //这里不能直接使用赋值的方式，会把书签也删除 kevin
                                            //bm.Range.InsertBefore(kv.Value);                        
                                            object rng = bm.Range;
                                            ((Range)rng).Text = string.IsNullOrEmpty(kv.Value) ? " " : kv.Value;
                                            oDoc.Bookmarks.Add(kv.Key, ref rng);
                                            //必须把comment移除，避免死循环
                                            comments.Remove(kv.Key);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    if (excel != null)
                    {
                        excel.Quit();
                    }
                    throw;
                }
                excel.Quit();
                string filename = Guid.NewGuid().ToString().Replace("-", "");
                if (comments.ContainsKey("文件名"))
                {
                    filename = comments["文件名"];
                }
                /*
                foreach (KeyValuePair<string, string> kv in comments) {
                    if (kv.Key == "文件名") {
                        filename = kv.Value;
                        continue;
                    }                    
                    Replace(string.Format("【{0}】",kv.Key),kv.Value);
                }
                 * */
                //更新页码
                oDoc.Fields.Update();
                string newfile = Path.Combine(savePath, filename + ".doc");
                SaveAs(newfile);
                //设置saved为true，避免关闭时出现提示是否保存
                oDoc.Saved = true;
                Quit();
                //删除临时文件和上传的数据文件，如果要留数据文件，就不要删
                try
                {
                    File.Delete(dataPath);
                }
                catch { }
                return newfile;
            }
            catch (Exception ex)
            {
                Quit();
                throw ex;
            }
        }
        #endregion
        #region 从WORD读取数据
        /// <summary>
        /// 从WORD读取数据 kevin
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDataFromDoc(string dataPath)
        {
            object falseValue = false;
            object trueValue = true;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                //打开文件
                oWord.Visible = false;
                OpenReadonly(dataPath);
                //获取数据
                if (oDoc.Bookmarks.Count > 0)
                {
                    foreach (Bookmark bm in oDoc.Bookmarks)
                    {
                        dict.Add(bm.Name, bm.Range.Text);
                    }
                }
                Quit();
                //删除临时文件和上传的数据文件，如果要留数据文件，就不要删
                try
                {
                    File.Delete(dataPath);
                }
                catch { }
                return dict;
            }
            catch (Exception ex)
            {
                Quit();
                throw ex;
                return null;
            }
        }
        #endregion
        #region 自动生成报告，邮件合并域方式 单套版
        /// <summary>
        /// 自动生成报告，邮件合并域方式 单套版 kevin
        /// </summary>
        /// <param name="tmpFile"></param>
        /// <param name="dataPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public string AutoReportByMailMerge(string tmpFile, string dataPath, string savePath)
        {
            object mis = System.Reflection.Missing.Value;
            object falseValue = false;
            object trueValue = true;
            string rtnvalue = null;
            try
            {
                //使用测算表来生成
                ExcelHelper excel = null;
                Dictionary<string, string> comments = new Dictionary<string, string>();
                try
                {
                    excel = new ExcelHelper(dataPath);
                    string sheetname = "报告自动生成";
                    if (!excel.ChangeCurrentWorkSheet(sheetname))
                    {
                        excel.Quit();
                        //删除错误数据文件
                        try { File.Delete(dataPath); }
                        catch { }
                        return null;
                    }
                    comments = excel.GetCommentsFromCurrent();
                    if (comments.Count <= 0)
                    {
                        excel.Quit();
                        //删除错误数据文件
                        try { File.Delete(dataPath); }
                        catch { }
                        return null;
                    }
                    //打开模板文件
                    oWord.Visible = false;
                    Open(tmpFile);
                }
                catch (Exception ex)
                {
                    if (excel != null)
                    {
                        excel.Quit();
                    }
                    throw;
                }
                excel.Quit();
                //新建临时工作簿
                excel = new ExcelHelper();
                string newDataPath = "";
                MailMerge mailMerge = null;
                //如果原文档有索引目录，先删除，否则邮件合并一执行，页码就没有了，这里先用批注作中转
                //因为书签和域都会在合并后丢失，然后用宏来添加索引目录，删除批注，这里比较复杂，没想到更好的办法

                object UseHeadingStyles = missing;
                object HeadingStyles = missing;
                object IncludePageNumbers = missing;
                object LowerHeadingLevel = missing;
                object RightAlignPageNumbers = missing;
                object TableID = missing;
                object UpperHeadingLevel = missing;
                object UseHyperlinks = missing;
                object UseFields = missing;
                object HidePageNumbersInWeb = missing;
                object AddedStyles = missing;
                object UseOutlineLevels = missing;
                try
                {
                    //直接用对象赋值不行，靠
                    //TablesOfContents TOC = null;
                    if (oDoc.TablesOfContents.Count > 0)
                    {
                        UseHeadingStyles = oDoc.TablesOfContents[1].UseHeadingStyles;
                        HeadingStyles = oDoc.TablesOfContents[1].HeadingStyles;
                        IncludePageNumbers = oDoc.TablesOfContents[1].IncludePageNumbers;
                        LowerHeadingLevel = oDoc.TablesOfContents[1].LowerHeadingLevel;
                        RightAlignPageNumbers = oDoc.TablesOfContents[1].RightAlignPageNumbers;
                        TableID = oDoc.TablesOfContents[1].TableID;
                        UpperHeadingLevel = oDoc.TablesOfContents[1].UpperHeadingLevel;
                        UseHyperlinks = oDoc.TablesOfContents[1].UseHyperlinks;
                        UseFields = oDoc.TablesOfContents[1].UseFields;
                        HidePageNumbersInWeb = oDoc.TablesOfContents[1].HidePageNumbersInWeb;
                        //下面两个值找不到
                        //AddedStyles = oDoc.TablesOfContents[1].AddedStyles;
                        //UseOutlineLevels = oDoc.TablesOfContents[1].UseOutlineLevels;
                        //TOC = (TablesOfContents)oDoc.TablesOfContents[1];
                        oDoc.TablesOfContents[1].Range.Select();
                        InsertText("");
                        oDoc.Comments.Add(oWord.Selection.Range, "索引目录");

                        oDoc.TablesOfContents[1].Delete();
                    }
                    //把域进行清理，因为可能会影响到域合并，这里是为了解决天津协会的模板。
                    foreach (Field field in oDoc.Fields)
                    {
                        //天津的模板有很多SET域，如果我们自己加，就用“fxt_”开头，把其他的都删掉。
                        if (field.Type == WdFieldType.wdFieldSet && field.Code.Text.ToLower().IndexOf("fxt_") < 0)
                            field.Delete();
                    }
                    mailMerge = oDoc.MailMerge;
                    //创建数据源字段
                    List<string> fields = new List<string>();
                    fields.Add("文件名");
                    foreach (MailMergeField field in mailMerge.Fields)
                    {
                        string fieldname = field.Code.Text.Replace(" MERGEFIELD  ", "").Replace("  \\* MERGEFORMAT ", "").Trim();
                        if (fields.Contains(fieldname)) continue;
                        fields.Add(fieldname);
                    }
                    //组装数据源
                    string[,] data = new string[2, fields.Count + 1];
                    for (int j = 0; j < fields.Count; j++)
                    {
                        data[0, j] = fields[j];
                        if (comments.ContainsKey(fields[j]))
                            data[1, j] = comments[fields[j]];
                        else
                            data[1, j] = "";
                    }
                    //保存数据源
                    newDataPath = dataPath.Substring(0, dataPath.LastIndexOf(".")) + Guid.NewGuid().ToString() + dataPath.Substring(dataPath.LastIndexOf("."));
                    excel.Array2ToExcel(data, 1, 1, 2, fields.Count);
                    excel.SaveFile(newDataPath);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (excel != null)
                    {
                        excel.Quit();
                    }
                }
                //打开数据源                
                string query = "select * from [Sheet1$]";
                object oquery = query;
                mailMerge.OpenDataSource(newDataPath, ref mis, ref mis, ref mis, ref trueValue, ref mis, ref mis,
                    ref mis, ref mis, ref mis, ref mis, ref mis, ref oquery, ref mis, ref mis, ref mis);
                //合并邮件域
                for (int i = 1; i <= mailMerge.DataSource.RecordCount; i++)
                {
                    //设置当前数据源，只能是一条记录
                    mailMerge.DataSource.FirstRecord = i;
                    mailMerge.DataSource.LastRecord = i;
                    string s = mailMerge.DataSource.DataFields[1].Value;
                    mailMerge.DataSource.ActiveRecord = WdMailMergeActiveRecord.wdNextRecord;
                    mailMerge.Destination = WdMailMergeDestination.wdSendToNewDocument;
                    //执行合并
                    mailMerge.Execute(ref falseValue);
                    Document newDoc = oWord.ActiveDocument;
                    //newDoc.RunAutoMacro(WdAutoMacros.wdAutoExec);
                    //插入索引和目录，由于邮件合并执行后，会删除文档中所有域和书签信息，所以这里采用批注的方式来实现
                    //因为目录不确定是什么模板和格式，所以交由宏来执行 这里折腾了很久。
                    //终于找到不用宏的方法，使用原目录来添加到新文件中。
                    Comments wcomments = newDoc.Comments;
                    for (int ii = 1; ii <= newDoc.Comments.Count; ii++)
                    {
                        if (wcomments[ii].Range.Text == "索引目录")
                        {
                            wcomments[ii].Scope.Text = "";
                            wcomments[ii].Scope.Select();
                            newDoc.TablesOfContents.Add(oWord.Selection.Range, UseHeadingStyles
                                , UpperHeadingLevel, LowerHeadingLevel, UseFields
                                , TableID, RightAlignPageNumbers, IncludePageNumbers
                                , AddedStyles, UseHeadingStyles, HidePageNumbersInWeb, UseOutlineLevels);
                            newDoc.TablesOfContents[1].Range.Select();
                            foreach (Style style in newDoc.Styles)
                            {
                                if (style.NameLocal == "目录")
                                    oWord.Selection.set_Style(style.NameLocal);
                            }
                            //RunMacro(new object[] { "CreateIndex" });
                            wcomments[ii].Delete();
                        }
                    }
                    //.TablesOfContents.Format = wdIndexIndent
                    //newDoc.Fields.Update();
                    //控制分页，改为用word插入分页符控制 
                    //if (newDoc.Bookmarks.Count > 0)
                    //{
                    //    //excel的下标都是要从1开始
                    //    for (int j = 1; j <= newDoc.Bookmarks.Count; j++)
                    //    {
                    //        object index = j;
                    //        if (bm.Name=="分页")
                    //        {
                    //            object pBreak = (int)WdBreakType.wdSectionBreakNextPage;
                    //            bm.Range.InsertBreak(pBreak);
                    //        }
                    //    }
                    //}
                    //执行后会自动打开一个新文档，所以不能用oDoc控制，要用ActiveDocument
                    if (string.IsNullOrEmpty(s)) s = Guid.NewGuid().ToString().Replace("-", "");
                    string newfile = Path.Combine(savePath, s + ".doc");
                    SaveAs(newDoc, newfile);
                    //设置saved为true，避免关闭时出现提示是否保存
                    newDoc.Saved = true;
                    newDoc.Close(ref missing, ref missing, ref missing);
                    if (newDoc != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(newDoc);
                        newDoc = null;
                    }
                    rtnvalue = newfile;
                }
                Quit();
                //删除临时文件和上传的数据文件，如果要留数据文件，就不要删
                try
                {
                    File.Delete(newDataPath);
                    File.Delete(dataPath);
                }
                catch { }
                return rtnvalue;
            }
            catch (Exception ex)
            {
                Quit();
                return null;
            }
        }
        #endregion
        private void RunMacro(object[] oRunArgs)
        {
            oWord.GetType().InvokeMember("Run",
                System.Reflection.BindingFlags.Default |
                System.Reflection.BindingFlags.InvokeMethod,
                null, oWord, oRunArgs);
        }
        #region 邮件合并域，用于报告批量生成
        /// <summary>
        /// 邮件合并域，用于报告批量生成 kevin
        /// </summary>
        /// <param name="tmpFile"></param>
        /// <param name="dataPath"></param>
        /// <param name="savePath"></param>
        public string[] mailmergeReplace(string tmpFile, string dataPath, string savePath)
        {
            object mis = System.Reflection.Missing.Value;
            object falseValue = false;
            object trueValue = true;

            try
            {
                oWord.Visible = false;
                Open(tmpFile);

                string query = "select * from [Sheet1$]";
                object oquery = query;

                MailMerge mailMerge = oDoc.MailMerge;
                //打开数据文件
                mailMerge.OpenDataSource(dataPath, ref mis, ref mis, ref mis, ref trueValue, ref mis, ref mis,
                    ref mis, ref mis, ref mis, ref mis, ref mis, ref oquery, ref mis, ref mis, ref mis);
                //ExcelHelper excel = new ExcelHelper(dataPath);

                string[] strs = new string[mailMerge.DataSource.RecordCount];
                for (int i = 1; i <= mailMerge.DataSource.RecordCount; i++)
                {
                    //设置当前数据源，只能是一条记录
                    mailMerge.DataSource.FirstRecord = i;
                    mailMerge.DataSource.LastRecord = i;
                    string s = mailMerge.DataSource.DataFields[1].Value;
                    mailMerge.DataSource.ActiveRecord = WdMailMergeActiveRecord.wdNextRecord;
                    mailMerge.Destination = WdMailMergeDestination.wdSendToNewDocument;

                    mailMerge.Execute(ref falseValue);
                    //执行后会自动打开一个新文档，所以不能用oDoc控制，要用ActiveDocument
                    Document newDoc = oWord.ActiveDocument;
                    /*
                    //copy excel表格，这里还不能研究出完全按格式COPY kevin
                    excel.RangeCopy(i + 1, "A1", "g28", null);
                    oWord.Selection.PasteAndFormat(WdRecoveryType.wdPasteDefault);
                     * */
                    if (string.IsNullOrEmpty(s)) s = Guid.NewGuid().ToString().Replace("-", "");
                    string newfile = Path.Combine(savePath, s + ".doc");
                    SaveAs(newDoc, newfile);
                    //设置saved为true，避免关闭时出现提示是否保存
                    newDoc.Saved = true;
                    newDoc.Close(ref missing, ref missing, ref missing);
                    if (newDoc != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(newDoc);
                        newDoc = null;
                    }
                    strs[i - 1] = newfile;
                }
                //excel.Quit();
                oDoc.Saved = true;
                Quit();
                return strs;
            }
            catch (Exception ex)
            {
                Quit();
                return null;
            }
        }
        #endregion
        #region rgb转换函数
        /// <summary>
        /// rgb转换函数
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        int RGB(int r, int g, int b)
        {
            return ((b << 16) | (ushort)(((ushort)g << 8) |
r));
        }
        Color RGBToColor(int color)
        {
            int r = 0xFF & color;
            int g = 0xFF00 & color;
            g >>= 8;
            int b = 0xFF0000 & color;
            b >>= 16;
            return Color.FromArgb(r, g, b);
        }
        #endregion
        #region
        /// <summary>
        /// 添加文本框
        /// <remarks>byte 2014-12-10</remarks>
        /// </summary>
        /// <param name="left">文本框的左边坐标</param>
        /// <param name="top">文本框的上边坐标</param>
        /// <param name="width">文本框宽度</param>
        /// <param name="height">文本框高度</param>
        /// <param name="strPicPath">图片路径</param>
        /// <param name="picWidth">图片宽度</param>
        /// <param name="picHeight">图片高度</param>
        public void AddTextbox(float left,
                                float top,
                                float width,
                                float height,
                                string strPicPath, float picWidth, float picHeight)
        {
            //see http://msdn.microsoft.com/zh-cn/library/microsoft.office.interop.word.shape_members(v=office.11).aspx
            //see http://msdn.microsoft.com/en-us/library/2a9dt54a.aspx
            #region 勿删，保留信息
            //int count = oWord.Documents.Count;            
            //int count1 = oDoc.Words.Count;
            //oDoc.Sentences[2].Select();     //激活当前的文档中的第二页
            //Microsoft.Office.Interop.Word.WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
            //int num = oDoc.ComputeStatistics(stat, ref missing);    //当前的文档中的页数
            #endregion
            Shape textbox = oDoc.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, left, top, width, height);
            textbox.RelativeHorizontalPosition = WdRelativeHorizontalPosition.wdRelativeHorizontalPositionPage;
            textbox.RelativeVerticalPosition = WdRelativeVerticalPosition.wdRelativeVerticalPositionPage;
            textbox.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse; // this will hide the border or line of textbox. 
            textbox.Select();
            InsertImage(strPicPath, picWidth, picHeight);
        }
        #endregion
    }
}
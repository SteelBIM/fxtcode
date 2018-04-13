
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.BLL.ImportTool;
using Kingsun.ExamPaper.Common;
using NPOI.HSSF.UserModel;

namespace Kingsun.ExamPaper.BLL.ImportTool
{
    public class ImportQuestion : BaseManagement
    {
        /// <summary>
        /// 根据excel路径导入题目
        /// </summary>
        /// <param name="path"></param>
        public void LoadExcelByPath(Stream stream,string path, int bookId, string bookName, int modEditionId, string localEdition, int gradeID, int bookReel)
        {
            IWorkbook hssfworkbook;
            #region 解析文件名，获取版本、教材、年级和册别
            string fileName = Path.GetFileName(path);
            string catalogname = "期末考试卷";//单元目录
            #endregion
            hssfworkbook = new XSSFWorkbook(stream);
            ProcessData(hssfworkbook, fileName, modEditionId, localEdition, bookName, gradeID, bookReel, catalogname, bookId);
        }
        /// <summary>
        /// 正式处理Excel数据
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="modEditionId">MOD版本名称(eg:人教PEP版)</param>
        /// <param name="path">本地文件路径(eg:人教PEP版/53/天天练/3B/U1/******.xls)</param>
        /// <param name="localEdition">本库版本(eg:人教PEP版_53)</param>
        /// <param name="bookName">教材(eg:天天练)</param>
        /// <param name="gradeid">年级(eg:3)</param>
        /// <param name="bookreel">册别(eg:2)</param>
        /// <param name="catalogname">单元目录(eg:U1_达标作业  ||  期中)</param>
        private void ProcessData(IWorkbook hssfworkbook, string path, int modEditionId, string localEdition, string bookName, int gradeid, int bookreel, string catalogname, int bookId)
        {
            string[] arrDirectory = path.Split('|');

            IList<CatalogQuestion> listCatalogQues = new List<CatalogQuestion>()
            {
                 new CatalogQuestion{ CatalogName="期末考试卷", CatalogID=0, ParentCatalogNo=0, CatalogNo=1, Mp3Url=""}//父级目录(即单元目录),只有一个,子级目录为期末考试卷1,期末考试卷2, ...
            };
            List<Question> listQues = new List<Question>();

            #region 读取目录
            int parentCatalogNo = 0;
            //处理非单元目录,即二级目录
            //第一页是参照题型,跳过,i从1开始.
            for (int i = 1; i < hssfworkbook.NumberOfSheets; i++)
            {
                ISheet currentSheet = hssfworkbook.GetSheetAt(i);
                CatalogQuestion cq = new CatalogQuestion();
                cq.QuestionList = new List<Question>();
                cq.CatalogID = 0;
                cq.CatalogNo = i + 1;
                cq.CatalogName = currentSheet.SheetName;//sheet名作为目录名
                cq.ParentCatalogNo = 1;
                cq.QuestionList = new List<Question>();
                cq.Mp3Url = "";
                listCatalogQues.Add(cq);
            }
            if (listCatalogQues.Count == 0)
            {
                throw new Exception("没有需要导入的单元目录，请检查是否填写表格!");
            }
            #endregion

            #region 读取题目
            //获取题目页
            List<ISheet> sheets = new List<ISheet>();
            for (int i = 1; i < hssfworkbook.NumberOfSheets; i++)
            {//第一页sheet是题型参考,不作处理
                ISheet questionSheet = hssfworkbook.GetSheetAt(i);
                IEnumerator questionRows = questionSheet.GetRowEnumerator();
                IRow currentQuestionRow;
                while (questionRows.MoveNext())
                {
                    currentQuestionRow = (XSSFRow)questionRows.Current;
                    #region 读取题目校验
                    //前四列均为空，则读取结束
                    if (CheckEmptyCell(currentQuestionRow.GetCell(0)) && CheckEmptyCell(currentQuestionRow.GetCell(1)) && CheckEmptyCell(currentQuestionRow.GetCell(2)) && CheckEmptyCell(currentQuestionRow.GetCell(3)))
                    {
                        break;
                    }
                    //第一行，则跳过
                    if (currentQuestionRow.GetCell(0).ToString().Trim() == "大题号")
                    {
                        //校验列名和顺序是否正确，不满足则终止导入
                        if (currentQuestionRow.GetCell(1).ToString().Trim() != "类型" || currentQuestionRow.GetCell(2).ToString().Trim() != "分数"
                            || currentQuestionRow.GetCell(3).ToString().Trim() != "小题号" || currentQuestionRow.GetCell(4).ToString().Trim() != "标题文字"
                            || currentQuestionRow.GetCell(5).ToString().Trim() != "标题内容" || currentQuestionRow.GetCell(6).ToString().Trim() != "声音命名"
                            || currentQuestionRow.GetCell(7).ToString().Trim() != "图片命名" || currentQuestionRow.GetCell(8).ToString().Trim() != "答案"
                            || currentQuestionRow.GetCell(9).ToString().Trim() != "选项")
                        {
                            throw new Exception("测评卷_" + questionSheet.SheetName + "，请检查Excel列名及顺序是否正确(格式：大题号|类型|分数|小题号|标题文字|标题内容|声音命名|图片命名|答案|选项)");
                        }
                        continue;
                    }
                    //前四列为必填项，不满足则终止导入
                    if (CheckEmptyCell(currentQuestionRow.GetCell(0)) || CheckEmptyCell(currentQuestionRow.GetCell(1)) || CheckEmptyCell(currentQuestionRow.GetCell(2)) || CheckEmptyCell(currentQuestionRow.GetCell(3)))
                    {
                        throw new Exception("测评卷_" + questionSheet.SheetName + "，前四列为必填项(必填项：大题号|类型|分数|小题号)");
                    }
                    #endregion

                    #region 正式读取题目
                    Question q = new Question();
                    try
                    {
                        string qTitle = "";                        
                        q.QuestionModel = currentQuestionRow.GetCell(1).ToString().Trim();
                        //由于当前页的目录名就是sheetName,故取当前试题所在目录CatalogNo
                        q.CatalogNo = listCatalogQues.Where(o => o.CatalogName == questionSheet.SheetName).FirstOrDefault().CatalogNo;
                        q.SecondContent = "";
                        //判断是否为大题
                        if (currentQuestionRow.GetCell(3).ToString() == "0")
                        {
                            //大题
                            q.QuestionNo = Convert.ToInt32(currentQuestionRow.GetCell(0).ToString().Trim());
                            q.Score = Convert.ToDecimal(currentQuestionRow.GetCell(2).ToString().Trim());
                            q.ParentNo = 0;
                            q.QuestionTitle = currentQuestionRow.GetCell(4).ToString().Trim();
                            qTitle = q.QuestionTitle;
                            q.QuestionContent = currentQuestionRow.GetCell(5) == null ? "" : currentQuestionRow.GetCell(5).ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
                        }
                        else
                        {
                            //小题
                            q.QuestionNo = Convert.ToInt32(currentQuestionRow.GetCell(3).ToString().Trim());
                            q.ParentNo = Convert.ToInt32(currentQuestionRow.GetCell(0).ToString().Trim());
                            q.QuestionTitle = qTitle;
                            q.Score = Convert.ToDecimal(currentQuestionRow.GetCell(2).ToString().Trim());

                            if (q.QuestionModel == "M26")
                            {
                                //当为连线题时，特殊处理
                                q.QuestionContent = questionSheet.SheetName + "/img/" + currentQuestionRow.GetCell(4).ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
                                q.SecondContent = questionSheet.SheetName + "/img/" + currentQuestionRow.GetCell(5).ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
                            }
                            else
                            {
                                if (!CheckEmptyCell(currentQuestionRow.GetCell(4)))
                                {
                                    q.QuestionContent = currentQuestionRow.GetCell(4).ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
                                }
                                else
                                {
                                    q.QuestionContent = "";
                                }
                            }                            
                        }
                        //上传音频
                        if (!CheckEmptyCell(currentQuestionRow.GetCell(6)))
                        {
                            q.Mp3Url = currentQuestionRow.GetCell(6).ToString().Trim();
                            if (!q.Mp3Url.EndsWith(".mp3"))
                            {
                                throw new Exception("测评卷_" + questionSheet.SheetName + "大题号_" + q.QuestionNo.ToString() + "&小题号_" + q.ParentNo.ToString() + "：音频后缀错误，请检查！");
                            }
                            q.Mp3Url = questionSheet.SheetName + "/mp3/" + q.Mp3Url;
                        }
                        else
                        {
                            q.Mp3Url = "";
                        }
                        //上传图片
                        if (!CheckEmptyCell(currentQuestionRow.GetCell(7)))
                        {
                            q.ImgUrl = currentQuestionRow.GetCell(7).ToString().Trim();
                            if (!q.ImgUrl.EndsWith(".jpg") && !q.ImgUrl.EndsWith(".png"))
                            {

                                throw new Exception("测评卷_" + questionSheet.SheetName + "大题号_" + q.QuestionNo.ToString() + "&小题号_" + q.ParentNo.ToString() + "：图片后缀错误，请检查！");
                            }
                            q.ImgUrl = questionSheet.SheetName + "/img/" + q.ImgUrl;
                        }
                        else
                        {
                            q.ImgUrl = "";
                        }

                        //填空题答案
                        q.BlankAnswer = currentQuestionRow.GetCell(8) != null ? currentQuestionRow.GetCell(8).ToString().Trim().Replace("‘", "'").Replace("’", "'") : "";
                        if (q.QuestionModel == "M27" && !string.IsNullOrEmpty(q.BlankAnswer))
                        {
                            if (!q.BlankAnswer.EndsWith(".jpg") && !q.BlankAnswer.EndsWith(".png"))
                            {
                                throw new Exception("测评卷_" + questionSheet.SheetName + "大题号_" + q.QuestionNo.ToString() + "&小题号_" + q.ParentNo.ToString() + "：答案图片后缀错误，请检查！");
                            }
                            q.BlankAnswer = questionSheet.SheetName + "/img/" + q.BlankAnswer;
                        }
                        //选择题选项
                        q.SelectList = new List<SelItem>();
                        ICell select1 = currentQuestionRow.GetCell(9);
                        ICell select2 = currentQuestionRow.GetCell(10);
                        ICell select3 = currentQuestionRow.GetCell(11);
                        ICell select4 = currentQuestionRow.GetCell(12);
                        if (!CheckEmptyCell(select1))
                        {
                            q.SelectList.Add(GetSelItem(hssfworkbook, select1, 1, arrDirectory, questionSheet.SheetName));
                        }
                        if (!CheckEmptyCell(select2))
                        {
                            q.SelectList.Add(GetSelItem(hssfworkbook, select2, 2, arrDirectory, questionSheet.SheetName));
                        }
                        if (!CheckEmptyCell(select3))
                        {
                            q.SelectList.Add(GetSelItem(hssfworkbook, select3, 3, arrDirectory, questionSheet.SheetName));
                        }
                        if (!CheckEmptyCell(select4))
                        {
                            q.SelectList.Add(GetSelItem(hssfworkbook, select4, 4, arrDirectory, questionSheet.SheetName));
                        }
                        if (currentQuestionRow.GetCell(3).ToString() != "0" && string.IsNullOrEmpty(q.BlankAnswer) && q.SelectList.Count == 0)
                        {
                            throw new Exception("测评卷_" + questionSheet.SheetName + "大题号_" + q.QuestionNo.ToString() + "&小题号_" + q.ParentNo.ToString() + "：没有填写“答案”或“选项”，请检查！");
                        }
                        listQues.Add(q);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("测评卷_" + questionSheet.SheetName + "大题号_" + q.QuestionNo.ToString() + "&小题号_" + q.ParentNo.ToString() + "：" + ex.ToString());
                    }                    
                    #endregion                    
                }
            }
            if (listQues.Count == 0)
            {
                throw new Exception("没有需要导入的题目，请检查是否填写表格！");
            }

            #endregion

            #region 导入单元目录
            CatalogBLL catalogBLL = new CatalogBLL();
            int parentCatalogID = 0;
            parentCatalogNo = 0;
            StringBuilder sbDeleteCata = new StringBuilder();
            //先清空该书本下的目录
            //sbDeleteCata.AppendFormat("delete from QTb_Catalog where BookID={0}", bookId);
            //ExcuteSqlWithTran(sbDeleteCata.ToString());
            #region 循环单元目录
            for (int i = 0; i < listCatalogQues.Count; i++)
            {
                if (listCatalogQues[i].ParentCatalogNo == 0)
                {
                    parentCatalogNo = listCatalogQues[i].CatalogNo;
                    parentCatalogID = catalogBLL.ImportCatalogInfo(localEdition, bookName, gradeid, bookreel, listCatalogQues[i].CatalogName, 1, listCatalogQues[i].PageNo, 0, i + 1, listCatalogQues[i].Mp3Url, modEditionId, bookId);
                    if (parentCatalogID == 0)
                    {
                        throw new Exception("目录：“" + listCatalogQues[i].CatalogName + "”导入失败");
                    }
                    else
                    {
                        listCatalogQues[i].CatalogID = parentCatalogID;
                        listCatalogQues[i].QuestionList = listQues.Where(s => s.CatalogNo == listCatalogQues[i].CatalogNo).ToList();
                    }
                }
                else if (listCatalogQues[i].ParentCatalogNo == parentCatalogNo)
                {
                    listCatalogQues[i].CatalogID = catalogBLL.ImportCatalogInfo(localEdition, bookName, gradeid, bookreel, listCatalogQues[i].CatalogName, 2, listCatalogQues[i].PageNo, parentCatalogID, i + 1, listCatalogQues[i].Mp3Url, modEditionId, bookId);
                    if (listCatalogQues[i].CatalogID == 0)
                    {
                        throw new Exception("目录：“" + listCatalogQues[i].CatalogName + "”导入失败");
                    }
                    else
                    {
                        listCatalogQues[i].QuestionList = listQues.Where(s => s.CatalogNo == listCatalogQues[i].CatalogNo).ToList();
                    }
                }
            }
            #endregion

            #endregion

            #region 使用事务导入题目
            StringBuilder sbDelCatalog = new StringBuilder();
            StringBuilder sbQues = new StringBuilder();
            StringBuilder sbAnswer = new StringBuilder();
            StringBuilder sbSelect = new StringBuilder();
            int parentNo = 0;
            string parentQID = "", questionID = "", parentTitle = "", questionModel = "";
            int mqCount = 0, sort = 0;
            for (int i = 0; i < listCatalogQues.Count; i++)
            {
                if (listCatalogQues[i].QuestionList != null && listCatalogQues[i].QuestionList.Count > 0)
                {
                    //导入前先清空该目录下的题目相关数据
                    sbDelCatalog.Append(string.Format(@"
                        delete from Tb_StuCatalog where CatalogID={0};
                        delete from Tb_BlankAnswer where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});
                        delete from Tb_SelectItem where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});
                        delete from Tb_QuestionInfo where CatalogID={0};", listCatalogQues[i].CatalogID));
                    //  sbDelCatalog.Append(string.Format(@"
                    //      delete from Tb_StuAnswer where CatalogID={0};
                    //      delete from Tb_StuCatalog where CatalogID={0};
                    //      delete from Tb_BlankAnswer where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});
                    //      delete from Tb_SelectItem where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});
                    //      delete from Tb_QuestionInfo where CatalogID={0};", listCatalogQues[i].CatalogID));

                    for (int j = 0; j < listCatalogQues[i].QuestionList.Count; j++)
                    {
                        sort++;
                        Question qi = listCatalogQues[i].QuestionList[j];
                        if (qi.ParentNo == 0)
                        {
                            //添加大题
                            if (mqCount > 0)
                            {
                                //赋值小题数字段
                                sbQues.Replace("[MQCount]", mqCount.ToString());
                            }
                            mqCount = 0;
                            parentNo = qi.QuestionNo;
                            parentTitle = qi.QuestionTitle;
                            questionModel = qi.QuestionModel;
                            parentQID = Guid.NewGuid().ToString();
                            sbQues.Append(string.Format(" union all  select '{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',[MQCount],'{11}' ",
                                parentQID, qi.QuestionTitle.Replace("'", "''"), qi.QuestionContent.Replace("'", "''"), qi.SecondContent.Replace("'", "''"), qi.Mp3Url, qi.ImgUrl, "NULL", questionModel,
                                listCatalogQues[i].CatalogID, sort, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), qi.Score));
                        }
                        else
                        {
                            //添加小题
                            mqCount++;
                            questionID = Guid.NewGuid().ToString();
                            sbQues.Append(string.Format(" union all  select '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},'{10}',[MQCount],'{11}' ",
                                questionID, parentTitle.Replace("'", "''"), qi.QuestionContent.Replace("'", "''"), qi.SecondContent.Replace("'", "''"), qi.Mp3Url, qi.ImgUrl, parentQID, questionModel,
                                listCatalogQues[i].CatalogID, sort, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), qi.Score));
                            if (!string.IsNullOrEmpty(qi.BlankAnswer))
                            {
                                //添加填空题答案
                                sbAnswer.Append(string.Format(" union all  select '{0}','{1}',{2} ", questionID, qi.BlankAnswer.Replace("'", "''"), 1));
                            }
                            if (qi.SelectList.Count > 0)
                            {
                                //添加选择题答案
                                for (int k = 0; k < qi.SelectList.Count; k++)
                                {
                                    sbSelect.Append(string.Format(" union all  select '{0}','{1}','{2}',{3},{4} ", questionID, qi.SelectList[k].SelectItem.Replace("'", "''"), qi.SelectList[k].ImgUrl, k + 1, qi.SelectList[k].IsAnswer));
                                }
                            }
                        }
                    }
                }
            }
            sbQues.Replace("[MQCount]", mqCount.ToString());
            StringBuilder sb = new StringBuilder();
            if (sbDelCatalog.Length > 0)
            {
                sb.Append(sbDelCatalog.ToString());
            }
            if (sbQues.Length > 0)
            {
                sb.Append("insert into Tb_QuestionInfo(QuestionID,QuestionTitle,QuestionContent,SecondContent,Mp3Url,ImgUrl,ParentID,QuestionModel,CatalogID,Sort,CreateDate,MinQueCount,Score) " + sbQues.ToString().Substring(10));
            }
            if (sbAnswer.Length > 0)
            {
                sb.Append(" ;insert into Tb_BlankAnswer(QuestionID,Answer,AnswerType) " + sbAnswer.ToString().Substring(10));
            }
            if (sbSelect.Length > 0)
            {
                sb.Append(" ;insert into Tb_SelectItem(QuestionID,SelectItem,ImgUrl,Sort,IsAnswer) " + sbSelect.ToString().Substring(10));
            }

            System.IO.StreamWriter sw = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
            sw.WriteLine(string.Format("开始导入期末测评卷时间={0}，bookName={1}\r\n{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bookName, sb.ToString()));
            sw.Flush();
            sw.Close();
            try
            {
                if (ExcuteSqlWithTran(sb.ToString()))
                {
                    System.IO.StreamWriter sw2 = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
                    sw2.WriteLine(string.Format("结束导入期末测评卷时间={0}，bookName={1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bookName));
                    sw2.Flush();
                    sw2.Close();
                    return;
                }
                else
                {
                    throw new Exception("执行sql时候出错!");
                }
            }
            catch(Exception ex)
            {
                System.IO.StreamWriter sw1 = System.IO.File.AppendText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Upload\\log.txt");
                sw1.WriteLine(string.Format("导入期末测评卷异常时间={0}，bookName={1}，{2}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bookName, ex.ToString()));
                sw1.Flush();
                sw1.Close();
            }
            
            #endregion
        }

        #region 获取选择题选项
        /// <summary>
        /// 获取选择题选项
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="cell">单元格</param>
        /// <param name="index">选项序号</param>
        /// <param name="arrDir">本地文件路径</param>
        /// <returns></returns>
        private SelItem GetSelItem(IWorkbook hssfworkbook, ICell cell, int index, string[] arrDir,string sheetName)
        {
            SelItem si = new SelItem();
            ICellStyle cstyle = cell.CellStyle;
            IFont font = hssfworkbook.GetFontAt(cstyle.FontIndex);
            XSSFFont f = (XSSFFont)font;
            XSSFColor xcolor = f.GetXSSFColor();
            si.IsAnswer = 0;
            if (xcolor != null)
            {
                byte[] c = xcolor.GetARgb();
                if (c != null & c[1] == 255 && c[2] == 0)
                {
                    si.IsAnswer = 1;
                }
                else
                {
                    si.IsAnswer = 0;
                }
            }
            si.SelectItem = "";
            if (cell.ToString().IndexOf(".jpg") > -1 || cell.ToString().IndexOf(".png") > -1)
            {
                si.ImgUrl = sheetName + "/img/" + cell.ToString().Trim();
            }
            else
            {
                si.SelectItem = cell.ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
            }
            return si;
        }
        #endregion

        #region 校验列是否为空
        /// <summary>
        /// 校验指定列是否为空
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static bool CheckEmptyCell(ICell cell)
        {
            if (cell == null || string.IsNullOrEmpty(cell.ToString().Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 处理册别
        /// <summary>
        /// 转换册别[字母--数字] 上册 A=1  下册 B=2  全册 C=3   0=未知册别
        /// </summary>
        /// <param name="BookReelStr"></param>
        /// <returns></returns>
        public int BookReelToInt(string BookReelStr)
        {
            int bookReelInt = 0;
            switch (BookReelStr.ToUpper())
            {
                case "A":
                    bookReelInt = 1;
                    break;
                case "B":
                    bookReelInt = 2;
                    break;
                case "C":
                    bookReelInt = 3;
                    break;
                default:
                    bookReelInt = 0;
                    break;
            }
            return bookReelInt;
        }
        #endregion
    }
}

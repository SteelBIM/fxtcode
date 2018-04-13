using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kingsun.ExamPaper.ImportTool
{
    public class ImportQuestion : BaseManagement
    {
        /// <summary>
        /// 根据excel路径导入题目
        /// </summary>
        /// <param name="path"></param>
        public void LoadExcelByPath(string path)
        {

            IWorkbook hssfworkbook;
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            #region 解析文件名，获取版本、教材、年级和册别
            string fileName = "";
            int startindex = path.LastIndexOf('\\');
            int endindex = path.LastIndexOf('.');
            //文件名：人教PEP版_53_天天练_3B_期中_题目导入模板  /  人教PEP版_53_天天练_3B_U1_达标作业_题目导入模板
            fileName = path.Substring(startindex + 1, endindex - startindex);
            string[] paramstitle = fileName.Split('_');
            if (paramstitle.Length < 6 || paramstitle.Length >7)
            {
                CacheHelper.Instance.Add("ReadErrorMsg", "导入模板命名不规范(格式1：人教PEP版_53_天天练_3B_期中_题目导入模板 || 格式2：人教PEP版_53_天天练_3B_U1_达标作业_题目导入模板)");
                return;
            }
            string modEdition = paramstitle[0];//MOD版本
            string localEdition = paramstitle[0] + "_" + paramstitle[1];//本库版本
            string bookName = paramstitle[2];//教材
            int gradeID = int.Parse(paramstitle[3].Substring(0, 1)) + 1;//年级
            int bookReel = BookReelToInt(paramstitle[3].Substring(1, 1));//册别
            string catalogname = paramstitle[4] + (paramstitle.Length == 7 ? ("_" + paramstitle[5]) : "");//单元目录
            #endregion

            try
            {
                //读取Excel文件
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new XSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                CacheHelper.Instance.Add("ReadErrorMsg", e.Message);
                return;
            }

            ProcessData(hssfworkbook, path, modEdition, localEdition, bookName, gradeID, bookReel, catalogname);

        }
        /// <summary>
        /// 正式处理Excel数据
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="modEdition">MOD版本名称(eg:人教PEP版)</param>
        /// <param name="path">本地文件路径(eg:人教PEP版/53/天天练/3B/U1/******.xls)</param>
        /// <param name="localEdition">本库版本(eg:人教PEP版_53)</param>
        /// <param name="bookName">教材(eg:天天练)</param>
        /// <param name="gradeid">年级(eg:3)</param>
        /// <param name="bookreel">册别(eg:2)</param>
        /// <param name="catalogname">单元目录(eg:U1_达标作业  ||  期中)</param>
        private void ProcessData(IWorkbook hssfworkbook,string path,string modEdition,string localEdition,string bookName,int gradeid,int bookreel,string catalogname)
        {
            string filepath = path.Substring(0, path.LastIndexOf("\\")) + "|" + localEdition + "_" + (gradeid - 1).ToString()
                + (bookreel == 1 ? "A" : (bookreel == 2 ? "B" : (bookreel == 3 ? "C" : ""))) + "_" + bookName + "_" + catalogname + "_";
            string[] arrDirectory = filepath.Split('|');

            IList<CatalogQuestion> listCatalogQues = new List<CatalogQuestion>();
            List<Question> listQues = new List<Question>();

            #region 读取目录
            //获取单元目录页
            ISheet catalogSheet = hssfworkbook.GetSheetAt(1);
            IEnumerator catalogRows = catalogSheet.GetRowEnumerator();
            IRow currentCatalogRow;
            int parentCatalogNo = 0;
            while (catalogRows.MoveNext())
            {
                currentCatalogRow = (XSSFRow)catalogRows.Current;
                //第一列为空，则读取结束
                if (CheckEmptyCell(currentCatalogRow.GetCell(0)))
                {
                    break;
                }
                //第一行，则跳过
                if (currentCatalogRow.GetCell(0).ToString().Trim() == "目录编号")
                {
                    //校验列名和顺序是否正确，不满足则终止导入
                    if ((currentCatalogRow.Cells.Count == 5 &&
                        (currentCatalogRow.GetCell(1).ToString().Trim() != "单元标题" || currentCatalogRow.GetCell(2).ToString().Trim() != "目录标题"
                        || currentCatalogRow.GetCell(3).ToString().Trim() != "页码" || currentCatalogRow.GetCell(4).ToString().Trim() != "音频"))
                        || (currentCatalogRow.Cells.Count == 4 &&
                        (currentCatalogRow.GetCell(1).ToString().Trim() != "单元标题" || currentCatalogRow.GetCell(2).ToString().Trim() != "目录标题"
                        || currentCatalogRow.GetCell(3).ToString().Trim() != "页码")))
                    {
                        CacheHelper.Instance.Add("DataErrorMsg", "请检查Excel列名及顺序是否正确(格式：目录编号|单元标题|目录标题|页码(可选列：音频)");
                        listCatalogQues.Clear();
                        return;
                    }
                    continue;
                }
                CatalogQuestion cq = new CatalogQuestion();
                cq.CatalogID = 0;
                cq.CatalogNo = Convert.ToInt32(currentCatalogRow.GetCell(0).ToString().Trim());
                if ((!CheckEmptyCell(currentCatalogRow.GetCell(1)) && !CheckEmptyCell(currentCatalogRow.GetCell(2))) || (CheckEmptyCell(currentCatalogRow.GetCell(1)) && CheckEmptyCell(currentCatalogRow.GetCell(2))))
                {
                    CacheHelper.Instance.Add("DataErrorMsg", "单元标题和目录标题不可同时填写，也不可均为空");
                    listCatalogQues.Clear();
                    return;
                }
                else if (!CheckEmptyCell(currentCatalogRow.GetCell(1)))
                {
                    cq.CatalogName = currentCatalogRow.GetCell(1).ToString().Trim();
                    cq.ParentCatalogNo = 0;
                    parentCatalogNo = cq.CatalogNo;
                }
                else
                {
                    cq.CatalogName = currentCatalogRow.GetCell(2).ToString().Trim();
                    cq.ParentCatalogNo = parentCatalogNo;
                }
                if (!CheckEmptyCell(currentCatalogRow.GetCell(3)))
                {
                    cq.PageNo = Convert.ToInt32(currentCatalogRow.GetCell(3).ToString().Trim());
                }
                else
                {
                    cq.PageNo = 0;
                }
                if (!CheckEmptyCell(currentCatalogRow.GetCell(4)))
                {
                    cq.Mp3Url = currentCatalogRow.GetCell(4).ToString();
                    if (!cq.Mp3Url.EndsWith(".mp3"))
                    {
                        CacheHelper.Instance.Add("DataErrorMsg", "单元目录编号_" + cq.CatalogNo.ToString() + "：音频后缀错误，请检查！");
                        listQues.Clear();
                        return;
                    }
                    cq.Mp3Url = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\mp3\\" + cq.Mp3Url);
                }
                else
                {
                    cq.Mp3Url = "";
                }
                cq.QuestionList = new List<Question>();
                listCatalogQues.Add(cq);
            }
            if (listCatalogQues.Count == 0)
            {
                CacheHelper.Instance.Add("DataErrorMsg", "没有需要导入的单元目录，请检查是否填写表格！");
                return;
            }
            #endregion

            #region 读取题目
            
            //获取题目页
            ISheet questionSheet = hssfworkbook.GetSheetAt(0);
            IEnumerator questionRows = questionSheet.GetRowEnumerator();
            IRow currentQuestionRow;
            while (questionRows.MoveNext())
            {
                currentQuestionRow = (XSSFRow)questionRows.Current;

                #region 读取题目校验
                //前四列均为空，则读取结束
                if (CheckEmptyCell(currentQuestionRow.GetCell(0))&&CheckEmptyCell(currentQuestionRow.GetCell(1))&&CheckEmptyCell(currentQuestionRow.GetCell(2))&&CheckEmptyCell(currentQuestionRow.GetCell(3)))
                {
                    break;
                }
                //第一行，则跳过
                if (currentQuestionRow.GetCell(0).ToString().Trim() == "大题号")
                {
                    //校验列名和顺序是否正确，不满足则终止导入
                    if (currentQuestionRow.GetCell(1).ToString().Trim() != "类型" || currentQuestionRow.GetCell(2).ToString().Trim() != "目录编号"
                        || currentQuestionRow.GetCell(3).ToString().Trim() != "小题号" || currentQuestionRow.GetCell(4).ToString().Trim() != "标题文字"
                        || currentQuestionRow.GetCell(5).ToString().Trim() != "标题内容" || currentQuestionRow.GetCell(6).ToString().Trim() != "声音命名"
                        || currentQuestionRow.GetCell(7).ToString().Trim() != "图片命名" || currentQuestionRow.GetCell(8).ToString().Trim() != "答案"
                        || currentQuestionRow.GetCell(9).ToString().Trim() != "选项")
                    {
                        CacheHelper.Instance.Add("DataErrorMsg", "请检查Excel列名及顺序是否正确(格式：大题号|类型|目录编号|小题号|标题文字|标题内容|声音命名|图片命名|答案|选项)");
                        listQues.Clear();
                        return;
                    }
                    continue;
                }
                //前四列为必填项，不满足则终止导入
                if (CheckEmptyCell(currentQuestionRow.GetCell(0)) || CheckEmptyCell(currentQuestionRow.GetCell(1)) || CheckEmptyCell(currentQuestionRow.GetCell(2)) || CheckEmptyCell(currentQuestionRow.GetCell(3)))
                {
                    CacheHelper.Instance.Add("DataErrorMsg", "前四列为必填项(必填项：大题号|类型|目录编号|小题号)");
                    listQues.Clear();
                    return;
                }
                #endregion

                #region 正式读取题目
                string qTitle = "";
                Question q = new Question();
                q.QuestionModel = currentQuestionRow.GetCell(1).ToString().Trim();
                q.CatalogNo = Convert.ToInt32(currentQuestionRow.GetCell(2).ToString().Trim());
                q.SecondContent = "";
                //判断是否为大题
                if (currentQuestionRow.GetCell(3).ToString() == "0")
                {
                    //大题
                    q.QuestionNo = Convert.ToInt32(currentQuestionRow.GetCell(0).ToString().Trim());
                    q.ParentNo = 0;
                    q.QuestionTitle = currentQuestionRow.GetCell(4).ToString().Trim();
                    qTitle = q.QuestionTitle;
                    q.QuestionContent = currentQuestionRow.GetCell(5).ToString().Trim().Replace("‘", "'").Replace("’", "'").Replace("[", "<u>").Replace("]", "</u>");
                }
                else
                {
                    //小题
                    q.QuestionNo = Convert.ToInt32(currentQuestionRow.GetCell(3).ToString().Trim());
                    q.ParentNo = Convert.ToInt32(currentQuestionRow.GetCell(0).ToString().Trim());
                    q.QuestionTitle = qTitle;
                    if (q.QuestionModel == "M26")
                    {
                        //当为连线题时，特殊处理
                        q.QuestionContent = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\img\\" + currentQuestionRow.GetCell(4).ToString().Trim());
                        q.SecondContent = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\img\\" + currentQuestionRow.GetCell(5).ToString().Trim());
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
                        CacheHelper.Instance.Add("DataErrorMsg", "大题号_" + q.ParentNo.ToString() + "&小题号_" + q.QuestionNo.ToString() + "：音频后缀错误，请检查！");
                        listQues.Clear();
                        return;
                    }
                    q.Mp3Url = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\mp3\\" + q.Mp3Url);
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
                        CacheHelper.Instance.Add("DataErrorMsg", "大题号_" + q.ParentNo.ToString() + "&小题号_" + q.QuestionNo.ToString() + "：图片后缀错误，请检查！");
                        listQues.Clear();
                        return;
                    }
                    q.ImgUrl = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\img\\" + q.ImgUrl);
                }
                else
                {
                    q.ImgUrl = "";
                }
                //填空题答案
                q.BlankAnswer = currentQuestionRow.GetCell(8).ToString().Trim().Replace("‘", "'").Replace("’", "'");
                if (q.QuestionModel == "M27" && !string.IsNullOrEmpty(q.BlankAnswer))
                {
                    if (!q.BlankAnswer.EndsWith(".jpg") && !q.BlankAnswer.EndsWith(".png"))
                    {
                        CacheHelper.Instance.Add("DataErrorMsg", "大题号_" + q.ParentNo.ToString() + "&小题号_" + q.QuestionNo.ToString() + "：答案图片后缀错误，请检查！");
                        listQues.Clear();
                        return;
                    }
                    q.BlankAnswer = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDirectory[1], arrDirectory[0] + "\\img\\" + q.BlankAnswer);
                }
                //选择题选项
                q.SelectList = new List<SelItem>();
                ICell select1 = currentQuestionRow.GetCell(9);
                ICell select2 = currentQuestionRow.GetCell(10);
                ICell select3 = currentQuestionRow.GetCell(11);
                ICell select4 = currentQuestionRow.GetCell(12);
                if (!CheckEmptyCell(select1))
                {
                    q.SelectList.Add(GetSelItem(hssfworkbook, select1, 1, arrDirectory));
                }
                if (!CheckEmptyCell(select2))
                {
                    q.SelectList.Add(GetSelItem(hssfworkbook, select2, 2, arrDirectory));
                }
                if (!CheckEmptyCell(select3))
                {
                    q.SelectList.Add(GetSelItem(hssfworkbook, select3, 3, arrDirectory));
                }
                if (!CheckEmptyCell(select4))
                {
                    q.SelectList.Add(GetSelItem(hssfworkbook, select4, 4, arrDirectory));
                }
                if (currentQuestionRow.GetCell(3).ToString() != "0" && string.IsNullOrEmpty(q.BlankAnswer) && q.SelectList.Count == 0)
                {
                    CacheHelper.Instance.Add("DataErrorMsg", "大题号_" + q.ParentNo.ToString() + "&小题号_" + q.QuestionNo.ToString() + "：没有填写“答案”或“选项”，请检查！");
                    listQues.Clear();
                    return;
                }
                #endregion

                listQues.Add(q);
            }
            if (listQues.Count == 0)
            {
                CacheHelper.Instance.Add("DataErrorMsg", "没有需要导入的题目，请检查是否填写表格！");
                return;
            }
            #endregion

            if (AppSetting.OnlyCheck)
            {
                return;
            }

            #region 导入单元目录
            CatalogBLL catalogBLL = new CatalogBLL();
            int parentCatalogID = 0;
            parentCatalogNo = 0;
            for (int i = 0; i < listCatalogQues.Count; i++)
            {
                if (listCatalogQues[i].ParentCatalogNo == 0)
                {
                    parentCatalogNo = listCatalogQues[i].CatalogNo;
                    parentCatalogID = catalogBLL.ImportCatalogInfo(localEdition, bookName, gradeid, bookreel, listCatalogQues[i].CatalogName, 1, listCatalogQues[i].PageNo, 0, i + 1,listCatalogQues[i].Mp3Url);
                    if (parentCatalogID == 0)
                    {
                        CacheHelper.Instance.Add("UnitErrorMsg", "目录：“" + listCatalogQues[i].CatalogName + "”导入失败");
                        listCatalogQues.Clear();
                        listQues.Clear();
                        return;
                    }
                    else
                    {
                        listCatalogQues[i].CatalogID = parentCatalogID;
                        listCatalogQues[i].QuestionList = listQues.Where(s => s.CatalogNo == listCatalogQues[i].CatalogNo).ToList();
                    }
                }
                else if (listCatalogQues[i].ParentCatalogNo == parentCatalogNo)
                {
                    listCatalogQues[i].CatalogID = catalogBLL.ImportCatalogInfo(localEdition, bookName, gradeid, bookreel, listCatalogQues[i].CatalogName, 2, listCatalogQues[i].PageNo, parentCatalogID, i + 1, listCatalogQues[i].Mp3Url);
                    if (listCatalogQues[i].CatalogID == 0)
                    {
                        CacheHelper.Instance.Add("UnitErrorMsg", "目录：“" + listCatalogQues[i].CatalogName + "”导入失败");
                        listCatalogQues.Clear();
                        listQues.Clear();
                        return;
                    }
                    else
                    {
                        listCatalogQues[i].QuestionList = listQues.Where(s => s.CatalogNo == listCatalogQues[i].CatalogNo).ToList();
                    }
                }
            }
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
                    sbDelCatalog.Append(string.Format("delete from Tb_StuAnswer where CatalogID={0};delete from Tb_StuCatalog where CatalogID={0};"
                     + "delete from Tb_BlankAnswer where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});"
                     + "delete from Tb_SelectItem where QuestionID in(select QuestionID from Tb_QuestionInfo where CatalogID={0});"
                     + "delete from Tb_QuestionInfo where CatalogID={0};", listCatalogQues[i].CatalogID));
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
                            sbQues.Append(string.Format(",('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},{9},'{10}',[MQCount])",
                                parentQID, qi.QuestionTitle.Replace("'", "''"), qi.QuestionContent.Replace("'", "''"), qi.SecondContent.Replace("'", "''"), qi.Mp3Url, qi.ImgUrl, "NULL", questionModel,
                                listCatalogQues[i].CatalogID, sort, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                        else
                        {
                            //添加小题
                            mqCount++;
                            questionID = Guid.NewGuid().ToString();
                            sbQues.Append(string.Format(",('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},'{10}',[MQCount])",
                                questionID, parentTitle.Replace("'", "''"), qi.QuestionContent.Replace("'", "''"), qi.SecondContent.Replace("'", "''"), qi.Mp3Url, qi.ImgUrl, parentQID, questionModel,
                                listCatalogQues[i].CatalogID, sort, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                            if (!string.IsNullOrEmpty(qi.BlankAnswer))
                            {
                                //添加填空题答案
                                sbAnswer.Append(string.Format(",('{0}','{1}',{2})", questionID, qi.BlankAnswer.Replace("'", "''"), 1));
                            }
                            if (qi.SelectList.Count > 0)
                            {
                                //添加选择题答案
                                for (int k = 0; k < qi.SelectList.Count; k++)
                                {
                                    sbSelect.Append(string.Format(",('{0}','{1}','{2}',{3},{4})", questionID, qi.SelectList[k].SelectItem.Replace("'", "''"), qi.SelectList[k].ImgUrl, k + 1, qi.SelectList[k].IsAnswer));
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
                sb.Append("insert into Tb_QuestionInfo(QuestionID,QuestionTitle,QuestionContent,SecondContent,Mp3Url,ImgUrl,ParentID,QuestionModel,CatalogID,Sort,CreateDate,MinQueCount) values" + sbQues.ToString().Substring(1));
            }
            if (sbAnswer.Length > 0)
            {
                sb.Append(" ;insert into Tb_BlankAnswer(QuestionID,Answer,AnswerType) values" + sbAnswer.ToString().Substring(1));
            }
            if (sbSelect.Length > 0)
            {
                sb.Append(" ;insert into Tb_SelectItem(QuestionID,SelectItem,ImgUrl,Sort,IsAnswer) values" + sbSelect.ToString().Substring(1));
            }
            if (ExcuteSqlWithTran(sb.ToString()))
            {
                return;
            }
            else
            {
                CacheHelper.Instance.Add("QuestionErrorMsg", _operatorError);
                return;
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
        private SelItem GetSelItem(IWorkbook hssfworkbook, ICell cell, int index, string[] arrDir)
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
                si.ImgUrl = UploadFile.UploadFileToPath(AppSetting.UploadFileUrl + "?path=" + arrDir[1], arrDir[0] + "\\img\\" + cell.ToString().Trim());
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

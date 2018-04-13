using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Kingsun.SynchronousStudy.Web.InterestDubbingGame
{
    public partial class DubbingImportExcel : System.Web.UI.Page
    {
        private IWorkbook _workbook;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 故事朗读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportSR_Click(object sender, EventArgs e)
        {
            string newFileName = string.Empty;
            string strName = flStoryReading.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
            if (string.IsNullOrEmpty(strName))
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('请选择文件！');</script>");
                return;
            }
            newFileName = GetNewFileName(strName, newFileName);
            if (string.IsNullOrEmpty(newFileName))
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('" + newFileName + "！');</script>");
                return;
            }
            DataTable dt = ExcelToDataTable("", newFileName, true);

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("Title");
            dtList.Columns.Add("OriginalText");
            dtList.Columns.Add("EvaluationContent");
            dtList.Columns.Add("SerialNumber");
            dtList.Columns.Add("GradeName");
            dtList.Columns.Add("GroupName");

            if (dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["Title"] = dt.Rows[i]["标题"].ToString();
                drlist["OriginalText"] = dt.Rows[i]["原文（有中文）"].ToString();
                drlist["EvaluationContent"] = dt.Rows[i]["评测内容（没有中文）"].ToString();
                drlist["SerialNumber"] = dt.Rows[i]["序号"].ToString();
                drlist["GradeName"] = dt.Rows[i]["所属年级"].ToString();
                drlist["GroupName"] = dt.Rows[i]["所属组别"].ToString();

                dtList.Rows.Add(drlist);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.InterestDubbingGameConnectionStr, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "TRUNCATE TABLE dbo.[QTB_IDG_StoryRead]";
            SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "QTB_IDG_StoryRead";
                sbc.WriteToServer(dtList); //此处报错
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
            }

            if (sbc.NotifyAfter <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('导入成功！');</script>");
                //File.Delete(newFileName);
            }

        }

        /// <summary>
        /// 课本剧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            string newFileName = string.Empty;
            string strName = flStoryReading.PostedFile.FileName; //使用fileupload控件获取上传文件的文件名
            if (string.IsNullOrEmpty(strName))
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('请选择文件！');</script>");
                return;
            }
            newFileName = GetNewFileName(strName, newFileName);
            DataTable dt = new DataTable();
            DataTable dtSheet2 = new DataTable();
            try
            {
                dt = ExcelToDataTable("总视频信息", newFileName, true);
                dtSheet2 = ExcelToDataTable("分视频信息", newFileName, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                throw ex;
            }
            //string path = Server.MapPath("~/Upload/Excel/");//"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\Excel\\" + GetBookName(Convert.ToInt32(Bookid)) + ".xls";
            //string fileName = FileUpload1.FileName;// GetBookName(Convert.ToInt32(Bookid)) + ".xls";


            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("VideoTitle");
            dtList.Columns.Add("VideoNumber");
            dtList.Columns.Add("MuteVideo");
            dtList.Columns.Add("CompleteVideo");
            dtList.Columns.Add("BackgroundAudio");
            dtList.Columns.Add("VideoCover");
            dtList.Columns.Add("VideoDesc");
            dtList.Columns.Add("VideoDifficulty");
            dtList.Columns.Add("GradeName");
            dtList.Columns.Add("GroupName");

            DataTable dtDialogue = new DataTable();
            dtDialogue.Columns.Add("ID");
            dtDialogue.Columns.Add("VideoNumber");
            dtDialogue.Columns.Add("DialogueNumber");
            dtDialogue.Columns.Add("DialogueText");
            dtDialogue.Columns.Add("StartTime");
            dtDialogue.Columns.Add("EndTime");

            if (dt == null && dtSheet2 == null)
            {
                return;
            }
            if (dt.Rows.Count <= 0 || dtSheet2.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["VideoNumber"] = ParseInt(dt.Rows[i]["序号"]);
                drlist["VideoTitle"] = dt.Rows[i]["视频标题"];
                drlist["MuteVideo"] = dt.Rows[i]["静音视频"];
                drlist["CompleteVideo"] = dt.Rows[i]["完整视频"];
                drlist["BackgroundAudio"] = dt.Rows[i]["背景音频"];
                drlist["VideoCover"] = dt.Rows[i]["视频封图"];
                drlist["VideoDesc"] = dt.Rows[i]["视频简介"];
                drlist["VideoDifficulty"] = ParseInt(dt.Rows[i]["难易程度"]);
                drlist["GradeName"] = dt.Rows[i]["所属年级"];
                drlist["GroupName"] = dt.Rows[i]["所属组别"];
                dtList.Rows.Add(drlist);
            }

            for (int i = 0; i < dtSheet2.Rows.Count; i++)
            {
                DataRow drDialogue = dtDialogue.NewRow();
                drDialogue["VideoNumber"] = dtSheet2.Rows[i]["序号"];
                drDialogue["DialogueText"] = dtSheet2.Rows[i]["分视频文本"].ToString();
                drDialogue["DialogueNumber"] = ParseInt(dtSheet2.Rows[i]["分视频序号"]);
                if (dtSheet2.Rows[i]["起始时间"].ToString() != "")
                {
                    if (IsDate(dtSheet2.Rows[i]["起始时间"].ToString()))
                    {
                        drDialogue["StartTime"] = dtSheet2.Rows[i]["起始时间"];
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的开始时间格式错误！');</script>");
                        return;
                    }

                }
                if (dtSheet2.Rows[i]["结束时间"].ToString() != "")
                {
                    if (IsDate(dtSheet2.Rows[i]["结束时间"].ToString()))
                    {
                        drDialogue["EndTime"] = dtSheet2.Rows[i]["结束时间"];
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的结束时间格式错误！');</script>");
                        return;
                    }
                }
                dtDialogue.Rows.Add(drDialogue);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.InterestDubbingGameConnectionStr, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };
            SqlBulkCopy sbc1 = new SqlBulkCopy(SqlHelper.InterestDubbingGameConnectionStr, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };


            string strSql = "TRUNCATE TABLE [dbo].[QTB_IDG_BookDrama] ";
            SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, strSql);
            strSql = "TRUNCATE TABLE [dbo].[QTB_IDG_BookDialogue]";
            SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "[QTB_IDG_BookDrama]";
                sbc.WriteToServer(dtList); //此处报错
                sbc1.DestinationTableName = "QTB_IDG_BookDialogue";
                sbc1.WriteToServer(dtDialogue); //此处报错
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
            }

            if (sbc.NotifyAfter <= 0 || sbc1.NotifyAfter <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入成功！');</script>");
                File.Delete(newFileName);
            }
        }

        private string GetNewFileName(string strName, string newFileName)
        {
            if (strName != "") //如果文件名存在
            {
                bool fileOk = false;
                int i = strName.LastIndexOf(".", StringComparison.Ordinal); //获取。的索引顺序号，在这里。代表文件名字与后缀的间隔
                string kzm = strName.Substring(i);
                //获取文件扩展名的另一种方法 string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string juedui = Server.MapPath("~\\Upload\\Excel\\");
                //设置文件保存的本地目录绝对路径，对于路径中的字符“＼”在字符串中必须以“＼＼”表示，因为“＼”为特殊字符。或者可以使用上一行的给路径前面加上＠
                newFileName = juedui + strName;
                if (flStoryReading.HasFile) //验证 FileUpload 控件确实包含文件
                {
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    foreach (string t in allowedExtensions)
                    {
                        if (kzm == t)
                        {
                            fileOk = true;
                        }
                    }
                }
                if (fileOk)
                {
                    try
                    {
                        // 判定该路径是否存在
                        if (!Directory.Exists(juedui))
                            Directory.CreateDirectory(juedui);
                        flStoryReading.PostedFile.SaveAs(newFileName); //将文件存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('" + StringRep(ex.Message) + "');</script>");
                    }
                }
            }
            return newFileName;
        }


        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="fileName"></param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, string fileName, bool isFirstRowColumn)
        {
            DataTable data = new DataTable();
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    _workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    _workbook = new HSSFWorkbook(fs);

                ISheet sheet;
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = _workbook.GetSheet(sheetName) ?? _workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                        int startRow;
                        if (isFirstRowColumn)
                        {
                            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                            {
                                ICell cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    string cellValue = cell.StringCellValue;
                                    if (cellValue != null)
                                    {
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }
                                }
                            }
                            startRow = sheet.FirstRowNum + 1;
                        }
                        else
                        {
                            startRow = sheet.FirstRowNum;
                        }

                        //最后一列的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null || row.Cells[0].ToString().Trim() == "") continue; //没有数据的行默认是null　　　　　　　

                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < _workbook.NumberOfSheets; c++)
                    {
                        sheet = _workbook.GetSheetAt(c);
                        if (sheet != null)
                        {
                            IRow firstRow = sheet.GetRow(0);
                            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                            int startRow = 1;
                            if (isFirstRowColumn)
                            {
                                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                {
                                    ICell cell = firstRow.GetCell(i);
                                    if (cell != null)
                                    {
                                        string cellValue = cell.StringCellValue;
                                        if (cellValue != null)
                                        {
                                            DataColumn column = new DataColumn(cellValue);
                                            data.Columns.Add(column);
                                        }
                                    }
                                }
                                isFirstRowColumn = false;
                            }


                            //最后一列的标号
                            int rowCount = sheet.LastRowNum;
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                IRow row = sheet.GetRow(i);
                                if (row == null || row.Cells[0].ToString().Trim() == "") continue; //没有数据的行默认是null　　　　　　　

                                DataRow dataRow = data.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; ++j)
                                {
                                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                        dataRow[j] = row.GetCell(j).ToString();
                                }
                                data.Rows.Add(dataRow);
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        public string StringRep(string name)
        {
            string str = name.Replace("'", "\'");
            return str;
        }

        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }

        /// <summary>
        /// 判断是否属于时间格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            string sql = string.Format(@"UPDATE  dbo.QTB_IDG_BookDrama
                                        SET     MuteVideo = '{0}'
                                                + CAST(VideoNumber AS VARCHAR(100)) + '/' + MuteVideo;
                                        UPDATE  dbo.QTB_IDG_BookDrama
                                        SET     CompleteVideo = '{0}'
                                                + CAST(VideoNumber AS VARCHAR(100)) + '/' + CompleteVideo;
                                        UPDATE  dbo.QTB_IDG_BookDrama
                                        SET     BackgroundAudio = '{0}'
                                                + CAST(VideoNumber AS VARCHAR(100)) + '/' + BackgroundAudio;
                                        UPDATE  dbo.QTB_IDG_BookDrama
                                        SET     VideoCover = '{0}'
                                                + CAST(VideoNumber AS VARCHAR(100)) + '/' + VideoCover;", ddlAddress.SelectedItem.Text);
            if (SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql) > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('更新成功!');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                  "<script type=\"text/javascript\">alert('更新失败!');</script>");
            }
        }
    }
}
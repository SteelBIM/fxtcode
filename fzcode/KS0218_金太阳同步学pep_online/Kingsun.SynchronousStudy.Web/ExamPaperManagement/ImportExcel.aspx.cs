using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Kingsun.ExamPaper.BLL;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class ImportExcel : System.Web.UI.Page
    {
        public int catalogid;
        public string catalogName;
        private CatalogBLL catalogBLL = new CatalogBLL();
        private IWorkbook _workbook;
        public string Type;

        protected void Page_Load(object sender, EventArgs e)
        {
            catalogid = Convert.ToInt32(Request.QueryString["catalogid"]);
            Type = Request.QueryString["Type"];
            catalogName = catalogBLL.GetCatalog(catalogid).CatalogName;
            lbCatalogName.Text = catalogName;
            if (Type != "catalog")
            {
                DataBind(catalogid);
                btnImport.Visible = false;
            }
        }

        private string GetBookName(int bookid)
        {
            string sql = @"SELECT top 1 [TeachingNaterialName] FROM [TB_ModuleConfiguration] WHERE BookID=" + bookid;
            string bookname = "";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bookname = ds.Tables[0].Rows[0][0].ToString();
            }
            return bookname;
        }

        private void DataBind(int bookid)
        {
            string sql = string.Format(@"SELECT  *
                            FROM    [TB_ModuleSort]
                            WHERE   ID IN ( SELECT  MAX(ID)
                                            FROM    [TB_ModuleSort]
                                            WHERE   BookID = {0}
                                                    AND ( SuperiorID = FirstTitleID
                                                          OR SuperiorID = SecondTitleID
                                                        )
                                            GROUP BY ModuleID );", bookid);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
            rpModule.DataSource = dt;
            rpModule.DataBind();
        }

        protected void rpModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "down")
            {
                string[] arg = e.CommandArgument.ToString().Split(',');
                string sql = string.Format(@"SELECT ModularName FROM [TB_ModularManage] WHERE ModularID={0} AND State=1", arg[1]);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                switch (ds.Tables[0].Rows[0]["ModularName"].ToString())
                {
                    case "趣配音":
                        ImportVideoDetails();
                        break;
                    case "说说看":
                        ImportHearResources();
                        break;
                    case "玩单词":
                        ImportWordGame();
                        break;
                    default:
                        break;
                }

            }
        }

        /// <summary>
        /// 趣配音资源导入
        /// </summary>
        private void ImportVideoDetails()
        {
            string newFileName = string.Empty;
            string strName = FileUpload1.PostedFile.FileName; //使用fileupload控件获取上传文件的文件名
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
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("BookName");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("FirstTitle");
            dtList.Columns.Add("SecondTitleID");
            dtList.Columns.Add("SecondTitle");
            dtList.Columns.Add("FirstModularID");
            dtList.Columns.Add("FirstModular");
            dtList.Columns.Add("SecondModularID");
            dtList.Columns.Add("SecondModular");
            dtList.Columns.Add("VideoTitle");
            dtList.Columns.Add("VideoNumber");
            dtList.Columns.Add("MuteVideo");
            dtList.Columns.Add("CompleteVideo");
            //dtList.Columns.Add("VideoTime");
            dtList.Columns.Add("BackgroundAudio");
            dtList.Columns.Add("VideoCover");
            dtList.Columns.Add("VideoDesc");
            dtList.Columns.Add("VideoDifficulty");

            DataTable dtDialogue = new DataTable();
            dtDialogue.Columns.Add("ID");
            dtDialogue.Columns.Add("VideoID");
            dtDialogue.Columns.Add("BookID");
            dtDialogue.Columns.Add("FirstTitleID");
            dtDialogue.Columns.Add("SecondTitleID");
            dtDialogue.Columns.Add("FirstModularID");
            dtDialogue.Columns.Add("SecondModularID");
            dtDialogue.Columns.Add("DialogueText");
            dtDialogue.Columns.Add("DialogueNumber");
            dtDialogue.Columns.Add("StartTime");
            dtDialogue.Columns.Add("EndTime");

            if (dt.Rows.Count <= 0 || dtSheet2.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = catalogid;
                drlist["BookName"] = catalogName;
                drlist["FirstTitleID"] = ParseInt(dt.Rows[i]["一级标题ID"]);
                drlist["FirstTitle"] = dt.Rows[i]["一级标题"];
                if (dt.Rows[i]["二级标题ID"].ToString() != "")
                {
                    drlist["SecondTitleID"] = dt.Rows[i]["二级标题ID"].ToString();
                }
                //drlist["SecondTitleID"] = ParseInt(dt.Rows[i]["二级标题ID"]);
                drlist["SecondTitle"] = dt.Rows[i]["二级标题"];
                if (dt.Rows[i]["一级模块ID"].ToString() != "")
                {
                    drlist["FirstModularID"] = dt.Rows[i]["一级模块ID"].ToString();
                }
                //drlist["FirstModularID"] = ParseInt(dt.Rows[i]["一级模块ID"]);
                drlist["FirstModular"] = dt.Rows[i]["一级模块"];
                drlist["SecondModularID"] = ParseInt(dt.Rows[i]["二级模块ID"]);
                drlist["SecondModular"] = dt.Rows[i]["二级模块"];
                drlist["VideoNumber"] = ParseInt(dt.Rows[i]["序号"]);
                drlist["VideoTitle"] = dt.Rows[i]["视频标题"];
                drlist["MuteVideo"] = dt.Rows[i]["静音视频"];
                drlist["CompleteVideo"] = dt.Rows[i]["完整视频"];
                //drlist["VideoTime"] = ParseInt(dt.Rows[i]["视频时长"]);
                drlist["BackgroundAudio"] = dt.Rows[i]["背景音频"];
                drlist["VideoCover"] = dt.Rows[i]["视频封图"];
                drlist["VideoDesc"] = dt.Rows[i]["视频简介"];
                drlist["VideoDifficulty"] = ParseInt(dt.Rows[i]["难易程度"]);
                dtList.Rows.Add(drlist);
            }

            for (int i = 0; i < dtSheet2.Rows.Count; i++)
            {
                DataRow drDialogue = dtDialogue.NewRow();
                drDialogue["BookID"] = catalogid;
                drDialogue["VideoID"] = dtSheet2.Rows[i]["序号"];
                //drDialogue["FirstTitleID"] = Convert.ToInt32((string)dt.Rows[i]["一级标题ID"] == "" ? "0" : dt.Rows[i]["一级标题ID"]);
                //drDialogue["SecondTitleID"] = Convert.ToInt32((string)dt.Rows[i]["二级标题ID"] == "" ? "0" : dt.Rows[i]["二级标题ID"]);
                //drDialogue["FirstModularID"] = Convert.ToInt32((string)dt.Rows[i]["一级模块ID"] == "" ? "0" : dt.Rows[i]["一级模块ID"]);
                //drDialogue["SecondModularID"] = Convert.ToInt32((string)dt.Rows[i]["二级模块ID"] == "" ? "0" : dt.Rows[i]["二级模块ID"]);
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

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };
            SqlBulkCopy sbc1 = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };


            string strSql = "DELETE FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID=" + Convert.ToInt32(catalogid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
            strSql = "DELETE FROM [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] WHERE BookID=" + Convert.ToInt32(catalogid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "[FZ_InterestDubbing].[dbo].[TB_VideoDetails]";
                sbc.WriteToServer(dtList); //此处报错
                sbc1.DestinationTableName = "[FZ_InterestDubbing].[dbo].[TB_VideoDialogue]";
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

        /// <summary>
        /// 每日听说资源导入
        /// </summary>
        private void ImportHearResources()
        {
            string newFileName = string.Empty;
            string strName = FileUpload1.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
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

            DataTable dt = ExcelToDataTable("总视频信息", newFileName, true);

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("SecondTitleID");
            dtList.Columns.Add("FirstModularID");
            dtList.Columns.Add("SecondModularID");
            dtList.Columns.Add("ModularEN");
            dtList.Columns.Add("SerialNumber");
            dtList.Columns.Add("TextSerialNumber");
            dtList.Columns.Add("RoleName");
            dtList.Columns.Add("TextDesc");
            dtList.Columns.Add("AudioFrequency");
            dtList.Columns.Add("Picture");
            dtList.Columns.Add("RepeatNumber");
            dtList.Columns.Add("CreateDate");


            if (dt == null || dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = ParseInt(catalogid);
                drlist["FirstTitleID"] = ParseInt(dt.Rows[i]["一级标题ID"].ToString().Trim());
                if (dt.Rows[i]["二级标题ID"].ToString() != "")
                {
                    drlist["SecondTitleID"] = ParseInt(dt.Rows[i]["二级标题ID"].ToString().Trim());
                }
                drlist["FirstModularID"] = ParseInt(dt.Rows[i]["一级模块ID"].ToString().Trim());
                drlist["SecondModularID"] = ParseInt(dt.Rows[i]["二级模块ID"].ToString().Trim());
                drlist["ModularEN"] = dt.Rows[i]["二级模块英文标题"].ToString().Trim();
                drlist["SerialNumber"] = ParseInt(dt.Rows[i]["序号"].ToString().Trim());
                drlist["TextSerialNumber"] = ParseInt(dt.Rows[i]["子序号"].ToString().Trim());
                if (dt.Rows[i]["角色名"].ToString() != "")
                {
                    drlist["RoleName"] = dt.Rows[i]["角色名"].ToString().Trim();
                }
                drlist["TextDesc"] = dt.Rows[i]["文本"].ToString().Trim();
                drlist["AudioFrequency"] = dt.Rows[i]["音频"].ToString().Trim();
                if (dt.Rows[i]["图片"].ToString() != "")
                {
                    drlist["Picture"] = dt.Rows[i]["图片"].ToString().Trim();
                }
                switch (dt.Rows[i]["二级模块"].ToString())
                {
                    case "跟读单词":
                        drlist["RepeatNumber"] = 3;
                        break;
                    case "跟读句子":
                        drlist["RepeatNumber"] = 2;
                        break;
                    case "跟读课文":
                        drlist["RepeatNumber"] = 2;
                        break;
                    case "跟读语音":
                        drlist["RepeatNumber"] = 2;
                        break;
                    default:
                        drlist["RepeatNumber"] = 0;
                        break;
                }

                dtList.Rows.Add(drlist);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "DELETE FROM [FZ_HearResources].[dbo].[TB_HearResources] WHERE BookID=" + Convert.ToInt32(catalogid) + " AND FirstModularID='" + dt.Rows[0]["一级模块ID"] + "'";
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "[FZ_HearResources].[dbo].[TB_HearResources]";
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
        /// 导入目录页码
        /// </summary>
        private void ImportCatalogPage()
        {
            string newFileName = string.Empty;
            string strName = FileUpload1.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
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

            DataTable dt = ExcelToDataTable("目录信息", newFileName, true);

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("SecondTitleID");
            dtList.Columns.Add("StartingPage");
            dtList.Columns.Add("EndingPage");

            if (dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = ParseInt(catalogid);
                drlist["FirstTitleID"] = ParseInt(dt.Rows[i]["一级标题ID"]);
                if (dt.Rows[i]["二级标题ID"].ToString() != "")
                {
                    drlist["SecondTitleID"] = dt.Rows[i]["二级标题ID"].ToString();
                }
                drlist["StartingPage"] = ParseInt(dt.Rows[i]["起始页码"]);
                drlist["EndingPage"] = ParseInt(dt.Rows[i]["终止页码"]);
                dtList.Rows.Add(drlist);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "DELETE FROM dbo.[TB_CatalogPage] WHERE BookID=" + Convert.ToInt32(catalogid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "TB_CatalogPage";
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
        /// 导入玩单词资源
        /// </summary>
        private void ImportWordGame()
        {
            string newFileName = string.Empty;
            string strName = FileUpload1.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
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

            DataTable dt = ExcelToDataTable("目录信息", newFileName, true);

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("SecondTitleID");
            dtList.Columns.Add("FirstModularID");
            dtList.Columns.Add("SecondModularID");
            dtList.Columns.Add("SerialNumber");
            dtList.Columns.Add("Word");
            dtList.Columns.Add("LetterAudio");
            dtList.Columns.Add("WordAudioWoman");
            dtList.Columns.Add("WordAudioHarmony");
            dtList.Columns.Add("WordImage");
            dtList.Columns.Add("WordInterpretationImage");
            dtList.Columns.Add("WordExampleSentence");
            dtList.Columns.Add("ExampleSentenceAudio");
            dtList.Columns.Add("CreateDate");

            if (dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = ParseInt(catalogid);
                drlist["FirstTitleID"] = ParseInt(dt.Rows[i]["一级标题ID"]);
                if (dt.Rows[i]["二级标题ID"].ToString() != "")
                {
                    drlist["SecondTitleID"] = ParseInt(dt.Rows[i]["二级标题ID"].ToString());
                }
                drlist["FirstModularID"] = ParseInt(dt.Rows[i]["一级模块ID"].ToString());
                drlist["SecondModularID"] = ParseInt(dt.Rows[i]["二级模块ID"].ToString());
                drlist["SerialNumber"] = ParseInt(dt.Rows[i]["序号"].ToString());
                drlist["Word"] = dt.Rows[i]["单词"].ToString() == "" ? null : dt.Rows[i]["单词"].ToString();
                drlist["LetterAudio"] = dt.Rows[i]["字母音频"].ToString() == "" ? null : dt.Rows[i]["字母音频"].ToString();
                drlist["WordAudioWoman"] = dt.Rows[i]["单词音频（女声）"].ToString() == "" ? null : dt.Rows[i]["单词音频（女声）"].ToString();
                drlist["WordAudioHarmony"] = dt.Rows[i]["单词音频（和声）"].ToString() == "" ? null : dt.Rows[i]["单词音频（和声）"].ToString();
                drlist["WordImage"] = dt.Rows[i]["单词图片"].ToString() == "" ? null : dt.Rows[i]["单词图片"].ToString();
                drlist["WordInterpretationImage"] = dt.Rows[i]["单词释义图片"].ToString() == "" ? null : dt.Rows[i]["单词释义图片"].ToString();
                drlist["WordExampleSentence"] = dt.Rows[i]["单词例句"].ToString() == "" ? null : dt.Rows[i]["单词例句"].ToString();
                drlist["ExampleSentenceAudio"] = dt.Rows[i]["例句音频"].ToString() == "" ? null : dt.Rows[i]["例句音频"].ToString();
                drlist["CreateDate"] = DateTime.Now;
                dtList.Rows.Add(drlist);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "DELETE FROM dbo.[TB_WordGame] WHERE BookID=" + Convert.ToInt32(catalogid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "TB_WordGame";
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
        /// 获取完整路径加文件名
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
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
                newFileName = juedui + strName + kzm;
                if (FileUpload1.HasFile) //验证 FileUpload 控件确实包含文件
                {
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int j = 0; j < allowedExtensions.Length; j++)
                    {
                        if (kzm == allowedExtensions[j])
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
                        FileUpload1.PostedFile.SaveAs(newFileName); //将图片存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('" + ex.Message + "');</script>");
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
                //FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath(fileName));
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    _workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    _workbook = new HSSFWorkbook(fs);

                ISheet sheet;
                if (sheetName != null)
                {
                    sheet = _workbook.GetSheet(sheetName) ?? _workbook.GetSheetAt(0);
                }
                else
                {
                    sheet = _workbook.GetSheetAt(0);
                }
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

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 只显示要显示的导入项
        /// </summary>
        /// <param name="modular"></param>
        /// <returns></returns>
        public string GetStyle(string modular)
        {
            string s = "display:none";
            switch (modular)
            {
                case "趣配音":
                    s = "";
                    break;
                case "说说看":
                    s = "";
                    break;
                case "玩单词":
                    s = "";
                    break;
                default:
                    break;
            }
            return s;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ImportCatalogPage();
        }
    }
}
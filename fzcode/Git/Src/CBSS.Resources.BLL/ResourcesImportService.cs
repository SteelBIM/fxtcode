using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.ResourcesManager.Contract.DataModel;
using CBSS.ResourcesManager.IBLL;

namespace CBSS.ResourcesManager.BLL
{
    public partial class ResourcesService : IResourcesService
    {
        /// <summary>
        /// 导入趣配音资源
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <param name="ofile"></param>
        /// <param name="bookId"></param>
        /// <param name="bookName"></param>
        /// <returns></returns>
        public string ImportVideoDetails(string fileName, string filePath, HttpPostedFileBase ofile, int bookId, string bookName)
        {
            string newFileName = string.Empty;
            string strName = fileName; //使用fileupload控件获取上传文件的文件名
            if (string.IsNullOrEmpty(strName))
            {
                return "请选择文件!";
            }
            newFileName = ExcelHelper.GetNewFileName(strName, filePath, ofile);
            DataTable dt = new DataTable();
            DataTable dtSheet2 = new DataTable();
            try
            {
                dt = ExcelHelper.ExcelToDataTableByNPOI("总视频信息", newFileName, true);
                dtSheet2 = ExcelHelper.ExcelToDataTableByNPOI("分视频信息", newFileName, true);
            }
            catch (Exception ex)
            {
                return "导入失败！" + ex.Message;
            }

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

            if (dt == null && dtSheet2 == null)
            {
                return "excel数据为空！";
            }
            if (dt.Rows.Count <= 0 || dtSheet2.Rows.Count <= 0)
            {
                return "Excel表数据为空或Excel是打开状态！";
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = bookId;
                drlist["BookName"] = bookName;
                drlist["FirstTitleID"] = StringHelper.StrToInt32(dt.Rows[i]["一级标题ID"].ToString(), 0);
                drlist["FirstTitle"] = dt.Rows[i]["一级标题"];
                if (dt.Rows[i]["二级标题ID"].ToString().Trim() != "")
                {
                    drlist["SecondTitleID"] = dt.Rows[i]["二级标题ID"].ToString().Trim();
                }
                else
                {
                    drlist["SecondTitleID"] = null;
                }
                drlist["SecondTitle"] = dt.Rows[i]["二级标题"].ToString().Trim();
                if (dt.Rows[i]["一级模块ID"].ToString().Trim() != "")
                {
                    drlist["FirstModularID"] = dt.Rows[i]["一级模块ID"].ToString().Trim();
                }
                drlist["FirstModular"] = dt.Rows[i]["一级模块"].ToString().Trim();
                if (dt.Rows[i]["二级模块ID"].ToString().Trim() != "")
                {
                    drlist["SecondModularID"] = dt.Rows[i]["二级模块ID"].ToString().Trim();
                }
                drlist["SecondModular"] = dt.Rows[i]["二级模块"].ToString().Trim();
                drlist["VideoNumber"] = StringHelper.StrToInt32(dt.Rows[i]["序号"].ToString(), 0);
                drlist["VideoTitle"] = dt.Rows[i]["视频标题"];
                drlist["MuteVideo"] = dt.Rows[i]["静音视频"];
                drlist["CompleteVideo"] = dt.Rows[i]["完整视频"];
                //drlist["VideoTime"] = ParseInt(dt.Rows[i]["视频时长"]);
                drlist["BackgroundAudio"] = dt.Rows[i]["背景音频"];
                drlist["VideoCover"] = dt.Rows[i]["视频封图"];
                drlist["VideoDesc"] = dt.Rows[i]["视频简介"];
                drlist["VideoDifficulty"] = StringHelper.StrToInt32(dt.Rows[i]["难易程度"].ToString(), 0);
                dtList.Rows.Add(drlist);
            }

            for (int i = 0; i < dtSheet2.Rows.Count; i++)
            {
                DataRow drDialogue = dtDialogue.NewRow();
                drDialogue["BookID"] = bookId;
                drDialogue["VideoID"] = dtSheet2.Rows[i]["序号"];
                drDialogue["DialogueText"] = dtSheet2.Rows[i]["分视频文本"].ToString();
                drDialogue["DialogueNumber"] = StringHelper.StrToInt32(dtSheet2.Rows[i]["分视频序号"].ToString(),0);
                if (dtSheet2.Rows[i]["起始时间"].ToString() != "")
                {
                    if (StringHelper.IsDateTime(dtSheet2.Rows[i]["起始时间"].ToString()))
                    {
                        drDialogue["StartTime"] = dtSheet2.Rows[i]["起始时间"];
                    }
                    else
                    {
                        return "序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的开始时间格式错误！";
                    }

                }
                if (dtSheet2.Rows[i]["结束时间"].ToString() != "")
                {
                    if (StringHelper.IsDateTime(dtSheet2.Rows[i]["结束时间"].ToString()))
                    {
                        drDialogue["EndTime"] = dtSheet2.Rows[i]["结束时间"];
                    }
                    else
                    {
                        return "序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的结束时间格式错误！";
                    }
                }
                dtDialogue.Rows.Add(drDialogue);
            }

            bool b1 = repository.Delete<InterestDubbingResource>(i => i.OldMODBookID == bookId);
            bool b2 = repository.Delete<InterestDubbingFragment>(i => i.OldMODBookID == bookId);
            if (b1 || b2)
            {
                Log4NetHelper.Error(LoggerType.WebExceptionLog, "删除书籍资源失败，书籍ID：" + bookId, null);
                return "删除数据失败！";
            }


            int t1 = ExcelHelper.ImportExcelByNPOI(repository.ConnectionString, dt, "TB_VideoDetails");
            int t2 = ExcelHelper.ImportExcelByNPOI(repository.ConnectionString, dtList, "TB_VideoDialogue");


            if (t1 <= 0 || t2 <= 0)
            {
                return "插入" + t1 + "条数据！";
            }
            else
            {
                File.Delete(newFileName);
                return "导入成功";
            }
        }


    }
}

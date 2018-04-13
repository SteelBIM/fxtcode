using System;
using System.Collections.Generic;
using System.IO;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Web.ExamPaperManagement;
using NPOI.OpenXml4Net.OPC.Internal;

namespace Kingsun.SynchronousStudy.Web.ExamPaperManagement
{
    public partial class ResourceManager : BasePage
    {
        public IList<QTb_Edition> listEdition = new List<QTb_Edition>();
        protected void Page_Load(object sender, EventArgs e)
        {
            listEdition = new EditionBLL().Search("ParentID=0 and IsRemove=0", "ParentID,EditionID");
            if (!IsPostBack)
            {
                ActionInit();
            }
        }

        private void ActionInit()
        {
            string action = "";
            string resid = "";
            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
            {
                action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
            }
            else
            {
                return;
            }
            switch (action)
            {
                case "query":
                    string strWhere = "";
                    if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        strWhere = Request.QueryString["queryStr"];
                    }
                    strWhere = string.IsNullOrEmpty(strWhere) ? " 1=1 " : strWhere;
                    int pageindex = int.Parse(Request.Form["page"].ToString());
                    int pagesize = int.Parse(Request.Form["rows"].ToString());
                    int totalcount, totalpage;
                    IList<V_Resource> courseList = new ResourceBLL().GetResourcePageList(pageindex, pagesize, strWhere, "EditionID,GradeID,BookReel", 1, out totalcount, out totalpage);
                    var obj = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "addres":
                    if (!string.IsNullOrEmpty(Request.Form["EditionID"]) && !string.IsNullOrEmpty(Request.Form["GradeID"]) && !string.IsNullOrEmpty(Request.Form["BookReel"]))
                    {
                        int editionid = Convert.ToInt32(Request.Form["EditionID"]);
                        int gradeid = Convert.ToInt32(Request.Form["GradeID"]);
                        int bookreel = Convert.ToInt32(Request.Form["BookReel"]);
                        QTb_Resource res = new ResourceBLL().GetResource(editionid, gradeid, bookreel);
                        if (res != null)
                        {
                            WriteErrorResult("已存在该条件下的记录，无需重复添加。");
                        }
                        else
                        {
                            res = new QTb_Resource();
                            res.ResMD5 = "";
                            res.ResUrl = "";
                            res.ResVersion = "";
                            res.ResUpTimes = 0;
                            res.GradeID = gradeid;
                            res.EditionID = editionid;
                            res.BookReel = bookreel;
                            if (new ResourceBLL().InsertResource(res))
                            {
                                WriteResult("");
                            }
                            else
                            {
                                WriteErrorResult("添加失败");
                            }
                        }
                    }
                    break;
                case "compress":
                    resid = Request.Form["ResID"];
                    string fileurl = Request.Form["FileUrl"];
                    if (!string.IsNullOrEmpty(resid) && !string.IsNullOrEmpty(fileurl))
                    {
                        DownFiles(Convert.ToInt32(resid), fileurl);
                    }
                    else
                    {
                        WriteErrorResult("没有获取到资源ID");
                    }
                    break;
                default:
                    Response.Write("");
                    Response.End();
                    break;
            }
        }

        #region 压缩并返回下载路径
        private void DownFiles(int ResID,string fileUrl)
        {
            QTb_Resource resource = new ResourceBLL().GetResource(ResID);
            if (resource == null)
            {
                WriteErrorResult("找不到资源相关信息");
            }
            //人教PEP版_53_3A
            string[] arrUrl = fileUrl.Split('_');
            if (arrUrl.Length==3)
            {
                string zipfilename = arrUrl[0] + "_" + arrUrl[1] + "_" + arrUrl[2] + ".zip";
                string zipFilePath = Server.MapPath(Request.ApplicationPath.ToString()) + "\\QuestionFiles\\DownZip\\" + zipfilename;
                if (File.Exists(zipFilePath))//如果文件存在就下载当前存在的文件，此文件是在一天之内创建
                {
                    //先删除原压缩包
                    File.Delete(zipFilePath);
                }

                //按下划线分割目录
                string Source = Server.MapPath(Request.ApplicationPath.ToString()) + "\\QuestionFiles\\" + arrUrl[0] + "\\" + arrUrl[1] + "\\" + arrUrl[2] + "\\";
                Directory.CreateDirectory(Path.GetDirectoryName(zipFilePath));
                string errorMsg = "";
                try
                {
                    Kingsun.SynchronousStudy.Common.ZipHelper.ZipNo(Source, zipFilePath);
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                if (string.IsNullOrEmpty(errorMsg))
                {
                    zipfilename = AppSetting.Root + "/QuestionFiles/DownZip/" + zipfilename;
                    resource.ResUrl = zipfilename;
                    resource.ResMD5 = Common.Common.GetMD5HashFromFile(zipFilePath);
                    resource.ResUpTimes = resource.ResUpTimes.GetValueOrDefault() + 1;
                    resource.ResVersion = resource.EditionID.Value.ToString() + "." + resource.GradeID.Value.ToString() + "." + resource.BookReel.Value.ToString() + "." + resource.ResUpTimes;
                    if (new ResourceBLL().UpdateResource(resource))
                    {
                        WriteResult("");
                    }
                    else
                    {
                        WriteErrorResult("打包成功(路径：" + resource.ResUrl + ")，但更新数据库失败！");
                    }
                }
                else
                {
                    File.Delete(zipFilePath);
                    WriteErrorResult("压缩失败：" + errorMsg);
                }
            }
            else
            {
                WriteErrorResult("该教材无需打包资源！");
            }
        }
        #endregion
    }
}
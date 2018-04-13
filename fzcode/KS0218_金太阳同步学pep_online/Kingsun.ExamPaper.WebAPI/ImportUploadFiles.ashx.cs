using Kingsun.ExamPaper.Common;
using System;
using System.IO;
using System.Web;

namespace Kingsun.ExamPaper.WebAPI
{
    /// <summary>
    /// ImportUploadFiles 的摘要说明
    /// </summary>
    public class ImportUploadFiles : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //跨域访问限制
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (context.Request.Files.Count > 0 && context.Request.Files[0].ContentLength > 0 && !string.IsNullOrEmpty(context.Request.QueryString["path"]))
            {
                string path = context.Request.QueryString["path"];
                string fileExt = context.Request.Files[0].FileName;
                int fileSize = context.Request.Files[0].ContentLength;
                string fileDir = "";
                if (fileExt.IndexOf(".jpg") > -1 || fileExt.IndexOf(".png") > -1)
                {
                    fileDir = Path.Combine("/QuestionFiles/" + path.Replace("_", "/") + "Img/");
                }
                else if (fileExt.IndexOf(".mp3") > -1)
                {
                    fileDir = Path.Combine("/QuestionFiles/" + path.Replace("_", "/") + "Mp3/");
                }
                else
                {
                    context.Response.Write(JsonHelper.EncodeJson("错误：没有找到文件路径"));
                    context.Response.End();
                }
                string filepath = "";
                filepath = fileDir + fileExt;
                fileDir = context.Server.MapPath(fileDir);
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }

                string filePath = Path.Combine(fileDir, fileExt);
                try
                {
                    context.Request.Files[0].SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    context.Response.Write(JsonHelper.EncodeJson("错误：保存文件失败." + ex.Message));
                    context.Response.End();
                }
                context.Response.Write(AppSetting.Root + filepath);
                context.Response.End();
            }
            else
            {
                context.Response.Write(JsonHelper.EncodeJson("错误：没有找到文件"));
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
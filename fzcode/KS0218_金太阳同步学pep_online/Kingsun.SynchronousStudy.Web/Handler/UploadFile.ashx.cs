using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// UploadFile 的摘要说明
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";// "text/plain"; 
            try
            {
                string action = context.Request["action"];
                switch (action)
                {
                    case "OssUploadImg":
                        OssUploadImg(context);
                        break;
                    case "UploadImg":
                        UploadImg(context);
                        break;
                    case "UploadImgToOss":
                        UploadImgToOss(context);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 上传图片到oss
        /// </summary>
        /// <param name="context"></param>
        public void UploadImgToOss(HttpContext context)
        {
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count > 0)
                {
                    string filesName = context.Request["filesName"];
                    string extension = System.IO.Path.GetExtension(files[filesName].FileName).ToLower();//扩展名
                    string fileName = Guid.NewGuid() + extension;
                    if (extension == ".bmp" || extension == ".gif" || extension == ".jpeg" || extension == ".jpg" || extension == ".png")
                    {
                        //string DownLoadPath = context.Server.MapPath("/Upload/Imgs/" + fileName);
                        string DownLoadPath = context.Server.MapPath("/Upload/" + System.IO.Path.GetFileName(files[filesName].FileName));
                        files[filesName].SaveAs(DownLoadPath);
                        string keyFilePath = context.Request["keyFilePath"];//保存oss的路径
                        string key = keyFilePath + Guid.NewGuid().ToString();
                        int flag = OSSHelper.PutObjectState("synchronousstudy", key, DownLoadPath);
                        if (flag == 1)
                        {
                            string url = "http://tbxcdn.kingsun.cn/" + key;// OSSHelper.getUrl("synchronousstudy", key);
                            if (System.IO.File.Exists(DownLoadPath))
                            {
                                System.IO.File.Delete(DownLoadPath);
                            }
                            context.Response.Write("{'msg':'1','data':'" + url + "'}");
                        }
                        else
                        {
                            context.Response.Write("{'msg':'0','data':'上传图片失败'}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{'msg':'0','data':'图片格式不正确'}");
                    }
                }
                else
                {
                    context.Response.Write("{'msg':'0','data':'上传图片失败'}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{'msg':'0','data':'上传图片失败'}");
            }
        }
        public void UploadImg(HttpContext context)
        {
            try
            {
                HttpFileCollection files = context.Request.Files;
                if (files.Count > 0)
                {
                    string extension = System.IO.Path.GetExtension(files["UploadImg"].FileName).ToLower();//扩展名
                    string fileName = Guid.NewGuid() + extension;
                    if (extension == ".bmp" || extension == ".gif" || extension == ".jpeg" || extension == ".jpg" || extension == ".png")
                    {
                        //string DownLoadPath = context.Server.MapPath("/Upload/Imgs/" + fileName);
                        string DownLoadPath = context.Server.MapPath("/Upload/" + System.IO.Path.GetFileName(files["UploadImg"].FileName));
                        files["UploadImg"].SaveAs(DownLoadPath);
                        string key = "SynchronousStudy/UpLoadFile/" + Guid.NewGuid().ToString();
                        int flag = OSSHelper.PutObjectState("synchronousstudy", key, DownLoadPath);
                        if (flag == 1)
                        {
                            string url = OSSHelper.getUrl("synchronousstudy", key);
                            if (System.IO.File.Exists(DownLoadPath))
                            {
                                System.IO.File.Delete(DownLoadPath);
                            }
                            context.Response.Write("{'msg':'1','data':'" + url + "'}");
                        }
                        else
                        {
                            context.Response.Write("{'msg':'0','data':'上传图片失败'}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{'msg':'0','data':'图片格式不正确'}");
                    }
                }
                else
                {
                    context.Response.Write("{'msg':'0','data':'上传图片失败'}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{'msg':'0','data':'上传图片失败'}");
            }
        }
        /// <summary>
        /// 上传文件到OSS服务器
        /// </summary>
        /// <param name="context"></param>
        public void OssUploadImg(HttpContext context)
        {
            try
            {

                HttpFileCollection files = context.Request.Files;
                if (files.Count > 0)
                {
                    string DownLoadPath = context.Server.MapPath("/Upload/" + System.IO.Path.GetFileName(files["UploadImg"].FileName));
                    files["UploadImg"].SaveAs(DownLoadPath);
                    string key = "SynchronousStudy/UpLoadFile/" + Guid.NewGuid().ToString();
                    int flag = OSSHelper.PutObjectState("synchronousstudy", key, DownLoadPath);
                    if (flag == 1)
                    {
                        string url = OSSHelper.getUrl("synchronousstudy", key);
                        if (System.IO.File.Exists(DownLoadPath))
                        {
                            System.IO.File.Delete(DownLoadPath);
                        }
                        context.Response.Write("{'msg':'1','url':'" + url + "'}");
                    }
                    else
                    {
                        context.Response.Write("{'msg':'0'}");
                    }
                }
                else
                {
                    context.Response.Write("{'msg':'0'}");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{'msg':'0'}");
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
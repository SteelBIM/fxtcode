using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CourseActivate.Resource.Constract.Model;
using CourseActivate.Resource.BLL;
using CourseActivate.Core.Utility;

namespace WebUploadTest
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        #region 文件分片

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string resid = context.Request["resid"];
            string bookid = context.Request["bookid"];
            string modularid = context.Request["modularid"];

            string name = context.Request["name"];         //文件的名称
            string filesize = context.Request["size"];     //文件的大小
            string md5str = context.Request["md5"];        //文件的md5值
            string folder = context.Server.MapPath("~/Upload/BookResource");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string rootpath = Path.Combine(folder, md5str) + "\\";   //文件的保存路径


            string targetPath = Path.Combine(rootpath, name);   //用于判断文件是否存在

            string sourcePath = Path.Combine(rootpath, md5str) + "\\";  //以文件的md5值作为保存文件分片的临时文件夹 

            if (System.IO.File.Exists(targetPath))
            {
                System.IO.File.Delete(targetPath);//允许重复上传
            }

            if (!System.IO.File.Exists(targetPath))
            {
                if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
                {
                    //取得chunk和chunks
                    int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                    int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数


                    string path = sourcePath + chunk;


                    //建立临时传输文件夹
                    if (!Directory.Exists(Path.GetDirectoryName(sourcePath)))
                    {
                        Directory.CreateDirectory(sourcePath);
                    }
                    if (context.Request.Files.Count != 0)
                    {
                        FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                        BinaryWriter AddWriter = new BinaryWriter(addFile);
                        //获得上传的分片数据流
                        HttpPostedFile file = context.Request.Files[0];
                        Stream stream = file.InputStream;

                        BinaryReader TempReader = new BinaryReader(stream);

                        //将上传的分片追加到临时文件末尾
                        AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                        //关闭BinaryReader文件阅读器
                        TempReader.Close();
                        stream.Close();
                        AddWriter.Close();
                        addFile.Close();

                        TempReader.Dispose();
                        stream.Dispose();
                        AddWriter.Dispose();
                        addFile.Dispose();

                        context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
                    }
                }
                else if (context.Request.Form.AllKeys.Any(m => m == "status"))
                {
                    //取得chunk和chunks
                    int chunk = Convert.ToInt32(context.Request.Form["chunkIndex"]);//当前分片在上传分片中的顺序（从0开始）
                    long size = Convert.ToInt32(context.Request.Form["size"]);
                    int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数


                    string path = sourcePath + chunk;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists == true && file.Length == size)
                    {
                        context.Response.Write("{\"ifExist\" : 1}");
                    }
                    else
                    {
                        context.Response.Write("{\"ifExist\" : 0}");
                    }

                }
                else//没有分片直接保存
                {
                    string saveName = context.Request["savename"];
                    Directory.CreateDirectory(rootpath);
                    // context.Request.Files[0].SaveAs(rootpath + context.Request.Files[0].FileName);
                    context.Request.Files[0].SaveAs(rootpath + saveName);//自定义名字


                    ////操作数据库
                    //SQLHelper sq = new SQLHelper();
                    //sq.Open();
                    //string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    //sq.CreateProcDataAdapter(name, filesize, md5str, rootpath, time);
                    //sq.Close();

                  // string resRoot = ConfigurationManager.AppSettings["ResourceRoot"];
                    string resRoot = "@/Upload/BookResource/";
                    tb_res_resource res = new tb_res_resource
                    {
                        //主要字段
                        ResID = int.Parse(resid),
                        ResName = saveName,
                        ResUrl = resRoot + md5str + "/" + saveName,
                        ResMD5 = MD5Helper.GetMD5HashFromFile(rootpath + saveName),
                        BookID = int.Parse(bookid),
                        ModularID = int.Parse(modularid),
                        IsForce = Convert.ToInt32(context.Request["isForce"] == "true"),
                        //其他字段                          

                        Status = 0,
                    };
                    ResourceBLL bll = new ResourceBLL();
                    if (res.ResID > 0)
                    {
                        bll.UpdateResource(res);
                    }
                    else
                    {
                        bll.AddResource(res);
                    }

                    new ResourceBLL().GetResJson(int.Parse(bookid), System.Web.HttpContext.Current.Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                    context.Response.Write("{\"chunked\" : false, \"hasError\" : false}");
                }

            }
            else
            {
                context.Response.Write("{\"ifExist\" : 1}");
            }
        }



        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}
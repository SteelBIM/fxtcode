using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using System.Data;
using System.Data.SqlClient;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using System.Configuration;
using CourseActivate.Core.Utility;

namespace WebUploadTest
{
    /// <summary>
    /// Summary description for MergeFiles
    /// </summary>  
    public class MergeFiles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string resid = context.Request["resid"];
            string bookid = context.Request["bookid"];
            string modularid = context.Request["modularid"];
            string saveName = context.Request["savename"];

            string name = context.Request["name"];        //文件的名称
            string md5str = context.Request["md5"];     //文件的md5值
            string filesize = context.Request["size"];  //文件的大小
            string fileExt = context.Request["fileExt"];
            string root = context.Server.MapPath("~/Upload/BookResource");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            string rootpath = Path.Combine(root, md5str) + "\\";   //保存上传文件的路径
            //去除name的文件后缀名                    
            string temp = Path.Combine(root, name);
            string tempName = System.IO.Path.GetFileNameWithoutExtension(temp);

            string sourcePath = Path.Combine(rootpath, md5str) + "\\";//源数据文件夹
            Directory.CreateDirectory(sourcePath);
          //  string targetPath = Path.Combine(rootpath, tempName + fileExt);//合并后的文件
            string targetPath = Path.Combine(rootpath, saveName);//合并后的文件


            DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
            if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
            {
                FileInfo[] files = dicInfo.GetFiles();
                foreach (FileInfo file in files.OrderBy(f => int.Parse(f.Name)))
                {
                    FileStream addFile = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                    BinaryWriter AddWriter = new BinaryWriter(addFile);

                    //获得上传的分片数据流
                    Stream stream = file.Open(FileMode.Open);
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
                }
                DeleteFolder(sourcePath);
                //操作数据库
                //SQLHelper sq = new SQLHelper();
                //sq.Open();
                //string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                //sq.CreateProcDataAdapter(name, filesize, md5str, rootpath, time);
                //sq.Close();
              
               // string resRoot = ConfigurationManager.AppSettings["ResourceRoot"];
                string resRoot = "@/Upload/BookResource/";
                tb_res_resource res = new tb_res_resource
                {
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
                if (res.ResID > 0) bll.UpdateResource(res);
                else bll.AddResource(res);

                new ResourceBLL().GetResJson(int.Parse(bookid), System.Web.HttpContext.Current.Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                //context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"savePath\" :\"" + System.Web.HttpUtility.UrlEncode(targetPath) + "\"}");
                //context.Response.Write("{\"hasError\" : false}");
            }
            else
                context.Response.Write("{\"hasError\" : true}");
        }



        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string fl in Directory.GetDirectories(strPath))
                {
                    Directory.Delete(fl, true);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string f in Directory.GetFiles(strPath))
                {
                    System.IO.File.Delete(f);
                }
            }
            Directory.Delete(strPath, true);
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
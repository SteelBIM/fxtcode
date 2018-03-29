using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppTest
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpFileCollection FileCollect = request.Files;
            if (FileCollect.Count > 0)          //如果集合的数量大于0
            {
                foreach (string str in FileCollect)
                {
                    HttpPostedFile FileSave = FileCollect[str];  //用key获取单个文件对象HttpPostedFile
                    string imgName = DateTime.Now.ToString("yyyyMMddhhmmss例子");
                    string imgPath = "/" + imgName + FileSave.FileName;     //通过此对象获取文件名
                    string AbsolutePath = context.Server.MapPath(imgPath);
                    FileSave.SaveAs(AbsolutePath);              //将上传的东西保存
                }
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
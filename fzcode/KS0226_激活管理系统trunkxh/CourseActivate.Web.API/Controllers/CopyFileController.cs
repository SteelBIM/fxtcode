using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using CourseActivate.Web.API.Models;
using CourseActivate.Core.Utility;
using System.Web.Http;
using Kingsun.Common;
using CourseActivate.Activate.BLL;
using CourseActivate.Activate.Constract.Model;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using CourseActivate.Framework.DAL;
using System.IO;
namespace CourseActivate.Web.API.Controllers
{
    public class CopyFileController : ApiController
    {
        //
        // GET: /CopyFile/

        [HttpGet]
        public string CopyFile(string fileUrl)
        {
            try
            {
                string relatePath = fileUrl.Substring(fileUrl.LastIndexOf("/Upload/"));
                string savePath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + relatePath);
                string dirString = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dirString))
                {
                    Directory.CreateDirectory(dirString);
                }
                WebClient wc = new WebClient();
                wc.DownloadFile(fileUrl, savePath);
                return "success";
            }
            catch (Exception ex)
            {
               return "失败:" + ex.Message;
            }
        }
        [HttpGet]
        public string DeleteFile(string fileUrl)
        {
            try
            {
                string relatePath = fileUrl.Substring(fileUrl.LastIndexOf("/Upload/"));
                string savePath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + relatePath);

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return "失败:"+ex.Message;
            }
        }

    }
}

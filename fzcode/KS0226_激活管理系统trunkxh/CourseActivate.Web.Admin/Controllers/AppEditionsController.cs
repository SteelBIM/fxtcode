using CourseActivate.Core.Utility;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using CourseActivate.Web.Admin.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class AppEditionsController : BaseController
    {
        //
        // GET: /AppEditions/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAPPList()
        {
            PageParameter<tb_res_app> pageParameter = new PageParameter<tb_res_app>();
            //  pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageIndex = 1;
            pageParameter.PageSize = int.MaxValue;



            pageParameter.OrderColumns = t1 => t1.CreateDate;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_res_app> batches = base.Manage.SelectPage<tb_res_app>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }

        public ActionResult AppVersions(Guid appId)
        {
            ViewBag.APPID = appId;
            return View();
        }

        public JsonResult GetAppVersions(Guid appId)
        {
            PageParameter<tb_res_appversion> pageParameter = new PageParameter<tb_res_appversion>();
            //  pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageIndex = 1;
            pageParameter.PageSize = int.MaxValue;
            pageParameter.Where = o => o.AppID == appId;

            pageParameter.OrderColumns = t1 => t1.CreateDate;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_res_appversion> batches = base.Manage.SelectPage<tb_res_appversion>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }

        public ActionResult AddVersionView(string vId, Guid appId)
        {
            var v = Manage.Select<tb_res_appversion>(vId);
            return PartialView(v ?? new tb_res_appversion() { AppID = appId });
        }


        public ActionResult DeleteVersion(int vId)
        {
            tb_res_appversion avinfo = Manage.Select<tb_res_appversion>(vId);
            var success = Manage.Delete<tb_res_appversion>(vId);
            if (success)
                new CourseActivate.Activate.BLL.ActivateCourseBLL().Remove(RedisConfiguration.GetAppVersionKey(avinfo.AppID.Value));
            return Json(success ? KingResponse.GetResponse("") : KingResponse.GetErrorResponse(""));
        }
        public ActionResult SaveApp()
        {
            var oFile = Request.Files["txt_file"];
            if (oFile != null)
            {
                string dir = Server.MapPath("~/Upload/App/");

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string newName = Guid.NewGuid() + Path.GetExtension(oFile.FileName);
                string path = dir + "/" + newName;//保存的实际路径
                oFile.SaveAs(path);
           //     var root = System.Configuration.ConfigurationManager.AppSettings.Get("AppRoot");
                var root = "@/Upload/App/";
                if (string.IsNullOrWhiteSpace(root))
                {
                    Json(KingResponse.GetErrorResponse("未设置文件保存根目录"));
                }
                string url = root + newName;//供下载该文件的链接
                var md5 = MD5Helper.GetMD5HashFromFile(path);//文件md5
                return Json(KingResponse.GetResponse(new { md5 = md5, url = url }));
            }
            return Json(KingResponse.GetErrorResponse("文件保存失败"));
        }

        [HttpPost]
        public ActionResult AddOrUpdateApp(tb_res_appversion model)
        {
            model.IsForce = Convert.ToInt32(Request.Form["IsForce"] == "on");
           
            if (!model.Version.StartsWith("V"))//版本存入数据库必须V开头
            {
                model.Version = "V" + model.Version;
            }
            bool success = false;
            if (model.APPVersionID.HasValue && model.APPVersionID.Value > 0)
            {
                success = Manage.Update<tb_res_appversion>(model);
            }
            else
            {
                model.CreateDate = DateTime.Now;
                success = Manage.Add(model) > 0;
            }
            if (success)
            {
                new CourseActivate.Activate.BLL.ActivateCourseBLL().Remove(RedisConfiguration.GetAppVersionKey(model.AppID.Value));
                string appRoot = ConfigurationManager.AppSettings["Webhost"];
                new ResourceBLL().CopyFileRequest(model.Url.Replace("@/",appRoot+"/"));
                return Json(KingResponse.GetResponse("操作成功"));
            }


            return Json(KingResponse.GetErrorResponse("操作失败"));
        }


        //List<tb_res_appversion> a = new List<tb_res_appversion> { new tb_res_appversion { APPVersionID = 1 } };
        //List<tb_res_appversion> b = new List<tb_res_appversion> { new tb_res_appversion { APPVersionID = 1 } };
        //var c = b.SequenceEqual(a, new compare());
        public class compare : IEqualityComparer<tb_res_appversion>
        {
            // 摘要: 
            //     确定指定的对象是否相等。
            //
            // 参数: 
            //   x:
            //     要比较的第一个对象。
            //
            //   y:
            //     要比较的第二个对象。
            //
            // 返回结果: 
            //     如果指定的对象相等，则为 true；否则为 false。
            //
            // 异常: 
            //   System.ArgumentException:
            //     x 和 y 的类型不同，它们都无法处理与另一个进行的比较。
            public bool Equals(tb_res_appversion x, tb_res_appversion y)
            {
                return x.APPVersionID == y.APPVersionID;
            }
            //
            // 摘要: 
            //     返回指定对象的哈希代码。
            //
            // 参数: 
            //   obj:
            //     System.Object，将为其返回哈希代码。
            //
            // 返回结果: 
            //     指定对象的哈希代码。
            //
            // 异常: 
            //   System.ArgumentNullException:
            //     obj 的类型为引用类型，obj 为 null。
            public int GetHashCode(tb_res_appversion obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
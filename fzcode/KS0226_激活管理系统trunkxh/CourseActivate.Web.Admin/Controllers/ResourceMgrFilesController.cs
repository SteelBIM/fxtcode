using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using CourseActivate.Resource.BLL;
using CourseActivate.Resource.Constract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ResourceMgrFilesController : BaseController
    {
        /// <summary>
        /// 当前课程的资源列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList(int bookid)
        {
            PageParameter<tb_res_resource> pageParameter = new PageParameter<tb_res_resource>();
            //pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            //pageParameter.PageSize = pagesize;

            pageParameter.OrderColumns = m => m.CreateTime;
            pageParameter.IsOrderByASC = 0;
            int total;
            // Expression<Func<tb_res_resource, tb_res_book, object>> joinOn = (m, n) => m.BookID == n.BookID;
            //IList<tb_res_resource> list = base.Manage.SelectPage<tb_res_resource, tb_res_book>(pageParameter, joinOn, out total);

            List<tb_res_resource> list = Manage.SelectSearch<tb_res_resource>(o => o.BookID == bookid);


            return Json(new { total = list.Count, rows = list });
        }

        [HttpPost]
        public JsonResult UpdateState(int resid, int status)
        {
            if (status == 0)//未启用
            {
                status = 1;//启用
            }
            else if (status == 1)
            {
                status = 2;//禁用
            }
            else if (status == 2)
            {
                status = 1;
            }

            //var res = Manage.Select<tb_res_resource>(resid);
            //res.Status = status;
            //res.UpdateTime = DateTime.Now;

            //  bool result = base.Manage.Update<tb_res_resource>(res);

          

            bool result = Manage.CustomUpdateEntity<tb_res_resource>(o => o.ResID.ToString(), new tb_res_resource { Status = status, ResID = resid, UpdateTime = DateTime.Now }, o => o.Status.ToString(), o => o.UpdateTime.ToString());

            var res = Manage.SelectSearch<tb_res_resource>(o => o.ResID == resid);
            if (res.Any())
            {
                int bookid = res[0].BookID.Value;
                new ResourceBLL().GetResJson(bookid, Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
            }

            if (result)
            {

                tb_res_resource resinfo = base.Manage.Select<tb_res_resource>(resid);
                if (resinfo != null)
                {
                    //修改redis资源缓存
                    new ResourceBLL().ResetRedisData(resinfo);
                }
            }
            return Json(new { Success = result });
        }


        public JsonResult ResDelete(int resid)
        {
         
            var res = base.Manage.Select<tb_res_resource>(resid);
            if (res == null)
            {
                return Json(KingResponse.GetErrorResponse("该资源已不存在!"));
            }
            if (res != null && res.Status > 0)
            {
                return Json(new { Success = false, ErrorMsg = "只有未启用过的资源才能删除!" });
            }
            int bookid = res.BookID.Value;
            bool success = base.Manage.Delete<tb_res_resource>(o => o.ResID == resid);
            if (success)
            {
                new ResourceBLL().RemoveRedisData(res);
                new ResourceBLL().GetResJson(bookid, Server.MapPath("~/Upload/BookResource/" + bookid + ".json"));
                string path = Server.MapPath("~/Upload/BookResource/" + res.ResMD5);
                if (Directory.Exists(path))
                {
                    System.IO.Directory.Delete(path, true);
                }

            }

            return Json(new { Success = true });
        }

        public JsonResult GetResByID(int resId)
        {
            var res = Manage.Select<tb_res_resource>(resId);
            return Json(res);
        }
    }
}
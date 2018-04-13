using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ResourceTypeConfigController : BaseController
    {
        // GET: ResourceTypeConfig
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ResourceTypeConfig_View(int pagesize, int pageindex)
        {
            PageParameter<tb_res_modular> pageParameter = new PageParameter<tb_res_modular>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.ModularID;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_res_modular> batches = Manage.SelectPage<tb_res_modular>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }
        public JsonResult ResourceTypeConfig_Add(string ModularName, string Remark)
        {
            tb_res_modular modular = new tb_res_modular();
            modular.ModularName = ModularName;
            modular.Remark = Remark;
            modular.CreateDate = DateTime.Now;
            modular.ModularLevel = 1;
            modular.ParentID = 0;
            modular.Status = true;
            if (Manage.Add<tb_res_modular>(modular) > 0)
            {
                return Json(KingResponse.GetResponse("成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("失败"));
            }
        }
        public int ResourceTypeConfigIsExist(int ModularID)
        {
            if (ModularID > 0)
                return base.GetTotalCount<tb_res_modular>(x => (x.ModularID == ModularID));
            return base.GetTotalCount<tb_res_modular>(x => (x.ModularID == ModularID));
        }
        public JsonResult ResourceTypeConfig_Update(int ModularID, string ModularName, string Remark)
        {
            if (ResourceTypeConfigIsExist(ModularID) > 0)
            {
                if (Manage.Update<tb_res_modular>(new { ModularName = ModularName, Remark = Remark }, t => t.ModularID == ModularID))
                {
                    return Json(KingResponse.GetResponse("更新成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("更新失败"));
                }
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("该条记录不存在"));
            }
        }
        public JsonResult ResourceTypeConfig_Delete(int ModularID)
        {
            if (Manage.Delete<tb_res_modular>(ModularID))
            {
                return Json(KingResponse.GetResponse("删除成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("操作失败"));
            }
        }
        public JsonResult GetResourceTypeById(int Id)
        {
            return Json(Manage.SelectSearch<tb_res_modular>((i => i.ModularID == Id)));
        }

        [HttpPost]
        public JsonResult GetAllResourceTypes()
        {
            return Json(Manage.SelectAll<tb_res_modular>());
        }
    }
}
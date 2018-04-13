using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class EditionConfigController : BaseController
    {
        // GET: EditionConfig
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult EditionConfig_View(int pagesize, int pageindex)
        {
            PageParameter<tb_code_edition> pageParameter = new PageParameter<tb_code_edition>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.EditionID;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<tb_code_edition> batches = Manage.SelectPage<tb_code_edition>(pageParameter, out total);
            return Json(new { total = total, rows = batches });
        }
        public JsonResult EditionConfig_Add(string EditionName, int subjectID, string Remark)
        {
            tb_code_edition edition = new tb_code_edition();
            edition.EditionName = EditionName;
            edition.SubjectID = subjectID;
            edition.Remark = Remark;
            edition.CreateTime = DateTime.Now;
            if (Manage.Add<tb_code_edition>(edition) > 0)
            {
                return Json(KingResponse.GetResponse("成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("失败"));
            }
        }
        public int EditionConfigIsExist(int EditionID)
        {
            if (EditionID > 0)
                return base.GetTotalCount<tb_code_edition>(x => (x.EditionID == EditionID));
            return base.GetTotalCount<tb_code_edition>(x => (x.EditionID == EditionID));
        }
        public JsonResult EditionConfig_Update(int EditionID, string EditionName, int subjectID, string Remark)
        {
            if (EditionConfigIsExist(EditionID) > 0)
            {
                if (Manage.Update<tb_code_edition>(new { EditionName = EditionName, SubjectID = subjectID, Remark = Remark }, t => t.EditionID == EditionID))
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
        public JsonResult EditionConfig_Delete(int EditionID)
        {
            if (Manage.Delete<tb_code_edition>(EditionID))
            {
                return Json(KingResponse.GetResponse("删除成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("操作失败"));
            }
        }
        public JsonResult GetEditionById(int Id)
        {
            return Json(Manage.SelectSearch<tb_code_edition>((i => i.EditionID == Id)));
        }
    }
}
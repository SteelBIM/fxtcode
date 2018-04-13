using CourseActivate.Account.Constract.VW;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ActivateTypeMgrController : BaseController
    {
        // GET: ActivateTypeMgr
        public ActionResult Index()
        {
            List<tb_publish> publish = Manage.SelectSearch<tb_publish>("status=1");
            ViewBag.PublishList = publish;
            List<tb_devicetype> device = Manage.SelectAll<tb_devicetype>();
            ViewBag.devicetypelist = device;
            return View();
        }
        [HttpPost]
        public JsonResult ActivateTypeMgr_View(int pagesize, int pageindex, string publishname, int type, int way)
        {
            List<Expression<Func<V_activateType, bool>>> exprlist = GetUserWheres(publishname, way, type);
            PageParameter<V_activateType> pageParameter = new PageParameter<V_activateType>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.activatetypeid;
            pageParameter.IsOrderByASC = 0;
            pageParameter.Wheres = exprlist;
            int total;
            IList<V_activateType> usre = base.Manage.SelectPage<V_activateType>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }

        /// <summary>
        /// 判断激活码类型是否存在
        /// </summary>
        /// <param name="activatetypeId"></param>
        /// <returns></returns>
        public int ActivateTypeIsExist(int activatetypeId)
        {
            if (activatetypeId > 0)
                return base.GetTotalCount<V_activateType>(x => (x.activatetypeid == activatetypeId));
            return base.GetTotalCount<V_activateType>(x => (x.activatetypeid == activatetypeId));
        }
        public JsonResult GetActivateById(int Id)
        {
            return Json(Manage.SelectSearch<V_activateType>((i => i.activatetypeid == Id)));
        }
        public JsonResult ActivateTypeMgr_Add(string activatetypename, int publishid, int type, int way, int devicenum, string remark)
        {
            tb_activatetype activate = new tb_activatetype();
            activate.activatetypename = activatetypename;
            activate.publishid = publishid;
            activate.type = type;
            activate.way = way;
            activate.status = 0;
            activate.devicenum = devicenum;
            activate.remark = remark;
            activate.createTime = DateTime.Now;
            int result = Manage.Add<tb_activatetype>(activate);
            if (result > 0)
            {
                activate.activatetypeid = result;
                #region redis同步
                new Activate.BLL.ActivateCourseBLL().SetActivateType(activate);
                #endregion
                return Json(KingResponse.GetResponse("成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("失败"));
            }
        }

        public JsonResult ActivateTypeMgr_Enable(int activatetypeid, int status)
        {
            bool update;
            if (status == 0 || status == 2)
            {
                update = Manage.Update<tb_activatetype>(new { status = 1 }, t => t.activatetypeid == activatetypeid);
            }
            else
            {
                update = Manage.Update<tb_activatetype>(new { status = 2 }, t => t.activatetypeid == activatetypeid);
            }
            if (update)
            {
                #region redis同步
                tb_activatetype typeinfo = Select<tb_activatetype>(activatetypeid);
                new Activate.BLL.ActivateCourseBLL().SetActivateType(typeinfo);
                #endregion
                return Json(KingResponse.GetResponse("成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("失败"));
            }
        }

        public JsonResult ActivateTypeMgr_Update(int activatetypeid, int devicenum, string remark)
        {
            if (ActivateTypeIsExist(activatetypeid) > 0)
            {
                if (Manage.Update<tb_activatetype>(new { devicenum = devicenum, remark = remark }, t => t.activatetypeid == activatetypeid))
                {
                    #region redis同步
                    tb_activatetype typeinfo = Select<tb_activatetype>(activatetypeid);
                    new Activate.BLL.ActivateCourseBLL().SetActivateType(typeinfo);
                    #endregion
                    return Json(KingResponse.GetResponse(""));
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
        public JsonResult ActivateTypeMgr_Delete(int activatetypeid)
        {
            if (Manage.Delete<tb_activatetype>(activatetypeid))
            {
                #region redis同步
                new Activate.BLL.ActivateCourseBLL().RemoveActivateType(activatetypeid);
                #endregion
                return Json(KingResponse.GetResponse("成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("失败"));
            }
        }

        public JsonResult GetPublish()
        {
            KingResponse res = new KingResponse();
            List<tb_publish> publish = Manage.SelectAll<tb_publish>();
            res.Data = publish;
            res.Success = true;
            return Json(res);
        }

        #region 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public List<Expression<Func<V_activateType, bool>>> GetUserWheres(string publishname, int way, int type)
        {

            List<Expression<Func<V_activateType, bool>>> exprlist = new List<Expression<Func<V_activateType, bool>>>();
            if (!string.IsNullOrEmpty(publishname) && publishname != "全部")
            {
                exprlist.Add(i => i.publishname == publishname);
            }
            if (way != 0)
            {
                exprlist.Add(i => i.way == way);
            }
            if (type != 0)
            {
                exprlist.Add(i => i.type == type);
            }
            return exprlist;
        }
        #endregion
    }
}
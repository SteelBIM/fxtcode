using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CourseActivate.Account.Constract.Models;
using CourseActivate.Core.Utility;
using CourseActivate.Account.Constract.VW;
using CourseActivate.Web.Admin.Models;

namespace CourseActivate.Web.Admin.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class AuthorityMgrController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AuthorityMgr_Add()
        {
            return View();
        }

        #region 获取action控制权限(用户view呈现操作按钮) GetcurrentAction()
        /// <summary>
        /// 获取action控制权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }
        #endregion


        #region  判断角色是否存在 GroupNameIsExist(int groupid, string groupname)
        /// <summary>
        /// 判断角色是否存在 GroupNameIsExist(int groupid, string groupname)
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public int GroupNameIsExist(int groupid, string groupname)
        {
            if (groupid > 0)
                return base.GetTotalCount<com_group>(x => (x.groupname == groupname && x.groupid != groupid && x.delflg == 0));

            return base.GetTotalCount<com_group>(x => (x.groupname == groupname && x.delflg == 0));
        }
        #endregion

        #region  获取所有的操作权限(新增时)GetAllAction()
        /// <summary>
        /// 获取所有的操作权限(新增修改时使用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllAction()
        {
            return Json(base.SelectAll<vw_allaction>());
        }
        #endregion

        #region 根据ID获取角色 GetGroupById(string Id)
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGroupById(string Id)
        {
            return Json(base.Select<com_group>(Id));
        }
        #endregion

        #region 添加角色并赋操作权限 AuthorityMgr_Add(string groupname, int dataauthority, string actions)
        /// <summary>
        /// 添加角色并赋操作权限
        /// </summary>
        /// <param name="groupname"></param>
        /// <param name="dataauthority">数据查看权限</param>
        /// <param name="actions"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuthorityMgr_Add(string groupname, string actions)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Add) //没有新增权限
            {
                res.ErrorMsg = "您没有操作权限~";
            }
            com_group subdata = new com_group() { groupname = groupname, creatername = masterinfo.mastername, description = "", delflg = 0, createtime = DateTime.Now.ToString() };
            int groupid = base.Add<com_group>(subdata);
            if (groupid > 0)
            {
                List<com_actiongroup> entitys = new List<com_actiongroup>();
                if (!string.IsNullOrEmpty(actions))
                {
                    string[] array = actions.TrimEnd(',').Split(',');
                    if (array.Length > 0)
                    {
                        foreach (string row in array)
                            entitys.Add(new com_actiongroup() { groupid = groupid, actionname = row });
                        if (entitys.Count > 0 && base.InsertRange(entitys) != null)
                        {
                            res.Success = true;
                        }
                        else
                        {
                            res.ErrorMsg = "角色添加成功，操作权限添加失败！请进入编辑~";
                        }
                    }

                }
            }
            return Json(res);
        }
        #endregion



        [HttpPost]
        public JsonResult AuthorityMgr_View(int pagesize, int pageindex)
        {
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<com_group> pageParameter = new PageParameter<com_group>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.Where = t => t.delflg == 0;
            pageParameter.OrderColumns = t1 => t1.groupid;
            int total;
            IList<com_group> data = base.Manage.SelectPage<com_group>(pageParameter, out total);
            return Json(new { total = total, rows = data });
        }

        #region 角色删除 AuthorityMgr_Del(int groupid)
        /// <summary>
        /// 角色删除  
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuthorityMgr_Del(int groupid)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Del)
                res.ErrorMsg = "您没有删除权限~";
            if (base.GetTotalCount<com_master>(m => m.groupid == groupid) > 0)
                res.ErrorMsg = "删除失败！ 该角色下有用户~";
            else if (base.LogicDelete<com_group>(g => g.groupid == groupid, "delflg"))
                res.Success = true;
            else
                res.ErrorMsg = "删除失败！请稍后重试~";
            return Json(res);
        }
        #endregion


        #region 修改 AuthorityMgr_Edit(int groupid, string groupname, int dataauthority, string actions)
        [HttpPost]
        public int AuthorityMgr_Edit(int groupid, string groupname, int dataauthority, string actions)
        {
            if (!action.Edit) //没有编辑权限
            {
                return -1;
            }
            com_group groupdata = new com_group() { groupid = groupid, groupname = groupname, dataauthority = dataauthority, createtime = DateTime.Now.ToString() };
            if (base.Update<com_group>(groupdata))
            {
                List<com_actiongroup> entitys = new List<com_actiongroup>();
                if (!string.IsNullOrEmpty(actions))
                {
                    string[] array = actions.TrimEnd(',').Split(',');
                    if (array.Length > 0)
                        foreach (string row in array)
                            entitys.Add(new com_actiongroup() { groupid = groupid, actionname = row });
                }
                if (entitys.Count > 0)//执行事务
                {
                    return base.Manage.UpdateActionBusinessAffairs<com_actiongroup>(ag => ag.groupid == groupid, entitys);
                }
            }
            return 0;
        }
        #endregion

        #region  获取角色的权限（修改时）  GetGroupAction(int groupid)
        /// <summary>
        /// 获取角色的权限
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGroupAction(int groupid)
        {
            return Json(base.Manage.SelectSearch<vw_action>(x => x.groupid == groupid, 0, " parentsequence,sequence "));
        }
        #endregion
    }
}

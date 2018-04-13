using System.Collections.Generic;
using System.Web.Mvc;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;
using System;
using KSWF.WFM.BLL;
using System.Linq.Expressions;

namespace KSWF.Web.Admin.Controllers
{
    public class AgentPolicyMgrController : BaseController
    {
        //
        // 代理商商务策略

        public ActionResult Index()
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

        /// <summary>
        /// 当前用户允许查询的员工列表
        /// </summary>
        /// <returns></returns>
        private List<cfg_keyvalue> GetMasterList()
        {
            List<cfg_keyvalue> list = new List<cfg_keyvalue>();
            List<com_master> vwlist = base.SelectSearch<com_master>(t => (t.pagentid == masterinfo.agentid && t.mastertype == 1 && t.state == 0));
            foreach (com_master u in vwlist)
            {
                list.Add(new cfg_keyvalue { Key = u.agentname, Value = u.agentname });
            }
            return list;
        }
        public List<cfg_keyvalue> GetChannelPrincipal()
        {

            List<string> listin = new List<string>();
            listin.Add("4");
            listin.Add("5");
            List<cfg_keyvalue> list = new List<cfg_keyvalue>();
            List<com_master> vwlist = base.SelectIn<com_master>(t => (t.agentid == masterinfo.agentid && t.mastertype == 0 && t.state == 0), "groupid", listin);
            foreach (com_master u in vwlist)
            {
                list.Add(new cfg_keyvalue { Key = u.truename, Value = u.truename });
            }
            return list;
        }

        public JsonResult AgentPolicyMgr_View(int pagesize, int pageindex, string agentname, int deptid)
        {

            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_agent> pageParameter = new PageParameter<vw_agent>();

            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            List<Expression<Func<vw_agent, bool>>> exprlist = new List<Expression<Func<vw_agent, bool>>>();
            exprlist.Add(t => t.pagentid == masterinfo.agentid);
            exprlist.Add(t => t.state == 0);
            if (!string.IsNullOrEmpty(agentname))
                exprlist.Add(t => t.agentname.Contains(agentname) || t.parentname.Contains(agentname));

            Recursive give = new Recursive();
            if (masterinfo.dataauthority == (int)Dataauthority.本人 || masterinfo.dataauthority == (int)Dataauthority.本人下级代理商)
            {
                exprlist.Add(t => t.masterid == masterinfo.masterid);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 || masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                if (deptid == 0)
                    deptid = masterinfo.deptid;
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());//添加本部门
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
                if (deptid == masterinfo.deptid && masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)//点击的部门等于用户部门
                {
                    exprlist.Add((t => t.masterid == masterinfo.masterid || t.deptid != deptid));
                }
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                if (deptid == 0)
                    deptid = masterinfo.deptid;
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());//添加本部门
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
            }
            else
            {

                if (deptid > 0)
                {

                    List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                    InIds.Add(deptid.ToString());
                    if (InIds != null && InIds.Count > 0)
                    {
                        pageParameter.Field = "deptid";
                        pageParameter.In = InIds;
                    }
                }
            }
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_agent> usre = base.Manage.SelectPage<vw_agent>(pageParameter, out total);
            EmployeeController employee = new EmployeeController();
            for (int i = 0; i < usre.Count; i++)
            {
                usre[i].responsiblearea = employee.GetArea(usre[i].mastername);

                vw_user vwuser = GetPolicy(usre[i].mastername);
                usre[i].rffectivePolicy = vwuser.rffectivePolicy;
                usre[i].notrffectivePolicy = vwuser.notrffectivePolicy;
                usre[i].lastoperationtime = vwuser.lastoperationtime;
            }
            return Json(new { total = total, rows = usre });
        }
        /// <summary>
        /// 获取用户策略
        /// </summary>
        /// <returns></returns>
        public vw_user GetPolicy(string mastername)
        {
            vw_user user = new vw_user();
            List<vw_masterbpolicypr> list = base.SelectSearch<vw_masterbpolicypr>(x => x.mastername == mastername);
            if (list != null && list.Count > 0)
            {
                user.lastoperationtime = list[0].createtime.ToString("yyyy-MM-dd");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].effectivestatus == "0")
                    {
                        user.notrffectivePolicy += list[i].pllicyname + ",";
                    }
                    else if (list[i].effectivestatus == "1")
                    {
                        user.rffectivePolicy += list[i].pllicyname + ",";
                    }
                }


            }
            else
            {
                user.rffectivePolicy = "";
                user.notrffectivePolicy = "";
                user.lastoperationtime = "";
            }
            if (user.rffectivePolicy == null)
            {
                user.rffectivePolicy = "";
            }
            else
            {
                user.rffectivePolicy = user.rffectivePolicy.TrimEnd(',');
            }
            if (user.lastoperationtime == null)
            {
                user.lastoperationtime = "";
            }
            if (user.notrffectivePolicy == null)
            {
                user.notrffectivePolicy = "";
            }
            else
            {

                user.notrffectivePolicy = user.notrffectivePolicy.TrimEnd(',');
            }

            return user;
        }


    }
}

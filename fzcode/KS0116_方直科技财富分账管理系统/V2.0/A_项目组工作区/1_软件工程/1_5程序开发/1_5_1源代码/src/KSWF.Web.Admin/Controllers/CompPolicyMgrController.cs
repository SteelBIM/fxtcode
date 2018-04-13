using System.Collections.Generic;
using System.Web.Mvc;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using System;
using KSWF.WFM.BLL;
using System.Linq.Expressions;



namespace KSWF.Web.Admin.Controllers
{
    public class CompPolicyMgrController : BaseController
    {
        //
        //员工商务策略设置

        public ActionResult Index()
        {
            //ViewBag.MasterList = GetMasterList();
            return View();
        }
        public ActionResult CompPolicyMgr_Add()
        {
            ViewBag.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.ChannelList = GetAgendChannel();
            return View();
        }
        //2017-06-16
        //代理商及员商务策略批量设置
        public ActionResult CompPolicyMgrbatch()
        {
            if (Session["masternames"] != null)
            {
                ViewBag.MasterNames = Session["masternames"];
            }
            ViewBag.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.ChannelList = GetAgendChannel();
            Session["masternames"] = null;
            return View();
        }

        public void Jump(string masternames)
        {
            Session["masternames"] = masternames;
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
        [HttpPost]
        public JsonResult GetMasterList(int deptid)
        {
            if (deptid > 0)
            {
                Recursive sive = new Recursive();
                List<string> deptlist = sive.GetDeptNodeId(deptid, masterinfo.agentid);
                deptlist.Add(deptid.ToString());
                List<vw_user> dlist = base.SelectIn<vw_user>(t => (t.agentid == masterinfo.agentid && t.areanumber > 0 && t.state == 0 && t.mastertype == 0), "deptid", deptlist);
                return Json(dlist);
            }
            List<vw_user> vwlist = base.SelectSearch<vw_user>(t => (t.agentid == masterinfo.agentid && t.areanumber > 0 && t.state == 0 && t.mastertype == 0));
            return Json(vwlist);
        }
        [HttpPost]
        /// <summary>
        /// 获取商务策略
        /// </summary>
        /// <param name="ptype">0员工商务策略  1代理商商务策略</param>
        /// <param name="productid">产品的key</param>
        /// <returns></returns>
        public JsonResult getbpolicyp(int ptype, int productid)
        {
            if (productid > 0)
                return Json(base.SelectSearch<KSWF.WFM.Constract.Models.cfg_bpolicy>(t => t.ptype == ptype && t.delflg == 0 && t.agentid == masterinfo.agentid && t.pid == productid));
            return Json(base.SelectSearch<KSWF.WFM.Constract.Models.cfg_bpolicy>(t => t.ptype == ptype && t.delflg == 0 && t.agentid == masterinfo.agentid));
        }

        [HttpPost]
        /// <summary>
        /// 获取员工商务策略
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult getmasterbpolicypbyid(int id)
        {
            return Json(base.SelectSearch<vw_masterbpolicypr>(t => t.id == id));

        }
        [HttpPost]
        public JsonResult CompPolicy_Add(int id, string mastername, int bid, DateTime startdate, int product)
        {
            join_masterbpolicypr info = new join_masterbpolicypr() { id = id, mastername = mastername, bid = bid, startdate = startdate, createid = masterinfo.masterid, createtime = DateTime.Now, grouplogo = "" };
            Core.Utility.KingResponse res = new KingResponse();
            if (id > 0)
            {
                if (!action.Add)
                {
                    res.ErrorMsg = "您没有编辑权限~";
                }
                else
                {
                    if (base.Update<join_masterbpolicypr>(info))
                        res.Success = true;
                }
            }
            else
            {

                if (info.startdate > DateTime.Now)//未生效
                {
                    List<vw_masterbpolicypr> notEffectivemasterbpolicypr = base.Manage.SelectSearch<vw_masterbpolicypr>(t => t.mastername == info.mastername && t.pid == product && t.startdate > DateTime.Now);
                    {
                        if (notEffectivemasterbpolicypr.Count > 0)
                        {
                            info.id = notEffectivemasterbpolicypr[0].id;
                            if (base.Update<join_masterbpolicypr>(info))
                                res.Success = true;
                        }
                        else
                        {
                            info.createid = masterinfo.masterid;
                            info.createtime = DateTime.Now;
                            if (base.Add<join_masterbpolicypr>(info) > 0)
                                res.Success = true;
                        }
                    }
                }
                else
                {

                    List<vw_masterbpolicypr> Effectivemasterbpolicypr = base.Manage.SelectSearch<vw_masterbpolicypr>(t => t.mastername == info.mastername && t.pid == product);
                    if (Effectivemasterbpolicypr.Count > 0)
                    {
                        //base.Delete<join_masterbpolicypr>(t => t.mastername == info.mastername && t.bid == bid && t.startdate <= DateTime.Now);//不管有没有未过期策略都删除
                        info.id = Effectivemasterbpolicypr[0].id;
                        if (base.Update<join_masterbpolicypr>(info))
                            res.Success = true;
                        if (Effectivemasterbpolicypr.Count > 1)
                        {
                            string Ids = "";
                            for (int i = 1; i < Effectivemasterbpolicypr.Count; i++)
                            {
                                Ids += Effectivemasterbpolicypr[i].id.ToString() + ",";
                            }
                            if (!string.IsNullOrEmpty(Ids))
                            {
                                base.DeleteMore<join_masterbpolicypr>(Ids.TrimEnd(','));
                            }
                        }
                    }
                    else
                    {
                        info.createid = masterinfo.masterid;
                        info.createtime = DateTime.Now;
                        if (base.Add<join_masterbpolicypr>(info) > 0)
                            res.Success = true;
                    }
                }
            }
            return Json(res);
        }
        [HttpPost]
        public bool CompPolicyMgr_Del(int Id, string pllicyname)
        {
            join_masterbpolicypr masterbpolicypr = base.Select<join_masterbpolicypr>(Id.ToString());
            if (base.DeleteById<join_masterbpolicypr>(Id))
            {
                GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
                give.MasterChangeInfoAdd(masterbpolicypr.mastername, (int)KSWF.Web.Admin.Models.ChangeType.策略变更, masterbpolicypr.bid.ToString(), pllicyname, "", "删除", masterinfo.mastername);
                return true;
            }
            return false;
        }

        [HttpPost]
        public JsonResult CompPolicyMgrAdd_View(string mastername, string selproductname, string status)
        {
            Recursive give = new Recursive();
            if (!action.View)
                return Json("");
            List<Expression<Func<vw_masterbpolicypr, bool>>> exprlist = new List<Expression<Func<vw_masterbpolicypr, bool>>>();
            exprlist.Add(t => t.mastername == mastername);
            if (!string.IsNullOrEmpty(selproductname))
            {
                exprlist.Add(t => t.pid == Convert.ToInt32(selproductname));
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "1")
                {
                    exprlist.Add(t => t.startdate < DateTime.Now);
                }
                else if (status == "0")
                {
                    exprlist.Add(t => t.startdate > DateTime.Now);
                }
            }




            List<vw_masterbpolicypr> masterbpolicypr = base.Manage.SelectSearchs<vw_masterbpolicypr>(exprlist);


            if (masterbpolicypr != null && masterbpolicypr.Count > 0)
            {
                for (int i = masterbpolicypr.Count - 1; i >= 0; i--)
                {
                    if (masterbpolicypr[i].startdate < DateTime.Now)
                    {
                        masterbpolicypr[i].effectivestatus = "1";
                        int number = 0;
                        for (int a = 0; a < masterbpolicypr.Count; a++)
                        {
                            if (masterbpolicypr[i].pid == masterbpolicypr[a].pid && masterbpolicypr[a].startdate < DateTime.Now)
                            {
                                number++;
                            }
                        }
                        if (number > 1)
                        {
                            int bid = masterbpolicypr[i].bid;
                            base.Delete<join_masterbpolicypr>(t => t.mastername == mastername && t.bid == bid && t.startdate < DateTime.Now);
                            masterbpolicypr.RemoveAt(i);
                        }
                    }
                    else
                    {
                        masterbpolicypr[i].effectivestatus = "0";
                    }
                }
            }
            return Json(new { total = masterbpolicypr.Count, rows = masterbpolicypr });
        }
        public List<Expression<Func<vw_user, bool>>> GetUserWheres(string mastername, int deptid, out string Flide, out List<string> inids)
        {
            Recursive give = new Recursive();
            List<Expression<Func<vw_user, bool>>> exprlist = new List<Expression<Func<vw_user, bool>>>();
            Flide = "";
            inids = null;
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
                    Flide = "deptid";
                    inids = InIds;
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
                    Flide = "deptid";
                    inids = InIds;
                }
            }
            else
            {
                if (deptid > 0)
                {
                    List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                    InIds.Add(deptid.ToString());//添加本部门
                    if (InIds != null && InIds.Count > 0)
                    {
                        Flide = "deptid";
                        inids = InIds;
                    }
                }
            }
            return exprlist;
        }
        #region 分页预览  Employee_View(int pagesize, int pageindex, int deptid, string mastername)
        [HttpPost]
        public JsonResult CompPolicyMgr_View(int pagesize, int pageindex, int deptid, string mastername, int type)
        {
            Recursive give = new Recursive();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_user> pageParameter = new PageParameter<vw_user>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;

            string Flids = "";
            List<string> InIds = null;
            List<Expression<Func<vw_user, bool>>> exprlist = GetUserWheres(mastername, deptid, out Flids, out InIds);
            if (type == 0)
            {
                exprlist.Add(t => t.areanumber > 0);
            }
            exprlist.Add(t => t.mastertype == 0 && t.state == 0);
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(mastername))
                exprlist.Add(t => t.truename.Contains(mastername));
            pageParameter.Field = Flids;
            pageParameter.In = InIds;
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_user> usre = base.Manage.SelectPage<vw_user>(pageParameter, out total);
            EmployeeController exployee = new EmployeeController();
            for (int i = 0; i < usre.Count; i++)
            {
                usre[i].responsiblearea = exployee.GetArea(usre[i].mastername);
                if (type == 0)//商务策略
                {
                    vw_user vwuser = GetPolicy(usre[i].mastername);
                    usre[i].rffectivePolicy = vwuser.rffectivePolicy;
                    usre[i].notrffectivePolicy = vwuser.notrffectivePolicy;
                    usre[i].lastoperationtime = vwuser.lastoperationtime;
                }
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
                    if (list[i].startdate > DateTime.Now)
                    {
                        user.notrffectivePolicy += list[i].pllicyname + ",";
                    }
                    else
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


        #endregion


        /// <summary>
        /// 批量编辑呈现
        /// </summary>
        /// <param name="masternames"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BatchUpdate_View(string mastername, int viewnumber)
        {
            if (viewnumber > 0)
            {
                Recursive give = new Recursive();
                if (!action.View)
                    return Json("");
                List<Expression<Func<vw_masterbpolicypr, bool>>> exprlist = new List<Expression<Func<vw_masterbpolicypr, bool>>>();
                exprlist.Add(t => t.mastername == mastername);
                List<vw_masterbpolicypr> masterbpolicypr = base.Manage.SelectSearch<vw_masterbpolicypr>(t => t.mastername == mastername, viewnumber, " createtime desc ");
                return Json(new { total = masterbpolicypr.Count, rows = masterbpolicypr });
            }
            List<vw_masterbpolicypr> masterbpolicyp = new List<vw_masterbpolicypr>();
            return Json(new { total = 0, rows = masterbpolicyp });
        }

        public JsonResult BatchCompPolicy_Add(int id, string masternames, int bid, DateTime startdate, int product, string grouplogo)
        {
            Core.Utility.KingResponse res = new KingResponse();
            DateTime CurrentTime = DateTime.Now;//当前时间 确保同一时间添加 修改时根据时间和用户名修改
            if (!string.IsNullOrEmpty(masternames))
            {
                string[] array = masternames.Split(',');

                if (!string.IsNullOrEmpty(grouplogo) && id > 0)
                {
                    #region 编辑
                    if (!action.Add)
                    {
                        res.ErrorMsg = "您没有编辑权限~";
                        return Json(res);
                    }
                    else
                    {
                        for (int n = 0; n < array.Length; n++)
                        {
                            var obj = new { bid = bid, startdate = startdate, createid = masterinfo.masterid, createtime = CurrentTime };
                            if (base.Update<join_masterbpolicypr>(obj, t => t.mastername == array[n] && t.grouplogo == grouplogo))
                            {

                            }
                            else
                            {
                                res.ErrorMsg = "编辑异常，请重试~";
                                return Json(res);
                            }
                        }
                    }
                    #endregion

                }
                else
                {
                    grouplogo = Core.Utility.PublicHelp.AgentId();
                    for (int n = 0; n < array.Length; n++)
                    {
                        string mastername = array[n];
                        join_masterbpolicypr info = new join_masterbpolicypr() { mastername = mastername, bid = bid, startdate = startdate, createid = masterinfo.masterid, createtime = CurrentTime, grouplogo = grouplogo };
                        if (info.startdate > DateTime.Now)//未生效
                        {
                            #region 未生效
                            List<vw_masterbpolicypr> notEffectivemasterbpolicypr = base.Manage.SelectSearch<vw_masterbpolicypr>(t => t.mastername == info.mastername && t.pid == product && t.startdate > DateTime.Now);
                            {
                                if (notEffectivemasterbpolicypr.Count > 0)
                                {
                                    info.id = notEffectivemasterbpolicypr[0].id;
                                    if (base.Update<join_masterbpolicypr>(info))
                                    { }
                                    else
                                    {
                                        res.ErrorMsg = "设置异常，请重试~";
                                        return Json(res);
                                    }
                                }
                                else
                                {
                                    info.createid = masterinfo.masterid;
                                    info.createtime = DateTime.Now;
                                    if (base.Add<join_masterbpolicypr>(info) > 0)
                                    { }
                                    else
                                    {
                                        res.ErrorMsg = "设置异常，请重试~";
                                        return Json(res);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            List<vw_masterbpolicypr> Effectivemasterbpolicypr = base.Manage.SelectSearch<vw_masterbpolicypr>(t => t.mastername == info.mastername && t.pid == product);
                            if (Effectivemasterbpolicypr.Count > 0)
                            {
                                #region 有策略
                                //base.Delete<join_masterbpolicypr>(t => t.mastername == info.mastername && t.bid == bid && t.startdate <= DateTime.Now);//不管有没有未过期策略都删除
                                info.id = Effectivemasterbpolicypr[0].id;
                                if (base.Update<join_masterbpolicypr>(info))
                                    res.Success = true;
                                if (Effectivemasterbpolicypr.Count > 1)
                                {
                                    string Ids = "";
                                    for (int i = 1; i < Effectivemasterbpolicypr.Count; i++)
                                    {
                                        Ids += Effectivemasterbpolicypr[i].id.ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Ids))
                                    {
                                        base.DeleteMore<join_masterbpolicypr>(Ids.TrimEnd(','));
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 新增
                                info.createid = masterinfo.masterid;
                                info.createtime = DateTime.Now;
                                if (base.Add<join_masterbpolicypr>(info) > 0)
                                { }
                                else
                                {
                                    res.ErrorMsg = "新增异常，请重试~";
                                    return Json(res);
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            res.Success = true;
            return Json(res);
        }
        [HttpPost]
        public bool BatchCompPolicyMgr_Del(string grouplogo)
        {
            return base.Delete<join_masterbpolicypr>(t => t.grouplogo == grouplogo);
        }
    }
}

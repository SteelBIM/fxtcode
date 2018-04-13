using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;


namespace KSWF.Web.Admin.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Order/
        Models.Recursive rec = new Models.Recursive();
        public ActionResult Index()
        {
            ViewBag.CatogoryList = KeyValueManage.GetCatogoryData();
            ViewBag.ChannelList = KeyValueManage.GetChannleData();
            ViewBag.PayTypeList = KeyValueManage.GetPayTypeData();
            ViewBag.QudaoList = KeyValueManage.GetQudaoData();
            ViewBag.PayType = KeyValueManage.GetPayType();
            ViewBag.DeptList = "";
            return View();
        }
        #region 获取员工
        public JsonResult GetEmployee(int deptid)
        {
            Recursive give = new Recursive();
            string Flids = "";
            List<string> InIds = null;
            List<Expression<Func<com_master, bool>>> exprlist = GetUserWheres(deptid, out Flids, out InIds);
            List<com_master> EmploueeList = base.Manage.SelectSearchs<com_master>(exprlist, "deptid", InIds);
            return Json(EmploueeList);
        }
        public List<Expression<Func<com_master, bool>>> GetUserWheres(int deptid, out string Flide, out List<string> inids)
        {
            Recursive give = new Recursive();
            List<Expression<Func<com_master, bool>>> exprlist = new List<Expression<Func<com_master, bool>>>();
            exprlist.Add(t => t.state == 0 && t.mastertype == 0 && t.agentid == masterinfo.agentid);
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
        #endregion




        #region 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [AuthenticationAttribute(IsCheck = false)]
        public List<Expression<Func<vw_agent, bool>>> GetUserWheres(int dataauthority, int deptid, out string Flide, out List<string> inids)
        {
            Recursive give = new Recursive();
            List<Expression<Func<vw_agent, bool>>> exprlist = new List<Expression<Func<vw_agent, bool>>>();

            if (deptid == 1)
                deptid = 0;

            Flide = "";
            inids = null;
            exprlist.Add(t => t.state == 0);
            if (dataauthority == (int)Dataauthority.全部)
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
                exprlist.Add(t => t.pagentid == masterinfo.agentid);
            }
            else
            {
                if (dataauthority == (int)Dataauthority.本人下级代理商)
                {
                    exprlist.Add(t => (t.parentid == masterinfo.masterid));
                }
                else if (dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 || dataauthority == (int)Dataauthority.本人下级部门下级代理商)
                {
                    int did = masterinfo.deptid;
                    if (deptid > 0)
                        did = deptid;
                    List<string> InIds = give.GetDeptNodeId(did, masterinfo.agentid);
                    if (dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 && deptid == 0)
                        InIds.Add(masterinfo.deptid.ToString());
                    else
                        InIds.Add(did.ToString());
                    List<vw_deptAgent> listmaster = base.SelectIn<vw_deptAgent>("deptid", InIds);
                    Flide = "masterid";
                    List<string> masterinids = new List<string>();
                    if (listmaster != null && listmaster.Count > 0)
                    {
                        for (int i = 0; i < listmaster.Count; i++)
                        {
                            masterinids.Add(listmaster[i].masterid.ToString());
                        }
                    }
                    else
                    {
                        masterinids.Add("");
                    }
                    inids = masterinids;
                }
            }
            return exprlist;
        }
        #endregion

        #region 获取代理商
        public JsonResult GetAgent(int deptid)
        {
            string flide = "";
            List<string> idslist = new List<string>();
            List<Expression<Func<vw_agent, bool>>> exprlist = GetUserWheres(masterinfo.dataauthority, deptid, out   flide, out idslist);
            List<vw_agent> Agent = base.Manage.SelectSearchs<vw_agent>(exprlist, flide, idslist);
            return Json(Agent);
        }
        #endregion
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        public JsonResult GetParentAreaList()
        {
            if (masterinfo.dataauthority == 0 || masterinfo.dataauthority == 2)
            {
                var areaList = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == masterinfo.deptid);
            }
            return Json(KingResponse.GetResponse(masterinfo.deptid));
        }


        [HttpPost]
        public JsonResult GetOrderPageList(int pagesize, int pageindex, [System.Web.Http.FromBody]OrderCondition ocInfo)
        {
            if (UserIdentity == 1 && masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                PageParameter<vw_order> param = new PageParameter<vw_order>();
                param.PageIndex = setpageindex(pageindex, pagesize);
                param.PageSize = pagesize;
                param.OrderColumns = T => T.o_datetime;
                param.IsOrderByASC = 0;
                List<string> deptids = null;
                param.Wheres = GetAgentOrderCondition(ocInfo, out deptids);
                param.In = deptids;
                param.Field = " m_deptid";
                int totalcount = 0;
                IList<vw_order> list2 = base.OrderManage.SelectPage<vw_order>(param, out totalcount);
                return Json(new { total = totalcount, rows = list2 });
            }

            PageParameter<vw_kswforder> param2 = new PageParameter<vw_kswforder>();
            param2.PageIndex = setpageindex(pageindex, pagesize);
            param2.PageSize = pagesize;
            param2.OrderColumns = T => T.o_datetime;
            param2.IsOrderByASC = 0;
            List<string> deptids2 = null;
            param2.Wheres = GetAgentOrderCondition2(ocInfo, out deptids2);
            param2.In = deptids2;
            param2.Field = " m_deptid";
            int totalcount2 = 0;

            IList<vw_kswforder> list = base.OrderManage.SelectPage<vw_kswforder>(param2, out totalcount2);
            return Json(new { total = totalcount2, rows = list });
        }

        public JsonResult GetOrdersTotal([System.Web.Http.FromBody]OrderCondition ocInfo)
        {
            decimal? paycount = 0M;
            int ordernumber = 0;
            if (UserIdentity == 1 && masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                PageParameter<vw_order> param = new PageParameter<vw_order>();
                List<string> deptids = null;
                param.Wheres = GetAgentOrderCondition(ocInfo, out deptids);
                param.In = deptids;
                param.Field = " m_deptid ";
                List<vw_order> list = base.OrderManage.SelectSearchs<vw_order>(param.Wheres, param.Field, param.In);
                if (list != null && list.Count > 0)
                {
                    ordernumber = list.Count;
                    for (int i = 0; i < list.Count; i++)
                    {
                        paycount += list[i].o_payamount;
                    }
                }
                return Json(new { ordernumber = ordernumber, paycount = paycount });
            }
            PageParameter<vw_kswforder> param2 = new PageParameter<vw_kswforder>();
            List<string> deptids2 = null;
            param2.Wheres = GetAgentOrderCondition2(ocInfo, out deptids2);
            param2.In = deptids2;
            param2.Field = " m_deptid ";


            List<vw_kswforder> list2 = base.OrderManage.SelectSearchs<vw_kswforder>(param2.Wheres, param2.Field, param2.In);
            if (list2 != null && list2.Count > 0)
            {
                ordernumber = list2.Count;
                for (int i = 0; i < list2.Count; i++)
                {
                    paycount += list2[i].o_payamount;
                }
            }
            return Json(new { ordernumber = ordernumber, paycount = paycount });
        }



        private List<Expression<Func<vw_kswforder, bool>>> GetAgentOrderCondition2(OrderCondition ocInfo, out List<string> depts)
        {
            List<Expression<Func<vw_kswforder, bool>>> expression = new List<Expression<Func<vw_kswforder, bool>>>();

            if (UserIdentity == 1 || masterinfo.dataauthority != (int)Dataauthority.全部)
            {
                expression.Add(t => t.o_bonus > 0 || t.o_totype == 2);
            }

            Recursive give = new Recursive();
            int deptid = masterinfo.deptid;
            List<string> InIds = new List<string>();
            if (ocInfo.Dept > 0)
            {
                deptid = Convert.ToInt32(ocInfo.Dept);
                InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
            }

            if (masterinfo.mastertype == 1 || masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                //if (deptid > 0)
                //{
                //    InIds.Add(deptid.ToString());
                //}
                expression.Add(t => t.agentid == masterinfo.agentid);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商)
            {
                if (InIds != null && InIds.Count > 0)
                {

                }
                else
                {
                    InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                    InIds.Add(masterinfo.deptid.ToString());
                }
                depts = InIds;
                expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                //depts = InIds;
                //if (InIds != null)
                //{
                //    expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
                //}
                //else
                //{
                //    expression.Add(t => t.m_mastername == masterinfo.mastername);
                //}
                if (InIds != null && InIds.Count > 0)
                {

                }
                else
                {
                    InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                }
                depts = InIds;
                if (depts != null && depts.Count > 0)
                    expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
                else
                    expression.Add(t => t.m_mastername == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级代理商)
            {
                expression.Add(t => t.parentname == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人)
            {
                expression.Add(t => t.m_mastername == masterinfo.mastername);
            }
            depts = InIds;


            if (ocInfo != null)
            {
                if (!string.IsNullOrEmpty(ocInfo.SearchKey))
                {
                    switch (ocInfo.SearchType)
                    {
                        case 0:
                            expression.Add(i => i.o_id == ocInfo.SearchKey.Trim());
                            break;
                        case 1:
                            expression.Add(i => i.u_teachername == ocInfo.SearchKey.Trim());
                            break;
                    }
                }
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.o_datetime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.o_datetime <= ocInfo.endDate);
                }
                if (ocInfo.PayType.HasValue)
                {
                    expression.Add(i => i.o_feetype == ocInfo.PayType);
                }
                if (ocInfo.SchoolID.HasValue)
                {
                    expression.Add(i => i.schoolid == ocInfo.SchoolID);
                }
                if (ocInfo.Version.HasValue)
                {
                    if (Convert.ToInt32(ocInfo.Version.Value) > 0)
                        expression.Add(i => i.p_versionid.Contains("," + ocInfo.Version.ToString() + ","));
                }


                //Version
                if (ocInfo.AreaCode.HasValue)
                {
                    string code = "";
                    string[] strs = new string[4];
                    strs[0] = ocInfo.AreaCode.ToString().Substring(0, 2);
                    strs[1] = ocInfo.AreaCode.ToString().Substring(2, 2);
                    strs[2] = ocInfo.AreaCode.ToString().Substring(4, 2);
                    strs[3] = ocInfo.AreaCode.ToString().Substring(6);
                    for (int i = 3; i >= 0; i--)
                    {
                        if (int.Parse(strs[i]) != 0)
                        {
                            code = strs[i] + code;
                        }
                    }
                    expression.Add(i => i.districtid.ToString().StartsWith(code));
                }
                if (ocInfo.GradeID.HasValue)
                {
                    expression.Add(i => i.gradeid == ocInfo.GradeID);
                }
                Guid g;
                if (!string.IsNullOrEmpty(ocInfo.ClassID) && Guid.TryParse(ocInfo.ClassID, out g))
                {
                    expression.Add(i => i.classid == g);
                }
                if (ocInfo.ChannelID.HasValue)
                {
                    expression.Add(i => i.channel == ocInfo.ChannelID.Value);
                }
                if (!string.IsNullOrEmpty(ocInfo.MasterName))
                {
                    expression.Add(i => i.m_mastername == ocInfo.MasterName);
                }
                if (!string.IsNullOrEmpty(ocInfo.Qudao))
                {
                    expression.Add(i => i.m_mastertype == ocInfo.Qudao);
                }
                if (!string.IsNullOrEmpty(ocInfo.Agency))
                {
                    expression.Add(i => i.m_mastername == ocInfo.Agency);
                }
            }
            return expression;
        }

        private List<Expression<Func<vw_order, bool>>> GetAgentOrderCondition(OrderCondition ocInfo, out List<string> depts)
        {
            List<Expression<Func<vw_order, bool>>> expression = new List<Expression<Func<vw_order, bool>>>();

            if (UserIdentity == 1 || masterinfo.dataauthority != (int)Dataauthority.全部)
            {
                expression.Add(t => t.o_bonus > 0 || t.o_totype == 2);
            }

            Recursive give = new Recursive();
            int deptid = masterinfo.deptid;
            List<string> InIds = new List<string>();
            if (ocInfo.Dept > 0)
            {
                deptid = Convert.ToInt32(ocInfo.Dept);
                InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
            }

            if (masterinfo.mastertype == 1 || masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                //if (deptid > 0)
                //{
                //    InIds.Add(deptid.ToString());
                //}
                expression.Add(t => t.agentid == masterinfo.agentid);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商)
            {
                if (InIds != null && InIds.Count > 0)
                {

                }
                else
                {
                    InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                    InIds.Add(masterinfo.deptid.ToString());
                }
                depts = InIds;
                expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                //depts = InIds;
                //if (InIds != null)
                //{
                //    expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
                //}
                //else
                //{
                //    expression.Add(t => t.m_mastername == masterinfo.mastername);
                //}
                if (InIds != null && InIds.Count > 0)
                {

                }
                else
                {
                    InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                }
                depts = InIds;
                if (depts != null && depts.Count > 0)
                    expression.Add(t => t.agentid == masterinfo.agentid || t.m_mastername == masterinfo.mastername);
                else
                    expression.Add(t => t.m_mastername == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人下级代理商)
            {
                expression.Add(t => t.parentname == masterinfo.mastername);
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人)
            {
                expression.Add(t => t.m_mastername == masterinfo.mastername);
            }
            depts = InIds;


            if (ocInfo != null)
            {
                if (!string.IsNullOrEmpty(ocInfo.SearchKey))
                {
                    switch (ocInfo.SearchType)
                    {
                        case 0:
                            expression.Add(i => i.o_id == ocInfo.SearchKey.Trim());
                            break;
                        case 1:
                            expression.Add(i => i.u_teachername == ocInfo.SearchKey.Trim());
                            break;
                    }
                }
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.o_datetime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.o_datetime <= ocInfo.endDate);
                }
                if (ocInfo.PayType.HasValue)
                {
                    expression.Add(i => i.o_feetype == ocInfo.PayType);
                }
                if (ocInfo.SchoolID.HasValue)
                {
                    expression.Add(i => i.schoolid == ocInfo.SchoolID);
                }
                if (ocInfo.Version.HasValue)
                {
                    if (Convert.ToInt32(ocInfo.Version.Value) > 0)
                        expression.Add(i => i.p_versionid.Contains("," + ocInfo.Version.ToString() + ","));
                }
                if (ocInfo.AreaCode.HasValue)
                {
                    string code = "";
                    string[] strs = new string[4];
                    strs[0] = ocInfo.AreaCode.ToString().Substring(0, 2);
                    strs[1] = ocInfo.AreaCode.ToString().Substring(2, 2);
                    strs[2] = ocInfo.AreaCode.ToString().Substring(4, 2);
                    strs[3] = ocInfo.AreaCode.ToString().Substring(6);
                    for (int i = 3; i >= 0; i--)
                    {
                        if (int.Parse(strs[i]) != 0)
                        {
                            code = strs[i] + code;
                        }
                    }
                    expression.Add(i => i.districtid.ToString().StartsWith(code));
                }
                if (ocInfo.GradeID.HasValue)
                {
                    expression.Add(i => i.gradeid == ocInfo.GradeID);
                }
                Guid g;
                if (!string.IsNullOrEmpty(ocInfo.ClassID) && Guid.TryParse(ocInfo.ClassID, out g))
                {
                    expression.Add(i => i.classid == g);
                }
                if (ocInfo.ChannelID.HasValue)
                {
                    expression.Add(i => i.channel == ocInfo.ChannelID.Value);
                }
                if (!string.IsNullOrEmpty(ocInfo.MasterName))
                {
                    expression.Add(i => i.m_mastername == ocInfo.MasterName);
                }
                if (!string.IsNullOrEmpty(ocInfo.Qudao))
                {
                    expression.Add(i => i.m_mastertype == ocInfo.Qudao);
                }
                if (!string.IsNullOrEmpty(ocInfo.Agency))
                {
                    expression.Add(i => i.m_mastername == ocInfo.Agency);
                }
            }
            return expression;
        }

        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public JsonResult GetAreaList(int parentid)
        {
            Core.Utility.KingResponse res = new KingResponse();
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            var strArea = client.GetAreaData(parentid);
            var listArea = JsonConvert.DeserializeObject<List<AreaView>>(strArea);
            res = KingResponse.GetResponse(listArea);
            return Json(res);
        }

        /// <summary>
        /// 获取学校信息
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public JsonResult GetSchoolInfo(int areaID)
        {
            Core.Utility.KingResponse res = new KingResponse();
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            string strschool = client.GetSchoolData(areaID.ToString(), "0", "");
            //KSWF.WFM.BLL.MateService.Service mateservice = new WFM.BLL.MateService.Service();
            //string strschool = mateservice.GetSchoolData(areaID.ToString(), "0", "");
            var listSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(strschool);
            List<ViewSchoolInfo> needSchools = new List<ViewSchoolInfo>();
            var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
            var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var school in listSchool)
            {
                if (schoolTypeNos.Contains(school.SchoolTypeNo))
                {
                    needSchools.Add(school);
                }
            }
            res = KingResponse.GetResponse(needSchools);
            return Json(res);
        }

        /// <summary>
        /// 获取学校的年级和班级
        /// </summary>
        /// <param name="schoolID"></param>
        /// <returns></returns>
        public JsonResult GetSchollGradeClass(int schoolID)
        {
            Core.Utility.KingResponse res = new KingResponse();
            KSWF.WFM.BLL.SNSService.FZUUMS_Relation2 rService = new WFM.BLL.SNSService.FZUUMS_Relation2();
            KSWF.WFM.BLL.SNSService.tb_Class[] clist = rService.GetSchoolClassCpoint(schoolID);
            if (clist == null || clist.Length == 0)
            {
                res = KingResponse.GetErrorResponse("该学校没有年级班级");
                return Json(res);
            }
            var ls = clist.GroupBy(a => a.GradeID).Select(g => (new { name = g.Key }));
            List<object> olist = new List<object>();
            foreach (var K in ls)
            {
                var list = clist.Where(i => i.GradeID == K.name);
                var obj = new { GradeID = K.name, GradeName = GetGradeName(K.name.Value), ClassList = list };
                olist.Add(obj);
            }
            res = KingResponse.GetResponse(olist);
            return Json(res);
        }

        private string GetGradeName(int gradeid)
        {
            string name = "";
            switch (gradeid)
            {
                case 0: name = "其他"; break;
                case 1: name = "学前"; break;
                case 2: name = "一年级"; break;
                case 3: name = "二年级"; break;
                case 4: name = "三年级"; break;
                case 5: name = "四年级"; break;
                case 6: name = "五年级"; break;
                case 7: name = "六年级"; break;
                case 8: name = "七年级"; break;
                case 9: name = "八年级"; break;
                case 10: name = "九年级"; break;
                case 11: name = "高一"; break;
                case 12: name = "高二"; break;
                case 13: name = "高三"; break;
                case 14: name = "幼儿园小班"; break;
                case 15: name = "幼儿园中班"; break;
                case 16: name = "幼儿园大班"; break;
            }
            return name;
        }

        /// <summary>
        /// 获取当前用户允许查询的部门
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCurrentDeptList()
        {
            List<TreeView> lt = new List<TreeView>();
            List<TreeView> lt2 = new List<TreeView>();
            TreeView tv = GetDepts();
            TreeView t = new TreeView() { Id = "0", text = "全部", ParentId = "0", tag = "0" };

            if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 0)//能否查看本部门及以下部门
            {
                if (string.IsNullOrEmpty(tv.Id))
                {
                    lt2.Add(t);
                    return Json(lt2);
                }
                lt.Add(tv);
                t.nodes = lt;
            }

            lt2.Add(t);
            return Json(lt2);
        }

        /// <summary>
        /// 导出订到到excel表格
        /// </summary>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        public FileResult ExportOrderXls([System.Web.Http.FromBody]OrderCondition ocInfo)
        {
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "订单" + DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(ocInfo.Agency))
            {
                if (UserIdentity == 1 && masterinfo.dataauthority == (int)Dataauthority.全部)
                {
                    PageParameter<vw_order> param = new PageParameter<vw_order>();
                    List<string> deptids = null;
                    param.Wheres = GetAgentOrderCondition(ocInfo, out deptids);
                    param.In = deptids;
                    param.Field = " m_deptid ";
                    List<vw_order> list = base.OrderManage.SelectSearchs<vw_order>(param.Wheres, param.Field, param.In, " o_datetime desc ");
                    CreateSheet2(list, book, tmpTitle + " ", 0, list.Count);
                }
                else
                {
                    PageParameter<vw_kswforder> param2 = new PageParameter<vw_kswforder>();
                    List<string> deptids2 = null;
                    param2.Wheres = GetAgentOrderCondition2(ocInfo, out deptids2);
                    param2.In = deptids2;
                    param2.Field = " m_deptid ";
                    List<vw_kswforder> list = base.OrderManage.SelectSearchs<vw_kswforder>(param2.Wheres, param2.Field, param2.In, " o_datetime desc ");
                    CreateSheet(list, book, tmpTitle + " ", 0, list.Count);
                }
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                tmpTitle = HttpUtility.UrlEncode(tmpTitle, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                tmpTitle = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tmpTitle)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", tmpTitle + ".xls");

        }
        public string PayType(int? type)
        {
            if (type == 0)
                return "微信";
            else if (type == 1)
                return "支付宝";
            else if (type == 2)
                return "苹果";
            return type.ToString();
        }
        private void CreateSheet(IList<vw_kswforder> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            //headerrow.CreateCell(0).SetCellValue("序号");
            //headerrow.CreateCell(2).SetCellValue("订单结算状态");
            //headerrow.CreateCell(9).SetCellValue("老师手机号");
            //headerrow.CreateCell(13).SetCellValue("商品年级");

            headerrow.CreateCell(0).SetCellValue("订单编号");
            headerrow.CreateCell(1).SetCellValue("订单日期");
            headerrow.CreateCell(2).SetCellValue("省");
            headerrow.CreateCell(3).SetCellValue("市");
            headerrow.CreateCell(4).SetCellValue("区/县");
            headerrow.CreateCell(5).SetCellValue("学校");
            headerrow.CreateCell(6).SetCellValue("年级");
            headerrow.CreateCell(7).SetCellValue("班级");
            headerrow.CreateCell(8).SetCellValue("老师姓名");
            headerrow.CreateCell(9).SetCellValue("渠道");
            headerrow.CreateCell(10).SetCellValue("部门");
            headerrow.CreateCell(11).SetCellValue("员工姓名/代理商名称");
            headerrow.CreateCell(12).SetCellValue("产品名称");
            headerrow.CreateCell(13).SetCellValue("版本");
            headerrow.CreateCell(14).SetCellValue("支付方式");
            headerrow.CreateCell(15).SetCellValue("支付金额");
            headerrow.CreateCell(16).SetCellValue("手续费");
            headerrow.CreateCell(17).SetCellValue("实际到账");

            for (int i = startIndex; i < endIndex; i++)
            {
                if (list[i] != null)
                {
                    vw_kswforder toinfo = list[i];
                    IRow row = sheet.CreateRow(i + 1);      //新创建一行
                    ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                    cell.CellStyle = style;                 //设置单元格格式
                    row.CreateCell(0).SetCellValue(toinfo.o_id);
                    row.CreateCell(1).SetCellValue(toinfo.o_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (toinfo.path != null)
                    {
                        string[] paths = toinfo.path.Trim().Split(' ');
                        //省
                        if (paths.Length > 0)
                        {
                            if (paths[0].Contains("省"))
                            {
                                row.CreateCell(2).SetCellValue(paths[0]);
                                if (paths.Length > 1)
                                {
                                    row.CreateCell(3).SetCellValue(paths[1]);
                                    if (paths.Length > 2)
                                    {
                                        row.CreateCell(4).SetCellValue(paths[2]);
                                    }
                                }
                            }
                            else if (paths[0].Contains("市"))
                            {
                                row.CreateCell(2).SetCellValue(paths[0]);
                                row.CreateCell(3).SetCellValue(paths[0]);
                                if (paths.Length > 1)
                                {
                                    row.CreateCell(4).SetCellValue(paths[1]);
                                }
                            }
                        }
                    }
                    row.CreateCell(5).SetCellValue(toinfo.schoolname);
                    row.CreateCell(6).SetCellValue(toinfo.gradename);
                    row.CreateCell(7).SetCellValue(toinfo.classname);
                    row.CreateCell(8).SetCellValue(toinfo.u_teachername);
                    row.CreateCell(9).SetCellValue(toinfo.m_mastertype);
                    row.CreateCell(10).SetCellValue(toinfo.m_deptname);
                    row.CreateCell(11).SetCellValue(toinfo.m_a_name);
                    row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.ProductName(toinfo.channel));
                    row.CreateCell(13).SetCellValue(toinfo.p_version);
                    row.CreateCell(14).SetCellValue(PayType(toinfo.o_feetype));
                    row.CreateCell(15).SetCellValue(toinfo.o_payamount.ToString("0.00"));
                    row.CreateCell(16).SetCellValue(toinfo.o_feeamount.ToString("0.00"));
                    row.CreateCell(17).SetCellValue(toinfo.o_actamount.ToString("0.00"));
                }
            }
        }
        private void CreateSheet2(IList<vw_order> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            //headerrow.CreateCell(0).SetCellValue("序号");
            //headerrow.CreateCell(2).SetCellValue("订单结算状态");
            //headerrow.CreateCell(9).SetCellValue("老师手机号");
            //headerrow.CreateCell(13).SetCellValue("商品年级");

            headerrow.CreateCell(0).SetCellValue("订单编号");
            headerrow.CreateCell(1).SetCellValue("订单日期");
            headerrow.CreateCell(2).SetCellValue("省");
            headerrow.CreateCell(3).SetCellValue("市");
            headerrow.CreateCell(4).SetCellValue("区/县");
            headerrow.CreateCell(5).SetCellValue("学校");
            headerrow.CreateCell(6).SetCellValue("年级");
            headerrow.CreateCell(7).SetCellValue("班级");
            headerrow.CreateCell(8).SetCellValue("老师姓名");
            headerrow.CreateCell(9).SetCellValue("渠道");
            headerrow.CreateCell(10).SetCellValue("部门");
            headerrow.CreateCell(11).SetCellValue("员工姓名/代理商名称");
            headerrow.CreateCell(12).SetCellValue("产品名称");
            headerrow.CreateCell(13).SetCellValue("版本");
            headerrow.CreateCell(14).SetCellValue("支付方式");
            headerrow.CreateCell(15).SetCellValue("支付金额");
            headerrow.CreateCell(16).SetCellValue("手续费");
            headerrow.CreateCell(17).SetCellValue("实际到账");

            for (int i = startIndex; i < endIndex; i++)
            {
                vw_order toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式
                row.CreateCell(0).SetCellValue(toinfo.o_id);
                row.CreateCell(1).SetCellValue(toinfo.o_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                if (toinfo.path != null)
                {
                    string[] paths = toinfo.path.Trim().Split(' ');
                    //省
                    if (paths.Length > 0)
                    {
                        if (paths[0].Contains("省"))
                        {
                            row.CreateCell(2).SetCellValue(paths[0]);
                            if (paths.Length > 1)
                            {
                                row.CreateCell(3).SetCellValue(paths[1]);
                                if (paths.Length > 2)
                                {
                                    row.CreateCell(4).SetCellValue(paths[2]);
                                }
                            }
                        }
                        else if (paths[0].Contains("市"))
                        {
                            row.CreateCell(2).SetCellValue(paths[0]);
                            row.CreateCell(3).SetCellValue(paths[0]);
                            if (paths.Length > 1)
                            {
                                row.CreateCell(4).SetCellValue(paths[1]);
                            }
                        }
                    }
                }
                row.CreateCell(5).SetCellValue(toinfo.schoolname);
                row.CreateCell(6).SetCellValue(toinfo.gradename);
                row.CreateCell(7).SetCellValue(toinfo.classname);
                row.CreateCell(8).SetCellValue(toinfo.u_teachername);
                row.CreateCell(9).SetCellValue(toinfo.m_mastertype);
                row.CreateCell(10).SetCellValue(toinfo.m_deptname);
                row.CreateCell(11).SetCellValue(toinfo.m_a_name);
                row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.ProductName(toinfo.channel));
                row.CreateCell(13).SetCellValue(toinfo.p_version);
                row.CreateCell(14).SetCellValue(PayType(toinfo.o_feetype));
                row.CreateCell(15).SetCellValue(toinfo.o_payamount.ToString("0.00"));
                row.CreateCell(16).SetCellValue(toinfo.o_feeamount.ToString("0.00"));
                row.CreateCell(17).SetCellValue(toinfo.o_actamount.ToString("0.00"));
            }
        }

    }
}

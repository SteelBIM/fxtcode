using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KSWF.Core.Utility;
using KSWF.Framework.BLL;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;
using System.Linq.Expressions;
using KSWF.WFM.Constract.VW;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Configuration;

namespace KSWF.Web.Admin.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 用户身份 0本公司 1代理商
        /// </summary>
        public int UserIdentity = 0;
        protected Jurisdiction action = new Jurisdiction();
        protected com_master masterinfo = new com_master();

        public int GetSchoolCount(int districtid)
        {
            string strDistrictid = districtid.ToString();
            string areaId = strDistrictid.Substring(0, strDistrictid.Length - 3) + "000";

            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            string strAllSchool = client.GetSchoolData(areaId, "", "");

            var listAllSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(strAllSchool);
            var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
            var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<ViewSchoolInfo> filterSchools = new List<ViewSchoolInfo>();
            foreach (var school in listAllSchool)
            {
                if (schoolTypeNos.Contains(school.SchoolTypeNo))
                {
                    filterSchools.Add(school);
                }
            }
            return filterSchools.Count;
        }

        public List<ViewSchoolInfo> GetSchools(int districtid)
        {
            string strDistrictid = districtid.ToString();
            string areaId = strDistrictid.Substring(0, strDistrictid.Length - 3) + "000";

            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            string strAllSchool = client.GetSchoolData(areaId, "", "");

            var listAllSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(strAllSchool);
            var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
            var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<ViewSchoolInfo> filterSchools = new List<ViewSchoolInfo>();
            foreach (var school in listAllSchool)
            {
                if (schoolTypeNos.Contains(school.SchoolTypeNo))
                {
                    filterSchools.Add(school);
                }
            }
            return filterSchools;
        }

        public int GetAreaCount(int districtid)
        {
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            var strChildArea = client.GetAreaData(districtid);
            var listChildArea = JsonConvert.DeserializeObject<List<AreaView>>(strChildArea);
            return listChildArea.Count;
        }

        public AreaView GetAreaInfo(int districtid)
        {
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            var strChildArea = client.GetAreaInfo(districtid);
            var listChildArea = JsonConvert.DeserializeObject<AreaView>(strChildArea);
            return listChildArea;
        }

        public List<AreaView> GetAreas(int districtid)
        {
            AreaService.ServiceSoapClient client = new AreaService.ServiceSoapClient();
            var strChildArea = client.GetAreaData(districtid);
            var listChildArea = JsonConvert.DeserializeObject<List<AreaView>>(strChildArea);
            return listChildArea;
        }

        public List<string> GetNotAffectDistrict(List<base_deptarea> allDistricts, List<base_deptarea> effectDistricts)
        {
            List<string> endDistricts = new List<string>();
            foreach (var item in allDistricts)
            {
                string strItemDistrictid = item.districtid.ToString();

                foreach (var item1 in effectDistricts)
                {
                    string strItem1Districtid = item1.districtid.ToString();

                    if (strItem1Districtid.Substring(2, 7) == "0000000")
                    {
                        //省
                        if (strItemDistrictid.Substring(0, 2) == strItem1Districtid.Substring(0, 2))
                        {
                            endDistricts.Add(item.deptid.ToString());
                        }
                    }
                    else if (strItem1Districtid.Substring(4, 5) == "00000")
                    {
                        //市
                        if (strItemDistrictid.Substring(0, 4) == strItem1Districtid.Substring(0, 4))
                        {
                            endDistricts.Add(item.deptid.ToString());
                        }
                    }
                    else if (strItem1Districtid.Substring(6, 3) == "000")
                    {
                        if (item1.schoolid > 0)
                        {
                            //学校
                            if (item.schoolid == item1.schoolid)
                            {
                                endDistricts.Add(item.deptid.ToString());
                            }
                        }
                        else
                        {
                            //区
                            if (strItemDistrictid.Substring(0, 6) == strItem1Districtid.Substring(0, 6))
                            {
                                endDistricts.Add(item.deptid.ToString());
                            }
                        }
                    }
                }
            }
            return endDistricts;
        }

        public List<string> GetNotAffectDistrict(List<join_mastertarea> allDistricts, List<base_deptarea> effectDistricts)
        {
            List<string> endDistricts = new List<string>();
            foreach (var item in allDistricts)
            {
                string strItemDistrictid = item.districtid.ToString();

                foreach (var item1 in effectDistricts)
                {
                    string strItem1Districtid = item1.districtid.ToString();

                    if (strItem1Districtid.Substring(2, 7) == "0000000")
                    {
                        //省
                        if (strItemDistrictid.Substring(0, 2) == strItem1Districtid.Substring(0, 2))
                        {
                            endDistricts.Add(item.mastername);
                        }
                    }
                    else if (strItem1Districtid.Substring(4, 5) == "00000")
                    {
                        //市
                        if (strItemDistrictid.Substring(0, 4) == strItem1Districtid.Substring(0, 4))
                        {
                            endDistricts.Add(item.mastername);
                        }
                    }
                    else if (strItem1Districtid.Substring(6, 3) == "000")
                    {
                        if (item1.schoolid > 0)
                        {
                            //学校
                            if (item.schoolid == item1.schoolid)
                            {
                                endDistricts.Add(item.mastername);
                            }
                        }
                        else
                        {
                            //区
                            if (strItemDistrictid.Substring(0, 6) == strItem1Districtid.Substring(0, 6))
                            {
                                endDistricts.Add(item.mastername);
                            }
                        }
                    }
                }
            }
            return endDistricts;
        }

        /// <summary>
        /// 检查所选区域是否已选择，产生互斥
        /// </summary>
        /// <param name="deptareas">区域集合</param>
        /// <param name="type">0部门区域 1员工/代理商区域</param>
        /// <returns>true冲突 false不冲突</returns>
        public bool CheckArea(List<base_deptarea> deptareas, int parentDeptId, int type)
        {
            int conflictAreaCount = 0;
            foreach (var item in deptareas)
            {
                Dictionary<string, object> dis = new Dictionary<string, object>();
                dis.Add("deptid", parentDeptId);
                dis.Add("districtid", item.districtid);
                dis.Add("type", type);
                dis.Add("schoolid", item.schoolid);
                var count = Manage.ExecuteProcedure<string>("checkArea", dis);
                conflictAreaCount += Convert.ToInt32(count[0]);
            }
            return conflictAreaCount > 0;
        }

        public List<TreeView> GetSearchSchools(string searchKey, string areaId)
        {
            List<TreeView> schools = (List<TreeView>)HttpContext.Cache.Get(areaId);
            List<TreeView> treeViews = new List<TreeView>();
            if (schools != null)
            {
                Regex rx = new Regex(searchKey);
                foreach (var item in schools)
                {
                    if (string.IsNullOrEmpty(searchKey))
                    {
                        treeViews.Add(item);
                    }
                    else
                    {
                        if (rx.IsMatch(item.text))
                        {
                            treeViews.Add(item);
                        }
                    }
                }
            }
            return treeViews;
        }

        public string GetDeptSqlString(int deptid)
        {
            //TreeView tv = GetDepts(deptid);
            return string.Format("m_deptid = {0}", deptid);
        }
        public List<string> GetDeptEmpAgenecys(bool hasDeptid, string agentid, int deptid = 0)
        {
            List<string> re = new List<string>();
            List<string> depts = new List<string>();
            if (hasDeptid)
            {
                depts = GetDeptIDs(deptid);
            }
            else
            {
                depts = GetDeptIDs();
            }
            if (depts != null && depts.Count > 0)
            {
                var deptemployees = Manage.SelectSearch<com_master>("mastertype=0 and agentid='" + agentid + "' and deptid in " + InFormat(depts));
                List<string> deptemployeenames = new List<string>();
                foreach (var item in deptemployees)
                {
                    deptemployeenames.Add(item.mastername);
                }
                var agenecys = Manage.SelectIn<com_master>("mastertype=1", "parentname", deptemployeenames);

                foreach (var item in agenecys)
                {
                    re.Add(item.mastername);
                }
            }
            return re;
        }

        public List<string> GetDeptIDs(int deptid)
        {
            List<string> list = new List<string>();
            TreeView tv = GetDepts(deptid);
            if (tv == null)
            {
                return list;
            }
            if (masterinfo.dataauthority == 0 || masterinfo.dataauthority == 2)
            {
                list.Add(tv.Id);
            }
            if (tv.nodes != null && tv.nodes.Count > 0)
            {
                list.AddRange(GetDeptID(tv.nodes));
            }
            return list;
        }

        public List<string> GetDeptIDs()
        {
            List<string> list = new List<string>();
            TreeView tv = GetDepts();
            if (tv == null)
            {
                return list;
            }
            if (masterinfo.dataauthority == 0 || masterinfo.dataauthority == 2)
            {
                list.Add(tv.Id);
            }
            if (tv.nodes != null && tv.nodes.Count > 0)
            {
                list.AddRange(GetDeptID(tv.nodes));
            }
            return list;
        }

        #region 获取部门最近结算日期 int GetDeptRecentlySettledTime(int deptid)
        /// <summary>
        /// 获取最近结算日期
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public int GetDeptRecentlySettledTime(int deptid)
        {
            List<order_setbonus_dept> list = OrderManage.SelectSearch<order_setbonus_dept>(t => (t.deptid == deptid && t.state == 0), 1, " createtime desc");
            if (list != null && list.Count > 0)
                return list[0].enddate;
            return 0;
        }
        #endregion

        #region 获取员工及代理商最近结算日期 int GetRecentlySettledTime(string mastername)
        /// <summary>
        /// 获取最近结算日期
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public int GetRecentlySettledTime(string mastername)
        {
            List<order_setbonus> list = OrderManage.SelectSearch<order_setbonus>(t => (t.mastername_t == mastername && t.state == 0), 1, " createtime desc");
            if (list != null && list.Count > 0)
                return list[0].enddate;
            return 0;
        }
        #endregion
      
        public bool IsExclusion(int brotherDistrictid, int needAddDistrictid)
        {
            string strBrotherDistrictid = brotherDistrictid.ToString();
            string strNeedAddDistrictid = needAddDistrictid.ToString();

            if (strNeedAddDistrictid == strBrotherDistrictid)//街道（假设最小可能）
            {
                return true;
            }
            else
            {
                if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 6) + "000"))//区
                {
                    return true;
                }
                else
                {
                    if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 4) + "00000")
                        || (strNeedAddDistrictid.Substring(0, 6) + "000") == strBrotherDistrictid)//市
                    {
                        return true;
                    }
                    else
                    {
                        if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 2) + "0000000")
                            || (strNeedAddDistrictid.Substring(0, 4) + "00000") == strBrotherDistrictid
                            || (strNeedAddDistrictid.Substring(0, 6) + "000") == strBrotherDistrictid)//省
                        {
                            return true;
                        }
                        else
                        {
                            if (strNeedAddDistrictid == "0"
                                || (strNeedAddDistrictid.Substring(0, 2) + "0000000") == strBrotherDistrictid
                                || (strNeedAddDistrictid.Substring(0, 4) + "00000") == strBrotherDistrictid
                                || (strNeedAddDistrictid.Substring(0, 6) + "000") == strBrotherDistrictid)//国
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 执行控制器方法之前先执行该方法
        /// 获取自定义的Session的
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Action"] != null && Session["LoginInfo"] != null)
            {
                masterinfo = Session["LoginInfo"] as com_master;
                if (masterinfo.agentid != Core.Utility.PublicHelp.OrgId)
                    UserIdentity = 1;
                string CurrentController = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();//当前Controller
                //string CurrentAction = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();//当前action
                //bool IsHaveAction = false;//是否拥有当前action权限
                List<vw_action> list = Session["Action"] as List<vw_action>;
                if (list != null && list.Count > 0)
                {
                    #region 获取权限
                    foreach (vw_action item in list)
                    {
                        if (item.actionurl == CurrentController)
                        {
                            if (item.actionname.Contains("View"))
                                action.View = true;
                            else if (item.actionname.Contains("Edit"))
                                action.Edit = true;
                            else if (item.actionname.Contains("Add"))
                                action.Add = true;
                            else if (item.actionname.Contains("Del"))
                                action.Del = true;
                            else if (item.actionname.Contains("Export"))
                                action.Export = true;
                            else if (item.actionname.Contains("Pullblack"))
                                action.Pullblack = true;
                            else if (item.actionname.Contains("Locking"))
                                action.Locking = true;
                            else if (item.actionname.Contains("Move"))
                                action.Move = true;
                            else if (item.actionname.Contains("Detailed"))
                                action.Detailed = true;
                            else if (item.actionname.Contains("Blacklist"))
                                action.Blacklist = true;
                            else if (item.actionname.Contains("Kont"))
                                action.Kont = true;
                            else if (item.actionname.Contains("Revoked"))
                                action.Revoked = true;
                            else if (item.actionname.Contains("Employee"))
                                action.Employee = true;
                            else if (item.actionname.Contains("Dept"))
                                action.Dept = true;
                            else if (item.actionname.Contains("Agent"))
                                action.Agent = true;
                            else if (item.actionname.Contains("Merge"))
                                action.Merge = true;
                        }
                    }
                    #endregion
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public object GetAgendChannel()
        {
            if (masterinfo.agentid == KSWF.Core.Utility.PublicHelp.OrgId)
                return KSWF.WFM.BLL.KeyValueManage.GetChannleData();
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();

            List<string> product = give.GetUserProduct(masterinfo.mastername);
            List<Expression<Func<vw_agendchannel, bool>>> exprlist = new List<Expression<Func<vw_agendchannel, bool>>>();
            exprlist.Add(vw => vw.agentid == masterinfo.agentid);
            return SelectSearchs<vw_agendchannel>(exprlist, "  `key`  ", product);
        }

        public List<string> GetAgencys(bool isAllAgencys = false)
        {
            List<string> com_masternames = new List<string>();
            if (masterinfo.dataauthority != 1)
            {
                List<com_master> com_masters = new List<com_master>();
                if (isAllAgencys == true)
                {
                    com_masters.Add(masterinfo);
                    if (masterinfo.mastertype == 2 || (masterinfo.groupid == 2 && masterinfo.agentid == "KSWF") || (masterinfo.groupid == 3 && masterinfo.agentid == "KSWF"))
                    {
                        com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + masterinfo.agentid + "'"));
                    }
                    else
                    {
                        com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + masterinfo.agentid + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'"));
                    }
                }
                else
                {
                    if (masterinfo.mastertype == 2 || (masterinfo.groupid == 2 && masterinfo.agentid == "KSWF") || (masterinfo.groupid == 3 && masterinfo.agentid == "KSWF"))
                    {
                        com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and parentname='" + masterinfo.mastername + "'"));
                    }
                    else
                    {
                        com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and parentname='" + masterinfo.mastername + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'"));
                    }
                }
                if (com_masters != null && com_masters.Count > 0)
                {
                    foreach (var item in com_masters)
                    {
                        if (item.mastertype == 1)
                        {
                            com_masternames.Add(item.mastername);
                        }
                        GetChildAgencys(item.agentid, com_masternames);
                    }
                }
            }
            return com_masternames;
        }

        public List<string> GetFirstAgencys(bool isAllAgencys = false)
        {
            List<string> com_masternames = new List<string>();
            if (masterinfo.dataauthority != 1)
            {
                List<com_master> com_masters = new List<com_master>();
                if (isAllAgencys == true)
                {
                    com_masters.Add(masterinfo);
                    com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + masterinfo.agentid + "'"));
                }
                else
                {
                    com_masters.AddRange(Manage.SelectSearch<com_master>("mastertype=1 and parentname='" + masterinfo.mastername + "'"));
                }
                if (com_masters != null && com_masters.Count > 0)
                {
                    foreach (var item in com_masters)
                    {
                        if (item.mastertype == 1)
                        {
                            com_masternames.Add(item.mastername);
                        }
                    }
                }
            }
            return com_masternames;
        }

        public void GetChildAgencys(string agentid, List<string> com_masternames)
        {
            List<com_master> com_masters = new List<com_master>();
            if (masterinfo.mastertype == 2 || (masterinfo.groupid == 2 && masterinfo.agentid == "KSWF") || (masterinfo.groupid == 3 && masterinfo.agentid == "KSWF"))
            {
                com_masters = Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + agentid + "'");
            }
            else
            {
                com_masters = Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + agentid + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'");
            }
            if (com_masters != null && com_masters.Count > 0)
            {
                foreach (var item in com_masters)
                {
                    com_masternames.Add(item.mastername);
                    GetChildAgencys(item.agentid, com_masternames);
                }
            }
        }

        public List<com_master> GetAgencysMaster(bool isAllAgencys = false)
        {
            List<com_master> com_masternames = new List<com_master>();
            if (masterinfo.dataauthority != 1)
            {
                List<com_master> com_masters = new List<com_master>();
                if (isAllAgencys == true)
                {
                    com_masters.Add(masterinfo);
                    string sql = "mastertype=1 and pagentid='" + masterinfo.agentid + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'";
                    var re = Manage.SelectSearch<com_master>(sql);
                    com_masters.AddRange(re);
                }
                else
                {
                    string sql = "mastertype=1 and parentname='" + masterinfo.mastername + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'";
                    var re = Manage.SelectSearch<com_master>(sql);
                    com_masters.AddRange(re);
                }
                if (com_masters != null && com_masters.Count > 0)
                {
                    foreach (var item in com_masters)
                    {
                        if (item.mastertype == 1)
                        {
                            com_masternames.Add(item);
                        }
                        GetChildAgencyMasters(item.agentid, com_masternames);
                    }
                }
            }
            return com_masternames;
        }

        public void GetChildAgencyMasters(string agentid, List<com_master> com_masternames)
        {
            List<com_master> com_masters = Manage.SelectSearch<com_master>("mastertype=1 and pagentid='" + agentid + "' and agent_enddate>='" + DateTime.Now.Date.ToShortDateString() + "'");
            if (com_masters != null && com_masters.Count > 0)
            {
                foreach (var item in com_masters)
                {
                    com_masternames.Add(item);
                    GetChildAgencyMasters(item.agentid, com_masternames);
                }
            }
        }

        public List<string> GetDeptID(List<TreeView> tvlist)
        {
            List<string> list = new List<string>();
            foreach (TreeView tv in tvlist)
            {
                list.Add(tv.Id);
                if (tv.nodes != null && tv.nodes.Count > 0)
                {
                    list.AddRange(GetDeptID(tv.nodes));
                }
            }
            return list;
        }

        public string InFormat(List<string> param, bool isInt = false)
        {
            StringBuilder sb = new StringBuilder();
            if (param.Count > 0)
            {
                sb.AppendFormat("(");
                int index = 0;
                for (int i = 0; i < param.Count; i++)
                {
                    if (!string.IsNullOrEmpty(param[i]) && index == 0)
                    {
                        if (isInt)
                        {
                            sb.AppendFormat("{0}", param[i]);
                        }
                        else
                        {
                            sb.AppendFormat("'{0}'", param[i]);
                        }
                        index += 1;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param[i]))
                        {
                            if (isInt)
                            {
                                sb.AppendFormat(",{0}", param[i]);
                            }
                            else
                            {
                                sb.AppendFormat(",'{0}'", param[i]);
                            }
                        }
                    }
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        public TreeView GetDepts(string IsAgent = "")
        {
            var deptid = masterinfo.deptid;
            if (masterinfo.deptid == 0 || (masterinfo.dataauthority == 0 && masterinfo.agentid == "KSWF"))//超级管理员部门id为0
            {
                deptid = 1;//设置到总公司
            }
            List<base_dept> dept = new List<base_dept>();
            if (IsAgent == "KSWF")
            {
                dept = manage.SelectSearch<base_dept>(x => (x.deptid == deptid && x.agentid == "KSWF"));
            }
            else if (IsAgent == "Agent")
            {
                dept = manage.SelectSearch<base_dept>(x => (x.deptid == deptid && x.agentid != "KSWF"));
            }
            else
            {
                dept = manage.SelectSearch<base_dept>(x => (x.deptid == deptid));
            }

            TreeView tv = new TreeView();
            if (dept.Count > 0)
            {
                tv.Id = dept[0].deptid.ToString();
                tv.text = dept[0].deptname;
                tv.tag = dept[0].parentid.ToString();
                tv.nodes = GetChildDepts(dept[0].deptid, IsAgent);
            }
            return tv;
        }

        public TreeView GetDepts(int deptid)
        {
            List<base_dept> dept = manage.SelectSearch<base_dept>(x => (x.deptid == deptid));

            TreeView tv = new TreeView();
            if (dept.Count > 0)
            {
                tv.Id = dept[0].deptid.ToString();
                tv.text = dept[0].deptname;
                tv.tag = dept[0].parentid.ToString();
                tv.nodes = GetChildDepts(dept[0].deptid);
            }
            return tv;
        }

        public List<TreeView> GetChildDepts(int parentid, string IsAgent = "")
        {
            List<base_dept> dept = new List<base_dept>();
            if (IsAgent == "KSWF")
            {
                dept = manage.SelectSearch<base_dept>(x => (x.parentid == parentid && x.agentid == "KSWF"));
            }
            else if (IsAgent == "Agent")
            {
                dept = manage.SelectSearch<base_dept>(x => (x.parentid == parentid && x.agentid != "KSWF"));
            }
            else
            {
                dept = manage.SelectSearch<base_dept>(x => (x.parentid == parentid));
            }

            List<TreeView> treeview = null;
            if (dept.Count > 0)
            {
                treeview = new List<TreeView>();
                foreach (var item in dept)
                {
                    TreeView tv = new TreeView();
                    tv.Id = item.deptid.ToString();
                    tv.text = item.deptname;
                    tv.tag = item.parentid.ToString();
                    tv.nodes = GetChildDepts(item.deptid, IsAgent);
                    treeview.Add(tv);
                }
            }
            return treeview;
        }

        public void GetChlildDept(int deptid, List<string> depts, string agentid)
        {
            depts.Add(deptid.ToString());
            var childDepts = Manage.SelectSearch<base_dept>(x => x.parentid == deptid && x.agentid == agentid);
            if (childDepts.Count > 0)
            {
                foreach (var item in childDepts)
                {
                    GetChlildDept(item.deptid, depts, agentid);
                }
            }
        }

        public List<string> GetAllDeptID(int parentid = 0)
        {
            var deptid = parentid;
            if (deptid == 0)
            {
                deptid = masterinfo.deptid;
                if (masterinfo.deptid == 0 || (masterinfo.dataauthority == 0 && masterinfo.agentid == "KSWF"))//超级管理员部门id为0
                {
                    deptid = 1;//设置到总公司
                }
            }
            List<base_dept> dept = manage.SelectAll<base_dept>();
            List<string> list = GetDeptID(dept, deptid);
            return list;
        }

        private List<string> GetDeptID(List<base_dept> list, int parentid)
        {
            List<base_dept> blist = list.Where(i => i.parentid == parentid).ToList();
            List<string> intlist = new List<string>();
            intlist.Add(parentid.ToString());
            foreach (base_dept bd in blist)
            {
                //intlist.Add(bd.deptid.ToString());
                intlist.AddRange(GetDeptID(list, bd.deptid));
            }
            return intlist;
        }
        /// <summary>
        /// 订单归属到部门 增加产品
        /// </summary>
        /// <param name="districtid"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public base_dept GetDeptByArea(string districtid, int? channel)
        {
            string straid = districtid.Substring(0, 6) + "000";
            int aid = Convert.ToInt32(straid);
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            List<base_dept> dept3 = manage.SelectSearch<base_dept, base_deptarea>((t1, t2) => t2.districtid == aid && t2.schoolid == 0, (t1, t2) => t1.deptid == t2.deptid).OrderByDescending(t1 => t1.deptid).ToList();
            if (dept3 != null && dept3.Count > 0)
            {
                base_dept rowdept = give.GetDept(dept3, channel.ToString());
                if (rowdept != null)
                    return rowdept;
            }

            string straid2 = districtid.Substring(0, 4) + "00000";
            int aid2 = Convert.ToInt32(straid2);
            List<base_dept> dept4 = manage.SelectSearch<base_dept, base_deptarea>((t1, t2) => t2.districtid == aid2, (t1, t2) => t1.deptid == t2.deptid).OrderByDescending(t1 => t1.deptid).ToList();
            if (dept4 != null && dept4.Count > 0)
            {
                base_dept rowdept1 = give.GetDept(dept4, channel.ToString());
                if (rowdept1 != null)
                    return rowdept1;
            }

            string straid3 = districtid.Substring(0, 2) + "0000000";
            int aid3 = Convert.ToInt32(straid3);
            List<base_dept> dept5 = manage.SelectSearch<base_dept, base_deptarea>((t1, t2) => t2.districtid == aid3, (t1, t2) => t1.deptid == t2.deptid).OrderByDescending(t1 => t1.deptid).ToList();
            if (dept5 != null && dept5.Count > 0)
            {
                base_dept rowdept2 = give.GetDept(dept5, channel.ToString());
                if (rowdept2 != null)
                    return rowdept2;
            }

            return null;
        }


        Manage manage = new Manage();

        public Manage Manage
        {
            get { return manage; }
        }

        public OrderBaseManage OrderManage
        {
            get
            {
                return new OrderBaseManage();
            }
        }

        public int Add<T>(T subdata, string[] array = null) where T : class, new()
        {
            return manage.Add<T>(subdata, array);
        }
        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities) where T : class, new()
        {
            return manage.InsertRange<T>(entities);
        }
        public int InsertRange<T0, T1>(List<T0> entities0, List<T1> entities1)
            where T0 : class, new()
            where T1 : class, new()
        {
            return manage.InsertRange<T0, T1>(entities0, entities1);
        }
        public bool Update<T1, T2, T3>(object obj1, Expression<Func<T1, bool>> expr1, object obj2, Expression<Func<T2, bool>> expr2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return manage.Update<T1, T2, T3>(obj1, expr1, obj2, expr2, obj3, expr3);
        }
        public bool UpdateDelete<T1, T2, T3>(Expression<Func<T1, object>> expr1, List<int> Ins1, Expression<Func<T2, object>> expr2, List<string> Ins2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return manage.UpdateDelete<T1, T2, T3>(expr1, Ins1, expr2, Ins2, obj3, expr3);
        }

        public bool Update<T>(T subdata) where T : class, new()
        {
            return manage.Update<T>(subdata);
        }
        public bool Update<T>(T info, string[] disableUpdateCoulums) where T : class, new()
        {
            return manage.Update<T>(info, disableUpdateCoulums);
        }
        public bool Update<T>(object obj, Expression<Func<T, bool>> expr) where T : class,new()
        {
            return manage.Update<T>(obj, expr);
        }
        /// <summary>
        /// 批量删除<T>(物理删除<T>(1,2,3) where T : class, new()) where T : class, new()
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteMore<T>(string Ids) where T : class, new()
        {
            return manage.DeleteMore<T>(Ids);
        }
        /// <summary>
        /// 物理删除根据ID删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteById<T>(int Id) where T : class, new()
        {
            return manage.Delete<T>(Id);
        }
        public bool Delete<T>(Expression<Func<T, bool>> expr) where T : class, new()
        {
            return manage.Delete<T>(expr); ;
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields) where T : class, new()
        {
            return manage.SelectGroupBy(expression, groupbyfields);
        }
        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where T : class, new()
        {
            return manage.SelectGroupBy<T>(expression, groupbyfields, Flide, Ids);
        }
        public List<T> SelectGroupBy<T>(string groupbyfield) where T : class, new()
        {
            return manage.SelectGroupBy<T>(groupbyfield);
        }

        /// <summary>
        /// 批量将指定字段修改成true
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool LogicDeleteMore<T>(string Ids, string filed) where T : class, new()
        {
            return manage.LogicDeleteMore<T>(Ids, filed);
        }
        /// <summary>
        /// 将指定字段修改成true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool LogicDelete<T>(Expression<Func<T, bool>> expr, string field) where T : class, new()
        {
            return manage.LogicDelete<T>(expr, field);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> SelectAll<T>() where T : class, new()
        {
            return manage.SelectAll<T>();
        }
        /// <summary>
        /// in条件查询 
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(Expression<Func<T, bool>> expr, string Flide, List<string> Ins) where T : class, new()
        {
            return manage.SelectIn<T>(expr, Flide, Ins);
        }
        /// <summary>
        /// in条件查询 
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(string Flide, List<string> Ins, string selectFile = "") where T : class, new()
        {
            return manage.SelectIn<T>(Flide, Ins, selectFile);
        }

        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> exprs, string Flids = "", List<string> InIds = null, string orderfile = "") where T : class, new()
        {
            return manage.SelectSearchs<T>(exprs, Flids, InIds, orderfile);
        }
        /// <summary>
        /// 搜索查询(多条件下使用 a=0 and b=1)
        /// </summary>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression, int topNumber, string orderby = "") where T : class, new()
        {
            return manage.SelectSearch<T>(expression, topNumber, orderby);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public List<T> SelectAppointField<T>(Expression<Func<T, bool>> expression, string Field) where T : class, new()
        {
            return manage.SelectAppointField<T>(expression, Field);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, string sqlwhere = "") where T : class, new()
        {
            return manage.GetTotalCount<T>(expression, sqlwhere);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(string sqlwhere = "") where T : class, new()
        {
            return manage.GetTotalCount<T>(sqlwhere);
        }
        public int GetTotalCount<T>(List<string> Ids, string Flide) where T : class, new()
        {
            return manage.GetTotalCount<T>(Ids, Flide);
        }
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            return manage.GetTotalCount<T>(expression, Ids, Flide);
        }

        public int GetTotalCount<T>(List<Expression<Func<T, bool>>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            return manage.GetTotalCount<T>(expression, Ids, Flide);
        }


        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Select<T>(string ID) where T : class, new()
        {
            return manage.Select<T>(ID);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IList<T> SelectPage<T>(PageParameter<T> parameter, out int totalCount) where T : class, new()
        {
            return manage.SelectPage<T>(parameter, out totalCount);
        }
        /// <summary>
        /// 条件搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return manage.SelectSearch<T>(expression);
        }
        /// <summary>
        /// 设置页面的索引
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int setpageindex(int index, int size)
        {
            if (index == 0)
                index = index / size;
            else
                index = index / size + 1;
            return index;
        }

        public bool TransactionAdd<T1, T2>(RelationEntity<T1, T2> relationEntity)
            where T1 : class, new()
            where T2 : class, new()
        {
            return manage.TransactionAdd<T1, T2>(relationEntity);
        }
        #region 提成结算单明细金额合计
        public List<orderkont> AddTotal(List<orderkont> Listorderkont, decimal? adjustamount)
        {
            if (Listorderkont != null && Listorderkont.Count > 0)
            {
                if (adjustamount != null && adjustamount != 0)
                {
                    Listorderkont.Add(new orderkont()
                    {
                        productname = "调整金额",
                        ordernumber = 0,
                        o_actamount = 0,
                        o_payamount = 0,
                        totalo_bonus = 0,
                        classnumber = 0,
                        classpayamount = 0,
                        classactamount = 0,
                        p_class_bonus = 0,
                        total = adjustamount
                    });
                }
                orderkont entity = new orderkont();
                entity.ordernumber = 0;
                entity.o_actamount = 0;
                entity.o_payamount = 0;
                entity.totalo_bonus = 0;
                entity.classnumber = 0;
                entity.classpayamount = 0;
                entity.classactamount = 0;
                entity.basis_bonus = 0;
                entity.p_class_bonus = 0;
                entity.total = 0M;
                foreach (orderkont row in Listorderkont)
                {
                    entity.ordernumber += row.ordernumber;
                    entity.o_actamount += row.o_actamount;
                    entity.o_payamount += row.o_payamount;
                    entity.totalo_bonus += NullConvertNumber(row.totalo_bonus);
                    entity.classnumber += row.classnumber;
                    entity.classpayamount += NullConvertNumber(row.classpayamount);
                    entity.classactamount += NullConvertNumber(row.classactamount);
                    entity.basis_bonus += NullConvertNumber(row.basis_bonus);
                    entity.p_class_bonus += NullConvertNumber(row.p_class_bonus);
                    entity.total += (NullConvertNumber(row.p_class_bonus) + NullConvertNumber(row.basis_bonus));
                }
                entity.total += adjustamount;
                entity.productname = "总计：";

                Listorderkont.Add(entity);
                return Listorderkont;
            }
            return new List<orderkont>();
        }
        public decimal? NullConvertNumber(decimal? number)
        {
            if (number != null)
                return number;
            return 0;
        }
        #endregion


        public int isend(int districtid, int schoolid)
        {
            if (schoolid > 0)
                return 2;
            if (districtid.ToString().Substring(4, 2) == "00")
                return 0;
            return 1;
        }
    }
}
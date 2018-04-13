using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KSWF.Web.Admin.Controllers
{
    public class ClassStatisController : BaseController
    {
        ClassStatisManage manage = new ClassStatisManage();
        // 班级统计

        public ActionResult Index()
        {
            ViewBag.CatogoryList = KeyValueManage.GetCatogoryData();
            ViewBag.ChannelList = KeyValueManage.GetChannleData();
            ViewBag.PayTypeList = KeyValueManage.GetPayTypeData();
            ViewBag.QudaoList = KeyValueManage.GetQudaoData();

            ViewBag.ActionExport = action.Export;

            ViewBag.ActionDetail = action.Detailed;

            //当前用户允许查询的员工列表
            if ((masterinfo.groupid == 2 || masterinfo.groupid == 3) && masterinfo.agentid == "KSWF")
            {
                ViewBag.MasterList = GetMasterList(0);
            }
            else
            {
                ViewBag.MasterList = GetMasterList(masterinfo.deptid);
            }

            //当前用户允许查询的代理商列表
            ViewBag.AgencyList = GetAgencyList();

            //当前用户允许查询的部门列表
            ViewBag.DeptList = "";

            return View();
        }
        /// <summary>
        /// 班级管理页面按钮显示控制权限
        /// </summary>
        /// <returns></returns>
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        public ActionResult StudentList()
        {
            string cid = Request.QueryString["CID"];
            if (string.IsNullOrEmpty(cid))
            {
                Response.Redirect("\\ClassStatis\\Index");
            }
            ViewBag.ClassID = cid;
            return View();
        }

        /// <summary>
        /// 当前用户允许查询的员工列表
        /// </summary>
        /// <returns></returns>
        private List<cfg_keyvalue> GetMasterList(int deptid)
        {
            return GetMaster(deptid);
        }

        private List<cfg_keyvalue> GetMaster(int deptid)
        {
            List<cfg_keyvalue> list = new List<cfg_keyvalue>();
            Recursive recursive = new Recursive();
            List<com_master> vwlist = new List<com_master>();
            List<string> inIds = new List<string>();
            if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 0 || masterinfo.dataauthority == 3)//部门+下级代理商
            {
                if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 0)//是否包含本部门
                {
                    inIds.Add(deptid.ToString());
                }
                inIds.AddRange(recursive.GetDeptNodeId(deptid, masterinfo.agentid));
                vwlist.AddRange(base.Manage.SelectIn<com_master>(x => x.mastertype == 0 && x.state == 0, "deptid", inIds));
            }
            else
            {
                vwlist.Add(masterinfo);
            }

            foreach (com_master u in vwlist)
            {
                list.Add(new cfg_keyvalue { Key = u.mastername, Value = u.truename });
            }
            return list;
        }
        /// <summary>
        /// 定时同步任务
        /// </summary>
        public void TimedSynchronization()
        {
            try
            {
            
                manage.InitTbClassData();
                manage.InitTbClassTeaData();
                manage.InitTbCLassStuData();
                manage.InitTbTmpAreaSchool();
                GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
                give.MasterChangeInfoDel();
                give.MasterChangeInfoAdd("数据同步", 1, "同步成功", "", "", "", "管理员");
            }
            catch (Exception ex)
            {
                GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
                give.MasterChangeInfoAdd("数据同步", 1, "进入同步(异常)", "", "", ex.Message.ToString(), "管理员");
            }
        }


        public JsonResult ReloadMetaData()
        {
            Core.Utility.KingResponse res = new KingResponse();
            try
            {
                manage.InitTbClassData();
                manage.InitTbClassTeaData();
                manage.InitTbCLassStuData();
                manage.InitTbTmpAreaSchool();
                res = KingResponse.GetResponse("");
            }
            catch (Exception ex)
            {
                res = KingResponse.GetErrorResponse(ex.Message);
            }
            return Json(res);
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
            res = KingResponse.GetResponse(listSchool);
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
            //List<TreeView> lt = new List<TreeView>();
            //List<TreeView> lt2 = new List<TreeView>();
            //TreeView tv = GetDepts("KSWF");
            //TreeView t = new TreeView() { Id = "0", text = "全部", ParentId = "0", tag = "0" };

            //if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 0)//能否查看本部门及以下部门
            //{
            //    if (string.IsNullOrEmpty(tv.Id))
            //    {
            //        lt2.Add(t);
            //        return Json(lt2);
            //    }
            //    lt.Add(tv);
            //    t.nodes = lt;
            //}

            //lt2.Add(t);
            //return Json(lt2);
            int deptId = masterinfo.deptid;
            if (UserIdentity == 1 && masterinfo.dataauthority == (int)Dataauthority.全部)
            {
                List<com_master> master = base.SelectSearch<com_master>(t => t.agentid == masterinfo.agentid && t.mastertype == 1);
                if (master != null && master.Count > 0)
                {
                    List<base_dept> deptlist = base.SelectSearch<base_dept>(t => t.deptid == master[0].deptid);
                    if (deptlist != null && deptlist.Count > 0)
                    {
                        deptId = deptlist[0].parentid;
                    }
                }
            }
            else if (UserIdentity == 0)
            {
                if (masterinfo.dataauthority == (int)Dataauthority.全部)
                {
                    deptId = 1;
                }
            }
            Recursive giveup = new Recursive();
            if (masterinfo.dataauthority == (int)Dataauthority.全部)
                return Json(giveup.GetDept(deptId, masterinfo.agentid));
            if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商)
            {
                return Json(giveup.GetDept(deptId, masterinfo.agentid));
            }
            if (masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                return Json(giveup.SelectDept(deptId, masterinfo.agentid));
            }
            List<base_dept> dept = base.SelectSearch<base_dept>(x => x.deptid == deptId);
            List<TreeView> treeview = new List<TreeView>();
            if (dept.Count > 0)
            {
                for (int i = 0; i < dept.Count; i++)
                {
                    treeview.Add(new TreeView() { tag = dept[i].deptid.ToString(), ParentId = dept[i].parentid.ToString(), text = dept[i].deptname, nodes = null, isContainNods = false });
                }
            }
           
            return Json(treeview);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        public JsonResult GetPageList(int pagesize, int pageindex, int MasterType, [FromBody]OrderCondition ocInfo)
        {
            PageParameter<KSWF.WFM.Constract.Statistic.vw_areaschool> param = new PageParameter<KSWF.WFM.Constract.Statistic.vw_areaschool>();

            param.PageIndex = setpageindex(pageindex, pagesize);
            param.PageSize = pagesize;

            param.OrderColumns = T => T.MasterName;
            param.IsOrderByASC = 1;
            if (!string.IsNullOrEmpty(ocInfo.SearchKey))
            {
                ocInfo.SearchKey = ocInfo.SearchKey.Trim();
            }
            ocInfo.SearchKey = KSWF.Core.Utility.PublicHelp.DelSQLStr(ocInfo.SearchKey);

            param.WhereSql = GetWhereSql(ocInfo);
            param.Wheres = GetOrderCondition(ocInfo);
            if (MasterType == 0)
            {
                param.Wheres.Add(i => i.AgentID == masterinfo.agentid);
            }
            else
            {
                param.Wheres.Add(i => i.AgentID != masterinfo.agentid);
            }

            int totalcount = 0;
            IList<KSWF.WFM.Constract.Statistic.vw_areaschool> list = manage.SelectPage<KSWF.WFM.Constract.Statistic.vw_areaschool>(param, out totalcount);//是否要去重复
            Session["ClassTotalCount"] = totalcount;
            return Json(new { total = totalcount, rows = list });
        }

        /// <summary>
        /// 获取班级统计信息
        /// </summary>
        /// <param name="MasterType"></param>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        public JsonResult GetTotalClassCount(int MasterType, [FromBody]OrderCondition ocInfo)
        {
            int? totalclassCount = Session["ClassTotalCount"] as int?;
            List<Expression<Func<KSWF.WFM.Constract.Statistic.vw_areaschool, bool>>> plist = GetOrderCondition(ocInfo);
            if (MasterType == 0)
            {
                plist.Add(i => i.AgentID == masterinfo.agentid);
            }
            else
            {
                plist.Add(i => i.AgentID != masterinfo.agentid);
            }
            List<KSWF.WFM.Constract.Statistic.vw_areaschool> list = manage.SelectSearch<KSWF.WFM.Constract.Statistic.vw_areaschool>(GetWhereSql(ocInfo), plist);
            decimal d = list.Sum(i => Convert.ToDecimal(i.StuCount));
            return Json(new { ClassCount = totalclassCount, StuCount = d });
        }


        private string GetWhereSql(OrderCondition ocInfo)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("( MasterName = '{0}'", masterinfo.mastername);//本人数据

            ////非部门负责人去掉自己的部门
            //if (masterinfo.dataauthority == 3 || masterinfo.dataauthority == 1)
            //{
            //    deptlist.Remove(masterinfo.deptid.ToString());
            //}

            Recursive give = new Recursive();

            if (masterinfo.dataauthority == (int)Dataauthority.本人 || masterinfo.dataauthority == (int)Dataauthority.本人下级代理商)
            {
                sb.AppendFormat("( MasterName = '{0}'", masterinfo.mastername);
                List<string> agentidlist = give.GetEmplloyAllNodsAgentId(masterinfo.mastername);
                if (agentidlist != null && agentidlist.Count > 0)
                {
                    sb.AppendFormat(" || AgentID in {0}", InFormat(agentidlist));
                }
                sb.AppendFormat(")");
            }
            else if (masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商 || masterinfo.dataauthority == (int)Dataauthority.本人下级部门下级代理商)
            {
                int deptid = masterinfo.deptid;
                if (ocInfo.Dept.HasValue && ocInfo.Dept.Value != 0)
                    deptid = ocInfo.Dept.Value;
                List<string> deptlist = give.GetDeptNodeIdList(deptid);
                if (deptid == masterinfo.deptid && masterinfo.dataauthority == (int)Dataauthority.本人本部门下级部门下级代理商)
                {
                    deptlist.Add(masterinfo.deptid.ToString());
                }

                sb.AppendFormat(" (MasterName = '{0}'", masterinfo.mastername);
                if (deptlist != null && deptlist.Count > 0)
                {
                    sb.AppendFormat(" || DeptID in {0}", InFormat(deptlist));
                }
                List<string> agentidlist = give.GetAllNodsAgentId(deptlist);
                if (agentidlist != null && agentidlist.Count > 0)
                {
                    sb.AppendFormat("|| AgentID in {0}", InFormat(agentidlist));
                }
                sb.AppendFormat(")");
            }
            else
            {
                int deptid = masterinfo.deptid;
                if (ocInfo.Dept.HasValue && ocInfo.Dept.Value >1 )
                    deptid = ocInfo.Dept.Value;
                List<string> deptlist = new List<string>();
                if (ocInfo.Dept.HasValue && ocInfo.Dept.Value >1)
                {
                    deptlist = give.GetDeptNodeIdList(ocInfo.Dept.Value);
                    deptlist.Add(ocInfo.Dept.Value.ToString());
                }

                if (UserIdentity == 1)
                {
                    List<string> agentidlist = give.GetNodsAgentId(masterinfo.agentid);
                    agentidlist.Add(masterinfo.agentid);
                    if (agentidlist != null && agentidlist.Count > 0)
                    {
                        sb.AppendFormat("(AgentID in {0}", InFormat(agentidlist));
                        sb.AppendFormat(")");
                    }
                }
                if (deptlist != null && deptlist.Count > 0)
                {
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.AppendFormat(" and ");
                    }
                    sb.AppendFormat("  ( DeptID in {0}", InFormat(deptlist));
                    sb.AppendFormat(")");

                }
            }

            if (ocInfo != null && !string.IsNullOrEmpty(ocInfo.SearchKey))
            {
                sb.Append("and ");
                sb.Append("(");
                sb.Append("yuwen like '%" + ocInfo.SearchKey + "%' ");
                sb.Append(" or ");
                sb.Append("shuxue like '%" + ocInfo.SearchKey + "%' ");
                sb.Append(" or ");
                sb.Append("yingyu like '%" + ocInfo.SearchKey + "%' ");
                sb.Append(" or ");
                sb.Append(" yuwenMobile ='" + ocInfo.SearchKey + "'");
                sb.Append(" or ");
                sb.Append(" shuxueMobile ='" + ocInfo.SearchKey + "'");
                sb.Append(" or ");
                sb.Append(" yingyuMobile ='" + ocInfo.SearchKey + "'");
                sb.Append(")");
            }
            if (string.IsNullOrEmpty(sb.ToString()))
            {
                sb.AppendFormat(" 1=1 ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取前台传来的条件，拉姆达表达式化
        /// </summary>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        private List<Expression<Func<KSWF.WFM.Constract.Statistic.vw_areaschool, bool>>> GetOrderCondition(OrderCondition ocInfo)
        {
            List<Expression<Func<KSWF.WFM.Constract.Statistic.vw_areaschool, bool>>> expression = new List<Expression<Func<KSWF.WFM.Constract.Statistic.vw_areaschool, bool>>>();
            if (ocInfo != null)
            {
                if (ocInfo.SchoolID.HasValue)
                {
                    expression.Add(i => i.SchoolID == ocInfo.SchoolID);
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
                    expression.Add(i => i.DistrictID.ToString().StartsWith(code));
                }
                if (ocInfo.GradeID.HasValue)
                {
                    expression.Add(i => i.GradeID == ocInfo.GradeID);
                }
                if (!string.IsNullOrEmpty(ocInfo.ClassID))
                {
                    expression.Add(i => i.ClassID == ocInfo.ClassID);
                }

                //if (ocInfo.Dept.HasValue)
                //{
                //    expression.Add(i => i.DeptID == ocInfo.Dept.Value);
                //}
                if (!string.IsNullOrEmpty(ocInfo.MasterName))
                {
                    expression.Add(i => i.MasterName == ocInfo.MasterName);
                }
            }
            return expression;
        }



        /// <summary>
        /// 学生信息分页获取
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="SearchKey"></param>
        /// <param name="ClassID"></param>
        /// <returns></returns>
        public JsonResult GetStudentPageList(int pagesize, int pageindex, string SearchKey, string ClassID)
        {
            PageParameter<KSWF.WFM.Constract.Statistic.vw_studentlist> param = new PageParameter<KSWF.WFM.Constract.Statistic.vw_studentlist>();
            if (pageindex == 0)
                pageindex = pageindex / pagesize;
            else
                pageindex = pageindex / pagesize + 1;
            param.PageIndex = pageindex;
            param.PageSize = pagesize;

            param.OrderColumns = T => T.CreateDate;
            param.IsOrderByASC = 1;
            SearchKey = SearchKey.Trim();
            SearchKey = KSWF.Core.Utility.PublicHelp.DelSQLStr(SearchKey);
            param.Where = i => i.ClassID == ClassID;
            if (!string.IsNullOrEmpty(SearchKey))
            {
                param.WhereSql = "(StuTrueName like '%" + SearchKey + "%' or StuMobile='" + SearchKey + "')";
            }
            int totalcount = 0;
            IList<KSWF.WFM.Constract.Statistic.vw_studentlist> list = manage.SelectPage<KSWF.WFM.Constract.Statistic.vw_studentlist>(param, out totalcount);//是否要去重复
            Session["TotalClassStudent"] = totalcount;
            return Json(new { total = totalcount, rows = list });
        }

        /// <summary>
        /// 获取班级的学生数量
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClassStudentCount()
        {
            return Json(new { StudentCount = Session["TotalClassStudent"] });
        }

        private List<string> GetCurrentDeptID(int deptid = 0)
        {
            List<string> list = new List<string>();
            list = base.GetAllDeptID(deptid);
            return list;
        }

        /// <summary>
        /// 当前用户允许查询的代理商列表
        /// </summary>
        /// <returns></returns>
        private List<cfg_keyvalue> GetAgencyList()
        {
            List<cfg_keyvalue> list = new List<cfg_keyvalue>();
            if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 0
                || masterinfo.dataauthority == 3 || masterinfo.dataauthority == 4)
            {
                List<string> agencynames = new List<string>();
                if (masterinfo.mastertype == 1 || masterinfo.mastertype == 2 || masterinfo.dataauthority == 0)
                {
                    agencynames = GetFirstAgencys(true);
                }
                else if (masterinfo.dataauthority == 2 || masterinfo.dataauthority == 3)
                {
                    agencynames = GetFirstAgencys();
                    agencynames.AddRange(GetDeptEmpAgenecys(true, masterinfo.agentid, masterinfo.deptid));
                }
                else
                {
                    agencynames = GetFirstAgencys();
                }
                var agencys = base.Manage.SelectIn<com_master>("mastername", agencynames);
                foreach (var item in agencys)
                {
                    list.Add(new cfg_keyvalue { Key = item.mastername, Value = item.agentname });
                }
            }
            return list;
        }


        /// <summary>
        /// 导出订到到excel表格
        /// </summary>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        public FileResult ExportStatisXls([FromBody]OrderCondition ocInfo, int MasterType)
        {
            MemoryStream ms = new MemoryStream();
            if (!action.Export)
            {
                return File(ms, "application/txt", "没有权限下载文件.txt");
            }
            List<Expression<Func<KSWF.WFM.Constract.Statistic.vw_areaschool, bool>>> param = GetOrderCondition(ocInfo);
            if (MasterType == 0)
            {
                param.Add(i => i.AgentID == "KSWF");
            }
            else
            {
                param.Add(i => i.AgentID != "KSWF");
            }

            IList<KSWF.WFM.Constract.Statistic.vw_areaschool> list = manage.SelectSearch<KSWF.WFM.Constract.Statistic.vw_areaschool>(GetWhereSql(ocInfo), param);
            if (list == null)
            {
                return File(ms, "application/txt", "当前数据为空.txt");
            }
            list = list.OrderBy(i => i.MasterName).ToList();
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "班级老师信息" + DateTime.Now.ToString("yyyy-MM-dd");
            CreateSheet(list, book, tmpTitle + " ", MasterType);
            book.Write(ms);
            /*这里判断使用的浏览器是否为Firefox
             * （1）
             * Firefox导出文件时不需要对文件名显示编码，编码后文件名会乱码
             * IE和Google需要编码才能保持文件名正常
             * 
             * （2）
             * 经过HttpUtility.UrlEncode方法加密过文件名后该方法将空格替换成了+号，用%20替换掉就可以正常显示了，
             * 但是这个方法在IE和谷歌里面可以解决问题，在火狐里面仍然无效，用%20替换+后输出的就是%20，并不会显示为空格，解决办法如下
             * */
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

        /// <summary>
        /// 创建班级统计表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="book"></param>
        /// <param name="tmpTitle"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        private void CreateSheet(IList<KSWF.WFM.Constract.Statistic.vw_areaschool> list, HSSFWorkbook book, string tmpTitle, int MasterType)
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
            if (MasterType == 1)
            {
                headerrow.CreateCell(0).SetCellValue("代理商");
                headerrow.CreateCell(1).SetCellValue("员工姓名");
            }
            else
            {
                headerrow.CreateCell(0).SetCellValue("部门");
                headerrow.CreateCell(1).SetCellValue("员工姓名");
            }
            headerrow.CreateCell(2).SetCellValue("用户名");
            headerrow.CreateCell(3).SetCellValue("省");
            headerrow.CreateCell(4).SetCellValue("市");
            headerrow.CreateCell(5).SetCellValue("区/县");
            headerrow.CreateCell(6).SetCellValue("学校");
            headerrow.CreateCell(7).SetCellValue("年级");
            headerrow.CreateCell(8).SetCellValue("班级");
            headerrow.CreateCell(9).SetCellValue("语文老师");
            headerrow.CreateCell(10).SetCellValue("语文老师电话");
            headerrow.CreateCell(11).SetCellValue("数学老师");
            headerrow.CreateCell(12).SetCellValue("数学老师电话");
            headerrow.CreateCell(13).SetCellValue("英语老师");
            headerrow.CreateCell(14).SetCellValue("英语老师电话");
            headerrow.CreateCell(15).SetCellValue("绑定人数");

            for (int i = 0; i < list.Count; i++)
            {
                KSWF.WFM.Constract.Statistic.vw_areaschool toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式


                if (MasterType == 1)
                {
                    row.CreateCell(0).SetCellValue(string.IsNullOrEmpty(toinfo.AgentName) ? toinfo.DeptName : toinfo.AgentName);
                    row.CreateCell(1).SetCellValue(toinfo.TrueName);
                }
                else
                {
                    row.CreateCell(0).SetCellValue(toinfo.DeptName);
                    row.CreateCell(1).SetCellValue(toinfo.TrueName);
                }
                row.CreateCell(2).SetCellValue(toinfo.MasterName);
                if (toinfo.Path != null)
                {
                    string[] paths = toinfo.Path.Split(' ');
                    //省
                    row.CreateCell(3).SetCellValue(paths[0]);
                    if (paths.Length > 1)
                    {
                        //市
                        if (paths.Length > 2)
                        {
                            row.CreateCell(4).SetCellValue(paths[1]);
                        }
                        else
                        {
                            row.CreateCell(4).SetCellValue(paths[0]);
                        }
                        //区
                        if (paths.Length > 2)
                        {
                            row.CreateCell(5).SetCellValue(paths[2]);
                        }
                        else
                        {
                            row.CreateCell(5).SetCellValue(paths[1]);
                        }
                    }
                }
                row.CreateCell(6).SetCellValue(toinfo.SchoolName);
                row.CreateCell(7).SetCellValue(toinfo.GradeName);
                row.CreateCell(8).SetCellValue(toinfo.ClassName);
                row.CreateCell(9).SetCellValue(toinfo.yuwen);
                row.CreateCell(10).SetCellValue(toinfo.yuwenMobile);
                row.CreateCell(11).SetCellValue(toinfo.shuxue);
                row.CreateCell(12).SetCellValue(toinfo.shuxueMobile);
                row.CreateCell(13).SetCellValue(toinfo.yingyu);
                row.CreateCell(14).SetCellValue(toinfo.yingyuMobile);
                row.CreateCell(15).SetCellValue(toinfo.StuCount);

            }
        }

        public FileResult ExportStudentXls(string ClassID)
        {
            MemoryStream ms = new MemoryStream();
            //要考虑数据权限（待处理）
            IList<KSWF.WFM.Constract.Statistic.vw_studentlist> list = manage.SelectSearch<KSWF.WFM.Constract.Statistic.vw_studentlist>(i => i.ClassID == ClassID);
            if (list == null)
            {
                return File(ms, "application/txt", "当前数据为空.txt");
            }
            list = list.OrderBy(i => i.CreateDate).ToList();
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "班级学生信息" + DateTime.Now.ToString("yyyy-MM-dd");
            CreateStudentSheet(list, book, tmpTitle + " ", 0, list.Count);
            book.Write(ms);
            /*这里判断使用的浏览器是否为Firefox
             * （1）
             * Firefox导出文件时不需要对文件名显示编码，编码后文件名会乱码
             * IE和Google需要编码才能保持文件名正常
             * 
             * （2）
             * 经过HttpUtility.UrlEncode方法加密过文件名后该方法将空格替换成了+号，用%20替换掉就可以正常显示了，
             * 但是这个方法在IE和谷歌里面可以解决问题，在火狐里面仍然无效，用%20替换+后输出的就是%20，并不会显示为空格，解决办法如下
             * */
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

        private void CreateStudentSheet(IList<KSWF.WFM.Constract.Statistic.vw_studentlist> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            //headerrow.CreateCell(0).SetCellValue("序号");
            //headerrow.CreateCe4ll(2).SetCellValue("订单结算状态");
            //headerrow.CreateCell(9).SetCellValue("老师手机号");
            //headerrow.CreateCell(13).SetCellValue("商品年级");

            headerrow.CreateCell(0).SetCellValue("学校");
            headerrow.CreateCell(1).SetCellValue("班级名称");
            headerrow.CreateCell(2).SetCellValue("班级ID");
            headerrow.CreateCell(3).SetCellValue("学生手机号");
            headerrow.CreateCell(4).SetCellValue("学生用户编号");
            headerrow.CreateCell(5).SetCellValue("学生用户名");
            headerrow.CreateCell(6).SetCellValue("学生姓名");
            headerrow.CreateCell(7).SetCellValue("加入时间");
            for (int i = startIndex; i < endIndex; i++)
            {
                KSWF.WFM.Constract.Statistic.vw_studentlist toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式
                row.CreateCell(0).SetCellValue(toinfo.SchoolName);
                row.CreateCell(1).SetCellValue(toinfo.ClassName);
                row.CreateCell(2).SetCellValue(toinfo.ClassNum);
                row.CreateCell(3).SetCellValue(toinfo.StuMobile);
                row.CreateCell(4).SetCellValue(toinfo.StuUserID);
                row.CreateCell(5).SetCellValue(toinfo.StuUserName);
                row.CreateCell(6).SetCellValue(toinfo.StuTrueName);
                row.CreateCell(7).SetCellValue(toinfo.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            }
        }
    }

    public static class ObjectExtend
    {


        //重写GetHashCode方法（重写Equals方法必须重写GetHashCode方法，否则发生警告

        public static string ToMinDistrictID(this object obj)
        {
            string temp = obj.ToString();
            try
            {
                string code = "";
                string[] strs = new string[4];
                strs[0] = temp.ToString().Substring(0, 2);
                strs[1] = temp.ToString().Substring(2, 2);
                strs[2] = temp.ToString().Substring(4, 2);
                strs[3] = temp.ToString().Substring(6);
                for (int i = 3; i >= 0; i--)
                {
                    if (int.Parse(strs[i]) != 0)
                    {
                        code = strs[i] + code;
                    }
                }
                return code;
            }
            catch
            {

            }
            return temp;
        }

        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToIntOrZero(this object obj)
        {
            int temp = 0;
            try
            {
                if (!int.TryParse(obj.ToString(), out temp))
                {
                    temp = 0;
                }
            }
            catch
            {

            }
            return temp;
        }
    }



}

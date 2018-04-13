using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Caching;

namespace KSWF.Web.Admin.Controllers
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public class EmployeeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Employee_Add()
        {
            return View();
        }
        /// <summary>
        /// 详细
        /// </summary>
        /// <returns></returns>
        public ActionResult Employee_Detailed()
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

        #region 获取部门(添加时选择)
        /// <summary>
        /// 获取部门(添加时选择)
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="AgentId"></param>
        /// <returns></returns>
        public JsonResult SelectDept()
        {
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
                    deptId = 0;
                }
            }
            Recursive giveup = new Recursive();
            if (deptId == 0 || masterinfo.dataauthority == (int)Dataauthority.全部)
                return Json(giveup.SelectDept(deptId, masterinfo.agentid));
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
        #endregion

        #region 判断用户中是否存在 UserNameIsExist(int UserId, string UserName)

        /// <summary>
        /// 判断用户中是否存在
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationAttribute(IsCheck = false)]
        public int UserNameIsExist(int UserId, string UserName)
        {
            if (UserId > 0)
                return base.GetTotalCount<com_master>(x => (x.mastername == UserName && x.masterid != UserId && x.state==0));

            return base.GetTotalCount<com_master>(x => (x.mastername == UserName && x.state == 0));
        }
        #endregion

        [HttpPost]
        public JsonResult GetAllGroup()
        {
            if (masterinfo.agentid == Core.Utility.PublicHelp.OrgId)
                return Json(base.SelectSearch<com_group>(x => x.delflg == 0));
            return Json(base.SelectSearch<com_group>(x => (x.delflg == 0 && x.creatername == "系统默认")));
        }
        public JsonResult GetMasterById(string Id)
        {
            return Json(base.Select<com_master>(Id));
        }

        public JsonResult GetMasterAgentNumber(string Id)
        {
            return Json(base.GetTotalCount<com_master>(t => t.state==0 &&  t.parentid == Convert.ToInt32(Id)));
        }

        #region 添加用户 Employee_Add(string jsondata, string Areas, string ProductIds)
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="Areas"></param>
        /// <param name="ProductIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Employee_Add(string jsondata, string Areas)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Add) //没有预览权限
            {
                res.ErrorMsg = "您没有操作权限~";
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                com_master masterdata = (com_master)serializer.Deserialize(jsondata, typeof(com_master));
                AreaFilter filter = new AreaFilter();
                int type = 1;
                if (UserIdentity == 1)
                    type = 2;
                KingResponse rest = filter.JudgmentArea(Areas, masterdata.mastername,masterdata.deptid, type, base.masterinfo.agentid, "");
                if (rest.Success)
                {
                    rest.Success = false;
                    if (rest.ErrorMsg.Contains("子部门负责区域相同"))
                    {
                        rest.ErrorMsg = "所选地区与子部门负责地区相同！请重新选择~";
                    }
                    return Json(rest);
                }

                string mastername = masterdata.mastername;//用户名
                masterdata.mastertype = 0;
                masterdata.createid = masterinfo.masterid;
                masterdata.parentname = "";
                masterdata.parentid = 0;
                masterdata.agentid = masterinfo.agentid;//添加用户使用固定id
                masterdata.password = PublicHelp.pswToSecurity(masterdata.password);
                masterdata.createtime = DateTime.Now;
                int masterid = base.Add<com_master>(masterdata);//添加用户
                if (masterid > 0)
                {
                    List<join_mastertarea> entitys = new List<join_mastertarea>();
                    if (!string.IsNullOrEmpty(Areas))
                    {
                        #region 设置区域
                        string[] areasarray = Areas.TrimEnd('@').Split('@');
                        foreach (string row in areasarray)
                        {
                            string[] areaarray = row.Split('|');
                            string ProductIds = "";
                            for (int i = 0; i < areasarray.Length; i++)
                            {
                                string[] cobyareaarray = areasarray[i].Split('|');
                                if (areaarray[0] == cobyareaarray[0] && areaarray[2] == cobyareaarray[2])
                                {
                                    ProductIds += cobyareaarray[4].ToString() + ",";
                                }
                            }
                            entitys.Add(new join_mastertarea() { mastername = mastername, districtid = areaarray[0], path = areaarray[1], schoolid = Convert.ToInt32(areaarray[2]), schoolname = areaarray[3] == null ? "" : areaarray[3], pids = AddFh(ProductIds) });
                        }
                        entitys = GoHeavy(entitys);
                        if (entitys.Count > 0 && base.InsertRange<join_mastertarea>(entitys) != null)
                        {
                            res.Success = true;
                            return Json(res);
                        }

                        #endregion
                    }
                    else
                    {
                        res.Success = true;
                        return Json(res);
                    }
                }
                else
                {
                    res.ErrorMsg = "添加失败！请重试~";
                }
            }
            return Json(res);
        }
        #endregion

        /// <summary>
        /// 用户对应区域去除重复
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<join_mastertarea> GoHeavy(List<join_mastertarea> entitys)
        {
            if (entitys != null && entitys.Count > 0)
            {
                for (int i = 0; i < entitys.Count; i++)
                {
                    for (int j = (i + 1); j < entitys.Count; j++)
                    {
                        if (entitys[j].districtid == entitys[i].districtid && entitys[j].schoolid == entitys[i].schoolid && entitys[j].pids == entitys[i].pids)
                        {
                            entitys.RemoveAt(j);//去除相同的项
                            i = 0;//从新开始去重，如果数组内有大量重复的项，仅一次去重不能解决问题。这样的用法会使效率慢1/3
                            j = 0;
                        }
                    }
                }
            }
            return entitys;
        }


        #region 编辑用户 Employee_Edit(string jsondata, string Areas)
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="Areas"></param>
        /// <param name="ProductIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Employee_Edit(string masterId, string mastername, string truename, string mobile, string email, string qq, string deptid, string groupid, string agent_remark, string dataauthority, string Areas, string productkeys)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Edit)
                res.ErrorMsg = "您没有操作权限~";
            else
            {

                AreaFilter filter = new AreaFilter();
                int type = 1;
                if (UserIdentity == 1)
                    type = 2;
                KingResponse rest = filter.JudgmentArea(Areas, mastername, int.Parse(deptid), type, base.masterinfo.agentid, ","+productkeys);
                if (rest.Success)
                {
                    rest.Success = false;
                    if (rest.ErrorMsg.Contains("子部门负责区域相同"))
                    {
                        rest.ErrorMsg = "所选地区与子部门负责地区相同！请重新选择~";
                    }
                    return Json(rest);
                }
                var obj = new { mastername = mastername, truename = truename, mobile = mobile, email = email, qq = qq, deptid = deptid, groupid = groupid, agent_remark = agent_remark, dataauthority = dataauthority };
                com_master masterinfo = base.Select<com_master>(masterId);
                if (base.Update<com_master>(obj, t => t.mastername == mastername))
                {
                    GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();

                    #region 增加部门角色变更记录
                    if (masterinfo.deptid != Convert.ToInt32(deptid))
                        give.MasterChangeInfoAdd(mastername, (int)KSWF.Web.Admin.Models.ChangeType.部门变更, masterinfo.deptid.ToString(), "", deptid, "", masterinfo.mastername);
                    if (masterinfo.groupid != Convert.ToInt32(groupid))
                        give.MasterChangeInfoAdd(mastername, (int)KSWF.Web.Admin.Models.ChangeType.角色变更, masterinfo.groupid.ToString(), "", groupid, "", masterinfo.mastername);
                    #endregion

                    #region 原负责区域
                    string masterareaids = "";
                    string masteraresnames = "";
                    List<join_mastertarea> masterareslist = base.SelectSearch<join_mastertarea>(j => j.mastername == mastername);
                    if (masterareslist != null && masterareslist.Count > 0)
                    {
                        foreach (join_mastertarea row in masterareslist)
                        {
                            if (row.schoolid > 0)
                            {
                                masterareaids += row.schoolid.ToString() + ",";
                                masteraresnames += row.path + row.schoolname + ",";
                            }
                            else
                            {
                                masterareaids += row.districtid + ",";
                                masteraresnames += row.path + ",";
                            }
                        }
                    }
                    #endregion

                    List<join_mastertarea> entitys = new List<join_mastertarea>();
                    if (!string.IsNullOrEmpty(Areas))
                    {
                        string newareaids = "";
                        string newareanames = "";

                        #region 设置区域
                        string[] areasarray = Areas.TrimEnd('@').Split('@');
                        if (areasarray.Length > 0)
                        {
                            foreach (string row in areasarray)
                            {
                                string[] areaarray = row.Split('|');
                                string ProductIds = "";
                                for (int i = 0; i < areasarray.Length; i++)
                                {
                                    string[] cobyareaarray = areasarray[i].Split('|');
                                    if (areaarray[0] == cobyareaarray[0] && areaarray[2] == cobyareaarray[2])
                                    {
                                        if (!string.IsNullOrEmpty(productkeys))
                                            ProductIds += productkeys;
                                        else
                                            ProductIds += GetProductIds(cobyareaarray[4].ToString());
                                    }
                                }
                                if (Convert.ToInt32(areaarray[2]) > 0)
                                    newareaids += areaarray[2] + ",";
                                else
                                    newareaids += areaarray[0];
                                newareanames += areaarray[1] + ",";
                                entitys.Add(new join_mastertarea() { mastername = mastername, districtid = areaarray[0], path = areaarray[1], schoolid = Convert.ToInt32(areaarray[2]), schoolname = areaarray[3] == null ? "" : areaarray[3], pids = AddFh(ProductIds) });
                            }
                            entitys = GoHeavy(entitys);
                            if (entitys.Count > 0 && base.Manage.UpdateActionBusinessAffairs<join_mastertarea>(ag => ag.mastername == mastername, entitys) > 0)//执行事务
                            {
                                #region 判断区域是否有改变或者减少
                                if (!string.IsNullOrEmpty(masterareaids))//用户原来负责的区域
                                {
                                    string[] arrayoldarea = masterareaids.TrimEnd(',').Split(',');
                                    foreach (string row in arrayoldarea)
                                    {
                                        newareaids = "," + newareaids;
                                        if (!newareaids.Contains("," + row + ","))
                                        {
                                            give.MasterChangeInfoAdd(mastername, (int)KSWF.Web.Admin.Models.ChangeType.区域变更, masterareaids, masteraresnames, newareaids, newareanames, masterinfo.mastername);
                                            break;
                                        }
                                    }
                                }
                                #endregion
                                res.Success = true;
                            }
                            else
                            {
                                res.ErrorMsg = "编辑负责区域失败！请重试~";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        base.Delete<join_mastertarea>(t => t.mastername == mastername);//没有选择地区，删除该用户下所有地区
                        if (string.IsNullOrEmpty(masterareaids))
                            give.MasterChangeInfoAdd(mastername, (int)KSWF.Web.Admin.Models.ChangeType.区域变更, masterareaids, masteraresnames, "", "删除", masterinfo.mastername);
                        res.Success = true;
                    }
                }
                else
                {
                    res.ErrorMsg = "编辑用户失败！请重试~";
                }
            }
            return Json(res);
        }
        #endregion


        public string GetProductIds(string productId)
        {
            string productIds = "";
            if (!string.IsNullOrEmpty(productId))
            {
                string[] array = productId.Split(',');
                if (array != null && array.Length > 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i] != null && !string.IsNullOrEmpty(array[i]))
                        { 
                            string id=","+array[i]+",";
                            if (!productIds.Contains(id))
                                productIds += array[i] + ",";
                        }
                    }
                }
            }
            return  productIds;
        }

        public string AddFh(string productId)
        {
            productId = GetProductIds(productId);
            productId=productId.Trim();
            if (!string.IsNullOrEmpty(productId))
            {
                if (productId.Substring(0, 1) != ",")
                {
                    productId = "," + productId;
                }
                if (productId.Substring(productId.Length-1, 1) != ",")
                {
                    productId += ",";
                }
            }
            return productId;
        }

        #region 修改密码 UpdatePossword(string MasterName, string Possword)
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="MasterName"></param>
        /// <param name="Possword"></param>
        /// <returns></returns>
        public JsonResult UpdatePossword(string MasterName, string Possword)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Edit)
                res.ErrorMsg = "您没有操作权限~";
            if (!string.IsNullOrEmpty(MasterName) && !string.IsNullOrEmpty(Possword))
            {
                if (base.Update<com_master>(new { password = PublicHelp.pswToSecurity(Possword) }, t => t.mastername == MasterName))
                {
                    res.Success = true;
                    return Json(res);
                }
            }
            else
            {
                res.ErrorMsg = "用户名密码不能为空~";
            }
            return Json(res);
        }
        #endregion


        #region 分页预览  Employee_View(int pagesize, int pageindex, int deptid, string mastername)
        [HttpPost]
        public JsonResult Employee_View(int pagesize, int pageindex, int deptid, string mastername, int type)
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
            exprlist.Add(t => t.mastertype == 0);
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(mastername))
            {
                exprlist.Add(t => t.truename.Contains(mastername));
            }
            pageParameter.Field = Flids;
            pageParameter.In = InIds;
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_user> usre = base.Manage.SelectPage<vw_user>(pageParameter, out total);
            for (int i = 0; i < usre.Count; i++)
            {
                usre[i].responsiblearea = GetArea(usre[i].mastername);
            }
            return Json(new { total = total, rows = usre });
        }
        /// <summary>
        /// 获取用户负责区域
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public string GetArea(string mastername)
        {
            List<join_mastertarea> arealist = base.SelectSearch<join_mastertarea>(x => x.mastername == mastername, 0);
            string area = "";
            if (arealist != null && arealist.Count > 0)
                for (int i = 0; i < arealist.Count; i++)
                    area += arealist[i].path + arealist[i].schoolname + ",";
            return area.TrimEnd(',');
        }
        #endregion

        #region 导出
        [HttpPost]
        public FileResult Employee_Export([System.Web.Http.FromBody]com_master masterInfo)
        {
            string Flids = "";
            List<string> InIds = null;
            List<Expression<Func<vw_user, bool>>> exprlist = GetUserWheres(masterInfo.mastername, masterInfo.deptid, out Flids, out InIds);
            exprlist.Add(t => t.mastertype == 0);
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(masterInfo.mastername))
                exprlist.Add(t => t.truename.Contains(masterInfo.mastername));
            List<vw_user> list = base.SelectSearchs<vw_user>(exprlist, Flids, InIds, " masterid desc");
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "用户" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("用户名");
            headerrow.CreateCell(2).SetCellValue("真实姓名");
            headerrow.CreateCell(3).SetCellValue("角色");
            headerrow.CreateCell(4).SetCellValue("所属部门");
            headerrow.CreateCell(5).SetCellValue("负责区域");
            for (int i = 0; i < list.Count; i++)
            {
                vw_user userinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(userinfo.mastername);
                row.CreateCell(2).SetCellValue(userinfo.truename);
                row.CreateCell(3).SetCellValue(userinfo.groupname);
                row.CreateCell(4).SetCellValue(userinfo.deptname);
                row.CreateCell(5).SetCellValue(GetArea(userinfo.mastername));
            }
            MemoryStream ms = new MemoryStream();
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
        #endregion

        #region 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public List<Expression<Func<vw_user, bool>>> GetUserWheres(string mastername, int deptid, out string Flide, out List<string> inids)
        {
            Recursive give = new Recursive();
            List<Expression<Func<vw_user, bool>>> exprlist = new List<Expression<Func<vw_user, bool>>>();
            exprlist.Add(t => t.state == 0);
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

        #region 区域加载（过滤）
        /// <summary>
        /// 区域加载（过滤）
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CompDepart_GetAreas(int deptid, int parentid, string mastername, int type, string productid)
        {
            if (UserIdentity == 1)
                type = 2;
            //根据deptid，在base_deptarea,获取到districtid集合
            List<TreeView> treeViews = new List<TreeView>();
            if (masterinfo.agentid == Core.Utility.PublicHelp.OrgId && parentid == 0)
            {
                TreeView baseTreeView = new TreeView();
                baseTreeView.text = "全国";
                baseTreeView.tag = "0|0=0";
                baseTreeView.nodes = new List<TreeView>();
                treeViews.Add(baseTreeView);
                return Json(KingResponse.GetResponse(treeViews));
            }
            var listArea = base.Manage.SelectSearch<base_deptarea>(x => x.deptid == deptid);
            string agentid = base.masterinfo.agentid;
            AreaFilter filter = new AreaFilter();
            foreach (var area in listArea)
            {
                TreeView treeView = new TreeView();

                CheckedCheck s = new CheckedCheck();
                string a = "";
                if (HasChild(area.districtid))
                {
                    treeView.nodes = new List<TreeView>();
                }
                else
                {
                    area.isend = 1;
                }

                //新增学校逻辑
                if (area.schoolid > 0)
                {
                    treeView.text = area.schoolname;
                    treeView.tag = area.districtid.ToString() + "|" + area.path + "|" + area.schoolid + "|" + area.schoolname;
                    treeView.Id = area.schoolid.ToString();
                }
                else
                {
                    treeView.text = area.path;
                    treeView.tag = area.districtid.ToString() + "|" + area.path + "|0|";
                    treeView.Id = area.districtid.ToString();
                }


                if (area.schoolid > 0)
                {
                    s.showcheckbox =  filter.JudgmentSelectedSchool(treeView.tag, mastername, deptid, type,  masterinfo.agentid, productid, out a);
                    s.@checked = SchoolIsChecked(area.schoolid, mastername, productid);
                    if (!s.showcheckbox)
                    {
                        s.showcheckbox = filter.JudgmentSelectedAreaIdSchool(treeView.tag, mastername, deptid, type, masterinfo.agentid, productid, out a);
                    }
                }
                else
                {
                    s.showcheckbox =   filter.JudgmentSelectedAreaId(treeView.tag, mastername, deptid, type, masterinfo.agentid, productid, out a);
                    s.@checked = AreaIsChecked(area.districtid.ToString(), mastername, productid);
                }
                treeView.state = s;
                treeViews.Add(treeView);
            }
            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }

        /// <summary>
        /// 地区是否选中
        /// </summary>
        /// <returns></returns>
        public bool AreaIsChecked(string areaId, string mastername, string pids)
        {

            bool ischecked = false;
            if (!string.IsNullOrEmpty(mastername))
            {
                return base.GetTotalCount<join_mastertarea>(j => (j.mastername == mastername && j.districtid == areaId && j.schoolid == 0 && j.pids.Contains(pids))) > 0;
            }
            return ischecked;
        }

        /// <summary>
        /// 学校是否选中
        /// </summary>
        /// <returns></returns>
        public bool SchoolIsChecked(int schoolid, string mastername, string pids)
        {

            bool ischecked = false;
            if (!string.IsNullOrEmpty(mastername))
            {
                return base.GetTotalCount<join_mastertarea>(j => (j.schoolid == schoolid && j.mastername == mastername && j.pids.Contains(pids))) > 0;
            }
            return ischecked;
        }

        private bool HasChild(int districtid)
        {
            ServiceArea servicearea = new ServiceArea();
            var strChildArea = servicearea.GetArea(districtid);
            var listChildArea = JsonConvert.DeserializeObject<List<AreaView>>(strChildArea);
            bool hasChild = false;
            if (listChildArea != null && listChildArea.Count > 0)
            {
                hasChild = listChildArea[0].IsEnd == "1" ? false : true;
            }
            return hasChild;
        }

        [HttpPost]
        public JsonResult CompDepart_GetChildrenAreas(string parentid, string mastername, int deptid, int type, string productid)
        {
            if (UserIdentity == 1)
                type = 2;
            List<TreeView> treeViews = new List<TreeView>();
            string[] strPars = parentid.Split(new char[] { '|' });

            if (strPars.Length > 0)
            {
                string[] isendAndPath = strPars[1].Split(new char[] { '=' });
                if (isendAndPath.Length > 0)
                {
                    //if (isendAndPath[0] == "0" && !string.IsNullOrEmpty(strPars[0]))
                    //{
                    ServiceArea servicearea = new ServiceArea();
                    var strArea = servicearea.GetArea(Convert.ToInt32(strPars[0]));
                    var listArea = JsonConvert.DeserializeObject<List<AreaView>>(strArea);
                    //if (listArea != null && listArea.Count > 0 && listArea[0].IsEnd != "1")
                    //{
                    AreaFilter filter = new AreaFilter();
                    foreach (var area in listArea)
                    {
                        TreeView treeView = new TreeView();
                        treeView.text = area.CodeName;
                        treeView.schoolid = 0;
                        if (HasChild(Convert.ToInt32(listArea[0].ID)))
                        {
                            treeView.nodes = new List<TreeView>();
                        }
                        else
                        {
                            area.IsEnd = "1";
                        }
                        CheckedCheck s = new CheckedCheck();
                        if (area.CodeName == "市辖区" || area.CodeName == "市辖县" || area.CodeName == "县")
                        {
                            treeView.tag = area.ID + "|" + area.Path+ "、"+ area.CodeName + "|0|";
                        }
                        else
                        {
                            treeView.tag = area.ID + "|" + area.Path + "|0|";
                        }
                        string a = "";
                        s.showcheckbox = filter.JudgmentSelectedAreaId(treeView.tag, mastername, deptid, type, masterinfo.agentid, productid, out a);
                        s.@checked = AreaIsChecked(area.ID, mastername, productid);
                        treeView.state = s;

                        treeView.Id = area.ID;
                        treeViews.Add(treeView);
                    }
                    //}
                    // }
                }
            }

            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }
        [HttpPost]
        public JsonResult CompDepart_GetSchools(string tag, string mastername, int deptid, int type, string productid)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                string[] array = tag.Split('|');
                if (Convert.ToInt32(array[2]) > 0)
                    return Json("");
            }

            if (UserIdentity == 1)
                type = 2;
            List<TreeView> treeViews = new List<TreeView>();
            string[] strPars = tag.Split(new char[] { '|' });

            ServiceArea servicearea = new ServiceArea();
            string areaId = strPars[0].Substring(0, strPars[0].Length - 3) + "000";
            string strAllSchool = servicearea.GetSchool(areaId);

            var listAllSchool = JsonConvert.DeserializeObject<List<ViewSchoolInfo>>(strAllSchool);
            var strSchoolTypeNo = ConfigurationManager.AppSettings["SchoolTypeNo"];
            var schoolTypeNos = strSchoolTypeNo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            AreaFilter filter = new AreaFilter();
            string agentid = masterinfo.agentid;
            List<vw_masterarea> masterarealist = filter.GetUserArea(mastername, type, agentid, productid, areaId.Substring(0,2));
            List<vw_deptarea> deptarelist = filter.GetDeptArea(deptid, agentid, areaId.Substring(0, 2));
            foreach (var school in listAllSchool)
            {
                if (schoolTypeNos.Contains(school.SchoolTypeNo))
                {
                    TreeView treeView = new TreeView();
                    treeView.tag = school.DistrictID.ToString() + "|" + school.Area + "|" + school.ID + "|" + school.SchoolName;
                    CheckedCheck s = new CheckedCheck();
                    bool isselected = filter.JudgmentSelectedSchoolFilter(school.ID, masterarealist, deptid, type, agentid, productid, deptarelist);
                    s.@checked = SchoolIsChecked(Convert.ToInt32(school.ID), mastername, school.DistrictID.ToString());
                    if (!isselected)
                    {
                        isselected = filter.JudgmentSelectedAreaIdSchoolFilter(school.DistrictID.ToString(), masterarealist, deptid, type, masterinfo.agentid, productid, deptarelist);
                    }
                    treeView.Id = school.DistrictID.ToString();
                    treeView.schoolid = school.ID;
                    s.showcheckbox = isselected;
                    treeView.text = school.SchoolName;
                    treeView.ParentId = school.DistrictID.ToString();
                    treeView.state = s;
                    treeViews.Add(treeView);

                }
            }
            HttpContext.Cache.Remove("schools");
            HttpContext.Cache.Add("schools", treeViews, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), CacheItemPriority.Normal, null);
            var result = KingResponse.GetResponse(treeViews);
            return Json(result);
        }

        #endregion

        /// <summary>
        /// 获取部门员工
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDeptEmployee(int masterid, int deptid)
        {
            List<com_master> masterlist = base.SelectSearch<com_master>(j => (j.state == 0 && j.deptid == deptid && j.masterid != masterid && (j.groupid == 4 || j.groupid == 5)));
            return Json(masterlist);
        }
        /// <summary>
        /// 获取业务员所有的代理商
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEmployeeAllAgent(int masterid)
        {
            List<com_master> masterlist = base.SelectSearch<com_master>(j => (j.state == 0 && j.parentid == masterid && j.masterid != masterid && j.mastertype == 1));
            return Json(masterlist);
        }
        [HttpPost]
        /// <summary>
        /// 拉黑
        /// </summary>
        /// <param name="masterid">被拉黑的用户</param>
        /// <param name="areahandover">交接区域的用户</param>
        /// <param name="channelhandover">交接的渠道经理</param>
        /// <returns></returns>
        public bool EmplloyPullBlack(int masterid, string mastername, string areahandover, string channelhandover)
        {
            if (!action.Pullblack)
                return false;
            object obj = new { state = 1, createtime = DateTime.Now };
            Expression<Func<com_master, bool>> expr = o => o.masterid == masterid;
            // || (!string.IsNullOrEmpty(channelhandover) && channelhandover.Contains("|"))
            if (!string.IsNullOrEmpty(areahandover))
            {
                object obj1 = null;
                Expression<Func<join_mastertarea, bool>> expr1 = null;
                if (!string.IsNullOrEmpty(areahandover))
                {
                    obj1 = new { mastername = areahandover };
                    expr1 = o => o.mastername == mastername;
                }
                object obj2 = null;
                Expression<Func<com_master, bool>> expr2 = null;
                if (!string.IsNullOrEmpty(channelhandover) && channelhandover.Contains("|"))
                {
                    string[] masters = channelhandover.Split('|');
                    obj2 = new { parentid = Convert.ToInt32(masters[0]), parentname = masters[1] };
                    expr2 = o => o.parentid == masterid;
                }
                return base.Update<com_master, join_mastertarea, com_master>(obj, expr, obj1, expr1, obj2, expr2);
            }
            List<string> masternames = new List<string>();
            masternames.Add(mastername);
            return base.UpdateDelete<base_dept, join_mastertarea, com_master>(null, null, ma => ma.mastername, masternames, new { state = 1, createtime = DateTime.Now }, expr);
        }

        [HttpPost]
        public JsonResult GetProduct()
        {
            if (masterinfo.agentid == Core.Utility.PublicHelp.OrgId)
                return Json(base.SelectSearch<cfg_keyvalue>(t => t.UseType == "Channel"));
            List<com_master> master = base.SelectSearch<com_master>(m => m.agentid == masterinfo.agentid && m.mastertype == 1);
            if (master != null && master.Count > 0)
            {
                List<join_mastertarea> masterarea = base.SelectSearch<join_mastertarea>(m => m.mastername == master[0].mastername, 1, "id");
                if (masterarea != null && masterarea.Count > 0)
                {
                    List<Expression<Func<cfg_keyvalue, bool>>> exprlist = new List<Expression<Func<cfg_keyvalue, bool>>>();
                    exprlist.Add(t => t.UseType == "Channel");
                    List<string> list = new List<string>();
                    string[] array = masterarea[0].pids.Split(',');
                    foreach (string row in array)
                        list.Add(row);
                    return Json(base.SelectSearchs<cfg_keyvalue>(exprlist, "`Key`", list));
                }
            }
            return Json("");
        }

    }
}


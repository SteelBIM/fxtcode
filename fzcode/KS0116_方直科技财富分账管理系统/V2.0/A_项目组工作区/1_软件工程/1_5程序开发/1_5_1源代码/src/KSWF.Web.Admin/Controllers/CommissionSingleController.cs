using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using KSWF.WFM.BLL;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using KSWF.WFM.Constract.Models;

namespace KSWF.Web.Admin.Controllers
{
    public class CommissionSingleController : BaseController
    {
        public static int Type = 0;
        // 应该付提成结算单
        public ActionResult Index()
        {
            return View();
        }
        //代理商结算单
        public ActionResult ViewBill()
        {
            if (Request["t"] != null)
                Type = 1;
            return View();
        }
        //员工结算单
        public ActionResult EmployeeBill()
        {
            if (Request["t"] != null)
                Type = 1;
            return View();
        }
        //部门结算单
        public ActionResult DeptBill()
        {
            if (Request["t"] != null)
                Type = 1;
            return View();
        }


        /// <summary>
        /// 更新权限
        /// </summary>
        public void UpdateAction()
        {
            if (Type > 0)
            {
                List<vw_action> list = Session["Action"] as List<vw_action>;
                if (list != null && list.Count > 0)
                {
                    #region 获取权限
                    foreach (vw_action item in list)
                    {
                        if (item.actionurl == "PaySingle")
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
                        }
                    }
                    #endregion
                }
            }
        }
        #region  获取代理商 员工 部门负责人
        [HttpPost]
        public JsonResult GetAgent(int deptid)
        {
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            return give.GetAgent(masterinfo.agentid, deptid);
        }
        [HttpPost]
        public JsonResult GetEmplloyee(int deptid)
        {
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            return give.GetEmplloyee(masterinfo.agentid, deptid);
        }

        [HttpPost]
        public JsonResult DeptPrincipalName( int deptid)
        {
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            return give.GetDeptPrincipalName(masterinfo.agentid, deptid);
        }
        #endregion


        #region 获取action控制权限(用户view呈现操作按钮) GetcurrentAction()
        /// <summary>
        /// 获取action控制权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            if (Type > 0)
            {
                List<vw_action> list = Session["Action"] as List<vw_action>;
                if (list != null && list.Count > 0)
                {
                    #region 获取权限
                    foreach (vw_action item in list)
                    {
                        if (item.actionurl == "PaySingle")
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
                        }
                    }
                    #endregion
                }
            }
            return Json(action);
        }
        #endregion

        /// <summary>
        /// 获取代理商结算单详细
        /// </summary>
        /// <param name="os_no"></param>
        /// <returns></returns>
        public JsonResult GetBillDetailed(string os_no)
        {
            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                List<vw_agent> masterlist = base.SelectSearch<vw_agent>(t => t.mastername == bonuslist[0].mastername_t);
                List<com_master> operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);
                return Json(new { bonuslist = bonuslist, masterlist = masterlist, operatingmasterlist = operatingmasterlist, agentmaster = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername_t) });
            }
            return Json("");
        }

        /// <summary>
        /// 获取员工结算单详细
        /// </summary>
        /// <param name="os_no"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeBillDetailed(string os_no)
        {
            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                List<vw_user> masterlist = base.SelectSearch<vw_user>(t => t.mastername == bonuslist[0].mastername_t);
                List<com_master> operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);
                return Json(new { bonuslist = bonuslist, masterlist = masterlist, operatingmasterlist = operatingmasterlist });
            }
            return Json("");
        }

        /// <summary>
        /// 获取部门结算单详细
        /// </summary>
        /// <param name="os_no"></param>
        /// <returns></returns>
        public JsonResult GetDeptBillDetailed(string os_no)
        {
            List<order_setbonus_dept> bonuslist = base.OrderManage.SelectSearch<order_setbonus_dept>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                List<com_master> masterlist = base.SelectSearch<com_master>(t => (t.deptid == bonuslist[0].deptid && t.groupid == 4));
                List<com_master> operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);
                return Json(new { bonuslist = bonuslist, masterlist = masterlist, operatingmasterlist = operatingmasterlist });
            }
            return Json("");
        }

        #region 员工结算单展示 EmployeeBill_View(int index, int size, string starttime, string endtime, string employeename, int deptid)
        /// <summary> 
        /// 员工结算单展示 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="employeename"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmployeeBill_View(int index, int size, string starttime, string endtime, string employeename, int deptid)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_employeebill> pageParameter = new PageParameter<vw_employeebill>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;

            List<Expression<Func<vw_employeebill, bool>>> exprlist = new List<Expression<Func<vw_employeebill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(employeename))
                exprlist.Add(t => t.mastername_t == employeename);
            if (!string.IsNullOrEmpty(starttime))
            {
                exprlist.Add(t => t.createtime > Convert.ToDateTime(starttime));
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                exprlist.Add(t => t.createtime < Convert.ToDateTime(endtime));
            }

            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
            }
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.createtime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<vw_employeebill> employeemention = base.OrderManage.SelectPage<vw_employeebill>(pageParameter, out total);
            return Json(new { total = total, rows = employeemention });
        }
        #endregion

        public decimal? EmployeeBillTotal(string starttime, string endtime, string employeename, int deptid)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return 0;
            PageParameter<vw_employeebill> pageParameter = new PageParameter<vw_employeebill>();
            List<Expression<Func<vw_employeebill, bool>>> exprlist = new List<Expression<Func<vw_employeebill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(employeename))
                exprlist.Add(t => t.mastername_t == employeename);
            if (!string.IsNullOrEmpty(starttime))
            {
                exprlist.Add(t => t.createtime > Convert.ToDateTime(starttime));
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                exprlist.Add(t => t.createtime < Convert.ToDateTime(endtime));
            }
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
            }
            List<vw_employeebill> employeemention = base.OrderManage.SelectSearchs<vw_employeebill>(exprlist, pageParameter.Field, pageParameter.In);
            decimal? billtotal = 0;
            if (employeemention != null && employeemention.Count > 0)
            {
                for (int i = 0; i < employeemention.Count; i++)
                {
                    billtotal += (employeemention[i].total_bonus + employeemention[i].adjust_amount);
                }
            }
            return billtotal;
        }

        #region 员工结算单导出
        [HttpPost]
        public FileResult Export([System.Web.Http.FromBody]vw_employeebill bill)
        {
            UpdateAction();
            if (!action.Export)
                return null;
            List<Expression<Func<vw_employeebill, bool>>> exprlist = new List<Expression<Func<vw_employeebill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(bill.truename))
                exprlist.Add(t => t.createtime > Convert.ToDateTime(bill.truename));

            if (!string.IsNullOrEmpty(bill.deptname))
                exprlist.Add(t => t.createtime < Convert.ToDateTime(bill.deptname));

            string Field = "deptid";
            List<string> InIds = null;

            if (!string.IsNullOrEmpty(bill.mastername_t))
                exprlist.Add(t => t.mastername_t == bill.mastername_t);

            if (bill.deptid > 0)
            {
                Recursive give = new Recursive();
                InIds = give.GetDeptNodeId(bill.deptid, masterinfo.agentid);
                InIds.Add(bill.deptid.ToString());
            }
            List<vw_employeebill> list = base.OrderManage.SelectSearchs<vw_employeebill>(exprlist, Field, InIds, " createtime desc ");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("结算单编号");
            headerrow.CreateCell(2).SetCellValue("员工姓名");
            headerrow.CreateCell(3).SetCellValue("结算时间");
            headerrow.CreateCell(4).SetCellValue("结算销售额");
            headerrow.CreateCell(5).SetCellValue("结算订单数");
            headerrow.CreateCell(6).SetCellValue("结算提成金额数");
            headerrow.CreateCell(7).SetCellValue("部门");
            headerrow.CreateCell(8).SetCellValue("生成日期");
            headerrow.CreateCell(9).SetCellValue("操作人");
            EmployeeController employee = new EmployeeController();

            for (int i = 0; i < list.Count; i++)
            {
                vw_employeebill billentitle = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(billentitle.os_no);
                row.CreateCell(2).SetCellValue(billentitle.truename);
                row.CreateCell(3).SetCellValue(ConvertTime(billentitle.startdate, 0) + "/" + ConvertTime(billentitle.enddate, -1));
                row.CreateCell(4).SetCellValue(billentitle.total_amount.ToString());
                row.CreateCell(5).SetCellValue(billentitle.total_count.ToString());
                row.CreateCell(6).SetCellValue((billentitle.total_bonus + (billentitle.adjust_amount == null ? 0 : billentitle.adjust_amount)).ToString());
                row.CreateCell(7).SetCellValue(billentitle.deptname);
                row.CreateCell(8).SetCellValue(billentitle.createtime.ToString());
                row.CreateCell(9).SetCellValue(billentitle.addname);
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

        #endregion


        public string ConvertTime(int time, int days = 0)
        {
            if (time > 0)
            {
                return Core.Utility.PublicHelp.ConvertIntDateTime(time).AddDays(days).ToString("yyyy-MM-dd");
            }
            return "";
        }




        #region 代理商结算单展示 AgentBill_View(int index, int size, string starttime, string endtime, string employeename)
        /// <summary>
        /// 代理商结算单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="employeename"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult AgentBill_View(int index, int size, string starttime, string endtime, string employeename, int deptid)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_agentbill> pageParameter = new PageParameter<vw_agentbill>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;

            List<Expression<Func<vw_agentbill, bool>>> exprlist = new List<Expression<Func<vw_agentbill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(employeename))
                exprlist.Add(t => t.mastername_t == employeename);
            if (!string.IsNullOrEmpty(starttime))
                exprlist.Add(t => t.createtime > Convert.ToDateTime(starttime));
            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.createtime < Convert.ToDateTime(endtime));

            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                pageParameter.Field = "deptid";
                pageParameter.In = InIds;
            }

            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.createtime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<vw_agentbill> employeemention = base.OrderManage.SelectPage<vw_agentbill>(pageParameter, out total);
            return Json(new { total = total, rows = employeemention });
        }
        #endregion

        /// <summary>
        /// 代理商已结算提成总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="employeename"></param>
        /// <returns></returns>
        public decimal? AgentBillTotal(string starttime, string endtime, string employeename, int deptid)
        {
            List<Expression<Func<vw_agentbill, bool>>> exprlist = new List<Expression<Func<vw_agentbill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(employeename))
                exprlist.Add(t => t.mastername_t == employeename);
            if (!string.IsNullOrEmpty(starttime))
                exprlist.Add(t => t.createtime > Convert.ToDateTime(starttime));
            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.createtime < Convert.ToDateTime(endtime));
            List<string> InIds = new List<string>();
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());//添加本部门
            }
            List<vw_agentbill> employeemention = base.OrderManage.SelectSearchs<vw_agentbill>(exprlist, "deptid", InIds);
            decimal? billtotal = 0;
            if (employeemention != null && employeemention.Count > 0)
            {
                for (int i = 0; i < employeemention.Count; i++)
                {
                    billtotal += (employeemention[i].total_bonus + employeemention[i].adjust_amount);
                }
            }
            return billtotal;
        }

        #region 代理商结算单导出
        [HttpPost]
        public FileResult AgentExport([System.Web.Http.FromBody]vw_employeebill bill)
        {
            UpdateAction();
            if (!action.Export)
                return null;
            List<Expression<Func<vw_agentbill, bool>>> exprlist = new List<Expression<Func<vw_agentbill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(bill.truename))
                exprlist.Add(t => t.createtime > Convert.ToDateTime(bill.truename));

            if (!string.IsNullOrEmpty(bill.deptname))
                exprlist.Add(t => t.createtime < Convert.ToDateTime(bill.deptname));

            if (!string.IsNullOrEmpty(bill.mastername_t))
                exprlist.Add(t => t.mastername_t == bill.mastername_t);

            List<vw_agentbill> list = base.OrderManage.SelectSearchs<vw_agentbill>(exprlist, "", null, " createtime desc ");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("结算单编号");
            headerrow.CreateCell(2).SetCellValue("代理商");
            headerrow.CreateCell(3).SetCellValue("结算时间");
            headerrow.CreateCell(4).SetCellValue("结算销售额");
            headerrow.CreateCell(5).SetCellValue("结算订单数");
            headerrow.CreateCell(6).SetCellValue("结算提成金额数");
            headerrow.CreateCell(7).SetCellValue("负责人姓名");
            headerrow.CreateCell(8).SetCellValue("部门");
            headerrow.CreateCell(9).SetCellValue("渠道经理");
            headerrow.CreateCell(10).SetCellValue("生成日期");
            headerrow.CreateCell(11).SetCellValue("操作人");
            vw_agentbill employee = new vw_agentbill();

            for (int i = 0; i < list.Count; i++)
            {
                vw_agentbill billentitle = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(billentitle.os_no);
                row.CreateCell(2).SetCellValue(billentitle.agentname);
                row.CreateCell(3).SetCellValue(ConvertTime(billentitle.startdate) + "/" + ConvertTime(billentitle.enddate, -1));
                row.CreateCell(4).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(billentitle.total_amount));
                row.CreateCell(5).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(billentitle.total_count));
                row.CreateCell(6).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((billentitle.total_bonus + billentitle.adjust_amount)));
                row.CreateCell(7).SetCellValue(billentitle.truename);
                row.CreateCell(8).SetCellValue(billentitle.deptname);
                row.CreateCell(9).SetCellValue(billentitle.channelmanager);
                row.CreateCell(10).SetCellValue(billentitle.createtime.ToString());
                row.CreateCell(11).SetCellValue(billentitle.addname);

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

        #endregion



        /// <summary>
        /// 代理商订单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgentidOrdreInfo_View(int index, int size, string mastername, string os_no)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");

            PageParameter<vw_agentorderdetails> pageParameter = new PageParameter<vw_agentorderdetails>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<string> agentlist = new List<string>();

            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                pageParameter.Wheres = GetAgentSelectWhere(mastername, bonuslist[0].startdate, bonuslist[0].enddate);
                pageParameter.OrderColumns = t1 => t1.o_datetime;
                pageParameter.IsOrderByASC = 0;
                int total = 0;
                IList<vw_agentorderdetails> employeeorder = base.OrderManage.SelectPage<vw_agentorderdetails>(pageParameter, out total);
                return Json(new { total = total, rows = employeeorder });
            }
            return Json("");
        }
        /// <summary>
        /// /获取代理商的查询 条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<Expression<Func<vw_agentorderdetails, bool>>> GetAgentSelectWhere(string mastername, int starttime, int endtime)
        {
            List<Expression<Func<vw_agentorderdetails, bool>>> exprlist = new List<Expression<Func<vw_agentorderdetails, bool>>>();
            if (starttime > 0)
            {
                DateTime stime = Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            DateTime etime = Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            exprlist.Add(t => t.o_datetime < etime);
            CommissionPayController pay = new CommissionPayController();
            List<com_master> list = base.SelectAppointField<com_master>(t => t.mastername == mastername, "agentid");
            exprlist.Add(t => t.agentid == list[0].agentid);
            return exprlist;
        }


        #region 代理商结算详细展示
        /// <summary>
        /// 代理商结算详细展示
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgentKontInformationDisplay(string mastername, string os_no)
        {
            CommissionPayController pay = new CommissionPayController();
            List<com_master> agentidlist = base.SelectSearch<com_master>(t => t.mastername == mastername);
            if (agentidlist != null && agentidlist.Count > 0)
            {

                List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
                if (bonuslist != null && bonuslist.Count > 0)
                {
                    OrderManage manage = new OrderManage();
                    List<orderkont> Listorderkont = manage.GetAgent(agentidlist[0].agentid, bonuslist[0].startdate, bonuslist[0].enddate);
                    Listorderkont = AddTotal(Listorderkont, bonuslist[0].adjust_amount);
                    if (Listorderkont != null)
                        return Json(new { total = Listorderkont.Count, rows = Listorderkont });
                }
            }
            return Json("");
        }
        #endregion

        /// <summary>
        /// 员工订单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmployeeOrdreInfo_View(int index, int size, string os_no)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<orderinfo> pageParameter = new PageParameter<orderinfo>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();

            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                exprlist.Add(t => ((t.m_mastername == bonuslist[0].mastername_t) && (t.o_totype == 0 || t.o_totype == 1)));

                exprlist.Add(t => t.o_bonus > 0);//只显示提成金额大于0的

                DateTime starttime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].startdate);
                exprlist.Add(t => t.o_datetime >= starttime);

                DateTime endtime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].enddate);
                exprlist.Add(t => t.o_datetime < endtime);

                pageParameter.Wheres = exprlist;
                pageParameter.OrderColumns = t1 => t1.o_datetime;
                pageParameter.IsOrderByASC = 0;
                int total = 0;
                IList<orderinfo> employeeorder = base.OrderManage.SelectPage<orderinfo>(pageParameter, out total);
                return Json(new { total = total, rows = employeeorder });
            }
            return Json("");
        }

        #region 结算员工结算详细信息展示
        /// <summary>
        /// 结算信息展示 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult KontInformationDisplay(string os_no)
        {
            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                OrderManage manage = new OrderManage();
                List<orderkont> Listorderkont = manage.Getemployee(bonuslist[0].mastername_t, bonuslist[0].startdate, bonuslist[0].enddate);
                Listorderkont = AddTotal(Listorderkont, bonuslist[0].adjust_amount);
                if (Listorderkont != null)
                    return Json(new { total = Listorderkont.Count, rows = Listorderkont });
            }
            return Json("");
        }
        #endregion

        #region 部门

        #region 部门结算单展示 DeptBill_View(int index, int size, string starttime, string endtime, string employeename, int deptid)
        /// <summary>
        /// 部门结算记录展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="principalname">部门负责人</param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeptBill_View(int index, int size, string starttime, string endtime, string principalname, int deptid)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<vw_deptbill> pageParameter = new PageParameter<vw_deptbill>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;

            List<Expression<Func<vw_deptbill, bool>>> exprlist = new List<Expression<Func<vw_deptbill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(principalname))
                exprlist.Add(t => t.principalname == principalname);

            if (!string.IsNullOrEmpty(starttime))
            {
                DateTime stime = Convert.ToDateTime(starttime);
                exprlist.Add(t => t.createtime > stime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                DateTime etime = Convert.ToDateTime(endtime);
                exprlist.Add(t => t.createtime < etime);
            }
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
            }
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.createtime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<vw_deptbill> employeemention = base.OrderManage.SelectPage<vw_deptbill>(pageParameter, out total);
            return Json(new { total = total, rows = employeemention });
        }
        #endregion






        /// <summary>
        /// 部门已结算提成总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="employeename"></param>
        /// <returns></returns>
        public decimal? DeptBillTotal(string starttime, string endtime, string principalname, int deptid)
        {
            PageParameter<vw_deptbill> pageParameter = new PageParameter<vw_deptbill>();
            List<Expression<Func<vw_deptbill, bool>>> exprlist = new List<Expression<Func<vw_deptbill, bool>>>();

            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(principalname))
                exprlist.Add(t => t.principalname == principalname);

            if (!string.IsNullOrEmpty(starttime))
            {
                DateTime stime = Convert.ToDateTime(starttime);
                exprlist.Add(t => t.createtime > stime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                DateTime etime = Convert.ToDateTime(endtime);
                exprlist.Add(t => t.createtime < etime);
            }

            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "deptid";
                    pageParameter.In = InIds;
                }
            }
            List<vw_deptbill> employeemention = base.OrderManage.SelectSearchs<vw_deptbill>(exprlist, pageParameter.Field, pageParameter.In);
            decimal? billtotal = 0;
            if (employeemention != null && employeemention.Count > 0)
            {
                for (int i = 0; i < employeemention.Count; i++)
                {
                    billtotal += (employeemention[i].total_bonus + (employeemention[i].adjust_amount == null ? 0 : employeemention[i].adjust_amount));
                }
            }
            return billtotal;
        }


        #region 部门结算单导出
        [HttpPost]
        public FileResult DeptExportBill([System.Web.Http.FromBody]vw_employeebill bill)
        {
            UpdateAction();
            if (!action.Export)
                return null;
            List<Expression<Func<vw_deptbill, bool>>> exprlist = new List<Expression<Func<vw_deptbill, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(bill.truename))
                exprlist.Add(t => t.createtime > Convert.ToDateTime(bill.truename));

            if (!string.IsNullOrEmpty(bill.deptname))
                exprlist.Add(t => t.createtime < Convert.ToDateTime(bill.deptname));

            string Field = "deptid";
            List<string> InIds = null;

            if (!string.IsNullOrEmpty(bill.mastername_t))
                exprlist.Add(t => t.principalname == bill.mastername_t);

            if (bill.deptid > 0)
            {
                Recursive give = new Recursive();
                InIds = give.GetDeptNodeId(bill.deptid, masterinfo.agentid);
                InIds.Add(bill.deptid.ToString());
            }
            List<vw_deptbill> list = base.OrderManage.SelectSearchs<vw_deptbill>(exprlist, Field, InIds, " createtime desc");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("结算单编号");
            headerrow.CreateCell(2).SetCellValue("部门");
            headerrow.CreateCell(3).SetCellValue("结算时间");
            headerrow.CreateCell(4).SetCellValue("结算销售额");
            headerrow.CreateCell(5).SetCellValue("结算订单数");
            headerrow.CreateCell(6).SetCellValue("结算提成金额");
            headerrow.CreateCell(7).SetCellValue("负责人姓名");
            headerrow.CreateCell(8).SetCellValue("生成日期");
            headerrow.CreateCell(9).SetCellValue("操作人");
            EmployeeController employee = new EmployeeController();

            for (int i = 0; i < list.Count; i++)
            {
                vw_deptbill billentitle = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(billentitle.os_no);
                row.CreateCell(2).SetCellValue(billentitle.deptname);
                row.CreateCell(3).SetCellValue(ConvertTime(billentitle.startdate) + "/" + ConvertTime(billentitle.enddate, -1));
                row.CreateCell(4).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(billentitle.total_amount));
                row.CreateCell(5).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(billentitle.total_count));
                row.CreateCell(6).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((billentitle.total_bonus + (billentitle.adjust_amount == null ? 0 : billentitle.adjust_amount))));
                row.CreateCell(7).SetCellValue(billentitle.principalname);
                row.CreateCell(8).SetCellValue(billentitle.createtime.ToString());
                row.CreateCell(9).SetCellValue(billentitle.addname);
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

        #endregion

        #region 部门结算详细信息展示
        /// <summary>
        /// 结算信息展示 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeptKontInformationDisplay(string os_no)
        {
            List<order_setbonus_dept> bonuslist = base.OrderManage.SelectSearch<order_setbonus_dept>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                int deptid = bonuslist[0].deptid;
                OrderManage manage = new OrderManage();
                List<orderkont> Listorderkont = manage.Getdept(deptid, bonuslist[0].startdate, bonuslist[0].enddate);
                Listorderkont = AddTotal(Listorderkont, 0);
                if (Listorderkont != null)
                    return Json(new { total = Listorderkont.Count, rows = Listorderkont });
            }
            return Json("");
        }
        #endregion

        /// <summary>
        /// 部门订单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DepteOrdreInfo_View(int index, int size, string os_no)
        {
            UpdateAction();
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<orderinfo> pageParameter = new PageParameter<orderinfo>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();

            List<order_setbonus_dept> bonuslist = base.OrderManage.SelectSearch<order_setbonus_dept>(t => t.os_no == os_no);
            if (bonuslist != null && bonuslist.Count > 0)
            {
                exprlist.Add(t => (t.m_deptid == bonuslist[0].deptid && t.o_totype == 2));


                DateTime starttime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].startdate);
                exprlist.Add(t => t.o_datetime >= starttime);

                DateTime endtime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].enddate);
                exprlist.Add(t => t.o_datetime < endtime);

                pageParameter.Wheres = exprlist;
                pageParameter.OrderColumns = t1 => t1.o_datetime;
                pageParameter.IsOrderByASC = 0;
                int total = 0;
                IList<orderinfo> employeeorder = base.OrderManage.SelectPage<orderinfo>(pageParameter, out total);
                return Json(new { total = total, rows = employeeorder });
            }
            return Json("");
        }

        #endregion

        #region 代理商导出
        /// <summary>
        /// 代理商导出 
        /// </summary>
        /// <param name="masterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult Agent_Export([System.Web.Http.FromBody]order_setbonus orderinfo)
        {
            string mastername = orderinfo.mastername;
            string os_no = orderinfo.os_no;
            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            List<vw_agent> commaster = base.SelectSearch<vw_agent>(t => t.mastername == bonuslist[0].mastername_t);
            List<com_master> agentmaster = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername_t);
            List<com_master> operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);

            List<orderkont> Listorderkont = new List<orderkont>();
            if (operatingmasterlist != null && operatingmasterlist.Count > 0)
            {
                OrderManage manage = new OrderManage();
                Listorderkont = manage.GetAgent(agentmaster[0].agentid, bonuslist[0].startdate, bonuslist[0].enddate);
                Listorderkont = AddTotal(Listorderkont, bonuslist[0].adjust_amount);
            }

            List<vw_agentorderdetails> orderlist = new List<vw_agentorderdetails>();
            if (bonuslist != null && bonuslist.Count > 0)
            {
                orderlist = base.OrderManage.SelectSearchs<vw_agentorderdetails>(GetAgentSelectWhere(mastername, bonuslist[0].startdate, bonuslist[0].enddate), "", null, " o_datetime desc ");
            }
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string Bill = "结算单" + DateTime.Now.ToString("yyyy-MM-dd");
            string Commission = "提成" + DateTime.Now.ToString("yyyy-MM-dd");
            string OrderInfo = "订单" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheetBill = book.CreateSheet(Bill);
            ISheet sheetCommission = book.CreateSheet(Commission);
            ISheet sheetOrderInfo = book.CreateSheet(OrderInfo);
            IRow headerrowbill = sheetBill.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle stylebill = book.CreateCellStyle();//创建表格样式
            stylebill.Alignment = HorizontalAlignment.Center;//水平对齐方式
            stylebill.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrowbill.CreateCell(0).SetCellValue("序号");
            headerrowbill.CreateCell(1).SetCellValue("结算单编号");
            headerrowbill.CreateCell(2).SetCellValue("结算日期");
            headerrowbill.CreateCell(3).SetCellValue("结算金额");
            headerrowbill.CreateCell(4).SetCellValue("代理商");
            headerrowbill.CreateCell(5).SetCellValue("电话");
            headerrowbill.CreateCell(6).SetCellValue("传真");
            headerrowbill.CreateCell(7).SetCellValue("部门");
            headerrowbill.CreateCell(8).SetCellValue("渠道经理");
            headerrowbill.CreateCell(9).SetCellValue("联系人");
            headerrowbill.CreateCell(10).SetCellValue("手机号");
            headerrowbill.CreateCell(11).SetCellValue("地址");
            headerrowbill.CreateCell(12).SetCellValue("调整金额");
            headerrowbill.CreateCell(13).SetCellValue("调整原因");
            headerrowbill.CreateCell(14).SetCellValue("生成日期");
            headerrowbill.CreateCell(15).SetCellValue("操作人");
            IRow row = sheetBill.CreateRow(1);
            ICell cell = row.CreateCell(1);
            cell.CellStyle = stylebill;
            row.CreateCell(0).SetCellValue(1);
            row.CreateCell(1).SetCellValue(bonuslist[0].os_no);
            if (bonuslist[0].startdate > 0)
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].startdate) + "--" + ConvertTime(bonuslist[0].enddate, -1));
            else
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].enddate, -1));
            row.CreateCell(3).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((bonuslist[0].total_bonus + bonuslist[0].adjust_amount)));
            row.CreateCell(4).SetCellValue(commaster[0].agentname);
            row.CreateCell(5).SetCellValue(agentmaster[0].agent_tel);
            row.CreateCell(6).SetCellValue(agentmaster[0].agent_fax);
            row.CreateCell(7).SetCellValue(commaster[0].deptname);
            row.CreateCell(8).SetCellValue(commaster[0].parentname);
            row.CreateCell(9).SetCellValue(commaster[0].truename);
            row.CreateCell(10).SetCellValue(agentmaster[0].mobile);
            row.CreateCell(11).SetCellValue(agentmaster[0].agent_addr);
            row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(bonuslist[0].adjust_amount));
            row.CreateCell(13).SetCellValue(bonuslist[0].adjust_reason);
            row.CreateCell(14).SetCellValue(bonuslist[0].createtime.ToString("yyyy-MM-dd"));
            row.CreateCell(15).SetCellValue(operatingmasterlist[0].truename);


            IRow headerrowCommission = sheetCommission.CreateRow(0);//创建一行，此行为第一行           
            headerrowCommission.CreateCell(0).SetCellValue("序号");
            headerrowCommission.CreateCell(1).SetCellValue("产品名称");
            headerrowCommission.CreateCell(2).SetCellValue("类别");
            headerrowCommission.CreateCell(3).SetCellValue("版本");
            headerrowCommission.CreateCell(4).SetCellValue("销售折扣");
            headerrowCommission.CreateCell(5).SetCellValue("班级奖励");
            headerrowCommission.CreateCell(6).SetCellValue("总订单数");
            headerrowCommission.CreateCell(7).SetCellValue("总销售金额");
            headerrowCommission.CreateCell(8).SetCellValue("总销售毛利");
            headerrowCommission.CreateCell(9).SetCellValue("绑定班级订单数");
            headerrowCommission.CreateCell(10).SetCellValue("绑定班级销售额");
            headerrowCommission.CreateCell(11).SetCellValue("绑定班级销售毛利");
            headerrowCommission.CreateCell(12).SetCellValue("销售折扣金额");
            headerrowCommission.CreateCell(13).SetCellValue("班级奖励金额");
            headerrowCommission.CreateCell(14).SetCellValue("合计");
            for (int i = 0; i < Listorderkont.Count; i++)
            {
                orderkont userinfo = Listorderkont[i];
                IRow rowwCommission = sheetCommission.CreateRow(i + 1);
                ICell cellwCommission = rowwCommission.CreateCell(i);
                cellwCommission.CellStyle = stylebill;
                rowwCommission.CreateCell(0).SetCellValue(i + 1);
                rowwCommission.CreateCell(1).SetCellValue(userinfo.productname);
                rowwCommission.CreateCell(2).SetCellValue(userinfo.p_category);
                rowwCommission.CreateCell(3).SetCellValue(userinfo.p_version);
                rowwCommission.CreateCell(4).SetCellValue(Core.Utility.PublicHelp.Percentage(userinfo.divided));
                rowwCommission.CreateCell(5).SetCellValue(Core.Utility.PublicHelp.Percentage(userinfo.class_divided));
                rowwCommission.CreateCell(6).SetCellValue(userinfo.ordernumber);
                rowwCommission.CreateCell(7).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_payamount));
                rowwCommission.CreateCell(8).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_actamount));
                rowwCommission.CreateCell(9).SetCellValue(userinfo.classnumber);
                rowwCommission.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.classpayamount));
                rowwCommission.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.classactamount));
                rowwCommission.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.basis_bonus));
                rowwCommission.CreateCell(13).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.p_class_bonus));
                if (userinfo.productname == "总计：" || userinfo.productname == "调整金额")
                {
                    rowwCommission.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.total));
                }
                else
                {
                    rowwCommission.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((userinfo.basis_bonus + userinfo.p_class_bonus)));
                }
            }


            IRow headerrowOrderInfo = sheetOrderInfo.CreateRow(0);//创建一行，此行为第一行           
            headerrowOrderInfo.CreateCell(0).SetCellValue("序号");
            headerrowOrderInfo.CreateCell(1).SetCellValue("订单编号");
            headerrowOrderInfo.CreateCell(2).SetCellValue("订单日期");
            headerrowOrderInfo.CreateCell(3).SetCellValue("所属地区");
            headerrowOrderInfo.CreateCell(4).SetCellValue("学校、班级");
            headerrowOrderInfo.CreateCell(5).SetCellValue("年级");
            headerrowOrderInfo.CreateCell(6).SetCellValue("班级");
            headerrowOrderInfo.CreateCell(7).SetCellValue("老师姓名");
            headerrowOrderInfo.CreateCell(8).SetCellValue("产品名称");
            headerrowOrderInfo.CreateCell(9).SetCellValue("支付金额");
            headerrowOrderInfo.CreateCell(10).SetCellValue("手续费");
            headerrowOrderInfo.CreateCell(11).SetCellValue("实际到账");
            headerrowOrderInfo.CreateCell(12).SetCellValue("提成金额");

            for (int i = 0; i < orderlist.Count; i++)
            {
                vw_agentorderdetails oinfo = orderlist[i];
                IRow rowOrderInfo = sheetOrderInfo.CreateRow(i + 1);
                ICell cellOrderInfo = rowOrderInfo.CreateCell(i);
                cellOrderInfo.CellStyle = stylebill;
                rowOrderInfo.CreateCell(0).SetCellValue(i + 1);
                rowOrderInfo.CreateCell(1).SetCellValue(oinfo.o_id);
                rowOrderInfo.CreateCell(2).SetCellValue(oinfo.o_datetime.ToString());
                rowOrderInfo.CreateCell(3).SetCellValue(oinfo.path.ToString());
                rowOrderInfo.CreateCell(4).SetCellValue(oinfo.schoolname.ToString());
                rowOrderInfo.CreateCell(5).SetCellValue(oinfo.gradename.ToString());
                rowOrderInfo.CreateCell(6).SetCellValue(oinfo.classname.ToString());
                rowOrderInfo.CreateCell(7).SetCellValue(oinfo.u_teachername.ToString());
                rowOrderInfo.CreateCell(8).SetCellValue(Core.Utility.PublicHelp.ProductName(oinfo.channel));
                rowOrderInfo.CreateCell(9).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_payamount));
                rowOrderInfo.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_feeamount));
                rowOrderInfo.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_actamount));
                rowOrderInfo.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_bonus));
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                Bill = HttpUtility.UrlEncode(Bill, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                Bill = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Bill)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", Bill + ".xls");
        }

        #endregion

        #region 员工导出
        /// <summary>
        /// 员工导出 
        /// </summary>
        /// <param name="masterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult Employee_Export([System.Web.Http.FromBody]order_setbonus orderinfo)
        {
            string mastername = orderinfo.mastername;
            string os_no = orderinfo.os_no;
            List<order_setbonus> bonuslist = base.OrderManage.SelectSearch<order_setbonus>(t => t.os_no == os_no);
            List<vw_user> masterlist = base.SelectSearch<vw_user>(t => t.mastername == bonuslist[0].mastername_t);
            List<com_master> operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);
            CommissionPayController pay = new CommissionPayController();
            List<string> agentidlist = pay.GetAllAubordinateAgentId(mastername);
            List<orderkont> Listorderkont = new List<orderkont>();
            if (agentidlist != null && agentidlist.Count > 0)
            {
                string agentids = "";
                foreach (string rowkont in agentidlist)
                {
                    agentids += "'" + rowkont + "',";
                }
                OrderManage manage = new OrderManage();
                Listorderkont = manage.Getemployee(bonuslist[0].mastername_t, bonuslist[0].startdate, bonuslist[0].enddate);
                Listorderkont = AddTotal(Listorderkont, bonuslist[0].adjust_amount);
            }

            List<string> agentlist = new List<string>();
            List<orderinfo> orderlist = new List<orderinfo>();
            if (bonuslist != null && bonuslist.Count > 0)
            {
                List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
                exprlist.Add(t => ((t.m_mastername == bonuslist[0].mastername_t) && (t.o_totype == 0 || t.o_totype == 1)));

                exprlist.Add(t => t.o_bonus > 0);//只显示提成金额大于0的

                DateTime starttime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].startdate);
                exprlist.Add(t => t.o_datetime >= starttime);

                DateTime endtime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].enddate);
                exprlist.Add(t => t.o_datetime < endtime);
                orderlist = base.OrderManage.SelectSearchs<orderinfo>(exprlist, "agentid", agentlist, " o_datetime desc ");
            }
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string Bill = "结算单" + DateTime.Now.ToString("yyyy-MM-dd");
            string Commission = "提成" + DateTime.Now.ToString("yyyy-MM-dd");
            string OrderInfo = "订单" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheetBill = book.CreateSheet(Bill);
            ISheet sheetCommission = book.CreateSheet(Commission);
            ISheet sheetOrderInfo = book.CreateSheet(OrderInfo);
            IRow headerrowbill = sheetBill.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle stylebill = book.CreateCellStyle();//创建表格样式
            stylebill.Alignment = HorizontalAlignment.Center;//水平对齐方式
            stylebill.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrowbill.CreateCell(0).SetCellValue("序号");
            headerrowbill.CreateCell(1).SetCellValue("结算单编号");
            headerrowbill.CreateCell(2).SetCellValue("结算日期");
            headerrowbill.CreateCell(3).SetCellValue("结算提成金额");
            headerrowbill.CreateCell(4).SetCellValue("员工姓名");
            headerrowbill.CreateCell(5).SetCellValue("部门");
            headerrowbill.CreateCell(6).SetCellValue("调整金额");
            headerrowbill.CreateCell(7).SetCellValue("调整原因");
            headerrowbill.CreateCell(8).SetCellValue("生成日期");
            headerrowbill.CreateCell(9).SetCellValue("操作人");
            IRow row = sheetBill.CreateRow(1);
            ICell cell = row.CreateCell(1);
            cell.CellStyle = stylebill;
            row.CreateCell(0).SetCellValue(1);
            row.CreateCell(1).SetCellValue(bonuslist[0].os_no);
            if (bonuslist[0].startdate > 0)
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].startdate) + "--" + ConvertTime(bonuslist[0].enddate, -1));
            else
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].enddate, -1));
            row.CreateCell(3).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((bonuslist[0].total_bonus + bonuslist[0].adjust_amount)));
            row.CreateCell(4).SetCellValue(masterlist[0].truename);
            row.CreateCell(5).SetCellValue(masterlist[0].deptname);
            row.CreateCell(6).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(bonuslist[0].adjust_amount));
            row.CreateCell(7).SetCellValue(bonuslist[0].adjust_reason);
            row.CreateCell(8).SetCellValue(bonuslist[0].createtime.ToString("yyyy-MM-dd"));
            row.CreateCell(9).SetCellValue(operatingmasterlist[0].truename);
            IRow headerrowCommission = sheetCommission.CreateRow(0);//创建一行，此行为第一行           
            headerrowCommission.CreateCell(0).SetCellValue("序号");
            headerrowCommission.CreateCell(1).SetCellValue("产品名称");
            headerrowCommission.CreateCell(2).SetCellValue("类别");
            headerrowCommission.CreateCell(3).SetCellValue("版本");
            headerrowCommission.CreateCell(4).SetCellValue("销售折扣");
            headerrowCommission.CreateCell(5).SetCellValue("班级奖励");
            headerrowCommission.CreateCell(6).SetCellValue("总订单数");
            headerrowCommission.CreateCell(7).SetCellValue("总销售金额");
            headerrowCommission.CreateCell(8).SetCellValue("总销售毛利");
            headerrowCommission.CreateCell(9).SetCellValue("绑定班级订单数");
            headerrowCommission.CreateCell(10).SetCellValue("绑定班级销售额");
            headerrowCommission.CreateCell(11).SetCellValue("绑定班级销售毛利");
            headerrowCommission.CreateCell(12).SetCellValue("基础提成金额");
            headerrowCommission.CreateCell(13).SetCellValue("班级奖励金额");
            headerrowCommission.CreateCell(14).SetCellValue("合计");
            for (int i = 0; i < Listorderkont.Count; i++)
            {
                orderkont userinfo = Listorderkont[i];
                IRow rowwCommission = sheetCommission.CreateRow(i + 1);
                ICell cellwCommission = rowwCommission.CreateCell(i);
                cellwCommission.CellStyle = stylebill;
                rowwCommission.CreateCell(0).SetCellValue(i + 1);
                rowwCommission.CreateCell(1).SetCellValue(userinfo.productname);
                rowwCommission.CreateCell(2).SetCellValue(userinfo.p_category);
                rowwCommission.CreateCell(3).SetCellValue(userinfo.p_version);
                rowwCommission.CreateCell(4).SetCellValue(Core.Utility.PublicHelp.Percentage(userinfo.divided));
                rowwCommission.CreateCell(5).SetCellValue(Core.Utility.PublicHelp.Percentage(userinfo.class_divided));
                rowwCommission.CreateCell(6).SetCellValue(userinfo.ordernumber);
                rowwCommission.CreateCell(7).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_payamount));
                rowwCommission.CreateCell(8).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_actamount));
                rowwCommission.CreateCell(9).SetCellValue(userinfo.classnumber);
                rowwCommission.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.classpayamount));
                rowwCommission.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.classactamount));
                if (userinfo.productname == "调整金额")
                    rowwCommission.CreateCell(12).SetCellValue("");
                else
                    rowwCommission.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.basis_bonus));
                rowwCommission.CreateCell(13).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.p_class_bonus));

                if (userinfo.productname == "总计：" || userinfo.productname == "调整金额")
                {
                    rowwCommission.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.total));
                }
                else
                {
                    rowwCommission.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces((userinfo.basis_bonus + userinfo.p_class_bonus)));
                }
                //rowwCommission.CreateCell(14).SetCellValue((userinfo.basis_bonus + userinfo.p_class_bonus).ToString());
            }
            IRow headerrowOrderInfo = sheetOrderInfo.CreateRow(0);//创建一行，此行为第一行           
            headerrowOrderInfo.CreateCell(0).SetCellValue("序号");
            headerrowOrderInfo.CreateCell(1).SetCellValue("订单编号");
            headerrowOrderInfo.CreateCell(2).SetCellValue("订单日期");
            headerrowOrderInfo.CreateCell(3).SetCellValue("所属地区");
            headerrowOrderInfo.CreateCell(4).SetCellValue("学校");
            headerrowOrderInfo.CreateCell(5).SetCellValue("年级");
            headerrowOrderInfo.CreateCell(6).SetCellValue("班级");
            headerrowOrderInfo.CreateCell(7).SetCellValue("老师姓名");
            headerrowOrderInfo.CreateCell(8).SetCellValue("部门");
            headerrowOrderInfo.CreateCell(9).SetCellValue("员工");
            headerrowOrderInfo.CreateCell(10).SetCellValue("产品名称");
            headerrowOrderInfo.CreateCell(11).SetCellValue("支付金额");
            headerrowOrderInfo.CreateCell(12).SetCellValue("手续费");
            headerrowOrderInfo.CreateCell(13).SetCellValue("实际到账");
            headerrowOrderInfo.CreateCell(14).SetCellValue("提成金额");
            for (int i = 0; i < orderlist.Count; i++)
            {
                orderinfo oinfo = orderlist[i];
                IRow rowOrderInfo = sheetOrderInfo.CreateRow(i + 1);
                ICell cellOrderInfo = rowOrderInfo.CreateCell(i);
                cellOrderInfo.CellStyle = stylebill;
                rowOrderInfo.CreateCell(0).SetCellValue(i + 1);
                rowOrderInfo.CreateCell(1).SetCellValue(oinfo.o_id);
                rowOrderInfo.CreateCell(2).SetCellValue(oinfo.o_datetime.ToString());
                rowOrderInfo.CreateCell(3).SetCellValue(oinfo.path);
                rowOrderInfo.CreateCell(4).SetCellValue(oinfo.schoolname);
                rowOrderInfo.CreateCell(5).SetCellValue(oinfo.gradename);
                rowOrderInfo.CreateCell(6).SetCellValue(oinfo.classname);
                rowOrderInfo.CreateCell(7).SetCellValue(oinfo.u_teachername);
                rowOrderInfo.CreateCell(8).SetCellValue(oinfo.m_deptname);
                rowOrderInfo.CreateCell(9).SetCellValue(oinfo.m_a_name);
                rowOrderInfo.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.ProductName(oinfo.channel));
                rowOrderInfo.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_payamount));
                rowOrderInfo.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_feeamount));
                rowOrderInfo.CreateCell(13).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_actamount));
                rowOrderInfo.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_bonus));
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                Bill = HttpUtility.UrlEncode(Bill, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                Bill = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Bill)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", Bill + ".xls");
        }

        #endregion

        #region 部门结算导出
        /// <summary>
        /// 部门结算导出 
        /// </summary>
        /// <param name="masterInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult Dept_Export([System.Web.Http.FromBody]order_setbonus_dept orderinfo)
        {
            string os_no = orderinfo.os_no;
            List<order_setbonus_dept> bonuslist = base.OrderManage.SelectSearch<order_setbonus_dept>(t => t.os_no == os_no);
            List<orderkont> Listorderkont = new List<orderkont>();
            List<com_master> masterlist = new List<com_master>();
            List<com_master> operatingmasterlist = new List<com_master>();
            if (bonuslist != null && bonuslist.Count > 0)
            {
                masterlist = base.SelectSearch<com_master>(t => (t.deptid == bonuslist[0].deptid && t.groupid == 4));
                operatingmasterlist = base.SelectSearch<com_master>(t => t.mastername == bonuslist[0].mastername);
                int deptid = bonuslist[0].deptid;
                OrderManage manage = new OrderManage();
                Listorderkont = manage.Getdept(deptid, bonuslist[0].startdate, bonuslist[0].enddate);
                Listorderkont = AddTotal(Listorderkont, 0);
            }
            List<string> agentlist = new List<string>();
            List<orderinfo> orderlist = new List<orderinfo>();
            if (bonuslist != null && bonuslist.Count > 0)
            {
                List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
                exprlist.Add(t => (t.m_deptid == bonuslist[0].deptid && t.o_totype == 2));
                DateTime starttime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].startdate);
                exprlist.Add(t => t.o_datetime >= starttime);
                DateTime endtime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(bonuslist[0].enddate);
                exprlist.Add(t => t.o_datetime < endtime);
                orderlist = base.OrderManage.SelectSearchs<orderinfo>(exprlist, "", null, " o_datetime desc ");
            }
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string Bill = "结算单" + DateTime.Now.ToString("yyyy-MM-dd");
            string Commission = "提成" + DateTime.Now.ToString("yyyy-MM-dd");
            string OrderInfo = "订单" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheetBill = book.CreateSheet(Bill);
            ISheet sheetCommission = book.CreateSheet(Commission);
            ISheet sheetOrderInfo = book.CreateSheet(OrderInfo);
            IRow headerrowbill = sheetBill.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle stylebill = book.CreateCellStyle();//创建表格样式
            stylebill.Alignment = HorizontalAlignment.Center;//水平对齐方式
            stylebill.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrowbill.CreateCell(0).SetCellValue("序号");
            headerrowbill.CreateCell(1).SetCellValue("结算单编号");
            headerrowbill.CreateCell(2).SetCellValue("结算日期");
            headerrowbill.CreateCell(3).SetCellValue("结算提成金额");
            headerrowbill.CreateCell(4).SetCellValue("部门负责人");
            headerrowbill.CreateCell(5).SetCellValue("所属部门");
            headerrowbill.CreateCell(6).SetCellValue("团队提成比例");
            headerrowbill.CreateCell(7).SetCellValue("调整金额");
            headerrowbill.CreateCell(8).SetCellValue("调整原因");
            headerrowbill.CreateCell(9).SetCellValue("生成日期");
            headerrowbill.CreateCell(10).SetCellValue("操作人");
            IRow row = sheetBill.CreateRow(1);
            ICell cell = row.CreateCell(1);
            cell.CellStyle = stylebill;
            row.CreateCell(0).SetCellValue(1);
            row.CreateCell(1).SetCellValue(bonuslist[0].os_no);
            if (bonuslist[0].startdate > 0)
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].startdate) + "--" + ConvertTime(bonuslist[0].enddate, -1));
            else
                row.CreateCell(2).SetCellValue(ConvertTime(bonuslist[0].enddate, -1));
            row.CreateCell(3).SetCellValue((bonuslist[0].total_bonus + bonuslist[0].adjust_amount).ToString());
            row.CreateCell(4).SetCellValue(masterlist[0].truename);
            row.CreateCell(5).SetCellValue(bonuslist[0].deptname);
            row.CreateCell(6).SetCellValue((bonuslist[0].team_bonus_r).ToString() + "%");
            row.CreateCell(7).SetCellValue(bonuslist[0].adjust_amount.ToString());
            row.CreateCell(8).SetCellValue(bonuslist[0].adjust_reason);
            row.CreateCell(9).SetCellValue(bonuslist[0].createtime.ToString("yyyy-MM-dd"));
            row.CreateCell(10).SetCellValue(operatingmasterlist[0].truename);
            IRow headerrowCommission = sheetCommission.CreateRow(0);//创建一行，此行为第一行           
            headerrowCommission.CreateCell(0).SetCellValue("序号");
            headerrowCommission.CreateCell(1).SetCellValue("产品名称");
            headerrowCommission.CreateCell(2).SetCellValue("类别");
            headerrowCommission.CreateCell(3).SetCellValue("版本");
            headerrowCommission.CreateCell(4).SetCellValue("总订单数");
            headerrowCommission.CreateCell(5).SetCellValue("总销售金额");
            headerrowCommission.CreateCell(6).SetCellValue("总销售毛利");

            for (int i = 0; i < Listorderkont.Count; i++)
            {
                orderkont userinfo = Listorderkont[i];
                IRow rowwCommission = sheetCommission.CreateRow(i + 1);
                ICell cellwCommission = rowwCommission.CreateCell(i);
                cellwCommission.CellStyle = stylebill;
                rowwCommission.CreateCell(0).SetCellValue(i + 1);
                rowwCommission.CreateCell(1).SetCellValue(userinfo.productname);
                rowwCommission.CreateCell(2).SetCellValue(userinfo.p_category);
                rowwCommission.CreateCell(3).SetCellValue(userinfo.p_version);
                rowwCommission.CreateCell(4).SetCellValue(userinfo.ordernumber);
                rowwCommission.CreateCell(5).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_payamount));
                rowwCommission.CreateCell(6).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(userinfo.o_actamount));
            }

            IRow headerrowOrderInfo = sheetOrderInfo.CreateRow(0);//创建一行，此行为第一行           
            headerrowOrderInfo.CreateCell(0).SetCellValue("序号");
            headerrowOrderInfo.CreateCell(1).SetCellValue("订单编号");
            headerrowOrderInfo.CreateCell(2).SetCellValue("订单日期");
            headerrowOrderInfo.CreateCell(3).SetCellValue("所属地区");
            headerrowOrderInfo.CreateCell(4).SetCellValue("学校");
            headerrowOrderInfo.CreateCell(5).SetCellValue("年级");
            headerrowOrderInfo.CreateCell(6).SetCellValue("班级");
            headerrowOrderInfo.CreateCell(7).SetCellValue("老师姓名");
            headerrowOrderInfo.CreateCell(8).SetCellValue("部门");
            headerrowOrderInfo.CreateCell(9).SetCellValue("员工");
            headerrowOrderInfo.CreateCell(10).SetCellValue("产品名称");
            headerrowOrderInfo.CreateCell(11).SetCellValue("支付金额");
            headerrowOrderInfo.CreateCell(12).SetCellValue("手续费");
            headerrowOrderInfo.CreateCell(13).SetCellValue("实际到账");
            for (int i = 0; i < orderlist.Count; i++)
            {
                orderinfo oinfo = orderlist[i];
                IRow rowOrderInfo = sheetOrderInfo.CreateRow(i + 1);
                ICell cellOrderInfo = rowOrderInfo.CreateCell(i);
                cellOrderInfo.CellStyle = stylebill;
                rowOrderInfo.CreateCell(0).SetCellValue(i + 1);
                rowOrderInfo.CreateCell(1).SetCellValue(oinfo.o_id);
                rowOrderInfo.CreateCell(2).SetCellValue(oinfo.o_datetime.ToString());
                rowOrderInfo.CreateCell(3).SetCellValue(oinfo.path);
                rowOrderInfo.CreateCell(4).SetCellValue(oinfo.schoolname);
                rowOrderInfo.CreateCell(5).SetCellValue(oinfo.gradename);
                rowOrderInfo.CreateCell(6).SetCellValue(oinfo.classname);
                rowOrderInfo.CreateCell(7).SetCellValue(oinfo.u_teachername);
                rowOrderInfo.CreateCell(8).SetCellValue(oinfo.m_deptname);
                rowOrderInfo.CreateCell(9).SetCellValue(oinfo.m_a_name);
                rowOrderInfo.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.ProductName(oinfo.channel));
                rowOrderInfo.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_payamount));
                rowOrderInfo.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_feeamount));
                rowOrderInfo.CreateCell(13).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(oinfo.o_actamount));
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                Bill = HttpUtility.UrlEncode(Bill, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                Bill = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Bill)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", Bill + ".xls");

        }
        #endregion

    }
}



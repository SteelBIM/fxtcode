using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using KSWF.WFM.BLL;
using KSWF.Core.Utility;
using KSWF.WFM.Constract.VW;
using KSWF.Web.Admin.Models;
using System.Linq.Expressions;
using KSWF.WFM.Constract.Models;

namespace KSWF.Web.Admin.Controllers
{
    public class CommissionPayController : BaseController
    {
        //2017-05-26
        // 提成结算
        public ActionResult Index()
        {
            return View();
        }
        //2017-05-27
        //员工核算 
        public ActionResult Accounting()
        {
            ViewBag.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Operator = masterinfo.truename;
            ViewBag.KontNumbering = Core.Utility.PublicHelp.AgentId();
            return View();
        }

        //2017-05-28
        //代理商核算 
        public ActionResult AccountingAgent()
        {
            string agentmastername = Request["n"];
            if (!string.IsNullOrEmpty(agentmastername))//当前代理商的签约截止日期与当前日期比较 
            {
                List<com_master> master = base.SelectSearch<com_master>(t => t.mastername == agentmastername);
                if (master != null && master.Count > 0)
                {
                    if (master[0].agent_enddate > DateTime.Now)
                        ViewBag.SignatureCoupureTime = DateTime.Now.ToString("yyyy-MM-dd");
                    else
                        ViewBag.SignatureCoupureTime = master[0].agent_enddate.ToString("yyyy-MM-dd");
                }
                else
                    ViewBag.SignatureCoupureTime = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.SignatureCoupureTime = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.Operator = masterinfo.truename;
            ViewBag.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.KontNumbering = Core.Utility.PublicHelp.AgentId();
            return View();
        }

        //2017-05-27
        //部门核算
        public ActionResult AccountingDept()
        {
            ViewBag.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Operator = masterinfo.truename;
            ViewBag.KontNumbering = Core.Utility.PublicHelp.AgentId();
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
        public JsonResult DeptPrincipalName(int deptid)
        {
            GiveUpActionAuthorityController give = new GiveUpActionAuthorityController();
            return give.GetDeptPrincipalName(masterinfo.agentid, deptid);
        }
        #endregion


        vw_agentmention ordertotallist = new vw_agentmention();

        #region 获取代理商提成总金额
        /// <summary>
        /// 获取代理商提成总金额
        /// </summary>
        /// <param name="agentname"></param>
        /// <param name="parantename"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAgentTotal(string agentname,int deptid)
        {
            string deptids = "";
            if (deptid > 0) 
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());//添加本部门
                for (int i = 0; i < InIds.Count; i++)
                {
                    deptids += InIds[i] + ",";
                }
            }
            List<OrdreTotal> entity = manage.GetAgentNotCommissionedOrdreTotal(masterinfo.agentid, agentname, deptids.TrimEnd(','));
            return Json(entity);
        }
        #endregion

        #region  呈现所有代理商及代理商的提成金额 AgentCommissionSttlement_View(int index, int size, string agentname, string parantename)
        /// <summary>
        /// 获取代理商提成结算
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="agentname">代理商名称</param>
        /// <param name="parantename">渠道经理名称</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgentCommissionSttlement_View(int index, int size, string agentname,int deptid)
        {
            if (!action.View)
                return Json("");
            PageParameter<vw_agentmention> pageParameter = new PageParameter<vw_agentmention>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<vw_agentmention, bool>>> exprlist = new List<Expression<Func<vw_agentmention, bool>>>();
            exprlist.Add(t => t.pagentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(agentname))
                exprlist.Add(t => t.agentname == agentname );
            
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());//添加本部门
                pageParameter.Field = "deptid";
                pageParameter.In = InIds;
            }
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_agentmention> agentmention = base.OrderManage.SelectPage<vw_agentmention>(pageParameter, out total);
            for (int i = 0; i < agentmention.Count; i++)
            {

                List<OrdreTotal> entitys = manage.GetAgentOrdreTotal(agentmention[i].agentid, agentmention[i].enddate, agentmention[i].agent_enddate);
                if (entitys != null && entitys.Count > 0)
                {
                    agentmention[i].payamount = entitys[0].o_payamount;
                    agentmention[i].ordernumber = entitys[0].o_number;
                }
            }
            return Json(new { total = total, rows = agentmention });
        }
        OrderManage manage = new OrderManage();



        #endregion

        #region 单个代理商信息展示
        /// <summary>
        /// 获取代理商提成金额
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAgentOrderTotal(string mastername, string endtime)
        {
            List<com_master> agentlist = base.SelectSearch<com_master>(t => t.mastername == mastername);
            OrdreTotal total = new OrdreTotal();
            List<OrdreTotal> entitys = manage.GetAgentOrdreTotal(agentlist[0].agentid, endtime);
            if (entitys != null && entitys.Count > 0)
            {
                total.o_payamount = entitys[0].o_payamount;
                total.o_number = entitys[0].o_number;
                total.o_bonus = entitys[0].o_bonus;
            }
            return Json(total);
        }
        /// <summary>
        /// 代理商订单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgentidOrdreInfo_View(int index, int size, string mastername, string endtime)
        {
            if (!action.View) //没有预览权限
                return Json("");
            List<Expression<Func<vw_agentorderdetails, bool>>> exprlist = new List<Expression<Func<vw_agentorderdetails, bool>>>();
            PageParameter<vw_agentorderdetails> pageParameter = new PageParameter<vw_agentorderdetails>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<com_master> agentlist = base.SelectSearch<com_master>(t => t.mastername == mastername);
            #region 上次结算日期
            int starttime = GetRecentlySettledTime(mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion
            if (agentlist != null && agentlist.Count > 0)
            {
                exprlist.Add(t => t.agentid == agentlist[0].agentid);
                DateTime agent_endtime = agentlist[0].agent_enddate.AddDays(1);
                exprlist.Add(t => t.o_datetime < agent_endtime);
                if (!string.IsNullOrEmpty(endtime))
                    exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            }
            else
                return Json("");
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.o_datetime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<vw_agentorderdetails> employeeorder = base.OrderManage.SelectPage<vw_agentorderdetails>(pageParameter, out total);
            return Json(new { total = total, rows = employeeorder });
        }

        List<string> listagentid = new List<string>();
        /// <summary>
        /// 获取所有下级代理商agentid
        /// </summary>
        /// <param name="agentid"></param>
        public void GetAllAubordinateAgentId(string agentid, string filed)
        {
            List<com_master> list = base.SelectAppointField<com_master>(t => (t.pagentid == agentid && t.mastertype == 1), filed);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].agentid != null)
                    {
                        listagentid.Add(list[i].agentid);
                        GetAllAubordinateAgentId(list[i].agentid, filed);
                    }
                }
            }
        }

        /// <summary>
        /// /获取所有下级代理商agentid及自己的agentid
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        public List<string> GetAllAubordinateAgentId(string mastername)
        {
            string filed = "agentid";
            listagentid = new List<string>();//重新实例化list
            List<com_master> list = base.SelectAppointField<com_master>(t => t.mastername == mastername, filed);
            if (list != null && list.Count > 0)
            {
                GetAllAubordinateAgentId(list[0].agentid, filed);
                listagentid.Add(list[0].agentid);//添加自己的agentid
                return listagentid;
            }
            return null;
        }

        /// <summary>
        /// /获取代理商的查询 条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<Expression<Func<vw_agentorderdetails, bool>>> GetAgentSelectWhere(string mastername, string endtime)
        {
            List<Expression<Func<vw_agentorderdetails, bool>>> exprlist = new List<Expression<Func<vw_agentorderdetails, bool>>>();
            exprlist.Add(t => t.o_bonus > 0);

            #region 上次结算日期
            int starttime = GetRecentlySettledTime(mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            // ogentidlist = GetAllAubordinateAgentId(mastername);

            return exprlist;
        }

        /// <summary>
        /// 获取代理商用户基础信息
        /// </summary>
        /// <param name="mastername"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAgentBasicInformation(string mastername)
        {
            List<vw_agent> agentlist = base.SelectSearch<vw_agent>(t => t.mastername == mastername);
            List<com_master> master = base.SelectSearch<com_master>(t => t.mastername == mastername);
            agentlist[0].mobile = master[0].mobile;
            agentlist[0].agent_tel = master[0].agent_tel;
            agentlist[0].agent_fax = master[0].agent_fax;
            agentlist[0].agent_addr = master[0].agent_addr;
            return Json(agentlist);
        }

        #endregion

        #region 获取所有员工未提成总金额
        /// <summary>
        /// 获取所有员工未提成总金额
        /// </summary>
        /// <param name="agentname"></param>
        /// <param name="parantename"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmployeeTotal(string truename, int deptid)
        {
            if (!action.View)
                return Json("");

            PageParameter<vw_employeeention> pageParameter = new PageParameter<vw_employeeention>();
            List<Expression<Func<vw_employeeention, bool>>> exprlist = new List<Expression<Func<vw_employeeention, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(truename))
                exprlist.Add(t => t.truename == truename);

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
            OrdreTotal total = new OrdreTotal();
            List<vw_employeeention> employeemention = base.OrderManage.SelectSearchs<vw_employeeention>(exprlist, pageParameter.Field, pageParameter.In);
            if (employeemention != null && employeemention.Count > 0)
            {
                foreach (vw_employeeention row in employeemention)
                {
                    total.o_bonus += row.o_bonus;
                    total.o_payamount += row.o_payamount;
                    total.o_number += row.o_number;
                }
            }
            return Json(total);
        }
        #endregion

        #region  呈现所有员工的提成金额 EmployeeCommissionSttlement_View(int index, int size, string agentname, string parantename)
        /// <summary>
        /// 获取员工提成结算
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="agentname"></param>
        /// <param name="parantename"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmployeeCommissionSttlement_View(int index, int size, string truename, int deptid)
        {
            if (!action.View)
                return Json("");
            PageParameter<vw_employeeention> pageParameter = new PageParameter<vw_employeeention>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<vw_employeeention, bool>>> exprlist = new List<Expression<Func<vw_employeeention, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);

            if (!string.IsNullOrEmpty(truename))
                exprlist.Add(t => t.truename == truename);

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
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_employeeention> employeemention = base.OrderManage.SelectPage<vw_employeeention>(pageParameter, out total);
            return Json(new { total = total, rows = employeemention });
        }
        #endregion



        #region 获取单个员工未提成总金额
        /// <summary>
        /// 获取单个员工未提成总金额
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEmployeeOrderTotal(string mastername, string endtime)
        {
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_mastername == mastername) && (t.o_totype == 0 || t.o_totype == 1)));
            exprlist.Add(t => t.o_bonus > 0);

            #region 上次结算日期
            int starttime = GetRecentlySettledTime(mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            List<orderinfo> employeeorder = base.OrderManage.SelectSearch<orderinfo>(exprlist);
            OrdreTotal total = new OrdreTotal();
            if (employeeorder != null && employeeorder.Count > 0)
            {
                total.o_number = employeeorder.Count;
                foreach (orderinfo row in employeeorder)
                {
                    total.o_bonus += row.o_bonus;
                    total.o_payamount += row.o_payamount;
                }
            }
            return Json(total);
        }

        /// <summary>
        /// 员工订单展示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmployeeOrdreInfo_View(int index, int size, string mastername, string endtime)
        {
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<orderinfo> pageParameter = new PageParameter<orderinfo>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_mastername == mastername) && (t.o_totype == 0 || t.o_totype == 1)));
            exprlist.Add(t => t.o_bonus > 0);//只显示提成金额大于0的

            int starttime = GetRecentlySettledTime(mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));

            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.o_datetime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<orderinfo> employeeorder = base.OrderManage.SelectPage<orderinfo>(pageParameter, out total);
            return Json(new { total = total, rows = employeeorder });
        }
        #endregion


        #region 员工订单明细导出
        [HttpPost]
        public FileResult DeptExport([System.Web.Http.FromBody]orderinfo order)
        {
            if (!action.Export)
                return null;
            PageParameter<orderinfo> pageParameter = new PageParameter<orderinfo>();

            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_deptid == order.m_deptid) && (t.o_totype == 2)));

            int starttime = GetDeptRecentlySettledTime(Convert.ToInt32(order.m_deptid));
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }

            if (!string.IsNullOrEmpty(order.o_datetime.ToString()))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(order.o_datetime));

            List<orderinfo> list = base.OrderManage.SelectSearchs<orderinfo>(exprlist,"",null," o_datetime desc");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);
            IRow headerrow = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("订单编号");
            headerrow.CreateCell(2).SetCellValue("订单日期");
            headerrow.CreateCell(3).SetCellValue("所属地区");
            headerrow.CreateCell(4).SetCellValue("学校");
            headerrow.CreateCell(5).SetCellValue("年级");
            headerrow.CreateCell(6).SetCellValue("班级");
            headerrow.CreateCell(7).SetCellValue("老师姓名");
            headerrow.CreateCell(8).SetCellValue("部门");
            headerrow.CreateCell(9).SetCellValue("产品名称");
            headerrow.CreateCell(10).SetCellValue("支付金额");
            headerrow.CreateCell(11).SetCellValue("手续费");
            headerrow.CreateCell(12).SetCellValue("实际到账");
            for (int i = 0; i < list.Count; i++)
            {
                orderinfo orderinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);
                ICell cell = row.CreateCell(i);
                cell.CellStyle = style;
                row.CreateCell(0).SetCellValue(i + 1);
                row.CreateCell(1).SetCellValue(orderinfo.o_id);
                row.CreateCell(2).SetCellValue(orderinfo.o_datetime.ToString());
                row.CreateCell(3).SetCellValue(orderinfo.path);
                row.CreateCell(4).SetCellValue(orderinfo.schoolname);
                row.CreateCell(5).SetCellValue(orderinfo.gradename);
                row.CreateCell(6).SetCellValue(orderinfo.classname);
                row.CreateCell(7).SetCellValue(orderinfo.u_teachername);
                row.CreateCell(8).SetCellValue(orderinfo.m_deptname);
                row.CreateCell(9).SetCellValue( Core.Utility.PublicHelp.ProductName(orderinfo.channel));
                row.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_payamount));
                row.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_feeamount));
                row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_actamount));


                
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

        #region 员工订单明细导出
        [HttpPost]
        public FileResult Export([System.Web.Http.FromBody]orderinfo order)
        {
            if (!action.Export)
                return null;
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_mastername == order.m_mastername) && (t.o_totype == 0 || t.o_totype == 1)));
            exprlist.Add(t => t.o_bonus > 0);//只显示提成金额大于0的

            #region 上次结算日期
            int starttime = GetRecentlySettledTime(order.m_mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion

            if (order.o_datetime != null && !string.IsNullOrEmpty(order.o_datetime.ToString()))
                exprlist.Add(t => t.o_datetime < order.o_datetime);

            List<orderinfo> list = base.OrderManage.SelectSearchs<orderinfo>(exprlist,"",null," o_datetime desc ");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);
            IRow headerrow = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("订单编号");
            headerrow.CreateCell(2).SetCellValue("订单日期");
            headerrow.CreateCell(3).SetCellValue("所属地区");
            headerrow.CreateCell(4).SetCellValue("学校");
            headerrow.CreateCell(5).SetCellValue("年级");
            headerrow.CreateCell(6).SetCellValue("班级");
            headerrow.CreateCell(7).SetCellValue("老师姓名");
            headerrow.CreateCell(8).SetCellValue("部门");
            headerrow.CreateCell(9).SetCellValue("员工");
            headerrow.CreateCell(10).SetCellValue("产品名称");
            headerrow.CreateCell(11).SetCellValue("支付金额");
            headerrow.CreateCell(12).SetCellValue("手续费");
            headerrow.CreateCell(13).SetCellValue("实际到账");
            headerrow.CreateCell(14).SetCellValue("提成金额");
            for (int i = 0; i < list.Count; i++)
            {
                orderinfo orderinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);
                ICell cell = row.CreateCell(i);
                cell.CellStyle = style;
                row.CreateCell(0).SetCellValue(i + 1);
                row.CreateCell(1).SetCellValue(orderinfo.o_id);
                row.CreateCell(2).SetCellValue(orderinfo.o_datetime.ToString());
                row.CreateCell(3).SetCellValue(orderinfo.path);
                row.CreateCell(4).SetCellValue(orderinfo.schoolname);
                row.CreateCell(5).SetCellValue(orderinfo.gradename);
                row.CreateCell(6).SetCellValue(orderinfo.classname);
                row.CreateCell(7).SetCellValue(orderinfo.u_teachername);
                row.CreateCell(8).SetCellValue(orderinfo.m_deptname);
                row.CreateCell(9).SetCellValue(orderinfo.m_a_name);
                row.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.ProductName(orderinfo.channel));
                row.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_payamount));
                row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_feeamount));
                row.CreateCell(13).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_actamount));
                row.CreateCell(14).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_bonus));
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

        #region 代理商订单明细导出
        [HttpPost]
        public FileResult AgentExport([System.Web.Http.FromBody]orderinfo order)
        {
            if (!action.Export)
                return null;
            List<Expression<Func<vw_agentorderdetails, bool>>> exprlist = new List<Expression<Func<vw_agentorderdetails, bool>>>();
            PageParameter<vw_agentorderdetails> pageParameter = new PageParameter<vw_agentorderdetails>();
            List<com_master> agentlist = base.SelectAppointField<com_master>(t => t.mastername == order.m_mastername, "agentid");
            #region 上次结算日期
            int starttime = GetRecentlySettledTime(order.m_mastername);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion
            if (agentlist != null && agentlist.Count > 0)
            {
             
                exprlist.Add(t => t.agentid == agentlist[0].agentid);
            }
            else
            {
                exprlist.Add(t => t.agentid == Core.Utility.PublicHelp.AgentId());
            }
            if (!string.IsNullOrEmpty(order.o_datetime.ToString()))
                exprlist.Add(t => t.o_datetime < order.o_datetime);
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.o_datetime;
            pageParameter.IsOrderByASC = 0;
            List<vw_agentorderdetails> list = base.OrderManage.SelectSearchs<vw_agentorderdetails>(pageParameter.Wheres,"",null," o_datetime desc");

            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("订单编号");
            headerrow.CreateCell(2).SetCellValue("订单日期");
            headerrow.CreateCell(3).SetCellValue("所属地区");
            headerrow.CreateCell(4).SetCellValue("学校");
            headerrow.CreateCell(5).SetCellValue("年级");
            headerrow.CreateCell(6).SetCellValue("班级");
            headerrow.CreateCell(7).SetCellValue("老师姓名");
            headerrow.CreateCell(8).SetCellValue("产品名称");
            headerrow.CreateCell(9).SetCellValue("支付金额");
            headerrow.CreateCell(10).SetCellValue("手续费");
            headerrow.CreateCell(11).SetCellValue("实际到账");
            headerrow.CreateCell(12).SetCellValue("提成金额");
            for (int i = 0; i < list.Count; i++)
            {
                orderinfo orderinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(orderinfo.o_id);
                row.CreateCell(2).SetCellValue(orderinfo.o_datetime.ToString());
                row.CreateCell(3).SetCellValue(orderinfo.path);
                row.CreateCell(4).SetCellValue(orderinfo.schoolname);
                row.CreateCell(5).SetCellValue(orderinfo.gradename);
                row.CreateCell(6).SetCellValue(orderinfo.classname);
                row.CreateCell(7).SetCellValue(orderinfo.u_teachername);
                row.CreateCell(8).SetCellValue(Core.Utility.PublicHelp.ProductName(orderinfo.channel));
                row.CreateCell(9).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_payamount));
                row.CreateCell(10).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_feeamount));
                row.CreateCell(11).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_actamount));
                row.CreateCell(12).SetCellValue(Core.Utility.PublicHelp.TakeTwoDecimalPlaces(orderinfo.o_bonus));
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

        #region 结算员工结算详细信息展示
        /// <summary>
        /// 结算信息展示 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult KontInformationDisplay(string mastername, DateTime endtime)
        {

            OrderManage manage = new OrderManage();
            List<orderkont> Listorderkont = manage.Getemployee(mastername, GetRecentlySettledTime(mastername), Convert.ToInt32(KSWF.Core.Utility.PublicHelp.ConvertDateTimeInt(endtime)));
            if (Listorderkont != null)
                return Json(new { total = Listorderkont.Count, rows =   AddTotal(Listorderkont,0) });
            return Json("");
        }
        #endregion

        #region 添加员工提成结算记录
        /// <summary>
        /// 添加员工提成结算记录
        /// </summary>
        /// <param name="os_no">结算单号</param>
        /// <param name="endtime">结算截止日期</param>
        /// <param name="mastername_t">被结算人</param>
        /// <param name="adjust_amount">调整金额</param>
        /// <param name="adjust_reason">调整原因</param>
        /// <returns></returns>
        [HttpPost]
        public int Setbonus_Add(string os_no, DateTime endtime, string mastername_t, decimal? adjust_amount, string adjust_reason)
        {
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_mastername == mastername_t) && (t.o_totype == 0 || t.o_totype == 1)));
            exprlist.Add(t => t.o_bonus > 0);
            exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            int starttime = GetRecentlySettledTime(mastername_t);
            if (starttime > 0)
            {
                DateTime stime=KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            List<orderinfo> employeeorder = base.OrderManage.SelectSearch<orderinfo>(exprlist);
            OrdreTotal total = new OrdreTotal();
            if (employeeorder != null && employeeorder.Count > 0)
            {
                total.o_number = employeeorder.Count;
                foreach (orderinfo row in employeeorder)
                {
                    total.o_bonus += row.o_bonus;
                    total.o_payamount += row.o_payamount;
                }
            }
            order_setbonus subdata = new order_setbonus()
            {
                os_no = os_no,
                osid = Guid.NewGuid(),
                mastername_t = mastername_t,
                startdate = GetRecentlySettledTime(mastername_t),
                enddate = Convert.ToInt32(KSWF.Core.Utility.PublicHelp.ConvertDateTimeInt(endtime)),
                total_count = total.o_number,
                total_bonus = total.o_bonus,
                total_amount = total.o_payamount,
                os_type = 0,
                state = 0,
                adjust_amount = adjust_amount,
                adjust_reason = adjust_reason.Trim(),
                createtime = DateTime.Now,
                mastername = masterinfo.mastername,
                agentid = masterinfo.agentid
            };
            List<order_setbonus> list = new List<order_setbonus>();
            list.Add(subdata);
            if (base.OrderManage.InsertRange<order_setbonus>(list) != null)
                return 1;
            return 0;

        }
        #endregion

        #region 修改代理商提成结算记录
        /// <summary>
        /// 添加员工提成结算记录
        /// </summary>
        /// <param name="os_no">结算单号</param>
        /// <param name="endtime">结算截止日期</param>
        /// <param name="mastername_t">被结算人</param>
        /// <param name="adjust_amount">调整金额</param>
        /// <param name="adjust_reason">调整原因</param>
        /// <returns></returns>
        [HttpPost]
        public bool Setbonus_update(Guid osid, decimal? adjust_amount, string adjust_reason, int type)
        {
            if (type == 1 || type == 0)
            {
                order_setbonus subdata = new order_setbonus()
                {
                    osid = osid,
                    adjust_amount = adjust_amount,
                    adjust_reason = adjust_reason.Trim() 
                };
                string[] disUpdateColums = new string[] { "os_no", "mastername_t", "startdate", "enddate", "total_count", "total_amount", "total_bonus", "os_type", "state", "agentid", "createtime","mastername" };
                return base.OrderManage.Update<order_setbonus>(subdata, disUpdateColums);
            }
            else
            {
                order_setbonus_dept subdeptdata = new order_setbonus_dept()
                {
                    osid = osid,
                    adjust_amount = adjust_amount,
                    adjust_reason = adjust_reason.Trim() 
                };
                string[] disUpdateColums = new string[] { "os_no", "mastername_t", "deptid", "deptname", "startdate", "enddate", "total_count", "total_amount", "total_bonus", "os_type", "state", "agentid", "team_bonus_r", "createtime", "mastername" };
                return base.OrderManage.Update<order_setbonus_dept>(subdeptdata, disUpdateColums);
            }

        }
        #endregion

        /// <summary>
        /// 计算代理商结算金额
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public OrdreTotal GetAgentTotal(string mastername, DateTime endtime)
        {
            List<com_master> agentlist = base.SelectSearch<com_master>(t => t.mastername == mastername);
            OrdreTotal total = new OrdreTotal();
            List<OrdreTotal> entitys = manage.GetAgentOrdreTotal(agentlist[0].agentid, endtime.ToString());
            if (entitys != null && entitys.Count > 0)
            {
                total.o_payamount = entitys[0].o_payamount;
                total.o_number = entitys[0].o_number;
                total.o_bonus = entitys[0].o_bonus;
            }
            return total;
        }

        #region 添加代理商提成结算记录
        /// <summary>
        /// 添加员工提成结算记录
        /// </summary>
        /// <param name="os_no">结算单号</param>
        /// <param name="endtime">结算截止日期</param>
        /// <param name="mastername_t">被结算人</param>
        /// <param name="adjust_amount">调整金额</param>
        /// <param name="adjust_reason">调整原因</param>
        /// <returns></returns>
        [HttpPost]
        public int AgentSetbonus_Add(string os_no, DateTime endtime, string mastername_t, decimal? adjust_amount, string adjust_reason)
        {
            OrdreTotal total = GetAgentTotal(mastername_t, endtime);
            order_setbonus subdata = new order_setbonus()
            {
                os_no = os_no,
                osid = Guid.NewGuid(),
                mastername_t = mastername_t,
                startdate = GetRecentlySettledTime(mastername_t),
                enddate = Convert.ToInt32(KSWF.Core.Utility.PublicHelp.ConvertDateTimeInt(endtime)),
                total_count = total.o_number,
                total_bonus = total.o_bonus,
                total_amount = total.o_payamount,
                os_type = 1,
                state = 0,
                adjust_amount = adjust_amount,
                adjust_reason = adjust_reason.Trim(),
                createtime = DateTime.Now,
                mastername = masterinfo.mastername,
                agentid = masterinfo.agentid
            };
            List<order_setbonus> list = new List<order_setbonus>();
            list.Add(subdata);
            if (base.OrderManage.InsertRange<order_setbonus>(list) != null)
                return 1;
            return 0;

        }
        #endregion

        #region 代理商结算详细展示
        /// <summary>
        /// 代理商结算详细展示
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgentKontInformationDisplay(string mastername, DateTime endtime)
        {
            List<com_master> list = base.SelectSearch<com_master>(t => t.mastername == mastername);
            if (list != null && list.Count > 0)
            {
                OrderManage manage = new OrderManage();

                List<orderkont> Listorderkont = manage.GetAgent(list[0].agentid, GetRecentlySettledTime(mastername), Convert.ToInt32(Core.Utility.PublicHelp.ConvertDateTimeInt(endtime)));
                if (Listorderkont != null)
                    return Json(new { total = Listorderkont.Count, rows = AddTotal(Listorderkont,0) });
            }
            return Json("");
        }
        #endregion

        #region 部门

        /// <summary>
        /// 获取部门未结算提成金额
        /// </summary>
        /// <param name="principalname"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public JsonResult GetDeptTotal(string principalname, int deptid)
        {
            PageParameter<vw_deptmention> pageParameter = new PageParameter<vw_deptmention>();
            List<Expression<Func<vw_deptmention, bool>>> exprlist = new List<Expression<Func<vw_deptmention, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(principalname))
                exprlist.Add(t => t.principalname == principalname);
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "m_deptid";
                    pageParameter.In = InIds;
                }
            }
            List<vw_deptmention> list = base.OrderManage.SelectSearchs<vw_deptmention>(exprlist, pageParameter.Field, pageParameter.In);
            decimal? dept_sales = 0;
            decimal? dept_number = 0;
            foreach (vw_deptmention row in list)
            {
                dept_number += row.o_number;
                dept_sales += row.o_payamount;
            }
            return Json(new { dept_sales = dept_sales, dept_number = dept_number });
        }


        #region 获取部门提成结算单DeptommissionSttlement_View(int index, int size, string principalname, int deptid)
        /// 获取部门提成结算单
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="principalname"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeptommissionSttlement_View(int index, int size, string principalname, int deptid)
        {
            if (!action.View) //没有预览权限
                return Json("");

            PageParameter<vw_deptmention> pageParameter = new PageParameter<vw_deptmention>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<vw_deptmention, bool>>> exprlist = new List<Expression<Func<vw_deptmention, bool>>>();
            exprlist.Add(t => t.agentid == masterinfo.agentid);
            if (!string.IsNullOrEmpty(principalname))
                exprlist.Add(t => t.principalname == principalname);
            //int starttime = GetDeptRecentlySettledTime(deptid);
            if (deptid > 0)
            {
                Recursive give = new Recursive();
                List<string> InIds = give.GetDeptNodeId(deptid, masterinfo.agentid);
                InIds.Add(deptid.ToString());
                if (InIds != null && InIds.Count > 0)
                {
                    pageParameter.Field = "m_deptid";
                    pageParameter.In = InIds;
                }
            }
            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.m_deptid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<vw_deptmention> employeemention = base.OrderManage.SelectPage<vw_deptmention>(pageParameter, out total);
            return Json(new { total = total, rows = employeemention });
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
        public JsonResult DeptOrdreInfo_View(int index, int size, int deptid, string endtime)
        {
            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<orderinfo> pageParameter = new PageParameter<orderinfo>();
            pageParameter.PageIndex = setpageindex(index, size);
            pageParameter.PageSize = size;
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_deptid == deptid) && (t.o_totype == 2)));

            int starttime = GetDeptRecentlySettledTime(deptid);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));

            pageParameter.Wheres = exprlist;
            pageParameter.OrderColumns = t1 => t1.o_datetime;
            pageParameter.IsOrderByASC = 0;
            int total = 0;
            IList<orderinfo> employeeorder = base.OrderManage.SelectPage<orderinfo>(pageParameter, out total);
            return Json(new { total = total, rows = employeeorder });
        }




        /// <summary>
        /// 获取部门总金额
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDeptOrderTotal(int deptid, string endtime)
        {
            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_deptid == deptid) && t.o_totype == 2));

            #region 上次结算日期
            int starttime = GetDeptRecentlySettledTime(deptid);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            List<orderinfo> employeeorder = base.OrderManage.SelectSearch<orderinfo>(exprlist);
            OrdreTotal total = new OrdreTotal();
            if (employeeorder != null && employeeorder.Count > 0)
            {
                total.o_number = employeeorder.Count;
                foreach (orderinfo row in employeeorder)
                {
                    total.o_bonus += row.o_bonus;
                    total.o_payamount += row.o_payamount;
                }
            }
            return Json(total);
        }
        #region 结算员工结算详细信息展示
        /// <summary>
        /// 结算信息展示 
        /// </summary>
        /// <param name="master"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeptKontInformationDisplay(int deptid, DateTime endtime)
        {
            OrderManage manage = new OrderManage();
            List<orderkont> Listorderkont = manage.Getdept(deptid, GetDeptRecentlySettledTime(deptid), Convert.ToInt32(KSWF.Core.Utility.PublicHelp.ConvertDateTimeInt(endtime)));
            if (Listorderkont != null)
                return Json(new { total = Listorderkont.Count, rows = Listorderkont });
            return Json("");
        }
        #endregion

        #region 添加员工提成结算记录
        /// <summary>
        /// 添加员工提成结算记录
        /// </summary>
        /// <param name="os_no">结算单号</param>
        /// <param name="endtime">结算截止日期</param>
        /// <param name="mastername_t">被结算人</param>
        /// <param name="adjust_amount">调整金额</param>
        /// <param name="adjust_reason">调整原因</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeptSetbonus_Add(string os_no, string endtime, int deptid, string deptname, int commission)
        {
            Core.Utility.KingResponse res = new KingResponse();
            string principalname = "";//部门负责人
            List<com_master> masterlist = base.SelectSearch<com_master>((t => t.deptid == deptid && t.groupid == 4), 1, " masterid desc ");
            if (masterlist != null && masterlist.Count > 0)
            {
                principalname = masterlist[0].mastername;
            }
            else
            {
                res.ErrorMsg = "找不到部门负责人~";
                return Json(res);
            }

            #region 总金额

            List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
            exprlist.Add(t => ((t.m_deptid == deptid) && t.o_totype == 2));

            #region 上次结算日期
            int starttime = GetDeptRecentlySettledTime(deptid);
            if (starttime > 0)
            {
                DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                exprlist.Add(t => t.o_datetime >= stime);
            }
            #endregion

            if (!string.IsNullOrEmpty(endtime))
                exprlist.Add(t => t.o_datetime < Convert.ToDateTime(endtime));
            List<orderinfo> employeeorder = base.OrderManage.SelectSearch<orderinfo>(exprlist);
            OrdreTotal total = new OrdreTotal();
            if (employeeorder != null && employeeorder.Count > 0)
            {
                total.o_number = employeeorder.Count;
                foreach (orderinfo row in employeeorder)
                {
                    total.o_bonus += row.o_bonus;
                    total.o_payamount += row.o_payamount;
                }
            }
            if (total.o_payamount <= 0)
            {

                res.ErrorMsg = "分成金额为0，请重新选择日期~";
                return Json(res);
            }
            #endregion

            order_setbonus_dept deptentity = new order_setbonus_dept()
            {
                osid = Guid.NewGuid(),
                os_no = os_no,
                deptid = deptid,
                deptname = deptname,
                mastername_t = principalname,
                startdate = starttime,
                enddate = Convert.ToInt32(Core.Utility.PublicHelp.ConvertDateTimeInt(Convert.ToDateTime(endtime))),
                total_count = total.o_number,
                total_amount = total.o_payamount,
                total_bonus = total.o_payamount * commission / 100,
                team_bonus_r = commission,
                state = 0,
                adjust_amount=0,
                adjust_reason="",
                mastername = masterinfo.mastername,
                createtime = DateTime.Now,
                agentid = masterinfo.agentid
            };

            if (base.OrderManage.Add<order_setbonus_dept>(deptentity) > 0)
            {
                res.Success = true;
            }
            return Json(res);

        }
        #endregion
        #endregion
    }
}

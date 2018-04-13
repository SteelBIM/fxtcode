using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
using KSWF.WFM.BLL;
using KSWF.WFM.Constract.Models;
using KSWF.WFM.Constract.VW;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KSWF.Web.Admin.Controllers
{
    public class NotKnotController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ChannelList = KeyValueManage.GetChannleData();
            return View();
        }

        [HttpPost]
        public JsonResult Statistics(int pannelType, string sTime, string eTime)
        {
            OrderManage om = new WFM.BLL.OrderManage();
            string whereSql = GetSqlWhere(pannelType);
            if (!string.IsNullOrEmpty(sTime))
                whereSql += " and o_datetime>'" + sTime + "'";
            if (!string.IsNullOrEmpty(eTime))
                whereSql += " and o_datetime<'" + eTime + "'";
            int endtime = pannelType == 2 ? GetDeptRecentlySettledTime(masterinfo.deptid) : GetRecentlySettledTime(masterinfo.mastername);
            DateTime etime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            var amount = om.GetSumAmount(whereSql, etime.ToShortDateString());


            if (pannelType == 0)
            {
                //List<OrdreTotal> orderinfo = om.GetAgentOrdreTotal(masterinfo.agentid, eTime, sTime);
                //if (orderinfo != null && orderinfo.Count > 0)
                //    return Json(new { totalOrder = orderinfo[0].o_number, totalPayAmount = orderinfo[0].o_payamount, totalActAmount = orderinfo[0].o_bonus });
                var agentAmount = om.GetAgentSumAmount(whereSql, etime.ToShortDateString());
                return Json(new { totalOrder = agentAmount.total, totalPayAmount = agentAmount.paycount, totalActAmount = agentAmount.actcount });
            }
            else if (pannelType == 1)
            {
                List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
                exprlist.Add(t => ((t.m_mastername == masterinfo.mastername) && (t.o_totype == 0 || t.o_totype == 1)));
                exprlist.Add(t => t.o_bonus > 0);

                #region 上次结算日期
                int starttime = GetRecentlySettledTime(masterinfo.mastername);
                if (starttime > 0)
                {
                    DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                    exprlist.Add(t => t.o_datetime >= stime);
                }
                #endregion

                if (!string.IsNullOrEmpty(eTime))
                    exprlist.Add(t => t.o_datetime < Convert.ToDateTime(eTime));
                if (!string.IsNullOrEmpty(sTime))
                    exprlist.Add(t => t.o_datetime > Convert.ToDateTime(sTime));
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
                return Json(new { totalOrder = total.o_number, totalPayAmount = total.o_payamount, totalActAmount = total.o_bonus });
            }
            else if (pannelType == 2)
            {
                List<Expression<Func<orderinfo, bool>>> exprlist = new List<Expression<Func<orderinfo, bool>>>();
                exprlist.Add(t => ((t.m_deptid == masterinfo.deptid) && t.o_totype == 2));

                #region 上次结算日期
                int starttime = GetDeptRecentlySettledTime(masterinfo.deptid);
                if (starttime > 0)
                {
                    DateTime stime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(starttime);
                    exprlist.Add(t => t.o_datetime >= stime);
                }
                #endregion

                if (!string.IsNullOrEmpty(eTime))
                    exprlist.Add(t => t.o_datetime < Convert.ToDateTime(eTime));
                if (!string.IsNullOrEmpty(sTime))
                    exprlist.Add(t => t.o_datetime > Convert.ToDateTime(sTime));
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
                return Json(new { totalOrder = total.o_number, totalPayAmount = total.o_payamount, totalActAmount = total.o_bonus });
            }


            return Json(new { totalOrder = 0, totalPayAmount = 0, totalActAmount = 0 });
        }

        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        [HttpPost]
        public JsonResult GetPageList(int pagesize, int pageindex, int pannelType, [System.Web.Http.FromBody]OrderCondition ocInfo)
        {
            if (pannelType == 2)
            {
                //部门
                return GetPageOrderinfo(pagesize, ref pageindex, pannelType, ocInfo);
            }
            else if (pannelType == 1)
            {
                //员工
                return GetPageOrderinfo(pagesize, ref pageindex, pannelType, ocInfo);
            }
            else
            {
                //代理商
                return GetPageAgentOrderinfo(pagesize, ref pageindex, pannelType, ocInfo);
            }
        }

        private JsonResult GetPageOrderinfo(int pagesize, ref int pageindex, int pannelType, OrderCondition ocInfo)
        {
            PageParameter<orderinfo> param = new PageParameter<orderinfo>();
            int totalcount = 0;
            if (pageindex == 0)
                pageindex = pageindex / pagesize;
            else
                pageindex = pageindex / pagesize + 1;
            param.PageIndex = pageindex;
            param.PageSize = pagesize;

            param.OrderColumns = i => i.o_datetime;
            param.IsOrderByASC = 0;

            param.WhereSql = GetSqlWhere(pannelType);
            param.Wheres = GetOrderCondition(ocInfo);

            //已结算的不显示(相对的是否结算)
            int endtime = pannelType == 2 ? GetDeptRecentlySettledTime(masterinfo.deptid) : GetRecentlySettledTime(masterinfo.mastername);
            DateTime etime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            param.Wheres.Add(i => i.o_datetime > etime);

            IList<orderinfo> list = base.OrderManage.SelectPage<orderinfo>(param, out totalcount).Distinct().ToList();
            foreach (var item in list)
            {
                if (item.agentid != null)
                {
                    if (item.agentid == masterinfo.agentid)
                    {
                        item.m_mastertype = "直销";
                    }
                    else
                    {
                        item.m_mastertype = "代理";
                    }
                }
            }

            return Json(new { total = totalcount, rows = list, pannelType = pannelType });
        }

        private JsonResult GetPageAgentOrderinfo(int pagesize, ref int pageindex, int pannelType, OrderCondition ocInfo)
        {
            PageParameter<vw_agentorderdetails> param = new PageParameter<vw_agentorderdetails>();
            int totalcount = 0;
            if (pageindex == 0)
                pageindex = pageindex / pagesize;
            else
                pageindex = pageindex / pagesize + 1;
            param.PageIndex = pageindex;
            param.PageSize = pagesize;

            param.OrderColumns = i => i.o_datetime;
            param.IsOrderByASC = 0;

            param.WhereSql = GetSqlWhere(pannelType);
            param.Wheres = GetAgentOrderCondition(ocInfo);

            //已结算的不显示(相对的是否结算)
            int endtime = pannelType == 2 ? GetDeptRecentlySettledTime(masterinfo.deptid) : GetRecentlySettledTime(masterinfo.mastername);
            DateTime etime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            param.Wheres.Add(i => i.o_datetime > etime);

            IList<vw_agentorderdetails> list = base.OrderManage.SelectPage<vw_agentorderdetails>(param, out totalcount).Distinct().ToList();
            foreach (var item in list)
            {
                if (item.agentid != null)
                {
                    if (item.agentid == masterinfo.agentid)
                    {
                        item.m_mastertype = "直销";
                    }
                    else
                    {
                        item.m_mastertype = "代理";
                    }
                }
            }

            return Json(new { total = totalcount, rows = list, pannelType = pannelType });
        }

        private string GetSqlWhere(int pannelType)
        {
            //是否有策略，个人的商务策略的分类和版本与订单不一致的不显示(没有策略的订单和不一致的没有提出，提成不等于0可以直接过滤)           
            //不在签约期内的订单数据，不显示（待定）
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            switch (pannelType)
            {
                //2商务负责人
                //3商务
                //4部门负责人
                //5业务员
                //6代理商
                case 1://员工
                    if (masterinfo.groupid == 4 || masterinfo.groupid == 5 || masterinfo.groupid == 6)
                    {
                        sb.Append(string.Format("m_mastername = '{0}' && m_mastertype=0 && (o_totype=0 || o_totype=1) && o_bonus >0 ", masterinfo.mastername));
                    }
                    else
                    {
                        sb.Append("1=2");
                    }
                    break;
                case 2://部门         
                    if (masterinfo.groupid == 4)
                    {
                        sb.Append(GetDeptSqlString(masterinfo.deptid));
                        sb.Append(" && o_totype=2 ");
                    }
                    else
                    {
                        sb.Append("1=2");//没有权限的处理
                    }
                    break;
                case 0://代理商
                    if (masterinfo.groupid == 6 || masterinfo.groupid == 2 || masterinfo.groupid == 3)
                    {
                        sb.AppendFormat("agentid='{0}' ", masterinfo.agentid);
                    }
                    else
                    {
                        sb.Append("1=2");
                    }
                    break;
            }
            sb.Append(")");
            return sb.ToString();
        }

        private List<Expression<Func<vw_agentorderdetails, bool>>> GetAgentOrderCondition(OrderCondition ocInfo)
        {
            List<Expression<Func<vw_agentorderdetails, bool>>> expression = new List<Expression<Func<vw_agentorderdetails, bool>>>();
            if (ocInfo != null)
            {
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.o_datetime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.o_datetime <= ocInfo.endDate);
                }
            }
            return expression;
        }

        private List<Expression<Func<orderinfo, bool>>> GetOrderCondition(OrderCondition ocInfo)
        {
            List<Expression<Func<orderinfo, bool>>> expression = new List<Expression<Func<orderinfo, bool>>>();
            if (ocInfo != null)
            {
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.o_datetime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.o_datetime <= ocInfo.endDate);
                }
            }
            return expression;
        }

        [HttpPost]
        public FileResult ExportAgentOrderXls([System.Web.Http.FromBody]OrderCondition ocInfo, int pannelType)
        {
            int endtime = pannelType == 0 ? GetDeptRecentlySettledTime(masterinfo.deptid) : GetRecentlySettledTime(masterinfo.mastername);
            DateTime etime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            IList<vw_agentorderdetails> list = base.OrderManage.SelectSearch<vw_agentorderdetails>(GetSqlWhere(pannelType), x => x.o_datetime > etime, GetAgentOrderCondition(ocInfo)).OrderByDescending(x => x.o_datetime).ToList();
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle;
            if (pannelType == 2)
            {
                //部门
                tmpTitle = "未结算订单（公司部门）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (pannelType == 0)
            {
                //代理商
                tmpTitle = "未结算订单（代理商）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //员工
                tmpTitle = "未结算订单（公司员工）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            CreateAgentSheet(list, book, tmpTitle + " ", 0, list.Count, pannelType);

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

        [HttpPost]
        public FileResult ExportOrderXls([System.Web.Http.FromBody]OrderCondition ocInfo, int pannelType)
        {
            int endtime = pannelType == 0 ? GetDeptRecentlySettledTime(masterinfo.deptid) : GetRecentlySettledTime(masterinfo.mastername);
            DateTime etime = KSWF.Core.Utility.PublicHelp.ConvertIntDateTime(endtime);
            IList<orderinfo> list = base.OrderManage.SelectSearch<orderinfo>(GetSqlWhere(pannelType), x => x.o_datetime > etime, GetOrderCondition(ocInfo)).OrderByDescending(x => x.o_datetime).ToList();
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle;
            if (pannelType == 2)
            {
                //部门
                tmpTitle = "未结算订单（公司部门）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            else if (pannelType == 0)
            {
                //代理商
                tmpTitle = "未结算订单（代理商）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                //员工
                tmpTitle = "未结算订单（公司员工）" + DateTime.Now.ToString("yyyy-MM-dd");
            }
            CreateSheet(list, book, tmpTitle + " ", 0, list.Count, pannelType);

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

        private void CreateAgentSheet(IList<vw_agentorderdetails> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex, int pannelType)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
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
            headerrow.CreateCell(13).SetCellValue("支付金额");
            headerrow.CreateCell(14).SetCellValue("手续费");
            headerrow.CreateCell(15).SetCellValue("实际到账");
            if (pannelType == 1 || pannelType == 0)
            {
                headerrow.CreateCell(16).SetCellValue("提成金额");
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                vw_agentorderdetails toinfo = list[i];
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
                row.CreateCell(9).SetCellValue(toinfo.agentid == masterinfo.agentid ? "直销" : "代理");
                row.CreateCell(10).SetCellValue(toinfo.m_deptname);
                row.CreateCell(11).SetCellValue(toinfo.m_a_name);
                row.CreateCell(12).SetCellValue(toinfo.channel == 1 ? "同步学" : "C++客户端");
                row.CreateCell(13).SetCellValue(toinfo.o_payamount.ToString("0.00"));
                row.CreateCell(14).SetCellValue(toinfo.o_feeamount.ToString("0.00"));
                row.CreateCell(15).SetCellValue(toinfo.o_actamount.ToString("0.00"));
                if (pannelType == 0 || pannelType == 1)
                {
                    row.CreateCell(16).SetCellValue(toinfo.o_bonus.Value.ToString("0.00"));
                }
            }
        }

        private void CreateSheet(IList<orderinfo> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex, int pannelType)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
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
            headerrow.CreateCell(13).SetCellValue("支付金额");
            headerrow.CreateCell(14).SetCellValue("手续费");
            headerrow.CreateCell(15).SetCellValue("实际到账");
            if (pannelType == 1 || pannelType == 0)
            {
                headerrow.CreateCell(16).SetCellValue("提成金额");
            }
            for (int i = startIndex; i < endIndex; i++)
            {
                orderinfo toinfo = list[i];
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
                row.CreateCell(9).SetCellValue(toinfo.agentid == masterinfo.agentid ? "直销" : "代理");
                row.CreateCell(10).SetCellValue(toinfo.m_deptname);
                row.CreateCell(11).SetCellValue(toinfo.m_a_name);
                row.CreateCell(12).SetCellValue(toinfo.channel == 1 ? "同步学" : "C++客户端");
                row.CreateCell(13).SetCellValue(toinfo.o_payamount.ToString("0.00"));
                row.CreateCell(14).SetCellValue(toinfo.o_feeamount.ToString("0.00"));
                row.CreateCell(15).SetCellValue(toinfo.o_actamount.ToString("0.00"));
                if (pannelType == 0 || pannelType == 1)
                {
                    row.CreateCell(16).SetCellValue(toinfo.o_bonus.Value.ToString("0.00"));
                }
            }
        }
    }
}

using KSWF.Core.Utility;
using KSWF.Web.Admin.Models;
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
    public class PaySingleController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetcurrentAction()
        {
            return Json(action);
        }

        [HttpPost]
        public JsonResult Statistics(int pannelType,string sTime,string eTime)
        {
            WFM.BLL.OrderManage om = new WFM.BLL.OrderManage();
            string whereSql = GetSqlWhere(pannelType);
            if (!string.IsNullOrEmpty(sTime))
                whereSql += " and createtime>'"+sTime+"'";
            if (!string.IsNullOrEmpty(eTime))
                whereSql += " and createtime<'" + eTime + "'";
            var totalBonus = om.GetTotalBonus(whereSql, pannelType);
            return Json(new { totalBonus = totalBonus });
        }

        [HttpPost]
        public JsonResult GetPageList(int pagesize, int pageindex,int pannelType,[System.Web.Http.FromBody]OrderCondition ocInfo)
        {           
            if (pannelType == 2)
            {
                //部门
                PageParameter<vw_deptbill> param = new PageParameter<vw_deptbill>();
                param.Wheres = GetOrderConditionDept(ocInfo);
                return GetPage<vw_deptbill>(pagesize, ref pageindex, pannelType, ocInfo, param);
            }
            else if (pannelType == 1)
            {
                //员工
                PageParameter<vw_employeebill> param = new PageParameter<vw_employeebill>();
                param.Wheres = GetOrderConditionEmployee(ocInfo);
                return GetPage<vw_employeebill>(pagesize, ref pageindex, pannelType, ocInfo, param);
            }
            else
            {
                //代理商
                PageParameter<vw_agentbill> param = new PageParameter<vw_agentbill>();
                param.Wheres = GetOrderConditionAgent(ocInfo);
                return GetPage<vw_agentbill>(pagesize, ref pageindex, pannelType, ocInfo, param);
            }      
        }

        private JsonResult GetPage<T>(int pagesize, ref int pageindex, int pannelType, OrderCondition ocInfo,PageParameter<T> param) where T :class, new()
        {
            int totalcount = 0;
            if (pageindex == 0)
                pageindex = pageindex / pagesize;
            else
                pageindex = pageindex / pagesize + 1;
            param.PageIndex = pageindex;
            param.PageSize = pagesize;
            param.StrOrderColumns = "createtime";
            param.IsOrderByASC = 0;

            param.WhereSql = GetSqlWhere(pannelType);
            IList<T> list = base.OrderManage.SelectPage<T>(param, out totalcount);
            return Json(new { total = totalcount, rows = list, pannelType = pannelType });
        }

        private string GetSqlWhere(int pannelType)
        {
            StringBuilder sb = new StringBuilder();
            switch (pannelType)
            {
                case 1://员工
                    if (masterinfo.groupid == 4 || masterinfo.groupid == 5 || masterinfo.groupid == 6)
                    {
                        sb.Append(string.Format("mastername_t = '{0}' && os_type=0", masterinfo.mastername));
                    }
                    else
                    {
                        sb.Append(" 1=2");
                    }
                    break;
                case 2://部门       
                    if (masterinfo.groupid == 4)
                    {
                        sb.Append(string.Format("deptid ={0} && mastername_t is not null && mastername_t!='' ", masterinfo.deptid));
                    }
                    else
                    {
                        sb.Append(" 1=2");
                    }
                    break;
                case 0://代理商
                    if (masterinfo.groupid == 6)
                    {
                        sb.Append(string.Format("mastername_t = '{0}' && os_type=1", masterinfo.mastername));
                    }
                    else
                    {
                        sb.Append(" 1=2");
                    }
                    break;
            }
            return  sb.ToString();
        }
 
        private List<Expression<Func<vw_employeebill, bool>>> GetOrderConditionEmployee(OrderCondition ocInfo)
        {
            List<Expression<Func<vw_employeebill, bool>>> expression = new List<Expression<Func<vw_employeebill, bool>>>();
           
            if (ocInfo != null)
            {
                if (ocInfo.startDate.HasValue)
                {
                   expression.Add(i => i.createtime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.createtime <= ocInfo.endDate);
                }
            }
            return expression;         
        }

        private List<Expression<Func<vw_agentbill, bool>>> GetOrderConditionAgent(OrderCondition ocInfo)
        {
            List<Expression<Func<vw_agentbill, bool>>> expression = new List<Expression<Func<vw_agentbill, bool>>>();

            if (ocInfo != null)
            {
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.createtime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.createtime <= ocInfo.endDate);
                }
            }
            return expression;
        }

        private List<Expression<Func<vw_deptbill, bool>>> GetOrderConditionDept(OrderCondition ocInfo) 
        {
            List<Expression<Func<vw_deptbill, bool>>> expression = new List<Expression<Func<vw_deptbill, bool>>>();

            if (ocInfo != null)
            {
                if (ocInfo.startDate.HasValue)
                {
                    expression.Add(i => i.createtime >= ocInfo.startDate);
                }
                if (ocInfo.endDate.HasValue)
                {
                    expression.Add(i => i.createtime <= ocInfo.endDate);
                }
            }
            return expression;
        }

        //------------导出需要抽象工具方法（不同List动态加载表头和内容）(待抽象处理)--------------
        /// <summary>
        /// 导出订到到excel表格
        /// </summary>
        /// <param name="ocInfo"></param>
        /// <returns></returns>
        public FileResult ExportOrderXls([System.Web.Http.FromBody]OrderCondition ocInfo, int pannelType)
        {
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿

            string tmpTitle;
            if (pannelType == 2)
            {
                //部门
                IList<vw_deptbill> list = base.OrderManage.SelectSearch<vw_deptbill>(GetSqlWhere(2),GetOrderConditionDept(ocInfo));
                tmpTitle = "提成结算单（公司部门）" + DateTime.Now.ToString("yyyy-MM-dd");
                CreateSheetDeptment(list, book, tmpTitle + " ", 0, list.Count);
            }
            else if (pannelType == 0)
            {
                //代理商
                IList<vw_agentbill> list = base.OrderManage.SelectSearch<vw_agentbill>(GetSqlWhere(0), GetOrderConditionAgent(ocInfo));
                tmpTitle = "提成结算单（代理商）" + DateTime.Now.ToString("yyyy-MM-dd");
                CreateSheetAgent(list, book, tmpTitle + " ", 0, list.Count);
            }
            else
            {
                //员工
                IList<vw_employeebill> list = base.OrderManage.SelectSearch<vw_employeebill>(GetSqlWhere(1), GetOrderConditionEmployee(ocInfo));
                tmpTitle = "提成结算单（公司员工）" + DateTime.Now.ToString("yyyy-MM-dd");
                CreateSheetEmployee(list, book, tmpTitle + " ", 0, list.Count);
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

        private void CreateSheetEmployee(IList<vw_employeebill> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("结算单编号");
            headerrow.CreateCell(1).SetCellValue("员工姓名");
            headerrow.CreateCell(2).SetCellValue("结算时间");
            headerrow.CreateCell(3).SetCellValue("结算销售额");
            headerrow.CreateCell(4).SetCellValue("结算订单数");
            headerrow.CreateCell(5).SetCellValue("结算提成金额");
            headerrow.CreateCell(6).SetCellValue("部门");
            headerrow.CreateCell(7).SetCellValue("生成日期");
            headerrow.CreateCell(8).SetCellValue("操作人");
            for (int i = startIndex; i < endIndex; i++)
            {
                vw_employeebill toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式
                row.CreateCell(0).SetCellValue(toinfo.os_no);
                row.CreateCell(1).SetCellValue(toinfo.truename);
                var endDate = PublicHelp.ConvertIntDateTime(toinfo.enddate);
                row.CreateCell(2).SetCellValue(endDate.AddDays(-1).ToString("yyyy-MM-dd"));
                row.CreateCell(3).SetCellValue(toinfo.total_amount.HasValue ? toinfo.total_amount.Value.ToString("0.00") : "0");
                row.CreateCell(4).SetCellValue(toinfo.total_count.ToString());
                var tb = toinfo.total_bonus.Value + toinfo.adjust_amount.Value;
                row.CreateCell(5).SetCellValue(tb.ToString("0.00"));
                row.CreateCell(6).SetCellValue(toinfo.deptname);
                row.CreateCell(7).SetCellValue(toinfo.createtime.ToString("yyyy-MM-dd"));
                row.CreateCell(8).SetCellValue(toinfo.addname);
            }
        }

        private void CreateSheetAgent(IList<vw_agentbill> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("结算单编号");
            headerrow.CreateCell(1).SetCellValue("代理商");
            headerrow.CreateCell(2).SetCellValue("结算时间");
            headerrow.CreateCell(3).SetCellValue("结算销售额");
            headerrow.CreateCell(4).SetCellValue("结算订单数");
            headerrow.CreateCell(5).SetCellValue("结算提成金额");
            headerrow.CreateCell(6).SetCellValue("负责人姓名");
            headerrow.CreateCell(7).SetCellValue("所属部门");
            headerrow.CreateCell(8).SetCellValue("渠道经理");
            headerrow.CreateCell(9).SetCellValue("生成日期");
            headerrow.CreateCell(10).SetCellValue("操作人");
            for (int i = startIndex; i < endIndex; i++)
            {
                vw_agentbill toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式
                row.CreateCell(0).SetCellValue(toinfo.os_no);
                row.CreateCell(1).SetCellValue(toinfo.agentname);
                var endDate = PublicHelp.ConvertIntDateTime(toinfo.enddate);
                row.CreateCell(2).SetCellValue(endDate.AddDays(-1).ToString("yyyy-MM-dd"));
                row.CreateCell(3).SetCellValue(toinfo.total_amount.HasValue ? toinfo.total_amount.Value.ToString("0.00") : "0");
                row.CreateCell(4).SetCellValue(toinfo.total_count.ToString());
                var tb = toinfo.total_bonus.Value + toinfo.adjust_amount.Value;
                row.CreateCell(5).SetCellValue(tb.ToString("0.00"));
                row.CreateCell(6).SetCellValue(toinfo.truename);
                row.CreateCell(7).SetCellValue(toinfo.deptname);
                row.CreateCell(8).SetCellValue(toinfo.channelmanager);
                row.CreateCell(9).SetCellValue(toinfo.createtime.ToString("yyyy-MM-dd"));
                row.CreateCell(10).SetCellValue(toinfo.addname);
            }
        }

        private void CreateSheetDeptment(IList<vw_deptbill> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("结算单编号");
            headerrow.CreateCell(1).SetCellValue("部门");
            headerrow.CreateCell(2).SetCellValue("结算时间");
            headerrow.CreateCell(3).SetCellValue("结算销售额");
            headerrow.CreateCell(4).SetCellValue("结算订单数");
            headerrow.CreateCell(5).SetCellValue("结算提成金额");
            headerrow.CreateCell(6).SetCellValue("负责人姓名");
            headerrow.CreateCell(7).SetCellValue("生成日期");
            headerrow.CreateCell(8).SetCellValue("操作人");
            for (int i = startIndex; i < endIndex; i++)
            {
                vw_deptbill toinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                row.CreateCell(0).SetCellValue(toinfo.os_no);
                row.CreateCell(1).SetCellValue(toinfo.deptname);
                var endDate = PublicHelp.ConvertIntDateTime(toinfo.enddate);
                row.CreateCell(2).SetCellValue(endDate.AddDays(-1).ToString("yyyy-MM-dd"));
                row.CreateCell(3).SetCellValue(toinfo.total_amount.HasValue ? toinfo.total_amount.Value.ToString("0.00") : "0");
                row.CreateCell(4).SetCellValue(toinfo.total_count.ToString());
                var tb = toinfo.total_bonus.Value + toinfo.adjust_amount.Value;
                row.CreateCell(5).SetCellValue(tb.ToString("0.00"));
                row.CreateCell(6).SetCellValue(toinfo.principalname);
                row.CreateCell(7).SetCellValue(toinfo.createtime.ToString("yyyy-MM-dd"));
                row.CreateCell(8).SetCellValue(toinfo.addname);
            }
        }
    }
}

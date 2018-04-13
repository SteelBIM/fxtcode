using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBSS.UserOrder.Contract.DataModel;
using CBSS.UserOrder.BLL;
using CBSS.Framework.Contract.Enums;
using System.Web.Http;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using CBSS.UserOrder.Contract.ViewModel;

namespace CBSS.Web.Admin.Areas.UserOrder.Controllers
{
    public class UserPayOrderController : ControllerBase
    {
        //
        // GET: /UserOrder/UserPayOrder/

        public ActionResult Index()
        {
            if (CheckActionName("User_View") == false)
                return Redirect("~/Account/Auth/Login");

            ViewBag.Export = action.Export;
            ViewData["PayStatus"] = new SelectList(WebControl.GetSelectList(typeof(PayStatusEnum)), "Value", "Text");
            ViewData["PayWay"] = new SelectList(WebControl.GetSelectList(typeof(PayWayEnum)), "Value", "Text");
            return View();
        }

        public JsonResult GetUserPayOrderList(int pagesize, int pageindex,int PayStatus,int PayWay, string AppName, string UserName, string UserPhone, string OrderID)
        {
            UserPayOrderRequest request = new UserPayOrderRequest();
            request.Status = PayStatus;
            request.PayWay = PayWay;
            request.AppName = AppName;
            request.UserName = UserName;
            request.UserPhone = UserPhone;
            request.OrderID = OrderID;
            request.PageIndex = setpageindex(pageindex, pagesize);
            request.PageSize = pagesize;
            int total = 0;
            IEnumerable<v_UserPayOrder> userpayorderlist = this.UserOrderService.GetUserPayOrderList(out total, request);
            return Json(new { total = total, rows = userpayorderlist });
        }


        public FileResult Export([FromBody]UserPayOrderRequest request)
        {
            MemoryStream ms = new MemoryStream();
            IEnumerable<v_UserPayOrder> userpayorderlist = this.UserOrderService.GetUserPayOrderList( request);
            if (userpayorderlist != null && userpayorderlist.Count() > 0)
            {
                HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
                string tmpTitle = "用户支付订单" + DateTime.Now.ToString("yyyy-MM-dd");
                CreateSheet(userpayorderlist.ToList(), book, tmpTitle);
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
            return File(ms, "application/txt", "当前数据为空.txt");
        }
        

        private void CreateSheet(List<v_UserPayOrder> userpayorderlist  , HSSFWorkbook book, string tmpTitle)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            //style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            //style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            headerrow.CreateCell(0).SetCellValue("订单号");
            headerrow.CreateCell(1).SetCellValue("应用名称");
            headerrow.CreateCell(2).SetCellValue("用户名称");
            headerrow.CreateCell(3).SetCellValue("用户手机号");
            headerrow.CreateCell(4).SetCellValue("支付方式");
            headerrow.CreateCell(5).SetCellValue("支付金额");
            headerrow.CreateCell(6).SetCellValue("状态");
            headerrow.CreateCell(7).SetCellValue("创建时间");
            headerrow.CreateCell(8).SetCellValue("支付时间");
            headerrow.CreateCell(9).SetCellValue("订单总价");
            headerrow.CreateCell(10).SetCellValue("优惠");
            headerrow.CreateCell(11).SetCellValue("商品");

            for (int i = 0; i < userpayorderlist.Count ; i++)
            {
                v_UserPayOrder toinfo = userpayorderlist[i];
                IRow row = sheet.CreateRow(i + 1);      //新创建一行
                ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                cell.CellStyle = style;                 //设置单元格格式
                row.CreateCell(0).SetCellValue(toinfo.OrderID);
                row.CreateCell(1).SetCellValue(toinfo.AppName);
                row.CreateCell(2).SetCellValue(toinfo.UserName);
                row.CreateCell(3).SetCellValue(toinfo.UserPhone);
                row.CreateCell(4).SetCellValue(CBSS.Core.Utility.EnumHelper.GetEnumDesc<CBSS.Framework.Contract.Enums.PayWayEnum>(toinfo.PayWay));
                row.CreateCell(5).SetCellValue(toinfo.PayMoney);
                row.CreateCell(6).SetCellValue(CBSS.Core.Utility.EnumHelper.GetEnumDesc<CBSS.Framework.Contract.Enums.PayStatusEnum>(toinfo.Status));
                row.CreateCell(7).SetCellValue(toinfo.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(8).SetCellValue(toinfo.PayDate.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(9).SetCellValue(toinfo.TotalPrice);
                row.CreateCell(10).SetCellValue(toinfo.PreferentialPrice);
                row.CreateCell(11).SetCellValue(toinfo.GoodNames);
            }
        }

    }
}

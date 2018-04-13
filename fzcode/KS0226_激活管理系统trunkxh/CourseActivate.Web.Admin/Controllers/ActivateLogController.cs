using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CourseActivate.Account.Constract.Models;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Web.Admin.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using CourseActivate.Account.Constract.VW;
using NPOI.XSSF.UserModel;

namespace CourseActivate.Web.Admin.Controllers
{
    public class ActivateLogController : BaseController
    {

        //
        // GET: /ActivateLog/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ActivateLogDetails(string activatecode = "")
        {
            ViewBag.test = activatecode;
            return View();
        }
        //

        public JsonResult GetBatchActivateInfo(int pagesize, int pageindex, DateTime? startdate, DateTime? enddate, string batch = "", string activate = "", string buyway = "")
        {
            string where = "1=1";
            if (startdate != null)
            {
                where += " AND a.createtime>='" + startdate.Value.ToShortDateString() + " 0:00:00'";
            }
            if (enddate != null)
            {
                where += " AND a.createtime<='" + enddate.Value.ToShortDateString() + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(batch))
            {
                where += " AND c.batchcode='" + batch + "'";
            }
            if (!string.IsNullOrEmpty(activate))
            {
                where += " AND a.activatecode='" + activate + "'";
            }

            if (!string.IsNullOrEmpty(buyway) && buyway != "0")
            {
                where += " AND d.way=" + buyway;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PageIndex", pageindex);
            dic.Add("PageCount", pagesize);
            dic.Add("Where", where);
            IList<BatchActivateInfo> bc = ExecuteProcedure<BatchActivateInfo>("Get_BatchActivateInfo", dic);

            if (bc.Count > 0)
            {
                return Json(new { total = bc[0].TotalNum, rows = bc });
            }
            else
            {
                return Json(new { total = 0, rows = bc });
            }
        }


        public JsonResult GetBatchActivateUseInfo(string activatecode = "")
        {
            string where = "1=1";
            if (!string.IsNullOrEmpty(activatecode))
            {
                where += " AND activatecode='" + activatecode + "'";
            }
            IList<V_activaterecord> bc = SelectWhere<V_activaterecord>(where);

            if (bc.Count > 0)
            {
                return Json(new { total = bc.Count, rows = bc });
            }
            else
            {
                return Json(new { total = 0, rows = bc });
            }
        }

        #region 导出
        public FileResult Employee_Export(int pagesize, int pageindex, DateTime? startdate, DateTime? enddate, string batch = "", string activate = "", string buyway = "")
        {
            string where = "1=1";
            if (startdate != null)
            {
                where += " AND a.createtime>='" + startdate.Value.ToShortDateString() + " 0:00:00'";
            }
            if (enddate != null)
            {
                where += " AND a.createtime<='" + enddate.Value.ToShortDateString() + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(batch))
            {
                where += " AND c.batchcode='" + batch + "'";
            }
            if (!string.IsNullOrEmpty(activate))
            {
                where += " AND a.activatecode='" + activate + "'";
            }

            if (!string.IsNullOrEmpty(buyway) && buyway != "0")
            {
                where += " AND d.way=" + buyway;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PageIndex", 0);
            dic.Add("PageCount", 100000);
            dic.Add("Where", where);
            IList<BatchActivateInfo> bc = ExecuteProcedure<BatchActivateInfo>("Get_BatchActivateInfo", dic);

            XSSFWorkbook book = new XSSFWorkbook();//创建工作簿
            string tmpTitle = "导出激活记录" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("首次激活时间");
            headerrow.CreateCell(2).SetCellValue("批次号");
            headerrow.CreateCell(3).SetCellValue("激活课程");
            headerrow.CreateCell(4).SetCellValue("激活码类型");
            headerrow.CreateCell(5).SetCellValue("出版社");
            headerrow.CreateCell(6).SetCellValue("激活码");
            headerrow.CreateCell(7).SetCellValue("用户");
            headerrow.CreateCell(8).SetCellValue("已激活设备数");
            for (int i = 0; i < bc.Count; i++)
            {
                //com_master userinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                //ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                //cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);                  //给单元格赋值
                var cell = row.CreateCell(1);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(bc[i].createtime.ToString("yyyy-MM-dd HH:mm:ss"));
                row.CreateCell(2).SetCellValue(bc[i].activatecode.Substring(0, 3));
                row.CreateCell(3).SetCellValue(bc[i].BookName);
                row.CreateCell(4).SetCellValue(bc[i].activatetypename);
                row.CreateCell(5).SetCellValue(bc[i].publishname);
                row.CreateCell(6).SetCellValue(bc[i].activatecode);
                row.CreateCell(7).SetCellValue(bc[i].username);
                row.CreateCell(8).SetCellValue(bc[i].usenum);
            }
            NpoiMemoryStream ms = new NpoiMemoryStream();
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
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", tmpTitle + ".xlsx");

        }

        #endregion

    }

    public class NpoiMemoryStream : MemoryStream
    {
        public NpoiMemoryStream()
        {
            AllowClose = false;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}
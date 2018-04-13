using CourseActivate.Account.Constract.VW;
using CourseActivate.Activate.Constract.Model;
using CourseActivate.Core.Utility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CourseActivate.Web.Admin.Controllers
{
    public class StatisticMgrController : BaseController
    {
        // GET: StatisticMgr
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Statisticactivate()
        {
            return View();
        }

        public ActionResult Statisticrecord()
        {
            return View();
        }
        /// <summary>
        /// 页面1查询
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public JsonResult StatisticMgr_View(int pagesize, int pageindex, int statge, int grade, int edition, int subject, int bookreel, int status)
        {
            List<Expression<Func<V_StatisticBook, bool>>> exprlist = GetbookWheres(statge, grade, edition, subject, bookreel, status);
            PageParameter<V_StatisticBook> pageParameter = new PageParameter<V_StatisticBook>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.bookid;
            pageParameter.IsOrderByASC = 1;
            pageParameter.Wheres = exprlist;
            int total;
            IList<V_StatisticBook> usre = base.Manage.SelectPage<V_StatisticBook>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }
        /// <summary>
        /// 页面2查询
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public JsonResult Statisticactivate_View(int pagesize, int pageindex, int bookid, DateTime startdate, DateTime enddate, string batchcode)
        {
            List<Expression<Func<V_Statisticactivate, bool>>> exprlist = GetactivateWheres(bookid, startdate, enddate, batchcode);
            PageParameter<V_Statisticactivate> pageParameter = new PageParameter<V_Statisticactivate>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.bookid;
            pageParameter.IsOrderByASC = 0;
            pageParameter.Wheres = exprlist;
            int total;
            IList<V_Statisticactivate> usre = base.Manage.SelectPage<V_Statisticactivate>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }
        /// <summary>
        /// 页面3查询
        /// </summary>
        /// <returns></returns>
        public JsonResult Statisticrecord_View(int pagesize, int pageindex, Guid activateuseid)
        {
            PageParameter<tb_batchactivateusedevice> pageParameter = new PageParameter<tb_batchactivateusedevice>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            pageParameter.OrderColumns = t1 => t1.createtime;
            pageParameter.IsOrderByASC = 0;
            pageParameter.WhereSql = "activateuseid='" + activateuseid + "'";
            int total;
            IList<tb_batchactivateusedevice> usre = base.Manage.SelectPage<tb_batchactivateusedevice>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }

        #region 获取查询条件
        /// <summary>
        /// 获取查询条件 页面1
        /// </summary>
        /// <returns></returns>
        public List<Expression<Func<V_StatisticBook, bool>>> GetbookWheres(int statge, int grade, int edition, int subject, int bookreel, int status)
        {

            List<Expression<Func<V_StatisticBook, bool>>> exprlist = new List<Expression<Func<V_StatisticBook, bool>>>();
            if (statge != -1) exprlist.Add(i => i.PeriodID == statge);
            if (grade != -1) exprlist.Add(i => i.GradeID == grade);
            if (edition != -1) exprlist.Add(i => i.EditionID == edition);
            if (subject != -1) exprlist.Add(i => i.SubjectID == subject);
            if (bookreel != -1) exprlist.Add(i => i.ReelID == bookreel);
            if (status != -1) exprlist.Add(i => i.Status == status);
            return exprlist;
        }
        /// <summary>
        /// 页面2
        /// </summary>
        /// <param name="bookid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="batchcode"></param>
        /// <returns></returns>
        public List<Expression<Func<V_Statisticactivate, bool>>> GetactivateWheres(int bookid, DateTime startdate, DateTime enddate, string batchcode)
        {
            List<Expression<Func<V_Statisticactivate, bool>>> exprlist = new List<Expression<Func<V_Statisticactivate, bool>>>();
            exprlist.Add(i => i.bookid == bookid);
            exprlist.Add(i => i.createtime > startdate && i.createtime < enddate);
            //exprlist.Add(i => i.createtime < enddate);
            if (batchcode != "") exprlist.Add(i => i.batchcode == batchcode);
            return exprlist;
        }

        public List<Expression<Func<V_Statisticactivate, bool>>> GetrecordWheres(string activatecode)
        {

            List<Expression<Func<V_Statisticactivate, bool>>> exprlist = new List<Expression<Func<V_Statisticactivate, bool>>>();
            exprlist.Add(i => i.activatecode == activatecode);
            return exprlist;
        }
        #endregion

        #region 导出
        public FileResult book_Export(int statge, int grade, int edition, int subject, int bookreel, int status)
        {
            List<Expression<Func<V_StatisticBook, bool>>> exprlist = GetbookWheres(statge, grade, edition, subject, bookreel, status);
            IList<V_StatisticBook> bc = Manage.SelectSearchs<V_StatisticBook>(exprlist);
            bc = bc.Take(100000).ToList();
            XSSFWorkbook book = new XSSFWorkbook();//创建工作簿
            string tmpTitle = "导出书本信息" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("序号");
            headerrow.CreateCell(1).SetCellValue("书本名称");
            headerrow.CreateCell(2).SetCellValue("书本id");
            headerrow.CreateCell(3).SetCellValue("激活码总数");
            headerrow.CreateCell(4).SetCellValue("已激活");
            headerrow.CreateCell(5).SetCellValue("状态");
            for (int i = 0; i < bc.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                //ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                //cell.CellStyle = style;        //设置单元格格式
                //row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(0).SetCellValue(bc[i].bookrecordid);
                row.CreateCell(1).SetCellValue(bc[i].BookName);
                row.CreateCell(2).SetCellValue(bc[i].bookid);
                row.CreateCell(3).SetCellValue(bc[i].num);
                row.CreateCell(4).SetCellValue(bc[i].usenum);
                if (bc[i].Status == 0)
                {
                    row.CreateCell(5).SetCellValue("未启用");
                }
                else if (bc[i].Status == 1)
                {
                    row.CreateCell(5).SetCellValue("启用");
                }
                else
                {
                    row.CreateCell(5).SetCellValue("禁用");
                }
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
        public FileResult activate_Export(int bookid, DateTime startdate, DateTime enddate, string batchcode)
        {
            List<Expression<Func<V_Statisticactivate, bool>>> exprlist = GetactivateWheres(bookid, startdate, enddate, batchcode);
            IList<V_Statisticactivate> bc = Manage.SelectSearchs<V_Statisticactivate>(exprlist);
            bc = bc.Take(100000).ToList();
            XSSFWorkbook book = new XSSFWorkbook();//创建工作簿
            string tmpTitle = "导出激活记录" + DateTime.Now.ToString("yyyy-MM-dd");
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("批次号");
            headerrow.CreateCell(1).SetCellValue("激活码");
            headerrow.CreateCell(2).SetCellValue("用户");
            headerrow.CreateCell(3).SetCellValue("已激活设备数");
            headerrow.CreateCell(4).SetCellValue("首次激活时间");
            for (int i = 0; i < bc.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                //ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                // cell.CellStyle = style;        //设置单元格格式
                //row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(0).SetCellValue(bc[i].batchcode);
                row.CreateCell(1).SetCellValue(bc[i].activatecode);
                row.CreateCell(2).SetCellValue(bc[i].username);
                row.CreateCell(3).SetCellValue(bc[i].usenum);
                var cell = row.CreateCell(4);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(bc[i].createtime.ToString("yyyy-MM-dd HH:mm:ss"));
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
        #endregion
    }
}
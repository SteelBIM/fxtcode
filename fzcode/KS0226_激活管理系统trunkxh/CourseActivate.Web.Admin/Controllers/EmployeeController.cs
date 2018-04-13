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
using CourseActivate.Core.Utility;
using CourseActivate.Account.Constract.VW;
using CourseActivate.Web.Admin.Models;
using CourseActivate.Account.Constract.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace CourseActivate.Web.Admin.Controllers
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
            IList<com_group> role = Manage.SelectSearch<com_group>("delflg=0");
            ViewBag.role = role;
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


        #region 判断用户中是否存在 UserNameIsExist(int UserId, string UserName)

        /// <summary>
        /// 判断用户中是否存在
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        public int UserNameIsExist(int UserId, string UserName)
        {
            if (UserId > 0)
                return base.GetTotalCount<com_master>(x => (x.mastername == UserName && x.masterid != UserId));

            return base.GetTotalCount<com_master>(x => (x.mastername == UserName));
        }
        #endregion

        [HttpPost]
        public JsonResult GetAllGroup()
        {
            return Json(base.SelectSearch<com_group>(x => x.delflg == 0));
        }
        public JsonResult GetMasterById(string Id)
        {
            return Json(base.Select<com_master>(Id));
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
        public JsonResult Employee_Add(string jsondata)
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
                string mastername = masterdata.mastername;//用户名
                masterdata.mastertype = 1;
                masterdata.createid = masterinfo.masterid;
                masterdata.password = PublicHelp.pswToSecurity(masterdata.password);//mima
                masterdata.createtime = DateTime.Now.ToString();//创建时间
                masterdata.updatetime = DateTime.Now.ToString();
                int masterid = base.Add<com_master>(masterdata);//添加用户
                if (masterid > 0)
                {
                    res.Success = true;
                    return Json(res);
                }
                else
                {
                    res.ErrorMsg = "添加失败！请重试~";
                }
            }
            return Json(res);
        }
        #endregion


        #region 编辑用户 Employee_Edit(string jsondata, string Areas)
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="Areas"></param>
        /// <param name="ProductIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Employee_Edit(string mastername, string mobile, string email, string agent_remark, int issend, int groupid)
        {
            Core.Utility.KingResponse res = new KingResponse();
            if (!action.Edit)
                res.ErrorMsg = "您没有操作权限~";
            else
            {

                var obj = new { mastername = mastername, mobile = mobile, email = email, remark = agent_remark, issend = issend, updatetime = DateTime.Now, groupid = groupid };
                if (base.Update<com_master>(obj, t => t.mastername == mastername))
                {
                    res.Success = true;
                }
                else
                {
                    res.ErrorMsg = "编辑失败！请重试~";
                }
            }
            return Json(res);
        }
        #endregion

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
        public JsonResult Employee_View(int pagesize, int pageindex, int deptid, int type)
        {

            if (!action.View) //没有预览权限
                return Json("");
            PageParameter<com_master> pageParameter = new PageParameter<com_master>();
            pageParameter.PageIndex = setpageindex(pageindex, pagesize);
            pageParameter.PageSize = pagesize;
            //mastername = mastername.Trim();
            //List<Expression<Func<com_master, bool>>> exprlist = GetUserWheres(mastername);
            //pageParameter.Wheres = exprlist;
            pageParameter.WhereSql = "mastername != 'admin'";
            pageParameter.OrderColumns = t1 => t1.masterid;
            pageParameter.IsOrderByASC = 0;
            int total;
            IList<com_master> usre = base.Manage.SelectPage<com_master>(pageParameter, out total);
            return Json(new { total = total, rows = usre });
        }

        #endregion

        #region 导出
        [HttpPost]
        public FileResult Employee_Export([System.Web.Http.FromBody]com_master masterInfo)
        {
            string Flids = "";
            List<string> InIds = null;
            List<Expression<Func<com_master, bool>>> exprlist = GetUserWheres(masterInfo.mastername);
            List<com_master> list = base.SelectSearchs<com_master>(exprlist, Flids, InIds, " masterid desc");
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "财富分账系统导出用户" + DateTime.Now.ToString("yyyy-MM-dd");
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
            headerrow.CreateCell(6).SetCellValue("状态");
            for (int i = 0; i < list.Count; i++)
            {
                com_master userinfo = list[i];
                IRow row = sheet.CreateRow(i + 1);               //新创建一行
                ICell cell = row.CreateCell(i);         //在新创建的一行中创建单元格
                cell.CellStyle = style;        //设置单元格格式
                row.CreateCell(0).SetCellValue(i + 1);        //给单元格赋值
                row.CreateCell(1).SetCellValue(userinfo.mastername);
                row.CreateCell(2).SetCellValue(userinfo.truename);
                row.CreateCell(3).SetCellValue("");
                row.CreateCell(4).SetCellValue("");
                row.CreateCell(5).SetCellValue("");
                row.CreateCell(6).SetCellValue(UserState(userinfo.state));
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
        public string UserState(int state)
        {
            //用户状态
            var reust = "";
            switch (state)
            {
                case 0:
                    reust = "正常";
                    break;
                case 1:
                    reust = "已锁定";
                    break;
                case 2:
                    reust = "已拉黑";
                    break;
                case 3:
                    reust = "已删除";
                    break;

            }
            return reust;
        }

        #endregion

        #region 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="mastername"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public List<Expression<Func<com_master, bool>>> GetUserWheres(string mastername)
        {

            List<Expression<Func<com_master, bool>>> exprlist = new List<Expression<Func<com_master, bool>>>();
            if (!string.IsNullOrEmpty(mastername))
            {
                exprlist.Add(i => i.mastername == mastername);
            }


            return exprlist;
        }
        #endregion

        public JsonResult Employee_Delete(int masterid)
        {
            KingResponse res = new KingResponse();
            if (Manage.Delete<com_master>(masterid))
            {
                return Json(KingResponse.GetResponse("删除成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("删除失败"));
            }
        }

    }
}


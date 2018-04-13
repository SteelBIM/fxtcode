using System;
using System.Collections.Generic;
//using Kingsun.SunnyTask.BLL;
//using Kingsun.SunnyTask.Common;
//using Kingsun.SunnyTask.Model;
//using Kingsun.SunnyTask.BLL.RedisBLL;
using Kingsun.ExamPaper.BLL.ImportTool;
using Kingsun.ExamPaper.BLL;
using Kingsun.ExamPaper.Model;
using System.Linq;

namespace Kingsun.SunnyTask.Web.Student
{
    public partial class StuTaskList : BasePage
    {        
        CatalogBLL catalogBLL = new CatalogBLL();
        public string UserId = "156841071";
        public string BookID = "186";
        public List<V_Catalog> Catalogs { get; set; }
        //拼接学生学期列表
        public string strSemesterList = "";
        protected void Page_Load(object sender, EventArgs e)
        {       
            if (!IsPostBack)
            {

                UserId =Convert.ToString(Session["userid"]) ?? UserId;
                BookID = Request.QueryString["bookid"] ?? BookID;
               
                Response.Cookies["UserId"].Value=UserId;
                Response.Cookies["BookID"].Value = BookID;
                string strWhere = " 1=1 ";
                int totalCount = 0;
                int totalPages = 0;
                IList<V_Catalog> catalogs = catalogBLL.GetCatalogPageList(1, int.MaxValue, strWhere + string.Format(" and CatalogID is not NULL and IsRemove=0  and BookID={0} ",BookID), "EditionID,SubjectID,GradeID,BookReel,BookID,Sort", 1, out totalCount, out totalPages);
                Catalogs = catalogs.ToList();
            }
        }

        private void ActionPage(string action)
        {
            switch (action)
            {
                case "CheckSecurityPhone":
                    //if (!string.IsNullOrEmpty(CurrentUserInfo.SecurityPhone))
                    //{
                    //    WriteResult("");
                    //}
                    //else
                    //{
                    //    WriteErrorResult("未填密保手机号哦！");
                    //}
                    break;
                case "SendVerifyCode":
                    //if (!string.IsNullOrEmpty(Request.Form["SecurityPhone"]))
                    //{
                    //    string phone = Request.Form["SecurityPhone"];
                    //    CacheHelper.Remove(phone);
                    //    //生成4位随机验证码
                    //    string verifyCode = (new Random().Next(99, 9999)).ToString().PadLeft(4, '0');
                    //    if (new SMS_MsgBLL().SendMessage(AppSetting.MessageToken, phone, "【方直科技金太阳】验证码：" + verifyCode + "，如非本人操作，请忽略本短信。"))
                    //    {
                    //        CacheHelper.Insert(phone, verifyCode);
                    //        WriteResult("", "验证码已发送，请注意查收！");
                    //    }
                    //    else
                    //    {
                    //        WriteErrorResult("验证码发送失败，请检查手机号！");
                    //    }
                    //}
                    //else
                    //{
                    //    WriteErrorResult("未获取到填写的手机号哦！");
                    //}
                    break;
                case "VerifyAndBind":
                    //if (!string.IsNullOrEmpty(Request.Form["SecurityPhone"]) && !string.IsNullOrEmpty(Request.Form["VerifyCode"]))
                    //{
                    //    string phone = Request.Form["SecurityPhone"];
                    //    string verifyCode = Request.Form["VerifyCode"];
                    //    if (!verifyCode.Equals(CacheHelper.GetCache(phone)))
                    //    {
                    //        WriteErrorResult("验证码错误啦！");
                    //    }
                    //    else
                    //    {
                    //        //绑定密保手机号
                    //        string msg = new UUMS_UserBLL().AddUserBindPhone(CurrentUserInfo.UserID, phone);
                    //        if (string.IsNullOrEmpty(msg))
                    //        {
                    //            WriteResult("");
                    //        }
                    //        else
                    //        {
                    //            WriteErrorResult(msg);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    WriteErrorResult("未获取到填写的手机号和验证码哦！");
                    //}
                    break;
                case "GetStuTaskList": //根学生ID,学科 查询学生作业列表，按截止时间逆序排序
                    string semester = Request.Form["Semester"].ToString();
                    string stuTaskState = Request.Form["StuTaskState"].ToString();
                    string PageIndex = Request.Form["PageIndex"];//当前页码
                    string PageSize = Request.Form["PageSize"];  //每页显示数
                    string subjectID = Request.Form["SubjectID"];
                    int totalCount = 0, totalPages = 0;
                    string strWhere = " 1=1 ";
                    IList<V_Catalog> catalogs = catalogBLL.GetCatalogPageList(int.Parse(PageIndex), int.Parse(PageSize), strWhere + " and CatalogID is not NULL and IsRemove=0  and BookID=186 ", "EditionID,SubjectID,GradeID,BookReel,BookID,Sort", 1, out totalCount, out totalPages);

                    WriteResult(new { TotalCount = totalCount, TotalPages = totalPages, StuTaskList = catalogs });
                    break;
                case "UpdateStuTask"://更新学生任务
                    //string stuTakID = Request.Form["StuTaskID"];
                    //if (!String.IsNullOrEmpty(stuTakID))
                    //{
                    //    //查询学生作业记录
                    //    Tb_StudentTask stuTask = stuTaskBLL.GetStuTaskByID(stuTakID);
                    //    if (stuTask != null)
                    //    {
                    //        if (String.IsNullOrEmpty(stuTask.DoDate))
                    //        {
                    //            stuTask.DoDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //            if (stuTaskBLL.UpdateStuTask(stuTask))
                    //            {
                    //                new R_StuTaskBLL().SetRStuTask(stuTakID, Request.Form["TaskID"], CurrentUserInfo.UserID, CurrentUserInfo.TrueName);
                    //                WriteResult("");
                    //            }
                    //            else
                    //            {
                    //                WriteErrorResult("更新学生任务失败！");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            WriteResult("");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        WriteErrorResult("未获取到学生作业哦！");
                    //    }
                    //}
                    //else
                    //{
                    //    WriteErrorResult("未获取到作业哦！");
                    //}
                    break;
                case "ClearCodeCookie":
                    //if (!string.IsNullOrEmpty(Request.Form["SecurityPhone"]))
                    //{
                    //    string phone = Request.Form["SecurityPhone"];
                    //    CacheHelper.Remove(phone);
                    //    WriteResult("");
                    //}
                    //else
                    //{
                    //    WriteErrorResult("未成功清除缓存信息！");
                    //}
                    break;
                default:
                    WriteResult("");
                    break;
            }
        }
    }
}
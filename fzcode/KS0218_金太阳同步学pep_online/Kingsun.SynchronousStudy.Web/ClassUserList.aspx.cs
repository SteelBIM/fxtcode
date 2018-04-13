using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.Web.Models.UserManagement;

namespace Kingsun.SynchronousStudy.Web
{
    public partial class ClassUserList : System.Web.UI.Page
    {

        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        public string appid = WebConfigurationManager.AppSettings["AppID"];
        public string teacherid = "";
        public string cid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            teacherid = Request.QueryString["TeacherID"];
            cid = Request.QueryString["ClassID"];
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            string classid = string.Empty;
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(teacherid))
            {
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(teacherid));
                if (user != null) 
                {
                    user.ClassSchList.ForEach(a => 
                    {
                        var classInfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                        if(classInfo!=null){
                            list.Add(classInfo.ClassNum.ToString());
                        }
                    });
                }
            }
            List<ClassInfoToWeb> returnList = new List<ClassInfoToWeb>();
            var classinfo = classBLL.SearchALL();
            if (classinfo != null) 
            {
                classinfo.ForEach(a => 
                {
                    
                    a.ClassStuList.ForEach(x => 
                    {
                        ClassInfoToWeb webclass = new ClassInfoToWeb();
                        webclass.ID = a.ClassID;
                        webclass.ClassShortID = a.ClassNum.ToString();
                        webclass.ClassName = a.ClassName;
                        var user=userBLL.GetUserInfoByUserId(Convert.ToInt32(x.StuID));
                        if(user!=null)
                        {
                            webclass.UserName = user.UserName;
                            webclass.TelePhone = user.TelePhone;
                        }
                        webclass.TrueName = x.StuName;
                        if (!returnList.Contains(webclass))
                        {

                            returnList.Add(webclass);
                        }
                    });

                    a.ClassTchList.ForEach(x =>
                    {
                        ClassInfoToWeb webclass = new ClassInfoToWeb();
                        webclass.ID = a.ClassID;
                        webclass.ClassShortID = a.ClassNum.ToString();
                        webclass.ClassName = a.ClassName;
                        var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(x.TchID));
                        if (user != null)
                        {
                            webclass.UserName = user.UserName;
                            webclass.TelePhone = user.TelePhone;
                        }
                        webclass.TrueName = x.TchName;
                        if (!returnList.Contains(webclass)) 
                        {

                            returnList.Add(webclass);
                        }
                    });
                });
            }

            if (!string.IsNullOrEmpty(cid))
            {
                classid = cid;
            }




            if (!string.IsNullOrEmpty(classid.Trim()))
            {

                var re=returnList.Where(a => list.Contains(a.ClassShortID));
                if (re != null)
                {
                    returnList = re.ToList();
                }
                else 
                {
                    returnList = new List<ClassInfoToWeb>();
                }
            }
            if (txtTelephone.Text.Trim() != "")
            {
                var rel=returnList.Where(a=>a.TelePhone.StartsWith(txtTelephone.Text.Trim()));
                if (rel != null)
                {
                    returnList = rel.ToList();
                }
                else 
                {
                    returnList = new List<ClassInfoToWeb>();
                }
            }

            if (returnList.Count > 0) 
            {
                returnList = QueryByPage(AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, returnList);
                AspNetPager1.RecordCount =returnList.Count;

                rpClassUserList.DataSource = returnList;
                rpClassUserList.DataBind();
            }
        }

        protected List<ClassInfoToWeb> QueryByPage(int PageSize, int CurPage, IList<ClassInfoToWeb> objs)
        {
             var query = from cms_contents in objs select cms_contents;
             return query.Take(PageSize * CurPage).Skip(PageSize * (CurPage - 1)).ToList(); 
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary> 
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    case '\'':
                        sb.Append("\'\'"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        protected void rpClassUserList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}
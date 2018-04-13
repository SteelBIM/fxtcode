using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using CourseActivate.Core.Utility;
using CourseActivate.Framework.BLL;
using CourseActivate.Web.Admin.Models;
using CourseActivate.Account.Constract.Models;
using System.Linq.Expressions;
using CourseActivate.Account.Constract.VW;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using CourseActivate.Activate.BLL;

namespace CourseActivate.Web.Admin.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 用户身份 0本公司 1代理商
        /// </summary>
        public int UserIdentity = 0;
        protected Jurisdiction action = new Jurisdiction();
        protected com_master masterinfo = new com_master();
        public static List<vw_action> vw_actionlist;

        public bool IsExclusion(int brotherDistrictid, int needAddDistrictid)
        {
            string strBrotherDistrictid = brotherDistrictid.ToString();
            string strNeedAddDistrictid = needAddDistrictid.ToString();

            if (strNeedAddDistrictid == strBrotherDistrictid)//街道（假设最小可能）
            {
                return true;
            }
            else
            {
                if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 6) + "000"))//区
                {
                    return true;
                }
                else
                {
                    if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 4) + "00000"))//市
                    {
                        return true;
                    }
                    else
                    {
                        if (strNeedAddDistrictid == (strBrotherDistrictid.Substring(0, 2) + "0000000"))//省
                        {
                            return true;
                        }
                        else
                        {
                            if (strNeedAddDistrictid == "0")//国
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 执行控制器方法之前先执行该方法
        /// 获取自定义的Session的
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (vw_actionlist == null &&  CookieHelper.GetCookie("LoginInfo")!=null)
            {
                var masterinfo = JsonHelper.JsonDeserialize<com_master>(CookieHelper.GetCookie("LoginInfo"));

                List<vw_action> action = new List<vw_action>();
                if (masterinfo.groupid == 0)
                {
                    List<vw_allaction> allaction = manage.SelectAll<vw_allaction>();
                    for (int i = 0; i < allaction.Count; i++)
                    {
                        action.Add(allaction[i]);
                    }
                }
                else
                    action = manage.SelectSearch<vw_action>(x => x.groupid == masterinfo.groupid, 10000, " parentsequence,sequence ");

                if (action != null && action.Count > 0)
                {
                    if (masterinfo.groupid == 0)
                        masterinfo.dataauthority = 0;
                    else
                    {
                        com_group comgroup = manage.Select<com_group>(masterinfo.groupid.ToString());
                        if (comgroup != null)
                            masterinfo.dataauthority = comgroup.dataauthority;
                    }
                    vw_actionlist = action.OrderBy(i => i.parentsequence).ThenBy(i => i.sequence).ToList();                    
                }
            }
            CookieHelper.SetCookie("ActionInfo", JsonHelper.ToJson(vw_actionlist), 7);
            if (CookieHelper.GetCookie("ActionInfo") != null && CookieHelper.GetCookie("LoginInfo") != null)
            {
                masterinfo = JsonHelper.JsonDeserialize<com_master>(CookieHelper.GetCookie("LoginInfo"));
                string CurrentController = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();//当前Controller
                //string CurrentAction = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();//当前action
                //bool IsHaveAction = false;//是否拥有当前action权限
                List<vw_action> list = JsonHelper.JSONStringToList<vw_action>(CookieHelper.GetCookie("ActionInfo"));
                if (list != null && list.Count > 0)
                {
                    #region 获取权限
                    foreach (vw_action item in list)
                    {
                        if (item.actionurl == CurrentController)
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
            base.OnActionExecuting(filterContext);
        }

        public string InFormat(List<string> param)
        {
            StringBuilder sb = new StringBuilder();
            if (param.Count > 0)
            {
                sb.AppendFormat("(");
                int index = 0;
                for (int i = 0; i < param.Count; i++)
                {
                    if (!string.IsNullOrEmpty(param[i]) && index == 0)
                    {
                        sb.AppendFormat("'{0}'", param[i]);
                        index += 1;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(param[i]))
                        {
                            sb.AppendFormat(",'{0}'", param[i]);
                        }
                    }
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        Manage manage = new Manage();

        public Manage Manage
        {
            get { return manage; }
        }

      
        public int Add<T>(T subdata, string[] array = null) where T : class, new()
        {
            return manage.Add<T>(subdata, array);
        }
        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities) where T : class, new()
        {
            return manage.InsertRange<T>(entities);
        }
        public bool Update<T>(T subdata) where T : class, new()
        {
            return manage.Update<T>(subdata);
        }
        public bool Update<T>(T info, string[] disableUpdateCoulums) where T : class, new()
        {
            return manage.Update<T>(info, disableUpdateCoulums);
        }
        public bool Update<T>(object obj, Expression<Func<T, bool>> expr) where T : class,new()
        {
            return manage.Update<T>(obj, expr);
        }
        /// <summary>
        /// 批量删除<T>(物理删除<T>(1,2,3) where T : class, new()) where T : class, new()
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteMore<T>(string Ids) where T : class, new()
        {
            return manage.DeleteMore<T>(Ids);
        }
        /// <summary>
        /// 物理删除根据ID删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteById<T>(object Id) where T : class, new()
        {
            return manage.Delete<T>(Id);
        }
        public bool Delete<T>(Expression<Func<T, bool>> expr) where T : class, new()
        {
            return manage.Delete<T>(expr); ;
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields) where T : class, new()
        {
            return manage.SelectGroupBy(expression, groupbyfields);
        }
        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where T : class, new()
        {
            return manage.SelectGroupBy<T>(expression, groupbyfields, Flide, Ids);
        }
        public List<T> SelectGroupBy<T>(string groupbyfield) where T : class, new()
        {
            return manage.SelectGroupBy<T>(groupbyfield);
        }

        /// <summary>
        /// 批量将指定字段修改成true
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool LogicDeleteMore<T>(string Ids, string filed) where T : class, new()
        {
            return manage.LogicDeleteMore<T>(Ids, filed);
        }
        /// <summary>
        /// 将指定字段修改成true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool LogicDelete<T>(Expression<Func<T, bool>> expr, string field) where T : class, new()
        {
            return manage.LogicDelete<T>(expr, field);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> SelectAll<T>() where T : class, new()
        {
            return manage.SelectAll<T>();
        }
        /// <summary>
        /// in条件查询 
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(Expression<Func<T, bool>> expr, string Flide, List<string> Ins) where T : class, new()
        {
            return manage.SelectIn<T>(expr, Flide, Ins);
        }
        /// <summary>
        /// in条件查询 
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(string Flide, List<string> Ins) where T : class, new()
        {
            return manage.SelectIn<T>(Flide, Ins);
        }

        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> exprs, string Flids = "", List<string> InIds = null, string orderfile = "") where T : class, new()
        {
            return manage.SelectSearchs<T>(exprs, Flids, InIds, orderfile);
        }
        /// <summary>
        /// 搜索查询(多条件下使用 a=0 and b=1)
        /// </summary>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression, int topNumber, string orderby = "") where T : class, new()
        {
            return manage.SelectSearch<T>(expression, topNumber, orderby);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public List<T> SelectAppointField<T>(Expression<Func<T, bool>> expression, string Field) where T : class, new()
        {
            return manage.SelectAppointField<T>(expression, Field);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, string sqlwhere = "") where T : class, new()
        {
            return manage.GetTotalCount<T>(expression, sqlwhere);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(string sqlwhere = "") where T : class, new()
        {
            return manage.GetTotalCount<T>(sqlwhere);
        }
        public int GetTotalCount<T>(List<string> Ids, string Flide) where T : class, new()
        {
            return manage.GetTotalCount<T>(Ids, Flide);
        }
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            return manage.GetTotalCount<T>(expression, Ids, Flide);
        }
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Select<T>(object ID) where T : class, new()
        {
            return manage.Select<T>(ID);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<T> SelectWhere<T>(string whereSql) where T : class, new()
        {
            return manage.SelectWhere<T>(whereSql);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IList<T> SelectPage<T>(PageParameter<T> parameter, out int totalCount) where T : class, new()
        {
            return manage.SelectPage<T>(parameter, out totalCount);
        }
        /// <summary>
        /// 条件搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return manage.SelectSearch<T>(expression);
        }
        /// <summary>
        /// 设置页面的索引
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public int setpageindex(int index, int size)
        {
            if (index == 0)
                index = index / size;
            else
                index = index / size + 1;
            return index;
        }

        public bool TransactionAdd<T1, T2>(RelationEntity<T1, T2> relationEntity)
            where T1 : class, new()
            where T2 : class, new()
        {
            return manage.TransactionAdd<T1, T2>(relationEntity);
        }

        public DataTable SelectDataTable(string sql, object obj = null)
        {
            return manage.SelectDataTable(sql, obj);
        }

        public IList<T> ExecuteProcedure<T>(string procName, Dictionary<string, object> dis = null)
        {
            return manage.ExecuteProcedure<T>(procName, dis);
        }

    }
}
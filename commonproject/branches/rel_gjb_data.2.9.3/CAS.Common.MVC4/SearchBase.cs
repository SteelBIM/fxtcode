using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;

namespace CAS.Common.MVC4
{
    /// <summary>
    /// 查询基类
    /// </summary>
    public class SearchBase
    {
        public SearchBase()
        {
            Parameters = new List<SqlParameter>();
        }
        private int cityId=0;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }

        private int areaId = 0;
        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaId
        {
            get { return areaId; }
            set { areaId = value; }
        }

        private int companyId=0;
        /// <summary>
        /// 客户公司ID
        /// </summary>
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        private int fxtCompanyId=0;
        /// <summary>
        /// 评估机构公司ID
        /// </summary>
        public int FxtCompanyId
        {
            get { return fxtCompanyId; }
            set { fxtCompanyId = value; }
        }
        private string dateBegin;
        /// <summary>
        /// 起始时间
        /// </summary>
        public string DateBegin 
        {
            get { return dateBegin; }
            set { dateBegin = value; }
        }
        private string dateEnd;
        /// <summary>
        /// 起始时间
        /// </summary>
        public string DateEnd
        {
            get { return dateEnd; }
            set { dateEnd = value; }
        }

        private int departmentId = 0;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        private int userId ;
        /// <summary>
        /// 登录ID
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userName;
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private bool isadmin = false;
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool isAdmin
        {
            get { return isadmin; }
            set { isadmin = value; }
        }
        private bool page = false;
        /// <summary>
        /// 是否要分页
        /// </summary>
        public bool Page
        {
            get { return page; }
            set { page = value; }
        }
        /// <summary>
        /// 分页的查询
        /// </summary>
        public string PageSelect(string sql){

            string select = string.Format(" declare @recordcount int \n select @recordcount= count(1)from({1})record  \n select * from ( select *,row_number() over(order by {0}) as rownum ,@recordcount recordcount from ({1}", orderBy, sql);
            return select;
            
        }

        private string sql;
        public string Sql 
        {
            get { return sql; }
            set { sql = value; }
        }

        public string PageSql {
            get {
                string pageSql = string.Format("select * from ( select *,row_number() over(order by {0}) as rownum ,(select count(1)from({1})RecordCount) recordcount from ({1}) a ) a where rownum between ({2}-1)*{3}+1 and {2}*{3}", orderBy, sql, pageIndex, pageRecords);
                return pageSql.ToLower();
            }
        }

        /// <summary>
        /// 分页的范围
        /// </summary>
        public string PageWhere
        {
            get
            {
                string where = string.Format(") a ) a where rownum between ({0}-1)*{1}+1 and {0}*{1} ", pageIndex, pageRecords);
                return where;
            }
        }

        private int pageIndex=0;
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        /// <summary>
        /// sql-top
        /// </summary>
        private int top;
        public int Top 
        {
            get { return top; }
            set { top = value;}
        }

        private int pageRecords=10;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageRecords
        {
            get { return pageRecords; }
            set { pageRecords = value; }
        }
        
        private string orderBy;
        /// <summary>
        /// 排序方式
        /// </summary>
        public string OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        private string where;
        /// <summary>
        /// 扩展查询条件
        /// </summary>
        public string Where
        {
            get { return where; }
            set { where = value; }
        }

        /// <summary>
        /// Where 属性对应的参数列表  
        /// </summary>
        public List<SqlParameter> Parameters { get; set; }

        private string key;
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 系统类型
        /// </summary>
        public int SysTypeCode { get; set; }

        public string TrueName { get; set; }

        public string MsgServer { get; set; }

        public string StrCode { get; set; }

        public string StrDate { get; set; }

        /// <summary>
        /// 查勘类型1031
        /// </summary>
        public int SurveyTypeCode { get; set; }

        /// <summary>
        /// 分支机构ID kevin 2013-4-16
        /// </summary>
        public int SubCompanyId { get; set; }
    }
}

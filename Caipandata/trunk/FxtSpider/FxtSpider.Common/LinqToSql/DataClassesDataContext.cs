using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Linq;
using System.Reflection;

namespace FxtSpider.Common.LinqToSql
{
    public partial class VIEW_案例信息_城市表_网站表
    {
        public static List<VIEW_案例信息_城市表_网站表> 案例信息_获取爬取数据_根据城市ID_创建时间区间([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> 城市ID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> 创建日期_开始时间, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> 创建日期_结束时间)
        {
            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 案例信息_获取爬取数据_根据城市ID_创建时间区间 {0},{1},{2}", new object[] { 城市ID, 创建日期_开始时间, 创建日期_结束时间 });
                list = result.ToList<VIEW_案例信息_城市表_网站表>();
            }
            return list;
        }
        public static List<VIEW_案例信息_城市表_网站表> 案例信息_获取爬取数据_根据城市ID_网站_创建时间区间([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> 城市ID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> 网站ID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> 创建日期_开始时间, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> 创建日期_结束时间)
        {

            List<VIEW_案例信息_城市表_网站表> list = new List<VIEW_案例信息_城市表_网站表>();
            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                var result = db.ExecuteQuery<VIEW_案例信息_城市表_网站表>("exec 案例信息_获取爬取数据_根据城市ID_网站_创建时间区间 {0},{1},{2},{3}", new object[] { 城市ID,网站ID, 创建日期_开始时间, 创建日期_结束时间 });
                list = result.ToList<VIEW_案例信息_城市表_网站表>();
            }
            return list;
        }
    }
    public partial class DataClassesDataContext
    {
        public DataClassesDataContext() :
            base(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {
            OnCreated();
        }
    }
}

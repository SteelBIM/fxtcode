using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtSpider.DAL.Manager
{
    public static class DBHelper
    {
        /// <summary>
        /// 用于封装分页sql
        /// </summary>
        const string PAGE_SQL = "select * from (select *,row_number() over(order by {0}) as rownumber  from ({1}) as tblpage1  ) as tblpage2 where rownumber between {2} and {3} ";
        /// <summary>
        /// 封装分页sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderBy">排序规则</param>
        /// <param name="pageIndex">当前页码,1开始</param>
        /// <param name="pageSize">每页多少条</param>
        /// <returns></returns>
        public static string GetPageSql(this string sql,string orderBy, int pageIndex, int pageSize)
        {
            int go = (pageIndex - 1) * pageSize + 1;
            int to = pageIndex * pageSize;
            string _sql = string.Format(PAGE_SQL, orderBy, sql, go, to);
            return _sql;

        }
    }
}

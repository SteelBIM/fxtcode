using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.Common
{
    public class PageParameter
    {
        /// <summary>
        /// 数据库中相关表表名
        /// </summary>
        public string TbNames { set; get; }

        /// <summary>
        /// 排序列名称,支持多列排序,例如ORDER BY column1,column2但是语句中不能还有ORDER BY关键字
        /// </summary>
        public string OrderColumns { set; get; }

        /// <summary>
        /// 数据库中相关表表列,默认为*
        /// </summary>
        public string Columns { set; get; }

        /// <summary>
        /// Where条件语句,不含有Where关键字
        /// </summary>
        public string Where { set; get; }

        /// <summary>
        /// 排序方式1:ASC,2:DESC  
        /// </summary>
        public int IsOrderByASC { set; get; }

        /// <summary>
        /// 当前分页页面数,如果程序是第一次使用则该值为1
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 程序需求每页显示的数据条数
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 数据库中总的页面数量
        /// </summary>
        public int TotalPages { set; get; }

        /// <summary>
        /// 数据库中总的记录数量
        /// </summary>
        public int TotalRecords { set; get; }

        public List<System.Data.Common.DbParameter> getParameterList()
        {
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            System.Data.SqlClient.SqlParameter[] param = new System.Data.SqlClient.SqlParameter[9];
            param[0] = new System.Data.SqlClient.SqlParameter("@Columns", System.Data.SqlDbType.NVarChar, 500);
            param[1] = new System.Data.SqlClient.SqlParameter("@TbNames", System.Data.SqlDbType.NVarChar, 200);
            param[2] = new System.Data.SqlClient.SqlParameter("@WhereCondition", System.Data.SqlDbType.NVarChar, 1500);
            param[3] = new System.Data.SqlClient.SqlParameter("@OrderColumns", System.Data.SqlDbType.NVarChar, 350);
            param[4] = new System.Data.SqlClient.SqlParameter("@IsOrderByASC", System.Data.SqlDbType.Int);
            param[5] = new System.Data.SqlClient.SqlParameter("@CurrentPageIndex", System.Data.SqlDbType.Int);
            param[6] = new System.Data.SqlClient.SqlParameter("@PageSize", System.Data.SqlDbType.Int);
            param[7] = new System.Data.SqlClient.SqlParameter("@TotalPages", System.Data.SqlDbType.Int);
            param[8] = new System.Data.SqlClient.SqlParameter("@TotalRecords", System.Data.SqlDbType.Int);
            param[0].Value = this.Columns;
            param[1].Value = this.TbNames;
            param[2].Value = this.Where;
            param[3].Value = this.OrderColumns;
            param[4].Value = this.IsOrderByASC;
            param[5].Value = this.PageIndex;
            param[6].Value = this.PageSize;
            param[7].Direction = System.Data.ParameterDirection.Output;
            param[8].Direction = System.Data.ParameterDirection.Output;
            list.AddRange(param);
            return list;


        }

    }
}

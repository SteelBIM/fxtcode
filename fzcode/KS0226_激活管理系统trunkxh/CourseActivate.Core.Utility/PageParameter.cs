using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;


namespace CourseActivate.Core.Utility
{
    public class PageParameter<T>
    {
        /// <summary>
        /// 排序列名称,支持多列排序,例如ORDER BY column1,column2但是语句中不能还有ORDER BY关键字
        /// </summary>
        public Expression<Func<T, object>> OrderColumns { set; get; }

        public string StrOrderColumns { set; get; }

        public Expression<Func<T, bool>> Where { get; set; }

        public List<Expression<Func<T, bool>>> Wheres { get; set; }

        /// <summary>
        /// 排序方式1:ASC,0:DESC 
        /// </summary>
        private int _isorderbyasc = 1;

        public int IsOrderByASC
        {
            set { _isorderbyasc = value; }
            get { return _isorderbyasc; }
        }

        /// <summary>
        /// 当前分页页面数,如果程序是第一次使用则该值为1
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 程序需求每页显示的数据条数
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 使用in查询的字段(没有条件不赋值)
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 使用in查询的条件(没有条件不赋值)
        /// </summary>
        public List<string> In { get; set; }

        public string WhereSql { get; set; }

    }
}

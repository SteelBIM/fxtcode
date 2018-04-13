using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>”的 XML 注释
    public class PageParameter<T>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>”的 XML 注释
    {
        /// <summary>
        /// 排序列名称,支持多列排序,例如ORDER BY column1,column2但是语句中不能还有ORDER BY关键字
        /// </summary>
        public Expression<Func<T, object>> OrderColumns { set; get; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.OrderColumns2”的 XML 注释
        public Expression<Func<T, object>> OrderColumns2 { set; get; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.OrderColumns2”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.StrOrderColumns”的 XML 注释
        public string StrOrderColumns { set; get; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.StrOrderColumns”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.Where”的 XML 注释
        public Expression<Func<T, bool>> Where { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.Where”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.Wheres”的 XML 注释
        public List<Expression<Func<T, bool>>> Wheres { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.Wheres”的 XML 注释

        /// <summary>
        /// 排序方式1:ASC,0:DESC 
        /// </summary>
        private int _isorderbyasc = 1;
        /// <summary>
        /// 排序方式1:ASC,0:DESC 
        /// </summary>
        public int IsOrderByASC
        {
            set { _isorderbyasc = value; }
            get { return _isorderbyasc; }
        }
        /// <summary>
        /// 排序方式1:ASC,0:DESC 
        /// </summary>
        private int _isorderbyasc2 = 1;
        /// <summary>
        /// 排序方式1:ASC,0:DESC 
        /// </summary>
        public int IsOrderByASC2
        {
            set { _isorderbyasc2 = value; }
            get { return _isorderbyasc2; }
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

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.WhereSql”的 XML 注释
        public string WhereSql { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageParameter<T>.WhereSql”的 XML 注释

      


    }
}

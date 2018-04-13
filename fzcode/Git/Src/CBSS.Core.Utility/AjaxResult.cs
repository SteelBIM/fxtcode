using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 表示Ajax操作结果 
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 获取 Ajax操作结果类型
        /// </summary>
        public ResultType type { get; set; }

        /// <summary>
        /// 获取 Ajax操作结果编码
        /// </summary>
        public int errorcode { get; set; }

        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object resultdata { get; set; }
    }
    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 消息结果类型
        /// </summary>
        info = 0,

        /// <summary>
        /// 成功结果类型
        /// </summary>
        success = 1,

        /// <summary>
        /// 警告结果类型
        /// </summary>
        warning = 2,

        /// <summary>
        /// 异常结果类型
        /// </summary>
        error = 3
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % this.rows == 0 ? records / this.rows : records / this.rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 查询条件Json
        /// </summary>
        public string conditionJson { get; set; }
    }
}

using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-06-17
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    /// <summary>
    ///Sys_TaskLog
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_TaskLog")]
    public class SysTaskLog : BaseTO
    {

        /// <summary>
        /// 自增
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 任务ID
        /// </summary>
        public virtual int TaskId
        {
            get;
            set;
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        public virtual string Message
        {
            get;
            set;
        }

    }
}
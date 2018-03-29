using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 李晓东
 * 时间: 2014-05-23
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    /// <summary>
    ///Sys_Task
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Sys_Task")]
    public class SysTask : BaseTO
    {

        /// <summary>
        /// 自增(2014.05.22新建表)
        /// </summary>
        [SQLField("Id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public virtual string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 执行状态
        /// </summary>
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 任务类型
        /// </summary>
        public virtual int TaskType
        {
            get;
            set;
        }
        /// <summary>
        /// 任务条数
        /// </summary>
        public virtual int Count
        {
            get;
            set;
        }
        /// <summary>
        /// 银行ID
        /// </summary>
        public virtual int BankId
        {
            get;
            set;
        }
        /// <summary>
        /// 银行所属项目ID
        /// </summary>
        public virtual int BankProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 执行者ID
        /// </summary>
        public virtual int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 成功条数
        /// </summary>
        public virtual int? SuccessCount
        {
            get;
            set;
        }
        /// <summary>
        /// 失败条数
        /// </summary>
        public virtual int? FailureCount
        {
            get;
            set;
        }
        /// <summary>
        /// URL地址结果集
        /// </summary>
        public virtual string Url1
        {
            get;
            set;
        }
        /// <summary>
        /// URL地址详细操作
        /// </summary>
        public virtual string Url2
        {
            get;
            set;
        }
        /// <summary>
        /// URL地址操作关联
        /// </summary>
        public virtual string Url3
        {
            get;
            set;
        }
        /// <summary>
        /// 文件ID
        /// </summary>
        public virtual int UploadFileId
        {
            get;
            set;
        }

    }
}
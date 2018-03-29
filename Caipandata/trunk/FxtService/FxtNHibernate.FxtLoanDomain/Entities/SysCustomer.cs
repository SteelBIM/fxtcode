using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 曹青
 * 时间: 2014-06-10
 * 摘要: 新建客户实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    [Serializable]
    [TableAttribute("dbo.Sys_Customer")]
    public class SysCustomer : BaseTO
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("customerid", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// 从属公司（上级公司ID）
        /// </summary>
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 从属公司名称（上级公司名称）
        /// </summary>
        public virtual string FxtCompanyName
        {
            get;
            set;
        }        
        /// <summary>
        /// 客户类型（评估机构2001010、银行2001013）
        /// </summary>
        public virtual int CustomerType
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual int CreateUserId
        {
            get;
            set;
        }
        /// <summary>
        /// 删除状态
        /// </summary>
        public virtual int Valid
        {
            get;
            set;
        }
    }

}

using CAS.Entity.BaseDAModels;
using System;

/**
 * 作者: 曹青
 * 时间: 2014-06-10
 * 摘要: 新建用户实体类
 * **/
namespace FxtNHibernate.FxtLoanDomain.Entities
{
    [Serializable]
    [TableAttribute("dbo.Sys_User")]
    public class SysUser : BaseTO
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户真实名
        /// </summary>
        public virtual string TrueName
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string UserPwd
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string EmailStr
        {
            get;
            set;
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 微信号
        /// </summary>
        public virtual string WxOpenId
        {
            get;
            set;
        }        
        /// <summary>
        /// 从属公司ID
        /// </summary>
        public virtual int FxtCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerId
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

using FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    /// <summary>
    /// 系统用户文件
    /// </summary>
    public class UserInfo : SysUser
    {
        /// <summary>
        /// 客户名称
        /// </summary>      
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户是否有效
        /// </summary>      
        public int CustomerValid { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>      
        public int CustomerType { get; set; }
    }

}

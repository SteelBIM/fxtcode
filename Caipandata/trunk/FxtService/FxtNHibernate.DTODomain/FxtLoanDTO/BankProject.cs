using FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    /// <summary>
    /// 银行文件项目
    /// </summary>
    public class BankProject : SysBankProject
    {
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
    }
}

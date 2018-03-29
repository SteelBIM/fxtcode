using FxtNHibernate.FxtLoanDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtNHibernate.DTODomain.FxtLoanDTO
{
    public class TaskToFile : SysTask
    {
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行项目名称
        /// </summary>
        public string BankProjectName { get; set; }
        /// <summary>
        /// 任务所属人
        /// </summary>
        public string UserName { get; set; }
    }
}

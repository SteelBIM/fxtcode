using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;

namespace CAS.Entity.DBEntity
{
    public class SYS_CustomApproveConditionStandard : SYSCustomApproveConditionStandard
    {
        /// <summary>
        /// 国家标准/自定义标准名称
        /// </summary>
        public string codename
        { get; set; }
    }
}

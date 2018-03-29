using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ReportNo : DatReportNo
    {
        
       /// <summary>
       /// 报告状态
       /// </summary>
        [SQLReadOnly]
        public string reportstatedesc { get; set; }
    }
}

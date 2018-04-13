using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    public class TBX_SchClassRelation
    {
        public IBS_SchClassRelation iBS_SchClassRelation { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        public TBX_SchClassRelation() 
        {
            iBS_SchClassRelation = new IBS_SchClassRelation();
        }
    }
}

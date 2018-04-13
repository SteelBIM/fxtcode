using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 班级与用户关系
    /// </summary>
    public class TBX_ClassUserRelation
    {
        public IBS_ClassUserRelation iBS_ClassUserRelation { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchName { get; set; }

        public TBX_ClassUserRelation() 
        {
            iBS_ClassUserRelation = new IBS_ClassUserRelation();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 学校与班级关系
    /// </summary>
    public class IBS_SchClassRelation
    {
        /// <summary>
        /// 学校ID
        /// </summary>
        public int SchID { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchName { get; set; }        

        /// <summary>
        /// 学校班级列表
        /// </summary>
        public List<SchClassS> SchClassList { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }

        public IBS_SchClassRelation() 
        {
            SchClassList = new List<SchClassS>();
        }
    }

    /// <summary>
    /// 班级简要信息
    /// </summary>
    public class SchClassS
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>
        public int GradeID { get; set; }

        /// <summary>
        /// 年级名称
        /// </summary>
        public string GradeName { get; set; }
    }
}

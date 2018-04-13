using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// 区域与学校关系（省市无学校列表，区有学校列表）
    /// </summary>
    public class IBS_AreaSchRelation
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }   

        /// <summary>
        /// 区域学校列表
        /// </summary>
        public List<AreaSchS> AreaSchList { get; set; }

        public IBS_AreaSchRelation() 
        {
            AreaSchList = new List<AreaSchS>();
        }
    }

    /// <summary>
    /// 班级简要信息
    /// </summary>
    public class AreaSchS
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public int SchD { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchName { get; set; }
    }
}

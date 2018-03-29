using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 土地信息查询条件
    /// </summary>
    public class SeachLand
    {
        /// <summary>
        /// 行政区
        /// </summary>
        public int areaid { get; set; }

        /// <summary>
        /// 片区
        /// </summary>
        public int subareaid { get; set; }

        /// <summary>
        /// 宗地号
        /// </summary>
        public string LandNo { get; set; }

        /// <summary>
        /// 土地使用证号
        /// </summary>
        public string FieldNo { get; set; }

        /// <summary>
        /// 土地用途
        /// </summary>
        public int PlanPurpose { get; set; }


        /// <summary>
        /// 土地使用起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }


        /// <summary>
        /// 土地使用结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 土地使用者
        /// </summary>
        public int LandOwnerId { get; set; }


        /// <summary>
        /// 土地所有者
        /// </summary>
        public int LandUseId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 坐标类
    /// </summary>
    public class DAT_Project_Coordinate
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int FxtCompanyId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public decimal? X { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? Y { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int? Valid { get; set; }
    }
}

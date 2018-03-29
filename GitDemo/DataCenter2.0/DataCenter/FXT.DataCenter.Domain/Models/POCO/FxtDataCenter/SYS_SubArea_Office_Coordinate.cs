using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea_Office_Coordinate
    {
        /// <summary>
        /// 片区坐标
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// SubAreaId
        /// </summary>
        public long SubAreaId { get; set; }
        /// <summary>
        /// AreaId
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// CityID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public decimal? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// FxtCompanyID
        /// </summary>
        public int FxtCompanyID { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid { get; set; }
    }
}

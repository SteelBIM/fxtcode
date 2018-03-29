using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea_Industry_Coordinate
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

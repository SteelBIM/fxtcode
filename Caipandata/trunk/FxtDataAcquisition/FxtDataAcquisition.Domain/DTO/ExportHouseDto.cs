using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class ExportHouseDto
    {
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 物理层
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// 实际层
        /// </summary>
        public string NominalFloor { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? BuildArea { get; set; }
       
        /// <summary>
        /// 朝向
        /// </summary>
        public string FrontCodeName { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public string SightCodeName { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        public string NoiseCodeName { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public string PurposeCodeName { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        public string StructureCodeName { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        public string VDCodeName { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public string HouseTypeCodeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.Models
{
    /// <summary>
    /// 房号详细
    /// </summary>
    public class HouseDetails
    {
        public int Id { get; set; }
        /// <summary>
        /// 房号ID
        /// </summary>
        public int HouseId { get; set; }
        public int BuildingId { get; set; }
        /// <summary>
        /// 房号名称
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 户型
        /// </summary>
        public int? HouseTypeCode { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        public string UnitNo { get; set; }
        /// <summary>
        /// 室号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public decimal? BuildArea { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public int? FrontCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal? SalePrice { get; set; }
        public decimal? Weight { get; set; }
        public string PhotoName { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 户型结构
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int? IsEValue { get; set; }
        public int CityID { get; set; }
        public int? OldId { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? Valid { get; set; }
        public DateTime? SaveDateTime { get; set; }
        public string SaveUser { get; set; }
        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// 面积确认
        /// </summary>
        public short? IsShowBuildingArea { get; set; }
        /// <summary>
        /// 套内面积
        /// </summary>
        public decimal? InnerBuildingArea { get; set; }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public int? SubHouseType { get; set; }
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public decimal? SubHouseArea { get; set; }
        public string Creator { get; set; }
        /// <summary>
        /// 名义层（实际层）
        /// </summary>
        public string NominalFloor { get; set; }
        /// <summary>
        /// 通风采光
        /// </summary>
        public int? VDCode { get; set; }
        /// <summary>
        /// 装修
        /// </summary>
        public int? FitmentCode { get; set; }
        /// <summary>
        /// 是否有厨房
        /// </summary>
        public int? Cookroom { get; set; }
        /// <summary>
        /// 阳台数
        /// </summary>
        public int? Balcony { get; set; }
        /// <summary>
        /// 洗手间数
        /// </summary>
        public int? Toilet { get; set; }
        /// <summary>
        /// 噪音情况
        /// </summary>
        public int? NoiseCode { get; set; }
        /// <summary>
        /// 数据中心房号ID
        /// </summary>
        public int? FxtHouseId { get; set; }

        public virtual House House { get; set; }
        public virtual Building Building { get; set; }
    }

}

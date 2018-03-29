namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class House
    {
        public Guid? AppId { get; set; }
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
        /// 起始层
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// 结束层
        /// </summary>
        public int? EndFloorNo { get; set; }
        /// <summary>
        /// 单元(室号)
        /// </summary>
        public string UnitNo { get; set; }
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
        /// 价格系数
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
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

        public string Creator { get; set; }

        public int? FxtHouseId { get; set; }

        public int? Status { get; set; }

        public int? TempletId { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
        public virtual Templet Templet { get; set; }
    }
}

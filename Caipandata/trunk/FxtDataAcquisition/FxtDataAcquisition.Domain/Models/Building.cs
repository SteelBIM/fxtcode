namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Building
    {
        public int BuildingId { get; set; }
        public Guid? AppId { get; set; }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string BuildingName { get; set; }

        public int ProjectId { get; set; }
        /// <summary>
        /// 楼栋用途
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// 建筑结构
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// 建筑类型
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// 总层数
        /// </summary>
        public int? TotalFloor { get; set; }
        /// <summary>
        /// 层高
        /// </summary>
        public decimal? FloorHigh { get; set; }
        /// <summary>
        /// 销售许可证
        /// </summary>
        public string SaleLicence { get; set; }
        /// <summary>
        /// 梯户比
        /// </summary>
        public string ElevatorRate { get; set; }
        /// <summary>
        /// 单元数
        /// </summary>
        public int? UnitsNumber { get; set; }
        /// <summary>
        /// 总户数
        /// </summary>
        public int? TotalNumber { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? TotalBuildArea { get; set; }
        /// <summary>
        /// 建筑时间
        /// </summary>
        public DateTime? BuildDate { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 均价层
        /// </summary>
        public int? AverageFloor { get; set; }
        /// <summary>
        /// 入伙时间
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 预售时间
        /// </summary>
        public DateTime? LicenceDate { get; set; }
        /// <summary>
        /// 楼栋别名
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// 价格系数
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int? IsEValue { get; set; }

        public int CityID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Valid { get; set; }
        /// <summary>
        /// 销售均价
        /// </summary>
        public decimal? SalePrice { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public int? LocationCode { get; set; }
        /// <summary>
        /// 景观
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// 朝向
        /// </summary>
        public int? FrontCode { get; set; }

        public int? FxtCompanyId { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }
        /// <summary>
        /// 外墙装修
        /// </summary>
        public int? Wall { get; set; }
        /// <summary>
        /// 是否带电梯
        /// </summary>
        public int? IsElevator { get; set; }
        /// <summary>
        /// 附属房屋均价
        /// </summary>
        public decimal? SubAveragePrice { get; set; }
        /// <summary>
        /// 价格系数说明
        /// </summary>
        public string PriceDetail { get; set; }
        /// <summary>
        /// 户型面积
        /// </summary>
        public int? BHouseTypeCode { get; set; }
        /// <summary>
        /// 楼间距
        /// </summary>
        public int? Distance { get; set; }
        /// <summary>
        /// 地下层数
        /// </summary>
        public int? Basement { get; set; }
        /// <summary>
        /// 门牌号（地址）
        /// </summary>
        public string Doorplate { get; set; }
        /// <summary>
        /// 产权形式
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// 是否虚拟楼栋
        /// </summary>
        public int? IsVirtual { get; set; }
        /// <summary>
        /// 楼层分布
        /// </summary>
        public string FloorSpread { get; set; }
        /// <summary>
        /// 裙楼层数
        /// </summary>
        public int? PodiumBuildingFloor { get; set; }
        /// <summary>
        /// 裙楼面积
        /// </summary>
        public decimal? PodiumBuildingArea { get; set; }
        /// <summary>
        /// 塔楼面积
        /// </summary>
        public decimal? TowerBuildingArea { get; set; }
        /// <summary>
        /// 地下室总面积
        /// </summary>
        public decimal? BasementArea { get; set; }
        /// <summary>
        /// 地下室用途
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// 住宅套数
        /// </summary>
        public int? HouseNumber { get; set; }
        /// <summary>
        /// 住宅总面积
        /// </summary>
        public decimal? HouseArea { get; set; }
        /// <summary>
        /// 非住宅套数
        /// </summary>
        public int? OtherNumber { get; set; }
        /// <summary>
        /// 非住宅面积
        /// </summary>
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// 内部装修
        /// </summary>
        public int? InnerFitmentCode { get; set; }
        /// <summary>
        /// 单层户数
        /// </summary>
        public int? FloorHouseNumber { get; set; }
        /// <summary>
        /// 电梯数量
        /// </summary>
        public int? LiftNumber { get; set; }
        /// <summary>
        /// 电梯品牌
        /// </summary>
        public string LiftBrand { get; set; }
        /// <summary>
        /// 设备设施
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// 管道燃气
        /// </summary>
        public int? PipelineGasCode { get; set; }
        /// <summary>
        /// 采暖方式
        /// </summary>
        public int? HeatingModeCode { get; set; }
        /// <summary>
        /// 墙体类型
        /// </summary>
        public int? WallTypeCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否带院子
        /// </summary>
        public int? IsYard { get; set; }
        public int? Status { get; set; }
        /// <summary>
        /// 维护情况
        /// </summary>
        public int? MaintenanceCode { get; set; }

        public int? FxtBuildingId { get; set; }

        public string Creator { get; set; }

        public int? TempletId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
        public virtual Templet Templet { get; set; }
    }
}

namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public int ProjectId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int? SubAreaId { get; set; }
        /// <summary>
        /// 宗地号
        /// </summary>
        public string FieldNo { get; set; }
        /// <summary>
        /// 主用途
        /// </summary>
        public int PurposeCode { get; set; }
        /// <summary>
        /// 楼盘地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 占地面积
        /// </summary>
        public decimal? LandArea { get; set; }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 土地使用年限
        /// </summary>
        public int? UsableYear { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 可销售面积
        /// </summary>
        public decimal? SalableArea { get; set; }
        /// <summary>
        /// 容积率
        /// </summary>
        public decimal? CubageRate { get; set; }
        /// <summary>
        /// 绿化率
        /// </summary>
        public decimal? GreenRate { get; set; }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime? BuildingDate { get; set; }
        /// <summary>
        /// 封顶日期
        /// </summary>
        public DateTime? CoverDate { get; set; }
        /// <summary>
        /// 开盘日期
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// 入伙日期
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 内部认购日期
        /// </summary>
        public DateTime? InnerSaleDate { get; set; }
        /// <summary>
        /// 产权形式
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// 车位数
        /// </summary>
        public int? ParkingNumber { get; set; }
        /// <summary>
        /// 项目均价
        /// </summary>
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// 管理处电话
        /// </summary>
        public string ManagerTel { get; set; }
        /// <summary>
        /// 物业费
        /// </summary>
        public string ManagerPrice { get; set; }
        /// <summary>
        /// 总套数
        /// </summary>
        public int? TotalNum { get; set; }
        /// <summary>
        /// 总栋数
        /// </summary>
        public int? BuildingNum { get; set; }
        /// <summary>
        /// 项目概况
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 主建筑物类型
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }
        /// <summary>
        /// 办公面积
        /// </summary>
        public decimal? OfficeArea { get; set; }
        /// <summary>
        /// 其他用途面积
        /// </summary>
        public decimal? OtherArea { get; set; }
        /// <summary>
        /// 土地规划用途
        /// </summary>
        public string PlanPurpose { get; set; }
        /// <summary>
        /// 价格调查时间
        /// </summary>
        public DateTime? PriceDate { get; set; }
        /// <summary>
        /// 是否完成基础数据
        /// </summary>
        public int? IsComplete { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string OtherName { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }
        /// <summary>
        /// 价格修正系数
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 商业面积
        /// </summary>
        public decimal? BusinessArea { get; set; }
        /// <summary>
        /// 工业面积
        /// </summary>
        public decimal? IndustryArea { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// 拼音简写
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        public int AreaID { get; set; }

        public string OldId { get; set; }

        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 环线ID
        /// </summary>
        public int? AreaLineId { get; set; }

        public int? Valid { get; set; }
        /// <summary>
        /// 开盘均价
        /// </summary>
        public decimal? SalePrice { get; set; }

        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        public string PinYinAll { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }

        public string Creator { get; set; }
        /// <summary>
        /// 是否空楼盘
        /// </summary>
        public int? IsEmpty { get; set; }

        public int? TotalId { get; set; }
        /// <summary>
        /// 东
        /// </summary>
        public string East { get; set; }
        /// <summary>
        /// 西
        /// </summary>
        public string West { get; set; }
        /// <summary>
        /// 南
        /// </summary>
        public string South { get; set; }
        /// <summary>
        /// 北
        /// </summary>
        public string North { get; set; }

        public int? FxtProjectId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 停车状况
        /// </summary>
        public int? ParkingStatus { get; set; }
        /// <summary>
        /// 建筑质量
        /// </summary>
        public int? BuildingQuality { get; set; }
        /// <summary>
        /// 小区规模
        /// </summary>
        public int? HousingScale { get; set; }
        /// <summary>
        /// 楼栋备注
        /// </summary>
        public string BuildingDetail { get; set; }
        /// <summary>
        /// 房号备注
        /// </summary>
        public string HouseDetail { get; set; }
        /// <summary>
        /// 地下室用途
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// 物业管理质量
        /// </summary>
        public int? ManagerQuality { get; set; }
        /// <summary>
        /// 设备设施
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// 配套等级
        /// </summary>
        public int? AppendageClass { get; set; }
        /// <summary>
        /// 区域分析
        /// </summary>
        public string RegionalAnalysis { get; set; }
        /// <summary>
        /// 有利因素
        /// </summary>
        public string Wrinkle { get; set; }
        /// <summary>
        /// 不利因素
        /// </summary>
        public string Aversion { get; set; }
        /// <summary>
        /// 车位描述
        /// </summary>
        public string ParkingDesc { get; set; }


        public int? PhotoCount { get; set; }
        public int? TempletId { get; set; }
        
        
        public virtual ICollection<Building> Buildings { get; set; }

        //public virtual AllotFlow AllotFlow { get; set; }

        public virtual ICollection<PAppendage> Appendages { get; set; }
        public virtual ICollection<PCompany> Companys { get; set; }
        public virtual Templet Templet { get; set; }


    }
}

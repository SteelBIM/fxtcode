using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtTempDomain.Entities
{
	 	//DAT_Project
    public class DATProject
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 片区
        /// </summary>
        public virtual int? SubAreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 宗地号
        /// </summary>
        public virtual string FieldNo
        {
            get;
            set;
        }
        /// <summary>
        /// 主用途
        /// </summary>
        public virtual int PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘地址
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 占地面积
        /// </summary>
        public virtual decimal? LandArea
        {
            get;
            set;
        }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public virtual DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 土地使用年限
        /// </summary>
        public virtual int? UsableYear
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual decimal? BuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// 可销售面积
        /// </summary>
        public virtual decimal? SalableArea
        {
            get;
            set;
        }
        /// <summary>
        /// 容积率
        /// </summary>
        public virtual decimal? CubageRate
        {
            get;
            set;
        }
        /// <summary>
        /// 绿化率
        /// </summary>
        public virtual decimal? GreenRate
        {
            get;
            set;
        }
        /// <summary>
        /// 开工日期
        /// </summary>
        public virtual DateTime? BuildingDate
        {
            get;
            set;
        }
        /// <summary>
        /// 封顶日期
        /// </summary>
        public virtual DateTime? CoverDate
        {
            get;
            set;
        }
        /// <summary>
        /// 开盘日期
        /// </summary>
        public virtual DateTime? SaleDate
        {
            get;
            set;
        }
        /// <summary>
        /// 入伙日期
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get;
            set;
        }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public virtual DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 内部认购日期
        /// </summary>
        public virtual DateTime? InnerSaleDate
        {
            get;
            set;
        }
        /// <summary>
        /// 产权形式
        /// </summary>
        public virtual int? RightCode
        {
            get;
            set;
        }
        /// <summary>
        /// 车位数
        /// </summary>
        public virtual int? ParkingNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 项目均价
        /// </summary>
        public virtual decimal? AveragePrice
        {
            get;
            set;
        }
        /// <summary>
        /// 管理处电话
        /// </summary>
        public virtual string ManagerTel
        {
            get;
            set;
        }
        /// <summary>
        /// 物业费
        /// </summary>
        public virtual string ManagerPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 总套数
        /// </summary>
        public virtual int? TotalNum
        {
            get;
            set;
        }
        /// <summary>
        /// 楼宇数
        /// </summary>
        public virtual int? BuildingNum
        {
            get;
            set;
        }
        /// <summary>
        /// 项目概况
        /// </summary>
        public virtual string Detail
        {
            get;
            set;
        }
        /// <summary>
        /// 主建筑物类型
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime? UpdateDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 办公面积
        /// </summary>
        public virtual decimal? OfficeArea
        {
            get;
            set;
        }
        /// <summary>
        /// 其他用途面积
        /// </summary>
        public virtual decimal? OtherArea
        {
            get;
            set;
        }
        /// <summary>
        /// 土地规划用途
        /// </summary>
        public virtual string PlanPurpose
        {
            get;
            set;
        }
        /// <summary>
        /// 价格调查时间
        /// </summary>
        public virtual DateTime? PriceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 是否完成基础数据
        /// </summary>
        public virtual int? IsComplete
        {
            get;
            set;
        }
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string OtherName
        {
            get;
            set;
        }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public virtual DateTime? SaveDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// SaveUser
        /// </summary>
        public virtual string SaveUser
        {
            get;
            set;
        }
        /// <summary>
        /// RP修正系数
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 商业面积
        /// </summary>
        public virtual decimal? BusinessArea
        {
            get;
            set;
        }
        /// <summary>
        /// 工业面积
        /// </summary>
        public virtual decimal? IndustryArea
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可估
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        /// <summary>
        /// 拼音简写
        /// </summary>
        public virtual string PinYin
        {
            get;
            set;
        }
        /// <summary>
        /// 城市ID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int AreaID
        {
            get;
            set;
        }
        /// <summary>
        /// OldId
        /// </summary>
        public virtual string OldId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 环线ID
        /// </summary>
        public virtual int? AreaLineId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        /// <summary>
        /// 开盘均价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get;
            set;
        }
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public virtual int? FxtCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        public virtual string PinYinAll
        {
            get;
            set;
        }
        /// <summary>
        /// X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        /// <summary>
        /// Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get;
            set;
        }
        /// <summary>
        /// Creator
        /// </summary>
        public virtual string Creator
        {
            get;
            set;
        }
        /// <summary>
        /// 是否空楼盘
        /// </summary>
        public virtual int? IsEmpty
        {
            get;
            set;
        }
        /// <summary>
        /// 总楼盘ID
        /// </summary>
        public virtual int? TotalId
        {
            get;
            set;
        }
        /// <summary>
        /// 东
        /// </summary>
        public virtual string East
        {
            get;
            set;
        }
        /// <summary>
        /// 西
        /// </summary>
        public virtual string West
        {
            get;
            set;
        }
        /// <summary>
        /// 南
        /// </summary>
        public virtual string South
        {
            get;
            set;
        }
        /// <summary>
        /// 北
        /// </summary>
        public virtual string North
        {
            get;
            set;
        }
        /// <summary>
        /// FxtProjectId
        /// </summary>
        public virtual int? FxtProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// Status
        /// </summary>
        public virtual int? Status
        {
            get;
            set;
        }

    }
}
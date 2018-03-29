using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //DAT_Building_sub
    public class DATBuildingsub
    {

        public virtual BuildingPKCompany LNKBCompanyPX { get; set; }
        /// <summary>
        /// BuildingName
        /// </summary>
        public virtual string BuildingName
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// PurposeCode
        /// </summary>
        public virtual int? PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// StructureCode
        /// </summary>
        public virtual int? StructureCode
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingTypeCode
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// TotalFloor
        /// </summary>
        public virtual int? TotalFloor
        {
            get;
            set;
        }
        /// <summary>
        /// FloorHigh
        /// </summary>
        public virtual decimal? FloorHigh
        {
            get;
            set;
        }
        /// <summary>
        /// SaleLicence
        /// </summary>
        public virtual string SaleLicence
        {
            get;
            set;
        }
        /// <summary>
        /// ElevatorRate
        /// </summary>
        public virtual string ElevatorRate
        {
            get;
            set;
        }
        /// <summary>
        /// UnitsNumber
        /// </summary>
        public virtual int? UnitsNumber
        {
            get;
            set;
        }
        /// <summary>
        /// TotalNumber
        /// </summary>
        public virtual int? TotalNumber
        {
            get;
            set;
        }
        /// <summary>
        /// TotalBuildArea
        /// </summary>
        public virtual decimal? TotalBuildArea
        {
            get;
            set;
        }
        /// <summary>
        /// BuildDate
        /// </summary>
        public virtual DateTime? BuildDate
        {
            get;
            set;
        }
        /// <summary>
        /// SaleDate
        /// </summary>
        public virtual DateTime? SaleDate
        {
            get;
            set;
        }
        /// <summary>
        /// AveragePrice
        /// </summary>
        public virtual decimal? AveragePrice
        {
            get;
            set;
        }
        /// <summary>
        /// AverageFloor
        /// </summary>
        public virtual int? AverageFloor
        {
            get;
            set;
        }
        /// <summary>
        /// JoinDate
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get;
            set;
        }
        /// <summary>
        /// LicenceDate
        /// </summary>
        public virtual DateTime? LicenceDate
        {
            get;
            set;
        }
        /// <summary>
        /// OtherName
        /// </summary>
        public virtual string OtherName
        {
            get;
            set;
        }
        /// <summary>
        /// Weight
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        /// <summary>
        /// IsEValue
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        /// <summary>
        /// CityID
        /// </summary>
        public virtual int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
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
        /// Valid
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        /// <summary>
        /// SalePrice
        /// </summary>
        public virtual decimal? SalePrice
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
        /// 楼栋位置
        /// </summary>
        public virtual int? LocationCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋景观
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋朝向
        /// </summary>
        public virtual int? FrontCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋结构修正价格
        /// </summary>
        public virtual decimal? StructureWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑类型修正价格
        /// </summary>
        public virtual decimal? BuildingTypeWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 年期修正价格
        /// </summary>
        public virtual decimal? YearWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 用途修正价格
        /// </summary>
        public virtual decimal? PurposeWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋位置修正价格
        /// </summary>
        public virtual decimal? LocationWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 景观修正价格
        /// </summary>
        public virtual decimal? SightWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 朝向修正价格
        /// </summary>
        public virtual decimal? FrontWeight
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
        /// 外墙
        /// </summary>
        public virtual int? Wall
        {
            get;
            set;
        }
        /// <summary>
        /// 是否带电梯
        /// </summary>
        public virtual int? IsElevator
        {
            get;
            set;
        }
        /// <summary>
        /// 附属房屋均价
        /// </summary>
        public virtual decimal? SubAveragePrice
        {
            get;
            set;
        }
        /// <summary>
        /// 价格系数说明
        /// </summary>
        public virtual string PriceDetail
        {
            get;
            set;
        }
        /// <summary>
        /// BHouseTypeCode
        /// </summary>
        public virtual int? BHouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// BHouseTypeWeight
        /// </summary>
        public virtual decimal? BHouseTypeWeight
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
        /// Distance
        /// </summary>
        public virtual int? Distance
        {
            get;
            set;
        }
        /// <summary>
        /// DistanceWeight
        /// </summary>
        public virtual decimal? DistanceWeight
        {
            get;
            set;
        }
        /// <summary>
        /// basement
        /// </summary>
        public virtual int? basement
        {
            get;
            set;
        }
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// ElevatorRateWeight
        /// </summary>
        public virtual decimal? ElevatorRateWeight
        {
            get;
            set;
        }
        /// <summary>
        /// IsYard
        /// </summary>
        public virtual int? IsYard
        {
            get;
            set;
        }
        /// <summary>
        /// YardWeight
        /// </summary>
        public virtual decimal? YardWeight
        {
            get;
            set;
        }
        /// <summary>
        /// Doorplate
        /// </summary>
        public virtual string Doorplate
        {
            get;
            set;
        }

    }

    public class BuildingPKCompany
    {
        /// <summary>
        /// BuildingId
        /// </summary>
        public virtual int BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// Fxt_CompanyId
        /// </summary>
        public virtual int Fxt_CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 判断两个对象是否相同，这个方法需要重写
        /// </summary>
        /// <param name="obj">进行比较的对象</param>
        /// <returns>真true或假false</returns>
        public override bool Equals(object obj)
        {
            if (obj is BuildingPKCompany)
            {
                BuildingPKCompany pk = obj as BuildingPKCompany;
                if (this.BuildingId == pk.BuildingId && this.Fxt_CompanyId == pk.Fxt_CompanyId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
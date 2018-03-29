using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //DAT_Project_sub
    public class DATProjectsub
    {

        public virtual ProjectPKCompany LNKPCompanyPX { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        public virtual string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// SubAreaId
        /// </summary>
        public virtual int? SubAreaId
        {
            get;
            set;
        }
        /// <summary>
        /// FieldNo
        /// </summary>
        public virtual string FieldNo
        {
            get;
            set;
        }
        /// <summary>
        /// PurposeCode
        /// </summary>
        public virtual int PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// Address
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }
        /// <summary>
        /// LandArea
        /// </summary>
        public virtual decimal? LandArea
        {
            get;
            set;
        }
        /// <summary>
        /// StartDate
        /// </summary>
        public virtual DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// UsableYear
        /// </summary>
        public virtual int? UsableYear
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingArea
        /// </summary>
        public virtual decimal? BuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// SalableArea
        /// </summary>
        public virtual decimal? SalableArea
        {
            get;
            set;
        }
        /// <summary>
        /// CubageRate
        /// </summary>
        public virtual decimal? CubageRate
        {
            get;
            set;
        }
        /// <summary>
        /// GreenRate
        /// </summary>
        public virtual decimal? GreenRate
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingDate
        /// </summary>
        public virtual DateTime? BuildingDate
        {
            get;
            set;
        }
        /// <summary>
        /// CoverDate
        /// </summary>
        public virtual DateTime? CoverDate
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
        /// JoinDate
        /// </summary>
        public virtual DateTime? JoinDate
        {
            get;
            set;
        }
        /// <summary>
        /// EndDate
        /// </summary>
        public virtual DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// InnerSaleDate
        /// </summary>
        public virtual DateTime? InnerSaleDate
        {
            get;
            set;
        }
        /// <summary>
        /// RightCode
        /// </summary>
        public virtual int? RightCode
        {
            get;
            set;
        }
        /// <summary>
        /// ParkingNumber
        /// </summary>
        public virtual int? ParkingNumber
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
        /// ManagerTel
        /// </summary>
        public virtual string ManagerTel
        {
            get;
            set;
        }
        /// <summary>
        /// ManagerPrice
        /// </summary>
        public virtual string ManagerPrice
        {
            get;
            set;
        }
        /// <summary>
        /// TotalNum
        /// </summary>
        public virtual int? TotalNum
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingNum
        /// </summary>
        public virtual int? BuildingNum
        {
            get;
            set;
        }
        /// <summary>
        /// Detail
        /// </summary>
        public virtual string Detail
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
        /// UpdateDateTime
        /// </summary>
        public virtual DateTime? UpdateDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// OfficeArea
        /// </summary>
        public virtual decimal? OfficeArea
        {
            get;
            set;
        }
        /// <summary>
        /// OtherArea
        /// </summary>
        public virtual decimal? OtherArea
        {
            get;
            set;
        }
        /// <summary>
        /// PlanPurpose
        /// </summary>
        public virtual string PlanPurpose
        {
            get;
            set;
        }
        /// <summary>
        /// PriceDate
        /// </summary>
        public virtual DateTime? PriceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 是否给过RP
        /// </summary>
        public virtual int? IsComplete
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
        /// BusinessArea
        /// </summary>
        public virtual decimal? BusinessArea
        {
            get;
            set;
        }
        /// <summary>
        /// IndustryArea
        /// </summary>
        public virtual decimal? IndustryArea
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
        /// PinYin
        /// </summary>
        public virtual string PinYin
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
        /// AreaID
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
        /// CreateTime
        /// </summary>
        public virtual DateTime? CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// AreaLineId
        /// </summary>
        public virtual int? AreaLineId
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
        /// IsEmpty
        /// </summary>
        public virtual int? IsEmpty
        {
            get;
            set;
        }
        /// <summary>
        /// TotalId
        /// </summary>
        public virtual int? TotalId
        {
            get;
            set;
        }
        /// <summary>
        /// East
        /// </summary>
        public virtual string East
        {
            get;
            set;
        }
        /// <summary>
        /// West
        /// </summary>
        public virtual string West
        {
            get;
            set;
        }
        /// <summary>
        /// South
        /// </summary>
        public virtual string South
        {
            get;
            set;
        }
        /// <summary>
        /// North
        /// </summary>
        public virtual string North
        {
            get;
            set;
        }

    }

    public class ProjectPKCompany
    {

        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// FxtCompanyId
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
            if (obj is ProjectPKCompany)
            {
                ProjectPKCompany pk = obj as ProjectPKCompany;
                if (this.ProjectId == pk.ProjectId && this.Fxt_CompanyId == pk.Fxt_CompanyId)
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
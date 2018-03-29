using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.4.16
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //DAT_House_sub
    public class DATHousesub
    {

        public virtual HousePKCompany LNKHCompanyPX { get; set; }
        /// <summary>
        /// BuildingId
        /// </summary>
        public virtual int BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// HouseName
        /// </summary>
        public virtual string HouseName
        {
            get;
            set;
        }
        /// <summary>
        /// HouseTypeCode
        /// </summary>
        public virtual int? HouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// FloorNo
        /// </summary>
        public virtual int FloorNo
        {
            get;
            set;
        }
        /// <summary>
        /// UnitNo
        /// </summary>
        public virtual string UnitNo
        {
            get;
            set;
        }
        /// <summary>
        /// BuildArea
        /// </summary>
        public virtual decimal? BuildArea
        {
            get;
            set;
        }
        /// <summary>
        /// FrontCode
        /// </summary>
        public virtual int? FrontCode
        {
            get;
            set;
        }
        /// <summary>
        /// SightCode
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        /// <summary>
        /// UnitPrice
        /// </summary>
        public virtual decimal? UnitPrice
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
        /// Weight
        /// </summary>
        public virtual decimal? Weight
        {
            get;
            set;
        }
        /// <summary>
        /// PhotoName
        /// </summary>
        public virtual string PhotoName
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
        /// StructureCode
        /// </summary>
        public virtual int? StructureCode
        {
            get;
            set;
        }
        /// <summary>
        /// TotalPrice
        /// </summary>
        public virtual decimal? TotalPrice
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
        /// Valid
        /// </summary>
        public virtual int? Valid
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
        /// IsShowBuildingArea
        /// </summary>
        public virtual int? IsShowBuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// InnerBuildingArea
        /// </summary>
        public virtual decimal? InnerBuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public virtual int? SubHouseType
        {
            get;
            set;
        }
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public virtual decimal? SubHouseArea
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
        /// NominalFloor
        /// </summary>
        public virtual string NominalFloor
        {
            get;
            set;
        }

    }

    public class HousePKCompany
    {
        /// <summary>
        /// HouseId
        /// </summary>
        public virtual int HouseId
        {
            get;
            set;
        }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public virtual int FxtCompanyId
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
            if (obj is HousePKCompany)
            {
                HousePKCompany pk = obj as HousePKCompany;
                if (this.HouseId == pk.HouseId && this.FxtCompanyId == pk.FxtCompanyId)
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
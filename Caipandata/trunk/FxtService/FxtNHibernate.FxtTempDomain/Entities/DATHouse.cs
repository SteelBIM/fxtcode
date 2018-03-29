using System;

/**
 * 作者: 曾智磊
 * 时间: 2014.03.03
 * 摘要: 新建实体类
 * **/
namespace FxtNHibernate.FxtTempDomain.Entities
{
    //DAT_House
    public class DATHouse
    {

        /// <summary>
        /// id
        /// </summary>
        public virtual int HouseId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        public virtual int BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// 物业名称
        /// </summary>
        public virtual string HouseName
        {
            get;
            set;
        }
        /// <summary>
        /// 户型
        /// </summary>
        public virtual int? HouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼层
        /// </summary>
        public virtual int FloorNo
        {
            get;
            set;
        }
        /// <summary>
        /// 单元
        /// </summary>
        public virtual string UnitNo
        {
            get;
            set;
        }
        /// <summary>
        /// 面积
        /// </summary>
        public virtual decimal? BuildArea
        {
            get;
            set;
        }
        /// <summary>
        /// 朝向
        /// </summary>
        public virtual int? FrontCode
        {
            get;
            set;
        }
        /// <summary>
        /// 景观
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal? UnitPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 总价
        /// </summary>
        public virtual decimal? SalePrice
        {
            get;
            set;
        }
        /// <summary>
        /// 权重值
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
        /// 户型结构
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
        /// FxtCompanyId
        /// </summary>
        public virtual int? FxtCompanyId
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
        /// FxtHouseId
        /// </summary>
        public virtual int? FxtHouseId
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
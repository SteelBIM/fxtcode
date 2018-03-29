using System;

/**
 * 作者: 李晓东
 * 时间: 2013-12-02
 * 摘要: 新建实体类
 *       2013.12.11 新增字段,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    /// <summary>
    ///DAT_Case_xb
    /// </summary>
    public class DATCasexb
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
        /// BuildingId
        /// </summary>
        public virtual int? BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// HouseId
        /// </summary>
        public virtual int? HouseId
        {
            get;
            set;
        }
        /// <summary>
        /// CompanyId
        /// </summary>
        public virtual int? CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// CaseDate
        /// </summary>
        public virtual DateTime CaseDate
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
        /// FloorNumber
        /// </summary>
        public virtual int? FloorNumber
        {
            get;
            set;
        }
        /// <summary>
        /// HouseNo
        /// </summary>
        public virtual string HouseNo
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
        /// UsableArea
        /// </summary>
        public virtual decimal? UsableArea
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
        /// UnitPrice
        /// </summary>
        public virtual decimal? UnitPrice
        {
            get;
            set;
        }
        /// <summary>
        /// MoneyUnitCode
        /// </summary>
        public virtual int? MoneyUnitCode
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
        /// CaseTypeCode
        /// </summary>
        public virtual int CaseTypeCode
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
        /// HouseTypeCode
        /// </summary>
        public virtual int? HouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// CreateDate
        /// </summary>
        public virtual DateTime? CreateDate
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
        /// Remark
        /// </summary>
        public virtual string Remark
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
        /// OldID
        /// </summary>
        public virtual int? OldID
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
        /// Valid
        /// </summary>
        public virtual int? Valid
        {
            get;
            set;
        }
        /// <summary>
        /// CaseID
        /// </summary>
        public virtual int CaseID
        {
            get;
            set;
        }
        /// <summary>
        /// FXTCompanyId
        /// </summary>
        public virtual int FXTCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// BuildingName
        /// </summary>
        public virtual string BuildingName
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
        /// RemainYear
        /// </summary>
        public virtual int? RemainYear
        {
            get;
            set;
        }
        /// <summary>
        /// Depreciation
        /// </summary>
        public virtual decimal? Depreciation
        {
            get;
            set;
        }
        /// <summary>
        /// FitmentCode
        /// </summary>
        public virtual int? FitmentCode
        {
            get;
            set;
        }
        /// <summary>
        /// 查勘ID
        /// </summary>
        public virtual int? SurveyId
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
        /// SourceName
        /// </summary>
        public virtual string SourceName
        {
            get;
            set;
        }
        /// <summary>
        /// SourceLink
        /// </summary>
        public virtual string SourceLink
        {
            get;
            set;
        }
        /// <summary>
        /// SourcePhone
        /// </summary>
        public virtual string SourcePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public virtual int? AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public virtual string AreaName
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑年代
        /// </summary>
        public virtual string BuildingDate
        {
            get;
            set;
        }
        /// <summary>
        /// 装修
        /// </summary>
        public virtual string ZhuangXiu
        {
            get;
            set;
        }
        /// <summary>
        /// 附属物业
        /// </summary>
        public virtual string SubHouse
        {
            get;
            set;
        }
        /// <summary>
        /// 配套设施
        /// </summary>
        public virtual string PeiTao
        {
            get;
            set;
        }
    }
}
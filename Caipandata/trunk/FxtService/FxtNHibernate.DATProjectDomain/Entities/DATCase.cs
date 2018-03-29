using System;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建实体类
 *       2013.12.11 新增字段,AreaId,AreaName,BuildingDate,ZhuangXiu,SubHouse,PeiTao
 * **/
namespace FxtNHibernate.DATProjectDomain.Entities
{
    //DAT_Case
    public class DATCase
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual int CaseID
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public virtual int ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼宇ID
        /// </summary>
        public virtual int? BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// 物业ID
        /// </summary>
        public virtual int? HouseId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司ID
        /// </summary>
        public virtual int? CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime CaseDate
        {
            get;
            set;
        }
        /// <summary>
        /// 用途Code
        /// </summary>
        public virtual int? PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼层
        /// </summary>
        public virtual int? FloorNumber
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
        /// 房号
        /// </summary>
        public virtual string HouseNo
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
        /// 使用面积
        /// </summary>
        public virtual decimal? UsableArea
        {
            get;
            set;
        }
        /// <summary>
        /// 朝向Code
        /// </summary>
        public virtual int? FrontCode
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
        /// 货币单位Code
        /// </summary>
        public virtual int? MoneyUnitCode
        {
            get;
            set;
        }
        /// <summary>
        /// 景观Code
        /// </summary>
        public virtual int? SightCode
        {
            get;
            set;
        }
        /// <summary>
        /// 案例类型
        /// </summary>
        public virtual int? CaseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑结构Code
        /// </summary>
        public virtual int? StructureCode
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑类型Code
        /// </summary>
        public virtual int? BuildingTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 户型Code
        /// </summary>
        public virtual int? HouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
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
        /// 总价
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
        public virtual int? CityID
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
        /// 案例所有者
        /// </summary>
        public virtual int? FXTCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 总层数
        /// </summary>
        public virtual int? TotalFloor
        {
            get;
            set;
        }
        /// <summary>
        /// 剩余年限
        /// </summary>
        public virtual int? RemainYear
        {
            get;
            set;
        }
        /// <summary>
        /// 成新率
        /// </summary>
        public virtual decimal? Depreciation
        {
            get;
            set;
        }
        /// <summary>
        /// 装修情况
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
        /// 来源
        /// </summary>
        public virtual string SourceName
        {
            get;
            set;
        }
        /// <summary>
        /// 来源链接
        /// </summary>
        public virtual string SourceLink
        {
            get;
            set;
        }
        /// <summary>
        /// 来源电话
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
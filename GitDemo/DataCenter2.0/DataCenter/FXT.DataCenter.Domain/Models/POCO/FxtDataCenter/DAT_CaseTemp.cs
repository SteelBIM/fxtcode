using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_CaseTemp
    {

        /// <summary>
        /// ID
        /// </summary>
        public long CaseID
        {
            get;
            set;
        }
        /// <summary>
        /// TaskID
        /// </summary>
        public long TaskID
        {
            get;
            set;
        }
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int? ProjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 楼宇ID
        /// </summary>
        public int? BuildingId
        {
            get;
            set;
        }
        /// <summary>
        /// 物业ID
        /// </summary>
        public int? HouseId
        {
            get;
            set;
        }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CaseDate
        {
            get;
            set;
        }
        /// <summary>
        /// 用途Code
        /// </summary>
        public int? PurposeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 楼层
        /// </summary>
        public int? FloorNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string BuildingName
        {
            get;
            set;
        }
        /// <summary>
        /// 房号
        /// </summary>
        public string HouseNo
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal? BuildingArea
        {
            get;
            set;
        }
        /// <summary>
        /// 使用面积
        /// </summary>
        public decimal? UsableArea
        {
            get;
            set;
        }
        /// <summary>
        /// 朝向Code
        /// </summary>
        public int? FrontCode
        {
            get;
            set;
        }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 货币单位Code
        /// </summary>
        public int? MoneyUnitCode
        {
            get;
            set;
        }
        /// <summary>
        /// 景观Code
        /// </summary>
        public int? SightCode
        {
            get;
            set;
        }
        /// <summary>
        /// 案例类型
        /// </summary>
        public int? CaseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑结构Code
        /// </summary>
        public int? StructureCode
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑类型Code
        /// </summary>
        public int? BuildingTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 户型Code
        /// </summary>
        public int? HouseTypeCode
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        {
            get;
            set;
        }
        /// <summary>
        /// 案例备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? TotalPrice
        {
            get;
            set;
        }
        /// <summary>
        /// OldID
        /// </summary>
        public int? OldID
        {
            get;
            set;
        }
        /// <summary>
        /// CityID
        /// </summary>
        public int CityID
        {
            get;
            set;
        }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid
        {
            get;
            set;
        }
        /// <summary>
        /// 案例所有公司
        /// </summary>
        public int FXTCompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 总层数
        /// </summary>
        public int? TotalFloor
        {
            get;
            set;
        }
        /// <summary>
        /// 剩余年限
        /// </summary>
        public int? RemainYear
        {
            get;
            set;
        }
        /// <summary>
        /// 成新率
        /// </summary>
        public decimal? Depreciation
        {
            get;
            set;
        }
        /// <summary>
        /// 装修情况(未用到)
        /// </summary>
        public int? FitmentCode
        {
            get;
            set;
        }
        /// <summary>
        /// 查勘ID
        /// </summary>
        public int? SurveyId
        {
            get;
            set;
        }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public DateTime? SaveDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser
        {
            get;
            set;
        }
        /// <summary>
        /// 来源
        /// </summary>
        public string SourceName
        {
            get;
            set;
        }
        /// <summary>
        /// 来源链接
        /// </summary>
        public string SourceLink
        {
            get;
            set;
        }
        /// <summary>
        /// 来源电话
        /// </summary>
        public string SourcePhone
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public int? AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string BuildingDate
        {
            get;
            set;
        }
        /// <summary>
        /// 装修
        /// </summary>
        public string ZhuangXiu
        {
            get;
            set;
        }
        /// <summary>
        /// 附属物业
        /// </summary>
        public string SubHouse
        {
            get;
            set;
        }
        /// <summary>
        /// 配套设施
        /// </summary>
        public string PeiTao
        {
            get;
            set;
        }
        /// <summary>
        /// ErrRemark
        /// </summary>
        public string ErrRemark
        {
            get;
            set;
        }
        /// <summary>
        /// ErrType
        /// </summary>
        public int? ErrType
        {
            get;
            set;
        }

    }
}

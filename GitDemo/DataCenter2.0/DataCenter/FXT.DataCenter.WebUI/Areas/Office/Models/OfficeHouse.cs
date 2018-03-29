using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.WebUI.Areas.Office.Models
{
    public class OfficeHouse
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string BuildingName { get; set; }
        public long BuildingId { get; set; }
        public int TotalFloor { get; set; }
        public int CityId { get; set; }
        public int FxtCompanyId { get; set; }
        public string ProjectAvePrice { get; set; }
        public string BuildingAvePrice { get; set; }
        public string PriceDetail { get; set; }
        public string ProWeight { get; set; }
        public DataTable Houses { get; set; }
        public IQueryable<DatHouseOffice> UnitNos { get; set; }
        public IQueryable<int> FloorNos { get; set; }

    }

    public class OfficeHouseOperate
    {
        public List<OfficeHouseViewModel> AddHouse {get;set; }
    
        public List<OfficeHouseViewModel> UpdateHouse {get;set; }
    
        public List<OfficeHouseViewModel> DeleteHouse {get;set; }

        public List<OfficeHouseViewModel> UpdateFloorNum { get; set; }
    
    }

    public class OfficeHouseViewModel
    {
        /// <summary>
        /// 办公房号表
        /// </summary>
        public long HouseId { get; set; }
        /// <summary>
        /// BuildingId
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 物理层
        /// </summary>
  
        public int FloorNo { get; set; }
        /// <summary>
        /// 实际层
        /// </summary>
 
        public string FloorNum { get; set; }

        /// <summary>
        /// 单元
        /// </summary>
        public string UnitNo { get; set; }

        /// <summary>
        /// 室号
        /// </summary>
        public string HouseNo { get; set; }

        /// <summary>
        /// 房号名称
        /// </summary>

        public string HouseName { get; set; }
        /// <summary>
        /// 证载用途（1002）
        /// </summary>
        public string PurposeCode { get; set; }
        /// <summary>
        /// 实际用途(1002)
        /// </summary>
        public string SJPurposeCode { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
   
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 套内面积(使用面积)
        /// </summary>
      
        public decimal? InnerBuildingArea { get; set; }
        /// <summary>
        /// 朝向2004
        /// </summary>
        public string FrontCode { get; set; }
        /// <summary>
        /// 景观2006
        /// </summary>
        public string SightCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
       
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 价格系数
        /// </summary>
      
        public decimal? Weight { get; set; }
        /// <summary>
        /// 是否可估
        /// </summary>
        public string IsEValue { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class DatProject
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// ProjectName
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// SubAreaId
        /// </summary>
        public int? SubAreaId { get; set; }
        /// <summary>
        /// FieldNo
        /// </summary>
        public string FieldNo { get; set; }
        /// <summary>
        /// PurposeCode
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// LandArea
        /// </summary>
        public double? LandArea { get; set; }
        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// UsableYear
        /// </summary>
        public int? UsableYear { get; set; }
        /// <summary>
        /// BuildingArea
        /// </summary>
        public double? BuildingArea { get; set; }
        /// <summary>
        /// SalableArea
        /// </summary>
        public double? SalableArea { get; set; }
        /// <summary>
        /// CubageRate
        /// </summary>
        public double? CubageRate { get; set; }
        /// <summary>
        /// GreenRate
        /// </summary>
        public double? GreenRate { get; set; }
        /// <summary>
        /// BuildingDate
        /// </summary>
        public DateTime? BuildingDate { get; set; }
        /// <summary>
        /// CoverDate
        /// </summary>
        public DateTime? CoverDate { get; set; }
        /// <summary>
        /// SaleDate
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// JoinDate
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// InnerSaleDate
        /// </summary>
        public DateTime? InnerSaleDate { get; set; }
        /// <summary>
        /// RightCode
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// ParkingNumber
        /// </summary>
        public int? ParkingNumber { get; set; }
        /// <summary>
        /// AveragePrice
        /// </summary>
        public double? AveragePrice { get; set; }
        /// <summary>
        /// ManagerTel
        /// </summary>
        public string ManagerTel { get; set; }
        /// <summary>
        /// ManagerPrice
        /// </summary>
        public string ManagerPrice { get; set; }
        /// <summary>
        /// TotalNum
        /// </summary>
        public int? TotalNum { get; set; }
        /// <summary>
        /// BuildingNum
        /// </summary>
        public int? BuildingNum { get; set; }
        /// <summary>
        /// Detail
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// BuildingTypeCode
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// UpdateDateTime
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }
        /// <summary>
        /// OfficeArea
        /// </summary>
        public double? OfficeArea { get; set; }
        /// <summary>
        /// OtherArea
        /// </summary>
        public double? OtherArea { get; set; }
        /// <summary>
        /// PlanPurpose
        /// </summary>
        public string PlanPurpose { get; set; }
        /// <summary>
        /// PriceDate
        /// </summary>
        public DateTime? PriceDate { get; set; }
        /// <summary>
        /// IsComplete
        /// </summary>
        public int? IsComplete { get; set; }
        /// <summary>
        /// OtherName
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public double? Weight { get; set; }
        /// <summary>
        /// BusinessArea
        /// </summary>
        public double? BusinessArea { get; set; }
        /// <summary>
        /// IndustryArea
        /// </summary>
        public double? IndustryArea { get; set; }
        /// <summary>
        /// IsEValue
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// PinYin
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// CityID
        /// </summary>
        public int? CityID { get; set; }
        /// <summary>
        /// AreaID
        /// </summary>
        public int? AreaID { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public string OldId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// AreaLineId
        /// </summary>
        public int? AreaLineId { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid { get; set; }
        /// <summary>
        /// SalePrice
        /// </summary>
        public double? SalePrice { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int? FxtCompanyId { get; set; }
        /// <summary>
        /// PinYinAll
        /// </summary>
        public string PinYinAll { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double? X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public double? Y { get; set; }
        /// <summary>
        /// XYScale
        /// </summary>
        public int? XYScale { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// IsEmpty
        /// </summary>
        public int? IsEmpty { get; set; }
        /// <summary>
        /// TotalId
        /// </summary>
        public int? TotalId { get; set; }
        /// <summary>
        /// East
        /// </summary>
        public string East { get; set; }
        /// <summary>
        /// West
        /// </summary>
        public string West { get; set; }
        /// <summary>
        /// South
        /// </summary>
        public string South { get; set; }
        /// <summary>
        /// North
        /// </summary>
        public string North { get; set; }
        /// <summary>
        /// BuildingQuality
        /// </summary>
        public int? BuildingQuality { get; set; }
        /// <summary>
        /// HousingScale
        /// </summary>
        public int? HousingScale { get; set; }
        /// <summary>
        /// BuildingDetail
        /// </summary>
        public string BuildingDetail { get; set; }
        /// <summary>
        /// HouseDetail
        /// </summary>
        public string HouseDetail { get; set; }
        /// <summary>
        /// BasementPurpose
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// ManagerQuality
        /// </summary>
        public int? ManagerQuality { get; set; }
        /// <summary>
        /// Facilities
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// AppendageClass
        /// </summary>
        public int? AppendageClass { get; set; }
        /// <summary>
        /// RegionalAnalysis
        /// </summary>
        public string RegionalAnalysis { get; set; }
        /// <summary>
        /// Wrinkle
        /// </summary>
        public string Wrinkle { get; set; }
        /// <summary>
        /// Aversion
        /// </summary>
        public string Aversion { get; set; }        
    }
}

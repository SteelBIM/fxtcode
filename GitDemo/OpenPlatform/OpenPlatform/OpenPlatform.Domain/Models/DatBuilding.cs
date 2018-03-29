using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Domain.Models
{
   public class DatBuilding
    {
        /// <summary>
        /// BuildingId
        /// </summary>
        public int BuildingId { get; set; }
        /// <summary>
        /// BuildingName
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public int? ProjectId { get; set; }
        /// <summary>
        /// PurposeCode
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// StructureCode
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// BuildingTypeCode
        /// </summary>
        public int? BuildingTypeCode { get; set; }
        /// <summary>
        /// TotalFloor
        /// </summary>
        public int? TotalFloor { get; set; }
        /// <summary>
        /// FloorHigh
        /// </summary>
        public double? FloorHigh { get; set; }
        /// <summary>
        /// SaleLicence
        /// </summary>
        public string SaleLicence { get; set; }
        /// <summary>
        /// ElevatorRate
        /// </summary>
        public string ElevatorRate { get; set; }
        /// <summary>
        /// UnitsNumber
        /// </summary>
        public int? UnitsNumber { get; set; }
        /// <summary>
        /// TotalNumber
        /// </summary>
        public int? TotalNumber { get; set; }
        /// <summary>
        /// TotalBuildArea
        /// </summary>
        public double? TotalBuildArea { get; set; }
        /// <summary>
        /// BuildDate
        /// </summary>
        public DateTime? BuildDate { get; set; }
        /// <summary>
        /// SaleDate
        /// </summary>
        public DateTime? SaleDate { get; set; }
        /// <summary>
        /// AveragePrice
        /// </summary>
        public double? AveragePrice { get; set; }
        /// <summary>
        /// AverageFloor
        /// </summary>
        public int? AverageFloor { get; set; }
        /// <summary>
        /// JoinDate
        /// </summary>
        public DateTime? JoinDate { get; set; }
        /// <summary>
        /// LicenceDate
        /// </summary>
        public DateTime? LicenceDate { get; set; }
        /// <summary>
        /// OtherName
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// Weight
        /// </summary>
        public double? Weight { get; set; }
        /// <summary>
        /// IsEValue
        /// </summary>
        public int? IsEValue { get; set; }
        /// <summary>
        /// CityID
        /// </summary>
        public int? CityID { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// OldId
        /// </summary>
        public string OldId { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid { get; set; }
        /// <summary>
        /// SalePrice
        /// </summary>
        public double? SalePrice { get; set; }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// LocationCode
        /// </summary>
        public int? LocationCode { get; set; }
        /// <summary>
        /// SightCode
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// FrontCode
        /// </summary>
        public int? FrontCode { get; set; }
        /// <summary>
        /// StructureWeight
        /// </summary>
        public double? StructureWeight { get; set; }
        /// <summary>
        /// BuildingTypeWeight
        /// </summary>
        public double? BuildingTypeWeight { get; set; }
        /// <summary>
        /// YearWeight
        /// </summary>
        public double? YearWeight { get; set; }
        /// <summary>
        /// PurposeWeight
        /// </summary>
        public double? PurposeWeight { get; set; }
        /// <summary>
        /// LocationWeight
        /// </summary>
        public double? LocationWeight { get; set; }
        /// <summary>
        /// SightWeight
        /// </summary>
        public double? SightWeight { get; set; }
        /// <summary>
        /// FrontWeight
        /// </summary>
        public double? FrontWeight { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int? FxtCompanyId { get; set; }
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
        /// Wall
        /// </summary>
        public int? Wall { get; set; }
        /// <summary>
        /// IsElevator
        /// </summary>
        public int? IsElevator { get; set; }
        /// <summary>
        /// SubAveragePrice
        /// </summary>
        public double? SubAveragePrice { get; set; }
        /// <summary>
        /// PriceDetail
        /// </summary>
        public string PriceDetail { get; set; }
        /// <summary>
        /// BHouseTypeCode
        /// </summary>
        public int? BHouseTypeCode { get; set; }
        /// <summary>
        /// BHouseTypeWeight
        /// </summary>
        public double? BHouseTypeWeight { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// Distance
        /// </summary>
        public int? Distance { get; set; }
        /// <summary>
        /// DistanceWeight
        /// </summary>
        public double? DistanceWeight { get; set; }
        /// <summary>
        /// basement
        /// </summary>
        public int? basement { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ElevatorRateWeight
        /// </summary>
        public double? ElevatorRateWeight { get; set; }
        /// <summary>
        /// IsYard
        /// </summary>
        public int? IsYard { get; set; }
        /// <summary>
        /// YardWeight
        /// </summary>
        public double? YardWeight { get; set; }
        /// <summary>
        /// Doorplate
        /// </summary>
        public string Doorplate { get; set; }
        /// <summary>
        /// RightCode
        /// </summary>
        public int? RightCode { get; set; }
        /// <summary>
        /// IsVirtual
        /// </summary>
        public int? IsVirtual { get; set; }
        /// <summary>
        /// FloorSpread
        /// </summary>
        public string FloorSpread { get; set; }
        /// <summary>
        /// PodiumBuildingFloor
        /// </summary>
        public int? PodiumBuildingFloor { get; set; }
        /// <summary>
        /// PodiumBuildingArea
        /// </summary>
        public double? PodiumBuildingArea { get; set; }
        /// <summary>
        /// TowerBuildingArea
        /// </summary>
        public double? TowerBuildingArea { get; set; }
        /// <summary>
        /// BasementArea
        /// </summary>
        public double? BasementArea { get; set; }
        /// <summary>
        /// BasementPurpose
        /// </summary>
        public string BasementPurpose { get; set; }
        /// <summary>
        /// HouseNumber
        /// </summary>
        public int? HouseNumber { get; set; }
        /// <summary>
        /// HouseArea
        /// </summary>
        public double? HouseArea { get; set; }
        /// <summary>
        /// OtherNumber
        /// </summary>
        public int? OtherNumber { get; set; }
        /// <summary>
        /// OtherArea
        /// </summary>
        public double? OtherArea { get; set; }
        /// <summary>
        /// innerFitmentCode
        /// </summary>
        public int? innerFitmentCode { get; set; }
        /// <summary>
        /// FloorHouseNumber
        /// </summary>
        public int? FloorHouseNumber { get; set; }
        /// <summary>
        /// LiftNumber
        /// </summary>
        public int? LiftNumber { get; set; }
        /// <summary>
        /// LiftBrand
        /// </summary>
        public string LiftBrand { get; set; }
        /// <summary>
        /// Facilities
        /// </summary>
        public string Facilities { get; set; }
        /// <summary>
        /// PipelineGasCode
        /// </summary>
        public int? PipelineGasCode { get; set; }
        /// <summary>
        /// HeatingModeCode
        /// </summary>
        public int? HeatingModeCode { get; set; }
        /// <summary>
        /// WallTypeCode
        /// </summary>
        public int? WallTypeCode { get; set; }
        /// <summary>
        /// orderbyIndex
        /// </summary>
        public int? orderbyIndex { get; set; }        
    }
}

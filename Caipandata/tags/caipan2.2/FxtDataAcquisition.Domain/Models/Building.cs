namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Building
    {
        public int BuildingId { get; set; }
        public Guid? AppId { get; set; }

        public string BuildingName { get; set; }

        public string Doorplate { get; set; }

        public int ProjectId { get; set; }

        public int? PurposeCode { get; set; }

        public int? StructureCode { get; set; }

        public int? BuildingTypeCode { get; set; }

        public int? TotalFloor { get; set; }

        public decimal? FloorHigh { get; set; }

        public string SaleLicence { get; set; }

        public string ElevatorRate { get; set; }

        public int? UnitsNumber { get; set; }

        public int? TotalNumber { get; set; }

        public decimal? TotalBuildArea { get; set; }

        public DateTime? BuildDate { get; set; }

        public DateTime? SaleDate { get; set; }

        public decimal? AveragePrice { get; set; }

        public int? AverageFloor { get; set; }

        public DateTime? JoinDate { get; set; }

        public DateTime? LicenceDate { get; set; }

        public string OtherName { get; set; }

        public decimal? Weight { get; set; }

        public int? IsEValue { get; set; }

        public int CityID { get; set; }

        public DateTime? CreateTime { get; set; }

        public string OldId { get; set; }

        public int? Valid { get; set; }

        public decimal? SalePrice { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }

        public int? LocationCode { get; set; }

        public int? SightCode { get; set; }

        public int? FrontCode { get; set; }

        public decimal? StructureWeight { get; set; }

        public decimal? BuildingTypeWeight { get; set; }

        public decimal? YearWeight { get; set; }

        public decimal? PurposeWeight { get; set; }

        public decimal? LocationWeight { get; set; }

        public decimal? SightWeight { get; set; }

        public decimal? FrontWeight { get; set; }

        public int? FxtCompanyId { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }

        public int? Wall { get; set; }

        public int? IsElevator { get; set; }

        public decimal? SubAveragePrice { get; set; }

        public string PriceDetail { get; set; }

        public int? BHouseTypeCode { get; set; }

        public decimal? BHouseTypeWeight { get; set; }

        public string Creator { get; set; }

        public int? Distance { get; set; }

        public decimal? DistanceWeight { get; set; }

        public int? basement { get; set; }

        public string Remark { get; set; }

        public int? FxtBuildingId { get; set; }

        public int? Status { get; set; }

        public int? MaintenanceCode { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
    }
}

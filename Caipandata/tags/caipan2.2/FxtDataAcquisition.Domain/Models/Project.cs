namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int? SubAreaId { get; set; }

        public string FieldNo { get; set; }

        public int PurposeCode { get; set; }

        public string Address { get; set; }

        public decimal? LandArea { get; set; }

        public DateTime? StartDate { get; set; }

        public int? UsableYear { get; set; }

        public decimal? BuildingArea { get; set; }

        public decimal? SalableArea { get; set; }

        public decimal? CubageRate { get; set; }

        public decimal? GreenRate { get; set; }

        public DateTime? BuildingDate { get; set; }

        public DateTime? CoverDate { get; set; }

        public DateTime? SaleDate { get; set; }

        public DateTime? JoinDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? InnerSaleDate { get; set; }

        public int? RightCode { get; set; }

        public int? ParkingNumber { get; set; }

        public decimal? AveragePrice { get; set; }

        public string ManagerTel { get; set; }

        public string ManagerPrice { get; set; }

        public int? TotalNum { get; set; }

        public int? BuildingNum { get; set; }

        public string Detail { get; set; }

        public int? BuildingTypeCode { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public decimal? OfficeArea { get; set; }

        public decimal? OtherArea { get; set; }

        public string PlanPurpose { get; set; }

        public DateTime? PriceDate { get; set; }

        public int? IsComplete { get; set; }

        public string OtherName { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }

        public decimal? Weight { get; set; }

        public decimal? BusinessArea { get; set; }

        public decimal? IndustryArea { get; set; }

        public int? IsEValue { get; set; }

        public string PinYin { get; set; }

        public int CityID { get; set; }

        public int AreaID { get; set; }

        public string OldId { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? AreaLineId { get; set; }

        public int? Valid { get; set; }

        public decimal? SalePrice { get; set; }

        public int? FxtCompanyId { get; set; }

        public string PinYinAll { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public int? XYScale { get; set; }

        public string Creator { get; set; }

        public int? IsEmpty { get; set; }

        public int? TotalId { get; set; }

        public string East { get; set; }

        public string West { get; set; }

        public string South { get; set; }

        public string North { get; set; }

        public int? FxtProjectId { get; set; }

        public int? Status { get; set; }

        public int? ParkingStatus { get; set; }

        public int? ManagerQuality { get; set; }

        public int? PhotoCount { get; set; }
        
        public virtual ICollection<Building> Buildings { get; set; }

        //public virtual AllotFlow AllotFlow { get; set; }
    }
}

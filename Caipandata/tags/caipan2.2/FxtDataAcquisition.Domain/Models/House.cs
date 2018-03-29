namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class House
    {
        public Guid? AppId { get; set; }
        public int HouseId { get; set; }

        public int BuildingId { get; set; }

        public string HouseName { get; set; }

        public int? HouseTypeCode { get; set; }

        public int FloorNo { get; set; }

        public int? EndFloorNo { get; set; }

        public string NominalFloor { get; set; }

        public string UnitNo { get; set; }

        public decimal? BuildArea { get; set; }

        public int? FrontCode { get; set; }

        public int? SightCode { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? Weight { get; set; }

        public string PhotoName { get; set; }

        public string Remark { get; set; }

        public int? StructureCode { get; set; }

        public decimal? TotalPrice { get; set; }

        public int? PurposeCode { get; set; }

        public int? IsEValue { get; set; }

        public int CityID { get; set; }

        public string OldId { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Valid { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }

        public int? FxtCompanyId { get; set; }

        public short? IsShowBuildingArea { get; set; }

        public decimal? InnerBuildingArea { get; set; }

        public int? SubHouseType { get; set; }

        public decimal? SubHouseArea { get; set; }

        public string Creator { get; set; }

        public int? FxtHouseId { get; set; }

        public int? Status { get; set; }

        public int? VDCode { get; set; }

        public int? NoiseCode { get; set; }

        public virtual Building Building { get; set; }

        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
    }
}

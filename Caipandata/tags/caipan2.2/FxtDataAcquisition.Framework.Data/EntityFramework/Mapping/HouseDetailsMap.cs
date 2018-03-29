using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class HouseDetailsMap : EntityTypeConfiguration<HouseDetails>
    {
        public HouseDetailsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.HouseName).HasMaxLength(20);
            this.Property(t => t.UnitNo).HasMaxLength(20);
            this.Property(t => t.RoomNo).HasMaxLength(20);
            this.Property(t => t.BuildArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.UnitPrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SalePrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.Weight).HasColumnType("numeric").HasPrecision(18, 4);
            this.Property(t => t.PhotoName).HasMaxLength(255);
            this.Property(t => t.TotalPrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.InnerBuildingArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SubHouseArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.Creator).HasMaxLength(50);
            this.Property(t => t.NominalFloor).HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("DAT_HouseDetails");
            this.Property(t => t.HouseId).HasColumnName("Id");
            this.Property(t => t.HouseId).HasColumnName("HouseId");
            this.Property(t => t.HouseName).HasColumnName("HouseName");
            this.Property(t => t.BuildingId).HasColumnName("BuildingId");
            this.Property(t => t.HouseTypeCode).HasColumnName("HouseTypeCode");
            this.Property(t => t.FloorNo).HasColumnName("FloorNo");
            this.Property(t => t.UnitNo).HasColumnName("UnitNo");
            this.Property(t => t.RoomNo).HasColumnName("RoomNo");
            this.Property(t => t.BuildArea).HasColumnName("BuildArea");
            this.Property(t => t.FrontCode).HasColumnName("FrontCode");
            this.Property(t => t.SightCode).HasColumnName("SightCode");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.SalePrice).HasColumnName("SalePrice");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.PhotoName).HasColumnName("PhotoName");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.StructureCode).HasColumnName("StructureCode");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.PurposeCode).HasColumnName("PurposeCode");
            this.Property(t => t.IsEValue).HasColumnName("IsEValue");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.OldId).HasColumnName("OldId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.SaveDateTime).HasColumnName("SaveDateTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.IsShowBuildingArea).HasColumnName("IsShowBuildingArea");
            this.Property(t => t.InnerBuildingArea).HasColumnName("InnerBuildingArea");
            this.Property(t => t.SubHouseType).HasColumnName("SubHouseType");
            this.Property(t => t.SubHouseArea).HasColumnName("SubHouseArea");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.NominalFloor).HasColumnName("NominalFloor");
            this.Property(t => t.VDCode).HasColumnName("VDCode");
            this.Property(t => t.FitmentCode).HasColumnName("FitmentCode");
            this.Property(t => t.Cookroom).HasColumnName("Cookroom");
            this.Property(t => t.Balcony).HasColumnName("Balcony");
            this.Property(t => t.Toilet).HasColumnName("Toilet");
            this.Property(t => t.NoiseCode).HasColumnName("NoiseCode");
            this.Property(t => t.FxtHouseId).HasColumnName("FxtHouseId");

            this.HasRequired(o => o.House).WithMany().HasForeignKey(o => o.HouseId);
            this.HasRequired(o => o.Building).WithMany().HasForeignKey(o => o.BuildingId);
        }
    }
}

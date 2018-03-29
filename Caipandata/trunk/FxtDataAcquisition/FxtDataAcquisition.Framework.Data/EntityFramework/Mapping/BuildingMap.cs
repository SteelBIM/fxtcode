using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class BuildingMap : EntityTypeConfiguration<Building>
    {
        public BuildingMap()
        {
            // Primary Key
            this.HasKey(t => t.BuildingId);

            // Properties
            this.Property(t => t.BuildingId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.BuildingName).HasMaxLength(80);
            this.Property(t => t.BuildingName).IsRequired();
            this.Property(t => t.Doorplate).HasMaxLength(200);
            this.Property(t => t.FloorHigh).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SaleLicence).HasMaxLength(50);
            this.Property(t => t.ElevatorRate).HasMaxLength(50);
            this.Property(t => t.TotalBuildArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.AveragePrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.OtherName).HasMaxLength(50);
            this.Property(t => t.Weight).HasColumnType("numeric").HasPrecision(9, 4);
            this.Property(t => t.SalePrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            //this.Property(t => t.StructureWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.BuildingTypeWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.YearWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.PurposeWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.LocationWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.SightWeight).HasColumnType("numeric").HasPrecision(9, 2);
            //this.Property(t => t.FrontWeight).HasColumnType("numeric").HasPrecision(9, 2);
            this.Property(t => t.X).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Y).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.SubAveragePrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.PriceDetail).HasMaxLength(512);
            //this.Property(t => t.BHouseTypeWeight).HasColumnType("numeric").HasPrecision(9, 2);
            this.Property(t => t.Creator).HasMaxLength(50);
            //this.Property(t => t.DistanceWeight).HasColumnType("numeric").HasPrecision(9, 2);
            this.Property(t => t.Remark).HasMaxLength(500);
            this.Property(t => t.FloorSpread).HasMaxLength(200);
            this.Property(t => t.Facilities).HasMaxLength(200);
            this.Property(t => t.PodiumBuildingArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.TowerBuildingArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.BasementArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.HouseArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.OtherArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.LiftBrand).HasMaxLength(50);
            this.Property(t => t.BasementPurpose).HasMaxLength(200);
            
            // Table & Column Mappings
            this.ToTable("DAT_Building");
            this.Property(t => t.BuildingId).HasColumnName("BuildingId");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.BuildingName).HasColumnName("BuildingName");
            this.Property(t => t.Doorplate).HasColumnName("Doorplate");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.PurposeCode).HasColumnName("PurposeCode");
            this.Property(t => t.StructureCode).HasColumnName("StructureCode");
            this.Property(t => t.BuildingTypeCode).HasColumnName("BuildingTypeCode");
            this.Property(t => t.TotalFloor).HasColumnName("TotalFloor");
            this.Property(t => t.FloorHigh).HasColumnName("FloorHigh");
            this.Property(t => t.SaleLicence).HasColumnName("SaleLicence");
            this.Property(t => t.ElevatorRate).HasColumnName("ElevatorRate");
            this.Property(t => t.UnitsNumber).HasColumnName("UnitsNumber");
            this.Property(t => t.TotalNumber).HasColumnName("TotalNumber");
            this.Property(t => t.TotalBuildArea).HasColumnName("TotalBuildArea");
            this.Property(t => t.BuildDate).HasColumnName("BuildDate");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.AveragePrice).HasColumnName("AveragePrice");
            this.Property(t => t.AverageFloor).HasColumnName("AverageFloor");
            this.Property(t => t.JoinDate).HasColumnName("JoinDate");
            this.Property(t => t.LicenceDate).HasColumnName("LicenceDate");
            this.Property(t => t.AveragePrice).HasColumnName("AveragePrice");
            this.Property(t => t.OtherName).HasColumnName("OtherName");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.IsEValue).HasColumnName("IsEValue");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.SalePrice).HasColumnName("SalePrice");
            this.Property(t => t.SaveDateTime).HasColumnName("SaveDateTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.LocationCode).HasColumnName("LocationCode");
            this.Property(t => t.SightCode).HasColumnName("SightCode");
            this.Property(t => t.FrontCode).HasColumnName("FrontCode");
            //this.Property(t => t.StructureWeight).HasColumnName("StructureWeight");
            //this.Property(t => t.BuildingTypeWeight).HasColumnName("BuildingTypeWeight");
            //this.Property(t => t.YearWeight).HasColumnName("YearWeight");
            //this.Property(t => t.PurposeWeight).HasColumnName("PurposeWeight");
            //this.Property(t => t.LocationWeight).HasColumnName("LocationWeight");
            //this.Property(t => t.SightWeight).HasColumnName("SightWeight");
            //this.Property(t => t.FrontWeight).HasColumnName("FrontWeight");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.X).HasColumnName("X");
            this.Property(t => t.Y).HasColumnName("Y");
            this.Property(t => t.XYScale).HasColumnName("XYScale");
            this.Property(t => t.Wall).HasColumnName("Wall");
            this.Property(t => t.IsElevator).HasColumnName("IsElevator");
            this.Property(t => t.SubAveragePrice).HasColumnName("SubAveragePrice");
            this.Property(t => t.PriceDetail).HasColumnName("PriceDetail");
            this.Property(t => t.BHouseTypeCode).HasColumnName("BHouseTypeCode");
            //this.Property(t => t.BHouseTypeWeight).HasColumnName("BHouseTypeWeight");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Distance).HasColumnName("Distance");
            //this.Property(t => t.DistanceWeight).HasColumnName("DistanceWeight");
            //this.Property(t => t.Basement).HasColumnName("Basement");
            this.Property(t => t.IsVirtual).HasColumnName("IsVirtual");
            this.Property(t => t.FloorSpread).HasColumnName("FloorSpread");
            this.Property(t => t.PodiumBuildingFloor).HasColumnName("PodiumBuildingFloor");
            this.Property(t => t.PodiumBuildingArea).HasColumnName("PodiumBuildingArea");
            this.Property(t => t.TowerBuildingArea).HasColumnName("TowerBuildingArea");
            this.Property(t => t.BasementArea).HasColumnName("BasementArea");
            this.Property(t => t.Facilities).HasColumnName("Facilities");
            this.Property(t => t.HouseNumber).HasColumnName("HouseNumber");
            this.Property(t => t.HouseArea).HasColumnName("HouseArea");
            this.Property(t => t.OtherNumber).HasColumnName("OtherNumber");
            this.Property(t => t.OtherArea).HasColumnName("OtherArea");
            this.Property(t => t.InnerFitmentCode).HasColumnName("InnerFitmentCode");
            this.Property(t => t.FloorHouseNumber).HasColumnName("FloorHouseNumber");
            this.Property(t => t.LiftNumber).HasColumnName("LiftNumber");
            this.Property(t => t.LiftBrand).HasColumnName("LiftBrand");
            this.Property(t => t.PipelineGasCode).HasColumnName("PipelineGasCode");
            this.Property(t => t.HeatingModeCode).HasColumnName("HeatingModeCode");
            this.Property(t => t.WallTypeCode).HasColumnName("WallTypeCode");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.FxtBuildingId).HasColumnName("FxtBuildingId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.MaintenanceCode).HasColumnName("MaintenanceCode");
            this.Property(t => t.BasementPurpose).HasColumnName("BasementPurpose");
            this.Property(t => t.TempletId).HasColumnName("TempletId");

            this.HasRequired(o => o.Project).WithMany().HasForeignKey(o => o.ProjectId);
            this.HasMany(o => o.Houses).WithRequired(o => o.Building);
            this.HasMany(o => o.HouseDetails).WithRequired(o => o.Building);
            this.HasRequired(o => o.Templet).WithMany().HasForeignKey(o => o.TempletId);

        }
    }
}

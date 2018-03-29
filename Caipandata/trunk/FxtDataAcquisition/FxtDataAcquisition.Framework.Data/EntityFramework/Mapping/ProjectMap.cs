using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class ProjectMap : EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectId);

            // Properties
            this.Property(t => t.ProjectId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.ProjectName).HasMaxLength(80);
            this.Property(t => t.ProjectName).IsRequired();
            this.Property(t => t.FieldNo).HasMaxLength(100);
            this.Property(t => t.Address).HasMaxLength(600);
            this.Property(t => t.LandArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.BuildingArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.SalableArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.CubageRate).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.GreenRate).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.AveragePrice).HasColumnType("numeric");
            this.Property(t => t.ManagerTel).HasMaxLength(50);
            this.Property(t => t.ManagerPrice).HasMaxLength(50);
            this.Property(t => t.OfficeArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.OtherArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.PlanPurpose).HasMaxLength(50);
            this.Property(t => t.OtherName).HasMaxLength(80);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.Weight).HasColumnType("numeric").HasPrecision(8, 4);
            this.Property(t => t.BusinessArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.IndustryArea).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.PinYin).HasMaxLength(50);
            this.Property(t => t.OldId).HasMaxLength(100);
            this.Property(t => t.SalePrice).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.PinYinAll).HasMaxLength(500);
            this.Property(t => t.X).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Y).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Creator).HasMaxLength(50);
            this.Property(t => t.East).HasMaxLength(100);
            this.Property(t => t.West).HasMaxLength(100);
            this.Property(t => t.South).HasMaxLength(100);
            this.Property(t => t.North).HasMaxLength(100);
            this.Property(t => t.BasementPurpose).HasMaxLength(200);
            this.Property(t => t.Facilities).HasMaxLength(200);
            this.Property(t => t.BuildingDetail).HasMaxLength(500);
            this.Property(t => t.HouseDetail).HasMaxLength(500);
            this.Property(t => t.RegionalAnalysis).HasMaxLength(500);
            this.Property(t => t.Wrinkle).HasMaxLength(200);
            this.Property(t => t.Aversion).HasMaxLength(200);
            this.Property(t => t.ParkingDesc).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("DAT_Project");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.SubAreaId).HasColumnName("SubAreaId");
            this.Property(t => t.FieldNo).HasColumnName("FieldNo");
            this.Property(t => t.PurposeCode).HasColumnName("PurposeCode");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.LandArea).HasColumnName("LandArea");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.UsableYear).HasColumnName("UsableYear");
            this.Property(t => t.BuildingArea).HasColumnName("BuildingArea");
            this.Property(t => t.SalableArea).HasColumnName("SalableArea");
            this.Property(t => t.CubageRate).HasColumnName("CubageRate");
            this.Property(t => t.GreenRate).HasColumnName("GreenRate");
            this.Property(t => t.BuildingDate).HasColumnName("BuildingDate");
            this.Property(t => t.CoverDate).HasColumnName("CoverDate");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.JoinDate).HasColumnName("JoinDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.InnerSaleDate).HasColumnName("InnerSaleDate");
            this.Property(t => t.RightCode).HasColumnName("RightCode");
            this.Property(t => t.ParkingNumber).HasColumnName("ParkingNumber");
            this.Property(t => t.AveragePrice).HasColumnName("AveragePrice");
            this.Property(t => t.ManagerTel).HasColumnName("ManagerTel");
            this.Property(t => t.ManagerPrice).HasColumnName("ManagerPrice");
            this.Property(t => t.TotalNum).HasColumnName("TotalNum");
            this.Property(t => t.BuildingNum).HasColumnName("BuildingNum");
            this.Property(t => t.Detail).HasColumnName("Detail");
            this.Property(t => t.BuildingTypeCode).HasColumnName("BuildingTypeCode");
            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
            this.Property(t => t.OfficeArea).HasColumnName("OfficeArea");
            this.Property(t => t.OtherArea).HasColumnName("OtherArea");
            this.Property(t => t.PlanPurpose).HasColumnName("PlanPurpose");
            this.Property(t => t.PriceDate).HasColumnName("PriceDate");
            this.Property(t => t.OtherName).HasColumnName("OtherName");
            this.Property(t => t.SaveDateTime).HasColumnName("SaveDateTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.BusinessArea).HasColumnName("BusinessArea");
            this.Property(t => t.IndustryArea).HasColumnName("IndustryArea");
            this.Property(t => t.IsEValue).HasColumnName("IsEValue");
            this.Property(t => t.PinYin).HasColumnName("PinYin");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.OldId).HasColumnName("OldId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.AreaLineId).HasColumnName("AreaLineId");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.SalePrice).HasColumnName("SalePrice");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.PinYinAll).HasColumnName("PinYinAll");
            this.Property(t => t.X).HasColumnName("X");
            this.Property(t => t.Y).HasColumnName("Y");
            this.Property(t => t.XYScale).HasColumnName("XYScale");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.IsEmpty).HasColumnName("IsEmpty");
            this.Property(t => t.TotalId).HasColumnName("TotalId");
            this.Property(t => t.East).HasColumnName("East");
            this.Property(t => t.West).HasColumnName("West");
            this.Property(t => t.South).HasColumnName("South");
            this.Property(t => t.North).HasColumnName("North");
            this.Property(t => t.FxtProjectId).HasColumnName("FxtProjectId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ParkingStatus).HasColumnName("ParkingStatus");
            this.Property(t => t.ManagerQuality).HasColumnName("ManagerQuality");
            this.Property(t => t.PhotoCount).HasColumnName("PhotoCount");
            this.Property(t => t.BuildingQuality).HasColumnName("BuildingQuality");
            this.Property(t => t.HousingScale).HasColumnName("HousingScale");
            this.Property(t => t.AppendageClass).HasColumnName("AppendageClass");
            this.Property(t => t.BasementPurpose).HasColumnName("BasementPurpose");
            this.Property(t => t.Facilities).HasColumnName("Facilities");
            this.Property(t => t.BuildingDetail).HasColumnName("BuildingDetail");
            this.Property(t => t.HouseDetail).HasColumnName("HouseDetail");
            this.Property(t => t.RegionalAnalysis).HasColumnName("RegionalAnalysis");
            this.Property(t => t.Wrinkle).HasColumnName("Wrinkle");
            this.Property(t => t.Aversion).HasColumnName("Aversion");
            this.Property(t => t.ParkingDesc).HasColumnName("ParkingDesc");

            //this.HasRequired(t => t.AllotFlow).WithOptional(t => t.Project);
            this.HasMany(t => t.Buildings).WithRequired(t => t.Project);
            this.HasMany(t => t.Appendages).WithRequired(t => t.Project);
            this.HasMany(t => t.Companys).WithRequired(t => t.Project);
            this.HasRequired(o => o.Templet).WithMany().HasForeignKey(o => o.TempletId);

        }
    }
}

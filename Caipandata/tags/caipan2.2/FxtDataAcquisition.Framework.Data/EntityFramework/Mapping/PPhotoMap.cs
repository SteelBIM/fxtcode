using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class PPhotoMap : EntityTypeConfiguration<PPhoto>
    {
        public PPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Path).HasMaxLength(200);
            this.Property(t => t.PhotoName).HasMaxLength(100);
            this.Property(t => t.X).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Y).HasColumnType("numeric").HasPrecision(18, 14);
            // Table & Column Mappings
            this.ToTable("LNK_P_Photo");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.PhotoTypeCode).HasColumnName("PhotoTypeCode");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.PhotoDate).HasColumnName("PhotoDate");
            this.Property(t => t.PhotoName).HasColumnName("PhotoName");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.BuildingId).HasColumnName("BuildingId");
            this.Property(t => t.X).HasColumnName("X");
            this.Property(t => t.Y).HasColumnName("Y");

        }
    }
}

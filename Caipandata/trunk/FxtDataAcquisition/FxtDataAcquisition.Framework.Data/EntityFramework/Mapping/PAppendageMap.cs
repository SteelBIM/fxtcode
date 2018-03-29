using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class PAppendageMap : EntityTypeConfiguration<PAppendage>
    {
        public PAppendageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Area).HasColumnType("numeric").HasPrecision(18, 2);
            this.Property(t => t.P_AName).HasMaxLength(255);
            this.Property(t => t.Address).HasMaxLength(255);
            this.Property(t => t.X).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Y).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Uid).HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("LNK_P_Appendage");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.AppendageCode).HasColumnName("AppendageCode");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.Area).HasColumnName("Area");
            this.Property(t => t.P_AName).HasColumnName("P_AName");
            this.Property(t => t.IsInner).HasColumnName("IsInner");
            this.Property(t => t.ClassCode).HasColumnName("ClassCode");
            this.Property(t => t.Distance).HasColumnName("Distance");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Uid).HasColumnName("Uid");
            this.Property(t => t.X).HasColumnName("X");
            this.Property(t => t.Y).HasColumnName("Y");

            this.HasRequired(o => o.Project).WithMany().HasForeignKey(o =>  o.ProjectId );
        }
    }
}

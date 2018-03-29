using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class CheckMap : EntityTypeConfiguration<Check>
    {
        public CheckMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.OtherId).HasMaxLength(50);
            this.Property(t => t.CheckRemark1).HasMaxLength(200);
            this.Property(t => t.CheckUserName1).HasMaxLength(50);
            this.Property(t => t.CheckRemark2).HasMaxLength(200);
            this.Property(t => t.CheckUserName2).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("Dat_Check");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.AllotId).HasColumnName("AllotId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.DatType).HasColumnName("DatType");
            this.Property(t => t.DatId).HasColumnName("DatId");
            this.Property(t => t.OtherId).HasColumnName("OtherId");
            this.Property(t => t.CheckUserName1).HasColumnName("CheckUserName1");
            this.Property(t => t.CheckState1).HasColumnName("CheckState1");
            this.Property(t => t.CheckRemark1).HasColumnName("CheckRemark1");
            this.Property(t => t.CheckDate1).HasColumnName("CheckDate1");
            this.Property(t => t.CheckUserName2).HasColumnName("CheckUserName2");
            this.Property(t => t.CheckState2).HasColumnName("CheckState2");
            this.Property(t => t.CheckRemark2).HasColumnName("CheckRemark2");
            this.Property(t => t.CheckDate2).HasColumnName("CheckDate2");

            this.HasRequired(o => o.AllotFlow).WithMany().HasForeignKey(o => o.AllotId);
        }
    }
}

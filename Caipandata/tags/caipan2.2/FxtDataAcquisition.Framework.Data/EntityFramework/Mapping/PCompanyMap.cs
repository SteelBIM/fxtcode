using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class PCompanyMap : EntityTypeConfiguration<PCompany>
    {
        public PCompanyMap()
        {
            //Primary Key
            this.HasKey(t => new { t.ProjectId, t.CompanyType, t.CityId });
            //this.HasKey(t => t.CompanyType);
            //this.HasKey(t => t.CityId);
            
            // Properties
            this.Property(t => t.ProjectId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.ProjectId).HasColumnOrder(0);
            this.Property(t => t.CompanyType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.CompanyType).HasColumnOrder(1);
            this.Property(t => t.CityId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.CityId).HasColumnOrder(2);
            this.Property(t => t.CompanyName).HasMaxLength(200);
            this.Property(t => t.CompanyName).IsRequired();
            // Table & Column Mappings
            this.ToTable("LNK_P_Company");
            this.Property(t => t.ProjectId).HasColumnName("ProjectId");
            this.Property(t => t.CompanyType).HasColumnName("CompanyType");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
        }
    }
}

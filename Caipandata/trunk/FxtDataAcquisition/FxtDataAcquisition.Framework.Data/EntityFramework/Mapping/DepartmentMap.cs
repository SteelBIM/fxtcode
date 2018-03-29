using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class DepartmentMap : EntityTypeConfiguration<Department>
    {
        public DepartmentMap()
        {
            // Primary Key
            this.HasKey(t => t.DepartmentId);

            // Properties
            this.Property(t => t.DepartmentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.DepartmentName).HasMaxLength(50);
            this.Property(t => t.DepartmentName).IsRequired();
            this.Property(t => t.Address).HasMaxLength(100);
            this.Property(t => t.Fax).HasMaxLength(50);
            this.Property(t => t.Telephone).HasMaxLength(50);
            this.Property(t => t.EMail).HasMaxLength(50);
            this.Property(t => t.LinkMan).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("Privi_Department");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.Fk_CompanyId).HasColumnName("Fk_CompanyId");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.FK_CityId).HasColumnName("FK_CityId");
            this.Property(t => t.FK_DepTypeCode).HasColumnName("FK_DepTypeCode");
            this.Property(t => t.FK_ParentId).HasColumnName("FK_ParentId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.Telephone).HasColumnName("Telephone");
            this.Property(t => t.EMail).HasColumnName("EMail");
            this.Property(t => t.LinkMan).HasColumnName("LinkMan");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.DValid).HasColumnName("DValid");
            this.Property(t => t.FK_DepAttr).HasColumnName("FK_DepAttr");

            this.HasMany(m => m.DepartmentUsers).WithRequired(m => m.Department);

        }
    }
}

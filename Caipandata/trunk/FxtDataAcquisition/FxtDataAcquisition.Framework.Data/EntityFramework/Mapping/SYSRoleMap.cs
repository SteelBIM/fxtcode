using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSRoleMap : EntityTypeConfiguration<SYS_Role>
    {
        public SYSRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.RoleName).IsRequired();
            this.Property(t => t.RoleName).HasMaxLength(50);
            this.Property(t => t.Remarks).HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("SYS_Role");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.FxtCompanyID).HasColumnName("FxtCompanyID");

            this.HasMany(m => m.RoleUsers).WithRequired(m=>m.Role);
            this.HasMany(m => m.RoleMenus).WithRequired(m => m.Role).HasForeignKey(m=>m.RoleID);
        }
    }
}

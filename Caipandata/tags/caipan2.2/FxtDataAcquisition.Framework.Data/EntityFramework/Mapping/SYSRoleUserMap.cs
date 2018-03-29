using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSRoleUserMap : EntityTypeConfiguration<SYS_Role_User>
    {
        public SYSRoleUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.UserName).IsRequired();
            this.Property(t => t.TrueName).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("SYS_Role_User");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.FxtCompanyID).HasColumnName("FxtCompanyID");
            this.Property(t => t.TrueName).HasColumnName("TrueName");

            this.HasRequired(m => m.Role).WithMany().HasForeignKey(m => m.RoleID);
        }
    }
}

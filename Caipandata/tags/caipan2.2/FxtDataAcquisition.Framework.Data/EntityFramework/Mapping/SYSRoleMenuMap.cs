using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSRoleMenuMap : EntityTypeConfiguration<SYS_Role_Menu>
    {
        public SYSRoleMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            // Table & Column Mappings
            this.ToTable("SYS_Role_Menu");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.FxtCompanyID).HasColumnName("FxtCompanyID");

            this.HasRequired(m => m.Role).WithMany().HasForeignKey(m => m.RoleID);
            this.HasRequired(m => m.Menu).WithMany().HasForeignKey(m => m.MenuID);
            this.HasMany(m => m.Functions).WithRequired(m => m.RoleMenu).HasForeignKey(m => m.RoleMenuID);
        }
    }
}

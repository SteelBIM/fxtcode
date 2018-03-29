using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSMenuMap : EntityTypeConfiguration<SYS_Menu>
    {
        public SYSMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.MenuName).HasMaxLength(50);
            this.Property(t => t.MenuName).IsRequired();
            this.Property(t => t.Remark).HasMaxLength(200);
            this.Property(t => t.URL).HasMaxLength(200);
            this.Property(t => t.URL).IsRequired();
            this.Property(t => t.IconClass).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("SYS_Menu");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.ModuleCode).HasColumnName("ModuleCode");
            this.Property(t => t.IconClass).HasColumnName("IconClass");

            this.HasMany(m => m.RoleMenus).WithRequired(m => m.Menu).HasForeignKey(m => m.MenuID);
        }
    }
}

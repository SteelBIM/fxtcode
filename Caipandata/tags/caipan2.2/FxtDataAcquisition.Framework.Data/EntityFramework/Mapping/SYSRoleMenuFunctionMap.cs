using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSRoleMenuFunctionMap : EntityTypeConfiguration<SYS_Role_Menu_Function>
    {
        public SYSRoleMenuFunctionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            // Table & Column Mappings
            this.ToTable("SYS_Role_Menu_Function");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.RoleMenuID).HasColumnName("RoleMenuID");
            this.Property(t => t.FunctionCode).HasColumnName("FunctionCode");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.FxtCompanyID).HasColumnName("FxtCompanyID");

            this.HasRequired(m => m.RoleMenu).WithMany().HasForeignKey(m => m.RoleMenuID);
        }
    }
}

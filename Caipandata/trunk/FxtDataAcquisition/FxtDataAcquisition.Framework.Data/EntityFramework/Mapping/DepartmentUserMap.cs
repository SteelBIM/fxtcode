using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class DepartmentUserMap : EntityTypeConfiguration<DepartmentUser>
    {
        public DepartmentUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.UserName).IsRequired();
            // Table & Column Mappings
            this.ToTable("Privi_Department_User");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.CityID).HasColumnName("CityID");
            this.Property(t => t.FxtCompanyID).HasColumnName("FxtCompanyID");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.HasRequired(o => o.Department).WithMany().HasForeignKey(o => o.DepartmentID);

        }
    }
}

using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSUserInfoMap : EntityTypeConfiguration<SYS_UserInfo>
    {
        public SYSUserInfoMap()
        {
            // Primary Key            
            this.HasKey(t => t.UserName);
            // Properties
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.IconUrl).HasMaxLength(200);
            this.Property(t => t.UpdateUser).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("SYS_UserInfo");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.IconUrl).HasColumnName("IconUrl");
            this.Property(t => t.Valid).HasColumnName("Valid");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");

        }
    }
}

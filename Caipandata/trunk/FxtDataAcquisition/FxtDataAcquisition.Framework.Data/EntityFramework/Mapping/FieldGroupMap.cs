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
    public class FieldGroupMap : EntityTypeConfiguration<FieldGroup>
    {
        public FieldGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.FieldGroupId);

            // Properties
            this.Property(t => t.FieldGroupId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.FieldGroupName).HasMaxLength(50);
            this.Property(t => t.AddUser).HasMaxLength(50);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.DelUser).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DAT_FieldGroup");
            this.Property(t => t.FieldGroupId).HasColumnName("FieldGroupId");
            this.Property(t => t.FieldGroupName).HasColumnName("FieldGroupName");
            this.Property(t => t.TempletId).HasColumnName("TempletId");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.AddUser).HasColumnName("AddUser");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.SaveTime).HasColumnName("SaveTime");
            this.Property(t => t.DelUser).HasColumnName("DelUser");
            this.Property(t => t.DelTime).HasColumnName("DelTime");
            this.Property(t => t.Vaild).HasColumnName("Vaild");

            //配置表的外键
            this.HasMany(t => t.Fields).WithRequired(t => t.FieldGroup);
            this.HasRequired(o => o.Templet).WithMany().HasForeignKey(o => o.TempletId);
        }
    }
}

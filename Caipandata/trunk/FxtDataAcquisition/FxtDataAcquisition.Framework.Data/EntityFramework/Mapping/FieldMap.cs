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
    public class FieldMap : EntityTypeConfiguration<Field>
    {
        public FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.FieldId);

            // Properties
            this.Property(t => t.FieldId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.FieldName).HasMaxLength(50);
            this.Property(t => t.AddUser).HasMaxLength(50);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.DelUser).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DAT_Field");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.FieldGroupId).HasColumnName("FieldGroupId");
            this.Property(t => t.FieldType).HasColumnName("FieldType");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");
            this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            this.Property(t => t.MinLength).HasColumnName("MinLength");
            this.Property(t => t.IsRequired).HasColumnName("IsRequired");
            this.Property(t => t.EdiTextType).HasColumnName("EdiTextType");
            this.Property(t => t.IsSelect).HasColumnName("IsSelect");
            this.Property(t => t.IsNull).HasColumnName("IsNull");
            this.Property(t => t.AddUser).HasColumnName("AddUser");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.SaveTime).HasColumnName("SaveTime");
            this.Property(t => t.DelUser).HasColumnName("DelUser");
            this.Property(t => t.DelTime).HasColumnName("DelTime");
            this.Property(t => t.Vaild).HasColumnName("Vaild");

            //配置表的外键
            this.HasRequired(o => o.FieldGroup).WithMany().HasForeignKey(o => o.FieldGroupId);
        }
    }
}

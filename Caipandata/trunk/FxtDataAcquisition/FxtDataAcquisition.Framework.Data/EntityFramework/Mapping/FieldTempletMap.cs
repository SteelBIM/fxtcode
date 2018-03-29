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
    public class FieldTempletMap : EntityTypeConfiguration<FieldTemplet>
    {
        public FieldTempletMap()
        {
            // Primary Key
            this.HasKey(t => t.FieldTempletId);

            // Properties
            this.Property(t => t.FieldTempletId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.FieldName).HasMaxLength(50);
            this.Property(t => t.AddUser).HasMaxLength(50);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.DelUser).HasMaxLength(50);
            this.Property(t => t.Type).HasMaxLength(20);
            this.Property(t => t.DefaultValue).HasMaxLength(500);
            this.Property(t => t.Choise).HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("DAT_FieldTemplet");
            this.Property(t => t.FieldTempletId).HasColumnName("FieldTempletId");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.FieldType).HasColumnName("FieldType");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.DefaultValue).HasColumnName("DefaultValue");
            this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            this.Property(t => t.IsRequire).HasColumnName("IsRequire");
            this.Property(t => t.EdiTextType).HasColumnName("EdiTextType");
            this.Property(t => t.Choise).HasColumnName("Choise");
            this.Property(t => t.IsSelect).HasColumnName("IsSelect");
            this.Property(t => t.AddUser).HasColumnName("AddUser");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.SaveTime).HasColumnName("SaveTime");
            this.Property(t => t.DelUser).HasColumnName("DelUser");
            this.Property(t => t.DelTime).HasColumnName("DelTime");
            this.Property(t => t.Vaild).HasColumnName("Vaild");

        }
    }
}

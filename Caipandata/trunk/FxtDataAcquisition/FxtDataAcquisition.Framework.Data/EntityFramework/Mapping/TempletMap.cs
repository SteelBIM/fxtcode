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
    public class TempletMap : EntityTypeConfiguration<Templet>
    {
        public TempletMap()
        {
            // Primary Key
            this.HasKey(t => t.TempletId);

            // Properties
            this.Property(t => t.TempletId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.TempletName).HasMaxLength(50);
            this.Property(t => t.AddUser).HasMaxLength(50);
            this.Property(t => t.SaveUser).HasMaxLength(50);
            this.Property(t => t.DelUser).HasMaxLength(50);
          
            // Table & Column Mappings
            this.ToTable("DAT_Templet");
            this.Property(t => t.TempletId).HasColumnName("TempletId");
            this.Property(t => t.TempletName).HasColumnName("TempletName");
            this.Property(t => t.DatType).HasColumnName("DatType");
            this.Property(t => t.AddUser).HasColumnName("AddUser");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.SaveUser).HasColumnName("SaveUser");
            this.Property(t => t.SaveTime).HasColumnName("SaveTime");
            this.Property(t => t.DelUser).HasColumnName("DelUser");
            this.Property(t => t.DelTime).HasColumnName("DelTime");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.Vaild).HasColumnName("Vaild");
            this.Property(t => t.IsCurrent).HasColumnName("IsCurrent");

            //配置表的外键
            this.HasMany(t => t.FieldGroups).WithRequired(t => t.Templet);
        }
    }
}

using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class SYSCodeMap : EntityTypeConfiguration<SYSCode>
    {
        public SYSCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.CodeName).HasMaxLength(50);
            this.Property(t => t.CodeName).IsRequired();
            this.Property(t => t.CodeType).HasMaxLength(50);
            this.Property(t => t.Remark).HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("SYS_Code");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CodeName).HasColumnName("CodeName");
            this.Property(t => t.CodeType).HasColumnName("CodeType");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.SubCode).HasColumnName("SubCode");
        }
    }
}

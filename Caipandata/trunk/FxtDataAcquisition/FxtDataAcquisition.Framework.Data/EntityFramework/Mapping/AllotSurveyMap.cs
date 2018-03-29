using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class AllotSurveyMap : EntityTypeConfiguration<AllotSurvey>
    {
        public AllotSurveyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.UserName).IsRequired();
            this.Property(t => t.Remark).HasMaxLength(200);
            this.Property(t => t.TrueName).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("Dat_AllotSurvey");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.AllotId).HasColumnName("AllotId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.FxtCompanyId).HasColumnName("FxtCompanyId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.StateCode).HasColumnName("StateCode");
            this.Property(t => t.StateDate).HasColumnName("StateDate");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.TrueName).HasColumnName("TrueName");

            this.HasRequired(o => o.AllotFlow).WithMany().HasForeignKey(o => o.AllotId);
            //Has方法：

            //HasOptional：前者包含后者一个实例或者为null
            //HasRequired：前者(A)包含后者(B)一个不为null的实例
            //HasMany：前者包含后者实例的集合
            //With方法：

            //WithOptional：后者(B)可以包含前者(A)一个实例或者null
            //WithRequired：后者包含前者一个不为null的实例
            //WithMany：后者包含前者实例的集合
        }
    }
}

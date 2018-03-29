using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework.Mapping
{
    public class AllotFlowMap : EntityTypeConfiguration<AllotFlow>
    {
        public AllotFlowMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.OtherId).HasMaxLength(50);
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.SurveyUserName).HasMaxLength(50);
            this.Property(t => t.Remark).HasMaxLength(1024);
            this.Property(t => t.X).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.Y).HasColumnType("numeric").HasPrecision(18, 14);
            this.Property(t => t.UserTrueName).HasMaxLength(50);
            this.Property(t => t.SurveyUserTrueName).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("Dat_AllotFlow");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.DatType).HasColumnName("DatType");
            this.Property(t => t.DatId).HasColumnName("DatId");
            this.Property(t => t.OtherId).HasColumnName("OtherId");
            this.Property(t => t.StateCode).HasColumnName("StateCode");
            this.Property(t => t.StateDate).HasColumnName("StateDate");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.SurveyUserName).HasColumnName("SurveyUserName");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.X).HasColumnName("X");
            this.Property(t => t.Y).HasColumnName("Y");
            this.Property(t => t.UserTrueName).HasColumnName("UserTrueName");
            this.Property(t => t.SurveyUserTrueName).HasColumnName("SurveyUserTrueName");

            //配置表的外键
            this.HasRequired(o => o.Project).WithMany().HasForeignKey(o => o.DatId);
            this.HasMany(t => t.AllotSurveys).WithRequired(t => t.AllotFlow);
            this.HasMany(t => t.Checks).WithRequired(t => t.AllotFlow);
        }
    }
}

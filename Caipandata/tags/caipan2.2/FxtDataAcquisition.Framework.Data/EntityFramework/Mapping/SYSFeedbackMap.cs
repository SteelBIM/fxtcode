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
    public class SYSFeedbackMap : EntityTypeConfiguration<Feedback>
    {
        public SYSFeedbackMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.Content).HasMaxLength(500);
            // Table & Column Mappings
            this.ToTable("SYS_Feedback");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.UserName).HasColumnName("UserName");
        }
    }
}

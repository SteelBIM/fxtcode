using System;
using CBSS.Framework.DAL;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using CBSS.Tbx.Contract;
using CBSS.Core.Log;

namespace CBSS.Tbx.DAL
{
    public class TbxDbContext 
    {
        //public TbxDbContext()
        //    : base(CachedConfigContext.Current.DaoConfig.Tbx, new LogDbContext())
        //{
        //}

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<TbxDbContext>(null);

        //    modelBuilder.Entity<Article>()
        //        .HasMany(e => e.Tags)
        //        .WithMany(e => e.Articles)
        //        .Map(m =>
        //        {
        //            m.ToTable("ArticleTag");
        //            m.MapLeftKey("ArticleId");
        //            m.MapRightKey("TagId");
        //        });

        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<Article> Articles { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}

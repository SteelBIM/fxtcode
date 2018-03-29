namespace FxtDataAcquisition.Framework.Data.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Framework.Data.EntityFramework.Mapping;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class FxtDataAcquisitionContext : DbContext
    {
        public FxtDataAcquisitionContext()
            : base("name=EFConnection")
        {
        }

        public virtual DbSet<AllotFlow> Dat_AllotFlow { get; set; }
        public virtual DbSet<AllotSurvey> Dat_AllotSurvey { get; set; }
        public virtual DbSet<Building> DAT_Building { get; set; }
        public virtual DbSet<Check> Dat_Check { get; set; }
        public virtual DbSet<House> DAT_House { get; set; }
        public virtual DbSet<Project> DAT_Project { get; set; }
        public virtual DbSet<PAppendage> LNK_P_Appendage { get; set; }
        public virtual DbSet<PCompany> LNK_P_Company { get; set; }
        public virtual DbSet<PPhoto> LNK_P_Photo { get; set; }
        public virtual DbSet<Department> Privi_Department { get; set; }
        public virtual DbSet<DepartmentUser> Privi_Department_User { get; set; }
        public virtual DbSet<SYSCode> SYS_Code { get; set; }
        public virtual DbSet<SYS_Menu> SYS_Menu { get; set; }
        public virtual DbSet<SYS_Role> SYS_Role { get; set; }
        public virtual DbSet<SYS_Role_Menu> SYS_Role_Menu { get; set; }
        public virtual DbSet<SYS_Role_Menu_Function> SYS_Role_Menu_Function { get; set; }
        public virtual DbSet<SYS_Role_User> SYS_Role_User { get; set; }
        public virtual DbSet<SYS_UserInfo> SYS_UserInfo { get; set; }
        public virtual DbSet<Feedback> SYS_Feedback { get; set; }
        public virtual DbSet<HouseDetails> HouseDetails { get; set; }
        public virtual DbSet<Templet> Templet { get; set; }
        public virtual DbSet<Field> Field { get; set; }
        public virtual DbSet<FieldTemplet> FieldTemplet { get; set; }
        public virtual DbSet<FieldGroup> FieldGroup { get; set; }
        public virtual DbSet<FieldGroupTemplet> FieldGroupTemplet { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new AllotFlowMap());
            modelBuilder.Configurations.Add(new AllotSurveyMap());
            modelBuilder.Configurations.Add(new CheckMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new BuildingMap());
            modelBuilder.Configurations.Add(new HouseMap());
            modelBuilder.Configurations.Add(new SYSCodeMap());
            modelBuilder.Configurations.Add(new SYSMenuMap());
            modelBuilder.Configurations.Add(new SYSRoleMap());
            modelBuilder.Configurations.Add(new SYSRoleMenuFunctionMap());
            modelBuilder.Configurations.Add(new SYSRoleMenuMap());
            modelBuilder.Configurations.Add(new PPhotoMap());
            modelBuilder.Configurations.Add(new PCompanyMap());
            modelBuilder.Configurations.Add(new PAppendageMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DepartmentUserMap());
            modelBuilder.Configurations.Add(new SYSUserInfoMap());
            modelBuilder.Configurations.Add(new SYSFeedbackMap());
            modelBuilder.Configurations.Add(new HouseDetailsMap());
            modelBuilder.Configurations.Add(new TempletMap());
            modelBuilder.Configurations.Add(new FieldMap());
            modelBuilder.Configurations.Add(new FieldTempletMap());
            modelBuilder.Configurations.Add(new FieldGroupMap());
            modelBuilder.Configurations.Add(new FieldGroupTempletMap());

        }
    }
}

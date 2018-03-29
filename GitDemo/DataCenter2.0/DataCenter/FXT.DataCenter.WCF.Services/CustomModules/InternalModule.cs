using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using Ninject.Modules;
using Ninject.Web.Common;

namespace FXT.DataCenter.WCF.Services.CustomModules
{
    /// <summary>
    /// 管理内部接口与实现类之间的映射
    /// </summary>
    public class InternalModule : NinjectModule
    {
        public override void Load()
        {
            //绑定接口与实现类，并声明生命周期为一次请求

            this.Bind<IImportTask>().To<ImportTask>().InRequestScope();
            this.Bind<IDropDownList>().To<DropDownList>().InRequestScope();
            this.Bind<IProjectSample>().To<ProjectSample>().InRequestScope();
            this.Bind<IDAT_Project>().To<DAT_ProjectDAL>().InRequestScope();
            this.Bind<IDAT_Building>().To<DAT_BuildingDAL>().InRequestScope();
            this.Bind<IDAT_House>().To<DAT_HouseDAL>().InRequestScope();
            this.Bind<IDAT_Land>().To<DAT_LandDAL>().InRequestScope();
            this.Bind<ILandCase>().To<LandCase>().InRequestScope();
            this.Bind<IDAT_Land_BasePrice>().To<DAT_Land_BasePriceDAL>().InRequestScope();
            this.Bind<IBusinessStreet>().To<BusinessStreet>().InRequestScope();
            this.Bind<IDat_Building_Biz>().To<Dat_Building_BizDAL>().InRequestScope();
            this.Bind<IBusinessCase>().To<BusinessCase>().InRequestScope();
            this.Bind<IDynamicPriceSurvey>().To<DynamicPriceSurvey>().InRequestScope();
            this.Bind<IDat_Floor_Biz>().To<Dat_Floor_BizDAL>().InRequestScope();
            this.Bind<IDat_House_Biz>().To<Dat_House_BizDAL>().InRequestScope();
            this.Bind<ICompany>().To<Company>().InRequestScope();
            this.Bind<IDAT_Company>().To<DAT_CompanyDAL>().InRequestScope();
            this.Bind<IBusinessStore>().To<BusinessStore>().InRequestScope();
            this.Bind<IProjectCaseTask>().To<ProjectCaseTask>().InRequestScope();
            this.Bind<IProjectCase>().To<ProjectCase>().InRequestScope();
            this.Bind<IOfficeSubArea>().To<OfficeSubArea>().InRequestScope();
            this.Bind<IOfficeProject>().To<OfficeProject>().InRequestScope();
            this.Bind<IOfficeBuilding>().To<OfficeBuilding>().InRequestScope();
            this.Bind<IOfficePeiTao>().To<OfficePeiTao>().InRequestScope();
            this.Bind<IOfficeTenant>().To<OfficeTenant>().InRequestScope();
            this.Bind<IOfficeCase>().To<OfficeCase>().InRequestScope();
            this.Bind<IOfficeHouse>().To<OfficeHouse>().InRequestScope();
            this.Bind<IOfficeDynamicPrice>().To<OfficeDynamicPrice>().InRequestScope();
            this.Bind<IIndustrySubArea>().To<IndustrySubArea>().InRequestScope();
            this.Bind<IIndustryProject>().To<IndustryProject>().InRequestScope();
            this.Bind<IIndustryBuilding>().To<IndustryBuilding>().InRequestScope();
            this.Bind<IIndustryPeiTao>().To<IndustryPeiTao>().InRequestScope();
            this.Bind<IIndustryTenant>().To<IndustryTenant>().InRequestScope();
            this.Bind<IIndustryCase>().To<IndustryCase>().InRequestScope();
            this.Bind<IIndustryHouse>().To<IndustryHouse>().InRequestScope();
            this.Bind<IIndustryDynamicPrice>().To<IndustryDynamicPrice>().InRequestScope();
            this.Bind<IProjectOtherName>().To<ProjectOtherName>().InRequestScope();
            this.Bind<IWaitBuildingProject>().To<WaitBuildingProject>().InRequestScope();
            this.Bind<IHumanProject>().To<HumanProject>().InRequestScope();
            this.Bind<IHumanHouse>().To<HumanHouse>().InRequestScope();
            this.Bind<ICodePrice>().To<CodePrice>().InRequestScope();
            this.Bind<IFloorPrice>().To<FloorPrice>().InRequestScope();
            this.Bind<IPropertyAddress>().To<PropertyAddress>().InRequestScope();
        }
    }
}

using System;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Data.ServicesImpl;
using Ninject;
using System.Web.Routing;

namespace FXT.DataCenter.WebUI.Infrastructure.IocBinder
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {

            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {

            //ninjectKernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));

            #region Authorize(权限模块)

            ninjectKernel.Bind<IRole>().To<Role>();
            ninjectKernel.Bind<IMenu>().To<Menu>();
            ninjectKernel.Bind<IUser>().To<User>();

            #endregion

            #region Land(土地模块)

            ninjectKernel.Bind<IDAT_Land>().To<DAT_LandDAL>();
            ninjectKernel.Bind<ILandCase>().To<LandCase>();
            ninjectKernel.Bind<IDAT_Land_BasePrice>().To<DAT_Land_BasePriceDAL>();

            #endregion

            #region Share(共享模块)

            ninjectKernel.Bind<ILog>().To<Log>();
            ninjectKernel.Bind<ICompanyProduct_Module>().To<CompanyProduct_ModuleDAL>();
            ninjectKernel.Bind<IDropDownList>().To<DropDownList>();
            ninjectKernel.Bind<ISYS_Login>().To<SYS_LoginDAL>();
            ninjectKernel.Bind<IImportTask>().To<ImportTask>();
            ninjectKernel.Bind<IShare>().To<Share>();
            #endregion

            #region House(住宅模块)

            ninjectKernel.Bind<IWaitBuildingProject>().To<WaitBuildingProject>();
            ninjectKernel.Bind<IProjectSample>().To<ProjectSample>();
            ninjectKernel.Bind<IProjectOtherName>().To<ProjectOtherName>();
            ninjectKernel.Bind<IProjectCaseTask>().To<ProjectCaseTask>();
            ninjectKernel.Bind<IProjectCase>().To<ProjectCase>();
            ninjectKernel.Bind<IDAT_Company>().To<DAT_CompanyDAL>();
            ninjectKernel.Bind<IDAT_Project>().To<DAT_ProjectDAL>();
            ninjectKernel.Bind<IDAT_Building>().To<DAT_BuildingDAL>();
            ninjectKernel.Bind<IDAT_House>().To<DAT_HouseDAL>();
            ninjectKernel.Bind<IProjectWeightRevised>().To<ProjectWeightRevised>();
            ninjectKernel.Bind<IBuildingWeightRevised>().To<BuildingWeightRevised>();
            ninjectKernel.Bind<IFloorPrice>().To<FloorPrice>();
            ninjectKernel.Bind<ICodePrice>().To<CodePrice>();
            ninjectKernel.Bind<IPropertyAddress>().To<PropertyAddress>();

            //ninjectKernel.Bind<ICityAvgPriceServices>().To<CityAvgPriceServices>();
            #endregion

            #region Business(商业模块)
            ninjectKernel.Bind<IBusinessCircle>().To<BusinessCircle>();
            ninjectKernel.Bind<IBusinessStreet>().To<BusinessStreet>();
            ninjectKernel.Bind<IBusinessStore>().To<BusinessStore>();
            ninjectKernel.Bind<IBusinessCase>().To<BusinessCase>();
            ninjectKernel.Bind<IDynamicPriceSurvey>().To<DynamicPriceSurvey>();
            ninjectKernel.Bind<IDat_Building_Biz>().To<Dat_Building_BizDAL>();
            ninjectKernel.Bind<IDat_Floor_Biz>().To<Dat_Floor_BizDAL>();
            ninjectKernel.Bind<IDat_House_Biz>().To<Dat_House_BizDAL>();
            ninjectKernel.Bind<IBusinessBuildingPhoto>().To<BusinessBuildingPhoto>();
            ninjectKernel.Bind<IBusinessFloorPhoto>().To<BusinessFloorPhoto>();
            #endregion

            #region Company(公司模块)
            ninjectKernel.Bind<ICompany>().To<Company>();
            #endregion

            #region Office(办公模块)
            ninjectKernel.Bind<IOfficeBuilding>().To<OfficeBuilding>();
            ninjectKernel.Bind<IOfficeHouse>().To<OfficeHouse>();
            ninjectKernel.Bind<IOfficeProject>().To<OfficeProject>();
            ninjectKernel.Bind<IOfficeSubArea>().To<OfficeSubArea>();
            ninjectKernel.Bind<IOfficePeiTao>().To<OfficePeiTao>();
            ninjectKernel.Bind<IOfficeTenant>().To<OfficeTenant>();
            ninjectKernel.Bind<IOfficeCase>().To<OfficeCase>();
            ninjectKernel.Bind<IOfficeDynamicPrice>().To<OfficeDynamicPrice>();
            #endregion

            #region Industry(工业模块)
            ninjectKernel.Bind<IIndustrySubArea>().To<IndustrySubArea>();
            ninjectKernel.Bind<IIndustryProject>().To<IndustryProject>();
            ninjectKernel.Bind<IIndustryPeiTao>().To<IndustryPeiTao>();
            ninjectKernel.Bind<IIndustryBuilding>().To<IndustryBuilding>();
            ninjectKernel.Bind<IIndustryHouse>().To<IndustryHouse>();
            ninjectKernel.Bind<IIndustryCase>().To<IndustryCase>();
            ninjectKernel.Bind<IIndustryDynamicPrice>().To<IndustryDynamicPrice>();
            ninjectKernel.Bind<IIndustryTenant>().To<IndustryTenant>();
            #endregion

            #region Industry(业主信息模块)
            ninjectKernel.Bind<IHumanProject>().To<HumanProject>();
            ninjectKernel.Bind<IHumanHouse>().To<HumanHouse>();
            #endregion

            #region 区域分析
            ninjectKernel.Bind<IDAT_Analysis_City>().To<DAT_Analysis_CityDAL>();
            #endregion
        }
    }
}
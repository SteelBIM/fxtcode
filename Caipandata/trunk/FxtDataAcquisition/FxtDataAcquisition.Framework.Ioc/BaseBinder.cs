using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Repositories;
using FxtDataAcquisition.Framework.Data.EntityFramework;
using FxtDataAcquisition.Framework.Data.EntityFramework.Repositories;
using Ninject.Modules;
using Ninject.Web.Common;

namespace FxtDataAcquisition.Framework.Ioc
{
    public class BaseBinder: NinjectModule
    {
        public override void Load()
        {
            Bind<IAdminService>().To<AdminService>();
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            
            Bind<IProjectAllotFlowSurveyService>().To<ProjectAllotFlowSurveyService>();
            Bind<IProjectAllotFlowSurveyRepository>().To<ProjectAllotFlowSurveyRepository>();
            
            Bind<IHouseService>().To<HouseService>();
            Bind<ISysRoleMenuFunctionService>().To<SysRoleMenuFunctionService>();
            Bind<IAllotFlowService>().To<AllotFlowService>();
            Bind<IPhotoService>().To<PhotoService>();
            Bind<IProjectService>().To<ProjectService>();
            Bind<IBuildingService>().To<BuildingService>();
            Bind<ICityService>().To<CityService>();
            Bind<IUserInfoService>().To<UserInfoService>();
            Bind<ICodeService>().To<CodeService>();
            Bind<ITempletService>().To<TempletService>();

            
        }
    }
}

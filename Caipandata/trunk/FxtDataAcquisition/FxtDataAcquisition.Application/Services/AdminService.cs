using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAllotFlowService _allotFlowService;
        private readonly IProjectAllotFlowSurveyService _projectAllotFlowSurveyService;
        private readonly IHouseService _houseService;
        private readonly ISysRoleMenuFunctionService _functionService;
        private readonly IPhotoService _photoService;
        private readonly IProjectService _projectService;
        private readonly IBuildingService _buildingService;
        private readonly ICityService _cityService;
        private readonly IUserInfoService _userService;
        private readonly ICodeService _codeService;
        private readonly ITempletService _templetService;
        
        public AdminService(IUnitOfWork unitOfWork, IAllotFlowService allotFlowService, IProjectAllotFlowSurveyService projectAllotFlowSurveyService,
            IHouseService houseService, ISysRoleMenuFunctionService functionService, IPhotoService photoService,
            IProjectService projectService, IBuildingService buildingService, ICityService cityService,
            IUserInfoService userService, ICodeService codeService, ITempletService templetService)
        {
            this._allotFlowService = allotFlowService;
            this._unitOfWork = unitOfWork;
            this._houseService = houseService;
            this._functionService = functionService;
            this._projectAllotFlowSurveyService = projectAllotFlowSurveyService;
            this._photoService = photoService;
            this._projectService = projectService;
            this._buildingService = buildingService;
            this._cityService = cityService;
            this._userService = userService;
            this._codeService = codeService;
            this._templetService = templetService;
        }

        public int Commit()
        {
            return _unitOfWork.Commit();
        }

        public IGenericRepository<AllotFlow> AllotFlowRepository
        {
            get { return _unitOfWork.AllotFlowRepository; }
        }

        public IGenericRepository<AllotSurvey> AllotSurveyRepository
        {
            get { return _unitOfWork.AllotSurveyRepository; }
        }

        public IGenericRepository<Check> CheckRepository
        {
            get { return _unitOfWork.CheckRepository; }
        }

        public IGenericRepository<Project> ProjectRepository
        {
            get { return _unitOfWork.ProjectRepository; }
        }

        public IGenericRepository<Building> BuildingRepository
        {
            get { return _unitOfWork.BuildingRepository; }
        }
        public IGenericRepository<House> HouseRepository
        {
            get { return _unitOfWork.HouseRepository; }
        }
        public IGenericRepository<PAppendage> P_AppendageRepository
        {
            get { return _unitOfWork.P_AppendageRepository; }
        }
        public IGenericRepository<PCompany> P_CompanyRepository
        {
            get { return _unitOfWork.P_CompanyRepository; }
        }
        public IGenericRepository<PPhoto> P_PhotoRepository
        {
            get { return _unitOfWork.P_PhotoRepository; }
        }
        public IGenericRepository<Department> DepartmentRepository
        {
            get { return _unitOfWork.DepartmentRepository; }
        }
        public IGenericRepository<DepartmentUser> DepartmentUserRepository
        {
            get { return _unitOfWork.DepartmentUserRepository; }
        }
        public IGenericRepository<SYSCode> SysCodeRepository
        {
            get { return _unitOfWork.SysCodeRepository; }
        }
        public IGenericRepository<SYS_Menu> SysMenuRepository
        {
            get { return _unitOfWork.SysMenuRepository; }
        }
        public IGenericRepository<SYS_Role> SysRoleRepository
        {
            get { return _unitOfWork.SysRoleRepository; }
        }
        public IGenericRepository<SYS_Role_User> SysRoleUserRepository
        {
            get { return _unitOfWork.SysRoleUserRepository; }
        }
        public IGenericRepository<SYS_Role_Menu> SysRoleMenuRepository
        {
            get { return _unitOfWork.SysRoleMenuRepository; }
        }

        public IGenericRepository<SYS_Role_Menu_Function> SysRoleMenuFunctionRepository
        {
            get { return _unitOfWork.SysRoleMenuFunctionRepository; }
        }

        public IGenericRepository<SYS_UserInfo> SysUserInfoRepository
        {
            get { return _unitOfWork.SysUserInfoRepository; }
        }

        public IGenericRepository<Feedback> SysFeedbackRepository
        {
            get { return _unitOfWork.SysFeedbackRepository; }
        }
        public IGenericRepository<HouseDetails> HouseDetailsRepository
        {
            get { return _unitOfWork.HouseDetailsRepository; }
        }
        public IGenericRepository<Templet> TempletRepository
        {
            get { return _unitOfWork.TempletRepository; }
        }
        public IGenericRepository<Field> FieldRepository
        {
            get { return _unitOfWork.FieldRepository; }
        }
        public IGenericRepository<FieldTemplet> FieldTempletRepository
        {
            get { return _unitOfWork.FieldTempletRepository; }
        }
        public IGenericRepository<FieldGroup> FieldGroupRepository
        {
            get { return _unitOfWork.FieldGroupRepository; }
        }
        public IGenericRepository<FieldGroupTemplet> FieldGroupTempletRepository
        {
            get { return _unitOfWork.FieldGroupTempletRepository; }
        }
        public IAllotFlowService AllotFlowService
        {
            get { return _allotFlowService; }
        }

        public IProjectAllotFlowSurveyService ProjectAllotFlowSurveyService
        {
            get { return _projectAllotFlowSurveyService; }
        }

        public IHouseService HouseService
        {
            get { return _houseService; }
        }

        public ISysRoleMenuFunctionService FunctionService
        {
            get { return _functionService; }
        }
        public IPhotoService PhotoService
        {
            get { return _photoService; }
        }
        public IProjectService ProjectService
        {
            get { return _projectService; }
        }

        public IBuildingService BuildingService
        {
            get { return _buildingService; }
        }

        public ICityService CityService
        {
            get { return _cityService; }
        }

        public IUserInfoService UserService
        {
            get { return _userService; }
        }

        public ICodeService CodeService
        {
            get { return _codeService; }
        }

        public ITempletService TempletService
        {
            get { return _templetService; }
        }

    }
}

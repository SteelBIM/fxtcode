using CAS.Common.MVC4;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        private IGenericRepository<AllotFlow> _allotFlowRepository;
        private IGenericRepository<AllotSurvey> _allotSurveyRepository;
        private IGenericRepository<Check> _checkRepository;
        private IGenericRepository<Project> _projectRepository;
        private IGenericRepository<Building> _buildingRepository;
        private IGenericRepository<House> _houseRepository;
        private IGenericRepository<PAppendage> _p_AppendageRepository;
        private IGenericRepository<PCompany> _p_CompanyRepository;
        private IGenericRepository<PPhoto> _p_PhotoRepository;
        private IGenericRepository<Department> _departmentRepository;
        private IGenericRepository<DepartmentUser> _departmentUserRepository;
        private IGenericRepository<SYSCode> _sysCodeRepository;
        private IGenericRepository<SYS_Menu> _sysMenuRepository;
        private IGenericRepository<SYS_Role> _sysRoleRepository;
        private IGenericRepository<SYS_Role_User> _sysRoleUserRepository;
        private IGenericRepository<SYS_Role_Menu> _sysRoleMenuRepository;
        private IGenericRepository<SYS_Role_Menu_Function> _sysRoleMenuFunctionRepository;
        private IGenericRepository<SYS_UserInfo> _sysUserInfoRepository;
        private IGenericRepository<Feedback> _feedbackRepository;
        private IGenericRepository<HouseDetails> _houseDetailsRepository;

        public UnitOfWork()
        {
            this._dbContext = new FxtDataAcquisitionContext();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public IGenericRepository<AllotFlow> AllotFlowRepository
        {
            get { return _allotFlowRepository ?? (_allotFlowRepository = new GenericRepository<AllotFlow>(_dbContext)); }
        }

        public IGenericRepository<AllotSurvey> AllotSurveyRepository
        {
            get { return _allotSurveyRepository ?? (_allotSurveyRepository = new GenericRepository<AllotSurvey>(_dbContext)); }
        }

        public IGenericRepository<Check> CheckRepository
        {
            get { return _checkRepository ?? (_checkRepository = new GenericRepository<Check>(_dbContext)); }
        }

        public IGenericRepository<Project> ProjectRepository
        {
            get { return _projectRepository ?? (_projectRepository = new GenericRepository<Project>(_dbContext)); }
        }

        public IGenericRepository<Building> BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new GenericRepository<Building>(_dbContext)); }
        }

        public IGenericRepository<House> HouseRepository
        {
            get { return _houseRepository ?? (_houseRepository = new GenericRepository<House>(_dbContext)); }
        }

        public IGenericRepository<PAppendage> P_AppendageRepository
        {
            get { return _p_AppendageRepository ?? (_p_AppendageRepository = new GenericRepository<PAppendage>(_dbContext)); }
        }

        public IGenericRepository<PCompany> P_CompanyRepository
        {
            get { return _p_CompanyRepository ?? (_p_CompanyRepository = new GenericRepository<PCompany>(_dbContext)); }
        }

        public IGenericRepository<PPhoto> P_PhotoRepository
        {
            get { return _p_PhotoRepository ?? (_p_PhotoRepository = new GenericRepository<PPhoto>(_dbContext)); }
        }

        public IGenericRepository<Department> DepartmentRepository
        {
            get { return _departmentRepository ?? (_departmentRepository = new GenericRepository<Department>(_dbContext)); }
        }

        public IGenericRepository<DepartmentUser> DepartmentUserRepository
        {
            get { return _departmentUserRepository ?? (_departmentUserRepository = new GenericRepository<DepartmentUser>(_dbContext)); }
        }

        public IGenericRepository<SYSCode> SysCodeRepository
        {
            get { return _sysCodeRepository ?? (_sysCodeRepository = new GenericRepository<SYSCode>(_dbContext)); }
        }

        public IGenericRepository<SYS_Menu> SysMenuRepository
        {
            get { return _sysMenuRepository ?? (_sysMenuRepository = new GenericRepository<SYS_Menu>(_dbContext)); }
        }

        public IGenericRepository<SYS_Role> SysRoleRepository
        {
            get { return _sysRoleRepository ?? (_sysRoleRepository = new GenericRepository<SYS_Role>(_dbContext)); }
        }

        public IGenericRepository<SYS_Role_User> SysRoleUserRepository
        {
            get { return _sysRoleUserRepository ?? (_sysRoleUserRepository = new GenericRepository<SYS_Role_User>(_dbContext)); }
        }

        public IGenericRepository<SYS_Role_Menu> SysRoleMenuRepository
        {
            get { return _sysRoleMenuRepository ?? (_sysRoleMenuRepository = new GenericRepository<SYS_Role_Menu>(_dbContext)); }
        }

        public IGenericRepository<SYS_Role_Menu_Function> SysRoleMenuFunctionRepository
        {
            get { return _sysRoleMenuFunctionRepository ?? (_sysRoleMenuFunctionRepository = new GenericRepository<SYS_Role_Menu_Function>(_dbContext)); }
        }

        public IGenericRepository<SYS_UserInfo> SysUserInfoRepository
        {
            get { return _sysUserInfoRepository ?? (_sysUserInfoRepository = new GenericRepository<SYS_UserInfo>(_dbContext)); }
        }
        public IGenericRepository<Feedback> SysFeedbackRepository
        {
            get { return _feedbackRepository ?? (_feedbackRepository = new GenericRepository<Feedback>(_dbContext)); }
        }
        public IGenericRepository<HouseDetails> HouseDetailsRepository
        {
            get { return _houseDetailsRepository ?? (_houseDetailsRepository = new GenericRepository<HouseDetails>(_dbContext)); }
        }
    }
}

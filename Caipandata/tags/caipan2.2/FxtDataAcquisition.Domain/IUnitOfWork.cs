using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AllotFlow> AllotFlowRepository { get; }
        IGenericRepository<AllotSurvey> AllotSurveyRepository { get; }
        IGenericRepository<Check> CheckRepository { get; }
        IGenericRepository<Project> ProjectRepository { get; }
        IGenericRepository<Building> BuildingRepository { get; }
        IGenericRepository<House> HouseRepository { get; }
        IGenericRepository<PAppendage> P_AppendageRepository { get; }
        IGenericRepository<PCompany> P_CompanyRepository { get; }
        IGenericRepository<PPhoto> P_PhotoRepository { get; }
        IGenericRepository<Department> DepartmentRepository { get; }
        IGenericRepository<DepartmentUser> DepartmentUserRepository { get; }
        IGenericRepository<SYSCode> SysCodeRepository { get; }
        IGenericRepository<SYS_Menu> SysMenuRepository { get; }
        IGenericRepository<SYS_Role> SysRoleRepository { get; }
        IGenericRepository<SYS_Role_User> SysRoleUserRepository { get; }
        IGenericRepository<SYS_Role_Menu> SysRoleMenuRepository { get; }
        IGenericRepository<SYS_Role_Menu_Function> SysRoleMenuFunctionRepository { get; }
        IGenericRepository<SYS_UserInfo> SysUserInfoRepository { get; }
        IGenericRepository<Feedback> SysFeedbackRepository { get; }
        IGenericRepository<HouseDetails> HouseDetailsRepository { get; }

        int Commit();
    }
}

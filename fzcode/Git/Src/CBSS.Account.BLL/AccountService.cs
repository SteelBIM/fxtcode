using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CBSS.Core.Utility;
using System.Data.Objects;
using CBSS.Framework.Contract;
using CBSS.Core.Cache;
using CBSS.Core.Config;
using CBSS.Framework.DAL;
using System.Linq.Expressions;
using CBSS.Account.IBLL;
using CBSS.Account.Contract;
using CBSS.Account.Contract.ViewModel;
using CBSS.Framework.Redis;
using System.Web;

namespace CBSS.Account.BLL
{
    public class AccountService : IAccountService
    {
        private readonly int _UserLoginTimeoutMinutes = CachedConfigContext.Current.SystemConfig.UserLoginTimeoutMinutes;
        private readonly string _LoginInfoKeyFormat = "LoginInfo_{0}";
        Repository repository = new Repository("Account");
        static RedisHashHelper hashRedis = new RedisHashHelper("Tbx");



        public Sys_LoginInfo GetLoginInfo(Guid token)
        {
            return CacheHelper.Get<Sys_LoginInfo>(string.Format(_LoginInfoKeyFormat, token), () =>
            {

                DateTime time = DateTime.Now.AddMinutes(-(_UserLoginTimeoutMinutes));
                //如果有超时的，启动超时处理
                var timeoutList = repository.SelectSearch<Sys_LoginInfo>(p => time > p.LastAccessTime).ToList();
                if (timeoutList.Count > 0)
                {
                    foreach (var li in timeoutList)
                    {
                        //  dbContext.LoginInfos.Remove(li);
                        //repository.Delete<LoginInfo>(li);
                    }

                }

                var loginInfo = repository.SelectSearch<Sys_LoginInfo>(l => l.LoginToken == token).FirstOrDefault();
                if (loginInfo != null)
                {
                    loginInfo.LastAccessTime = DateTime.Now;
                    repository.Update<Sys_LoginInfo>(loginInfo);
                }
                return loginInfo;
            });
        }

        public Sys_LoginInfo Login(string loginName, string password)
        {
            Sys_LoginInfo loginInfo = new Sys_LoginInfo();

            password = SecurityHelper.MD5(password);
            loginName = loginName.Trim();


            var user = repository.SelectSearch<Sys_User>(u => u.LoginName == loginName && u.Password == password && u.IsActive).FirstOrDefault();
            if (user != null)
            {
                var ip = Fetch.UserIp;
                loginInfo = repository.SelectSearch<Sys_LoginInfo>(p => p.LoginName == loginName && p.ClientIP == ip).FirstOrDefault();
                if (loginInfo != null)
                {
                    loginInfo.LastAccessTime = DateTime.Now;
                }
                else
                {
                    loginInfo = new Sys_LoginInfo(user.ID, user.LoginName);
                    loginInfo.ClientIP = ip;
                    //loginInfo.BusinessPermissionString = //user.BusinessPermissionList;
                    //  dbContext.Insert<LoginInfo>(loginInfo);
                    repository.Insert<Sys_LoginInfo>(loginInfo);
                }
                if (user.RoleId > 0)
                {
                    loginInfo.BusinessPermissionString = GetRoleAction(user.RoleId);
                }
                else
                {
                    loginInfo.BusinessPermissionString = GetAllAction();
                }
                //IEnumerable<v_allaction> allaction = GetAllAction();
                //写入redis
                //hashRedis.Set<IEnumerable<v_allaction>>("Redis_AllAction", HttpUtility.UrlEncode(loginInfo.UserID.ToString()), allaction);
                //loginInfo.BusinessPermissionString = GetAllAction();// repository.ListAll<v_allaction>();
            }
            return loginInfo;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<v_allaction> Get_AllActionByUserID(string UserID)
        {
            return hashRedis.Get<IEnumerable<v_allaction>>("Redis_AllAction", UserID);
        }
        public void Logout(Guid token)
        {
            var loginInfo = repository.SelectSearch<Sys_LoginInfo>(l => l.LoginToken == token).FirstOrDefault();
            if (loginInfo != null)
            {
                repository.Delete<Sys_LoginInfo>(l => l.LoginToken == token);
            }
            CacheHelper.Remove(string.Format(_LoginInfoKeyFormat, token));
        }

        public bool ModifyPwd(Sys_User user)
        {
            user.Password = SecurityHelper.MD5(user.Password); 
            //  if (dbContext.Users.Any(l => l.ID == user.ID && user.Password == l.Password))
            if (repository.SelectSearch<Sys_User>(o => o.ID == user.ID && user.Password == o.Password).Any())
            {
                //if (!string.IsNullOrEmpty(user.NewPassword))
                //    user.Password = SecurityHelper.MD5(user.NewPassword);
                user.Email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                user.Mobile = string.IsNullOrEmpty(user.Mobile) ? "" : user.Mobile;
                user.Password = SecurityHelper.MD5(user.NewPassword);
                bool flag = repository.Update<Sys_User>(user);
                return flag;
            }
            else
            {
                throw new BusinessException("Password", "原密码不正确！"); 
            } 
        }

        public Sys_User GetUser(int id)
        {
            return repository.SelectSearch<Sys_User>(u => u.ID == id).SingleOrDefault();
        }

        public IEnumerable<Sys_User> GetUserList(out int totalcount, UserRequest request = null)
        {
            request = request ?? new UserRequest();

            List<Expression<Func<Sys_User, bool>>> exprlist = new List<Expression<Func<Sys_User, bool>>>();
            exprlist.Add(o => o.RoleId > 0);

            if (!string.IsNullOrEmpty(request.LoginName))
                exprlist.Add(u => u.LoginName.Contains(request.LoginName.Trim()));

            if (!string.IsNullOrEmpty(request.Mobile))
                exprlist.Add(u => u.Mobile.Contains(request.Mobile.Trim()));

            PageParameter<Sys_User> pageParameter = new PageParameter<Sys_User>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.ID;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Sys_User>(pageParameter, out totalcount);

        }


        public void SaveUser(Sys_User user)
        {

            if (user.ID > 0)
            {
                user.Email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                user.Mobile = string.IsNullOrEmpty(user.Mobile) ? "" : user.Mobile;
                repository.Update<Sys_User>(user);
            }
            else
            {
                var existUser = repository.SelectSearch<Sys_User>(u => u.LoginName == user.LoginName);
                if (existUser.Any())
                {
                    throw new BusinessException("LoginName", "此登录名已存在！");
                }
                else
                {
                    repository.Insert<Sys_User>(user);
                }
            }

        }

        public void DeleteUser(List<int> ids)
        {
            string[] stringIDs = ids.Select(o => o.ToString()).ToArray();
            //     dbContext.Users.Include("Roles").Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { a.Roles.Clear(); dbContext.Users.Remove(a); });
            repository.DeleteMore<Sys_User>(stringIDs);
            //   dbContext.SaveChanges();

        }

        public Sys_Role GetRole(int id)
        {
            return repository.GetByID<Sys_Role>(id);
            // return dbContext.Find<Role>(id);
        }

        public int GetRoleUserNumber(int RoleId)
        {
            return repository.GetTotalCount<Sys_User>(u => u.RoleId == RoleId);
        }


        public IEnumerable<Sys_Role> GetRoleList(out int totalcount, RoleRequest request = null)
        {
            request = request ?? new RoleRequest();
            List<Expression<Func<Sys_Role, bool>>> exprlist = new List<Expression<Func<Sys_Role, bool>>>();

            if (!string.IsNullOrEmpty(request.RoleName))
                exprlist.Add(r => r.Name.Contains(request.RoleName.Trim()));


            PageParameter<Sys_Role> pageParameter = new PageParameter<Sys_Role>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = r => r.ID;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<Sys_Role>(pageParameter, out totalcount);
        }

        public IEnumerable<Sys_Role> GetRoleList()
        {
            return repository.ListAll<Sys_Role>();
        }

        public IEnumerable<v_allaction> GetAllAction()
        {
            return repository.ListAll<v_allaction>().OrderBy(a => a.sequence);
        }
        public IEnumerable<v_action> GetRoleAction(int roleid)
        {
            return repository.SelectSearch<v_action>(v => v.ID == roleid).OrderBy(a => a.sequence);
        }


        public void SaveRole(Sys_Role role)
        {
            if (role.ID > 0)
            {
                var existUser = repository.SelectSearch<Sys_Role>(u => u.Name == role.Name && u.ID != role.ID);
                if (existUser.Any())
                {
                    throw new BusinessException("RoleName", "此角色名已存在！");
                }
                else
                {
                    role.BusinessPermissionString = role.BusinessPermissionString == null ? "" : role.BusinessPermissionString;
                    repository.Update<Sys_Role>(role);
                }
            }
            else
            {
                var existUser = repository.SelectSearch<Sys_Role>(u => u.Name == role.Name);
                if (existUser.Any())
                {
                    throw new BusinessException("RoleName", "此角色名已存在！");
                }
                else
                {
                    repository.Insert<Sys_Role>(role);
                }
            }
        }

        public void DeleteRole(List<int> ids)
        {

            // dbContext.Roles.Include("Users").Where(u => ids.Contains(u.ID)).ToList().ForEach(a => { a.Users.Clear(); dbContext.Roles.Remove(a); });
            repository.DeleteMore<Sys_Role>(ids.Select(o => o.ToString()).ToArray());
            //   dbContext.SaveChanges();

        }
    }
}

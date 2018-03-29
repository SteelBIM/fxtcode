using FxtDataAcquisition.Data;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class UserInfoManager
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(UserInfoManager));
        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="cityId">当前城市</param>
        /// <param name="companyId">所属结构</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="roleId">根据角色ID查询,可为null</param>
        /// <param name="departmentId">根据分组ID查询,可为null</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="loginusername">当前登录的username(用于验证api)</param>
        /// <param name="loginsignname">当前登录的signname(用于验证api)</param>
        /// <param name="loginAppList">当前登录的用户拥有的api信息集合(用于验证api)</param>
        /// <param name="isGetCount"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static List<UserInfoJoinRoleJoinDepartment> GetUserInfoJoinRoleJoinDepartmentByRoleIdAndDepartmentIdAndUserName(int cityId, int companyId, string keyWord, int? roleId, int? departmentId, int pageIndex, int pageSize, out int count, string loginusername, string loginsignname, List<UserCenter_Apps> loginAppList, bool isGetCount = true, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                List<UserInfoJoinRoleJoinDepartment> list = new List<UserInfoJoinRoleJoinDepartment>();
                List<UserCenter_UserInfo> userList = new List<UserCenter_UserInfo>();
                List<View_UserJoinRoleJoinDepartmen> roleList = new List<View_UserJoinRoleJoinDepartmen>();
                List<View_UserJoinDepartment> departmentList = new List<View_UserJoinDepartment>();
                List<string> userNames = new List<string>();
                //无小组和角色搜索条件
                if (Convert.ToInt32(roleId) < 1 && Convert.ToInt32(departmentId) < 1)
                {
                    userList = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, userNames.ToArray(), keyWord, null, pageIndex, pageSize, out count, loginusername, loginsignname, loginAppList);
                    foreach (UserCenter_UserInfo userInfo in userList)
                    {
                        userNames.Add(userInfo.UserName);
                    }
                    roleList = ViewUserJoinRoleJoinDepartmenManager.GetUserJoinRoleJoinDepartmenByUserNames(companyId, cityId, userNames.ToArray(), _db: db);
                    departmentList = ViewUserJoinDepartmentManager.GetUserJoinDepartmentByUserNames(companyId, cityId, userNames.ToArray(), _db: db);
                    list = UserInfoJoinRoleJoinDepartment.GetList(userList, roleList, departmentList);
                }//无小组搜索条件
                else if (Convert.ToInt32(departmentId) < 1)
                {
                    List<View_UserJoinRoleJoinDepartmen> roleList2 = ViewUserJoinRoleJoinDepartmenManager.GetUserJoinRoleJoinDepartmenByRoleIdAndUserName(companyId, cityId, keyWord, Convert.ToInt32(roleId), pageIndex, pageSize, out count, isGetCount, db);
                    foreach (View_UserJoinRoleJoinDepartmen roleInfo in roleList2)
                    {
                        userNames.Add(roleInfo.UserName);
                        if (Convert.ToInt32(roleInfo.DepartmentID) > 0)
                        {
                            View_UserJoinDepartment depaObj = new View_UserJoinDepartment { CityID = roleInfo.CityID, DepartmentID = Convert.ToInt32(roleInfo.DepartmentID), DepartmentName = roleInfo.DepartmentName, DValid = roleInfo.DValid, FxtCompanyID = companyId, UserName = roleInfo.UserName };
                            departmentList.Add(depaObj);
                        }
                    }
                    int count2 = 0;
                    roleList = ViewUserJoinRoleJoinDepartmenManager.GetUserJoinRoleJoinDepartmenByUserNames(companyId, cityId, userNames.ToArray(), _db: db);
                    userList = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, userNames.ToArray(), keyWord, null, pageIndex, pageSize, out count2, loginusername, loginsignname, loginAppList);
                    list = UserInfoJoinRoleJoinDepartment.GetList(roleList2, departmentList, userList, roleList);
                }//有小组搜索条件
                else
                {
                    departmentList = ViewUserJoinDepartmentManager.GetUserJoinDepartmentByDepartmentIdAndRoleIdAndUserName(companyId, cityId, keyWord, Convert.ToInt32(departmentId), roleId, pageIndex, pageSize, out count, isGetCount, db);
                    foreach (View_UserJoinDepartment depanfo in departmentList)
                    {
                        userNames.Add(depanfo.UserName);
                    }
                    int count2 = 0;
                    roleList = ViewUserJoinRoleJoinDepartmenManager.GetUserJoinRoleJoinDepartmenByUserNames(companyId, cityId, userNames.ToArray(), _db: db);
                    userList = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, userNames.ToArray(), keyWord, null, 1, pageSize, out count2, loginusername, loginsignname, loginAppList);
                    list = UserInfoJoinRoleJoinDepartment.GetList(departmentList, userList, roleList);
                }
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        //public static List<UserInfoJoinRoleJoinDepartment> GetUserInfoJoinRoleJoinDepartmentBySurveyRight(int cityId, int companyId,string nowUserName, string keyword,
        //     int[] functionCodes, int pageIndex, int pageSize, string loginusername, string loginsignname, out int count, bool isGetCount = true, DataBase _db = null)
        //{
        //    count = 0;
        //    List<UserInfoJoinRoleJoinDepartment> list = new List<UserInfoJoinRoleJoinDepartment>();
        //    DataBase db = new DataBase(_db);
        //    StringBuilder sb = new StringBuilder();
        //    List<NHParameter> parameters = new List<NHParameter>();
        //    parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
        //    parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
        //    sb.Append(string.Format("{0} CityID=:cityId and FxtCompanyId=:companyId ", NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSRoleUser, "distinct", "UserName")));
        //    //根据操作权限
        //    if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_3))//查看公司全部(管理员+分配人+审核人)
        //    {
        //    }
        //    else if (functionCodes != null && functionCodes.Contains(SYSCodeManager.FunOperCode_2))//查看小组内(组长)
        //    {
        //        sb.Append(string.Format("and (UserName=:userName or  UserName in (select UserName from {0} with(nolock) where DepartmentID in (select DepartmentID from {0} with(nolock) where  CityID=:cityId and FxtCompanyID=:companyId and UserName=:userName  and DepartmentID in (select DepartmentID from {1} with(nolock) where DValid=1))))", NHibernateUtility.TableName_PriviDepartmentUser, NHibernateUtility.TableName_PriviDepartment));
        //        parameters.Add(new NHParameter("userName", nowUserName, NHibernateUtil.String));
        //    }
        //    else//查看自己(查勘员)
        //    {
        //        sb.Append(" and UserName=:userName ");
        //        parameters.Add(new NHParameter("userName", nowUserName, NHibernateUtil.String));
        //    }
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        sb.Append(" and UserName like :keyword ");
        //        parameters.Add(new NHParameter("keyword", "%" + keyword + "%", NHibernateUtil.String));
        //    }
        //    UtilityPager pageInfo = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
        //    IList ilist = db.DB.GetCustomSQLQueryObjectList(pageInfo, sb.ToString(), parameters);
        //    count = pageInfo.Count;
        //    if (ilist == null || ilist.Count < 1)
        //    {
        //        db.Close();
        //        return new List<UserInfoJoinRoleJoinDepartment>();
        //    }
        //    List<string> userNameList = new List<string>();
        //    foreach (var l in ilist)
        //    {
        //        userNameList.Add(Convert.ToString(l));
        //    }
        //    int count2 = 0;
        //    List<View_UserJoinRoleJoinDepartmen> roleList = ViewUserJoinRoleJoinDepartmenManager.GetUserJoinRoleJoinDepartmenByUserNames(companyId, cityId, userNameList.ToArray(), _db: db);
        //    List<UserCenter_UserInfo> userList = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, userNameList.ToArray(), null, null, pageIndex, pageSize, out count2, loginusername, loginsignname);
        //    List<UserInfoJoinRoleJoinDepartment> list = UserInfoJoinRoleJoinDepartment.GetList(roleList2, departmentList, userList, roleList);
        //}

        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="cityId">当前城市</param>
        /// <param name="companyId">所属机构ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="loginusername">当前登录的username(用于api验证)</param>
        /// <param name="loginsignname">当前登录的signname(用于api验证)</param>
        /// <param name="loginAppList">当前登录的用户拥有的api信息集合(用于验证api)</param>
        /// <param name="pdu"></param>
        /// <param name="roleUserList"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static UserCenter_UserInfo GetUserInfoByUserName(int cityId, int companyId, string userName, string loginusername, string loginsignname, List<UserCenter_Apps> loginAppList, out PriviDepartmentUser pdu, out IList<SYSRoleUser> roleUserList, DataBase _db = null)
        {
            pdu = null;
            roleUserList = new List<SYSRoleUser>();
            int count = 0;
            UserCenter_UserInfo userInfo = null;
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            List<UserCenter_UserInfo> userInfoList = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, new string[] { userName }, "", null, 1, 1, out count, loginusername, loginsignname, loginAppList);
            if (userInfoList == null || userInfoList.Count < 1)
            {
                return null;
            }
            userInfo = userInfoList[0];
            DataBase db = new DataBase(_db);
            try
            {
                pdu = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, companyId, userName, db);
                roleUserList = SYSRoleUserManager.GetSYSRoleUserByUserName(cityId, companyId, userName, db);
                db.Close();
                return userInfo;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 设置用户信息(分配小组+角色)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <param name="departmentId">小组ID</param>
        /// <param name="roleIds">多个角色ID组成的数组</param>
        /// <param name="message"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static bool SetUserInfo(int cityId, int companyId, string userName, string truename, int? departmentId, int[] roleIds, out string message, DataBase _db = null)
        {
            //DataTable aa = new DataTable();
            //aa.Rows.Add()
            //    aa.Rows.InsertAt()
            DataBase db = new DataBase(_db);
            try
            {
                bool result = true;
                message = "";
                using (ITransaction tx = db.DB.BeginTransaction())
                {
                    try
                    {
                        #region(更新小组信息)
                        if (Convert.ToInt32(departmentId) == 0)
                        {
                            if (!PriviDepartmentUserManager.DeleteByUserName(cityId, companyId, userName, db, tx))
                            {
                                message = "删除小组异常";
                                result = false;
                                tx.Rollback();
                                goto end;
                            }
                            goto 更新角色信息;
                        }
                        bool pduUpdate = true;//是否存在并更新
                        PriviDepartmentUser pdu = PriviDepartmentUserManager.GetDepartmentUserByUserName(cityId, companyId, userName, db);
                        if (pdu == null)
                        {
                            pduUpdate = false;
                            pdu = new PriviDepartmentUser();
                        }
                        pdu.UserName = userName;
                        pdu.DepartmentID = Convert.ToInt32(departmentId);
                        pdu.CreateDate = DateTime.Now;
                        pdu.CityID = cityId;
                        pdu.FxtCompanyID = companyId;
                        bool pduUpdateResult = true;
                        if (pduUpdate)//更新
                        {
                            pduUpdateResult = db.DB.Update(pdu, tx);
                        }
                        else
                        {
                            pduUpdateResult = db.DB.Create(pdu, tx);
                        }
                        if (!pduUpdateResult)//插入
                        {
                            message = "更新小组异常";
                            result = false;
                            tx.Rollback();
                            goto end;
                        }
                        #endregion

                    更新角色信息:
                        #region (更新角色信息)
                        bool upRoleResult = SYSRoleUserManager.SetRoleUser(cityId, companyId, userName, truename, out message, roleIds, db, tx);
                        if (!upRoleResult)
                        {
                            result = false;
                            tx.Rollback();
                            goto end;
                        }
                    ////如果为删除所有
                    //if (roleIds == null || roleIds.Length < 1)
                    //{
                    //    if (!SYSRoleUserManager.DeleteByUserName(cityId, companyId, userName, db, tx))
                    //    {
                    //        message = "删除所有角色异常";
                    //        result = false;
                    //        tx.Rollback();
                    //        goto end;
                    //    }
                    //    goto resultend;
                    //}
                    //IList<SYSRoleUser> roleUserList = SYSRoleUserManager.GetSYSRoleUserByUserName(cityId, companyId, userName, db);
                    //IList<SYSRoleUser> addUserList = new List<SYSRoleUser>();//存储要新增的role
                    //List<int> delList = new List<int>();//存储要删除的role
                    ////获取要删除的role
                    //IList<SYSRoleUser> delUserList = roleUserList.Where(obj => !roleIds.Contains(obj.RoleID)).ToList();
                    //foreach (SYSRoleUser roleUser in delUserList)
                    //{
                    //    delList.Add(roleUser.RoleID);
                    //}
                    ////获取要新增的role
                    //foreach (int roleId in roleIds)
                    //{
                    //    int existsCount = roleUserList.Where(obj => obj.RoleID == roleId).Count();
                    //    int existsCount2 = addUserList.Where(obj => obj.RoleID == roleId).Count();
                    //    if (existsCount < 1 && existsCount2 < 1)
                    //    {
                    //        SYSRoleUser roleUser = new SYSRoleUser { CityID = cityId, FxtCompanyID = companyId, RoleID = roleId, UserName = userName };
                    //        addUserList.Add(roleUser);
                    //    }
                    //}
                    //if (!SYSRoleUserManager.DeleteByUserName(cityId, companyId, userName, delList.ToArray(), db, tx))
                    //{
                    //    message = "删除指定角色异常";
                    //    result = false;
                    //    tx.Rollback();
                    //    goto end;
                    //}
                    //if (!db.DB.Create<SYSRoleUser>(addUserList, tx))
                    //{
                    //    message = "新增角色异常";
                    //    result = false;
                    //    tx.Rollback();
                    //    goto end;
                    //}
                        #endregion
                    resultend:
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        message = "系统异常";
                        log.Error(message, ex);
                        result = false;

                    }
                }
            end:
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }


    }
}

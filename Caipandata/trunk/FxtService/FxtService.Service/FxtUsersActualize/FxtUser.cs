using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtService.Contract.FxtUsersInterface;
using FxtNHibernater.Data;
using FxtNHibernate.FxtDataUserDomain.Entities;
using Newtonsoft.Json;
using FxtService.Common;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using FxtNHibernate.DTODomain.FxtDataUserDTO;
using System.Web.Script.Serialization;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtCommonLibrary.LibraryUtils;
using CAS.DataAccess.DA;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using FxtNHibernate.DTODomain.FxtLoanDTO;

/**
 * 作者:李晓东
 * 摘要:2014.01.02新建
 *      2014.0.20 新增 GetUploads() 获得文件列表 Entrance() 验证入口方法 修改人:李晓东
 *      2014.06.11 修改人 曹青
 *                 新增 客户管理 CheckCustomerName、AddEditCustomer、GetCustomer
 *                 新增 用户管理 CheckUserName、AddEditUser、GetUser
 *      2014.06.12 修改人:贺黎亮
 *                 移除文件GetUploads(),GetFiles()方法
 *      2014.06.17 修改人 曹青
 *                 新增 UserLogin、ModifyPassword
 *      2014.06.18 修改人 曹青
 *                 新增 DeleteActiveCustomer、DeleteActiveUser          
 * **/
namespace FxtService.Service.FxtUsersActualize
{
    [FxtService.Service.ServiceBehavior]
    public class FxtUser : Exceptions, IFxtUsers
    {
        #region 客户管理
        /// <summary>
        /// 验证客户名称
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customerName"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        public string CheckCustomerName(int customerId, string customerName, int fxtCompanyId)
        {
            List<SysCustomer> list = GetCustomerList(customerId, 0, customerName, fxtCompanyId, null);
            return (list != null && list.Count > 0) ? Utility.GetJson(0, "客户名称已存在", null, 0) : Utility.GetJson(1, "成功", null, 0);
        }

        /// <summary>
        /// 新增、修改客户
        /// </summary>
        /// <param name="dataCustomer">客户JSON对象</param>
        /// <returns></returns>
        public string AddEditCustomer(string dataCustomer)
        {
            SysCustomer sysCustomer = Utils.Deserialize<SysCustomer>(dataCustomer);
            if (sysCustomer.FxtCompanyId <= 0)
            {
                return Utility.GetJson(0, "从属公司不存在,请联系管理员添加公司至用户中心", null, 0);
            }
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            //新增、修改时需要验证公司名称           
            List<SysCustomer> list = GetCustomerList(sysCustomer.CustomerId, 0, sysCustomer.CustomerName, sysCustomer.FxtCompanyId, null);
            if (list != null && list.Count() > 0)
            {
                return Utility.GetJson(0, "客户名称已存在", null, 0);
            }
            if (sysCustomer.CustomerId <= 0)
            {
                flag = BaseDA.InsertFromEntity<SysCustomer>(sysCustomer);
            }
            else
            {
                if (sysCustomer.FxtCompanyId == 25) return Utility.GetJson(0, "房迅通不允许修改");
                sysCustomer.SetIgnoreFields(new string[] { "FxtCompanyId", "FxtCompanyName", "CustomerType", "Valid", "CreateUserId", "CreateDate" });
                flag = BaseDA.UpdateFromEntity<SysCustomer>(sysCustomer);
            }
            if (flag > 0)
                return Utility.GetJson(1, "操作成功");
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 删除、激活客户
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public string DeleteActiveCustomer(int customerId, int valid)
        {
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            //验证客户      
            List<SysCustomer> list = GetCustomerList(0, customerId, null, 0, null);
            if (list == null || list.Count() <= 0)
            {
                return Utility.GetJson(0, "客户不存在", null, 0);
            }
            if (valid == 0) //删除时，验证是否存在用户
            {
                List<SysUser> userlist = GetUserList(0, 0, null, 1, customerId);
                if (userlist != null && userlist.Count() > 0)
                {
                    return Utility.GetJson(0, ("当前客户下存在" + userlist.Count() + "个用户，不允许删除"), null, 0);
                }
            }
            SysCustomer sysCustomer = list.FirstOrDefault();
            if (sysCustomer.FxtCompanyId == 25) return Utility.GetJson(0, "房迅通不允许操作");
            sysCustomer.Valid = valid;
            sysCustomer.SetAvailableFields(new string[] { "valid" });
            flag = BaseDA.UpdateFromEntity<SysCustomer>(sysCustomer);
            if (flag > 0)
                return Utility.GetJson(1, "操作成功");
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 获取客户列表、单个客户信息
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="key">查询关键字</param>
        /// <param name="customerType">客户类型</param>
        /// <param name="fxtCompanyId">从属公司ID</param>
        /// <param name="valid">是否有效</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        public string GetCustomer(int customerId, string key, int customerType, int fxtCompanyId, int valid, int pageIndex, int pageSize, string orderProperty)
        {
            UtilityPager pager = null;
            if (customerId <= 0 && pageSize > 0)
            {
                pager = new UtilityPager();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
            }
            List<SysCustomer> list = null;
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, strSql = string.Empty, searchwhere = string.Empty;
            SqlParameter[] parameters = null;
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                orderProperty = " order by " + orderProperty;
            }
            if (!Utils.IsNullOrEmpty(key))
            {
                searchwhere = " and (customername like @KEY escape '$' or fxtcompanyname like @KEY escape '$')  ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@KEY", "%" + key + "%", SqlDbType.NVarChar, 100) };
            }
            if (customerType > 0)
            {
                searchwhere += " and (customertype=" + customerType + ")  ";
            }
            if (fxtCompanyId > 0)
            {
                searchwhere += " and (fxtcompanyid=" + fxtCompanyId + ")  ";
            }
            if (valid != -1)
            {
                searchwhere += " and (valid=" + (valid == 0 ? 0 : 1) + ")  ";
            }
            if (customerId > 0)
            {
                strWhere = string.Format("{0} where valid={2} and customerid={1} ", Utility.loan_Sys_Customer, customerId, (valid == 0 ? 0 : 1));
                strSql = string.Format("{0} 1=1 and valid={2}  and customerid={1} ", Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer), customerId, (valid == 0 ? 0 : 1));
            }
            else
            {
                strWhere = string.Format("{0} where 1=1 " + searchwhere, Utility.loan_Sys_Customer);
                strSql = string.Format("{0} 1=1  " + searchwhere + orderProperty, Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer));
            }
            list = _mssqlado.GetList<SysCustomer>(strSql, pager, strWhere, parameters);
            if (customerId > 0 && list != null && list.Count() > 0)
            {
                return Utility.GetJson(1, "获取成功", list.FirstOrDefault(), (pager == null) ? list.Count : pager.Count);
            }
            else
            {
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
            }
        }

        /// <summary>
        /// 根据客户名称获取客户列表
        /// </summary>
        /// <param name="ignoreId">忽略的ID</param>
        /// <param name="containsId">包含的ID</param>
        /// <param name="customerName"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="valid">是否有效</param>
        /// <returns></returns>
        private List<SysCustomer> GetCustomerList(int ignoreId, int containsId, string customerName, int fxtCompanyId, int? valid)
        {
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, strSql = string.Empty;
            SqlParameter[] parameters = null;
            strWhere = string.Format("{0} where 1=1", Utility.loan_Sys_Customer);
            strSql = string.Format("{0} 1=1", Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer));
            if (ignoreId > 0)
            {
                strWhere += " and customerid <> " + ignoreId;
                strSql += " and customerid <> " + ignoreId;
            }
            if (containsId > 0)
            {
                strWhere += " and customerid = " + containsId;
                strSql += " and customerid = " + containsId;
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                strWhere += " and customername=@customername";
                strSql += "  and customername=@customername ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@customername", customerName, SqlDbType.NVarChar, 100) };
            }
            if (fxtCompanyId > 0)
            {
                strWhere += " and fxtcompanyid = " + fxtCompanyId;
                strSql += " and fxtcompanyid =" + fxtCompanyId;
            }
            if (valid != null)
            {
                strWhere += " and valid = " + valid;
                strSql += " and valid = " + valid;
            }
            return _mssqlado.GetList<SysCustomer>(strSql, null, strWhere, parameters);
        }
        #endregion

        #region 用户管理
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public string UserLogin(string userName, string userPwd)
        {
            List<SysUser> list = GetUserList(0, 0, userName, 1);
            if (list == null || list.Count() <= 0)
            {
                return Utility.GetJson(0, "用户名输入错误", null, 0);
            }
            SysUser user = (SysUser)list.FirstOrDefault();
            if (user.UserPwd != userPwd)
            {
                return Utility.GetJson(0, "密码输入错误", null, 0);
            }
            else
            {
                UserInfo loginUser = new UserInfo()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    TrueName = user.TrueName,
                    UserPwd = user.UserPwd,
                    EmailStr = user.EmailStr,
                    Mobile = user.Mobile,
                    WxOpenId = user.WxOpenId,
                    FxtCompanyId = user.FxtCompanyId,
                    CustomerId = user.CustomerId,
                };

                MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
                string sql = string.Format("{0} customerid in (" + loginUser.CustomerId + ") ", Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer));
                List<SysCustomer> customerlist = _mssqlado.GetList<SysCustomer>(sql);
                if (customerlist != null && customerlist.Count > 0)
                {
                    loginUser.CustomerName = customerlist.FirstOrDefault().CustomerName;
                    loginUser.CustomerType = customerlist.FirstOrDefault().CustomerType;

                }
                //登录成功，返回用户实体
                loginUser.UserPwd = "";
                return Utility.GetJson(1, "登录成功", loginUser);
            }
        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string CheckUserName(int id, string userName)
        {
            List<SysUser> list = GetUserList(id, 0, userName, null);
            return (list != null && list.Count > 0) ? Utility.GetJson(0, "用户名已存在", null, 0) : Utility.GetJson(1, "成功", null, 0);
        }

        /// <summary>
        /// 新增、修改用户
        /// </summary>
        /// <param name="dataUser">用户JSON对象</param>
        /// <returns></returns>
        public string AddEditUser(string dataUser)
        {
            SysUser sysUser = Utils.Deserialize<SysUser>(dataUser);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            if (sysUser.Id <= 0)//新增
            {
                //新增时验证用户名称           
                List<SysUser> list = GetUserList(sysUser.Id, 0, sysUser.UserName, null);
                if (list != null && list.Count() > 0)
                {
                    return Utility.GetJson(0, "用户名已存在", null, 0);
                }
                flag = BaseDA.InsertFromEntity<SysUser>(sysUser);
            }
            else//修改
            {
                if (string.IsNullOrEmpty(sysUser.UserPwd))//密码为空，不修改密码
                {
                    sysUser.SetIgnoreFields(new string[] { "CustomerId", "FxtCompanyId", "UserPwd", "UserName", "Valid", "CreateUserId", "CreateDate" });
                }
                else
                {
                    sysUser.SetIgnoreFields(new string[] { "CustomerId", "FxtCompanyId", "UserName", "Valid", "CreateUserId", "CreateDate" });
                }
                flag = BaseDA.UpdateFromEntity<SysUser>(sysUser);
            }
            if (flag > 0)
                return Utility.GetJson(1, "操作成功");
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 删除、激活用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public string DeleteActiveUser(int id, int valid)
        {
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            //验证用户         
            List<SysUser> list = GetUserList(0, id, null, null);
            if (list == null || list.Count() <= 0)
            {
                return Utility.GetJson(0, "用户不存在", null, 0);
            }
            SysUser sysUser = list.FirstOrDefault();
            if (valid == 1) //激活时，验证客户是否有效
            {
                //验证客户      
                List<SysCustomer> custoemrlist = GetCustomerList(0, sysUser.CustomerId, null, 0, 1);
                if (custoemrlist == null || custoemrlist.Count() <= 0)
                {
                    return Utility.GetJson(0, "客户已删除，不允许激活用户", null, 0);
                }
            }
            if (sysUser.UserName == "admin@fxt") return Utility.GetJson(0, "管理员不允许删除");
            sysUser.Valid = valid;
            sysUser.SetAvailableFields(new string[] { "valid" });
            flag = BaseDA.UpdateFromEntity<SysUser>(sysUser);

            if (flag > 0)
                return Utility.GetJson(1, "操作成功");
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldUserPwd"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public string ModifyPassword(int id, string oldUserPwd, string userPwd)
        {
            List<SysUser> list = GetUserList(0, id, null, 1);
            if (list == null || list.Count() <= 0)
            {
                return Utility.GetJson(0, "用户不存在", null, 0);
            }
            SysUser user = (SysUser)list.FirstOrDefault();
            if (user.UserPwd != oldUserPwd)
            {
                return Utility.GetJson(0, "原密码输入错误", null, 0);
            }
            else
            {
                SysUser sysUser = new SysUser()
                {
                    Id = id,
                    UserPwd = userPwd
                };
                MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
                sysUser.SetAvailableFields(new string[] { "userPwd" });
                return (BaseDA.UpdateFromEntity<SysUser>(sysUser) > 0) ? Utility.GetJson(1, "操作成功") : Utility.GetJson(0, "");
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="customerId"></param>
        /// <param name="valid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        public string GetUser(int id, string key, int fxtCompanyId, int customerId, int valid, int pageIndex, int pageSize, string orderProperty)
        {
            UtilityPager pager = null;
            if (id <= 0 && pageSize > 0)
            {
                pager = new UtilityPager();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
            }
            List<UserInfo> list = null;
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, strSql = string.Empty, searchwhere = string.Empty;
            SqlParameter[] parameters = null;
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                orderProperty = " order by " + orderProperty;
            }
            if (!Utils.IsNullOrEmpty(key))
            {
                searchwhere = " and (username like @KEY escape '$' or truename like @KEY escape '$')  ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@KEY", "%" + key + "%", SqlDbType.NVarChar, 100) };
            }
            if (fxtCompanyId > 0)
            {
                searchwhere += " and (fxtcompanyid=" + fxtCompanyId + ")  ";
            }
            if (customerId > 0)
            {
                searchwhere += " and (customerid=" + customerId + ")  ";
            }
            if (valid != -1)
            {
                searchwhere += " and (valid=" + (valid == 0 ? 0 : 1) + ")  ";
            }
            if (id > 0)
            {
                strWhere = string.Format("{0} where valid={2} and id={1} ", Utility.loan_Sys_User, id, (valid == 0 ? 0 : 1));
                strSql = string.Format("{0} 1=1 and valid={2}  and id={1} ", Utility.GetMSSQL_SQL(typeof(SysUser), Utility.loan_Sys_User), id, (valid == 0 ? 0 : 1));
            }
            else
            {
                strWhere = string.Format("{0} where 1=1 " + searchwhere, Utility.loan_Sys_User);
                strSql = string.Format("{0} 1=1  " + searchwhere + orderProperty, Utility.GetMSSQL_SQL(typeof(SysUser), Utility.loan_Sys_User));
            }
            list = _mssqlado.GetList<UserInfo>(strSql, pager, strWhere, parameters);
            if (list.Count > 0)
            {
                string sql = string.Format("{0} customerid in (" + string.Join(",", list.Select(o => o.CustomerId).ToArray().Distinct()) + ") ", Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer));
                List<SysCustomer> customerlist = _mssqlado.GetList<SysCustomer>(sql);
                if (customerlist == null || customerlist.Count <= 0)
                {
                    return Utility.GetJson(0, "获取失败,数据有误");
                }
                list.ForEach(o =>
                {
                    o.CustomerName = customerlist.Where(p => p.CustomerId == o.CustomerId).FirstOrDefault().CustomerName;
                    o.CustomerValid = customerlist.Where(p => p.CustomerId == o.CustomerId).FirstOrDefault().Valid;
                });
                //清空用户密码
                list.ForEach(o => o.UserPwd = "");
                if (id > 0)
                {
                    return Utility.GetJson(1, "获取成功", list.FirstOrDefault(), (pager == null) ? list.Count : pager.Count);
                }
                else
                {
                    return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
                }
            }
            else
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);

        }

        /// <summary>
        /// 根据参数获取用户列表
        /// </summary>
        /// <param name="ignoreId">忽略的ID</param>
        /// <param name="containsId">包含的ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="valid">是否有效</param>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        private List<SysUser> GetUserList(int ignoreId, int containsId, string userName, int? valid, int customerId = 0)
        {
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, strSql = string.Empty;
            SqlParameter[] parameters = null;
            strWhere = string.Format("{0} where 1=1", Utility.loan_Sys_User);
            strSql = string.Format("{0} 1=1 and 1=1", Utility.GetMSSQL_SQL(typeof(SysUser), Utility.loan_Sys_User));
            if (ignoreId > 0)
            {
                strWhere += " and id <> " + ignoreId;
                strSql += " and id <> " + ignoreId;
            }
            if (containsId > 0)
            {
                strWhere += " and id = " + containsId;
                strSql += " and id = " + containsId;
            }
            if (!string.IsNullOrEmpty(userName))
            {
                strWhere += " and username=@username";
                strSql += "  and username=@username ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@username", userName, SqlDbType.NVarChar, 100) };
            }
            if (valid != null)
            {
                strWhere += " and valid = " + valid;
                strSql += " and valid = " + valid;
            }
            if (customerId > 0)
            {
                strWhere += " and customerid = " + customerId;
                strSql += " and customerid = " + customerId;
            }
            return _mssqlado.GetList<SysUser>(strSql, null, strWhere, parameters);
        }
        #endregion

        #region old
        /*
        #region 产品
        public string GetProduct()
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);


            Type model = typeof(SysProduct);
            IList<SysProduct> list = null;
            IList<Tree> listTree = new List<Tree>();

            string sql = string.Format("{0} Id>0",
                Utility.GetMSSQL_SQL(typeof(SysProduct), Utility.SysProduct));

            list = mssqlado.GetList<SysProduct>(sql);

            foreach (var pitem in list)//父级
            {
                Tree pt = new Tree();
                pt.Id = pitem.Id;
                pt.ParentId = 0;
                pt.Text = pitem.ProductName;
                pt.IsMenu = true;

                sql = string.Format("{0} ProductId={1}",
                Utility.GetMSSQL_SQL(typeof(SysProduct), Utility.SysProduct), pitem.Id);

                var listpm = mssqlado.GetList<SysProductMenu>(sql);
                if (listpm != null && listpm.Count > 0)
                {
                    IList<Tree> listTreeMenu = new List<Tree>();

                    StringBuilder sbMenuId = new StringBuilder();
                    foreach (var itempm in listpm)
                    {
                        sbMenuId.AppendFormat("{0},", itempm.MenuId);
                    }

                    sql = string.Format("{0} Id in ({1})",
                        Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu),
                        sbMenuId.ToString().TrimEnd(','));

                    IList<SysMenu> listmenu = mssqlado.GetList<SysMenu>(sql);//得到相关产品所属的所有菜单

                    foreach (var itemmenu in listmenu)
                    {
                        Tree mt = new Tree();
                        mt.Id = itemmenu.Id;
                        mt.Text = itemmenu.MenuName;
                        mt.ParentId = pitem.Id;
                        mt.IsMenu = true;

                        sql = string.Format("{0} Id in ({1})",
                        Utility.GetMSSQL_SQL(typeof(SysMenuPurview), Utility.SysMenuPurview),
                        sbMenuId.ToString().TrimEnd(','));

                        var listmp = mssqlado.GetList<SysMenuPurview>(sql);//得到菜单的权限

                        if (listmp != null && listmp.Count > 0)
                        {
                            IList<Tree> listTreePurview = new List<Tree>();
                            StringBuilder sbPurviewId = new StringBuilder();
                            foreach (var itempm in listmp)
                            {
                                sbPurviewId.AppendFormat("{0},", itempm.PurviewId);
                            }

                            sql = string.Format("{0} Id in ({1})",
                                Utility.GetMSSQL_SQL(typeof(SysPurview), Utility.SysPurview),
                                sbPurviewId.ToString().TrimEnd(','));

                            IList<SysPurview> listpurview = mssqlado.GetList<SysPurview>(sql);//得到相关菜单所属的所有权限
                            foreach (var itempv in listpurview)
                            {
                                Tree put = new Tree();
                                put.Id = itempv.Id;
                                put.ParentId = itemmenu.Id;
                                put.Text = itempv.PurviewName;
                                listTreePurview.Add(put);

                            }
                            mt.Children = listTreePurview;
                        }

                        listTreeMenu.Add(mt);
                    }

                    pt.Children = listTreeMenu;
                }
                listTree.Add(pt);
            }
            return GetJson(1, null, listTree);

        }

        public bool Products(string products, string operat)
        {
            SysProduct sysProduct =
                JsonConvert.DeserializeObject<SysProduct>(products);
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            MSSQLADODAL.SetConnection(Utility.DBFxtData);
            string sql = string.Empty;
            bool flag = false;
            switch (operat.ToUpper())
            {
                case "D":
                    flag = Operting(sysProduct, operat);
                    if (flag)
                    {
                        sql = string.Format("{0} ProductId={1}",
                            Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu),
                            sysProduct.Id);

                        IList<SysProductMenu> delPMList = mssqlado.GetList<SysProductMenu>(sql);

                        StringBuilder sbMenuId = new StringBuilder();
                        if (delPMList != null && delPMList.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in delPMList)
                            {
                                sb.AppendFormat("{0},", item.Id);
                                sbMenuId.AppendFormat("{0},", item.MenuId);
                            }
                            sql = string.Format("{0} Id in ({1})",
                                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu),
                                sb.ToString().TrimEnd(','));
                            flag = mssqlado.CUD(sql) > 0;//产品关联菜单表删除
                            if (flag)
                            {
                                sql = string.Format("{0} Id in ({1})",
                                    Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu),
                                    sbMenuId.ToString().TrimEnd(','));
                                flag = mssqlado.CUD(sql) > 0;//相关菜单删除
                            }
                        }
                    }
                    break;
                default:
                    if (operat.Equals("C"))
                    {
                        sysProduct.CreateDate = DateTime.Now;
                        flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysProduct>(sysProduct) > 0;
                    }
                    else if (operat.Equals("U"))
                    {
                        sql = string.Format("{0} Id={1}",
                            Utility.GetMSSQL_SQL(typeof(SysProduct), Utility.SysProduct), sysProduct.Id);
                        sysProduct.CreateDate = mssqlado.GetModel<SysProduct>(sql).CreateDate;
                        flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysProduct>(sysProduct) > 0;
                    }
                    else if (operat.Equals("D"))
                    {
                        flag = CAS.DataAccess.DA.BaseDA.DeleteByPrimaryKey<SysProduct>(sysProduct) > 0;
                    }
                    break;
            }
            return flag;
        }

        public string GetProducts(int id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} Id={1}",
                Utility.GetMSSQL_SQL(typeof(SysProduct), Utility.SysProduct), id);
            SysProduct sysProduct = mssqlado.GetModel<SysProduct>(sql);

            return GetJson(1, null, sysProduct);
        }

        public string GetProductAll()
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} Id>0", Utility.GetMSSQL_SQL(typeof(SysProduct), Utility.SysProduct));
            IList<SysProduct> listp = mssqlado.GetList<SysProduct>(sql);
            return GetJson(1, null, listp);
        }

        public string GetProductMenuByPurview(int id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} ProductId={1}",
                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu), id);

            IList<SysProductMenu> listProductMenu = mssqlado.GetList<SysProductMenu>(sql);//得到该产品所有的菜单

            List<Tree> listp = new List<Tree>();
            StringBuilder sbProductMenu = new StringBuilder();
            foreach (var item in listProductMenu)
            {
                sbProductMenu.AppendFormat("{0},", item.MenuId);
            }

            sql = string.Format("{0} Id in ({1})",
                Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu),
                sbProductMenu.ToString().TrimEnd(','));//得到菜单
            if (!string.IsNullOrEmpty(sbProductMenu.ToString()))
            {
                IList<SysMenu> listMenu = mssqlado.GetList<SysMenu>(sql);

                foreach (var itemm in listMenu)
                {
                    Tree ptree = new Tree()
                    {
                        Id = itemm.Id,
                        Text = itemm.MenuName
                    };
                    sql = string.Format("{0} MenuId={1}",
                        Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu), itemm.Id);
                    IList<SysMenuPurview> listMenuPurview = mssqlado.GetList<SysMenuPurview>(sql);//得到菜单的所有权限

                    List<Tree> listc = new List<Tree>();
                    foreach (var itemmp in listMenuPurview)
                    {
                        sql = string.Format("{0} Id={1}",
                        Utility.GetMSSQL_SQL(typeof(SysPurview), Utility.SysPurview), itemmp.PurviewId.Value);

                        SysPurview sysPurview = mssqlado.GetModel<SysPurview>(sql);
                        Tree _tree = new Tree()
                            {
                                Id = itemmp.Id,
                                Text = sysPurview.PurviewName
                            };
                        listc.Add(_tree);
                    }
                    ptree.Children = listc;
                    listp.Add(ptree);
                }
            }

            return GetJson(1, null, listp);
        }

        #endregion

        #region 菜单

        public bool Menus(int productid, string menus, string operat)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            MSSQLADODAL.SetConnection(Utility.DBFxtData);
            SysMenu sysMenu =
                JsonConvert.DeserializeObject<SysMenu>(menus);
            sysMenu.CreateDate = DateTime.Now;
            bool flag = false;
            switch (operat.ToUpper())
            {
                case "C":

                    int menuId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysMenu>(sysMenu);

                    flag = menuId > 0;
                    if (flag)
                    {
                        SysProductMenu sysProductMenu = new SysProductMenu();
                        sysProductMenu.ProductId = productid;
                        sysProductMenu.MenuId = menuId;
                        sysProductMenu.CreateDate = DateTime.Now;
                        flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysProductMenu>(sysProductMenu) > 0;
                    }
                    break;
                case "U":
                    flag = Operting(sysMenu, operat);
                    if (flag)
                    {
                        JObject uObj = JObject.Parse(GetProductMenus(sysMenu.Id, 0));
                        SysProductMenu spm =
                            JsonConvert.DeserializeObject<SysProductMenu>(uObj["data"].ToString());
                        SysProductMenu usmp = new SysProductMenu();
                        usmp.ProductId = productid;
                        usmp.MenuId = sysMenu.Id;
                        usmp.Id = spm.Id;
                        usmp.CreateDate = spm.CreateDate;
                        flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysProductMenu>(usmp) > 0;
                    }
                    break;
                case "D":
                    flag = Operting(sysMenu, operat);
                    if (flag)
                    {
                        string sql = string.Format("{0} ParentId={1}",
                            Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu), sysMenu.Id);
                        IList<SysMenu> delList = mssqlado.GetList<SysMenu>(sql);
                        StringBuilder sbId = new StringBuilder();
                        StringBuilder sbProdcuctMenuId = new StringBuilder();
                        //string hql = string.Empty;
                        //当子集满足时才执行
                        if (delList != null && delList.Count > 0)
                        {
                            foreach (var item in delList)
                            {
                                sbId.AppendFormat("{0},", item.Id);
                                sbProdcuctMenuId.AppendFormat("{0},", item.Id);
                            }
                            sql = string.Format("{0} Id in ({1})",
                            Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu),
                            sbId.ToString().TrimEnd(','));

                            flag = mssqlado.CUD(sql) > 0;
                        }

                        //查找与产品相关的所有菜单关联表信息
                        sbProdcuctMenuId.AppendFormat("{0},", sysMenu.Id);
                        sql = string.Format("{0} MenuId in ({1})",
                                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu),
                                sbProdcuctMenuId.ToString().TrimEnd(','));
                        IList<SysProductMenu> delPMList = mssqlado.GetList<SysProductMenu>(sql);
                        if (delPMList != null && delPMList.Count > 0)
                        {
                            sbProdcuctMenuId.Clear();
                            foreach (var item in delPMList)
                            {
                                sbProdcuctMenuId.AppendFormat("{0},", item.Id);
                            }

                            sql = string.Format("{0} Id in ({1})",
                                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu),
                                sbProdcuctMenuId.ToString().TrimEnd(','));

                            flag = mssqlado.CUD(sql) > 0;
                        }
                    }
                    break;
            }
            return flag;
        }

        public bool MenuPurview(int menuid, string purview)
        {
            bool flag = false;
            if (!menuid.Equals(0))
            {
                MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
                MSSQLADODAL.SetConnection(Utility.DBFxtData);
                string sql = string.Format("{0} MenuId={1}",
                    Utility.GetMSSQL_SQL(typeof(SysMenuPurview), Utility.SysMenuPurview),
                    menuid);

                flag = mssqlado.CUD(sql) > 0;
                if (flag)
                {
                    if (!string.IsNullOrEmpty(purview))
                    {
                        string[] array = purview.Split(',');
                        foreach (var item in array)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysMenuPurview>(new SysMenuPurview()
                                {
                                    MenuId = menuid,
                                    PurviewId = Convert.ToInt32(item)
                                });
                            }
                        }
                    }
                }
            }
            return flag;
        }

        public string GetMenuPurview(int menuid)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);

            string sql = string.Format("{0} MenuId={1}",
                Utility.GetMSSQL_SQL(typeof(SysMenuPurview), Utility.SysMenuPurview),
                menuid);

            IList<SysMenuPurview> list = mssqlado.GetList<SysMenuPurview>(sql);

            JObject _jobject = new JObject();
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    sb.AppendFormat("{0},", item.PurviewId);
                }
                _jobject.Add(new JProperty("purview"
                    , sb.ToString().TrimEnd(',')));
            }
            return GetJson(1, null, _jobject);
        }

        public string GetMenuListByProductId(int productid)
        {
            MSSQLADODAL mssql = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} ProductId={1}",
                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu), productid);

            IList<SysProductMenu> spmList = mssql.GetList<SysProductMenu>(sql);

            IList<SysMenu> list = null;
            if (spmList != null && spmList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in spmList)
                {
                    sb.AppendFormat("{0},", item.MenuId);
                }

                sql = string.Format("{0} ParentId=0 and  Id in ({1})",
                        Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu),
                        sb.ToString().TrimEnd(','));

                list = mssql.GetList<SysMenu>(sql);
            }

            return GetJson(1, null, list);
        }

        public string GetMenuListAll()
        {
            try
            {
                MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
                Type model = typeof(SysMenu);
                string sql = string.Format("{0} 1=1", Utility.GetMSSQL_SQL(model, Utility.SysMenu));
                IList<SysMenu> list = mssqlado.GetList<SysMenu>(sql);
                return GetJson(1, null, list);
            }
            catch (Exception ex)
            {
                return GetJson(0, ex.Message, null);
            }
        }

        public string GetMenus(int id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} Id={1}",
                Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu), id);
            SysMenu sysMenu = mssqlado.GetModel<SysMenu>(sql);
            if (sysMenu.ParentId != 0)
            {
                sql = string.Format("{0} Id={1}",
                Utility.GetMSSQL_SQL(typeof(SysMenu), Utility.SysMenu), sysMenu.ParentId);

                sysMenu.ParentId = mssqlado.GetModel<SysMenu>(sql).Id;
            }
            SysMenus sysmenus = new SysMenus(sysMenu);

            sql = string.Format("{0} MenuId={1}",
                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu), id);

            sysmenus.ProductId = mssqlado.GetModel<SysProductMenu>(sql).ProductId;

            return GetJson(1, null, sysmenus);
        }


        public bool ProductMenus(string productmenus, string operat)
        {
            SysProductMenu sysProductMenu =
                JsonConvert.DeserializeObject<SysProductMenu>(productmenus);
            bool flag = false;
            MSSQLADODAL.SetConnection(Utility.DBFxtData);
            if (operat.ToUpper().Equals("C"))
            {
                sysProductMenu.CreateDate = DateTime.Now;
                flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysProductMenu>(sysProductMenu) > 0;
            }
            else if (operat.ToUpper().Equals("U"))
            {
                flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysProductMenu>(sysProductMenu) > 0;
            }
            else if (operat.ToUpper().Equals("D"))
            {
                flag = CAS.DataAccess.DA.BaseDA.DeleteByPrimaryKey<SysProductMenu>(sysProductMenu) > 0;
            }
            return flag;// Operting(sysProductMenu, operat);
        }

        public string GetProductMenus(int menuid, int productid)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            string sql = string.Format("{0} MenuId={1} || ProductId={2}",
                Utility.GetMSSQL_SQL(typeof(SysProductMenu), Utility.SysProductMenu), menuid, productid);
            SysProductMenu smp = mssqlado.GetModel<SysProductMenu>(sql);
            return GetJson(1, "", smp);
        }

        #endregion

        #region 权限

        public string GetPurview(string purview, int pageSize, int pageIndex)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);

            SysPurview sysProduct = JsonConvert.DeserializeObject<SysPurview>(purview);

            string sql = string.Format("{0} {1}",
                Utility.GetMSSQL_SQL(typeof(SysPurview), Utility.SysPurview),
                Utility.GetModelFieldKeyValue(sysProduct)),
                sqlWhere = string.Format("{0} where {1}", Utility.SysPurview,
                Utility.GetModelFieldKeyValue(sysProduct));

            UtilityPager pager = new UtilityPager(pageSize, pageIndex);
            List<SysPurview> list = mssqlado.GetList<SysPurview>(sql, pager, sqlWhere);
            if (list != null && list.Count > 0)
            {
                return GetJson(1, "成功", list, pager.Count);
            }
            return GetJson(0, null);
        }

        public string GetPurviewAll()
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);

            List<Tree> _listp = new List<Tree>();
            Tree tree = new Tree()
            {
                Id = 0,
                Text = ""
            };

            string sql = string.Format("{0} Id>0",
                Utility.GetMSSQL_SQL(typeof(SysPurview), Utility.SysPurview));

            List<SysPurview> list = mssqlado.GetList<SysPurview>(sql);

            List<Tree> _listc = new List<Tree>();
            foreach (var item in list)
            {
                Tree _tree = new Tree()
                {
                    Id = item.Id,
                    Text = item.PurviewName
                };
                _listc.Add(_tree);
            }
            tree.Children = _listc;
            _listp.Add(tree);

            return GetJson(1, null, _listp);

        }

        public bool Purviews(string purview, string operta)
        {
            SysPurview syspurview = JsonConvert.DeserializeObject<SysPurview>(purview);
            bool flag = false;
            if (syspurview.Id == 0)
            {
                MSSQLADODAL.SetConnection(Utility.DBFxtData);
                syspurview.CreateDate = DateTime.Now;
                flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysPurview>(syspurview) > 0;
            }
            else
            {
                if (operta.ToLower().Equals("U"))
                {
                    JObject jobject = JObject.Parse(GetPurviews(syspurview.Id));
                    SysPurview up = JsonConvert
                        .DeserializeObject<SysPurview>(jobject["data"].ToString());
                    syspurview.CreateDate = up.CreateDate;

                    MSSQLADODAL.SetConnection(Utility.DBFxtData);
                    flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysPurview>(syspurview) > 0;
                }
                else
                {
                    MSSQLADODAL.SetConnection(Utility.DBFxtData);
                    flag = CAS.DataAccess.DA.BaseDA.DeleteByPrimaryKey<SysPurview>(syspurview) > 0;
                }
            }
            return flag;
        }

        public string GetPurviews(int id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);

            string sql = string.Format("{0} Id={1}",
                Utility.GetMSSQL_SQL(typeof(SysPurview), Utility.SysPurview), id);

            SysPurview sysPurview = mssqlado.GetModel<SysPurview>(sql);
            if (sysPurview != null)
                return GetJson(1, "成功", sysPurview);
            return GetJson(0, null);
        }

        #endregion

        #region 用户

        public bool UserPurview(string userid, int productid, string purview)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);
            bool flag = false;
            string sql = string.Format("{0} UserId='{1}' and ProductId={2}",
                Utility.GetMSSQL_SQL(typeof(SysUserProduct), Utility.SysUserProduct),
                userid.Trim(), productid);
            SysUserProduct existsSUP = mssqlado.GetModel<SysUserProduct>(sql);

            if (existsSUP == null)
            {
                SysUserProduct sysUP = new SysUserProduct()
                {
                    UserId = userid.Trim(),
                    ProductId = productid,
                    CreateDate = DateTime.Now,
                    ExpiredDate = DateTime.Now.AddYears(1)
                };
                flag = Operting(sysUP, "C");
            }
            else
                flag = true;//存在不添加
            if (flag)
            {
                sql = string.Format("{0} UserId='{1}' and ProductId={2}",
                    Utility.GetMSSQL_SQL(typeof(SysUserPurview), Utility.SysUserPurview),
                    userid.Trim(), productid);

                IList<SysUserPurview> listUserPurview = mssqlado.GetList<SysUserPurview>(sql);
                //删除已有权限
                foreach (var item in listUserPurview)
                {
                    SysUserPurview dsup = new SysUserPurview()
                    {
                        ID = item.ID
                    };
                    Operting(dsup, "D");
                }

                if (!string.IsNullOrEmpty(purview))//存储用户的权限
                {
                    string[] array = purview.Split(',');
                    foreach (var item in array)
                    {
                        SysUserPurview sysUPurview = new SysUserPurview()
                        {
                            UserId = userid,
                            ProductId = productid,
                            MenuPurviewId = Convert.ToInt32(item)
                        };
                        flag = Operting(sysUPurview, "C");
                    }
                }
            }

            return flag;
        }

        public string GetUserPurview(string userid)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtData);

            string sql = string.Format("{0} UserId='{1}'",
                Utility.GetMSSQL_SQL(typeof(SysUserPurview), Utility.SysUserPurview), userid.Trim());
            IList<SysUserPurview> listUserPurview = mssqlado.GetList<SysUserPurview>(sql);

            StringBuilder sb = new StringBuilder();
            int ProductId = 0;
            foreach (var item in listUserPurview)
            {
                if (ProductId.Equals(0))
                    ProductId = item.ProductId.Value;
                sb.AppendFormat("{0},", item.MenuPurviewId);
            }
            JObject _jobject = new JObject();
            _jobject.Add(new JProperty("purview", sb.ToString().TrimEnd(',')));
            _jobject.Add(new JProperty("purviewproductid", ProductId));
            return GetJson(1, null, _jobject);
        }
        #endregion

        
        public object Entrance(string date, string code, string methodName, string methodValue)
        {

            object objClass = System.Reflection.Assembly
                .Load("FxtService.Service")
                .CreateInstance("FxtService.Service.FxtUsersActualize.FxtUser");

            JObject jobject = JObject.Parse(methodValue);
            int i = 0, count = jobject.Count;
            object[] value = new object[count];
            foreach (var item in jobject)
            {
                JTokenType vtype = item.Value.Type;
                if (vtype == JTokenType.String)
                {
                    value[i] = item.Value.Value<string>();
                }
                else if (vtype == JTokenType.Integer)
                {
                    value[i] = item.Value.Value<int>();
                }
                i++;
            }

            return objClass.GetType().GetMethod(methodName).Invoke(this, value);
        }

        bool Operting(object model, string operat)
        {
            MSSQLDBDAL mssql = new MSSQLDBDAL(Utility.DBFxtData);
            bool flag = false;
            switch (operat.ToUpper())
            {
                case "C":
                    flag = Convert.ToInt32(mssql.Create(model)) > 0;
                    break;
                case "U":
                    flag = mssql.Update(model);
                    break;
                case "D":
                    flag = mssql.Delete(model);
                    break;
            }
            mssql.Close();
            return flag;
        }

        string GetJson(int type, string message, object list = null, int count = 0)
        {
            var vobj = new
            {
                type = type,
                message = message,
                data = list,
                count = count
            };
            return JsonConvert.SerializeObject(vobj);
        }*/
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

/**
 * 作者:李晓东
 * 摘要:2014.01.02新建 
 *      2014.01.03 建立GetProducts 修改人:李晓东
 *      2014.06.11 修改人 曹青
 *                 新增 客户管理 CheckCustomerName、AddEditCustomer、GetCustomer
 *                 新增 用户管理 CheckUserName、AddEditUser、GetUser
 *      2014.06.17 修改人 曹青
 *                 新增 UserLogin、ModifyPassword
 *      2014.06.18 修改人 曹青
 *                 新增 DeleteActiveCustomer、DeleteActiveUser      
 * 
 * **/
namespace FxtService.Contract.FxtUsersInterface
{
    [ServiceContract()]
    public interface IFxtUsers
    {
        #region 客户管理
        [OperationContract]
        string CheckCustomerName(int customerId, string customerName, int fxtCompanyId);

        [OperationContract]
        string AddEditCustomer(string dataCustomer);

        [OperationContract]
        string DeleteActiveCustomer(int customerId, int valid);

        [OperationContract]
        string GetCustomer(int customerId, string key, int customerType, int fxtCompanyId, int valid, int pageIndex, int pageSize, string orderProperty);
        #endregion

        #region 用户管理
        [OperationContract]
        string UserLogin(string userName, string userPwd);

        [OperationContract]
        string CheckUserName(int id, string userName);

        [OperationContract]
        string AddEditUser(string dataUser);

        [OperationContract]
        string DeleteActiveUser(int id, int valid);

        [OperationContract]
        string ModifyPassword(int id, string oldUserPwd, string userPwd);

        [OperationContract]
        string GetUser(int id, string key, int fxtCompanyId, int customerId, int valid, int pageIndex, int pageSize, string orderProperty);
        #endregion

        #region old
        /*
        #region 产品
        [OperationContract]
        string GetProduct();

        [OperationContract]
        string GetProducts(int id);

        [OperationContract]
        bool Products(string products, string operat);

        [OperationContract]
        string GetProductAll();

        //得到产品下面的所有菜单及菜单相关的权限
        [OperationContract]
        string GetProductMenuByPurview(int id);
        #endregion
        #region 菜单
        [OperationContract]
        bool Menus(int productid, string menus, string operat);

        [OperationContract]
        bool MenuPurview(int menuid, string purview);

        [OperationContract]
        string GetMenuPurview(int menuid);

        [OperationContract]
        string GetMenuListByProductId(int productid);

        [OperationContract]
        string GetMenuListAll();

        [OperationContract]
        string GetMenus(int id);

        [OperationContract]
        bool ProductMenus(string productmenus, string operat);

        [OperationContract]
        string GetProductMenus(int menuid, int productid);
        #endregion
        #region 权限(角色)
        [OperationContract]
        string GetPurview(string purview, int pageSize, int pageIndex);

        [OperationContract]
        string GetPurviewAll();

        [OperationContract]
        bool Purviews(string purview, string operta);

        [OperationContract]
        string GetPurviews(int id);
        #endregion

        #region 用户
        //用户所有权限
        [OperationContract]
        string GetUserPurview(string userid);

        //存储用户权限
        [OperationContract]
        bool UserPurview(string userid, int productid, string purview);



        #endregion



        //验证
        [OperationContract]
        object Entrance(string date, string code, string methodName, string methodValue);
        */
#endregion
    }
}

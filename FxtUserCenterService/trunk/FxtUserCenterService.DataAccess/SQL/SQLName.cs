using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace FxtUserCenterService.DataAccess
{
    public class Common
    {
        /// <summary>
        /// 读取SQL语句，注意文件名和传入的参数大小写要匹配
        /// SQL文件必须修改为“嵌入的资源”
        /// 如果不用嵌入也可以使用文件方式读取，好处是应用程序不会重启，但明文存放可能引起容易被篡改等安全问题
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetSql(string sql)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = "FxtUserCenterService.DataAccess.SQL." + sql + ".sql";
            string result = "";
            try
            {
                Stream stream = _assembly.GetManifestResourceStream(resourceName);
                StreamReader myread = new StreamReader(stream);
                result = myread.ReadToEnd();
                myread.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //转为小写，避免sql与前台json大小写不一致
            return result.ToLower();
        }
    }
    /// <summary>
    /// sql类
    /// </summary>
    public class SQLName
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public class UserInfo 
        {
            /// <summary>
            /// 获得用户与App信息
            /// </summary>
            public static string GetUserAndAppInfo = Common.GetSql("UserInfo.GetUserAndAppInfo");

            /// <summary>
            /// 获得用户与App信息
            /// </summary>
            public static string GetApps = Common.GetSql("UserInfo.GetApps");

            /// <summary>
            /// 验证用户密码
            /// </summary>
            public static string CheckUserPwd = Common.GetSql("UserInfo.CheckUserPwd");

            /// <summary>
            /// 获取用户信息
            /// </summary>
            public static string GetUserInfo = Common.GetSql("UserInfo.GetUserInfo");  
          
            /// <summary>
            /// 修改用户密码
            /// </summary>
            public static string UpdatePassWord = Common.GetSql("UserInfo.UpdatePassWord");

            /// <summary>
            /// 据用户名获取用户真实姓名（多个用户名用逗号隔开）
            /// zhoub 20160908
            /// </summary>
            public static string GetUserInfoByUserNames = Common.GetSql("UserInfo.GetUserInfoByUserNames");

            /// <summary>
            /// 根据公司ID、用户名查询有效的客户帐号信息(数据中心网站需求)
            /// zhoub 20161026
            /// </summary>
            public static string GetUserListByUserName = Common.GetSql("UserInfo.GetUserListByUserName");

            /// <summary>
            /// 根据公司ID、用户名或真实姓名查询客户帐号信息(数据中心网站需求)
            /// zhoub 20161026
            /// </summary>
            public static string GetUserListByUserNameOrTrueName = Common.GetSql("UserInfo.GetUserListByUserNameOrTrueName");
        }
        /// <summary>
        /// 公司表
        /// </summary>
        public class Company
        {
            /// <summary>
            /// 获得公司信息通过signname
            /// </summary>
            public static string GetCompanyInfoBySignName = Common.GetSql("Company.GetCompanyInfoBySignName");

             /// <summary>
            /// 获得公司列表
            /// </summary>
            public static string GetCompanyList = Common.GetSql("Company.GetCompanyList");

            /// <summary>
            /// 获得已签约客户的
            /// </summary>
            public static string GetCompanyListIssigned = Common.GetSql("Company.GetCompanyListIssigned");

            /// <summary>
            /// 新增客户
            /// </summary>
            public static string AddCompany = Common.GetSql("Company.AddCompany");

            /// <summary>
            /// 根据产品code获取已签约且业务数据库连接不为空的公司
            /// </summary>
            public static string GetCompanyBusinessDBList = Common.GetSql("Company.GetCompanyBusinessDB");

            /// <summary>
            /// 根据公司ID获取公司信息（多个ID用逗号隔开）
            /// zhoub 20160908
            /// </summary>
            public static string GetCompanyInfoByCompanyIds = Common.GetSql("Company.GetCompanyInfoByCompanyIds"); 
        }

        /// <summary>
        /// 公司表
        /// </summary>
        public class ProductApp
        {
            /// <summary>
            /// 获得Appkey 
            /// </summary>
            public static string GetAppkey = Common.GetSql("ProductApp.GetAppkey");

            /// <summary>
            /// 获得Appkey 
            /// </summary>
            public static string GetProductAPIInfo = Common.GetSql("ProductApp.GetProductAPIInfo");
        }

        /// <summary>
        /// 产品表
        /// </summary>
        public class CompanyProduct
        {
            /// <summary>
            /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话
            /// </summary>
            public static string UpdateProductPartialInfo = Common.GetSql("CompanyProduct.UpdateProductPartialInfo");
            /// <summary>
            ///根据WebUrl查询产品信息
            /// </summary>
            public static string GetProductInfoByWebUrl = Common.GetSql("CompanyProduct.GetProductInfoByWebUrl");

            /// <summary>
            ///根据公司ID和产品code获取公司开通产品模块城市ID(数据中心网站需求)
            /// </summary>
            public static string GetCompanyProductAndCompanyProductModuleCityIds = Common.GetSql("CompanyProduct.GetCompanyProductAndCompanyProductModuleCityIds");

            /// <summary>
            /// 根据公司ID和产品code获取公司开通产品模块详细信息(数据中心网站需求)
            /// </summary>
            public static string GetCompanyProductModuleDetails = Common.GetSql("CompanyProduct.GetCompanyProductModuleDetails");
        }

        /// <summary>
        /// 产品表
        /// </summary>
        public class FxtLog
        {
            /// <summary>
            /// 操作日志
            /// </summary>
            public static string InsertLog = Common.GetSql("FxtLog.InsertLog");
            /// <summary>
            ///登录
            /// </summary>
            public static string SignIn = Common.GetSql("FxtLog.SignIn");
            /// <summary>
            /// 退出
            /// </summary>
            public static string SignOut = Common.GetSql("FxtLog.SignOut");
            /// <summary>
            ///更新在线时间
            /// </summary>
            public static string UpdateActiveTime = Common.GetSql("FxtLog.UpdateActiveTime");
        }

        /// <summary>
        /// 简单密码
        /// </summary>
        public class SimplePassWord
        {
            /// <summary>
            /// 是否存在简单密码表中
            /// </summary>
            public static string CheckIsSimplePassWord = Common.GetSql("SimplePassWord.CheckIsSimplePassWord");
        }

        /// <summary>
        /// 应用安全信息表
        /// </summary>
        public class CompanyProductSafe
        {
            /// <summary>
            /// 验证调用身份
            /// </summary>
            public static string ValidateCallIdentity = Common.GetSql("CompanyProductSafe.ValidateCallIdentity");
        }
    }
}

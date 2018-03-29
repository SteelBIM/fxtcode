using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CAS.Common;
using FxtNHibernate.DTODomain.FxtLoanDTO;

namespace FxtCollateralManager.Common
{
    public class Public
    {
        private static UserInfo loginInfo = null;
        /// <summary>
        /// 登录后公用变量
        /// </summary>
        public static UserInfo LoginInfo
        {
            get
            {
                loginInfo = new UserInfo();
                loginInfo.Id = StringHelper.TryGetInt(SessionHelper.Get("UserId"));
                if (loginInfo.Id > 0)
                {
                    loginInfo.UserName = SessionHelper.Get("UserName");
                    loginInfo.TrueName = SessionHelper.Get("TrueName");
                    loginInfo.FxtCompanyId = StringHelper.TryGetInt(SessionHelper.Get("FxtCompanyId"));
                    loginInfo.CustomerId = StringHelper.TryGetInt(SessionHelper.Get("CustomerId"));
                    loginInfo.CustomerName = SessionHelper.Get("CustomerName");
                    loginInfo.CustomerType = StringHelper.TryGetInt(SessionHelper.Get("CustomerType"));
                    loginInfo.EmailStr = SessionHelper.Get("EmailStr");
                    loginInfo.Mobile = SessionHelper.Get("Mobile");
                }
                return loginInfo;
            }
            set { loginInfo = value; }
        }

        /// <summary>
        /// 验证用户是否已登录
        /// </summary>
        /// <returns></returns>
        public static bool CheckLogin()
        {
            bool rtn = Public.LoginInfo.Id > 0;
            //if (!rtn)
            //{
                //未登录
                //HttpContext.Current.Response.Redirect("/Home/Login");
                //HttpContext.Current.Response.Write("<script type='text/javascript'>alert('登录信息已过期，请重新登录');top.location.href='/Home/Login';</script>");
                //HttpContext.Current.Response.End();
            //}
            return rtn;
        }

        public static Dictionary<int, List<int>> rightdic = null;//各公司类型对应权限
        /// <summary>
        /// 判断是否存在权限
        /// </summary>
        /// <param name="priv"></param>
        /// <returns></returns>
        public static bool CheckHasPrivilege(EnumHelper.PrivilegeNo priv)
        {
            //未登录
            if (LoginInfo.Id <= 0) return false;

            bool hasRight = false;
            if (priv == EnumHelper.PrivilegeNo.UserIsLogin)// 用户登录即有
            {
                hasRight = true;
            }
            else
            {
                if (rightdic == null)
                {
                    rightdic = new Dictionary<int, List<int>>();
                    #region 权限赋值
                    //房迅通
                    rightdic.Add((int)EnumHelper.CustomerType.Company_Fxt, new List<int>(){ 
                            (int)EnumHelper.PrivilegeNo.ProjectManage//管理项目及任务
                            ,(int)EnumHelper.PrivilegeNo.CollateralManage//押品管理
                            ,(int)EnumHelper.PrivilegeNo.CollateralMonitor//押品监测
                            ,(int)EnumHelper.PrivilegeNo.CollateralRePrice//押品复估
                            ,(int)EnumHelper.PrivilegeNo.CollateralAssetsValueMonitor//押品资产价值动态监测
                            ,(int)EnumHelper.PrivilegeNo.StressTest//压力测试
                            ,(int)EnumHelper.PrivilegeNo.CustomerManage//客户管理
                            ,(int)EnumHelper.PrivilegeNo.ManageMyFile//我的文件
                            ,(int)EnumHelper.PrivilegeNo.Import//导入
                            ,(int)EnumHelper.PrivilegeNo.Export//导出
                        });
                    //银行
                    rightdic.Add((int)EnumHelper.CustomerType.Company_Bank, new List<int>(){ 
                            (int)EnumHelper.PrivilegeNo.CollateralMonitor//押品监测
                            ,(int)EnumHelper.PrivilegeNo.CollateralAssetsValueMonitor//押品资产价值动态监测
                            ,(int)EnumHelper.PrivilegeNo.StressTest//压力测试
                            ,(int)EnumHelper.PrivilegeNo.Import//导入
                            ,(int)EnumHelper.PrivilegeNo.Export//导出
                        });
                    //评估机构
                    rightdic.Add((int)EnumHelper.CustomerType.Company_Soa, new List<int>(){ 
                            (int)EnumHelper.PrivilegeNo.ProjectManage//管理项目及任务
                            ,(int)EnumHelper.PrivilegeNo.CollateralManage//押品管理
                            ,(int)EnumHelper.PrivilegeNo.CollateralMonitor//押品监测
                            ,(int)EnumHelper.PrivilegeNo.CollateralRePrice//押品复估
                            ,(int)EnumHelper.PrivilegeNo.CollateralAssetsValueMonitor//押品资产价值动态监测
                            ,(int)EnumHelper.PrivilegeNo.StressTest//压力测试
                            ,(int)EnumHelper.PrivilegeNo.ManageMyFile//我的文件
                            ,(int)EnumHelper.PrivilegeNo.Import//导入
                            ,(int)EnumHelper.PrivilegeNo.Export//导出             
                        });
                    #endregion
                }
                if (!hasRight && rightdic != null && rightdic.ContainsKey(LoginInfo.CustomerType))
                {
                    List<int> rights = rightdic[LoginInfo.CustomerType];//权限                    
                    if (rights != null && rights.Count > 0)
                        hasRight = rights.Contains((int)priv);
                }
            }
            return hasRight;
        }
    }
}

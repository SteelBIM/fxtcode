using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/***
 * 作者:  曹青
 * 时间:  2014.06.19
 * 摘要:  创建 EnumHelper 枚举类，存储公司角色、功能模块权限
 * **/
namespace FxtCollateralManager.Common
{
    /// <summary>
    /// 枚举类
    /// </summary>
    public class EnumHelper
    {
        //公司角色
        public enum CustomerType
        {
            /// <summary>
            /// 房迅通
            /// </summary>
            Company_Fxt = 1,
            /// <summary>
            /// 银行
            /// </summary>
            Company_Bank = 2001013,
            /// <summary>
            /// 评估机构
            /// </summary>
            Company_Soa = 2001010,
        }

        //功能模块权限
        public enum PrivilegeNo
        {
            /// <summary>
            /// 登录
            /// </summary>
            UserIsLogin = 1,

            /// <summary>
            /// 管理项目及任务
            /// </summary>
            ProjectManage = 10001,
            /// <summary>
            /// 押品管理
            /// </summary>
            CollateralManage = 10002,
            /// <summary>
            /// 押品监测
            /// </summary>
            CollateralMonitor = 10003,
            /// <summary>
            /// 押品复估
            /// </summary>
            CollateralRePrice = 10004,
            /// <summary>
            /// 押品资产价值动态监测
            /// </summary>
            CollateralAssetsValueMonitor = 10005,
            /// <summary>
            /// 压力测试
            /// </summary>
            StressTest = 10006,
            /// <summary>
            /// 客户管理
            /// </summary>
            CustomerManage = 10007,
            /// <summary>
            /// 我的文件
            /// </summary>
            ManageMyFile = 10008,
            /// <summary>
            /// 导入
            /// </summary>
            Import = 10009,
            /// <summary>
            /// 导出
            /// </summary>
            Export = 10010,
        }
    }
}

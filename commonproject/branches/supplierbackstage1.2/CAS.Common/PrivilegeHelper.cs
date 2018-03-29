using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
{
    /// <summary>
    /// 权限帮助类
    /// </summary>
    public class PrivilegeHelper
    {
        //估价宝权限
        public struct GJBPrivilegeNo
        {
            /// <summary>
            /// 招商银行 机构端 登录
            /// </summary>
            public const string CMB_SOA_Login = "1003021";
            /// <summary>
            /// 设置分配人——新增
            /// </summary>
            public const string AssignUser_Add = "045A";
            /// <summary>
            /// 设置分配人——修改
            /// </summary>
            public const string AssignUser_Modify = "045M";
            /// <summary>
            /// 设置分配人——删除
            /// </summary>
            public const string AssignUser_Del = "045D";
            /// <summary>
            /// 设置分配人——查看
            /// </summary>
            public const string AssignUser_Look = "045";
            /// <summary>
            /// 技术团队设置——新增
            /// </summary>
            public const string BusinessType_Add = "043A";
            /// <summary>
            /// 技术团队设置——修改
            /// </summary>
            public const string BusinessType_Modify = "043M";
            /// <summary>
            /// 技术团队设置——删除
            /// </summary>
            public const string BusinessType_Delete = "043D";
            /// <summary>
            /// 技术团队——查看
            /// </summary>
            public const string BusinessType_Look = "043";
            /// <summary>
            /// 收费标准设置——新增
            /// </summary>
            public const string ChargeStandard_Add = "048A";
            /// <summary>
            /// 收费标准设置——修改
            /// </summary>
            public const string ChargeStandard_Modify = "048M";
            /// <summary>
            /// 收费标准设置——删除
            /// </summary>
            public const string ChargeStandard_Delete = "048D";
            /// <summary>
            /// 收费标准设置——查看
            /// </summary>
            public const string ChargeStandard_Look = "048";
            /// <summary>
            /// 收费类别设置——新增
            /// </summary>
            public const string ChargeTypeSetup_Add = "048E";
            /// <summary>
            /// 收费类别设置——修改
            /// </summary>
            public const string ChargeTypeSetup_Modify = "048F";
            /// <summary>
            /// 收费类别设置——删除
            /// </summary>
            public const string ChargeTypeSetup_Delete = "048G";
            /// <summary>
            /// 收费类别设置——查看
            /// </summary>
            public const string ChargeTypeSetup_Look = "048V";
            /// <summary>
            /// 城市区域设置——新增
            /// </summary>
            public const string CitySetting_Add = "042A";
            /// <summary>
            /// 城市区域设置——修改
            /// </summary>
            public const string CitySetting_Modify = "042M";
            /// <summary>
            /// 城市区域设置——删除
            /// </summary>
            public const string CitySetting_Delete = "042D";
            /// <summary>
            /// 签字估价师设置——新增
            /// </summary>
            public const string SignedAppraisers_Add = "046A";
            /// <summary>
            /// 签字估价师设置——修改
            /// </summary>
            public const string SignedAppraisers_Modify = "046M";
            /// <summary>
            /// 签字估价师——删除
            /// </summary>
            public const string SignedAppraisers_Del = "046D";
            /// <summary>
            /// 签字估价师设置——查看
            /// </summary>
            public const string SignedAppraisers_Look = "046";
            /// <summary>
            /// 估价师证书管理——查看
            /// </summary>
            public const string AppraisersCert_Look = "126";
            /// <summary>
            /// 估价师证书管理——管理
            /// </summary>
            public const string AppraisersCert_Manage = "126GL";
            /// <summary>
            /// 税费模板设置——新增
            /// </summary>
            public const string TaxTemplate_Add = "059A";
            /// <summary>
            /// 税费模板设置——修改
            /// </summary>
            public const string TaxTemplate_Modify = "059M";
            /// <summary>
            /// 税费模板设置——删除
            /// </summary>
            public const string TaxTemplate_Delete = "059D";
            /// <summary>
            /// 税费模板设置——复制
            /// </summary>
            public const string TaxTemplate_Copy = "059C";
            /// <summary>
            /// 税费模板设置——查看
            /// </summary>
            public const string TaxTemplate_Look = "059";
            /// <summary>
            /// 报告归档设置——新增
            /// </summary>
            public const string ReportBackupSetup_Add = "044A";
            /// <summary>
            /// 报告归档设置——修改
            /// </summary>
            public const string ReportBackupSetup_Modify = "044M";
            /// <summary>
            /// 报告归档设置——删除
            /// </summary>
            public const string ReportBackupSetup_Delete = "044D";
            /// <summary>
            /// 报告归档设置——查看
            /// </summary>
            public const string ReportBackupSetup_Look = "044";
            ///// <summary>
            ///// 收费类别设置——新增
            ///// </summary>
            //public const string ChargeTypeSetup_Add = "048A";
            ///// <summary>
            ///// 收费类别设置——修改
            ///// </summary>
            //public const string ChargeTypeSetup_Modify = "048M";
            ///// <summary>
            ///// 收费类别设置——删除
            ///// </summary>
            //public const string ChargeTypeSetup_Delete = "048D";
            ///// <summary>
            ///// 收费类别设置——查看
            ///// </summary>
            //public const string ChargeTypeSetup_Look = "048";
            /// <summary>
            /// 评估目的设置——新增
            /// </summary>
            public const string EvaluationPurposesSetup_Add = "049A";
            /// <summary>
            /// 评估目的设置——修改
            /// </summary>
            public const string EvaluationPurposesSetup_Modify = "049M";
            /// <summary>
            /// 评估目的设置——删除
            /// </summary>
            public const string EvaluationPurposesSetup_Delete = "049D";
            /// <summary>
            /// 评估目的设置——查看
            /// </summary>
            public const string EvaluationPurposesSetup_Look = "049";

            /// <summary>
            /// 报告子类型设置——新增
            /// </summary>
            public const string ReportSubType_Add = "236A";
            /// <summary>
            /// 报告子类型设置——修改
            /// </summary>
            public const string ReportSubType_Modify = "236M";
            /// <summary>
            /// 报告子类型设置——删除
            /// </summary>
            public const string ReportSubType_Delete = "236D";
            /// <summary>
            /// 报告子类型设置——查看
            /// </summary>
            public const string ReportSubType_Look = "236";

            /// <summary>
            /// 评估对象参数设置——新增
            /// </summary>
            public const string AssessmentObjectSetup_Add = "050A";
            /// <summary>
            /// 评估对象参数设置——修改
            /// </summary>
            public const string AssessmentObjectSetup_Modify = "050M";
            /// <summary>
            /// 评估对象参数设置——删除
            /// </summary>
            public const string AssessmentObjectSetup_Delete = "050D";
            /// <summary>
            /// 评估对象参数设置——查看
            /// </summary>
            public const string AssessmentObjectSetup_Look = "050";
            /// <summary>
            /// 通知公告——查看
            /// </summary>
            public const string GongGao_View = "004";
            /// <summary>
            /// 通告查看部门
            /// </summary>
            public const string GongGao_View_BuMen = "004SB";
            /// <summary>
            /// 通告查看全部
            /// </summary>
            public const string GongGao_View_All = "004SC";
            /// <summary>
            /// 通知公告——新增
            /// </summary>
            public const string GongGao_Add = "004A";
            /// <summary>
            /// 通知公告——修改
            /// </summary>
            public const string GongGao_Modify = "004M";
            /// <summary>
            /// 通知公告——删除
            /// </summary>
            public const string GongGao_Delete = "004D";
            /// <summary>
            /// 通告类别新增
            /// </summary>
            public const string GongGaoType_Add = "004E";
            /// <summary>
            /// 通告类别修改
            /// </summary>
            public const string GongGaoType_Edit = "004F";
            /// <summary>
            /// 通告类别删除
            /// </summary>
            public const string GongGaoType_Delete = "004G";
            /// <summary>
            /// 通告类别查看
            /// </summary>
            public const string GongGaoType_Look = "004V";
            
            /// <summary>
            /// 手机短信——发送外部短信
            /// </summary>
            public const string MobileSms_SendWai = "033M";
            /// <summary>
            /// 手机短息——发送内部短信
            /// </summary>
            public const string MobileSms_SendNei = "033N";
            /// <summary>
            /// 手机短信——查看全部短信
            /// </summary>
            public const string MobileSms_ViewAll = "033V";
            /// <summary>
            /// 手机短信——查看部门短信
            /// </summary>
            public const string MobileSms_ViewBuMen = "033SB";
            /// <summary>
            /// 手机短信——查看手机短信
            /// </summary>
            public const string MobileSms_View = "033";
            /// <summary>
            /// 手机短信发送——删除
            /// </summary>
            public const string MobileSms_Delete = "033D";
            /// <summary>
            /// 流程设计——查看
            /// </summary>
            public const string FlowDisign_Look = "080";
            /// <summary>
            /// 流程设计——节点设计
            /// </summary>
            public const string FlowDisign_NodeDesign = "080N";
            /// <summary>
            /// 流程设计——新增
            /// </summary>
            public const string FlowDisign_Add = "080A";
            /// <summary>
            /// 流程设计——修改
            /// </summary>
            public const string FlowDisign_Modify = "080M";
            /// <summary>
            /// 流程设计——删除
            /// </summary>
            public const string FlowDisign_Delete = "080D";
            /// <summary>
            /// 表单设计——查看
            /// </summary>
            public const string FormDisign_Look = "087";
            /// <summary>
            /// 表单设计——设计
            /// </summary>
            public const string FormDisign_Design = "087N";
            /// <summary>
            /// 表单设计——新增
            /// </summary>
            public const string FormDisign_Add = "087A";
            /// <summary>
            /// 表单设计——修改
            /// </summary>
            public const string FormDisign_Modify = "087M";
            /// <summary>
            /// 表单设计——删除
            /// </summary>
            public const string FormDisign_Delete = "087D";
            /// <summary>
            /// 工作监控——查看
            /// </summary>
            public const string HelpWorkFlow_Look = "018";
            /// <summary>
            /// 工作监控——办理
            /// </summary>
            public const string HelpWorkFlow_Do = "018M";
            /// <summary>
            /// 工作监控——删除
            /// </summary>
            public const string HelpWorkFlow_Delete = "018D";
            /// <summary>
            /// 新建工作——查看
            /// </summary>
            public const string NewWork_Look = "013";
            /// <summary>
            /// 新建工作——新增
            /// </summary>
            public const string NewWork_Add = "013A";

            /// <summary>
            /// 表单分类——查看
            /// </summary>
            public const string FormType_Look = "021";
            /// <summary>
            /// 表单分类——新增
            /// </summary>
            public const string FormType_Add = "021A";
            /// <summary>
            /// 表单分类——修改
            /// </summary>
            public const string FormType_Modify = "021M";
            /// <summary>
            /// 表单分类——删除
            /// </summary>
            public const string FormType_Delete = "021D";

            /// <summary>
            /// 行政审批已完成——查看
            /// </summary>
            public const string OkWorkFlow_Look = "019";
            /// <summary>
            /// 行政审批已完成——查看部门
            /// </summary>
            public const string OkWorkFlow_LookBuMen = "019SB";
            /// <summary>
            /// 行政审批已完成——查看全部
            /// </summary>
            public const string OkWorkFlow_LookAll = "019SC";
            /// <summary>
            /// 行政审批已完成——删除
            /// </summary>
            public const string OkWorkFlow_Delete = "019D";
            /// <summary>
            /// 工作查询——查看
            /// </summary>
            public const string SearchWorkFlow_Look = "017";
            /// <summary>
            /// 工作查询——查看部门
            /// </summary>
            public const string SearchWorkFlow_LookBuMen = "017SB";
            /// <summary>
            /// 工作查询——查看全部
            /// </summary>
            public const string SearchWorkFlow_LookAll = "017SC";
            /// <summary>
            /// 常用审批——新增
            /// </summary>
            public const string ShenPi_Add = "011A";
            /// <summary>
            /// 常用审批——修改
            /// </summary>
            public const string ShenPi_Modify = "011M";
            /// <summary>
            /// 常用审批——删除
            /// </summary>
            public const string ShenPi_Delete = "011D";
            /// <summary>
            /// 常用审批——查看
            /// </summary>
            public const string ShenPi_Look = "011";
            /// <summary>
            /// 红头文件模板——新增
            /// </summary>
            public const string RedHeadFile_Add = "032A";
            /// <summary>
            /// 红头文件模板——修改
            /// </summary>
            public const string RedHeadFile_Modify = "032M";
            /// <summary>
            /// 红头文件模板——删除
            /// </summary>
            public const string RedHeadFile_Delete = "032D";
            public const string RedHeadFile_View = "032";
            /// <summary>
            /// 系统日志——查看
            /// </summary>
            public const string SystemLog_Look = "031";
            /// <summary>
            /// 系统日志——删除
            /// </summary>
            public const string SystemLog_Delete = "031D";

            /// <summary>
            /// 发送传阅——查看
            /// </summary>
            public const string SendFile_View = "076";
            /// <summary>
            /// 发送传阅——新增
            /// </summary>
            public const string SendFile_Add = "076A";
            /// <summary>
            /// 发送传阅——删除
            /// </summary>
            public const string SendFile_Delete = "076D";

            /// <summary>
            /// 公共通讯簿——新增
            /// </summary>
            public const string TongXunLu_Add = "006A";
            /// <summary>
            /// 公共通讯簿——修改
            /// </summary>
            public const string TongXunLu_Modify = "006M";
            /// <summary>
            /// 公共通讯簿——删除
            /// </summary>
            public const string TongXunLu_Delete = "006D";
            /// <summary>
            /// 公司信息——查看
            /// </summary>
            public const string CompanyInfo_Look = "104";
            /// <summary>
            /// 公司信息——修改 
            /// </summary>
            public const string CompanyInfo_Modify = "104M";

            /// <summary>
            /// 分支机构——查看
            /// </summary>
            public const string BranchManage_Look = "055";
            /// <summary>
            /// 分支机构——添加分公司
            /// </summary>
            public const string BranchManage_AddSubCompany = "055A";
            /// <summary>
            /// 分支机构——修改分公司
            /// </summary>
            public const string BranchManage_EditSubCompany = "055M";
            /// <summary>
            /// 分支机构——删除分公司
            /// </summary>
            public const string BranchManage_DeleteSubCompany = "055D";
            /// <summary>
            /// 分支机构——添加部门
            /// </summary>
            public const string BranchManage_AddSubDepartment = "055S";
            /// <summary>
            /// 分支机构——修改部门
            /// </summary>
            public const string BranchManage_EditSubDepartment = "055N";
            /// <summary>
            /// 分支机构——删除部门
            /// </summary>
            public const string BranchManage_DeleteSubDepartment = "055F";

            /// <summary>
            /// 用户管理——查看
            /// </summary>
            public const string UserManage_Look = "058";
            /// <summary>
            /// 用户管理——新增
            /// </summary>
            public const string UserManage_Add = "058A";
            /// <summary>
            /// 用户管理——修改
            /// </summary>
            public const string UserManage_Modify = "058M";
            /// <summary>
            /// 用户管理——删除
            /// </summary>
            public const string UserManage_Delete = "058D";
            /// <summary>
            /// 业务错误登记——查看
            /// </summary>
            public const string BusinessErrorsMapping_Look = "105";
            /// <summary>
            /// 业务错误登记——添加
            /// </summary>
            public const string BusinessErrorsMapping_Add = "105A";
            ///// <summary>
            ///// 业务错误登记——修改
            ///// </summary>
            //public const string BusinessErrorsMapping_Delete = "105D";

            /// <summary>
            /// 业务错误管理——查看
            /// </summary>
            public const string BusinessErrors_Look = "073";
            /// <summary>
            /// 业务错误管理——添加
            /// </summary>
            public const string BusinessErrors_Add = "073A";
            /// <summary>
            /// 业务错误管理——修改
            /// </summary>
            public const string BusinessErrors_Modify = "073M";
            /// <summary>
            /// 业务错误管理——删除
            /// </summary>
            public const string BusinessErrors_Delete = "073D";
            /// <summary>
            /// 业务错误管理——类别添加
            /// </summary>
            public const string BusinessErrors_TypeAdd = "073S";
            /// <summary>
            /// 业务错误管理——类别修改
            /// </summary>
            public const string BusinessErrors_TypeModify = "073N";
            /// <summary>
            /// 业务错误管理——类别删除
            /// </summary>
            public const string BusinessErrors_TypeDelete = "073F";

            /// <summary>
            /// 个人通讯簿——新增
            /// </summary>
            public const string GRTongXunLu_Add = "008A";
            /// <summary>
            /// 个人通讯簿——修改
            /// </summary>
            public const string GRTongXunLu_Modify = "008M";
            /// <summary>
            /// 个人通讯簿——删除
            /// </summary>
            public const string GRTongXunLu_Delete = "008D";
            /// <summary>
            /// 收件箱——删除
            /// </summary>
            public const string LandEmailShou_Delete = "003D";

            /// <summary>
            /// 已发送——删除
            /// </summary>
            public const string LandEmailYiFa_Delete = "034D";

            /// <summary>
            /// 职务管理——新增
            /// </summary>
            public const string ZhiWuMange_Add = "061A";
            /// <summary>
            /// 职务管理——修改
            /// </summary>
            public const string ZhiWuMange_Modify = "061M";
            /// <summary>
            /// 职务管理——删除
            /// </summary>
            public const string ZhiWuManage_Delete = "061D";
            /// <summary>
            /// 职务管理——查看
            /// </summary>
            public const string ZhiWuManage_View = "061";
            /// <summary>
            /// 用户信息查询——查看
            /// </summary>
            public const string SystemUserSearch_View = "038";
            /// <summary>
            /// 组织机构信息查询
            /// </summary>
            public const string BuMenInfoSearch_View = "037";
            /// <summary>
            /// 报告查询——查看
            /// </summary>
            public const string ReportSearch_Look = "098";
            /// <summary>
            /// 报告查询——查看全部
            /// </summary>
            public const string ReportSearch_ZSeeCurrent = "098SC";
            /// <summary>
            /// 报告统计——查看
            /// </summary>
            public const string ReportTongJi_Look = "099";
            /// <summary>
            /// 报告统计——查看全部
            /// </summary>
            public const string ReportTongJi_SeeCurrent = "099SC";
            /// <summary>
            /// 报告归档——查看
            /// </summary>
            public const string BackFile_Look = "213";
            /// <summary>
            /// 报告归档——查看部门
            /// </summary>
            public const string BackFile_LookViewBuMen = "213SB";
            /// <summary>
            /// 报告归档——查看全部
            /// </summary>
            public const string BackFile_LookViewAll = "213SC";
            /// <summary>
            /// 报告归档——修改
            /// </summary>
            public const string BackFile_Edit = "213M";
            /// <summary>
            /// 报告收费——查看
            /// </summary>
            public const string ChargeList_Look = "211";
            /// <summary>
            /// 报告收费——修改
            /// </summary>
            public const string ChargeList_Modify = "211M";
            /// <summary>
            /// 报告收费——查看全部
            /// </summary>
            //public const string ChargeList_SeeCurrent = "211SC";
            /// <summary>
            /// 角色管理——查看
            /// </summary>
            public const string RoleManage_Look = "057";
            /// <summary>
            /// 角色管理——添加
            /// </summary>
            public const string RoleManage_Add = "057A";
            /// <summary>
            /// 角色管理——修改
            /// </summary>
            public const string RoleManage_Modify = "057M";
            /// <summary>
            /// 角色管理——删除
            /// </summary>
            public const string RoleManage_Delete = "057D";
            /// <summary>
            /// 客户公司——查看
            /// </summary>
            public const string CustomerManage_Look = "230";
            /// <summary>
            /// 客户公司——添加
            /// </summary>
            public const string CustomerManage_Add = "230A";
            /// <summary>
            /// 客户公司——修改
            /// </summary>
            public const string CustomerManage_Modify = "230M";
            /// <summary>
            /// 客户公司——删除
            /// </summary>
            public const string CustomerManage_Delete = "230D";

            /// <summary>
            /// 客户公司类别——新增
            /// </summary>
            public const string CustomerManageType_Add = "230S";
            /// <summary>
            /// 客户公司类别——修改
            /// </summary>
            public const string CustomerManageType_Modify = "230N";
            /// <summary>
            /// 客户公司类别——删除
            /// </summary>
            public const string CustomerManageType_Delete = "230F";

            /// <summary>
            /// 客户账号——查看
            /// </summary>
            public const string CustomerAccount_Look = "231";
            /// <summary>
            /// 客户账号——添加
            /// </summary>
            public const string CustomerAccount_Add = "231A";
            /// <summary>
            /// 客户账号——修改
            /// </summary>
            public const string CustomerAccount_Modify = "231M";
            /// <summary>
            /// 客户账号——删除
            /// </summary>
            public const string CustomerAccount_Delete = "231D";

            /// <summary>
            /// 对公待回价——查看
            /// </summary>
            public const string ComWaitQuery_View = "081";
            /// <summary>
            /// 对公待回价——修改
            /// </summary>
            public const string ComWaitQuery_Modify = "081M";
            /// <summary>
            /// 对公待回价——新增
            /// </summary>
            public const string ComWaitQuery_Add = "081A";
            /// <summary>
            /// 对公待回价——删除
            /// </summary>
            public const string ComWaitQuery_Delete = "081D";
            /// <summary>
            /// 对公待回价——回价
            /// </summary>
            public const string ComWaitQuery_RePrice = "081P";
            /// <summary>
            /// 对公待回价——查看全部
            /// </summary>
            public const string ComWaitQuery_SeeCurrent = "081SC";
            /// <summary>
            /// 对公已回价——查看
            /// </summary>
            public const string ComHasQuery_View = "082";
            /// <summary>
            /// 对公已回价——修改
            /// </summary>
            public const string ComHasQuery_Modify = "082M";
            /// <summary>
            /// 对公已回价——删除
            /// </summary>
            public const string ComHasQuery_Delete = "082D";
            /// <summary>
            /// 对公已回价——转业务
            /// </summary>
            public const string ComHasQuery_RquestEnstrust = "082R";
            /// <summary>
            /// 对公已回价——查看全部
            /// </summary>
            public const string ComHasQuery_SeeCurrent = "082SC";
            /// <summary>
            /// 对公询价待分配——查看
            /// </summary>
            public const string ComWaitAssignQuery_View = "135";
            /// <summary>
            /// 对公询价待分配——分配
            /// </summary>
            public const string ComWaitAssignQuery_Assign = "135F";
            /// <summary>
            /// 对公询价待分配——查看全部
            /// </summary>
            public const string ComWairAssignQuery_ViewAll = "135SC";


            /// <summary>
            /// 个人待回价——查看
            /// </summary>
            public const string WaitQuery_View = "078";
            /// <summary>
            /// 个人待回价——新增
            /// </summary>
            public const string WaitQuery_Add = "078A";
            /// <summary>
            /// 个人待回价——修改
            /// </summary>
            public const string waitQuery_Edit = "078M";
            /// <summary>
            /// 个人待回价——删除
            /// </summary>
            public const string WaitQuery_Delete = "078D";
            /// <summary>
            /// 个人待回价——回价
            /// </summary>
            public const string WaitQuery_RePrice = "078P";
            /// <summary>
            /// 个人待回价——查看全部
            /// </summary>
            public const string WaitQuery_SeeCurrent = "078SC";
            /// <summary>
            /// 个人已回价——查看
            /// </summary>
            public const string HasQuery_View = "079";
            /// <summary>
            /// 个人已回价——修改
            /// </summary>
            public const string HasQuery_Modify = "079M";
            /// <summary>
            /// 个人已回价——删除
            /// </summary>
            public const string HasQuery_Delete = "079D";
            /// <summary>
            ///个人已回价——转业务
            /// </summary>
            public const string HasQuery_RquestEnstrust = "079R";
            /// <summary>
            ///个人已回价——查看全部
            /// </summary>
            public const string HasQuery_SeeCurrent = "079SC";
            /// <summary>
            /// 个人询价待分配——查看
            /// </summary>
            public const string WaitAssignQuery_View = "134";
            /// <summary>
            /// 个人询价待分配——分配
            /// </summary>
            public const string WaitAssignQuery_Assign = "134F";
            /// <summary>
            /// 个人询价待分配——查看全部
            /// </summary>
            public const string WaitAssignQuery_ViewAll = "134SC";
            /// <summary>
            /// 报告监控——查看
            /// </summary>
            public const string ReportHelpWorkFlow_Look = "106";
            /// <summary>
            /// 报告监控——办理
            /// </summary>
            public const string ReportHelpWorkFlow_Approval = "106M";
            /// <summary>
            /// 已审批——查看
            /// </summary>
            public const string Approved_Look = "086";
            /// <summary>
            /// 已审批——查看全部
            /// </summary>
            public const string Approved_SeeCurrent = "086SC";
            /// <summary>
            /// 待审批——查看
            /// </summary>
            public const string PendingApproval_Look = "085";
            /// <summary>
            /// 待审批——办理
            /// </summary>
            public const string PendingApproval_Approval = "085M";
            /// <summary>
            /// 待审批——查看全部
            /// </summary>
            public const string PendingApproval_SeeCurrent = "085SC";
            /// <summary>
            /// 已作废报告查看
            /// </summary>
            public const string ReportInvalid_Look = "098";
            /// <summary>
            /// 已作废报告查看--查看全部
            /// </summary>
            public const string ReportInvalid_SeeCurrent = "098SC";
            /// <summary>
            /// 可统计业绩报告查看
            /// </summary>
            public const string PerformanceDependent_Look = "099";
            /// <summary>
            /// 可统计业绩报告查看--查看全部
            /// </summary>
            public const string PerformanceDependent_SeeCurrent = "099SC";
            /// <summary>
            /// 可上报业绩报告查看
            /// </summary>
            public const string ReportedResults_Look = "111";
            /// <summary>
            /// 可上报业绩报告查看——查看全部
            /// </summary>
            public const string ReportedResults_SeeCurrent = "111SC";

            /// <summary>
            /// 询价发起价格争议 -- 个人
            /// </summary>
            public const string QueryAdjust_Private = "079J";
            /// <summary>
            /// 询价发起价格争议 -- 对公
            /// </summary>
            public const string QueryAdjust_Public = "082J";

            /// <summary>
            /// 业务——分配
            /// </summary>
            public const string Entrust_Assign = "210AS";
            /// <summary>
            /// 业务——新增
            /// </summary>
            public const string Entrust_Add = "210A";
            /// <summary>
            /// 业务——删除
            /// </summary>
            public const string Entrust_Delete = "210D";
            /// <summary>
            /// 业务——查看
            /// </summary>
            public const string Entrust_View = "210";
            /// <summary>
            /// 业务——查看部门
            /// </summary>
            public const string Entrust_ViewBuMen = "210SB";
            /// <summary>
            /// 业务——查看全部
            /// </summary>
            public const string Entrust_ViewAll = "210SC";
            /// <summary>
            /// 业务——业务废除
            /// </summary>
            public const string Entrust_Cancel = "210F";

            /// <summary>
            /// 报告查询——查看
            /// </summary>
            public const string ReportSearch_View = "114";
            /// <summary>
            /// 报告查询——查看全部
            /// </summary>
            public const string ReportSearch_SeeCurrent = "114SC";
            /// <summary>
            /// 报告查询——查看部门
            /// </summary>
            public const string ReportSearch_SeeBuMen = "114SB";
            /// <summary>
            /// 报告查询——数据导出
            /// </summary>
            public const string ReportSearch_Export = "114E";

            /*业务表单设置*/
            /// <summary>
            /// 询价自定义表单——查看
            /// </summary>
            public const string CustomerBusinessQuery_View = "052";
            /// <summary>
            /// 询价自定义表单——新增
            /// </summary>
            public const string CustomerBusinessQuery_Add = "052A";
            /// <summary>
            /// 询价自定义表单——修改
            /// </summary>
            public const string CustomerBusinessQuery_Edit = "052M";
            /// <summary>
            /// 预评自定义表单——查看
            /// </summary>
            public const string CustomerBusinessYP_View = "053";
            /// <summary>
            /// 预评自定义表单——新增
            /// </summary>
            public const string CustomerBusinessYP_Add = "053A";
            /// <summary>
            /// 预评自定义表单——修改
            /// </summary>
            public const string CustomerBusinessYP_Edit = "053M";
            /// <summary>
            /// 报告自定义表单——查看
            /// </summary>
            public const string CustomerBusinessReport_View = "054";
            /// <summary>
            /// 报告自定义表单——新增
            /// </summary>
            public const string CustomerBusinessReport_Add = "054A";
            /// <summary>
            /// 报告自定义表单——修改
            /// </summary>
            public const string CustomerBusinessReport_Edit = "054M";
            /// <summary>
            /// 委托自定义表单——查看 
            /// </summary>
            public const string CustomerBusinessEntrust_View = "102";
            /// <summary>
            /// 委托自定义表单——新增
            /// </summary>
            public const string CustomerBusinessEntrust_Add = "102A";
            /// <summary>
            /// 委托自定义表单——修改
            /// </summary>
            public const string CustomerBusinessEntrust_Edit = "102M";
            /// <summary>
            /// 委估对象自定义表单——查看
            /// </summary>
            public const string CustomerBusinessObject_View = "112";
            /// <summary>
            /// 委估对象自定义表单——新增
            /// </summary>
            public const string CustomerBusinessObject_Add = "112A";
            /// <summary>
            /// 违规对象自定义表单——修改
            /// </summary>
            public const string CustomerBusinessObject_Edit = "112M";
            /// <summary>
            /// 报告待投递-修改
            /// </summary>
            public const string ReportNoDeliver_Edit = "129M";
            /// <summary>
            /// 报告已投递-修改
            /// </summary>
            public const string ReportHasDeliver_Edit = "130M";
            /// <summary>
            /// 报告待投递-查看
            /// </summary>
            public const string ReportNoDeliver_View = "129";
            /// <summary>
            /// 报告已投递-查看
            /// </summary>
            public const string ReportHasDeliver_View = "130";
            /// <summary>
            /// 报告待投递-查看全部
            /// </summary>
            public const string ReportNoDeliver_View_All = "129SC";
            /// <summary>
            /// 报告已投递-查看全部
            /// </summary>
            public const string ReportHasDeliver_View_All = "130SC";
            /// <summary>
            /// 系统全局设置-查看
            /// </summary>
            public const string SystemConfig_View = "131";
            /// <summary>
            /// 系统全局设置-修改
            /// </summary>
            public const string SystemConfig_Modify = "131M";

            /// <summary>
            /// 询价分配人设置——查看
            /// </summary>
            public const string QueryAssignUser_View = "133";
            /// <summary>
            /// 询价分配人设置——新增
            /// </summary>
            public const string QueryAssignUser_Add = "133A";
            /// <summary>
            /// 询价分配人设置——修改
            /// </summary>
            public const string QueryAssignUser_Edit = "133E";
            /// <summary>
            /// 询价分配人设置——删除
            /// </summary>
            public const string QueryAssignUser_Delete = "133D";
            /// <summary>
            /// 菜单设置——查看
            /// </summary>
            public const string TreeList_View = "132";
            /// <summary>
            /// 菜单设置-修改
            /// </summary>
            public const string TreeList_Edit = "132M";

            #region 重整菜单后权限
            /// <summary>
            /// 询价新增
            /// </summary>
            public const string QueryAdd = "200A";
            ///// <summary>
            ///// 询价修改
            ///// </summary>
            //public const string QueryModify = "200M";
            /// <summary>
            /// 询价删除
            /// </summary>
            public const string QueryDelete = "200D";
            /// <summary>
            /// 询价回价
            /// </summary>
            public const string QueryRePrice = "200P";
            /// <summary>
            /// 询价价格争议
            /// </summary>
            public const string QueryAdjust = "200J";
            /// <summary>
            /// 询价分配
            /// </summary>
            public const string QueryAssign = "200F";
            /// <summary>
            /// 转交他人权限
            /// </summary>
            public const string QueryZJ = "200Z";
            /// <summary>
            /// 询价全部——查看
            /// </summary>
            public const string QueryView = "202";
            /// <summary>
            /// 询价全部-查看全部
            /// </summary>
            public const string QueryViewAll = "202SC";
            /// <summary>
            /// 住宅询价查看
            /// </summary>
            public const string QueryHouse_View = "203";
            /// <summary>
            /// 住宅询价查看部门
            /// </summary>
            public const string QueryHouse_ViewBuMen = "203SB";
            /// <summary>
            /// 住宅询价查看全部
            /// </summary>
            public const string QueryHouse_ViewAll = "203SC";
            /// <summary>
            /// 办公询价查看
            /// </summary>
            public const string QueryOffice_View = "205";
            /// <summary>
            /// 办公询价查看部门
            /// </summary>
            public const string QueryOffice_ViewBuMen = "205SB";
            /// <summary>
            /// 办公询价查看全部
            /// </summary>
            public const string QueryOffice_ViewAll = "205SC";
            /// <summary>
            /// 商业询价查看
            /// </summary>
            public const string QueryBusiness_View = "204";
            /// <summary>
            /// 商业询价查看部门
            /// </summary>
            public const string QueryBusiness_ViewBuMen = "204SB";
            /// <summary>
            /// 商业询价查看全部
            /// </summary>
            public const string QueryBusiness_ViewAll = "204SC";
            /// <summary>
            /// 工业询价查看
            /// </summary>
            public const string QueryFactory_View = "206";
            /// <summary>
            /// 工业询价查看部门
            /// </summary>
            public const string QueryFactory_ViewBuMen = "206SB";
            /// <summary>
            /// 工业询价查看全部
            /// </summary>
            public const string QueryFactory_ViewAll = "206SC";
            /// <summary>
            /// 土地询价查看
            /// </summary>
            public const string LandQuery_View = "207";
            /// <summary>
            /// 土地询价查看部门
            /// </summary>
            public const string LandQuery_ViewBuMen = "207SB";
            /// <summary>
            /// 土地询价查看全部
            /// </summary>
            public const string LandQuery_ViewAll = "207SC";
            /// <summary>
            /// 资产询价查看
            /// </summary>
            public const string ZiChanQuery_View = "208";
            /// <summary>
            /// 资产询价查看部门
            /// </summary>
            public const string ZiChanQuery_ViewBuMen = "208SB";
            /// <summary>
            /// 资产询价查看男全部
            /// </summary>
            public const string ZiChanQuery_ViewAll = "208SC";
            /// <summary>
            /// 其它询价查看
            /// </summary>
            public const string OtherQuery_View = "209";
            /// <summary>
            /// 其它询价查看部门
            /// </summary>
            public const string OtherQuery_ViewBuMen = "209SB";
            /// <summary>
            /// 其它询价查看全部
            /// </summary>
            public const string OtherQuery_ViewAll = "209SC";

            /// <summary>
            /// 询价新增(多套)
            /// </summary>
            public const string MoreQueryAdd = "247A";
            /// <summary>
            /// 询价删除(多套)
            /// </summary>
            public const string MoreQueryDelete = "247D";
            /// <summary>
            /// 询价回价(多套)
            /// </summary>
            public const string MoreQueryRePrice = "247P";
            /// <summary>
            /// 询价价格争议(多套)
            /// </summary>
            public const string MoreQueryAdjust = "247J";
            /// <summary>
            /// 询价分配(多套)
            /// </summary>
            public const string MoreQueryAssign = "247F";
            /// <summary>
            /// 转交他人权限(多套)
            /// </summary>
            public const string MoreQueryZJ = "247Z";
            /// <summary>
            /// 询价全部——查看(多套)
            /// </summary>
            public const string MoreQueryView = "247";
            /// <summary>
            /// 询价全部-查看部门(多套)
            /// </summary>
            public const string MoreQueryViewBuMen = "247SB";
            /// <summary>
            /// 询价全部-查看全部(多套)
            /// </summary>
            public const string MoreQueryViewAll = "247SC";

            /// <summary>
            /// 自动估价记录——查看
            /// </summary>
            public const string QueryHistoryView = "242";
            /// <summary>
            /// 自动估价记录-查看部门
            /// </summary>
            public const string QueryHistoryViewBuMen = "242SB";
            /// <summary>
            /// 自动估价记录-查看全部
            /// </summary>
            public const string QueryHistoryViewAll = "242SC";

            public const string YPView = "214";
            /// <summary>
            /// 预评查看部门
            /// </summary>
            public const string YPViewBuMen = "214SB";
            /// <summary>
            /// 预评查看全部
            /// </summary>
            public const string YPViewAll = "214SC";
            /// <summary>
            /// 预评修改
            /// </summary>
            public const string ReportView = "212";
            public const string ReportViewBuMen = "212SB";
            public const string ReportViewAll = "212S";
            public const string ReportAdd = "212A";
            public const string ReportDelete = "212D";
            //转交他人的报告
            public const string ReportHandOver = "212Z";

            public const string ReportNoView = "238";
            public const string ReportNoDelete = "238D";
            public const string ReportNoViewAll = "212SC";
            public const string ReportNoCloseNo = "212C";
            ///// <summary>
            ///// 预评转报告
            ///// </summary>
            //public const string YPToReport= "214AY";
            /// <summary>
            /// 报告价格争议
            /// </summary>
            public const string ReportAdjust = "212J";
            /// <summary>
            /// 预评删除
            /// </summary>
            public const string YPDelete = "214D";
            /// <summary>
            ///预评新增
            /// </summary>
            public const string YPAdd = "214A";
            /// <summary>
            /// 转交他人的预评
            /// </summary>
            public const string YPHandOver = "214Z";
            /// <summary>
            /// 委托书查看
            /// </summary>
            public const string AuthorisationLetter_Look = "233";
            /// <summary>
            /// 委托书新增
            /// </summary>
            public const string AuthorisationLetter_Add = "233A";
            /// <summary>
            /// 委托书修改
            /// </summary>
            public const string AuthorisationLetter_Modify = "233M";
            /// <summary>
            /// 委托书删除
            /// </summary>
            public const string AuthorisationLetter_Delete = "233D";
            /// <summary>
            /// 承诺函查看
            /// </summary>
            public const string EntrustLetter_Look = "234";
            /// <summary>
            /// 承诺函添加
            /// </summary>
            public const string EntrustLetter_Add = "234A";
            /// <summary>
            /// 承诺函修改
            /// </summary>
            public const string EntrustLetter_Modify = "234M";
            /// <summary>
            /// 承诺函删除
            /// </summary>
            public const string EntrustLetter_Delete = "234D";

            public const string AppraisalStage_View = "241";
            public const string AppraisalStage_Add = "241A";
            public const string AppraisalStage_Modify = "241M";
            public const string AppraisalStage_Delete = "241D";
            #endregion

            #region 云查勘使用权限
            /// <summary>
            /// 查勘任务——查看
            /// </summary>
            public const string SurveyTaskList_View = "9999";
            /// <summary>
            /// 查勘任务——分配
            /// </summary>
            public const string Survey_Assign = "9999AS";

            //查勘列表权限 使用OA中查勘任务查勘权限（SurveyTaskList_View）
            //分配/重新分配查勘 使用OA中分配查勘权限（Survey_Assign）        
            /// <summary>
            /// 查勘列表——查看部门
            /// </summary>
            public const string SurveyTaskList_View_BuMen = "9999SB";
            /// <summary>
            /// 查勘列表——查看全部
            /// </summary>
            public const string SurveyTaskList_View_All = "9999SC";
            /// <summary>
            /// 新增查勘
            /// </summary>
            public const string Survey_Add = "9999A";
            /// <summary>
            /// 修改查勘
            /// </summary>
            public const string Survey_Modify = "9999M";
            /// <summary>
            /// 撤销查勘
            /// </summary>
            public const string Survey_Cancel = "9999C";
            /// <summary>
            /// 查勘
            /// </summary>
            public const string Survey_Survey = "9999S";
            /// <summary>
            /// 修改查勘照片
            /// </summary>
            public const string Survey_Modify_Photo = "9999MP";
            /// <summary>
            /// 修改查勘地图标注
            /// </summary>
            public const string Survey_Modify_Map = "9999MM";
            /// <summary>
            /// 删除查勘信息
            /// </summary>
            public const string Survey_Delete = "9999D";

            /// <summary>
            /// 云查勘设置——查看
            /// </summary>
            public const string Survey_Setting_View = "120";
            #endregion

            #region 报告生成权限
            /// <summary>
            /// 查看
            /// </summary>
            public const string Report_Templete_View = "108";
            /// <summary>
            /// 新增
            /// </summary>
            public const string Report_Templete_Add = "108A";
            /// <summary>
            /// 修改
            /// </summary>
            public const string Report_Templete_Edit = "108M";
            /// <summary>
            /// 删除
            /// </summary>
            public const string Report_Templete_Delete = "108D";
            #endregion

            #region 价格纠错幅度权限
            /// <summary>
            /// 查看
            /// </summary>
            public const string AdjustAmount_View = "125";
            /// <summary>
            /// 修改
            /// </summary>
            public const string AdjustAmount_Modify = "125M";
            #endregion

            #region 收费模块权限

            /// <summary>
            /// 选择/修改收费标准
            /// </summary>
            public const string Charge_SelectOrEditStandard = "211SS/E";
            /// <summary>
            /// 添加收支记录
            /// </summary>
            public const string Charge_AddInOrOut = "211AI/O";
            /// <summary>
            /// 添加开票记录
            /// </summary>
            public const string Charge_AddBill = "211AB";
            /// <summary>
            /// 添加退费
            /// </summary>
            public const string Charge_AddReund = "211AR";
            /// <summary>
            /// 添加报告结单
            /// </summary>
            public const string Charge_AddOver = "211AO";
            /// <summary>
            /// 导出到Excel
            /// </summary>
            public const string Charge_ExportDetail = "211ED";

            /// <summary>
            /// 报告收费标准设置（自定义）——查看
            /// </summary>
            public const string ChargeStandardSetting_View = "048";
            /// <summary>
            /// 报告收费标准设置（自定义）——新增
            /// </summary>
            public const string ChargeStandardSetting_Add = "048A";
            /// <summary>
            /// 报告收费标准设置（自定义）——修改
            /// </summary>
            public const string ChargeStandardSetting_Modify = "048M";
            /// <summary>
            /// 报告收费标准设置（自定义）——删除
            /// </summary>
            public const string ChargeStandardSetting_Delete = "048D";

            /// <summary>
            ///普通查看自己
            /// </summary>
            public const string Charge_ViewSelf = "239";
            /// <summary>
            ///普通查看部门
            /// </summary>
            public const string Charge_ViewDepartment = "239VD";
            /// <summary>
            /// 普通查看全部
            /// </summary>
            public const string Charge_ViewAll = "239VA";

            /// <summary>
            /// 月结结算
            /// </summary>
            public const string Charge_MonthlyAccount = "240AA";
            /// <summary>
            /// 月结查看自己
            /// </summary>
            public const string Charge_MonthlyViewSelf = "240";
            /// <summary>
            /// 月结查看部门
            /// </summary>
            public const string Charge_MonthlyViewDepartment = "240VD";
            /// <summary>
            /// 月结查看全部
            /// </summary>
            public const string Charge_MonthlyViewAll = "240VA";
            #endregion

            #region 税费模板
            public const string TaxTemplateExpression_Delete = "248D";
            public const string TaxTemplateExpression_View = "248";
            public const string TaxTemplateExpression_Modify = "248M";
            public const string TaxTemplateExpression_Copy = "248C";
            public const string TaxTemplateExpression_Add = "248A";
            #endregion

            #region 账号禁用设置
            /// <summary>
            /// 账号禁用查看
            /// </summary>
            public const string AccountDisableRule_View = "249";
            #endregion

            #region  高级查询
            public const string Search_View = "9998";
            #endregion
        }


        /// <summary>
        /// 权限编号
        /// </summary>
        public enum PrivilegeNo
        {
            UserIsLogin = 1, // 用户登录即有 kevin
            SuperAdmin = 99,//超级管理员，管理所有城市，只能登录管理工作台 kevin

            UserTableColumnAdd = 200101,
            UserTableColumnEdit = 200102,
            UserTableColumnDel = 200103,
            /// <summary>
            /// 所有物理删除权限
            /// </summary>
            DeleteOnPhysically = 100000,

            #region 用户申报审核管理
            /// <summary>
            /// 申报审核管理
            /// </summary>
            ManageDeclareUser = 212000,
            /// <summary>
            /// 审核申报用户
            /// </summary>
            CheckDeclareUser = 212001,
            /// <summary>
            /// 暂停/激活申报用户
            /// </summary>
            SuspendedDeclareUser = 212002,
            /// <summary>
            /// 删除申报用户
            /// </summary>
            DeleteDeclareUser = 212003,
            #endregion

            #region 机构管理
            /// <summary>
            /// 机构管理
            /// </summary>
            ManageCompany = 210000,
            /// <summary>
            /// 新增
            /// </summary>
            AddCompany = 210001,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyCompany = 210002,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteCompany = 210003,
            /// <summary>
            /// 审核
            /// </summary>
            CheckCompany = 210004,
            /// <summary>
            /// 暂停/激活
            /// </summary>
            SuspendedCompany = 210005,
            /// <summary>
            /// 入围机构管理
            /// </summary>
            AtInstitutions = 210400,
            /// <summary>
            /// 新增入围机构
            /// </summary>
            AddInstiturions = 210401,
            /// <summary>
            /// 修改入围机构
            /// </summary>
            UpdateInstiturions = 210402,
            /// <summary>
            /// 删除入围机构 
            /// </summary>
            DeleteInstiturions = 210403,
            #endregion

            #region 楼盘管理
            /// <summary>
            /// 楼盘管理
            /// </summary>
            ManageProjectMain = 107000,

            #region 楼盘字典
            /// <summary>
            /// 楼盘字典管理
            /// </summary>
            ManageProject = 107300,
            /// <summary>
            /// 新增
            /// </summary>
            AddProject = 107301,
            /// <summary>
            /// 修改
            /// </summary>
            EditProject = 107302,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteProject = 107303,
            /// <summary>
            /// 导出
            /// </summary>
            ExportProject = 107304,
            /// <summary>
            /// 修改价格系数
            /// </summary>
            EditWeight = 107305,
            /// <summary>
            /// 重新初始化房号
            /// </summary>
            ReInitHouse = 107306,
            /// <summary>
            /// 删除待建楼盘
            /// </summary>
            DeleteWaitProject = 107307,
            #endregion

            #region 案例
            /// <summary>
            /// 案例管理
            /// </summary>
            ManageCase = 107200,
            /// <summary>
            /// 评估案例
            /// </summary>
            AssessCase = 107242,
            /// <summary>
            /// 市场案例
            /// </summary>
            MarketCase = 107241,
            /// <summary>
            /// 查看案例
            /// </summary>
            ShowCase = 107240,

            #region 住宅
            /// <summary>
            /// 案例管理
            /// </summary>
            ManageCaseHouse = 107210,
            /// <summary>
            /// 楼盘匹配
            /// </summary>
            ProjectMatch = 107211,
            /// <summary>
            /// 案例系数设置
            /// </summary>
            CaseSetting = 107212,
            /// <summary>
            /// 新增
            /// </summary>
            AddCaseHouse = 107213,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyCaseHouse = 107214,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteCaseHouse = 107215,
            #endregion

            #region 工商
            /// <summary>
            /// 案例管理
            /// </summary>
            ManageCaseBusiness = 107220,
            /// <summary>
            /// 新增
            /// </summary>
            AddCaseBusiness = 107221,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyCaseBusiness = 107222,
            /// <summary>
            /// 删除
            /// </summary>   
            /// 
            DeleteCaseBusiness = 107223,
            #endregion

            #region 土地
            /// <summary>
            /// 案例管理
            /// </summary>
            ManageCaseLand = 107230,
            /// <summary>
            /// 新增
            /// </summary>
            AddCaseLand = 107231,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyCaseLand = 107232,
            /// <summary>
            /// 删除
            /// </summary>  
            DeleteCaseLand = 107233,
            #endregion
            #endregion

            #region 走势
            PriceTrends = 107400,
            #endregion

            #region 城市均价
            /// <summary>
            /// 城市均价管理
            /// </summary>
            ManageAvgPriceMonth = 107500,
            /// <summary>
            /// 新增
            /// </summary>
            AddAvgPriceMonth = 107510,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyAvgPriceMonth = 107520,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteAvgPriceMonth = 107530,
            #endregion

            #region 相关公司
            /// <summary>
            /// 相关公司管理
            /// </summary>
            ManageAboutCompany = 107100,
            /// <summary>
            /// 新增
            /// </summary>
            AddAboutCompany = 107101,
            /// <summary>
            /// 修改
            /// </summary>
            ModifyAboutCompany = 107102,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteAboutCompany = 107103,
            #endregion
            #endregion

            #region 内容管理
            /// <summary>
            /// 内容管理
            /// </summary>
            ManageContent = 106000,
            #region 推荐管理
            /// <summary>
            /// 推荐管理
            /// </summary>
            ManageRecommend = 106010,
            /// <summary>
            /// 新增
            /// </summary>
            AddRecommend = 106011,
            /// <summary>
            /// 审核
            /// </summary>
            CheckRecommend = 106012,
            #endregion


            #region 资讯类型
            /// <summary>
            /// 资讯类型
            /// </summary>
            ManageAritcleType = 106101,
            /// <summary>
            /// 资讯类型添加
            /// </summary>
            ManageAritcleTypeAdd = 106102,
            /// <summary>
            /// 资讯类型修改
            /// </summary>
            ManageAritcleTypeUpdate = 106103,
            /// <summary>
            /// 资讯类型删除
            /// </summary>
            ManageAritcleTypeDelete = 106104,
            /// <summary>
            /// 资讯子类型管理
            /// </summary>
            ManageAritcleTypeChild = 106109,

            #endregion

            #region 资讯
            /// <summary>
            /// 资讯
            /// </summary>
            ManageAritcle = 106701,
            /// <summary>
            /// 资讯添加
            /// </summary>
            ManageAritcleAdd = 106702,
            /// <summary>
            /// 资讯修改
            /// </summary>
            ManageAritcleUpdate = 106703,
            /// <summary>
            /// 资讯删除
            /// </summary>
            ManageAritcleDelete = 106704,

            #endregion

            #region 单页内容
            /// <summary>
            /// 单页内容
            /// </summary>
            SinglePageAritcle = 106201,
            /// <summary>
            /// 单页内容添加
            /// </summary>
            SinglePageAritcleAdd = 106202,
            /// <summary>
            /// 单页内容修改
            /// </summary>
            SinglePageAritcleUpdate = 106203,
            /// <summary>
            /// 单页内容删除
            /// </summary>
            SinglePageAritcleDelete = 106204,

            #endregion


            #region 友情链接
            /// <summary>
            /// 友情链接
            /// </summary>
            LinkFriend = 106301,
            /// <summary>
            /// 友情链接添加
            /// </summary>
            LinkFriendAdd = 106302,
            /// <summary>
            /// 友情链接修改
            /// </summary>
            LinkFriendUpdate = 106303,
            /// <summary>
            /// 友情链接删除
            /// </summary>
            LinkFriendDelete = 106304,

            #endregion



            #region 友情连接类型
            /// <summary>
            /// 友情链接类型
            /// </summary>
            LinkFriendType = 106401,
            /// <summary>
            /// 友情链接类型添加
            /// </summary>
            LinkFriendTypeAdd = 106402,
            /// <summary>
            /// 友情链接类型修改
            /// </summary>
            LinkFriendTypeUpdate = 106403,
            /// <summary>
            /// 友情链接类型删除
            /// </summary>
            LinkFriendTypeDelete = 106404,

            #endregion



            #region 客户咨询审核
            /// <summary>
            /// 客户咨询审核
            /// </summary>
            CustomerAdReview = 106501,
            /// <summary>
            /// 客户咨询审核添加
            /// </summary>
            CustomerAdReviewAdd = 106502,
            /// <summary>
            /// 客户咨询审核
            /// </summary>
            CustomerAdReviewSh = 106503,
            /// <summary>
            /// 客户咨询审核删除
            /// </summary>
            CustomerAdReviewDelete = 106504,

            #endregion


            #region 广告管理
            /// <summary>
            /// 广告管理
            /// </summary>
            ManageAd = 106601,
            /// <summary>
            /// 广告管理添加
            /// </summary>
            ManageAdAdd = 106602,
            /// <summary>
            /// 广告管理修改
            /// </summary>
            ManageAdUpdate = 106603,
            /// <summary>
            /// 广告管理删除
            /// </summary>
            ManageAdDelete = 106604,

            #endregion


            #endregion

            #region 系统设置
            /// <summary>
            /// 系统设置
            /// </summary>
            ManageSetting = 104000,
            #region 自定义流程管理
            CustomFlowManage = 318000,
            #endregion

            #region 组织机构
            ManageStructure = 104400,
            /// <summary>
            /// 机构/部门管理
            /// </summary>
            ManageDeprtment = 104420,
            #endregion

            #region  用户管理
            /// <summary>
            /// 用户管理
            /// </summary>
            ManageUser = 104200,
            /// <summary>
            /// 新增
            /// </summary>
            AddUser = 104201,
            /// <summary>
            /// 给员工设置超级管理员用户组角色
            /// </summary>
            SetSuperAminiGroup = 104202,
            /// <summary>
            /// 管理其他公司的员工
            /// </summary>
            ManageOtherCompanyUser = 104204,
            /// <summary>
            /// 修改员工信息
            /// </summary>
            EditUser = 104205,
            /// <summary>
            /// 申报用户
            /// </summary>
            //ApplyUser = 104206,
            /// <summary>
            /// 员工调动
            /// </summary>
            TransferUser = 104207,
            /// <summary>
            /// 暂停激活
            /// </summary>
            SuspendActiveUser = 104208,
            /// <summary>
            /// 离职/在职
            /// </summary>
            ResignUser = 104209,
            /// <summary>
            /// 删除
            /// </summary>
            DeleteUser = 104210,
            /// <summary>
            /// 设置用户产品
            /// </summary>
            SetUserProduct = 104211,
            #endregion

            #region 用户组及权限
            /// <summary>
            /// 用户组及权限管理
            /// </summary>
            ManageGroupAndUser = 104100,
            /// <summary>
            /// 管理用户组
            /// </summary>
            ManageUserGroup = 104110,
            /// <summary>
            /// 设置用户组的权限
            /// </summary>
            SetGroupRight = 104111,
            /// <summary>
            /// 编辑用户组
            /// </summary>
            EditGroup = 104112,
            /// <summary>
            /// 编辑权限
            /// </summary>
            MamageRight = 104130,
            /// <summary>
            /// 管理系统预设用户组
            /// </summary>
            ManageSysGroup = 104120,
            /// <summary>
            /// 设置系统用户组权限
            /// </summary>
            SetSysGroupRight = 104121,
            /// <summary>
            /// 编辑系统用户组
            /// </summary>
            EditSysGroup = 104122,
            /// <summary>
            /// 编辑权限
            /// </summary>
            EditRight = 104131,
            #endregion

            #region 职务管理
            ManagePosition = 104430,
            #endregion
            /// <summary>
            /// 审批条件设置
            /// </summary>
            CheckSettings = 104010,
            /// <summary>
            /// 询价接口设置
            /// </summary>                
            QueryInterface = 104001,
            /// <summary>
            /// 咨询专家管理
            /// </summary>
            ManageExpert = 104002,
            /// <summary>
            /// 公司资料
            /// </summary>
            CompanyInfo = 104003,
            /// <summary>
            /// 公司资料
            /// </summary>
            CompanyAutoPriceInCity = 104004,
            #endregion

            #region 协会-执业管理
            /// <summary>
            /// 执业管理菜单
            /// </summary>
            ManagePractice = 311000,



            /// <summary>
            /// 执业管理
            /// </summary>
            ManagePracticeChild = 311001,
            /// <summary>
            /// 执业人员
            /// </summary>
            PersonnelPractice = 313002,
            /// <summary>
            /// 暂停执业人员
            /// </summary>
            PersonnelPracticePause = 313010,
            /// <summary>
            /// 启用执业人员
            /// </summary>
            PersonnelPracticeStart = 313011,
            /// <summary>
            /// 暂停执业机构
            /// </summary>
            CompanylPracticePause = 313012,
            /// <summary>
            /// 启用执业机构
            /// </summary>
            CompanylPracticeStart = 313013,



            /// <summary>
            /// 执业机构
            /// </summary>
            CompanyPractice = 313001,


            /// <summary>
            /// 执业记录
            /// </summary>
            PracticeRecord = 311005,
            /// <summary>
            /// 人员执业记录
            /// </summary>
            PersonnelPracticeRecord = 313004,
            /// <summary>
            /// 添加人员执业记录
            /// </summary>
            PersonnelPracticeRecordAdd = 313005,
            /// <summary>
            /// 修改人员执业记录
            /// </summary>
            PersonnelPracticeRecordUpdate = 313006,
            /// <summary>
            /// 删除人员执业记录
            /// </summary>
            PersonnelPracticeRecordDelete = 313007,



            /// <summary>
            /// 机构执业记录
            /// </summary>
            CompanyPracticeRecord = 313003,
            /// <summary>
            /// 添加机构执业记录
            /// </summary>
            CompanyPracticeRecordAdd = 313110,
            /// <summary>
            /// 修改机构执业记录
            /// </summary>
            CompanyPracticeRecordUpdate = 313111,
            /// <summary>
            /// 删除机构执业记录
            /// </summary>
            CompanyPracticeRecordDelete = 313112,



            #endregion

            #region 协会-机构排名

            /// <summary>
            /// 机构排名菜单
            /// </summary>
            InstitutionsRanking = 312000,

            /// <summary>
            /// 业绩排名
            /// </summary>
            RankingPerformance = 312101,
            /// <summary>
            ///按评估面积
            /// </summary>
            RankingPerformanceArea = 312103,
            /// <summary>
            ///按评估总额
            /// </summary>
            RankingPerformanceMoney = 312102,


            /// <summary>
            /// 评价排名
            /// </summary>
            EvaluationOfRank = 312201,

            #endregion

            #region 协会-报告管理
            /// <summary>
            /// 报告管理菜单
            /// </summary>
            ManageReport = 313000,

            /// <summary>
            /// 管理菜单
            /// </summary>
            ManageReportChild = 313100,
            #endregion

            #region 协会-奖惩管理
            /// <summary>
            /// 奖惩管理菜单
            /// </summary>
            ManageRewards = 314000,
            /// <summary>
            /// 个人奖惩
            /// </summary>
            PersonRewards = 314200,
            /// <summary>
            /// 添加个人奖惩
            /// </summary>
            PersonRewardsAdd = 314201,
            /// <summary>
            /// 修改个人奖惩
            /// </summary>
            PersonRewardsUpdate = 314202,
            /// <summary>
            /// 删除个人奖惩
            /// </summary>
            PersonRewardsDelete = 314203,


            /// <summary>
            /// 奖惩机构
            /// </summary>
            CompanyRewards = 314100,
            /// <summary>
            /// 奖惩机构 添加
            /// </summary>
            CompanyRewardsAdd = 314101,
            /// <summary>
            /// 奖惩机构 修改
            /// </summary>
            CompanyRewardsUpdate = 314102,
            /// <summary>
            /// 奖惩机构 删除
            /// </summary>
            CompanyRewardsDelete = 314103,




            #endregion

            #region 协会-推荐管理
            /// <summary>
            /// 推荐管理菜单
            /// </summary>
            XhManageRecommend = 315000,
            /// <summary>
            /// 推荐个人
            /// </summary>
            XhPesronRecommend = 315200,
            /// <summary>
            /// 推荐个人--操作推荐
            /// </summary>
            XhPesronRecommendOper = 315201,


            /// <summary>
            /// 推荐机构
            /// </summary>
            XhCompanyRecommend = 315100,
            /// <summary>
            /// 推荐机构--操作推荐
            /// </summary>
            XhCompanyRecommendOper = 315202,


            #endregion

            #region 消息
            /// <summary>
            /// 消息菜单
            /// </summary>
            ManageMessage = 105000,


            /// <summary>
            /// 消息 通知
            /// </summary>
            NoticeMessage = 105001,


            /// <summary>
            /// 消息 通知-收件箱
            /// </summary>
            NoticeMessageReceive = 105003,
            /// <summary>
            /// 消息 通知-发件箱
            /// </summary>
            NoticeMessageSend = 105004,


            /// <summary>
            /// 消息 站内信
            /// </summary>
            InternalMessage = 105002,


            /// <summary>0112
            /// 消息 站内信-收件箱
            /// </summary>
            InternalMessageReceive = 105006,
            /// <summary>
            /// 消息 站内信-发件箱
            /// </summary>
            InternalMessageSend = 105007,


            /// <summary>
            /// 消息 专家咨询
            /// </summary>
            Consultation = 105009,

            /// <summary>
            /// 业务消息
            /// </summary>
            BusinessMes = 105010,

            /// <summary>
            /// 系统消息
            /// </summary>
            SysMes = 105011,

            #endregion

            #region 询价/审批管理

            /// <summary>
            /// 询价管理菜单1111
            /// </summary>
            ManageQuery = 101000,

            /// <summary>
            /// 受理
            /// </summary>
            AcceptQuery = 101001,
            /// <summary>
            /// 编辑询价
            /// </summary>
            EditQuery = 101002,
            /// <summary>
            /// 回价
            /// </summary>
            BidQuery = 101003,
            /// <summary>
            /// 分配查勘
            /// </summary>
            AssignSurvey = 101004,
            /// <summary>
            /// 取消受理
            /// </summary>
            CancelAccept = 101005,
            /// <summary>
            /// 调价
            /// </summary>
            AdjustQuery = 101006,
            /// <summary>
            /// 删除询价记录或估价记录
            /// </summary>
            DeleteQuery = 101007,
            /// <summary>
            /// 发起人工询价
            /// </summary>
            CanQuery = 101010,
            /// <summary>
            /// 父级可以查看子级询价记录
            /// </summary>
            FQueryList = 101022,
            /// <summary>
            /// 查看自身询价记录
            /// </summary>
            QueryList = 101021,
            /// <summary>
            /// 查看询价记录
            /// </summary>
            ShowQueryList = 101020,

            #region 审批管理
            /// <summary>
            /// 审批管理
            /// </summary>
            ManageCheck = 101070,
            /// <summary>
            /// 管理回价审批
            /// </summary>
            ManageBidQueryCheck = 101040,
            /// <summary>
            /// 回价一审
            /// </summary>
            BidQueryCheck1 = 101041,
            /// <summary>
            /// 回价二审
            /// </summary>
            BidQueryCheck2 = 101042,
            /// <summary>
            /// 回价三审
            /// </summary>
            BidQueryCheck3 = 101043,
            /// <summary>
            /// 管理调价审批
            /// </summary>
            ManageAdjustQueryCheck = 101050,
            /// <summary>
            /// 调价一审
            /// </summary>
            AdjustQueryCheck1 = 101051,
            /// <summary>
            /// 调价二审
            /// </summary>
            AdjustQueryCheck2 = 101052,
            /// <summary>
            /// 调价三审
            /// </summary>
            AdjustQueryCheck3 = 101053,
            #endregion
            #endregion

            #region 委托管理
            /// <summary>
            /// 查看自身委托记录
            /// </summary>
            EntrustList = 102011,
            /// <summary>
            /// 查看下级与自身委托记录
            /// </summary>
            FEntrustList = 102012,
            /// <summary>
            /// 发起委托
            /// </summary>
            EntrustStart = 102003,
            /// <summary>
            /// 直接委托
            /// </summary>
            DirtEntrust = 102004,
            /// <summary>
            /// 询价委托
            /// </summary>
            QueryEntrust = 102005,
            /// <summary>
            /// 委托列表
            /// </summary>
            ShowEntrust = 102010,
            /// <summary>
            /// 受理委托
            /// </summary>
            AcceptEntrust = 102001,
            /// <summary>
            /// 设置委托规则
            /// </summary>
            SetEntrustRelues = 102007,
            /// <summary>
            /// 委托管理 
            /// </summary>
            EntrustManage = 102000,
            #endregion

            #region 估价管理
            ManageQProject = 111100,
            /// <summary>
            /// 估价管理
            /// </summary>
            ManageEvalue = 111000,
            /// <summary>
            /// 编辑项目
            /// </summary>
            EditQProject = 111101,
            /// <summary>
            /// 新增项目
            /// </summary>
            AddQProject = 111501,
            /// <summary>
            /// 查看项目
            /// </summary>
            LookQProject = 111502,
            /// <summary>
            /// 撤销项目
            /// </summary>
            CancelQProject = 111102,
            /// <summary>
            /// 编辑报告
            /// </summary>
            EditReport = 111201,
            /// <summary>
            /// 撤销报告
            /// </summary>
            CancelReport = 111202,
            /// <summary>
            /// 报告备案
            /// </summary>
            SaveUpReport = 111203,
            /// <summary>
            /// 发送报告
            /// </summary>
            SendReport = 111204,
            /// <summary>
            /// 删除报告
            /// </summary>
            DeleteReport = 111205,
            /// <summary>
            /// 报告退件
            /// </summary>
            RejectReport = 111207,
            /// <summary>
            /// 报告下载
            /// </summary>
            ReportDown = 111208,
            /// <summary>
            /// 报告管理
            /// </summary>
            ReportManage = 111200,
            /// <summary>
            /// 附件管理
            /// </summary>
            AttachmentManage = 111210,
            /// <summary>
            /// 查看报告记录
            /// </summary>
            ReportListManage = 111220,
            /// <summary>
            /// 查看下属与自身收到报告
            /// </summary>
            ReportListShow = 111221,
            /// <summary>
            /// 查看本用户收到的报告
            /// </summary>
            FReportListShow = 111222,
            /// <summary>
            /// 查看报告
            /// </summary>
            LookReportManage = 111223,
            #region 估价对象
            /// <summary>
            /// 估价对象管理
            /// </summary>
            ManageReportQuery = 111300,
            /// <summary>
            /// 编辑估价对象
            /// </summary>
            EditReportQuery = 111301,
            /// <summary>
            /// 删除估价对象
            /// </summary>
            DeleteReportQuery = 111302,
            /// <summary>
            /// 估价对象产权信息
            /// </summary>
            QueryPropertyManage = 111310,
            /// <summary>
            /// 管理估价对象附件
            /// </summary>
            ManageReportQueryFile = 111320,
            #endregion

            #region 报告附件
            /// <summary>
            /// 报告估价师签章
            /// </summary>
            ReportAssessorSignature = 111211,
            /// <summary>
            /// 报告公司签章
            /// </summary>
            ReportCompanySignature = 111213,
            /// <summary>
            /// 报告法人签章
            /// </summary>
            ReportLegalSignature = 111214,
            /// <summary>
            /// 编辑报告附件(新增，删除)
            /// </summary>
            EditReportFile = 111212,
            #endregion
            #endregion

            #region 资料管理
            /// <summary>
            /// 显示公司的信息
            /// </summary>
            ShowCompanyInfo = 317001,
            /// <summary>
            /// 显示部门、分支机构
            /// </summary>
            ShowDepartmentInfo = 317002,
            /// <summary>
            /// 公司、部门资料管理
            /// </summary>
            OrganizationInfo = 317003,
            /// <summary>
            /// 修改产部门信息
            /// </summary>
            UpdateDepartmentInfo = 317005,
            /// <summary>
            /// 修改产公司信息
            /// </summary>
            UpdateCompanyInfo = 317004,
            #endregion

            #region 税费模版
            TaxesTemplate = 316000,
            AddTaxesTemplate = 316001,
            UpdateTaxesTemplate = 316002,
            DeleteTaxesTemplate = 316003,
            LeadTaxesTemplate = 316004,
            #endregion

            #region 日志管理
            LogManage = 1004005,
            #endregion

            #region 统计
            QueryAndEntrustNum = 103001,//业务统计
            FBusinessStatist = 103102,//统计下属与自身业务
            BusinessStatist = 103101,//统计自身业务
            StatisticsManage = 103000,//统计管理
            #endregion



            #region 查勘管理
            /// <summary>
            /// 管理查勘
            /// </summary>
            ManageSurvey = 108000,

            #region 编辑查勘
            /// <summary>
            /// 编辑查勘
            /// </summary>
            EditSurvey = 108100,
            /// <summary>
            /// 分配查勘
            /// </summary>
            Dispatch_AssignSurvey = 108101,
            DeleteSurvey = 108102,
            UpdateSurvey = 108103,
            #endregion

            #region 查勘列表
            /// <summary>
            /// 查勘列表
            /// </summary>
            ListSurvey = 108200,
            /// <summary>
            /// 查看本人查勘列表
            /// </summary>
            ListUserSurvey = 108201,
            /// <summary>
            /// 查看全部查勘列表
            /// </summary>
            ListAllSurvey = 108202,
            #endregion
            #endregion

            temp = 1
        }
    }
}

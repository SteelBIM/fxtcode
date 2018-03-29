using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
{
    public class ConstCommon
    {
        //这里存放常量，可以改为从资源文件读取。
        #region 常用字段名
        public const string UserId = "UserId";
        public const string USERTOKEN = "usertoken";
        public const string CompanyId = "CompanyId";
        public const string FxtCompanyId = "Fk_Fxt_CompanyId";
        public const string DepartmentId = "FK_DepartmentId";
        public const string DepartmentName = "DepartmentName";
        public const string DeptFullName = "DeptFullName";
        public const string ProvinceId = "ProvinceId";
        public const string ProvinceName = "ProvinceName";
        public const string CityId = "CityId";
        public const string CityName = "CityName";
        public const string AreaId = "AreaId";
        public const string FxtCompanyName = "FxtCompanyName";
        public const string Telephone = "Telephone";
        public const string SysDefaultPage = "SysDefaultPage";
        public const string SysLoginPage = "SysLoginPage";
        public const string FileNo = "FileNo";
        public const string SkinPath = "SkinPath";
        public const string FxtCompanyUrl = "FxtCompanyUrl";
        public const string ShortName = "ShortName";
        public const string LogoPath = "LogoPath";
        public const string SmallLogoPath = "SmallLogoPath";
        public const string WebSysName = "WebSysName";
        public const string UserName = "UserName";
        public const string FK_DepartMentId = "FK_DepartMentId";
        public const string MessageTable = "MessageTable";
        public const string ErrorString = "ErrorString";
        public const string CompanyName = "CompanyName";
        public const string FullCompanyName = "FullCompanyName";
        public const string Fk_SysTypeCode = "Fk_SysTypeCode";
        public const string SubAreaId = "SubAreaId";
        public const string ProjectId = "ProjectId";
        public const string ProjectName = "ProjectName";
        public const string BuildingId = "BuildingId";
        public const string HouseId = "HouseId";
        public const string PurposeCode = "PurposeCode";
        public const string FrontCode = "FrontCode";
        public const string SightCode = "SightCode";
        public const string SysTypeCode = "SysTypeCode";
        public const string SPECSTR = "@@@@";
        public static string MobilePhone = "MobilePhone";
        public const string UserRights = "UserRights";
        public const string FDepartmentId = "FDepartmentId";
        public const string SourceIP = "SourceIP";
        public const string topdeptid = "topdeptid"; 
        #endregion

        //系统类型
        #region 系统类型
        public const int SYSTYPECODE_MOBILE_INQUIRY = 1003004;
        public const int SYSTYPECODE_DISPATCHCENTER = 1003005;
        public const int SYSTYPECODE_SURVEY_ENTERPRISE = 1003008;
        public const int SYSTYPECODE_QUERY_ANDROID = 1003009;
        public const int SYSTYPECODE_QUERY_IOS = 1003010;
        public const int SYSTYPECODE_MANAGE = 1003011;
        public const int SYSTYPECODE_ASSESSMENT = 1003012;
        public const int SYSTYPECODE_BANK = 1003013;
        public const int SYSTYPECODE_ASSOCIATION = 1003014;
        public const int SYSTYPECODE_SASAC = 1003015;
        public const int SYSTYPECODE_COURT = 1003016;
        #endregion
        //Api类
        #region 系统类型
        /// <summary>
        /// 用户中心Api
        /// </summary>
        public const int userCenterApiCode = 1003105;
        /// <summary>
        /// 数据中心Api
        /// </summary>
        public const int dataCenterApiCode = 1003104;
        /// <summary>
        /// 云查勘Api
        /// </summary>
        public const int surveyCenterApiCode = 1003102;
        /// <summary>
        /// 估价宝Api
        /// </summary>
        public const int gjbCenterApiCode = 1003100;
        #endregion
        //业务流初始code
        #region 业务流初始code
        public const int Flow_Query = 5016001;
        public const int Flow_Entrust = 9009001;
        #endregion
        //消息发送方式
        #region 消息发送方式
        public const int Business_Type = 7004001;
        #endregion
        //消息类型
        #region 消息类型
         public const int Tips_Message = 7003004;
        #endregion
         //业务类型
         #region 业务类型
         public const int Business_Query = 1027002;
         #endregion
         //预设组
        #region  预设组
        public const int BANK_GEOUP = 101;
        public const int ASSOCIATION_GEOUP = 102;
        public const int ASSESSMENT_GEOUP = 103;
        public const int SASAC_GEOUP = 104;
        public const int COURT_GEOUP = 105;
        public const int SURVEY_ENTERPRISE_GEOUP = 112;
        public const int DISPATCHCENTER_GEOUP = 115;
        public const int MANAGE_GEOUP = 106;
        #endregion
        //读取多少项
        #region 读取多少项
        //返回楼盘项数
        public const int Project_Top = 15;
        #endregion
        //主题
        public const string Theme = "theme";
        //error 提示
        #region error 提示
        public const string Error_InterfaceExpired = "接口过期";
        public const string Error_InterfaceTimeNotMatch = "接口操作时间不匹配";
        public const string Error_SystemVersionNotMatch = "系统版本不匹配";
        public const string Error_SystemError = "对不起，系统出错，请您联系客服!";
        public const string Error_MissingArgs = "缺少参数";
        public const string Error_IllegalPath = "非法路径";
        public const string Error_Exception = "对不起，服务器异常，请联系客服！";
        public const string Error_CalculateTax = "无法计算税费！";
        #endregion
        //操作逻辑提示
        #region  登录
        public const string Operate_Activation = "该账号未激活";
        public const string Operate_Registration = "该账号可以使用";
        public const string Operate_AccountRegistered = "该账号已被使用";
        public const string Operate_VerificationTimeOut = "验证码已过期";
        public const string Operate_UserIdIsNotExists = "用户名不存在";
        public const string Operate_UserIdOrPasswordError = "用户名或密码错误";
        public const string Operate_HasNoCurrentProduct = "该账号不存在或没有当前产品的权限";
        public const string Operate_UserIsSuspended = "该账号已被暂停使用";
        public const string Operate_UserIsNotAudit = "该账号尚未审核通过";
        public const string Operate_UserIsExpires = "该账号已失效";
        public const string Operate_UserIsDimission = "该员工已离职";
        public const string NEEDLOGIN = "您可能长时间未活动，请退出并重新登录"; //API过期返回给调用方（后台代码） kevin;    
        #endregion
        #region 操作成功提示
        public const string Register_success = "账号已注册成功";
        public const string Activation_success = "账号已激活成功";
        public const string Operation_success = "success";
        #endregion
        #region 回价/调价
        /// <summary>
        /// 回价金额低于审批金额，自动审批
        /// </summary>
        public const string AutoCheckRemark_TotalPrice = "回价金额低于审批金额，自动审批";
        public const string AutoCheckRemark_Percent = "调价幅度低于审批百分比，自动审批";
        #endregion
        #region 报告/委托
        public const string CompleteReport = "报告完成";
        #endregion
        /// <summary>
        /// 红头文件书签名
        /// </summary>
        public const string RedHeadBookmark = "PO_Content";
        /// <summary>
        /// 用户在线状态刷新时间间隔（分）
        /// </summary>
        public const int RefreshInterval = 6;
        /// <summary>
        /// 系统管理员账号
        /// </summary>
        public const string Administrator = "admin";
        /// <summary>
        /// 重复提交
        /// </summary>
        public const string WorkFlowDuplicateSubmission = "该业务对象有尚未办理完成的流程，不能再次提交！";
        /// <summary>
        /// 公司ID_用户ID
        /// </summary>
        public const string NodeJsUserMessage = "user_{0}_{1}";
        /// <summary>
        /// 公司ID_部门ID
        /// </summary>
        public const string NodeJsDepartmentMessage = "user_{0}_{1}";
        /// <summary>
        /// 公司ID
        /// </summary>
        public const string NodeJsCompanyMessage = "user_{0}";
        public const string DefaultHeaderColor = "#eee";
        public const string DefaultMenuBackColor = "#566FAD";
        public const int MaxCharLength = 50;
        /// <summary>
        /// WCF登录接口加密的Key
        /// </summary>
        public const string WcfLoginMd5Key = "fxtlogin*$^0314";
        /// <summary>
        /// WCF接口超时时间
        /// </summary>
        public const long WcfApiTimeOut = 6000000000;

         /// <summary>
        /// 用户密码加密的Key
        /// </summary>
        public const string WcfPassWordMd5Key = "fxtproduct*&2014";

        /// <summary>
        /// Apppwd加密的Key
        /// </summary>
        public const string WcfAppwdMd5Key = "fxtapp*&2014";


        /// <summary>
        /// Token加密的Key
        /// </summary>
        public const string WcfMobileTokenKey = "fxtmobile*&2014";
    }

    public class CacheKey
    {
        /// <summary>
        /// 系统全局配置 kale
        /// </summary>
        public const string SystemConfig = "SystemConfig-Id-{0}";
        /// <summary>
        /// 自定义列 kale
        /// </summary>
        public const string DisplayColumn = "Company-{0}-UserId-{1}-DisplayCode-{2}";
        public const string DefaultDisplayColumn = "Company-{0}-DisplayCode-{1}";
        /// <summary>
        /// 系统CODE kevin
        /// </summary>
        public const string SysCode = "SysCode_{0}";
        /// <summary>
        /// 价格信息 kevin
        /// </summary>
        public const string FieldsTablePrice = "FieldsTable_{0}_2018002_0_200106003";
        /// <summary>
        /// 委估对象 kevin
        /// </summary>
        public const string FieldsTableObject = "FieldsTable_{0}_2018002_{1}_200106003";
        /// <summary>
        /// 询价客户信息 kevin
        /// </summary>
        public const string FieldsTableQuery = "FieldsTable_{0}_2018003_0_200106001";
        /// <summary>
        /// 委托信息 kevin
        /// </summary>
        public const string FieldsTableEntrust = "FieldsTable_{0}_2018001_0_200107001";
        /// <summary>
        /// 预评信息 kevin
        /// </summary>
        public const string FieldsTableYP = "FieldsTable_{0}_2018005_0_200108001";
        /// <summary>
        /// 报告信息 kevin
        /// </summary>
        public const string FieldsTableReport = "FieldsTable_{0}_2018006_0_200109001";
        /// <summary>
        /// 税费信息 hody
        /// </summary>
        public const string FieldsTableTax = "FieldsTable_{0}_2018002_0_200106004";
    }

    public class WcfConst 
    {
        /// <summary>
        /// 安全验证必传参数 []{"appid", "signname", "apppwd", "time", "code"};
        /// </summary>
        public static string[] WcfApiSecurity { get { return new[] { "appid", "signname", "apppwd", "time", "functionname", "code" }; } }
    }

}

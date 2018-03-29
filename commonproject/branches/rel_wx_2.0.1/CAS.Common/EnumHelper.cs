using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAS.Common
{
    public class EnumHelper
    {
        /// <summary>
        /// 系统code kevin 2013-4-1
        /// </summary>
        public enum Codes
        {
            SysTypeCodeSurveyPerson = 1003007,//云查勘个人版
            SysTypeCodeSurveyEnt = 1003008,//云查勘企业版
            SysTypeCodeGJB = 1003018,//gjb
            SysTypeCodeCMB_Bank = 1003020,//招行审贷平台银行端
            SysTypeCodeCMB_SOA = 1003021,//招行审贷平台评估机构端
            SysTypeCodeBank_Survey = 1003022,//云查勘银行版

            BusinessTypeWT = 2018001, //委托
            BusinessTypeWTObject = 2018002, //委估对象
            BusinessTypeQuery = 2018003, //询价
            BusinessTypeSurvey = 2018004, //查勘
            BusinessTypeYP = 2018005, //预评
            BusinessTypeReport = 2018006, //报告
            BusinessTypeJJ = 2018007, //鉴价
            //委估对象类型
            ObjectTypeHouse = 1031001,//住宅
            ObjectTypeOffice = 1031002,//办公
            ObjectTypeBusiness = 1031003,//商业
            ObjectTypeFactory = 1031004,//工业
            ObjectTypeLand = 1031005,//土地
            ObjectTypeZiChan = 1031006,//资产
            ObjectTypeOther = 1031007,//其他
            //查勘来源
            SurveySourceEnt = 6057001, // 企业版
            SurveySourcePer = 6057002, //个人版
            SurveySourceInput = 6057003, //手工
            SurveySourceOut = 6057004,//外部（当前公司充当第三方）
            //查勘优先级
            SurveyClass1 = 6052001, // 常规
            SurveyClass2 = 6052002, //较急
            SurveyClass3 = 6052003, //紧急
            //查勘状态
            SurveyStatusWaitAccept = 5016017, //已委托查勘，待受理
            SurveyStatusRe = 5016018, //委托查勘被拒绝
            SurveyStatusWaitFenPei = 5016019, //已受理查勘委托，待分配查勘
            SurveyStatusWaitSurvey = 5016020, //已分配查勘，待查勘
            SurveyStatusSurveying = 5016021, //已领堪，查勘中
            SurveyStatusComplete = 5016022, //已完成查勘
            SurveyStatusConfirm = 5016023, //已确收查勘表
            SurveyStatusCancel = 5016024, //已撤销查勘

            //照片类型
            PhotoTypeHouse = 200201,//住宅
            PhotoTypeOffice = 200202,//办公
            PhotoTypeBusiness = 200203,//商业
            PhotoTypeFactory = 200204,//工业
            PhotoTypeLand = 200205,//土地
            PhotoTypeZiChan = 200206,//资产
            PhotoTypeOther = 200207,//其他

            ReportTypeHouse = 10010001,//房地产评估报告
            ReportTypeLand = 10010002,//土地评估报告
            ReportTypeZiChan = 10010003,//资产评估报告
        }

        public enum TaxTempateWithDefault
        {
            No = 0,//不包含
            Yes = 1,//包含
            Only = 2//只包含
        }

        public enum ExpirationDateType
        {
            hour = 0,
            minute = 1,
            second = 2
        }

        //日志类型 kevin
        public enum LogType
        {
            Query = 7002001,//询价
            AdjustPrice = 7002002,//调价
            Employee = 7002003,//员工
            User = 7002004,//平台用户
            Report = 7002005,//报告            
            Project = 7002006,//楼盘
            Building = 7002007,//楼栋
            House = 7002008,//房号
            CaseZZ = 7002009,//住宅案例
            CaseGS = 7002010,//工商案例
            CaseTD = 7002011,//土地案例
            TaxTemplete = 7002012,//税费模板
            State = 7002013,//统计
            Message = 7002014,//消息通知
            Entrust = 7002015,//委托评估
            QProject = 7002016,//评估项目
            QObject = 7002017,//评估对象
            ProjectPublic = 7002018,//项目公示
            UserInfo = 7002019,//个人资料
            LoginOut = 7002020,//登录退出
            Company = 7002021,//平台机构
            UserGroup = 7002022,//用户组
            UserGroupPirv = 7002023,//用户组权限
            UserPriv = 7002024,//用户权限
            TempProject = 7002025,//采集楼盘
            TempBuilding = 7002026,//采集楼栋
            TempHouse = 7002027,//采集房号
            ProjectMatch = 7002028,//楼盘匹配
            //SubCompany = 7002029,//分支机构
            Department = 7002030,//部门/分支机构
            Postition = 7002031,//职务
            CompanyDeptInfo = 7002032,//资料管理
            SystemLog = 7002033,//系统日志
            CustomerCompany = 7002034,//相关公司
            WebSiteArticleType = 7002035,//网站资讯类型
            WebSiteArticle = 7002036,//网站资讯内容
            WebSiteSingleArticle = 7002037,//网站单页内容
            WebSiteLinkType = 7002038,//网站友情链接类型
            WebSiteLink = 7002039,//网站友情链接
            WebSiteQuestion = 7002040,//网站客户咨询
            WebSiteAD = 7002041,//网站广告
            AuditCondition = 7002042,//审批条件
            Survey = 7002043,//查勘
            ReportFiles = 7002044,//报告附件
            ReportObjectFiles = 7002045,//评估对象附件
            ReportObjectProperty = 7002046,//产权信息
            OtherQuerySystem = 7002047,//询价系统接口
            Recommend = 7002048,//推荐管理
            TempCase = 7002049,//采集案例
            TempLate = 7002050,//预估单模板
            /// <summary>
            /// 省市地区片区
            /// </summary>
            Area = 7002050,
            Code = 7002051,
            /// <summary>
            /// 权限
            /// </summary>
            Right = 7002052,
            AvgPriceMonth = 7002053,//城市月均价
            CompanyAutoPriceInCity = 7002054,//城市合作机构
            FinalistCompany = 7002055, //入围机构
            FinalistItem = 7002056 //委托项目
        }
        //日志操作类型 kevin
        public enum EventType
        {
            Add = 7001001,//新增
            Edit = 7001002,//修改
            Delete = 7001003,//删除
            Search = 7001004,//查询
            Audit = 7001005,//审核
            Login = 7001006,//登录
            Logout = 7001007,//退出
            Import = 7001008,//导入
            Export = 7001009,//导出
            Press = 7001010,//催办
            Tax = 7001011,//算税费
            Accept = 7001012,//受理
            GivePrice = 7001013,//回价
            Sign = 7001014,//签章
            Ass = 7001015,//备案
            Send = 7001016,//发送
            Download = 7001017,//下载
            Cancel = 7001018,//撤消
            Recommend = 70010010,//推荐
            Allot = 7001020,//分配
            Enroll = 7001021,//报名
            Transfer = 7001022,//调动
            Copy = 7001023,//复制
            Pause = 7001024,//暂停
            Start = 7001025,//启用
            Ask = 7001026,//申报
            DownloadPic = 7001027 //打包下载图片
        }

        /// <summary>
        /// 节点位置
        /// <remarks>变量名再做重构</remarks>
        /// </summary>
        public enum WorkFlowNodeLocations
        {
            [Description("开始")]
            开始节点,
            [Description("中间段")]
            中间段,
            [Description("结束")]
            结束节点
        }

        /// <summary>
        /// 业务节点
        /// <remarks>变量名再做重构</remarks>
        /// </summary>
        public enum WorkFlowBusinessNode
        {
            互审 = 0,
            初审 = 1,
            二审 = 2,
            三审 = 3,
            终审 = 4,
            盖章 = 5,
            复印 = 6,
            其他 = 7
        }

        /// <summary>
        /// 候选审批人模式
        /// <remarks>变量名再做重构</remarks>
        /// </summary>
        public enum WorkFlowApprovalUsersType
        {
            [Description("审批时自由指定")]
            审批时自由指定,
            [Description("从默认审批人中选择")]
            从默认审批人中选择,
            [Description("从默认审批机构中选择")]
            从默认审批机构中选择,
            [Description("从默认审批部门中选择")]
            从默认审批部门中选择,
            [Description("从默认审批角色中选择")]
            从默认审批角色中选择,
            [Description("自动选择流程发起人")]
            自动选择流程发起人,
            [Description("自动选择本部门主管")]
            自动选择本部门主管,
            [Description("自动选择上级部门主管")]
            自动选择上级部门主管,
            [Description("无需审批人")]
            无需审批人
        }

        /// <summary>
        /// 步骤评审模式
        /// <remarks>变量名再做重构</remarks>
        /// </summary>
        public enum WorkFlowReviewType
        {
            [Description("一人通过可向下流转")]
            一人通过可向下流转,
            [Description("全部通过可向下流转")]
            全部通过可向下流转
        }

        /// <summary>
        /// 步骤审核状态
        /// <remarks>变量名再做重构</remarks>
        /// </summary>
        public enum WorkFlowStepStatus
        {
            等待审核 = 0,
            审核通过 = 1,
            驳回发文人 = 2,
            驳回到其他节点 = 4,
            登记错误不过流程 = 5,
            强制审核 = 6,
            强制驳回发文人 = 7,
            强制驳回到其他节点 = 8,
            强制登记错误不过流程 = 9,
            /// <summary>
            /// 目前该常量只应用在Dat_NWorkStepLog的status中,应用场景：审批流程在满足一定条件后自动结束
            /// </summary>
            结束步骤自动结束 = 10,
            回到初审前 = 11,
            有争议的通过 = 12,
            强制有争议的通过 = 13,
        }

        //工作状态
        public enum WorkFlowStatus
        {
            [Description("正常结束")]
            正常结束,
            [Description("强制结束")]
            强制结束,
            [Description("已被驳回")]
            已被驳回,
            [Description("正在办理")]
            正在办理,
            [Description("回到初审前")]
            回到初审前
        }

        //附件业务类型
        public enum DatFileType
        {
            Object = 2018002, //委估对象 kevin
            Query = 2018003, //询价 kevin
            Report = 2018006,//报告
            YP = 2018005,//预评
            Email = 8001001,//邮件类型
            GongGao = 8001002, //公告类型
            ChuanYue = 8001003,//传阅文件类型
            RedHead = 8001004, //红头文件附件
            User = 8001005,//用户信息附件
            CustomFLowForm = 8001006,  //自定义流程表单
            ReportTemplate = 8001007, //报告Word模板文件 kevin
            ReportDataTemplate = 8001009, //报告数据Excel模板文件 kevin
            MathTemplate = 8001008 //测算表模板文件 kevin
        }

        /// <summary>
        /// 消息类型 1:待办 2：消息 3：通知
        /// </summary>
        public enum MessageType
        {
            待办 = 1,
            消息 = 2,
            通知 = 3
        }

        public enum SendType
        {
            用户消息,
            部门消息,       //机构消息也在此部门消息范围内
            公司消息
        }

        public enum SimMessageType
        {
            待分配,
            分配,
            已分配,
            业务完成,//指报告或者预评
            转交确认,
            转交已确认,
            转交通知,
            转交撤销,
            待回价,
            已回价,
            询价已分配,
            价格纠错,
            价格纠错审批,
            客户回价消息,
            业务回价消息,
            撤销业务
        }
    }
}

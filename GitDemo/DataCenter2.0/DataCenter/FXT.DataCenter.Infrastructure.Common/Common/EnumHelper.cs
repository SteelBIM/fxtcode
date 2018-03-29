using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FXT.DataCenter.Infrastructure.Common.Common
{
    public class EnumHelper
    {
        public enum Status
        {
            /// <summary>
            /// 失败
            /// </summary>
            Failure = -1,
            /// <summary>
            /// 错误
            /// </summary>
            LoginFailure = -2,
            /// <summary>
            /// 成功
            /// </summary>
            Success = 1,
            /// <summary>
            /// 无状态
            /// </summary>
            None = 0
        }

        /// <summary>
        /// 系统code kevin 2013-4-1
        /// </summary>
        public enum Codes
        {
            SysTypeCodeDataCenter=1003002,//运维中心

            SysTypeCodeSurveyPerson = 1003007,//云查勘个人版
            SysTypeCodeSurveyEnt = 1003008,//云查勘企业版
            SysTypeCodeGJB = 1003018,//gjb
            SysTypeCodeCMB_Bank = 1003020,//招行审贷平台银行端
            SysTypeCodeCMB_SOA = 1003021,//招行审贷平台评估机构端

            SysTypeCodeSurvey_Bank = 1003022,//云查勘金融版
            SysTypeCodeSurvey_SOA = 1003101,//云查勘估价宝版
            SysTypeCodeSurvey_Alone = 1003102,//云查勘独立版（接口）
            SysTypeCodeSurvey_Open = 1003103,//云查勘开放版  


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
    }
}

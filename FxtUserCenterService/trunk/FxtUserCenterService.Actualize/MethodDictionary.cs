using FxtUserCenterService.Actualize.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FxtUserCenterService.Actualize
{
    public class MethodDictionary
    {

        //获取company相关功能
        private const string companyone = "companyone";
        private const string companytwo = "companytwo";
        private const string companythree = "companythree";
        private const string companyfour = "companyfour";
        private const string companyfive = "companyfive";
        private const string companysix = "companysix";
        private const string companyseven = "companyseven";
        private const string companyeight = "companyeight"; //获得已签约客户的
        private const string companynine = "companynine";
        private const string companyten = "companyten";
        private const string companyeleven = "companyeleven";
        private const string companytwelve = "companytwelve";
        private const string companythirteen = "companythirteen"; 

        //获取user相关功能
        private const string userone = "userone";
        private const string usertwo = "usertwo";
        private const string userthree = "userthree";
        private const string userfour = "userfour";
        private const string userfive = "userfive";
        private const string usersix = "usersix";
        private const string userseven = "userseven";
        private const string usereight = "usereight";
        private const string usernine = "usernine";
        private const string userten = "userten";
        private const string usereleven = "usereleven";
        private const string usertwelve = "usertwelve";

        //获取mobilepush相关功能
        private const string mpone = "mpone";
        private const string mptwo = "mptwo";
        private const string mpthree = "mpthree";
        //个推接口
        private const string mpgtone = "mpgtone";
        private const string mpgttwo = "mpgttwo";

        //获取productfeedback相关功能
        private const string pfbone = "pfbone";

        //获取sendsms相关功能
        private const string ssone = "ssone";

        //获取companyproduct相关功能
        private const string cpone = "cpone";
        private const string cptwo = "cptwo";
        private const string cpfour = "cpfour";
        private const string cpfive = "cpfive";
        //获取公司开通产品的城市ID
        private const string cptcityids = "cptcityids";
        //获取公司开通产品模块的城市ID
        private const string cptmcityids = "cptmcityids";
        //开通产品
        private const string cpthree = "cpthree";
        //查询是否开通产品模块权限
        private const string cpmone = "cpmone";
        //根据公司ID和产品code获取公司开通产品模块城市ID
        private const string cpmthree = "cpmthree";
        //根据公司ID和产品code获取公司开通产品模块详细信息
        private const string cpmfour = "cpmfour";

        //获取功能清单
        private const string flone = "flone";

        //获取产品的API信息
        private const string productapiinfo = "productapiinfo";
        //修复产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话
        private const string modifyproductpartinfo = "modifyproductpartinfo";
        //根据WebUrl查询产品信息
        private const string productinfobyurl = "productinfobyurl";

        //系统操作日志
        private const string lol = "lol";
        //登录日志
        private const string silog = "silog";
        //退出日志
        private const string solog = "solog";
        //更新在线时间
        private const string updateonline = "updateonline";
        //验证应用身份
        private const string valide_id = "valide_id";


        //获取company相关功能-->


        /// <summary>
        /// 存放字典
        /// </summary>
        public static readonly Dictionary<string, string> MethodDic = new Dictionary<string, string>
            {
                { companyone,"GetCompanyByCode" },
                { companytwo,"GetCompanyById" },
                { companythree,"CompanyEdit" },
                { companyfour,"GetCompanyBySearch" },
                { companyfive,"GetCompanyBySignName" },
                { companysix,"GetCompanyInfoIfSecurity" },
                { companyseven,"GetCompanyList" },
	            { companyeight,"GetCompanyListIssigned" },//获得已签约客户的-->
                { companynine,"CompanyAdd" },
                { companyten,"GetCompanyBusinessDBList" },
                { companyeleven,"UpdateWXByCompanyId" },
                { companytwelve,"GetCompanyInfoByCompanyIds" },
                { companythirteen,"GetCompanyProductByCompanyidAndProductTypeCode" },
                
                //产品模块权限
                { cpmone,"GetCompanyProductModuleWhetherToOpen" },
                { cpmthree,"GetCompanyProductAndCompanyProductModuleCityIds" },
                { cpmfour,"GetCompanyProductModuleDetails" },

                //获取user相关功能-->
                { userone,"UserCheck" },
                { usertwo,"UserAdd" },
                { userthree,"UserFind" },
                { userfour,"UserHandlerAdd" },
                { userfive,"UserHandlerEdit" },
                { usersix,"UserHandlerDelete" },
                { userseven,"UserList" },
                { usereight,"UpdatePwd" },
                 { usernine,"UpdateUserPwd" },
                 { userten,"GetUserInfoByUserNames" },
                 { usereleven,"GetUserListByUserName" },
                 { usertwelve,"GetUserListByUserNameOrTrueName" },


                //获取mobilepush相关功能-->
                { mpone,"MobilePushBind" },
                { mptwo,"MobilePushSend" },
                { mpthree,"MobilePushEdit" },
                //个推接口-->
                { mpgtone,"MobileGeTuiBind" },
                { mpgttwo,"MobileGeTuiSend" },
    
                //获取productfeedback相关功能-->
                { pfbone,"ProductFeedBack" },   

                //获取sendsms相关功能-->
                { ssone,"SendSMS" },

                //获取companyproduct相关功能-->
                { cpone,"GetCompanyProduct" },
                { cptwo,"GetCompanyProductList" },
                { cpfour,"CompanyProductAdd" },
                { cpfive,"GjbCompanyProductAdd" },
                //获取公司开通产品的城市ID-->
                { cptcityids,"GetCompanyProductCityIds" },
                //获取公司开通产品模块的城市ID-->
                { cptmcityids,"GetCompanyProductModuleCityIds" },
                //开通产品
                { cpthree,"SetOpenProducts" },

                //获取功能清单-->
                { flone,"GetFunlist" },

                //获取产品的API信息-->
                { productapiinfo,"GetProductAPIInfo" },
                //修复产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话-->
                { modifyproductpartinfo,"UpdateProductPartialInfo" },
                //根据WebUrl查询产品信息-->
                { productinfobyurl,"GetProductInfoByWebUrl" },

                //系统操作日志-->
                { lol,"OperateLog" },
                //登录日志-->
                { silog,"SignIn" },
                //退出日志-->
                { solog,"SignOut" },
                //更新在线时间-->
                { updateonline,"UpdateActiveTime" },
	            //验证应用身份-->
	            { valide_id,"ValidateCallIdentity" }
            };

        /// <summary>
        /// 通过反射找方法
        /// </summary>
        /// <param name="type">代理方法名</param>
        /// <returns></returns>
        public static string GetMethodInfo(string type)
        {
            string methName = "";
            try
            {
                methName = MethodDictionary.MethodDic[type];
            }
            catch (Exception ex)
            {

                methName = "";
            }
            
            return methName;
        }
    }
}

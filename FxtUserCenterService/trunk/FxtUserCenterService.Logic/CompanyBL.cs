using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Common;

namespace FxtUserCenterService.Logic
{
    public class CompanyBL
    {
        public static CompanyInfo GetCompanyBySignName(string signName)
        {
            return CompanyDA.GetCompanyBySignName(signName);
        }

        public static CompanyInfo Get(string companycode)
        {
            return CompanyDA.Get(companycode);
        }

        public static CompanyInfo Get(int companyid)
        {
            return CompanyDA.Get(companyid);
        }

        public static CompanyInfo GetByName(string name)
        {
            return CompanyDA.GetByName(name);
        }

        public static int update(string companycode, string wxid, string wxname)
        {
            return CompanyDA.update(companycode, wxid, wxname);
        }

        /// <summary>
        /// 根据公司ID更新微信ID、微信名称
        /// zhoub 20160907
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="wxid"></param>
        /// <param name="wxname"></param>
        /// <returns></returns>
        public static int UpdateWXByCompanyId(int companyid, string wxid, string wxname)
        {
            return CompanyDA.UpdateWXByCompanyId(companyid, wxid, wxname);
        }

        /// <summary>
        /// 通过productTypeCode、key搜索
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyInfo(SearchBase search, int productTypeCode, string key)
        {
            return CompanyDA.GetCompanyInfo(search, productTypeCode, key);
        }
        /// <summary>
        /// 根据参数查询公司
        /// 修改人 caoq 2014-12-11 增加参数producttypecode（查询已开通指定产品公司）
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyname">查询公司名称</param>
        /// <param name="companytypecode">公司类型</param>
        /// <param name="producttypecode">产品CODE(查询已开通指定产品公司)</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyList(SearchBase search, string companyname, int companytypecode, int producttypecode)
        {
            return CompanyDA.GetCompanyList(search, companyname, companytypecode, producttypecode);
        }

         /// <summary>
        /// 获得公司信息通过signname
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <param name="signname">公司标示</param>
        /// <param name="appid">appid</param>
        /// <param name="apppwd">apppwd</param>
        /// <returns></returns>
        public static CompanyInfo GetCompanyInfoBySignName(int systypecode, string signname, int appid, string apppwd, string functionname)
        {
            return CompanyDA.GetCompanyInfoBySignName(systypecode, signname, appid, apppwd, functionname);
        }

        /// <summary>
        /// 获得已签约客户的
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <param name="signname">公司标示</param>
        /// <param name="appid">appid</param>
        /// <param name="apppwd">apppwd</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyListIssigned(SearchBase search, int provinceid, int issigned)
        {
            return CompanyDA.GetCompanyListIssigned(search, provinceid, issigned);
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static CompanyInfo Add(CompanyInfo model)
        {
            var conpany = Get(model.companycode);
            var companyid = Get(model.companyid);
            var companyname = GetByName(model.companyname);
            if (conpany != null)
            {
            }
            else if (companyid != null)
            {
                conpany = companyid;
            }
            else if (companyname != null)
            {
                conpany = companyname;
            }
            else
            {
                int id = CompanyDA.Add(model);
                if (id > 0)
                {
                    conpany = model;
                }
                else
                {
                    conpany = null;
                }
            }

            return conpany;
        }

        /// <summary>
        /// 根据产品code获取已签约且业务数据库连接不为空的公司
        /// zhoub 20160727
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyBusinessDBList(int systypecode)
        {
            return CompanyDA.GetCompanyBusinessDBList(systypecode);
        }

        /// <summary>
        /// 根据公司ID获取公司信息（多个ID用逗号隔开）
        /// zhoub 20160908
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyInfoByCompanyIds(string companyids)
        {
            return CompanyDA.GetCompanyInfoByCompanyIds(companyids);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.DataAccess;
using FxtUserCenterService.Entity;
using CAS.Entity;
using FxtUserCenterService.Entity.InheritClass;
using CAS.Common;

namespace FxtUserCenterService.Logic
{
    public class CompanyProductBL
    {
        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <param name="companycode">公司编码</param>
        /// <returns></returns>
        public static CompanyProduct GetInfo(int companyid, int producttypecode, string signname, int isvalid, string companycode)
        {
            List<CompanyProduct> prolist = CompanyProductDA.Get(companyid, (producttypecode > 0 ? producttypecode.ToString() : ""), signname, isvalid, companycode);
            return ((prolist != null && prolist.Count() > 0) ? prolist.FirstOrDefault() : null);
        }


        /// <summary>
        /// 根据公司id获取所有产品信息(caoq 2013-11-23)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <returns></returns>
        public static List<CompanyProduct> GetList(int companyid, int[] producttypecode, string signname, int isvalid)
        {
            string pro = (producttypecode == null || producttypecode.Length == 0) ? "" : string.Join(",", producttypecode.Select(i => i.ToString()).ToArray());
            return CompanyProductDA.Get(companyid, pro, signname, isvalid, null);
        }

        /// <summary>
        /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话(hody 2014-04-24)
        /// </summary>
        /// <param name="logoPath">CAS产品LOGO</param>
        /// <param name="smallLogoPath">CAS产品小LOGO</param>
        /// <param name="telephone">对外显示的产品名</param>
        /// <param name="titleName">产品联系电话</param>
        /// <returns></returns>
        public static int UpdateProductPartialInfo(string logoPath, string smallLogoPath, string telephone, string titleName, int companyid, int systypecode, string bgpic, string homepage, string twodimensionalcode)
        {
            return CompanyProductDA.UpdateProductPartialInfo(logoPath, smallLogoPath, telephone, titleName, companyid, systypecode, bgpic, homepage, twodimensionalcode);
        }

        /// <summary>
        /// 根据WebUrl查询产品信息
        /// </summary>
        /// <param name="weburl">网址</param>
        /// <param name="weburl1">备用网址</param>
        /// <returns></returns>
        public static InheritCompanyProduct GetProductInfoByWebUrl(string weburl, string weburl1)
        {
            return CompanyProductDA.GetProductInfoByWebUrl(weburl, weburl1);
        }

        /// <summary>
        /// 产品授权
        /// 20150811 wb add
        /// </summary>
        /// <returns></returns>
        public static int SetOpenProduct(CompanyProduct entity)
        {
            //云查勘所有产品单独处理
            int[] yckPtcs = new int[] 
            {
                (int)EnumHelper.Codes.SysTypeCodeSurveyPerson,//云查勘个人版
                (int)EnumHelper.Codes.SysTypeCodeSurveyEnt,//云查勘企业版
                (int)EnumHelper.Codes.SysTypeCodeSurvey_Bank,//云查勘金融版
                (int)EnumHelper.Codes.SysTypeCodeSurvey_SOA,//云查勘估价宝版
                //SysTypeCodeSurvey_Alone = 1003102,//云查勘独立版（接口）
                (int)EnumHelper.Codes.SysTypeCodeSurvey_Open//云查勘开放版  
            };

            //更新开通城市权限到期时间 zhoub 20160621 add
            List<CompanyProduct> listCompanyProduct = CompanyProductBL.GetCompanyProductByCodeAndCompanyIdAndCityId(entity.producttypecode, entity.companyid);
            foreach (CompanyProduct cp in listCompanyProduct)
            {
                cp.overdate = entity.overdate;
                CompanyProductUpdate(cp);
            }

            if (Array.IndexOf(yckPtcs, entity.producttypecode) != -1)
            {
                #region 云查勘 授权

                //云查勘 重要说明：
                //1.云查勘所有版本，在数据库只能有一条记录。
                //2.升级顺序(低到高)：云查勘个人版 > 云查勘金融版 > (云查勘企业版/云查勘开放版) > 云查勘估价宝版。
                //3.高级别云查勘授权时：如是本身直接更新；如比自己级别高不处理；如果比自己级别低直接升级为自己高级别。
                //4.升级时通过CompanyID、AppAbbreviation='yck'、producttypecode，直接更新producttypecode。
                //5.云查勘企业版/云查勘开放版 为同一级别

                List<CompanyProduct> list = CompanyProductDA.GetCompanyProductList(entity.companyid, yckPtcs, (int)entity.cityid);

                if (list != null && list.Count > 0)
                {
                    //已开通的云查勘 一条数据为正常
                    if (list.Count > 1)
                        throw new Exception("用户中心:云查勘授权数据有误，请联系维护人员！（companyid=" + entity.companyid + "）");

                    #region 更新时针对不同系统做更新字段处理 当前只更新overdate、valid、MaxSubCompanyCount
                    switch (entity.producttypecode)
                    {
                        default:
                            //处理可以更新的字段
                            if (entity.valid == 0)
                                entity.SetAvailableFields(new string[] { "valid" });//禁用时不做其他更新
                            else
                                entity.SetAvailableFields(new string[] { "overdate", "valid", "MaxSubCompanyCount" });

                            //忽略
                            entity.SetIgnoreFields(new string[] { });
                            //主键
                            entity.SetPrimaryKey<CompanyProduct>(new string[] { "companyid", "producttypecode", "cityid" });
                            //主鍵非Identity
                            entity.SetPrimaryKeyIsIdentify(false);
                            break;
                    }
                    #endregion

                    #region 更新、升级、遇高级别不处理
                    switch (entity.producttypecode)
                    {
                        case (int)EnumHelper.Codes.SysTypeCodeSurvey_SOA://云查勘估价宝版
                            if (list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_SOA)
                            {
                                //直接更新
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                            else
                            {
                                //升级为(云查勘估价宝版)
                                if (CompanyProductDA.UpdateCompanyProductPtc(entity.companyid, list[0].producttypecode, entity.producttypecode) <= 0)
                                    throw new Exception("用户中心:云查勘升级失败，请联系维护人员！（companyid=" + entity.companyid + "）");
                                //更新操作
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                        //break;

                        case (int)EnumHelper.Codes.SysTypeCodeSurveyEnt://云查勘企业版　同级别
                        case (int)EnumHelper.Codes.SysTypeCodeSurvey_Open://云查勘开放版
                            if (list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_SOA)
                            {
                                return 1;//不做处理
                            }
                            else if (list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurveyEnt
                                    || list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_Open)
                            {
                                //直接更新
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                            else
                            {
                                //升级为(云查勘企业版、云查勘开放版)
                                if (CompanyProductDA.UpdateCompanyProductPtc(entity.companyid, list[0].producttypecode, entity.producttypecode) <= 0)
                                    throw new Exception("用户中心:云查勘升级失败，请联系维护人员！（companyid=" + entity.companyid + "）");
                                //更新操作
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                        //break;

                        case (int)EnumHelper.Codes.SysTypeCodeSurvey_Bank://云查勘金融版
                            if (list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_SOA
                                || list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurveyEnt
                                || list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_Open
                                )
                            {
                                return 1;//不做处理
                            }
                            else if (list[0].producttypecode == (int)EnumHelper.Codes.SysTypeCodeSurvey_Bank)
                            {
                                //直接更新
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                            else
                            {
                                //升级为(云查勘金融版)
                                if (CompanyProductDA.UpdateCompanyProductPtc(entity.companyid, list[0].producttypecode, entity.producttypecode) <= 0)
                                    throw new Exception("用户中心:云查勘升级失败，请联系维护人员！（companyid=" + entity.companyid + "）");
                                //更新操作
                                return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                            }
                        //break;

                        case (int)EnumHelper.Codes.SysTypeCodeSurveyPerson://云查勘个人版
                            //更新操作
                            return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                        //break;

                        case (int)EnumHelper.Codes.SysTypeCodeSurvey_Alone:
                            throw new Exception("云查勘独立版（接口）,产品授权未实现！");

                        default:
                            throw new Exception("此产品授权未实现！");
                    }
                    #endregion
                }
                else
                {
                    return CompanyProductDA.InsertFromEntity<CompanyProduct>(entity);
                }
                #endregion
            }
            else
            {
                #region 非云查勘 通用处理
                if (CompanyProductDA.CheckedCompanyProductExists(entity.companyid, entity.producttypecode, (int)entity.cityid))
                {
                    //针对不同系统更新时做实体处理
                    switch (entity.producttypecode)
                    {
                        default:
                            //处理可以更新的字段
                            if (entity.valid == 0)
                                entity.SetAvailableFields(new string[] { "valid" });//禁用时不做其他更新
                            else
                                entity.SetAvailableFields(new string[] { "overdate", "valid", "MaxSubCompanyCount" });

                            //忽略
                            entity.SetIgnoreFields(new string[] { });
                            //主键
                            entity.SetPrimaryKey<CompanyProduct>(new string[] { "companyid", "producttypecode", "cityid" });
                            //主鍵非Identity
                            entity.SetPrimaryKeyIsIdentify(false);
                            break;
                    }

                    return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
                }
                else
                {
                    return CompanyProductDA.InsertFromEntity<CompanyProduct>(entity);
                }
                #endregion
            }
        }

        /// <summary>
        /// 根据产品code、公司ID查询已开通城市权限
        /// zhoub 20160531
        /// </summary>
        /// <param name="producttypecode">产品code</param>
        /// <param name="companyid">公司ID</param>
        /// <returns></returns>
        public static List<CompanyProduct> GetCompanyProductByCodeAndCompanyIdAndCityId(int producttypecode, int companyid)
        {
            return CompanyProductDA.GetCompanyProductByCodeAndCompanyIdAndCityId(producttypecode, companyid);
        }

        /// <summary>
        /// 更新产品权限表数据
        /// zhoub 20160620
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int CompanyProductUpdate(CompanyProduct entity)
        {
            return CompanyProductDA.UpdateFromEntity<CompanyProduct>(entity);
        }

        /// <summary>
        /// 添加产品权限表数据
        /// zhoub 20160621
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int CompanyProductAdd(CompanyProduct entity)
        {
            return CompanyProductDA.InsertFromEntity<CompanyProduct>(entity);
        }

        /// <summary>
        /// 流量配置
        /// zhoub 20160621
        /// </summary>
        /// <param name="companyid">公司ID</param>
        /// <returns></returns>
        public static int FlowControlConfig(int companyid)
        {
            return CompanyProductDA.FlowControlConfig(companyid);
        }

        /// <summary>
        /// 查询公司是否包含该产品和城市
        /// zhoub 20160914
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static List<CompanyProduct> GetCompanyProductByCompanyidAndProductTypeCode(int companyid, int producttypecode, int cityid)
        {
            return CompanyProductDA.GetCompanyProductByCompanyidAndProductTypeCode(companyid,producttypecode,cityid);
        }
    }
}


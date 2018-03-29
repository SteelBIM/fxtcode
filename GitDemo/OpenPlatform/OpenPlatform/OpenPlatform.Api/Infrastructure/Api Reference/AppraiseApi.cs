using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Ninject;
using OpenPlatform.Api.Infrastructure;
using OpenPlatform.Api.Infrastructure.Utils;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.DTO;
using OpenPlatform.Domain.Models;
using OpenPlatform.Framework.IoC;
using OpenPlatform.Framework.Utils;
using OpenPlatform.Framework.Utils.Log;

namespace OpenPlatform.Api.Infrastructure
{
    public class AppraiseApi
    {
        private static readonly IEvaluationService Services2 = new StandardKernel(new EvaluationServiceBinder()).Get<IEvaluationService>();

        public static void Invoke(string idNum, List<string> companyIds)
        {

            try
            {
                dynamic gjbObj = GetGjbConfigs();      //估价宝接口配置参数
                dynamic dcObj = GetAutoPriceConfigs(); //自动估价接口配置参数

                if (!companyIds.Any())                 //到云查勘中查出companyIds
                {
                    List<string> companyIdsFromYck;
                    try
                    {
                        companyIdsFromYck = GetSurveyData<List<string>>("survey/get_company_info", new { idCards = idNum });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("云查勘：获取公司ID接口异常", ex);
                    }

                    if (companyIdsFromYck == null || !companyIdsFromYck.Any()) return;

                    companyIds = companyIdsFromYck.Distinct().ToList();
                }

                foreach (var companyId in companyIds)
                {
                    DataIntegration(gjbObj, dcObj, int.Parse(companyId), idNum);
                }


            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// 把来自同一笔业务的估价宝和云查勘数据合并
        /// </summary>
        /// <param name="gjbObj">估价宝接口配置参数</param>
        /// <param name="dcObj">自动估价接口参数</param>
        /// <param name="companyid">公司ID</param>
        /// <param name="idNum">身份证ID</param>
        public static void DataIntegration(dynamic gjbObj, dynamic dcObj, int companyid, string idNum)
        {
            List<Evaluation4GjbDto> gjbApiData;
            try
            {
                gjbApiData = GetReturnValue<List<Evaluation4GjbDto>>(gjbObj, new { companyid, idcard = idNum }) as List<Evaluation4GjbDto>;
            }
            catch (Exception ex)
            {
                throw new Exception("估价宝接口异常", ex);
            }

            if (gjbApiData == null || !gjbApiData.Any()) return;
            foreach (var item in gjbApiData)
            {

                /*
                 * 委估业务信息
                 */
                Services2.AddEntrust(item);


                /*
                 * 委估对象信息
                 */
                var objects = item.EntrustObject;
                if (objects.Any())
                {
                    foreach (var obj in objects)
                    {

                        SurveyInfoDto surveyInfo = null;
                        long sId = 0; //获取照片查勘接口需要查勘ID
                        if (obj.Valid == 1)//已删除委估对象，无需再调用查勘接口
                        {

                            /*
                            * 自动估价
                            */
                            if (obj.AutoPrice == null || obj.AutoPrice < 1)
                            {
                                try
                                {
                                    var autoPrice = GetReturnValue<AutoPrice>(dcObj, new { cityid = obj.CityId, projectid = obj.ProjectId, buildingid = obj.BuildingId, houseId = obj.HouseId, type = 0, systypecode = dcObj.systypecode as string, userid = "admin@szbn", companyid = companyid, queryTypeCode = 1004001, buildingarea = obj.BuildingArea }, "casAtuoPrice") as AutoPrice;
                                    if (autoPrice != null) obj.AutoPrice = Convert.ToDecimal(autoPrice.UnitPrice);

                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("自动估价接口异常", ex);
                                }

                            }


                            /*
                            * 查勘信息
                            */
                            try
                            {
                                surveyInfo = GetSurveyData<SurveyInfoDto>("survey/get_surveyhouse_info", new { fxtCompanyId = companyid, objectId = obj.GjbObjId });
                               
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("云查勘:获取云查勘信息接口", ex);
                            }

                            if (surveyInfo != null)
                            {
                                sId = surveyInfo.Sid;

                                SurveyInfoConverter(ref surveyInfo);

                                obj.Surveyor = surveyInfo.Surveyor;
                                obj.IsSurvey = surveyInfo.IsSurvey;
                                obj.FinancingPurpose = surveyInfo.FinancingPurpose;
                                obj.Usage = surveyInfo.Usage;
                                obj.DecorationValue = surveyInfo.DecorationValue;
                                obj.BusLineNum = surveyInfo.BusLineNum;
                                obj.HousingLocation = surveyInfo.HousingLocation;
                                obj.PublicFacilitiesNum = surveyInfo.PublicFacilitiesNum;
                                obj.SurveyBeginTime = surveyInfo.SurveyBeginTime;
                                obj.SurveyEndTime = surveyInfo.SurveyEndTime;

                                if (!string.IsNullOrEmpty(surveyInfo.BuildingStructure))
                                {
                                    obj.BuildingStructure = surveyInfo.BuildingStructure;
                                }
                                if (!string.IsNullOrEmpty(surveyInfo.TotalFloor))
                                {
                                    obj.TotalFloor = surveyInfo.TotalFloor;
                                }
                                if (!string.IsNullOrEmpty(surveyInfo.Floor))
                                {
                                    obj.Floor = surveyInfo.Floor;
                                }
                                if (!string.IsNullOrEmpty(surveyInfo.RoomNum))
                                {
                                    obj.RoomNum = surveyInfo.RoomNum;
                                }
                                if (!string.IsNullOrEmpty(surveyInfo.BalconyNum))
                                {
                                    obj.BalconyNum = surveyInfo.BalconyNum;
                                }
                                if (surveyInfo.PrepareLoanAmount != null)
                                {
                                    obj.PrepareLoanAmount = surveyInfo.PrepareLoanAmount;
                                }


                            }


                        }


                        obj.EntrustId = item.GjbEntrustId;
                        obj.FxtCompanyId = item.FxtCompanyId;
                        Services2.AddEntrustObject(obj);


                        /*
                        * 照片信息 file/ get_files
                        */
                        List<Surveyfiles> fileInfo;
                        try
                        {
                            fileInfo = GetSurveyData<List<Surveyfiles>>("file/get_files", new { sid = sId, fxtCompanyId = companyid });
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("云查勘：获取照片信息接口", ex);
                        }

                        if (fileInfo != null)
                        {
                            foreach (var fi in fileInfo)
                            {
                                fi.GJBObjId = obj.GjbObjId;
                                fi.FxtCompanyId = item.FxtCompanyId;
                                Services2.AddPictures(fi);
                            }

                        }


                        /*
                         * 产权信息
                         */
                        var propertyInfo = obj.PropertyInfo;
                        if (propertyInfo != null && propertyInfo.Any())
                        {

                            var index = 0;
                            foreach (var pi in propertyInfo)
                            {
                                index++;
                                if (surveyInfo != null)
                                {
                                    if (surveyInfo.IdNum != null)
                                    {
                                        if (surveyInfo.IdNum.Contains(pi.IdNum))
                                        {
                                            pi.MaritalStatus = surveyInfo.MaritalStatus;
                                            pi.HasChildren = surveyInfo.HasChildren;
                                        }
                                    }
                                }
                                pi.IsFirstCall = index == 1;
                                pi.GjbObjId = obj.GjbObjId;
                                pi.FxtCompanyId = item.FxtCompanyId;

                                Services2.AddProperty(pi);
                            }

                        }


                        /*
                         * 购房人信息
                         */
                        var buyerInfo = obj.BuyerInfo;
                        if (buyerInfo != null && buyerInfo.Any())
                        {
                            var index = 0;
                            foreach (var bi in buyerInfo)
                            {
                                index++;
                                bi.IsFirstCall = index == 1;
                                bi.GJBObjId = obj.GjbObjId;
                                bi.FxtCompanyId = item.FxtCompanyId;

                                Services2.AddBuyer(bi);
                            }


                        }


                    }
                }
            }
        }

        /// <summary>
        /// 云查勘自定义字段析取
        /// </summary>
        /// <param name="surveyInfo"></param>
        public static void SurveyInfoConverter(ref SurveyInfoDto surveyInfo)
        {
            try
            {

                if (surveyInfo == null)
                {
                    surveyInfo = new SurveyInfoDto();
                    return;
                }
                if (string.IsNullOrEmpty(surveyInfo.CustomFields))
                {
                    //surveyInfo = new SurveyInfoDto();
                    return;
                }

                var customFields = JsonConvert.DeserializeObject<List<CustomFields>>(surveyInfo.CustomFields);

                foreach (var c in customFields)
                {
                    if (c.c.Trim() == "重要信息")
                    {

                        var customFieldValues = JsonConvert.DeserializeObject<List<CustomFieldValue>>(c.v.ToString());
                        foreach (var v in customFieldValues)
                        {
                            if (v.c.Trim() == "装修价值")
                            {
                                if (string.IsNullOrEmpty(v.s))
                                {
                                    surveyInfo.DecorationValue = null;
                                }
                                else
                                {
                                    surveyInfo.DecorationValue = Convert.ToDecimal(v.s);
                                }

                            }
                            if (v.c.Trim() == "公交线路数量")
                            {
                                surveyInfo.BusLineNum = v.s;
                            }
                            if (v.c.Trim() == "房产位置")
                            {
                                surveyInfo.HousingLocation = v.s;
                            }
                            if (v.c.Trim() == "公共配套设施数量")
                            {
                                surveyInfo.PublicFacilitiesNum = v.s;
                            }
                            if (v.c.Trim() == "婚姻状况")
                            {
                                surveyInfo.MaritalStatus = v.s;
                            }
                            if (v.c.Trim() == "有无子女")
                            {
                                surveyInfo.HasChildren = v.s;
                            }
                            if (v.c.Trim() == "融资目的")
                            {
                                surveyInfo.FinancingPurpose = v.s;
                            }
                            if (v.c.Trim() == "使用情况")
                            {
                                surveyInfo.Usage = v.s;
                            }
                            if (v.c.Trim() == "拟贷金额")
                            {
                                if (string.IsNullOrEmpty(v.s))
                                {
                                    surveyInfo.PrepareLoanAmount = null;
                                }
                                else
                                {
                                    surveyInfo.PrepareLoanAmount = Convert.ToDecimal(v.s);
                                }
                            }
                        }

                        break;

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("40001", ex);
            }


        }

        /// <summary>
        /// 获取估价宝接口配置参数
        /// </summary>
        /// <returns></returns>
        private static dynamic GetGjbConfigs()
        {
            dynamic gjbObj = new ExpandoObject();
            gjbObj.appid = ConfigurationHelper.GjbAppId;
            gjbObj.apppwd = ConfigurationHelper.GjbAppWd;
            gjbObj.systypecode = ConfigurationHelper.GjbSysTypeCode;
            gjbObj.signname = ConfigurationHelper.GjbSignName;
            gjbObj.appkey = ConfigurationHelper.GjbAppKey;
            gjbObj.apiurl = ConfigurationHelper.GjbAppUrl;
            gjbObj.funname = ConfigurationHelper.GjbFunName;

            return gjbObj;
        }

        /// <summary>
        /// 获取自动估价接口配置参数
        /// </summary>
        /// <returns></returns>
        private static dynamic GetAutoPriceConfigs()
        {
            dynamic dcObj = new ExpandoObject();
            dcObj.appid = ConfigurationHelper.DcAppId;
            dcObj.apppwd = ConfigurationHelper.DcAppWd;
            dcObj.systypecode = ConfigurationHelper.DcSysTypeCode;
            dcObj.signname = ConfigurationHelper.DcSignName;
            dcObj.appkey = ConfigurationHelper.DcAppKey;
            dcObj.funname = ConfigurationHelper.DcFunName;
            dcObj.apiurl = ConfigurationHelper.DcAppUrl;

            return dcObj;
        }


        #region 帮助
        /// <summary>
        /// 估价宝和数据中心自动估价接口通用获取数据方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="funinfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static T GetReturnValue<T>(dynamic d, object funinfo, string type = null)
        {
            var posts = GetArgs(d, funinfo, type);
            var result = ApiPostBack(d.apiurl as string, posts, "application/json");
            var r = JsonHelper.JSONToObject<JsonHelper.ReturnData>(result);
            var values = JsonConvert.DeserializeObject<T>(r.data.ToString(), new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            });
            return values;
        }

        /// <summary>
        /// 构建估价宝和自动估价接口请求参数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="funinfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetArgs(dynamic d, object funinfo, string type = null)
        {
            var appid = d.appid as string;
            var apppwd = d.apppwd as string;
            var systypecode = d.systypecode as string;
            var signname = d.signname as string;
            var appkey = d.appkey as string;
            var functionname = d.funname as string;

            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            var code = EncryptHelper.GetMd5(pwdArray, appkey);

            var sinfo = JsonConvert.SerializeObject(new
            {
                appid,
                apppwd,
                signname,
                time,
                functionname,
                code
            });

            string info;

            if (string.IsNullOrEmpty(type))
            {
                string pwd = null;
                info = JsonConvert.SerializeObject(new
                {
                    appinfo =
                        new
                        {
                            splatype = "ie",
                            platver = "1.0",
                            stype = "ct",
                            version = "1.0",
                            vcode = "20",
                            systypecode,
                            channel = "360"
                        },
                    uinfo = new { username = "admin@gjb", password = pwd, fxtcompanyid = 365, subcompanyid = 0 },
                    funinfo
                });

            }
            else
            {
                info = JsonConvert.SerializeObject(new
                 {
                     appinfo =
                         new
                         {
                             splatype = "win",
                             stype = "gjb",
                             version = "1.0",
                             vcode = "1",
                             systypecode,
                             channel = "360"
                         },
                     uinfo = new { username = "admin@fxtpublic", token = "" },
                     funinfo
                 });
            }



            var args = new
            {
                sinfo,
                info
            };

            return JsonConvert.SerializeObject(args);


        }

        /// <summary>
        /// 自动估价返回实体类
        /// </summary>
        private class AutoPrice
        {
            public decimal? UnitPrice { get; set; }
        }


        //JAVA云查勘接口调用
        public static T GetSurveyData<T>(string url, dynamic obj) where T : new()
        {
            var token = Token.SurveryToken;
            if (string.IsNullOrEmpty(token))
            {
                NLogHelper.Info("token is Null or Empty");
                return new T();
            }

            var loginInfo = new SurveyApi.LoginInfoEntity
            {
                Username = "admin@gjb",
                Truename = "管理员",
                PwdToSurvey = SurveyApi.GetPassWordMd5("admin@#$^&!!"),
                Token = token,
                Fxtcompanyid = 365
            };

            var surveyapi = new SurveyApi(loginInfo);
            surveyapi.UrlApi += url;
            surveyapi.body = obj;
            var postData = surveyapi.GetJsonString();
            var result = ApiPostBack(surveyapi.UrlApi, postData, "application/json");
            var data = JsonConvert.DeserializeObject<SurveyApi.SurveyReturnData<T>>(result);
            if (data.code == 0)
            {
                return data.body;
            }

            //当本地缓存token与服务器token不一致时，清掉本地缓存token，重新获取服务器token
            if (data.code == 4002)
            {
                Token.ResetSurveryToken();
            }

            NLogHelper.Info(result);
            return new T();
        }

        /// <summary>
        /// HTTP 请求响应
        /// </summary>
        /// <param name="url"></param>
        /// <param name="post"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static string ApiPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            string result;
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JsonHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }

        #endregion

    }
}

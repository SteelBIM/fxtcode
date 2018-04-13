using CBSS.Tbx.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Tbx.Contract.DataModel;
using System.Linq.Expressions;
using CBSS.Core.Utility;
using CBSS.Framework.DAL;
using CBSS.Tbx.IBLL;
using CBSS.IBS.IBLL;
using CBSS.Framework.Contract.API;
using System.Reflection;
using CBSS.Framework.Redis;
using System.Collections;
using System.Web.Security;
using CBSS.Core.Cache;
using System.Configuration;
using CBSS.Core.Log;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Framework.Contract;
using CBSS.IBS.Contract.IBSResource;
using Omu.ValueInjecter;
using Newtonsoft.Json.Linq;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        public App GetApp(string id)
        {
            try
            {
                return repository.SelectSearch<App>(m => m.AppID == id).SingleOrDefault();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取状态启用的应用
        /// </summary>
        /// <returns></returns>
        public IEnumerable<App> GetAppListByStatus()
        {
            return repository.SelectSearch<App>(a => a.Status == 1);
        }

        public IEnumerable<App> GetAppList(out int totalcount, AppRequest request = null)
        {
            request = request ?? new AppRequest();

            List<Expression<Func<App, bool>>> exprlist = new List<Expression<Func<App, bool>>>();
            exprlist.Add(o => true);
            if (!string.IsNullOrEmpty(request.AppName))
                exprlist.Add(u => u.AppName.Contains(request.AppName.Trim()));

            PageParameter<App> pageParameter = new PageParameter<App>();
            pageParameter.Wheres = exprlist;
            pageParameter.PageIndex = request.PageIndex;
            pageParameter.PageSize = request.PageSize;
            pageParameter.OrderColumns = p => p.CreateDate;
            pageParameter.IsOrderByASC = 0;
            totalcount = 0;
            return repository.SelectPage<App>(pageParameter, out totalcount);
        }
        /// <summary>
        /// 保存应用
        /// </summary>
        /// <param name="model"></param>
        public int SaveApp(App model)
        {
            if (!string.IsNullOrEmpty(model.AppID))
            {
                //验证重名
                var list = repository.SelectSearch<App>(a => a.AppName == model.AppName && a.AppID != model.AppID);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                return repository.Update<App>(model) ? 1 : 0;
            }
            else
            {
                //验证重名
                var list = repository.SelectSearch<App>(a => a.AppName == model.AppName);
                if (list != null && list.Count() > 0)
                {
                    return 2;
                }
                model.AppID = Guid.NewGuid().ToString();
                var obj = repository.InsertReturnEntity<App>(model);//.Insert<App>(model);
                return obj != null ? 1 : 0;
            }
        }
        /// <summary>
        /// 根据AppID删除应用(0：有对应版本不能删除，1：删除成功，2：删除失败)
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        public int DelAppByAppID(string AppID)
        {
            Guid guidAppID = Guid.Parse(AppID);
            IEnumerable<AppVersion> listAppVersion = repository.SelectSearch<AppVersion>(a => a.AppID == AppID);
            IEnumerable<AppSkinVersion> listAppSkinVersion = repository.SelectSearch<AppSkinVersion>(a => a.AppID == AppID);
            IEnumerable<AppBookCatalogModuleItem> listAppBookCatalogModuleItem = repository.SelectSearch<AppBookCatalogModuleItem>(a => a.AppID == guidAppID);
            IEnumerable<AppGoodItem> listAppGoodItem = repository.SelectSearch<AppGoodItem>(a => a.AppID == AppID);
            IEnumerable<AppMarketBook> listAppMarketBook = repository.SelectSearch<AppMarketBook>(a => a.AppID == AppID);
            IEnumerable<AppMarketClassify> listAppMarketClassify = repository.SelectSearch<AppMarketClassify>(a => a.AppID == AppID);
            if ((listAppVersion != null && listAppVersion.Count() > 0) || (listAppSkinVersion != null && listAppSkinVersion.Count() > 0) || (listAppBookCatalogModuleItem != null && listAppBookCatalogModuleItem.Count() > 0) || (listAppGoodItem != null && listAppGoodItem.Count() > 0) || (listAppMarketBook != null && listAppMarketBook.Count() > 0) || (listAppMarketClassify != null && listAppMarketClassify.Count() > 0))
            {
                return 0;
            }
            else
            {
                return repository.Delete<App>(AppID) ? 1 : 2;
            }
        }

        public bool DeleteApp(List<int> ids)
        {
            return repository.DeleteMore<App>(ids.Select(a => a.ToString()).ToArray());
        }
        public bool DelAppGoodItem(int AppGoodItemID)
        {
            return repository.Delete<AppGoodItem>(AppGoodItemID);
        }
        public void SaveAppGoodItem(AppGoodItem model)
        {
            if (model.AppGoodItemID > 0)
            {
                repository.Update<AppGoodItem>(model);
            }
            else
            {
                repository.Insert<AppGoodItem>(model);
            }
        }
        public IEnumerable<AppGoodItem> GetAppGoodItemList(Expression<Func<AppGoodItem, bool>> expression)
        {
            return repository.SelectSearch<AppGoodItem>(expression);
        }
        /// <summary>
        /// 接口参数验证,非空非null验证
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public APIResponse VerifyParam<T>(string inputStr, out T input, List<string> ignoreParams = null)
        {
            if (ignoreParams == null)
            {
                ignoreParams = new List<string>();
            }
            ignoreParams.Add("PKey");
            input = JsonConvertHelper.ToObject<T>(inputStr);
            Type t = input.GetType();
            var pros = t.GetProperties();
            for (int i = 0; i < pros.Length; i++)
            {
                string proName = pros[i].Name;
                object o = pros[i].GetValue(input, null);
                string v = Convert.ToString(o);
                if (!ignoreParams.Contains(proName))
                {
                    if (string.IsNullOrEmpty(v))
                    {                     
                        return APIResponse.GetErrorResponse(ErrorCodeEnum.请求参数不能为空,"参数名:"+ pros[i].Name);
                        
                    }
                }
            }
            return APIResponse.GetResponse();
        }
        
        /// <summary>
        /// 手机发送验证码
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public APIResponse SendTelephoneCode(string telephone)
        {
            var code = redis.Get<Tb_PhoneCode>("UserPhoneCode", telephone);

            if (code != null)
            {
                if (code.EndDate > DateTime.Now)
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请使用五分钟内获取的验证码登录);
                }
            }

            Tb_PhoneCode phoneCode = new Tb_PhoneCode();
            phoneCode.Code = CommonHelper.RndNum(6);
            phoneCode.EndDate = DateTime.Now.AddMinutes(5);
            phoneCode.TelePhone = telephone;

            redis.Set<Tb_PhoneCode>("UserPhoneCode", telephone, phoneCode);
            //redis.SetExpire("UserPhoneCode", new TimeSpan(0, 5, 0));
            string messageContent = "您的短信验证码为：" + phoneCode.Code + ",有效时间为5分钟，如非本人操作,请忽略本短信.";

            if (SendMessage(messageContent, telephone))
            {
                return APIResponse.GetResponse();
            }
            else
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.验证码发送失败);
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public APIResponse DecidePhoneCode(string telephone, string code)
        {
            var redisCode = redis.Get<Tb_PhoneCode>("UserPhoneCode", telephone);

            if (redisCode == null)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.请输入有效的验证码);
            }
            else
            {
                if (redisCode.Code != code)
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.请输入有效的验证码);
                }
            }
            redis.Remove("UserPhoneCode", telephone);
            return APIResponse.GetResponse();
        }

        /// <summary>
        /// 手机发送信息
        /// </summary>
        /// <param name="messageContent"></param>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public bool SendMessage(string messageContent, string telephone)
        {
            string messageToken = XMLHelper.GetAppSetting("MessageToken");
            SMSService.SMSService smssmessage = new SMSService.SMSService();
            string results = smssmessage.SendMessage(messageToken, telephone, messageContent);
            string[] resultArr = results.Split(new char[] { ',' });
            if (resultArr[0] == "0" || resultArr[0] == "200")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否包含字符串 如果为空 或者包含 返回true
        /// </summary>
        /// <param name="p_StringName"></param>
        /// <returns></returns>
        public bool ContainsBadChar(string p_StringName)
        {
            //如果字符串为NULLor空则返回空字符串  
            bool success = false;
            if (p_StringName.Length > 6 && p_StringName.Length < 2)
            {
                return true;
            }

            if (string.IsNullOrEmpty(p_StringName))
            {
                return true;
            }

            string _StringBadChar, _TempChar;
            string[] _ArraryBadChar;
            _StringBadChar = "／,：,；,（,）,¥,「,」,＂,、,[,],{,},#,%,-,*,+,=,\\,|,~,＜,＞,$,€,"
                + "^,•,',#,$,%,^,&,*,(,),+,',\" ,/,;,',{,},+,=,-,`,（,）,……,%,￥,#,！,~,《,》,？,，,。,、,；,‘,【,】,+,—,—,";

            _ArraryBadChar = _StringBadChar.Split(',');
            _TempChar = p_StringName;

            for (int i = 0; i < _ArraryBadChar.Length; i++)
            {
                if (_ArraryBadChar[i].Length > 0)
                {
                    success = _TempChar.Contains(_ArraryBadChar[i]);
                    if (success)
                    {
                        return success;
                    }
                }
            }
            return success;

        }

        public List<AppVersion> GetVersion(string appID, string versionNumber, int appType, out string errMsg)
        {
            errMsg = "";
            List<AppVersion> reAppVersions = new List<AppVersion>();
            var appVersions = repository.SelectSearch<AppVersion>(x => x.AppID == appID && x.Status == 1 && x.AppType == appType, "CreateDate desc");
            if (appVersions.Count > 0)
            {
                var appVersion = appVersions.First();

                System.Version v1 = new System.Version(appVersion.AppVersionNumber);//.Remove(0, 1)
                if (v1 != null)
                {
                    string version = "0.0.0";
                    version = versionNumber;

                    System.Version v2 = new System.Version(version);//.Remove(0, 1)
                    if (v1 == v2 || v1 < v2)
                    {
                        errMsg = "当前版本已是最新！";
                    }
                    else
                    {
                        foreach (var item in appVersions)
                        {
                            reAppVersions.Add(item);
                            if (item.AppVersionUpdateType == "1")
                            {
                                return reAppVersions;
                            }
                        }
                    }
                }
            }
            else
            {
                errMsg = "系统不存该版本";
            }
            return null;
        }

        /// <summary>
        /// 根据userid 往redis添加对应版本
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SetValidUserRecord(string UserId)
        {

            if (string.IsNullOrEmpty(UserId))
            {
                return false;
            }
            string value = redis.Get("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId);//从Redis读取值
            if (string.IsNullOrEmpty(value))
            {
                redis.Set("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId, DateTime.Now.ToShortDateString());//写入Redis
                return true;
            }
            else
            {
                if (Convert.ToDateTime(value).ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    redis.Set("ValidUserRecord_Day" + DateTime.Now.ToShortDateString(), UserId, DateTime.Now.ToShortDateString());//写入Redis
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 登陆埋点，保存
        /// </summary>
        /// <returns></returns>
        public void SetUserLoginRecord(Rds_UserLoginRecord loginRecord)
        {
            var value = redis.Get<Rds_UserLoginRecords>("Rds_UserLoginRecords", loginRecord.UserID.ToString());//从Redis读取值
            if (value != null)
            {
                if (value.records.FirstOrDefault(a => a.Status == 2) == null)
                {
                    loginRecord.Status = 2;//第一次登陆
                }
            }
            else
            {
                loginRecord.Status = 2;
                value = new Rds_UserLoginRecords();
                value.records = new List<Rds_UserLoginRecord>();
            }
            value.records.Add(loginRecord);
            redis.Set("Rds_UserLoginRecords", loginRecord.UserID.ToString(), value);//写入Redis
        }

        /// <summary>
        /// App使用时间埋点
        /// </summary>
        /// <returns></returns>
        public bool SetUseAppRecord(Rds_UseAppRecord useAppRecord)
        {
            var value = redis.Get<Rds_UseAppRecords>("Rds_UseAppRecords", useAppRecord.UserID.ToString());//从Redis读取值
            if (value == null)
            {
                value = new Rds_UseAppRecords();
                value.records = new List<Rds_UseAppRecord>();
            }
            value.records.Add(useAppRecord);
            return redis.Set("Rds_UseAppRecords", useAppRecord.UserID.ToString(), value);//写入Redis
        }

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        public ArrayList QueryCombo(string UserID)
        {

            ArrayList list = new ArrayList();
            try
            {
                #region 获取同步课堂权限    

                TBService.WebServicePatch wp = new TBService.WebServicePatch();
                var tlist = wp.LoadSyncClassBind(UserID);
                if (tlist != null)
                {
                    foreach (var item in tlist)
                    {
                        list.Add(new
                        {
                            UserID = UserID,
                            CourseID = item.BookID ?? "",
                            EndDate = (DateTime.Now.AddMonths(12).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                            Months = 1
                        });
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, "QueryCombo获取同步课堂权限出错，UserID=" + UserID, ex);
            }
            finally
            {
                var umList = repository.SelectSearch<TB_UserMember>(x => x.UserID == UserID, "EndDate desc");
                if (umList != null)
                {
                    Dictionary<string, long> dir = new Dictionary<string, long>();

                    for (int i = 0; i < umList.Count; i++)
                    {
                        string key = umList[i].CourseID + "@" + umList[i].Months;
                        if (!dir.Keys.Contains<string>(key))
                            if (umList[i].EndDate.Value > DateTime.Now)
                            {
                                dir.Add(key, (umList[i].EndDate.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
                            }
                    }

                    foreach (var item in dir)
                    {

                        list.Add(new
                        {
                            UserID = UserID,
                            CourseID = item.Key.Split('@')[0],
                            EndDate = item.Value,
                            Months = item.Key.Split('@')[1]
                        });

                    }
                }
            }

            if (list.Count == 0) return null;
            return list;
        }

        public string AddToken(string userId, string equipmentID)
        {
            string tk = PswToSecurity(userId + "|" + equipmentID);
            string tokenname = XMLHelper.GetAppSetting("tokename");
            var redisTokenName = redis.Get(tokenname, userId);
            if (redisTokenName != null)
            {
                redis.Remove(tokenname, userId);
            }
            if (redis.Set(tokenname, userId, tk + "|" + DateTime.Now.ToString()))
                return tk;
            return "";
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strPsw">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public string PswToSecurity(string strPsw)
        {
            if (!string.IsNullOrEmpty(strPsw) && strPsw.Length != 0)
            {
#pragma warning disable CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
                return FormsAuthentication.HashPasswordForStoringInConfigFile(strPsw, "MD5");
#pragma warning restore CS0618 // “FormsAuthentication.HashPasswordForStoringInConfigFile(string, string)”已过时:“The recommended alternative is to use the Membership APIs, such as Membership.CreateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.”
            }
            else
            {
                return string.Empty;
            }
        }

        public void RemoveOnlineUser(string userid)
        {
            string cacheKey = XMLHelper.GetAppSetting("cacheKey");
            Hashtable OnlineUserList = (Hashtable)CacheHelper.Get(cacheKey);
            if (OnlineUserList != null)
            {
                OnlineUserList.Remove(userid);
            }
        }

        void DeepCopy<T, V>(T des, V source)
        {
            var desType = des.GetType();
            var sourceType = source.GetType();
            des.InjectFrom(source);

            var desMembers = desType.GetProperties();
            var sourceMembers = sourceType.GetProperties();
            foreach (var m in desMembers)
            {
                if (m.PropertyType.IsClass && m.PropertyType != typeof(string))
                {
                    //m.GetValue(des).InjectFrom(m.GetValue(source));
                    var n = sourceMembers.FirstOrDefault(o => o.Name == m.Name);
                    if (n != null)
                    {

                        if (m.GetValue(des) != null && n.GetValue(source) != null)
                        {
                            DeepCopy(m.GetValue(des), n.GetValue(source));
                        }
                    }

                }
            }
        }
        public APIResponse GetStuTopicSetAnswer(Rds_UserTopicSetAnswerModel model)
        {
            try
            {
                var stuAnswer = redis.Get<Rds_UserTopicSetAnswerModel>(typeof(Rds_UserTopicSetAnswerModel).Name + "_" + model.studentId.ToString().Substring(0, 2), model.studentId + "_" + model.id);
                var paper = ibsService.GetTopicSetById(model.id);

                var paperAndAnswer = (paper.Data as TopicSetsReponseById).topicSet.ToJson().ToObject<UserTopicSetAnswerModel>();
                //  DeepCopy<UserTopicSetAnswerModel, Rds_UserTopicSetAnswerModel>(paperAndAnswer, stuAnswer);

                if (paperAndAnswer != null && stuAnswer != null)
                {
                    //var jObj1 = JObject.FromObject(paperAndAnswer);
                    //if (stuAnswer != null)
                    //{
                    //    var jObj2 = JObject.FromObject(stuAnswer);
                    //    jObj1.Merge(jObj2);
                    //}
                    paperAndAnswer.InjectFrom(stuAnswer);
                    paperAndAnswer.imitationReading.InjectFrom(stuAnswer.imitationReading);
                    paperAndAnswer.infoAcq.InjectFrom(stuAnswer.infoAcq);
                    for (int i = 0; i < paperAndAnswer.infoAcq.infoAcqSections.Count(); i++)
                    {
                        paperAndAnswer.infoAcq.infoAcqSections[i].InjectFrom(stuAnswer.infoAcq.infoAcqSections[i]);
                        for (int j = 0; j < paperAndAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains.Count(); j++)
                        {
                            paperAndAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains[j].InjectFrom(stuAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains[j]);
                            for (int n = 0; n < paperAndAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains[j].infoAcqItems.Count(); n++)
                            {
                                paperAndAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains[j].infoAcqItems[n].InjectFrom(stuAnswer.infoAcq.infoAcqSections[i].infoAcqSectionMains[j].infoAcqItems[n]);
                            }
                        }
                    }
                    paperAndAnswer.infoRepostAndQuery.InjectFrom(stuAnswer.infoRepostAndQuery);
                    paperAndAnswer.infoRepostAndQuery.infoRepost.InjectFrom(stuAnswer.infoRepostAndQuery.infoRepost);
                    paperAndAnswer.infoRepostAndQuery.infoQuery.InjectFrom(stuAnswer.infoRepostAndQuery.infoQuery);
                    for (int m = 0; m < paperAndAnswer.infoRepostAndQuery.infoQuery.infoQueryItems.Count(); m++)
                    {
                        paperAndAnswer.infoRepostAndQuery.infoQuery.infoQueryItems[m].InjectFrom(stuAnswer.infoRepostAndQuery.infoQuery.infoQueryItems[m]);
                    }


                }
                return APIResponse.GetResponse(paperAndAnswer);
                //  paperAndAnswer = JObj2.ToObject<UserTopicSetAnswerModel>();


            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败, Framework.Contract.Enums.LogLevelEnum.Error, ex);
            }

#pragma warning disable CS0162 // 检测到无法访问的代码
            return APIResponse.GetResponse("");
#pragma warning restore CS0162 // 检测到无法访问的代码
        }
        public APIResponse SubmitTopicSetAnswer(Rds_UserTopicSetAnswerModel answer)
        {
            try
            {
                var oldAnswer = redis.Get<Rds_UserTopicSetAnswerModel>(typeof(Rds_UserTopicSetAnswerModel).Name + "_" + answer.studentId.ToString().Substring(0, 2), answer.studentId + "_" + answer.id);

                var oldScore = oldAnswer == null ? 0 : oldAnswer.stuScore;//旧分数

                if (oldScore <= answer.stuScore)
                {
                    redis.Set<Rds_UserTopicSetAnswerModel>(typeof(Rds_UserTopicSetAnswerModel).Name + "_" + answer.studentId.ToString().Substring(0, 2), answer.studentId + "_" + answer.id, answer);
                    redisList.LPush("UserSpokenPaperRecord", answer.ToJson());
                }
            }
            catch (Exception ex)
            {
                return APIResponse.GetErrorResponse(ErrorCodeEnum.操作失败, Framework.Contract.Enums.LogLevelEnum.Error, ex);
            }
            return APIResponse.GetResponse("");
        }
        public APIResponse AddReport<T>(T report, string hashID, string key, string proName, TypeOfReport type)
        {
            Type t = report.GetType();
            //var proFrequency = t.GetProperty("Frequency");

            var redisReport = redis.Get<T>(hashID + "_" + key.Substring(0, 2), key);
            if (redisReport == null)
            {
                //proFrequency.SetValue(report, 1);
                redis.Set<T>(hashID + "_" + key.Substring(0, 2), key, report);
                KeyAndType keyAndType = new KeyAndType
                {
                    key = key,
                    Type = type
                };
                redisList.LPush("StudyReport", keyAndType.ToJson());
            }
            else
            {

                var pro = t.GetProperty(proName);

                object reportObject = pro.GetValue(report, null);
                int reportValue = Convert.ToInt32(reportObject);

                object redisReportObject = pro.GetValue(redisReport, null);
                int redisReportValue = Convert.ToInt32(redisReportObject);

                if (redisReportValue <= reportValue)
                {
                    //object objFrequency = proFrequency.GetValue(report, null);
                    //int intFrequency = Convert.ToInt32(objFrequency);
                    //proFrequency.SetValue(report, intFrequency + 1);

                    redis.Set<T>(hashID + "_" + key.Substring(0, 2), key, report);
                    KeyAndType keyAndType = new KeyAndType
                    {
                        key = key,
                        Type = type
                    };
                    redisList.LPush("StudyReport", keyAndType.ToJson());
                }
            }
            return APIResponse.GetResponse();
        }
        public APIResponse AddArticleReport(Rds_UserArticleReadRecord report, string hashID, string key, string proName, TypeOfReport type)
        {
            Type t = report.GetType();
            //var proFrequency = t.GetProperty("Frequency");

            var redisReports = redis.Get<List<Rds_UserArticleReadRecord>>(hashID + "_" + key.Substring(0, 2), key);
            if (redisReports == null)
            {
                //proFrequency.SetValue(report, 1);
                redis.Set<List<Rds_UserArticleReadRecord>>(hashID + "_" + key.Substring(0, 2), key, new List<Rds_UserArticleReadRecord> { report });
                KeyAndType keyAndType = new KeyAndType
                {
                    key = key,
                    Type = type
                };
                redisList.LPush("StudyReport", keyAndType.ToJson());
            }
            else
            {
                var rdsArticleReport = redisReports.FirstOrDefault(o => o.Sort == report.Sort);

                if (rdsArticleReport == null)
                {
                    redisReports.Add(report);
                    redis.Set<List<Rds_UserArticleReadRecord>>(hashID + "_" + key.Substring(0, 2), key, redisReports);
                    KeyAndType keyAndType = new KeyAndType
                    {
                        key = key,
                        Type = type
                    };
                    redisList.LPush("StudyReport", keyAndType.ToJson());
                }
                else
                {
                    if (rdsArticleReport.AvgScore <= report.AvgScore)
                    {
                        //object objFrequency = proFrequency.GetValue(report, null);
                        //int intFrequency = Convert.ToInt32(objFrequency);
                        //proFrequency.SetValue(report, intFrequency + 1);
                        rdsArticleReport.InjectFrom(report);
                        rdsArticleReport.Sentences = report.Sentences;
                        redis.Set<List<Rds_UserArticleReadRecord>>(hashID + "_" + key.Substring(0, 2), key, redisReports);
                        KeyAndType keyAndType = new KeyAndType
                        {
                            key = key,
                            Type = type
                        };
                        redisList.LPush("StudyReport", keyAndType.ToJson());
                    }
                }
            }
            return APIResponse.GetResponse();
        }
        public APIResponse ShareReport<T>(T report, string hashID, string key)
        {
            redis.Set<T>(hashID + "_" + key.Substring(0, 1), key, report);
            string shareHttp = XMLHelper.GetAppSetting("shareHttp");
            return APIResponse.GetResponse(shareHttp + key);
        }
        public T GetShareReport<T>(string key)
        {
            return redis.Get<T>(typeof(T).Name + "_" + key[0], key);
        }
        public APIResponse GetWordsDictationShare(string hashID, string key)
        {
            var share = redis.Get<Rds_UserWordDictationRecord>(hashID, key);
            return APIResponse.GetResponse(share);
        }

        public APIResponse GetReport<RT, RK, T, K>(int userID, int catalogID, int moduleID, string hashID, string recordName, string reportName)
            where T : class, new()
            where K : class, new()
            where RT : class, new()
            where RK : class, new()
        {
            string key = userID + "_" + catalogID + "_" + moduleID;
            var redisReport = redis.Get<RT>(hashID + "_" + key.Substring(0, 2), key);

            if (redisReport == null)
            {
                //查询父表，报告表
                string reportWhereSql = string.Format("UserID={0} and CatalogID={1} and ModuleID={2}", userID, catalogID, moduleID);
                var reports = tbxRecordRepository.SelectSearch<T>(reportWhereSql);
                if (reports.Count() == 0)
                {
                    return APIResponse.GetErrorResponse(ErrorCodeEnum.未找到报告);
                }
                var report = reports.First();

                //获取父表的ReportID
                Type t = typeof(T);
                var pro = t.GetProperty(reportName);
                object reportObject = pro.GetValue(report, null);
                Guid reportValue = (Guid)reportObject;

                //根据ReportID查询子表，记录表
                string recordSql = string.Format(reportName + "='{0}'", reportValue);
                var records = repository.SelectSearch<K>(recordSql);

                //转换子表，数据库实体到Redis实体
                List<RK> rAddRecods = new List<RK>();
                foreach (var item in records)
                {
                    var rRecord = item.ToJson().ToObject<RK>();
                    rAddRecods.Add(rRecord);
                }

                //转换父表，数据库实体到Redis实体，并给记录属性赋值
                var rReport = report.ToJson().ToObject<RT>();

                Type et = typeof(RT);
                var recordPro = et.GetProperty(recordName);
                recordPro.SetValue(rReport, rAddRecods);

                //保存记录到Redis中
                redis.Set<RT>(hashID + "_" + key.Substring(0, 2), key, rReport);

                //返回数据
                return APIResponse.GetResponse(rReport);
            }
            else
            {
                return APIResponse.GetResponse(redisReport);
            }
        }
    }


}

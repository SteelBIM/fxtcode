using CDI.Common;
using CDI.Models;
using CDI.Service.Dao;
using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using XCode;

namespace CDI.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CDIService : ICDIService
    {
        ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
        static readonly string productCode = "1003064";//案例数据导入工具code

        public string Echo(string msg)
        {
            string usedProtocol = OperationContext.Current.Channel.LocalAddress.Uri.Scheme;
            string rtv = string.Format("You entered: [{0}], and you used protocol {1}", msg, usedProtocol);
            logger.Debug(rtv);
            return rtv;
        }

        public byte[] ValidateAddress(byte[] addr)
        {
            return RunAction<ResponseModel>((response) =>
            {
                var result = BinarySerialization.ReadFromBytes<AddressRequestModel>(addr);
                var mac = MacAddress.Find(new string[] { MacAddress._.Mac, MacAddress._.ProductCode }, new object[] { result.MacAddress, productCode });
                response.Status = mac != null ? 1 : 0;
            });
        }

        public byte[] Login(byte[] bUser)
        {
            return RunAction<LoginResponseModel>((response) =>
            {
                var userModel = BinarySerialization.ReadFromBytes<LoginRequestModel>(bUser);
                //check address
                var mac = MacAddress.Find(new string[] { MacAddress._.Mac, MacAddress._.ProductCode }, new object[] { userModel.MacAddress, productCode });
                if (mac == null)
                {
                    response.Status = 0;
                    response.Message = "登录失败:非法访问!";
                    return;
                }
                //check user name
                var dbUser = DBUserList.Find(new string[] { DBUserList._.UserName, DBUserList._.ProductCode }, new object[] { userModel.UserName, productCode });
                if (dbUser == null)
                {
                    response.Status = 0;
                    response.Message = "登录失败:当前用户无权限查询数据库!";
                    return;
                }

                //check account and pwd
                Exception ex = null;
                bool result = FxtUserCenterService_GetUser(userModel.UserName, userModel.Password, (int)CDI.Common.EnumHelper.Codes.SysTypeCodeDataCenter, out ex);
                if (result)
                {
                    //用户登录信息写入DB
                    var userInfo = AddUserInfo(userModel.UserName);
                    response.Status = 1;
                    response.Token = userInfo.Token;
                }
                else
                {
                    if (ex == null)
                    {
                        response.Status = 0;
                        response.Message = "登录失败";
                    }
                    else
                    {
                        response.Status = -1;
                        response.Message = "登录失败:" + ex.Message;
                    }
                }
            });
        }

        public byte[] Logout(byte[] userToken)
        {
            return RunAction<ResponseModel>(() =>
            {
                var user = BinarySerialization.ReadFromBytes<TokenRequestModel>(userToken);
                var userlist = UserLoginState.FindAllByToken(user.Token);
                if (userlist.Count > 0)
                {
                    int r = userlist.Delete();
                    if (r != userlist.Count)
                    {
                        throw new Exception("删除用户登录状态记录失败");
                    }
                }
            });
        }

        #region 登录辅助方法

        private UserLoginState AddUserInfo(string userName)
        {
            var userlist = UserLoginState.FindAllByUserName(userName);
            if (userlist.Count > 0)
            {
                DateTime expiredDate = DateTime.Now;
                var expiredUser = from UserLoginState u in userlist where u.ExpireDate < expiredDate select u;
                if (expiredUser.Count() > 0)
                {
                    EntityList<UserLoginState> tempList = new EntityList<UserLoginState>(expiredUser);
                    int count = tempList.Delete();
                }
            }

            UserLoginState userInfo = new UserLoginState();
            userInfo.Token = Guid.NewGuid().ToString("d");
            userInfo.UserName = userName;
            userInfo.LoginDate = DateTime.Now;
            userInfo.ExpireDate = userInfo.LoginDate.AddHours(ConfigurationHelper.SessionTimeout);
            userInfo.CreateBy = userInfo.UserName;
            userInfo.CreateDT = userInfo.LoginDate;
            userInfo.ProductCode = productCode;
            int r = userInfo.Insert();
            if (r != 1)
            {
                throw new Exception("用户登录信息写入失败");
            }

            return userInfo;
        }

        /// <summary>
        /// 中心服务器检查用户的接口
        /// </summary>
        private bool FxtUserCenterService_GetUser(string userName, string password, int productCode, out Exception ex)
        {
            //web.config设置中心用户API的地址
            string api = ConfigurationManager.AppSettings["fxtusercenterloginservice"];
            ex = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var time = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var sinfo = new { time, code = CDI.Common.StringHelper.GetLoginCodeMd5(time) };
                    var info = new
                    {
                        uinfo = new { username = userName, password = CDI.Common.StringHelper.GetPassWordMd5(password) },
                        appinfo = new ApplicationInfo(productCode.ToString())
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        return true;
                    }
                    else
                    {
                        ex = new Exception(rtn.returntext.ToString());
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            return false;
        }
        /// <summary>
        /// 用户中心数据提交回调
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="post">提交数据(json)</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        private string APIPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            var result = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CertificateValidationCallbackResult);
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }

        private bool CertificateValidationCallbackResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受  
            return true;
        }

        #endregion

        #region Common Action

        private byte[] RunAction<T>(Action handleAction) where T : ResponseModel, new()
        {
            T response = new T();
            try
            {
                handleAction();
                response.Status = 1;
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Message = ex.Message;
                logger.Error(ex);
            }
            return BinarySerialization.WriteToBytes(response);
        }

        private byte[] RunAction<T>(Action<T> handleAction) where T : ResponseModel, new()
        {
            T response = new T();
            try
            {
                handleAction(response);
            }
            catch (Exception ex)
            {
                response.Status = -1;
                response.Message = ex.Message;
                logger.Error(ex);
            }
            return BinarySerialization.WriteToBytes(response);
        }

        private bool ValidateUserToken(ResponseModel response, byte[] userToken, out string userName)
        {
            userName = null;
            var user = BinarySerialization.ReadFromBytes<TokenRequestModel>(userToken);
            var userlist = UserLoginState.FindAllByToken(user.Token);
            if (userlist.Count == 0)
            {
                response.Status = -2;
                response.Message = "用户未登录";
                return false;
            }
            if (userlist[0].ExpireDate < DateTime.Now)
            {
                response.Status = -3;
                response.Message = "用户登录已过期,请重新登录";
                userlist[0].Delete();
                return false;
            }
            userName = userlist[0].UserName;
            if (userlist.Count > 1)
            {
                userlist.RemoveAt(0);
                userlist.Delete();
            }
            return true;
        }

        #endregion


        public byte[] QueryCityInfoList(byte[] userToken)
        {
            return RunAction<CityResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.Citys = service.QueryCityInfoList();
                response.Status = 1;
            });
        }

        public byte[] QueryAreaInfoMap(byte[] userToken, byte[] args)
        {
            return RunAction<AreaResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                IntRequestModel req = BinarySerialization.ReadFromBytes<IntRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.Areas = service.QueryAreaInfoMap(req.Number);
                response.Status = 1;
            });
        }

        public byte[] QueryPurposeInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryPurposeInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryFrontInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryFrontInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryBuildingTypeInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryBuildingTypeInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryHouseTypeInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryHouseTypeInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryStructureInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryStructureInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryFitmentInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryFitmentInfoMap();
                response.Status = 1;
            });
        }

        public byte[] QueryMoneyUnitInfoMap(byte[] userToken)
        {
            return RunAction<SysCodeResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ServiceFacade service = new ServiceFacade();
                response.SysCodes = service.QueryMoneyUnitInfoMap();
                response.Status = 1;
            });
        }

        public byte[] BatchInsertDataCase(byte[] userToken, byte[] args)
        {
            return RunAction<BatchInsertResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                BatchInsertRequestModel req = BinarySerialization.ReadFromBytes<BatchInsertRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.Count = service.BatchInsertDataCase(req.DC, req.TableName);
                response.Status = 1;
            });
        }

        public byte[] QueryDataProjectList(byte[] userToken, byte[] args)
        {
            return RunAction<DataProjectResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                DataProjectRequestModel req = BinarySerialization.ReadFromBytes<DataProjectRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.DataProjects = service.QueryDataProjectList(req.CityID, req.AreaID, req.TableName);
                response.Status = 1;
            });
        }

        public byte[] PagingQueryProjectList(byte[] userToken, byte[] args)
        {
            return RunAction<DataProjectResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                ProjectPageRequestModel req = BinarySerialization.ReadFromBytes<ProjectPageRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.DataProjects = service.PagingQueryProjectList(req.CityID, req.AreaID, req.TableName, req.PageIndex);
                response.Status = 1;
            });
        }

        public byte[] GetNetworkNames(byte[] userToken, byte[] args)
        {
            return RunAction<ProjectNameResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                var req = BinarySerialization.ReadFromBytes<ProjectNameRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.NetworkNames = service.GetNetworkNames(req.CityID, req.PageNumber, req.PageSize);
                response.Status = 1;
            });
        }

        public byte[] PagingQueryCityList(byte[] userToken, byte[] args)
        {
            return RunAction<CityResponseModel>((response) =>
            {
                string userName = null;
                if (!ValidateUserToken(response, userToken, out userName))
                {
                    return;
                }
                IntRequestModel req = BinarySerialization.ReadFromBytes<IntRequestModel>(args);
                ServiceFacade service = new ServiceFacade();
                response.Citys = service.PagingQueryCityList(req.Number);
                response.Status = 1;
            });
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.AliPay.Util;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Pay.lib;


namespace Kingsun.SynchronousStudy.App.Controllers
{
    /// <summary>
    /// 华为支付
    /// </summary>
    public class HMSPayController : ApiController
    {
        BaseManagement bm = new BaseManagement();
        private RedisListHelper listredis = new RedisListHelper();

        private readonly string _bj = ConfigurationManager.ConnectionStrings["BJDBConnectionStr"].ConnectionString;
        private readonly string _sz = ConfigurationManager.ConnectionStrings["SZDBConnectionStr"].ConnectionString;
        private readonly string _ot = ConfigurationManager.ConnectionStrings["OTDBConnectionStr"].ConnectionString;
        private readonly string _rjpep = ConfigurationManager.ConnectionStrings["RJPEPDBConnectionStr"].ConnectionString;
        private readonly string _rjyx = ConfigurationManager.ConnectionStrings["RJYXDBConnectionStr"].ConnectionString;

        private static readonly string _hwCpid = WebConfigurationManager.AppSettings["hw_CPID"];
        private static readonly string _hwAppid = WebConfigurationManager.AppSettings["hw_APPID"];
        private static readonly string _hwMerchantName = WebConfigurationManager.AppSettings["hw_merchantName"];
        private static readonly string _hwPrivateKey = WebConfigurationManager.AppSettings["hw_private_key"];
        private static readonly string _hwPublicKey = WebConfigurationManager.AppSettings["hw_public_key"];


        /// <summary>
        /// 发起支付,获取支付所需参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse PostOrderID([FromBody]KingRequest request)
        {
            PostOrderID submitData = JsonHelper.DecodeJson<PostOrderID>(request.Data);
            #region 校验相应的数据有效性
            Log4Net.LogHelper.Info("支付开始，TotalMoney=" + submitData.TotalMoney);
            Guid FID;
            if (!Guid.TryParse(submitData.FeeComboID, out FID))
            {
                return GetErrorResult("套餐编不正确");
            }

            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return GetErrorResult("用户编号不能为空");
            }

            int? IsDiscount = 0;
            TB_FeeCombo feecombo = bm.Select<TB_FeeCombo>(FID);
            if (feecombo == null)
            {
                return GetErrorResult("找不到套餐数据");
            }
            else
            {
                if (submitData.TotalMoney < feecombo.FeePrice)
                {
                    IsDiscount = 1;
                }
            }

            #endregion

            #region 订单生成
            TB_Order order = new TB_Order();
            order.ID = Guid.NewGuid();
            order.CreateDate = DateTime.Now;
            order.TotalMoney = submitData.TotalMoney;
            order.OrderID = WxPayApi.GenerateOutTradeNo();
            order.UserID = submitData.UserID;
            order.State = "0000";
            order.PayWay = "华为支付";
            order.CourseID = submitData.CourseID;
            order.FeeComboID = FID;
            order.IsDiscount = IsDiscount ?? 0;
            order.SourceType = submitData.channel == 301 ? 1 : 0;

            try
            {
                //301:优学 1：同步学
                if (submitData.channel == 301)
                {
                    string sqlstr = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType)
                                                                                  VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}'  )",
                                                                           order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType);
                    int s = SqlHelper.ExecuteNonQuery(_rjyx, CommandType.Text, sqlstr);
                    if (s <= 0)
                    {
                        Log4Net.LogHelper.Error("订单插入失败，order=" + JsonHelper.EncodeJson(order) + "APPID:" + submitData.AppID);
                        return GetErrorResult("订单插入失败1");
                    }
                }

                string sql = string.Format(@"INSERT INTO dbo.TB_Order ( ID ,OrderID ,TotalMoney ,CreateDate ,State ,UserID ,PayWay ,FeeComboID ,CourseID ,IsDiscount ,SourceType)
                                                              VALUES  ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}' , '{6}' , '{7}' , '{8}' , '{9}' , '{10}'  )",
                                                                       order.ID, order.OrderID, order.TotalMoney, order.CreateDate, order.State, order.UserID, order.PayWay, order.FeeComboID, order.CourseID, order.IsDiscount, order.SourceType);
                var i = OrderSql(submitData.AppID, sql);
                if (i <= 0)
                {
                    Log4Net.LogHelper.Error("订单插入失败2，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID);
                    return GetErrorResult("订单插入失败");
                }

            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 2627:  //重复插入
                        Log4Net.LogHelper.Error(ex, "订单重复插入，order=" + JsonHelper.EncodeJson(order));
                        return GetErrorResult("订单重复插入");
                    default:
                        Log4Net.LogHelper.Error(ex, "订单插入脚本执行失败，order=" + JsonHelper.EncodeJson(order) + "；APPID:" + submitData.AppID);
                        return GetErrorResult("订单插入脚本执行失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion

            #region 支付系统创建订单

            try
            {
                Log4Net.LogHelper.Info("创建订单amount=" + submitData.TotalMoney);
                ArrayList sArray = new ArrayList();
                sArray.Add("amount=" + submitData.TotalMoney);
                sArray.Add("applicationID=" + _hwAppid);
                sArray.Add("country=CN");
                sArray.Add("currency=CNY");
                sArray.Add("merchantId=" + _hwCpid);
                sArray.Add("productDesc=" + feecombo.FeeName);
                sArray.Add("productName=" + feecombo.FeeName);
                sArray.Add("requestId=" + order.OrderID);
                sArray.Add("sdkChannel=1");
                sArray.Add("urlVer=2");
                sArray.Sort();
                string sign = Create_linkstring(sArray);
                sign = sign.Substring(0, sign.Length - 1);
                string mysign = RSAFromPkcs8.sign(sign, _hwPrivateKey, "UTF-8");
                var obj = new
                {
                    productName = feecombo.FeeName,
                    productDesc = feecombo.FeeName,
                    merchantId = _hwCpid,
                    applicationID = _hwAppid,
                    amount = Convert.ToDouble(submitData.TotalMoney).ToString("0.00"),
                    requestId = order.OrderID,
                    country = "CN",
                    currency = "CNY",
                    sdkChannel = 1,
                    urlVer = "2",
                    merchantName = _hwMerchantName,
                    serviceCatalog = "X9",
                    sign = mysign
                };

                return GetResult(obj);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "订单插入异常，order=" + JsonHelper.EncodeJson(order));
                return GetErrorResult("订单插入异常");
            }
            #endregion
        }

        /// <summary>
        /// 获取权限(APP端支付成功后调用)
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpPost]
        public ApiResponse PostHmsAgentSuccess([FromBody] KingRequest request)
        {
            PaySucessInfo submitData = JsonHelper.DecodeJson<PaySucessInfo>(request.Data);
            if (submitData == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.OrderID))
            {
                return GetErrorResult("订单ID不能为空");
            }

            try
            {
                string strsql = string.Format(@"SELECT [ID]
                                              ,[OrderID]
                                              ,[TotalMoney]
                                              ,[CreateDate]
                                              ,[State]
                                              ,[UserID]
                                              ,[PayWay]
                                              ,[FeeComboID]
                                              ,[CourseID]
                                              ,[IsDiscount]
                                              ,[SourceType] FROM dbo.TB_Order WHERE OrderID='{0}'", submitData.OrderID);
                DataSet ds = SelectOrderSql(submitData.AppID, strsql);

                if (ds.Tables.Count > 0)
                {
                    List<TB_Order> orderList = Extension.Convert2Object<TB_Order>(ds.Tables[0]);
                    if (orderList != null)
                    {
                        var order = orderList.FirstOrDefault();
                        if (order != null)
                        {
                            string sql = string.Format(@"UPDATE	dbo.TB_Order SET State='0001' WHERE OrderID='{0}'", submitData.OrderID);
                            var i = OrderSql(submitData.AppID, sql);
                            if (i > 0)
                            {
                                order.State = "0001";
                                #region 分库订单写入队列
                                listredis.LPush("Order2BaseDBQueue", order.ToJson());
                                #endregion
                                return GetResult(NewQueryCombo(submitData.AppID, order.UserID), "支付成功");
                            }
                        }
                    }

                }

                return GetErrorResult("支付状态异常！");

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error("支付确认PaySuccess出错，ex=" + ex.Message);
                return GetErrorResult(ex.Message);
            }

        }

        public ArrayList NewQueryCombo(string AppID, string UserID)
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
                Log4Net.LogHelper.Error(ex, "QueryCombo获取同步课堂权限出错，UserID=" + UserID);
            }
            finally
            {
                string sql = string.Format(@"SELECT DISTINCT
                                                    CourseID ,
                                                    SUM(Months) 'Months'
                                            FROM    dbo.TB_UserMember
                                            WHERE   UserID = '{0}'
                                            GROUP BY CourseID
                                            ORDER BY CourseID", UserID);
                DataSet ds = SelectOrderSql(AppID, sql);
                List<memberinfo> mi = JsonHelper.DataSetToIList<memberinfo>(ds, 0);
                string strsql = string.Format(@"SELECT  [ID]
                                                      ,[UserID]
                                                      ,[TbOrderID]
                                                      ,[Months]
                                                      ,[StartDate]
                                                      ,[EndDate]
                                                      ,[CourseID]
                                                      ,[Status]
                                                      ,[CreateTime]
                                                      ,[SourceType]
                                                  FROM [TB_UserMember] WHERE UserID='" + UserID + "' order By EndDate desc");
                DataSet datads = SelectOrderSql(AppID, strsql);
                List<TB_UserMember> umList = JsonHelper.DataSetToIList<TB_UserMember>(datads, 0);
                //var umList = usermanager.Search<TB_UserMember>("UserID='" + UserID + "'  order By EndDate desc");
                if (umList != null)
                {
                    if (mi != null)
                    {
                        foreach (var em in mi)
                        {
                            TB_UserMember tu = umList.Where(i => i.CourseID == em.CourseID).OrderBy(i => i.CreateTime).FirstOrDefault();
                            if (tu != null)
                            {
                                if (tu.CreateTime != null)
                                    if (tu.Months != null)
                                        list.Add(new
                                        {
                                            UserID = UserID,
                                            CourseID = tu.CourseID,
                                            EndDate = (tu.CreateTime.Value.AddMonths((int)tu.Months).ToUniversalTime().Ticks - 621355968000000000) / 10000000,
                                            Months = tu.Months
                                        });
                            }
                        }

                    }
                }
            }

            return list;
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        [HttpPost]
        public ApiResponse PostQueryOrder([FromBody] KingRequest request)
        {
            QueryOrder qo = JsonHelper.DecodeJson<QueryOrder>(request.Data);

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
            ArrayList sArray = new ArrayList();
            sArray.Add("keyType=1");
            sArray.Add("merchantId=" + _hwCpid);
            sArray.Add("requestId=" + qo.OrderID);
            sArray.Add("time=" + t);
            sArray.Sort();
            string mysign = Create_linkstring(sArray);
            mysign = mysign.Substring(0, mysign.Length - 1);
            string sign = RSAFromPkcs8.sign(mysign, _hwPrivateKey, "UTF-8");


            var obj = new
            {
                merchantId = _hwCpid,
                requestId = qo.OrderID,
                keyType = 1,
                time = t,
                sign = sign
            };

            return GetResult(JsonHelper.EncodeJson(obj));
        }


        /// <summary>
        /// 处理提醒过程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ProcessNotify()
        {
            HttpContext context = HttpContext.Current;

            Dictionary<string, string> dicc = new Dictionary<string, string>();
            dicc.Add("-1", "华为支付");
            dicc.Add("0", "华为钱包");
            dicc.Add("1", "充值卡");
            dicc.Add("2", "游戏卡");
            dicc.Add("3", "信用卡");
            dicc.Add("4", "支付宝");
            dicc.Add("6", "短代");
            dicc.Add("12", "天翼");
            dicc.Add("13", "PayPal");
            dicc.Add("15", "联通话费");
            dicc.Add("16", "借记卡");
            dicc.Add("17", "微信");
            dicc.Add("18", "花瓣（仅仅支持华为钱包充值,不对商户开放）");
            dicc.Add("19", "礼品卡");
            dicc.Add("20", "现金余额");
            dicc.Add("30", "银视通");
            dicc.Add("50", "预付款");
            dicc.Add("52", "M2E");
            dicc.Add("53", "FPX");
            dicc.Add("54", "FPXE");
            dicc.Add("55", "融资");
            dicc.Add("56", "GlobalPay");
            dicc.Add("57", "分期");
            dicc.Add("58", "MP");
            dicc.Add("59", "MOLPAY");
            dicc.Add("100-199", "CUSTPAY1-100（支付定制支付方式）");

            string payType = context.Request.Form["payType"];
            string orderId = context.Request.Form["requestId"];
            string payWay = "";
            dicc.TryGetValue(payType, out payWay);

            string extReserved = context.Request.Form["extReserved"];
            if (extReserved != null)
            {
                extReserved ext = JsonHelper.DeepDecodeJson<extReserved>(extReserved);
                if (ext != null)
                {
                    string sql = string.Format(@"UPDATE dbo.TB_Order SET PayWay='{0}' WHERE OrderID='{1}'", "华为-" + payWay, orderId);
                    OrderSql(ext.appID, sql);
                }
            }

            var obj = new
            {
                result = 0
            };

            return JsonHelper.EncodeJson(obj);
            //ArrayList sPara = new ArrayList();  //需要加密的参数数组
            //HttpContext context = HttpContext.Current;

            //string s = context.Request.Form.ToString();

            //ArrayList sArrary = GetRequestPost(context);
            //sPara = AlipayFunction.Para_filter(sArrary);  //过滤空值、sign与sign_type参数
            //sPara.Sort();                                   //得到从字母a到z排序后的加密参数数组
            //string prestr = Create_linkstring(sPara);
            //string sign = context.Request.Form["sign"];

            ////string mysign = Build_mysign(sPara);
            //prestr = prestr.Substring(0, prestr.Length - 1);
            //bool b = RSAFromPkcs8.verify(prestr, sign, _hwPublicKey, "UTF-8");

            ////bool b = RSACryptoServiceHelper.RSACheckSign(prestr, sign, "RSA2");
            ////bool b = AlipaySignature.RSACheckContent(prestr, sign, _hwPublicKey, "UTF-8", "RSA2");
            //if (b)
            //{

            //}
        }

        public void testRSA()
        {
            byte[] messagebytes = Encoding.UTF8.GetBytes("luo罗");
            RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
            string privatekey = oRSA.ToXmlString(true);
            string publickey = oRSA.ToXmlString(false);

            //私钥签名 
            RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
            oRSA3.FromXmlString(privatekey);
            byte[] AOutput = oRSA3.SignData(messagebytes, "SHA1");
            //公钥验证 
            RSACryptoServiceProvider oRSA4 = new RSACryptoServiceProvider();
            oRSA4.FromXmlString(publickey);
            bool bVerify = oRSA4.VerifyData(messagebytes, "SHA1", AOutput);
        }



        public ArrayList GetRequestPost(HttpContext context)
        {
            int i = 0;
            ArrayList sArray = new ArrayList();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = context.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i] + "=" + context.Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 生成签名结果
        /// </summary>
        /// <param name="sArray">要加密的数组</param>
        /// <param name="key">安全校验码</param>
        /// <param name="sign_type">加密类型</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果字符串</returns>
        public static string Build_mysign(ArrayList sArray)
        {
            string prestr = Create_linkstring(sArray);  //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr = prestr.Substring(0, nLen - 1);

            string mysign = RSAFromPkcs8.encryptData(prestr, _hwPublicKey, "UTF-8");	//把最终的字符串加密，获得签名结果

            return mysign;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string Create_linkstring(ArrayList sArray)
        {
            int nCount = sArray.Count;
            int i = 0;
            StringBuilder prestr = new StringBuilder();
            for (i = 0; i < nCount; i++)
            {
                prestr.Append(sArray[i].ToString() + "&");
            }

            return prestr.ToString();
        }



        private ApiResponse GetErrorResult(string message)
        {
            return new ApiResponse
            {
                Success = false,
                data = null,
                Message = message
            };
        }

        private ApiResponse GetResult(object Data, string message = "")
        {

            return new ApiResponse
            {
                Success = true,
                data = Data,
                Message = message
            };
        }

        private int OrderSql(string AppID, string sql)
        {
            int i = 0;
            switch (AppID)
            {
                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410": //北京版
                    i = SqlHelper.ExecuteNonQuery(_bj, CommandType.Text, sql);
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385": //广州版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2": //牛津深圳版
                    i = SqlHelper.ExecuteNonQuery(_sz, CommandType.Text, sql);
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62": //广东版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c": //新课标标准实验版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999": //牛津上海本地版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d": //人教PEP版(同步学)
                    i = SqlHelper.ExecuteNonQuery(_rjpep, CommandType.Text, sql);
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4": //人教版新起点
                    i = SqlHelper.ExecuteNonQuery(_rjpep, CommandType.Text, sql);
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766": //江苏译林
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3": //人教版
                    i = SqlHelper.ExecuteNonQuery(_rjpep, CommandType.Text, sql);
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7": //牛津上海全国版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa": //山东版
                    i = SqlHelper.ExecuteNonQuery(_ot, CommandType.Text, sql);
                    break;
                default:
                    break;
            }
            return i;
        }

        public DataSet SelectOrderSql(string AppID, string sql)
        {
            DataSet ds = new DataSet();
            switch (AppID)
            {
                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410": //北京版
                    ds = SqlHelper.ExecuteDataset(_bj, CommandType.Text, sql);
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385": //广州版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2": //牛津深圳版
                    ds = SqlHelper.ExecuteDataset(_sz, CommandType.Text, sql);
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62": //广东版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c": //新课标标准实验版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999": //牛津上海本地版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d": //人教PEP版(同步学)
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4": //人教版新起点
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766": //江苏译林
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3": //人教版
                    ds = SqlHelper.ExecuteDataset(_rjpep, CommandType.Text, sql);
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7": //牛津上海全国版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa": //山东版
                    ds = SqlHelper.ExecuteDataset(_ot, CommandType.Text, sql);
                    break;
                default:
                    break;
            }
            return ds;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数
        /// </summary>
        /// <param name="sArray">加密参数组</param>
        /// <returns>去掉空值与签名参数后的新加密参数组</returns>
        public static ArrayList Para_filter(ArrayList sArray)
        {
            int nCount = sArray.Count;
            int i;
            for (i = 0; i < nCount; i++)
            {
                //把sArray的数组里的元素格式：变量名=值，分割开来
                int nPos = sArray[i].ToString().IndexOf('=');              //获得=字符的位置
                int nLen = sArray[i].ToString().Length;                    //获得字符串长度
                string itemName = sArray[i].ToString().Substring(0, nPos); //获得变量名
                string itemValue = "";                                     //获得变量的值
                if (nPos + 1 < nLen)
                {
                    itemValue = sArray[i].ToString().Substring(nPos + 1);
                }

                if (itemName.ToLower() == "sign" || itemName.ToLower() == "sign_type" || itemValue == "")
                {
                    sArray.RemoveAt(i);
                    nCount--;
                    i--;
                }
            }

            return sArray;
        }
    }

    public class QueryOrder
    {
        public string OrderID { get; set; }
    }

    public class extReserved
    {
        public string packageName { get; set; }
        public string appID { get; set; }
    }
}
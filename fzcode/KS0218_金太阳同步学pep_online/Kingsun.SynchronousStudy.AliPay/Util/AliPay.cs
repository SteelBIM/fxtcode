using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Security.Cryptography;
using System.Diagnostics;
using System.ComponentModel;
using System.Web.Configuration;
using System.Xml;
using Kingsun.SynchronousStudy.AliPay.Util.Result;

namespace Kingsun.SynchronousStudy.AliPay.Util
{
    /// <summary>
    /// 支付宝接口
    /// </summary>
    public class AliPay
    {
        string _aliaccountid = WebConfigurationManager.AppSettings["ali_accountid"];
        string _alipartnerid = WebConfigurationManager.AppSettings["ali_partnerid"];
        string _aliprivatekey = WebConfigurationManager.AppSettings["ali_private_key"];
        string _alinotifyurl = WebConfigurationManager.AppSettings["ali_notify_url"];
        string _alikeycode = WebConfigurationManager.AppSettings["ali_keycode"];
        string _alipayway = WebConfigurationManager.AppSettings["ali_payway"];
        protected EventHandlerList eventList = new EventHandlerList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ProcessNotifyEventHandler(object sender, NotifyEventArgs e);
        #region 实现交易状态枚举的事件

        private static readonly Object waitBuyerPayKey = new object();
        private static readonly Object waitSellerConfirmTradeKey = new object();
        private static readonly Object waitSysConfirmPayKey = new object();
        private static readonly Object waitSellerSendGoodsKey = new object();
        private static readonly Object waitBuyerConfirmGoodsKey = new object();
        private static readonly Object waitSysPaySellerKey = new object();
        private static readonly Object tradeFinishedKey = new object();
        private static readonly Object tradeClosedKey = new object();

        /// <summary>
        /// 等待买家付款
        /// </summary>
        public event ProcessNotifyEventHandler WaitBuyerPay
        {
            add
            {
                eventList.AddHandler(waitBuyerPayKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitBuyerPayKey, value);
            }
        }
        /// <summary>
        /// 交易已创建，等待卖家确认
        /// </summary>
        public event ProcessNotifyEventHandler WaitSellerConfirmTrade
        {
            add
            {
                eventList.AddHandler(waitSellerConfirmTradeKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitSellerConfirmTradeKey, value);
            }
        }
        /// <summary>
        /// 确认买家付款中，暂勿发货
        /// </summary>
        public event ProcessNotifyEventHandler WaitSysConfirmPay
        {
            add
            {
                eventList.AddHandler(waitSysConfirmPayKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitSysConfirmPayKey, value);
            }
        }
        /// <summary>
        /// 支付宝收到买家付款，请卖家发货
        /// </summary>
        public event ProcessNotifyEventHandler WaitSellerSendGoods
        {
            add
            {
                eventList.AddHandler(waitSellerSendGoodsKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitSellerSendGoodsKey, value);
            }
        }
        /// <summary>
        /// 确认买家付款中，暂勿发货
        /// </summary>
        public event ProcessNotifyEventHandler WaitBuyerConfirmGoods
        {
            add
            {
                eventList.AddHandler(waitBuyerConfirmGoodsKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitBuyerConfirmGoodsKey, value);
            }
        }
        /// <summary>
        /// 买家确认收到货，等待支付宝打款给卖家
        /// </summary>
        public event ProcessNotifyEventHandler WaitSysPaySeller
        {
            add
            {
                eventList.AddHandler(waitSysPaySellerKey, value);
            }
            remove
            {
                eventList.RemoveHandler(waitSysPaySellerKey, value);
            }
        }
        /// <summary>
        /// 交易成功结束
        /// </summary>
        public event ProcessNotifyEventHandler TradeFinished
        {
            add
            {
                eventList.AddHandler(tradeFinishedKey, value);
            }
            remove
            {
                eventList.RemoveHandler(tradeFinishedKey, value);
            }
        }

        /// <summary>
        /// 交易中途关闭（未完成）
        /// </summary>
        public event ProcessNotifyEventHandler TradeClosed
        {
            add
            {
                eventList.AddHandler(tradeClosedKey, value);
            }
            remove
            {
                eventList.RemoveHandler(tradeClosedKey, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitSellerSendGoods(NotifyEventArgs e)
        {

            ProcessNotifyEventHandler eventHandler = eventList[waitSellerSendGoodsKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitBuyerPay(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[waitBuyerPayKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitSysConfirmPay(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[waitSysConfirmPayKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitSellerConfirmTrade(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[waitSellerConfirmTradeKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitBuyerConfirmGoods(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[waitBuyerConfirmGoodsKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnWaitSysPaySeller(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[waitSysPaySellerKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTradeFinished(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[tradeFinishedKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTradeClosed(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[tradeClosedKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        #endregion
        #region 实现退款状态枚举的事件
        #endregion
        #region 返回用户支付宝Notify信息的事件
        protected static readonly object notifyEventKey = new object();

        public event ProcessNotifyEventHandler NotifyEvent
        {
            add
            {
                eventList.AddHandler(notifyEventKey, value);
            }
            remove
            {
                eventList.RemoveHandler(notifyEventKey, value);
            }
        }

        protected virtual void OnNotifyEvent(NotifyEventArgs e)
        {
            ProcessNotifyEventHandler eventHandler = eventList[notifyEventKey] as ProcessNotifyEventHandler;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }
        #endregion
        #region public

        /// <summary>
        /// 创建虚拟交易
        /// </summary>
        /// <param name="gatewayUrl">提交支付宝的地址</param>
        /// <param name="digitalGoods">交易参数</param>
        /// <param name="page">Page对象</param>
        public void CreateDigitalTrade(string gatewayUrl, DigitalGoods digitalGoods, HttpContext context)
        {
            HttpResponse Response = context.Response;
            string t = gatewayUrl + Md5SignParam(digitalGoods);
            string url = string.Format("<script language='javascript'>window.location = (\"{0}\") </script>", t);
            Response.Write(url);

        }
        /// <summary>
        /// 创建标准交易，包含虚拟交易
        /// </summary>
        /// <param name="gatewayUrl">提交支付宝的地址</param>
        /// <param name="standardGoods">交易参数</param>
        /// <param name="page">Page对象</param>
        public void CreateStandardTrade(string gatewayUrl, StandardGoods standardGoods, Page page)
        {
            //HttpResponse Response = page.Response;
            //Response.Redirect(gatewayUrl + "?" + Md5SignParam(standardGoods));
            HttpResponse Response = page.Response;
            string t = gatewayUrl + "?" + Md5SignParam(standardGoods);
            string url = string.Format("<script language='javascript'>window.open(\"{0}\") </script>", t);
            Response.Write(url);
        }
        /// <summary>
        /// 处理返回的Notify
        /// </summary>
        /// <param name="page">传如Page对象</param>
        /// <param name="verifyUrl">验证的地址，如：https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">帐户的交易安全校验码（key）</param>
        /// <param name="verify">verify对象</param>
        /// <param name="encode">编码类型</param>
        /// <exception cref="SignVerifyFailedException">支付宝通知签名验证失败</exception>
        /// <exception  cref="CommonAliPayBaseException">支付宝通知验证失败</exception>
        public void CommonProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify, string encode)
        {
            NotifyEventArgs dn = new NotifyEventArgs();
            dn = ParseNotify(context.Request.QueryString, dn);//构造事件参数

            if (VerifyNotify(verifyUrl, verify))//验证成功
            {
                SortedList<string, string> sortedList = GetParam(dn, true);
                string param = GetUrlParam(sortedList, false);

                string sign = GetMd5Sign(encode, param + key);
                if (sign == dn.Sign)//验证签名
                {
                    OnNotifyEvent(dn);
                }
                else
                {
                    dn.Trade_Status = "支付宝通知签名验证失败";
                    OnNotifyEvent(dn);
                }
            }
            else
            {
                dn.Trade_Status = "支付宝订单通知验证失败";
                OnNotifyEvent(dn);
            }
        }

        /// <summary>
        /// 处理返回的Notify
        /// </summary>
        /// <param name="page">传如Page对象</param>
        /// <param name="verifyUrl">验证的地址，如：https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">帐户的交易安全校验码（key）</param>
        /// <param name="verify">verify对象</param>
        /// <param name="encode">编码类型</param>
        /// <exception cref="SignVerifyFailedException">支付宝通知签名验证失败</exception>
        /// <exception  cref="CommonAliPayBaseException">支付宝通知验证失败</exception>
        public void ProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify, string encode)
        {
            if (VerifyNotify(verifyUrl, verify))  //验证成功
            {
                NotifyEventArgs dn = new NotifyEventArgs();
                dn = ParseNotify(context.Request.Form, dn);//构造事件参数
                SortedList<string, string> sortedList = GetParam(dn, true);
                string param = GetUrlParam(sortedList, false);

                string sign = GetMd5Sign(encode, param + key);
                if (sign == dn.Sign)//验证签名
                {
                    switch (dn.Trade_Status)
                    {
                        case "WAIT_SELLER_SEND_GOODS":
                            OnWaitSellerSendGoods(dn);
                            break;
                        case "WAIT_BUYER_PAY":
                            OnWaitBuyerPay(dn);
                            break;
                        case "WAIT_SELLER_CONFIRM_TRADE":
                            OnWaitSellerConfirmTrade(dn);
                            break;
                        case "WAIT_SYS_CONFIRM_PAY":
                            OnWaitSysConfirmPay(dn);
                            break;
                        case "WAIT_BUYER_CONFIRM_GOODS":
                            OnWaitBuyerConfirmGoods(dn);
                            break;
                        case "WAIT_SYS_PAY_SELLER":
                            OnWaitSysPaySeller(dn);
                            break;
                        case "TRADE_SUCCESS"://"TRADE_FINISHED"
                            OnTradeFinished(dn);
                            break;
                        case "TRADE_CLOSED":
                            OnTradeClosed(dn);
                            break;
                        default:
                            throw new NotImplementedException(dn.Trade_Status);
                    }
                    context.Response.Write("success");
                }
                else
                {
                    context.Response.Write("fail");
                    throw new CommonAliPayBaseException("支付宝通知签名验证失败", 102);
                }
            }
            else
            {
                context.Response.Write("fail");
                throw new CommonAliPayBaseException("支付宝通知验证失败", 101);
            }
        }
        /// <summary>
        /// 处理返回的Notify
        /// </summary>
        /// <param name="context"></param>
        /// <param name="verifyUrl">验证的地址，如：https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">帐户的交易安全校验码（key）</param>
        /// <param name="verify">verify对象</param>
        public void ProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify)
        {
            NotifyEventArgs dn = new NotifyEventArgs();
            dn = ParseNotify(context.Request.Form, dn);//构造事件参数
            //if (VerifyNotify(verifyUrl, verify))  //验证成功
            // {
            ArrayList sArrary = GetRequestPost(context);
            ///////////////////////以下参数是需要设置的相关配置参数，设置后不会更改的//////////////////////
            string partner = verify.Partner;                //合作身份者ID
            string input_charset = "utf-8";     //字符编码格式 目前支持 gb2312 或 utf-8
            string sign_type = "MD5";           //加密方式 不需修改
            string transport = "https";        //访问模式,根据自己的服务器是否支持ssl访问，若支持请选择https；若不支持请选择http
            //////////////////////////////////////////////////////////////////////////////////////////////

            if (sArrary.Count > 0)//判断是否有带返回参数
            {
                AlipayNotify aliNotify = new AlipayNotify(sArrary, context.Request.Form["notify_id"], partner, key, input_charset, sign_type, transport);
                string responseTxt = aliNotify.ResponseTxt; //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
                string sign = context.Request.Form["sign"];         //获取支付宝反馈回来的sign结果
                string mysign = aliNotify.Mysign;           //获取通知返回后计算后（验证）的加密结果

                //写日志记录（若要调试，请取消下面两行注释）
                //string sWord = "responseTxt=" + responseTxt + "\n notify_url_log:sign=" + context.Request.Form["sign"] + "&mysign=" + mysign + "\n notify回来的参数：" + AlipayFunction.Create_linkstring(sArrary);
                //string filefolder = System.Configuration.ConfigurationManager.AppSettings["logfile"].ToString();
                //AlipayFunction.log_result(context.Server.MapPath("~/" + filefolder + "/" + DateTime.Now.ToString().Replace(":", "")) + ".txt", sWord);

                //判断responsetTxt是否为ture，生成的签名结果mysign与获得的签名结果sign是否一致
                //responsetTxt的结果不是true，与服务器设置问题、合作身份者ID、notify_id一分钟失效有关
                //mysign与sign不等，与安全校验码、请求时的参数格式（如：带自定义参数等）、编码格式有关
                if (responseTxt == "true")// && sign == mysign)//验证成功
                {
                    //SortedList<string, string> sortedList = GetParam(dn, true);
                    //string param = GetUrlParam(sortedList, false);

                    //string sign = GetMd5Sign("utf-8", param + key);
                    //if (sign == dn.Sign)//验证签名
                    //{
                    switch (context.Request.Form["trade_status"])
                    {
                        case "WAIT_SELLER_SEND_GOODS":
                            OnWaitSellerSendGoods(dn);
                            break;
                        case "WAIT_BUYER_PAY":
                            OnWaitBuyerPay(dn);
                            break;
                        case "WAIT_SELLER_CONFIRM_TRADE":
                            OnWaitSellerConfirmTrade(dn);
                            break;
                        case "WAIT_SYS_CONFIRM_PAY":
                            OnWaitSysConfirmPay(dn);
                            break;
                        case "WAIT_BUYER_CONFIRM_GOODS":
                            OnWaitBuyerConfirmGoods(dn);
                            break;
                        case "WAIT_SYS_PAY_SELLER":
                            OnWaitSysPaySeller(dn);
                            break;
                        case "TRADE_SUCCESS"://"TRADE_FINISHED"
                            OnTradeFinished(dn);
                            break;
                        case "TRADE_CLOSED":
                            OnTradeClosed(dn);
                            break;
                        default:
                            throw new NotImplementedException(dn.Trade_Status);
                    }
                    //context.Response.Write("success");
                }
                else
                {
                    //context.Response.Write("fail");
                    dn.Trade_Status = "支付宝通知签名验证失败";
                    OnNotifyEvent(dn);
                    throw new CommonAliPayBaseException("支付宝通知签名验证失败", 102);
                }
            }
            //}
            //else
            //{
            //    //context.Response.Write("fail");
            //    dn.Trade_Status = "支付宝通知验证失败";
            //    OnNotifyEvent(dn);
            //    throw new CommonAliPayBaseException("支付宝通知验证失败", 101);
            //}
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组--zjz-20100910---
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
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
        #endregion

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <param name="partner">合作身份者ID</param>
        /// <returns>时间戳字符串</returns>
        public bool Query_Order(string orderid)
        {
            ///////////////////////以下参数是需要设置的相关配置参数，设置后不会更改的//////////////////////
            string partner;            //合作身份者ID
            string input_charset = "utf-8";     //字符编码格式 目前支持 gb2312 或 utf-8
            string sign_type = "MD5";           //加密方式 不需修改
            string transport = "https";        //访问模式,根据自己的服务器是否支持ssl访问，若支持请选择https；若不支持请选择http

            string key;//填写自己的key
            string gateway;//填写支付网关="https://www.alipay.com/cooperate/gateway.do"
            string AccountID;
            //////////////////////////////////////////////////////////////////////////////////////////////

            //SetParameters(out key, out partner, out gateway, out AccountID, orderid);

            string param;
            string mysign;
            string url = _alipayway + "service=single_trade_query&sign={0}&out_trade_no={1}&partner={2}&_input_charset={3}&sign_type={4}";
            if (!string.IsNullOrEmpty(_alipartnerid))
            {
                ArrayList sPara = new ArrayList();  //需要加密的参数数组
                sPara.Add("_input_charset=" + input_charset.ToLower());
                sPara.Add("out_trade_no=" + orderid);
                sPara.Add("partner=" + _alipartnerid);
                sPara.Add("service=single_trade_query");
                //sPara.Add("sign_type=" + sign_type);
                mysign = AlipayFunction.Build_mysign(sPara, _alikeycode, sign_type, input_charset);
                url = string.Format(url, mysign, orderid, _alipartnerid, input_charset, sign_type);


                XmlTextReader Reader = new XmlTextReader(url);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Reader);

                string result = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (result == "T")
                {
                    if (xmlDoc.SelectSingleNode("/alipay/response/trade/trade_status").InnerText == "TRADE_SUCCESS")
                    {
                        return true;
                    }
                }

                //HttpContext.Current.Response.Write(xmlDoc.InnerText);
                return false;
            }

            return false;
        }

        #region private
        /// <summary>
        /// 通知验证接口
        /// </summary>
        /// <param name="verifyUrl"></param>
        /// <param name="verify">验证参数</param>
        /// <returns>true，通过验证</returns>
        private bool VerifyNotify(string verifyUrl, Verify verify)
        {
            SortedList<string, string> sl = GetParam(verify, false);
            string param = GetParamString(verifyUrl, sl);
            string result = Get_Http(param, 120000);
            return bool.Parse(result);
        }

        /// <summary>
        ///  解析Form集合到DigitalNotifyEventArgs,值类型会被初始化为null
        /// </summary>
        /// <param name="nv"></param>
        /// <param name="obj"></param>
        /// <remarks>为防止值类型的默认值污染参数集合,用了nullable范型</remarks>
        public NotifyEventArgs ParseNotify(NameValueCollection nv, object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo pi in propertyInfos)
            {
                string v = nv.Get(pi.Name.ToLower());
                if (v != null)
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(obj, v, null);
                    }
                    else if (pi.PropertyType == typeof(int?))
                    {
                        pi.SetValue(obj, int.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(decimal?))
                    {
                        pi.SetValue(obj, decimal.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(DateTime?))
                    {
                        pi.SetValue(obj, DateTime.Parse(v), null);
                    }
                    else if (pi.PropertyType == typeof(bool))
                    {
                        pi.SetValue(obj, bool.Parse(v), null);
                    }
                    else
                    {
                        //转型失败会抛出异常
                        pi.SetValue(obj, v, null);
                    }
                }
            }
            return (NotifyEventArgs)obj;

        }
        /// <summary>
        /// 获取Md5sign后的参数
        /// </summary>
        /// <param name="digitalGoods"></param>
        /// <returns></returns>
        private string Md5SignParam(DigitalGoods digitalGoods)
        {
            if (digitalGoods.Sign_Type.ToLower() != "md5")
            {
                throw new CommonAliPayBaseException("Sign_Type不支持MD5", 100);
            }

            SortedList<string, string> goods = GetParam(digitalGoods, false);

            string param = GetUrlParam(goods, false) + digitalGoods.Sign;
            string encodeParam = GetUrlParam(goods, true) + "&";
            string sign = GetMd5Sign(digitalGoods._Input_Charset, param);
            //return encodeParam + string.Format("sign={0}&sign_type={1}", HttpUtility.HtmlEncode(sign),
            //    HttpUtility.HtmlEncode(digitalGoods.Sign_Type));


            return encodeParam + string.Format("sign={0}&sign_type={1}", sign, digitalGoods.Sign_Type);
        }
        /// <summary>
        /// 获取排序后的参数
        /// 修改通知验证后参数的获取*****20090917 wuxw
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="flag">支付通知验证的标志,flag为true即为通知验证.flag为false为返回的参数获取</param>
        /// <returns></returns>
        private SortedList<string, string> GetParam(object obj, bool flag)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            SortedList<string, string> sortedList = new SortedList<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.GetValue(obj, null) != null)
                {
                    if (flag)//为true时通知验证的参数获取
                    {
                        if (pi.Name.ToLower() == "sign" || pi.Name.ToLower() == "sign_type")
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (pi.Name.ToLower() == "sign" || pi.Name.ToLower() == "sign_type" || pi.Name.ToLower() == "price" || pi.Name.ToLower() == "quantity")
                        {
                            continue;
                        }
                    }
                    if (pi.Name.ToLower() == "notify_time")
                    {
                        DateTime notifyDateTime = (DateTime)pi.GetValue(obj, null);
                        string notifyTime = notifyDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        sortedList.Add(pi.Name.ToLower(), notifyTime);
                    }
                    else
                    {
                        sortedList.Add(pi.Name.ToLower(), pi.GetValue(obj, null).ToString());
                    }
                }
            }
            return sortedList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetParamString(string verifyUrl, SortedList<string, string> list)
        {
            StringBuilder paramString = new StringBuilder();
            paramString.Append(verifyUrl);

            string[] paraList = new string[] { "service", "partner", "notify_id" };
            for (int i = 0; i < paraList.Length; i++)
            {
                paramString.Append(paraList[i]);
                paramString.Append("=");
                paramString.Append(list[paraList[i]]);
                paramString.Append("&");
            }
            paramString.Remove(paramString.Length - 1, 1);

            return paramString.ToString();
        }

        /// <summary>
        /// 获取Url的参数
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="isEncode">参数是否经过编码,被签名的参数不用编码,Post的参数要编码</param>
        /// <returns></returns>
        private string GetUrlParam(SortedList<string, string> sortedList, bool isEncode)
        {
            StringBuilder param = new StringBuilder();
            StringBuilder encodeParam = new StringBuilder();
            if (isEncode == false)
            {

                foreach (KeyValuePair<string, string> kvp in sortedList)
                {
                    string t = string.Format("{0}={1}", kvp.Key, kvp.Value);
                    param.Append(t + "&");
                }
                return param.ToString().TrimEnd('&');
            }
            else
            {
                foreach (KeyValuePair<string, string> kvp in sortedList)
                {
                    string et = string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value));
                    encodeParam.Append(et + "&");
                }
                return encodeParam.ToString().TrimEnd('&');
            }
        }

        /// <summary>
        /// 获取字符串的MD5签名
        /// </summary>
        /// <param name="encode">签名时用的编码</param>
        /// <param name="param">要签名的字符串</param>
        /// <returns></returns>
        private string GetMd5Sign(string encode, string param)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(Encoding.GetEncoding(encode).GetBytes(param));
            StringBuilder sBuilder = new StringBuilder(32);
            for (int i = 0; i < data.Length; i++)
            {
                // sBuilder.Append(data[i].ToString("x2"));
                sBuilder.Append(data[i].ToString("x").PadLeft(2, '0'));
            }
            return sBuilder.ToString();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="data"></param>
        ///// <param name="encode"></param>
        ///// <returns></returns>
        //private string PostData(string url, string data, string encode)
        //{
        //    CookieContainer cc = new CookieContainer();
        //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //    request.CookieContainer = cc;
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    Stream requestStream = request.GetRequestStream();
        //    byte[] byteArray = Encoding.UTF8.GetBytes(data);
        //    requestStream.Write(byteArray, 0, byteArray.Length);
        //    requestStream.Close();
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    Uri responseUri = response.ResponseUri;
        //    Stream receiveStream = response.GetResponseStream();
        //    StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding(encode));
        //    string result = readStream.ReadToEnd();
        //    return result;          
        //}

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="a_strUrl"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public String Get_Http(String a_strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                //                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {

                strResult = "错误：" + exp.Message;
            }

            return strResult;
        }
        #endregion

    }
}

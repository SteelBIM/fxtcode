using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Configuration;
using CBSS.Core.Utility;
using CBSS.Core.Log;

namespace CBSS.Core.Pay.WXPay.lib
{
    public class WxPayApi
    {
        static string filepath = "Config/PayConfig.xml";
        private static string _szWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "sz_wx_appid");
        private static string _szWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "sz_wx_mchid");
        private static string _szWxKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "sz_wx_key");
        private static string _bjWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "bj_wx_appid");
        private static string _bjWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "bj_wx_mchid");
        private static string _bjWxKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "bj_wx_key");
        private static string _shbdAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shbd_appid");
        private static string _shbdMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shbd_mchid");
        private static string _shbdKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shbd_key");
        private static string _shqgWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shqg_wx_appid");
        private static string _shqgWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shqg_wx_mchid");
        private static string _shqgWxKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "shqg_wx_key");
        private static string _gdAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gd_appid");
        private static string _gdMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gd_mchid");
        private static string _gdKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gd_key");
        private static string _gzAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gz_appid");
        private static string _gzMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gz_mchid");
        private static string _gzKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "gz_key");
        private static string _rjpepWxAppid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "rjpep_wx_appid");
        private static string _rjpepWxMchid = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "rjpep_wx_mchid");
        private static string _rjpepWxKey = XMLHelper.GetAppSetting(filepath, "WXPayConfig", "rjpep_wx_key");

        /**
        * 提交被扫支付API
        * 收银员使用扫码设备读取微信用户刷卡授权码以后，二维码或条码信息传送至商户收银台，
        * 由商户收银台或者商户后台调用该接口发起支付。
        * @param WxPayData inputObj 提交给被扫支付API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回调用结果，其他抛异常
        */
        public static WxPayData Micropay(WxPayData inputObj, int timeOut = 10)
        {
            string url = "https://api.mch.weixin.qq.com/pay/micropay";
            //检测必填参数
            if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("提交被扫支付API接口中，缺少必填参数body！");
            }
            else if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("提交被扫支付API接口中，缺少必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("提交被扫支付API接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("auth_code"))
            {
                throw new WxPayException("提交被扫支付API接口中，缺少必填参数auth_code！");
            }

            inputObj.SetValue("spbill_create_ip", WxPayConfig.IP);//终端ip
            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            Log.Debug("WxPayApi", "MicroPay request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            Log.Debug("WxPayApi", "MicroPay response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        *    
        * 查询订单
        * @param WxPayData inputObj 提交给查询订单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回订单查询结果，其他抛异常
        */
        public static WxPayData OrderQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            string APPID = "";
            string MCHID = "";
            string KEY = "";
            switch (inputObj.GetValue("attach").ToString())
            {
                case "com.sz.syslearning":
                    APPID = _szWxAppid;
                    MCHID = _szWxMchid;
                    KEY = _szWxKey;
                    break;
                case "com.bj.syslearning":
                    APPID = _bjWxAppid;
                    MCHID = _bjWxMchid;
                    KEY = _bjWxKey;
                    break;
                case "com.shqg.syslearning":
                    APPID = _shqgWxAppid;
                    MCHID = _shqgWxMchid;
                    KEY = _shqgWxKey;
                    break;
                case "com.shbd.syslearning":
                    APPID = _shbdAppid;
                    MCHID = _shbdMchid;
                    KEY = _shbdKey;
                    break;
                case "com.gd.syslearning":
                    APPID = _gdAppid;
                    MCHID = _gdMchid;
                    KEY = _gdKey;
                    break;
                case "com.gz.syslearning":
                    APPID = _gzAppid;
                    MCHID = _gzMchid;
                    KEY = _gzKey;
                    break;
                case "com.rj.syslearning":
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
                default:
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
            }

            inputObj.SetValue("appid", APPID);//公众账号ID
            inputObj.SetValue("mch_id", MCHID);//商户号
            inputObj.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.NewMakeSign(KEY));//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            Log.Debug("WxPayApi", "OrderQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口提交数据
            Log.Debug("WxPayApi", "OrderQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }




        /**
        * 
        * 撤销订单API接口
        * @param WxPayData inputObj 提交给撤销订单API接口的参数，out_trade_no和transaction_id必填一个
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回API调用结果，其他抛异常
        */
        public static WxPayData Reverse(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/reverse";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("撤销订单API接口中，参数out_trade_no和transaction_id必须填写一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            Log.Debug("WxPayApi", "Reverse request : " + xml);

            string response = HttpService.Post(xml, url, true, timeOut);

            Log.Debug("WxPayApi", "Reverse response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 申请退款
        * @param WxPayData inputObj 提交给申请退款API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回接口调用结果，其他抛异常
        */
        public static WxPayData Refund(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("退款申请接口中，out_trade_no、transaction_id至少填一个！");
            }
            else if (!inputObj.IsSet("out_refund_no"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数out_refund_no！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数total_fee！");
            }
            else if (!inputObj.IsSet("refund_fee"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数refund_fee！");
            }
            else if (!inputObj.IsSet("op_user_id"))
            {
                throw new WxPayException("退款申请接口中，缺少必填参数op_user_id！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();
            var start = DateTime.Now;

            Log.Debug("WxPayApi", "Refund request : " + xml);
            string response = HttpService.Post(xml, url, true, timeOut);//调用HTTP通信接口提交数据到API
            Log.Debug("WxPayApi", "Refund response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
	    * 
	    * 查询退款
	    * 提交退款申请后，通过该接口查询退款状态。退款有一定延时，
	    * 用零钱支付的退款20分钟内到账，银行卡支付的退款3个工作日后重新查询退款状态。
	    * out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个
	    * @param WxPayData inputObj 提交给查询退款API的参数
	    * @param int timeOut 接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回，其他抛异常
	    */
        public static WxPayData RefundQuery(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/refundquery";
            //检测必填参数
            if (!inputObj.IsSet("out_refund_no") && !inputObj.IsSet("out_trade_no") &&
                !inputObj.IsSet("transaction_id") && !inputObj.IsSet("refund_id"))
            {
                throw new WxPayException("退款查询接口中，out_refund_no、out_trade_no、transaction_id、refund_id四个参数必填一个！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            Log.Debug("WxPayApi", "RefundQuery request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            Log.Debug("WxPayApi", "RefundQuery response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的结果转换为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 下载对账单
        * @param WxPayData inputObj 提交给下载对账单API的参数
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData DownloadBill(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/downloadbill";
            //检测必填参数
            if (!inputObj.IsSet("bill_date"))
            {
                throw new WxPayException("对账单接口中，缺少必填参数bill_date！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名

            string xml = inputObj.ToXml();

            Log.Debug("WxPayApi", "DownloadBill request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);//调用HTTP通信接口以提交数据到API
            Log.Debug("WxPayApi", "DownloadBill result : " + response);

            WxPayData result = new WxPayData();
            //若接口调用失败会返回xml格式的结果
            if (response.Substring(0, 5) == "<xml>")
            {
                result.FromXml(response);
            }
            //接口调用成功则返回非xml格式的数据
            else
                result.SetValue("result", response);

            return result;
        }


        /**
	    * 
	    * 转换短链接
	    * 该接口主要用于扫码原生支付模式一中的二维码链接转成短链接(weixin://wxpay/s/XXXXXX)，
	    * 减小二维码数据量，提升扫描速度和精确度。
	    * @param WxPayData inputObj 提交给转换短连接API的参数
	    * @param int timeOut 接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回，其他抛异常
	    */
        public static WxPayData ShortUrl(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/tools/shorturl";
            //检测必填参数
            if (!inputObj.IsSet("long_url"))
            {
                throw new WxPayException("需要转换的URL，签名用原串，传输需URL encode！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串	
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            Log.Debug("WxPayApi", "ShortUrl request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            Log.Debug("WxPayApi", "ShortUrl response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData UnifiedOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "缺少统一支付接口必填参数out_trade_no！", null);
                throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "缺少统一支付接口必填参数body！", null);
                throw new WxPayException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "缺少统一支付接口必填参数total_fee！", null);
                throw new WxPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "缺少统一支付接口必填参数trade_type！", null);
                throw new WxPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！", null);
                throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！", null);
                throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            string APPID = "";
            string MCHID = "";
            string KEY = "";
            switch (inputObj.GetValue("attach").ToString())
            {
                case "com.sz.syslearning":
                    APPID = _szWxAppid;
                    MCHID = _szWxMchid;
                    KEY = _szWxKey;
                    break;
                case "com.bj.syslearning":
                    APPID = _bjWxAppid;
                    MCHID = _bjWxMchid;
                    KEY = _bjWxKey;
                    break;
                case "com.shqg.syslearning":
                    APPID = _shqgWxAppid;
                    MCHID = _shqgWxMchid;
                    KEY = _shqgWxKey;
                    break;
                case "com.shbd.syslearning":
                    APPID = _shbdAppid;
                    MCHID = _shbdMchid;
                    KEY = _shbdKey;
                    break;
                case "com.gd.syslearning":
                    APPID = _gdAppid;
                    MCHID = _gdMchid;
                    KEY = _gdKey;
                    break;
                case "com.gz.syslearning":
                    APPID = _gzAppid;
                    MCHID = _gzMchid;
                    KEY = _gzKey;
                    break;
                case "com.rj.syslearning":
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
                default:
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
            }


            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", WxPayConfig.NOTIFY_URL);//异步通知url
            }

            inputObj.SetValue("appid", APPID);//公众账号ID
            inputObj.SetValue("mch_id", MCHID);//商户号
            inputObj.SetValue("spbill_create_ip", WxPayConfig.IP);//终端ip	  	    
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.NewMakeSign(KEY));
            string xml = inputObj.ToXml();

            var start = DateTime.Now;

            Log.Debug("WxPayApi", "UnfiedOrder request : " + xml);
            string response = HttpService.Post(xml, url, false, timeOut);
            Log.Debug("WxPayApi", "UnfiedOrder response : " + response);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);
            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
        * 
        * 关闭订单
        * @param WxPayData inputObj 提交给关闭订单API的参数
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData CloseOrder(WxPayData inputObj, int timeOut = 6)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("关闭订单接口中，out_trade_no必填！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串		
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            var start = DateTime.Now;//请求开始时间

            string response = HttpService.Post(xml, url, false, timeOut);

            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);

            WxPayData result = new WxPayData();
            result.FromXml(response);

            ReportCostTime(url, timeCost, result);//测速上报

            return result;
        }


        /**
	    * 
	    * 测速上报
	    * @param string interface_url 接口URL
	    * @param int timeCost 接口耗时
	    * @param WxPayData inputObj参数数组
	    */
        private static void ReportCostTime(string interface_url, int timeCost, WxPayData inputObj)
        {
            //如果不需要进行上报
            if (WxPayConfig.REPORT_LEVENL == 0)
            {
#pragma warning disable CS0162 // 检测到无法访问的代码
                return;
#pragma warning restore CS0162 // 检测到无法访问的代码
            }

            //如果仅失败上报
            if (WxPayConfig.REPORT_LEVENL == 1 && inputObj.IsSet("return_code") && inputObj.GetValue("return_code").ToString() == "SUCCESS" &&
             inputObj.IsSet("result_code") && inputObj.GetValue("result_code").ToString() == "SUCCESS")
            {
                return;
            }

            //上报逻辑
            WxPayData data = new WxPayData();
            data.SetValue("interface_url", interface_url);
            data.SetValue("execute_time_", timeCost);
            //返回状态码
            if (inputObj.IsSet("return_code"))
            {
                data.SetValue("return_code", inputObj.GetValue("return_code"));
            }
            //返回信息
            if (inputObj.IsSet("return_msg"))
            {
                data.SetValue("return_msg", inputObj.GetValue("return_msg"));
            }
            //业务结果
            if (inputObj.IsSet("result_code"))
            {
                data.SetValue("result_code", inputObj.GetValue("result_code"));
            }
            //错误代码
            if (inputObj.IsSet("err_code"))
            {
                data.SetValue("err_code", inputObj.GetValue("err_code"));
            }
            //错误代码描述
            if (inputObj.IsSet("err_code_des"))
            {
                data.SetValue("err_code_des", inputObj.GetValue("err_code_des"));
            }
            //商户订单号
            if (inputObj.IsSet("out_trade_no"))
            {
                data.SetValue("out_trade_no", inputObj.GetValue("out_trade_no"));
            }
            //设备号
            if (inputObj.IsSet("device_info"))
            {
                data.SetValue("device_info", inputObj.GetValue("device_info"));
            }

            try
            {
                Report(data);
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (WxPayException ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                //不做任何处理
            }
        }


        /**
	    * 
	    * 测速上报接口实现
	    * @param WxPayData inputObj 提交给测速上报接口的参数
	    * @param int timeOut 测速上报接口超时时间
	    * @throws WxPayException
	    * @return 成功时返回测速上报接口返回的结果，其他抛异常
	    */
        public static WxPayData Report(WxPayData inputObj, int timeOut = 1)
        {
            string url = "https://api.mch.weixin.qq.com/payitil/report";
            //检测必填参数
            if (!inputObj.IsSet("interface_url"))
            {
                throw new WxPayException("接口URL，缺少必填参数interface_url！");
            }
            if (!inputObj.IsSet("return_code"))
            {
                throw new WxPayException("返回状态码，缺少必填参数return_code！");
            }
            if (!inputObj.IsSet("result_code"))
            {
                throw new WxPayException("业务结果，缺少必填参数result_code！");
            }
            if (!inputObj.IsSet("user_ip"))
            {
                throw new WxPayException("访问接口IP，缺少必填参数user_ip！");
            }
            if (!inputObj.IsSet("execute_time_"))
            {
                throw new WxPayException("接口耗时，缺少必填参数execute_time_！");
            }

            inputObj.SetValue("appid", WxPayConfig.APPID);//公众账号ID
            inputObj.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            inputObj.SetValue("user_ip", WxPayConfig.IP);//终端ip
            inputObj.SetValue("time", DateTime.Now.ToString("yyyyMMddHHmmss"));//商户上报时间	 
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            Log.Info("WxPayApi", "Report request : " + xml);

            string response = HttpService.Post(xml, url, false, timeOut);

            Log.Info("WxPayApi", "Report response : " + response);

            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <param name="totalmoney">商品金额</param>
        /// <param name="body">商品描述</param>
        /// <returns></returns>
        public static WxPayData GetPrePayInfo(string orderid, int totalmoney, string body, string packageName)
        {
            string APPID = "";
            string MCHID = "";
            string KEY = "";
            switch (packageName)
            {
                case "com.sz.syslearning":
                    APPID = _szWxAppid;
                    MCHID = _szWxMchid;
                    KEY = _szWxKey;
                    break;
                case "com.bj.syslearning":
                    APPID = _bjWxAppid;
                    MCHID = _bjWxMchid;
                    KEY = _bjWxKey;
                    break;
                case "com.shqg.syslearning":
                    APPID = _shqgWxAppid;
                    MCHID = _shqgWxMchid;
                    KEY = _shqgWxKey;
                    break;
                case "com.shbd.syslearning":
                    APPID = _shbdAppid;
                    MCHID = _shbdMchid;
                    KEY = _shbdKey;
                    break;
                case "com.gd.syslearning":
                    APPID = _gdAppid;
                    MCHID = _gdMchid;
                    KEY = _gdKey;
                    break;
                case "com.gz.syslearning":
                    APPID = _gzAppid;
                    MCHID = _gzMchid;
                    KEY = _gzKey;
                    break;
                case "com.rj.syslearning":
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
                default:
                    APPID = _rjpepWxAppid;
                    MCHID = _rjpepWxMchid;
                    KEY = _rjpepWxKey;
                    break;
            }

            WxPayData data = new WxPayData();
            data.SetValue("body", body);//商品描述
            data.SetValue("attach", packageName);//附加数据
            data.SetValue("out_trade_no", orderid);//随机字符串
            data.SetValue("total_fee", totalmoney);//总金额
            //data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            //data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            //data.SetValue("goods_tag", goodstag);//商品标记
            data.SetValue("trade_type", "APP");//交易类型
            Log4NetHelper.Info(LoggerType.ApiExceptionLog, "统一下单返回的结果值=====：" + data.ToXml());
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //二次签名
                WxPayData rdata = new WxPayData();

                rdata.SetValue("appid", APPID);
                rdata.SetValue("partnerid", MCHID);

                if (result.IsSet("prepay_id"))
                {
                    rdata.SetValue("prepayid", result.GetValue("prepay_id"));
                }

                if (result.IsSet("nonce_str"))
                {
                    rdata.SetValue("noncestr", result.GetValue("nonce_str"));
                }
                rdata.SetValue("package", "Sign=WXPay");

                rdata.SetValue("timestamp", WxPayApi.GenerateTimeStamp());

                string sign = rdata.NewMakeSign(KEY);

                if (sign != null)
                {
                    result.SetValue("sign", sign);
                    result.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        ///**
        //* 根据当前系统时间加随机序列来生成订单号
        // * @return 订单号
        //*/
        //public static string GenerateOutTradeNo()
        //{
        //    var ran = new Random();
        //    return string.Format("{0}{1}{2}", WxPayConfig.MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), new Random(Guid.NewGuid().GetHashCode()).Next(999));
        //}

        /**
* 根据当前系统时间加随机序列来生成订单号
 * @return 订单号
*/
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), new Random(Guid.NewGuid().GetHashCode()).Next(999));
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
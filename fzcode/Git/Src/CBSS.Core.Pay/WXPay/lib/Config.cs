using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace CBSS.Core.Pay.WXPay.lib
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        //public const string APPID = "wx97b5ee71152e455a";
        //public const string MCHID = "1460931902";//同步学-英语广东版
        //public const string KEY = "fangzhijintaiyangkingsunsoft2017";
        //public const string APPSECRET = "db1173895f4b82e50b07605f4dae5db0";
        public static string APPID
        {
            get
            {
                return AppKey("rjpep_wx_appid");
            }
        }

        public static string MCHID
        {
            get
            {
                return AppKey("rjpep_wx_mchid");
            }
        }

        public static string KEY
        {
            get
            {
                return AppKey("rjpep_wx_key");
            }
        }

        public static string APPSECRET
        {
            get
            {
                return AppKey("rjpep_wx_appsecret");
            }
        }


        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        //public const string SSLCERT_PATH = "D:\\users\\fulin.yuan\\Desktop\\cert\\apiclient_cert.p12";
        public static string SSLCERT_PATH
        {
            get
            {
                return AppKey("wx_sslcert_path");
            }
        }

        //public const string SSLCERT_PASSWORD = "1460931902";
        public static string SSLCERT_PASSWORD
        {
            get
            {
                return AppKey("rjpep_wx_sslcert_password");
            }
        }


        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        //public const string NOTIFY_URL = "http://paytest.tbx.kingsun.cn/example/ResultNotifyPage.aspx";
        public static string NOTIFY_URL
        {
            get
            {
                return AppKey("wx_notify_url");
            }
        }
        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "192.168.3.144";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 2;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 3;

        public static string AppKey(string key)
        {
            try
            {
                string filepath = "Config/PayConfig.xml";
                return XMLHelper.GetAppSetting(filepath, "WXPayConfig", key);
            }
            catch
            {
                return null;
            }
        }
    }
}
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
    /// ֧�����ӿ�
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
        #region ʵ�ֽ���״̬ö�ٵ��¼�

        private static readonly Object waitBuyerPayKey = new object();
        private static readonly Object waitSellerConfirmTradeKey = new object();
        private static readonly Object waitSysConfirmPayKey = new object();
        private static readonly Object waitSellerSendGoodsKey = new object();
        private static readonly Object waitBuyerConfirmGoodsKey = new object();
        private static readonly Object waitSysPaySellerKey = new object();
        private static readonly Object tradeFinishedKey = new object();
        private static readonly Object tradeClosedKey = new object();

        /// <summary>
        /// �ȴ���Ҹ���
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
        /// �����Ѵ������ȴ�����ȷ��
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
        /// ȷ����Ҹ����У����𷢻�
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
        /// ֧�����յ���Ҹ�������ҷ���
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
        /// ȷ����Ҹ����У����𷢻�
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
        /// ���ȷ���յ������ȴ�֧������������
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
        /// ���׳ɹ�����
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
        /// ������;�رգ�δ��ɣ�
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
        #region ʵ���˿�״̬ö�ٵ��¼�
        #endregion
        #region �����û�֧����Notify��Ϣ���¼�
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
        /// �������⽻��
        /// </summary>
        /// <param name="gatewayUrl">�ύ֧�����ĵ�ַ</param>
        /// <param name="digitalGoods">���ײ���</param>
        /// <param name="page">Page����</param>
        public void CreateDigitalTrade(string gatewayUrl, DigitalGoods digitalGoods, HttpContext context)
        {
            HttpResponse Response = context.Response;
            string t = gatewayUrl + Md5SignParam(digitalGoods);
            string url = string.Format("<script language='javascript'>window.location = (\"{0}\") </script>", t);
            Response.Write(url);

        }
        /// <summary>
        /// ������׼���ף��������⽻��
        /// </summary>
        /// <param name="gatewayUrl">�ύ֧�����ĵ�ַ</param>
        /// <param name="standardGoods">���ײ���</param>
        /// <param name="page">Page����</param>
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
        /// �����ص�Notify
        /// </summary>
        /// <param name="page">����Page����</param>
        /// <param name="verifyUrl">��֤�ĵ�ַ���磺https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">�ʻ��Ľ��װ�ȫУ���루key��</param>
        /// <param name="verify">verify����</param>
        /// <param name="encode">��������</param>
        /// <exception cref="SignVerifyFailedException">֧����֪ͨǩ����֤ʧ��</exception>
        /// <exception  cref="CommonAliPayBaseException">֧����֪ͨ��֤ʧ��</exception>
        public void CommonProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify, string encode)
        {
            NotifyEventArgs dn = new NotifyEventArgs();
            dn = ParseNotify(context.Request.QueryString, dn);//�����¼�����

            if (VerifyNotify(verifyUrl, verify))//��֤�ɹ�
            {
                SortedList<string, string> sortedList = GetParam(dn, true);
                string param = GetUrlParam(sortedList, false);

                string sign = GetMd5Sign(encode, param + key);
                if (sign == dn.Sign)//��֤ǩ��
                {
                    OnNotifyEvent(dn);
                }
                else
                {
                    dn.Trade_Status = "֧����֪ͨǩ����֤ʧ��";
                    OnNotifyEvent(dn);
                }
            }
            else
            {
                dn.Trade_Status = "֧��������֪ͨ��֤ʧ��";
                OnNotifyEvent(dn);
            }
        }

        /// <summary>
        /// �����ص�Notify
        /// </summary>
        /// <param name="page">����Page����</param>
        /// <param name="verifyUrl">��֤�ĵ�ַ���磺https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">�ʻ��Ľ��װ�ȫУ���루key��</param>
        /// <param name="verify">verify����</param>
        /// <param name="encode">��������</param>
        /// <exception cref="SignVerifyFailedException">֧����֪ͨǩ����֤ʧ��</exception>
        /// <exception  cref="CommonAliPayBaseException">֧����֪ͨ��֤ʧ��</exception>
        public void ProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify, string encode)
        {
            if (VerifyNotify(verifyUrl, verify))  //��֤�ɹ�
            {
                NotifyEventArgs dn = new NotifyEventArgs();
                dn = ParseNotify(context.Request.Form, dn);//�����¼�����
                SortedList<string, string> sortedList = GetParam(dn, true);
                string param = GetUrlParam(sortedList, false);

                string sign = GetMd5Sign(encode, param + key);
                if (sign == dn.Sign)//��֤ǩ��
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
                    throw new CommonAliPayBaseException("֧����֪ͨǩ����֤ʧ��", 102);
                }
            }
            else
            {
                context.Response.Write("fail");
                throw new CommonAliPayBaseException("֧����֪ͨ��֤ʧ��", 101);
            }
        }
        /// <summary>
        /// �����ص�Notify
        /// </summary>
        /// <param name="context"></param>
        /// <param name="verifyUrl">��֤�ĵ�ַ���磺https://www.alipay.com/cooperate/gateway.do</param>
        /// <param name="key">�ʻ��Ľ��װ�ȫУ���루key��</param>
        /// <param name="verify">verify����</param>
        public void ProcessNotify(HttpContext context, string verifyUrl, string key, Verify verify)
        {
            NotifyEventArgs dn = new NotifyEventArgs();
            dn = ParseNotify(context.Request.Form, dn);//�����¼�����
            //if (VerifyNotify(verifyUrl, verify))  //��֤�ɹ�
            // {
            ArrayList sArrary = GetRequestPost(context);
            ///////////////////////���²�������Ҫ���õ�������ò��������ú󲻻���ĵ�//////////////////////
            string partner = verify.Partner;                //���������ID
            string input_charset = "utf-8";     //�ַ������ʽ Ŀǰ֧�� gb2312 �� utf-8
            string sign_type = "MD5";           //���ܷ�ʽ �����޸�
            string transport = "https";        //����ģʽ,�����Լ��ķ������Ƿ�֧��ssl���ʣ���֧����ѡ��https������֧����ѡ��http
            //////////////////////////////////////////////////////////////////////////////////////////////

            if (sArrary.Count > 0)//�ж��Ƿ��д����ز���
            {
                AlipayNotify aliNotify = new AlipayNotify(sArrary, context.Request.Form["notify_id"], partner, key, input_charset, sign_type, transport);
                string responseTxt = aliNotify.ResponseTxt; //��ȡԶ�̷�����ATN�������֤�Ƿ���֧��������������������
                string sign = context.Request.Form["sign"];         //��ȡ֧��������������sign���
                string mysign = aliNotify.Mysign;           //��ȡ֪ͨ���غ�������֤���ļ��ܽ��

                //д��־��¼����Ҫ���ԣ���ȡ����������ע�ͣ�
                //string sWord = "responseTxt=" + responseTxt + "\n notify_url_log:sign=" + context.Request.Form["sign"] + "&mysign=" + mysign + "\n notify�����Ĳ�����" + AlipayFunction.Create_linkstring(sArrary);
                //string filefolder = System.Configuration.ConfigurationManager.AppSettings["logfile"].ToString();
                //AlipayFunction.log_result(context.Server.MapPath("~/" + filefolder + "/" + DateTime.Now.ToString().Replace(":", "")) + ".txt", sWord);

                //�ж�responsetTxt�Ƿ�Ϊture�����ɵ�ǩ�����mysign���õ�ǩ�����sign�Ƿ�һ��
                //responsetTxt�Ľ������true����������������⡢���������ID��notify_idһ����ʧЧ�й�
                //mysign��sign���ȣ��밲ȫУ���롢����ʱ�Ĳ�����ʽ���磺���Զ�������ȣ��������ʽ�й�
                if (responseTxt == "true")// && sign == mysign)//��֤�ɹ�
                {
                    //SortedList<string, string> sortedList = GetParam(dn, true);
                    //string param = GetUrlParam(sortedList, false);

                    //string sign = GetMd5Sign("utf-8", param + key);
                    //if (sign == dn.Sign)//��֤ǩ��
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
                    dn.Trade_Status = "֧����֪ͨǩ����֤ʧ��";
                    OnNotifyEvent(dn);
                    throw new CommonAliPayBaseException("֧����֪ͨǩ����֤ʧ��", 102);
                }
            }
            //}
            //else
            //{
            //    //context.Response.Write("fail");
            //    dn.Trade_Status = "֧����֪ͨ��֤ʧ��";
            //    OnNotifyEvent(dn);
            //    throw new CommonAliPayBaseException("֧����֪ͨ��֤ʧ��", 101);
            //}
        }
        /// <summary>
        /// ��ȡ֧����POST����֪ͨ��Ϣ�����ԡ�������=����ֵ������ʽ�������--zjz-20100910---
        /// </summary>
        /// <returns>request��������Ϣ��ɵ�����</returns>
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
        /// ���ڷ����㣬���ýӿ�query_timestamp����ȡʱ����Ĵ�����
        /// ע�⣺Զ�̽���XML������IIS�����������й�
        /// </summary>
        /// <param name="partner">���������ID</param>
        /// <returns>ʱ����ַ���</returns>
        public bool Query_Order(string orderid)
        {
            ///////////////////////���²�������Ҫ���õ�������ò��������ú󲻻���ĵ�//////////////////////
            string partner;            //���������ID
            string input_charset = "utf-8";     //�ַ������ʽ Ŀǰ֧�� gb2312 �� utf-8
            string sign_type = "MD5";           //���ܷ�ʽ �����޸�
            string transport = "https";        //����ģʽ,�����Լ��ķ������Ƿ�֧��ssl���ʣ���֧����ѡ��https������֧����ѡ��http

            string key;//��д�Լ���key
            string gateway;//��д֧������="https://www.alipay.com/cooperate/gateway.do"
            string AccountID;
            //////////////////////////////////////////////////////////////////////////////////////////////

            //SetParameters(out key, out partner, out gateway, out AccountID, orderid);

            string param;
            string mysign;
            string url = _alipayway + "service=single_trade_query&sign={0}&out_trade_no={1}&partner={2}&_input_charset={3}&sign_type={4}";
            if (!string.IsNullOrEmpty(_alipartnerid))
            {
                ArrayList sPara = new ArrayList();  //��Ҫ���ܵĲ�������
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
        /// ֪ͨ��֤�ӿ�
        /// </summary>
        /// <param name="verifyUrl"></param>
        /// <param name="verify">��֤����</param>
        /// <returns>true��ͨ����֤</returns>
        private bool VerifyNotify(string verifyUrl, Verify verify)
        {
            SortedList<string, string> sl = GetParam(verify, false);
            string param = GetParamString(verifyUrl, sl);
            string result = Get_Http(param, 120000);
            return bool.Parse(result);
        }

        /// <summary>
        ///  ����Form���ϵ�DigitalNotifyEventArgs,ֵ���ͻᱻ��ʼ��Ϊnull
        /// </summary>
        /// <param name="nv"></param>
        /// <param name="obj"></param>
        /// <remarks>Ϊ��ֵֹ���͵�Ĭ��ֵ��Ⱦ��������,����nullable����</remarks>
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
                        //ת��ʧ�ܻ��׳��쳣
                        pi.SetValue(obj, v, null);
                    }
                }
            }
            return (NotifyEventArgs)obj;

        }
        /// <summary>
        /// ��ȡMd5sign��Ĳ���
        /// </summary>
        /// <param name="digitalGoods"></param>
        /// <returns></returns>
        private string Md5SignParam(DigitalGoods digitalGoods)
        {
            if (digitalGoods.Sign_Type.ToLower() != "md5")
            {
                throw new CommonAliPayBaseException("Sign_Type��֧��MD5", 100);
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
        /// ��ȡ�����Ĳ���
        /// �޸�֪ͨ��֤������Ļ�ȡ*****20090917 wuxw
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="flag">֧��֪ͨ��֤�ı�־,flagΪtrue��Ϊ֪ͨ��֤.flagΪfalseΪ���صĲ�����ȡ</param>
        /// <returns></returns>
        private SortedList<string, string> GetParam(object obj, bool flag)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            SortedList<string, string> sortedList = new SortedList<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.GetValue(obj, null) != null)
                {
                    if (flag)//Ϊtrueʱ֪ͨ��֤�Ĳ�����ȡ
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
        /// ��ȡUrl�Ĳ���
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="isEncode">�����Ƿ񾭹�����,��ǩ���Ĳ������ñ���,Post�Ĳ���Ҫ����</param>
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
        /// ��ȡ�ַ�����MD5ǩ��
        /// </summary>
        /// <param name="encode">ǩ��ʱ�õı���</param>
        /// <param name="param">Ҫǩ�����ַ���</param>
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
        /// ��ȡԶ�̷�����ATN���
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

                strResult = "����" + exp.Message;
            }

            return strResult;
        }
        #endregion

    }
}

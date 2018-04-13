using System;
using System.Collections;


namespace CBSS.Core.Pay.AliPay.Util
{
    #region BuyParam

    /// <summary>
    /// 交易参数，默认编码改为了Utf－8
    /// </summary>
    public class StandardGoods:DigitalGoods
    {
        #region Member Variables


       
        //实物独有的
        protected decimal? _discount;
        protected string _paymentType;
        protected string _logisticsType;
        protected decimal? _logisticsFee;
        protected string _logisticsPayment;
        protected string _receiveName;
        protected string _receiveAddress;
        protected string _receiveZip;
        protected string _receivePhone;
        protected string _receiveMobile;
        #endregion

        #region Constructors

        public StandardGoods() { }

        /// <summary>
        /// 所有参数都是必须的，但是sellerEmail和sellerId可以任意填写其一,另一个设置位Null
        /// </summary>
        /// <param name="service">trade_create_by_buyer</param>
        /// <param name="partner">合作伙伴在支付宝的用户ID</param>
        /// <param name="sign">支付宝的Key</param>
        /// <param name="signType"></param>
        /// <param name="subject">商品名称</param>
        /// <param name="outTradeNo">外部交易号</param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="sellerEmail">和sellerId任意填写一个</param>
        /// <param name="sellerId"></param>
        /// <param name="logisticsType"></param>
        /// <param name="logisticsType"></param>
        /// <param name="logisticsPayment"></param>
        /// <param name="paymentType">1-6,不能为空</param>
        public StandardGoods(string service, string partner, string sign, string signType, string subject, string outTradeNo, decimal price, int? quantity, string sellerEmail, string sellerId,
                string logisticsType, decimal? logisticsFee, string logisticsPayment,string paymentType)
            : base(service, partner, sign, signType, subject, outTradeNo, price, quantity, sellerEmail, sellerId)
        {

            this._logisticsType = logisticsType;
            this._logisticsFee = logisticsFee;
            this._logisticsPayment = logisticsPayment;
            this._paymentType = paymentType;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="partner"></param>
        /// <param name="notifyUrl"></param>
        /// <param name="returnUrl"></param>
        /// <param name="agent"></param>
        /// <param name="inputCharset"></param>
        /// <param name="sign"></param>
        /// <param name="signType"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="showUrl"></param>
        /// <param name="sellerEmail"></param>
        /// <param name="sellerId"></param>
        /// <param name="buyerEmail"></param>
        /// <param name="buyerId"></param>
        /// <param name="buyerMsg"></param>
        /// <param name="logisticsType"></param>
        /// <param name="logisticsFee"></param>
        /// <param name="logisticsPayment"></param>
        public StandardGoods(string service, string partner, string notifyUrl, string returnUrl, string agent, string inputCharset, string sign, string signType, string subject, string body, string outTradeNo, decimal price, int? quantity, 
            string showUrl, string sellerEmail, string sellerId, string buyerEmail, string buyerId, string buyerMsg,
             string logisticsType, decimal? logisticsFee, string logisticsPayment, string paymentType)
            : this(service, partner, sign, signType, subject, outTradeNo, price, quantity, sellerEmail, sellerId,  logisticsType, logisticsFee, logisticsPayment,paymentType)
        {

            this._notifyUrl = notifyUrl;
            this._returnUrl = returnUrl;
            this._agent = agent;
            this._inputCharset = inputCharset;
            this._body = body;
            this._showUrl = showUrl;
            this._buyerEmail = buyerEmail;
            this._buyerId = buyerId;
            this._buyerMsg = buyerMsg;
        }

        #endregion

        #region Public Properties
       
        //实物独有的
        public decimal? Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        public string Payment_Type
        {
            get { return _paymentType; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for PaymentType", value, value.ToString());
                _paymentType = value;
            }
        }
        public string Logistics_Type
        {
            get { return _logisticsType; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for LogisticsType", value, value.ToString());
                _logisticsType = value;
            }
        }

        public decimal? Logistics_Fee
        {
            get { return _logisticsFee; }
            set { _logisticsFee = value; }
        }

        public string Logistics_Payment
        {
            get { return _logisticsPayment; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for LogisticsPayment", value, value.ToString());
                _logisticsPayment = value;
            }
        }

        public string Receive_Name
        {
            get { return _receiveName; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveName", value, value.ToString());
                _receiveName = value;
            }
        }

        public string Receive_Address
        {
            get { return _receiveAddress; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveAddress", value, value.ToString());
                _receiveAddress = value;
            }
        }

        public string Receive_Zip
        {
            get { return _receiveZip; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveZip", value, value.ToString());
                _receiveZip = value;
            }
        }

        public string Receive_Phone
        {
            get { return _receivePhone; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceivePhone", value, value.ToString());
                _receivePhone = value;
            }
        }

        public string Receive_Mobile
        {
            get { return _receiveMobile; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ReceiveMobile", value, value.ToString());
                _receiveMobile = value;
            }
        }

        #endregion
    }

    #endregion
}
